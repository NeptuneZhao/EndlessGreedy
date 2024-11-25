using System;
using UnityEngine;

// Token: 0x02000A10 RID: 2576
public class RadiationEmitter : SimComponent
{
	// Token: 0x06004AB2 RID: 19122 RVA: 0x001AB37B File Offset: 0x001A957B
	protected override void OnSpawn()
	{
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "RadiationEmitter.OnSpawn");
		base.OnSpawn();
	}

	// Token: 0x06004AB3 RID: 19123 RVA: 0x001AB3A5 File Offset: 0x001A95A5
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.OnCleanUp();
	}

	// Token: 0x06004AB4 RID: 19124 RVA: 0x001AB3C9 File Offset: 0x001A95C9
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x06004AB5 RID: 19125 RVA: 0x001AB3D2 File Offset: 0x001A95D2
	public int GetEmissionCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.emissionOffset);
	}

	// Token: 0x06004AB6 RID: 19126 RVA: 0x001AB3F0 File Offset: 0x001A95F0
	public void Refresh()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x06004AB7 RID: 19127 RVA: 0x001AB44E File Offset: 0x001A964E
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x06004AB8 RID: 19128 RVA: 0x001AB458 File Offset: 0x001A9658
	private void SetRadiusProportionalToRads()
	{
		this.emitRadiusX = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
		this.emitRadiusY = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
	}

	// Token: 0x06004AB9 RID: 19129 RVA: 0x001AB4AC File Offset: 0x001A96AC
	protected override void OnSimActivate()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x06004ABA RID: 19130 RVA: 0x001AB50C File Offset: 0x001A970C
	protected override void OnSimDeactivate()
	{
		int emissionCell = this.GetEmissionCell();
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x06004ABB RID: 19131 RVA: 0x001AB550 File Offset: 0x001A9750
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		int emissionCell = this.GetEmissionCell();
		SimMessages.AddRadiationEmitter(cb_handle.index, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x06004ABC RID: 19132 RVA: 0x001AB5A3 File Offset: 0x001A97A3
	protected override void OnSimUnregister()
	{
		RadiationEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x06004ABD RID: 19133 RVA: 0x001AB5B0 File Offset: 0x001A97B0
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveRadiationEmitter(-1, sim_handle);
	}

	// Token: 0x06004ABE RID: 19134 RVA: 0x001AB5C4 File Offset: 0x001A97C4
	private void OnDrawGizmosSelected()
	{
		int emissionCell = this.GetEmissionCell();
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(emissionCell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x06004ABF RID: 19135 RVA: 0x001AB618 File Offset: 0x001A9818
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(RadiationEmitter.StaticUnregister);
	}

	// Token: 0x040030F3 RID: 12531
	public bool radiusProportionalToRads;

	// Token: 0x040030F4 RID: 12532
	[SerializeField]
	public short emitRadiusX = 4;

	// Token: 0x040030F5 RID: 12533
	[SerializeField]
	public short emitRadiusY = 4;

	// Token: 0x040030F6 RID: 12534
	[SerializeField]
	public float emitRads = 10f;

	// Token: 0x040030F7 RID: 12535
	[SerializeField]
	public float emitRate = 1f;

	// Token: 0x040030F8 RID: 12536
	[SerializeField]
	public float emitSpeed = 1f;

	// Token: 0x040030F9 RID: 12537
	[SerializeField]
	public float emitDirection;

	// Token: 0x040030FA RID: 12538
	[SerializeField]
	public float emitAngle = 360f;

	// Token: 0x040030FB RID: 12539
	[SerializeField]
	public RadiationEmitter.RadiationEmitterType emitType;

	// Token: 0x040030FC RID: 12540
	[SerializeField]
	public Vector3 emissionOffset = Vector3.zero;

	// Token: 0x02001A2E RID: 6702
	public enum RadiationEmitterType
	{
		// Token: 0x04007BAA RID: 31658
		Constant,
		// Token: 0x04007BAB RID: 31659
		Pulsing,
		// Token: 0x04007BAC RID: 31660
		PulsingAveraged,
		// Token: 0x04007BAD RID: 31661
		SimplePulse,
		// Token: 0x04007BAE RID: 31662
		RadialBeams,
		// Token: 0x04007BAF RID: 31663
		Attractor
	}
}

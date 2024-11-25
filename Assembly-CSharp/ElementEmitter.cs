using System;
using UnityEngine;

// Token: 0x02000879 RID: 2169
public class ElementEmitter : SimComponent
{
	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06003C99 RID: 15513 RVA: 0x001504C3 File Offset: 0x0014E6C3
	// (set) Token: 0x06003C9A RID: 15514 RVA: 0x001504CB File Offset: 0x0014E6CB
	public bool isEmitterBlocked { get; private set; }

	// Token: 0x06003C9B RID: 15515 RVA: 0x001504D4 File Offset: 0x0014E6D4
	protected override void OnSpawn()
	{
		this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterBlocked), true));
		this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterUnblocked), true));
		base.OnSpawn();
	}

	// Token: 0x06003C9C RID: 15516 RVA: 0x00150535 File Offset: 0x0014E735
	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
		Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
		base.OnCleanUp();
	}

	// Token: 0x06003C9D RID: 15517 RVA: 0x0015055D File Offset: 0x0014E75D
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x06003C9E RID: 15518 RVA: 0x00150568 File Offset: 0x0014E768
	protected override void OnSimActivate()
	{
		int game_cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		if (this.outputElement.elementHash != (SimHashes)0 && this.outputElement.massGenerationRate > 0f && this.emissionFrequency > 0f)
		{
			float emit_temperature = (this.outputElement.minOutputTemperature == 0f) ? base.GetComponent<PrimaryElement>().Temperature : this.outputElement.minOutputTemperature;
			SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.outputElement.elementHash, this.emissionFrequency, this.outputElement.massGenerationRate, emit_temperature, this.maxPressure, this.outputElement.addedDiseaseIdx, this.outputElement.addedDiseaseCount);
		}
		if (this.showDescriptor)
		{
			this.statusHandle = base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ElementEmitterOutput, this);
		}
	}

	// Token: 0x06003C9F RID: 15519 RVA: 0x00150684 File Offset: 0x0014E884
	protected override void OnSimDeactivate()
	{
		int game_cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		if (this.showDescriptor)
		{
			this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x06003CA0 RID: 15520 RVA: 0x0015071C File Offset: 0x0014E91C
	public void ForceEmit(float mass, byte disease_idx, int disease_count, float temperature = -1f)
	{
		if (mass <= 0f)
		{
			return;
		}
		float temperature2 = (temperature > 0f) ? temperature : this.outputElement.minOutputTemperature;
		Element element = ElementLoader.FindElementByHash(this.outputElement.elementHash);
		if (element.IsGas || element.IsLiquid)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), this.outputElement.elementHash, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature2, disease_idx, disease_count, true, -1);
		}
		else if (element.IsSolid)
		{
			element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), mass, temperature2, disease_idx, disease_count, false, true, false);
		}
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(this.outputElement.elementHash).name, base.gameObject.transform, 1.5f, false);
	}

	// Token: 0x06003CA1 RID: 15521 RVA: 0x00150818 File Offset: 0x0014EA18
	private void OnEmitterBlocked()
	{
		this.isEmitterBlocked = true;
		base.Trigger(1615168894, this);
	}

	// Token: 0x06003CA2 RID: 15522 RVA: 0x0015082D File Offset: 0x0014EA2D
	private void OnEmitterUnblocked()
	{
		this.isEmitterBlocked = false;
		base.Trigger(-657992955, this);
	}

	// Token: 0x06003CA3 RID: 15523 RVA: 0x00150842 File Offset: 0x0014EA42
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		SimMessages.AddElementEmitter(this.maxPressure, cb_handle.index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
	}

	// Token: 0x06003CA4 RID: 15524 RVA: 0x0015087D File Offset: 0x0014EA7D
	protected override void OnSimUnregister()
	{
		ElementEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x06003CA5 RID: 15525 RVA: 0x0015088A File Offset: 0x0014EA8A
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementEmitter(-1, sim_handle);
	}

	// Token: 0x06003CA6 RID: 15526 RVA: 0x001508A0 File Offset: 0x0014EAA0
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(cell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x06003CA7 RID: 15527 RVA: 0x00150925 File Offset: 0x0014EB25
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementEmitter.StaticUnregister);
	}

	// Token: 0x0400250A RID: 9482
	[SerializeField]
	public ElementConverter.OutputElement outputElement;

	// Token: 0x0400250B RID: 9483
	[SerializeField]
	public float emissionFrequency = 1f;

	// Token: 0x0400250C RID: 9484
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x0400250D RID: 9485
	[SerializeField]
	public float maxPressure = 1f;

	// Token: 0x0400250E RID: 9486
	private Guid statusHandle = Guid.Empty;

	// Token: 0x0400250F RID: 9487
	public bool showDescriptor = true;

	// Token: 0x04002510 RID: 9488
	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x04002511 RID: 9489
	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
}

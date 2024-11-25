using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000672 RID: 1650
[AddComponentMenu("KMonoBehaviour/scripts/BuildingElementEmitter")]
public class BuildingElementEmitter : KMonoBehaviour, IGameObjectEffectDescriptor, IElementEmitter, ISim200ms
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x060028C7 RID: 10439 RVA: 0x000E7090 File Offset: 0x000E5290
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060028C8 RID: 10440 RVA: 0x000E70A7 File Offset: 0x000E52A7
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000E70AF File Offset: 0x000E52AF
	public SimHashes Element
	{
		get
		{
			return this.element;
		}
	}

	// Token: 0x060028CA RID: 10442 RVA: 0x000E70B7 File Offset: 0x000E52B7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		base.Subscribe<BuildingElementEmitter>(824508782, BuildingElementEmitter.OnActiveChangedDelegate);
		this.SimRegister();
	}

	// Token: 0x060028CB RID: 10443 RVA: 0x000E70F1 File Offset: 0x000E52F1
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x060028CC RID: 10444 RVA: 0x000E7115 File Offset: 0x000E5315
	private void OnActiveChanged(object data)
	{
		this.simActive = ((Operational)data).IsActive;
		this.dirty = true;
	}

	// Token: 0x060028CD RID: 10445 RVA: 0x000E712F File Offset: 0x000E532F
	public void Sim200ms(float dt)
	{
		this.UnsafeUpdate(dt);
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x000E7138 File Offset: 0x000E5338
	private unsafe void UnsafeUpdate(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
		int handleIndex = Sim.GetHandleIndex(this.simHandle);
		Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
		if (emittedMassInfo.mass > 0f)
		{
			Game.Instance.accumulators.Accumulate(this.accumulator, emittedMassInfo.mass);
			if (this.element == SimHashes.Oxygen)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, emittedMassInfo.mass, base.gameObject.GetProperName(), null);
			}
		}
	}

	// Token: 0x060028CF RID: 10447 RVA: 0x000E71D8 File Offset: 0x000E53D8
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			if (this.element != (SimHashes)0 && this.emitRate > 0f)
			{
				int game_cell = Grid.PosToCell(new Vector3(base.transform.GetPosition().x + this.modifierOffset.x, base.transform.GetPosition().y + this.modifierOffset.y, 0f));
				SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.element, 0.2f, this.emitRate * 0.2f, this.temperature, float.MaxValue, this.emitDiseaseIdx, this.emitDiseaseCount);
			}
			this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmittingElement, this);
			return;
		}
		SimMessages.ModifyElementEmitter(this.simHandle, 0, 0, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, this);
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x000E7310 File Offset: 0x000E5510
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(BuildingElementEmitter.OnSimRegisteredCallback), this, "BuildingElementEmitter").index, -1, -1);
		}
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x000E736B File Offset: 0x000E556B
	private void SimUnregister()
	{
		if (this.simHandle != -1)
		{
			if (Sim.IsValidHandle(this.simHandle))
			{
				SimMessages.RemoveElementEmitter(-1, this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000E7396 File Offset: 0x000E5596
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((BuildingElementEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x000E73A4 File Offset: 0x000E55A4
	private void OnSimRegistered(int handle)
	{
		if (this != null)
		{
			this.simHandle = handle;
			return;
		}
		SimMessages.RemoveElementEmitter(-1, handle);
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x000E73C0 File Offset: 0x000E55C0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.element).tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04001766 RID: 5990
	[SerializeField]
	public float emitRate = 0.3f;

	// Token: 0x04001767 RID: 5991
	[SerializeField]
	[Serialize]
	public float temperature = 293f;

	// Token: 0x04001768 RID: 5992
	[SerializeField]
	[HashedEnum]
	public SimHashes element = SimHashes.Oxygen;

	// Token: 0x04001769 RID: 5993
	[SerializeField]
	public Vector2 modifierOffset;

	// Token: 0x0400176A RID: 5994
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x0400176B RID: 5995
	[SerializeField]
	public byte emitDiseaseIdx = byte.MaxValue;

	// Token: 0x0400176C RID: 5996
	[SerializeField]
	public int emitDiseaseCount;

	// Token: 0x0400176D RID: 5997
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x0400176E RID: 5998
	private int simHandle = -1;

	// Token: 0x0400176F RID: 5999
	private bool simActive;

	// Token: 0x04001770 RID: 6000
	private bool dirty = true;

	// Token: 0x04001771 RID: 6001
	private Guid statusHandle;

	// Token: 0x04001772 RID: 6002
	private static readonly EventSystem.IntraObjectHandler<BuildingElementEmitter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<BuildingElementEmitter>(delegate(BuildingElementEmitter component, object data)
	{
		component.OnActiveChanged(data);
	});
}

using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000792 RID: 1938
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Valve")]
public class Valve : Workable, ISaveLoadable
{
	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x060034F7 RID: 13559 RVA: 0x00120BF5 File Offset: 0x0011EDF5
	public float QueuedMaxFlow
	{
		get
		{
			if (this.chore == null)
			{
				return -1f;
			}
			return this.desiredFlow;
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x060034F8 RID: 13560 RVA: 0x00120C0B File Offset: 0x0011EE0B
	public float DesiredFlow
	{
		get
		{
			return this.desiredFlow;
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060034F9 RID: 13561 RVA: 0x00120C13 File Offset: 0x0011EE13
	public float MaxFlow
	{
		get
		{
			return this.valveBase.MaxFlow;
		}
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x00120C20 File Offset: 0x0011EE20
	private void OnCopySettings(object data)
	{
		Valve component = ((GameObject)data).GetComponent<Valve>();
		if (component != null)
		{
			this.ChangeFlow(component.desiredFlow);
		}
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x00120C50 File Offset: 0x0011EE50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.synchronizeAnims = false;
		this.valveBase.CurrentFlow = this.valveBase.MaxFlow;
		this.desiredFlow = this.valveBase.MaxFlow;
		base.Subscribe<Valve>(-905833192, Valve.OnCopySettingsDelegate);
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x00120CAD File Offset: 0x0011EEAD
	protected override void OnSpawn()
	{
		this.ChangeFlow(this.desiredFlow);
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x00120CCC File Offset: 0x0011EECC
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x00120CE0 File Offset: 0x0011EEE0
	public void ChangeFlow(float amount)
	{
		this.desiredFlow = Mathf.Clamp(amount, 0f, this.valveBase.MaxFlow);
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.PumpingLiquidOrGas, this.desiredFlow >= 0f, this.valveBase.AccumulatorHandle);
		if (DebugHandler.InstantBuildMode)
		{
			this.UpdateFlow();
			return;
		}
		if (this.desiredFlow == this.valveBase.CurrentFlow)
		{
			if (this.chore != null)
			{
				this.chore.Cancel("desiredFlow == currentFlow");
				this.chore = null;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
			return;
		}
		if (this.chore == null)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.ValveRequest, this);
			component.AddStatusItem(Db.Get().BuildingStatusItems.PendingWork, this);
			this.chore = new WorkChore<Valve>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
	}

	// Token: 0x060034FF RID: 13567 RVA: 0x00120E1B File Offset: 0x0011F01B
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.UpdateFlow();
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x00120E2C File Offset: 0x0011F02C
	public void UpdateFlow()
	{
		this.valveBase.CurrentFlow = this.desiredFlow;
		this.valveBase.UpdateAnim();
		if (this.chore != null)
		{
			this.chore.Cancel("forced complete");
		}
		this.chore = null;
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
	}

	// Token: 0x04001F6E RID: 8046
	[MyCmpReq]
	private ValveBase valveBase;

	// Token: 0x04001F6F RID: 8047
	[Serialize]
	private float desiredFlow = 0.5f;

	// Token: 0x04001F70 RID: 8048
	private Chore chore;

	// Token: 0x04001F71 RID: 8049
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F72 RID: 8050
	private static readonly EventSystem.IntraObjectHandler<Valve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Valve>(delegate(Valve component, object data)
	{
		component.OnCopySettings(data);
	});
}

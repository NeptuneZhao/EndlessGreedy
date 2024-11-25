using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C7 RID: 1735
public class ElectrobankDischarger : Generator
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06002BE0 RID: 11232 RVA: 0x000F6790 File Offset: 0x000F4990
	public float ElectrobankJoulesStored
	{
		get
		{
			float num = 0f;
			foreach (Electrobank electrobank in this.storedCells)
			{
				num += electrobank.Charge;
			}
			return num;
		}
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x000F67EC File Offset: 0x000F49EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ElectrobankDischarger.StatesInstance(this);
		this.smi.StartSM();
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		base.Subscribe(-592767678, new Action<object>(this.RefreshOperationalActive));
		this.RefreshCells(null);
		this.RefreshOperationalActive(null);
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.PowerFetch);
		this.filteredStorage.SetHasMeter(false);
		this.filteredStorage.FilterChanged();
		Storage storage = this.storage;
		storage.onDestroyItemsDropped = (Action<List<GameObject>>)Delegate.Combine(storage.onDestroyItemsDropped, new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeconstruction));
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x000F68B0 File Offset: 0x000F4AB0
	private void OnBatteriesDroppedFromDeconstruction(List<GameObject> items)
	{
		if (items != null)
		{
			for (int i = 0; i < items.Count; i++)
			{
				Electrobank component = items[i].GetComponent<Electrobank>();
				if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
				{
					component.RemovePower(component.Charge, true);
				}
			}
		}
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x000F690A File Offset: 0x000F4B0A
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x000F691D File Offset: 0x000F4B1D
	private void OnStorageChange(object data = null)
	{
		this.RefreshCells(null);
		this.RefreshOperationalActive(null);
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x000F6930 File Offset: 0x000F4B30
	public void UpdateMeter()
	{
		if (this.meterController == null)
		{
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}
		this.meterController.SetPositionPercent(this.smi.master.ElectrobankJoulesStored / 120000f);
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x000F6989 File Offset: 0x000F4B89
	private void RefreshOperationalActive(object data = null)
	{
		if (this.operational.IsOperational)
		{
			if (this.storedCells.Count > 0)
			{
				this.operational.SetActive(true, false);
				return;
			}
			this.operational.SetActive(false, false);
		}
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x000F69C4 File Offset: 0x000F4BC4
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsActive)
		{
			return;
		}
		float num = 0f;
		float num2 = Mathf.Min(this.wattageRating * dt, this.Capacity - this.JoulesAvailable);
		for (int i = this.storedCells.Count - 1; i >= 0; i--)
		{
			num += this.storedCells[i].RemovePower(num2 - num, true);
			if (num >= num2)
			{
				break;
			}
		}
		if (num > 0f)
		{
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x000F6A70 File Offset: 0x000F4C70
	private void RefreshCells(object data = null)
	{
		this.storedCells.Clear();
		foreach (GameObject gameObject in this.storage.GetItems())
		{
			Electrobank component = gameObject.GetComponent<Electrobank>();
			if (component != null)
			{
				this.storedCells.Add(component);
			}
		}
	}

	// Token: 0x0400193D RID: 6461
	public float wattageRating;

	// Token: 0x0400193E RID: 6462
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400193F RID: 6463
	private ElectrobankDischarger.StatesInstance smi;

	// Token: 0x04001940 RID: 6464
	private List<Electrobank> storedCells = new List<Electrobank>();

	// Token: 0x04001941 RID: 6465
	private MeterController meterController;

	// Token: 0x04001942 RID: 6466
	protected FilteredStorage filteredStorage;

	// Token: 0x020014C9 RID: 5321
	public class StatesInstance : GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.GameInstance
	{
		// Token: 0x06008C1D RID: 35869 RVA: 0x00338DB5 File Offset: 0x00336FB5
		public StatesInstance(ElectrobankDischarger master) : base(master)
		{
		}
	}

	// Token: 0x020014CA RID: 5322
	public class States : GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger>
	{
		// Token: 0x06008C1E RID: 35870 RVA: 0x00338DC0 File Offset: 0x00336FC0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.noBattery;
			this.root.EventTransition(GameHashes.ActiveChanged, this.discharging, (ElectrobankDischarger.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.noBattery.PlayAnim("off").Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.inoperational.PlayAnim("on").Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).EnterTransition(this.noBattery, (ElectrobankDischarger.StatesInstance smi) => smi.master.storage.items.Count == 0);
			this.discharging.Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).EventTransition(GameHashes.ActiveChanged, this.inoperational, (ElectrobankDischarger.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive).QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).Update(delegate(ElectrobankDischarger.StatesInstance smi, float dt)
			{
				smi.master.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ElectrobankJoulesAvailable, smi.master);
				smi.master.UpdateMeter();
			}, UpdateRate.SIM_200ms, false);
			this.discharging_pst.Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).PlayAnim("working_pst");
		}

		// Token: 0x04006AF5 RID: 27381
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State noBattery;

		// Token: 0x04006AF6 RID: 27382
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State inoperational;

		// Token: 0x04006AF7 RID: 27383
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging;

		// Token: 0x04006AF8 RID: 27384
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging_pst;
	}
}

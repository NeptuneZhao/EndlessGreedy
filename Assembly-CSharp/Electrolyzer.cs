using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
[SerializationConfig(MemberSerialization.OptIn)]
public class Electrolyzer : StateMachineComponent<Electrolyzer.StatesInstance>
{
	// Token: 0x06002BEA RID: 11242 RVA: 0x000F6AFC File Offset: 0x000F4CFC
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.hasMeter)
		{
			this.meter = new MeterController(component, "U2H_meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new Vector3(-0.4f, 0.5f, -0.1f), new string[]
			{
				"U2H_meter_target",
				"U2H_meter_tank",
				"U2H_meter_waterbody",
				"U2H_meter_level"
			});
		}
		base.smi.StartSM();
		this.UpdateMeter();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x000F6B91 File Offset: 0x000F4D91
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06002BEC RID: 11244 RVA: 0x000F6BB0 File Offset: 0x000F4DB0
	public void UpdateMeter()
	{
		if (this.hasMeter)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06002BED RID: 11245 RVA: 0x000F6BF0 File Offset: 0x000F4DF0
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.OffsetCell(num, this.emissionOffset);
			return !GameUtil.FloodFillCheck<Electrolyzer>(new Func<int, Electrolyzer, bool>(Electrolyzer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x000F6C34 File Offset: 0x000F4E34
	private static bool OverPressure(int cell, Electrolyzer electrolyzer)
	{
		return Grid.Mass[cell] > electrolyzer.maxMass;
	}

	// Token: 0x04001943 RID: 6467
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04001944 RID: 6468
	[SerializeField]
	public bool hasMeter = true;

	// Token: 0x04001945 RID: 6469
	[SerializeField]
	public CellOffset emissionOffset = CellOffset.none;

	// Token: 0x04001946 RID: 6470
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001947 RID: 6471
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04001948 RID: 6472
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001949 RID: 6473
	private MeterController meter;

	// Token: 0x020014CB RID: 5323
	public class StatesInstance : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.GameInstance
	{
		// Token: 0x06008C20 RID: 35872 RVA: 0x00338F74 File Offset: 0x00337174
		public StatesInstance(Electrolyzer smi) : base(smi)
		{
		}
	}

	// Token: 0x020014CC RID: 5324
	public class States : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer>
	{
		// Token: 0x06008C21 RID: 35873 RVA: 0x00338F80 File Offset: 0x00337180
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (Electrolyzer.StatesInstance smi) => !smi.master.operational.IsOperational).EventHandler(GameHashes.OnStorageChange, delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (Electrolyzer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (Electrolyzer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (Electrolyzer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (Electrolyzer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (Electrolyzer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x04006AF9 RID: 27385
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State disabled;

		// Token: 0x04006AFA RID: 27386
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State waiting;

		// Token: 0x04006AFB RID: 27387
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State converting;

		// Token: 0x04006AFC RID: 27388
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State overpressure;
	}
}

using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000763 RID: 1891
[SerializationConfig(MemberSerialization.OptIn)]
public class RustDeoxidizer : StateMachineComponent<RustDeoxidizer.StatesInstance>
{
	// Token: 0x060032C9 RID: 13001 RVA: 0x00117359 File Offset: 0x00115559
	protected override void OnSpawn()
	{
		base.smi.StartSM();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x060032CA RID: 13002 RVA: 0x0011737B File Offset: 0x0011557B
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x060032CB RID: 13003 RVA: 0x0011739C File Offset: 0x0011559C
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.CellAbove(num);
			return !GameUtil.FloodFillCheck<RustDeoxidizer>(new Func<int, RustDeoxidizer, bool>(RustDeoxidizer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x001173DA File Offset: 0x001155DA
	private static bool OverPressure(int cell, RustDeoxidizer rustDeoxidizer)
	{
		return Grid.Mass[cell] > rustDeoxidizer.maxMass;
	}

	// Token: 0x04001DFD RID: 7677
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04001DFE RID: 7678
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001DFF RID: 7679
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04001E00 RID: 7680
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001E01 RID: 7681
	private MeterController meter;

	// Token: 0x020015EB RID: 5611
	public class StatesInstance : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.GameInstance
	{
		// Token: 0x06009066 RID: 36966 RVA: 0x0034B626 File Offset: 0x00349826
		public StatesInstance(RustDeoxidizer smi) : base(smi)
		{
		}
	}

	// Token: 0x020015EC RID: 5612
	public class States : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer>
	{
		// Token: 0x06009067 RID: 36967 RVA: 0x0034B630 File Offset: 0x00349830
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (RustDeoxidizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (RustDeoxidizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (RustDeoxidizer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (RustDeoxidizer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (RustDeoxidizer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (RustDeoxidizer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x04006E2F RID: 28207
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State disabled;

		// Token: 0x04006E30 RID: 28208
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State waiting;

		// Token: 0x04006E31 RID: 28209
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State converting;

		// Token: 0x04006E32 RID: 28210
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State overpressure;
	}
}

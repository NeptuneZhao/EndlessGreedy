using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200073F RID: 1855
[SerializationConfig(MemberSerialization.OptIn)]
public class OilRefinery : StateMachineComponent<OilRefinery.StatesInstance>
{
	// Token: 0x06003157 RID: 12631 RVA: 0x0011024C File Offset: 0x0010E44C
	protected override void OnSpawn()
	{
		base.Subscribe<OilRefinery>(-1697596308, OilRefinery.OnStorageChangedDelegate);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, null);
		base.smi.StartSM();
		this.maxSrcMass = base.GetComponent<ConduitConsumer>().capacityKG;
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x001102AC File Offset: 0x0010E4AC
	private void OnStorageChanged(object data)
	{
		float positionPercent = Mathf.Clamp01(this.storage.GetMassAvailable(SimHashes.CrudeOil) / this.maxSrcMass);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x001102E4 File Offset: 0x0010E4E4
	private static bool UpdateStateCb(int cell, object data)
	{
		OilRefinery oilRefinery = data as OilRefinery;
		if (Grid.Element[cell].IsGas)
		{
			oilRefinery.cellCount += 1f;
			oilRefinery.envPressure += Grid.Mass[cell];
		}
		return true;
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x00110334 File Offset: 0x0010E534
	private void TestAreaPressure()
	{
		this.envPressure = 0f;
		this.cellCount = 0f;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, new Func<int, object, bool>(OilRefinery.UpdateStateCb));
			this.envPressure /= this.cellCount;
		}
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x001103AA File Offset: 0x0010E5AA
	private bool IsOverPressure()
	{
		return this.envPressure >= this.overpressureMass;
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x001103BD File Offset: 0x0010E5BD
	private bool IsOverWarningPressure()
	{
		return this.envPressure >= this.overpressureWarningMass;
	}

	// Token: 0x04001CFC RID: 7420
	private bool wasOverPressure;

	// Token: 0x04001CFD RID: 7421
	[SerializeField]
	public float overpressureWarningMass = 4.5f;

	// Token: 0x04001CFE RID: 7422
	[SerializeField]
	public float overpressureMass = 5f;

	// Token: 0x04001CFF RID: 7423
	private float maxSrcMass;

	// Token: 0x04001D00 RID: 7424
	private float envPressure;

	// Token: 0x04001D01 RID: 7425
	private float cellCount;

	// Token: 0x04001D02 RID: 7426
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001D03 RID: 7427
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D04 RID: 7428
	[MyCmpAdd]
	private OilRefinery.WorkableTarget workable;

	// Token: 0x04001D05 RID: 7429
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x04001D06 RID: 7430
	private const bool hasMeter = true;

	// Token: 0x04001D07 RID: 7431
	private MeterController meter;

	// Token: 0x04001D08 RID: 7432
	private static readonly EventSystem.IntraObjectHandler<OilRefinery> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<OilRefinery>(delegate(OilRefinery component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x020015A4 RID: 5540
	public class StatesInstance : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.GameInstance
	{
		// Token: 0x06008F59 RID: 36697 RVA: 0x00347235 File Offset: 0x00345435
		public StatesInstance(OilRefinery smi) : base(smi)
		{
		}

		// Token: 0x06008F5A RID: 36698 RVA: 0x00347240 File Offset: 0x00345440
		public void TestAreaPressure()
		{
			base.smi.master.TestAreaPressure();
			bool flag = base.smi.master.IsOverPressure();
			bool flag2 = base.smi.master.IsOverWarningPressure();
			if (flag)
			{
				base.smi.master.wasOverPressure = true;
				base.sm.isOverPressure.Set(true, this, false);
				return;
			}
			if (base.smi.master.wasOverPressure && !flag2)
			{
				base.sm.isOverPressure.Set(false, this, false);
			}
		}
	}

	// Token: 0x020015A5 RID: 5541
	public class States : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery>
	{
		// Token: 0x06008F5B RID: 36699 RVA: 0x003472D0 File Offset: 0x003454D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OilRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.needResources, (OilRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.needResources.EventTransition(GameHashes.OnStorageChange, this.ready, (OilRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.ready.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.overpressure, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsTrue).Transition(this.needResources, (OilRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false), UpdateRate.SIM_200ms).ToggleChore((OilRefinery.StatesInstance smi) => new WorkChore<OilRefinery.WorkableTarget>(Db.Get().ChoreTypes.Fabricate, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), this.needResources);
			this.overpressure.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.ready, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null);
		}

		// Token: 0x04006D7D RID: 28029
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressure;

		// Token: 0x04006D7E RID: 28030
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressureWarning;

		// Token: 0x04006D7F RID: 28031
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State disabled;

		// Token: 0x04006D80 RID: 28032
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State overpressure;

		// Token: 0x04006D81 RID: 28033
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State needResources;

		// Token: 0x04006D82 RID: 28034
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State ready;
	}

	// Token: 0x020015A6 RID: 5542
	[AddComponentMenu("KMonoBehaviour/Workable/WorkableTarget")]
	public class WorkableTarget : Workable
	{
		// Token: 0x06008F5D RID: 36701 RVA: 0x0034748C File Offset: 0x0034568C
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.showProgressBar = false;
			this.workerStatusItem = null;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_oilrefinery_kanim")
			};
		}

		// Token: 0x06008F5E RID: 36702 RVA: 0x003474F0 File Offset: 0x003456F0
		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x06008F5F RID: 36703 RVA: 0x00347503 File Offset: 0x00345703
		protected override void OnStartWork(WorkerBase worker)
		{
			this.operational.SetActive(true, false);
		}

		// Token: 0x06008F60 RID: 36704 RVA: 0x00347512 File Offset: 0x00345712
		protected override void OnStopWork(WorkerBase worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x06008F61 RID: 36705 RVA: 0x00347521 File Offset: 0x00345721
		protected override void OnCompleteWork(WorkerBase worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x06008F62 RID: 36706 RVA: 0x00347530 File Offset: 0x00345730
		public override bool InstantlyFinish(WorkerBase worker)
		{
			return false;
		}

		// Token: 0x04006D83 RID: 28035
		[MyCmpGet]
		public Operational operational;
	}
}

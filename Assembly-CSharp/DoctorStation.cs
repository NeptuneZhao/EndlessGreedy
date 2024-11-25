using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200085E RID: 2142
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStation")]
public class DoctorStation : Workable
{
	// Token: 0x06003BA9 RID: 15273 RVA: 0x00148AFC File Offset: 0x00146CFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x00148B04 File Offset: 0x00146D04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.doctor_workable = base.GetComponent<DoctorStationDoctorWorkable>();
		base.SetWorkTime(float.PositiveInfinity);
		this.smi = new DoctorStation.StatesInstance(this);
		this.smi.StartSM();
		this.OnStorageChange(null);
		base.Subscribe<DoctorStation>(-1697596308, DoctorStation.OnStorageChangeDelegate);
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x00148B68 File Offset: 0x00146D68
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
			this.smi = null;
		}
		base.OnCleanUp();
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x00148B9C File Offset: 0x00146D9C
	private void OnStorageChange(object data = null)
	{
		this.treatments_available.Clear();
		foreach (GameObject gameObject in this.storage.items)
		{
			MedicinalPill component = gameObject.GetComponent<MedicinalPill>();
			if (component != null)
			{
				Tag tag = gameObject.PrefabID();
				foreach (string id in component.info.curedSicknesses)
				{
					this.AddTreatment(id, tag);
				}
			}
		}
		bool value = this.treatments_available.Count > 0;
		this.smi.sm.hasSupplies.Set(value, this.smi, false);
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x00148C8C File Offset: 0x00146E8C
	private void AddTreatment(string id, Tag tag)
	{
		if (!this.treatments_available.ContainsKey(id))
		{
			this.treatments_available.Add(id, tag);
		}
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x00148CB3 File Offset: 0x00146EB3
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.hasPatient.Set(true, this.smi, false);
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x00148CDA File Offset: 0x00146EDA
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.hasPatient.Set(false, this.smi, false);
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x00148D01 File Offset: 0x00146F01
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06003BB1 RID: 15281 RVA: 0x00148D04 File Offset: 0x00146F04
	public void SetHasDoctor(bool has)
	{
		this.smi.sm.hasDoctor.Set(has, this.smi, false);
	}

	// Token: 0x06003BB2 RID: 15282 RVA: 0x00148D24 File Offset: 0x00146F24
	public void CompleteDoctoring()
	{
		if (!base.worker)
		{
			return;
		}
		this.CompleteDoctoring(base.worker.gameObject);
	}

	// Token: 0x06003BB3 RID: 15283 RVA: 0x00148D48 File Offset: 0x00146F48
	private void CompleteDoctoring(GameObject target)
	{
		Sicknesses sicknesses = target.GetSicknesses();
		if (sicknesses != null)
		{
			bool flag = false;
			foreach (SicknessInstance sicknessInstance in sicknesses)
			{
				Tag tag;
				if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
				{
					Game.Instance.savedInfo.curedDisease = true;
					sicknessInstance.Cure();
					this.storage.ConsumeIgnoringDisease(tag, 1f);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				global::Debug.LogWarningFormat(base.gameObject, "Failed to treat any disease for {0}", new object[]
				{
					target
				});
			}
		}
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x00148DFC File Offset: 0x00146FFC
	public bool IsDoctorAvailable(GameObject target)
	{
		if (!string.IsNullOrEmpty(this.doctor_workable.requiredSkillPerk))
		{
			MinionResume component = target.GetComponent<MinionResume>();
			if (!MinionResume.AnyOtherMinionHasPerk(this.doctor_workable.requiredSkillPerk, component))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x00148E38 File Offset: 0x00147038
	public bool IsTreatmentAvailable(GameObject target)
	{
		Sicknesses sicknesses = target.GetSicknesses();
		if (sicknesses != null)
		{
			foreach (SicknessInstance sicknessInstance in sicknesses)
			{
				Tag tag;
				if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x0400240C RID: 9228
	private static readonly EventSystem.IntraObjectHandler<DoctorStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DoctorStation>(delegate(DoctorStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x0400240D RID: 9229
	[MyCmpReq]
	public Storage storage;

	// Token: 0x0400240E RID: 9230
	[MyCmpReq]
	public Operational operational;

	// Token: 0x0400240F RID: 9231
	private DoctorStationDoctorWorkable doctor_workable;

	// Token: 0x04002410 RID: 9232
	private Dictionary<HashedString, Tag> treatments_available = new Dictionary<HashedString, Tag>();

	// Token: 0x04002411 RID: 9233
	private DoctorStation.StatesInstance smi;

	// Token: 0x04002412 RID: 9234
	public static readonly Chore.Precondition TreatmentAvailable = new Chore.Precondition
	{
		id = "TreatmentAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.TREATMENT_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((DoctorStation)data).IsTreatmentAvailable(context.consumerState.gameObject);
		}
	};

	// Token: 0x04002413 RID: 9235
	public static readonly Chore.Precondition DoctorAvailable = new Chore.Precondition
	{
		id = "DoctorAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.DOCTOR_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((DoctorStation)data).IsDoctorAvailable(context.consumerState.gameObject);
		}
	};

	// Token: 0x0200175F RID: 5983
	public class States : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation>
	{
		// Token: 0x06009576 RID: 38262 RVA: 0x0035FA0C File Offset: 0x0035DC0C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DoctorStation.StatesInstance smi) => smi.master.operational.IsOperational);
			this.operational.EventTransition(GameHashes.OperationalChanged, this.unoperational, (DoctorStation.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.operational.not_ready);
			this.operational.not_ready.ParamTransition<bool>(this.hasSupplies, this.operational.ready, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.DefaultState(this.operational.ready.idle).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreatePatientChore), null).ParamTransition<bool>(this.hasSupplies, this.operational.not_ready, (DoctorStation.StatesInstance smi, bool p) => !p);
			this.operational.ready.idle.ParamTransition<bool>(this.hasPatient, this.operational.ready.has_patient, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.has_patient.ParamTransition<bool>(this.hasPatient, this.operational.ready.idle, (DoctorStation.StatesInstance smi, bool p) => !p).DefaultState(this.operational.ready.has_patient.waiting).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreateDoctorChore), null);
			this.operational.ready.has_patient.waiting.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.being_treated, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.has_patient.being_treated.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.waiting, (DoctorStation.StatesInstance smi, bool p) => !p).Enter(delegate(DoctorStation.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(DoctorStation.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

		// Token: 0x06009577 RID: 38263 RVA: 0x0035FD04 File Offset: 0x0035DF04
		private Chore CreatePatientChore(DoctorStation.StatesInstance smi)
		{
			WorkChore<DoctorStation> workChore = new WorkChore<DoctorStation>(Db.Get().ChoreTypes.GetDoctored, smi.master, null, true, null, null, null, false, null, false, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
			workChore.AddPrecondition(DoctorStation.TreatmentAvailable, smi.master);
			workChore.AddPrecondition(DoctorStation.DoctorAvailable, smi.master);
			return workChore;
		}

		// Token: 0x06009578 RID: 38264 RVA: 0x0035FD60 File Offset: 0x0035DF60
		private Chore CreateDoctorChore(DoctorStation.StatesInstance smi)
		{
			DoctorStationDoctorWorkable component = smi.master.GetComponent<DoctorStationDoctorWorkable>();
			return new WorkChore<DoctorStationDoctorWorkable>(Db.Get().ChoreTypes.Doctor, component, null, true, null, null, null, false, null, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x04007293 RID: 29331
		public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State unoperational;

		// Token: 0x04007294 RID: 29332
		public DoctorStation.States.OperationalStates operational;

		// Token: 0x04007295 RID: 29333
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasSupplies;

		// Token: 0x04007296 RID: 29334
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasPatient;

		// Token: 0x04007297 RID: 29335
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasDoctor;

		// Token: 0x02002582 RID: 9602
		public class OperationalStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x0400A717 RID: 42775
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State not_ready;

			// Token: 0x0400A718 RID: 42776
			public DoctorStation.States.ReadyStates ready;
		}

		// Token: 0x02002583 RID: 9603
		public class ReadyStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x0400A719 RID: 42777
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State idle;

			// Token: 0x0400A71A RID: 42778
			public DoctorStation.States.PatientStates has_patient;
		}

		// Token: 0x02002584 RID: 9604
		public class PatientStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x0400A71B RID: 42779
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State waiting;

			// Token: 0x0400A71C RID: 42780
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State being_treated;
		}
	}

	// Token: 0x02001760 RID: 5984
	public class StatesInstance : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.GameInstance
	{
		// Token: 0x0600957A RID: 38266 RVA: 0x0035FDA7 File Offset: 0x0035DFA7
		public StatesInstance(DoctorStation master) : base(master)
		{
		}
	}
}

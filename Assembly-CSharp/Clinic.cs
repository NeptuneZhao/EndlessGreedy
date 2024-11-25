using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007B2 RID: 1970
[AddComponentMenu("KMonoBehaviour/Workable/Clinic")]
public class Clinic : Workable, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x060035F9 RID: 13817 RVA: 0x00126230 File Offset: 0x00124430
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.assignable.subSlots = new AssignableSlot[]
		{
			Db.Get().AssignableSlots.MedicalBed
		};
		this.assignable.AddAutoassignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAutoAssignTo));
		this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanManuallyAssignTo));
	}

	// Token: 0x060035FA RID: 13818 RVA: 0x0012629B File Offset: 0x0012449B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		Components.Clinics.Add(this);
		base.SetWorkTime(float.PositiveInfinity);
		this.clinicSMI = new Clinic.ClinicSM.Instance(this);
		this.clinicSMI.StartSM();
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x001262DB File Offset: 0x001244DB
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		Components.Clinics.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x001262FC File Offset: 0x001244FC
	private KAnimFile[] GetAppropriateOverrideAnims(WorkerBase worker)
	{
		KAnimFile[] result = null;
		if (!worker.GetSMI<WoundMonitor.Instance>().ShouldExitInfirmary())
		{
			result = this.workerInjuredAnims;
		}
		else if (this.workerDiseasedAnims != null && this.IsValidEffect(this.diseaseEffect) && worker.GetSMI<SicknessMonitor.Instance>().IsSick())
		{
			result = this.workerDiseasedAnims;
		}
		return result;
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x0012634C File Offset: 0x0012454C
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		this.overrideAnims = this.GetAppropriateOverrideAnims(worker);
		return base.GetAnim(worker);
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x00126362 File Offset: 0x00124562
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("Sleep", false);
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x00126380 File Offset: 0x00124580
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		KAnimFile[] appropriateOverrideAnims = this.GetAppropriateOverrideAnims(worker);
		if (appropriateOverrideAnims == null || appropriateOverrideAnims != this.overrideAnims)
		{
			return true;
		}
		base.OnWorkTick(worker, dt);
		return false;
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x001263AD File Offset: 0x001245AD
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("Sleep");
		base.OnStopWork(worker);
	}

	// Token: 0x06003601 RID: 13825 RVA: 0x001263C8 File Offset: 0x001245C8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.assignable.Unassign();
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Clinic.EffectsRemoved.Length; i++)
		{
			string effect_id = Clinic.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x0012640F File Offset: 0x0012460F
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x00126414 File Offset: 0x00124614
	private Chore CreateWorkChore(ChoreType chore_type, bool allow_prioritization, bool allow_in_red_alert, PriorityScreen.PriorityClass priority_class, bool ignore_schedule_block = false)
	{
		return new WorkChore<Clinic>(chore_type, this, null, true, null, null, null, allow_in_red_alert, null, ignore_schedule_block, true, null, false, true, allow_prioritization, priority_class, 5, false, false);
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x00126440 File Offset: 0x00124640
	private bool CanAutoAssignTo(MinionAssignablesProxy worker)
	{
		bool flag = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			if (this.IsValidEffect(this.healthEffect))
			{
				Health component = minionIdentity.GetComponent<Health>();
				if (component != null && component.hitPoints < component.maxHitPoints)
				{
					flag = true;
				}
			}
			if (!flag && this.IsValidEffect(this.diseaseEffect))
			{
				flag = (minionIdentity.GetComponent<MinionModifiers>().sicknesses.Count > 0);
			}
		}
		return flag;
	}

	// Token: 0x06003605 RID: 13829 RVA: 0x001264B8 File Offset: 0x001246B8
	private bool CanManuallyAssignTo(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = this.IsHealthBelowThreshold(minionIdentity.gameObject);
		}
		return result;
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x001264EC File Offset: 0x001246EC
	private bool IsHealthBelowThreshold(GameObject minion)
	{
		Health health = (minion != null) ? minion.GetComponent<Health>() : null;
		if (health != null)
		{
			float num = health.hitPoints / health.maxHitPoints;
			if (health != null)
			{
				return num < this.MedicalAttentionMinimum;
			}
		}
		return false;
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x00126537 File Offset: 0x00124737
	private bool IsValidEffect(string effect)
	{
		return effect != null && effect != "";
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x00126549 File Offset: 0x00124749
	private bool AllowDoctoring()
	{
		return this.IsValidEffect(this.doctoredDiseaseEffect) || this.IsValidEffect(this.doctoredHealthEffect);
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x00126568 File Offset: 0x00124768
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.IsValidEffect(this.healthEffect))
		{
			Effect.AddModifierDescriptions(base.gameObject, descriptors, this.healthEffect, false);
		}
		if (this.diseaseEffect != this.healthEffect && this.IsValidEffect(this.diseaseEffect))
		{
			Effect.AddModifierDescriptions(base.gameObject, descriptors, this.diseaseEffect, false);
		}
		if (this.AllowDoctoring())
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.DOCTORING, UI.BUILDINGEFFECTS.TOOLTIPS.DOCTORING, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			if (this.IsValidEffect(this.doctoredHealthEffect))
			{
				Effect.AddModifierDescriptions(base.gameObject, descriptors, this.doctoredHealthEffect, true);
			}
			if (this.doctoredDiseaseEffect != this.doctoredHealthEffect && this.IsValidEffect(this.doctoredDiseaseEffect))
			{
				Effect.AddModifierDescriptions(base.gameObject, descriptors, this.doctoredDiseaseEffect, true);
			}
		}
		return descriptors;
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x0600360A RID: 13834 RVA: 0x0012665E File Offset: 0x0012485E
	public float MedicalAttentionMinimum
	{
		get
		{
			return this.sicknessSliderValue / 100f;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x0600360B RID: 13835 RVA: 0x0012666C File Offset: 0x0012486C
	string ISliderControl.SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TITLE";
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x0600360C RID: 13836 RVA: 0x00126673 File Offset: 0x00124873
	string ISliderControl.SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x0012667F File Offset: 0x0012487F
	int ISliderControl.SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x00126682 File Offset: 0x00124882
	float ISliderControl.GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x00126689 File Offset: 0x00124889
	float ISliderControl.GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x00126690 File Offset: 0x00124890
	float ISliderControl.GetSliderValue(int index)
	{
		return this.sicknessSliderValue;
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x00126698 File Offset: 0x00124898
	void ISliderControl.SetSliderValue(float percent, int index)
	{
		if (percent != this.sicknessSliderValue)
		{
			this.sicknessSliderValue = (float)Mathf.RoundToInt(percent);
			Game.Instance.Trigger(875045922, null);
		}
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x001266C0 File Offset: 0x001248C0
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP"), this.sicknessSliderValue);
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x001266E1 File Offset: 0x001248E1
	string ISliderControl.GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP";
	}

	// Token: 0x0400201C RID: 8220
	[MyCmpReq]
	private Assignable assignable;

	// Token: 0x0400201D RID: 8221
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x0400201E RID: 8222
	private const int MAX_RANGE = 10;

	// Token: 0x0400201F RID: 8223
	private const float CHECK_RANGE_INTERVAL = 10f;

	// Token: 0x04002020 RID: 8224
	public float doctorVisitInterval = 300f;

	// Token: 0x04002021 RID: 8225
	public KAnimFile[] workerInjuredAnims;

	// Token: 0x04002022 RID: 8226
	public KAnimFile[] workerDiseasedAnims;

	// Token: 0x04002023 RID: 8227
	public string diseaseEffect;

	// Token: 0x04002024 RID: 8228
	public string healthEffect;

	// Token: 0x04002025 RID: 8229
	public string doctoredDiseaseEffect;

	// Token: 0x04002026 RID: 8230
	public string doctoredHealthEffect;

	// Token: 0x04002027 RID: 8231
	public string doctoredPlaceholderEffect;

	// Token: 0x04002028 RID: 8232
	private Clinic.ClinicSM.Instance clinicSMI;

	// Token: 0x04002029 RID: 8233
	public static readonly Chore.Precondition IsOverSicknessThreshold = new Chore.Precondition
	{
		id = "IsOverSicknessThreshold",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((Clinic)data).IsHealthBelowThreshold(context.consumerState.gameObject);
		}
	};

	// Token: 0x0400202A RID: 8234
	[Serialize]
	private float sicknessSliderValue = 70f;

	// Token: 0x0200166A RID: 5738
	public class ClinicSM : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic>
	{
		// Token: 0x06009224 RID: 37412 RVA: 0x003531DC File Offset: 0x003513DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Clinic.ClinicSM.Instance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Assignable>().Unassign();
			});
			this.operational.DefaultState(this.operational.idle).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Clinic.ClinicSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.AssigneeChanged, this.unoperational, null).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.Heal, false, true, PriorityScreen.PriorityClass.personalNeeds, false), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.healthEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.HealCritical, false, true, PriorityScreen.PriorityClass.personalNeeds, false), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.healthEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.RestDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.SleepDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect));
			this.operational.idle.WorkableStartTransition((Clinic.ClinicSM.Instance smi) => smi.master, this.operational.healing);
			this.operational.healing.DefaultState(this.operational.healing.undoctored).WorkableStopTransition((Clinic.ClinicSM.Instance smi) => smi.GetComponent<Clinic>(), this.operational.idle).Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(false, false);
			});
			this.operational.healing.undoctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StartEffect(smi.master.healthEffect, false);
				smi.StartEffect(smi.master.diseaseEffect, false);
				bool flag = false;
				if (smi.master.worker != null)
				{
					flag = (smi.HasEffect(smi.master.doctoredHealthEffect) || smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredPlaceholderEffect));
				}
				if (smi.master.AllowDoctoring())
				{
					if (flag)
					{
						smi.GoTo(this.operational.healing.doctored);
						return;
					}
					smi.StartDoctorChore();
				}
			}).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StopEffect(smi.master.healthEffect);
				smi.StopEffect(smi.master.diseaseEffect);
				smi.StopDoctorChore();
			});
			this.operational.healing.newlyDoctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
				smi.StartEffect(smi.master.doctoredHealthEffect, true);
				smi.GoTo(this.operational.healing.doctored);
			});
			this.operational.healing.doctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				if (smi.HasEffect(smi.master.doctoredPlaceholderEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredPlaceholderEffect);
					EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
					if (effectInstance2 != null)
					{
						float num = effectInstance.effect.duration - effectInstance.timeRemaining;
						effectInstance2.timeRemaining = effectInstance2.effect.duration - num;
					}
					EffectInstance effectInstance3 = smi.StartEffect(smi.master.doctoredHealthEffect, true);
					if (effectInstance3 != null)
					{
						float num2 = effectInstance.effect.duration - effectInstance.timeRemaining;
						effectInstance3.timeRemaining = effectInstance3.effect.duration - num2;
					}
					component.Remove(smi.master.doctoredPlaceholderEffect);
				}
			}).ScheduleGoTo(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				float num = smi.master.doctorVisitInterval;
				if (smi.HasEffect(smi.master.doctoredHealthEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredHealthEffect);
					num = Mathf.Min(num, effectInstance.GetTimeRemaining());
				}
				if (smi.HasEffect(smi.master.doctoredDiseaseEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
					num = Mathf.Min(num, effectInstance.GetTimeRemaining());
				}
				return num;
			}, this.operational.healing.undoctored).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				if (smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredHealthEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
					if (effectInstance == null)
					{
						effectInstance = component.Get(smi.master.doctoredHealthEffect);
					}
					EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredPlaceholderEffect, true);
					effectInstance2.timeRemaining = effectInstance2.effect.duration - (effectInstance.effect.duration - effectInstance.timeRemaining);
					component.Remove(smi.master.doctoredDiseaseEffect);
					component.Remove(smi.master.doctoredHealthEffect);
				}
			});
		}

		// Token: 0x04006F9E RID: 28574
		public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State unoperational;

		// Token: 0x04006F9F RID: 28575
		public Clinic.ClinicSM.OperationalStates operational;

		// Token: 0x0200255E RID: 9566
		public class OperationalStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
		{
			// Token: 0x0400A684 RID: 42628
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State idle;

			// Token: 0x0400A685 RID: 42629
			public Clinic.ClinicSM.HealingStates healing;
		}

		// Token: 0x0200255F RID: 9567
		public class HealingStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
		{
			// Token: 0x0400A686 RID: 42630
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State undoctored;

			// Token: 0x0400A687 RID: 42631
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State doctored;

			// Token: 0x0400A688 RID: 42632
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State newlyDoctored;
		}

		// Token: 0x02002560 RID: 9568
		public new class Instance : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.GameInstance
		{
			// Token: 0x0600BE7E RID: 48766 RVA: 0x003D8D2E File Offset: 0x003D6F2E
			public Instance(Clinic master) : base(master)
			{
			}

			// Token: 0x0600BE7F RID: 48767 RVA: 0x003D8D38 File Offset: 0x003D6F38
			public void StartDoctorChore()
			{
				if (base.master.IsValidEffect(base.master.doctoredHealthEffect) || base.master.IsValidEffect(base.master.doctoredDiseaseEffect))
				{
					this.doctorChore = new WorkChore<DoctorChoreWorkable>(Db.Get().ChoreTypes.Doctor, base.smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
					WorkChore<DoctorChoreWorkable> workChore = this.doctorChore;
					workChore.onComplete = (Action<Chore>)Delegate.Combine(workChore.onComplete, new Action<Chore>(delegate(Chore chore)
					{
						base.smi.GoTo(base.smi.sm.operational.healing.newlyDoctored);
					}));
				}
			}

			// Token: 0x0600BE80 RID: 48768 RVA: 0x003D8DD2 File Offset: 0x003D6FD2
			public void StopDoctorChore()
			{
				if (this.doctorChore != null)
				{
					this.doctorChore.Cancel("StopDoctorChore");
					this.doctorChore = null;
				}
			}

			// Token: 0x0600BE81 RID: 48769 RVA: 0x003D8DF4 File Offset: 0x003D6FF4
			public bool HasEffect(string effect)
			{
				bool result = false;
				if (base.master.IsValidEffect(effect))
				{
					result = base.smi.master.worker.GetComponent<Effects>().HasEffect(effect);
				}
				return result;
			}

			// Token: 0x0600BE82 RID: 48770 RVA: 0x003D8E30 File Offset: 0x003D7030
			public EffectInstance StartEffect(string effect, bool should_save)
			{
				if (base.master.IsValidEffect(effect))
				{
					WorkerBase worker = base.smi.master.worker;
					if (worker != null)
					{
						Effects component = worker.GetComponent<Effects>();
						if (!component.HasEffect(effect))
						{
							return component.Add(effect, should_save);
						}
					}
				}
				return null;
			}

			// Token: 0x0600BE83 RID: 48771 RVA: 0x003D8E80 File Offset: 0x003D7080
			public void StopEffect(string effect)
			{
				if (base.master.IsValidEffect(effect))
				{
					WorkerBase worker = base.smi.master.worker;
					if (worker != null)
					{
						Effects component = worker.GetComponent<Effects>();
						if (component.HasEffect(effect))
						{
							component.Remove(effect);
						}
					}
				}
			}

			// Token: 0x0400A689 RID: 42633
			private WorkChore<DoctorChoreWorkable> doctorChore;
		}
	}
}

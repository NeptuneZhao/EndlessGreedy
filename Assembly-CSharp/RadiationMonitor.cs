using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000995 RID: 2453
public class RadiationMonitor : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance>
{
	// Token: 0x0600478C RID: 18316 RVA: 0x00199184 File Offset: 0x00197384
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.init;
		this.init.Transition(null, (RadiationMonitor.Instance smi) => !Sim.IsRadiationEnabled(), UpdateRate.SIM_200ms).Transition(this.active, (RadiationMonitor.Instance smi) => Sim.IsRadiationEnabled(), UpdateRate.SIM_200ms);
		this.active.Update(new Action<RadiationMonitor.Instance, float>(RadiationMonitor.CheckRadiationLevel), UpdateRate.SIM_1000ms, false).DefaultState(this.active.idle);
		this.active.idle.DoNothing().ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ParamTransition<float>(this.radiationExposure, this.active.sick.major, RadiationMonitor.COMPARE_GTE_MAJOR).ParamTransition<float>(this.radiationExposure, this.active.sick.minor, RadiationMonitor.COMPARE_GTE_MINOR);
		this.active.sick.ParamTransition<float>(this.radiationExposure, this.active.idle, RadiationMonitor.COMPARE_LT_MINOR).Enter(delegate(RadiationMonitor.Instance smi)
		{
			smi.sm.isSick.Set(true, smi, false);
		}).Exit(delegate(RadiationMonitor.Instance smi)
		{
			smi.sm.isSick.Set(false, smi, false);
		});
		this.active.sick.minor.ToggleEffect(RadiationMonitor.minorSicknessEffect).ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ParamTransition<float>(this.radiationExposure, this.active.sick.major, RadiationMonitor.COMPARE_GTE_MAJOR).ToggleAnims("anim_loco_radiation1_kanim", 4f).ToggleAnims("anim_idle_radiation1_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation1, null).DefaultState(this.active.sick.minor.waiting);
		this.active.sick.minor.reacting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.minor.waiting);
		this.active.sick.major.ToggleEffect(RadiationMonitor.majorSicknessEffect).ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ToggleAnims("anim_loco_radiation2_kanim", 4f).ToggleAnims("anim_idle_radiation2_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation2, null).DefaultState(this.active.sick.major.waiting);
		this.active.sick.major.waiting.ScheduleGoTo(120f, this.active.sick.major.vomiting);
		this.active.sick.major.vomiting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.major.waiting);
		this.active.sick.extreme.ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ToggleEffect(RadiationMonitor.extremeSicknessEffect).ToggleAnims("anim_loco_radiation3_kanim", 4f).ToggleAnims("anim_idle_radiation3_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation3, null).DefaultState(this.active.sick.extreme.waiting);
		this.active.sick.extreme.waiting.ScheduleGoTo(60f, this.active.sick.extreme.vomiting);
		this.active.sick.extreme.vomiting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.extreme.waiting);
		this.active.sick.deadly.ToggleAnims("anim_loco_radiation4_kanim", 4f).ToggleAnims("anim_idle_radiation4_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation4, null).Enter(delegate(RadiationMonitor.Instance smi)
		{
			smi.GetComponent<Health>().Incapacitate(GameTags.RadiationSicknessIncapacitation);
		});
	}

	// Token: 0x0600478D RID: 18317 RVA: 0x001996A0 File Offset: 0x001978A0
	private Chore CreateVomitChore(RadiationMonitor.Instance smi)
	{
		Notification notification = new Notification(DUPLICANTS.STATUSITEMS.RADIATIONVOMITING.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.RADIATIONVOMITING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		return new VomitChore(Db.Get().ChoreTypes.Vomit, smi.master, Db.Get().DuplicantStatusItems.Vomiting, notification, null);
	}

	// Token: 0x0600478E RID: 18318 RVA: 0x00199718 File Offset: 0x00197918
	private static void RadiationRecovery(RadiationMonitor.Instance smi, float dt)
	{
		float num = Db.Get().Attributes.RadiationRecovery.Lookup(smi.gameObject).GetTotalValue() * dt;
		smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).ApplyDelta(num);
		smi.master.Trigger(1556680150, num);
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x00199788 File Offset: 0x00197988
	private static void CheckRadiationLevel(RadiationMonitor.Instance smi, float dt)
	{
		RadiationMonitor.RadiationRecovery(smi, dt);
		smi.sm.timeUntilNextExposureReact.Delta(-dt, smi);
		smi.sm.timeUntilNextSickReact.Delta(-dt, smi);
		int num = Grid.PosToCell(smi.gameObject);
		if (Grid.IsValidCell(num))
		{
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(smi.gameObject).GetTotalValue());
			float num3 = Grid.Radiation[num] * 1f * num2 / 600f * dt;
			smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).ApplyDelta(num3);
			float num4 = num3 / dt * 600f;
			smi.sm.currentExposurePerCycle.Set(num4, smi, false);
			if (smi.sm.timeUntilNextExposureReact.Get(smi) <= 0f && !smi.HasTag(GameTags.InTransitTube) && RadiationMonitor.COMPARE_REACT(smi, num4))
			{
				smi.sm.timeUntilNextExposureReact.Set(120f, smi, false);
				Emote radiation_Glare = Db.Get().Emotes.Minion.Radiation_Glare;
				smi.master.gameObject.GetSMI<ReactionMonitor.Instance>().AddSelfEmoteReactable(smi.master.gameObject, "RadiationReact", radiation_Glare, true, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 20f, float.NegativeInfinity, 0f, null);
			}
		}
		if (smi.sm.timeUntilNextSickReact.Get(smi) <= 0f && smi.sm.isSick.Get(smi) && !smi.HasTag(GameTags.InTransitTube))
		{
			smi.sm.timeUntilNextSickReact.Set(60f, smi, false);
			Emote radiation_Itch = Db.Get().Emotes.Minion.Radiation_Itch;
			smi.master.gameObject.GetSMI<ReactionMonitor.Instance>().AddSelfEmoteReactable(smi.master.gameObject, "RadiationReact", radiation_Itch, true, Db.Get().ChoreTypes.RadiationPain, 0f, 20f, float.NegativeInfinity, 0f, null);
		}
		smi.sm.radiationExposure.Set(smi.master.gameObject.GetComponent<KSelectable>().GetAmounts().GetValue("RadiationBalance"), smi, false);
	}

	// Token: 0x04002EB1 RID: 11953
	public const float BASE_ABSORBTION_RATE = 1f;

	// Token: 0x04002EB2 RID: 11954
	public const float MIN_TIME_BETWEEN_EXPOSURE_REACTS = 120f;

	// Token: 0x04002EB3 RID: 11955
	public const float MIN_TIME_BETWEEN_SICK_REACTS = 60f;

	// Token: 0x04002EB4 RID: 11956
	public const int VOMITS_PER_CYCLE_MAJOR = 5;

	// Token: 0x04002EB5 RID: 11957
	public const int VOMITS_PER_CYCLE_EXTREME = 10;

	// Token: 0x04002EB6 RID: 11958
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter radiationExposure;

	// Token: 0x04002EB7 RID: 11959
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter currentExposurePerCycle;

	// Token: 0x04002EB8 RID: 11960
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSick;

	// Token: 0x04002EB9 RID: 11961
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeUntilNextExposureReact;

	// Token: 0x04002EBA RID: 11962
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeUntilNextSickReact;

	// Token: 0x04002EBB RID: 11963
	public static string minorSicknessEffect = "RadiationExposureMinor";

	// Token: 0x04002EBC RID: 11964
	public static string majorSicknessEffect = "RadiationExposureMajor";

	// Token: 0x04002EBD RID: 11965
	public static string extremeSicknessEffect = "RadiationExposureExtreme";

	// Token: 0x04002EBE RID: 11966
	public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State init;

	// Token: 0x04002EBF RID: 11967
	public RadiationMonitor.ActiveStates active;

	// Token: 0x04002EC0 RID: 11968
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_RECOVERY_IMMEDIATE = (RadiationMonitor.Instance smi, float p) => p > 100f * smi.difficultySettingMod / 2f;

	// Token: 0x04002EC1 RID: 11969
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_REACT = (RadiationMonitor.Instance smi, float p) => p >= 133f * smi.difficultySettingMod;

	// Token: 0x04002EC2 RID: 11970
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_LT_MINOR = (RadiationMonitor.Instance smi, float p) => p < 100f * smi.difficultySettingMod;

	// Token: 0x04002EC3 RID: 11971
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_MINOR = (RadiationMonitor.Instance smi, float p) => p >= 100f * smi.difficultySettingMod;

	// Token: 0x04002EC4 RID: 11972
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_MAJOR = (RadiationMonitor.Instance smi, float p) => p >= 300f * smi.difficultySettingMod;

	// Token: 0x04002EC5 RID: 11973
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_EXTREME = (RadiationMonitor.Instance smi, float p) => p >= 600f * smi.difficultySettingMod;

	// Token: 0x04002EC6 RID: 11974
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_DEADLY = (RadiationMonitor.Instance smi, float p) => p >= 900f * smi.difficultySettingMod;

	// Token: 0x02001964 RID: 6500
	public class ActiveStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007945 RID: 31045
		public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04007946 RID: 31046
		public RadiationMonitor.SickStates sick;
	}

	// Token: 0x02001965 RID: 6501
	public class SickStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007947 RID: 31047
		public RadiationMonitor.SickStates.MinorStates minor;

		// Token: 0x04007948 RID: 31048
		public RadiationMonitor.SickStates.MajorStates major;

		// Token: 0x04007949 RID: 31049
		public RadiationMonitor.SickStates.ExtremeStates extreme;

		// Token: 0x0400794A RID: 31050
		public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State deadly;

		// Token: 0x020025C3 RID: 9667
		public class MinorStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400A823 RID: 43043
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x0400A824 RID: 43044
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State reacting;
		}

		// Token: 0x020025C4 RID: 9668
		public class MajorStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400A825 RID: 43045
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x0400A826 RID: 43046
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State vomiting;
		}

		// Token: 0x020025C5 RID: 9669
		public class ExtremeStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400A827 RID: 43047
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x0400A828 RID: 43048
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State vomiting;
		}
	}

	// Token: 0x02001966 RID: 6502
	public new class Instance : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C5D RID: 40029 RVA: 0x00372194 File Offset: 0x00370394
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			if (Sim.IsRadiationEnabled())
			{
				SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Radiation);
				if (currentQualitySetting != null)
				{
					string id = currentQualitySetting.id;
					if (id == "Easiest")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.EASIEST;
						return;
					}
					if (id == "Easier")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.EASIER;
						return;
					}
					if (id == "Harder")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.HARDER;
						return;
					}
					if (!(id == "Hardest"))
					{
						return;
					}
					this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.HARDEST;
				}
			}
		}

		// Token: 0x06009C5E RID: 40030 RVA: 0x00372244 File Offset: 0x00370444
		public float SicknessSecondsRemaining()
		{
			return 600f * (Mathf.Max(0f, base.sm.radiationExposure.Get(base.smi) - 100f * this.difficultySettingMod) / 100f);
		}

		// Token: 0x06009C5F RID: 40031 RVA: 0x00372280 File Offset: 0x00370480
		public string GetEffectStatusTooltip()
		{
			if (this.effects.HasEffect(RadiationMonitor.minorSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.minorSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.minorSicknessEffect));
			}
			if (this.effects.HasEffect(RadiationMonitor.majorSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.majorSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.majorSicknessEffect));
			}
			if (this.effects.HasEffect(RadiationMonitor.extremeSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.extremeSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.extremeSicknessEffect));
			}
			return DUPLICANTS.MODIFIERS.RADIATIONEXPOSUREDEADLY.TOOLTIP;
		}

		// Token: 0x0400794B RID: 31051
		public Effects effects;

		// Token: 0x0400794C RID: 31052
		public float difficultySettingMod = 1f;
	}
}

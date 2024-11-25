using System;
using Klei.AI;
using TUNING;

// Token: 0x0200097F RID: 2431
public class ExternalTemperatureMonitor : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance>
{
	// Token: 0x06004709 RID: 18185 RVA: 0x00196464 File Offset: 0x00194664
	public static float GetExternalColdThreshold(Attributes affected_attributes)
	{
		return -0.039f;
	}

	// Token: 0x0600470A RID: 18186 RVA: 0x0019646B File Offset: 0x0019466B
	public static float GetExternalWarmThreshold(Attributes affected_attributes)
	{
		return 0.008f;
	}

	// Token: 0x0600470B RID: 18187 RVA: 0x00196474 File Offset: 0x00194674
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.comfortable.Transition(this.transitionToTooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).Transition(this.transitionToTooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms);
		this.transitionToTooWarm.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot(), UpdateRate.SIM_200ms).Transition(this.tooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.transitionToTooCool.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold(), UpdateRate.SIM_200ms).Transition(this.tooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.tooWarm.ToggleTag(GameTags.FeelingWarm).Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.EffectAdded, this.comfortable, (ExternalTemperatureMonitor.Instance smi, object obj) => !smi.IsTooHot()).Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
		this.tooCool.ToggleTag(GameTags.FeelingCold).Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.EffectAdded, this.comfortable, (ExternalTemperatureMonitor.Instance smi, object obj) => !smi.IsTooCold()).Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
	}

	// Token: 0x04002E4C RID: 11852
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State comfortable;

	// Token: 0x04002E4D RID: 11853
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooWarm;

	// Token: 0x04002E4E RID: 11854
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooWarm;

	// Token: 0x04002E4F RID: 11855
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooCool;

	// Token: 0x04002E50 RID: 11856
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooCool;

	// Token: 0x04002E51 RID: 11857
	private const float BODY_TEMPERATURE_AFFECT_EXTERNAL_FEEL_THRESHOLD = 0.5f;

	// Token: 0x04002E52 RID: 11858
	public static readonly float BASE_STRESS_TOLERANCE_COLD = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS * 0.2f;

	// Token: 0x04002E53 RID: 11859
	public static readonly float BASE_STRESS_TOLERANCE_WARM = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS * 0.2f;

	// Token: 0x04002E54 RID: 11860
	private const float START_GAME_AVERAGING_DELAY = 6f;

	// Token: 0x04002E55 RID: 11861
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x04002E56 RID: 11862
	private const float TRANSITION_OUT_DELAY = 6f;

	// Token: 0x0200192B RID: 6443
	public new class Instance : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06009B6D RID: 39789 RVA: 0x0036F7EA File Offset: 0x0036D9EA
		public float GetCurrentColdThreshold
		{
			get
			{
				if (this.internalTemperatureMonitor.IdealTemperatureDelta() > 0.5f)
				{
					return 0f;
				}
				return CreatureSimTemperatureTransfer.PotentialEnergyFlowToCreature(Grid.PosToCell(base.gameObject), this.primaryElement, this.temperatureTransferer, 1f);
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06009B6E RID: 39790 RVA: 0x0036F825 File Offset: 0x0036DA25
		public float GetCurrentHotThreshold
		{
			get
			{
				return this.HotThreshold;
			}
		}

		// Token: 0x06009B6F RID: 39791 RVA: 0x0036F830 File Offset: 0x0036DA30
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.attributes = base.gameObject.GetAttributes();
			this.internalTemperatureMonitor = base.gameObject.GetSMI<TemperatureMonitor.Instance>();
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.temperatureTransferer = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			this.effects = base.gameObject.GetComponent<Effects>();
			this.traits = base.gameObject.GetComponent<Traits>();
		}

		// Token: 0x06009B70 RID: 39792 RVA: 0x0036F944 File Offset: 0x0036DB44
		public bool IsTooHot()
		{
			return !this.effects.HasEffect("RefreshingTouch") && !this.effects.HasImmunityTo(this.warmAirEffect) && this.temperatureTransferer.LastTemperatureRecordIsReliable && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage > ExternalTemperatureMonitor.GetExternalWarmThreshold(base.smi.attributes);
		}

		// Token: 0x06009B71 RID: 39793 RVA: 0x0036F9B4 File Offset: 0x0036DBB4
		public bool IsTooCold()
		{
			for (int i = 0; i < this.immunityToColdEffects.Length; i++)
			{
				if (this.effects.HasEffect(this.immunityToColdEffects[i]))
				{
					return false;
				}
			}
			return !this.effects.HasImmunityTo(this.coldAirEffect) && (!(this.traits != null) || !this.traits.IsEffectIgnored(this.coldAirEffect)) && !WarmthProvider.IsWarmCell(Grid.PosToCell(this)) && this.temperatureTransferer.LastTemperatureRecordIsReliable && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage < ExternalTemperatureMonitor.GetExternalColdThreshold(base.smi.attributes);
		}

		// Token: 0x04007889 RID: 30857
		public float HotThreshold = 306.15f;

		// Token: 0x0400788A RID: 30858
		public Effects effects;

		// Token: 0x0400788B RID: 30859
		public Traits traits;

		// Token: 0x0400788C RID: 30860
		public Attributes attributes;

		// Token: 0x0400788D RID: 30861
		public AmountInstance internalTemperature;

		// Token: 0x0400788E RID: 30862
		private TemperatureMonitor.Instance internalTemperatureMonitor;

		// Token: 0x0400788F RID: 30863
		public CreatureSimTemperatureTransfer temperatureTransferer;

		// Token: 0x04007890 RID: 30864
		public PrimaryElement primaryElement;

		// Token: 0x04007891 RID: 30865
		private Effect warmAirEffect = Db.Get().effects.Get("WarmAir");

		// Token: 0x04007892 RID: 30866
		private Effect coldAirEffect = Db.Get().effects.Get("ColdAir");

		// Token: 0x04007893 RID: 30867
		private Effect[] immunityToColdEffects = new Effect[]
		{
			Db.Get().effects.Get("WarmTouch"),
			Db.Get().effects.Get("WarmTouchFood")
		};
	}
}

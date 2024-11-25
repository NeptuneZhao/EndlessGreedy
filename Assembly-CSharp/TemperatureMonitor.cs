using System;
using Klei.AI;
using TUNING;

// Token: 0x020009A8 RID: 2472
public class TemperatureMonitor : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance>
{
	// Token: 0x060047EC RID: 18412 RVA: 0x0019C030 File Offset: 0x0019A230
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.homeostatic;
		this.root.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.averageTemperature = smi.primaryElement.Temperature;
		}).Update("UpdateTemperature", delegate(TemperatureMonitor.Instance smi, float dt)
		{
			smi.UpdateTemperature(dt);
		}, UpdateRate.SIM_200ms, false);
		this.homeostatic.Transition(this.hyperthermic_pre, (TemperatureMonitor.Instance smi) => smi.IsHyperthermic(), UpdateRate.SIM_200ms).Transition(this.hypothermic_pre, (TemperatureMonitor.Instance smi) => smi.IsHypothermic(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null);
		this.hyperthermic_pre.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.GoTo(this.hyperthermic);
		});
		this.hypothermic_pre.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.GoTo(this.hypothermic);
		});
		this.hyperthermic.Transition(this.homeostatic, (TemperatureMonitor.Instance smi) => !smi.IsHyperthermic(), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.CoolDown);
		this.hypothermic.Transition(this.homeostatic, (TemperatureMonitor.Instance smi) => !smi.IsHypothermic(), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.WarmUp);
	}

	// Token: 0x04002F16 RID: 12054
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State homeostatic;

	// Token: 0x04002F17 RID: 12055
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic;

	// Token: 0x04002F18 RID: 12056
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic;

	// Token: 0x04002F19 RID: 12057
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic_pre;

	// Token: 0x04002F1A RID: 12058
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic_pre;

	// Token: 0x04002F1B RID: 12059
	private const float TEMPERATURE_AVERAGING_RANGE = 4f;

	// Token: 0x04002F1C RID: 12060
	public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter warmUpCell;

	// Token: 0x04002F1D RID: 12061
	public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter coolDownCell;

	// Token: 0x02001997 RID: 6551
	public new class Instance : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009D3C RID: 40252 RVA: 0x00374564 File Offset: 0x00372764
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.primaryElement = base.GetComponent<PrimaryElement>();
			this.temperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.warmUpQuery = new SafetyQuery(Game.Instance.safetyConditions.WarmUpChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.coolDownQuery = new SafetyQuery(Game.Instance.safetyConditions.CoolDownChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = base.GetComponent<Navigator>();
		}

		// Token: 0x06009D3D RID: 40253 RVA: 0x00374610 File Offset: 0x00372810
		public void UpdateTemperature(float dt)
		{
			base.smi.averageTemperature *= 1f - dt / 4f;
			base.smi.averageTemperature += base.smi.primaryElement.Temperature * (dt / 4f);
			base.smi.temperature.SetValue(base.smi.averageTemperature);
		}

		// Token: 0x06009D3E RID: 40254 RVA: 0x00374682 File Offset: 0x00372882
		public bool IsHyperthermic()
		{
			return this.temperature.value > this.HyperthermiaThreshold;
		}

		// Token: 0x06009D3F RID: 40255 RVA: 0x00374697 File Offset: 0x00372897
		public bool IsHypothermic()
		{
			return this.temperature.value < this.HypothermiaThreshold;
		}

		// Token: 0x06009D40 RID: 40256 RVA: 0x003746AC File Offset: 0x003728AC
		public float ExtremeTemperatureDelta()
		{
			if (this.temperature.value > this.HyperthermiaThreshold)
			{
				return this.temperature.value - this.HyperthermiaThreshold;
			}
			if (this.temperature.value < this.HypothermiaThreshold)
			{
				return this.temperature.value - this.HypothermiaThreshold;
			}
			return 0f;
		}

		// Token: 0x06009D41 RID: 40257 RVA: 0x0037470A File Offset: 0x0037290A
		public float IdealTemperatureDelta()
		{
			return this.temperature.value - DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
		}

		// Token: 0x06009D42 RID: 40258 RVA: 0x0037472C File Offset: 0x0037292C
		public int GetWarmUpCell()
		{
			return base.sm.warmUpCell.Get(base.smi);
		}

		// Token: 0x06009D43 RID: 40259 RVA: 0x00374744 File Offset: 0x00372944
		public int GetCoolDownCell()
		{
			return base.sm.coolDownCell.Get(base.smi);
		}

		// Token: 0x06009D44 RID: 40260 RVA: 0x0037475C File Offset: 0x0037295C
		public void UpdateWarmUpCell()
		{
			this.warmUpQuery.Reset();
			this.navigator.RunQuery(this.warmUpQuery);
			base.sm.warmUpCell.Set(this.warmUpQuery.GetResultCell(), base.smi, false);
		}

		// Token: 0x06009D45 RID: 40261 RVA: 0x003747A8 File Offset: 0x003729A8
		public void UpdateCoolDownCell()
		{
			this.coolDownQuery.Reset();
			this.navigator.RunQuery(this.coolDownQuery);
			base.sm.coolDownCell.Set(this.coolDownQuery.GetResultCell(), base.smi, false);
		}

		// Token: 0x040079F3 RID: 31219
		public AmountInstance temperature;

		// Token: 0x040079F4 RID: 31220
		public PrimaryElement primaryElement;

		// Token: 0x040079F5 RID: 31221
		private Navigator navigator;

		// Token: 0x040079F6 RID: 31222
		private SafetyQuery warmUpQuery;

		// Token: 0x040079F7 RID: 31223
		private SafetyQuery coolDownQuery;

		// Token: 0x040079F8 RID: 31224
		public float averageTemperature;

		// Token: 0x040079F9 RID: 31225
		public float HypothermiaThreshold = 307.15f;

		// Token: 0x040079FA RID: 31226
		public float HyperthermiaThreshold = 313.15f;
	}
}

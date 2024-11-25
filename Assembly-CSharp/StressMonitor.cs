using System;
using Klei.AI;
using Klei.CustomSettings;

// Token: 0x020009A5 RID: 2469
public class StressMonitor : GameStateMachine<StressMonitor, StressMonitor.Instance>
{
	// Token: 0x060047E6 RID: 18406 RVA: 0x0019BD28 File Offset: 0x00199F28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		default_state = this.satisfied;
		this.root.Update("StressMonitor", delegate(StressMonitor.Instance smi, float dt)
		{
			smi.ReportStress(dt);
		}, UpdateRate.SIM_200ms, false);
		this.satisfied.TriggerOnEnter(GameHashes.NotStressed, null).Transition(this.stressed.tier1, (StressMonitor.Instance smi) => smi.stress.value >= 60f, UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Neutral, null);
		this.stressed.ToggleStatusItem(Db.Get().DuplicantStatusItems.Stressed, null).Transition(this.satisfied, (StressMonitor.Instance smi) => smi.stress.value < 60f, UpdateRate.SIM_200ms).ToggleReactable((StressMonitor.Instance smi) => smi.CreateConcernReactable()).TriggerOnEnter(GameHashes.Stressed, null);
		this.stressed.tier1.Transition(this.stressed.tier2, (StressMonitor.Instance smi) => smi.HasHadEnough(), UpdateRate.SIM_200ms);
		this.stressed.tier2.TriggerOnEnter(GameHashes.StressedHadEnough, null).Transition(this.stressed.tier1, (StressMonitor.Instance smi) => !smi.HasHadEnough(), UpdateRate.SIM_200ms);
	}

	// Token: 0x04002F0E RID: 12046
	public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002F0F RID: 12047
	public StressMonitor.Stressed stressed;

	// Token: 0x04002F10 RID: 12048
	private const float StressThreshold_One = 60f;

	// Token: 0x04002F11 RID: 12049
	private const float StressThreshold_Two = 100f;

	// Token: 0x02001990 RID: 6544
	public class Stressed : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040079DC RID: 31196
		public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier1;

		// Token: 0x040079DD RID: 31197
		public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier2;
	}

	// Token: 0x02001991 RID: 6545
	public new class Instance : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009D1E RID: 40222 RVA: 0x00373F5C File Offset: 0x0037215C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.stress = Db.Get().Amounts.Stress.Lookup(base.gameObject);
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.StressBreaks.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.StressBreaks);
			this.allowStressBreak = settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}

		// Token: 0x06009D1F RID: 40223 RVA: 0x00373FD3 File Offset: 0x003721D3
		public bool IsStressed()
		{
			return base.IsInsideState(base.sm.stressed);
		}

		// Token: 0x06009D20 RID: 40224 RVA: 0x00373FE6 File Offset: 0x003721E6
		public bool HasHadEnough()
		{
			return this.allowStressBreak && this.stress.value >= 100f;
		}

		// Token: 0x06009D21 RID: 40225 RVA: 0x00374008 File Offset: 0x00372208
		public void ReportStress(float dt)
		{
			for (int num = 0; num != this.stress.deltaAttribute.Modifiers.Count; num++)
			{
				AttributeModifier attributeModifier = this.stress.deltaAttribute.Modifiers[num];
				DebugUtil.DevAssert(!attributeModifier.IsMultiplier, "Reporting stress for multipliers not supported yet.", null);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.StressDelta, attributeModifier.Value * dt, attributeModifier.GetDescription(), base.gameObject.GetProperName());
			}
		}

		// Token: 0x06009D22 RID: 40226 RVA: 0x00374084 File Offset: 0x00372284
		public Reactable CreateConcernReactable()
		{
			return new EmoteReactable(base.master.gameObject, "StressConcern", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 30f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Concern);
		}

		// Token: 0x040079DE RID: 31198
		public AmountInstance stress;

		// Token: 0x040079DF RID: 31199
		private bool allowStressBreak = true;
	}
}

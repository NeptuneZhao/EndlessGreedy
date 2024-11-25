using System;
using STRINGS;

// Token: 0x02000664 RID: 1636
public class BionicUpgrade_OnGoingEffect : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>
{
	// Token: 0x0600285B RID: 10331 RVA: 0x000E4C7C File Offset: 0x000E2E7C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.ToggleEffect(new Func<BionicUpgrade_OnGoingEffect.Instance, string>(BionicUpgrade_OnGoingEffect.GetEffectName)).Enter(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.State.Callback(BionicUpgrade_OnGoingEffect.ApplySkills)).Exit(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.State.Callback(BionicUpgrade_OnGoingEffect.RemoveSkills)).EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBatterySaveMode)).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Not(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsOnline))).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x000E4D9C File Offset: 0x000E2F9C
	public static string GetEffectName(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		return ((BionicUpgrade_OnGoingEffect.Def)smi.def).EFFECT_NAME;
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x000E4DAE File Offset: 0x000E2FAE
	public static void ApplySkills(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		smi.ApplySkills();
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x000E4DB6 File Offset: 0x000E2FB6
	public static void RemoveSkills(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		smi.RemoveSkills();
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x000E4DBE File Offset: 0x000E2FBE
	public static bool IsOnlineAndNotInBatterySaveMode(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsOnline(smi) && !BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBatterySaveMode(smi);
	}

	// Token: 0x02001444 RID: 5188
	public new class Def : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def
	{
		// Token: 0x060089F0 RID: 35312 RVA: 0x00331D91 File Offset: 0x0032FF91
		public Def(string upgradeID, string effectID, string[] skills = null) : base(upgradeID)
		{
			this.EFFECT_NAME = effectID;
			this.SKILLS_IDS = skills;
		}

		// Token: 0x04006944 RID: 26948
		public string EFFECT_NAME;

		// Token: 0x04006945 RID: 26949
		public string[] SKILLS_IDS;
	}

	// Token: 0x02001445 RID: 5189
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.BaseInstance
	{
		// Token: 0x060089F1 RID: 35313 RVA: 0x00331DA8 File Offset: 0x0032FFA8
		public Instance(IStateMachineTarget master, BionicUpgrade_OnGoingEffect.Def def) : base(master, def)
		{
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x060089F2 RID: 35314 RVA: 0x00331DBE File Offset: 0x0032FFBE
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x060089F3 RID: 35315 RVA: 0x00331DE4 File Offset: 0x0032FFE4
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				string str = "<b>" + ((currentWattageCost >= 0f) ? "+" : "-") + "</b>";
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x060089F4 RID: 35316 RVA: 0x00331E84 File Offset: 0x00330084
		public void ApplySkills()
		{
			BionicUpgrade_OnGoingEffect.Def def = (BionicUpgrade_OnGoingEffect.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.GrantSkill(skillId);
				}
			}
		}

		// Token: 0x060089F5 RID: 35317 RVA: 0x00331ED0 File Offset: 0x003300D0
		public void RemoveSkills()
		{
			BionicUpgrade_OnGoingEffect.Def def = (BionicUpgrade_OnGoingEffect.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.UngrantSkill(skillId);
				}
			}
		}

		// Token: 0x04006946 RID: 26950
		private MinionResume resume;
	}
}

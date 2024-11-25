using System;
using Klei.AI;
using STRINGS;

// Token: 0x0200080E RID: 2062
public class HappinessMonitor : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>
{
	// Token: 0x06003905 RID: 14597 RVA: 0x00136D78 File Offset: 0x00134F78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.happy, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.neutral, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsNeutral), UpdateRate.SIM_1000ms).Transition(this.glum, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsGlum), UpdateRate.SIM_1000ms).Transition(this.miserable, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsMisirable), UpdateRate.SIM_1000ms);
		this.happy.DefaultState(this.happy.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Happy);
		this.happy.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.happyWildEffect).TagTransition(GameTags.Creatures.Wild, this.happy.tame, true);
		this.happy.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.happyTameEffect).TagTransition(GameTags.Creatures.Wild, this.happy.wild, false);
		this.neutral.DefaultState(this.neutral.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsNeutral)), UpdateRate.SIM_1000ms);
		this.neutral.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.neutralWildEffect).TagTransition(GameTags.Creatures.Wild, this.neutral.tame, true);
		this.neutral.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.neutralTameEffect).TagTransition(GameTags.Creatures.Wild, this.neutral.wild, false);
		this.glum.DefaultState(this.glum.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsGlum)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		this.glum.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.glumWildEffect).TagTransition(GameTags.Creatures.Wild, this.glum.tame, true);
		this.glum.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.glumTameEffect).TagTransition(GameTags.Creatures.Wild, this.glum.wild, false);
		this.miserable.DefaultState(this.miserable.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsMisirable)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		this.miserable.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.miserableWildEffect).TagTransition(GameTags.Creatures.Wild, this.miserable.tame, true);
		this.miserable.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.miserableTameEffect).TagTransition(GameTags.Creatures.Wild, this.miserable.wild, false);
		this.happyWildEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_WILD.NAME, CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.happyTameEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_TAME.NAME, CREATURES.MODIFIERS.HAPPY_TAME.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralWildEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralTameEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.glumWildEffect = new Effect("Glum", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.glumTameEffect = new Effect("Glum", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.miserableWildEffect = new Effect("Miserable", CREATURES.MODIFIERS.MISERABLE.NAME, CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.miserableTameEffect = new Effect("Miserable", CREATURES.MODIFIERS.MISERABLE.NAME, CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.happyTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 9f, CREATURES.MODIFIERS.HAPPY_TAME.NAME, true, false, true));
		this.glumWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.GLUM.NAME, false, false, true));
		this.glumTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.GLUM.NAME, false, false, true));
		this.miserableTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.MISERABLE.NAME, false, false, true));
		this.miserableTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, true, false, true));
		this.miserableWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.MISERABLE.NAME, false, false, true));
		this.miserableWildEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, true, false, true));
	}

	// Token: 0x06003906 RID: 14598 RVA: 0x001373EB File Offset: 0x001355EB
	private static bool IsHappy(HappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.happyThreshold;
	}

	// Token: 0x06003907 RID: 14599 RVA: 0x00137408 File Offset: 0x00135608
	private static bool IsNeutral(HappinessMonitor.Instance smi)
	{
		float totalValue = smi.happiness.GetTotalValue();
		return totalValue > smi.def.glumThreshold && totalValue < smi.def.happyThreshold;
	}

	// Token: 0x06003908 RID: 14600 RVA: 0x00137440 File Offset: 0x00135640
	private static bool IsGlum(HappinessMonitor.Instance smi)
	{
		float totalValue = smi.happiness.GetTotalValue();
		return totalValue > smi.def.miserableThreshold && totalValue <= smi.def.glumThreshold;
	}

	// Token: 0x06003909 RID: 14601 RVA: 0x0013747A File Offset: 0x0013567A
	private static bool IsMisirable(HappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() <= smi.def.miserableThreshold;
	}

	// Token: 0x0400224C RID: 8780
	private GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied;

	// Token: 0x0400224D RID: 8781
	private HappinessMonitor.HappyState happy;

	// Token: 0x0400224E RID: 8782
	private HappinessMonitor.NeutralState neutral;

	// Token: 0x0400224F RID: 8783
	private HappinessMonitor.UnhappyState glum;

	// Token: 0x04002250 RID: 8784
	private HappinessMonitor.MiserableState miserable;

	// Token: 0x04002251 RID: 8785
	private Effect happyWildEffect;

	// Token: 0x04002252 RID: 8786
	private Effect happyTameEffect;

	// Token: 0x04002253 RID: 8787
	private Effect neutralTameEffect;

	// Token: 0x04002254 RID: 8788
	private Effect neutralWildEffect;

	// Token: 0x04002255 RID: 8789
	private Effect glumWildEffect;

	// Token: 0x04002256 RID: 8790
	private Effect glumTameEffect;

	// Token: 0x04002257 RID: 8791
	private Effect miserableWildEffect;

	// Token: 0x04002258 RID: 8792
	private Effect miserableTameEffect;

	// Token: 0x0200170A RID: 5898
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400718C RID: 29068
		public float happyThreshold = 4f;

		// Token: 0x0400718D RID: 29069
		public float glumThreshold = -1f;

		// Token: 0x0400718E RID: 29070
		public float miserableThreshold = -10f;
	}

	// Token: 0x0200170B RID: 5899
	public class MiserableState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x0400718F RID: 29071
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04007190 RID: 29072
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x0200170C RID: 5900
	public class NeutralState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04007191 RID: 29073
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04007192 RID: 29074
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x0200170D RID: 5901
	public class UnhappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04007193 RID: 29075
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04007194 RID: 29076
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x0200170E RID: 5902
	public class HappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04007195 RID: 29077
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04007196 RID: 29078
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x0200170F RID: 5903
	public new class Instance : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.GameInstance
	{
		// Token: 0x0600947B RID: 38011 RVA: 0x0035C90C File Offset: 0x0035AB0C
		public Instance(IStateMachineTarget master, HappinessMonitor.Def def) : base(master, def)
		{
			this.happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}

		// Token: 0x04007197 RID: 29079
		public AttributeInstance happiness;
	}
}

using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020008CB RID: 2251
public class SuffocationMonitor : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance>
{
	// Token: 0x06003FFC RID: 16380 RVA: 0x0016A85C File Offset: 0x00168A5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (SuffocationMonitor.Instance smi) => smi.breathing, null).EventTransition(GameHashes.ExitedBreathableArea, this.noOxygen, (SuffocationMonitor.Instance smi) => !smi.IsInBreathableArea());
		this.satisfied.normal.Transition(this.satisfied.low, (SuffocationMonitor.Instance smi) => smi.oxygenBreather.IsLowOxygenAtMouthCell(), UpdateRate.SIM_200ms);
		this.satisfied.low.Transition(this.satisfied.normal, (SuffocationMonitor.Instance smi) => !smi.oxygenBreather.IsLowOxygenAtMouthCell(), UpdateRate.SIM_200ms).Transition(this.noOxygen, (SuffocationMonitor.Instance smi) => !smi.IsInBreathableArea(), UpdateRate.SIM_200ms).ToggleEffect("LowOxygen");
		this.noOxygen.EventTransition(GameHashes.EnteredBreathableArea, this.satisfied, (SuffocationMonitor.Instance smi) => smi.IsInBreathableArea()).TagTransition(GameTags.RecoveringBreath, this.satisfied, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (SuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
		this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.noOxygen.suffocating, (SuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (SuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(SuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x04002A44 RID: 10820
	public SuffocationMonitor.SatisfiedState satisfied;

	// Token: 0x04002A45 RID: 10821
	public SuffocationMonitor.NoOxygenState noOxygen;

	// Token: 0x04002A46 RID: 10822
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State death;

	// Token: 0x04002A47 RID: 10823
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x020017FE RID: 6142
	public class NoOxygenState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040074A0 RID: 29856
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State holdingbreath;

		// Token: 0x040074A1 RID: 29857
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State suffocating;
	}

	// Token: 0x020017FF RID: 6143
	public class SatisfiedState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040074A2 RID: 29858
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x040074A3 RID: 29859
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State low;
	}

	// Token: 0x02001800 RID: 6144
	public new class Instance : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x0600971A RID: 38682 RVA: 0x0036445D File Offset: 0x0036265D
		// (set) Token: 0x0600971B RID: 38683 RVA: 0x00364465 File Offset: 0x00362665
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x0600971C RID: 38684 RVA: 0x00364470 File Offset: 0x00362670
		public Instance(OxygenBreather oxygen_breather) : base(oxygen_breather)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(base.master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.breathing = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = oxygen_breather;
		}

		// Token: 0x0600971D RID: 38685 RVA: 0x00364514 File Offset: 0x00362714
		public bool IsInBreathableArea()
		{
			return base.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath) || base.master.GetComponent<Sensors>().GetSensor<BreathableAreaSensor>().IsBreathable() || this.oxygenBreather.HasTag(GameTags.InTransitTube);
		}

		// Token: 0x0600971E RID: 38686 RVA: 0x00364561 File Offset: 0x00362761
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x0600971F RID: 38687 RVA: 0x00364578 File Offset: 0x00362778
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x06009720 RID: 38688 RVA: 0x003645B2 File Offset: 0x003627B2
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x040074A4 RID: 29860
		private AmountInstance breath;

		// Token: 0x040074A5 RID: 29861
		public AttributeModifier breathing;

		// Token: 0x040074A6 RID: 29862
		public AttributeModifier holdingbreath;
	}
}

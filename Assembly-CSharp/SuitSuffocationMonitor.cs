using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000B2C RID: 2860
public class SuitSuffocationMonitor : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance>
{
	// Token: 0x06005558 RID: 21848 RVA: 0x001E7930 File Offset: 0x001E5B30
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (SuitSuffocationMonitor.Instance smi) => smi.breathing, null).Transition(this.nooxygen, (SuitSuffocationMonitor.Instance smi) => smi.IsTankEmpty(), UpdateRate.SIM_200ms);
		this.satisfied.normal.Transition(this.satisfied.low, (SuitSuffocationMonitor.Instance smi) => smi.suitTank.NeedsRecharging(), UpdateRate.SIM_200ms);
		this.satisfied.low.DoNothing();
		this.nooxygen.ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (SuitSuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.nooxygen.holdingbreath);
		this.nooxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.nooxygen.suffocating, (SuitSuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.nooxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (SuitSuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(SuitSuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
	}

	// Token: 0x040037DE RID: 14302
	public SuitSuffocationMonitor.SatisfiedState satisfied;

	// Token: 0x040037DF RID: 14303
	public SuitSuffocationMonitor.NoOxygenState nooxygen;

	// Token: 0x040037E0 RID: 14304
	public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State death;

	// Token: 0x02001B81 RID: 7041
	public class NoOxygenState : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007FE6 RID: 32742
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State holdingbreath;

		// Token: 0x04007FE7 RID: 32743
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State suffocating;
	}

	// Token: 0x02001B82 RID: 7042
	public class SatisfiedState : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007FE8 RID: 32744
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x04007FE9 RID: 32745
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State low;
	}

	// Token: 0x02001B83 RID: 7043
	public new class Instance : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x0600A390 RID: 41872 RVA: 0x00389F48 File Offset: 0x00388148
		// (set) Token: 0x0600A391 RID: 41873 RVA: 0x00389F50 File Offset: 0x00388150
		public SuitTank suitTank { get; private set; }

		// Token: 0x0600A392 RID: 41874 RVA: 0x00389F5C File Offset: 0x0038815C
		public Instance(IStateMachineTarget master, SuitTank suit_tank) : base(master)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.breathing = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.suitTank = suit_tank;
		}

		// Token: 0x0600A393 RID: 41875 RVA: 0x00389FFB File Offset: 0x003881FB
		public bool IsTankEmpty()
		{
			return this.suitTank.IsEmpty();
		}

		// Token: 0x0600A394 RID: 41876 RVA: 0x0038A008 File Offset: 0x00388208
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x0600A395 RID: 41877 RVA: 0x0038A01F File Offset: 0x0038821F
		public bool IsSuffocating()
		{
			return this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x0600A396 RID: 41878 RVA: 0x0038A040 File Offset: 0x00388240
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x04007FEA RID: 32746
		private AmountInstance breath;

		// Token: 0x04007FEB RID: 32747
		public AttributeModifier breathing;

		// Token: 0x04007FEC RID: 32748
		public AttributeModifier holdingbreath;

		// Token: 0x04007FED RID: 32749
		private OxygenBreather masterOxygenBreather;
	}
}

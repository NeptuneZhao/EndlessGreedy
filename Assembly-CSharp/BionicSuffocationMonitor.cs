using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200096A RID: 2410
public class BionicSuffocationMonitor : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>
{
	// Token: 0x0600469F RID: 18079 RVA: 0x00193EBC File Offset: 0x001920BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.normal;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.normal.ToggleAttributeModifier("Breathing", (BionicSuffocationMonitor.Instance smi) => smi.breathing, null).EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.noOxygen, (BionicSuffocationMonitor.Instance smi) => !smi.IsBreathing());
		this.noOxygen.EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.normal, (BionicSuffocationMonitor.Instance smi) => smi.IsBreathing()).TagTransition(GameTags.RecoveringBreath, this.normal, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (BionicSuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
		this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.noOxygen.suffocating, (BionicSuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (BionicSuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(BionicSuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x04002E03 RID: 11779
	public BionicSuffocationMonitor.NoOxygenState noOxygen;

	// Token: 0x04002E04 RID: 11780
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State normal;

	// Token: 0x04002E05 RID: 11781
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State death;

	// Token: 0x04002E06 RID: 11782
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State dead;

	// Token: 0x020018EA RID: 6378
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018EB RID: 6379
	public class NoOxygenState : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State
	{
		// Token: 0x040077D6 RID: 30678
		public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State holdingbreath;

		// Token: 0x040077D7 RID: 30679
		public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State suffocating;
	}

	// Token: 0x020018EC RID: 6380
	public new class Instance : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.GameInstance
	{
		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06009A6E RID: 39534 RVA: 0x0036D264 File Offset: 0x0036B464
		// (set) Token: 0x06009A6F RID: 39535 RVA: 0x0036D26C File Offset: 0x0036B46C
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x06009A70 RID: 39536 RVA: 0x0036D278 File Offset: 0x0036B478
		public Instance(IStateMachineTarget master, BionicSuffocationMonitor.Def def) : base(master, def)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.breathing = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x06009A71 RID: 39537 RVA: 0x0036D31D File Offset: 0x0036B51D
		public bool IsBreathing()
		{
			return !this.oxygenBreather.IsSuffocating || base.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.HasTag(GameTags.InTransitTube);
		}

		// Token: 0x06009A72 RID: 39538 RVA: 0x0036D355 File Offset: 0x0036B555
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x06009A73 RID: 39539 RVA: 0x0036D36C File Offset: 0x0036B56C
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x06009A74 RID: 39540 RVA: 0x0036D3A6 File Offset: 0x0036B5A6
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x040077D8 RID: 30680
		private AmountInstance breath;

		// Token: 0x040077D9 RID: 30681
		public AttributeModifier breathing;

		// Token: 0x040077DA RID: 30682
		public AttributeModifier holdingbreath;
	}
}

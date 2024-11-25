using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020009BD RID: 2493
[SkipSaveFileSerialization]
public class PrefersWarmer : StateMachineComponent<PrefersWarmer.StatesInstance>
{
	// Token: 0x06004874 RID: 18548 RVA: 0x0019EFF1 File Offset: 0x0019D1F1
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020019C9 RID: 6601
	public class StatesInstance : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer, object>.GameInstance
	{
		// Token: 0x06009E0A RID: 40458 RVA: 0x00376AD1 File Offset: 0x00374CD1
		public StatesInstance(PrefersWarmer master) : base(master)
		{
		}
	}

	// Token: 0x020019CA RID: 6602
	public class States : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer>
	{
		// Token: 0x06009E0B RID: 40459 RVA: 0x00376ADA File Offset: 0x00374CDA
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, (PrefersWarmer.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x04007AA6 RID: 31398
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.SKINNY, DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, false, false, true);
	}
}

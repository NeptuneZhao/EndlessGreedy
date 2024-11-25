using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020009BC RID: 2492
[SkipSaveFileSerialization]
public class PrefersColder : StateMachineComponent<PrefersColder.StatesInstance>
{
	// Token: 0x06004872 RID: 18546 RVA: 0x0019EFDC File Offset: 0x0019D1DC
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020019C7 RID: 6599
	public class StatesInstance : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder, object>.GameInstance
	{
		// Token: 0x06009E06 RID: 40454 RVA: 0x00376A5A File Offset: 0x00374C5A
		public StatesInstance(PrefersColder master) : base(master)
		{
		}
	}

	// Token: 0x020019C8 RID: 6600
	public class States : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder>
	{
		// Token: 0x06009E07 RID: 40455 RVA: 0x00376A63 File Offset: 0x00374C63
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (PrefersColder.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x04007AA5 RID: 31397
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.PUDGY, DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, false, false, true);
	}
}

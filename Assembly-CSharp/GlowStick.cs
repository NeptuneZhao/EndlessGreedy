using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020008D7 RID: 2263
[SkipSaveFileSerialization]
public class GlowStick : StateMachineComponent<GlowStick.StatesInstance>
{
	// Token: 0x0600408E RID: 16526 RVA: 0x0016F909 File Offset: 0x0016DB09
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001817 RID: 6167
	public class StatesInstance : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick, object>.GameInstance
	{
		// Token: 0x06009770 RID: 38768 RVA: 0x00365748 File Offset: 0x00363948
		public StatesInstance(GlowStick master) : base(master)
		{
			this._radiationEmitter.emitRads = 100f;
			this._radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			this._radiationEmitter.emitRate = 0.5f;
			this._radiationEmitter.emitRadiusX = 3;
			this._radiationEmitter.emitRadiusY = 3;
			this.radiationResistance = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, TRAITS.GLOWSTICK_RADIATION_RESISTANCE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
			this.luminescenceModifier = new AttributeModifier(Db.Get().Attributes.Luminescence.Id, TRAITS.GLOWSTICK_LUX_VALUE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
		}

		// Token: 0x0400750B RID: 29963
		[MyCmpAdd]
		private RadiationEmitter _radiationEmitter;

		// Token: 0x0400750C RID: 29964
		public AttributeModifier radiationResistance;

		// Token: 0x0400750D RID: 29965
		public AttributeModifier luminescenceModifier;
	}

	// Token: 0x02001818 RID: 6168
	public class States : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick>
	{
		// Token: 0x06009771 RID: 38769 RVA: 0x00365804 File Offset: 0x00363A04
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleComponent<RadiationEmitter>(false).ToggleAttributeModifier("Radiation Resistance", (GlowStick.StatesInstance smi) => smi.radiationResistance, null).ToggleAttributeModifier("Luminescence Modifier", (GlowStick.StatesInstance smi) => smi.luminescenceModifier, null);
		}
	}
}

using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A0E RID: 2574
[SkipSaveFileSerialization]
public class RadiationEater : StateMachineComponent<RadiationEater.StatesInstance>
{
	// Token: 0x06004AB0 RID: 19120 RVA: 0x001AB366 File Offset: 0x001A9566
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001A2C RID: 6700
	public class StatesInstance : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater, object>.GameInstance
	{
		// Token: 0x06009F44 RID: 40772 RVA: 0x0037BC56 File Offset: 0x00379E56
		public StatesInstance(RadiationEater master) : base(master)
		{
			this.radiationEating = new AttributeModifier(Db.Get().Attributes.RadiationRecovery.Id, TRAITS.RADIATION_EATER_RECOVERY, DUPLICANTS.TRAITS.RADIATIONEATER.NAME, false, false, true);
		}

		// Token: 0x06009F45 RID: 40773 RVA: 0x0037BC90 File Offset: 0x00379E90
		public void OnEatRads(float radsEaten)
		{
			float delta = Mathf.Abs(radsEaten) * TRAITS.RADS_TO_CALS;
			base.smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Calories).ApplyDelta(delta);
		}

		// Token: 0x04007BA8 RID: 31656
		public AttributeModifier radiationEating;
	}

	// Token: 0x02001A2D RID: 6701
	public class States : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater>
	{
		// Token: 0x06009F46 RID: 40774 RVA: 0x0037BCDC File Offset: 0x00379EDC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier("Radiation Eating", (RadiationEater.StatesInstance smi) => smi.radiationEating, null).EventHandler(GameHashes.RadiationRecovery, delegate(RadiationEater.StatesInstance smi, object data)
			{
				float radsEaten = (float)data;
				smi.OnEatRads(radsEaten);
			});
		}
	}
}

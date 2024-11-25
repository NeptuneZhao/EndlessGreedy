using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200093D RID: 2365
public class LeadSuitMonitor : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance>
{
	// Token: 0x060044C7 RID: 17607 RVA: 0x001875C4 File Offset: 0x001857C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wearingSuit;
		base.Target(this.owner);
		this.wearingSuit.DefaultState(this.wearingSuit.hasBattery);
		this.wearingSuit.hasBattery.Update(new Action<LeadSuitMonitor.Instance, float>(LeadSuitMonitor.CoolSuit), UpdateRate.SIM_200ms, false).TagTransition(GameTags.SuitBatteryOut, this.wearingSuit.noBattery, false);
		this.wearingSuit.noBattery.Enter(delegate(LeadSuitMonitor.Instance smi)
		{
			Attributes attributes = smi.sm.owner.Get(smi).GetAttributes();
			if (attributes != null)
			{
				foreach (AttributeModifier modifier in smi.noBatteryModifiers)
				{
					attributes.Add(modifier);
				}
			}
		}).Exit(delegate(LeadSuitMonitor.Instance smi)
		{
			Attributes attributes = smi.sm.owner.Get(smi).GetAttributes();
			if (attributes != null)
			{
				foreach (AttributeModifier modifier in smi.noBatteryModifiers)
				{
					attributes.Remove(modifier);
				}
			}
		}).TagTransition(GameTags.SuitBatteryOut, this.wearingSuit.hasBattery, true);
	}

	// Token: 0x060044C8 RID: 17608 RVA: 0x0018769C File Offset: 0x0018589C
	public static void CoolSuit(LeadSuitMonitor.Instance smi, float dt)
	{
		if (!smi.navigator)
		{
			return;
		}
		GameObject gameObject = smi.sm.owner.Get(smi);
		if (!gameObject)
		{
			return;
		}
		ScaldingMonitor.Instance smi2 = gameObject.GetSMI<ScaldingMonitor.Instance>();
		if (smi2 != null && smi2.AverageExternalTemperature >= smi.lead_suit_tank.coolingOperationalTemperature)
		{
			smi.lead_suit_tank.batteryCharge -= 1f / smi.lead_suit_tank.batteryDuration * dt;
			if (smi.lead_suit_tank.IsEmpty())
			{
				gameObject.AddTag(GameTags.SuitBatteryOut);
			}
		}
	}

	// Token: 0x04002CF9 RID: 11513
	public LeadSuitMonitor.WearingSuit wearingSuit;

	// Token: 0x04002CFA RID: 11514
	public StateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

	// Token: 0x020018A1 RID: 6305
	public class WearingSuit : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040076D1 RID: 30417
		public GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State hasBattery;

		// Token: 0x040076D2 RID: 30418
		public GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State noBattery;
	}

	// Token: 0x020018A2 RID: 6306
	public new class Instance : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600991C RID: 39196 RVA: 0x0036A388 File Offset: 0x00368588
		public Instance(IStateMachineTarget master, GameObject owner) : base(master)
		{
			base.sm.owner.Set(owner, base.smi, false);
			this.navigator = owner.GetComponent<Navigator>();
			this.lead_suit_tank = master.GetComponent<LeadSuitTank>();
			this.noBatteryModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float)(-(float)TUNING.EQUIPMENT.SUITS.LEADSUIT_INSULATION), STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.SUIT_OUT_OF_BATTERIES, false, false, true));
			this.noBatteryModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.THERMAL_CONDUCTIVITY_BARRIER, -TUNING.EQUIPMENT.SUITS.LEADSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.SUIT_OUT_OF_BATTERIES, false, false, true));
		}

		// Token: 0x040076D3 RID: 30419
		public Navigator navigator;

		// Token: 0x040076D4 RID: 30420
		public LeadSuitTank lead_suit_tank;

		// Token: 0x040076D5 RID: 30421
		public List<AttributeModifier> noBatteryModifiers = new List<AttributeModifier>();
	}
}

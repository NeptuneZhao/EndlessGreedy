﻿using System;
using UnityEngine;

// Token: 0x02000989 RID: 2441
public class IncapacitationMonitor : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance>
{
	// Token: 0x06004743 RID: 18243 RVA: 0x00197B30 File Offset: 0x00195D30
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.healthy.TagTransition(GameTags.CaloriesDepleted, this.incapacitated, false).TagTransition(GameTags.HitPointsDepleted, this.incapacitated, false).TagTransition(GameTags.HitByHighEnergyParticle, this.incapacitated, false).TagTransition(GameTags.RadiationSicknessIncapacitation, this.incapacitated, false).Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.RecoverStamina(dt, smi);
		}, UpdateRate.SIM_200ms, false);
		this.start_recovery.TagTransition(new Tag[]
		{
			GameTags.CaloriesDepleted,
			GameTags.HitPointsDepleted
		}, this.healthy, true);
		this.incapacitated.EventTransition(GameHashes.IncapacitationRecovery, this.start_recovery, null).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((IncapacitationMonitor.Instance smi) => new BeIncapacitatedChore(smi.master), null).ParamTransition<float>(this.bleedOutStamina, this.die, GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ToggleUrge(Db.Get().Urges.BeIncapacitated).Enter(delegate(IncapacitationMonitor.Instance smi)
		{
			smi.master.Trigger(-1506500077, null);
		}).Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.Bleed(dt, smi);
		}, UpdateRate.SIM_200ms, false);
		this.die.Enter(delegate(IncapacitationMonitor.Instance smi)
		{
			smi.master.gameObject.GetSMI<DeathMonitor.Instance>().Kill(smi.GetCauseOfIncapacitation());
		});
	}

	// Token: 0x04002E82 RID: 11906
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x04002E83 RID: 11907
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State start_recovery;

	// Token: 0x04002E84 RID: 11908
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x04002E85 RID: 11909
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State die;

	// Token: 0x04002E86 RID: 11910
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter bleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x04002E87 RID: 11911
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseBleedOutSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x04002E88 RID: 11912
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseStaminaRecoverSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x04002E89 RID: 11913
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter maxBleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x02001947 RID: 6471
	public new class Instance : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009BF4 RID: 39924 RVA: 0x00371580 File Offset: 0x0036F780
		public Instance(IStateMachineTarget master) : base(master)
		{
			Health component = master.GetComponent<Health>();
			if (component)
			{
				component.CanBeIncapacitated = true;
			}
		}

		// Token: 0x06009BF5 RID: 39925 RVA: 0x003715AA File Offset: 0x0036F7AA
		public void Bleed(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(dt * -smi.sm.baseBleedOutSpeed.Get(smi), smi);
		}

		// Token: 0x06009BF6 RID: 39926 RVA: 0x003715D4 File Offset: 0x0036F7D4
		public void RecoverStamina(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(Mathf.Min(dt * smi.sm.baseStaminaRecoverSpeed.Get(smi), smi.sm.maxBleedOutStamina.Get(smi) - smi.sm.bleedOutStamina.Get(smi)), smi);
		}

		// Token: 0x06009BF7 RID: 39927 RVA: 0x0037162E File Offset: 0x0036F82E
		public float GetBleedLifeTime(IncapacitationMonitor.Instance smi)
		{
			return Mathf.Floor(smi.sm.bleedOutStamina.Get(smi) / smi.sm.baseBleedOutSpeed.Get(smi));
		}

		// Token: 0x06009BF8 RID: 39928 RVA: 0x00371658 File Offset: 0x0036F858
		public Death GetCauseOfIncapacitation()
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (component.HasTag(GameTags.HitByHighEnergyParticle))
			{
				return Db.Get().Deaths.HitByHighEnergyParticle;
			}
			if (component.HasTag(GameTags.RadiationSicknessIncapacitation))
			{
				return Db.Get().Deaths.Radiation;
			}
			if (component.HasTag(GameTags.CaloriesDepleted))
			{
				return Db.Get().Deaths.Starvation;
			}
			if (component.HasTag(GameTags.HitPointsDepleted))
			{
				return Db.Get().Deaths.Slain;
			}
			return Db.Get().Deaths.Generic;
		}
	}
}

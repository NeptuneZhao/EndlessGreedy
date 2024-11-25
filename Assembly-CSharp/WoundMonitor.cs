﻿using System;

// Token: 0x020009AF RID: 2479
public class WoundMonitor : GameStateMachine<WoundMonitor, WoundMonitor.Instance>
{
	// Token: 0x0600480F RID: 18447 RVA: 0x0019CD94 File Offset: 0x0019AF94
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		this.root.ToggleAnims("anim_hits_kanim", 0f).EventHandler(GameHashes.HealthChanged, delegate(WoundMonitor.Instance smi, object data)
		{
			smi.OnHealthChanged(data);
		});
		this.healthy.EventTransition(GameHashes.HealthChanged, this.wounded, (WoundMonitor.Instance smi) => smi.health.State > Health.HealthState.Perfect);
		this.wounded.ToggleUrge(Db.Get().Urges.Heal).Enter(delegate(WoundMonitor.Instance smi)
		{
			switch (smi.health.State)
			{
			case Health.HealthState.Scuffed:
				smi.GoTo(this.wounded.light);
				return;
			case Health.HealthState.Injured:
				smi.GoTo(this.wounded.medium);
				return;
			case Health.HealthState.Critical:
				smi.GoTo(this.wounded.heavy);
				return;
			default:
				return;
			}
		}).EventHandler(GameHashes.HealthChanged, delegate(WoundMonitor.Instance smi)
		{
			smi.GoToProperHeathState();
		});
		this.wounded.medium.ToggleAnims("anim_loco_wounded_kanim", 1f);
		this.wounded.heavy.ToggleAnims("anim_loco_wounded_kanim", 3f).Update("LookForAvailableClinic", delegate(WoundMonitor.Instance smi, float dt)
		{
			smi.FindAvailableMedicalBed();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x04002F2E RID: 12078
	public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x04002F2F RID: 12079
	public WoundMonitor.Wounded wounded;

	// Token: 0x020019AA RID: 6570
	public class Wounded : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007A4C RID: 31308
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State light;

		// Token: 0x04007A4D RID: 31309
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State medium;

		// Token: 0x04007A4E RID: 31310
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State heavy;
	}

	// Token: 0x020019AB RID: 6571
	public new class Instance : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009DAD RID: 40365 RVA: 0x00375AFC File Offset: 0x00373CFC
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.health = master.GetComponent<Health>();
			this.worker = master.GetComponent<WorkerBase>();
		}

		// Token: 0x06009DAE RID: 40366 RVA: 0x00375B20 File Offset: 0x00373D20
		public void OnHealthChanged(object data)
		{
			float num = (float)data;
			if (this.health.hitPoints != 0f && num < 0f)
			{
				this.PlayHitAnimation();
			}
		}

		// Token: 0x06009DAF RID: 40367 RVA: 0x00375B54 File Offset: 0x00373D54
		private void PlayHitAnimation()
		{
			string text = null;
			KBatchedAnimController kbatchedAnimController = base.smi.Get<KBatchedAnimController>();
			if (kbatchedAnimController.CurrentAnim != null)
			{
				text = kbatchedAnimController.CurrentAnim.name;
			}
			KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
			if (text != null)
			{
				if (text.Contains("hit"))
				{
					return;
				}
				if (text.Contains("2_0"))
				{
					return;
				}
				if (text.Contains("2_1"))
				{
					return;
				}
				if (text.Contains("2_-1"))
				{
					return;
				}
				if (text.Contains("2_-2"))
				{
					return;
				}
				if (text.Contains("1_-1"))
				{
					return;
				}
				if (text.Contains("1_-2"))
				{
					return;
				}
				if (text.Contains("1_1"))
				{
					return;
				}
				if (text.Contains("1_2"))
				{
					return;
				}
				if (text.Contains("breathe_"))
				{
					return;
				}
				if (text.Contains("death_"))
				{
					return;
				}
				if (text.Contains("impact"))
				{
					return;
				}
			}
			string s = "hit";
			AttackChore.StatesInstance smi = base.gameObject.GetSMI<AttackChore.StatesInstance>();
			if (smi != null && smi.GetCurrentState() == smi.sm.attack)
			{
				s = smi.master.GetHitAnim();
			}
			if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				s = "hit_ladder";
			}
			else if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Pole)
			{
				s = "hit_pole";
			}
			kbatchedAnimController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			if (text != null)
			{
				kbatchedAnimController.Queue(text, playMode, 1f, 0f);
			}
		}

		// Token: 0x06009DB0 RID: 40368 RVA: 0x00375CD8 File Offset: 0x00373ED8
		public void PlayKnockedOverImpactAnimation()
		{
			string text = null;
			KBatchedAnimController kbatchedAnimController = base.smi.Get<KBatchedAnimController>();
			if (kbatchedAnimController.CurrentAnim != null)
			{
				text = kbatchedAnimController.CurrentAnim.name;
			}
			KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
			if (text != null)
			{
				if (text.Contains("impact"))
				{
					return;
				}
				if (text.Contains("2_0"))
				{
					return;
				}
				if (text.Contains("2_1"))
				{
					return;
				}
				if (text.Contains("2_-1"))
				{
					return;
				}
				if (text.Contains("2_-2"))
				{
					return;
				}
				if (text.Contains("1_-1"))
				{
					return;
				}
				if (text.Contains("1_-2"))
				{
					return;
				}
				if (text.Contains("1_1"))
				{
					return;
				}
				if (text.Contains("1_2"))
				{
					return;
				}
				if (text.Contains("breathe_"))
				{
					return;
				}
				if (text.Contains("death_"))
				{
					return;
				}
			}
			string s = "impact";
			kbatchedAnimController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			if (text != null)
			{
				kbatchedAnimController.Queue(text, playMode, 1f, 0f);
			}
		}

		// Token: 0x06009DB1 RID: 40369 RVA: 0x00375DE8 File Offset: 0x00373FE8
		public void GoToProperHeathState()
		{
			switch (base.smi.health.State)
			{
			case Health.HealthState.Perfect:
				base.smi.GoTo(base.sm.healthy);
				return;
			case Health.HealthState.Alright:
				break;
			case Health.HealthState.Scuffed:
				base.smi.GoTo(base.sm.wounded.light);
				break;
			case Health.HealthState.Injured:
				base.smi.GoTo(base.sm.wounded.medium);
				return;
			case Health.HealthState.Critical:
				base.smi.GoTo(base.sm.wounded.heavy);
				return;
			default:
				return;
			}
		}

		// Token: 0x06009DB2 RID: 40370 RVA: 0x00375E8B File Offset: 0x0037408B
		public bool ShouldExitInfirmary()
		{
			return this.health.State == Health.HealthState.Perfect;
		}

		// Token: 0x06009DB3 RID: 40371 RVA: 0x00375EA0 File Offset: 0x003740A0
		public void FindAvailableMedicalBed()
		{
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			Ownables soleOwner = base.gameObject.GetComponent<MinionIdentity>().GetSoleOwner();
			if (soleOwner.GetSlot(clinic).assignable == null)
			{
				soleOwner.AutoAssignSlot(clinic);
			}
		}

		// Token: 0x04007A4F RID: 31311
		public Health health;

		// Token: 0x04007A50 RID: 31312
		private WorkerBase worker;
	}
}

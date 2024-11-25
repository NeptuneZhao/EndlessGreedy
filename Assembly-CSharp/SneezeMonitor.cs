using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020009A0 RID: 2464
public class SneezeMonitor : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance>
{
	// Token: 0x060047C8 RID: 18376 RVA: 0x0019AF70 File Offset: 0x00199170
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.ParamTransition<bool>(this.isSneezy, this.sneezy, (SneezeMonitor.Instance smi, bool p) => p);
		this.sneezy.ParamTransition<bool>(this.isSneezy, this.idle, (SneezeMonitor.Instance smi, bool p) => !p).ToggleReactable((SneezeMonitor.Instance smi) => smi.GetReactable());
	}

	// Token: 0x04002EF1 RID: 12017
	public StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSneezy = new StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);

	// Token: 0x04002EF2 RID: 12018
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04002EF3 RID: 12019
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State taking_medicine;

	// Token: 0x04002EF4 RID: 12020
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State sneezy;

	// Token: 0x04002EF5 RID: 12021
	public const float SINGLE_SNEEZE_TIME_MINOR = 140f;

	// Token: 0x04002EF6 RID: 12022
	public const float SINGLE_SNEEZE_TIME_MAJOR = 70f;

	// Token: 0x04002EF7 RID: 12023
	public const float SNEEZE_TIME_VARIANCE = 0.3f;

	// Token: 0x04002EF8 RID: 12024
	public const float SHORT_SNEEZE_THRESHOLD = 5f;

	// Token: 0x02001982 RID: 6530
	public new class Instance : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009CE2 RID: 40162 RVA: 0x0037372C File Offset: 0x0037192C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sneezyness = Db.Get().Attributes.Sneezyness.Lookup(master.gameObject);
			this.OnSneezyChange();
			AttributeInstance attributeInstance = this.sneezyness;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(this.OnSneezyChange));
		}

		// Token: 0x06009CE3 RID: 40163 RVA: 0x0037378D File Offset: 0x0037198D
		public override void StopSM(string reason)
		{
			AttributeInstance attributeInstance = this.sneezyness;
			attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(this.OnSneezyChange));
			base.StopSM(reason);
		}

		// Token: 0x06009CE4 RID: 40164 RVA: 0x003737C0 File Offset: 0x003719C0
		public float NextSneezeInterval()
		{
			if (this.sneezyness.GetTotalValue() <= 0f)
			{
				return 70f;
			}
			float num = (this.IsMinorSneeze() ? 140f : 70f) / this.sneezyness.GetTotalValue();
			return UnityEngine.Random.Range(num * 0.7f, num * 1.3f);
		}

		// Token: 0x06009CE5 RID: 40165 RVA: 0x00373819 File Offset: 0x00371A19
		public bool IsMinorSneeze()
		{
			return this.sneezyness.GetTotalValue() <= 5f;
		}

		// Token: 0x06009CE6 RID: 40166 RVA: 0x00373830 File Offset: 0x00371A30
		private void OnSneezyChange()
		{
			base.smi.sm.isSneezy.Set(this.sneezyness.GetTotalValue() > 0f, base.smi, false);
		}

		// Token: 0x06009CE7 RID: 40167 RVA: 0x00373864 File Offset: 0x00371A64
		public Reactable GetReactable()
		{
			float localCooldown = this.NextSneezeInterval();
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Sneeze", Db.Get().ChoreTypes.Cough, 0f, localCooldown, float.PositiveInfinity, 0f);
			string s = "sneeze";
			string s2 = "sneeze_pst";
			Emote emote = Db.Get().Emotes.Minion.Sneeze;
			if (this.IsMinorSneeze())
			{
				s = "sneeze_short";
				s2 = "sneeze_short_pst";
				emote = Db.Get().Emotes.Minion.Sneeze_Short;
			}
			selfEmoteReactable.SetEmote(emote);
			return selfEmoteReactable.RegisterEmoteStepCallbacks(s, new Action<GameObject>(this.TriggerDisurbance), null).RegisterEmoteStepCallbacks(s2, null, new Action<GameObject>(this.ResetSneeze));
		}

		// Token: 0x06009CE8 RID: 40168 RVA: 0x00373933 File Offset: 0x00371B33
		private void TriggerDisurbance(GameObject go)
		{
			if (this.IsMinorSneeze())
			{
				AcousticDisturbance.Emit(go, 2);
				return;
			}
			AcousticDisturbance.Emit(go, 3);
		}

		// Token: 0x06009CE9 RID: 40169 RVA: 0x0037394C File Offset: 0x00371B4C
		private void ResetSneeze(GameObject go)
		{
			base.smi.GoTo(base.sm.idle);
		}

		// Token: 0x040079AB RID: 31147
		private AttributeInstance sneezyness;

		// Token: 0x040079AC RID: 31148
		private StatusItem statusItem;
	}
}

using System;
using Klei.AI;

// Token: 0x02000661 RID: 1633
public class BionicUpgrade_Effect : GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>
{
	// Token: 0x0600284C RID: 10316 RVA: 0x000E4767 File Offset: 0x000E2967
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
		this.root.Enter(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.DisableEffect));
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x000E47A1 File Offset: 0x000E29A1
	public static void EnableEffect(BionicUpgrade_Effect.Instance smi)
	{
		smi.ApplyEffect();
	}

	// Token: 0x0600284E RID: 10318 RVA: 0x000E47A9 File Offset: 0x000E29A9
	public static void DisableEffect(BionicUpgrade_Effect.Instance smi)
	{
		smi.RemoveEffect();
	}

	// Token: 0x0200143B RID: 5179
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400693A RID: 26938
		public string EFFECT_NAME;
	}

	// Token: 0x0200143C RID: 5180
	public new class Instance : GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.GameInstance
	{
		// Token: 0x060089D3 RID: 35283 RVA: 0x00331A23 File Offset: 0x0032FC23
		public Instance(IStateMachineTarget master, BionicUpgrade_Effect.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x060089D4 RID: 35284 RVA: 0x00331A3C File Offset: 0x0032FC3C
		public void ApplyEffect()
		{
			Effect newEffect = Db.Get().effects.Get(base.def.EFFECT_NAME);
			this.effects.Add(newEffect, false);
		}

		// Token: 0x060089D5 RID: 35285 RVA: 0x00331A74 File Offset: 0x0032FC74
		public void RemoveEffect()
		{
			Effect effect = Db.Get().effects.Get(base.def.EFFECT_NAME);
			this.effects.Remove(effect);
		}

		// Token: 0x0400693B RID: 26939
		private Effects effects;
	}
}

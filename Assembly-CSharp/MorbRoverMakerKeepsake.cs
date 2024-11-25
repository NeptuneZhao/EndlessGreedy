using System;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class MorbRoverMakerKeepsake : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>
{
	// Token: 0x06001034 RID: 4148 RVA: 0x0005BCA0 File Offset: 0x00059EA0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.silent;
		this.silent.PlayAnim("silent").Enter(new StateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State.Callback(MorbRoverMakerKeepsake.CalculateNextActivationTime)).Update(new Action<MorbRoverMakerKeepsake.Instance, float>(MorbRoverMakerKeepsake.TimerUpdate), UpdateRate.SIM_200ms, false);
		this.talking.PlayAnim("idle").OnAnimQueueComplete(this.silent);
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0005BD0D File Offset: 0x00059F0D
	public static void CalculateNextActivationTime(MorbRoverMakerKeepsake.Instance smi)
	{
		smi.CalculateNextActivationTime();
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0005BD15 File Offset: 0x00059F15
	public static void TimerUpdate(MorbRoverMakerKeepsake.Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() > smi.NextActivationTime)
		{
			smi.GoTo(smi.sm.talking);
		}
	}

	// Token: 0x040009DB RID: 2523
	public const string SILENT_ANIMATION_NAME = "silent";

	// Token: 0x040009DC RID: 2524
	public const string TALKING_ANIMATION_NAME = "idle";

	// Token: 0x040009DD RID: 2525
	public GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State silent;

	// Token: 0x040009DE RID: 2526
	public GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State talking;

	// Token: 0x0200112D RID: 4397
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005F74 RID: 24436
		public Vector2 OperationalRandomnessRange = new Vector2(120f, 600f);
	}

	// Token: 0x0200112E RID: 4398
	public new class Instance : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.GameInstance
	{
		// Token: 0x06007EC5 RID: 32453 RVA: 0x0030B191 File Offset: 0x00309391
		public Instance(IStateMachineTarget master, MorbRoverMakerKeepsake.Def def) : base(master, def)
		{
		}

		// Token: 0x06007EC6 RID: 32454 RVA: 0x0030B1A8 File Offset: 0x003093A8
		public void CalculateNextActivationTime()
		{
			float time = GameClock.Instance.GetTime();
			float minInclusive = time + base.def.OperationalRandomnessRange.x;
			float maxInclusive = time + base.def.OperationalRandomnessRange.y;
			this.NextActivationTime = UnityEngine.Random.Range(minInclusive, maxInclusive);
		}

		// Token: 0x04005F75 RID: 24437
		public float NextActivationTime = -1f;
	}
}

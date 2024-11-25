using System;
using UnityEngine;

// Token: 0x0200097E RID: 2430
public class EmoteMonitor : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance>
{
	// Token: 0x06004707 RID: 18183 RVA: 0x001963E0 File Offset: 0x001945E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.ScheduleGoTo((float)UnityEngine.Random.Range(30, 90), this.ready);
		this.ready.ToggleUrge(Db.Get().Urges.Emote).EventHandler(GameHashes.BeginChore, delegate(EmoteMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
	}

	// Token: 0x04002E4A RID: 11850
	public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002E4B RID: 11851
	public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State ready;

	// Token: 0x02001929 RID: 6441
	public new class Instance : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009B68 RID: 39784 RVA: 0x0036F795 File Offset: 0x0036D995
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009B69 RID: 39785 RVA: 0x0036F79E File Offset: 0x0036D99E
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.Emote))
			{
				this.GoTo(base.sm.satisfied);
			}
		}
	}
}

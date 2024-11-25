using System;

// Token: 0x020000BF RID: 191
public class BeeHiveMonitor : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>
{
	// Token: 0x0600036D RID: 877 RVA: 0x0001C994 File Offset: 0x0001AB94
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.Nighttime, (BeeHiveMonitor.Instance smi) => GameClock.Instance, this.night, (BeeHiveMonitor.Instance smi) => GameClock.Instance.IsNighttime());
		this.night.EventTransition(GameHashes.NewDay, (BeeHiveMonitor.Instance smi) => GameClock.Instance, this.idle, (BeeHiveMonitor.Instance smi) => !GameClock.Instance.IsNighttime()).ToggleBehaviour(GameTags.Creatures.WantsToMakeHome, new StateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.Transition.ConditionCallback(this.ShouldMakeHome), null);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0001CA6A File Offset: 0x0001AC6A
	public bool ShouldMakeHome(BeeHiveMonitor.Instance smi)
	{
		return !this.CanGoHome(smi);
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0001CA76 File Offset: 0x0001AC76
	public bool CanGoHome(BeeHiveMonitor.Instance smi)
	{
		return smi.gameObject.GetComponent<Bee>().FindHiveInRoom() != null;
	}

	// Token: 0x04000266 RID: 614
	public GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.State idle;

	// Token: 0x04000267 RID: 615
	public GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.State night;

	// Token: 0x02000FF8 RID: 4088
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FF9 RID: 4089
	public new class Instance : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.GameInstance
	{
		// Token: 0x06007AE9 RID: 31465 RVA: 0x00302D6F File Offset: 0x00300F6F
		public Instance(IStateMachineTarget master, BeeHiveMonitor.Def def) : base(master, def)
		{
		}
	}
}

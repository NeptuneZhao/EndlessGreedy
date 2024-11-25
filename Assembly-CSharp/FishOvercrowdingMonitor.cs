using System;

// Token: 0x0200080A RID: 2058
public class FishOvercrowdingMonitor : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>
{
	// Token: 0x060038DD RID: 14557 RVA: 0x0013666C File Offset: 0x0013486C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Register)).Exit(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Unregister));
		this.satisfied.DoNothing();
		this.overcrowded.DoNothing();
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x001366C2 File Offset: 0x001348C2
	private static void Register(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager.Instance.Add(smi);
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x001366D0 File Offset: 0x001348D0
	private static void Unregister(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager instance = FishOvercrowingManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Remove(smi);
	}

	// Token: 0x04002238 RID: 8760
	public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State satisfied;

	// Token: 0x04002239 RID: 8761
	public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State overcrowded;

	// Token: 0x02001705 RID: 5893
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001706 RID: 5894
	public new class Instance : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x06009463 RID: 37987 RVA: 0x0035C2F5 File Offset: 0x0035A4F5
		public Instance(IStateMachineTarget master, FishOvercrowdingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009464 RID: 37988 RVA: 0x0035C2FF File Offset: 0x0035A4FF
		public void SetOvercrowdingInfo(int cell_count, int fish_count)
		{
			this.cellCount = cell_count;
			this.fishCount = fish_count;
		}

		// Token: 0x04007183 RID: 29059
		public int cellCount;

		// Token: 0x04007184 RID: 29060
		public int fishCount;
	}
}

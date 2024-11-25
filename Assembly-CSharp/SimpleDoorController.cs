using System;

// Token: 0x02000AE4 RID: 2788
public class SimpleDoorController : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>
{
	// Token: 0x060052DE RID: 21214 RVA: 0x001DB4B0 File Offset: 0x001D96B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inactive;
		this.inactive.TagTransition(GameTags.RocketOnGround, this.active, false);
		this.active.DefaultState(this.active.closed).TagTransition(GameTags.RocketOnGround, this.inactive, true).Enter(delegate(SimpleDoorController.StatesInstance smi)
		{
			smi.Register();
		}).Exit(delegate(SimpleDoorController.StatesInstance smi)
		{
			smi.Unregister();
		});
		this.active.closed.PlayAnim((SimpleDoorController.StatesInstance smi) => smi.GetDefaultAnim(), KAnim.PlayMode.Loop).ParamTransition<int>(this.numOpens, this.active.opening, (SimpleDoorController.StatesInstance smi, int p) => p > 0);
		this.active.opening.PlayAnim("enter_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.open);
		this.active.open.PlayAnim("enter_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.numOpens, this.active.closedelay, (SimpleDoorController.StatesInstance smi, int p) => p == 0);
		this.active.closedelay.ParamTransition<int>(this.numOpens, this.active.open, (SimpleDoorController.StatesInstance smi, int p) => p > 0).ScheduleGoTo(0.5f, this.active.closing);
		this.active.closing.PlayAnim("enter_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.closed);
	}

	// Token: 0x040036BA RID: 14010
	public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State inactive;

	// Token: 0x040036BB RID: 14011
	public SimpleDoorController.ActiveStates active;

	// Token: 0x040036BC RID: 14012
	public StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.IntParameter numOpens;

	// Token: 0x02001B34 RID: 6964
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B35 RID: 6965
	public class ActiveStates : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State
	{
		// Token: 0x04007F11 RID: 32529
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closed;

		// Token: 0x04007F12 RID: 32530
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State opening;

		// Token: 0x04007F13 RID: 32531
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State open;

		// Token: 0x04007F14 RID: 32532
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closedelay;

		// Token: 0x04007F15 RID: 32533
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closing;
	}

	// Token: 0x02001B36 RID: 6966
	public class StatesInstance : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.GameInstance, INavDoor
	{
		// Token: 0x0600A2D9 RID: 41689 RVA: 0x003889C5 File Offset: 0x00386BC5
		public StatesInstance(IStateMachineTarget master, SimpleDoorController.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A2DA RID: 41690 RVA: 0x003889D0 File Offset: 0x00386BD0
		public string GetDefaultAnim()
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				return component.initialAnim;
			}
			return "idle_loop";
		}

		// Token: 0x0600A2DB RID: 41691 RVA: 0x00388A00 File Offset: 0x00386C00
		public void Register()
		{
			int i = Grid.PosToCell(base.gameObject.transform.GetPosition());
			Grid.HasDoor[i] = true;
		}

		// Token: 0x0600A2DC RID: 41692 RVA: 0x00388A30 File Offset: 0x00386C30
		public void Unregister()
		{
			int i = Grid.PosToCell(base.gameObject.transform.GetPosition());
			Grid.HasDoor[i] = false;
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x0600A2DD RID: 41693 RVA: 0x00388A5F File Offset: 0x00386C5F
		public bool isSpawned
		{
			get
			{
				return base.master.gameObject.GetComponent<KMonoBehaviour>().isSpawned;
			}
		}

		// Token: 0x0600A2DE RID: 41694 RVA: 0x00388A76 File Offset: 0x00386C76
		public void Close()
		{
			base.sm.numOpens.Delta(-1, base.smi);
		}

		// Token: 0x0600A2DF RID: 41695 RVA: 0x00388A90 File Offset: 0x00386C90
		public bool IsOpen()
		{
			return base.IsInsideState(base.sm.active.open) || base.IsInsideState(base.sm.active.closedelay);
		}

		// Token: 0x0600A2E0 RID: 41696 RVA: 0x00388AC2 File Offset: 0x00386CC2
		public void Open()
		{
			base.sm.numOpens.Delta(1, base.smi);
		}
	}
}

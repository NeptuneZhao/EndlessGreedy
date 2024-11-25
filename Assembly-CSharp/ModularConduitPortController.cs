using System;

// Token: 0x02000736 RID: 1846
public class ModularConduitPortController : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>
{
	// Token: 0x0600310A RID: 12554 RVA: 0x0010E8FC File Offset: 0x0010CAFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		ModularConduitPortController.InitializeStatusItems();
		this.off.PlayAnim("off", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on, (ModularConduitPortController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (ModularConduitPortController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.on.idle.PlayAnim("idle").ParamTransition<bool>(this.hasRocket, this.on.finished, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.idleStatusItem, null);
		this.on.finished.PlayAnim("finished", KAnim.PlayMode.Loop).ParamTransition<bool>(this.hasRocket, this.on.idle, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.isUnloading, this.on.unloading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ParamTransition<bool>(this.isLoading, this.on.loading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.loadedStatusItem, null);
		this.on.unloading.Enter("SetActive(true)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("unloading_pre").QueueAnim("unloading_loop", true, null).ParamTransition<bool>(this.isUnloading, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.hasRocket, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.unloadingStatusItem, null);
		this.on.unloading_pst.PlayAnim("unloading_pst").OnAnimQueueComplete(this.on.finished);
		this.on.loading.Enter("SetActive(true)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("loading_pre").QueueAnim("loading_loop", true, null).ParamTransition<bool>(this.isLoading, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.hasRocket, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.loadingStatusItem, null);
		this.on.loading_pst.PlayAnim("loading_pst").OnAnimQueueComplete(this.on.finished);
	}

	// Token: 0x0600310B RID: 12555 RVA: 0x0010EC04 File Offset: 0x0010CE04
	private static void InitializeStatusItems()
	{
		if (ModularConduitPortController.idleStatusItem == null)
		{
			ModularConduitPortController.idleStatusItem = new StatusItem("ROCKET_PORT_IDLE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.unloadingStatusItem = new StatusItem("ROCKET_PORT_UNLOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.loadingStatusItem = new StatusItem("ROCKET_PORT_LOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.loadedStatusItem = new StatusItem("ROCKET_PORT_LOADED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}
	}

	// Token: 0x04001CC3 RID: 7363
	private GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State off;

	// Token: 0x04001CC4 RID: 7364
	private ModularConduitPortController.OnStates on;

	// Token: 0x04001CC5 RID: 7365
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isUnloading;

	// Token: 0x04001CC6 RID: 7366
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isLoading;

	// Token: 0x04001CC7 RID: 7367
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter hasRocket;

	// Token: 0x04001CC8 RID: 7368
	private static StatusItem idleStatusItem;

	// Token: 0x04001CC9 RID: 7369
	private static StatusItem unloadingStatusItem;

	// Token: 0x04001CCA RID: 7370
	private static StatusItem loadingStatusItem;

	// Token: 0x04001CCB RID: 7371
	private static StatusItem loadedStatusItem;

	// Token: 0x02001595 RID: 5525
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006D53 RID: 27987
		public ModularConduitPortController.Mode mode;
	}

	// Token: 0x02001596 RID: 5526
	public enum Mode
	{
		// Token: 0x04006D55 RID: 27989
		Unload,
		// Token: 0x04006D56 RID: 27990
		Both,
		// Token: 0x04006D57 RID: 27991
		Load
	}

	// Token: 0x02001597 RID: 5527
	private class OnStates : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State
	{
		// Token: 0x04006D58 RID: 27992
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State idle;

		// Token: 0x04006D59 RID: 27993
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading;

		// Token: 0x04006D5A RID: 27994
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading_pst;

		// Token: 0x04006D5B RID: 27995
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading;

		// Token: 0x04006D5C RID: 27996
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading_pst;

		// Token: 0x04006D5D RID: 27997
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State finished;
	}

	// Token: 0x02001598 RID: 5528
	public new class Instance : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.GameInstance
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06008F2D RID: 36653 RVA: 0x00346CE3 File Offset: 0x00344EE3
		public ModularConduitPortController.Mode SelectedMode
		{
			get
			{
				return base.def.mode;
			}
		}

		// Token: 0x06008F2E RID: 36654 RVA: 0x00346CF0 File Offset: 0x00344EF0
		public Instance(IStateMachineTarget master, ModularConduitPortController.Def def) : base(master, def)
		{
		}

		// Token: 0x06008F2F RID: 36655 RVA: 0x00346CFA File Offset: 0x00344EFA
		public ConduitType GetConduitType()
		{
			return base.GetComponent<IConduitConsumer>().ConduitType;
		}

		// Token: 0x06008F30 RID: 36656 RVA: 0x00346D07 File Offset: 0x00344F07
		public void SetUnloading(bool isUnloading)
		{
			base.sm.isUnloading.Set(isUnloading, this, false);
		}

		// Token: 0x06008F31 RID: 36657 RVA: 0x00346D1D File Offset: 0x00344F1D
		public void SetLoading(bool isLoading)
		{
			base.sm.isLoading.Set(isLoading, this, false);
		}

		// Token: 0x06008F32 RID: 36658 RVA: 0x00346D33 File Offset: 0x00344F33
		public void SetRocket(bool hasRocket)
		{
			base.sm.hasRocket.Set(hasRocket, this, false);
		}

		// Token: 0x06008F33 RID: 36659 RVA: 0x00346D49 File Offset: 0x00344F49
		public bool IsLoading()
		{
			return base.sm.isLoading.Get(this);
		}

		// Token: 0x04006D5E RID: 27998
		[MyCmpGet]
		public Operational operational;
	}
}

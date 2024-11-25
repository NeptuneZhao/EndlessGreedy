using System;
using UnityEngine;

// Token: 0x02000B54 RID: 2900
public class WindTunnelWorkerStateMachine : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase>
{
	// Token: 0x060056B3 RID: 22195 RVA: 0x001EFC38 File Offset: 0x001EDE38
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.OverrideAnim);
		this.pre_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreFrontAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreBackAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.LoopAnim, KAnim.PlayMode.Loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (WindTunnelWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.pst_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstBackAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstFrontAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x040038BD RID: 14525
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_front;

	// Token: 0x040038BE RID: 14526
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_back;

	// Token: 0x040038BF RID: 14527
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop;

	// Token: 0x040038C0 RID: 14528
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_back;

	// Token: 0x040038C1 RID: 14529
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_front;

	// Token: 0x040038C2 RID: 14530
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State complete;

	// Token: 0x040038C3 RID: 14531
	public StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.TargetParameter worker;

	// Token: 0x02001BA8 RID: 7080
	public class StatesInstance : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.GameInstance
	{
		// Token: 0x0600A406 RID: 41990 RVA: 0x0038B3C3 File Offset: 0x003895C3
		public StatesInstance(WorkerBase master, VerticalWindTunnelWorkable workable) : base(master)
		{
			this.workable = workable;
			base.sm.worker.Set(master, base.smi);
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x0600A407 RID: 41991 RVA: 0x0038B3EA File Offset: 0x003895EA
		public HashedString OverrideAnim
		{
			get
			{
				return this.workable.overrideAnim;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x0600A408 RID: 41992 RVA: 0x0038B3F7 File Offset: 0x003895F7
		public string PreFrontAnim
		{
			get
			{
				return this.workable.preAnims[0];
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600A409 RID: 41993 RVA: 0x0038B406 File Offset: 0x00389606
		public string PreBackAnim
		{
			get
			{
				return this.workable.preAnims[1];
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x0600A40A RID: 41994 RVA: 0x0038B415 File Offset: 0x00389615
		public string LoopAnim
		{
			get
			{
				return this.workable.loopAnim;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x0600A40B RID: 41995 RVA: 0x0038B422 File Offset: 0x00389622
		public string PstBackAnim
		{
			get
			{
				return this.workable.pstAnims[0];
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x0600A40C RID: 41996 RVA: 0x0038B431 File Offset: 0x00389631
		public string PstFrontAnim
		{
			get
			{
				return this.workable.pstAnims[1];
			}
		}

		// Token: 0x04008051 RID: 32849
		private VerticalWindTunnelWorkable workable;
	}
}

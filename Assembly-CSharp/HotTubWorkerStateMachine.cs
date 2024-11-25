using System;
using UnityEngine;

// Token: 0x020008F1 RID: 2289
public class HotTubWorkerStateMachine : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase>
{
	// Token: 0x060041DC RID: 16860 RVA: 0x0017577C File Offset: 0x0017397C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims("anim_interacts_hottub_kanim", 0f);
		this.pre_front.PlayAnim("working_pre_front").OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim("working_pre_back").Enter(delegate(HotTubWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((HotTubWorkerStateMachine.StatesInstance smi) => HotTubWorkerStateMachine.workAnimLoopVariants[UnityEngine.Random.Range(0, HotTubWorkerStateMachine.workAnimLoopVariants.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.loop_reenter).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (HotTubWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.loop_reenter.GoTo(this.loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (HotTubWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.pst_back.PlayAnim("working_pst_back").OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim("working_pst_front").Enter(delegate(HotTubWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x04002BA8 RID: 11176
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_front;

	// Token: 0x04002BA9 RID: 11177
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_back;

	// Token: 0x04002BAA RID: 11178
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop;

	// Token: 0x04002BAB RID: 11179
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop_reenter;

	// Token: 0x04002BAC RID: 11180
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_back;

	// Token: 0x04002BAD RID: 11181
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_front;

	// Token: 0x04002BAE RID: 11182
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State complete;

	// Token: 0x04002BAF RID: 11183
	public StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.TargetParameter worker;

	// Token: 0x04002BB0 RID: 11184
	public static string[] workAnimLoopVariants = new string[]
	{
		"working_loop1",
		"working_loop2",
		"working_loop3"
	};

	// Token: 0x02001860 RID: 6240
	public class StatesInstance : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.GameInstance
	{
		// Token: 0x0600981B RID: 38939 RVA: 0x003675D3 File Offset: 0x003657D3
		public StatesInstance(WorkerBase master) : base(master)
		{
			base.sm.worker.Set(master, base.smi);
		}
	}
}

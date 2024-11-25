using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200076A RID: 1898
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitDropper : StateMachineComponent<SolidConduitDropper.SMInstance>
{
	// Token: 0x06003314 RID: 13076 RVA: 0x00118965 File Offset: 0x00116B65
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x0011896D File Offset: 0x00116B6D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x00118980 File Offset: 0x00116B80
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x00118988 File Offset: 0x00116B88
	private void Update()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
		base.smi.sm.isclosed.Set(!this.operational.IsOperational, base.smi, false);
		this.storage.DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x04001E28 RID: 7720
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001E29 RID: 7721
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x04001E2A RID: 7722
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x020015F7 RID: 5623
	public class SMInstance : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.GameInstance
	{
		// Token: 0x06009089 RID: 37001 RVA: 0x0034BCC2 File Offset: 0x00349EC2
		public SMInstance(SolidConduitDropper master) : base(master)
		{
		}
	}

	// Token: 0x020015F8 RID: 5624
	public class States : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper>
	{
		// Token: 0x0600908A RID: 37002 RVA: 0x0034BCCC File Offset: 0x00349ECC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("Update", delegate(SolidConduitDropper.SMInstance smi, float dt)
			{
				smi.master.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>(this.isclosed, this.closed, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
			this.closed.PlayAnim("closed").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>(this.isclosed, this.idle, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
		}

		// Token: 0x04006E48 RID: 28232
		public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter consuming;

		// Token: 0x04006E49 RID: 28233
		public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter isclosed;

		// Token: 0x04006E4A RID: 28234
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State idle;

		// Token: 0x04006E4B RID: 28235
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State working;

		// Token: 0x04006E4C RID: 28236
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State post;

		// Token: 0x04006E4D RID: 28237
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State closed;
	}
}

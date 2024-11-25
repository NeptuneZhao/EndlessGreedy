using System;

// Token: 0x0200055B RID: 1371
public class Dreamer : GameStateMachine<Dreamer, Dreamer.Instance>
{
	// Token: 0x06001FA4 RID: 8100 RVA: 0x000B1DD0 File Offset: 0x000AFFD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notDreaming;
		this.notDreaming.OnSignal(this.startDreaming, this.dreaming, (Dreamer.Instance smi) => smi.currentDream != null);
		this.dreaming.Enter(new StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State.Callback(Dreamer.PrepareDream)).OnSignal(this.stopDreaming, this.notDreaming).Update(new Action<Dreamer.Instance, float>(this.UpdateDream), UpdateRate.SIM_EVERY_TICK, false).Exit(new StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State.Callback(this.RemoveDream));
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x000B1E69 File Offset: 0x000B0069
	private void RemoveDream(Dreamer.Instance smi)
	{
		smi.SetDream(null);
		NameDisplayScreen.Instance.StopDreaming(smi.gameObject);
	}

	// Token: 0x06001FA6 RID: 8102 RVA: 0x000B1E82 File Offset: 0x000B0082
	private void UpdateDream(Dreamer.Instance smi, float dt)
	{
		NameDisplayScreen.Instance.DreamTick(smi.gameObject, dt);
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x000B1E95 File Offset: 0x000B0095
	private static void PrepareDream(Dreamer.Instance smi)
	{
		NameDisplayScreen.Instance.SetDream(smi.gameObject, smi.currentDream);
	}

	// Token: 0x040011D1 RID: 4561
	public StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.Signal stopDreaming;

	// Token: 0x040011D2 RID: 4562
	public StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.Signal startDreaming;

	// Token: 0x040011D3 RID: 4563
	public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State notDreaming;

	// Token: 0x040011D4 RID: 4564
	public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State dreaming;

	// Token: 0x02001354 RID: 4948
	public class DreamingState : GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400663D RID: 26173
		public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State hidden;

		// Token: 0x0400663E RID: 26174
		public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State visible;
	}

	// Token: 0x02001355 RID: 4949
	public new class Instance : GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060086A9 RID: 34473 RVA: 0x00329873 File Offset: 0x00327A73
		public Instance(IStateMachineTarget master) : base(master)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x060086AA RID: 34474 RVA: 0x0032988E File Offset: 0x00327A8E
		public void SetDream(Dream dream)
		{
			this.currentDream = dream;
		}

		// Token: 0x060086AB RID: 34475 RVA: 0x00329897 File Offset: 0x00327A97
		public void StartDreaming()
		{
			base.sm.startDreaming.Trigger(base.smi);
		}

		// Token: 0x060086AC RID: 34476 RVA: 0x003298AF File Offset: 0x00327AAF
		public void StopDreaming()
		{
			this.SetDream(null);
			base.sm.stopDreaming.Trigger(base.smi);
		}

		// Token: 0x0400663F RID: 26175
		public Dream currentDream;
	}
}

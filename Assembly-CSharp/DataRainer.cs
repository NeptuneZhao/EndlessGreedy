using System;
using TUNING;

// Token: 0x0200047B RID: 1147
public class DataRainer : GameStateMachine<DataRainer, DataRainer.Instance>
{
	// Token: 0x060018C9 RID: 6345 RVA: 0x0008402C File Offset: 0x0008222C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>(this.databanksCreated, this.overjoyed.exitEarly, (DataRainer.Instance smi, int p) => p >= TRAITS.JOY_REACTIONS.DATA_RAINER.NUM_MICROCHIPS).Exit(delegate(DataRainer.Instance smi)
		{
			this.databanksCreated.Set(0, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(DataRainer.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.raining);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerPlanning, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.raining, (DataRainer.Instance smi) => smi.IsRecTime());
		this.overjoyed.raining.ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerRaining, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.idle, (DataRainer.Instance smi) => !smi.IsRecTime()).ToggleChore((DataRainer.Instance smi) => new DataRainerChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(DataRainer.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04000DC0 RID: 3520
	public StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.IntParameter databanksCreated;

	// Token: 0x04000DC1 RID: 3521
	public static float databankSpawnInterval = 1.8f;

	// Token: 0x04000DC2 RID: 3522
	public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000DC3 RID: 3523
	public DataRainer.OverjoyedStates overjoyed;

	// Token: 0x02001248 RID: 4680
	public class OverjoyedStates : GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040062DA RID: 25306
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040062DB RID: 25307
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State raining;

		// Token: 0x040062DC RID: 25308
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x02001249 RID: 4681
	public new class Instance : GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060082AE RID: 33454 RVA: 0x0031D3E8 File Offset: 0x0031B5E8
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060082AF RID: 33455 RVA: 0x0031D3F1 File Offset: 0x0031B5F1
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060082B0 RID: 33456 RVA: 0x0031D414 File Offset: 0x0031B614
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}
	}
}

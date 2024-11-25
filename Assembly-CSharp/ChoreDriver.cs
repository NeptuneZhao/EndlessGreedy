using System;
using STRINGS;

// Token: 0x02000468 RID: 1128
public class ChoreDriver : StateMachineComponent<ChoreDriver.StatesInstance>
{
	// Token: 0x0600183B RID: 6203 RVA: 0x00081905 File Offset: 0x0007FB05
	public Chore GetCurrentChore()
	{
		return base.smi.GetCurrentChore();
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x00081912 File Offset: 0x0007FB12
	public bool HasChore()
	{
		return base.smi.GetCurrentChore() != null;
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x00081922 File Offset: 0x0007FB22
	public void StopChore()
	{
		base.smi.sm.stop.Trigger(base.smi);
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x00081940 File Offset: 0x0007FB40
	public void SetChore(Chore.Precondition.Context context)
	{
		Chore currentChore = base.smi.GetCurrentChore();
		if (currentChore != context.chore)
		{
			this.StopChore();
			if (context.chore.IsValid())
			{
				context.chore.PrepareChore(ref context);
				this.context = context;
				base.smi.sm.nextChore.Set(context.chore, base.smi, false);
				return;
			}
			string text = "Null";
			string text2 = "Null";
			if (currentChore != null)
			{
				text = currentChore.GetType().Name;
			}
			if (context.chore != null)
			{
				text2 = context.chore.GetType().Name;
			}
			Debug.LogWarning(string.Concat(new string[]
			{
				"Stopping chore ",
				text,
				" to start ",
				text2,
				" but stopping the first chore cancelled the second one."
			}));
		}
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x00081A14 File Offset: 0x0007FC14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04000D7B RID: 3451
	[MyCmpAdd]
	private User user;

	// Token: 0x04000D7C RID: 3452
	private Chore.Precondition.Context context;

	// Token: 0x0200121E RID: 4638
	public class StatesInstance : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.GameInstance
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x0600822F RID: 33327 RVA: 0x0031BF7C File Offset: 0x0031A17C
		// (set) Token: 0x06008230 RID: 33328 RVA: 0x0031BF84 File Offset: 0x0031A184
		public string masterProperName { get; private set; }

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06008231 RID: 33329 RVA: 0x0031BF8D File Offset: 0x0031A18D
		// (set) Token: 0x06008232 RID: 33330 RVA: 0x0031BF95 File Offset: 0x0031A195
		public KPrefabID masterPrefabId { get; private set; }

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06008233 RID: 33331 RVA: 0x0031BF9E File Offset: 0x0031A19E
		// (set) Token: 0x06008234 RID: 33332 RVA: 0x0031BFA6 File Offset: 0x0031A1A6
		public Navigator navigator { get; private set; }

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06008235 RID: 33333 RVA: 0x0031BFAF File Offset: 0x0031A1AF
		// (set) Token: 0x06008236 RID: 33334 RVA: 0x0031BFB7 File Offset: 0x0031A1B7
		public WorkerBase worker { get; private set; }

		// Token: 0x06008237 RID: 33335 RVA: 0x0031BFC0 File Offset: 0x0031A1C0
		public StatesInstance(ChoreDriver master) : base(master)
		{
			this.masterProperName = base.master.GetProperName();
			this.masterPrefabId = base.master.GetComponent<KPrefabID>();
			this.navigator = base.master.GetComponent<Navigator>();
			this.worker = base.master.GetComponent<WorkerBase>();
			this.choreConsumer = base.GetComponent<ChoreConsumer>();
			ChoreConsumer choreConsumer = this.choreConsumer;
			choreConsumer.choreRulesChanged = (System.Action)Delegate.Combine(choreConsumer.choreRulesChanged, new System.Action(this.OnChoreRulesChanged));
		}

		// Token: 0x06008238 RID: 33336 RVA: 0x0031C04C File Offset: 0x0031A24C
		public void BeginChore()
		{
			Chore nextChore = this.GetNextChore();
			Chore chore = base.smi.sm.currentChore.Set(nextChore, base.smi, false);
			if (chore != null && chore.IsPreemptable && chore.driver != null)
			{
				chore.Fail("Preemption!");
			}
			base.smi.sm.nextChore.Set(null, base.smi, false);
			Chore chore2 = chore;
			chore2.onExit = (Action<Chore>)Delegate.Combine(chore2.onExit, new Action<Chore>(this.OnChoreExit));
			chore.Begin(base.master.context);
			base.Trigger(-1988963660, chore);
		}

		// Token: 0x06008239 RID: 33337 RVA: 0x0031C100 File Offset: 0x0031A300
		public void EndChore(string reason)
		{
			if (this.GetCurrentChore() != null)
			{
				Chore currentChore = this.GetCurrentChore();
				base.smi.sm.currentChore.Set(null, base.smi, false);
				Chore chore = currentChore;
				chore.onExit = (Action<Chore>)Delegate.Remove(chore.onExit, new Action<Chore>(this.OnChoreExit));
				currentChore.Fail(reason);
				base.Trigger(1745615042, currentChore);
			}
			if (base.smi.choreConsumer.prioritizeBrainIfNoChore)
			{
				Game.BrainScheduler.PrioritizeBrain(this.brain);
			}
		}

		// Token: 0x0600823A RID: 33338 RVA: 0x0031C191 File Offset: 0x0031A391
		private void OnChoreExit(Chore chore)
		{
			base.smi.sm.stop.Trigger(base.smi);
		}

		// Token: 0x0600823B RID: 33339 RVA: 0x0031C1AE File Offset: 0x0031A3AE
		public Chore GetNextChore()
		{
			return base.smi.sm.nextChore.Get(base.smi);
		}

		// Token: 0x0600823C RID: 33340 RVA: 0x0031C1CB File Offset: 0x0031A3CB
		public Chore GetCurrentChore()
		{
			return base.smi.sm.currentChore.Get(base.smi);
		}

		// Token: 0x0600823D RID: 33341 RVA: 0x0031C1E8 File Offset: 0x0031A3E8
		private void OnChoreRulesChanged()
		{
			Chore currentChore = this.GetCurrentChore();
			if (currentChore != null && !this.choreConsumer.IsPermittedOrEnabled(currentChore.choreType, currentChore))
			{
				this.EndChore("Permissions changed");
			}
		}

		// Token: 0x0400626F RID: 25199
		private ChoreConsumer choreConsumer;

		// Token: 0x04006270 RID: 25200
		[MyCmpGet]
		private Brain brain;
	}

	// Token: 0x0200121F RID: 4639
	public class States : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver>
	{
		// Token: 0x0600823E RID: 33342 RVA: 0x0031C220 File Offset: 0x0031A420
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.nochore;
			this.saveHistory = true;
			this.nochore.Update(delegate(ChoreDriver.StatesInstance smi, float dt)
			{
				if (smi.masterPrefabId.HasTag(GameTags.BaseMinion) && !smi.masterPrefabId.HasTag(GameTags.Dead))
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.WorkTime, dt, string.Format(UI.ENDOFDAYREPORT.NOTES.TIME_SPENT, DUPLICANTS.CHORES.THINKING.NAME), smi.master.GetProperName());
				}
			}, UpdateRate.SIM_200ms, false).ParamTransition<Chore>(this.nextChore, this.haschore, (ChoreDriver.StatesInstance smi, Chore next_chore) => next_chore != null);
			this.haschore.Enter("BeginChore", delegate(ChoreDriver.StatesInstance smi)
			{
				smi.BeginChore();
			}).Update(delegate(ChoreDriver.StatesInstance smi, float dt)
			{
				if (smi.masterPrefabId.HasTag(GameTags.BaseMinion) && !smi.masterPrefabId.HasTag(GameTags.Dead))
				{
					Chore chore = this.currentChore.Get(smi);
					if (chore == null)
					{
						return;
					}
					if (smi.navigator.IsMoving())
					{
						ReportManager.Instance.ReportValue(ReportManager.ReportType.TravelTime, dt, GameUtil.GetChoreName(chore, null), smi.master.GetProperName());
						return;
					}
					ReportManager.ReportType reportType = chore.GetReportType();
					Workable workable = smi.worker.GetWorkable();
					if (workable != null)
					{
						ReportManager.ReportType reportType2 = workable.GetReportType();
						if (reportType != reportType2)
						{
							reportType = reportType2;
						}
					}
					ReportManager.Instance.ReportValue(reportType, dt, string.Format(UI.ENDOFDAYREPORT.NOTES.WORK_TIME, GameUtil.GetChoreName(chore, null)), smi.master.GetProperName());
				}
			}, UpdateRate.SIM_200ms, false).Exit("EndChore", delegate(ChoreDriver.StatesInstance smi)
			{
				smi.EndChore("ChoreDriver.SignalStop");
			}).OnSignal(this.stop, this.nochore);
		}

		// Token: 0x04006271 RID: 25201
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> currentChore;

		// Token: 0x04006272 RID: 25202
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> nextChore;

		// Token: 0x04006273 RID: 25203
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Signal stop;

		// Token: 0x04006274 RID: 25204
		public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State nochore;

		// Token: 0x04006275 RID: 25205
		public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State haschore;
	}
}

using System;
using System.Collections.Generic;

// Token: 0x02000463 RID: 1123
public abstract class StandardChoreBase : Chore
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060017BF RID: 6079 RVA: 0x0007FE03 File Offset: 0x0007E003
	// (set) Token: 0x060017C0 RID: 6080 RVA: 0x0007FE0B File Offset: 0x0007E00B
	public override int id { get; protected set; }

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060017C1 RID: 6081 RVA: 0x0007FE14 File Offset: 0x0007E014
	// (set) Token: 0x060017C2 RID: 6082 RVA: 0x0007FE1C File Offset: 0x0007E01C
	public override int priorityMod { get; protected set; }

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060017C3 RID: 6083 RVA: 0x0007FE25 File Offset: 0x0007E025
	// (set) Token: 0x060017C4 RID: 6084 RVA: 0x0007FE2D File Offset: 0x0007E02D
	public override ChoreType choreType { get; protected set; }

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060017C5 RID: 6085 RVA: 0x0007FE36 File Offset: 0x0007E036
	// (set) Token: 0x060017C6 RID: 6086 RVA: 0x0007FE3E File Offset: 0x0007E03E
	public override ChoreDriver driver { get; protected set; }

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060017C7 RID: 6087 RVA: 0x0007FE47 File Offset: 0x0007E047
	// (set) Token: 0x060017C8 RID: 6088 RVA: 0x0007FE4F File Offset: 0x0007E04F
	public override ChoreDriver lastDriver { get; protected set; }

	// Token: 0x060017C9 RID: 6089 RVA: 0x0007FE58 File Offset: 0x0007E058
	public override bool SatisfiesUrge(Urge urge)
	{
		return urge == this.choreType.urge;
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x0007FE68 File Offset: 0x0007E068
	public override bool IsValid()
	{
		return this.provider != null && this.gameObject.GetMyWorldId() != -1;
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060017CB RID: 6091 RVA: 0x0007FE8B File Offset: 0x0007E08B
	// (set) Token: 0x060017CC RID: 6092 RVA: 0x0007FE93 File Offset: 0x0007E093
	public override IStateMachineTarget target { get; protected set; }

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060017CD RID: 6093 RVA: 0x0007FE9C File Offset: 0x0007E09C
	// (set) Token: 0x060017CE RID: 6094 RVA: 0x0007FEA4 File Offset: 0x0007E0A4
	public override bool isComplete { get; protected set; }

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060017CF RID: 6095 RVA: 0x0007FEAD File Offset: 0x0007E0AD
	// (set) Token: 0x060017D0 RID: 6096 RVA: 0x0007FEB5 File Offset: 0x0007E0B5
	public override bool IsPreemptable { get; protected set; }

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0007FEBE File Offset: 0x0007E0BE
	// (set) Token: 0x060017D2 RID: 6098 RVA: 0x0007FEC6 File Offset: 0x0007E0C6
	public override ChoreConsumer overrideTarget { get; protected set; }

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060017D3 RID: 6099 RVA: 0x0007FECF File Offset: 0x0007E0CF
	// (set) Token: 0x060017D4 RID: 6100 RVA: 0x0007FED7 File Offset: 0x0007E0D7
	public override Prioritizable prioritizable { get; protected set; }

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060017D5 RID: 6101 RVA: 0x0007FEE0 File Offset: 0x0007E0E0
	// (set) Token: 0x060017D6 RID: 6102 RVA: 0x0007FEE8 File Offset: 0x0007E0E8
	public override ChoreProvider provider { get; set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060017D7 RID: 6103 RVA: 0x0007FEF1 File Offset: 0x0007E0F1
	// (set) Token: 0x060017D8 RID: 6104 RVA: 0x0007FEF9 File Offset: 0x0007E0F9
	public override bool runUntilComplete { get; set; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060017D9 RID: 6105 RVA: 0x0007FF02 File Offset: 0x0007E102
	// (set) Token: 0x060017DA RID: 6106 RVA: 0x0007FF0A File Offset: 0x0007E10A
	public override bool isExpanded { get; protected set; }

	// Token: 0x060017DB RID: 6107 RVA: 0x0007FF13 File Offset: 0x0007E113
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return this.IsPreemptable;
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x0007FF1B File Offset: 0x0007E11B
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x0007FF1D File Offset: 0x0007E11D
	public override string GetReportName(string context = null)
	{
		if (context == null || this.choreType.reportName == null)
		{
			return this.choreType.Name;
		}
		return string.Format(this.choreType.reportName, context);
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x0007FF4C File Offset: 0x0007E14C
	public override void Cancel(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x0007FFC4 File Offset: 0x0007E1C4
	public override void Cleanup()
	{
		this.ClearPrioritizable();
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x0007FFCC File Offset: 0x0007E1CC
	public override ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x0007FFD4 File Offset: 0x0007E1D4
	public override void AddPrecondition(Chore.Precondition precondition, object data = null)
	{
		this.arePreconditionsDirty = true;
		this.preconditions.Add(new Chore.PreconditionInstance
		{
			condition = precondition,
			data = data
		});
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x0008000C File Offset: 0x0007E20C
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		Chore.Precondition.Context item = new Chore.Precondition.Context(this, consumer_state, is_attempting_override, null);
		item.RunPreconditions();
		if (!item.IsComplete())
		{
			incomplete_contexts.Add(item);
			return;
		}
		if (item.IsSuccess())
		{
			succeeded_contexts.Add(item);
			return;
		}
		failed_contexts.Add(item);
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x00080056 File Offset: 0x0007E256
	public override void Fail(string reason)
	{
		if (this.provider == null)
		{
			return;
		}
		if (this.driver == null)
		{
			return;
		}
		if (!this.runUntilComplete)
		{
			this.Cancel(reason);
			return;
		}
		this.End(reason);
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x00080090 File Offset: 0x0007E290
	public override void Begin(Chore.Precondition.Context context)
	{
		if (this.driver != null)
		{
			Debug.LogErrorFormat("Chore.Begin driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver,
				context.consumerState.choreDriver
			});
		}
		if (this.provider == null)
		{
			Debug.LogErrorFormat("Chore.Begin provider is null {0} {1} {2}, provider {3}, driver {4}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver
			});
		}
		this.driver = context.consumerState.choreDriver;
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		KSelectable component = this.driver.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, this.GetStatusItem(), this);
		}
		smi.StartSM();
		if (this.onBegin != null)
		{
			this.onBegin(this);
		}
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x000801DA File Offset: 0x0007E3DA
	public override bool InProgress()
	{
		return this.driver != null;
	}

	// Token: 0x060017E6 RID: 6118
	protected abstract StateMachine.Instance GetSMI();

	// Token: 0x060017E7 RID: 6119 RVA: 0x000801E8 File Offset: 0x0007E3E8
	public StandardChoreBase(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete, Action<Chore> on_complete, Action<Chore> on_begin, Action<Chore> on_end, PriorityScreen.PriorityClass priority_class, int priority_value, bool is_preemptable, bool allow_in_context_menu, int priority_mod, bool add_to_daily_report, ReportManager.ReportType report_type)
	{
		this.target = target;
		if (priority_value == 2147483647)
		{
			priority_class = PriorityScreen.PriorityClass.topPriority;
			priority_value = 2;
		}
		if (priority_value < 1 || priority_value > 9)
		{
			Debug.LogErrorFormat("Priority Value Out Of Range: {0}", new object[]
			{
				priority_value
			});
		}
		this.masterPriority = new PrioritySetting(priority_class, priority_value);
		this.priorityMod = priority_mod;
		this.id = Chore.GetNextChoreID();
		if (chore_provider == null)
		{
			chore_provider = GlobalChoreProvider.Instance;
			DebugUtil.Assert(chore_provider != null);
		}
		this.choreType = chore_type;
		this.runUntilComplete = run_until_complete;
		this.onComplete = on_complete;
		this.onEnd = on_end;
		this.onBegin = on_begin;
		this.IsPreemptable = is_preemptable;
		this.AddPrecondition(ChorePreconditions.instance.IsValid, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPermitted, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPreemptable, null);
		this.AddPrecondition(ChorePreconditions.instance.HasUrge, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingEarly, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingLate, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOverrideTargetNullOrMe, null);
		chore_provider.AddChore(this);
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x0008032C File Offset: 0x0007E52C
	public virtual void SetPriorityMod(int priorityMod)
	{
		this.priorityMod = priorityMod;
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x00080338 File Offset: 0x0007E538
	public override List<Chore.PreconditionInstance> GetPreconditions()
	{
		if (this.arePreconditionsDirty)
		{
			this.preconditions.Sort((Chore.PreconditionInstance x, Chore.PreconditionInstance y) => x.condition.sortOrder.CompareTo(y.condition.sortOrder));
			this.arePreconditionsDirty = false;
		}
		return this.preconditions;
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x00080384 File Offset: 0x0007E584
	protected void SetPrioritizable(Prioritizable prioritizable)
	{
		if (prioritizable != null && prioritizable.IsPrioritizable())
		{
			this.prioritizable = prioritizable;
			this.masterPriority = prioritizable.GetMasterPriority();
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000803D7 File Offset: 0x0007E5D7
	private void ClearPrioritizable()
	{
		if (this.prioritizable != null)
		{
			Prioritizable prioritizable = this.prioritizable;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x0008040E File Offset: 0x0007E60E
	private void OnMasterPriorityChanged(PrioritySetting priority)
	{
		this.masterPriority = priority;
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x00080417 File Offset: 0x0007E617
	public void SetOverrideTarget(ChoreConsumer chore_consumer)
	{
		if (chore_consumer != null)
		{
			string name = chore_consumer.name;
		}
		this.overrideTarget = chore_consumer;
		this.Fail("New override target");
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x0008043C File Offset: 0x0007E63C
	protected virtual void End(string reason)
	{
		if (this.driver != null)
		{
			KSelectable component = this.driver.GetComponent<KSelectable>();
			if (component != null)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
			}
		}
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		smi.StopSM(reason);
		if (this.driver == null)
		{
			return;
		}
		this.lastDriver = this.driver;
		this.driver = null;
		if (this.onEnd != null)
		{
			this.onEnd(this);
		}
		if (this.onExit != null)
		{
			this.onExit(this);
		}
		this.driver = null;
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x00080504 File Offset: 0x0007E704
	protected void Succeed(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		this.isComplete = true;
		if (this.onComplete != null)
		{
			this.onComplete(this);
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x00080597 File Offset: 0x0007E797
	protected virtual StatusItem GetStatusItem()
	{
		return this.choreType.statusItem;
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x000805A4 File Offset: 0x0007E7A4
	protected virtual void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			this.Succeed(reason);
			return;
		}
		this.Fail(reason);
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x000805B9 File Offset: 0x0007E7B9
	private bool RemoveFromProvider()
	{
		if (this.provider != null)
		{
			this.provider.RemoveChore(this);
			return true;
		}
		return false;
	}

	// Token: 0x04000D42 RID: 3394
	private Action<Chore> onBegin;

	// Token: 0x04000D43 RID: 3395
	private Action<Chore> onEnd;

	// Token: 0x04000D44 RID: 3396
	public Action<Chore> onCleanup;

	// Token: 0x04000D45 RID: 3397
	private List<Chore.PreconditionInstance> preconditions = new List<Chore.PreconditionInstance>();

	// Token: 0x04000D46 RID: 3398
	private bool arePreconditionsDirty;

	// Token: 0x04000D47 RID: 3399
	public bool addToDailyReport;

	// Token: 0x04000D48 RID: 3400
	public ReportManager.ReportType reportType;
}

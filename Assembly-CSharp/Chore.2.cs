using System;
using UnityEngine;

// Token: 0x02000464 RID: 1124
public class Chore<StateMachineInstanceType> : StandardChoreBase, IStateMachineTarget where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060017F3 RID: 6131 RVA: 0x000805D8 File Offset: 0x0007E7D8
	// (set) Token: 0x060017F4 RID: 6132 RVA: 0x000805E0 File Offset: 0x0007E7E0
	public StateMachineInstanceType smi { get; protected set; }

	// Token: 0x060017F5 RID: 6133 RVA: 0x000805E9 File Offset: 0x0007E7E9
	protected override StateMachine.Instance GetSMI()
	{
		return this.smi;
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x000805F6 File Offset: 0x0007E7F6
	public int Subscribe(int hash, Action<object> handler)
	{
		return this.GetComponent<KPrefabID>().Subscribe(hash, handler);
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x00080605 File Offset: 0x0007E805
	public void Unsubscribe(int hash, Action<object> handler)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(hash, handler);
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x00080614 File Offset: 0x0007E814
	public void Unsubscribe(int id)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(id);
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x00080622 File Offset: 0x0007E822
	public void Trigger(int hash, object data = null)
	{
		this.GetComponent<KPrefabID>().Trigger(hash, data);
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x00080631 File Offset: 0x0007E831
	public ComponentType GetComponent<ComponentType>()
	{
		return this.target.GetComponent<ComponentType>();
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060017FB RID: 6139 RVA: 0x0008063E File Offset: 0x0007E83E
	public override GameObject gameObject
	{
		get
		{
			return this.target.gameObject;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060017FC RID: 6140 RVA: 0x0008064B File Offset: 0x0007E84B
	public Transform transform
	{
		get
		{
			return this.target.gameObject.transform;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060017FD RID: 6141 RVA: 0x0008065D File Offset: 0x0007E85D
	public string name
	{
		get
		{
			return this.gameObject.name;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060017FE RID: 6142 RVA: 0x0008066A File Offset: 0x0007E86A
	public override bool isNull
	{
		get
		{
			return this.target.isNull;
		}
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x00080678 File Offset: 0x0007E878
	public Chore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, PriorityScreen.PriorityClass master_priority_class = PriorityScreen.PriorityClass.basic, int master_priority_value = 5, bool is_preemptable = false, bool allow_in_context_menu = true, int priority_mod = 0, bool add_to_daily_report = false, ReportManager.ReportType report_type = ReportManager.ReportType.WorkTime) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, master_priority_class, master_priority_value, is_preemptable, allow_in_context_menu, priority_mod, add_to_daily_report, report_type)
	{
		target.Subscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		this.reportType = report_type;
		this.addToDailyReport = add_to_daily_report;
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, chore_type.Name, GameUtil.GetChoreName(this, null));
		}
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x000806F1 File Offset: 0x0007E8F1
	public override string ResolveString(string str)
	{
		if (!this.target.isNull)
		{
			str = str.Replace("{Target}", this.target.gameObject.GetProperName());
		}
		return base.ResolveString(str);
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x00080724 File Offset: 0x0007E924
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.target != null)
		{
			this.target.Unsubscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		}
		if (this.onCleanup != null)
		{
			this.onCleanup(this);
		}
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x00080764 File Offset: 0x0007E964
	private void OnTargetDestroyed(object data)
	{
		this.Cancel("Target Destroyed");
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x00080771 File Offset: 0x0007E971
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return base.CanPreempt(context);
	}
}

using System;
using UnityEngine;

// Token: 0x02000DE6 RID: 3558
[AddComponentMenu("KMonoBehaviour/scripts/UIScheduler")]
public class UIScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x06007102 RID: 28930 RVA: 0x002AC769 File Offset: 0x002AA969
	public static void DestroyInstance()
	{
		UIScheduler.Instance = null;
	}

	// Token: 0x06007103 RID: 28931 RVA: 0x002AC771 File Offset: 0x002AA971
	protected override void OnPrefabInit()
	{
		UIScheduler.Instance = this;
	}

	// Token: 0x06007104 RID: 28932 RVA: 0x002AC779 File Offset: 0x002AA979
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x06007105 RID: 28933 RVA: 0x002AC78D File Offset: 0x002AA98D
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x06007106 RID: 28934 RVA: 0x002AC7A4 File Offset: 0x002AA9A4
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x06007107 RID: 28935 RVA: 0x002AC7B1 File Offset: 0x002AA9B1
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06007108 RID: 28936 RVA: 0x002AC7C5 File Offset: 0x002AA9C5
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x06007109 RID: 28937 RVA: 0x002AC7D2 File Offset: 0x002AA9D2
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x04004DB3 RID: 19891
	private Scheduler scheduler = new Scheduler(new UIScheduler.UISchedulerClock());

	// Token: 0x04004DB4 RID: 19892
	public static UIScheduler Instance;

	// Token: 0x02001EEF RID: 7919
	public class UISchedulerClock : SchedulerClock
	{
		// Token: 0x0600AD03 RID: 44291 RVA: 0x003A856D File Offset: 0x003A676D
		public override float GetTime()
		{
			return Time.unscaledTime;
		}
	}
}

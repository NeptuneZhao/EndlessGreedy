using System;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
[AddComponentMenu("KMonoBehaviour/scripts/GameScheduler")]
public class GameScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x060019C5 RID: 6597 RVA: 0x000899D4 File Offset: 0x00087BD4
	public static void DestroyInstance()
	{
		GameScheduler.Instance = null;
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000899DC File Offset: 0x00087BDC
	protected override void OnPrefabInit()
	{
		GameScheduler.Instance = this;
		Singleton<StateMachineManager>.Instance.RegisterScheduler(this.scheduler);
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x000899F4 File Offset: 0x00087BF4
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x00089A08 File Offset: 0x00087C08
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x00089A1F File Offset: 0x00087C1F
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x00089A2C File Offset: 0x00087C2C
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x00089A40 File Offset: 0x00087C40
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x00089A4D File Offset: 0x00087C4D
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x04000EB1 RID: 3761
	private Scheduler scheduler = new Scheduler(new GameScheduler.GameSchedulerClock());

	// Token: 0x04000EB2 RID: 3762
	public static GameScheduler Instance;

	// Token: 0x02001275 RID: 4725
	public class GameSchedulerClock : SchedulerClock
	{
		// Token: 0x06008344 RID: 33604 RVA: 0x0031EA9F File Offset: 0x0031CC9F
		public override float GetTime()
		{
			return GameClock.Instance.GetTime();
		}
	}
}

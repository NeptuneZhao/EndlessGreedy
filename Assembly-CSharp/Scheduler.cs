using System;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public class Scheduler : IScheduler
{
	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060019CF RID: 6607 RVA: 0x00089A6D File Offset: 0x00087C6D
	public int Count
	{
		get
		{
			return this.entries.Count;
		}
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x00089A7A File Offset: 0x00087C7A
	public Scheduler(SchedulerClock clock)
	{
		this.clock = clock;
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x00089A9F File Offset: 0x00087C9F
	public float GetTime()
	{
		return this.clock.GetTime();
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x00089AAC File Offset: 0x00087CAC
	private SchedulerHandle Schedule(SchedulerEntry entry)
	{
		this.entries.Enqueue(entry.time, entry);
		return new SchedulerHandle(this, entry);
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x00089AC8 File Offset: 0x00087CC8
	private SchedulerHandle Schedule(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		SchedulerEntry entry = new SchedulerEntry(name, time + this.clock.GetTime(), time_interval, callback, callback_data, profiler_obj);
		return this.Schedule(entry);
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x00089AF8 File Offset: 0x00087CF8
	public void FreeResources()
	{
		this.clock = null;
		if (this.entries != null)
		{
			while (this.entries.Count > 0)
			{
				this.entries.Dequeue().Value.FreeResources();
			}
		}
		this.entries = null;
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x00089B48 File Offset: 0x00087D48
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		if (group != null && group.scheduler != this)
		{
			global::Debug.LogError("Scheduler group mismatch!");
		}
		SchedulerHandle schedulerHandle = this.Schedule(name, time, -1f, callback, callback_data, null);
		if (group != null)
		{
			group.Add(schedulerHandle);
		}
		return schedulerHandle;
	}

	// Token: 0x060019D6 RID: 6614 RVA: 0x00089B8C File Offset: 0x00087D8C
	public void Clear(SchedulerHandle handle)
	{
		handle.entry.Clear();
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x00089B9C File Offset: 0x00087D9C
	public void Update()
	{
		if (this.Count == 0)
		{
			return;
		}
		int count = this.Count;
		int num = 0;
		using (new KProfiler.Region("Scheduler.Update", null))
		{
			float time = this.clock.GetTime();
			if (this.previousTime != time)
			{
				this.previousTime = time;
				while (num < count && time >= this.entries.Peek().Key)
				{
					SchedulerEntry value = this.entries.Dequeue().Value;
					if (value.callback != null)
					{
						value.callback(value.callbackData);
					}
					num++;
				}
			}
		}
	}

	// Token: 0x04000EB3 RID: 3763
	public FloatHOTQueue<SchedulerEntry> entries = new FloatHOTQueue<SchedulerEntry>();

	// Token: 0x04000EB4 RID: 3764
	private SchedulerClock clock;

	// Token: 0x04000EB5 RID: 3765
	private float previousTime = float.NegativeInfinity;
}

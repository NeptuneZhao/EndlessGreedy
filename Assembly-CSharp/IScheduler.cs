using System;

// Token: 0x020004A9 RID: 1193
public interface IScheduler
{
	// Token: 0x060019CE RID: 6606
	SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null);
}

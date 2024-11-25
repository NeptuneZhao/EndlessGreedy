using System;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public struct SchedulerEntry
{
	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060019DA RID: 6618 RVA: 0x00089C64 File Offset: 0x00087E64
	// (set) Token: 0x060019DB RID: 6619 RVA: 0x00089C6C File Offset: 0x00087E6C
	public SchedulerEntry.Details details { readonly get; private set; }

	// Token: 0x060019DC RID: 6620 RVA: 0x00089C75 File Offset: 0x00087E75
	public SchedulerEntry(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		this.time = time;
		this.details = new SchedulerEntry.Details(name, callback, callback_data, time_interval, profiler_obj);
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x00089C91 File Offset: 0x00087E91
	public void FreeResources()
	{
		this.details = null;
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060019DE RID: 6622 RVA: 0x00089C9A File Offset: 0x00087E9A
	public Action<object> callback
	{
		get
		{
			return this.details.callback;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060019DF RID: 6623 RVA: 0x00089CA7 File Offset: 0x00087EA7
	public object callbackData
	{
		get
		{
			return this.details.callbackData;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060019E0 RID: 6624 RVA: 0x00089CB4 File Offset: 0x00087EB4
	public float timeInterval
	{
		get
		{
			return this.details.timeInterval;
		}
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x00089CC1 File Offset: 0x00087EC1
	public override string ToString()
	{
		return this.time.ToString();
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x00089CCE File Offset: 0x00087ECE
	public void Clear()
	{
		this.details.callback = null;
	}

	// Token: 0x04000EB6 RID: 3766
	public float time;

	// Token: 0x02001276 RID: 4726
	public class Details
	{
		// Token: 0x06008346 RID: 33606 RVA: 0x0031EAB3 File Offset: 0x0031CCB3
		public Details(string name, Action<object> callback, object callback_data, float time_interval, GameObject profiler_obj)
		{
			this.timeInterval = time_interval;
			this.callback = callback;
			this.callbackData = callback_data;
		}

		// Token: 0x04006384 RID: 25476
		public Action<object> callback;

		// Token: 0x04006385 RID: 25477
		public object callbackData;

		// Token: 0x04006386 RID: 25478
		public float timeInterval;
	}
}

using System;
using System.Collections.Generic;

// Token: 0x020004AD RID: 1197
public class SchedulerGroup
{
	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00089CDC File Offset: 0x00087EDC
	// (set) Token: 0x060019E4 RID: 6628 RVA: 0x00089CE4 File Offset: 0x00087EE4
	public Scheduler scheduler { get; private set; }

	// Token: 0x060019E5 RID: 6629 RVA: 0x00089CED File Offset: 0x00087EED
	public SchedulerGroup(Scheduler scheduler)
	{
		this.scheduler = scheduler;
		this.Reset();
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x00089D0D File Offset: 0x00087F0D
	public void FreeResources()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		this.scheduler = null;
		if (this.handles != null)
		{
			this.handles.Clear();
		}
		this.handles = null;
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x00089D44 File Offset: 0x00087F44
	public void Reset()
	{
		foreach (SchedulerHandle schedulerHandle in this.handles)
		{
			schedulerHandle.ClearScheduler();
		}
		this.handles.Clear();
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x00089DA4 File Offset: 0x00087FA4
	public void Add(SchedulerHandle handle)
	{
		this.handles.Add(handle);
	}

	// Token: 0x04000EB9 RID: 3769
	private List<SchedulerHandle> handles = new List<SchedulerHandle>();
}

using System;

// Token: 0x020004AE RID: 1198
public struct SchedulerHandle
{
	// Token: 0x060019E9 RID: 6633 RVA: 0x00089DB2 File Offset: 0x00087FB2
	public SchedulerHandle(Scheduler scheduler, SchedulerEntry entry)
	{
		this.entry = entry;
		this.scheduler = scheduler;
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060019EA RID: 6634 RVA: 0x00089DC2 File Offset: 0x00087FC2
	public float TimeRemaining
	{
		get
		{
			if (!this.IsValid)
			{
				return -1f;
			}
			return this.entry.time - this.scheduler.GetTime();
		}
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x00089DE9 File Offset: 0x00087FE9
	public void FreeResources()
	{
		this.entry.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x00089DFD File Offset: 0x00087FFD
	public void ClearScheduler()
	{
		if (this.scheduler == null)
		{
			return;
		}
		this.scheduler.Clear(this);
		this.scheduler = null;
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060019ED RID: 6637 RVA: 0x00089E20 File Offset: 0x00088020
	public bool IsValid
	{
		get
		{
			return this.scheduler != null;
		}
	}

	// Token: 0x04000EBA RID: 3770
	public SchedulerEntry entry;

	// Token: 0x04000EBB RID: 3771
	private Scheduler scheduler;
}

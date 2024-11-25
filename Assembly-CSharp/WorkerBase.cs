using System;
using Klei.AI;

// Token: 0x020005F9 RID: 1529
public abstract class WorkerBase : KMonoBehaviour
{
	// Token: 0x06002570 RID: 9584
	public abstract bool UsesMultiTool();

	// Token: 0x06002571 RID: 9585
	public abstract bool IsFetchDrone();

	// Token: 0x06002572 RID: 9586
	public abstract KBatchedAnimController GetAnimController();

	// Token: 0x06002573 RID: 9587
	public abstract WorkerBase.State GetState();

	// Token: 0x06002574 RID: 9588
	public abstract WorkerBase.StartWorkInfo GetStartWorkInfo();

	// Token: 0x06002575 RID: 9589
	public abstract Workable GetWorkable();

	// Token: 0x06002576 RID: 9590
	public abstract AttributeConverterInstance GetAttributeConverter(string id);

	// Token: 0x06002577 RID: 9591
	public abstract Guid OfferStatusItem(StatusItem item, object data = null);

	// Token: 0x06002578 RID: 9592
	public abstract void RevokeStatusItem(Guid id);

	// Token: 0x06002579 RID: 9593
	public abstract void StartWork(WorkerBase.StartWorkInfo start_work_info);

	// Token: 0x0600257A RID: 9594
	public abstract void StopWork();

	// Token: 0x0600257B RID: 9595
	public abstract bool InstantlyFinish();

	// Token: 0x0600257C RID: 9596
	public abstract WorkerBase.WorkResult Work(float dt);

	// Token: 0x0600257D RID: 9597
	public abstract CellOffset[] GetFetchCellOffsets();

	// Token: 0x0600257E RID: 9598
	public abstract void SetWorkCompleteData(object data);

	// Token: 0x020013EA RID: 5098
	public class StartWorkInfo
	{
		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060088C7 RID: 35015 RVA: 0x0032F1DD File Offset: 0x0032D3DD
		// (set) Token: 0x060088C8 RID: 35016 RVA: 0x0032F1E5 File Offset: 0x0032D3E5
		public Workable workable { get; set; }

		// Token: 0x060088C9 RID: 35017 RVA: 0x0032F1EE File Offset: 0x0032D3EE
		public StartWorkInfo(Workable workable)
		{
			this.workable = workable;
		}
	}

	// Token: 0x020013EB RID: 5099
	public enum State
	{
		// Token: 0x04006863 RID: 26723
		Idle,
		// Token: 0x04006864 RID: 26724
		Working,
		// Token: 0x04006865 RID: 26725
		PendingCompletion,
		// Token: 0x04006866 RID: 26726
		Completing
	}

	// Token: 0x020013EC RID: 5100
	public enum WorkResult
	{
		// Token: 0x04006868 RID: 26728
		Success,
		// Token: 0x04006869 RID: 26729
		InProgress,
		// Token: 0x0400686A RID: 26730
		Failed
	}
}

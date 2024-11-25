using System;

// Token: 0x02000431 RID: 1073
public interface IWorkerPrioritizable
{
	// Token: 0x060016DF RID: 5855
	bool GetWorkerPriority(WorkerBase worker, out int priority);
}

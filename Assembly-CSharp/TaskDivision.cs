using System;

// Token: 0x020007DD RID: 2013
internal class TaskDivision<Task, SharedData> where Task : DivisibleTask<SharedData>, new()
{
	// Token: 0x06003789 RID: 14217 RVA: 0x0012EB5C File Offset: 0x0012CD5C
	public TaskDivision(int taskCount)
	{
		this.tasks = new Task[taskCount];
		for (int num = 0; num != this.tasks.Length; num++)
		{
			this.tasks[num] = Activator.CreateInstance<Task>();
		}
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x0012EB9F File Offset: 0x0012CD9F
	public TaskDivision() : this(CPUBudget.coreCount)
	{
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x0012EBAC File Offset: 0x0012CDAC
	public void Initialize(int count)
	{
		int num = count / this.tasks.Length;
		for (int num2 = 0; num2 != this.tasks.Length; num2++)
		{
			this.tasks[num2].start = num2 * num;
			this.tasks[num2].end = this.tasks[num2].start + num;
		}
		DebugUtil.Assert(this.tasks[this.tasks.Length - 1].end + count % this.tasks.Length == count);
		this.tasks[this.tasks.Length - 1].end = count;
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x0012EC70 File Offset: 0x0012CE70
	public void Run(SharedData sharedData)
	{
		Task[] array = this.tasks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Run(sharedData);
		}
	}

	// Token: 0x0400217B RID: 8571
	public Task[] tasks;
}

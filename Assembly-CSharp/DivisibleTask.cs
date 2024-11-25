using System;

// Token: 0x020007DC RID: 2012
internal abstract class DivisibleTask<SharedData> : IWorkItem<SharedData>
{
	// Token: 0x06003786 RID: 14214 RVA: 0x0012EB43 File Offset: 0x0012CD43
	public void Run(SharedData sharedData)
	{
		this.RunDivision(sharedData);
	}

	// Token: 0x06003787 RID: 14215 RVA: 0x0012EB4C File Offset: 0x0012CD4C
	protected DivisibleTask(string name)
	{
		this.name = name;
	}

	// Token: 0x06003788 RID: 14216
	protected abstract void RunDivision(SharedData sharedData);

	// Token: 0x04002178 RID: 8568
	public string name;

	// Token: 0x04002179 RID: 8569
	public int start;

	// Token: 0x0400217A RID: 8570
	public int end;
}

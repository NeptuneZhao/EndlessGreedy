using System;
using System.Collections.Generic;

// Token: 0x020007AD RID: 1965
public class CheckedHandleVector<T> where T : new()
{
	// Token: 0x060035C0 RID: 13760 RVA: 0x001247AC File Offset: 0x001229AC
	public CheckedHandleVector(int initial_size)
	{
		this.handleVector = new HandleVector<T>(initial_size);
		this.isFree = new List<bool>(initial_size);
		for (int i = 0; i < initial_size; i++)
		{
			this.isFree.Add(true);
		}
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x001247FC File Offset: 0x001229FC
	public HandleVector<T>.Handle Add(T item, string debug_info)
	{
		HandleVector<T>.Handle result = this.handleVector.Add(item);
		if (result.index >= this.isFree.Count)
		{
			this.isFree.Add(false);
		}
		else
		{
			this.isFree[result.index] = false;
		}
		int i = this.handleVector.Items.Count;
		while (i > this.debugInfo.Count)
		{
			this.debugInfo.Add(null);
		}
		this.debugInfo[result.index] = debug_info;
		return result;
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x0012488C File Offset: 0x00122A8C
	public T Release(HandleVector<T>.Handle handle)
	{
		if (this.isFree[handle.index])
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Tried to double free checked handle ",
				handle.index,
				"- Debug info:",
				this.debugInfo[handle.index]
			});
		}
		this.isFree[handle.index] = true;
		return this.handleVector.Release(handle);
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x0012490B File Offset: 0x00122B0B
	public T Get(HandleVector<T>.Handle handle)
	{
		return this.handleVector.GetItem(handle);
	}

	// Token: 0x04002008 RID: 8200
	private HandleVector<T> handleVector;

	// Token: 0x04002009 RID: 8201
	private List<string> debugInfo = new List<string>();

	// Token: 0x0400200A RID: 8202
	private List<bool> isFree;
}

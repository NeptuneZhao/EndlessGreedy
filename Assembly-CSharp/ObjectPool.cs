using System;
using System.Collections.Generic;

// Token: 0x0200040D RID: 1037
public class ObjectPool<T>
{
	// Token: 0x060015E2 RID: 5602 RVA: 0x000776E4 File Offset: 0x000758E4
	public ObjectPool(Func<T> instantiator, int initial_count = 0)
	{
		this.instantiator = instantiator;
		this.unused = new Stack<T>(initial_count);
		for (int i = 0; i < initial_count; i++)
		{
			this.unused.Push(instantiator());
		}
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x00077728 File Offset: 0x00075928
	public virtual T GetInstance()
	{
		T result = default(T);
		if (this.unused.Count > 0)
		{
			result = this.unused.Pop();
		}
		else
		{
			result = this.instantiator();
		}
		return result;
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x00077766 File Offset: 0x00075966
	public void ReleaseInstance(T instance)
	{
		if (object.Equals(instance, null))
		{
			return;
		}
		this.unused.Push(instance);
	}

	// Token: 0x04000C73 RID: 3187
	protected Stack<T> unused;

	// Token: 0x04000C74 RID: 3188
	protected Func<T> instantiator;
}

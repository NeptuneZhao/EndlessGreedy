using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200040C RID: 1036
public class ListWithEvents<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060015CF RID: 5583 RVA: 0x000774B2 File Offset: 0x000756B2
	public int Count
	{
		get
		{
			return this.internalList.Count;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060015D0 RID: 5584 RVA: 0x000774BF File Offset: 0x000756BF
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.internalList).IsReadOnly;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060015D1 RID: 5585 RVA: 0x000774CC File Offset: 0x000756CC
	// (remove) Token: 0x060015D2 RID: 5586 RVA: 0x00077504 File Offset: 0x00075704
	public event Action<T> onAdd;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060015D3 RID: 5587 RVA: 0x0007753C File Offset: 0x0007573C
	// (remove) Token: 0x060015D4 RID: 5588 RVA: 0x00077574 File Offset: 0x00075774
	public event Action<T> onRemove;

	// Token: 0x17000056 RID: 86
	public T this[int index]
	{
		get
		{
			return this.internalList[index];
		}
		set
		{
			this.internalList[index] = value;
		}
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x000775C6 File Offset: 0x000757C6
	public void Add(T item)
	{
		this.internalList.Add(item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x000775E8 File Offset: 0x000757E8
	public void Insert(int index, T item)
	{
		this.internalList.Insert(index, item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x0007760C File Offset: 0x0007580C
	public void RemoveAt(int index)
	{
		T obj = this.internalList[index];
		this.internalList.RemoveAt(index);
		if (this.onRemove != null)
		{
			this.onRemove(obj);
		}
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x00077646 File Offset: 0x00075846
	public bool Remove(T item)
	{
		bool flag = this.internalList.Remove(item);
		if (flag && this.onRemove != null)
		{
			this.onRemove(item);
		}
		return flag;
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x0007766B File Offset: 0x0007586B
	public void Clear()
	{
		while (this.Count > 0)
		{
			this.RemoveAt(0);
		}
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x0007767F File Offset: 0x0007587F
	public int IndexOf(T item)
	{
		return this.internalList.IndexOf(item);
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x0007768D File Offset: 0x0007588D
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.internalList.CopyTo(array, arrayIndex);
	}

	// Token: 0x060015DE RID: 5598 RVA: 0x0007769C File Offset: 0x0007589C
	public bool Contains(T item)
	{
		return this.internalList.Contains(item);
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x000776AA File Offset: 0x000758AA
	public IEnumerator<T> GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x000776BC File Offset: 0x000758BC
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x04000C70 RID: 3184
	private List<T> internalList = new List<T>();
}

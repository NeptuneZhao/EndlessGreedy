using System;
using System.Collections.Generic;

// Token: 0x0200041A RID: 1050
public class StringSearchableList<T>
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x0600164D RID: 5709 RVA: 0x000783ED File Offset: 0x000765ED
	// (set) Token: 0x0600164E RID: 5710 RVA: 0x000783F5 File Offset: 0x000765F5
	public bool didUseFilter { get; private set; }

	// Token: 0x0600164F RID: 5711 RVA: 0x000783FE File Offset: 0x000765FE
	public StringSearchableList(List<T> allValues, StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.allValues = allValues;
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.filteredValues = new List<T>();
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x0007842A File Offset: 0x0007662A
	public StringSearchableList(StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.allValues = new List<T>();
		this.filteredValues = new List<T>();
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x0007845C File Offset: 0x0007665C
	public void Refilter()
	{
		if (StringSearchableListUtil.ShouldUseFilter(this.filter))
		{
			this.filteredValues.Clear();
			foreach (T t in this.allValues)
			{
				if (!this.shouldFilterOutFn(t, this.filter))
				{
					this.filteredValues.Add(t);
				}
			}
			this.didUseFilter = true;
			return;
		}
		if (this.filteredValues.Count != this.allValues.Count)
		{
			this.filteredValues.Clear();
			this.filteredValues.AddRange(this.allValues);
		}
		this.didUseFilter = false;
	}

	// Token: 0x04000C86 RID: 3206
	public string filter = "";

	// Token: 0x04000C87 RID: 3207
	public List<T> allValues;

	// Token: 0x04000C88 RID: 3208
	public List<T> filteredValues;

	// Token: 0x04000C8A RID: 3210
	public readonly StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn;

	// Token: 0x02001175 RID: 4469
	// (Invoke) Token: 0x06007FC2 RID: 32706
	public delegate bool ShouldFilterOutFn(T candidateValue, in string filter);
}

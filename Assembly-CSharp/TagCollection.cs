using System;
using System.Collections.Generic;

// Token: 0x02000B2E RID: 2862
public class TagCollection : IReadonlyTags
{
	// Token: 0x0600555D RID: 21853 RVA: 0x001E7B51 File Offset: 0x001E5D51
	public TagCollection()
	{
	}

	// Token: 0x0600555E RID: 21854 RVA: 0x001E7B64 File Offset: 0x001E5D64
	public TagCollection(int[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(initialTags[i]);
		}
	}

	// Token: 0x0600555F RID: 21855 RVA: 0x001E7BA0 File Offset: 0x001E5DA0
	public TagCollection(string[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(Hash.SDBMLower(initialTags[i]));
		}
	}

	// Token: 0x06005560 RID: 21856 RVA: 0x001E7BE0 File Offset: 0x001E5DE0
	public TagCollection(TagCollection initialTags)
	{
		if (initialTags != null && initialTags.tags != null)
		{
			this.tags.UnionWith(initialTags.tags);
		}
	}

	// Token: 0x06005561 RID: 21857 RVA: 0x001E7C10 File Offset: 0x001E5E10
	public TagCollection Append(TagCollection others)
	{
		foreach (int item in others.tags)
		{
			this.tags.Add(item);
		}
		return this;
	}

	// Token: 0x06005562 RID: 21858 RVA: 0x001E7C6C File Offset: 0x001E5E6C
	public void AddTag(string tag)
	{
		this.tags.Add(Hash.SDBMLower(tag));
	}

	// Token: 0x06005563 RID: 21859 RVA: 0x001E7C80 File Offset: 0x001E5E80
	public void AddTag(int tag)
	{
		this.tags.Add(tag);
	}

	// Token: 0x06005564 RID: 21860 RVA: 0x001E7C8F File Offset: 0x001E5E8F
	public void RemoveTag(string tag)
	{
		this.tags.Remove(Hash.SDBMLower(tag));
	}

	// Token: 0x06005565 RID: 21861 RVA: 0x001E7CA3 File Offset: 0x001E5EA3
	public void RemoveTag(int tag)
	{
		this.tags.Remove(tag);
	}

	// Token: 0x06005566 RID: 21862 RVA: 0x001E7CB2 File Offset: 0x001E5EB2
	public bool HasTag(string tag)
	{
		return this.tags.Contains(Hash.SDBMLower(tag));
	}

	// Token: 0x06005567 RID: 21863 RVA: 0x001E7CC5 File Offset: 0x001E5EC5
	public bool HasTag(int tag)
	{
		return this.tags.Contains(tag);
	}

	// Token: 0x06005568 RID: 21864 RVA: 0x001E7CD4 File Offset: 0x001E5ED4
	public bool HasTags(int[] searchTags)
	{
		for (int i = 0; i < searchTags.Length; i++)
		{
			if (!this.tags.Contains(searchTags[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040037E1 RID: 14305
	private HashSet<int> tags = new HashSet<int>();
}

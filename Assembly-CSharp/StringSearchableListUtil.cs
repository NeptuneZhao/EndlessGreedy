using System;
using System.Linq;

// Token: 0x0200041B RID: 1051
public static class StringSearchableListUtil
{
	// Token: 0x06001652 RID: 5714 RVA: 0x00078524 File Offset: 0x00076724
	public static bool DoAnyTagsMatchFilter(string[] lowercaseTags, in string filter)
	{
		string text = filter.Trim().ToLowerInvariant();
		string[] source = text.Split(' ', StringSplitOptions.None);
		for (int i = 0; i < lowercaseTags.Length; i++)
		{
			string tag = lowercaseTags[i];
			if (StringSearchableListUtil.DoesTagMatchFilter(tag, text))
			{
				return true;
			}
			if ((from f in source
			select StringSearchableListUtil.DoesTagMatchFilter(tag, f)).All((bool result) => result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x000785B7 File Offset: 0x000767B7
	public static bool DoesTagMatchFilter(string lowercaseTag, in string filter)
	{
		return string.IsNullOrWhiteSpace(filter) || lowercaseTag.Contains(filter);
	}

	// Token: 0x06001654 RID: 5716 RVA: 0x000785D1 File Offset: 0x000767D1
	public static bool ShouldUseFilter(string filter)
	{
		return !string.IsNullOrWhiteSpace(filter);
	}
}

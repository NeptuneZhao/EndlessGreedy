using System;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public static class KSelectableExtensions
{
	// Token: 0x06002099 RID: 8345 RVA: 0x000B6467 File Offset: 0x000B4667
	public static string GetProperName(this Component cmp)
	{
		if (cmp != null && cmp.gameObject != null)
		{
			return cmp.gameObject.GetProperName();
		}
		return "";
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x000B6494 File Offset: 0x000B4694
	public static string GetProperName(this GameObject go)
	{
		if (go != null)
		{
			KSelectable component = go.GetComponent<KSelectable>();
			if (component != null)
			{
				return component.GetName();
			}
		}
		return "";
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x000B64C6 File Offset: 0x000B46C6
	public static string GetProperName(this KSelectable cmp)
	{
		if (cmp != null)
		{
			return cmp.GetName();
		}
		return "";
	}
}

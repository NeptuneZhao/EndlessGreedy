using System;
using UnityEngine;

// Token: 0x02000938 RID: 2360
public static class KPrefabIDExtensions
{
	// Token: 0x06004495 RID: 17557 RVA: 0x00186ABA File Offset: 0x00184CBA
	public static Tag PrefabID(this Component cmp)
	{
		return cmp.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06004496 RID: 17558 RVA: 0x00186AC7 File Offset: 0x00184CC7
	public static Tag PrefabID(this GameObject go)
	{
		return go.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06004497 RID: 17559 RVA: 0x00186AD4 File Offset: 0x00184CD4
	public static Tag PrefabID(this StateMachine.Instance smi)
	{
		return smi.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06004498 RID: 17560 RVA: 0x00186AE1 File Offset: 0x00184CE1
	public static bool IsPrefabID(this Component cmp, Tag id)
	{
		return cmp.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06004499 RID: 17561 RVA: 0x00186AEF File Offset: 0x00184CEF
	public static bool IsPrefabID(this GameObject go, Tag id)
	{
		return go.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x0600449A RID: 17562 RVA: 0x00186AFD File Offset: 0x00184CFD
	public static bool HasTag(this Component cmp, Tag tag)
	{
		return cmp.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x0600449B RID: 17563 RVA: 0x00186B0B File Offset: 0x00184D0B
	public static bool HasTag(this GameObject go, Tag tag)
	{
		return go.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x0600449C RID: 17564 RVA: 0x00186B19 File Offset: 0x00184D19
	public static bool HasAnyTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x0600449D RID: 17565 RVA: 0x00186B27 File Offset: 0x00184D27
	public static bool HasAnyTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x0600449E RID: 17566 RVA: 0x00186B35 File Offset: 0x00184D35
	public static bool HasAllTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x0600449F RID: 17567 RVA: 0x00186B43 File Offset: 0x00184D43
	public static bool HasAllTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x060044A0 RID: 17568 RVA: 0x00186B51 File Offset: 0x00184D51
	public static void AddTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x060044A1 RID: 17569 RVA: 0x00186B60 File Offset: 0x00184D60
	public static void AddTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x060044A2 RID: 17570 RVA: 0x00186B6F File Offset: 0x00184D6F
	public static void RemoveTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().RemoveTag(tag);
	}

	// Token: 0x060044A3 RID: 17571 RVA: 0x00186B7D File Offset: 0x00184D7D
	public static void RemoveTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().RemoveTag(tag);
	}
}

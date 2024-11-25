using System;
using UnityEngine;

// Token: 0x020008C4 RID: 2244
public static class GameTagExtensions
{
	// Token: 0x06003F2A RID: 16170 RVA: 0x00163C8F File Offset: 0x00161E8F
	public static GameObject Prefab(this Tag tag)
	{
		return Assets.GetPrefab(tag);
	}

	// Token: 0x06003F2B RID: 16171 RVA: 0x00163C97 File Offset: 0x00161E97
	public static string ProperName(this Tag tag)
	{
		return TagManager.GetProperName(tag, false);
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x00163CA0 File Offset: 0x00161EA0
	public static string ProperNameStripLink(this Tag tag)
	{
		return TagManager.GetProperName(tag, true);
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x00163CA9 File Offset: 0x00161EA9
	public static Tag Create(SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x00163CBD File Offset: 0x00161EBD
	public static Tag CreateTag(this SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}
}

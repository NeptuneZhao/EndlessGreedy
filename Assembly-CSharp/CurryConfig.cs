using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class CurryConfig : IEntityConfig
{
	// Token: 0x0600087F RID: 2175 RVA: 0x00036F00 File Offset: 0x00035100
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00036F08 File Offset: 0x00035108
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Curry", STRINGS.ITEMS.FOOD.CURRY.NAME, STRINGS.ITEMS.FOOD.CURRY.DESC, 1f, false, Assets.GetAnim("curried_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CURRY);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00036F6C File Offset: 0x0003516C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00036F6E File Offset: 0x0003516E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E3 RID: 1507
	public const string ID = "Curry";
}

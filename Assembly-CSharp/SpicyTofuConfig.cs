using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class SpicyTofuConfig : IEntityConfig
{
	// Token: 0x0600093F RID: 2367 RVA: 0x00038482 File Offset: 0x00036682
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0003848C File Offset: 0x0003668C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpicyTofu", STRINGS.ITEMS.FOOD.SPICYTOFU.NAME, STRINGS.ITEMS.FOOD.SPICYTOFU.DESC, 1f, false, Assets.GetAnim("spicey_tofu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICY_TOFU);
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x000384F0 File Offset: 0x000366F0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x000384F2 File Offset: 0x000366F2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400062D RID: 1581
	public const string ID = "SpicyTofu";

	// Token: 0x0400062E RID: 1582
	public static ComplexRecipe recipe;
}

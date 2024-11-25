using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class GrilledPrickleFruitConfig : IEntityConfig
{
	// Token: 0x060008C1 RID: 2241 RVA: 0x0003755C File Offset: 0x0003575C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00037564 File Offset: 0x00035764
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GrilledPrickleFruit", STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.NAME, STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("gristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GRILLED_PRICKLEFRUIT);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x000375C8 File Offset: 0x000357C8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x000375CA File Offset: 0x000357CA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005FC RID: 1532
	public const string ID = "GrilledPrickleFruit";

	// Token: 0x040005FD RID: 1533
	public static ComplexRecipe recipe;
}

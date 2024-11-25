using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class BerryPieConfig : IEntityConfig
{
	// Token: 0x06000849 RID: 2121 RVA: 0x000368FE File Offset: 0x00034AFE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00036908 File Offset: 0x00034B08
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BerryPie", STRINGS.ITEMS.FOOD.BERRYPIE.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DESC, 1f, false, Assets.GetAnim("wormwood_berry_pie_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.55f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BERRY_PIE);
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0003696C File Offset: 0x00034B6C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0003696E File Offset: 0x00034B6E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005CA RID: 1482
	public const string ID = "BerryPie";

	// Token: 0x040005CB RID: 1483
	public static ComplexRecipe recipe;
}

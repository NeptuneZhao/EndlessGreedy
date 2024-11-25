using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class FriedMushroomConfig : IEntityConfig
{
	// Token: 0x060008AD RID: 2221 RVA: 0x00037358 File Offset: 0x00035558
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00037360 File Offset: 0x00035560
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushroom", STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.NAME, STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscapfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIED_MUSHROOM);
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x000373C4 File Offset: 0x000355C4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x000373C6 File Offset: 0x000355C6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F4 RID: 1524
	public const string ID = "FriedMushroom";

	// Token: 0x040005F5 RID: 1525
	public static ComplexRecipe recipe;
}

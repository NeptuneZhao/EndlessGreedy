using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class FriedMushBarConfig : IEntityConfig
{
	// Token: 0x060008A8 RID: 2216 RVA: 0x000372DE File Offset: 0x000354DE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x000372E8 File Offset: 0x000354E8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushBar", STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.NAME, STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIEDMUSHBAR);
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0003734C File Offset: 0x0003554C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0003734E File Offset: 0x0003554E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F2 RID: 1522
	public const string ID = "FriedMushBar";

	// Token: 0x040005F3 RID: 1523
	public static ComplexRecipe recipe;
}

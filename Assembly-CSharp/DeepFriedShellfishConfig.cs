using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class DeepFriedShellfishConfig : IEntityConfig
{
	// Token: 0x06000899 RID: 2201 RVA: 0x00037174 File Offset: 0x00035374
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0003717C File Offset: 0x0003537C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedShellfish", STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.DESC, 1f, false, Assets.GetAnim("deepfried_shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_SHELLFISH);
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x000371E0 File Offset: 0x000353E0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000371E2 File Offset: 0x000353E2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005EE RID: 1518
	public const string ID = "DeepFriedShellfish";

	// Token: 0x040005EF RID: 1519
	public static ComplexRecipe recipe;
}

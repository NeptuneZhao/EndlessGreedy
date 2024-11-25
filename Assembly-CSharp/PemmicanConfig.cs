using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class PemmicanConfig : IEntityConfig
{
	// Token: 0x060008F2 RID: 2290 RVA: 0x00037B84 File Offset: 0x00035D84
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x00037B8C File Offset: 0x00035D8C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Pemmican", STRINGS.ITEMS.FOOD.PEMMICAN.NAME, STRINGS.ITEMS.FOOD.PEMMICAN.DESC, 1f, false, Assets.GetAnim("pemmican_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.PEMMICAN);
		ComplexRecipeManager.Get().GetRecipe(PemmicanConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00037C13 File Offset: 0x00035E13
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00037C15 File Offset: 0x00035E15
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400060E RID: 1550
	public const string ID = "Pemmican";

	// Token: 0x0400060F RID: 1551
	public static ComplexRecipe recipe;
}

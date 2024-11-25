using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class TofuConfig : IEntityConfig
{
	// Token: 0x0600096E RID: 2414 RVA: 0x0003890D File Offset: 0x00036B0D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00038914 File Offset: 0x00036B14
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Tofu", STRINGS.ITEMS.FOOD.TOFU.NAME, STRINGS.ITEMS.FOOD.TOFU.DESC, 1f, false, Assets.GetAnim("loafu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.TOFU);
		ComplexRecipeManager.Get().GetRecipe(TofuConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0003899B File Offset: 0x00036B9B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0003899D File Offset: 0x00036B9D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000643 RID: 1603
	public const string ID = "Tofu";

	// Token: 0x04000644 RID: 1604
	public static ComplexRecipe recipe;
}

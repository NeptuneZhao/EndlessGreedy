using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class BasicPlantBarConfig : IEntityConfig
{
	// Token: 0x0600083F RID: 2111 RVA: 0x000367E9 File Offset: 0x000349E9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x000367F0 File Offset: 0x000349F0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantBar", STRINGS.ITEMS.FOOD.BASICPLANTBAR.NAME, STRINGS.ITEMS.FOOD.BASICPLANTBAR.DESC, 1f, false, Assets.GetAnim("liceloaf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTBAR);
		ComplexRecipeManager.Get().GetRecipe(BasicPlantBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00036877 File Offset: 0x00034A77
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00036879 File Offset: 0x00034A79
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005C7 RID: 1479
	public const string ID = "BasicPlantBar";

	// Token: 0x040005C8 RID: 1480
	public static ComplexRecipe recipe;
}

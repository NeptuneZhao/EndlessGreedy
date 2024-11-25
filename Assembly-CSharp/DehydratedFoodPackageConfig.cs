using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class DehydratedFoodPackageConfig : IEntityConfig
{
	// Token: 0x06000859 RID: 2137 RVA: 0x00036A84 File Offset: 0x00034C84
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00036A8B File Offset: 0x00034C8B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00036A8D File Offset: 0x00034C8D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00036A90 File Offset: 0x00034C90
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_burger_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedFoodPackageConfig.ID.Name, STRINGS.ITEMS.FOOD.BURGER.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BURGER.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BURGER);
		return gameObject;
	}

	// Token: 0x040005D2 RID: 1490
	public static Tag ID = new Tag("DehydratedFoodPackage");

	// Token: 0x040005D3 RID: 1491
	public const float MASS = 1f;

	// Token: 0x040005D4 RID: 1492
	public const string ANIM_FILE = "dehydrated_food_burger_kanim";

	// Token: 0x040005D5 RID: 1493
	public const string INITIAL_ANIM = "idle";
}

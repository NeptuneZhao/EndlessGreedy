using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class DehydratedCurryConfig : IEntityConfig
{
	// Token: 0x06000884 RID: 2180 RVA: 0x00036F78 File Offset: 0x00035178
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00036F7F File Offset: 0x0003517F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00036F81 File Offset: 0x00035181
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00036F84 File Offset: 0x00035184
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_curry_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedCurryConfig.ID.Name, STRINGS.ITEMS.FOOD.CURRY.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.CURRY.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.CURRY);
		return gameObject;
	}

	// Token: 0x040005E4 RID: 1508
	public static Tag ID = new Tag("DehydratedCurry");

	// Token: 0x040005E5 RID: 1509
	public const float MASS = 1f;

	// Token: 0x040005E6 RID: 1510
	public const string ANIM_FILE = "dehydrated_food_curry_kanim";

	// Token: 0x040005E7 RID: 1511
	public const string INITIAL_ANIM = "idle";
}

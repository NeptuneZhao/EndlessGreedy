using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class DehydratedSpicyTofuConfig : IEntityConfig
{
	// Token: 0x06000944 RID: 2372 RVA: 0x000384FC File Offset: 0x000366FC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00038503 File Offset: 0x00036703
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x00038505 File Offset: 0x00036705
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00038508 File Offset: 0x00036708
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicy_tofu_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSpicyTofuConfig.ID.Name, STRINGS.ITEMS.FOOD.SPICYTOFU.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SPICYTOFU.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICY_TOFU);
		return gameObject;
	}

	// Token: 0x0400062F RID: 1583
	public static Tag ID = new Tag("DehydratedSpicyTofu");

	// Token: 0x04000630 RID: 1584
	public const float MASS = 1f;

	// Token: 0x04000631 RID: 1585
	public const string ANIM_FILE = "dehydrated_food_spicy_tofu_kanim";

	// Token: 0x04000632 RID: 1586
	public const string INITIAL_ANIM = "idle";
}

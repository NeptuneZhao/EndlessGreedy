using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class DehydratedMushroomWrapConfig : IEntityConfig
{
	// Token: 0x060008E2 RID: 2274 RVA: 0x000379FC File Offset: 0x00035BFC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00037A03 File Offset: 0x00035C03
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00037A05 File Offset: 0x00035C05
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00037A08 File Offset: 0x00035C08
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_mushroom_wrap_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedMushroomWrapConfig.ID.Name, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.MUSHROOM_WRAP);
		return gameObject;
	}

	// Token: 0x04000607 RID: 1543
	public static Tag ID = new Tag("DehydratedMushroomWrap");

	// Token: 0x04000608 RID: 1544
	public const float MASS = 1f;

	// Token: 0x04000609 RID: 1545
	public const string ANIM_FILE = "dehydrated_food_mushroom_wrap_kanim";

	// Token: 0x0400060A RID: 1546
	public const string INITIAL_ANIM = "idle";
}

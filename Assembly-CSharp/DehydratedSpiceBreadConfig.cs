using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class DehydratedSpiceBreadConfig : IEntityConfig
{
	// Token: 0x06000933 RID: 2355 RVA: 0x0003832C File Offset: 0x0003652C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00038333 File Offset: 0x00036533
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00038335 File Offset: 0x00036535
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x00038338 File Offset: 0x00036538
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicebread_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSpiceBreadConfig.ID.Name, STRINGS.ITEMS.FOOD.SPICEBREAD.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SPICEBREAD.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICEBREAD);
		return gameObject;
	}

	// Token: 0x04000627 RID: 1575
	public static Tag ID = new Tag("DehydratedSpiceBread");

	// Token: 0x04000628 RID: 1576
	public const float MASS = 1f;

	// Token: 0x04000629 RID: 1577
	public const string ANIM_FILE = "dehydrated_food_spicebread_kanim";

	// Token: 0x0400062A RID: 1578
	public const string INITIAL_ANIM = "idle";
}

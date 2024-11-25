using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class DehydratedSurfAndTurfConfig : IEntityConfig
{
	// Token: 0x0600094F RID: 2383 RVA: 0x00038608 File Offset: 0x00036808
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0003860F File Offset: 0x0003680F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x00038611 File Offset: 0x00036811
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x00038614 File Offset: 0x00036814
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_surf_and_turf_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSurfAndTurfConfig.ID.Name, STRINGS.ITEMS.FOOD.SURFANDTURF.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SURFANDTURF.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SURF_AND_TURF);
		return gameObject;
	}

	// Token: 0x04000635 RID: 1589
	public static Tag ID = new Tag("DehydratedSurfAndTurf");

	// Token: 0x04000636 RID: 1590
	public const float MASS = 1f;

	// Token: 0x04000637 RID: 1591
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x04000638 RID: 1592
	public const string ANIM_FILE = "dehydrated_food_surf_and_turf_kanim";

	// Token: 0x04000639 RID: 1593
	public const string INITIAL_ANIM = "idle";
}

using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class DehydratedBerryPieConfig : IEntityConfig
{
	// Token: 0x0600084E RID: 2126 RVA: 0x00036978 File Offset: 0x00034B78
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0003697F File Offset: 0x00034B7F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00036981 File Offset: 0x00034B81
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00036984 File Offset: 0x00034B84
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_berry_pie_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedBerryPieConfig.ID.Name, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BERRY_PIE);
		return gameObject;
	}

	// Token: 0x040005CC RID: 1484
	public static Tag ID = new Tag("DehydratedBerryPie");

	// Token: 0x040005CD RID: 1485
	public const float MASS = 1f;

	// Token: 0x040005CE RID: 1486
	public const string ANIM_FILE = "dehydrated_food_berry_pie_kanim";

	// Token: 0x040005CF RID: 1487
	public const string INITIAL_ANIM = "idle";
}

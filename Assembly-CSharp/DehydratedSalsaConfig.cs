using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class DehydratedSalsaConfig : IEntityConfig
{
	// Token: 0x06000923 RID: 2339 RVA: 0x000381A4 File Offset: 0x000363A4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x000381AB File Offset: 0x000363AB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x000381AD File Offset: 0x000363AD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x000381B0 File Offset: 0x000363B0
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_salsa_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSalsaConfig.ID.Name, STRINGS.ITEMS.FOOD.SALSA.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SALSA.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SALSA);
		return gameObject;
	}

	// Token: 0x04000620 RID: 1568
	public static Tag ID = new Tag("DehydratedSalsa");

	// Token: 0x04000621 RID: 1569
	public const float MASS = 1f;

	// Token: 0x04000622 RID: 1570
	public const string ANIM_FILE = "dehydrated_food_salsa_kanim";

	// Token: 0x04000623 RID: 1571
	public const string INITIAL_ANIM = "idle";
}

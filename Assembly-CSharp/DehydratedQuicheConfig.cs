using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class DehydratedQuicheConfig : IEntityConfig
{
	// Token: 0x0600090D RID: 2317 RVA: 0x00037F2C File Offset: 0x0003612C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00037F33 File Offset: 0x00036133
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00037F35 File Offset: 0x00036135
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00037F38 File Offset: 0x00036138
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_quiche_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedQuicheConfig.ID.Name, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.QUICHE);
		return gameObject;
	}

	// Token: 0x04000618 RID: 1560
	public static Tag ID = new Tag("DehydratedQuiche");

	// Token: 0x04000619 RID: 1561
	public const float MASS = 1f;

	// Token: 0x0400061A RID: 1562
	public const string ANIM_FILE = "dehydrated_food_quiche_kanim";

	// Token: 0x0400061B RID: 1563
	public const string INITIAL_ANIM = "idle";
}

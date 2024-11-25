using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class BasicPlantFoodConfig : IEntityConfig
{
	// Token: 0x06000844 RID: 2116 RVA: 0x00036883 File Offset: 0x00034A83
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0003688C File Offset: 0x00034A8C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantFood", STRINGS.ITEMS.FOOD.BASICPLANTFOOD.NAME, STRINGS.ITEMS.FOOD.BASICPLANTFOOD.DESC, 1f, false, Assets.GetAnim("meallicegrain_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTFOOD);
		return gameObject;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x000368F2 File Offset: 0x00034AF2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x000368F4 File Offset: 0x00034AF4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005C9 RID: 1481
	public const string ID = "BasicPlantFood";
}

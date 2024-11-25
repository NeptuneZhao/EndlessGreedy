using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BD RID: 445
public class RawEggConfig : IEntityConfig
{
	// Token: 0x06000913 RID: 2323 RVA: 0x00037FBE File Offset: 0x000361BE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00037FC8 File Offset: 0x000361C8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("RawEgg", STRINGS.ITEMS.FOOD.RAWEGG.NAME, STRINGS.ITEMS.FOOD.RAWEGG.DESC, 1f, false, Assets.GetAnim("rawegg_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.RAWEGG);
		TemperatureCookable temperatureCookable = gameObject.AddOrGet<TemperatureCookable>();
		temperatureCookable.cookTemperature = 344.15f;
		temperatureCookable.cookedID = "CookedEgg";
		return gameObject;
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00038049 File Offset: 0x00036249
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0003804B File Offset: 0x0003624B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400061C RID: 1564
	public const string ID = "RawEgg";
}

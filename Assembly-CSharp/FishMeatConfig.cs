using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class FishMeatConfig : IEntityConfig
{
	// Token: 0x060008A3 RID: 2211 RVA: 0x00037264 File Offset: 0x00035464
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0003726C File Offset: 0x0003546C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FishMeat", STRINGS.ITEMS.FOOD.FISHMEAT.NAME, STRINGS.ITEMS.FOOD.FISHMEAT.DESC, 1f, false, Assets.GetAnim("pacufillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FISH_MEAT);
		return gameObject;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x000372D2 File Offset: 0x000354D2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x000372D4 File Offset: 0x000354D4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F1 RID: 1521
	public const string ID = "FishMeat";
}

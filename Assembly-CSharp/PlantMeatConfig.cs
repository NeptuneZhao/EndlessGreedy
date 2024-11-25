using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class PlantMeatConfig : IEntityConfig
{
	// Token: 0x060008FC RID: 2300 RVA: 0x00037CA9 File Offset: 0x00035EA9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x00037CB0 File Offset: 0x00035EB0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PlantMeat", STRINGS.ITEMS.FOOD.PLANTMEAT.NAME, STRINGS.ITEMS.FOOD.PLANTMEAT.DESC, 1f, false, Assets.GetAnim("critter_trap_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.PLANTMEAT);
		return gameObject;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x00037D16 File Offset: 0x00035F16
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00037D18 File Offset: 0x00035F18
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000612 RID: 1554
	public const string ID = "PlantMeat";
}

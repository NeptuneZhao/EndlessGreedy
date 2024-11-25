using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class ShellfishMeatConfig : IEntityConfig
{
	// Token: 0x06000929 RID: 2345 RVA: 0x00038236 File Offset: 0x00036436
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00038240 File Offset: 0x00036440
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("ShellfishMeat", STRINGS.ITEMS.FOOD.SHELLFISHMEAT.NAME, STRINGS.ITEMS.FOOD.SHELLFISHMEAT.DESC, 1f, false, Assets.GetAnim("shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SHELLFISH_MEAT);
		return gameObject;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x000382A6 File Offset: 0x000364A6
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x000382A8 File Offset: 0x000364A8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000624 RID: 1572
	public const string ID = "ShellfishMeat";
}

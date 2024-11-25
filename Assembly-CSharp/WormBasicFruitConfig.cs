using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D0 RID: 464
public class WormBasicFruitConfig : IEntityConfig
{
	// Token: 0x06000978 RID: 2424 RVA: 0x00038A20 File Offset: 0x00036C20
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00038A28 File Offset: 0x00036C28
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFruit", STRINGS.ITEMS.FOOD.WORMBASICFRUIT.NAME, STRINGS.ITEMS.FOOD.WORMBASICFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_basic_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFRUIT);
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00038A8C File Offset: 0x00036C8C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00038A8E File Offset: 0x00036C8E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000647 RID: 1607
	public const string ID = "WormBasicFruit";
}

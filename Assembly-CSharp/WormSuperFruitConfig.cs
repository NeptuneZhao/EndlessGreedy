using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class WormSuperFruitConfig : IEntityConfig
{
	// Token: 0x06000982 RID: 2434 RVA: 0x00038B10 File Offset: 0x00036D10
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00038B18 File Offset: 0x00036D18
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFruit", STRINGS.ITEMS.FOOD.WORMSUPERFRUIT.NAME, STRINGS.ITEMS.FOOD.WORMSUPERFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_super_fruits_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFRUIT);
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00038B7C File Offset: 0x00036D7C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00038B7E File Offset: 0x00036D7E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400064A RID: 1610
	public const string ID = "WormSuperFruit";
}

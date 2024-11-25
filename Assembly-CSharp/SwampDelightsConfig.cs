using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class SwampDelightsConfig : IEntityConfig
{
	// Token: 0x06000955 RID: 2389 RVA: 0x0003869A File Offset: 0x0003689A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x000386A4 File Offset: 0x000368A4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampDelights", STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.NAME, STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.DESC, 1f, false, Assets.GetAnim("swamp_delights_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMP_DELIGHTS);
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x00038708 File Offset: 0x00036908
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0003870A File Offset: 0x0003690A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400063A RID: 1594
	public const string ID = "SwampDelights";
}

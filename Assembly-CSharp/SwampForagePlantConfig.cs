using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class SwampForagePlantConfig : IEntityConfig
{
	// Token: 0x060007F2 RID: 2034 RVA: 0x00034FB4 File Offset: 0x000331B4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x00034FBC File Offset: 0x000331BC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampForagePlant", STRINGS.ITEMS.FOOD.SWAMPFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.SWAMPFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("swamptuber_vegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFORAGEPLANT);
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00035020 File Offset: 0x00033220
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00035022 File Offset: 0x00033222
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400059F RID: 1439
	public const string ID = "SwampForagePlant";
}

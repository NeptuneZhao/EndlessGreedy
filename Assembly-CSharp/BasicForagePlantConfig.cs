using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class BasicForagePlantConfig : IEntityConfig
{
	// Token: 0x060006B5 RID: 1717 RVA: 0x0002CF4A File Offset: 0x0002B14A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x0002CF54 File Offset: 0x0002B154
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicForagePlant", STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("muckrootvegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BASICFORAGEPLANT);
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x0002CFB8 File Offset: 0x0002B1B8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x0002CFBA File Offset: 0x0002B1BA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004BD RID: 1213
	public const string ID = "BasicForagePlant";
}

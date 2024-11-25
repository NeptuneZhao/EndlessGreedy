using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class HardSkinBerryConfig : IEntityConfig
{
	// Token: 0x06000735 RID: 1845 RVA: 0x0003066C File Offset: 0x0002E86C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00030674 File Offset: 0x0002E874
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("HardSkinBerry", STRINGS.ITEMS.FOOD.HARDSKINBERRY.NAME, STRINGS.ITEMS.FOOD.HARDSKINBERRY.DESC, 1f, false, Assets.GetAnim("iceBerry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.HARDSKINBERRY);
		return gameObject;
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x000306DA File Offset: 0x0002E8DA
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x000306DC File Offset: 0x0002E8DC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000527 RID: 1319
	public const string ID = "HardSkinBerry";
}

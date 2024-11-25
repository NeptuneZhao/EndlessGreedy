using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class FriesCarrotConfig : IEntityConfig
{
	// Token: 0x060008B2 RID: 2226 RVA: 0x000373D0 File Offset: 0x000355D0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x000373D8 File Offset: 0x000355D8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriesCarrot", STRINGS.ITEMS.FOOD.FRIESCARROT.NAME, STRINGS.ITEMS.FOOD.FRIESCARROT.DESC, 1f, false, Assets.GetAnim("rootfries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIES_CARROT);
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0003743C File Offset: 0x0003563C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0003743E File Offset: 0x0003563E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F6 RID: 1526
	public const string ID = "FriesCarrot";

	// Token: 0x040005F7 RID: 1527
	public static ComplexRecipe recipe;
}

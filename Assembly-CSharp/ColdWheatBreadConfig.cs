using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class ColdWheatBreadConfig : IEntityConfig
{
	// Token: 0x06000866 RID: 2150 RVA: 0x00036CA7 File Offset: 0x00034EA7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00036CB0 File Offset: 0x00034EB0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ColdWheatBread", STRINGS.ITEMS.FOOD.COLDWHEATBREAD.NAME, STRINGS.ITEMS.FOOD.COLDWHEATBREAD.DESC, 1f, false, Assets.GetAnim("frostbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COLD_WHEAT_BREAD);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00036D14 File Offset: 0x00034F14
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00036D16 File Offset: 0x00034F16
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005D9 RID: 1497
	public const string ID = "ColdWheatBread";

	// Token: 0x040005DA RID: 1498
	public static ComplexRecipe recipe;
}

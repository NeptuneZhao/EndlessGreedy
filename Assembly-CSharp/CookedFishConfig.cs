using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class CookedFishConfig : IEntityConfig
{
	// Token: 0x06000870 RID: 2160 RVA: 0x00036D98 File Offset: 0x00034F98
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00036DA0 File Offset: 0x00034FA0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedFish", STRINGS.ITEMS.FOOD.COOKEDFISH.NAME, STRINGS.ITEMS.FOOD.COOKEDFISH.DESC, 1f, false, Assets.GetAnim("grilled_pacu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_FISH);
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x00036E04 File Offset: 0x00035004
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x00036E06 File Offset: 0x00035006
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005DD RID: 1501
	public const string ID = "CookedFish";

	// Token: 0x040005DE RID: 1502
	public static ComplexRecipe recipe;
}

using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class SpiceBreadConfig : IEntityConfig
{
	// Token: 0x0600092E RID: 2350 RVA: 0x000382B2 File Offset: 0x000364B2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x000382BC File Offset: 0x000364BC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpiceBread", STRINGS.ITEMS.FOOD.SPICEBREAD.NAME, STRINGS.ITEMS.FOOD.SPICEBREAD.DESC, 1f, false, Assets.GetAnim("pepperbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICEBREAD);
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00038320 File Offset: 0x00036520
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00038322 File Offset: 0x00036522
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000625 RID: 1573
	public const string ID = "SpiceBread";

	// Token: 0x04000626 RID: 1574
	public static ComplexRecipe recipe;
}

using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class DeepFriedNoshConfig : IEntityConfig
{
	// Token: 0x06000894 RID: 2196 RVA: 0x000370FC File Offset: 0x000352FC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00037104 File Offset: 0x00035304
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedNosh", STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.DESC, 1f, false, Assets.GetAnim("deepfried_nosh_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_NOSH);
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00037168 File Offset: 0x00035368
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0003716A File Offset: 0x0003536A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005EC RID: 1516
	public const string ID = "DeepFriedNosh";

	// Token: 0x040005ED RID: 1517
	public static ComplexRecipe recipe;
}

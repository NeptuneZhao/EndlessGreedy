using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class FieldRationConfig : IEntityConfig
{
	// Token: 0x0600089E RID: 2206 RVA: 0x000371EC File Offset: 0x000353EC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x000371F4 File Offset: 0x000353F4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FieldRation", STRINGS.ITEMS.FOOD.FIELDRATION.NAME, STRINGS.ITEMS.FOOD.FIELDRATION.DESC, 1f, false, Assets.GetAnim("fieldration_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FIELDRATION);
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00037258 File Offset: 0x00035458
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0003725A File Offset: 0x0003545A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F0 RID: 1520
	public const string ID = "FieldRation";
}

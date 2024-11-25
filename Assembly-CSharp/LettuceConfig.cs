using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class LettuceConfig : IEntityConfig
{
	// Token: 0x060008C6 RID: 2246 RVA: 0x000375D4 File Offset: 0x000357D4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x000375DC File Offset: 0x000357DC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Lettuce", STRINGS.ITEMS.FOOD.LETTUCE.NAME, STRINGS.ITEMS.FOOD.LETTUCE.DESC, 1f, false, Assets.GetAnim("sea_lettuce_leaves_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.LETTUCE);
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00037640 File Offset: 0x00035840
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00037642 File Offset: 0x00035842
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005FE RID: 1534
	public const string ID = "Lettuce";
}

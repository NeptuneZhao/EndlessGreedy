using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001CA RID: 458
public class SwampFruitConfig : IEntityConfig
{
	// Token: 0x0600095A RID: 2394 RVA: 0x00038714 File Offset: 0x00036914
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0003871C File Offset: 0x0003691C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(SwampFruitConfig.ID, STRINGS.ITEMS.FOOD.SWAMPFRUIT.NAME, STRINGS.ITEMS.FOOD.SWAMPFRUIT.DESC, 1f, false, Assets.GetAnim("swampcrop_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 0.72f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFRUIT);
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00038780 File Offset: 0x00036980
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x00038782 File Offset: 0x00036982
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400063B RID: 1595
	public static string ID = "SwampFruit";
}

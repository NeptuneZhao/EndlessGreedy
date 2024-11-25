using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class SwampLilyFlowerConfig : IEntityConfig
{
	// Token: 0x06000960 RID: 2400 RVA: 0x00038798 File Offset: 0x00036998
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x000387A0 File Offset: 0x000369A0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(SwampLilyFlowerConfig.ID, ITEMS.INGREDIENTS.SWAMPLILYFLOWER.NAME, ITEMS.INGREDIENTS.SWAMPLILYFLOWER.DESC, 1f, false, Assets.GetAnim("swamplilyflower_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00038817 File Offset: 0x00036A17
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00038819 File Offset: 0x00036A19
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400063C RID: 1596
	public static float SEEDS_PER_FRUIT = 1f;

	// Token: 0x0400063D RID: 1597
	public static string ID = "SwampLilyFlower";
}

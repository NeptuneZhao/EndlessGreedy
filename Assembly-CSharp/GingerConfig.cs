using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class GingerConfig : IEntityConfig
{
	// Token: 0x0600072F RID: 1839 RVA: 0x000305CD File Offset: 0x0002E7CD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x000305D4 File Offset: 0x0002E7D4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(GingerConfig.ID, STRINGS.ITEMS.INGREDIENTS.GINGER.NAME, STRINGS.ITEMS.INGREDIENTS.GINGER.DESC, 1f, true, Assets.GetAnim("ginger_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.45f, 0.4f, true, TUNING.SORTORDER.BUILDINGELEMENTS + GingerConfig.SORTORDER, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0003064E File Offset: 0x0002E84E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x00030650 File Offset: 0x0002E850
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000525 RID: 1317
	public static string ID = "GingerConfig";

	// Token: 0x04000526 RID: 1318
	public static int SORTORDER = 1;
}

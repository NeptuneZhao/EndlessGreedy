using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class TableSaltConfig : IEntityConfig
{
	// Token: 0x06000968 RID: 2408 RVA: 0x00038869 File Offset: 0x00036A69
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00038870 File Offset: 0x00036A70
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(TableSaltConfig.ID, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC, 1f, false, Assets.GetAnim("seed_saltPlant_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + TableSaltTuning.SORTORDER, SimHashes.Salt, new List<Tag>
		{
			GameTags.Other,
			GameTags.Experimental
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x000388F5 File Offset: 0x00036AF5
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x000388F7 File Offset: 0x00036AF7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000642 RID: 1602
	public static string ID = "TableSalt";
}

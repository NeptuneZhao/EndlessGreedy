using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000226 RID: 550
public class PowerStationToolsConfig : IEntityConfig
{
	// Token: 0x06000B60 RID: 2912 RVA: 0x000432F1 File Offset: 0x000414F1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x000432F8 File Offset: 0x000414F8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PowerStationTools", ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_electrician_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialProduct,
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x00043373 File Offset: 0x00041573
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00043375 File Offset: 0x00041575
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400078C RID: 1932
	public const string ID = "PowerStationTools";

	// Token: 0x0400078D RID: 1933
	public static readonly Tag tag = TagManager.Create("PowerStationTools");

	// Token: 0x0400078E RID: 1934
	public const float MASS = 5f;
}

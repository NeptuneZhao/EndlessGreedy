using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class FarmStationToolsConfig : IEntityConfig
{
	// Token: 0x06000B4E RID: 2894 RVA: 0x000430CD File Offset: 0x000412CD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x000430D4 File Offset: 0x000412D4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FarmStationTools", ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_planttender_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00043144 File Offset: 0x00041344
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x00043146 File Offset: 0x00041346
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000783 RID: 1923
	public const string ID = "FarmStationTools";

	// Token: 0x04000784 RID: 1924
	public static readonly Tag tag = TagManager.Create("FarmStationTools");

	// Token: 0x04000785 RID: 1925
	public const float MASS = 5f;
}

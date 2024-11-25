using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class GasGrassHarvestedConfig : IEntityConfig
{
	// Token: 0x06000F04 RID: 3844 RVA: 0x00056FDC File Offset: 0x000551DC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x00056FE4 File Offset: 0x000551E4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GasGrassHarvested", CREATURES.SPECIES.GASGRASS.NAME, CREATURES.SPECIES.GASGRASS.DESC, 1f, false, Assets.GetAnim("harvested_gassygrass_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.Other
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x00057054 File Offset: 0x00055254
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x00057056 File Offset: 0x00055256
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000933 RID: 2355
	public const string ID = "GasGrassHarvested";
}

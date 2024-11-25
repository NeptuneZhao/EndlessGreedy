using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000227 RID: 551
public class ResearchDatabankConfig : IEntityConfig
{
	// Token: 0x06000B66 RID: 2918 RVA: 0x00043390 File Offset: 0x00041590
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00043398 File Offset: 0x00041598
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("ResearchDatabank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.DESC, 1f, true, Assets.GetAnim("floppy_disc_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Experimental
		});
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			gameObject.AddTag(GameTags.HideFromSpawnTool);
		}
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = (float)ROCKETRY.DESTINATION_RESEARCH.BASIC;
		return gameObject;
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x00043431 File Offset: 0x00041631
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00043434 File Offset: 0x00041634
	public void OnSpawn(GameObject inst)
	{
		if (SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC2_ID") && SaveLoader.Instance.ClusterLayout != null && SaveLoader.Instance.ClusterLayout.clusterTags.Contains("CeresCluster"))
		{
			inst.AddOrGet<KBatchedAnimController>().SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("floppy_disc_ceres_kanim")
			});
		}
	}

	// Token: 0x0400078F RID: 1935
	public const string ID = "ResearchDatabank";

	// Token: 0x04000790 RID: 1936
	public static readonly Tag TAG = TagManager.Create("ResearchDatabank");

	// Token: 0x04000791 RID: 1937
	public const float MASS = 1f;
}

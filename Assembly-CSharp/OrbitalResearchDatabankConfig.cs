using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000225 RID: 549
public class OrbitalResearchDatabankConfig : IEntityConfig
{
	// Token: 0x06000B5A RID: 2906 RVA: 0x000431DF File Offset: 0x000413DF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x000431E8 File Offset: 0x000413E8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("OrbitalResearchDatabank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.DESC, 1f, true, Assets.GetAnim("floppy_disc_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Experimental
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = (float)ROCKETRY.DESTINATION_RESEARCH.BASIC;
		return gameObject;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0004326D File Offset: 0x0004146D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x00043270 File Offset: 0x00041470
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

	// Token: 0x04000789 RID: 1929
	public const string ID = "OrbitalResearchDatabank";

	// Token: 0x0400078A RID: 1930
	public static readonly Tag TAG = TagManager.Create("OrbitalResearchDatabank");

	// Token: 0x0400078B RID: 1931
	public const float MASS = 1f;
}

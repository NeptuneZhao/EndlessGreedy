using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class GeneShufflerRechargeConfig : IEntityConfig
{
	// Token: 0x06000F09 RID: 3849 RVA: 0x00057060 File Offset: 0x00055260
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x00057068 File Offset: 0x00055268
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("GeneShufflerRecharge", ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME, ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.DESC, 5f, true, Assets.GetAnim("vacillator_charge_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x000570D1 File Offset: 0x000552D1
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x000570D3 File Offset: 0x000552D3
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000934 RID: 2356
	public const string ID = "GeneShufflerRecharge";

	// Token: 0x04000935 RID: 2357
	public static readonly Tag tag = TagManager.Create("GeneShufflerRecharge");

	// Token: 0x04000936 RID: 2358
	public const float MASS = 5f;
}

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class HeatCubeConfig : IEntityConfig
{
	// Token: 0x06000F1B RID: 3867 RVA: 0x00057FAB File Offset: 0x000561AB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x00057FB4 File Offset: 0x000561B4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("HeatCube", "Heat Cube", "A cube that holds heat.", 1000f, true, Assets.GetAnim("copper_kanim"), "idle_tallstone", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, SORTORDER.BUILDINGELEMENTS, SimHashes.Diamond, new List<Tag>
		{
			GameTags.MiscPickupable,
			GameTags.IndustrialIngredient
		});
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x00058022 File Offset: 0x00056222
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x00058024 File Offset: 0x00056224
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000953 RID: 2387
	public const string ID = "HeatCube";
}

using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class CrabShellConfig : IEntityConfig
{
	// Token: 0x06000B36 RID: 2870 RVA: 0x00042DEF File Offset: 0x00040FEF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x00042DF8 File Offset: 0x00040FF8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.DESC, 10f, true, Assets.GetAnim("crabshells_large_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x00042E81 File Offset: 0x00041081
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x00042E83 File Offset: 0x00041083
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000773 RID: 1907
	public const string ID = "CrabShell";

	// Token: 0x04000774 RID: 1908
	public static readonly Tag TAG = TagManager.Create("CrabShell");

	// Token: 0x04000775 RID: 1909
	public const float MASS = 10f;
}

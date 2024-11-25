using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class BabyCrabShellConfig : IEntityConfig
{
	// Token: 0x06000B2A RID: 2858 RVA: 0x00042C5D File Offset: 0x00040E5D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x00042C64 File Offset: 0x00040E64
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BabyCrabShell", ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.DESC, 5f, true, Assets.GetAnim("crabshells_small_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00042CED File Offset: 0x00040EED
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x00042CEF File Offset: 0x00040EEF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400076C RID: 1900
	public const string ID = "BabyCrabShell";

	// Token: 0x0400076D RID: 1901
	public static readonly Tag TAG = TagManager.Create("BabyCrabShell");

	// Token: 0x0400076E RID: 1902
	public const float MASS = 5f;
}

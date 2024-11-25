using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200021E RID: 542
public class BabyCrabWoodShellConfig : IEntityConfig
{
	// Token: 0x06000B30 RID: 2864 RVA: 0x00042D0A File Offset: 0x00040F0A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x00042D14 File Offset: 0x00040F14
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BabyCrabWoodShell", ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.VARIANT_WOOD.NAME, ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.VARIANT_WOOD.DESC, 10f, true, Assets.GetAnim("crabshells_small_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics,
			GameTags.MoltShell
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>().symbolPrefix = "wood_";
		SymbolOverrideControllerUtil.AddToPrefab(gameObject).ApplySymbolOverridesByAffix(Assets.GetAnim("crabshells_small_kanim"), "wood_", null, 0);
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x00042DD2 File Offset: 0x00040FD2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00042DD4 File Offset: 0x00040FD4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400076F RID: 1903
	public const string ID = "BabyCrabWoodShell";

	// Token: 0x04000770 RID: 1904
	public static readonly Tag TAG = TagManager.Create("BabyCrabWoodShell");

	// Token: 0x04000771 RID: 1905
	public const float MASS = 10f;

	// Token: 0x04000772 RID: 1906
	public const string symbolPrefix = "wood_";
}

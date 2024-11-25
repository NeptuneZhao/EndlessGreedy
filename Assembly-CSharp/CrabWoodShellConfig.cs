using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000220 RID: 544
public class CrabWoodShellConfig : IEntityConfig
{
	// Token: 0x06000B3C RID: 2876 RVA: 0x00042E9E File Offset: 0x0004109E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x00042EA8 File Offset: 0x000410A8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabWoodShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.DESC, 100f, true, Assets.GetAnim("crabshells_large_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics,
			GameTags.MoltShell
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>().symbolPrefix = "wood_";
		SymbolOverrideControllerUtil.AddToPrefab(gameObject).ApplySymbolOverridesByAffix(Assets.GetAnim("crabshells_large_kanim"), "wood_", null, 0);
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x00042F66 File Offset: 0x00041166
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x00042F68 File Offset: 0x00041168
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000776 RID: 1910
	public const string ID = "CrabWoodShell";

	// Token: 0x04000777 RID: 1911
	public static readonly Tag TAG = TagManager.Create("CrabWoodShell");

	// Token: 0x04000778 RID: 1912
	public const float MASS = 100f;

	// Token: 0x04000779 RID: 1913
	public const string symbolPrefix = "wood_";
}

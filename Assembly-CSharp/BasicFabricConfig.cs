using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class BasicFabricConfig : IEntityConfig
{
	// Token: 0x060006A9 RID: 1705 RVA: 0x0002CC5E File Offset: 0x0002AE5E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0002CC68 File Offset: 0x0002AE68
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(BasicFabricConfig.ID, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.DESC, 1f, true, Assets.GetAnim("swampreedwool_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + BasicFabricTuning.SORTORDER, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.BuildingFiber
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(this.decorModifier);
		return gameObject;
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0002CCFE File Offset: 0x0002AEFE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x0002CD00 File Offset: 0x0002AF00
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004B8 RID: 1208
	public static string ID = "BasicFabric";

	// Token: 0x040004B9 RID: 1209
	private AttributeModifier decorModifier = new AttributeModifier("Decor", 0.1f, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, true, false, true);
}

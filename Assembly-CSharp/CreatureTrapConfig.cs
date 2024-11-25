using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class CreatureTrapConfig : IBuildingConfig
{
	// Token: 0x060001BB RID: 443 RVA: 0x0000CAE8 File Offset: 0x0000ACE8
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureTrap", 2, 1, "creaturetrap_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		return buildingDef;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000CB4C File Offset: 0x0000AD4C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(CreatureTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Walker,
			GameTags.Creatures.Hoverer
		};
		trapTrigger.trappedOffset = new Vector2(0.5f, 0f);
		go.AddOrGet<Trap>();
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000CBBE File Offset: 0x0000ADBE
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000117 RID: 279
	public const string ID = "CreatureTrap";

	// Token: 0x04000118 RID: 280
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}

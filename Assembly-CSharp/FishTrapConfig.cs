using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class FishTrapConfig : IBuildingConfig
{
	// Token: 0x06000699 RID: 1689 RVA: 0x0002C810 File Offset: 0x0002AA10
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishTrap", 1, 2, "fishtrap_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0002C870 File Offset: 0x0002AA70
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(FishTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Swimmer
		};
		trapTrigger.trappedOffset = new Vector2(0f, 1f);
		go.AddOrGet<Trap>();
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0002C8D8 File Offset: 0x0002AAD8
	public override void DoPostConfigureComplete(GameObject go)
	{
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		def.initialLures = new Tag[]
		{
			GameTags.Creatures.FishTrapLure
		};
	}

	// Token: 0x040004B3 RID: 1203
	public const string ID = "FishTrap";

	// Token: 0x040004B4 RID: 1204
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}

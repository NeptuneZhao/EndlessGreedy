using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class CreatureFeederConfig : IBuildingConfig
{
	// Token: 0x060001B5 RID: 437 RVA: 0x0000C91C File Offset: 0x0000AB1C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CreatureFeeder";
		int width = 1;
		int height = 2;
		string anim = "feeder_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000C96D File Offset: 0x0000AB6D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000C970 File Offset: 0x0000AB70
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 2000f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<StorageLocker>().choreTypeID = Db.Get().ChoreTypes.RanchingFetch.Id;
		go.AddOrGet<UserNameable>();
		go.AddOrGet<TreeFilterable>();
		go.AddOrGet<CreatureFeeder>();
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000C9F1 File Offset: 0x0000ABF1
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000C9FC File Offset: 0x0000ABFC
	public override void ConfigurePost(BuildingDef def)
	{
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, Diet> keyValuePair in DietManager.CollectDiets(new Tag[]
		{
			GameTags.Creatures.Species.LightBugSpecies,
			GameTags.Creatures.Species.HatchSpecies,
			GameTags.Creatures.Species.MoleSpecies,
			GameTags.Creatures.Species.CrabSpecies,
			GameTags.Creatures.Species.StaterpillarSpecies,
			GameTags.Creatures.Species.DivergentSpecies,
			GameTags.Creatures.Species.DeerSpecies,
			GameTags.Creatures.Species.BellySpecies,
			GameTags.Creatures.Species.SealSpecies
		}))
		{
			list.Add(keyValuePair.Key);
		}
		def.BuildingComplete.GetComponent<Storage>().storageFilters = list;
	}

	// Token: 0x04000116 RID: 278
	public const string ID = "CreatureFeeder";
}

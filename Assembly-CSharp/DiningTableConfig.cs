using System;
using TUNING;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class DiningTableConfig : IBuildingConfig
{
	// Token: 0x0600021E RID: 542 RVA: 0x0000EC74 File Offset: 0x0000CE74
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DiningTable";
		int width = 1;
		int height = 1;
		string anim = "diningtable_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.WorkTime = 20f;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000ECD7 File Offset: 0x0000CED7
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.MessTable, false);
		go.AddOrGet<MessStation>();
		go.AddOrGet<AnimTileable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000ED08 File Offset: 0x0000CF08
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.MessStation.Id;
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = TableSaltTuning.SALTSHAKERSTORAGEMASS;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = TableSaltConfig.ID.ToTag();
		manualDeliveryKG.capacity = TableSaltTuning.SALTSHAKERSTORAGEMASS;
		manualDeliveryKG.refillMass = TableSaltTuning.CONSUMABLE_RATE;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FoodFetch.IdHash;
		manualDeliveryKG.ShowStatusItem = false;
	}

	// Token: 0x04000159 RID: 345
	public const string ID = "DiningTable";
}

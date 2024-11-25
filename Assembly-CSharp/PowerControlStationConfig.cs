using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class PowerControlStationConfig : IBuildingConfig
{
	// Token: 0x06001125 RID: 4389 RVA: 0x000607A4 File Offset: 0x0005E9A4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PowerControlStation";
		int width = 2;
		int height = 4;
		string anim = "electricianworkdesk_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x00060824 File Offset: 0x0005EA24
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x00060850 File Offset: 0x0005EA50
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 50f;
		storage.showInUI = true;
		storage.storageFilters = new List<Tag>
		{
			PowerControlStationConfig.MATERIAL_FOR_TINKER
		};
		TinkerStation tinkerstation = go.AddOrGet<TinkerStation>();
		tinkerstation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_electricianworkdesk_kanim")
		};
		tinkerstation.inputMaterial = PowerControlStationConfig.MATERIAL_FOR_TINKER;
		tinkerstation.massPerTinker = 5f;
		tinkerstation.outputPrefab = PowerControlStationConfig.TINKER_TOOLS;
		tinkerstation.outputTemperature = 308.15f;
		tinkerstation.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerstation.choreType = Db.Get().ChoreTypes.PowerFabricate.IdHash;
		tinkerstation.useFilteredStorage = true;
		tinkerstation.fetchChoreType = Db.Get().ChoreTypes.PowerFetch.IdHash;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			TinkerStation component = game_object.GetComponent<TinkerStation>();
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			tinkerstation.SetWorkTime(160f);
		};
	}

	// Token: 0x04000A7B RID: 2683
	public const string ID = "PowerControlStation";

	// Token: 0x04000A7C RID: 2684
	public static Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;

	// Token: 0x04000A7D RID: 2685
	public static Tag TINKER_TOOLS = PowerStationToolsConfig.tag;

	// Token: 0x04000A7E RID: 2686
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x04000A7F RID: 2687
	public static string ROLE_PERK = "CanPowerTinker";

	// Token: 0x04000A80 RID: 2688
	public const float OUTPUT_TEMPERATURE = 308.15f;
}

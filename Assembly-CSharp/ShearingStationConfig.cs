﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020003AB RID: 939
public class ShearingStationConfig : IBuildingConfig
{
	// Token: 0x06001381 RID: 4993 RVA: 0x0006BA00 File Offset: 0x00069C00
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ShearingStation";
		int width = 3;
		int height = 3;
		string anim = "shearing_station_kanim";
		int hitpoints = 100;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.Floodable = true;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.DefaultAnimState = "on";
		buildingDef.ShowInBuildMenu = true;
		return buildingDef;
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0006BAC0 File Offset: 0x00069CC0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0006BB18 File Offset: 0x00069D18
	public override void DoPostConfigureComplete(GameObject go)
	{
		RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
		def.IsCritterEligibleToBeRanchedCb = delegate(GameObject creature_go, RanchStation.Instance ranch_station_smi)
		{
			IShearable smi = creature_go.GetSMI<IShearable>();
			return smi != null && smi.IsFullyGrown();
		};
		def.OnRanchCompleteCb = delegate(GameObject creature_go)
		{
			creature_go.GetSMI<IShearable>().Shear();
		};
		def.RancherInteractAnim = "anim_interacts_shearingstation_kanim";
		def.WorkTime = 12f;
		def.RanchedPreAnim = "shearing_pre";
		def.RanchedLoopAnim = "shearing_loop";
		def.RanchedPstAnim = "shearing_pst";
		go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanUseRanchStation.Id;
		Prioritizable.AddRef(go);
	}

	// Token: 0x04000B38 RID: 2872
	public const string ID = "ShearingStation";
}

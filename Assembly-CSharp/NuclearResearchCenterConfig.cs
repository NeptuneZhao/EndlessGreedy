using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class NuclearResearchCenterConfig : IBuildingConfig
{
	// Token: 0x0600105D RID: 4189 RVA: 0x0005C8A7 File Offset: 0x0005AAA7
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0005C8B0 File Offset: 0x0005AAB0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "NuclearResearchCenter";
		int width = 5;
		int height = 3;
		string anim = "material_research_centre_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(-2, 1);
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "NuclearResearchCenter");
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("HEP_STORAGE", new CellOffset(2, 2), STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x0005C9B8 File Offset: 0x0005ABB8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.capacity = 100f;
		highEnergyParticleStorage.PORT_ID = "HEP_STORAGE";
		highEnergyParticleStorage.showCapacityStatusItem = true;
		NuclearResearchCenterWorkable nuclearResearchCenterWorkable = go.AddOrGet<NuclearResearchCenterWorkable>();
		nuclearResearchCenterWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_material_research_centre_kanim")
		};
		nuclearResearchCenterWorkable.requiredSkillPerk = Db.Get().SkillPerks.AllowNuclearResearch.Id;
		NuclearResearchCenter nuclearResearchCenter = go.AddOrGet<NuclearResearchCenter>();
		nuclearResearchCenter.researchTypeID = "nuclear";
		nuclearResearchCenter.materialPerPoint = 10f;
		nuclearResearchCenter.timePerPoint = 100f;
		nuclearResearchCenter.inputMaterial = NuclearResearchCenterConfig.INPUT_MATERIAL;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0005CA88 File Offset: 0x0005AC88
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A0F RID: 2575
	public const string ID = "NuclearResearchCenter";

	// Token: 0x04000A10 RID: 2576
	public const string PORT_ID = "HEP_STORAGE";

	// Token: 0x04000A11 RID: 2577
	public const float BASE_TIME_PER_POINT = 100f;

	// Token: 0x04000A12 RID: 2578
	public const float PARTICLES_PER_POINT = 10f;

	// Token: 0x04000A13 RID: 2579
	public const float CAPACITY = 100f;

	// Token: 0x04000A14 RID: 2580
	public static readonly Tag INPUT_MATERIAL = GameTags.HighEnergyParticle;
}

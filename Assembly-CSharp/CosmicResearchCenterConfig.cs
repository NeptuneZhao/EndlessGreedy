using System;
using TUNING;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class CosmicResearchCenterConfig : IBuildingConfig
{
	// Token: 0x060001A5 RID: 421 RVA: 0x0000BB30 File Offset: 0x00009D30
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000BB38 File Offset: 0x00009D38
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CosmicResearchCenter";
		int width = 4;
		int height = 4;
		string anim = "research_space_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000BBC8 File Offset: 0x00009DC8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = CosmicResearchCenterConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 3f;
		manualDeliveryKG.capacity = 300f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
		researchCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_space_kanim")
		};
		researchCenter.research_point_type_id = "space";
		researchCenter.inputMaterial = CosmicResearchCenterConfig.INPUT_MATERIAL;
		researchCenter.mass_per_point = 1f;
		researchCenter.requiredSkillPerk = Db.Get().SkillPerks.AllowInterstellarResearch.Id;
		researchCenter.workLayer = Grid.SceneLayer.BuildingFront;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(CosmicResearchCenterConfig.INPUT_MATERIAL, 0.02f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000BCF8 File Offset: 0x00009EF8
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400010E RID: 270
	public const string ID = "CosmicResearchCenter";

	// Token: 0x0400010F RID: 271
	public const float BASE_SECONDS_PER_POINT = 50f;

	// Token: 0x04000110 RID: 272
	public const float MASS_PER_POINT = 1f;

	// Token: 0x04000111 RID: 273
	public const float BASE_MASS_PER_SECOND = 0.02f;

	// Token: 0x04000112 RID: 274
	public const float CAPACITY = 300f;

	// Token: 0x04000113 RID: 275
	public static readonly Tag INPUT_MATERIAL = ResearchDatabankConfig.TAG;
}

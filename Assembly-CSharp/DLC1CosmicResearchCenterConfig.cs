using System;
using TUNING;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class DLC1CosmicResearchCenterConfig : IBuildingConfig
{
	// Token: 0x060001D6 RID: 470 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000D2BC File Offset: 0x0000B4BC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DLC1CosmicResearchCenter";
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

	// Token: 0x060001D8 RID: 472 RVA: 0x0000D34C File Offset: 0x0000B54C
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
		manualDeliveryKG.RequestedItemTag = DLC1CosmicResearchCenterConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 3f;
		manualDeliveryKG.capacity = 300f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
		researchCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_space_kanim")
		};
		researchCenter.research_point_type_id = "orbital";
		researchCenter.inputMaterial = DLC1CosmicResearchCenterConfig.INPUT_MATERIAL;
		researchCenter.mass_per_point = 1f;
		researchCenter.requiredSkillPerk = Db.Get().SkillPerks.AllowOrbitalResearch.Id;
		researchCenter.workLayer = Grid.SceneLayer.BuildingFront;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(DLC1CosmicResearchCenterConfig.INPUT_MATERIAL, 0.02f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000D47C File Offset: 0x0000B67C
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000121 RID: 289
	public const string ID = "DLC1CosmicResearchCenter";

	// Token: 0x04000122 RID: 290
	public const float BASE_SECONDS_PER_POINT = 50f;

	// Token: 0x04000123 RID: 291
	public const float MASS_PER_POINT = 1f;

	// Token: 0x04000124 RID: 292
	public const float BASE_MASS_PER_SECOND = 0.02f;

	// Token: 0x04000125 RID: 293
	public const float CAPACITY = 300f;

	// Token: 0x04000126 RID: 294
	public static readonly Tag INPUT_MATERIAL = OrbitalResearchDatabankConfig.TAG;
}

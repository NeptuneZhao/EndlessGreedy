using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000311 RID: 785
public class OrbitalResearchCenterConfig : IBuildingConfig
{
	// Token: 0x0600107A RID: 4218 RVA: 0x0005D3AA File Offset: 0x0005B5AA
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x0005D3B4 File Offset: 0x0005B5B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OrbitalResearchCenter";
		int width = 2;
		int height = 3;
		string anim = "orbital_research_station_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x0005D444 File Offset: 0x0005B644
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddOrGet<InOrbitRequired>();
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<Prioritizable>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 308.15f;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_orbital_research_station_kanim")
		};
		Prioritizable.AddRef(go);
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		this.ConfigureRecipes();
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x0005D4E8 File Offset: 0x0005B6E8
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(OrbitalResearchCenterConfig.INPUT_MATERIAL, 5f, true)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("OrbitalResearchDatabank".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("OrbitalResearchCenter", array, array2), array, array2)
		{
			time = 33f,
			description = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"OrbitalResearchCenter"
			}
		};
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x0005D584 File Offset: 0x0005B784
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A2B RID: 2603
	public const string ID = "OrbitalResearchCenter";

	// Token: 0x04000A2C RID: 2604
	public const float BASE_SECONDS_PER_POINT = 33f;

	// Token: 0x04000A2D RID: 2605
	public const float MASS_PER_POINT = 5f;

	// Token: 0x04000A2E RID: 2606
	public static readonly Tag INPUT_MATERIAL = SimHashes.Polypropylene.CreateTag();

	// Token: 0x04000A2F RID: 2607
	public const float OUTPUT_TEMPERATURE = 308.15f;
}

﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class AdvancedCraftingTableConfig : IBuildingConfig
{
	// Token: 0x06000047 RID: 71 RVA: 0x00003C16 File Offset: 0x00001E16
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003C20 File Offset: 0x00001E20
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AdvancedCraftingTable";
		int width = 3;
		int height = 3;
		string anim = "advanced_crafting_table_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		return buildingDef;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00003C9C File Offset: 0x00001E9C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<Prioritizable>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 318.15f;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_advanced_crafting_table_kanim")
		};
		Prioritizable.AddRef(go);
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		this.ConfigureRecipes();
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003D14 File Offset: 0x00001F14
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 200f, true)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FetchDrone".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ElectrobankConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedCraftingTable", array, array2), array, array2)
		{
			time = INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 4f,
			description = "_description",
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"AdvancedCraftingTable"
			}
		};
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003DB6 File Offset: 0x00001FB6
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			component.requiredSkillPerk = Db.Get().SkillPerks.CanCraftElectronics.Id;
		};
	}

	// Token: 0x04000040 RID: 64
	public const string ID = "AdvancedCraftingTable";
}

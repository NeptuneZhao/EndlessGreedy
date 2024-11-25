using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class FossilDigSiteConfig : IBuildingConfig
{
	// Token: 0x060009B6 RID: 2486 RVA: 0x00039817 File Offset: 0x00037A17
	public static string GetBodyContentForFossil(int id)
	{
		return CODEX.STORY_TRAITS.FOSSILHUNT.DNADATA_ENTRY.TELEPORTFAILURE;
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00039824 File Offset: 0x00037A24
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FossilDig";
		int width = 5;
		int height = 3;
		string anim = "fossil_dig_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
		string[] construction_materials = new string[]
		{
			SimHashes.Fossil.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.Entombable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Overheatable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "medium";
		buildingDef.UseStructureTemperature = false;
		return buildingDef;
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x000398CC File Offset: 0x00037ACC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		Prioritizable.AddRef(go);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil, true);
		component.Temperature = 315f;
		go.AddOrGetDef<MajorFossilDigSite.Def>().questCriteria = FossilDigSiteConfig.QUEST_CRITERIA;
		go.AddOrGetDef<FossilHuntInitializer.Def>().IsMainDigsite = true;
		go.AddOrGet<MajorDigSiteWorkable>();
		go.AddOrGet<Operational>();
		go.AddOrGet<EntombVulnerable>();
		go.AddOrGet<FossilMineWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fossil_dig_kanim")
		};
		FossilMine fossilMine = go.AddOrGet<FossilMine>();
		fossilMine.heatedTemperature = 0f;
		fossilMine.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, fossilMine);
		go.AddOrGet<Demolishable>().allowDemolition = false;
		FossilDigsiteLampLight fossilDigsiteLampLight = go.AddOrGet<FossilDigsiteLampLight>();
		fossilDigsiteLampLight.Color = Color.yellow;
		fossilDigsiteLampLight.overlayColour = LIGHT2D.WALLLIGHT_COLOR;
		fossilDigsiteLampLight.Range = 3f;
		fossilDigsiteLampLight.Angle = 0f;
		fossilDigsiteLampLight.Direction = LIGHT2D.DEFAULT_DIRECTION;
		fossilDigsiteLampLight.Offset = LIGHT2D.MAJORFOSSILDIGSITE_LAMP_OFFSET;
		fossilDigsiteLampLight.shape = global::LightShape.Circle;
		fossilDigsiteLampLight.drawOverlay = true;
		fossilDigsiteLampLight.Lux = 1000;
		fossilDigsiteLampLight.enabled = false;
		this.ConfigureRecipes();
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00039A0D File Offset: 0x00037C0D
	public override void DoPostConfigureComplete(GameObject go)
	{
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		component.defaultAnim = "covered";
		component.initialAnim = "covered";
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00039A38 File Offset: 0x00037C38
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Fossil.CreateTag(), 100f)
		};
		ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("FossilDig", array, array2), array, array2);
		complexRecipe.time = 80f;
		complexRecipe.description = CODEX.STORY_TRAITS.FOSSILHUNT.REWARDS.MINED_FOSSIL.DESC;
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe.fabricators = new List<Tag>
		{
			"FossilDig"
		};
		complexRecipe.sortOrder = 21;
	}

	// Token: 0x04000669 RID: 1641
	public static int DiscoveredDigsitesRequired = 4;

	// Token: 0x0400066A RID: 1642
	public static HashedString hashID = new HashedString("FossilDig");

	// Token: 0x0400066B RID: 1643
	public const string ID = "FossilDig";

	// Token: 0x0400066C RID: 1644
	public static readonly HashedString QUEST_CRITERIA = "LostSpecimen";

	// Token: 0x0400066D RID: 1645
	public const string CODEX_ENTRY_ID = "STORYTRAITFOSSILHUNT";

	// Token: 0x020010DE RID: 4318
	public static class FOSSIL_HUNT_LORE_UNLOCK_ID
	{
		// Token: 0x06007D54 RID: 32084 RVA: 0x00308009 File Offset: 0x00306209
		public static string For(int id)
		{
			return string.Format("story_trait_fossilhunt_poi{0}", Mathf.Clamp(id, 1, FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.popupsAvailablesForSmallSites));
		}

		// Token: 0x04005E3C RID: 24124
		public static int popupsAvailablesForSmallSites = 3;
	}
}

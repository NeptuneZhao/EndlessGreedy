using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

[HarmonyPatch(typeof(GlassForgeConfig))]
public class SuperGlassForge
{
	private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};

	public static readonly List<Storage.StoredItemModifier> OutputItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate
	};

	[HarmonyPostfix]
	[HarmonyPatch("CreateBuildingDef")]
	public static void Postfix(ref BuildingDef __result)
	{
		__result.SelfHeatKilowattsWhenActive = -200f;
	}

	[HarmonyPrefix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static bool Prefix(GameObject go)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

		GlassForge glassForge = go.AddOrGet<GlassForge>();
		glassForge.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();

		ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
		glassForge.duplicantOperated = true;
		BuildingTemplates.CreateComplexFabricatorStorage(go, glassForge);

		glassForge.outStorage.capacityKg = 2000f;
		glassForge.storeProduced = true;
		glassForge.inStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
		glassForge.buildStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
		glassForge.outStorage.SetDefaultStoredItemModifiers(OutputItemModifiers);
		glassForge.outputOffset = new Vector3(1f, 0.5f);

		complexFabricatorWorkable.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_metalrefinery_kanim") };
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.storage = glassForge.outStorage;
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = null;
		conduitDispenser.alwaysDispense = true;

		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Sand).tag, 100f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Glass).tag, 100f)
		};

		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("GlassForge", array[0].material);
		string text = ComplexRecipeManager.MakeRecipeID("GlassForge", array, array2);
		new ComplexRecipe(text, array, array2)
		{
			time = 15f,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.GLASSFORGE.RECIPE_DESCRIPTION, ElementLoader.GetElement(array2[0].material).name, ElementLoader.GetElement(array[0].material).name),
			fabricators = new List<Tag> { TagManager.Create("GlassForge") }
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		Prioritizable.AddRef(go);

		return false;
	}
}
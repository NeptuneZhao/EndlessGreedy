using HarmonyLib;
using System.Collections.Generic;
using TUNING;

/// <summary>
/// 食物压制器
/// </summary>
[HarmonyPatch(typeof(MicrobeMusherConfig))]
internal class SuperMicrobeMusher
{
	[HarmonyPrefix]
	[HarmonyPatch("ConfigureRecipes")]
	public static bool Prefix()
	{
		// 软泥膏
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("MushBar".ToTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		MushBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array, array2), array, array2)
		{
			time = 5f,
			description = STRINGS.ITEMS.FOOD.MUSHBAR.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "MicrobeMusher" },
			sortOrder = 1
		};

		// 米虱糕
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("BasicPlantBar".ToTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		BasicPlantBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array3, array4), array3, array4)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.BASICPLANTBAR.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "MicrobeMusher" },
			sortOrder = 2
		};

		// 豆腐
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Tofu".ToTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		TofuConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array5, array6), array5, array6)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.TOFU.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "MicrobeMusher" },
			sortOrder = 3
		};

		// 浆果糕
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("FruitCake".ToTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		FruitCakeConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array7, array8), array7, array8)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.FRUITCAKE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "MicrobeMusher" },
			sortOrder = 3
		};

		// 干肉饼
		ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Pemmican".ToTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		PemmicanConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", array9, array10), array9, array10, DlcManager.AVAILABLE_DLC_2)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.PEMMICAN.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "MicrobeMusher" },
			sortOrder = 4
		};

		return false;
	}
}

/// <summary>
/// 电动烤炉
/// </summary>
[HarmonyPatch(typeof(CookingStationConfig))]
internal class SuperCookingStation
{
	[HarmonyPrefix, HarmonyPatch("ConfigureRecipes")]
	public static bool Prefix()
	{
		// 腌制米虱
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("PickledMeal", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		PickledMealConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array, array2), array, array2)
		{
			time = FOOD.RECIPES.SMALL_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.PICKLEDMEAL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 21
		};

		// 煎泥膏
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("FriedMushBar".ToTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		FriedMushBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array3, array4), array3, array4)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 1
		};

		// 煎蘑菇
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("FriedMushroom", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		FriedMushroomConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array5, array6), array5, array6)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 20
		};

		// 蛋奶酥煎饼
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Pancakes", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array7, array8), array7, array8)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.PANCAKES.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 20
		};

		// 烤肉串
		ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("CookedMeat", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array9, array10), array9, array10)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 21
		};

		// 烤海鲜
		ComplexRecipe.RecipeElement[] array11 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array12 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("CookedFish", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array11, array12), array11, array12)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 22
		};

		// 炙烤刺果
		ComplexRecipe.RecipeElement[] array15 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array16 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("GrilledPrickleFruit", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		GrilledPrickleFruitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array15, array16), array15, array16)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 20
		};

		if (DlcManager.IsExpansion1Active())
		{
			ComplexRecipe.RecipeElement[] array17 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
			};
			ComplexRecipe.RecipeElement[] array18 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement("SwampDelights", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
			};
			CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array17, array18), array17, array18)
			{
				time = FOOD.RECIPES.STANDARD_COOK_TIME,
				description = STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.RECIPEDESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag> { "CookingStation" },
				sortOrder = 20
			};
		}

		// 冰霜面包
		ComplexRecipe.RecipeElement[] array19 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array20 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("ColdWheatBread", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		ColdWheatBreadConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array19, array20), array19, array20)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.COLDWHEATBREAD.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 50
		};

		// 煎蛋卷
		ComplexRecipe.RecipeElement[] array21 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array22 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("CookedEgg", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array21, array22), array21, array22)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.COOKEDEGG.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 1
		};

		if (DlcManager.IsExpansion1Active())
		{
			ComplexRecipe.RecipeElement[] array23 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
			};
			ComplexRecipe.RecipeElement[] array24 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement("WormBasicFood", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
			};
			WormBasicFoodConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array23, array24), array23, array24)
			{
				time = FOOD.RECIPES.STANDARD_COOK_TIME,
				description = STRINGS.ITEMS.FOOD.WORMBASICFOOD.RECIPEDESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag> { "CookingStation" },
				sortOrder = 20
			};
		}

		if (DlcManager.IsExpansion1Active())
		{
			ComplexRecipe.RecipeElement[] array25 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
			};
			ComplexRecipe.RecipeElement[] array26 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement("WormSuperFood", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
			};
			WormSuperFoodConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array25, array26), array25, array26)
			{
				time = FOOD.RECIPES.STANDARD_COOK_TIME,
				description = STRINGS.ITEMS.FOOD.WORMSUPERFOOD.RECIPEDESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag> { "CookingStation" },
				sortOrder = 20
			};
		}

		ComplexRecipe.RecipeElement[] array27 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array28 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("CookedPikeapple", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		CookedPikeappleConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", array27, array28), array27, array28, DlcManager.AVAILABLE_DLC_2)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "CookingStation" },
			sortOrder = 18
		};
		return false;
	}
}

/// <summary>
/// 燃气灶
/// </summary>
[HarmonyPatch(typeof(GourmetCookingStationConfig))]
internal class SueprGourmetCookingStation
{
	[HarmonyPrefix, HarmonyPatch("ConfigureRecipes")]
	public static bool Prefix()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[1]
		{
            new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
        };
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Salsa", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		SalsaConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array, array2), array, array2)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.SALSA.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "GourmetCookingStation" },
			sortOrder = 300
		};

		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[1]
		{
            new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
        };
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("MushroomWrap", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		MushroomWrapConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array3, array4), array3, array4)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.MUSHROOMWRAP.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "GourmetCookingStation" },
			sortOrder = 400
		};

		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[1]
		{
            new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
        };
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("SurfAndTurf", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		SurfAndTurfConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array5, array6), array5, array6)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.SURFANDTURF.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "GourmetCookingStation" },
			sortOrder = 500
		};

		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[1]
		{
            new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
        };
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("SpiceBread", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		SpiceBreadConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array7, array8), array7, array8)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.SPICEBREAD.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "GourmetCookingStation" },
			sortOrder = 600
		};

		ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[1]
		{
            new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
        };
		ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("SpicyTofu", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		SpicyTofuConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array9, array10), array9, array10)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.SPICYTOFU.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "GourmetCookingStation" },
			sortOrder = 800
		};

		ComplexRecipe.RecipeElement[] array11 = new ComplexRecipe.RecipeElement[1]
		{
            new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
        };
		ComplexRecipe.RecipeElement[] array12 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Curry", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		SpicyTofuConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array11, array12), array11, array12)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.CURRY.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "GourmetCookingStation" },
			sortOrder = 800
		};

		ComplexRecipe.RecipeElement[] array13 = new ComplexRecipe.RecipeElement[1]
		{
            new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
        };
		ComplexRecipe.RecipeElement[] array14 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Quiche", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		QuicheConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array13, array14), array13, array14)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.QUICHE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "GourmetCookingStation" },
			sortOrder = 800
		};

		ComplexRecipe.RecipeElement[] array15 = new ComplexRecipe.RecipeElement[1]
		{
            new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
        };
		ComplexRecipe.RecipeElement[] array16 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement("Burger", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		BurgerConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array15, array16), array15, array16)
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = STRINGS.ITEMS.FOOD.BURGER.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "GourmetCookingStation" },
			sortOrder = 900
		};

		if (DlcManager.IsExpansion1Active())
		{
			ComplexRecipe.RecipeElement[] array17 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement("Water".ToTag(), 100f)
            };
			ComplexRecipe.RecipeElement[] array18 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement("BerryPie", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
			};
			BerryPieConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", array17, array18), array17, array18)
			{
				time = FOOD.RECIPES.STANDARD_COOK_TIME,
				description = STRINGS.ITEMS.FOOD.BERRYPIE.RECIPEDESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag> { "GourmetCookingStation" },
				sortOrder = 900
			};
		}
		return false;
	}
}
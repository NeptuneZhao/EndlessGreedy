using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02000EFA RID: 3834
	public class FOOD
	{
		// Token: 0x0400579F RID: 22431
		public const float EATING_SECONDS_PER_CALORIE = 2E-05f;

		// Token: 0x040057A0 RID: 22432
		public static float FOOD_CALORIES_PER_CYCLE = -DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE;

		// Token: 0x040057A1 RID: 22433
		public const int FOOD_AMOUNT_INGREDIENT_ONLY = 0;

		// Token: 0x040057A2 RID: 22434
		public const float KCAL_SMALL_PORTION = 600000f;

		// Token: 0x040057A3 RID: 22435
		public const float KCAL_BONUS_COOKING_LOW = 250000f;

		// Token: 0x040057A4 RID: 22436
		public const float KCAL_BASIC_PORTION = 800000f;

		// Token: 0x040057A5 RID: 22437
		public const float KCAL_PREPARED_FOOD = 4000000f;

		// Token: 0x040057A6 RID: 22438
		public const float KCAL_BONUS_COOKING_BASIC = 400000f;

		// Token: 0x040057A7 RID: 22439
		public const float KCAL_BONUS_COOKING_DEEPFRIED = 1200000f;

		// Token: 0x040057A8 RID: 22440
		public const float DEFAULT_PRESERVE_TEMPERATURE = 255.15f;

		// Token: 0x040057A9 RID: 22441
		public const float DEFAULT_ROT_TEMPERATURE = 277.15f;

		// Token: 0x040057AA RID: 22442
		public const float HIGH_PRESERVE_TEMPERATURE = 283.15f;

		// Token: 0x040057AB RID: 22443
		public const float HIGH_ROT_TEMPERATURE = 308.15f;

		// Token: 0x040057AC RID: 22444
		public const float EGG_COOK_TEMPERATURE = 344.15f;

		// Token: 0x040057AD RID: 22445
		public const float DEFAULT_MASS = 1f;

		// Token: 0x040057AE RID: 22446
		public const float DEFAULT_SPICE_MASS = 1f;

		// Token: 0x040057AF RID: 22447
		public const float ROT_TO_ELEMENT_TIME = 600f;

		// Token: 0x040057B0 RID: 22448
		public const int MUSH_BAR_SPAWN_GERMS = 1000;

		// Token: 0x040057B1 RID: 22449
		public const float IDEAL_TEMPERATURE_TOLERANCE = 10f;

		// Token: 0x040057B2 RID: 22450
		public const int FOOD_QUALITY_AWFUL = -1;

		// Token: 0x040057B3 RID: 22451
		public const int FOOD_QUALITY_TERRIBLE = 0;

		// Token: 0x040057B4 RID: 22452
		public const int FOOD_QUALITY_MEDIOCRE = 1;

		// Token: 0x040057B5 RID: 22453
		public const int FOOD_QUALITY_GOOD = 2;

		// Token: 0x040057B6 RID: 22454
		public const int FOOD_QUALITY_GREAT = 3;

		// Token: 0x040057B7 RID: 22455
		public const int FOOD_QUALITY_AMAZING = 4;

		// Token: 0x040057B8 RID: 22456
		public const int FOOD_QUALITY_WONDERFUL = 5;

		// Token: 0x040057B9 RID: 22457
		public const int FOOD_QUALITY_MORE_WONDERFUL = 6;

		// Token: 0x02001FE1 RID: 8161
		public class SPOIL_TIME
		{
			// Token: 0x040090BC RID: 37052
			public const float DEFAULT = 4800f;

			// Token: 0x040090BD RID: 37053
			public const float QUICK = 2400f;

			// Token: 0x040090BE RID: 37054
			public const float SLOW = 9600f;

			// Token: 0x040090BF RID: 37055
			public const float VERYSLOW = 19200f;
		}

		// Token: 0x02001FE2 RID: 8162
		public class FOOD_TYPES
		{
			// Token: 0x040090C0 RID: 37056
			public static readonly EdiblesManager.FoodInfo FIELDRATION = new EdiblesManager.FoodInfo("FieldRation", "", 800000f, -1, 255.15f, 277.15f, 19200f, false);

			// Token: 0x040090C1 RID: 37057
			public static readonly EdiblesManager.FoodInfo MUSHBAR = new EdiblesManager.FoodInfo("MushBar", "", 800000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090C2 RID: 37058
			public static readonly EdiblesManager.FoodInfo BASICPLANTFOOD = new EdiblesManager.FoodInfo("BasicPlantFood", "", 600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090C3 RID: 37059
			public static readonly EdiblesManager.FoodInfo BASICFORAGEPLANT = new EdiblesManager.FoodInfo("BasicForagePlant", "", 800000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x040090C4 RID: 37060
			public static readonly EdiblesManager.FoodInfo FORESTFORAGEPLANT = new EdiblesManager.FoodInfo("ForestForagePlant", "", 6400000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x040090C5 RID: 37061
			public static readonly EdiblesManager.FoodInfo SWAMPFORAGEPLANT = new EdiblesManager.FoodInfo("SwampForagePlant", "EXPANSION1_ID", 2400000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x040090C6 RID: 37062
			public static readonly EdiblesManager.FoodInfo ICECAVESFORAGEPLANT = new EdiblesManager.FoodInfo("IceCavesForagePlant", "DLC2_ID", 800000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x040090C7 RID: 37063
			public static readonly EdiblesManager.FoodInfo MUSHROOM = new EdiblesManager.FoodInfo(MushroomConfig.ID, "", 2400000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090C8 RID: 37064
			public static readonly EdiblesManager.FoodInfo LETTUCE = new EdiblesManager.FoodInfo("Lettuce", "", 400000f, 0, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090C9 RID: 37065
			public static readonly EdiblesManager.FoodInfo RAWEGG = new EdiblesManager.FoodInfo("RawEgg", "", 1600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090CA RID: 37066
			public static readonly EdiblesManager.FoodInfo MEAT = new EdiblesManager.FoodInfo("Meat", "", 1600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090CB RID: 37067
			public static readonly EdiblesManager.FoodInfo PLANTMEAT = new EdiblesManager.FoodInfo("PlantMeat", "EXPANSION1_ID", 1200000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090CC RID: 37068
			public static readonly EdiblesManager.FoodInfo PRICKLEFRUIT = new EdiblesManager.FoodInfo(PrickleFruitConfig.ID, "", 1600000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090CD RID: 37069
			public static readonly EdiblesManager.FoodInfo SWAMPFRUIT = new EdiblesManager.FoodInfo(SwampFruitConfig.ID, "EXPANSION1_ID", 1840000f, 0, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090CE RID: 37070
			public static readonly EdiblesManager.FoodInfo FISH_MEAT = new EdiblesManager.FoodInfo("FishMeat", "", 1000000f, 2, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090CF RID: 37071
			public static readonly EdiblesManager.FoodInfo SHELLFISH_MEAT = new EdiblesManager.FoodInfo("ShellfishMeat", "", 1000000f, 2, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090D0 RID: 37072
			public static readonly EdiblesManager.FoodInfo WORMBASICFRUIT = new EdiblesManager.FoodInfo("WormBasicFruit", "EXPANSION1_ID", 800000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090D1 RID: 37073
			public static readonly EdiblesManager.FoodInfo WORMSUPERFRUIT = new EdiblesManager.FoodInfo("WormSuperFruit", "EXPANSION1_ID", 250000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090D2 RID: 37074
			public static readonly EdiblesManager.FoodInfo HARDSKINBERRY = new EdiblesManager.FoodInfo("HardSkinBerry", "DLC2_ID", 800000f, -1, 255.15f, 277.15f, 9600f, true);

			// Token: 0x040090D3 RID: 37075
			public static readonly EdiblesManager.FoodInfo CARROT = new EdiblesManager.FoodInfo(CarrotConfig.ID, "DLC2_ID", 4000000f, 0, 255.15f, 277.15f, 9600f, true);

			// Token: 0x040090D4 RID: 37076
			public static readonly EdiblesManager.FoodInfo PEMMICAN = new EdiblesManager.FoodInfo("Pemmican", "DLC2_ID", FOOD.FOOD_TYPES.HARDSKINBERRY.CaloriesPerUnit * 2f + 1000000f, 2, 255.15f, 277.15f, 19200f, false);

			// Token: 0x040090D5 RID: 37077
			public static readonly EdiblesManager.FoodInfo FRIES_CARROT = new EdiblesManager.FoodInfo("FriesCarrot", "DLC2_ID", 5400000f, 3, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090D6 RID: 37078
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_MEAT = new EdiblesManager.FoodInfo("DeepFriedMeat", "DLC2_ID", 4000000f, 3, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090D7 RID: 37079
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_NOSH = new EdiblesManager.FoodInfo("DeepFriedNosh", "DLC2_ID", 5000000f, 3, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090D8 RID: 37080
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_FISH = new EdiblesManager.FoodInfo("DeepFriedFish", "DLC2_ID", 4200000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090D9 RID: 37081
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_SHELLFISH = new EdiblesManager.FoodInfo("DeepFriedShellfish", "DLC2_ID", 4200000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090DA RID: 37082
			public static readonly EdiblesManager.FoodInfo PICKLEDMEAL = new EdiblesManager.FoodInfo("PickledMeal", "", 1800000f, -1, 255.15f, 277.15f, 19200f, true);

			// Token: 0x040090DB RID: 37083
			public static readonly EdiblesManager.FoodInfo BASICPLANTBAR = new EdiblesManager.FoodInfo("BasicPlantBar", "", 1700000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090DC RID: 37084
			public static readonly EdiblesManager.FoodInfo FRIEDMUSHBAR = new EdiblesManager.FoodInfo("FriedMushBar", "", 1050000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090DD RID: 37085
			public static readonly EdiblesManager.FoodInfo GAMMAMUSH = new EdiblesManager.FoodInfo("GammaMush", "", 1050000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090DE RID: 37086
			public static readonly EdiblesManager.FoodInfo GRILLED_PRICKLEFRUIT = new EdiblesManager.FoodInfo("GrilledPrickleFruit", "", 2000000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090DF RID: 37087
			public static readonly EdiblesManager.FoodInfo SWAMP_DELIGHTS = new EdiblesManager.FoodInfo("SwampDelights", "EXPANSION1_ID", 2240000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090E0 RID: 37088
			public static readonly EdiblesManager.FoodInfo FRIED_MUSHROOM = new EdiblesManager.FoodInfo("FriedMushroom", "", 2800000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090E1 RID: 37089
			public static readonly EdiblesManager.FoodInfo COOKED_PIKEAPPLE = new EdiblesManager.FoodInfo("CookedPikeapple", "DLC2_ID", 1200000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090E2 RID: 37090
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_BREAD = new EdiblesManager.FoodInfo("ColdWheatBread", "", 1200000f, 2, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090E3 RID: 37091
			public static readonly EdiblesManager.FoodInfo COOKED_EGG = new EdiblesManager.FoodInfo("CookedEgg", "", 2800000f, 2, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090E4 RID: 37092
			public static readonly EdiblesManager.FoodInfo COOKED_FISH = new EdiblesManager.FoodInfo("CookedFish", "", 1600000f, 3, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090E5 RID: 37093
			public static readonly EdiblesManager.FoodInfo COOKED_MEAT = new EdiblesManager.FoodInfo("CookedMeat", "", 4000000f, 3, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090E6 RID: 37094
			public static readonly EdiblesManager.FoodInfo PANCAKES = new EdiblesManager.FoodInfo("Pancakes", "", 3600000f, 3, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090E7 RID: 37095
			public static readonly EdiblesManager.FoodInfo WORMBASICFOOD = new EdiblesManager.FoodInfo("WormBasicFood", "EXPANSION1_ID", 1200000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090E8 RID: 37096
			public static readonly EdiblesManager.FoodInfo WORMSUPERFOOD = new EdiblesManager.FoodInfo("WormSuperFood", "EXPANSION1_ID", 2400000f, 3, 255.15f, 277.15f, 19200f, true);

			// Token: 0x040090E9 RID: 37097
			public static readonly EdiblesManager.FoodInfo FRUITCAKE = new EdiblesManager.FoodInfo("FruitCake", "", 4000000f, 3, 255.15f, 277.15f, 19200f, false);

			// Token: 0x040090EA RID: 37098
			public static readonly EdiblesManager.FoodInfo SALSA = new EdiblesManager.FoodInfo("Salsa", "", 4400000f, 4, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090EB RID: 37099
			public static readonly EdiblesManager.FoodInfo SURF_AND_TURF = new EdiblesManager.FoodInfo("SurfAndTurf", "", 6000000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090EC RID: 37100
			public static readonly EdiblesManager.FoodInfo MUSHROOM_WRAP = new EdiblesManager.FoodInfo("MushroomWrap", "", 4800000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090ED RID: 37101
			public static readonly EdiblesManager.FoodInfo TOFU = new EdiblesManager.FoodInfo("Tofu", "", 3600000f, 2, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090EE RID: 37102
			public static readonly EdiblesManager.FoodInfo CURRY = new EdiblesManager.FoodInfo("Curry", "", 5000000f, 4, 255.15f, 277.15f, 9600f, true).AddEffects(new List<string>
			{
				"HotStuff",
				"WarmTouchFood"
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

			// Token: 0x040090EF RID: 37103
			public static readonly EdiblesManager.FoodInfo SPICEBREAD = new EdiblesManager.FoodInfo("SpiceBread", "", 4000000f, 5, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090F0 RID: 37104
			public static readonly EdiblesManager.FoodInfo SPICY_TOFU = new EdiblesManager.FoodInfo("SpicyTofu", "", 4000000f, 5, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"WarmTouchFood"
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

			// Token: 0x040090F1 RID: 37105
			public static readonly EdiblesManager.FoodInfo QUICHE = new EdiblesManager.FoodInfo("Quiche", "", 6400000f, 5, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090F2 RID: 37106
			public static readonly EdiblesManager.FoodInfo BERRY_PIE = new EdiblesManager.FoodInfo("BerryPie", "EXPANSION1_ID", 4200000f, 5, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090F3 RID: 37107
			public static readonly EdiblesManager.FoodInfo BURGER = new EdiblesManager.FoodInfo("Burger", "", 6000000f, 6, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"GoodEats"
			}, DlcManager.AVAILABLE_ALL_VERSIONS).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040090F4 RID: 37108
			public static readonly EdiblesManager.FoodInfo BEAN = new EdiblesManager.FoodInfo("BeanPlantSeed", "", 0f, 3, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040090F5 RID: 37109
			public static readonly EdiblesManager.FoodInfo SPICENUT = new EdiblesManager.FoodInfo(SpiceNutConfig.ID, "", 0f, 0, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040090F6 RID: 37110
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_SEED = new EdiblesManager.FoodInfo("ColdWheatSeed", "", 0f, 0, 283.15f, 308.15f, 9600f, true);
		}

		// Token: 0x02001FE3 RID: 8163
		public class RECIPES
		{
			// Token: 0x040090F7 RID: 37111
			public static float SMALL_COOK_TIME = 30f;

			// Token: 0x040090F8 RID: 37112
			public static float STANDARD_COOK_TIME = 50f;
		}
	}
}

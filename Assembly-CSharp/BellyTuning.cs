using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x02000095 RID: 149
public static class BellyTuning
{
	// Token: 0x040001B3 RID: 435
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "IceBellyEgg".ToTag(),
			weight = 1f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "GoldBellyEgg".ToTag(),
			weight = 0f
		}
	};

	// Token: 0x040001B4 RID: 436
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_GOLD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "IceBellyEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "GoldBellyEgg".ToTag(),
			weight = 0.98f
		}
	};

	// Token: 0x040001B5 RID: 437
	public const float KW_GENERATED_TO_WARM_UP = 1.3f;

	// Token: 0x040001B6 RID: 438
	public static float STANDARD_CALORIES_PER_CYCLE = 4f * FOOD.FOOD_TYPES.CARROT.CaloriesPerUnit / (CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == CarrotConfig.ID).cropDuration / 600f);

	// Token: 0x040001B7 RID: 439
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001B8 RID: 440
	public static float STANDARD_STOMACH_SIZE = BellyTuning.STANDARD_CALORIES_PER_CYCLE * 10f;

	// Token: 0x040001B9 RID: 441
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001BA RID: 442
	public const float EGG_MASS = 8f;

	// Token: 0x040001BB RID: 443
	public const int GERMS_EMMITED_PER_KG_POOPED = 1000;

	// Token: 0x040001BC RID: 444
	public static string GERM_ID_EMMITED_ON_POOP = "PollenGerms";

	// Token: 0x040001BD RID: 445
	public static float CALORIES_PER_UNIT_EATEN = FOOD.FOOD_TYPES.CARROT.CaloriesPerUnit;

	// Token: 0x040001BE RID: 446
	public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == CarrotConfig.ID).cropDuration / 600f;

	// Token: 0x040001BF RID: 447
	public const float CONSUMED_MASS_TO_POOP_MASS_MULTIPLIER = 67.474f;

	// Token: 0x040001C0 RID: 448
	public const float MIN_POOP_SIZE_IN_KG = 1f;
}

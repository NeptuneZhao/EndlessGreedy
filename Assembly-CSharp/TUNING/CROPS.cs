using System;
using System.Collections.Generic;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000EFB RID: 3835
	public class CROPS
	{
		// Token: 0x040057BA RID: 22458
		public const float WILD_GROWTH_RATE_MODIFIER = 0.25f;

		// Token: 0x040057BB RID: 22459
		public const float GROWTH_RATE = 0.0016666667f;

		// Token: 0x040057BC RID: 22460
		public const float WILD_GROWTH_RATE = 0.00041666668f;

		// Token: 0x040057BD RID: 22461
		public const float PLANTERPLOT_GROWTH_PENTALY = -0.5f;

		// Token: 0x040057BE RID: 22462
		public const float BASE_BONUS_SEED_PROBABILITY = 0.1f;

		// Token: 0x040057BF RID: 22463
		public const float SELF_HARVEST_TIME = 2400f;

		// Token: 0x040057C0 RID: 22464
		public const float SELF_PLANT_TIME = 2400f;

		// Token: 0x040057C1 RID: 22465
		public const float TREE_BRANCH_SELF_HARVEST_TIME = 12000f;

		// Token: 0x040057C2 RID: 22466
		public const float FERTILIZATION_GAIN_RATE = 1.6666666f;

		// Token: 0x040057C3 RID: 22467
		public const float FERTILIZATION_LOSS_RATE = -0.16666667f;

		// Token: 0x040057C4 RID: 22468
		public static List<Crop.CropVal> CROP_TYPES = new List<Crop.CropVal>
		{
			new Crop.CropVal("BasicPlantFood", 1800f, 1, true),
			new Crop.CropVal(PrickleFruitConfig.ID, 3600f, 1, true),
			new Crop.CropVal(SwampFruitConfig.ID, 3960f, 1, true),
			new Crop.CropVal(MushroomConfig.ID, 4500f, 1, true),
			new Crop.CropVal("ColdWheatSeed", 10800f, 18, true),
			new Crop.CropVal(SpiceNutConfig.ID, 4800f, 4, true),
			new Crop.CropVal(BasicFabricConfig.ID, 1200f, 1, true),
			new Crop.CropVal(SwampLilyFlowerConfig.ID, 7200f, 2, true),
			new Crop.CropVal("GasGrassHarvested", 2400f, 1, true),
			new Crop.CropVal("WoodLog", 2700f, 300, true),
			new Crop.CropVal(SimHashes.WoodLog.ToString(), 2700f, 300, true),
			new Crop.CropVal(SimHashes.SugarWater.ToString(), 150f, 20, true),
			new Crop.CropVal("SpaceTreeBranch", 2700f, 1, true),
			new Crop.CropVal("HardSkinBerry", 1800f, 1, true),
			new Crop.CropVal(CarrotConfig.ID, 5400f, 1, true),
			new Crop.CropVal(SimHashes.OxyRock.ToString(), 1200f, 2 * Mathf.RoundToInt(17.76f), true),
			new Crop.CropVal("Lettuce", 7200f, 12, true),
			new Crop.CropVal("BeanPlantSeed", 12600f, 12, true),
			new Crop.CropVal("OxyfernSeed", 7200f, 1, true),
			new Crop.CropVal("PlantMeat", 18000f, 10, true),
			new Crop.CropVal("WormBasicFruit", 2400f, 1, true),
			new Crop.CropVal("WormSuperFruit", 4800f, 8, true),
			new Crop.CropVal(SimHashes.Salt.ToString(), 3600f, 65, true),
			new Crop.CropVal(SimHashes.Water.ToString(), 6000f, 350, true)
		};
	}
}

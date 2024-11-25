using System;
using System.Collections.Generic;
using System.Linq;

namespace TUNING
{
	// Token: 0x02000F01 RID: 3841
	public class STORAGEFILTERS
	{
		// Token: 0x04005848 RID: 22600
		public static List<Tag> DEHYDRATED = new List<Tag>
		{
			GameTags.Dehydrated
		};

		// Token: 0x04005849 RID: 22601
		public static List<Tag> FOOD = new List<Tag>
		{
			GameTags.Edible,
			GameTags.CookingIngredient,
			GameTags.Medicine
		};

		// Token: 0x0400584A RID: 22602
		public static List<Tag> BAGABLE_CREATURES = new List<Tag>
		{
			GameTags.BagableCreature
		};

		// Token: 0x0400584B RID: 22603
		public static List<Tag> SWIMMING_CREATURES = new List<Tag>
		{
			GameTags.SwimmingCreature
		};

		// Token: 0x0400584C RID: 22604
		public static List<Tag> NOT_EDIBLE_SOLIDS = new List<Tag>
		{
			GameTags.Alloy,
			GameTags.RefinedMetal,
			GameTags.Metal,
			GameTags.BuildableRaw,
			GameTags.BuildableProcessed,
			GameTags.Farmable,
			GameTags.Organics,
			GameTags.Compostable,
			GameTags.Seed,
			GameTags.Agriculture,
			GameTags.Filter,
			GameTags.ConsumableOre,
			GameTags.Sublimating,
			GameTags.Liquifiable,
			GameTags.IndustrialProduct,
			GameTags.IndustrialIngredient,
			GameTags.MedicalSupplies,
			GameTags.Clothes,
			GameTags.ManufacturedMaterial,
			GameTags.Egg,
			GameTags.RareMaterials,
			GameTags.Other,
			GameTags.StoryTraitResource,
			GameTags.Dehydrated,
			GameTags.ChargedPortableBattery
		};

		// Token: 0x0400584D RID: 22605
		public static List<Tag> SPECIAL_STORAGE = new List<Tag>
		{
			GameTags.Clothes,
			GameTags.Egg,
			GameTags.Sublimating
		};

		// Token: 0x0400584E RID: 22606
		public static List<Tag> STORAGE_LOCKERS_STANDARD = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Union(new List<Tag>
		{
			GameTags.Medicine
		}).ToList<Tag>();

		// Token: 0x0400584F RID: 22607
		public static List<Tag> POWER_BANKS = new List<Tag>
		{
			GameTags.ChargedPortableBattery
		};

		// Token: 0x04005850 RID: 22608
		public static List<Tag> LIQUIDS = new List<Tag>
		{
			GameTags.Liquid
		};

		// Token: 0x04005851 RID: 22609
		public static List<Tag> GASES = new List<Tag>
		{
			GameTags.Breathable,
			GameTags.Unbreathable
		};

		// Token: 0x04005852 RID: 22610
		public static List<Tag> PAYLOADS = new List<Tag>
		{
			"RailGunPayload"
		};

		// Token: 0x04005853 RID: 22611
		public static Tag[] SOLID_TRANSFER_ARM_CONVEYABLE = new List<Tag>
		{
			GameTags.Seed,
			GameTags.CropSeed
		}.Concat(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD.Concat(STORAGEFILTERS.FOOD).Concat(STORAGEFILTERS.PAYLOADS)).ToArray<Tag>();
	}
}

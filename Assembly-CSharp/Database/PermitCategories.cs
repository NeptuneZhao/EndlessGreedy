using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000E6D RID: 3693
	public static class PermitCategories
	{
		// Token: 0x060074AA RID: 29866 RVA: 0x002D7D0F File Offset: 0x002D5F0F
		public static string GetDisplayName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].displayName;
		}

		// Token: 0x060074AB RID: 29867 RVA: 0x002D7D21 File Offset: 0x002D5F21
		public static string GetUppercaseDisplayName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].displayName.ToUpper();
		}

		// Token: 0x060074AC RID: 29868 RVA: 0x002D7D38 File Offset: 0x002D5F38
		public static string GetIconName(PermitCategory category)
		{
			return PermitCategories.CategoryInfos[category].iconName;
		}

		// Token: 0x060074AD RID: 29869 RVA: 0x002D7D4C File Offset: 0x002D5F4C
		public static PermitCategory GetCategoryForId(string id)
		{
			try
			{
				return (PermitCategory)Enum.Parse(typeof(PermitCategory), id);
			}
			catch (ArgumentException)
			{
				Debug.LogError(id + " is not a valid PermitCategory.");
			}
			return PermitCategory.Equipment;
		}

		// Token: 0x060074AE RID: 29870 RVA: 0x002D7D98 File Offset: 0x002D5F98
		public static Option<ClothingOutfitUtility.OutfitType> GetOutfitTypeFor(PermitCategory permitCategory)
		{
			return PermitCategories.CategoryInfos[permitCategory].outfitType;
		}

		// Token: 0x04005431 RID: 21553
		private static Dictionary<PermitCategory, PermitCategories.CategoryInfo> CategoryInfos = new Dictionary<PermitCategory, PermitCategories.CategoryInfo>
		{
			{
				PermitCategory.Equipment,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.EQUIPMENT, "icon_inventory_equipment", Option.None)
			},
			{
				PermitCategory.DupeTops,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_TOPS, "icon_inventory_tops", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeBottoms,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_BOTTOMS, "icon_inventory_bottoms", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_GLOVES, "icon_inventory_gloves", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_SHOES, "icon_inventory_shoes", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeHats,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_HATS, "icon_inventory_hats", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.DupeAccessories,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_ACCESSORIES, "icon_inventory_accessories", ClothingOutfitUtility.OutfitType.Clothing)
			},
			{
				PermitCategory.AtmoSuitHelmet,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_HELMET, "icon_inventory_atmosuit_helmet", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitBody,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BODY, "icon_inventory_atmosuit_body", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitGloves,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_GLOVES, "icon_inventory_atmosuit_gloves", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitBelt,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BELT, "icon_inventory_atmosuit_belt", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.AtmoSuitShoes,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_SHOES, "icon_inventory_atmosuit_boots", ClothingOutfitUtility.OutfitType.AtmoSuit)
			},
			{
				PermitCategory.Building,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.BUILDINGS, "icon_inventory_buildings", Option.None)
			},
			{
				PermitCategory.Critter,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.CRITTERS, "icon_inventory_critters", Option.None)
			},
			{
				PermitCategory.Sweepy,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.SWEEPYS, "icon_inventory_sweepys", Option.None)
			},
			{
				PermitCategory.Duplicant,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPLICANTS, "icon_inventory_duplicants", Option.None)
			},
			{
				PermitCategory.Artwork,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ARTWORKS, "icon_inventory_artworks", Option.None)
			},
			{
				PermitCategory.JoyResponse,
				new PermitCategories.CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSE, "icon_inventory_joyresponses", ClothingOutfitUtility.OutfitType.JoyResponse)
			}
		};

		// Token: 0x02001F6E RID: 8046
		private class CategoryInfo
		{
			// Token: 0x0600AF29 RID: 44841 RVA: 0x003B0DF9 File Offset: 0x003AEFF9
			public CategoryInfo(string displayName, string iconName, Option<ClothingOutfitUtility.OutfitType> outfitType)
			{
				this.displayName = displayName;
				this.iconName = iconName;
				this.outfitType = outfitType;
			}

			// Token: 0x04008E8A RID: 36490
			public string displayName;

			// Token: 0x04008E8B RID: 36491
			public string iconName;

			// Token: 0x04008E8C RID: 36492
			public Option<ClothingOutfitUtility.OutfitType> outfitType;
		}
	}
}

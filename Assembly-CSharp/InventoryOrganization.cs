﻿using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000C6D RID: 3181
public static class InventoryOrganization
{
	// Token: 0x06006196 RID: 24982 RVA: 0x002452C8 File Offset: 0x002434C8
	public static string GetPermitSubcategory(PermitResource permit)
	{
		foreach (KeyValuePair<string, HashSet<string>> keyValuePair in InventoryOrganization.subcategoryIdToPermitIdsMap)
		{
			string text;
			HashSet<string> hashSet;
			keyValuePair.Deconstruct(out text, out hashSet);
			string result = text;
			if (hashSet.Contains(permit.Id))
			{
				return result;
			}
		}
		return "UNCATEGORIZED";
	}

	// Token: 0x06006197 RID: 24983 RVA: 0x0024533C File Offset: 0x0024353C
	public static string GetCategoryName(string categoryId)
	{
		return Strings.Get("STRINGS.UI.KLEI_INVENTORY_SCREEN.TOP_LEVEL_CATEGORIES." + categoryId.ToUpper());
	}

	// Token: 0x06006198 RID: 24984 RVA: 0x00245358 File Offset: 0x00243558
	public static string GetSubcategoryName(string subcategoryId)
	{
		return Strings.Get("STRINGS.UI.KLEI_INVENTORY_SCREEN.SUBCATEGORIES." + subcategoryId.ToUpper());
	}

	// Token: 0x06006199 RID: 24985 RVA: 0x00245374 File Offset: 0x00243574
	public static void Initialize()
	{
		if (InventoryOrganization.initialized)
		{
			return;
		}
		InventoryOrganization.initialized = true;
		InventoryOrganization.GenerateTopLevelCategories();
		InventoryOrganization.GenerateSubcategories();
		foreach (KeyValuePair<string, List<string>> keyValuePair in InventoryOrganization.categoryIdToSubcategoryIdsMap)
		{
			string text;
			List<string> list;
			keyValuePair.Deconstruct(out text, out list);
			string key = text;
			List<string> list2 = list;
			bool value = true;
			foreach (string key2 in list2)
			{
				HashSet<string> hashSet;
				if (InventoryOrganization.subcategoryIdToPermitIdsMap.TryGetValue(key2, out hashSet) && hashSet.Count != 0)
				{
					value = false;
					break;
				}
			}
			InventoryOrganization.categoryIdToIsEmptyMap[key] = value;
		}
	}

	// Token: 0x0600619A RID: 24986 RVA: 0x0024544C File Offset: 0x0024364C
	private static void AddTopLevelCategory(string categoryID, Sprite icon, string[] subcategoryIDs)
	{
		InventoryOrganization.categoryIdToSubcategoryIdsMap.Add(categoryID, new List<string>(subcategoryIDs));
		InventoryOrganization.categoryIdToIconMap.Add(categoryID, icon);
	}

	// Token: 0x0600619B RID: 24987 RVA: 0x0024546C File Offset: 0x0024366C
	private static void AddSubcategory(string subcategoryID, Sprite icon, int sortkey, string[] permitIDs)
	{
		if (InventoryOrganization.subcategoryIdToPermitIdsMap.ContainsKey(subcategoryID))
		{
			return;
		}
		InventoryOrganization.subcategoryIdToPresentationDataMap.Add(subcategoryID, new InventoryOrganization.SubcategoryPresentationData(subcategoryID, icon, sortkey));
		InventoryOrganization.subcategoryIdToPermitIdsMap.Add(subcategoryID, new HashSet<string>());
		for (int i = 0; i < permitIDs.Length; i++)
		{
			InventoryOrganization.subcategoryIdToPermitIdsMap[subcategoryID].Add(permitIDs[i]);
		}
	}

	// Token: 0x0600619C RID: 24988 RVA: 0x002454CC File Offset: 0x002436CC
	private static void GenerateTopLevelCategories()
	{
		InventoryOrganization.AddTopLevelCategory("CLOTHING_TOPS", Assets.GetSprite("icon_inventory_tops"), new string[]
		{
			"CLOTHING_TOPS_BASIC",
			"CLOTHING_TOPS_TSHIRT",
			"CLOTHING_TOPS_FANCY",
			"CLOTHING_TOPS_JACKET",
			"CLOTHING_TOPS_UNDERSHIRT",
			"CLOTHING_TOPS_DRESS"
		});
		InventoryOrganization.AddTopLevelCategory("CLOTHING_BOTTOMS", Assets.GetSprite("icon_inventory_bottoms"), new string[]
		{
			"CLOTHING_BOTTOMS_BASIC",
			"CLOTHING_BOTTOMS_FANCY",
			"CLOTHING_BOTTOMS_SHORTS",
			"CLOTHING_BOTTOMS_SKIRTS",
			"CLOTHING_BOTTOMS_UNDERWEAR"
		});
		InventoryOrganization.AddTopLevelCategory("CLOTHING_GLOVES", Assets.GetSprite("icon_inventory_gloves"), new string[]
		{
			"CLOTHING_GLOVES_BASIC",
			"CLOTHING_GLOVES_PRINTS",
			"CLOTHING_GLOVES_SHORT",
			"CLOTHING_GLOVES_FORMAL"
		});
		InventoryOrganization.AddTopLevelCategory("CLOTHING_SHOES", Assets.GetSprite("icon_inventory_shoes"), new string[]
		{
			"CLOTHING_SHOES_BASIC",
			"CLOTHING_SHOES_FANCY",
			"CLOTHING_SHOE_SOCKS"
		});
		InventoryOrganization.AddTopLevelCategory("ATMOSUITS", Assets.GetSprite("icon_inventory_atmosuit_helmet"), new string[]
		{
			"ATMOSUIT_BODIES_BASIC",
			"ATMOSUIT_BODIES_FANCY",
			"ATMOSUIT_HELMETS_BASIC",
			"ATMOSUIT_HELMETS_FANCY",
			"ATMOSUIT_BELTS_BASIC",
			"ATMOSUIT_BELTS_FANCY",
			"ATMOSUIT_GLOVES_BASIC",
			"ATMOSUIT_GLOVES_FANCY",
			"ATMOSUIT_SHOES_BASIC",
			"ATMOSUIT_SHOES_FANCY"
		});
		InventoryOrganization.AddTopLevelCategory("BUILDINGS", Assets.GetSprite("icon_inventory_buildings"), new string[]
		{
			"BUILDINGS_BED_COT",
			"BUILDINGS_BED_LUXURY",
			"BUILDINGS_FLOWER_VASE",
			"BUILDING_CEILING_LIGHT",
			"BUILDINGS_STORAGE",
			"BUILDINGS_INDUSTRIAL",
			"BUILDINGS_FOOD",
			"BUILDINGS_RANCHING",
			"BUILDINGS_WASHROOM",
			"BUILDINGS_RECREATION",
			"BUILDINGS_PRINTING_POD"
		});
		InventoryOrganization.AddTopLevelCategory("WALLPAPERS", Def.GetFacadeUISprite("ExteriorWall_tropical"), new string[]
		{
			"BUILDING_WALLPAPER_BASIC",
			"BUILDING_WALLPAPER_FANCY",
			"BUILDING_WALLPAPER_PRINTS"
		});
		InventoryOrganization.AddTopLevelCategory("ARTWORK", Assets.GetSprite("icon_inventory_artworks"), new string[]
		{
			"BUILDING_CANVAS_STANDARD",
			"BUILDING_CANVAS_PORTRAIT",
			"BUILDING_CANVAS_LANDSCAPE",
			"BUILDING_SCULPTURE"
		});
		InventoryOrganization.AddTopLevelCategory("JOY_RESPONSES", Assets.GetSprite("icon_inventory_joyresponses"), new string[]
		{
			"JOY_BALLOON"
		});
	}

	// Token: 0x0600619D RID: 24989 RVA: 0x00245768 File Offset: 0x00243968
	private static void GenerateSubcategories()
	{
		InventoryOrganization.AddSubcategory("BUILDING_CEILING_LIGHT", Def.GetUISprite("CeilingLight", "ui", false).first, 100, new string[]
		{
			"CeilingLight_mining",
			"CeilingLight_flower",
			"CeilingLight_polka_lamp_shade",
			"CeilingLight_burt_shower",
			"CeilingLight_ada_flask_round",
			"CeilingLight_rubiks",
			"FloorLamp_leg",
			"FloorLamp_bristle_blossom",
			"permit_floorlamp_cottage"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_BED_COT", Def.GetUISprite("Bed", "ui", false).first, 200, new string[]
		{
			"Bed_star_curtain",
			"Bed_canopy",
			"Bed_rowan_tropical",
			"Bed_ada_science_lab",
			"Bed_stringlights",
			"permit_bed_jorge",
			"permit_bed_cottage"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_BED_LUXURY", Def.GetUISprite("LuxuryBed", "ui", false).first, 300, new string[]
		{
			"LuxuryBed_boat",
			"LuxuryBed_bouncy",
			"LuxuryBed_grandprix",
			"LuxuryBed_rocket",
			"LuxuryBed_puft",
			"LuxuryBed_hand",
			"LuxuryBed_rubiks",
			"LuxuryBed_red_rose",
			"LuxuryBed_green_mush",
			"LuxuryBed_yellow_tartar",
			"LuxuryBed_purple_brainfat",
			"permit_elegantbed_hatch",
			"permit_elegantbed_pipsqueak"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_FLOWER_VASE", Def.GetUISprite("FlowerVase", "ui", false).first, 400, new string[]
		{
			"FlowerVase_retro",
			"FlowerVase_retro_red",
			"FlowerVase_retro_white",
			"FlowerVase_retro_green",
			"FlowerVase_retro_blue",
			"FlowerVaseWall_retro_green",
			"FlowerVaseWall_retro_yellow",
			"FlowerVaseWall_retro_red",
			"FlowerVaseWall_retro_blue",
			"FlowerVaseWall_retro_white",
			"FlowerVaseHanging_retro_red",
			"FlowerVaseHanging_retro_green",
			"FlowerVaseHanging_retro_blue",
			"FlowerVaseHanging_retro_yellow",
			"FlowerVaseHanging_retro_white",
			"FlowerVaseHanging_beaker",
			"FlowerVaseHanging_rubiks",
			"PlanterBox_mealwood",
			"PlanterBox_bristleblossom",
			"PlanterBox_wheezewort",
			"PlanterBox_sleetwheat",
			"PlanterBox_salmon_pink"
		});
		InventoryOrganization.AddSubcategory("BUILDING_WALLPAPER_BASIC", Assets.GetSprite("icon_inventory_solid_wallpapers"), 500, new string[]
		{
			"ExteriorWall_basic_white",
			"ExteriorWall_basic_blue_cobalt",
			"ExteriorWall_basic_green_kelly",
			"ExteriorWall_basic_grey_charcoal",
			"ExteriorWall_basic_orange_satsuma",
			"ExteriorWall_basic_pink_flamingo",
			"ExteriorWall_basic_red_deep",
			"ExteriorWall_basic_yellow_lemon",
			"ExteriorWall_pastel_pink",
			"ExteriorWall_pastel_yellow",
			"ExteriorWall_pastel_green",
			"ExteriorWall_pastel_blue",
			"ExteriorWall_pastel_purple"
		});
		InventoryOrganization.AddSubcategory("BUILDING_WALLPAPER_FANCY", Assets.GetSprite("icon_inventory_geometric_wallpapers"), 600, new string[]
		{
			"ExteriorWall_diagonal_red_deep_white",
			"ExteriorWall_diagonal_orange_satsuma_white",
			"ExteriorWall_diagonal_yellow_lemon_white",
			"ExteriorWall_diagonal_green_kelly_white",
			"ExteriorWall_diagonal_blue_cobalt_white",
			"ExteriorWall_diagonal_pink_flamingo_white",
			"ExteriorWall_diagonal_grey_charcoal_white",
			"ExteriorWall_circle_red_deep_white",
			"ExteriorWall_circle_orange_satsuma_white",
			"ExteriorWall_circle_yellow_lemon_white",
			"ExteriorWall_circle_green_kelly_white",
			"ExteriorWall_circle_blue_cobalt_white",
			"ExteriorWall_circle_pink_flamingo_white",
			"ExteriorWall_circle_grey_charcoal_white",
			"ExteriorWall_stripes_blue",
			"ExteriorWall_stripes_diagonal_blue",
			"ExteriorWall_stripes_circle_blue",
			"ExteriorWall_squares_red_deep_white",
			"ExteriorWall_squares_orange_satsuma_white",
			"ExteriorWall_squares_yellow_lemon_white",
			"ExteriorWall_squares_green_kelly_white",
			"ExteriorWall_squares_blue_cobalt_white",
			"ExteriorWall_squares_pink_flamingo_white",
			"ExteriorWall_squares_grey_charcoal_white",
			"ExteriorWall_plus_red_deep_white",
			"ExteriorWall_plus_orange_satsuma_white",
			"ExteriorWall_plus_yellow_lemon_white",
			"ExteriorWall_plus_green_kelly_white",
			"ExteriorWall_plus_blue_cobalt_white",
			"ExteriorWall_plus_pink_flamingo_white",
			"ExteriorWall_plus_grey_charcoal_white",
			"ExteriorWall_stripes_rose",
			"ExteriorWall_stripes_diagonal_rose",
			"ExteriorWall_stripes_circle_rose",
			"ExteriorWall_stripes_mush",
			"ExteriorWall_stripes_diagonal_mush",
			"ExteriorWall_stripes_circle_mush",
			"ExteriorWall_stripes_yellow_tartar",
			"ExteriorWall_stripes_diagonal_yellow_tartar",
			"ExteriorWall_stripes_circle_yellow_tartar",
			"ExteriorWall_stripes_purple_brainfat",
			"ExteriorWall_stripes_diagonal_purple_brainfat",
			"ExteriorWall_stripes_circle_purple_brainfat"
		});
		InventoryOrganization.AddSubcategory("BUILDING_WALLPAPER_PRINTS", Assets.GetSprite("icon_inventory_patterned_wallpapers"), 700, new string[]
		{
			"ExteriorWall_balm_lily",
			"ExteriorWall_clouds",
			"ExteriorWall_coffee",
			"ExteriorWall_mosaic",
			"ExteriorWall_mushbar",
			"ExteriorWall_plaid",
			"ExteriorWall_rain",
			"ExteriorWall_rainbow",
			"ExteriorWall_snow",
			"ExteriorWall_sun",
			"ExteriorWall_polka",
			"ExteriorWall_blueberries",
			"ExteriorWall_grapes",
			"ExteriorWall_lemon",
			"ExteriorWall_lime",
			"ExteriorWall_satsuma",
			"ExteriorWall_strawberry",
			"ExteriorWall_watermelon",
			"ExteriorWall_toiletpaper",
			"ExteriorWall_plunger",
			"ExteriorWall_tropical",
			"ExteriorWall_kitchen_retro1",
			"ExteriorWall_floppy_azulene_vitro",
			"ExteriorWall_floppy_black_white",
			"ExteriorWall_floppy_peagreen_balmy",
			"ExteriorWall_floppy_satsuma_yellowcake",
			"ExteriorWall_floppy_magma_amino",
			"ExteriorWall_orange_juice",
			"ExteriorWall_paint_blots",
			"ExteriorWall_telescope",
			"ExteriorWall_tictactoe_o",
			"ExteriorWall_tictactoe_x",
			"ExteriorWall_dice_1",
			"ExteriorWall_dice_2",
			"ExteriorWall_dice_3",
			"ExteriorWall_dice_4",
			"ExteriorWall_dice_5",
			"ExteriorWall_dice_6",
			"permit_walls_wood_panel",
			"permit_walls_igloo",
			"permit_walls_forest",
			"permit_walls_southwest"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_RECREATION", Def.GetUISprite("WaterCooler", "ui", false).first, 700, new string[]
		{
			"ItemPedestal_hand",
			"MassageTable_shiatsu",
			"MassageTable_balloon",
			"WaterCooler_round_body",
			"WaterCooler_balloon",
			"WaterCooler_yellow_tartar",
			"WaterCooler_red_rose",
			"WaterCooler_green_mush",
			"WaterCooler_purple_brainfat",
			"WaterCooler_blue_babytears",
			"CornerMoulding_shineornaments",
			"CrownMoulding_shineornaments"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_PRINTING_POD", Def.GetUISprite("Headquarters", "ui", false).first, 750, new string[]
		{
			"permit_headquarters_ceres"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_STORAGE", Def.GetUISprite("StorageLocker", "ui", false).first, 800, new string[]
		{
			"StorageLocker_green_mush",
			"StorageLocker_red_rose",
			"StorageLocker_blue_babytears",
			"StorageLocker_purple_brainfat",
			"StorageLocker_yellow_tartar",
			"StorageLocker_polka_darknavynookgreen",
			"StorageLocker_polka_darkpurpleresin",
			"StorageLocker_stripes_red_white",
			"Refrigerator_stripes_red_white",
			"Refrigerator_blue_babytears",
			"Refrigerator_green_mush",
			"Refrigerator_red_rose",
			"Refrigerator_yellow_tartar",
			"Refrigerator_purple_brainfat",
			"GasReservoir_lightgold",
			"GasReservoir_peagreen",
			"GasReservoir_lightcobalt",
			"GasReservoir_polka_darkpurpleresin",
			"GasReservoir_polka_darknavynookgreen",
			"GasReservoir_blue_babytears",
			"GasReservoir_yellow_tartar",
			"GasReservoir_green_mush",
			"GasReservoir_red_rose",
			"GasReservoir_purple_brainfat",
			"permit_smartstoragelocker_gravitas"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_INDUSTRIAL", Def.GetUISprite("RockCrusher", "ui", false).first, 800, new string[]
		{
			"RockCrusher_hands",
			"RockCrusher_teeth",
			"RockCrusher_roundstamp",
			"RockCrusher_spikebeds",
			"RockCrusher_chomp",
			"RockCrusher_gears",
			"RockCrusher_balloon"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_FOOD", Def.GetUISprite("EggCracker", "ui", false).first, 800, new string[]
		{
			"EggCracker_beaker",
			"EggCracker_flower",
			"EggCracker_hands",
			"MicrobeMusher_purple_brainfat",
			"MicrobeMusher_yellow_tartar",
			"MicrobeMusher_red_rose",
			"MicrobeMusher_green_mush",
			"MicrobeMusher_blue_babytears",
			"permit_cookingstation_cottage",
			"permit_cookingstation_gourmet_cottage"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_RANCHING", Def.GetUISprite("RanchStation", "ui", false).first, 800, new string[]
		{
			"permit_rancherstation_cottage"
		});
		InventoryOrganization.AddSubcategory("BUILDINGS_WASHROOM", Def.GetUISprite("FlushToilet", "ui", false).first, 800, new string[]
		{
			"FlushToilet_polka_darkpurpleresin",
			"FlushToilet_polka_darknavynookgreen",
			"FlushToilet_purple_brainfat",
			"FlushToilet_yellow_tartar",
			"FlushToilet_red_rose",
			"FlushToilet_green_mush",
			"FlushToilet_blue_babytears",
			"WashSink_purple_brainfat",
			"WashSink_blue_babytears",
			"WashSink_green_mush",
			"WashSink_yellow_tartar",
			"WashSink_red_rose"
		});
		InventoryOrganization.AddSubcategory("JOY_BALLOON", Db.Get().Permits.BalloonArtistFacades[0].GetPermitPresentationInfo().sprite, 100, new string[]
		{
			"BalloonRedFireEngineLongSparkles",
			"BalloonYellowLongSparkles",
			"BalloonBlueLongSparkles",
			"BalloonGreenLongSparkles",
			"BalloonPinkLongSparkles",
			"BalloonPurpleLongSparkles",
			"BalloonBabyPacuEgg",
			"BalloonBabyGlossyDreckoEgg",
			"BalloonBabyHatchEgg",
			"BalloonBabyPokeshellEgg",
			"BalloonBabyPuftEgg",
			"BalloonBabyShovoleEgg",
			"BalloonBabyPipEgg",
			"BalloonCandyBlueberry",
			"BalloonCandyGrape",
			"BalloonCandyLemon",
			"BalloonCandyLime",
			"BalloonCandyOrange",
			"BalloonCandyStrawberry",
			"BalloonCandyWatermelon",
			"BalloonHandGold"
		});
		InventoryOrganization.AddSubcategory("JOY_STICKER", Db.Get().Permits.StickerBombs[0].GetPermitPresentationInfo().sprite, 200, new string[]
		{
			"a",
			"b",
			"c",
			"d",
			"e",
			"f",
			"g",
			"h",
			"rocket",
			"paperplane",
			"plant",
			"plantpot",
			"mushroom",
			"mermaid",
			"spacepet",
			"spacepet2",
			"spacepet3",
			"spacepet4",
			"spacepet5",
			"unicorn"
		});
		InventoryOrganization.AddSubcategory("PRIMO_GARB", null, 200, new string[]
		{
			"clubshirt",
			"cummerbund",
			"decor_02",
			"decor_03",
			"decor_04",
			"decor_05",
			"gaudysweater",
			"limone",
			"mondrian",
			"overalls",
			"triangles",
			"workout"
		});
		InventoryOrganization.AddSubcategory("BUILDING_CANVAS_STANDARD", Def.GetUISprite("Canvas", "ui", false).first, 100, new string[]
		{
			"Canvas_Bad",
			"Canvas_Average",
			"Canvas_Good",
			"Canvas_Good2",
			"Canvas_Good3",
			"Canvas_Good4",
			"Canvas_Good5",
			"Canvas_Good6",
			"Canvas_Good7",
			"Canvas_Good8",
			"Canvas_Good9",
			"Canvas_Good10",
			"Canvas_Good11",
			"Canvas_Good13",
			"Canvas_Good12",
			"Canvas_Good14",
			"Canvas_Good15",
			"Canvas_Good16",
			"permit_painting_art_ceres_a"
		});
		InventoryOrganization.AddSubcategory("BUILDING_CANVAS_PORTRAIT", Def.GetUISprite("CanvasTall", "ui", false).first, 200, new string[]
		{
			"CanvasTall_Bad",
			"CanvasTall_Average",
			"CanvasTall_Good",
			"CanvasTall_Good2",
			"CanvasTall_Good3",
			"CanvasTall_Good4",
			"CanvasTall_Good5",
			"CanvasTall_Good6",
			"CanvasTall_Good7",
			"CanvasTall_Good8",
			"CanvasTall_Good9",
			"CanvasTall_Good11",
			"CanvasTall_Good10",
			"CanvasTall_Good14",
			"permit_painting_tall_art_ceres_a"
		});
		InventoryOrganization.AddSubcategory("BUILDING_CANVAS_LANDSCAPE", Def.GetUISprite("CanvasWide", "ui", false).first, 300, new string[]
		{
			"CanvasWide_Bad",
			"CanvasWide_Average",
			"CanvasWide_Good",
			"CanvasWide_Good2",
			"CanvasWide_Good3",
			"CanvasWide_Good4",
			"CanvasWide_Good5",
			"CanvasWide_Good6",
			"CanvasWide_Good7",
			"CanvasWide_Good8",
			"CanvasWide_Good9",
			"CanvasWide_Good10",
			"CanvasWide_Good11",
			"CanvasWide_Good13",
			"permit_painting_wide_art_ceres_a"
		});
		InventoryOrganization.AddSubcategory("BUILDING_SCULPTURE", Def.GetUISprite("Sculpture", "ui", false).first, 400, new string[]
		{
			"Sculpture_Bad",
			"Sculpture_Average",
			"Sculpture_Good1",
			"Sculpture_Good2",
			"Sculpture_Good3",
			"Sculpture_Good5",
			"Sculpture_Good6",
			"SmallSculpture_Bad",
			"SmallSculpture_Average",
			"SmallSculpture_Good",
			"SmallSculpture_Good2",
			"SmallSculpture_Good3",
			"SmallSculpture_Good5",
			"SmallSculpture_Good6",
			"IceSculpture_Bad",
			"IceSculpture_Average",
			"MarbleSculpture_Bad",
			"MarbleSculpture_Average",
			"MarbleSculpture_Good1",
			"MarbleSculpture_Good2",
			"MarbleSculpture_Good3",
			"MetalSculpture_Bad",
			"MetalSculpture_Average",
			"MetalSculpture_Good1",
			"MetalSculpture_Good2",
			"MetalSculpture_Good3",
			"MetalSculpture_Good5",
			"Sculpture_Good4",
			"SmallSculpture_Good4",
			"MetalSculpture_Good4",
			"MarbleSculpture_Good4",
			"MarbleSculpture_Good5",
			"IceSculpture_Average2",
			"IceSculpture_Average3",
			"permit_sculpture_wood_amazing_action_puft",
			"permit_sculpture_wood_amazing_rear_puft",
			"permit_sculpture_wood_amazing_rear_shovevole",
			"permit_sculpture_wood_amazing_action_gulp",
			"permit_sculpture_wood_amazing_rear_cuddlepip",
			"permit_sculpture_wood_amazing_rear_drecko",
			"permit_sculpture_wood_okay_mid_one",
			"permit_sculpture_wood_amazing_action_pacu",
			"permit_sculpture_wood_crap_low_one",
			"permit_sculpture_wood_amazing_action_wood_deer",
			"permit_icesculpture_amazing_idle_seal",
			"permit_icesculpture_amazing_idle_bammoth",
			"permit_icesculpture_amazing_idle_wood_deer"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_BASIC", Assets.GetSprite("icon_inventory_basic_shirts"), 100, new string[]
		{
			"TopBasicBlack",
			"TopBasicWhite",
			"TopBasicRed",
			"TopBasicOrange",
			"TopBasicYellow",
			"TopBasicGreen",
			"TopBasicAqua",
			"TopBasicPurple",
			"TopBasicPinkOrchid"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_TSHIRT", Assets.GetSprite("icon_inventory_tees"), 300, new string[]
		{
			"TopRaglanDeepRed",
			"TopRaglanCobalt",
			"TopRaglanFlamingo",
			"TopRaglanKellyGreen",
			"TopRaglanCharcoal",
			"TopRaglanLemon",
			"TopRaglanSatsuma",
			"TopTShirtWhite",
			"TopTShirtMagenta"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_UNDERSHIRT", Assets.GetSprite("icon_inventory_undershirts"), 400, new string[]
		{
			"TopUndershirtExecutive",
			"TopUndershirtUnderling",
			"TopUndershirtGroupthink",
			"TopUndershirtStakeholder",
			"TopUndershirtAdmin",
			"TopUndershirtBuzzword",
			"TopUndershirtSynergy",
			"TopGinchPinkSaltrock",
			"TopGinchPurpleDusky",
			"TopGinchBlueBasin",
			"TopGinchTealBalmy",
			"TopGinchGreenLime",
			"TopGinchYellowYellowcake",
			"TopGinchOrangeAtomic",
			"TopGinchRedMagma",
			"TopGinchGreyGrey",
			"TopGinchGreyCharcoal"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_FANCY", Assets.GetSprite("icon_inventory_specialty_tops"), 500, new string[]
		{
			"TopResearcher",
			"TopX1Pinchapeppernutbells",
			"TopXSporchid",
			"TopPompomShinebugsPinkPeppernut",
			"TopSnowflakeBlue",
			"TopWaistcoatPinstripeSlate",
			"TopWater",
			"TopFloralPink",
			"permit_top_flannel_red",
			"permit_top_flannel_orange",
			"permit_top_flannel_yellow",
			"permit_top_flannel_green",
			"permit_top_flannel_blue_middle",
			"permit_top_flannel_purple",
			"permit_top_flannel_pink_orchid",
			"permit_top_flannel_white",
			"permit_top_flannel_black",
			"permit_top_jersey_01",
			"permit_top_jersey_02",
			"permit_top_jersey_03",
			"permit_top_jersey_04",
			"permit_top_jersey_05",
			"permit_top_jersey_07",
			"permit_top_jersey_08",
			"permit_top_jersey_09",
			"permit_top_jersey_10",
			"permit_top_jersey_11",
			"permit_top_jersey_12"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_JACKET", Assets.GetSprite("icon_inventory_jackets"), 500, new string[]
		{
			"TopJellypuffJacketBlueberry",
			"TopJellypuffJacketGrape",
			"TopJellypuffJacketLemon",
			"TopJellypuffJacketLime",
			"TopJellypuffJacketSatsuma",
			"TopJellypuffJacketStrawberry",
			"TopJellypuffJacketWatermelon",
			"TopAthlete",
			"TopCircuitGreen",
			"TopDenimBlue",
			"TopRebelGi",
			"TopJacketSmokingBurgundy",
			"TopMechanic",
			"TopVelourBlack",
			"TopVelourBlue",
			"TopVelourPink",
			"TopTweedPinkOrchid",
			"TopBuilder",
			"TopKnitPolkadotTurq",
			"TopFlashy"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_TOPS_DRESS", Assets.GetSprite("icon_inventory_dress_fancy"), 500, new string[]
		{
			"DressSleevelessBowBw",
			"BodysuitBallerinaPink",
			"PjCloversGlitchKelly",
			"PjHeartsChilliStrawberry"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_BASIC", Assets.GetSprite("icon_inventory_basic_pants"), 100, new string[]
		{
			"BottomBasicBlack",
			"BottomBasicWhite",
			"BottomBasicRed",
			"BottomBasicOrange",
			"BottomBasicYellow",
			"BottomBasicGreen",
			"BottomBasicAqua",
			"BottomBasicPurple",
			"BottomBasicPinkOrchid",
			"PantsBasicRedOrange",
			"PantsBasicLightBrown",
			"PantsBasicOrangeSatsuma"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_FANCY", Assets.GetSprite("icon_inventory_fancy_pants"), 200, new string[]
		{
			"PantsAthlete",
			"PantsCircuitGreen",
			"PantsJeans",
			"PantsRebelGi",
			"PantsResearch",
			"PantsPinstripeSlate",
			"PantsVelourBlack",
			"PantsVelourBlue",
			"PantsVelourPink",
			"PantsKnitPolkadotTurq",
			"PantsGiBeltWhiteBlack",
			"PantsBeltKhakiTan"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_SHORTS", Assets.GetSprite("icon_inventory_shorts"), 300, new string[]
		{
			"ShortsBasicDeepRed",
			"ShortsBasicSatsuma",
			"ShortsBasicYellowcake",
			"ShortsBasicKellyGreen",
			"ShortsBasicBlueCobalt",
			"ShortsBasicPinkFlamingo",
			"ShortsBasicCharcoal"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_SKIRTS", Assets.GetSprite("icon_inventory_skirts"), 300, new string[]
		{
			"SkirtBasicBlueMiddle",
			"SkirtBasicPurple",
			"SkirtBasicGreen",
			"SkirtBasicOrange",
			"SkirtBasicPinkOrchid",
			"SkirtBasicRed",
			"SkirtBasicYellow",
			"SkirtBasicPolkadot",
			"SkirtBasicWatermelon",
			"SkirtDenimBlue",
			"SkirtLeopardPrintBluePink",
			"SkirtSparkleBlue",
			"SkirtBallerinaPink",
			"SkirtTweedPinkOrchid"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_BOTTOMS_UNDERWEAR", Assets.GetSprite("icon_inventory_underwear"), 300, new string[]
		{
			"BottomBriefsExecutive",
			"BottomBriefsUnderling",
			"BottomBriefsGroupthink",
			"BottomBriefsStakeholder",
			"BottomBriefsAdmin",
			"BottomBriefsBuzzword",
			"BottomBriefsSynergy",
			"BottomGinchPinkGluon",
			"BottomGinchPurpleCortex",
			"BottomGinchBlueFrosty",
			"BottomGinchTealLocus",
			"BottomGinchGreenGoop",
			"BottomGinchYellowBile",
			"BottomGinchOrangeNybble",
			"BottomGinchRedIronbow",
			"BottomGinchGreyPhlegm",
			"BottomGinchGreyObelus"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_GLOVES_BASIC", Assets.GetSprite("icon_inventory_basic_gloves"), 100, new string[]
		{
			"GlovesBasicBlack",
			"GlovesBasicWhite",
			"GlovesBasicRed",
			"GlovesBasicOrange",
			"GlovesBasicYellow",
			"GlovesBasicGreen",
			"GlovesBasicAqua",
			"GlovesBasicPurple",
			"GlovesBasicPinkOrchid",
			"GlovesBasicSlate",
			"GlovesBasicBlueGrey",
			"GlovesBasicBrownKhaki",
			"GlovesBasicGrey",
			"GlovesBasicPinksalmon",
			"GlovesBasicTan"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_GLOVES_FORMAL", Assets.GetSprite("icon_inventory_fancy_gloves"), 200, new string[]
		{
			"GlovesFormalWhite",
			"GlovesLongWhite",
			"Gloves2ToneCreamCharcoal",
			"GlovesSparkleWhite",
			"GlovesGinchPinkSaltrock",
			"GlovesGinchPurpleDusky",
			"GlovesGinchBlueBasin",
			"GlovesGinchTealBalmy",
			"GlovesGinchGreenLime",
			"GlovesGinchYellowYellowcake",
			"GlovesGinchOrangeAtomic",
			"GlovesGinchRedMagma",
			"GlovesGinchGreyGrey",
			"GlovesGinchGreyCharcoal",
			"permit_gloves_hockey_01",
			"permit_gloves_hockey_02",
			"permit_gloves_hockey_03",
			"permit_gloves_hockey_04",
			"permit_gloves_hockey_05",
			"permit_gloves_hockey_07",
			"permit_gloves_hockey_08",
			"permit_gloves_hockey_09",
			"permit_gloves_hockey_10",
			"permit_gloves_hockey_11",
			"permit_gloves_hockey_12",
			"permit_mittens_knit_black_smog",
			"permit_mittens_knit_white",
			"permit_mittens_knit_yellowcake",
			"permit_mittens_knit_orange_tectonic",
			"permit_mittens_knit_green_enzyme",
			"permit_mittens_knit_blue_azulene",
			"permit_mittens_knit_purple_astral",
			"permit_mittens_knit_pink_cosmic"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_GLOVES_SHORT", Assets.GetSprite("icon_inventory_short_gloves"), 300, new string[]
		{
			"GlovesCufflessBlueberry",
			"GlovesCufflessGrape",
			"GlovesCufflessLemon",
			"GlovesCufflessLime",
			"GlovesCufflessSatsuma",
			"GlovesCufflessStrawberry",
			"GlovesCufflessWatermelon",
			"GlovesCufflessBlack"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_GLOVES_PRINTS", Assets.GetSprite("icon_inventory_specialty_gloves"), 400, new string[]
		{
			"GlovesAthlete",
			"GlovesCircuitGreen",
			"GlovesAthleticRedDeep",
			"GlovesAthleticOrangeSatsuma",
			"GlovesAthleticYellowLemon",
			"GlovesAthleticGreenKelly",
			"GlovesAthleticBlueCobalt",
			"GlovesAthleticPinkFlamingo",
			"GlovesAthleticGreyCharcoal",
			"GlovesDenimBlue",
			"GlovesBallerinaPink",
			"GlovesKnitGold",
			"GlovesKnitMagenta"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_SHOES_BASIC", Assets.GetSprite("icon_inventory_basic_shoes"), 100, new string[]
		{
			"ShoesBasicBlack",
			"ShoesBasicWhite",
			"ShoesBasicRed",
			"ShoesBasicOrange",
			"ShoesBasicYellow",
			"ShoesBasicGreen",
			"ShoesBasicAqua",
			"ShoesBasicPurple",
			"ShoesBasicPinkOrchid",
			"ShoesBasicBlueGrey",
			"ShoesBasicTan",
			"ShoesBasicGray",
			"ShoesDenimBlue"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_SHOES_FANCY", Assets.GetSprite("icon_inventory_fancy_shoes"), 200, new string[]
		{
			"ShoesBallerinaPink",
			"ShoesMaryjaneSocksBw",
			"ShoesClassicFlatsCreamCharcoal",
			"ShoesVelourBlue",
			"ShoesVelourPink",
			"ShoesVelourBlack",
			"ShoesFlashy"
		});
		InventoryOrganization.AddSubcategory("CLOTHING_SHOE_SOCKS", Assets.GetSprite("icon_inventory_socks"), 500, new string[]
		{
			"SocksAthleticDeepRed",
			"SocksAthleticOrangeSatsuma",
			"SocksAthleticYellowLemon",
			"SocksAthleticGreenKelly",
			"SocksAthleticBlueCobalt",
			"SocksAthleticPinkFlamingo",
			"SocksAthleticGreyCharcoal",
			"SocksLegwarmersBlueberry",
			"SocksLegwarmersGrape",
			"SocksLegwarmersLemon",
			"SocksLegwarmersLime",
			"SocksLegwarmersSatsuma",
			"SocksLegwarmersStrawberry",
			"SocksLegwarmersWatermelon",
			"SocksGinchPinkSaltrock",
			"SocksGinchPurpleDusky",
			"SocksGinchBlueBasin",
			"SocksGinchTealBalmy",
			"SocksGinchGreenLime",
			"SocksGinchYellowYellowcake",
			"SocksGinchOrangeAtomic",
			"SocksGinchRedMagma",
			"SocksGinchGreyGrey",
			"SocksGinchGreyCharcoal"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_BODIES_BASIC", Assets.GetSprite("icon_inventory_atmosuit_body"), 100, new string[]
		{
			"AtmoSuitBasicYellow",
			"AtmoSuitSparkleRed",
			"AtmoSuitSparkleGreen",
			"AtmoSuitSparkleBlue",
			"AtmoSuitSparkleLavender",
			"AtmoSuitPuft",
			"AtmoSuitConfetti",
			"AtmoSuitCrispEggplant",
			"AtmoSuitBasicNeonPink",
			"AtmoSuitMultiRedBlack",
			"AtmoSuitRocketmelon",
			"AtmoSuitMultiBlueGreyBlack",
			"AtmoSuitMultiBlueYellowRed",
			"permit_atmosuit_80s"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_HELMETS_BASIC", Assets.GetSprite("icon_inventory_atmosuit_helmet"), 300, new string[]
		{
			"AtmoHelmetLimone",
			"AtmoHelmetSparkleRed",
			"AtmoHelmetSparkleGreen",
			"AtmoHelmetSparkleBlue",
			"AtmoHelmetSparklePurple",
			"AtmoHelmetPuft",
			"AtmoHelmetConfetti",
			"AtmoHelmetEggplant",
			"AtmoHelmetCummerbundRed",
			"AtmoHelmetWorkoutLavender",
			"AtmoHelmetRocketmelon",
			"AtmoHelmetMondrianBlueRedYellow",
			"AtmoHelmetOverallsRed",
			"permit_atmo_helmet_80s"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_GLOVES_BASIC", Assets.GetSprite("icon_inventory_atmosuit_gloves"), 500, new string[]
		{
			"AtmoGlovesLime",
			"AtmoGlovesSparkleRed",
			"AtmoGlovesSparkleGreen",
			"AtmoGlovesSparkleBlue",
			"AtmoGlovesSparkleLavender",
			"AtmoGlovesPuft",
			"AtmoGlovesGold",
			"AtmoGlovesEggplant",
			"AtmoGlovesWhite",
			"AtmoGlovesStripesLavender",
			"AtmoGlovesRocketmelon",
			"AtmoGlovesBrown",
			"permit_atmo_gloves_80s"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_BELTS_BASIC", Assets.GetSprite("icon_inventory_atmosuit_belt"), 700, new string[]
		{
			"AtmoBeltBasicLime",
			"AtmoBeltSparkleRed",
			"AtmoBeltSparkleGreen",
			"AtmoBeltSparkleBlue",
			"AtmoBeltSparkleLavender",
			"AtmoBeltPuft",
			"AtmoBeltBasicGold",
			"AtmoBeltEggplant",
			"AtmoBeltBasicGrey",
			"AtmoBeltBasicNeonPink",
			"AtmoBeltRocketmelon",
			"AtmoBeltTwoToneBrown",
			"permit_atmo_belt_80s"
		});
		InventoryOrganization.AddSubcategory("ATMOSUIT_SHOES_BASIC", Assets.GetSprite("icon_inventory_atmosuit_boots"), 900, new string[]
		{
			"AtmoShoesBasicYellow",
			"AtmoShoesSparkleBlack",
			"AtmoShoesPuft",
			"AtmoShoesStealth",
			"AtmoShoesEggplant",
			"AtmoShoesBasicLavender",
			"AtmoBootsRocketmelon",
			"permit_atmo_shoes_80s"
		});
	}

	// Token: 0x0400422B RID: 16939
	public static Dictionary<string, List<string>> categoryIdToSubcategoryIdsMap = new Dictionary<string, List<string>>();

	// Token: 0x0400422C RID: 16940
	public static Dictionary<string, Sprite> categoryIdToIconMap = new Dictionary<string, Sprite>();

	// Token: 0x0400422D RID: 16941
	public static Dictionary<string, bool> categoryIdToIsEmptyMap = new Dictionary<string, bool>();

	// Token: 0x0400422E RID: 16942
	public static bool initialized = false;

	// Token: 0x0400422F RID: 16943
	public static Dictionary<string, HashSet<string>> subcategoryIdToPermitIdsMap = new Dictionary<string, HashSet<string>>
	{
		{
			"UNCATEGORIZED",
			new HashSet<string>()
		},
		{
			"YAML",
			new HashSet<string>()
		}
	};

	// Token: 0x04004230 RID: 16944
	public static Dictionary<string, InventoryOrganization.SubcategoryPresentationData> subcategoryIdToPresentationDataMap = new Dictionary<string, InventoryOrganization.SubcategoryPresentationData>
	{
		{
			"UNCATEGORIZED",
			new InventoryOrganization.SubcategoryPresentationData("UNCATEGORIZED", Assets.GetSprite("error_message"), 0)
		},
		{
			"YAML",
			new InventoryOrganization.SubcategoryPresentationData("YAML", Assets.GetSprite("error_message"), 0)
		}
	};

	// Token: 0x02001D44 RID: 7492
	public class SubcategoryPresentationData
	{
		// Token: 0x0600A82A RID: 43050 RVA: 0x0039C16B File Offset: 0x0039A36B
		public SubcategoryPresentationData(string subcategoryID, Sprite icon, int sortKey)
		{
			this.subcategoryID = subcategoryID;
			this.sortKey = sortKey;
			this.icon = icon;
		}

		// Token: 0x04008688 RID: 34440
		public string subcategoryID;

		// Token: 0x04008689 RID: 34441
		public int sortKey;

		// Token: 0x0400868A RID: 34442
		public Sprite icon;
	}

	// Token: 0x02001D45 RID: 7493
	public static class InventoryPermitCategories
	{
		// Token: 0x0400868B RID: 34443
		public const string CLOTHING_TOPS = "CLOTHING_TOPS";

		// Token: 0x0400868C RID: 34444
		public const string CLOTHING_BOTTOMS = "CLOTHING_BOTTOMS";

		// Token: 0x0400868D RID: 34445
		public const string CLOTHING_GLOVES = "CLOTHING_GLOVES";

		// Token: 0x0400868E RID: 34446
		public const string CLOTHING_SHOES = "CLOTHING_SHOES";

		// Token: 0x0400868F RID: 34447
		public const string ATMOSUITS = "ATMOSUITS";

		// Token: 0x04008690 RID: 34448
		public const string BUILDINGS = "BUILDINGS";

		// Token: 0x04008691 RID: 34449
		public const string WALLPAPERS = "WALLPAPERS";

		// Token: 0x04008692 RID: 34450
		public const string ARTWORK = "ARTWORK";

		// Token: 0x04008693 RID: 34451
		public const string JOY_RESPONSES = "JOY_RESPONSES";

		// Token: 0x04008694 RID: 34452
		public const string ATMO_SUIT_HELMET = "ATMO_SUIT_HELMET";

		// Token: 0x04008695 RID: 34453
		public const string ATMO_SUIT_BODY = "ATMO_SUIT_BODY";

		// Token: 0x04008696 RID: 34454
		public const string ATMO_SUIT_GLOVES = "ATMO_SUIT_GLOVES";

		// Token: 0x04008697 RID: 34455
		public const string ATMO_SUIT_BELT = "ATMO_SUIT_BELT";

		// Token: 0x04008698 RID: 34456
		public const string ATMO_SUIT_SHOES = "ATMO_SUIT_SHOES";
	}

	// Token: 0x02001D46 RID: 7494
	public static class PermitSubcategories
	{
		// Token: 0x04008699 RID: 34457
		public const string YAML = "YAML";

		// Token: 0x0400869A RID: 34458
		public const string UNCATEGORIZED = "UNCATEGORIZED";

		// Token: 0x0400869B RID: 34459
		public const string JOY_BALLOON = "JOY_BALLOON";

		// Token: 0x0400869C RID: 34460
		public const string JOY_STICKER = "JOY_STICKER";

		// Token: 0x0400869D RID: 34461
		public const string PRIMO_GARB = "PRIMO_GARB";

		// Token: 0x0400869E RID: 34462
		public const string CLOTHING_TOPS_BASIC = "CLOTHING_TOPS_BASIC";

		// Token: 0x0400869F RID: 34463
		public const string CLOTHING_TOPS_TSHIRT = "CLOTHING_TOPS_TSHIRT";

		// Token: 0x040086A0 RID: 34464
		public const string CLOTHING_TOPS_FANCY = "CLOTHING_TOPS_FANCY";

		// Token: 0x040086A1 RID: 34465
		public const string CLOTHING_TOPS_JACKET = "CLOTHING_TOPS_JACKET";

		// Token: 0x040086A2 RID: 34466
		public const string CLOTHING_TOPS_UNDERSHIRT = "CLOTHING_TOPS_UNDERSHIRT";

		// Token: 0x040086A3 RID: 34467
		public const string CLOTHING_TOPS_DRESS = "CLOTHING_TOPS_DRESS";

		// Token: 0x040086A4 RID: 34468
		public const string CLOTHING_BOTTOMS_BASIC = "CLOTHING_BOTTOMS_BASIC";

		// Token: 0x040086A5 RID: 34469
		public const string CLOTHING_BOTTOMS_FANCY = "CLOTHING_BOTTOMS_FANCY";

		// Token: 0x040086A6 RID: 34470
		public const string CLOTHING_BOTTOMS_SHORTS = "CLOTHING_BOTTOMS_SHORTS";

		// Token: 0x040086A7 RID: 34471
		public const string CLOTHING_BOTTOMS_SKIRTS = "CLOTHING_BOTTOMS_SKIRTS";

		// Token: 0x040086A8 RID: 34472
		public const string CLOTHING_BOTTOMS_UNDERWEAR = "CLOTHING_BOTTOMS_UNDERWEAR";

		// Token: 0x040086A9 RID: 34473
		public const string CLOTHING_GLOVES_BASIC = "CLOTHING_GLOVES_BASIC";

		// Token: 0x040086AA RID: 34474
		public const string CLOTHING_GLOVES_PRINTS = "CLOTHING_GLOVES_PRINTS";

		// Token: 0x040086AB RID: 34475
		public const string CLOTHING_GLOVES_SHORT = "CLOTHING_GLOVES_SHORT";

		// Token: 0x040086AC RID: 34476
		public const string CLOTHING_GLOVES_FORMAL = "CLOTHING_GLOVES_FORMAL";

		// Token: 0x040086AD RID: 34477
		public const string CLOTHING_SHOES_BASIC = "CLOTHING_SHOES_BASIC";

		// Token: 0x040086AE RID: 34478
		public const string CLOTHING_SHOES_FANCY = "CLOTHING_SHOES_FANCY";

		// Token: 0x040086AF RID: 34479
		public const string CLOTHING_SHOE_SOCKS = "CLOTHING_SHOE_SOCKS";

		// Token: 0x040086B0 RID: 34480
		public const string ATMOSUIT_HELMETS_BASIC = "ATMOSUIT_HELMETS_BASIC";

		// Token: 0x040086B1 RID: 34481
		public const string ATMOSUIT_HELMETS_FANCY = "ATMOSUIT_HELMETS_FANCY";

		// Token: 0x040086B2 RID: 34482
		public const string ATMOSUIT_BODIES_BASIC = "ATMOSUIT_BODIES_BASIC";

		// Token: 0x040086B3 RID: 34483
		public const string ATMOSUIT_BODIES_FANCY = "ATMOSUIT_BODIES_FANCY";

		// Token: 0x040086B4 RID: 34484
		public const string ATMOSUIT_GLOVES_BASIC = "ATMOSUIT_GLOVES_BASIC";

		// Token: 0x040086B5 RID: 34485
		public const string ATMOSUIT_GLOVES_FANCY = "ATMOSUIT_GLOVES_FANCY";

		// Token: 0x040086B6 RID: 34486
		public const string ATMOSUIT_BELTS_BASIC = "ATMOSUIT_BELTS_BASIC";

		// Token: 0x040086B7 RID: 34487
		public const string ATMOSUIT_BELTS_FANCY = "ATMOSUIT_BELTS_FANCY";

		// Token: 0x040086B8 RID: 34488
		public const string ATMOSUIT_SHOES_BASIC = "ATMOSUIT_SHOES_BASIC";

		// Token: 0x040086B9 RID: 34489
		public const string ATMOSUIT_SHOES_FANCY = "ATMOSUIT_SHOES_FANCY";

		// Token: 0x040086BA RID: 34490
		public const string BUILDING_WALLPAPER_BASIC = "BUILDING_WALLPAPER_BASIC";

		// Token: 0x040086BB RID: 34491
		public const string BUILDING_WALLPAPER_FANCY = "BUILDING_WALLPAPER_FANCY";

		// Token: 0x040086BC RID: 34492
		public const string BUILDING_WALLPAPER_PRINTS = "BUILDING_WALLPAPER_PRINTS";

		// Token: 0x040086BD RID: 34493
		public const string BUILDINGS_STORAGE = "BUILDINGS_STORAGE";

		// Token: 0x040086BE RID: 34494
		public const string BUILDINGS_INDUSTRIAL = "BUILDINGS_INDUSTRIAL";

		// Token: 0x040086BF RID: 34495
		public const string BUILDINGS_FOOD = "BUILDINGS_FOOD";

		// Token: 0x040086C0 RID: 34496
		public const string BUILDINGS_RANCHING = "BUILDINGS_RANCHING";

		// Token: 0x040086C1 RID: 34497
		public const string BUILDINGS_WASHROOM = "BUILDINGS_WASHROOM";

		// Token: 0x040086C2 RID: 34498
		public const string BUILDINGS_RECREATION = "BUILDINGS_RECREATION";

		// Token: 0x040086C3 RID: 34499
		public const string BUILDINGS_PRINTING_POD = "BUILDINGS_PRINTING_POD";

		// Token: 0x040086C4 RID: 34500
		public const string BUILDING_CANVAS_STANDARD = "BUILDING_CANVAS_STANDARD";

		// Token: 0x040086C5 RID: 34501
		public const string BUILDING_CANVAS_PORTRAIT = "BUILDING_CANVAS_PORTRAIT";

		// Token: 0x040086C6 RID: 34502
		public const string BUILDING_CANVAS_LANDSCAPE = "BUILDING_CANVAS_LANDSCAPE";

		// Token: 0x040086C7 RID: 34503
		public const string BUILDING_SCULPTURE = "BUILDING_SCULPTURE";

		// Token: 0x040086C8 RID: 34504
		public const string MONUMENT_PARTS = "MONUMENT_PARTS";

		// Token: 0x040086C9 RID: 34505
		public const string BUILDINGS_FLOWER_VASE = "BUILDINGS_FLOWER_VASE";

		// Token: 0x040086CA RID: 34506
		public const string BUILDINGS_BED_COT = "BUILDINGS_BED_COT";

		// Token: 0x040086CB RID: 34507
		public const string BUILDINGS_BED_LUXURY = "BUILDINGS_BED_LUXURY";

		// Token: 0x040086CC RID: 34508
		public const string BUILDING_CEILING_LIGHT = "BUILDING_CEILING_LIGHT";
	}
}

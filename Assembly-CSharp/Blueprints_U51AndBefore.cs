﻿using System;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;

// Token: 0x02000521 RID: 1313
public class Blueprints_U51AndBefore : BlueprintProvider
{
	// Token: 0x06001D65 RID: 7525 RVA: 0x0009B9F0 File Offset: 0x00099BF0
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x0009B9F7 File Offset: 0x00099BF7
	public override void SetupBlueprints()
	{
		this.SetupBuildingFacades();
		this.SetupArtables();
		this.SetupClothingItems();
		this.SetupClothingOutfits();
		this.SetupBalloonArtistFacades();
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x0009BA18 File Offset: 0x00099C18
	public void SetupBuildingFacades()
	{
		this.blueprintCollection.buildingFacades.AddRange(new BuildingFacadeInfo[]
		{
			new BuildingFacadeInfo("ExteriorWall_basic_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_WHITE.DESC, PermitRarity.Universal, "ExteriorWall", "walls_basic_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVase_retro", BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_SUNNY.NAME, BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_SUNNY.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_yellow_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVase_retro_red", BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_BOLD.NAME, BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_BOLD.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_red_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVase_retro_white", BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_ELEGANT.NAME, BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_ELEGANT.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVase_retro_green", BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_BRIGHT.NAME, BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_BRIGHT.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_green_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVase_retro_blue", BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_DREAMY.NAME, BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_DREAMY.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_blue_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_boat", BUILDINGS.PREFABS.LUXURYBED.FACADES.BOAT.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.BOAT.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_boat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_bouncy", BUILDINGS.PREFABS.LUXURYBED.FACADES.BOUNCY_BED.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.BOUNCY_BED.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_bouncy_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_grandprix", BUILDINGS.PREFABS.LUXURYBED.FACADES.GRANDPRIX.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.GRANDPRIX.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_grandprix_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_rocket", BUILDINGS.PREFABS.LUXURYBED.FACADES.ROCKET_BED.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.ROCKET_BED.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_rocket_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_puft", BUILDINGS.PREFABS.LUXURYBED.FACADES.PUFT_BED.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.PUFT_BED.DESC, PermitRarity.Loyalty, "LuxuryBed", "elegantbed_puft_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_pastel_pink", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPINK.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPINK.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_pink_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_pastel_yellow", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELYELLOW.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELYELLOW.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_yellow_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_pastel_green", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELGREEN.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELGREEN.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_green_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_pastel_blue", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELBLUE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELBLUE.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_blue_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_pastel_purple", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPURPLE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPURPLE.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_purple_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_balm_lily", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BALM_LILY.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BALM_LILY.DESC, PermitRarity.Decent, "ExteriorWall", "walls_balm_lily_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_clouds", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CLOUDS.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CLOUDS.DESC, PermitRarity.Decent, "ExteriorWall", "walls_clouds_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_coffee", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.COFFEE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.COFFEE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_coffee_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_mosaic", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.AQUATICMOSAIC.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.AQUATICMOSAIC.DESC, PermitRarity.Decent, "ExteriorWall", "walls_mosaic_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_mushbar", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.MUSHBAR.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.MUSHBAR.DESC, PermitRarity.Decent, "ExteriorWall", "walls_mushbar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plaid", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLAID.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLAID.DESC, PermitRarity.Decent, "ExteriorWall", "walls_plaid_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_rain", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.RAIN.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.RAIN.DESC, PermitRarity.Decent, "ExteriorWall", "walls_rain_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_rainbow", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.RAINBOW.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.RAINBOW.DESC, PermitRarity.Decent, "ExteriorWall", "walls_rainbow_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_snow", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SNOW.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SNOW.DESC, PermitRarity.Decent, "ExteriorWall", "walls_snow_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_sun", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SUN.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SUN.DESC, PermitRarity.Decent, "ExteriorWall", "walls_sun_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_polka", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPOLKA.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPOLKA.DESC, PermitRarity.Decent, "ExteriorWall", "walls_polka_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_diagonal_red_deep_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_RED_DEEP_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_RED_DEEP_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_red_deep_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_diagonal_orange_satsuma_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_ORANGE_SATSUMA_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_ORANGE_SATSUMA_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_orange_satsuma_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_diagonal_yellow_lemon_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_YELLOW_LEMON_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_YELLOW_LEMON_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_yellow_lemon_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_diagonal_green_kelly_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_GREEN_KELLY_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_GREEN_KELLY_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_green_kelly_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_diagonal_blue_cobalt_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_BLUE_COBALT_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_BLUE_COBALT_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_blue_cobalt_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_diagonal_pink_flamingo_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_PINK_FLAMINGO_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_PINK_FLAMINGO_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_pink_flamingo_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_diagonal_grey_charcoal_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_GREY_CHARCOAL_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_GREY_CHARCOAL_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_grey_charcoal_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_circle_red_deep_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_RED_DEEP_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_RED_DEEP_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_red_deep_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_circle_orange_satsuma_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_ORANGE_SATSUMA_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_ORANGE_SATSUMA_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_orange_satsuma_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_circle_yellow_lemon_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_YELLOW_LEMON_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_YELLOW_LEMON_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_yellow_lemon_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_circle_green_kelly_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_GREEN_KELLY_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_GREEN_KELLY_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_green_kelly_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_circle_blue_cobalt_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_BLUE_COBALT_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_BLUE_COBALT_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_blue_cobalt_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_circle_pink_flamingo_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_PINK_FLAMINGO_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_PINK_FLAMINGO_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_pink_flamingo_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_circle_grey_charcoal_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_GREY_CHARCOAL_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_GREY_CHARCOAL_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_grey_charcoal_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Bed_star_curtain", BUILDINGS.PREFABS.BED.FACADES.STARCURTAIN.NAME, BUILDINGS.PREFABS.BED.FACADES.STARCURTAIN.DESC, PermitRarity.Nifty, "Bed", "bed_star_curtain_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Bed_canopy", BUILDINGS.PREFABS.BED.FACADES.CREAKY.NAME, BUILDINGS.PREFABS.BED.FACADES.CREAKY.DESC, PermitRarity.Nifty, "Bed", "bed_canopy_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Bed_rowan_tropical", BUILDINGS.PREFABS.BED.FACADES.STAYCATION.NAME, BUILDINGS.PREFABS.BED.FACADES.STAYCATION.DESC, PermitRarity.Nifty, "Bed", "bed_rowan_tropical_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Bed_ada_science_lab", BUILDINGS.PREFABS.BED.FACADES.SCIENCELAB.NAME, BUILDINGS.PREFABS.BED.FACADES.SCIENCELAB.DESC, PermitRarity.Nifty, "Bed", "bed_ada_science_lab_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("CeilingLight_mining", BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.MINING.NAME, BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.MINING.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_mining_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("CeilingLight_flower", BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.BLOSSOM.NAME, BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.BLOSSOM.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_flower_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("CeilingLight_polka_lamp_shade", BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.POLKADOT.NAME, BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.POLKADOT.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_polka_lamp_shade_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("CeilingLight_burt_shower", BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.FAUXPIPE.NAME, BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.FAUXPIPE.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_burt_shower_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("CeilingLight_ada_flask_round", BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.LABFLASK.NAME, BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.LABFLASK.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_ada_flask_round_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseWall_retro_green", BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_GREEN.NAME, BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_GREEN.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_green_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseWall_retro_yellow", BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_YELLOW.NAME, BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_YELLOW.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_yellow_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseWall_retro_red", BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_RED.NAME, BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_RED.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_red_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseWall_retro_blue", BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_BLUE.NAME, BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_BLUE.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_blue_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseWall_retro_white", BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_WHITE.NAME, BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_WHITE.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_basic_blue_cobalt", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_BLUE_COBALT.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_BLUE_COBALT.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_blue_cobalt_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_basic_green_kelly", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_GREEN_KELLY.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_GREEN_KELLY.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_green_kelly_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_basic_grey_charcoal", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_GREY_CHARCOAL.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_GREY_CHARCOAL.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_grey_charcoal_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_basic_orange_satsuma", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_ORANGE_SATSUMA.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_ORANGE_SATSUMA.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_orange_satsuma_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_basic_pink_flamingo", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_PINK_FLAMINGO.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_PINK_FLAMINGO.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_pink_flamingo_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_basic_red_deep", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_RED_DEEP.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_RED_DEEP.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_red_deep_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_basic_yellow_lemon", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_YELLOW_LEMON.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_YELLOW_LEMON.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_yellow_lemon_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_blueberries", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BLUEBERRIES.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BLUEBERRIES.DESC, PermitRarity.Decent, "ExteriorWall", "walls_blueberries_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_grapes", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.GRAPES.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.GRAPES.DESC, PermitRarity.Decent, "ExteriorWall", "walls_grapes_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_lemon", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.LEMON.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.LEMON.DESC, PermitRarity.Decent, "ExteriorWall", "walls_lemon_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_lime", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.LIME.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.LIME.DESC, PermitRarity.Decent, "ExteriorWall", "walls_lime_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_satsuma", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SATSUMA.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SATSUMA.DESC, PermitRarity.Decent, "ExteriorWall", "walls_satsuma_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_strawberry", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRAWBERRY.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRAWBERRY.DESC, PermitRarity.Decent, "ExteriorWall", "walls_strawberry_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_watermelon", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.WATERMELON.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.WATERMELON.DESC, PermitRarity.Decent, "ExteriorWall", "walls_watermelon_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseHanging_retro_red", BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_RED.NAME, BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_RED.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_red_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseHanging_retro_green", BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_GREEN.NAME, BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_GREEN.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_green_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseHanging_retro_blue", BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_BLUE.NAME, BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_BLUE.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_blue_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseHanging_retro_yellow", BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_YELLOW.NAME, BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_YELLOW.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_yellow_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseHanging_retro_white", BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_WHITE.NAME, BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_WHITE.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_toiletpaper", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TOILETPAPER.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TOILETPAPER.DESC, PermitRarity.Decent, "ExteriorWall", "walls_toiletpaper_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plunger", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUNGER.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUNGER.DESC, PermitRarity.Decent, "ExteriorWall", "walls_plunger_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_tropical", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TROPICAL.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TROPICAL.DESC, PermitRarity.Decent, "ExteriorWall", "walls_tropical_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ItemPedestal_hand", BUILDINGS.PREFABS.ITEMPEDESTAL.FACADES.HAND.NAME, BUILDINGS.PREFABS.ITEMPEDESTAL.FACADES.HAND.DESC, PermitRarity.Decent, "ItemPedestal", "pedestal_hand_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("MassageTable_shiatsu", BUILDINGS.PREFABS.MASSAGETABLE.FACADES.SHIATSU.NAME, BUILDINGS.PREFABS.MASSAGETABLE.FACADES.SHIATSU.DESC, PermitRarity.Splendid, "MassageTable", "masseur_shiatsu_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("RockCrusher_hands", BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.HANDS.NAME, BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.HANDS.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_hands_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("RockCrusher_teeth", BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.TEETH.NAME, BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.TEETH.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_teeth_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WaterCooler_round_body", BUILDINGS.PREFABS.WATERCOOLER.FACADES.ROUND_BODY.NAME, BUILDINGS.PREFABS.WATERCOOLER.FACADES.ROUND_BODY.DESC, PermitRarity.Splendid, "WaterCooler", "watercooler_round_body_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_blue", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_BLUE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_BLUE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_blue_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_blue", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_BLUE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_BLUE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_blue_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_circle_blue", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_BLUE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_BLUE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_blue_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_squares_red_deep_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_RED_DEEP_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_RED_DEEP_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_red_deep_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_squares_orange_satsuma_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_ORANGE_SATSUMA_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_ORANGE_SATSUMA_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_orange_satsuma_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_squares_yellow_lemon_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_YELLOW_LEMON_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_YELLOW_LEMON_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_yellow_lemon_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_squares_green_kelly_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_GREEN_KELLY_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_GREEN_KELLY_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_green_kelly_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_squares_blue_cobalt_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_BLUE_COBALT_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_BLUE_COBALT_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_blue_cobalt_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_squares_pink_flamingo_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_PINK_FLAMINGO_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_PINK_FLAMINGO_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_pink_flamingo_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_squares_grey_charcoal_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_GREY_CHARCOAL_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_GREY_CHARCOAL_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_grey_charcoal_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("EggCracker_beaker", BUILDINGS.PREFABS.EGGCRACKER.FACADES.BEAKER.NAME, BUILDINGS.PREFABS.EGGCRACKER.FACADES.BEAKER.DESC, PermitRarity.Nifty, "EggCracker", "egg_cracker_beaker_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("EggCracker_flower", BUILDINGS.PREFABS.EGGCRACKER.FACADES.FLOWER.NAME, BUILDINGS.PREFABS.EGGCRACKER.FACADES.FLOWER.DESC, PermitRarity.Nifty, "EggCracker", "egg_cracker_flower_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("EggCracker_hands", BUILDINGS.PREFABS.EGGCRACKER.FACADES.HANDS.NAME, BUILDINGS.PREFABS.EGGCRACKER.FACADES.HANDS.DESC, PermitRarity.Nifty, "EggCracker", "egg_cracker_hands_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("CeilingLight_rubiks", BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.RUBIKS.NAME, BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.RUBIKS.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_rubiks_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseHanging_beaker", BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.BEAKER.NAME, BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.BEAKER.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_beaker_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlowerVaseHanging_rubiks", BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RUBIKS.NAME, BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RUBIKS.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_rubiks_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_hand", BUILDINGS.PREFABS.LUXURYBED.FACADES.HAND.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.HAND.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_hand_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_rubiks", BUILDINGS.PREFABS.LUXURYBED.FACADES.RUBIKS.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.RUBIKS.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_rubiks_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("RockCrusher_roundstamp", BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.ROUNDSTAMP.NAME, BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.ROUNDSTAMP.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_roundstamp_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("RockCrusher_spikebeds", BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.SPIKEBEDS.NAME, BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.SPIKEBEDS.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_spikebeds_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("StorageLocker_green_mush", BUILDINGS.PREFABS.STORAGELOCKER.FACADES.GREEN_MUSH.NAME, BUILDINGS.PREFABS.STORAGELOCKER.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_green_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("StorageLocker_red_rose", BUILDINGS.PREFABS.STORAGELOCKER.FACADES.RED_ROSE.NAME, BUILDINGS.PREFABS.STORAGELOCKER.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_red_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("StorageLocker_blue_babytears", BUILDINGS.PREFABS.STORAGELOCKER.FACADES.BLUE_BABYTEARS.NAME, BUILDINGS.PREFABS.STORAGELOCKER.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_blue_babytears_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("StorageLocker_purple_brainfat", BUILDINGS.PREFABS.STORAGELOCKER.FACADES.PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.STORAGELOCKER.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("StorageLocker_yellow_tartar", BUILDINGS.PREFABS.STORAGELOCKER.FACADES.YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.STORAGELOCKER.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("PlanterBox_mealwood", BUILDINGS.PREFABS.PLANTERBOX.FACADES.MEALWOOD.NAME, BUILDINGS.PREFABS.PLANTERBOX.FACADES.MEALWOOD.DESC, PermitRarity.Common, "PlanterBox", "planterbox_skin_mealwood_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("PlanterBox_bristleblossom", BUILDINGS.PREFABS.PLANTERBOX.FACADES.BRISTLEBLOSSOM.NAME, BUILDINGS.PREFABS.PLANTERBOX.FACADES.BRISTLEBLOSSOM.DESC, PermitRarity.Common, "PlanterBox", "planterbox_skin_bristleblossom_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("PlanterBox_wheezewort", BUILDINGS.PREFABS.PLANTERBOX.FACADES.WHEEZEWORT.NAME, BUILDINGS.PREFABS.PLANTERBOX.FACADES.WHEEZEWORT.DESC, PermitRarity.Decent, "PlanterBox", "planterbox_skin_wheezewort_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("PlanterBox_sleetwheat", BUILDINGS.PREFABS.PLANTERBOX.FACADES.SLEETWHEAT.NAME, BUILDINGS.PREFABS.PLANTERBOX.FACADES.SLEETWHEAT.DESC, PermitRarity.Common, "PlanterBox", "planterbox_skin_sleetwheat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("PlanterBox_salmon_pink", BUILDINGS.PREFABS.PLANTERBOX.FACADES.SALMON_PINK.NAME, BUILDINGS.PREFABS.PLANTERBOX.FACADES.SALMON_PINK.DESC, PermitRarity.Common, "PlanterBox", "planterbox_skin_salmon_pink_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_lightgold", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.LIGHTGOLD.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.LIGHTGOLD.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_lightgold_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_peagreen", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.PEAGREEN.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.PEAGREEN.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_peagreen_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_lightcobalt", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.LIGHTCOBALT.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.LIGHTCOBALT.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_lightcobalt_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_polka_darkpurpleresin", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.POLKA_DARKPURPLERESIN.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.POLKA_DARKPURPLERESIN.DESC, PermitRarity.Splendid, "GasReservoir", "gasstorage_polka_darkpurpleresin_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_polka_darknavynookgreen", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.POLKA_DARKNAVYNOOKGREEN.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.POLKA_DARKNAVYNOOKGREEN.DESC, PermitRarity.Splendid, "GasReservoir", "gasstorage_polka_darknavynookgreen_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_kitchen_retro1", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.KITCHEN_RETRO1.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.KITCHEN_RETRO1.DESC, PermitRarity.Decent, "ExteriorWall", "walls_kitchen_retro1_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plus_red_deep_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_RED_DEEP_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_RED_DEEP_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_red_deep_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plus_orange_satsuma_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_ORANGE_SATSUMA_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_ORANGE_SATSUMA_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_orange_satsuma_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plus_yellow_lemon_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_YELLOW_LEMON_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_YELLOW_LEMON_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_yellow_lemon_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plus_green_kelly_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_GREEN_KELLY_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_GREEN_KELLY_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_green_kelly_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plus_blue_cobalt_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_BLUE_COBALT_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_BLUE_COBALT_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_blue_cobalt_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plus_pink_flamingo_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_PINK_FLAMINGO_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_PINK_FLAMINGO_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_pink_flamingo_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_plus_grey_charcoal_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_GREY_CHARCOAL_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_GREY_CHARCOAL_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_grey_charcoal_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("RockCrusher_chomp", BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.CHOMP.NAME, BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.CHOMP.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_chomp_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("RockCrusher_gears", BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.GEARS.NAME, BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.GEARS.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_gears_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("RockCrusher_balloon", BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.BALLOON.NAME, BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.BALLOON.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_balloon_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("StorageLocker_polka_darknavynookgreen", BUILDINGS.PREFABS.STORAGELOCKER.FACADES.POLKA_DARKNAVYNOOKGREEN.NAME, BUILDINGS.PREFABS.STORAGELOCKER.FACADES.POLKA_DARKNAVYNOOKGREEN.DESC, PermitRarity.Splendid, "StorageLocker", "storagelocker_polka_darknavynookgreen_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("StorageLocker_polka_darkpurpleresin", BUILDINGS.PREFABS.STORAGELOCKER.FACADES.POLKA_DARKPURPLERESIN.NAME, BUILDINGS.PREFABS.STORAGELOCKER.FACADES.POLKA_DARKPURPLERESIN.DESC, PermitRarity.Splendid, "StorageLocker", "storagelocker_polka_darkpurpleresin_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_blue_babytears", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.BLUE_BABYTEARS.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_blue_babytears_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_yellow_tartar", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_green_mush", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.GREEN_MUSH.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_green_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_red_rose", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.RED_ROSE.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_red_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("GasReservoir_purple_brainfat", BUILDINGS.PREFABS.GASRESERVOIR.FACADES.PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.GASRESERVOIR.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("MassageTable_balloon", BUILDINGS.PREFABS.MASSAGETABLE.FACADES.MASSEUR_BALLOON.NAME, BUILDINGS.PREFABS.MASSAGETABLE.FACADES.MASSEUR_BALLOON.DESC, PermitRarity.Splendid, "MassageTable", "masseur_balloon_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WaterCooler_balloon", BUILDINGS.PREFABS.WATERCOOLER.FACADES.BALLOON.NAME, BUILDINGS.PREFABS.WATERCOOLER.FACADES.BALLOON.DESC, PermitRarity.Splendid, "WaterCooler", "watercooler_balloon_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Bed_stringlights", BUILDINGS.PREFABS.BED.FACADES.STRINGLIGHTS.NAME, BUILDINGS.PREFABS.BED.FACADES.STRINGLIGHTS.DESC, PermitRarity.Nifty, "Bed", "bed_stringlights_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("CornerMoulding_shineornaments", BUILDINGS.PREFABS.CORNERMOULDING.FACADES.SHINEORNAMENTS.NAME, BUILDINGS.PREFABS.CORNERMOULDING.FACADES.SHINEORNAMENTS.DESC, PermitRarity.Decent, "CornerMoulding", "corner_tile_shineornaments_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("CrownMoulding_shineornaments", BUILDINGS.PREFABS.CROWNMOULDING.FACADES.SHINEORNAMENTS.NAME, BUILDINGS.PREFABS.CROWNMOULDING.FACADES.SHINEORNAMENTS.DESC, PermitRarity.Decent, "CrownMoulding", "crown_moulding_shineornaments_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FloorLamp_leg", BUILDINGS.PREFABS.FLOORLAMP.FACADES.LEG.NAME, BUILDINGS.PREFABS.FLOORLAMP.FACADES.LEG.DESC, PermitRarity.Decent, "FloorLamp", "floorlamp_leg_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FloorLamp_bristle_blossom", BUILDINGS.PREFABS.FLOORLAMP.FACADES.BRISTLEBLOSSOM.NAME, BUILDINGS.PREFABS.FLOORLAMP.FACADES.BRISTLEBLOSSOM.DESC, PermitRarity.Decent, "FloorLamp", "floorlamp_bristle_blossom_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_rose", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_ROSE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_ROSE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_rose", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_ROSE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_ROSE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_circle_rose", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_ROSE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_ROSE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_mush", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_MUSH.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_MUSH.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_mush", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_MUSH.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_MUSH.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_circle_mush", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_MUSH.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_MUSH.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("StorageLocker_stripes_red_white", BUILDINGS.PREFABS.STORAGELOCKER.FACADES.STRIPES_RED_WHITE.NAME, BUILDINGS.PREFABS.STORAGELOCKER.FACADES.STRIPES_RED_WHITE.DESC, PermitRarity.Splendid, "StorageLocker", "storagelocker_stripes_red_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Refrigerator_stripes_red_white", BUILDINGS.PREFABS.REFRIGERATOR.FACADES.STRIPES_RED_WHITE.NAME, BUILDINGS.PREFABS.REFRIGERATOR.FACADES.STRIPES_RED_WHITE.DESC, PermitRarity.Splendid, "Refrigerator", "fridge_stripes_red_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Refrigerator_blue_babytears", BUILDINGS.PREFABS.REFRIGERATOR.FACADES.BLUE_BABYTEARS.NAME, BUILDINGS.PREFABS.REFRIGERATOR.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_blue_babytears_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Refrigerator_green_mush", BUILDINGS.PREFABS.REFRIGERATOR.FACADES.GREEN_MUSH.NAME, BUILDINGS.PREFABS.REFRIGERATOR.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_green_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Refrigerator_red_rose", BUILDINGS.PREFABS.REFRIGERATOR.FACADES.RED_ROSE.NAME, BUILDINGS.PREFABS.REFRIGERATOR.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_red_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Refrigerator_yellow_tartar", BUILDINGS.PREFABS.REFRIGERATOR.FACADES.YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.REFRIGERATOR.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("Refrigerator_purple_brainfat", BUILDINGS.PREFABS.REFRIGERATOR.FACADES.PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.REFRIGERATOR.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("MicrobeMusher_purple_brainfat", BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("MicrobeMusher_yellow_tartar", BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("MicrobeMusher_red_rose", BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.RED_ROSE.NAME, BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_red_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("MicrobeMusher_green_mush", BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.GREEN_MUSH.NAME, BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_green_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("MicrobeMusher_blue_babytears", BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.BLUE_BABYTEARS.NAME, BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_blue_babytears_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WashSink_purple_brainfat", BUILDINGS.PREFABS.WASHSINK.FACADES.PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.WASHSINK.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WashSink_blue_babytears", BUILDINGS.PREFABS.WASHSINK.FACADES.BLUE_BABYTEARS.NAME, BUILDINGS.PREFABS.WASHSINK.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_blue_babytears_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WashSink_green_mush", BUILDINGS.PREFABS.WASHSINK.FACADES.GREEN_MUSH.NAME, BUILDINGS.PREFABS.WASHSINK.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_green_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WashSink_yellow_tartar", BUILDINGS.PREFABS.WASHSINK.FACADES.YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.WASHSINK.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WashSink_red_rose", BUILDINGS.PREFABS.WASHSINK.FACADES.RED_ROSE.NAME, BUILDINGS.PREFABS.WASHSINK.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_red_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlushToilet_polka_darkpurpleresin", BUILDINGS.PREFABS.FLUSHTOILET.FACADES.POLKA_DARKPURPLERESIN.NAME, BUILDINGS.PREFABS.FLUSHTOILET.FACADES.POLKA_DARKPURPLERESIN.DESC, PermitRarity.Splendid, "FlushToilet", "toiletflush_polka_darkpurpleresin_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlushToilet_polka_darknavynookgreen", BUILDINGS.PREFABS.FLUSHTOILET.FACADES.POLKA_DARKNAVYNOOKGREEN.NAME, BUILDINGS.PREFABS.FLUSHTOILET.FACADES.POLKA_DARKNAVYNOOKGREEN.DESC, PermitRarity.Splendid, "FlushToilet", "toiletflush_polka_darknavynookgreen_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlushToilet_purple_brainfat", BUILDINGS.PREFABS.FLUSHTOILET.FACADES.PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.FLUSHTOILET.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlushToilet_yellow_tartar", BUILDINGS.PREFABS.FLUSHTOILET.FACADES.YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.FLUSHTOILET.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlushToilet_red_rose", BUILDINGS.PREFABS.FLUSHTOILET.FACADES.RED_ROSE.NAME, BUILDINGS.PREFABS.FLUSHTOILET.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_red_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlushToilet_green_mush", BUILDINGS.PREFABS.FLUSHTOILET.FACADES.GREEN_MUSH.NAME, BUILDINGS.PREFABS.FLUSHTOILET.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_green_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("FlushToilet_blue_babytears", BUILDINGS.PREFABS.FLUSHTOILET.FACADES.BLUE_BABYTEARS.NAME, BUILDINGS.PREFABS.FLUSHTOILET.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_blue_babytears_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_red_rose", BUILDINGS.PREFABS.LUXURYBED.FACADES.RED_ROSE.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "LuxuryBed", "elegantbed_red_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_green_mush", BUILDINGS.PREFABS.LUXURYBED.FACADES.GREEN_MUSH.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "LuxuryBed", "elegantbed_green_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_yellow_tartar", BUILDINGS.PREFABS.LUXURYBED.FACADES.YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "LuxuryBed", "elegantbed_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("LuxuryBed_purple_brainfat", BUILDINGS.PREFABS.LUXURYBED.FACADES.PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.LUXURYBED.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "LuxuryBed", "elegantbed_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WaterCooler_yellow_tartar", BUILDINGS.PREFABS.WATERCOOLER.FACADES.YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.WATERCOOLER.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WaterCooler_red_rose", BUILDINGS.PREFABS.WATERCOOLER.FACADES.RED_ROSE.NAME, BUILDINGS.PREFABS.WATERCOOLER.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_red_rose_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WaterCooler_green_mush", BUILDINGS.PREFABS.WATERCOOLER.FACADES.GREEN_MUSH.NAME, BUILDINGS.PREFABS.WATERCOOLER.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_green_mush_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WaterCooler_purple_brainfat", BUILDINGS.PREFABS.WATERCOOLER.FACADES.PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.WATERCOOLER.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("WaterCooler_blue_babytears", BUILDINGS.PREFABS.WATERCOOLER.FACADES.BLUE_BABYTEARS.NAME, BUILDINGS.PREFABS.WATERCOOLER.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_blue_babytears_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_yellow_tartar", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_YELLOW_TARTAR.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_yellow_tartar", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_YELLOW_TARTAR.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_circle_yellow_tartar", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_YELLOW_TARTAR.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_YELLOW_TARTAR.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_yellow_tartar_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_purple_brainfat", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_PURPLE_BRAINFAT.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_purple_brainfat", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_PURPLE_BRAINFAT.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_stripes_circle_purple_brainfat", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_PURPLE_BRAINFAT.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_PURPLE_BRAINFAT.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_purple_brainfat_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_floppy_azulene_vitro", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_AZULENE_VITRO.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_AZULENE_VITRO.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_azulene_vitro_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_floppy_black_white", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_BLACK_WHITE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_BLACK_WHITE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_black_white_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_floppy_peagreen_balmy", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_PEAGREEN_BALMY.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_PEAGREEN_BALMY.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_peagreen_balmy_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_floppy_satsuma_yellowcake", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_SATSUMA_YELLOWCAKE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_SATSUMA_YELLOWCAKE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_satsuma_yellowcake_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_floppy_magma_amino", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_MAGMA_AMINO.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_MAGMA_AMINO.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_magma_amino_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_orange_juice", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.ORANGE_JUICE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.ORANGE_JUICE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_orange_juice_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_paint_blots", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PAINT_BLOTS.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PAINT_BLOTS.DESC, PermitRarity.Decent, "ExteriorWall", "walls_paint_blots_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_telescope", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TELESCOPE.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TELESCOPE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_telescope_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_tictactoe_o", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TICTACTOE_O.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TICTACTOE_O.DESC, PermitRarity.Decent, "ExteriorWall", "walls_tictactoe_o_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_tictactoe_x", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TICTACTOE_X.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TICTACTOE_X.DESC, PermitRarity.Decent, "ExteriorWall", "walls_tictactoe_x_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_dice_1", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_1.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_1.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_1_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_dice_2", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_2.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_2.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_2_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_dice_3", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_3.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_3.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_3_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_dice_4", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_4.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_4.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_4_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_dice_5", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_5.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_5.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_5_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null),
			new BuildingFacadeInfo("ExteriorWall_dice_6", BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_6.NAME, BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_6.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_6_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null)
		});
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x0009E388 File Offset: 0x0009C588
	private void SetupArtables()
	{
		this.blueprintCollection.artables.AddRange(new ArtableInfo[]
		{
			new ArtableInfo("Canvas_Good7", BUILDINGS.PREFABS.CANVAS.FACADES.ART_I.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_I.DESC, PermitRarity.Decent, "painting_art_i_kanim", "art_i", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good8", BUILDINGS.PREFABS.CANVAS.FACADES.ART_J.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_J.DESC, PermitRarity.Decent, "painting_art_j_kanim", "art_j", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good9", BUILDINGS.PREFABS.CANVAS.FACADES.ART_K.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_K.DESC, PermitRarity.Decent, "painting_art_k_kanim", "art_k", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("CanvasTall_Good5", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_G.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_G.DESC, PermitRarity.Decent, "painting_tall_art_g_kanim", "art_g", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasTall_Good6", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_H.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_H.DESC, PermitRarity.Decent, "painting_tall_art_h_kanim", "art_h", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasTall_Good7", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_I.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_I.DESC, PermitRarity.Decent, "painting_tall_art_i_kanim", "art_i", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasWide_Good5", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_G.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_G.DESC, PermitRarity.Decent, "painting_wide_art_g_kanim", "art_g", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("CanvasWide_Good6", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_H.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_H.DESC, PermitRarity.Decent, "painting_wide_art_h_kanim", "art_h", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("CanvasWide_Good7", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_I.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_I.DESC, PermitRarity.Decent, "painting_wide_art_i_kanim", "art_i", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("Sculpture_Good4", BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_4.NAME, BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_4.DESC, PermitRarity.Decent, "sculpture_amazing_4_kanim", "amazing_4", 15, true, "LookingGreat", "Sculpture", ""),
			new ArtableInfo("SmallSculpture_Good4", BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_4.NAME, BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_4.DESC, PermitRarity.Decent, "sculpture_1x2_amazing_4_kanim", "amazing_4", 15, true, "LookingGreat", "SmallSculpture", ""),
			new ArtableInfo("MetalSculpture_Good4", BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_4.NAME, BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_4.DESC, PermitRarity.Decent, "sculpture_metal_amazing_4_kanim", "amazing_4", 15, true, "LookingGreat", "MetalSculpture", ""),
			new ArtableInfo("MarbleSculpture_Good4", BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_4.NAME, BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_4.DESC, PermitRarity.Decent, "sculpture_marble_amazing_4_kanim", "amazing_4", 15, true, "LookingGreat", "MarbleSculpture", ""),
			new ArtableInfo("MarbleSculpture_Good5", BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_5.NAME, BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_5.DESC, PermitRarity.Decent, "sculpture_marble_amazing_5_kanim", "amazing_5", 15, true, "LookingGreat", "MarbleSculpture", ""),
			new ArtableInfo("IceSculpture_Average2", BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_2.NAME, BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_2.DESC, PermitRarity.Decent, "icesculpture_idle_2_kanim", "idle_2", 10, false, "LookingOkay", "IceSculpture", ""),
			new ArtableInfo("Canvas_Good10", BUILDINGS.PREFABS.CANVAS.FACADES.ART_L.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_L.DESC, PermitRarity.Decent, "painting_art_l_kanim", "art_l", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good11", BUILDINGS.PREFABS.CANVAS.FACADES.ART_M.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_M.DESC, PermitRarity.Decent, "painting_art_m_kanim", "art_m", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("CanvasTall_Good8", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_J.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_J.DESC, PermitRarity.Decent, "painting_tall_art_j_kanim", "art_j", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasTall_Good9", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_K.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_K.DESC, PermitRarity.Decent, "painting_tall_art_k_kanim", "art_k", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasWide_Good8", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_J.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_J.DESC, PermitRarity.Decent, "painting_wide_art_j_kanim", "art_j", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("CanvasWide_Good9", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_K.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_K.DESC, PermitRarity.Decent, "painting_wide_art_k_kanim", "art_k", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("Canvas_Good13", BUILDINGS.PREFABS.CANVAS.FACADES.ART_O.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_O.DESC, PermitRarity.Decent, "painting_art_o_kanim", "art_o", 15, true, "LookingGreat", "Canvas", ""),
			new ArtableInfo("CanvasWide_Good10", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_L.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_L.DESC, PermitRarity.Decent, "painting_wide_art_l_kanim", "art_l", 15, true, "LookingGreat", "CanvasWide", ""),
			new ArtableInfo("CanvasTall_Good11", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_M.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_M.DESC, PermitRarity.Decent, "painting_tall_art_m_kanim", "art_m", 15, true, "LookingGreat", "CanvasTall", ""),
			new ArtableInfo("Sculpture_Good5", BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_5.NAME, BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_5.DESC, PermitRarity.Decent, "sculpture_amazing_5_kanim", "amazing_5", 15, true, "LookingGreat", "Sculpture", ""),
			new ArtableInfo("SmallSculpture_Good5", BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_5.NAME, BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_5.DESC, PermitRarity.Decent, "sculpture_1x2_amazing_5_kanim", "amazing_5", 15, true, "LookingGreat", "SmallSculpture", ""),
			new ArtableInfo("SmallSculpture_Good6", BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_6.NAME, BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_6.DESC, PermitRarity.Decent, "sculpture_1x2_amazing_6_kanim", "amazing_6", 15, true, "LookingGreat", "SmallSculpture", ""),
			new ArtableInfo("MetalSculpture_Good5", BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_5.NAME, BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_5.DESC, PermitRarity.Decent, "sculpture_metal_amazing_5_kanim", "amazing_5", 15, true, "LookingGreat", "MetalSculpture", ""),
			new ArtableInfo("IceSculpture_Average3", BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_3.NAME, BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_3.DESC, PermitRarity.Decent, "icesculpture_idle_3_kanim", "idle_3", 10, true, "LookingOkay", "IceSculpture", ""),
			new ArtableInfo("Canvas_Good12", BUILDINGS.PREFABS.CANVAS.FACADES.ART_N.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_N.DESC, PermitRarity.Decent, "painting_art_n_kanim", "art_n", 15, true, "LookingGreat", "Canvas", ""),
			new ArtableInfo("Canvas_Good14", BUILDINGS.PREFABS.CANVAS.FACADES.ART_P.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_P.DESC, PermitRarity.Decent, "painting_art_p_kanim", "art_p", 15, true, "LookingGreat", "Canvas", ""),
			new ArtableInfo("CanvasWide_Good11", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_M.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_M.DESC, PermitRarity.Decent, "painting_wide_art_m_kanim", "art_m", 15, true, "LookingGreat", "CanvasWide", ""),
			new ArtableInfo("CanvasTall_Good10", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_L.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_L.DESC, PermitRarity.Decent, "painting_tall_art_l_kanim", "art_l", 15, true, "LookingGreat", "CanvasTall", ""),
			new ArtableInfo("Sculpture_Good6", BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_6.NAME, BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_6.DESC, PermitRarity.Decent, "sculpture_amazing_6_kanim", "amazing_6", 15, true, "LookingGreat", "Sculpture", ""),
			new ArtableInfo("Canvas_Good15", BUILDINGS.PREFABS.CANVAS.FACADES.ART_Q.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_Q.DESC, PermitRarity.Decent, "painting_art_q_kanim", "art_q", 15, true, "LookingGreat", "Canvas", ""),
			new ArtableInfo("CanvasTall_Good14", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_P.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_P.DESC, PermitRarity.Decent, "painting_tall_art_p_kanim", "art_p", 15, true, "LookingGreat", "CanvasTall", ""),
			new ArtableInfo("Canvas_Good16", BUILDINGS.PREFABS.CANVAS.FACADES.ART_R.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_R.DESC, PermitRarity.Decent, "painting_art_r_kanim", "art_r", 15, true, "LookingGreat", "Canvas", ""),
			new ArtableInfo("CanvasWide_Good13", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_O.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_O.DESC, PermitRarity.Decent, "painting_wide_art_o_kanim", "art_o", 15, true, "LookingGreat", "CanvasWide", "")
		});
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x0009ED00 File Offset: 0x0009CF00
	private void SetupClothingItems()
	{
		this.blueprintCollection.clothingItems.AddRange(new ClothingItemInfo[]
		{
			new ClothingItemInfo("TopBasicBlack", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_BLACK.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_black_kanim"),
			new ClothingItemInfo("TopBasicWhite", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_WHITE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_white_kanim"),
			new ClothingItemInfo("TopBasicRed", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_RED_BURNT.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_RED_BURNT.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_red_kanim"),
			new ClothingItemInfo("TopBasicOrange", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_ORANGE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_ORANGE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_orange_kanim"),
			new ClothingItemInfo("TopBasicYellow", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_YELLOW.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_YELLOW.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_yellow_kanim"),
			new ClothingItemInfo("TopBasicGreen", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_GREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_GREEN.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_green_kanim"),
			new ClothingItemInfo("TopBasicAqua", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_BLUE_MIDDLE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_blue_middle_kanim"),
			new ClothingItemInfo("TopBasicPurple", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_PURPLE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_PURPLE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_purple_kanim"),
			new ClothingItemInfo("TopBasicPinkOrchid", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_PINK_ORCHID.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_PINK_ORCHID.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_pink_orchid_kanim"),
			new ClothingItemInfo("BottomBasicBlack", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_BLACK.DESC, PermitCategory.DupeBottoms, PermitRarity.Universal, "pants_basic_black_kanim"),
			new ClothingItemInfo("BottomBasicWhite", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_WHITE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_white_kanim"),
			new ClothingItemInfo("BottomBasicRed", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_RED.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_RED.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_red_kanim"),
			new ClothingItemInfo("BottomBasicOrange", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_ORANGE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_ORANGE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_orange_kanim"),
			new ClothingItemInfo("BottomBasicYellow", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_YELLOW.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_YELLOW.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_yellow_kanim"),
			new ClothingItemInfo("BottomBasicGreen", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_GREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_GREEN.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_green_kanim"),
			new ClothingItemInfo("BottomBasicAqua", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_BLUE_MIDDLE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_blue_middle_kanim"),
			new ClothingItemInfo("BottomBasicPurple", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_PURPLE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_PURPLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_purple_kanim"),
			new ClothingItemInfo("BottomBasicPinkOrchid", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_PINK_ORCHID.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_PINK_ORCHID.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_pink_orchid_kanim"),
			new ClothingItemInfo("GlovesBasicBlack", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLACK.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_black_kanim"),
			new ClothingItemInfo("GlovesBasicWhite", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_WHITE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_white_kanim"),
			new ClothingItemInfo("GlovesBasicRed", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_RED.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_RED.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_red_kanim"),
			new ClothingItemInfo("GlovesBasicOrange", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_ORANGE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_ORANGE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_orange_kanim"),
			new ClothingItemInfo("GlovesBasicYellow", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_YELLOW.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_YELLOW.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_yellow_kanim"),
			new ClothingItemInfo("GlovesBasicGreen", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_GREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_GREEN.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_green_kanim"),
			new ClothingItemInfo("GlovesBasicAqua", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLUE_MIDDLE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_blue_middle_kanim"),
			new ClothingItemInfo("GlovesBasicPurple", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PURPLE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PURPLE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_purple_kanim"),
			new ClothingItemInfo("GlovesBasicPinkOrchid", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PINK_ORCHID.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PINK_ORCHID.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_pink_orchid_kanim"),
			new ClothingItemInfo("ShoesBasicBlack", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLACK.DESC, PermitCategory.DupeShoes, PermitRarity.Universal, "shoes_basic_black_kanim"),
			new ClothingItemInfo("ShoesBasicWhite", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_WHITE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_white_kanim"),
			new ClothingItemInfo("ShoesBasicRed", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_RED.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_RED.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_red_kanim"),
			new ClothingItemInfo("ShoesBasicOrange", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_ORANGE.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_ORANGE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_orange_kanim"),
			new ClothingItemInfo("ShoesBasicYellow", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_YELLOW.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_YELLOW.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_yellow_kanim"),
			new ClothingItemInfo("ShoesBasicGreen", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_GREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_GREEN.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_green_kanim"),
			new ClothingItemInfo("ShoesBasicAqua", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLUE_MIDDLE.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_blue_middle_kanim"),
			new ClothingItemInfo("ShoesBasicPurple", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_PURPLE.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_PURPLE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_purple_kanim"),
			new ClothingItemInfo("ShoesBasicPinkOrchid", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_PINK_ORCHID.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_PINK_ORCHID.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_pink_orchid_kanim"),
			new ClothingItemInfo("TopRaglanDeepRed", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_DEEPRED.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_DEEPRED.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_deepred_kanim"),
			new ClothingItemInfo("TopRaglanCobalt", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_COBALT.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_COBALT.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_cobalt_kanim"),
			new ClothingItemInfo("TopRaglanFlamingo", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_FLAMINGO.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_FLAMINGO.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_flamingo_kanim"),
			new ClothingItemInfo("TopRaglanKellyGreen", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_KELLYGREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_KELLYGREEN.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_kellygreen_kanim"),
			new ClothingItemInfo("TopRaglanCharcoal", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_CHARCOAL.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_charcoal_kanim"),
			new ClothingItemInfo("TopRaglanLemon", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_LEMON.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_LEMON.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_lemon_kanim"),
			new ClothingItemInfo("TopRaglanSatsuma", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_SATSUMA.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_satsuma_kanim"),
			new ClothingItemInfo("ShortsBasicDeepRed", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_DEEPRED.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_DEEPRED.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_deepred_kanim"),
			new ClothingItemInfo("ShortsBasicSatsuma", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_SATSUMA.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_satsuma_kanim"),
			new ClothingItemInfo("ShortsBasicYellowcake", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_YELLOWCAKE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_YELLOWCAKE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_yellowcake_kanim"),
			new ClothingItemInfo("ShortsBasicKellyGreen", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_KELLYGREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_KELLYGREEN.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_kellygreen_kanim"),
			new ClothingItemInfo("ShortsBasicBlueCobalt", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_BLUE_COBALT.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_BLUE_COBALT.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_blue_cobalt_kanim"),
			new ClothingItemInfo("ShortsBasicPinkFlamingo", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_PINK_FLAMINGO.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_PINK_FLAMINGO.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_pink_flamingo_kanim"),
			new ClothingItemInfo("ShortsBasicCharcoal", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_CHARCOAL.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_charcoal_kanim"),
			new ClothingItemInfo("SocksAthleticDeepRed", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_DEEPRED.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_DEEPRED.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_red_deep_kanim"),
			new ClothingItemInfo("SocksAthleticOrangeSatsuma", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_SATSUMA.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_orange_satsuma_kanim"),
			new ClothingItemInfo("SocksAthleticYellowLemon", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_LEMON.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_LEMON.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_yellow_lemon_kanim"),
			new ClothingItemInfo("SocksAthleticGreenKelly", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_KELLYGREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_KELLYGREEN.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_green_kelly_kanim"),
			new ClothingItemInfo("SocksAthleticBlueCobalt", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_COBALT.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_COBALT.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_blue_cobalt_kanim"),
			new ClothingItemInfo("SocksAthleticPinkFlamingo", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_FLAMINGO.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_FLAMINGO.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_pink_flamingo_kanim"),
			new ClothingItemInfo("SocksAthleticGreyCharcoal", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_CHARCOAL.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_grey_charcoal_kanim"),
			new ClothingItemInfo("GlovesAthleticRedDeep", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_DEEPRED.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_DEEPRED.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_red_deep_kanim"),
			new ClothingItemInfo("GlovesAthleticOrangeSatsuma", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_SATSUMA.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_orange_satsuma_kanim"),
			new ClothingItemInfo("GlovesAthleticYellowLemon", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_LEMON.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_LEMON.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_yellow_lemon_kanim"),
			new ClothingItemInfo("GlovesAthleticGreenKelly", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_KELLYGREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_KELLYGREEN.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_green_kelly_kanim"),
			new ClothingItemInfo("GlovesAthleticBlueCobalt", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_COBALT.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_COBALT.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_blue_cobalt_kanim"),
			new ClothingItemInfo("GlovesAthleticPinkFlamingo", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_FLAMINGO.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_FLAMINGO.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_pink_flamingo_kanim"),
			new ClothingItemInfo("GlovesAthleticGreyCharcoal", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_CHARCOAL.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_grey_charcoal_kanim"),
			new ClothingItemInfo("TopJellypuffJacketBlueberry", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_BLUEBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_BLUEBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_blueberry_kanim"),
			new ClothingItemInfo("TopJellypuffJacketGrape", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_GRAPE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_GRAPE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_grape_kanim"),
			new ClothingItemInfo("TopJellypuffJacketLemon", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_LEMON.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_LEMON.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_lemon_kanim"),
			new ClothingItemInfo("TopJellypuffJacketLime", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_LIME.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_LIME.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_lime_kanim"),
			new ClothingItemInfo("TopJellypuffJacketSatsuma", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_SATSUMA.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_satsuma_kanim"),
			new ClothingItemInfo("TopJellypuffJacketStrawberry", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_STRAWBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_STRAWBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_strawberry_kanim"),
			new ClothingItemInfo("TopJellypuffJacketWatermelon", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_WATERMELON.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_WATERMELON.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_watermelon_kanim"),
			new ClothingItemInfo("GlovesCufflessBlueberry", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_BLUEBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_BLUEBERRY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_blueberry_kanim"),
			new ClothingItemInfo("GlovesCufflessGrape", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_GRAPE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_GRAPE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_grape_kanim"),
			new ClothingItemInfo("GlovesCufflessLemon", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_LEMON.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_LEMON.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_lemon_kanim"),
			new ClothingItemInfo("GlovesCufflessLime", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_LIME.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_LIME.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_lime_kanim"),
			new ClothingItemInfo("GlovesCufflessSatsuma", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_SATSUMA.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_satsuma_kanim"),
			new ClothingItemInfo("GlovesCufflessStrawberry", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_STRAWBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_STRAWBERRY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_strawberry_kanim"),
			new ClothingItemInfo("GlovesCufflessWatermelon", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_WATERMELON.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_WATERMELON.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_watermelon_kanim"),
			new ClothingItemInfo("visonly_AtmoHelmetClear", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Universal, "atmo_helmet_clear_kanim"),
			new ClothingItemInfo("visonly_AtmoSuitBasicBlue", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Universal, "atmosuit_basic_blue_kanim"),
			new ClothingItemInfo("visonly_AtmoGlovesBasicBlue", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Universal, "atmo_gloves_blue_kanim"),
			new ClothingItemInfo("visonly_AtmoBeltBasicBlue", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Universal, "atmo_belt_basic_blue_kanim"),
			new ClothingItemInfo("visonly_AtmoShoesBasicBlack", EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Universal, "atmo_shoes_basic_black_kanim"),
			new ClothingItemInfo("AtmoHelmetLimone", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.LIMONE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Universal, "atmo_helmet_limone_lime_kanim"),
			new ClothingItemInfo("AtmoSuitBasicYellow", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.LIMONE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Universal, "atmosuit_basic_yellow_kanim"),
			new ClothingItemInfo("AtmoGlovesLime", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.LIMONE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Universal, "atmo_gloves_lime_kanim"),
			new ClothingItemInfo("AtmoBeltBasicLime", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.LIMONE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Universal, "atmo_belt_basic_lime_kanim"),
			new ClothingItemInfo("AtmoShoesBasicYellow", EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.LIMONE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Universal, "atmo_shoes_basic_yellow_kanim"),
			new ClothingItemInfo("AtmoHelmetPuft", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.PUFT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.PUFT.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Loyalty, "atmo_helmet_puft_kanim"),
			new ClothingItemInfo("AtmoSuitPuft", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.PUFT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.PUFT.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Loyalty, "atmosuit_puft_kanim"),
			new ClothingItemInfo("AtmoGlovesPuft", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.PUFT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.PUFT.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Loyalty, "atmo_gloves_puft_kanim"),
			new ClothingItemInfo("AtmoBeltPuft", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.PUFT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.PUFT.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Loyalty, "atmo_belt_puft_kanim"),
			new ClothingItemInfo("AtmoShoesPuft", EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.PUFT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.PUFT.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Loyalty, "atmo_shoes_puft_kanim"),
			new ClothingItemInfo("TopTShirtWhite", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TSHIRT_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TSHIRT_WHITE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_tshirt_white_kanim"),
			new ClothingItemInfo("TopTShirtMagenta", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TSHIRT_MAGENTA.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TSHIRT_MAGENTA.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_tshirt_magenta_kanim"),
			new ClothingItemInfo("TopAthlete", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.ATHLETE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.ATHLETE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_athlete_kanim"),
			new ClothingItemInfo("TopCircuitGreen", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.CIRCUIT_GREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.CIRCUIT_GREEN.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_circuit_green_kanim"),
			new ClothingItemInfo("GlovesBasicBlueGrey", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLUEGREY.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLUEGREY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_bluegrey_kanim"),
			new ClothingItemInfo("GlovesBasicBrownKhaki", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BROWN_KHAKI.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BROWN_KHAKI.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_brown_khaki_kanim"),
			new ClothingItemInfo("GlovesAthlete", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.ATHLETE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.ATHLETE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_athlete_kanim"),
			new ClothingItemInfo("GlovesCircuitGreen", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CIRCUIT_GREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CIRCUIT_GREEN.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_circuit_green_kanim"),
			new ClothingItemInfo("PantsBasicRedOrange", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_REDORANGE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_REDORANGE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_redorange_kanim"),
			new ClothingItemInfo("PantsBasicLightBrown", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_LIGHTBROWN.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_LIGHTBROWN.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_lightbrown_kanim"),
			new ClothingItemInfo("PantsAthlete", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.ATHLETE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.ATHLETE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_athlete_kanim"),
			new ClothingItemInfo("PantsCircuitGreen", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.CIRCUIT_GREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.CIRCUIT_GREEN.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_circuit_green_kanim"),
			new ClothingItemInfo("ShoesBasicBlueGrey", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLUEGREY.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLUEGREY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_bluegrey_kanim"),
			new ClothingItemInfo("ShoesBasicTan", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_TAN.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_TAN.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_tan_kanim"),
			new ClothingItemInfo("AtmoHelmetSparkleRed", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_RED.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_RED.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_sparkle_red_kanim"),
			new ClothingItemInfo("AtmoHelmetSparkleGreen", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_GREEN.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_GREEN.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_sparkle_green_kanim"),
			new ClothingItemInfo("AtmoHelmetSparkleBlue", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_BLUE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_BLUE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_sparkle_blue_kanim"),
			new ClothingItemInfo("AtmoHelmetSparklePurple", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_PURPLE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_PURPLE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_sparkle_purple_kanim"),
			new ClothingItemInfo("AtmoSuitSparkleRed", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_RED.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_RED.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_sparkle_red_kanim"),
			new ClothingItemInfo("AtmoSuitSparkleGreen", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_GREEN.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_GREEN.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_sparkle_green_kanim"),
			new ClothingItemInfo("AtmoSuitSparkleBlue", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_BLUE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_BLUE.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_sparkle_blue_kanim"),
			new ClothingItemInfo("AtmoSuitSparkleLavender", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_LAVENDER.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_LAVENDER.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_sparkle_lavender_kanim"),
			new ClothingItemInfo("AtmoGlovesSparkleRed", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_RED.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_RED.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_sparkle_red_kanim"),
			new ClothingItemInfo("AtmoGlovesSparkleGreen", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_GREEN.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_GREEN.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_sparkle_green_kanim"),
			new ClothingItemInfo("AtmoGlovesSparkleBlue", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_BLUE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_BLUE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_sparkle_blue_kanim"),
			new ClothingItemInfo("AtmoGlovesSparkleLavender", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_LAVENDER.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_LAVENDER.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_sparkle_lavender_kanim"),
			new ClothingItemInfo("AtmoBeltSparkleRed", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_RED.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_RED.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Splendid, "atmo_belt_sparkle_red_kanim"),
			new ClothingItemInfo("AtmoBeltSparkleGreen", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_GREEN.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_GREEN.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Splendid, "atmo_belt_sparkle_green_kanim"),
			new ClothingItemInfo("AtmoBeltSparkleBlue", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_BLUE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_BLUE.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Splendid, "atmo_belt_sparkle_blue_kanim"),
			new ClothingItemInfo("AtmoBeltSparkleLavender", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_LAVENDER.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_LAVENDER.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Splendid, "atmo_belt_sparkle_lavender_kanim"),
			new ClothingItemInfo("AtmoShoesSparkleBlack", EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.SPARKLE_BLACK.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.SPARKLE_BLACK.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_sparkle_black_kanim"),
			new ClothingItemInfo("TopDenimBlue", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.DENIM_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.DENIM_BLUE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_denim_blue_kanim"),
			new ClothingItemInfo("TopUndershirtExecutive", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_STRAWBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_STRAWBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_strawberry_kanim"),
			new ClothingItemInfo("TopUndershirtUnderling", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_SATSUMA.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_satsuma_kanim"),
			new ClothingItemInfo("TopUndershirtGroupthink", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_LEMON.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_LEMON.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_lemon_kanim"),
			new ClothingItemInfo("TopUndershirtStakeholder", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_LIME.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_LIME.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_lime_kanim"),
			new ClothingItemInfo("TopUndershirtAdmin", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_BLUEBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_BLUEBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_blueberry_kanim"),
			new ClothingItemInfo("TopUndershirtBuzzword", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_GRAPE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_GRAPE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_grape_kanim"),
			new ClothingItemInfo("TopUndershirtSynergy", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_WATERMELON.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_WATERMELON.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_watermelon_kanim"),
			new ClothingItemInfo("TopResearcher", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.NERD_BROWN.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.NERD_BROWN.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_nerd_white_cream_kanim"),
			new ClothingItemInfo("TopRebelGi", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GI_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GI_WHITE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_gi_white_kanim"),
			new ClothingItemInfo("BottomBriefsExecutive", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_STRAWBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_STRAWBERRY.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_strawberry_kanim"),
			new ClothingItemInfo("BottomBriefsUnderling", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_SATSUMA.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_satsuma_kanim"),
			new ClothingItemInfo("BottomBriefsGroupthink", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_LEMON.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_LEMON.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_lemon_kanim"),
			new ClothingItemInfo("BottomBriefsStakeholder", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_LIME.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_LIME.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_lime_kanim"),
			new ClothingItemInfo("BottomBriefsAdmin", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_BLUEBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_BLUEBERRY.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_blueberry_kanim"),
			new ClothingItemInfo("BottomBriefsBuzzword", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_GRAPE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_GRAPE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_grape_kanim"),
			new ClothingItemInfo("BottomBriefsSynergy", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_WATERMELON.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_WATERMELON.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_watermelon_kanim"),
			new ClothingItemInfo("PantsJeans", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.DENIM_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.DENIM_BLUE.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_denim_blue_kanim"),
			new ClothingItemInfo("PantsRebelGi", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GI_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GI_WHITE.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_gi_white_kanim"),
			new ClothingItemInfo("PantsResearch", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.NERD_BROWN.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.NERD_BROWN.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_nerd_brown_kanim"),
			new ClothingItemInfo("ShoesBasicGray", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_GREY.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_GREY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_grey_kanim"),
			new ClothingItemInfo("ShoesDenimBlue", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.DENIM_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.DENIM_BLUE.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_denim_blue_kanim"),
			new ClothingItemInfo("SocksLegwarmersBlueberry", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_BLUEBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_BLUEBERRY.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_blueberry_kanim"),
			new ClothingItemInfo("SocksLegwarmersGrape", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_GRAPE.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_GRAPE.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_grape_kanim"),
			new ClothingItemInfo("SocksLegwarmersLemon", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_LEMON.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_LEMON.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_lemon_kanim"),
			new ClothingItemInfo("SocksLegwarmersLime", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_LIME.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_LIME.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_lime_kanim"),
			new ClothingItemInfo("SocksLegwarmersSatsuma", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_SATSUMA.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_satsuma_kanim"),
			new ClothingItemInfo("SocksLegwarmersStrawberry", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_STRAWBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_STRAWBERRY.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_strawberry_kanim"),
			new ClothingItemInfo("SocksLegwarmersWatermelon", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_WATERMELON.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_WATERMELON.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_watermelon_kanim"),
			new ClothingItemInfo("GlovesCufflessBlack", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_BLACK.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_black_kanim"),
			new ClothingItemInfo("GlovesDenimBlue", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.DENIM_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.DENIM_BLUE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_denim_blue_kanim"),
			new ClothingItemInfo("AtmoGlovesGold", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.GOLD.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.GOLD.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_gold_kanim"),
			new ClothingItemInfo("AtmoGlovesEggplant", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.PURPLE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.PURPLE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_purple_kanim"),
			new ClothingItemInfo("AtmoHelmetEggplant", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CLUBSHIRT_PURPLE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CLUBSHIRT_PURPLE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_clubshirt_purple_kanim"),
			new ClothingItemInfo("AtmoHelmetConfetti", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.TRIANGLES_TURQ.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.TRIANGLES_TURQ.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_triangles_turq_kanim"),
			new ClothingItemInfo("AtmoShoesStealth", EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_BLACK.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_BLACK.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_basic_black_kanim"),
			new ClothingItemInfo("AtmoShoesEggplant", EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_PURPLE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_PURPLE.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_basic_purple_kanim"),
			new ClothingItemInfo("AtmoSuitCrispEggplant", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.BASIC_PURPLE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.BASIC_PURPLE.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Nifty, "atmosuit_basic_purple_kanim"),
			new ClothingItemInfo("AtmoSuitConfetti", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.PRINT_TRIANGLES_TURQ.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.PRINT_TRIANGLES_TURQ.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_print_triangles_turq_kanim"),
			new ClothingItemInfo("AtmoBeltBasicGold", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_GOLD.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_GOLD.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_basic_gold_kanim"),
			new ClothingItemInfo("AtmoBeltEggplant", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.TWOTONE_PURPLE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.TWOTONE_PURPLE.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_2tone_purple_kanim"),
			new ClothingItemInfo("SkirtBasicBlueMiddle", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_BLUE_MIDDLE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_blue_middle_kanim"),
			new ClothingItemInfo("SkirtBasicPurple", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_PURPLE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_PURPLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_purple_kanim"),
			new ClothingItemInfo("SkirtBasicGreen", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_GREEN.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_GREEN.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_green_kanim"),
			new ClothingItemInfo("SkirtBasicOrange", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_ORANGE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_ORANGE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_orange_kanim"),
			new ClothingItemInfo("SkirtBasicPinkOrchid", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_PINK_ORCHID.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_PINK_ORCHID.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_pink_orchid_kanim"),
			new ClothingItemInfo("SkirtBasicRed", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_RED.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_RED.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_red_kanim"),
			new ClothingItemInfo("SkirtBasicYellow", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_YELLOW.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_YELLOW.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_yellow_kanim"),
			new ClothingItemInfo("SkirtBasicPolkadot", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_POLKADOT.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_POLKADOT.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_basic_polkadot_kanim"),
			new ClothingItemInfo("SkirtBasicWatermelon", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_WATERMELON.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_WATERMELON.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_basic_watermelon_kanim"),
			new ClothingItemInfo("SkirtDenimBlue", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_DENIM_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_DENIM_BLUE.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_denim_blue_kanim"),
			new ClothingItemInfo("SkirtLeopardPrintBluePink", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_LEOPARD_PRINT_BLUE_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_LEOPARD_PRINT_BLUE_PINK.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_leopard_print_blue_pink_kanim"),
			new ClothingItemInfo("SkirtSparkleBlue", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_SPARKLE_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_SPARKLE_BLUE.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_sparkle_blue_kanim"),
			new ClothingItemInfo("AtmoBeltBasicGrey", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_GREY.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_GREY.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_basic_grey_kanim"),
			new ClothingItemInfo("AtmoBeltBasicNeonPink", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_NEON_PINK.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_NEON_PINK.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_basic_neon_pink_kanim"),
			new ClothingItemInfo("AtmoGlovesWhite", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.WHITE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.WHITE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_white_kanim"),
			new ClothingItemInfo("AtmoGlovesStripesLavender", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.STRIPES_LAVENDER.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.STRIPES_LAVENDER.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_stripes_lavender_kanim"),
			new ClothingItemInfo("AtmoHelmetCummerbundRed", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CUMMERBUND_RED.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CUMMERBUND_RED.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_cummerbund_red_kanim"),
			new ClothingItemInfo("AtmoHelmetWorkoutLavender", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.WORKOUT_LAVENDER.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.WORKOUT_LAVENDER.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_workout_lavender_kanim"),
			new ClothingItemInfo("AtmoShoesBasicLavender", EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_LAVENDER.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_LAVENDER.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_basic_lavender_kanim"),
			new ClothingItemInfo("AtmoSuitBasicNeonPink", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.BASIC_NEON_PINK.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.BASIC_NEON_PINK.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Nifty, "atmosuit_basic_neon_pink_kanim"),
			new ClothingItemInfo("AtmoSuitMultiRedBlack", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_RED_BLACK.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_RED_BLACK.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_multi_red_black_kanim"),
			new ClothingItemInfo("TopJacketSmokingBurgundy", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JACKET_SMOKING_BURGUNDY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JACKET_SMOKING_BURGUNDY.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jacket_smoking_burgundy_kanim"),
			new ClothingItemInfo("TopMechanic", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.MECHANIC.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.MECHANIC.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_mechanic_kanim"),
			new ClothingItemInfo("TopVelourBlack", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_BLACK.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_velour_black_kanim"),
			new ClothingItemInfo("TopVelourBlue", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_BLUE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_velour_blue_kanim"),
			new ClothingItemInfo("TopVelourPink", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_PINK.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_velour_pink_kanim"),
			new ClothingItemInfo("TopWaistcoatPinstripeSlate", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.WAISTCOAT_PINSTRIPE_SLATE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.WAISTCOAT_PINSTRIPE_SLATE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_waistcoat_pinstripe_slate_kanim"),
			new ClothingItemInfo("TopWater", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.WATER.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.WATER.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_water_kanim"),
			new ClothingItemInfo("TopTweedPinkOrchid", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TWEED_PINK_ORCHID.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TWEED_PINK_ORCHID.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_tweed_pink_orchid_kanim"),
			new ClothingItemInfo("DressSleevelessBowBw", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.DRESS_SLEEVELESS_BOW_BW.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.DRESS_SLEEVELESS_BOW_BW.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "dress_sleeveless_bow_bw_kanim"),
			new ClothingItemInfo("BodysuitBallerinaPink", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BODYSUIT_BALLERINA_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BODYSUIT_BALLERINA_PINK.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "bodysuit_ballerina_pink_kanim"),
			new ClothingItemInfo("PantsBasicOrangeSatsuma", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_ORANGE_SATSUMA.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_ORANGE_SATSUMA.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_orange_satsuma_kanim"),
			new ClothingItemInfo("PantsPinstripeSlate", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.PINSTRIPE_SLATE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.PINSTRIPE_SLATE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_pinstripe_slate_kanim"),
			new ClothingItemInfo("PantsVelourBlack", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_BLACK.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_velour_black_kanim"),
			new ClothingItemInfo("PantsVelourBlue", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_BLUE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_velour_blue_kanim"),
			new ClothingItemInfo("PantsVelourPink", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_PINK.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_velour_pink_kanim"),
			new ClothingItemInfo("SkirtBallerinaPink", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BALLERINA_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BALLERINA_PINK.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_ballerina_pink_kanim"),
			new ClothingItemInfo("SkirtTweedPinkOrchid", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_TWEED_PINK_ORCHID.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_TWEED_PINK_ORCHID.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_tweed_pink_orchid_kanim"),
			new ClothingItemInfo("ShoesBallerinaPink", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BALLERINA_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BALLERINA_PINK.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_ballerina_pink_kanim"),
			new ClothingItemInfo("ShoesMaryjaneSocksBw", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.MARYJANE_SOCKS_BW.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.MARYJANE_SOCKS_BW.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_maryjane_socks_bw_kanim"),
			new ClothingItemInfo("ShoesClassicFlatsCreamCharcoal", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.CLASSICFLATS_CREAM_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.CLASSICFLATS_CREAM_CHARCOAL.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_classicflats_cream_charcoal_kanim"),
			new ClothingItemInfo("ShoesVelourBlue", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_BLUE.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_velour_blue_kanim"),
			new ClothingItemInfo("ShoesVelourPink", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_PINK.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_velour_pink_kanim"),
			new ClothingItemInfo("ShoesVelourBlack", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_BLACK.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_velour_black_kanim"),
			new ClothingItemInfo("GlovesBasicGrey", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_GREY.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_GREY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_grey_kanim"),
			new ClothingItemInfo("GlovesBasicPinksalmon", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PINKSALMON.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PINKSALMON.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_pinksalmon_kanim"),
			new ClothingItemInfo("GlovesBasicTan", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_TAN.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_TAN.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_tan_kanim"),
			new ClothingItemInfo("GlovesBallerinaPink", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BALLERINA_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BALLERINA_PINK.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_ballerina_pink_kanim"),
			new ClothingItemInfo("GlovesFormalWhite", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.FORMAL_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.FORMAL_WHITE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_formal_white_kanim"),
			new ClothingItemInfo("GlovesLongWhite", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.LONG_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.LONG_WHITE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_long_white_kanim"),
			new ClothingItemInfo("Gloves2ToneCreamCharcoal", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.TWOTONE_CREAM_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.TWOTONE_CREAM_CHARCOAL.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_2tone_cream_charcoal_kanim"),
			new ClothingItemInfo("AtmoHelmetRocketmelon", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CANTALOUPE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_cantaloupe_kanim"),
			new ClothingItemInfo("AtmoSuitRocketmelon", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.CANTALOUPE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_cantaloupe_kanim"),
			new ClothingItemInfo("AtmoBeltRocketmelon", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.CANTALOUPE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_cantaloupe_kanim"),
			new ClothingItemInfo("AtmoGlovesRocketmelon", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.CANTALOUPE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_cantaloupe_kanim"),
			new ClothingItemInfo("AtmoBootsRocketmelon", EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.CANTALOUPE.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_cantaloupe_kanim"),
			new ClothingItemInfo("TopXSporchid", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.X_SPORCHID.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.X_SPORCHID.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_x_sporchid_kanim"),
			new ClothingItemInfo("TopX1Pinchapeppernutbells", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.X1_PINCHAPEPPERNUTBELLS.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.X1_PINCHAPEPPERNUTBELLS.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_x1_pinchapeppernutbells_kanim"),
			new ClothingItemInfo("TopPompomShinebugsPinkPeppernut", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.POMPOM_SHINEBUGS_PINK_PEPPERNUT.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.POMPOM_SHINEBUGS_PINK_PEPPERNUT.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_pompom_shinebugs_pink_peppernut_kanim"),
			new ClothingItemInfo("TopSnowflakeBlue", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.SNOWFLAKE_BLUE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.SNOWFLAKE_BLUE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_snowflake_blue_kanim"),
			new ClothingItemInfo("AtmoBeltTwoToneBrown", EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.TWOTONE_BROWN.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.TWOTONE_BROWN.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_2tone_brown_kanim"),
			new ClothingItemInfo("AtmoSuitMultiBlueGreyBlack", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_BLUE_GREY_BLACK.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_BLUE_GREY_BLACK.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_multi_blue_grey_black_kanim"),
			new ClothingItemInfo("AtmoSuitMultiBlueYellowRed", EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_BLUE_YELLOW_RED.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_BLUE_YELLOW_RED.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_multi_blue_yellow_red_kanim"),
			new ClothingItemInfo("AtmoGlovesBrown", EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.BROWN.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.BROWN.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_brown_kanim"),
			new ClothingItemInfo("AtmoHelmetMondrianBlueRedYellow", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.MONDRIAN_BLUE_RED_YELLOW.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.MONDRIAN_BLUE_RED_YELLOW.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_mondrian_blue_red_yellow_kanim"),
			new ClothingItemInfo("AtmoHelmetOverallsRed", EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.OVERALLS_RED.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.OVERALLS_RED.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_overalls_red_kanim"),
			new ClothingItemInfo("PjCloversGlitchKelly", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.PJ_CLOVERS_GLITCH_KELLY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.PJ_CLOVERS_GLITCH_KELLY.DESC, PermitCategory.DupeTops, PermitRarity.Splendid, "pj_clovers_glitch_kelly_kanim"),
			new ClothingItemInfo("PjHeartsChilliStrawberry", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.PJ_HEARTS_CHILLI_STRAWBERRY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.PJ_HEARTS_CHILLI_STRAWBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Splendid, "pj_hearts_chilli_strawberry_kanim"),
			new ClothingItemInfo("BottomGinchPinkGluon", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_PINK_GLUON.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_PINK_GLUON.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_pink_gluon_kanim"),
			new ClothingItemInfo("BottomGinchPurpleCortex", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_PURPLE_CORTEX.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_PURPLE_CORTEX.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_purple_cortex_kanim"),
			new ClothingItemInfo("BottomGinchBlueFrosty", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_BLUE_FROSTY.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_BLUE_FROSTY.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_blue_frosty_kanim"),
			new ClothingItemInfo("BottomGinchTealLocus", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_TEAL_LOCUS.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_TEAL_LOCUS.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_teal_locus_kanim"),
			new ClothingItemInfo("BottomGinchGreenGoop", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREEN_GOOP.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREEN_GOOP.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_green_goop_kanim"),
			new ClothingItemInfo("BottomGinchYellowBile", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_YELLOW_BILE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_YELLOW_BILE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_yellow_bile_kanim"),
			new ClothingItemInfo("BottomGinchOrangeNybble", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_ORANGE_NYBBLE.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_ORANGE_NYBBLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_orange_nybble_kanim"),
			new ClothingItemInfo("BottomGinchRedIronbow", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_RED_IRONBOW.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_RED_IRONBOW.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_red_ironbow_kanim"),
			new ClothingItemInfo("BottomGinchGreyPhlegm", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREY_PHLEGM.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREY_PHLEGM.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_grey_phlegm_kanim"),
			new ClothingItemInfo("BottomGinchGreyObelus", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREY_OBELUS.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREY_OBELUS.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_grey_obelus_kanim"),
			new ClothingItemInfo("PantsKnitPolkadotTurq", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.KNIT_POLKADOT_TURQ.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.KNIT_POLKADOT_TURQ.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_knit_polkadot_turq_kanim"),
			new ClothingItemInfo("PantsGiBeltWhiteBlack", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GI_BELT_WHITE_BLACK.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GI_BELT_WHITE_BLACK.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_gi_belt_white_black_kanim"),
			new ClothingItemInfo("PantsBeltKhakiTan", EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BELT_KHAKI_TAN.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BELT_KHAKI_TAN.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_belt_khaki_tan_kanim"),
			new ClothingItemInfo("ShoesFlashy", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.FLASHY.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.FLASHY.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_flashy_kanim"),
			new ClothingItemInfo("SocksGinchPinkSaltrock", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_PINK_SALTROCK.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_PINK_SALTROCK.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_pink_saltrock_kanim"),
			new ClothingItemInfo("SocksGinchPurpleDusky", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_PURPLE_DUSKY.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_PURPLE_DUSKY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_purple_dusky_kanim"),
			new ClothingItemInfo("SocksGinchBlueBasin", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_BLUE_BASIN.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_BLUE_BASIN.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_blue_basin_kanim"),
			new ClothingItemInfo("SocksGinchTealBalmy", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_TEAL_BALMY.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_TEAL_BALMY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_teal_balmy_kanim"),
			new ClothingItemInfo("SocksGinchGreenLime", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREEN_LIME.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREEN_LIME.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_green_lime_kanim"),
			new ClothingItemInfo("SocksGinchYellowYellowcake", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_YELLOW_YELLOWCAKE.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_YELLOW_YELLOWCAKE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_yellow_yellowcake_kanim"),
			new ClothingItemInfo("SocksGinchOrangeAtomic", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_ORANGE_ATOMIC.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_ORANGE_ATOMIC.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_orange_atomic_kanim"),
			new ClothingItemInfo("SocksGinchRedMagma", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_RED_MAGMA.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_RED_MAGMA.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_red_magma_kanim"),
			new ClothingItemInfo("SocksGinchGreyGrey", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREY_GREY.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREY_GREY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_grey_grey_kanim"),
			new ClothingItemInfo("SocksGinchGreyCharcoal", EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREY_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREY_CHARCOAL.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_grey_charcoal_kanim"),
			new ClothingItemInfo("GlovesBasicSlate", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_SLATE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_SLATE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_slate_kanim"),
			new ClothingItemInfo("GlovesKnitGold", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.KNIT_GOLD.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.KNIT_GOLD.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_knit_gold_kanim"),
			new ClothingItemInfo("GlovesKnitMagenta", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.KNIT_MAGENTA.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.KNIT_MAGENTA.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_knit_magenta_kanim"),
			new ClothingItemInfo("GlovesSparkleWhite", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.SPARKLE_WHITE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.SPARKLE_WHITE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_sparkle_white_kanim"),
			new ClothingItemInfo("GlovesGinchPinkSaltrock", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_PINK_SALTROCK.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_PINK_SALTROCK.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_pink_saltrock_kanim"),
			new ClothingItemInfo("GlovesGinchPurpleDusky", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_PURPLE_DUSKY.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_PURPLE_DUSKY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_purple_dusky_kanim"),
			new ClothingItemInfo("GlovesGinchBlueBasin", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_BLUE_BASIN.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_BLUE_BASIN.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_blue_basin_kanim"),
			new ClothingItemInfo("GlovesGinchTealBalmy", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_TEAL_BALMY.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_TEAL_BALMY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_teal_balmy_kanim"),
			new ClothingItemInfo("GlovesGinchGreenLime", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREEN_LIME.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREEN_LIME.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_green_lime_kanim"),
			new ClothingItemInfo("GlovesGinchYellowYellowcake", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_YELLOW_YELLOWCAKE.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_YELLOW_YELLOWCAKE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_yellow_yellowcake_kanim"),
			new ClothingItemInfo("GlovesGinchOrangeAtomic", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_ORANGE_ATOMIC.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_ORANGE_ATOMIC.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_orange_atomic_kanim"),
			new ClothingItemInfo("GlovesGinchRedMagma", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_RED_MAGMA.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_RED_MAGMA.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_red_magma_kanim"),
			new ClothingItemInfo("GlovesGinchGreyGrey", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREY_GREY.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREY_GREY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_grey_grey_kanim"),
			new ClothingItemInfo("GlovesGinchGreyCharcoal", EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREY_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREY_CHARCOAL.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_grey_charcoal_kanim"),
			new ClothingItemInfo("TopBuilder", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BUILDER.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BUILDER.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_builder_kanim"),
			new ClothingItemInfo("TopFloralPink", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.FLORAL_PINK.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.FLORAL_PINK.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_floral_pink_kanim"),
			new ClothingItemInfo("TopGinchPinkSaltrock", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_PINK_SALTROCK.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_PINK_SALTROCK.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_pink_saltrock_kanim"),
			new ClothingItemInfo("TopGinchPurpleDusky", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_PURPLE_DUSKY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_PURPLE_DUSKY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_purple_dusky_kanim"),
			new ClothingItemInfo("TopGinchBlueBasin", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_BLUE_BASIN.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_BLUE_BASIN.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_blue_basin_kanim"),
			new ClothingItemInfo("TopGinchTealBalmy", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_TEAL_BALMY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_TEAL_BALMY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_teal_balmy_kanim"),
			new ClothingItemInfo("TopGinchGreenLime", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREEN_LIME.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREEN_LIME.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_green_lime_kanim"),
			new ClothingItemInfo("TopGinchYellowYellowcake", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_YELLOW_YELLOWCAKE.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_YELLOW_YELLOWCAKE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_yellow_yellowcake_kanim"),
			new ClothingItemInfo("TopGinchOrangeAtomic", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_ORANGE_ATOMIC.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_ORANGE_ATOMIC.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_orange_atomic_kanim"),
			new ClothingItemInfo("TopGinchRedMagma", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_RED_MAGMA.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_RED_MAGMA.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_red_magma_kanim"),
			new ClothingItemInfo("TopGinchGreyGrey", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREY_GREY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREY_GREY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_grey_grey_kanim"),
			new ClothingItemInfo("TopGinchGreyCharcoal", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREY_CHARCOAL.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREY_CHARCOAL.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_grey_charcoal_kanim"),
			new ClothingItemInfo("TopKnitPolkadotTurq", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.KNIT_POLKADOT_TURQ.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.KNIT_POLKADOT_TURQ.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_knit_polkadot_turq_kanim"),
			new ClothingItemInfo("TopFlashy", EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.FLASHY.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.FLASHY.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_flashy_kanim")
		});
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x000A1CBC File Offset: 0x0009FEBC
	private void SetupClothingOutfits()
	{
		this.<SetupClothingOutfits>g__Add|5_0("BasicBlack", new string[]
		{
			"TopBasicBlack",
			"BottomBasicBlack",
			"GlovesBasicBlack",
			"ShoesBasicBlack"
		}, UI.OUTFITS.BASIC_BLACK.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicWhite", new string[]
		{
			"TopBasicWhite",
			"BottomBasicWhite",
			"GlovesBasicWhite",
			"ShoesBasicWhite"
		}, UI.OUTFITS.BASIC_WHITE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicRed", new string[]
		{
			"TopBasicRed",
			"BottomBasicRed",
			"GlovesBasicRed",
			"ShoesBasicRed"
		}, UI.OUTFITS.BASIC_RED.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicOrange", new string[]
		{
			"TopBasicOrange",
			"BottomBasicOrange",
			"GlovesBasicOrange",
			"ShoesBasicOrange"
		}, UI.OUTFITS.BASIC_ORANGE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicYellow", new string[]
		{
			"TopBasicYellow",
			"BottomBasicYellow",
			"GlovesBasicYellow",
			"ShoesBasicYellow"
		}, UI.OUTFITS.BASIC_YELLOW.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicGreen", new string[]
		{
			"TopBasicGreen",
			"BottomBasicGreen",
			"GlovesBasicGreen",
			"ShoesBasicGreen"
		}, UI.OUTFITS.BASIC_GREEN.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicAqua", new string[]
		{
			"TopBasicAqua",
			"BottomBasicAqua",
			"GlovesBasicAqua",
			"ShoesBasicAqua"
		}, UI.OUTFITS.BASIC_AQUA.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicPurple", new string[]
		{
			"TopBasicPurple",
			"BottomBasicPurple",
			"GlovesBasicPurple",
			"ShoesBasicPurple"
		}, UI.OUTFITS.BASIC_PURPLE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicPinkOrchid", new string[]
		{
			"TopBasicPinkOrchid",
			"BottomBasicPinkOrchid",
			"GlovesBasicPinkOrchid",
			"ShoesBasicPinkOrchid"
		}, UI.OUTFITS.BASIC_PINK_ORCHID.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicDeepRed", new string[]
		{
			"TopRaglanDeepRed",
			"ShortsBasicDeepRed",
			"GlovesAthleticRedDeep",
			"SocksAthleticDeepRed"
		}, UI.OUTFITS.BASIC_DEEPRED.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicOrangeSatsuma", new string[]
		{
			"TopRaglanSatsuma",
			"ShortsBasicSatsuma",
			"GlovesAthleticOrangeSatsuma",
			"SocksAthleticOrangeSatsuma"
		}, UI.OUTFITS.BASIC_SATSUMA.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicLemon", new string[]
		{
			"TopRaglanLemon",
			"ShortsBasicYellowcake",
			"GlovesAthleticYellowLemon",
			"SocksAthleticYellowLemon"
		}, UI.OUTFITS.BASIC_LEMON.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicBlueCobalt", new string[]
		{
			"TopRaglanCobalt",
			"ShortsBasicBlueCobalt",
			"GlovesAthleticBlueCobalt",
			"SocksAthleticBlueCobalt"
		}, UI.OUTFITS.BASIC_BLUE_COBALT.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicGreenKelly", new string[]
		{
			"TopRaglanKellyGreen",
			"ShortsBasicKellyGreen",
			"GlovesAthleticGreenKelly",
			"SocksAthleticGreenKelly"
		}, UI.OUTFITS.BASIC_GREEN_KELLY.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicPinkFlamingo", new string[]
		{
			"TopRaglanFlamingo",
			"ShortsBasicPinkFlamingo",
			"GlovesAthleticPinkFlamingo",
			"SocksAthleticPinkFlamingo"
		}, UI.OUTFITS.BASIC_PINK_FLAMINGO.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("BasicGreyCharcoal", new string[]
		{
			"TopRaglanCharcoal",
			"ShortsBasicCharcoal",
			"GlovesAthleticGreyCharcoal",
			"SocksAthleticGreyCharcoal"
		}, UI.OUTFITS.BASIC_GREY_CHARCOAL.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("JellypuffBlueberry", new string[]
		{
			"TopJellypuffJacketBlueberry",
			"GlovesCufflessBlueberry"
		}, UI.OUTFITS.JELLYPUFF_BLUEBERRY.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("JellypuffGrape", new string[]
		{
			"TopJellypuffJacketGrape",
			"GlovesCufflessGrape"
		}, UI.OUTFITS.JELLYPUFF_GRAPE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("JellypuffLemon", new string[]
		{
			"TopJellypuffJacketLemon",
			"GlovesCufflessLemon"
		}, UI.OUTFITS.JELLYPUFF_LEMON.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("JellypuffLime", new string[]
		{
			"TopJellypuffJacketLime",
			"GlovesCufflessLime"
		}, UI.OUTFITS.JELLYPUFF_LIME.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("JellypuffSatsuma", new string[]
		{
			"TopJellypuffJacketSatsuma",
			"GlovesCufflessSatsuma"
		}, UI.OUTFITS.JELLYPUFF_SATSUMA.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("JellypuffStrawberry", new string[]
		{
			"TopJellypuffJacketStrawberry",
			"GlovesCufflessStrawberry"
		}, UI.OUTFITS.JELLYPUFF_STRAWBERRY.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("JellypuffWatermelon", new string[]
		{
			"TopJellypuffJacketWatermelon",
			"GlovesCufflessWatermelon"
		}, UI.OUTFITS.JELLYPUFF_WATERMELON.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("Athlete", new string[]
		{
			"TopAthlete",
			"PantsAthlete",
			"GlovesAthlete",
			"ShoesBasicBlack"
		}, UI.OUTFITS.ATHLETE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("Circuit", new string[]
		{
			"TopCircuitGreen",
			"PantsCircuitGreen",
			"GlovesCircuitGreen"
		}, UI.OUTFITS.CIRCUIT.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoLimone", new string[]
		{
			"AtmoHelmetLimone",
			"AtmoSuitBasicYellow",
			"AtmoGlovesLime",
			"AtmoBeltBasicLime",
			"AtmoShoesBasicYellow"
		}, UI.OUTFITS.ATMOSUIT_LIMONE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoPuft", new string[]
		{
			"AtmoHelmetPuft",
			"AtmoSuitPuft",
			"AtmoGlovesPuft",
			"AtmoBeltPuft",
			"AtmoShoesPuft"
		}, UI.OUTFITS.ATMOSUIT_PUFT.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoSparkleRed", new string[]
		{
			"AtmoHelmetSparkleRed",
			"AtmoSuitSparkleRed",
			"AtmoGlovesSparkleRed",
			"AtmoBeltSparkleRed",
			"AtmoShoesSparkleBlack"
		}, UI.OUTFITS.ATMOSUIT_SPARKLE_RED.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoSparkleBlue", new string[]
		{
			"AtmoHelmetSparkleBlue",
			"AtmoSuitSparkleBlue",
			"AtmoGlovesSparkleBlue",
			"AtmoBeltSparkleBlue",
			"AtmoShoesSparkleBlack"
		}, UI.OUTFITS.ATMOSUIT_SPARKLE_BLUE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoSparkleGreen", new string[]
		{
			"AtmoHelmetSparkleGreen",
			"AtmoSuitSparkleGreen",
			"AtmoGlovesSparkleGreen",
			"AtmoBeltSparkleGreen",
			"AtmoShoesSparkleBlack"
		}, UI.OUTFITS.ATMOSUIT_SPARKLE_GREEN.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoSparkleLavender", new string[]
		{
			"AtmoHelmetSparklePurple",
			"AtmoSuitSparkleLavender",
			"AtmoGlovesSparkleLavender",
			"AtmoBeltSparkleLavender",
			"AtmoShoesSparkleBlack"
		}, UI.OUTFITS.ATMOSUIT_SPARKLE_LAVENDER.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoConfetti", new string[]
		{
			"AtmoHelmetConfetti",
			"AtmoSuitConfetti",
			"AtmoGlovesGold",
			"AtmoBeltBasicGold",
			"AtmoShoesStealth"
		}, UI.OUTFITS.ATMOSUIT_CONFETTI.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoEggplant", new string[]
		{
			"AtmoHelmetEggplant",
			"AtmoSuitCrispEggplant",
			"AtmoGlovesEggplant",
			"AtmoBeltEggplant",
			"AtmoShoesEggplant"
		}, UI.OUTFITS.ATMOSUIT_BASIC_PURPLE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("CanadianTuxedo", new string[]
		{
			"TopDenimBlue",
			"PantsJeans",
			"GlovesDenimBlue",
			"ShoesDenimBlue"
		}, UI.OUTFITS.CANUXTUX.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("Researcher", new string[]
		{
			"TopResearcher",
			"PantsResearch",
			"GlovesBasicBrownKhaki",
			"ShoesBasicGray"
		}, UI.OUTFITS.NERD.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("UndiesExec", new string[]
		{
			"TopUndershirtExecutive",
			"BottomBriefsExecutive"
		}, UI.OUTFITS.GONCHIES_STRAWBERRY.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("UndiesUnderling", new string[]
		{
			"TopUndershirtUnderling",
			"BottomBriefsUnderling"
		}, UI.OUTFITS.GONCHIES_SATSUMA.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("UndiesGroupthink", new string[]
		{
			"TopUndershirtGroupthink",
			"BottomBriefsGroupthink"
		}, UI.OUTFITS.GONCHIES_LEMON.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("UndiesStakeholder", new string[]
		{
			"TopUndershirtStakeholder",
			"BottomBriefsStakeholder"
		}, UI.OUTFITS.GONCHIES_LIME.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("UndiesAdmin", new string[]
		{
			"TopUndershirtAdmin",
			"BottomBriefsAdmin"
		}, UI.OUTFITS.GONCHIES_BLUEBERRY.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("UndiesBuzzword", new string[]
		{
			"TopUndershirtBuzzword",
			"BottomBriefsBuzzword"
		}, UI.OUTFITS.GONCHIES_GRAPE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("UndiesSynergy", new string[]
		{
			"TopUndershirtSynergy",
			"BottomBriefsSynergy"
		}, UI.OUTFITS.GONCHIES_WATERMELON.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("RebelGiOutfit", new string[]
		{
			"TopRebelGi",
			"PantsGiBeltWhiteBlack",
			"GlovesCufflessBlack"
		}, UI.OUTFITS.REBELGI.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoPinkPurple", new string[]
		{
			"AtmoBeltBasicNeonPink",
			"AtmoGlovesStripesLavender",
			"AtmoHelmetWorkoutLavender",
			"AtmoSuitBasicNeonPink",
			"AtmoShoesBasicLavender"
		}, UI.OUTFITS.ATMOSUIT_PINK_PURPLE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoRedGrey", new string[]
		{
			"AtmoBeltBasicGrey",
			"AtmoGlovesWhite",
			"AtmoHelmetCummerbundRed",
			"AtmoSuitMultiRedBlack"
		}, UI.OUTFITS.ATMOSUIT_RED_GREY.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("Donor", new string[]
		{
			"TopJacketSmokingBurgundy",
			"BottomBasicBlack",
			"GlovesBasicBlack"
		}, UI.OUTFITS.DONOR.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("EngineerCoveralls", new string[]
		{
			"TopMechanic",
			"PantsBasicRedOrange",
			"GlovesBasicGrey",
			"ShoesBasicBlack"
		}, UI.OUTFITS.MECHANIC.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("PhdVelour", new string[]
		{
			"TopVelourBlack",
			"PantsVelourBlack",
			"GlovesBasicWhite",
			"ShoesVelourBlack"
		}, UI.OUTFITS.VELOUR_BLACK.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("PhdDress", new string[]
		{
			"DressSleevelessBowBw",
			"GlovesLongWhite",
			"ShoesMaryjaneSocksBw"
		}, UI.OUTFITS.SLEEVELESS_BOW_BW.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("ShortwaveVelour", new string[]
		{
			"TopVelourBlue",
			"PantsVelourBlue",
			"GlovesBasicWhite",
			"ShoesVelourBlue"
		}, UI.OUTFITS.VELOUR_BLUE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GammaVelour", new string[]
		{
			"TopVelourPink",
			"PantsVelourPink",
			"GlovesBasicPinksalmon",
			"ShoesVelourPink"
		}, UI.OUTFITS.VELOUR_PINK.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("HvacCoveralls", new string[]
		{
			"TopWater",
			"PantsBeltKhakiTan",
			"GlovesBasicTan",
			"ShoesBasicTan"
		}, UI.OUTFITS.WATER.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("NobelPinstripe", new string[]
		{
			"TopWaistcoatPinstripeSlate",
			"PantsPinstripeSlate",
			"GlovesBasicSlate"
		}, UI.OUTFITS.WAISTCOAT_PINSTRIPE_SLATE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("PowerBrunch", new string[]
		{
			"TopTweedPinkOrchid",
			"SkirtTweedPinkOrchid",
			"Gloves2ToneCreamCharcoal",
			"ShoesClassicFlatsCreamCharcoal"
		}, UI.OUTFITS.TWEED_PINK_ORCHID.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("Ballet", new string[]
		{
			"BodysuitBallerinaPink",
			"SkirtBallerinaPink",
			"GlovesBallerinaPink",
			"ShoesBallerinaPink"
		}, UI.OUTFITS.BALLET.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("AtmoRocketmelon", new string[]
		{
			"AtmoHelmetRocketmelon",
			"AtmoSuitRocketmelon",
			"AtmoGlovesRocketmelon",
			"AtmoBeltRocketmelon",
			"AtmoBootsRocketmelon"
		}, UI.OUTFITS.ATMOSUIT_CANTALOUPE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("TopXSporchid", new string[]
		{
			"TopXSporchid"
		}, UI.OUTFITS.X_SPORCHID.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("TopX1Pinchapeppernutbells", new string[]
		{
			"TopX1Pinchapeppernutbells"
		}, UI.OUTFITS.X1_PINCHAPEPPERNUTBELLS.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("TopPompomShinebugsPinkPeppernut", new string[]
		{
			"TopPompomShinebugsPinkPeppernut"
		}, UI.OUTFITS.POMPOM_SHINEBUGS_PINK_PEPPERNUT.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("TopSnowflakeBlue", new string[]
		{
			"TopSnowflakeBlue"
		}, UI.OUTFITS.SNOWFLAKE_BLUE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("PolkaDotTracksuit", new string[]
		{
			"TopKnitPolkadotTurq",
			"PantsKnitPolkadotTurq",
			"GlovesKnitMagenta"
		}, UI.OUTFITS.POLKADOT_TRACKSUIT.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("Superstar", new string[]
		{
			"TopFlashy",
			"ShoesFlashy",
			"GlovesSparkleWhite",
			"BottomBasicBlack"
		}, UI.OUTFITS.SUPERSTAR.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("Spiffy", new string[]
		{
			"AtmoHelmetOverallsRed",
			"AtmoSuitMultiBlueGreyBlack",
			"AtmoGlovesBrown",
			"AtmoBeltTwoToneBrown"
		}, UI.OUTFITS.ATMOSUIT_SPIFFY.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("Cubist", new string[]
		{
			"AtmoHelmetMondrianBlueRedYellow",
			"AtmoSuitMultiBlueYellowRed",
			"AtmoGlovesGold",
			"AtmoBeltBasicGold"
		}, UI.OUTFITS.ATMOSUIT_CUBIST.NAME, BlueprintProvider.OutfitType.AtmoSuit);
		this.<SetupClothingOutfits>g__Add|5_0("Lucky", new string[]
		{
			"PjCloversGlitchKelly"
		}, UI.OUTFITS.LUCKY.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("Sweetheart", new string[]
		{
			"PjHeartsChilliStrawberry"
		}, UI.OUTFITS.SWEETHEART.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchGluon", new string[]
		{
			"TopGinchPinkSaltrock",
			"BottomGinchPinkGluon",
			"GlovesGinchPinkSaltrock",
			"SocksGinchPinkSaltrock"
		}, UI.OUTFITS.GINCH_GLUON.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchCortex", new string[]
		{
			"TopGinchPurpleDusky",
			"BottomGinchPurpleCortex",
			"GlovesGinchPurpleDusky",
			"SocksGinchPurpleDusky"
		}, UI.OUTFITS.GINCH_CORTEX.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchFrosty", new string[]
		{
			"TopGinchBlueBasin",
			"BottomGinchBlueFrosty",
			"GlovesGinchBlueBasin",
			"SocksGinchBlueBasin"
		}, UI.OUTFITS.GINCH_FROSTY.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchLocus", new string[]
		{
			"TopGinchTealBalmy",
			"BottomGinchTealLocus",
			"GlovesGinchTealBalmy",
			"SocksGinchTealBalmy"
		}, UI.OUTFITS.GINCH_LOCUS.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchGoop", new string[]
		{
			"TopGinchGreenLime",
			"BottomGinchGreenGoop",
			"GlovesGinchGreenLime",
			"SocksGinchGreenLime"
		}, UI.OUTFITS.GINCH_GOOP.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchBile", new string[]
		{
			"TopGinchYellowYellowcake",
			"BottomGinchYellowBile",
			"GlovesGinchYellowYellowcake",
			"SocksGinchYellowYellowcake"
		}, UI.OUTFITS.GINCH_BILE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchNybble", new string[]
		{
			"TopGinchOrangeAtomic",
			"BottomGinchOrangeNybble",
			"GlovesGinchOrangeAtomic",
			"SocksGinchOrangeAtomic"
		}, UI.OUTFITS.GINCH_NYBBLE.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchIronbow", new string[]
		{
			"TopGinchRedMagma",
			"BottomGinchRedIronbow",
			"GlovesGinchRedMagma",
			"SocksGinchRedMagma"
		}, UI.OUTFITS.GINCH_IRONBOW.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchPhlegm", new string[]
		{
			"TopGinchGreyGrey",
			"BottomGinchGreyPhlegm",
			"GlovesGinchGreyGrey",
			"SocksGinchGreyGrey"
		}, UI.OUTFITS.GINCH_PHLEGM.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("GinchObelus", new string[]
		{
			"TopGinchGreyCharcoal",
			"BottomGinchGreyObelus",
			"GlovesGinchGreyCharcoal",
			"SocksGinchGreyCharcoal"
		}, UI.OUTFITS.GINCH_OBELUS.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("HiVis", new string[]
		{
			"TopBuilder",
			"PantsBasicOrangeSatsuma",
			"GlovesBasicYellow",
			"ShoesBasicBlack"
		}, UI.OUTFITS.HIVIS.NAME, BlueprintProvider.OutfitType.Clothing);
		this.<SetupClothingOutfits>g__Add|5_0("Downtime", new string[]
		{
			"TopFloralPink",
			"GlovesKnitGold"
		}, UI.OUTFITS.DOWNTIME.NAME, BlueprintProvider.OutfitType.Clothing);
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x000A2DB4 File Offset: 0x000A0FB4
	private void SetupBalloonArtistFacades()
	{
		this.blueprintCollection.balloonArtistFacades.AddRange(new BalloonArtistFacadeInfo[]
		{
			new BalloonArtistFacadeInfo("BalloonRedFireEngineLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_FIREENGINE_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_FIREENGINE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_red_fireengine_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonYellowLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_YELLOW_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_YELLOW_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_yellow_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonBlueLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BLUE_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BLUE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_blue_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonGreenLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_GREEN_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_GREEN_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_green_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonPinkLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PINK_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PINK_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_pink_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonPurpleLongSparkles", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PURPLE_LONG_SPARKLES.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PURPLE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_purple_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonBabyPacuEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PACU_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PACU_EGG.DESC, PermitRarity.Splendid, "balloon_babypacu_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonBabyGlossyDreckoEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_GLOSSY_DRECKO_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_GLOSSY_DRECKO_EGG.DESC, PermitRarity.Splendid, "balloon_babyglossydrecko_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonBabyHatchEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_HATCH_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_HATCH_EGG.DESC, PermitRarity.Splendid, "balloon_babyhatch_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonBabyPokeshellEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_POKESHELL_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_POKESHELL_EGG.DESC, PermitRarity.Splendid, "balloon_babypokeshell_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonBabyPuftEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PUFT_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PUFT_EGG.DESC, PermitRarity.Splendid, "balloon_babypuft_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonBabyShovoleEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_SHOVOLE_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_SHOVOLE_EGG.DESC, PermitRarity.Splendid, "balloon_babyshovole_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonBabyPipEgg", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PIP_EGG.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PIP_EGG.DESC, PermitRarity.Splendid, "balloon_babypip_egg_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonCandyBlueberry", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_BLUEBERRY.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_BLUEBERRY.DESC, PermitRarity.Decent, "balloon_candy_blueberry_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonCandyGrape", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_GRAPE.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_GRAPE.DESC, PermitRarity.Decent, "balloon_candy_grape_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonCandyLemon", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LEMON.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LEMON.DESC, PermitRarity.Decent, "balloon_candy_lemon_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonCandyLime", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LIME.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LIME.DESC, PermitRarity.Decent, "balloon_candy_lime_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonCandyOrange", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_ORANGE.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_ORANGE.DESC, PermitRarity.Decent, "balloon_candy_orange_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonCandyStrawberry", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_STRAWBERRY.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_STRAWBERRY.DESC, PermitRarity.Decent, "balloon_candy_strawberry_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonCandyWatermelon", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_WATERMELON.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_WATERMELON.DESC, PermitRarity.Decent, "balloon_candy_watermelon_kanim", BalloonArtistFacadeType.ThreeSet),
			new BalloonArtistFacadeInfo("BalloonHandGold", EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.HAND_GOLD.NAME, EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.HAND_GOLD.DESC, PermitRarity.Decent, "balloon_hand_gold_kanim", BalloonArtistFacadeType.ThreeSet)
		});
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x000A3134 File Offset: 0x000A1334
	[CompilerGenerated]
	private void <SetupClothingOutfits>g__Add|5_0(string outfitId, string[] itemIds, string name, BlueprintProvider.OutfitType outfitType)
	{
		this.blueprintCollection.outfits.Add(new ClothingOutfitResource(outfitId, itemIds, name, (ClothingOutfitUtility.OutfitType)outfitType));
	}
}

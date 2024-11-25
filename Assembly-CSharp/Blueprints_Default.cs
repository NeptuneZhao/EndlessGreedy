﻿using System;
using Database;
using STRINGS;

// Token: 0x0200051F RID: 1311
public class Blueprints_Default : BlueprintProvider
{
	// Token: 0x06001D57 RID: 7511 RVA: 0x000996B0 File Offset: 0x000978B0
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x000996B7 File Offset: 0x000978B7
	public override void SetupBlueprints()
	{
		this.SetupBuildingFacades();
		this.SetupArtables();
		this.SetupClothingItems();
		this.SetupClothingOutfits();
		this.SetupBalloonArtistFacades();
		this.SetupStickerBombFacades();
		this.SetupEquippableFacades();
		this.SetupMonumentParts();
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x000996E9 File Offset: 0x000978E9
	public void SetupBuildingFacades()
	{
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x000996EC File Offset: 0x000978EC
	private void SetupArtables()
	{
		this.blueprintCollection.artables.AddRange(new ArtableInfo[]
		{
			new ArtableInfo("Canvas_Bad", BUILDINGS.PREFABS.CANVAS.FACADES.ART_A.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_A.DESC, PermitRarity.Universal, "painting_art_a_kanim", "art_a", 5, false, "LookingUgly", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Average", BUILDINGS.PREFABS.CANVAS.FACADES.ART_B.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_B.DESC, PermitRarity.Universal, "painting_art_b_kanim", "art_b", 10, false, "LookingOkay", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good", BUILDINGS.PREFABS.CANVAS.FACADES.ART_C.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_C.DESC, PermitRarity.Universal, "painting_art_c_kanim", "art_c", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good2", BUILDINGS.PREFABS.CANVAS.FACADES.ART_D.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_D.DESC, PermitRarity.Universal, "painting_art_d_kanim", "art_d", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good3", BUILDINGS.PREFABS.CANVAS.FACADES.ART_E.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_E.DESC, PermitRarity.Universal, "painting_art_e_kanim", "art_e", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good4", BUILDINGS.PREFABS.CANVAS.FACADES.ART_F.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_F.DESC, PermitRarity.Universal, "painting_art_f_kanim", "art_f", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good5", BUILDINGS.PREFABS.CANVAS.FACADES.ART_G.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_G.DESC, PermitRarity.Universal, "painting_art_g_kanim", "art_g", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("Canvas_Good6", BUILDINGS.PREFABS.CANVAS.FACADES.ART_H.NAME, BUILDINGS.PREFABS.CANVAS.FACADES.ART_H.DESC, PermitRarity.Universal, "painting_art_h_kanim", "art_h", 15, true, "LookingGreat", "Canvas", "canvas"),
			new ArtableInfo("CanvasTall_Bad", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_A.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_A.DESC, PermitRarity.Universal, "painting_tall_art_a_kanim", "art_a", 5, false, "LookingUgly", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasTall_Average", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_B.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_B.DESC, PermitRarity.Universal, "painting_tall_art_b_kanim", "art_b", 10, false, "LookingOkay", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasTall_Good", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_C.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_C.DESC, PermitRarity.Universal, "painting_tall_art_c_kanim", "art_c", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasTall_Good2", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_D.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_D.DESC, PermitRarity.Universal, "painting_tall_art_d_kanim", "art_d", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasTall_Good3", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_E.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_E.DESC, PermitRarity.Universal, "painting_tall_art_e_kanim", "art_e", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasTall_Good4", BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_F.NAME, BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_F.DESC, PermitRarity.Universal, "painting_tall_art_f_kanim", "art_f", 15, true, "LookingGreat", "CanvasTall", "canvas"),
			new ArtableInfo("CanvasWide_Bad", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_A.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_A.DESC, PermitRarity.Universal, "painting_wide_art_a_kanim", "art_a", 5, false, "LookingUgly", "CanvasWide", "canvas"),
			new ArtableInfo("CanvasWide_Average", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_B.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_B.DESC, PermitRarity.Universal, "painting_wide_art_b_kanim", "art_b", 10, false, "LookingOkay", "CanvasWide", "canvas"),
			new ArtableInfo("CanvasWide_Good", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_C.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_C.DESC, PermitRarity.Universal, "painting_wide_art_c_kanim", "art_c", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("CanvasWide_Good2", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_D.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_D.DESC, PermitRarity.Universal, "painting_wide_art_d_kanim", "art_d", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("CanvasWide_Good3", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_E.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_E.DESC, PermitRarity.Universal, "painting_wide_art_e_kanim", "art_e", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("CanvasWide_Good4", BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_F.NAME, BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_F.DESC, PermitRarity.Universal, "painting_wide_art_f_kanim", "art_f", 15, true, "LookingGreat", "CanvasWide", "canvas"),
			new ArtableInfo("Sculpture_Bad", BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_CRAP_1.NAME, BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_CRAP_1.DESC, PermitRarity.Universal, "sculpture_crap_1_kanim", "crap_1", 5, false, "LookingUgly", "Sculpture", ""),
			new ArtableInfo("Sculpture_Average", BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_GOOD_1.NAME, BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_GOOD_1.DESC, PermitRarity.Universal, "sculpture_good_1_kanim", "good_1", 10, false, "LookingOkay", "Sculpture", ""),
			new ArtableInfo("Sculpture_Good1", BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_1.NAME, BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_1.DESC, PermitRarity.Universal, "sculpture_amazing_1_kanim", "amazing_1", 15, true, "LookingGreat", "Sculpture", ""),
			new ArtableInfo("Sculpture_Good2", BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_2.NAME, BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_2.DESC, PermitRarity.Universal, "sculpture_amazing_2_kanim", "amazing_2", 15, true, "LookingGreat", "Sculpture", ""),
			new ArtableInfo("Sculpture_Good3", BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_3.NAME, BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_3.DESC, PermitRarity.Universal, "sculpture_amazing_3_kanim", "amazing_3", 15, true, "LookingGreat", "Sculpture", ""),
			new ArtableInfo("SmallSculpture_Bad", BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_CRAP.NAME, BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_CRAP.DESC, PermitRarity.Universal, "sculpture_1x2_crap_1_kanim", "crap_1", 5, false, "LookingUgly", "SmallSculpture", ""),
			new ArtableInfo("SmallSculpture_Average", BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_GOOD.NAME, BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_GOOD.DESC, PermitRarity.Universal, "sculpture_1x2_good_1_kanim", "good_1", 10, false, "LookingOkay", "SmallSculpture", ""),
			new ArtableInfo("SmallSculpture_Good", BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_1.NAME, BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_1.DESC, PermitRarity.Universal, "sculpture_1x2_amazing_1_kanim", "amazing_1", 15, true, "LookingGreat", "SmallSculpture", ""),
			new ArtableInfo("SmallSculpture_Good2", BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_2.NAME, BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_2.DESC, PermitRarity.Universal, "sculpture_1x2_amazing_2_kanim", "amazing_2", 15, true, "LookingGreat", "SmallSculpture", ""),
			new ArtableInfo("SmallSculpture_Good3", BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_3.NAME, BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_3.DESC, PermitRarity.Universal, "sculpture_1x2_amazing_3_kanim", "amazing_3", 15, true, "LookingGreat", "SmallSculpture", ""),
			new ArtableInfo("IceSculpture_Bad", BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_CRAP.NAME, BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_CRAP.DESC, PermitRarity.Universal, "icesculpture_crap_kanim", "crap", 5, false, "LookingUgly", "IceSculpture", ""),
			new ArtableInfo("IceSculpture_Average", BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_1.NAME, BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_1.DESC, PermitRarity.Universal, "icesculpture_idle_kanim", "idle", 10, false, "LookingOkay", "IceSculpture", "good"),
			new ArtableInfo("MarbleSculpture_Bad", BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_CRAP_1.NAME, BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_CRAP_1.DESC, PermitRarity.Universal, "sculpture_marble_crap_1_kanim", "crap_1", 5, false, "LookingUgly", "MarbleSculpture", ""),
			new ArtableInfo("MarbleSculpture_Average", BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_GOOD_1.NAME, BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_GOOD_1.DESC, PermitRarity.Universal, "sculpture_marble_good_1_kanim", "good_1", 10, false, "LookingOkay", "MarbleSculpture", ""),
			new ArtableInfo("MarbleSculpture_Good1", BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_1.NAME, BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_1.DESC, PermitRarity.Universal, "sculpture_marble_amazing_1_kanim", "amazing_1", 15, true, "LookingGreat", "MarbleSculpture", ""),
			new ArtableInfo("MarbleSculpture_Good2", BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_2.NAME, BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_2.DESC, PermitRarity.Universal, "sculpture_marble_amazing_2_kanim", "amazing_2", 15, true, "LookingGreat", "MarbleSculpture", ""),
			new ArtableInfo("MarbleSculpture_Good3", BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_3.NAME, BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_3.DESC, PermitRarity.Universal, "sculpture_marble_amazing_3_kanim", "amazing_3", 15, true, "LookingGreat", "MarbleSculpture", ""),
			new ArtableInfo("MetalSculpture_Bad", BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_CRAP_1.NAME, BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_CRAP_1.DESC, PermitRarity.Universal, "sculpture_metal_crap_1_kanim", "crap_1", 5, false, "LookingUgly", "MetalSculpture", ""),
			new ArtableInfo("MetalSculpture_Average", BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_GOOD_1.NAME, BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_GOOD_1.DESC, PermitRarity.Universal, "sculpture_metal_good_1_kanim", "good_1", 10, false, "LookingOkay", "MetalSculpture", ""),
			new ArtableInfo("MetalSculpture_Good1", BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_1.NAME, BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_1.DESC, PermitRarity.Universal, "sculpture_metal_amazing_1_kanim", "amazing_1", 15, true, "LookingGreat", "MetalSculpture", ""),
			new ArtableInfo("MetalSculpture_Good2", BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_2.NAME, BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_2.DESC, PermitRarity.Universal, "sculpture_metal_amazing_2_kanim", "amazing_2", 15, true, "LookingGreat", "MetalSculpture", ""),
			new ArtableInfo("MetalSculpture_Good3", BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_3.NAME, BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_3.DESC, PermitRarity.Universal, "sculpture_metal_amazing_3_kanim", "amazing_3", 15, true, "LookingGreat", "MetalSculpture", "")
		});
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x0009A155 File Offset: 0x00098355
	private void SetupClothingItems()
	{
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x0009A157 File Offset: 0x00098357
	private void SetupClothingOutfits()
	{
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x0009A159 File Offset: 0x00098359
	private void SetupBalloonArtistFacades()
	{
	}

	// Token: 0x06001D5E RID: 7518 RVA: 0x0009A15C File Offset: 0x0009835C
	private void SetupStickerBombFacades()
	{
		this.blueprintCollection.stickerBombFacades.AddRange(new StickerBombFacadeInfo[]
		{
			new StickerBombFacadeInfo("a", STICKERNAMES.STICKER_A, "TODO:DbStickers", PermitRarity.Unknown, "sticker_a_kanim", "a"),
			new StickerBombFacadeInfo("b", STICKERNAMES.STICKER_B, "TODO:DbStickers", PermitRarity.Unknown, "sticker_b_kanim", "b"),
			new StickerBombFacadeInfo("c", STICKERNAMES.STICKER_C, "TODO:DbStickers", PermitRarity.Unknown, "sticker_c_kanim", "c"),
			new StickerBombFacadeInfo("d", STICKERNAMES.STICKER_D, "TODO:DbStickers", PermitRarity.Unknown, "sticker_d_kanim", "d"),
			new StickerBombFacadeInfo("e", STICKERNAMES.STICKER_E, "TODO:DbStickers", PermitRarity.Unknown, "sticker_e_kanim", "e"),
			new StickerBombFacadeInfo("f", STICKERNAMES.STICKER_F, "TODO:DbStickers", PermitRarity.Unknown, "sticker_f_kanim", "f"),
			new StickerBombFacadeInfo("g", STICKERNAMES.STICKER_G, "TODO:DbStickers", PermitRarity.Unknown, "sticker_g_kanim", "g"),
			new StickerBombFacadeInfo("h", STICKERNAMES.STICKER_H, "TODO:DbStickers", PermitRarity.Unknown, "sticker_h_kanim", "h"),
			new StickerBombFacadeInfo("rocket", STICKERNAMES.STICKER_ROCKET, "TODO:DbStickers", PermitRarity.Unknown, "sticker_rocket_kanim", "rocket"),
			new StickerBombFacadeInfo("paperplane", STICKERNAMES.STICKER_PAPERPLANE, "TODO:DbStickers", PermitRarity.Unknown, "sticker_paperplane_kanim", "paperplane"),
			new StickerBombFacadeInfo("plant", STICKERNAMES.STICKER_PLANT, "TODO:DbStickers", PermitRarity.Unknown, "sticker_plant_kanim", "plant"),
			new StickerBombFacadeInfo("plantpot", STICKERNAMES.STICKER_PLANTPOT, "TODO:DbStickers", PermitRarity.Unknown, "sticker_plantpot_kanim", "plantpot"),
			new StickerBombFacadeInfo("mushroom", STICKERNAMES.STICKER_MUSHROOM, "TODO:DbStickers", PermitRarity.Unknown, "sticker_mushroom_kanim", "mushroom"),
			new StickerBombFacadeInfo("mermaid", STICKERNAMES.STICKER_MERMAID, "TODO:DbStickers", PermitRarity.Unknown, "sticker_mermaid_kanim", "mermaid"),
			new StickerBombFacadeInfo("spacepet", STICKERNAMES.STICKER_SPACEPET, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet_kanim", "spacepet"),
			new StickerBombFacadeInfo("spacepet2", STICKERNAMES.STICKER_SPACEPET2, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet2_kanim", "spacepet2"),
			new StickerBombFacadeInfo("spacepet3", STICKERNAMES.STICKER_SPACEPET3, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet3_kanim", "spacepet3"),
			new StickerBombFacadeInfo("spacepet4", STICKERNAMES.STICKER_SPACEPET4, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet4_kanim", "spacepet4"),
			new StickerBombFacadeInfo("spacepet5", STICKERNAMES.STICKER_SPACEPET5, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet5_kanim", "spacepet5"),
			new StickerBombFacadeInfo("unicorn", STICKERNAMES.STICKER_UNICORN, "TODO:DbStickers", PermitRarity.Unknown, "sticker_unicorn_kanim", "unicorn")
		});
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x0009A498 File Offset: 0x00098698
	private void SetupEquippableFacades()
	{
		this.blueprintCollection.equippableFacades.AddRange(new EquippableFacadeInfo[]
		{
			new EquippableFacadeInfo("clubshirt", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.CLUBSHIRT, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_clubshirt_kanim", "shirt_clubshirt_kanim"),
			new EquippableFacadeInfo("cummerbund", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.CUMMERBUND, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_cummerbund_kanim", "shirt_cummerbund_kanim"),
			new EquippableFacadeInfo("decor_02", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.DECOR_02, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_decor02_kanim", "shirt_decor02_kanim"),
			new EquippableFacadeInfo("decor_03", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.DECOR_03, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_decor03_kanim", "shirt_decor03_kanim"),
			new EquippableFacadeInfo("decor_04", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.DECOR_04, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_decor04_kanim", "shirt_decor04_kanim"),
			new EquippableFacadeInfo("decor_05", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.DECOR_05, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_decor05_kanim", "shirt_decor05_kanim"),
			new EquippableFacadeInfo("gaudysweater", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.GAUDYSWEATER, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_gaudysweater_kanim", "shirt_gaudysweater_kanim"),
			new EquippableFacadeInfo("limone", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.LIMONE, "n/a", PermitRarity.Unknown, "CustomClothing", "body_suit_limone_kanim", "suit_limone_kanim"),
			new EquippableFacadeInfo("mondrian", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.MONDRIAN, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_mondrian_kanim", "shirt_mondrian_kanim"),
			new EquippableFacadeInfo("overalls", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.OVERALLS, "n/a", PermitRarity.Unknown, "CustomClothing", "body_suit_overalls_kanim", "suit_overalls_kanim"),
			new EquippableFacadeInfo("triangles", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.TRIANGLES, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_triangles_kanim", "shirt_triangles_kanim"),
			new EquippableFacadeInfo("workout", EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.WORKOUT, "n/a", PermitRarity.Unknown, "CustomClothing", "body_suit_workout_kanim", "suit_workout_kanim")
		});
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x0009A6D0 File Offset: 0x000988D0
	private void SetupMonumentParts()
	{
		this.blueprintCollection.monumentParts.AddRange(new MonumentPartInfo[]
		{
			new MonumentPartInfo("bottom_option_a", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_a_kanim", "option_a", "straight_legs", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_b", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_b_kanim", "option_b", "wide_stance", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_c", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_c_kanim", "option_c", "hmmm_legs", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_d", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_d_kanim", "option_d", "sitting_stool", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_e", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_e_kanim", "option_e", "wide_stance2", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_f", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_f_kanim", "option_f", "posing1", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_g", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_g_kanim", "option_g", "knee_kick", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_h", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_h_kanim", "option_h", "step_on_hatches", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_i", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_i_kanim", "option_i", "sit_on_tools", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_j", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_j_kanim", "option_j", "water_pacu", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("bottom_option_k", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_k_kanim", "option_k", "sit_on_eggs", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_a", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_a_kanim", "option_a", "thumbs_up", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_b", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_b_kanim", "option_b", "wrench", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_c", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_c_kanim", "option_c", "hmmm", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_d", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_d_kanim", "option_d", "hips_hands", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_e", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_e_kanim", "option_e", "hold_face", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_f", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_f_kanim", "option_f", "finger_gun", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_g", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_g_kanim", "option_g", "model_pose", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_h", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_h_kanim", "option_h", "punch", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_i", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_i_kanim", "option_i", "holding_hatch", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_j", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_j_kanim", "option_j", "model_pose2", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_k", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_k_kanim", "option_k", "balancing", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("mid_option_l", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_l_kanim", "option_l", "holding_babies", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_a", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_a_kanim", "option_a", "leira", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_b", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_b_kanim", "option_b", "mae", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_c", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_c_kanim", "option_c", "puft", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_d", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_d_kanim", "option_d", "nikola", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_e", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_e_kanim", "option_e", "burt", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_f", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_f_kanim", "option_f", "rowan", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_g", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_g_kanim", "option_g", "nisbet", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_h", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_h_kanim", "option_h", "joshua", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_i", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_i_kanim", "option_i", "ren", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_j", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_j_kanim", "option_j", "hatch", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_k", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_k_kanim", "option_k", "drecko", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_l", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_l_kanim", "option_l", "driller", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_m", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_m_kanim", "option_m", "gassymoo", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_n", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_n_kanim", "option_n", "glom", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_o", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_o_kanim", "option_o", "lightbug", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_p", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_p_kanim", "option_p", "slickster", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS),
			new MonumentPartInfo("top_option_q", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_q_kanim", "option_q", "pacu", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_ALL_VERSIONS)
		});
		this.blueprintCollection.monumentParts.AddRange(new MonumentPartInfo[]
		{
			new MonumentPartInfo("bottom_option_l", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_l_kanim", "option_l", "rocketnosecone", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("bottom_option_m", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_m_kanim", "option_m", "rocketsugarengine", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("bottom_option_n", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_n_kanim", "option_n", "rocketnCO2", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("bottom_option_o", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_o_kanim", "option_o", "rocketpetro", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("bottom_option_p", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_p_kanim", "option_p", "rocketnoseconesmall", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("bottom_option_q", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_q_kanim", "option_q", "rocketradengine", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("bottom_option_r", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_r_kanim", "option_r", "sweepyoff", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("bottom_option_s", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_s_kanim", "option_s", "sweepypeek", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("bottom_option_t", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_base_t_kanim", "option_t", "sweepy", MonumentPartResource.Part.Bottom, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("mid_option_m", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_m_kanim", "option_m", "rocket", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("mid_option_n", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_n_kanim", "option_n", "holding_baby_worm", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("mid_option_o", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_mid_o_kanim", "option_o", "holding_baby_blarva_critter", MonumentPartResource.Part.Middle, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_r", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_r_kanim", "option_r", "bee", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_s", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_s_kanim", "option_s", "critter", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_t", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_t_kanim", "option_t", "caterpillar", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_u", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_u_kanim", "option_u", "worm", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_v", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_v_kanim", "option_v", "scout_bot", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_w", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_w_kanim", "option_w", "MiMa", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_x", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_x_kanim", "option_x", "Stinky", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_y", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_y_kanim", "option_y", "Harold", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new MonumentPartInfo("top_option_z", "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitRarity.Unknown, "monument_upper_z_kanim", "option_z", "Nails", MonumentPartResource.Part.Top, DlcManager.AVAILABLE_EXPANSION1_ONLY)
		});
	}
}

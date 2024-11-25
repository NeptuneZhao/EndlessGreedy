using System;
using Database;

// Token: 0x0200051D RID: 1309
public abstract class BlueprintProvider
{
	// Token: 0x06001D4C RID: 7500 RVA: 0x00099290 File Offset: 0x00097490
	protected void AddBuilding(string prefabConfigId, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, prefabConfigId, animFile, this.dlcIds, null));
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x000992FC File Offset: 0x000974FC
	protected void AddClothing(BlueprintProvider.ClothingType clothingType, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.clothingItems.Add(new ClothingItemInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), (PermitCategory)clothingType, rarity, animFile)
		{
			dlcIds = this.dlcIds
		});
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x00099370 File Offset: 0x00097570
	protected BlueprintProvider.ArtableInfoAuthoringHelper AddArtable(BlueprintProvider.ArtableType artableType, PermitRarity rarity, string permitId, string animFile)
	{
		string text;
		switch (artableType)
		{
		case BlueprintProvider.ArtableType.Painting:
			text = "Canvas";
			break;
		case BlueprintProvider.ArtableType.PaintingTall:
			text = "CanvasTall";
			break;
		case BlueprintProvider.ArtableType.PaintingWide:
			text = "CanvasWide";
			break;
		case BlueprintProvider.ArtableType.Sculpture:
			text = "Sculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureSmall:
			text = "SmallSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureIce:
			text = "IceSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureMetal:
			text = "MetalSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureMarble:
			text = "MarbleSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureWood:
			text = "WoodSculpture";
			break;
		default:
			text = null;
			break;
		}
		bool flag = true;
		if (text == null)
		{
			DebugUtil.DevAssert(false, "Failed to get buildingConfigId from " + artableType.ToString(), null);
			flag = false;
		}
		BlueprintProvider.ArtableInfoAuthoringHelper result;
		if (flag)
		{
			KAnimFile kanimFile;
			ArtableInfo artableInfo = new ArtableInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, (!Assets.TryGetAnim(animFile, out kanimFile)) ? null : kanimFile.GetData().GetAnim(0).name, 0, false, "error", text, "")
			{
				dlcIds = this.dlcIds
			};
			result = new BlueprintProvider.ArtableInfoAuthoringHelper(artableType, artableInfo);
			result.Quality(BlueprintProvider.ArtableQuality.LookingGreat);
			this.blueprintCollection.artables.Add(artableInfo);
		}
		else
		{
			result = default(BlueprintProvider.ArtableInfoAuthoringHelper);
		}
		return result;
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000994D4 File Offset: 0x000976D4
	protected void AddJoyResponse(BlueprintProvider.JoyResponseType joyResponseType, PermitRarity rarity, string permitId, string animFile)
	{
		if (joyResponseType == BlueprintProvider.JoyResponseType.BallonSet)
		{
			this.blueprintCollection.balloonArtistFacades.Add(new BalloonArtistFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, BalloonArtistFacadeType.ThreeSet)
			{
				dlcIds = this.dlcIds
			});
			return;
		}
		throw new NotImplementedException("Missing case for " + joyResponseType.ToString());
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x00099568 File Offset: 0x00097768
	protected void AddOutfit(BlueprintProvider.OutfitType outfitType, string outfitId, string[] permitIdList)
	{
		this.blueprintCollection.outfits.Add(new ClothingOutfitResource(outfitId, permitIdList, Strings.Get("STRINGS.BLUEPRINTS." + outfitId.ToUpper() + ".NAME"), (ClothingOutfitUtility.OutfitType)outfitType)
		{
			dlcIds = this.dlcIds
		});
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x000995B8 File Offset: 0x000977B8
	public virtual string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001D52 RID: 7506
	public abstract void SetupBlueprints();

	// Token: 0x06001D53 RID: 7507 RVA: 0x000995BF File Offset: 0x000977BF
	public void Interal_PreSetupBlueprints()
	{
		this.dlcIds = this.GetDlcIds();
	}

	// Token: 0x0400108C RID: 4236
	public BlueprintCollection blueprintCollection;

	// Token: 0x0400108D RID: 4237
	private string[] dlcIds;

	// Token: 0x020012D5 RID: 4821
	public enum ArtableType
	{
		// Token: 0x040064B4 RID: 25780
		Painting,
		// Token: 0x040064B5 RID: 25781
		PaintingTall,
		// Token: 0x040064B6 RID: 25782
		PaintingWide,
		// Token: 0x040064B7 RID: 25783
		Sculpture,
		// Token: 0x040064B8 RID: 25784
		SculptureSmall,
		// Token: 0x040064B9 RID: 25785
		SculptureIce,
		// Token: 0x040064BA RID: 25786
		SculptureMetal,
		// Token: 0x040064BB RID: 25787
		SculptureMarble,
		// Token: 0x040064BC RID: 25788
		SculptureWood
	}

	// Token: 0x020012D6 RID: 4822
	public enum ArtableQuality
	{
		// Token: 0x040064BE RID: 25790
		LookingGreat,
		// Token: 0x040064BF RID: 25791
		LookingOkay,
		// Token: 0x040064C0 RID: 25792
		LookingUgly
	}

	// Token: 0x020012D7 RID: 4823
	public enum ClothingType
	{
		// Token: 0x040064C2 RID: 25794
		DupeTops = 1,
		// Token: 0x040064C3 RID: 25795
		DupeBottoms,
		// Token: 0x040064C4 RID: 25796
		DupeGloves,
		// Token: 0x040064C5 RID: 25797
		DupeShoes,
		// Token: 0x040064C6 RID: 25798
		DupeHats,
		// Token: 0x040064C7 RID: 25799
		DupeAccessories,
		// Token: 0x040064C8 RID: 25800
		AtmoSuitHelmet,
		// Token: 0x040064C9 RID: 25801
		AtmoSuitBody,
		// Token: 0x040064CA RID: 25802
		AtmoSuitGloves,
		// Token: 0x040064CB RID: 25803
		AtmoSuitBelt,
		// Token: 0x040064CC RID: 25804
		AtmoSuitShoes
	}

	// Token: 0x020012D8 RID: 4824
	public enum OutfitType
	{
		// Token: 0x040064CE RID: 25806
		Clothing,
		// Token: 0x040064CF RID: 25807
		AtmoSuit = 2
	}

	// Token: 0x020012D9 RID: 4825
	public enum JoyResponseType
	{
		// Token: 0x040064D1 RID: 25809
		BallonSet
	}

	// Token: 0x020012DA RID: 4826
	protected readonly ref struct ArtableInfoAuthoringHelper
	{
		// Token: 0x060084F0 RID: 34032 RVA: 0x00325186 File Offset: 0x00323386
		public ArtableInfoAuthoringHelper(BlueprintProvider.ArtableType artableType, ArtableInfo artableInfo)
		{
			this.artableType = artableType;
			this.artableInfo = artableInfo;
		}

		// Token: 0x060084F1 RID: 34033 RVA: 0x00325198 File Offset: 0x00323398
		public void Quality(BlueprintProvider.ArtableQuality artableQuality)
		{
			if (this.artableInfo == null)
			{
				return;
			}
			int num;
			int num2;
			int num3;
			if (this.artableType == BlueprintProvider.ArtableType.SculptureWood)
			{
				num = 4;
				num2 = 8;
				num3 = 12;
			}
			else
			{
				num = 5;
				num2 = 10;
				num3 = 15;
			}
			int decor_value;
			bool cheer_on_complete;
			string status_id;
			switch (artableQuality)
			{
			case BlueprintProvider.ArtableQuality.LookingGreat:
				decor_value = num3;
				cheer_on_complete = true;
				status_id = "LookingGreat";
				break;
			case BlueprintProvider.ArtableQuality.LookingOkay:
				decor_value = num2;
				cheer_on_complete = false;
				status_id = "LookingOkay";
				break;
			case BlueprintProvider.ArtableQuality.LookingUgly:
				decor_value = num;
				cheer_on_complete = false;
				status_id = "LookingUgly";
				break;
			default:
				throw new ArgumentException();
			}
			this.artableInfo.decor_value = decor_value;
			this.artableInfo.cheer_on_complete = cheer_on_complete;
			this.artableInfo.status_id = status_id;
		}

		// Token: 0x040064D2 RID: 25810
		private readonly BlueprintProvider.ArtableType artableType;

		// Token: 0x040064D3 RID: 25811
		private readonly ArtableInfo artableInfo;
	}
}

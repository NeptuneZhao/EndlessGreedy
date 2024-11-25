using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class BlueprintCollection
{
	// Token: 0x06001CE0 RID: 7392 RVA: 0x00098ACE File Offset: 0x00096CCE
	public void AddBlueprintsFrom<T>(T provider) where T : BlueprintProvider
	{
		provider.blueprintCollection = this;
		provider.Interal_PreSetupBlueprints();
		provider.SetupBlueprints();
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x00098AF4 File Offset: 0x00096CF4
	public void AddBlueprintsFrom(BlueprintCollection collection)
	{
		this.artables.AddRange(collection.artables);
		this.buildingFacades.AddRange(collection.buildingFacades);
		this.clothingItems.AddRange(collection.clothingItems);
		this.balloonArtistFacades.AddRange(collection.balloonArtistFacades);
		this.stickerBombFacades.AddRange(collection.stickerBombFacades);
		this.equippableFacades.AddRange(collection.equippableFacades);
		this.monumentParts.AddRange(collection.monumentParts);
		this.outfits.AddRange(collection.outfits);
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x00098B8C File Offset: 0x00096D8C
	public void PostProcess()
	{
		if (Application.isPlaying)
		{
			this.artables.RemoveAll(new Predicate<ArtableInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.buildingFacades.RemoveAll(new Predicate<BuildingFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.clothingItems.RemoveAll(new Predicate<ClothingItemInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.balloonArtistFacades.RemoveAll(new Predicate<BalloonArtistFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.stickerBombFacades.RemoveAll(new Predicate<StickerBombFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.equippableFacades.RemoveAll(new Predicate<EquippableFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.monumentParts.RemoveAll(new Predicate<MonumentPartInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.outfits.RemoveAll(new Predicate<ClothingOutfitResource>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
		}
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x00098CD0 File Offset: 0x00096ED0
	[CompilerGenerated]
	internal static bool <PostProcess>g__ShouldExcludeBlueprint|10_0(IBlueprintDlcInfo blueprintDlcInfo)
	{
		if (!DlcManager.IsAnyContentSubscribed(blueprintDlcInfo.dlcIds))
		{
			return true;
		}
		IBlueprintInfo blueprintInfo = blueprintDlcInfo as IBlueprintInfo;
		KAnimFile kanimFile;
		if (blueprintInfo != null && !Assets.TryGetAnim(blueprintInfo.animFile, out kanimFile))
		{
			DebugUtil.DevAssert(false, string.Concat(new string[]
			{
				"Couldnt find anim \"",
				blueprintInfo.animFile,
				"\" for blueprint \"",
				blueprintInfo.id,
				"\""
			}), null);
		}
		return false;
	}

	// Token: 0x04001049 RID: 4169
	public List<ArtableInfo> artables = new List<ArtableInfo>();

	// Token: 0x0400104A RID: 4170
	public List<BuildingFacadeInfo> buildingFacades = new List<BuildingFacadeInfo>();

	// Token: 0x0400104B RID: 4171
	public List<ClothingItemInfo> clothingItems = new List<ClothingItemInfo>();

	// Token: 0x0400104C RID: 4172
	public List<BalloonArtistFacadeInfo> balloonArtistFacades = new List<BalloonArtistFacadeInfo>();

	// Token: 0x0400104D RID: 4173
	public List<StickerBombFacadeInfo> stickerBombFacades = new List<StickerBombFacadeInfo>();

	// Token: 0x0400104E RID: 4174
	public List<EquippableFacadeInfo> equippableFacades = new List<EquippableFacadeInfo>();

	// Token: 0x0400104F RID: 4175
	public List<MonumentPartInfo> monumentParts = new List<MonumentPartInfo>();

	// Token: 0x04001050 RID: 4176
	public List<ClothingOutfitResource> outfits = new List<ClothingOutfitResource>();
}

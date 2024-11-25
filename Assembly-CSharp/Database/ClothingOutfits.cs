﻿using System;

namespace Database
{
	// Token: 0x02000E58 RID: 3672
	public class ClothingOutfits : ResourceSet<ClothingOutfitResource>
	{
		// Token: 0x06007466 RID: 29798 RVA: 0x002D08F4 File Offset: 0x002CEAF4
		public ClothingOutfits(ResourceSet parent, ClothingItems items_resource) : base("ClothingOutfits", parent)
		{
			base.Initialize();
			this.resources.AddRange(Blueprints.Get().all.outfits);
			foreach (ClothingOutfitResource clothingOutfitResource in this.resources)
			{
				string[] itemsInOutfit = clothingOutfitResource.itemsInOutfit;
				for (int i = 0; i < itemsInOutfit.Length; i++)
				{
					string itemId = itemsInOutfit[i];
					int num = items_resource.resources.FindIndex((ClothingItemResource e) => e.Id == itemId);
					if (num < 0)
					{
						DebugUtil.DevAssert(false, string.Concat(new string[]
						{
							"Outfit \"",
							clothingOutfitResource.Id,
							"\" contains an item that doesn't exist. Given item id: \"",
							itemId,
							"\""
						}), null);
					}
					else
					{
						ClothingItemResource clothingItemResource = items_resource.resources[num];
						if (clothingItemResource.outfitType != clothingOutfitResource.outfitType)
						{
							DebugUtil.DevAssert(false, string.Format("Outfit \"{0}\" contains an item that has a mis-matched outfit type. Defined outfit's type: \"{1}\". Given item: {{ id: \"{2}\" forOutfitType: \"{3}\" }}", new object[]
							{
								clothingOutfitResource.Id,
								clothingOutfitResource.outfitType,
								itemId,
								clothingItemResource.outfitType
							}), null);
						}
					}
				}
			}
			ClothingOutfitUtility.LoadClothingOutfitData(this);
		}
	}
}

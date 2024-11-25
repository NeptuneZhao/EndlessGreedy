using System;

namespace Database
{
	// Token: 0x02000E56 RID: 3670
	public class ClothingItems : ResourceSet<ClothingItemResource>
	{
		// Token: 0x06007459 RID: 29785 RVA: 0x002D06BC File Offset: 0x002CE8BC
		public ClothingItems(ResourceSet parent) : base("ClothingItems", parent)
		{
			base.Initialize();
			foreach (ClothingItemInfo clothingItemInfo in Blueprints.Get().all.clothingItems)
			{
				this.Add(clothingItemInfo.id, clothingItemInfo.name, clothingItemInfo.desc, clothingItemInfo.outfitType, clothingItemInfo.category, clothingItemInfo.rarity, clothingItemInfo.animFile, clothingItemInfo.dlcIds);
			}
		}

		// Token: 0x0600745A RID: 29786 RVA: 0x002D075C File Offset: 0x002CE95C
		public ClothingItemResource TryResolveAccessoryResource(ResourceGuid AccessoryGuid)
		{
			if (AccessoryGuid.Guid != null)
			{
				string[] array = AccessoryGuid.Guid.Split('.', StringSplitOptions.None);
				if (array.Length != 0)
				{
					string symbol_name = array[array.Length - 1];
					return this.resources.Find((ClothingItemResource ci) => symbol_name.Contains(ci.Id));
				}
			}
			return null;
		}

		// Token: 0x0600745B RID: 29787 RVA: 0x002D07B0 File Offset: 0x002CE9B0
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile)
		{
			this.Add(id, name, desc, outfitType, category, rarity, animFile, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x0600745C RID: 29788 RVA: 0x002D07D4 File Offset: 0x002CE9D4
		public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] dlcIds)
		{
			ClothingItemResource item = new ClothingItemResource(id, name, desc, outfitType, category, rarity, animFile, dlcIds);
			this.resources.Add(item);
		}
	}
}

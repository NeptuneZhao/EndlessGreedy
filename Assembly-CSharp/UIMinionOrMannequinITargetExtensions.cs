using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;

// Token: 0x02000DE5 RID: 3557
public static class UIMinionOrMannequinITargetExtensions
{
	// Token: 0x060070F6 RID: 28918 RVA: 0x002AC473 File Offset: 0x002AA673
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitResource outfit)
	{
		self.SetOutfit(outfit.outfitType, from itemId in outfit.itemsInOutfit
		select Db.Get().Permits.ClothingItems.Get(itemId));
	}

	// Token: 0x060070F7 RID: 28919 RVA: 0x002AC4AB File Offset: 0x002AA6AB
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, OutfitDesignerScreen_OutfitState outfit)
	{
		self.SetOutfit(outfit.outfitType, from itemId in outfit.GetItems()
		select Db.Get().Permits.ClothingItems.Get(itemId));
	}

	// Token: 0x060070F8 RID: 28920 RVA: 0x002AC4E3 File Offset: 0x002AA6E3
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitTarget outfit)
	{
		self.SetOutfit(outfit.OutfitType, outfit.ReadItemValues());
	}

	// Token: 0x060070F9 RID: 28921 RVA: 0x002AC4F9 File Offset: 0x002AA6F9
	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitUtility.OutfitType outfitType, Option<ClothingOutfitTarget> outfit)
	{
		if (outfit.HasValue)
		{
			self.SetOutfit(outfit.Value);
			return;
		}
		self.ClearOutfit(outfitType);
	}

	// Token: 0x060070FA RID: 28922 RVA: 0x002AC519 File Offset: 0x002AA719
	public static void ClearOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitUtility.OutfitType outfitType)
	{
		self.SetOutfit(outfitType, UIMinionOrMannequinITargetExtensions.EMPTY_OUTFIT);
	}

	// Token: 0x060070FB RID: 28923 RVA: 0x002AC527 File Offset: 0x002AA727
	public static void React(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.None);
	}

	// Token: 0x060070FC RID: 28924 RVA: 0x002AC530 File Offset: 0x002AA730
	public static void ReactToClothingItemChange(this UIMinionOrMannequin.ITarget self, PermitCategory clothingChangedCategory)
	{
		self.React(UIMinionOrMannequinITargetExtensions.<ReactToClothingItemChange>g__GetSource|7_0(clothingChangedCategory));
	}

	// Token: 0x060070FD RID: 28925 RVA: 0x002AC53E File Offset: 0x002AA73E
	public static void ReactToPersonalityChange(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.OnPersonalityChanged);
	}

	// Token: 0x060070FE RID: 28926 RVA: 0x002AC547 File Offset: 0x002AA747
	public static void ReactToFullOutfitChange(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.OnWholeOutfitChanged);
	}

	// Token: 0x060070FF RID: 28927 RVA: 0x002AC550 File Offset: 0x002AA750
	public static IEnumerable<ClothingItemResource> GetOutfitWithDefaultItems(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return outfit;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			throw new NotSupportedException();
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			using (DictionaryPool<PermitCategory, ClothingItemResource, UIMinionOrMannequin.ITarget>.PooledDictionary pooledDictionary = PoolsFor<UIMinionOrMannequin.ITarget>.AllocateDict<PermitCategory, ClothingItemResource>())
			{
				foreach (ClothingItemResource clothingItemResource in outfit)
				{
					DebugUtil.DevAssert(!pooledDictionary.ContainsKey(clothingItemResource.Category), "Duplicate item for category", null);
					pooledDictionary[clothingItemResource.Category] = clothingItemResource;
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitHelmet))
				{
					pooledDictionary[PermitCategory.AtmoSuitHelmet] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoHelmetClear");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBody))
				{
					pooledDictionary[PermitCategory.AtmoSuitBody] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoSuitBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitGloves))
				{
					pooledDictionary[PermitCategory.AtmoSuitGloves] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoGlovesBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBelt))
				{
					pooledDictionary[PermitCategory.AtmoSuitBelt] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoBeltBasicBlue");
				}
				if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitShoes))
				{
					pooledDictionary[PermitCategory.AtmoSuitShoes] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoShoesBasicBlack");
				}
				return pooledDictionary.Values.ToArray<ClothingItemResource>();
			}
			break;
		}
		throw new NotImplementedException();
	}

	// Token: 0x06007101 RID: 28929 RVA: 0x002AC704 File Offset: 0x002AA904
	[CompilerGenerated]
	internal static UIMinionOrMannequinReactSource <ReactToClothingItemChange>g__GetSource|7_0(PermitCategory clothingChangedCategory)
	{
		switch (clothingChangedCategory)
		{
		case PermitCategory.DupeTops:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitBelt:
			return UIMinionOrMannequinReactSource.OnTopChanged;
		case PermitCategory.DupeBottoms:
			return UIMinionOrMannequinReactSource.OnBottomChanged;
		case PermitCategory.DupeGloves:
		case PermitCategory.AtmoSuitGloves:
			return UIMinionOrMannequinReactSource.OnGlovesChanged;
		case PermitCategory.DupeShoes:
		case PermitCategory.AtmoSuitShoes:
			return UIMinionOrMannequinReactSource.OnShoesChanged;
		case PermitCategory.DupeHats:
		case PermitCategory.AtmoSuitHelmet:
			return UIMinionOrMannequinReactSource.OnHatChanged;
		}
		DebugUtil.DevAssert(false, string.Format("Couldn't find a reaction for \"{0}\" clothing item category being changed", clothingChangedCategory), null);
		return UIMinionOrMannequinReactSource.None;
	}

	// Token: 0x04004DB2 RID: 19890
	public static readonly ClothingItemResource[] EMPTY_OUTFIT = new ClothingItemResource[0];
}

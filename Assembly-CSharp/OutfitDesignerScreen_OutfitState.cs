using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000D05 RID: 3333
public class OutfitDesignerScreen_OutfitState
{
	// Token: 0x06006799 RID: 26521 RVA: 0x0026AFB8 File Offset: 0x002691B8
	private OutfitDesignerScreen_OutfitState(ClothingOutfitUtility.OutfitType outfitType, ClothingOutfitTarget sourceTarget, ClothingOutfitTarget destinationTarget)
	{
		this.outfitType = outfitType;
		this.destinationTarget = destinationTarget;
		this.sourceTarget = sourceTarget;
		this.name = sourceTarget.ReadName();
		this.slots = OutfitDesignerScreen_OutfitState.Slots.For(outfitType);
		foreach (ClothingItemResource item in sourceTarget.ReadItemValues())
		{
			this.ApplyItem(item);
		}
	}

	// Token: 0x0600679A RID: 26522 RVA: 0x0026B03C File Offset: 0x0026923C
	public static OutfitDesignerScreen_OutfitState ForTemplateOutfit(ClothingOutfitTarget outfitTemplate)
	{
		global::Debug.Assert(outfitTemplate.IsTemplateOutfit());
		return new OutfitDesignerScreen_OutfitState(outfitTemplate.OutfitType, outfitTemplate, outfitTemplate);
	}

	// Token: 0x0600679B RID: 26523 RVA: 0x0026B058 File Offset: 0x00269258
	public static OutfitDesignerScreen_OutfitState ForMinionInstance(ClothingOutfitTarget sourceTarget, GameObject minionInstance)
	{
		return new OutfitDesignerScreen_OutfitState(sourceTarget.OutfitType, sourceTarget, ClothingOutfitTarget.FromMinion(sourceTarget.OutfitType, minionInstance));
	}

	// Token: 0x0600679C RID: 26524 RVA: 0x0026B074 File Offset: 0x00269274
	public unsafe void ApplyItem(ClothingItemResource item)
	{
		*this.slots.GetItemSlotForCategory(item.Category) = item;
	}

	// Token: 0x0600679D RID: 26525 RVA: 0x0026B092 File Offset: 0x00269292
	public unsafe Option<ClothingItemResource> GetItemForCategory(PermitCategory category)
	{
		return *this.slots.GetItemSlotForCategory(category);
	}

	// Token: 0x0600679E RID: 26526 RVA: 0x0026B0A8 File Offset: 0x002692A8
	public unsafe void SetItemForCategory(PermitCategory category, Option<ClothingItemResource> item)
	{
		if (item.IsSome())
		{
			DebugUtil.DevAssert(item.Unwrap().outfitType == this.outfitType, string.Format("Tried to set clothing item with outfit type \"{0}\" to outfit of type \"{1}\"", item.Unwrap().outfitType, this.outfitType), null);
			DebugUtil.DevAssert(item.Unwrap().Category == category, string.Format("Tried to set clothing item with category \"{0}\" to slot with type \"{1}\"", item.Unwrap().Category, category), null);
		}
		*this.slots.GetItemSlotForCategory(category) = item;
	}

	// Token: 0x0600679F RID: 26527 RVA: 0x0026B148 File Offset: 0x00269348
	public void AddItemValuesTo(ICollection<ClothingItemResource> clothingItems)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				clothingItems.Add(ptr.Unwrap());
			}
		}
	}

	// Token: 0x060067A0 RID: 26528 RVA: 0x0026B194 File Offset: 0x00269394
	public void AddItemsTo(ICollection<string> itemIds)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				itemIds.Add(ptr.Unwrap().Id);
			}
		}
	}

	// Token: 0x060067A1 RID: 26529 RVA: 0x0026B1E4 File Offset: 0x002693E4
	public string[] GetItems()
	{
		List<string> list = new List<string>();
		this.AddItemsTo(list);
		return list.ToArray();
	}

	// Token: 0x060067A2 RID: 26530 RVA: 0x0026B204 File Offset: 0x00269404
	public bool DoesContainLockedItems()
	{
		bool result;
		using (ListPool<string, OutfitDesignerScreen_OutfitState>.PooledList pooledList = PoolsFor<OutfitDesignerScreen_OutfitState>.AllocateList<string>())
		{
			this.AddItemsTo(pooledList);
			result = ClothingOutfitTarget.DoesContainLockedItems(pooledList);
		}
		return result;
	}

	// Token: 0x060067A3 RID: 26531 RVA: 0x0026B244 File Offset: 0x00269444
	public bool IsDirty()
	{
		using (HashSetPool<string, OutfitDesignerScreen>.PooledHashSet pooledHashSet = PoolsFor<OutfitDesignerScreen>.AllocateHashSet<string>())
		{
			this.AddItemsTo(pooledHashSet);
			string[] array = this.destinationTarget.ReadItems();
			if (pooledHashSet.Count != array.Length)
			{
				return true;
			}
			foreach (string item in array)
			{
				if (!pooledHashSet.Contains(item))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x040045F1 RID: 17905
	public string name;

	// Token: 0x040045F2 RID: 17906
	private OutfitDesignerScreen_OutfitState.Slots slots;

	// Token: 0x040045F3 RID: 17907
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x040045F4 RID: 17908
	public ClothingOutfitTarget sourceTarget;

	// Token: 0x040045F5 RID: 17909
	public ClothingOutfitTarget destinationTarget;

	// Token: 0x02001E21 RID: 7713
	public abstract class Slots
	{
		// Token: 0x0600AAA0 RID: 43680 RVA: 0x003A2DD0 File Offset: 0x003A0FD0
		private Slots(int slotsCount)
		{
			this.array = new Option<ClothingItemResource>[slotsCount];
		}

		// Token: 0x0600AAA1 RID: 43681 RVA: 0x003A2DE4 File Offset: 0x003A0FE4
		public static OutfitDesignerScreen_OutfitState.Slots For(ClothingOutfitUtility.OutfitType outfitType)
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
				return new OutfitDesignerScreen_OutfitState.Slots.Clothing();
			case ClothingOutfitUtility.OutfitType.JoyResponse:
				throw new NotSupportedException("OutfitType.JoyResponse cannot be used with OutfitDesignerScreen_OutfitState. Use JoyResponseOutfitTarget instead.");
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				return new OutfitDesignerScreen_OutfitState.Slots.Atmosuit();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600AAA2 RID: 43682
		public abstract ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category);

		// Token: 0x0600AAA3 RID: 43683 RVA: 0x003A2E18 File Offset: 0x003A1018
		private ref Option<ClothingItemResource> FallbackSlot(OutfitDesignerScreen_OutfitState.Slots self, PermitCategory category)
		{
			DebugUtil.DevAssert(false, string.Format("Couldn't get a {0}<{1}> for {2} \"{3}\" on {4}.{5}", new object[]
			{
				"Option",
				"ClothingItemResource",
				"PermitCategory",
				category,
				"Slots",
				self.GetType().Name
			}), null);
			return ref OutfitDesignerScreen_OutfitState.Slots.dummySlot;
		}

		// Token: 0x04008974 RID: 35188
		public Option<ClothingItemResource>[] array;

		// Token: 0x04008975 RID: 35189
		private static Option<ClothingItemResource> dummySlot;

		// Token: 0x0200265A RID: 9818
		public class Clothing : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600C221 RID: 49697 RVA: 0x003DFF47 File Offset: 0x003DE147
			public Clothing() : base(6)
			{
			}

			// Token: 0x17000C2C RID: 3116
			// (get) Token: 0x0600C222 RID: 49698 RVA: 0x003DFF50 File Offset: 0x003DE150
			public ref Option<ClothingItemResource> hatSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000C2D RID: 3117
			// (get) Token: 0x0600C223 RID: 49699 RVA: 0x003DFF5E File Offset: 0x003DE15E
			public ref Option<ClothingItemResource> topSlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000C2E RID: 3118
			// (get) Token: 0x0600C224 RID: 49700 RVA: 0x003DFF6C File Offset: 0x003DE16C
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000C2F RID: 3119
			// (get) Token: 0x0600C225 RID: 49701 RVA: 0x003DFF7A File Offset: 0x003DE17A
			public ref Option<ClothingItemResource> bottomSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000C30 RID: 3120
			// (get) Token: 0x0600C226 RID: 49702 RVA: 0x003DFF88 File Offset: 0x003DE188
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x17000C31 RID: 3121
			// (get) Token: 0x0600C227 RID: 49703 RVA: 0x003DFF96 File Offset: 0x003DE196
			public ref Option<ClothingItemResource> accessorySlot
			{
				get
				{
					return ref this.array[5];
				}
			}

			// Token: 0x0600C228 RID: 49704 RVA: 0x003DFFA4 File Offset: 0x003DE1A4
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.DupeHats)
				{
					return this.hatSlot;
				}
				if (category == PermitCategory.DupeTops)
				{
					return this.topSlot;
				}
				if (category == PermitCategory.DupeGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.DupeBottoms)
				{
					return this.bottomSlot;
				}
				if (category == PermitCategory.DupeShoes)
				{
					return this.shoesSlot;
				}
				if (category == PermitCategory.DupeAccessories)
				{
					return this.accessorySlot;
				}
				return base.FallbackSlot(this, category);
			}
		}

		// Token: 0x0200265B RID: 9819
		public class Atmosuit : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600C229 RID: 49705 RVA: 0x003DFFFB File Offset: 0x003DE1FB
			public Atmosuit() : base(5)
			{
			}

			// Token: 0x17000C32 RID: 3122
			// (get) Token: 0x0600C22A RID: 49706 RVA: 0x003E0004 File Offset: 0x003DE204
			public ref Option<ClothingItemResource> helmetSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000C33 RID: 3123
			// (get) Token: 0x0600C22B RID: 49707 RVA: 0x003E0012 File Offset: 0x003DE212
			public ref Option<ClothingItemResource> bodySlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000C34 RID: 3124
			// (get) Token: 0x0600C22C RID: 49708 RVA: 0x003E0020 File Offset: 0x003DE220
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000C35 RID: 3125
			// (get) Token: 0x0600C22D RID: 49709 RVA: 0x003E002E File Offset: 0x003DE22E
			public ref Option<ClothingItemResource> beltSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000C36 RID: 3126
			// (get) Token: 0x0600C22E RID: 49710 RVA: 0x003E003C File Offset: 0x003DE23C
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x0600C22F RID: 49711 RVA: 0x003E004C File Offset: 0x003DE24C
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.AtmoSuitHelmet)
				{
					return this.helmetSlot;
				}
				if (category == PermitCategory.AtmoSuitBody)
				{
					return this.bodySlot;
				}
				if (category == PermitCategory.AtmoSuitGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.AtmoSuitBelt)
				{
					return this.beltSlot;
				}
				if (category == PermitCategory.AtmoSuitShoes)
				{
					return this.shoesSlot;
				}
				return base.FallbackSlot(this, category);
			}
		}
	}
}

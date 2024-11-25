using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x020007B6 RID: 1974
public readonly struct ClothingOutfitTarget : IEquatable<ClothingOutfitTarget>
{
	// Token: 0x170003BC RID: 956
	// (get) Token: 0x0600362B RID: 13867 RVA: 0x001272FF File Offset: 0x001254FF
	public string OutfitId
	{
		get
		{
			return this.impl.OutfitId;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x0600362C RID: 13868 RVA: 0x0012730C File Offset: 0x0012550C
	public ClothingOutfitUtility.OutfitType OutfitType
	{
		get
		{
			return this.impl.OutfitType;
		}
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x00127319 File Offset: 0x00125519
	public string[] ReadItems()
	{
		return this.impl.ReadItems(this.OutfitType).Where(new Func<string, bool>(ClothingOutfitTarget.DoesClothingItemExist)).ToArray<string>();
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x00127342 File Offset: 0x00125542
	public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
	{
		this.impl.WriteItems(outfitType, items);
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x0600362F RID: 13871 RVA: 0x00127351 File Offset: 0x00125551
	public bool CanWriteItems
	{
		get
		{
			return this.impl.CanWriteItems;
		}
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x0012735E File Offset: 0x0012555E
	public string ReadName()
	{
		return this.impl.ReadName();
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x0012736B File Offset: 0x0012556B
	public void WriteName(string name)
	{
		this.impl.WriteName(name);
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06003632 RID: 13874 RVA: 0x00127379 File Offset: 0x00125579
	public bool CanWriteName
	{
		get
		{
			return this.impl.CanWriteName;
		}
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x00127386 File Offset: 0x00125586
	public void Delete()
	{
		this.impl.Delete();
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06003634 RID: 13876 RVA: 0x00127393 File Offset: 0x00125593
	public bool CanDelete
	{
		get
		{
			return this.impl.CanDelete;
		}
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x001273A0 File Offset: 0x001255A0
	public bool DoesExist()
	{
		return this.impl.DoesExist();
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x001273AD File Offset: 0x001255AD
	public ClothingOutfitTarget(ClothingOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x001273B6 File Offset: 0x001255B6
	public bool DoesContainLockedItems()
	{
		return ClothingOutfitTarget.DoesContainLockedItems(this.ReadItems());
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x001273C4 File Offset: 0x001255C4
	public static bool DoesContainLockedItems(IList<string> itemIds)
	{
		foreach (string id in itemIds)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(id);
			if (permitResource != null && !permitResource.IsUnlocked())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x00127428 File Offset: 0x00125628
	public IEnumerable<ClothingItemResource> ReadItemValues()
	{
		return from i in this.ReadItems()
		select Db.Get().Permits.ClothingItems.Get(i);
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x00127454 File Offset: 0x00125654
	public static bool DoesClothingItemExist(string clothingItemId)
	{
		return !Db.Get().Permits.ClothingItems.TryGet(clothingItemId).IsNullOrDestroyed();
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x00127473 File Offset: 0x00125673
	public bool Is<T>() where T : ClothingOutfitTarget.Implementation
	{
		return this.impl is T;
	}

	// Token: 0x0600363C RID: 13884 RVA: 0x00127484 File Offset: 0x00125684
	public bool Is<T>(out T value) where T : ClothingOutfitTarget.Implementation
	{
		ClothingOutfitTarget.Implementation implementation = this.impl;
		if (implementation is T)
		{
			T t = (T)((object)implementation);
			value = t;
			return true;
		}
		value = default(T);
		return false;
	}

	// Token: 0x0600363D RID: 13885 RVA: 0x001274B8 File Offset: 0x001256B8
	public bool IsTemplateOutfit()
	{
		return this.Is<ClothingOutfitTarget.DatabaseAuthoredTemplate>() || this.Is<ClothingOutfitTarget.UserAuthoredTemplate>();
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x001274CA File Offset: 0x001256CA
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.NEW)));
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x001274EB File Offset: 0x001256EB
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType, string id)
	{
		if (ClothingOutfitTarget.DoesTemplateExist(id))
		{
			throw new ArgumentException("Can not create a new target with id " + id + ", an outfit with that id already exists");
		}
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, id));
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x0012751C File Offset: 0x0012571C
	public static ClothingOutfitTarget ForTemplateCopyOf(ClothingOutfitTarget sourceTarget)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(sourceTarget.OutfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.COPY_OF.Replace("{OutfitName}", sourceTarget.ReadName()))));
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x0012754F File Offset: 0x0012574F
	public static ClothingOutfitTarget FromMinion(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.MinionInstance(outfitType, minionInstance));
	}

	// Token: 0x06003642 RID: 13890 RVA: 0x00127564 File Offset: 0x00125764
	public static ClothingOutfitTarget FromTemplateId(string outfitId)
	{
		return ClothingOutfitTarget.TryFromTemplateId(outfitId).Value;
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x00127580 File Offset: 0x00125780
	public static Option<ClothingOutfitTarget> TryFromTemplateId(string outfitId)
	{
		if (outfitId == null)
		{
			return Option.None;
		}
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		ClothingOutfitUtility.OutfitType outfitType;
		if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfitId, out customTemplateOutfitEntry) && Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
		}
		ClothingOutfitResource clothingOutfitResource = Db.Get().Permits.ClothingOutfits.TryGet(outfitId);
		if (!clothingOutfitResource.IsNullOrDestroyed())
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(clothingOutfitResource));
		}
		return Option.None;
	}

	// Token: 0x06003644 RID: 13892 RVA: 0x00127619 File Offset: 0x00125819
	public static bool DoesTemplateExist(string outfitId)
	{
		return Db.Get().Permits.ClothingOutfits.TryGet(outfitId) != null || CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(outfitId);
	}

	// Token: 0x06003645 RID: 13893 RVA: 0x0012764E File Offset: 0x0012584E
	public static IEnumerable<ClothingOutfitTarget> GetAllTemplates()
	{
		foreach (ClothingOutfitResource outfit in Db.Get().Permits.ClothingOutfits.resources)
		{
			yield return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(outfit));
		}
		List<ClothingOutfitResource>.Enumerator enumerator = default(List<ClothingOutfitResource>.Enumerator);
		foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> keyValuePair in CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit)
		{
			string text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			keyValuePair.Deconstruct(out text, out customTemplateOutfitEntry);
			string outfitId = text;
			ClothingOutfitUtility.OutfitType outfitType;
			if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				yield return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
			}
		}
		Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator enumerator2 = default(Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06003646 RID: 13894 RVA: 0x00127657 File Offset: 0x00125857
	public static ClothingOutfitTarget GetRandom()
	{
		return ClothingOutfitTarget.GetAllTemplates().GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x00127664 File Offset: 0x00125864
	public static Option<ClothingOutfitTarget> GetRandom(ClothingOutfitUtility.OutfitType onlyOfType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from t in ClothingOutfitTarget.GetAllTemplates()
		where t.OutfitType == onlyOfType
		select t;
		if (enumerable == null || enumerable.Count<ClothingOutfitTarget>() == 0)
		{
			return Option.None;
		}
		return enumerable.GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x001276B8 File Offset: 0x001258B8
	public static string GetUniqueNameIdFrom(string preferredName)
	{
		if (!ClothingOutfitTarget.DoesTemplateExist(preferredName))
		{
			return preferredName;
		}
		string replacement = "testOutfit";
		string a = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 1.ToString());
		string b = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 2.ToString());
		string text;
		if (a != b)
		{
			text = UI.OUTFIT_NAME.RESOLVE_CONFLICT;
		}
		else
		{
			text = "{OutfitName} ({ConflictNumber})";
		}
		for (int i = 1; i < 10000; i++)
		{
			string text2 = text.Replace("{OutfitName}", preferredName).Replace("{ConflictNumber}", i.ToString());
			if (!ClothingOutfitTarget.DoesTemplateExist(text2))
			{
				return text2;
			}
		}
		throw new Exception("Couldn't get a unique name for preferred name: " + preferredName);
	}

	// Token: 0x06003649 RID: 13897 RVA: 0x00127786 File Offset: 0x00125986
	public static bool operator ==(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return a.Equals(b);
	}

	// Token: 0x0600364A RID: 13898 RVA: 0x00127790 File Offset: 0x00125990
	public static bool operator !=(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return !a.Equals(b);
	}

	// Token: 0x0600364B RID: 13899 RVA: 0x001277A0 File Offset: 0x001259A0
	public override bool Equals(object obj)
	{
		if (obj is ClothingOutfitTarget)
		{
			ClothingOutfitTarget other = (ClothingOutfitTarget)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x0600364C RID: 13900 RVA: 0x001277C5 File Offset: 0x001259C5
	public bool Equals(ClothingOutfitTarget other)
	{
		if (this.impl == null || other.impl == null)
		{
			return this.impl == null == (other.impl == null);
		}
		return this.OutfitId == other.OutfitId;
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x001277FE File Offset: 0x001259FE
	public override int GetHashCode()
	{
		return Hash.SDBMLower(this.impl.OutfitId);
	}

	// Token: 0x04002033 RID: 8243
	public readonly ClothingOutfitTarget.Implementation impl;

	// Token: 0x04002034 RID: 8244
	public static readonly string[] NO_ITEMS = new string[0];

	// Token: 0x04002035 RID: 8245
	public static readonly ClothingItemResource[] NO_ITEM_VALUES = new ClothingItemResource[0];

	// Token: 0x02001670 RID: 5744
	public interface Implementation
	{
		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x0600922B RID: 37419
		string OutfitId { get; }

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600922C RID: 37420
		ClothingOutfitUtility.OutfitType OutfitType { get; }

		// Token: 0x0600922D RID: 37421
		string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x0600922E RID: 37422
		void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items);

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x0600922F RID: 37423
		bool CanWriteItems { get; }

		// Token: 0x06009230 RID: 37424
		string ReadName();

		// Token: 0x06009231 RID: 37425
		void WriteName(string name);

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06009232 RID: 37426
		bool CanWriteName { get; }

		// Token: 0x06009233 RID: 37427
		void Delete();

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06009234 RID: 37428
		bool CanDelete { get; }

		// Token: 0x06009235 RID: 37429
		bool DoesExist();
	}

	// Token: 0x02001671 RID: 5745
	public readonly struct MinionInstance : ClothingOutfitTarget.Implementation
	{
		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06009236 RID: 37430 RVA: 0x003536A2 File Offset: 0x003518A2
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06009237 RID: 37431 RVA: 0x003536A5 File Offset: 0x003518A5
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06009238 RID: 37432 RVA: 0x003536A8 File Offset: 0x003518A8
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009239 RID: 37433 RVA: 0x003536AB File Offset: 0x003518AB
		public bool DoesExist()
		{
			return !this.minionInstance.IsNullOrDestroyed();
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x0600923A RID: 37434 RVA: 0x003536BC File Offset: 0x003518BC
		public string OutfitId
		{
			get
			{
				return this.minionInstance.GetInstanceID().ToString() + "_outfit";
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x0600923B RID: 37435 RVA: 0x003536E6 File Offset: 0x003518E6
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x0600923C RID: 37436 RVA: 0x003536EE File Offset: 0x003518EE
		public MinionInstance(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.m_outfitType = outfitType;
			this.accessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x0600923D RID: 37437 RVA: 0x0035370A File Offset: 0x0035190A
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.accessorizer.GetClothingItemsIds(outfitType);
		}

		// Token: 0x0600923E RID: 37438 RVA: 0x00353718 File Offset: 0x00351918
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			this.accessorizer.ClearClothingItems(new ClothingOutfitUtility.OutfitType?(outfitType));
			this.accessorizer.ApplyClothingItems(outfitType, from i in items
			select Db.Get().Permits.ClothingItems.Get(i));
		}

		// Token: 0x0600923F RID: 37439 RVA: 0x00353767 File Offset: 0x00351967
		public string ReadName()
		{
			return UI.OUTFIT_NAME.MINIONS_OUTFIT.Replace("{MinionName}", this.minionInstance.GetProperName());
		}

		// Token: 0x06009240 RID: 37440 RVA: 0x00353783 File Offset: 0x00351983
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not change change the outfit id for a minion instance");
		}

		// Token: 0x06009241 RID: 37441 RVA: 0x0035378F File Offset: 0x0035198F
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a minion instance outfit");
		}

		// Token: 0x04006FAF RID: 28591
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;

		// Token: 0x04006FB0 RID: 28592
		public readonly GameObject minionInstance;

		// Token: 0x04006FB1 RID: 28593
		public readonly WearableAccessorizer accessorizer;
	}

	// Token: 0x02001672 RID: 5746
	public readonly struct UserAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06009242 RID: 37442 RVA: 0x0035379B File Offset: 0x0035199B
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06009243 RID: 37443 RVA: 0x0035379E File Offset: 0x0035199E
		public bool CanWriteName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06009244 RID: 37444 RVA: 0x003537A1 File Offset: 0x003519A1
		public bool CanDelete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009245 RID: 37445 RVA: 0x003537A4 File Offset: 0x003519A4
		public bool DoesExist()
		{
			return CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId);
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06009246 RID: 37446 RVA: 0x003537C0 File Offset: 0x003519C0
		public string OutfitId
		{
			get
			{
				return this.m_outfitId[0];
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06009247 RID: 37447 RVA: 0x003537CA File Offset: 0x003519CA
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06009248 RID: 37448 RVA: 0x003537D2 File Offset: 0x003519D2
		public UserAuthoredTemplate(ClothingOutfitUtility.OutfitType outfitType, string outfitId)
		{
			this.m_outfitId = new string[]
			{
				outfitId
			};
			this.m_outfitType = outfitType;
		}

		// Token: 0x06009249 RID: 37449 RVA: 0x003537EC File Offset: 0x003519EC
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(this.OutfitId, out customTemplateOutfitEntry))
			{
				ClothingOutfitUtility.OutfitType outfitType2;
				global::Debug.Assert(Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType2) && outfitType2 == this.m_outfitType);
				return customTemplateOutfitEntry.itemIds;
			}
			return ClothingOutfitTarget.NO_ITEMS;
		}

		// Token: 0x0600924A RID: 37450 RVA: 0x00353844 File Offset: 0x00351A44
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			CustomClothingOutfits.Instance.Internal_EditOutfit(outfitType, this.OutfitId, items);
		}

		// Token: 0x0600924B RID: 37451 RVA: 0x00353858 File Offset: 0x00351A58
		public string ReadName()
		{
			return this.OutfitId;
		}

		// Token: 0x0600924C RID: 37452 RVA: 0x00353860 File Offset: 0x00351A60
		public void WriteName(string name)
		{
			if (this.OutfitId == name)
			{
				return;
			}
			if (ClothingOutfitTarget.DoesTemplateExist(name))
			{
				throw new Exception(string.Concat(new string[]
				{
					"Can not change outfit name from \"",
					this.OutfitId,
					"\" to \"",
					name,
					"\", \"",
					name,
					"\" already exists"
				}));
			}
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId))
			{
				CustomClothingOutfits.Instance.Internal_RenameOutfit(this.m_outfitType, this.OutfitId, name);
			}
			else
			{
				CustomClothingOutfits.Instance.Internal_EditOutfit(this.m_outfitType, name, ClothingOutfitTarget.NO_ITEMS);
			}
			this.m_outfitId[0] = name;
		}

		// Token: 0x0600924D RID: 37453 RVA: 0x0035391A File Offset: 0x00351B1A
		public void Delete()
		{
			CustomClothingOutfits.Instance.Internal_RemoveOutfit(this.m_outfitType, this.OutfitId);
		}

		// Token: 0x04006FB2 RID: 28594
		private readonly string[] m_outfitId;

		// Token: 0x04006FB3 RID: 28595
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}

	// Token: 0x02001673 RID: 5747
	public readonly struct DatabaseAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600924E RID: 37454 RVA: 0x00353932 File Offset: 0x00351B32
		public bool CanWriteItems
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600924F RID: 37455 RVA: 0x00353935 File Offset: 0x00351B35
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06009250 RID: 37456 RVA: 0x00353938 File Offset: 0x00351B38
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009251 RID: 37457 RVA: 0x0035393B File Offset: 0x00351B3B
		public bool DoesExist()
		{
			return true;
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06009252 RID: 37458 RVA: 0x0035393E File Offset: 0x00351B3E
		public string OutfitId
		{
			get
			{
				return this.m_outfitId;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06009253 RID: 37459 RVA: 0x00353946 File Offset: 0x00351B46
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06009254 RID: 37460 RVA: 0x0035394E File Offset: 0x00351B4E
		public DatabaseAuthoredTemplate(ClothingOutfitResource outfit)
		{
			this.m_outfitId = outfit.Id;
			this.m_outfitType = outfit.outfitType;
			this.resource = outfit;
		}

		// Token: 0x06009255 RID: 37461 RVA: 0x0035396F File Offset: 0x00351B6F
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.resource.itemsInOutfit;
		}

		// Token: 0x06009256 RID: 37462 RVA: 0x0035397C File Offset: 0x00351B7C
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			throw new InvalidOperationException("Can not set items on a Db authored outfit");
		}

		// Token: 0x06009257 RID: 37463 RVA: 0x00353988 File Offset: 0x00351B88
		public string ReadName()
		{
			return this.resource.Name;
		}

		// Token: 0x06009258 RID: 37464 RVA: 0x00353995 File Offset: 0x00351B95
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not set name on a Db authored outfit");
		}

		// Token: 0x06009259 RID: 37465 RVA: 0x003539A1 File Offset: 0x00351BA1
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a Db authored outfit");
		}

		// Token: 0x04006FB4 RID: 28596
		public readonly ClothingOutfitResource resource;

		// Token: 0x04006FB5 RID: 28597
		private readonly string m_outfitId;

		// Token: 0x04006FB6 RID: 28598
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}
}

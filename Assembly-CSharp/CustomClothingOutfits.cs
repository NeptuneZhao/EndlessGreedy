using System;
using System.Collections.Generic;

// Token: 0x020007B3 RID: 1971
public class CustomClothingOutfits
{
	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06003616 RID: 13846 RVA: 0x0012676A File Offset: 0x0012496A
	public static CustomClothingOutfits Instance
	{
		get
		{
			if (CustomClothingOutfits._instance == null)
			{
				CustomClothingOutfits._instance = new CustomClothingOutfits();
			}
			return CustomClothingOutfits._instance;
		}
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x00126782 File Offset: 0x00124982
	public SerializableOutfitData.Version2 Internal_GetOutfitData()
	{
		return this.serializableOutfitData;
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x0012678A File Offset: 0x0012498A
	public void Internal_SetOutfitData(SerializableOutfitData.Version2 data)
	{
		this.serializableOutfitData = data;
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x00126794 File Offset: 0x00124994
	public void Internal_EditOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name, string[] outfit_items)
	{
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfit_name, out customTemplateOutfitEntry))
		{
			customTemplateOutfitEntry = new SerializableOutfitData.Version2.CustomTemplateOutfitEntry();
			customTemplateOutfitEntry.outfitType = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			customTemplateOutfitEntry.itemIds = outfit_items;
			this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[outfit_name] = customTemplateOutfitEntry;
		}
		else
		{
			ClothingOutfitUtility.OutfitType outfitType;
			if (!Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				throw new NotSupportedException(string.Concat(new string[]
				{
					"Cannot edit outfit \"",
					outfit_name,
					"\" of unknown outfit type \"",
					customTemplateOutfitEntry.outfitType,
					"\""
				}));
			}
			if (outfitType != outfit_type)
			{
				throw new NotSupportedException(string.Format("Cannot edit outfit \"{0}\" of outfit type \"{1}\" to be an outfit of type \"{2}\"", outfit_name, customTemplateOutfitEntry.outfitType, outfit_type));
			}
			customTemplateOutfitEntry.itemIds = outfit_items;
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x00126868 File Offset: 0x00124A68
	public void Internal_RenameOutfit(ClothingOutfitUtility.OutfitType outfit_type, string old_outfit_name, string new_outfit_name)
	{
		if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(old_outfit_name))
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Can't rename outfit \"",
				old_outfit_name,
				"\" to \"",
				new_outfit_name,
				"\": missing \"",
				old_outfit_name,
				"\" entry"
			}));
		}
		if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(new_outfit_name))
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Can't rename outfit \"",
				old_outfit_name,
				"\" to \"",
				new_outfit_name,
				"\": entry \"",
				new_outfit_name,
				"\" already exists"
			}));
		}
		this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Add(new_outfit_name, this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[old_outfit_name]);
		foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
		{
			string text;
			Dictionary<string, string> dictionary;
			keyValuePair.Deconstruct(out text, out dictionary);
			Dictionary<string, string> dictionary2 = dictionary;
			if (dictionary2 != null)
			{
				using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
				{
					foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
					{
						string a;
						keyValuePair2.Deconstruct(out text, out a);
						string item = text;
						if (a == old_outfit_name)
						{
							pooledList.Add(item);
						}
					}
					foreach (string key in pooledList)
					{
						dictionary2[key] = new_outfit_name;
					}
				}
			}
		}
		this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(old_outfit_name);
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x00126A68 File Offset: 0x00124C68
	public void Internal_RemoveOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name)
	{
		if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(outfit_name))
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
			{
				string text;
				Dictionary<string, string> dictionary;
				keyValuePair.Deconstruct(out text, out dictionary);
				Dictionary<string, string> dictionary2 = dictionary;
				if (dictionary2 != null)
				{
					using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
					{
						foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
						{
							string a;
							keyValuePair2.Deconstruct(out text, out a);
							string item = text;
							if (a == outfit_name)
							{
								pooledList.Add(item);
							}
						}
						foreach (string key in pooledList)
						{
							dictionary2.Remove(key);
						}
					}
				}
			}
			ClothingOutfitUtility.SaveClothingOutfitData();
		}
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x00126BAC File Offset: 0x00124DAC
	public bool Internal_TryGetDuplicantPersonalityOutfit(ClothingOutfitUtility.OutfitType outfit_type, string personalityId, out string outfitId)
	{
		if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
		{
			string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			if (this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId].ContainsKey(name))
			{
				outfitId = this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name];
				return true;
			}
		}
		outfitId = null;
		return false;
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x00126C1C File Offset: 0x00124E1C
	public void Internal_SetDuplicantPersonalityOutfit(ClothingOutfitUtility.OutfitType outfit_type, string personalityId, Option<string> outfit_id)
	{
		string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
		Dictionary<string, string> dictionary;
		if (outfit_id.HasValue)
		{
			if (!this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
			{
				this.serializableOutfitData.PersonalityIdToAssignedOutfits.Add(personalityId, new Dictionary<string, string>());
			}
			this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name] = outfit_id.Value;
		}
		else if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.TryGetValue(personalityId, out dictionary))
		{
			dictionary.Remove(name);
			if (dictionary.Count == 0)
			{
				this.serializableOutfitData.PersonalityIdToAssignedOutfits.Remove(personalityId);
			}
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x0400202B RID: 8235
	private static CustomClothingOutfits _instance;

	// Token: 0x0400202C RID: 8236
	private SerializableOutfitData.Version2 serializableOutfitData = new SerializableOutfitData.Version2();
}

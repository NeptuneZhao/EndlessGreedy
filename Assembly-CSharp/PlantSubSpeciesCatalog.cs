using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009F7 RID: 2551
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantSubSpeciesCatalog : KMonoBehaviour
{
	// Token: 0x060049D5 RID: 18901 RVA: 0x001A6413 File Offset: 0x001A4613
	public static void DestroyInstance()
	{
		PlantSubSpeciesCatalog.Instance = null;
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x060049D6 RID: 18902 RVA: 0x001A641C File Offset: 0x001A461C
	public bool AnyNonOriginalDiscovered
	{
		get
		{
			foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
			{
				if (keyValuePair.Value.Find((PlantSubSpeciesCatalog.SubSpeciesInfo ss) => !ss.IsOriginal).IsValid)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x060049D7 RID: 18903 RVA: 0x001A64A4 File Offset: 0x001A46A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PlantSubSpeciesCatalog.Instance = this;
	}

	// Token: 0x060049D8 RID: 18904 RVA: 0x001A64B2 File Offset: 0x001A46B2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.EnsureOriginalSubSpecies();
	}

	// Token: 0x060049D9 RID: 18905 RVA: 0x001A64C0 File Offset: 0x001A46C0
	public List<Tag> GetAllDiscoveredSpecies()
	{
		return this.discoveredSubspeciesBySpecies.Keys.ToList<Tag>();
	}

	// Token: 0x060049DA RID: 18906 RVA: 0x001A64D4 File Offset: 0x001A46D4
	public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllSubSpeciesForSpecies(Tag speciesID)
	{
		List<PlantSubSpeciesCatalog.SubSpeciesInfo> result;
		if (this.discoveredSubspeciesBySpecies.TryGetValue(speciesID, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060049DB RID: 18907 RVA: 0x001A64F4 File Offset: 0x001A46F4
	public bool GetOriginalSubSpecies(Tag speciesID, out PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo)
	{
		if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
		{
			subSpeciesInfo = default(PlantSubSpeciesCatalog.SubSpeciesInfo);
			return false;
		}
		subSpeciesInfo = this.discoveredSubspeciesBySpecies[speciesID].Find((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.IsOriginal);
		return true;
	}

	// Token: 0x060049DC RID: 18908 RVA: 0x001A6550 File Offset: 0x001A4750
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpecies(Tag speciesID, Tag subSpeciesID)
	{
		return this.discoveredSubspeciesBySpecies[speciesID].Find((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.ID == subSpeciesID);
	}

	// Token: 0x060049DD RID: 18909 RVA: 0x001A6588 File Offset: 0x001A4788
	public PlantSubSpeciesCatalog.SubSpeciesInfo FindSubSpecies(Tag subSpeciesID)
	{
		Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo> <>9__0;
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
		{
			List<PlantSubSpeciesCatalog.SubSpeciesInfo> value = keyValuePair.Value;
			Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo> match;
			if ((match = <>9__0) == null)
			{
				match = (<>9__0 = ((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.ID == subSpeciesID));
			}
			PlantSubSpeciesCatalog.SubSpeciesInfo result = value.Find(match);
			if (result.ID.IsValid)
			{
				return result;
			}
		}
		return default(PlantSubSpeciesCatalog.SubSpeciesInfo);
	}

	// Token: 0x060049DE RID: 18910 RVA: 0x001A6630 File Offset: 0x001A4830
	public void DiscoverSubSpecies(PlantSubSpeciesCatalog.SubSpeciesInfo newSubSpeciesInfo, MutantPlant source)
	{
		if (!this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Contains(newSubSpeciesInfo))
		{
			this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Add(newSubSpeciesInfo);
			Notification notification = new Notification(MISC.NOTIFICATIONS.NEWMUTANTSEED.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(this.NewSubspeciesTooltipCB), newSubSpeciesInfo, true, 0f, null, null, source.transform, true, false, false);
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x060049DF RID: 18911 RVA: 0x001A66B8 File Offset: 0x001A48B8
	private string NewSubspeciesTooltipCB(List<Notification> notifications, object data)
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = (PlantSubSpeciesCatalog.SubSpeciesInfo)data;
		return MISC.NOTIFICATIONS.NEWMUTANTSEED.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x060049E0 RID: 18912 RVA: 0x001A66E8 File Offset: 0x001A48E8
	public void IdentifySubSpecies(Tag subSpeciesID)
	{
		if (this.identifiedSubSpecies.Add(subSpeciesID))
		{
			this.FindSubSpecies(subSpeciesID);
			foreach (object obj in Components.MutantPlants)
			{
				MutantPlant mutantPlant = (MutantPlant)obj;
				if (mutantPlant.HasTag(subSpeciesID))
				{
					mutantPlant.UpdateNameAndTags();
				}
			}
			GeneticAnalysisCompleteMessage message = new GeneticAnalysisCompleteMessage(subSpeciesID);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x060049E1 RID: 18913 RVA: 0x001A6770 File Offset: 0x001A4970
	public bool IsSubSpeciesIdentified(Tag subSpeciesID)
	{
		return this.identifiedSubSpecies.Contains(subSpeciesID);
	}

	// Token: 0x060049E2 RID: 18914 RVA: 0x001A677E File Offset: 0x001A497E
	public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllUnidentifiedSubSpecies(Tag speciesID)
	{
		return this.discoveredSubspeciesBySpecies[speciesID].FindAll((PlantSubSpeciesCatalog.SubSpeciesInfo ss) => !this.IsSubSpeciesIdentified(ss.ID));
	}

	// Token: 0x060049E3 RID: 18915 RVA: 0x001A67A0 File Offset: 0x001A49A0
	public bool IsValidPlantableSeed(Tag seedID, Tag subspeciesID)
	{
		if (!seedID.IsValid)
		{
			return false;
		}
		MutantPlant component = Assets.GetPrefab(seedID).GetComponent<MutantPlant>();
		if (component == null)
		{
			return !subspeciesID.IsValid;
		}
		List<PlantSubSpeciesCatalog.SubSpeciesInfo> allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(component.SpeciesID);
		return allSubSpeciesForSpecies != null && allSubSpeciesForSpecies.FindIndex((PlantSubSpeciesCatalog.SubSpeciesInfo s) => s.ID == subspeciesID) != -1 && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subspeciesID);
	}

	// Token: 0x060049E4 RID: 18916 RVA: 0x001A6824 File Offset: 0x001A4A24
	private void EnsureOriginalSubSpecies()
	{
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<MutantPlant>())
		{
			MutantPlant component = gameObject.GetComponent<MutantPlant>();
			Tag speciesID = component.SpeciesID;
			if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
			{
				this.discoveredSubspeciesBySpecies[speciesID] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>();
				this.discoveredSubspeciesBySpecies[speciesID].Add(component.GetSubSpeciesInfo());
			}
			this.identifiedSubSpecies.Add(component.SubSpeciesID);
		}
	}

	// Token: 0x04003071 RID: 12401
	public static PlantSubSpeciesCatalog Instance;

	// Token: 0x04003072 RID: 12402
	[Serialize]
	private Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> discoveredSubspeciesBySpecies = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();

	// Token: 0x04003073 RID: 12403
	[Serialize]
	private HashSet<Tag> identifiedSubSpecies = new HashSet<Tag>();

	// Token: 0x02001A09 RID: 6665
	[Serializable]
	public struct SubSpeciesInfo : IEquatable<PlantSubSpeciesCatalog.SubSpeciesInfo>
	{
		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06009EE5 RID: 40677 RVA: 0x0037A967 File Offset: 0x00378B67
		public bool IsValid
		{
			get
			{
				return this.ID.IsValid;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06009EE6 RID: 40678 RVA: 0x0037A974 File Offset: 0x00378B74
		public bool IsOriginal
		{
			get
			{
				return this.mutationIDs == null || this.mutationIDs.Count == 0;
			}
		}

		// Token: 0x06009EE7 RID: 40679 RVA: 0x0037A98E File Offset: 0x00378B8E
		public SubSpeciesInfo(Tag speciesID, List<string> mutationIDs)
		{
			this.speciesID = speciesID;
			this.mutationIDs = ((mutationIDs != null) ? new List<string>(mutationIDs) : new List<string>());
			this.ID = PlantSubSpeciesCatalog.SubSpeciesInfo.SubSpeciesIDFromMutations(speciesID, mutationIDs);
		}

		// Token: 0x06009EE8 RID: 40680 RVA: 0x0037A9BC File Offset: 0x00378BBC
		public static Tag SubSpeciesIDFromMutations(Tag speciesID, List<string> mutationIDs)
		{
			if (mutationIDs == null || mutationIDs.Count == 0)
			{
				Tag tag = speciesID;
				return tag.ToString() + "_Original";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(speciesID);
			foreach (string value in mutationIDs)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString().ToTag();
		}

		// Token: 0x06009EE9 RID: 40681 RVA: 0x0037AA60 File Offset: 0x00378C60
		public string GetMutationsNames()
		{
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				return CREATURES.PLANT_MUTATIONS.NONE.NAME;
			}
			return string.Join(", ", Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs));
		}

		// Token: 0x06009EEA RID: 40682 RVA: 0x0037AAAC File Offset: 0x00378CAC
		public string GetNameWithMutations(string properName, bool identified, bool cleanOriginal)
		{
			string result;
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				if (cleanOriginal)
				{
					result = properName;
				}
				else
				{
					result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", CREATURES.PLANT_MUTATIONS.NONE.NAME);
				}
			}
			else if (!identified)
			{
				result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", CREATURES.PLANT_MUTATIONS.UNIDENTIFIED);
			}
			else
			{
				result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", string.Join(", ", Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs)));
			}
			return result;
		}

		// Token: 0x06009EEB RID: 40683 RVA: 0x0037AB64 File Offset: 0x00378D64
		public static bool operator ==(PlantSubSpeciesCatalog.SubSpeciesInfo obj1, PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
		{
			return obj1.Equals(obj2);
		}

		// Token: 0x06009EEC RID: 40684 RVA: 0x0037AB6E File Offset: 0x00378D6E
		public static bool operator !=(PlantSubSpeciesCatalog.SubSpeciesInfo obj1, PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
		{
			return !(obj1 == obj2);
		}

		// Token: 0x06009EED RID: 40685 RVA: 0x0037AB7A File Offset: 0x00378D7A
		public override bool Equals(object other)
		{
			return other is PlantSubSpeciesCatalog.SubSpeciesInfo && this == (PlantSubSpeciesCatalog.SubSpeciesInfo)other;
		}

		// Token: 0x06009EEE RID: 40686 RVA: 0x0037AB97 File Offset: 0x00378D97
		public bool Equals(PlantSubSpeciesCatalog.SubSpeciesInfo other)
		{
			return this.ID == other.ID;
		}

		// Token: 0x06009EEF RID: 40687 RVA: 0x0037ABAA File Offset: 0x00378DAA
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		// Token: 0x06009EF0 RID: 40688 RVA: 0x0037ABC0 File Offset: 0x00378DC0
		public string GetMutationsTooltip()
		{
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				return CREATURES.STATUSITEMS.ORIGINALPLANTMUTATION.TOOLTIP;
			}
			if (!PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.ID))
			{
				return CREATURES.STATUSITEMS.UNKNOWNMUTATION.TOOLTIP;
			}
			string id = this.mutationIDs[0];
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			return CREATURES.STATUSITEMS.SPECIFICPLANTMUTATION.TOOLTIP.Replace("{MutationName}", plantMutation.Name) + "\n" + plantMutation.GetTooltip();
		}

		// Token: 0x04007B1E RID: 31518
		public Tag speciesID;

		// Token: 0x04007B1F RID: 31519
		public Tag ID;

		// Token: 0x04007B20 RID: 31520
		public List<string> mutationIDs;

		// Token: 0x04007B21 RID: 31521
		private const string ORIGINAL_SUFFIX = "_Original";
	}
}

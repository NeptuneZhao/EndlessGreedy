using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000AE6 RID: 2790
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{id}: {type} at distance {distance}")]
public class SpaceDestination
{
	// Token: 0x060052E5 RID: 21221 RVA: 0x001DB81C File Offset: 0x001D9A1C
	private static global::Tuple<SimHashes, MathUtil.MinMax> GetRareElement(SimHashes id)
	{
		foreach (global::Tuple<SimHashes, MathUtil.MinMax> tuple in SpaceDestination.RARE_ELEMENTS)
		{
			if (tuple.first == id)
			{
				return tuple;
			}
		}
		return null;
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x060052E6 RID: 21222 RVA: 0x001DB878 File Offset: 0x001D9A78
	public int OneBasedDistance
	{
		get
		{
			return this.distance + 1;
		}
	}

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x060052E7 RID: 21223 RVA: 0x001DB882 File Offset: 0x001D9A82
	public float CurrentMass
	{
		get
		{
			return (float)this.GetDestinationType().minimumMass + this.availableMass;
		}
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x060052E8 RID: 21224 RVA: 0x001DB897 File Offset: 0x001D9A97
	public float AvailableMass
	{
		get
		{
			return this.availableMass;
		}
	}

	// Token: 0x060052E9 RID: 21225 RVA: 0x001DB8A0 File Offset: 0x001D9AA0
	public SpaceDestination(int id, string type, int distance)
	{
		this.id = id;
		this.type = type;
		this.distance = distance;
		SpaceDestinationType destinationType = this.GetDestinationType();
		this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		this.GenerateSurfaceElements();
		this.GenerateResearchOpportunities();
	}

	// Token: 0x060052EA RID: 21226 RVA: 0x001DB91C File Offset: 0x001D9B1C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 9))
		{
			SpaceDestinationType destinationType = this.GetDestinationType();
			this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		}
	}

	// Token: 0x060052EB RID: 21227 RVA: 0x001DB95B File Offset: 0x001D9B5B
	public SpaceDestinationType GetDestinationType()
	{
		return Db.Get().SpaceDestinationTypes.Get(this.type);
	}

	// Token: 0x060052EC RID: 21228 RVA: 0x001DB974 File Offset: 0x001D9B74
	public SpaceDestination.ResearchOpportunity TryCompleteResearchOpportunity()
	{
		foreach (SpaceDestination.ResearchOpportunity researchOpportunity in this.researchOpportunities)
		{
			if (researchOpportunity.TryComplete(this))
			{
				return researchOpportunity;
			}
		}
		return null;
	}

	// Token: 0x060052ED RID: 21229 RVA: 0x001DB9D0 File Offset: 0x001D9BD0
	public void GenerateSurfaceElements()
	{
		foreach (KeyValuePair<SimHashes, MathUtil.MinMax> keyValuePair in this.GetDestinationType().elementTable)
		{
			this.recoverableElements.Add(keyValuePair.Key, UnityEngine.Random.value);
		}
	}

	// Token: 0x060052EE RID: 21230 RVA: 0x001DBA38 File Offset: 0x001D9C38
	public SpacecraftManager.DestinationAnalysisState AnalysisState()
	{
		return SpacecraftManager.instance.GetDestinationAnalysisState(this);
	}

	// Token: 0x060052EF RID: 21231 RVA: 0x001DBA48 File Offset: 0x001D9C48
	public void GenerateResearchOpportunities()
	{
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.UPPERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.LOWERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.MAGNETICFIELD, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SUBSURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		float num = 0f;
		foreach (global::Tuple<float, int> tuple in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num += tuple.first;
		}
		float num2 = UnityEngine.Random.value * num;
		int num3 = 0;
		foreach (global::Tuple<float, int> tuple2 in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num2 -= tuple2.first;
			if (num2 <= 0f)
			{
				num3 = tuple2.second;
			}
		}
		for (int i = 0; i < num3; i++)
		{
			this.researchOpportunities[UnityEngine.Random.Range(0, this.researchOpportunities.Count)].discoveredRareResource = SpaceDestination.RARE_ELEMENTS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ELEMENTS.Count)].first;
		}
		if (UnityEngine.Random.value < 0.33f)
		{
			int index = UnityEngine.Random.Range(0, this.researchOpportunities.Count);
			this.researchOpportunities[index].discoveredRareItem = SpaceDestination.RARE_ITEMS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ITEMS.Count)].first;
		}
	}

	// Token: 0x060052F0 RID: 21232 RVA: 0x001DBC40 File Offset: 0x001D9E40
	public float GetResourceValue(SimHashes resource, float roll)
	{
		if (this.GetDestinationType().elementTable.ContainsKey(resource))
		{
			return this.GetDestinationType().elementTable[resource].Lerp(roll);
		}
		if (SpaceDestinationTypes.extendedElementTable.ContainsKey(resource))
		{
			return SpaceDestinationTypes.extendedElementTable[resource].Lerp(roll);
		}
		return 0f;
	}

	// Token: 0x060052F1 RID: 21233 RVA: 0x001DBCA4 File Offset: 0x001D9EA4
	public Dictionary<SimHashes, float> GetMissionResourceResult(float totalCargoSpace, float reservedMass, bool solids = true, bool liquids = true, bool gasses = true)
	{
		Dictionary<SimHashes, float> dictionary = new Dictionary<SimHashes, float>();
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
			}
		}
		float num2 = Mathf.Min(this.CurrentMass + reservedMass - (float)this.GetDestinationType().minimumMass, totalCargoSpace);
		foreach (KeyValuePair<SimHashes, float> keyValuePair2 in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair2.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsGas && gasses))
			{
				float value = num2 * (this.GetResourceValue(keyValuePair2.Key, keyValuePair2.Value) / num);
				dictionary.Add(keyValuePair2.Key, value);
			}
		}
		return dictionary;
	}

	// Token: 0x060052F2 RID: 21234 RVA: 0x001DBE18 File Offset: 0x001DA018
	public Dictionary<Tag, int> GetRecoverableEntities()
	{
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		Dictionary<string, int> recoverableEntities = this.GetDestinationType().recoverableEntities;
		if (recoverableEntities != null)
		{
			foreach (KeyValuePair<string, int> keyValuePair in recoverableEntities)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return dictionary;
	}

	// Token: 0x060052F3 RID: 21235 RVA: 0x001DBE90 File Offset: 0x001DA090
	public Dictionary<Tag, int> GetMissionEntityResult()
	{
		return this.GetRecoverableEntities();
	}

	// Token: 0x060052F4 RID: 21236 RVA: 0x001DBE98 File Offset: 0x001DA098
	public float ReserveResources(CargoBay bay)
	{
		float num = 0f;
		if (bay != null)
		{
			Storage component = bay.GetComponent<Storage>();
			foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
			{
				if (this.HasElementType(bay.storageType))
				{
					num += component.capacityKg;
					this.availableMass = Mathf.Max(0f, this.availableMass - component.capacityKg);
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x060052F5 RID: 21237 RVA: 0x001DBF34 File Offset: 0x001DA134
	public bool HasElementType(CargoBay.CargoType type)
	{
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && type == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && type == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && type == CargoBay.CargoType.Gasses))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060052F6 RID: 21238 RVA: 0x001DBFCC File Offset: 0x001DA1CC
	public void Replenish(float dt)
	{
		SpaceDestinationType destinationType = this.GetDestinationType();
		if (this.CurrentMass < (float)destinationType.maxiumMass)
		{
			this.availableMass += destinationType.replishmentPerSim1000ms;
		}
	}

	// Token: 0x060052F7 RID: 21239 RVA: 0x001DC004 File Offset: 0x001DA204
	public float GetAvailableResourcesPercentage(CargoBay.CargoType cargoType)
	{
		float num = 0f;
		float totalMass = this.GetTotalMass();
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && cargoType == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && cargoType == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && cargoType == CargoBay.CargoType.Gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value) / totalMass;
			}
		}
		return num;
	}

	// Token: 0x060052F8 RID: 21240 RVA: 0x001DC0BC File Offset: 0x001DA2BC
	public float GetTotalMass()
	{
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
		}
		return num;
	}

	// Token: 0x040036BF RID: 14015
	private const int MASS_TO_RECOVER_AMOUNT = 1000;

	// Token: 0x040036C0 RID: 14016
	private static List<global::Tuple<float, int>> RARE_ELEMENT_CHANCES = new List<global::Tuple<float, int>>
	{
		new global::Tuple<float, int>(1f, 0),
		new global::Tuple<float, int>(0.33f, 1),
		new global::Tuple<float, int>(0.03f, 2)
	};

	// Token: 0x040036C1 RID: 14017
	private static readonly List<global::Tuple<SimHashes, MathUtil.MinMax>> RARE_ELEMENTS = new List<global::Tuple<SimHashes, MathUtil.MinMax>>
	{
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Katairite, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Niobium, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Fullerene, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Isoresin, new MathUtil.MinMax(1f, 10f))
	};

	// Token: 0x040036C2 RID: 14018
	private const float RARE_ITEM_CHANCE = 0.33f;

	// Token: 0x040036C3 RID: 14019
	private static readonly List<global::Tuple<string, MathUtil.MinMax>> RARE_ITEMS = new List<global::Tuple<string, MathUtil.MinMax>>
	{
		new global::Tuple<string, MathUtil.MinMax>("GeneShufflerRecharge", new MathUtil.MinMax(1f, 2f))
	};

	// Token: 0x040036C4 RID: 14020
	[Serialize]
	public int id;

	// Token: 0x040036C5 RID: 14021
	[Serialize]
	public string type;

	// Token: 0x040036C6 RID: 14022
	public bool startAnalyzed;

	// Token: 0x040036C7 RID: 14023
	[Serialize]
	public int distance;

	// Token: 0x040036C8 RID: 14024
	[Serialize]
	public float activePeriod = 20f;

	// Token: 0x040036C9 RID: 14025
	[Serialize]
	public float inactivePeriod = 10f;

	// Token: 0x040036CA RID: 14026
	[Serialize]
	public float startingOrbitPercentage;

	// Token: 0x040036CB RID: 14027
	[Serialize]
	public Dictionary<SimHashes, float> recoverableElements = new Dictionary<SimHashes, float>();

	// Token: 0x040036CC RID: 14028
	[Serialize]
	public List<SpaceDestination.ResearchOpportunity> researchOpportunities = new List<SpaceDestination.ResearchOpportunity>();

	// Token: 0x040036CD RID: 14029
	[Serialize]
	private float availableMass;

	// Token: 0x02001B39 RID: 6969
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ResearchOpportunity
	{
		// Token: 0x0600A2EC RID: 41708 RVA: 0x00388B37 File Offset: 0x00386D37
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.discoveredRareResource == (SimHashes)0)
			{
				this.discoveredRareResource = SimHashes.Void;
			}
			if (this.dataValue > 50)
			{
				this.dataValue = 50;
			}
		}

		// Token: 0x0600A2ED RID: 41709 RVA: 0x00388B5E File Offset: 0x00386D5E
		public ResearchOpportunity(string description, int pointValue)
		{
			this.description = description;
			this.dataValue = pointValue;
		}

		// Token: 0x0600A2EE RID: 41710 RVA: 0x00388B80 File Offset: 0x00386D80
		public bool TryComplete(SpaceDestination destination)
		{
			if (!this.completed)
			{
				this.completed = true;
				if (this.discoveredRareResource != SimHashes.Void && !destination.recoverableElements.ContainsKey(this.discoveredRareResource))
				{
					destination.recoverableElements.Add(this.discoveredRareResource, UnityEngine.Random.value);
				}
				return true;
			}
			return false;
		}

		// Token: 0x04007F1E RID: 32542
		[Serialize]
		public string description;

		// Token: 0x04007F1F RID: 32543
		[Serialize]
		public int dataValue;

		// Token: 0x04007F20 RID: 32544
		[Serialize]
		public bool completed;

		// Token: 0x04007F21 RID: 32545
		[Serialize]
		public SimHashes discoveredRareResource = SimHashes.Void;

		// Token: 0x04007F22 RID: 32546
		[Serialize]
		public string discoveredRareItem;
	}
}

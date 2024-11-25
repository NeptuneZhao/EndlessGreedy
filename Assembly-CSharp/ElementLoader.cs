using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Klei;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02000941 RID: 2369
public class ElementLoader
{
	// Token: 0x060044DF RID: 17631 RVA: 0x00188210 File Offset: 0x00186410
	public static float GetMinMeltingPointAmongElements(IList<Tag> elements)
	{
		float num = float.MaxValue;
		for (int i = 0; i < elements.Count; i++)
		{
			Element element = ElementLoader.GetElement(elements[i]);
			if (element != null)
			{
				num = Mathf.Min(num, element.highTemp);
			}
		}
		return num;
	}

	// Token: 0x060044E0 RID: 17632 RVA: 0x00188254 File Offset: 0x00186454
	public static List<ElementLoader.ElementEntry> CollectElementsFromYAML()
	{
		List<ElementLoader.ElementEntry> list = new List<ElementLoader.ElementEntry>();
		ListPool<FileHandle, ElementLoader>.PooledList pooledList = ListPool<FileHandle, ElementLoader>.Allocate();
		FileSystem.GetFiles(FileSystem.Normalize(ElementLoader.path), "*.yaml", pooledList);
		ListPool<YamlIO.Error, ElementLoader>.PooledList errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();
		YamlIO.ErrorHandler <>9__0;
		foreach (FileHandle fileHandle in pooledList)
		{
			if (!Path.GetFileName(fileHandle.full_path).StartsWith("."))
			{
				string full_path = fileHandle.full_path;
				YamlIO.ErrorHandler handle_error;
				if ((handle_error = <>9__0) == null)
				{
					handle_error = (<>9__0 = delegate(YamlIO.Error error, bool force_log_as_warning)
					{
						errors.Add(error);
					});
				}
				ElementLoader.ElementEntryCollection elementEntryCollection = YamlIO.LoadFile<ElementLoader.ElementEntryCollection>(full_path, handle_error, null);
				if (elementEntryCollection != null)
				{
					list.AddRange(elementEntryCollection.elements);
				}
			}
		}
		pooledList.Recycle();
		if (Global.Instance != null && Global.Instance.modManager != null)
		{
			Global.Instance.modManager.HandleErrors(errors);
		}
		errors.Recycle();
		return list;
	}

	// Token: 0x060044E1 RID: 17633 RVA: 0x00188368 File Offset: 0x00186568
	public static void Load(ref Hashtable substanceList, Dictionary<string, SubstanceTable> substanceTablesByDlc)
	{
		ElementLoader.elements = new List<Element>();
		ElementLoader.elementTable = new Dictionary<int, Element>();
		ElementLoader.elementTagTable = new Dictionary<Tag, Element>();
		foreach (ElementLoader.ElementEntry elementEntry in ElementLoader.CollectElementsFromYAML())
		{
			int num = Hash.SDBMLower(elementEntry.elementId);
			if (!ElementLoader.elementTable.ContainsKey(num) && substanceTablesByDlc.ContainsKey(elementEntry.dlcId))
			{
				Element element = new Element();
				element.id = (SimHashes)num;
				element.name = Strings.Get(elementEntry.localizationID);
				element.nameUpperCase = element.name.ToUpper();
				element.description = Strings.Get(elementEntry.description);
				element.tag = TagManager.Create(elementEntry.elementId, element.name);
				ElementLoader.CopyEntryToElement(elementEntry, element);
				ElementLoader.elements.Add(element);
				ElementLoader.elementTable[num] = element;
				ElementLoader.elementTagTable[element.tag] = element;
				if (!ElementLoader.ManifestSubstanceForElement(element, ref substanceList, substanceTablesByDlc[elementEntry.dlcId]))
				{
					global::Debug.LogWarning("Missing substance for element: " + element.id.ToString());
				}
			}
		}
		ElementLoader.FinaliseElementsTable(ref substanceList);
		WorldGen.SetupDefaultElements();
	}

	// Token: 0x060044E2 RID: 17634 RVA: 0x001884E0 File Offset: 0x001866E0
	private static void CopyEntryToElement(ElementLoader.ElementEntry entry, Element elem)
	{
		Hash.SDBMLower(entry.elementId);
		elem.tag = TagManager.Create(entry.elementId.ToString());
		elem.specificHeatCapacity = entry.specificHeatCapacity;
		elem.thermalConductivity = entry.thermalConductivity;
		elem.molarMass = entry.molarMass;
		elem.strength = entry.strength;
		elem.disabled = entry.isDisabled;
		elem.dlcId = entry.dlcId;
		elem.flow = entry.flow;
		elem.maxMass = entry.maxMass;
		elem.maxCompression = entry.liquidCompression;
		elem.viscosity = entry.speed;
		elem.minHorizontalFlow = entry.minHorizontalFlow;
		elem.minVerticalFlow = entry.minVerticalFlow;
		elem.solidSurfaceAreaMultiplier = entry.solidSurfaceAreaMultiplier;
		elem.liquidSurfaceAreaMultiplier = entry.liquidSurfaceAreaMultiplier;
		elem.gasSurfaceAreaMultiplier = entry.gasSurfaceAreaMultiplier;
		elem.state = entry.state;
		elem.hardness = entry.hardness;
		elem.lowTemp = entry.lowTemp;
		elem.lowTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionTarget);
		elem.highTemp = entry.highTemp;
		elem.highTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.highTempTransitionTarget);
		elem.highTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.highTempTransitionOreId);
		elem.highTempTransitionOreMassConversion = entry.highTempTransitionOreMassConversion;
		elem.lowTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionOreId);
		elem.lowTempTransitionOreMassConversion = entry.lowTempTransitionOreMassConversion;
		elem.sublimateId = (SimHashes)Hash.SDBMLower(entry.sublimateId);
		elem.convertId = (SimHashes)Hash.SDBMLower(entry.convertId);
		elem.sublimateFX = (SpawnFXHashes)Hash.SDBMLower(entry.sublimateFx);
		elem.sublimateRate = entry.sublimateRate;
		elem.sublimateEfficiency = entry.sublimateEfficiency;
		elem.sublimateProbability = entry.sublimateProbability;
		elem.offGasPercentage = entry.offGasPercentage;
		elem.lightAbsorptionFactor = entry.lightAbsorptionFactor;
		elem.radiationAbsorptionFactor = entry.radiationAbsorptionFactor;
		elem.radiationPer1000Mass = entry.radiationPer1000Mass;
		elem.toxicity = entry.toxicity;
		elem.elementComposition = entry.composition;
		Tag phaseTag = TagManager.Create(entry.state.ToString());
		elem.materialCategory = ElementLoader.CreateMaterialCategoryTag(elem.id, phaseTag, entry.materialCategory);
		elem.oreTags = ElementLoader.CreateOreTags(elem.materialCategory, phaseTag, entry.tags);
		elem.buildMenuSort = entry.buildMenuSort;
		Sim.PhysicsData defaultValues = default(Sim.PhysicsData);
		defaultValues.temperature = entry.defaultTemperature;
		defaultValues.mass = entry.defaultMass;
		defaultValues.pressure = entry.defaultPressure;
		switch (entry.state)
		{
		case Element.State.Gas:
			GameTags.GasElements.Add(elem.tag);
			defaultValues.mass = 1f;
			elem.maxMass = 1.8f;
			break;
		case Element.State.Liquid:
			GameTags.LiquidElements.Add(elem.tag);
			break;
		case Element.State.Solid:
			GameTags.SolidElements.Add(elem.tag);
			break;
		}
		elem.defaultValues = defaultValues;
	}

	// Token: 0x060044E3 RID: 17635 RVA: 0x001887E4 File Offset: 0x001869E4
	private static bool ManifestSubstanceForElement(Element elem, ref Hashtable substanceList, SubstanceTable substanceTable)
	{
		elem.substance = null;
		if (substanceList.ContainsKey(elem.id))
		{
			elem.substance = (substanceList[elem.id] as Substance);
			return false;
		}
		if (substanceTable != null)
		{
			elem.substance = substanceTable.GetSubstance(elem.id);
		}
		if (elem.substance == null)
		{
			elem.substance = new Substance();
			substanceTable.GetList().Add(elem.substance);
		}
		elem.substance.elementID = elem.id;
		elem.substance.renderedByWorld = elem.IsSolid;
		elem.substance.idx = substanceList.Count;
		if (elem.substance.uiColour == ElementLoader.noColour)
		{
			int count = ElementLoader.elements.Count;
			int idx = elem.substance.idx;
			elem.substance.uiColour = Color.HSVToRGB((float)idx / (float)count, 1f, 1f);
		}
		string name = UI.StripLinkFormatting(elem.name);
		elem.substance.name = name;
		elem.substance.nameTag = elem.tag;
		elem.substance.audioConfig = ElementsAudio.Instance.GetConfigForElement(elem.id);
		substanceList.Add(elem.id, elem.substance);
		return true;
	}

	// Token: 0x060044E4 RID: 17636 RVA: 0x00188952 File Offset: 0x00186B52
	public static Element FindElementByName(string name)
	{
		return ElementLoader.FindElementByHash((SimHashes)Hash.SDBMLower(name));
	}

	// Token: 0x060044E5 RID: 17637 RVA: 0x0018895F File Offset: 0x00186B5F
	public static Element FindElementByTag(Tag tag)
	{
		return ElementLoader.GetElement(tag);
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x00188968 File Offset: 0x00186B68
	public static Element FindElementByHash(SimHashes hash)
	{
		Element result = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out result);
		return result;
	}

	// Token: 0x060044E7 RID: 17639 RVA: 0x00188988 File Offset: 0x00186B88
	public static ushort GetElementIndex(SimHashes hash)
	{
		Element element = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out element);
		if (element != null)
		{
			return element.idx;
		}
		return ushort.MaxValue;
	}

	// Token: 0x060044E8 RID: 17640 RVA: 0x001889B4 File Offset: 0x00186BB4
	public static Element GetElement(Tag tag)
	{
		Element result;
		ElementLoader.elementTagTable.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x060044E9 RID: 17641 RVA: 0x001889D0 File Offset: 0x00186BD0
	public static SimHashes GetElementID(Tag tag)
	{
		Element element;
		ElementLoader.elementTagTable.TryGetValue(tag, out element);
		if (element != null)
		{
			return element.id;
		}
		return SimHashes.Vacuum;
	}

	// Token: 0x060044EA RID: 17642 RVA: 0x001889FC File Offset: 0x00186BFC
	private static SimHashes GetID(int column, int row, string[,] grid, SimHashes defaultValue = SimHashes.Vacuum)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find element at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return defaultValue;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return defaultValue;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SimHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find element {0}: {1}", text, ex.ToString()));
			return defaultValue;
		}
		return (SimHashes)obj;
	}

	// Token: 0x060044EB RID: 17643 RVA: 0x00188AC8 File Offset: 0x00186CC8
	private static SpawnFXHashes GetSpawnFX(int column, int row, string[,] grid)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find SpawnFXHashes at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return SpawnFXHashes.None;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return SpawnFXHashes.None;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SpawnFXHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find FX {0}: {1}", text, ex.ToString()));
			return SpawnFXHashes.None;
		}
		return (SpawnFXHashes)obj;
	}

	// Token: 0x060044EC RID: 17644 RVA: 0x00188B94 File Offset: 0x00186D94
	private static Tag CreateMaterialCategoryTag(SimHashes element_id, Tag phaseTag, string materialCategoryField)
	{
		if (!string.IsNullOrEmpty(materialCategoryField))
		{
			Tag tag = TagManager.Create(materialCategoryField);
			if (!GameTags.MaterialCategories.Contains(tag) && !GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				global::Debug.LogWarningFormat("Element {0} has category {1}, but that isn't in GameTags.MaterialCategores!", new object[]
				{
					element_id,
					materialCategoryField
				});
			}
			return tag;
		}
		return phaseTag;
	}

	// Token: 0x060044ED RID: 17645 RVA: 0x00188BEC File Offset: 0x00186DEC
	private static Tag[] CreateOreTags(Tag materialCategory, Tag phaseTag, string[] ore_tags_split)
	{
		List<Tag> list = new List<Tag>();
		if (ore_tags_split != null)
		{
			foreach (string text in ore_tags_split)
			{
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(TagManager.Create(text));
				}
			}
		}
		list.Add(phaseTag);
		if (materialCategory.IsValid && !list.Contains(materialCategory))
		{
			list.Add(materialCategory);
		}
		return list.ToArray();
	}

	// Token: 0x060044EE RID: 17646 RVA: 0x00188C50 File Offset: 0x00186E50
	private static void FinaliseElementsTable(ref Hashtable substanceList)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element != null)
			{
				if (element.substance == null)
				{
					global::Debug.LogWarning("Skipping finalise for missing element: " + element.id.ToString());
				}
				else
				{
					global::Debug.Assert(element.substance.nameTag.IsValid);
					if (element.thermalConductivity == 0f)
					{
						element.state |= Element.State.TemperatureInsulated;
					}
					if (element.strength == 0f)
					{
						element.state |= Element.State.Unbreakable;
					}
					if (element.IsSolid)
					{
						Element element2 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element2 != null)
						{
							element.highTempTransition = element2;
						}
					}
					else if (element.IsLiquid)
					{
						Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element3 != null)
						{
							element.highTempTransition = element3;
						}
						Element element4 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element4 != null)
						{
							element.lowTempTransition = element4;
						}
					}
					else if (element.IsGas)
					{
						Element element5 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element5 != null)
						{
							element.lowTempTransition = element5;
						}
					}
				}
			}
		}
		ElementLoader.elements = (from e in ElementLoader.elements
		orderby (int)(e.state & Element.State.Solid) descending, e.id
		select e).ToList<Element>();
		for (int i = 0; i < ElementLoader.elements.Count; i++)
		{
			if (ElementLoader.elements[i].substance != null)
			{
				ElementLoader.elements[i].substance.idx = i;
			}
			ElementLoader.elements[i].idx = (ushort)i;
		}
	}

	// Token: 0x060044EF RID: 17647 RVA: 0x00188E58 File Offset: 0x00187058
	private static void ValidateElements()
	{
		global::Debug.Log("------ Start Validating Elements ------");
		foreach (Element element in ElementLoader.elements)
		{
			string text = string.Format("{0} ({1})", element.tag.ProperNameStripLink(), element.state);
			if (element.IsLiquid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.sublimateRate == 0f, text + ": Liquids don't use sublimateRate, use offGasPercentage instead.");
				global::Debug.Assert(element.offGasPercentage > 0f, text + ": Missing offGasPercentage");
			}
			if (element.IsSolid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.offGasPercentage == 0f, text + ": Solids don't use offGasPercentage, use sublimateRate instead.");
				global::Debug.Assert(element.sublimateRate > 0f, text + ": Missing sublimationRate");
				global::Debug.Assert(element.sublimateRate * element.sublimateEfficiency > 0.001f, text + ": Sublimation rate and efficiency will result in gas that will be obliterated because its less than 1g. Increase these values and use sublimateProbability if you want a low amount of sublimation");
			}
			if (element.highTempTransition != null && element.highTempTransition.lowTempTransition == element)
			{
				global::Debug.Assert(element.highTemp >= element.highTempTransition.lowTemp, text + ": highTemp is higher than transition element's (" + element.highTempTransition.tag.ProperNameStripLink() + ") lowTemp");
			}
			global::Debug.Assert(element.defaultValues.mass <= element.maxMass, text + ": Default mass should be less than max mass");
			if (false)
			{
				if (element.IsSolid && element.highTempTransition != null && element.highTempTransition.IsLiquid && element.defaultValues.mass > element.highTempTransition.maxMass)
				{
					global::Debug.LogWarning(string.Format("{0} defaultMass {1} > {2}: maxMass {3}", new object[]
					{
						text,
						element.defaultValues.mass,
						element.highTempTransition.tag.ProperNameStripLink(),
						element.highTempTransition.maxMass
					}));
				}
				if (element.defaultValues.mass < element.maxMass && element.IsLiquid)
				{
					global::Debug.LogWarning(string.Format("{0} has defaultMass: {1} and maxMass {2}", element.tag.ProperNameStripLink(), element.defaultValues.mass, element.maxMass));
				}
			}
		}
		global::Debug.Log("------ End Validating Elements ------");
	}

	// Token: 0x04002D07 RID: 11527
	public static List<Element> elements;

	// Token: 0x04002D08 RID: 11528
	public static Dictionary<int, Element> elementTable;

	// Token: 0x04002D09 RID: 11529
	public static Dictionary<Tag, Element> elementTagTable;

	// Token: 0x04002D0A RID: 11530
	private static string path = Application.streamingAssetsPath + "/elements/";

	// Token: 0x04002D0B RID: 11531
	private static readonly Color noColour = new Color(0f, 0f, 0f, 0f);

	// Token: 0x020018A4 RID: 6308
	public class ElementEntryCollection
	{
		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06009921 RID: 39201 RVA: 0x0036A520 File Offset: 0x00368720
		// (set) Token: 0x06009922 RID: 39202 RVA: 0x0036A528 File Offset: 0x00368728
		public ElementLoader.ElementEntry[] elements { get; set; }
	}

	// Token: 0x020018A5 RID: 6309
	public class ElementComposition
	{
		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06009925 RID: 39205 RVA: 0x0036A541 File Offset: 0x00368741
		// (set) Token: 0x06009926 RID: 39206 RVA: 0x0036A549 File Offset: 0x00368749
		public string elementID { get; set; }

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06009927 RID: 39207 RVA: 0x0036A552 File Offset: 0x00368752
		// (set) Token: 0x06009928 RID: 39208 RVA: 0x0036A55A File Offset: 0x0036875A
		public float percentage { get; set; }
	}

	// Token: 0x020018A6 RID: 6310
	public class ElementEntry
	{
		// Token: 0x06009929 RID: 39209 RVA: 0x0036A563 File Offset: 0x00368763
		public ElementEntry()
		{
			this.lowTemp = 0f;
			this.highTemp = 10000f;
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x0600992A RID: 39210 RVA: 0x0036A581 File Offset: 0x00368781
		// (set) Token: 0x0600992B RID: 39211 RVA: 0x0036A589 File Offset: 0x00368789
		public string elementId { get; set; }

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x0600992C RID: 39212 RVA: 0x0036A592 File Offset: 0x00368792
		// (set) Token: 0x0600992D RID: 39213 RVA: 0x0036A59A File Offset: 0x0036879A
		public float specificHeatCapacity { get; set; }

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x0600992E RID: 39214 RVA: 0x0036A5A3 File Offset: 0x003687A3
		// (set) Token: 0x0600992F RID: 39215 RVA: 0x0036A5AB File Offset: 0x003687AB
		public float thermalConductivity { get; set; }

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06009930 RID: 39216 RVA: 0x0036A5B4 File Offset: 0x003687B4
		// (set) Token: 0x06009931 RID: 39217 RVA: 0x0036A5BC File Offset: 0x003687BC
		public float solidSurfaceAreaMultiplier { get; set; }

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06009932 RID: 39218 RVA: 0x0036A5C5 File Offset: 0x003687C5
		// (set) Token: 0x06009933 RID: 39219 RVA: 0x0036A5CD File Offset: 0x003687CD
		public float liquidSurfaceAreaMultiplier { get; set; }

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06009934 RID: 39220 RVA: 0x0036A5D6 File Offset: 0x003687D6
		// (set) Token: 0x06009935 RID: 39221 RVA: 0x0036A5DE File Offset: 0x003687DE
		public float gasSurfaceAreaMultiplier { get; set; }

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06009936 RID: 39222 RVA: 0x0036A5E7 File Offset: 0x003687E7
		// (set) Token: 0x06009937 RID: 39223 RVA: 0x0036A5EF File Offset: 0x003687EF
		public float defaultMass { get; set; }

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06009938 RID: 39224 RVA: 0x0036A5F8 File Offset: 0x003687F8
		// (set) Token: 0x06009939 RID: 39225 RVA: 0x0036A600 File Offset: 0x00368800
		public float defaultTemperature { get; set; }

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x0600993A RID: 39226 RVA: 0x0036A609 File Offset: 0x00368809
		// (set) Token: 0x0600993B RID: 39227 RVA: 0x0036A611 File Offset: 0x00368811
		public float defaultPressure { get; set; }

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x0600993C RID: 39228 RVA: 0x0036A61A File Offset: 0x0036881A
		// (set) Token: 0x0600993D RID: 39229 RVA: 0x0036A622 File Offset: 0x00368822
		public float molarMass { get; set; }

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x0600993E RID: 39230 RVA: 0x0036A62B File Offset: 0x0036882B
		// (set) Token: 0x0600993F RID: 39231 RVA: 0x0036A633 File Offset: 0x00368833
		public float lightAbsorptionFactor { get; set; }

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06009940 RID: 39232 RVA: 0x0036A63C File Offset: 0x0036883C
		// (set) Token: 0x06009941 RID: 39233 RVA: 0x0036A644 File Offset: 0x00368844
		public float radiationAbsorptionFactor { get; set; }

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06009942 RID: 39234 RVA: 0x0036A64D File Offset: 0x0036884D
		// (set) Token: 0x06009943 RID: 39235 RVA: 0x0036A655 File Offset: 0x00368855
		public float radiationPer1000Mass { get; set; }

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06009944 RID: 39236 RVA: 0x0036A65E File Offset: 0x0036885E
		// (set) Token: 0x06009945 RID: 39237 RVA: 0x0036A666 File Offset: 0x00368866
		public string lowTempTransitionTarget { get; set; }

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06009946 RID: 39238 RVA: 0x0036A66F File Offset: 0x0036886F
		// (set) Token: 0x06009947 RID: 39239 RVA: 0x0036A677 File Offset: 0x00368877
		public float lowTemp { get; set; }

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06009948 RID: 39240 RVA: 0x0036A680 File Offset: 0x00368880
		// (set) Token: 0x06009949 RID: 39241 RVA: 0x0036A688 File Offset: 0x00368888
		public string highTempTransitionTarget { get; set; }

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x0600994A RID: 39242 RVA: 0x0036A691 File Offset: 0x00368891
		// (set) Token: 0x0600994B RID: 39243 RVA: 0x0036A699 File Offset: 0x00368899
		public float highTemp { get; set; }

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x0600994C RID: 39244 RVA: 0x0036A6A2 File Offset: 0x003688A2
		// (set) Token: 0x0600994D RID: 39245 RVA: 0x0036A6AA File Offset: 0x003688AA
		public string lowTempTransitionOreId { get; set; }

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x0600994E RID: 39246 RVA: 0x0036A6B3 File Offset: 0x003688B3
		// (set) Token: 0x0600994F RID: 39247 RVA: 0x0036A6BB File Offset: 0x003688BB
		public float lowTempTransitionOreMassConversion { get; set; }

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06009950 RID: 39248 RVA: 0x0036A6C4 File Offset: 0x003688C4
		// (set) Token: 0x06009951 RID: 39249 RVA: 0x0036A6CC File Offset: 0x003688CC
		public string highTempTransitionOreId { get; set; }

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06009952 RID: 39250 RVA: 0x0036A6D5 File Offset: 0x003688D5
		// (set) Token: 0x06009953 RID: 39251 RVA: 0x0036A6DD File Offset: 0x003688DD
		public float highTempTransitionOreMassConversion { get; set; }

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06009954 RID: 39252 RVA: 0x0036A6E6 File Offset: 0x003688E6
		// (set) Token: 0x06009955 RID: 39253 RVA: 0x0036A6EE File Offset: 0x003688EE
		public string sublimateId { get; set; }

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06009956 RID: 39254 RVA: 0x0036A6F7 File Offset: 0x003688F7
		// (set) Token: 0x06009957 RID: 39255 RVA: 0x0036A6FF File Offset: 0x003688FF
		public string sublimateFx { get; set; }

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06009958 RID: 39256 RVA: 0x0036A708 File Offset: 0x00368908
		// (set) Token: 0x06009959 RID: 39257 RVA: 0x0036A710 File Offset: 0x00368910
		public float sublimateRate { get; set; }

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x0600995A RID: 39258 RVA: 0x0036A719 File Offset: 0x00368919
		// (set) Token: 0x0600995B RID: 39259 RVA: 0x0036A721 File Offset: 0x00368921
		public float sublimateEfficiency { get; set; }

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x0600995C RID: 39260 RVA: 0x0036A72A File Offset: 0x0036892A
		// (set) Token: 0x0600995D RID: 39261 RVA: 0x0036A732 File Offset: 0x00368932
		public float sublimateProbability { get; set; }

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x0600995E RID: 39262 RVA: 0x0036A73B File Offset: 0x0036893B
		// (set) Token: 0x0600995F RID: 39263 RVA: 0x0036A743 File Offset: 0x00368943
		public float offGasPercentage { get; set; }

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06009960 RID: 39264 RVA: 0x0036A74C File Offset: 0x0036894C
		// (set) Token: 0x06009961 RID: 39265 RVA: 0x0036A754 File Offset: 0x00368954
		public string materialCategory { get; set; }

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06009962 RID: 39266 RVA: 0x0036A75D File Offset: 0x0036895D
		// (set) Token: 0x06009963 RID: 39267 RVA: 0x0036A765 File Offset: 0x00368965
		public string[] tags { get; set; }

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06009964 RID: 39268 RVA: 0x0036A76E File Offset: 0x0036896E
		// (set) Token: 0x06009965 RID: 39269 RVA: 0x0036A776 File Offset: 0x00368976
		public bool isDisabled { get; set; }

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06009966 RID: 39270 RVA: 0x0036A77F File Offset: 0x0036897F
		// (set) Token: 0x06009967 RID: 39271 RVA: 0x0036A787 File Offset: 0x00368987
		public float strength { get; set; }

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06009968 RID: 39272 RVA: 0x0036A790 File Offset: 0x00368990
		// (set) Token: 0x06009969 RID: 39273 RVA: 0x0036A798 File Offset: 0x00368998
		public float maxMass { get; set; }

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x0600996A RID: 39274 RVA: 0x0036A7A1 File Offset: 0x003689A1
		// (set) Token: 0x0600996B RID: 39275 RVA: 0x0036A7A9 File Offset: 0x003689A9
		public byte hardness { get; set; }

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x0600996C RID: 39276 RVA: 0x0036A7B2 File Offset: 0x003689B2
		// (set) Token: 0x0600996D RID: 39277 RVA: 0x0036A7BA File Offset: 0x003689BA
		public float toxicity { get; set; }

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x0600996E RID: 39278 RVA: 0x0036A7C3 File Offset: 0x003689C3
		// (set) Token: 0x0600996F RID: 39279 RVA: 0x0036A7CB File Offset: 0x003689CB
		public float liquidCompression { get; set; }

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06009970 RID: 39280 RVA: 0x0036A7D4 File Offset: 0x003689D4
		// (set) Token: 0x06009971 RID: 39281 RVA: 0x0036A7DC File Offset: 0x003689DC
		public float speed { get; set; }

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06009972 RID: 39282 RVA: 0x0036A7E5 File Offset: 0x003689E5
		// (set) Token: 0x06009973 RID: 39283 RVA: 0x0036A7ED File Offset: 0x003689ED
		public float minHorizontalFlow { get; set; }

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06009974 RID: 39284 RVA: 0x0036A7F6 File Offset: 0x003689F6
		// (set) Token: 0x06009975 RID: 39285 RVA: 0x0036A7FE File Offset: 0x003689FE
		public float minVerticalFlow { get; set; }

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06009976 RID: 39286 RVA: 0x0036A807 File Offset: 0x00368A07
		// (set) Token: 0x06009977 RID: 39287 RVA: 0x0036A80F File Offset: 0x00368A0F
		public string convertId { get; set; }

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06009978 RID: 39288 RVA: 0x0036A818 File Offset: 0x00368A18
		// (set) Token: 0x06009979 RID: 39289 RVA: 0x0036A820 File Offset: 0x00368A20
		public float flow { get; set; }

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x0600997A RID: 39290 RVA: 0x0036A829 File Offset: 0x00368A29
		// (set) Token: 0x0600997B RID: 39291 RVA: 0x0036A831 File Offset: 0x00368A31
		public int buildMenuSort { get; set; }

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x0600997C RID: 39292 RVA: 0x0036A83A File Offset: 0x00368A3A
		// (set) Token: 0x0600997D RID: 39293 RVA: 0x0036A842 File Offset: 0x00368A42
		public Element.State state { get; set; }

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x0600997E RID: 39294 RVA: 0x0036A84B File Offset: 0x00368A4B
		// (set) Token: 0x0600997F RID: 39295 RVA: 0x0036A853 File Offset: 0x00368A53
		public string localizationID { get; set; }

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06009980 RID: 39296 RVA: 0x0036A85C File Offset: 0x00368A5C
		// (set) Token: 0x06009981 RID: 39297 RVA: 0x0036A864 File Offset: 0x00368A64
		public string dlcId { get; set; }

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06009982 RID: 39298 RVA: 0x0036A86D File Offset: 0x00368A6D
		// (set) Token: 0x06009983 RID: 39299 RVA: 0x0036A875 File Offset: 0x00368A75
		public ElementLoader.ElementComposition[] composition { get; set; }

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06009984 RID: 39300 RVA: 0x0036A87E File Offset: 0x00368A7E
		// (set) Token: 0x06009985 RID: 39301 RVA: 0x0036A8A9 File Offset: 0x00368AA9
		public string description
		{
			get
			{
				return this.description_backing ?? ("STRINGS.ELEMENTS." + this.elementId.ToString().ToUpper() + ".DESC");
			}
			set
			{
				this.description_backing = value;
			}
		}

		// Token: 0x04007709 RID: 30473
		private string description_backing;
	}
}

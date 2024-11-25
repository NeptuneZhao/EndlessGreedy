﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BFC RID: 3068
public class CodexEntryGenerator_Elements
{
	// Token: 0x06005E0E RID: 24078 RVA: 0x0022DA24 File Offset: 0x0022BC24
	public static Dictionary<string, CodexEntry> GenerateEntries()
	{
		CodexEntryGenerator_Elements.<>c__DisplayClass8_0 CS$<>8__locals1;
		CS$<>8__locals1.entriesElements = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary2 = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary3 = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary4 = new Dictionary<string, CodexEntry>();
		CodexEntryGenerator_Elements.<GenerateEntries>g__AddCategoryEntry|8_0(CodexEntryGenerator_Elements.ELEMENTS_SOLIDS_ID, UI.CODEX.CATEGORYNAMES.ELEMENTSSOLID, Assets.GetSprite("ui_elements-solid"), dictionary, ref CS$<>8__locals1);
		CodexEntryGenerator_Elements.<GenerateEntries>g__AddCategoryEntry|8_0(CodexEntryGenerator_Elements.ELEMENTS_LIQUIDS_ID, UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID, Assets.GetSprite("ui_elements-liquids"), dictionary2, ref CS$<>8__locals1);
		CodexEntryGenerator_Elements.<GenerateEntries>g__AddCategoryEntry|8_0(CodexEntryGenerator_Elements.ELEMENTS_GASES_ID, UI.CODEX.CATEGORYNAMES.ELEMENTSGAS, Assets.GetSprite("ui_elements-gases"), dictionary3, ref CS$<>8__locals1);
		CodexEntryGenerator_Elements.<GenerateEntries>g__AddCategoryEntry|8_0(CodexEntryGenerator_Elements.ELEMENTS_OTHER_ID, UI.CODEX.CATEGORYNAMES.ELEMENTSOTHER, Assets.GetSprite("ui_elements-other"), dictionary4, ref CS$<>8__locals1);
		foreach (Element element in ElementLoader.elements)
		{
			if (!element.disabled)
			{
				bool flag = false;
				Tag[] oreTags = element.oreTags;
				for (int i = 0; i < oreTags.Length; i++)
				{
					if (oreTags[i] == GameTags.HideFromCodex)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					global::Tuple<Sprite, Color> tuple = Def.GetUISprite(element, "ui", false);
					if (tuple.first == null)
					{
						if (element.id == SimHashes.Void)
						{
							tuple = new global::Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-void"), Color.white);
						}
						else if (element.id == SimHashes.Vacuum)
						{
							tuple = new global::Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-vacuum"), Color.white);
						}
					}
					List<ContentContainer> list = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(element.name, list);
					CodexEntryGenerator.GenerateImageContainers(new global::Tuple<Sprite, Color>[]
					{
						tuple
					}, list, ContentContainer.ContentLayout.Horizontal);
					CodexEntryGenerator_Elements.GenerateElementDescriptionContainers(element, list);
					string text;
					Dictionary<string, CodexEntry> dictionary5;
					if (element.IsSolid)
					{
						text = CodexEntryGenerator_Elements.ELEMENTS_SOLIDS_ID;
						dictionary5 = dictionary;
					}
					else if (element.IsLiquid)
					{
						text = CodexEntryGenerator_Elements.ELEMENTS_LIQUIDS_ID;
						dictionary5 = dictionary2;
					}
					else if (element.IsGas)
					{
						text = CodexEntryGenerator_Elements.ELEMENTS_GASES_ID;
						dictionary5 = dictionary3;
					}
					else
					{
						text = CodexEntryGenerator_Elements.ELEMENTS_OTHER_ID;
						dictionary5 = dictionary4;
					}
					string text2 = element.id.ToString();
					CodexEntry codexEntry = new CodexEntry(text, list, element.name);
					codexEntry.parentId = text;
					codexEntry.icon = tuple.first;
					codexEntry.iconColor = tuple.second;
					CodexCache.AddEntry(text2, codexEntry, null);
					dictionary5.Add(text2, codexEntry);
				}
			}
		}
		string text3 = "IceBellyPoop";
		GameObject gameObject = Assets.TryGetPrefab(text3);
		if (gameObject != null)
		{
			string elements_SOLIDS_ID = CodexEntryGenerator_Elements.ELEMENTS_SOLIDS_ID;
			Dictionary<string, CodexEntry> dictionary6 = dictionary;
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			InfoDescription component2 = gameObject.GetComponent<InfoDescription>();
			string properName = gameObject.GetProperName();
			string description = component2.description;
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(gameObject, "ui", false);
			List<ContentContainer> list2 = new List<ContentContainer>();
			CodexEntryGenerator.GenerateTitleContainers(properName, list2);
			CodexEntryGenerator.GenerateImageContainers(new global::Tuple<Sprite, Color>[]
			{
				uisprite
			}, list2, ContentContainer.ContentLayout.Horizontal);
			CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(component.PrefabTag, list2);
			list2.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(description, CodexTextStyle.Body, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical));
			CodexEntry codexEntry2 = new CodexEntry(elements_SOLIDS_ID, list2, properName);
			codexEntry2.parentId = elements_SOLIDS_ID;
			codexEntry2.icon = uisprite.first;
			codexEntry2.iconColor = uisprite.second;
			CodexCache.AddEntry(text3, codexEntry2, null);
			dictionary6.Add(text3, codexEntry2);
		}
		CodexEntryGenerator.PopulateCategoryEntries(CS$<>8__locals1.entriesElements);
		return CS$<>8__locals1.entriesElements;
	}

	// Token: 0x06005E0F RID: 24079 RVA: 0x0022DDFC File Offset: 0x0022BFFC
	public static void GenerateElementDescriptionContainers(Element element, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		if (element.highTempTransition != null)
		{
			list.Add(new CodexTemperatureTransitionPanel(element, CodexTemperatureTransitionPanel.TransitionType.HEAT));
		}
		if (element.lowTempTransition != null)
		{
			list.Add(new CodexTemperatureTransitionPanel(element, CodexTemperatureTransitionPanel.TransitionType.COOL));
		}
		foreach (Element element2 in ElementLoader.elements)
		{
			if (!element2.disabled)
			{
				if (element2.highTempTransition == element || ElementLoader.FindElementByHash(element2.highTempTransitionOreID) == element)
				{
					list2.Add(new CodexTemperatureTransitionPanel(element2, CodexTemperatureTransitionPanel.TransitionType.HEAT));
				}
				if (element2.lowTempTransition == element || ElementLoader.FindElementByHash(element2.lowTempTransitionOreID) == element)
				{
					list2.Add(new CodexTemperatureTransitionPanel(element2, CodexTemperatureTransitionPanel.TransitionType.COOL));
				}
			}
		}
		if (list.Count > 0)
		{
			ContentContainer contentContainer = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTTRANSITIONSTO, contentContainer)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer);
		}
		if (list2.Count > 0)
		{
			ContentContainer contentContainer2 = new ContentContainer(list2, ContentContainer.ContentLayout.Vertical);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTTRANSITIONSFROM, contentContainer2)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer2);
		}
		CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(element.tag, containers);
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexSpacer(),
			new CodexText(element.FullDescription(true), CodexTextStyle.Body, null),
			new CodexSpacer()
		}, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005E10 RID: 24080 RVA: 0x0022DFB0 File Offset: 0x0022C1B0
	public static void GenerateMadeAndUsedContainers(Tag tag, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		Func<ComplexRecipe.RecipeElement, bool> <>9__0;
		Func<ComplexRecipe.RecipeElement, bool> <>9__1;
		foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
		{
			IEnumerable<ComplexRecipe.RecipeElement> ingredients = complexRecipe.ingredients;
			Func<ComplexRecipe.RecipeElement, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((ComplexRecipe.RecipeElement i) => i.material == tag));
			}
			if (ingredients.Any(predicate))
			{
				list.Add(new CodexRecipePanel(complexRecipe, false));
			}
			IEnumerable<ComplexRecipe.RecipeElement> results = complexRecipe.results;
			Func<ComplexRecipe.RecipeElement, bool> predicate2;
			if ((predicate2 = <>9__1) == null)
			{
				predicate2 = (<>9__1 = ((ComplexRecipe.RecipeElement i) => i.material == tag));
			}
			if (results.Any(predicate2))
			{
				list2.Add(new CodexRecipePanel(complexRecipe, true));
			}
		}
		List<CodexEntryGenerator_Elements.ConversionEntry> list3;
		if (CodexEntryGenerator_Elements.GetElementEntryContext().usedMap.map.TryGetValue(tag, out list3))
		{
			foreach (CodexEntryGenerator_Elements.ConversionEntry conversionEntry in list3)
			{
				list.Add(new CodexConversionPanel(conversionEntry.title, conversionEntry.inSet.ToArray<ElementUsage>(), conversionEntry.outSet.ToArray<ElementUsage>(), conversionEntry.prefab));
			}
		}
		List<CodexEntryGenerator_Elements.ConversionEntry> list4;
		if (CodexEntryGenerator_Elements.GetElementEntryContext().madeMap.map.TryGetValue(tag, out list4))
		{
			foreach (CodexEntryGenerator_Elements.ConversionEntry conversionEntry2 in list4)
			{
				list2.Add(new CodexConversionPanel(conversionEntry2.title, conversionEntry2.inSet.ToArray<ElementUsage>(), conversionEntry2.outSet.ToArray<ElementUsage>(), conversionEntry2.prefab));
			}
		}
		ContentContainer contentContainer = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
		ContentContainer contentContainer2 = new ContentContainer(list2, ContentContainer.ContentLayout.Vertical);
		if (list.Count > 0)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTCONSUMEDBY, contentContainer)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer);
		}
		if (list2.Count > 0)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTPRODUCEDBY, contentContainer2)
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(contentContainer2);
		}
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x0022E244 File Offset: 0x0022C444
	public static CodexEntryGenerator_Elements.ElementEntryContext GetElementEntryContext()
	{
		if (CodexEntryGenerator_Elements.contextInstance == null)
		{
			CodexEntryGenerator_Elements.CodexElementMap codexElementMap = new CodexEntryGenerator_Elements.CodexElementMap();
			CodexEntryGenerator_Elements.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.madeMap = new CodexEntryGenerator_Elements.CodexElementMap();
			CodexEntryGenerator_Elements.<>c__DisplayClass12_1 CS$<>8__locals2;
			CS$<>8__locals2.waterTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
			CS$<>8__locals2.dirtyWaterTag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
			foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
			{
				foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
				{
					BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
					if (buildingDef == null)
					{
						global::Debug.LogError("Building def for id " + keyValuePair.Key + " is null");
					}
					if (!buildingDef.Deprecated && !buildingDef.BuildingComplete.HasTag(GameTags.DevBuilding))
					{
						CodexEntryGenerator_Elements.<GetElementEntryContext>g__CheckPrefab|12_0(buildingDef.BuildingComplete, codexElementMap, CS$<>8__locals1.madeMap, ref CS$<>8__locals1, ref CS$<>8__locals2);
					}
				}
			}
			HashSet<GameObject> hashSet = new HashSet<GameObject>(Assets.GetPrefabsWithComponent<Harvestable>());
			foreach (GameObject item in Assets.GetPrefabsWithComponent<WiltCondition>())
			{
				hashSet.Add(item);
			}
			foreach (GameObject gameObject in hashSet)
			{
				if (!(gameObject.GetComponent<BudUprootedMonitor>() != null))
				{
					CodexEntryGenerator_Elements.<GetElementEntryContext>g__CheckPrefab|12_0(gameObject, codexElementMap, CS$<>8__locals1.madeMap, ref CS$<>8__locals1, ref CS$<>8__locals2);
				}
			}
			foreach (GameObject gameObject2 in Assets.GetPrefabsWithComponent<CreatureBrain>())
			{
				if (gameObject2.GetDef<BabyMonitor.Def>() == null)
				{
					CodexEntryGenerator_Elements.<GetElementEntryContext>g__CheckPrefab|12_0(gameObject2, codexElementMap, CS$<>8__locals1.madeMap, ref CS$<>8__locals1, ref CS$<>8__locals2);
				}
			}
			foreach (KeyValuePair<Tag, Diet> keyValuePair2 in DietManager.CollectSaveDiets(null))
			{
				GameObject gameObject3 = Assets.GetPrefab(keyValuePair2.Key).gameObject;
				if (gameObject3.GetDef<BabyMonitor.Def>() == null)
				{
					float num = 0f;
					foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(gameObject3.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
					{
						if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
						{
							num = attributeModifier.Value;
						}
					}
					Diet value = keyValuePair2.Value;
					foreach (Diet.Info info in value.infos)
					{
						float num2 = -num / info.caloriesPerKg;
						float amount = num2 * info.producedConversionRate;
						foreach (Tag tag in info.consumedTags)
						{
							ElementUsage item2 = value.IsConsumedTagAbleToBeEatenDirectly(tag) ? new ElementUsage(tag, num2, true, new Func<Tag, float, bool, string>(GameUtil.GetFormattedDirectPlantConsumptionValuePerCycle)) : new ElementUsage(tag, num2, true);
							CodexEntryGenerator_Elements.ConversionEntry conversionEntry = new CodexEntryGenerator_Elements.ConversionEntry();
							conversionEntry.title = gameObject3.GetProperName();
							conversionEntry.prefab = gameObject3;
							conversionEntry.inSet = new HashSet<ElementUsage>();
							conversionEntry.inSet.Add(item2);
							conversionEntry.outSet = new HashSet<ElementUsage>();
							conversionEntry.outSet.Add(new ElementUsage(info.producedElement, amount, true));
							codexElementMap.Add(tag, conversionEntry);
							CS$<>8__locals1.madeMap.Add(info.producedElement, conversionEntry);
						}
					}
				}
			}
			CodexEntryGenerator_Elements.contextInstance = new CodexEntryGenerator_Elements.ElementEntryContext
			{
				usedMap = codexElementMap,
				madeMap = CS$<>8__locals1.madeMap
			};
		}
		return CodexEntryGenerator_Elements.contextInstance;
	}

	// Token: 0x06005E14 RID: 24084 RVA: 0x0022E794 File Offset: 0x0022C994
	[CompilerGenerated]
	internal static void <GenerateEntries>g__AddCategoryEntry|8_0(string categoryId, string name, Sprite icon, Dictionary<string, CodexEntry> entries, ref CodexEntryGenerator_Elements.<>c__DisplayClass8_0 A_4)
	{
		CodexEntry codexEntry = CodexEntryGenerator.GenerateCategoryEntry(categoryId, name, entries, icon, true, true, null);
		codexEntry.parentId = CodexEntryGenerator_Elements.ELEMENTS_ID;
		codexEntry.category = CodexEntryGenerator_Elements.ELEMENTS_ID;
		A_4.entriesElements.Add(categoryId, codexEntry);
	}

	// Token: 0x06005E15 RID: 24085 RVA: 0x0022E7D4 File Offset: 0x0022C9D4
	[CompilerGenerated]
	internal static void <GetElementEntryContext>g__CheckPrefab|12_0(GameObject prefab, CodexEntryGenerator_Elements.CodexElementMap usedMap, CodexEntryGenerator_Elements.CodexElementMap made, ref CodexEntryGenerator_Elements.<>c__DisplayClass12_0 A_3, ref CodexEntryGenerator_Elements.<>c__DisplayClass12_1 A_4)
	{
		HashSet<ElementUsage> hashSet = new HashSet<ElementUsage>();
		HashSet<ElementUsage> hashSet2 = new HashSet<ElementUsage>();
		EnergyGenerator component = prefab.GetComponent<EnergyGenerator>();
		if (component)
		{
			IEnumerable<EnergyGenerator.InputItem> inputs = component.formula.inputs;
			foreach (EnergyGenerator.InputItem inputItem in (inputs ?? Enumerable.Empty<EnergyGenerator.InputItem>()))
			{
				hashSet.Add(new ElementUsage(inputItem.tag, inputItem.consumptionRate, true));
			}
			IEnumerable<EnergyGenerator.OutputItem> outputs = component.formula.outputs;
			foreach (EnergyGenerator.OutputItem outputItem in (outputs ?? Enumerable.Empty<EnergyGenerator.OutputItem>()))
			{
				Tag tag = ElementLoader.FindElementByHash(outputItem.element).tag;
				hashSet2.Add(new ElementUsage(tag, outputItem.creationRate, true));
			}
		}
		IEnumerable<ElementConverter> components = prefab.GetComponents<ElementConverter>();
		foreach (ElementConverter elementConverter in (components ?? Enumerable.Empty<ElementConverter>()))
		{
			IEnumerable<ElementConverter.ConsumedElement> consumedElements = elementConverter.consumedElements;
			foreach (ElementConverter.ConsumedElement consumedElement in (consumedElements ?? Enumerable.Empty<ElementConverter.ConsumedElement>()))
			{
				hashSet.Add(new ElementUsage(consumedElement.Tag, consumedElement.MassConsumptionRate, true));
			}
			IEnumerable<ElementConverter.OutputElement> outputElements = elementConverter.outputElements;
			foreach (ElementConverter.OutputElement outputElement in (outputElements ?? Enumerable.Empty<ElementConverter.OutputElement>()))
			{
				Tag tag2 = ElementLoader.FindElementByHash(outputElement.elementHash).tag;
				hashSet2.Add(new ElementUsage(tag2, outputElement.massGenerationRate, true));
			}
		}
		IEnumerable<ElementConsumer> components2 = prefab.GetComponents<ElementConsumer>();
		foreach (ElementConsumer elementConsumer in (components2 ?? Enumerable.Empty<ElementConsumer>()))
		{
			if (!elementConsumer.storeOnConsume)
			{
				Tag tag3 = ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag;
				hashSet.Add(new ElementUsage(tag3, elementConsumer.consumptionRate, true));
			}
		}
		IrrigationMonitor.Def def = prefab.GetDef<IrrigationMonitor.Def>();
		if (def != null)
		{
			foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in def.consumedElements)
			{
				hashSet.Add(new ElementUsage(consumeInfo.tag, consumeInfo.massConsumptionRate, true));
			}
		}
		FertilizationMonitor.Def def2 = prefab.GetDef<FertilizationMonitor.Def>();
		if (def2 != null)
		{
			foreach (PlantElementAbsorber.ConsumeInfo consumeInfo2 in def2.consumedElements)
			{
				hashSet.Add(new ElementUsage(consumeInfo2.tag, consumeInfo2.massConsumptionRate, true));
			}
		}
		Crop component2 = prefab.GetComponent<Crop>();
		if (component2 != null)
		{
			hashSet2.Add(new ElementUsage(component2.cropId, (float)component2.cropVal.numProduced / component2.cropVal.cropDuration, true));
		}
		FlushToilet component3 = prefab.GetComponent<FlushToilet>();
		if (component3)
		{
			hashSet.Add(new ElementUsage(A_4.waterTag, component3.massConsumedPerUse, false));
			hashSet2.Add(new ElementUsage(A_4.dirtyWaterTag, component3.massEmittedPerUse, false));
		}
		HandSanitizer component4 = prefab.GetComponent<HandSanitizer>();
		if (component4)
		{
			Tag tag4 = ElementLoader.FindElementByHash(component4.consumedElement).tag;
			hashSet.Add(new ElementUsage(tag4, component4.massConsumedPerUse, false));
			if (component4.outputElement != SimHashes.Vacuum)
			{
				Tag tag5 = ElementLoader.FindElementByHash(component4.outputElement).tag;
				hashSet2.Add(new ElementUsage(tag5, component4.massConsumedPerUse, false));
			}
		}
		if (prefab.IsPrefabID("Moo"))
		{
			hashSet.Add(new ElementUsage("GasGrass", MooConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE, false));
			hashSet2.Add(new ElementUsage(ElementLoader.FindElementByHash(SimHashes.Milk).tag, MooTuning.MILK_PER_CYCLE, false));
		}
		CodexEntryGenerator_Elements.ConversionEntry conversionEntry = new CodexEntryGenerator_Elements.ConversionEntry();
		conversionEntry.title = prefab.GetProperName();
		conversionEntry.prefab = prefab;
		conversionEntry.inSet = hashSet;
		conversionEntry.outSet = hashSet2;
		if (hashSet.Count > 0 && hashSet2.Count > 0)
		{
			usedMap.Add(prefab.PrefabID(), conversionEntry);
		}
		foreach (ElementUsage elementUsage in hashSet)
		{
			usedMap.Add(elementUsage.tag, conversionEntry);
		}
		foreach (ElementUsage elementUsage2 in hashSet2)
		{
			A_3.madeMap.Add(elementUsage2.tag, conversionEntry);
		}
		PlantBranchGrower.Def def3 = prefab.GetDef<PlantBranchGrower.Def>();
		if (def3 != null)
		{
			GameObject prefab2 = Assets.GetPrefab(def3.BRANCH_PREFAB_NAME);
			if (prefab2 != null)
			{
				Crop component5 = prefab2.GetComponent<Crop>();
				if (component5 != null && (component2 == null || component5.cropId != component2.cropId || component5.cropVal.numProduced != component2.cropVal.numProduced))
				{
					CodexEntryGenerator_Elements.ConversionEntry conversionEntry2 = new CodexEntryGenerator_Elements.ConversionEntry();
					conversionEntry2.title = prefab2.GetProperName();
					conversionEntry2.prefab = prefab;
					usedMap.Add(prefab.PrefabID(), conversionEntry2);
					conversionEntry2.inSet = new HashSet<ElementUsage>();
					conversionEntry2.outSet = new HashSet<ElementUsage>();
					conversionEntry2.outSet.Add(new ElementUsage(component5.cropId, (float)component5.cropVal.numProduced, false));
					A_3.madeMap.Add(component5.cropId, conversionEntry2);
				}
			}
		}
		ScaleGrowthMonitor.Def def4 = prefab.GetDef<ScaleGrowthMonitor.Def>();
		if (def4 != null)
		{
			CodexEntryGenerator_Elements.ConversionEntry conversionEntry3 = new CodexEntryGenerator_Elements.ConversionEntry();
			conversionEntry3.title = Assets.GetPrefab("ShearingStation").GetProperName();
			conversionEntry3.prefab = Assets.GetPrefab("ShearingStation");
			conversionEntry3.inSet = new HashSet<ElementUsage>();
			conversionEntry3.inSet.Add(new ElementUsage(prefab.PrefabID(), 1f, false));
			usedMap.Add(prefab.PrefabID(), conversionEntry3);
			conversionEntry3.outSet = new HashSet<ElementUsage>();
			conversionEntry3.outSet.Add(new ElementUsage(def4.itemDroppedOnShear, def4.dropMass, false));
			A_3.madeMap.Add(def4.itemDroppedOnShear, conversionEntry3);
		}
		WellFedShearable.Def def5 = prefab.GetDef<WellFedShearable.Def>();
		if (def5 != null)
		{
			CodexEntryGenerator_Elements.ConversionEntry conversionEntry4 = new CodexEntryGenerator_Elements.ConversionEntry();
			conversionEntry4.title = Assets.GetPrefab("ShearingStation").GetProperName();
			conversionEntry4.prefab = Assets.GetPrefab("ShearingStation");
			conversionEntry4.inSet = new HashSet<ElementUsage>();
			conversionEntry4.inSet.Add(new ElementUsage(prefab.PrefabID(), 1f, false));
			usedMap.Add(prefab.PrefabID(), conversionEntry4);
			conversionEntry4.outSet = new HashSet<ElementUsage>();
			conversionEntry4.outSet.Add(new ElementUsage(def5.itemDroppedOnShear, def5.dropMass, false));
			A_3.madeMap.Add(def5.itemDroppedOnShear, conversionEntry4);
		}
		Butcherable component6 = prefab.GetComponent<Butcherable>();
		if (component6 != null)
		{
			CodexEntryGenerator_Elements.ConversionEntry conversionEntry5 = new CodexEntryGenerator_Elements.ConversionEntry();
			conversionEntry5.title = prefab.GetProperName();
			conversionEntry5.prefab = prefab;
			usedMap.Add(prefab.PrefabID(), conversionEntry5);
			conversionEntry5.outSet = new HashSet<ElementUsage>();
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			foreach (string text in component6.drops)
			{
				float num;
				dictionary.TryGetValue(text, out num);
				dictionary[text] = num + Assets.GetPrefab(text).GetComponent<PrimaryElement>().Mass;
			}
			foreach (KeyValuePair<string, float> keyValuePair in dictionary)
			{
				string text2;
				float num2;
				keyValuePair.Deconstruct(out text2, out num2);
				string s = text2;
				float amount = num2;
				conversionEntry5.outSet.Add(new ElementUsage(s, amount, false));
				A_3.madeMap.Add(s, conversionEntry5);
			}
		}
	}

	// Token: 0x04003ED5 RID: 16085
	public static string ELEMENTS_ID = CodexCache.FormatLinkID("ELEMENTS");

	// Token: 0x04003ED6 RID: 16086
	public static string ELEMENTS_SOLIDS_ID = CodexCache.FormatLinkID("ELEMENTS_SOLID");

	// Token: 0x04003ED7 RID: 16087
	public static string ELEMENTS_LIQUIDS_ID = CodexCache.FormatLinkID("ELEMENTS_LIQUID");

	// Token: 0x04003ED8 RID: 16088
	public static string ELEMENTS_GASES_ID = CodexCache.FormatLinkID("ELEMENTS_GAS");

	// Token: 0x04003ED9 RID: 16089
	public static string ELEMENTS_OTHER_ID = CodexCache.FormatLinkID("ELEMENTS_OTHER");

	// Token: 0x04003EDA RID: 16090
	private static CodexEntryGenerator_Elements.ElementEntryContext contextInstance;

	// Token: 0x02001CE6 RID: 7398
	public class ConversionEntry
	{
		// Token: 0x0400856C RID: 34156
		public string title;

		// Token: 0x0400856D RID: 34157
		public GameObject prefab;

		// Token: 0x0400856E RID: 34158
		public HashSet<ElementUsage> inSet = new HashSet<ElementUsage>();

		// Token: 0x0400856F RID: 34159
		public HashSet<ElementUsage> outSet = new HashSet<ElementUsage>();
	}

	// Token: 0x02001CE7 RID: 7399
	public class CodexElementMap
	{
		// Token: 0x0600A737 RID: 42807 RVA: 0x00399E74 File Offset: 0x00398074
		public void Add(Tag t, CodexEntryGenerator_Elements.ConversionEntry ce)
		{
			List<CodexEntryGenerator_Elements.ConversionEntry> list;
			if (this.map.TryGetValue(t, out list))
			{
				list.Add(ce);
				return;
			}
			this.map[t] = new List<CodexEntryGenerator_Elements.ConversionEntry>
			{
				ce
			};
		}

		// Token: 0x04008570 RID: 34160
		public Dictionary<Tag, List<CodexEntryGenerator_Elements.ConversionEntry>> map = new Dictionary<Tag, List<CodexEntryGenerator_Elements.ConversionEntry>>();
	}

	// Token: 0x02001CE8 RID: 7400
	public class ElementEntryContext
	{
		// Token: 0x04008571 RID: 34161
		public CodexEntryGenerator_Elements.CodexElementMap madeMap = new CodexEntryGenerator_Elements.CodexElementMap();

		// Token: 0x04008572 RID: 34162
		public CodexEntryGenerator_Elements.CodexElementMap usedMap = new CodexEntryGenerator_Elements.CodexElementMap();
	}
}

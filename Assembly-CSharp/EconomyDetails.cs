using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Klei.AI;
using ProcGen;
using TUNING;
using UnityEngine;

// Token: 0x02000868 RID: 2152
public class EconomyDetails
{
	// Token: 0x06003C00 RID: 15360 RVA: 0x0014A718 File Offset: 0x00148918
	public EconomyDetails()
	{
		this.massResourceType = new EconomyDetails.Resource.Type("Mass", "kg");
		this.heatResourceType = new EconomyDetails.Resource.Type("Heat Energy", "kdtu");
		this.energyResourceType = new EconomyDetails.Resource.Type("Energy", "joules");
		this.timeResourceType = new EconomyDetails.Resource.Type("Time", "seconds");
		this.attributeResourceType = new EconomyDetails.Resource.Type("Attribute", "units");
		this.caloriesResourceType = new EconomyDetails.Resource.Type("Calories", "kcal");
		this.amountResourceType = new EconomyDetails.Resource.Type("Amount", "units");
		this.buildingTransformationType = new EconomyDetails.Transformation.Type("Building");
		this.foodTransformationType = new EconomyDetails.Transformation.Type("Food");
		this.plantTransformationType = new EconomyDetails.Transformation.Type("Plant");
		this.creatureTransformationType = new EconomyDetails.Transformation.Type("Creature");
		this.dupeTransformationType = new EconomyDetails.Transformation.Type("Duplicant");
		this.referenceTransformationType = new EconomyDetails.Transformation.Type("Reference");
		this.effectTransformationType = new EconomyDetails.Transformation.Type("Effect");
		this.geyserActivePeriodTransformationType = new EconomyDetails.Transformation.Type("GeyserActivePeriod");
		this.geyserLifetimeTransformationType = new EconomyDetails.Transformation.Type("GeyserLifetime");
		this.energyResource = this.CreateResource(TagManager.Create("Energy"), this.energyResourceType);
		this.heatResource = this.CreateResource(TagManager.Create("Heat"), this.heatResourceType);
		this.duplicantTimeResource = this.CreateResource(TagManager.Create("DupeTime"), this.timeResourceType);
		this.caloriesResource = this.CreateResource(new Tag(Db.Get().Amounts.Calories.deltaAttribute.Id), this.caloriesResourceType);
		this.fixedCaloriesResource = this.CreateResource(new Tag(Db.Get().Amounts.Calories.Id), this.caloriesResourceType);
		foreach (Element element in ElementLoader.elements)
		{
			this.CreateResource(element);
		}
		foreach (Tag tag in new List<Tag>
		{
			GameTags.CombustibleLiquid,
			GameTags.CombustibleGas,
			GameTags.CombustibleSolid
		})
		{
			this.CreateResource(tag, this.massResourceType);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			this.CreateResource(foodInfo.Id.ToTag(), this.amountResourceType);
		}
		this.GatherStartingBiomeAmounts();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			this.CreateTransformation(kprefabID, kprefabID.PrefabTag);
			if (kprefabID.GetComponent<GeyserConfigurator>() != null)
			{
				KPrefabID prefab_id = kprefabID;
				Tag prefabTag = kprefabID.PrefabTag;
				this.CreateTransformation(prefab_id, prefabTag.ToString() + "_ActiveOnly");
			}
		}
		foreach (Effect effect in Db.Get().effects.resources)
		{
			this.CreateTransformation(effect);
		}
		EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(TagManager.Create("Duplicant"), this.dupeTransformationType, 1f, false);
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * Assets.GetPrefab(MinionConfig.ID).GetComponent<OxygenBreather>().O2toCO2conversion));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, 0.875f));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, DUPLICANTSTATS.STANDARD.BaseStats.GUESSTIMATE_CALORIES_BURNED_PER_SECOND * 0.001f));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND));
		this.transformations.Add(transformation);
		EconomyDetails.Transformation transformation2 = new EconomyDetails.Transformation(TagManager.Create("Electrolysis"), this.referenceTransformationType, 1f, false);
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), 1.7777778f));
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Hydrogen), 0.22222222f));
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Water), -2f));
		this.transformations.Add(transformation2);
		EconomyDetails.Transformation transformation3 = new EconomyDetails.Transformation(TagManager.Create("MethaneCombustion"), this.referenceTransformationType, 1f, false);
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Methane), -1f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -4f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), 2.75f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Water), 2.25f));
		this.transformations.Add(transformation3);
		EconomyDetails.Transformation transformation4 = new EconomyDetails.Transformation(TagManager.Create("CoalCombustion"), this.referenceTransformationType, 1f, false);
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Carbon), -1f));
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -2.6666667f));
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), 3.6666667f));
		this.transformations.Add(transformation4);
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x0014AD9C File Offset: 0x00148F9C
	private static void WriteProduct(StreamWriter o, string a, string b)
	{
		o.Write(string.Concat(new string[]
		{
			"\"=PRODUCT(",
			a,
			", ",
			b,
			")\""
		}));
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x0014ADCF File Offset: 0x00148FCF
	private static void WriteProduct(StreamWriter o, string a, string b, string c)
	{
		o.Write(string.Concat(new string[]
		{
			"\"=PRODUCT(",
			a,
			", ",
			b,
			", ",
			c,
			")\""
		}));
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x0014AE10 File Offset: 0x00149010
	public void DumpTransformations(EconomyDetails.Scenario scenario, StreamWriter o)
	{
		List<EconomyDetails.Resource> used_resources = new List<EconomyDetails.Resource>();
		foreach (EconomyDetails.Transformation transformation in this.transformations)
		{
			if (scenario.IncludesTransformation(transformation))
			{
				foreach (EconomyDetails.Transformation.Delta delta in transformation.deltas)
				{
					if (!used_resources.Contains(delta.resource))
					{
						used_resources.Add(delta.resource);
					}
				}
			}
		}
		used_resources.Sort((EconomyDetails.Resource x, EconomyDetails.Resource y) => x.tag.Name.CompareTo(y.tag.Name));
		List<EconomyDetails.Ratio> list = new List<EconomyDetails.Ratio>();
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Algae), this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.energyResource, this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Oxygen), this.energyResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Water), this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.DirtyWater), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Water), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Fertilizer), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.energyResource, this.CreateResource(new Tag(Db.Get().Amounts.Stress.deltaAttribute.Id), this.amountResourceType), true));
		list.RemoveAll((EconomyDetails.Ratio x) => !used_resources.Contains(x.input) || !used_resources.Contains(x.output));
		o.Write("Id");
		o.Write(",Count");
		o.Write(",Type");
		o.Write(",Time(s)");
		int num = 4;
		foreach (EconomyDetails.Resource resource in used_resources)
		{
			o.Write(string.Concat(new string[]
			{
				", ",
				resource.tag.Name,
				"(",
				resource.type.unit,
				")"
			}));
			num++;
		}
		o.Write(",MassDelta");
		foreach (EconomyDetails.Ratio ratio in list)
		{
			o.Write(string.Concat(new string[]
			{
				", ",
				ratio.output.tag.Name,
				"(",
				ratio.output.type.unit,
				")/",
				ratio.input.tag.Name,
				"(",
				ratio.input.type.unit,
				")"
			}));
			num++;
		}
		string str = "B";
		o.Write("\n");
		int num2 = 1;
		this.transformations.Sort((EconomyDetails.Transformation x, EconomyDetails.Transformation y) => x.tag.Name.CompareTo(y.tag.Name));
		for (int i = 0; i < this.transformations.Count; i++)
		{
			EconomyDetails.Transformation transformation2 = this.transformations[i];
			if (scenario.IncludesTransformation(transformation2))
			{
				num2++;
			}
		}
		string text = "B" + (num2 + 4).ToString();
		int num3 = 1;
		for (int j = 0; j < this.transformations.Count; j++)
		{
			EconomyDetails.Transformation transformation3 = this.transformations[j];
			if (scenario.IncludesTransformation(transformation3))
			{
				if (transformation3.tag == new Tag(EconomyDetails.debugTag))
				{
					int num4 = 0 + 1;
				}
				num3++;
				o.Write("\"" + transformation3.tag.Name + "\"");
				o.Write("," + scenario.GetCount(transformation3.tag).ToString());
				o.Write(",\"" + transformation3.type.id + "\"");
				if (!transformation3.timeInvariant)
				{
					o.Write(",\"" + transformation3.timeInSeconds.ToString("0.00") + "\"");
				}
				else
				{
					o.Write(",\"invariant\"");
				}
				string a = str + num3.ToString();
				float num5 = 0f;
				bool flag = false;
				foreach (EconomyDetails.Resource resource2 in used_resources)
				{
					EconomyDetails.Transformation.Delta delta2 = null;
					foreach (EconomyDetails.Transformation.Delta delta3 in transformation3.deltas)
					{
						if (delta3.resource.tag == resource2.tag)
						{
							delta2 = delta3;
							break;
						}
					}
					o.Write(",");
					if (delta2 != null && delta2.amount != 0f)
					{
						if (delta2.resource.type == this.massResourceType)
						{
							flag = true;
							num5 += delta2.amount;
						}
						if (!transformation3.timeInvariant)
						{
							EconomyDetails.WriteProduct(o, a, (delta2.amount / transformation3.timeInSeconds).ToString("0.00000"), text);
						}
						else
						{
							EconomyDetails.WriteProduct(o, a, delta2.amount.ToString("0.00000"));
						}
					}
				}
				o.Write(",");
				if (flag)
				{
					num5 /= transformation3.timeInSeconds;
					EconomyDetails.WriteProduct(o, a, num5.ToString("0.00000"), text);
				}
				foreach (EconomyDetails.Ratio ratio2 in list)
				{
					o.Write(", ");
					EconomyDetails.Transformation.Delta delta4 = transformation3.GetDelta(ratio2.input);
					EconomyDetails.Transformation.Delta delta5 = transformation3.GetDelta(ratio2.output);
					if (delta5 != null && delta4 != null && delta4.amount < 0f && (delta5.amount > 0f || ratio2.allowNegativeOutput))
					{
						o.Write(delta5.amount / Mathf.Abs(delta4.amount));
					}
				}
				o.Write("\n");
			}
		}
		int num6 = 4;
		for (int k = 0; k < num; k++)
		{
			if (k >= num6 && k < num6 + used_resources.Count)
			{
				string text2 = ((char)(65 + k % 26)).ToString();
				int num7 = Mathf.FloorToInt((float)k / 26f);
				if (num7 > 0)
				{
					text2 = ((char)(65 + num7 - 1)).ToString() + text2;
				}
				o.Write(string.Concat(new string[]
				{
					"\"=SUM(",
					text2,
					"2: ",
					text2,
					num2.ToString(),
					")\""
				}));
			}
			o.Write(",");
		}
		string str2 = "B" + (num2 + 5).ToString();
		o.Write("\n");
		o.Write("\nTiming:");
		o.Write("\nTimeInSeconds:," + scenario.timeInSeconds.ToString());
		o.Write("\nSecondsPerCycle:," + 600f.ToString());
		o.Write("\nCycles:,=" + text + "/" + str2);
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x0014B73C File Offset: 0x0014993C
	public EconomyDetails.Resource CreateResource(Tag tag, EconomyDetails.Resource.Type resource_type)
	{
		foreach (EconomyDetails.Resource resource in this.resources)
		{
			if (resource.tag == tag)
			{
				return resource;
			}
		}
		EconomyDetails.Resource resource2 = new EconomyDetails.Resource(tag, resource_type);
		this.resources.Add(resource2);
		return resource2;
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x0014B7B4 File Offset: 0x001499B4
	public EconomyDetails.Resource CreateResource(Element element)
	{
		return this.CreateResource(element.tag, this.massResourceType);
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x0014B7C8 File Offset: 0x001499C8
	public EconomyDetails.Transformation CreateTransformation(Effect effect)
	{
		EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(new Tag(effect.Id), this.effectTransformationType, 1f, false);
		foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
		{
			EconomyDetails.Resource resource = this.CreateResource(new Tag(attributeModifier.AttributeId), this.attributeResourceType);
			transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource, attributeModifier.Value));
		}
		this.transformations.Add(transformation);
		return transformation;
	}

	// Token: 0x06003C07 RID: 15367 RVA: 0x0014B868 File Offset: 0x00149A68
	public EconomyDetails.Transformation GetTransformation(Tag tag)
	{
		foreach (EconomyDetails.Transformation transformation in this.transformations)
		{
			if (transformation.tag == tag)
			{
				return transformation;
			}
		}
		return null;
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x0014B8CC File Offset: 0x00149ACC
	public EconomyDetails.Transformation CreateTransformation(KPrefabID prefab_id, Tag tag)
	{
		if (tag == new Tag(EconomyDetails.debugTag))
		{
			int num = 0 + 1;
		}
		Building component = prefab_id.GetComponent<Building>();
		ElementConverter component2 = prefab_id.GetComponent<ElementConverter>();
		EnergyConsumer component3 = prefab_id.GetComponent<EnergyConsumer>();
		ElementConsumer component4 = prefab_id.GetComponent<ElementConsumer>();
		BuildingElementEmitter component5 = prefab_id.GetComponent<BuildingElementEmitter>();
		Generator component6 = prefab_id.GetComponent<Generator>();
		EnergyGenerator component7 = prefab_id.GetComponent<EnergyGenerator>();
		ManualGenerator component8 = prefab_id.GetComponent<ManualGenerator>();
		ManualDeliveryKG[] components = prefab_id.GetComponents<ManualDeliveryKG>();
		StateMachineController component9 = prefab_id.GetComponent<StateMachineController>();
		Edible component10 = prefab_id.GetComponent<Edible>();
		Crop component11 = prefab_id.GetComponent<Crop>();
		Uprootable component12 = prefab_id.GetComponent<Uprootable>();
		ComplexRecipe complexRecipe = ComplexRecipeManager.Get().recipes.Find((ComplexRecipe r) => r.FirstResult == prefab_id.PrefabTag);
		List<FertilizationMonitor.Def> list = null;
		List<IrrigationMonitor.Def> list2 = null;
		GeyserConfigurator component13 = prefab_id.GetComponent<GeyserConfigurator>();
		Toilet component14 = prefab_id.GetComponent<Toilet>();
		FlushToilet component15 = prefab_id.GetComponent<FlushToilet>();
		RelaxationPoint component16 = prefab_id.GetComponent<RelaxationPoint>();
		CreatureCalorieMonitor.Def def = prefab_id.gameObject.GetDef<CreatureCalorieMonitor.Def>();
		if (component9 != null)
		{
			list = component9.GetDefs<FertilizationMonitor.Def>();
			list2 = component9.GetDefs<IrrigationMonitor.Def>();
		}
		EconomyDetails.Transformation transformation = null;
		float time_in_seconds = 1f;
		if (component10 != null)
		{
			transformation = new EconomyDetails.Transformation(tag, this.foodTransformationType, time_in_seconds, complexRecipe != null);
		}
		else if (component2 != null || component3 != null || component4 != null || component5 != null || component6 != null || component7 != null || component12 != null || component13 != null || component14 != null || component15 != null || component16 != null || def != null)
		{
			if (component12 != null || component11 != null)
			{
				if (component11 != null)
				{
					time_in_seconds = component11.cropVal.cropDuration;
				}
				transformation = new EconomyDetails.Transformation(tag, this.plantTransformationType, time_in_seconds, false);
			}
			else if (def != null)
			{
				transformation = new EconomyDetails.Transformation(tag, this.creatureTransformationType, time_in_seconds, false);
			}
			else if (component13 != null)
			{
				GeyserConfigurator.GeyserInstanceConfiguration geyserInstanceConfiguration = new GeyserConfigurator.GeyserInstanceConfiguration
				{
					typeId = component13.presetType,
					rateRoll = 0.5f,
					iterationLengthRoll = 0.5f,
					iterationPercentRoll = 0.5f,
					yearLengthRoll = 0.5f,
					yearPercentRoll = 0.5f
				};
				if (tag.Name.Contains("_ActiveOnly"))
				{
					float iterationLength = geyserInstanceConfiguration.GetIterationLength();
					transformation = new EconomyDetails.Transformation(tag, this.geyserActivePeriodTransformationType, iterationLength, false);
				}
				else
				{
					float yearLength = geyserInstanceConfiguration.GetYearLength();
					transformation = new EconomyDetails.Transformation(tag, this.geyserLifetimeTransformationType, yearLength, false);
				}
			}
			else
			{
				if (component14 != null || component15 != null)
				{
					time_in_seconds = 600f;
				}
				transformation = new EconomyDetails.Transformation(tag, this.buildingTransformationType, time_in_seconds, false);
			}
		}
		if (transformation != null)
		{
			if (component2 != null && component2.consumedElements != null)
			{
				foreach (ElementConverter.ConsumedElement consumedElement in component2.consumedElements)
				{
					EconomyDetails.Resource resource = this.CreateResource(consumedElement.Tag, this.massResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource, -consumedElement.MassConsumptionRate));
				}
				if (component2.outputElements != null)
				{
					foreach (ElementConverter.OutputElement outputElement in component2.outputElements)
					{
						Element element = ElementLoader.FindElementByHash(outputElement.elementHash);
						EconomyDetails.Resource resource2 = this.CreateResource(element.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource2, outputElement.massGenerationRate));
					}
				}
			}
			if (component4 != null && component7 == null && (component2 == null || prefab_id.GetComponent<AlgaeHabitat>() != null))
			{
				EconomyDetails.Resource resource3 = this.GetResource(ElementLoader.FindElementByHash(component4.elementToConsume).tag);
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource3, -component4.consumptionRate));
			}
			if (component3 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.energyResource, -component3.WattsNeededWhenActive));
			}
			if (component5 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(component5.element), component5.emitRate));
			}
			if (component6 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.energyResource, component6.GetComponent<Building>().Def.GeneratorWattageRating));
			}
			if (component7 != null)
			{
				if (component7.formula.inputs != null)
				{
					foreach (EnergyGenerator.InputItem inputItem in component7.formula.inputs)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(inputItem.tag), -inputItem.consumptionRate));
					}
				}
				if (component7.formula.outputs != null)
				{
					foreach (EnergyGenerator.OutputItem outputItem in component7.formula.outputs)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(outputItem.element), outputItem.creationRate));
					}
				}
			}
			if (component)
			{
				BuildingDef def2 = component.Def;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.heatResource, def2.SelfHeatKilowattsWhenActive + def2.ExhaustKilowattsWhenActive));
			}
			if (component8)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, -1f));
			}
			if (component10)
			{
				EdiblesManager.FoodInfo foodInfo = component10.FoodInfo;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.fixedCaloriesResource, foodInfo.CaloriesPerUnit * 0.001f));
				ComplexRecipeManager.Get().recipes.Find((ComplexRecipe a) => a.FirstResult == tag);
			}
			if (component11 != null)
			{
				EconomyDetails.Resource resource4 = this.CreateResource(TagManager.Create(component11.cropVal.cropId), this.amountResourceType);
				float num2 = (float)component11.cropVal.numProduced;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource4, num2));
				GameObject prefab = Assets.GetPrefab(new Tag(component11.cropVal.cropId));
				if (prefab != null)
				{
					Edible component17 = prefab.GetComponent<Edible>();
					if (component17 != null)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, component17.FoodInfo.CaloriesPerUnit * num2 * 0.001f));
					}
				}
			}
			if (complexRecipe != null)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
				{
					this.CreateResource(recipeElement.material, this.amountResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(recipeElement.material), -recipeElement.amount));
				}
				foreach (ComplexRecipe.RecipeElement recipeElement2 in complexRecipe.results)
				{
					this.CreateResource(recipeElement2.material, this.amountResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(recipeElement2.material), recipeElement2.amount));
				}
			}
			if (components != null)
			{
				for (int j = 0; j < components.Length; j++)
				{
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, -0.1f * transformation.timeInSeconds));
				}
			}
			if (list != null && list.Count > 0)
			{
				foreach (FertilizationMonitor.Def def3 in list)
				{
					foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in def3.consumedElements)
					{
						EconomyDetails.Resource resource5 = this.CreateResource(consumeInfo.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource5, -consumeInfo.massConsumptionRate * transformation.timeInSeconds));
					}
				}
			}
			if (list2 != null && list2.Count > 0)
			{
				foreach (IrrigationMonitor.Def def4 in list2)
				{
					foreach (PlantElementAbsorber.ConsumeInfo consumeInfo2 in def4.consumedElements)
					{
						EconomyDetails.Resource resource6 = this.CreateResource(consumeInfo2.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource6, -consumeInfo2.massConsumptionRate * transformation.timeInSeconds));
					}
				}
			}
			if (component13 != null)
			{
				GeyserConfigurator.GeyserInstanceConfiguration geyserInstanceConfiguration2 = new GeyserConfigurator.GeyserInstanceConfiguration
				{
					typeId = component13.presetType,
					rateRoll = 0.5f,
					iterationLengthRoll = 0.5f,
					iterationPercentRoll = 0.5f,
					yearLengthRoll = 0.5f,
					yearPercentRoll = 0.5f
				};
				if (tag.Name.Contains("_ActiveOnly"))
				{
					float amount = geyserInstanceConfiguration2.GetMassPerCycle() / 600f * geyserInstanceConfiguration2.GetIterationLength();
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(geyserInstanceConfiguration2.GetElement().CreateTag(), this.massResourceType), amount));
				}
				else
				{
					float amount2 = geyserInstanceConfiguration2.GetMassPerCycle() / 600f * geyserInstanceConfiguration2.GetYearLength() * geyserInstanceConfiguration2.GetYearPercent();
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(geyserInstanceConfiguration2.GetElement().CreateTag(), this.massResourceType), amount2));
				}
			}
			if (component14 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), -DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.Dirt), -component14.solidWastePerUse.mass));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(component14.solidWastePerUse.elementID), component14.solidWastePerUse.mass));
			}
			if (component15 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), -DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.Water), -component15.massConsumedPerUse));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.DirtyWater), component15.massEmittedPerUse));
			}
			if (component16 != null)
			{
				foreach (AttributeModifier attributeModifier in component16.CreateEffect().SelfModifiers)
				{
					EconomyDetails.Resource resource7 = this.CreateResource(new Tag(attributeModifier.AttributeId), this.attributeResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource7, attributeModifier.Value));
				}
			}
			if (def != null)
			{
				this.CollectDietTransformations(prefab_id);
			}
			this.transformations.Add(transformation);
		}
		return transformation;
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x0014C4CC File Offset: 0x0014A6CC
	private void CollectDietTransformations(KPrefabID prefab_id)
	{
		Trait trait = Db.Get().traits.Get(prefab_id.GetComponent<Modifiers>().initialTraits[0]);
		CreatureCalorieMonitor.Def def = prefab_id.gameObject.GetDef<CreatureCalorieMonitor.Def>();
		WildnessMonitor.Def def2 = prefab_id.gameObject.GetDef<WildnessMonitor.Def>();
		List<AttributeModifier> list = new List<AttributeModifier>();
		list.AddRange(trait.SelfModifiers);
		list.AddRange(def2.tameEffect.SelfModifiers);
		float num = 0f;
		float num2 = 0f;
		foreach (AttributeModifier attributeModifier in list)
		{
			if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.maxAttribute.Id)
			{
				num = attributeModifier.Value;
			}
			if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
			{
				num2 = attributeModifier.Value;
			}
		}
		foreach (Diet.Info info in def.diet.infos)
		{
			foreach (Tag tag in info.consumedTags)
			{
				float time_in_seconds = Mathf.Abs(num / num2);
				float num3 = num / info.caloriesPerKg;
				float amount = num3 * info.producedConversionRate;
				EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(new Tag(prefab_id.PrefabTag.Name + "Diet" + tag.Name), this.creatureTransformationType, time_in_seconds, false);
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(tag, this.massResourceType), -num3));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(info.producedElement.ToString()), this.massResourceType), amount));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, num));
				this.transformations.Add(transformation);
			}
		}
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x0014C714 File Offset: 0x0014A914
	private static void CollectDietScenarios(List<EconomyDetails.Scenario> scenarios)
	{
		EconomyDetails.Scenario scenario = new EconomyDetails.Scenario("diets/all", 0f, null);
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.gameObject.GetDef<CreatureCalorieMonitor.Def>();
			if (def != null)
			{
				EconomyDetails.Scenario scenario2 = new EconomyDetails.Scenario("diets/" + kprefabID.name, 0f, null);
				Diet.Info[] infos = def.diet.infos;
				for (int i = 0; i < infos.Length; i++)
				{
					foreach (Tag tag in infos[i].consumedTags)
					{
						Tag tag2 = kprefabID.PrefabTag.Name + "Diet" + tag.Name;
						scenario2.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
						scenario.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
					}
				}
				scenarios.Add(scenario2);
			}
		}
		scenarios.Add(scenario);
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x0014C864 File Offset: 0x0014AA64
	public void GatherStartingBiomeAmounts()
	{
		for (int i = 0; i < Grid.CellCount; i++)
		{
			if (global::World.Instance.zoneRenderData.worldZoneTypes[i] == SubWorld.ZoneType.Sandstone)
			{
				Element key = Grid.Element[i];
				float num = 0f;
				this.startingBiomeAmounts.TryGetValue(key, out num);
				this.startingBiomeAmounts[key] = num + Grid.Mass[i];
				this.startingBiomeCellCount++;
			}
		}
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x0014C8D9 File Offset: 0x0014AAD9
	public EconomyDetails.Resource GetResource(SimHashes element)
	{
		return this.GetResource(ElementLoader.FindElementByHash(element).tag);
	}

	// Token: 0x06003C0D RID: 15373 RVA: 0x0014C8EC File Offset: 0x0014AAEC
	public EconomyDetails.Resource GetResource(Tag tag)
	{
		foreach (EconomyDetails.Resource resource in this.resources)
		{
			if (resource.tag == tag)
			{
				return resource;
			}
		}
		DebugUtil.LogErrorArgs(new object[]
		{
			"Found a tag without a matching resource!",
			tag
		});
		return null;
	}

	// Token: 0x06003C0E RID: 15374 RVA: 0x0014C96C File Offset: 0x0014AB6C
	private float GetDupeBreathingPerSecond(EconomyDetails details)
	{
		return details.GetTransformation(TagManager.Create("Duplicant")).GetDelta(details.GetResource(GameTags.Oxygen)).amount;
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x0014C994 File Offset: 0x0014AB94
	private EconomyDetails.BiomeTransformation CreateBiomeTransformationFromTransformation(EconomyDetails details, Tag transformation_tag, Tag input_resource_tag, Tag output_resource_tag)
	{
		EconomyDetails.Resource resource = details.GetResource(input_resource_tag);
		EconomyDetails.Resource resource2 = details.GetResource(output_resource_tag);
		EconomyDetails.Transformation transformation = details.GetTransformation(transformation_tag);
		float num = transformation.GetDelta(resource2).amount / -transformation.GetDelta(resource).amount;
		float num2 = this.GetDupeBreathingPerSecond(details) * 600f;
		return new EconomyDetails.BiomeTransformation((transformation_tag.Name + input_resource_tag.Name + "Cycles").ToTag(), resource, num / -num2);
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x0014CA0C File Offset: 0x0014AC0C
	private static void DumpEconomyDetails()
	{
		global::Debug.Log("Starting Economy Details Dump...");
		EconomyDetails details = new EconomyDetails();
		List<EconomyDetails.Scenario> list = new List<EconomyDetails.Scenario>();
		EconomyDetails.Scenario item = new EconomyDetails.Scenario("default", 1f, (EconomyDetails.Transformation t) => true);
		list.Add(item);
		EconomyDetails.Scenario item2 = new EconomyDetails.Scenario("all_buildings", 1f, (EconomyDetails.Transformation t) => t.type == details.buildingTransformationType);
		list.Add(item2);
		EconomyDetails.Scenario item3 = new EconomyDetails.Scenario("all_plants", 1f, (EconomyDetails.Transformation t) => t.type == details.plantTransformationType);
		list.Add(item3);
		EconomyDetails.Scenario item4 = new EconomyDetails.Scenario("all_creatures", 1f, (EconomyDetails.Transformation t) => t.type == details.creatureTransformationType);
		list.Add(item4);
		EconomyDetails.Scenario item5 = new EconomyDetails.Scenario("all_stress", 1f, (EconomyDetails.Transformation t) => t.GetDelta(details.GetResource(new Tag(Db.Get().Amounts.Stress.deltaAttribute.Id))) != null);
		list.Add(item5);
		EconomyDetails.Scenario item6 = new EconomyDetails.Scenario("all_foods", 1f, (EconomyDetails.Transformation t) => t.type == details.foodTransformationType);
		list.Add(item6);
		EconomyDetails.Scenario item7 = new EconomyDetails.Scenario("geysers/geysers_active_period_only", 1f, (EconomyDetails.Transformation t) => t.type == details.geyserActivePeriodTransformationType);
		list.Add(item7);
		EconomyDetails.Scenario item8 = new EconomyDetails.Scenario("geysers/geysers_whole_lifetime", 1f, (EconomyDetails.Transformation t) => t.type == details.geyserLifetimeTransformationType);
		list.Add(item8);
		EconomyDetails.Scenario scenario = new EconomyDetails.Scenario("oxygen/algae_distillery", 0f, null);
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("AlgaeDistillery"), 3f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("AlgaeHabitat"), 22f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 9f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		list.Add(scenario);
		EconomyDetails.Scenario scenario2 = new EconomyDetails.Scenario("oxygen/algae_habitat_electrolyzer", 0f, null);
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("AlgaeHabitat", 1f));
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("Duplicant", 1f));
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("Electrolyzer", 1f));
		list.Add(scenario2);
		EconomyDetails.Scenario scenario3 = new EconomyDetails.Scenario("oxygen/electrolyzer", 0f, null);
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 9f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		list.Add(scenario3);
		EconomyDetails.Scenario scenario4 = new EconomyDetails.Scenario("purifiers/methane_generator", 0f, null);
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MethaneGenerator"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FertilizerMaker"), 3f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 2f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PrickleFlower"), 0f));
		list.Add(scenario4);
		EconomyDetails.Scenario scenario5 = new EconomyDetails.Scenario("purifiers/water_purifier", 0f, null);
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Compost"), 2f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 2f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PrickleFlower"), 29f));
		list.Add(scenario5);
		EconomyDetails.Scenario scenario6 = new EconomyDetails.Scenario("energy/petroleum_generator", 0f, null);
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PetroleumGenerator"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("OilRefinery"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("CO2Scrubber"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MethaneGenerator"), 1f));
		list.Add(scenario6);
		EconomyDetails.Scenario scenario7 = new EconomyDetails.Scenario("energy/coal_generator", 0f, (EconomyDetails.Transformation t) => t.tag.Name.Contains("Hatch"));
		scenario7.AddEntry(new EconomyDetails.Scenario.Entry("Generator", 1f));
		list.Add(scenario7);
		EconomyDetails.Scenario scenario8 = new EconomyDetails.Scenario("waste/outhouse", 0f, null);
		scenario8.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Outhouse"), 1f));
		scenario8.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Compost"), 1f));
		list.Add(scenario8);
		EconomyDetails.Scenario scenario9 = new EconomyDetails.Scenario("stress/massage_table", 0f, null);
		scenario9.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MassageTable"), 1f));
		scenario9.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("ManualGenerator"), 1f));
		list.Add(scenario9);
		EconomyDetails.Scenario scenario10 = new EconomyDetails.Scenario("waste/flush_toilet", 0f, null);
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FlushToilet"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FertilizerMaker"), 1f));
		list.Add(scenario10);
		EconomyDetails.CollectDietScenarios(list);
		foreach (EconomyDetails.Transformation transformation in details.transformations)
		{
			EconomyDetails.Transformation transformation_iter = transformation;
			EconomyDetails.Scenario item9 = new EconomyDetails.Scenario("transformations/" + transformation.tag.Name, 1f, (EconomyDetails.Transformation t) => transformation_iter == t);
			list.Add(item9);
		}
		foreach (EconomyDetails.Transformation transformation2 in details.transformations)
		{
			EconomyDetails.Scenario scenario11 = new EconomyDetails.Scenario("transformation_groups/" + transformation2.tag.Name, 0f, null);
			scenario11.AddEntry(new EconomyDetails.Scenario.Entry(transformation2.tag, 1f));
			foreach (EconomyDetails.Transformation transformation3 in details.transformations)
			{
				bool flag = false;
				foreach (EconomyDetails.Transformation.Delta delta in transformation2.deltas)
				{
					if (delta.resource.type != details.energyResourceType)
					{
						foreach (EconomyDetails.Transformation.Delta delta2 in transformation3.deltas)
						{
							if (delta.resource == delta2.resource)
							{
								scenario11.AddEntry(new EconomyDetails.Scenario.Entry(transformation3.tag, 0f));
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
			}
			list.Add(scenario11);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			EconomyDetails.Scenario scenario12 = new EconomyDetails.Scenario("food/" + foodInfo.Id, 0f, null);
			Tag tag2 = TagManager.Create(foodInfo.Id);
			scenario12.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
			scenario12.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 1f));
			List<Tag> list2 = new List<Tag>();
			list2.Add(tag2);
			while (list2.Count > 0)
			{
				Tag tag = list2[0];
				list2.RemoveAt(0);
				ComplexRecipe complexRecipe = ComplexRecipeManager.Get().recipes.Find((ComplexRecipe a) => a.FirstResult == tag);
				if (complexRecipe != null)
				{
					foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
					{
						scenario12.AddEntry(new EconomyDetails.Scenario.Entry(recipeElement.material, 1f));
						list2.Add(recipeElement.material);
					}
				}
				foreach (KPrefabID kprefabID in Assets.Prefabs)
				{
					Crop component = kprefabID.GetComponent<Crop>();
					if (component != null && component.cropVal.cropId == tag.Name)
					{
						scenario12.AddEntry(new EconomyDetails.Scenario.Entry(kprefabID.PrefabTag, 1f));
						list2.Add(kprefabID.PrefabTag);
					}
				}
			}
			list.Add(scenario12);
		}
		if (!Directory.Exists("assets/Tuning/Economy"))
		{
			Directory.CreateDirectory("assets/Tuning/Economy");
		}
		foreach (EconomyDetails.Scenario scenario13 in list)
		{
			string path = "assets/Tuning/Economy/" + scenario13.name + ".csv";
			if (!Directory.Exists(System.IO.Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				details.DumpTransformations(scenario13, streamWriter);
			}
		}
		float dupeBreathingPerSecond = details.GetDupeBreathingPerSecond(details);
		List<EconomyDetails.BiomeTransformation> list3 = new List<EconomyDetails.BiomeTransformation>();
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "MineralDeoxidizer".ToTag(), GameTags.Algae, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "AlgaeHabitat".ToTag(), GameTags.Algae, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "AlgaeHabitat".ToTag(), GameTags.Water, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "Electrolyzer".ToTag(), GameTags.Water, GameTags.Oxygen));
		list3.Add(new EconomyDetails.BiomeTransformation("StartingOxygenCycles".ToTag(), details.GetResource(GameTags.Oxygen), 1f / -(dupeBreathingPerSecond * 600f)));
		list3.Add(new EconomyDetails.BiomeTransformation("StartingOxyliteCycles".ToTag(), details.CreateResource(GameTags.OxyRock, details.massResourceType), 1f / -(dupeBreathingPerSecond * 600f)));
		string path2 = "assets/Tuning/Economy/biomes/starting_amounts.csv";
		if (!Directory.Exists(System.IO.Path.GetDirectoryName(path2)))
		{
			Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path2));
		}
		using (StreamWriter streamWriter2 = new StreamWriter(path2))
		{
			streamWriter2.Write("Resource,Amount");
			foreach (EconomyDetails.BiomeTransformation biomeTransformation in list3)
			{
				streamWriter2.Write("," + biomeTransformation.tag.ToString());
			}
			streamWriter2.Write("\n");
			streamWriter2.Write("Cells, " + details.startingBiomeCellCount.ToString() + "\n");
			foreach (KeyValuePair<Element, float> keyValuePair in details.startingBiomeAmounts)
			{
				streamWriter2.Write(keyValuePair.Key.id.ToString() + ", " + keyValuePair.Value.ToString());
				foreach (EconomyDetails.BiomeTransformation biomeTransformation2 in list3)
				{
					streamWriter2.Write(",");
					float num = biomeTransformation2.Transform(keyValuePair.Key, keyValuePair.Value);
					if (num > 0f)
					{
						streamWriter2.Write(num);
					}
				}
				streamWriter2.Write("\n");
			}
		}
		global::Debug.Log("Completed economy details dump!!");
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x0014D994 File Offset: 0x0014BB94
	private static void DumpNameMapping()
	{
		string path = "assets/Tuning/Economy/name_mapping.csv";
		if (!Directory.Exists("assets/Tuning/Economy"))
		{
			Directory.CreateDirectory("assets/Tuning/Economy");
		}
		using (StreamWriter streamWriter = new StreamWriter(path))
		{
			streamWriter.Write("Game Name, Prefab Name, Anim Files\n");
			foreach (KPrefabID kprefabID in Assets.Prefabs)
			{
				string text = TagManager.StripLinkFormatting(kprefabID.GetProperName());
				Tag tag = kprefabID.PrefabID();
				if (!text.IsNullOrWhiteSpace() && !tag.Name.Contains("UnderConstruction") && !tag.Name.Contains("Preview"))
				{
					streamWriter.Write(text);
					TextWriter textWriter = streamWriter;
					string str = ",";
					Tag tag2 = tag;
					textWriter.Write(str + tag2.ToString());
					KAnimControllerBase component = kprefabID.GetComponent<KAnimControllerBase>();
					if (component != null)
					{
						foreach (KAnimFile kanimFile in component.AnimFiles)
						{
							streamWriter.Write("," + kanimFile.name);
						}
					}
					else
					{
						streamWriter.Write(",");
					}
					streamWriter.Write("\n");
				}
			}
			using (List<PermitResource>.Enumerator enumerator2 = Db.Get().Permits.resources.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PermitResource permit = enumerator2.Current;
					if (Blueprints.Get().skinsRelease.buildingFacades.Any((BuildingFacadeInfo info) => info.id == permit.Id) || Blueprints.Get().skinsRelease.clothingItems.Any((ClothingItemInfo info) => info.id == permit.Id) || Blueprints.Get().skinsRelease.artables.Any((ArtableInfo info) => info.id == permit.Id))
					{
						string value = TagManager.StripLinkFormatting(permit.Name);
						streamWriter.Write(value);
						string id = permit.Id;
						streamWriter.Write("," + id);
						BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
						string str2;
						if (buildingFacadeResource != null)
						{
							str2 = buildingFacadeResource.AnimFile;
						}
						else
						{
							ClothingItemResource clothingItemResource = permit as ClothingItemResource;
							if (clothingItemResource != null)
							{
								str2 = clothingItemResource.AnimFile.name;
							}
							else
							{
								ArtableStage artableStage = permit as ArtableStage;
								if (artableStage != null)
								{
									str2 = artableStage.animFile;
								}
								else
								{
									str2 = "";
								}
							}
						}
						streamWriter.Write("," + str2);
						streamWriter.Write("\n");
					}
				}
			}
		}
	}

	// Token: 0x04002449 RID: 9289
	private List<EconomyDetails.Transformation> transformations = new List<EconomyDetails.Transformation>();

	// Token: 0x0400244A RID: 9290
	private List<EconomyDetails.Resource> resources = new List<EconomyDetails.Resource>();

	// Token: 0x0400244B RID: 9291
	public Dictionary<Element, float> startingBiomeAmounts = new Dictionary<Element, float>();

	// Token: 0x0400244C RID: 9292
	public int startingBiomeCellCount;

	// Token: 0x0400244D RID: 9293
	public EconomyDetails.Resource energyResource;

	// Token: 0x0400244E RID: 9294
	public EconomyDetails.Resource heatResource;

	// Token: 0x0400244F RID: 9295
	public EconomyDetails.Resource duplicantTimeResource;

	// Token: 0x04002450 RID: 9296
	public EconomyDetails.Resource caloriesResource;

	// Token: 0x04002451 RID: 9297
	public EconomyDetails.Resource fixedCaloriesResource;

	// Token: 0x04002452 RID: 9298
	public EconomyDetails.Resource.Type massResourceType;

	// Token: 0x04002453 RID: 9299
	public EconomyDetails.Resource.Type heatResourceType;

	// Token: 0x04002454 RID: 9300
	public EconomyDetails.Resource.Type energyResourceType;

	// Token: 0x04002455 RID: 9301
	public EconomyDetails.Resource.Type timeResourceType;

	// Token: 0x04002456 RID: 9302
	public EconomyDetails.Resource.Type attributeResourceType;

	// Token: 0x04002457 RID: 9303
	public EconomyDetails.Resource.Type caloriesResourceType;

	// Token: 0x04002458 RID: 9304
	public EconomyDetails.Resource.Type amountResourceType;

	// Token: 0x04002459 RID: 9305
	public EconomyDetails.Transformation.Type buildingTransformationType;

	// Token: 0x0400245A RID: 9306
	public EconomyDetails.Transformation.Type foodTransformationType;

	// Token: 0x0400245B RID: 9307
	public EconomyDetails.Transformation.Type plantTransformationType;

	// Token: 0x0400245C RID: 9308
	public EconomyDetails.Transformation.Type creatureTransformationType;

	// Token: 0x0400245D RID: 9309
	public EconomyDetails.Transformation.Type dupeTransformationType;

	// Token: 0x0400245E RID: 9310
	public EconomyDetails.Transformation.Type referenceTransformationType;

	// Token: 0x0400245F RID: 9311
	public EconomyDetails.Transformation.Type effectTransformationType;

	// Token: 0x04002460 RID: 9312
	private const string GEYSER_ACTIVE_SUFFIX = "_ActiveOnly";

	// Token: 0x04002461 RID: 9313
	public EconomyDetails.Transformation.Type geyserActivePeriodTransformationType;

	// Token: 0x04002462 RID: 9314
	public EconomyDetails.Transformation.Type geyserLifetimeTransformationType;

	// Token: 0x04002463 RID: 9315
	private static string debugTag = "CO2Scrubber";

	// Token: 0x0200176B RID: 5995
	public class Resource
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06009596 RID: 38294 RVA: 0x00360024 File Offset: 0x0035E224
		// (set) Token: 0x06009597 RID: 38295 RVA: 0x0036002C File Offset: 0x0035E22C
		public Tag tag { get; private set; }

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06009598 RID: 38296 RVA: 0x00360035 File Offset: 0x0035E235
		// (set) Token: 0x06009599 RID: 38297 RVA: 0x0036003D File Offset: 0x0035E23D
		public EconomyDetails.Resource.Type type { get; private set; }

		// Token: 0x0600959A RID: 38298 RVA: 0x00360046 File Offset: 0x0035E246
		public Resource(Tag tag, EconomyDetails.Resource.Type type)
		{
			this.tag = tag;
			this.type = type;
		}

		// Token: 0x02002587 RID: 9607
		public class Type
		{
			// Token: 0x17000C21 RID: 3105
			// (get) Token: 0x0600BF3A RID: 48954 RVA: 0x003DB143 File Offset: 0x003D9343
			// (set) Token: 0x0600BF3B RID: 48955 RVA: 0x003DB14B File Offset: 0x003D934B
			public string id { get; private set; }

			// Token: 0x17000C22 RID: 3106
			// (get) Token: 0x0600BF3C RID: 48956 RVA: 0x003DB154 File Offset: 0x003D9354
			// (set) Token: 0x0600BF3D RID: 48957 RVA: 0x003DB15C File Offset: 0x003D935C
			public string unit { get; private set; }

			// Token: 0x0600BF3E RID: 48958 RVA: 0x003DB165 File Offset: 0x003D9365
			public Type(string id, string unit)
			{
				this.id = id;
				this.unit = unit;
			}
		}
	}

	// Token: 0x0200176C RID: 5996
	public class BiomeTransformation
	{
		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x0600959B RID: 38299 RVA: 0x0036005C File Offset: 0x0035E25C
		// (set) Token: 0x0600959C RID: 38300 RVA: 0x00360064 File Offset: 0x0035E264
		public Tag tag { get; private set; }

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x0600959D RID: 38301 RVA: 0x0036006D File Offset: 0x0035E26D
		// (set) Token: 0x0600959E RID: 38302 RVA: 0x00360075 File Offset: 0x0035E275
		public EconomyDetails.Resource resource { get; private set; }

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x0600959F RID: 38303 RVA: 0x0036007E File Offset: 0x0035E27E
		// (set) Token: 0x060095A0 RID: 38304 RVA: 0x00360086 File Offset: 0x0035E286
		public float ratio { get; private set; }

		// Token: 0x060095A1 RID: 38305 RVA: 0x0036008F File Offset: 0x0035E28F
		public BiomeTransformation(Tag tag, EconomyDetails.Resource resource, float ratio)
		{
			this.tag = tag;
			this.resource = resource;
			this.ratio = ratio;
		}

		// Token: 0x060095A2 RID: 38306 RVA: 0x003600AC File Offset: 0x0035E2AC
		public float Transform(Element element, float amount)
		{
			if (this.resource.tag == element.tag)
			{
				return this.ratio * amount;
			}
			return 0f;
		}
	}

	// Token: 0x0200176D RID: 5997
	public class Ratio
	{
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x060095A3 RID: 38307 RVA: 0x003600D4 File Offset: 0x0035E2D4
		// (set) Token: 0x060095A4 RID: 38308 RVA: 0x003600DC File Offset: 0x0035E2DC
		public EconomyDetails.Resource input { get; private set; }

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060095A5 RID: 38309 RVA: 0x003600E5 File Offset: 0x0035E2E5
		// (set) Token: 0x060095A6 RID: 38310 RVA: 0x003600ED File Offset: 0x0035E2ED
		public EconomyDetails.Resource output { get; private set; }

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x060095A7 RID: 38311 RVA: 0x003600F6 File Offset: 0x0035E2F6
		// (set) Token: 0x060095A8 RID: 38312 RVA: 0x003600FE File Offset: 0x0035E2FE
		public bool allowNegativeOutput { get; private set; }

		// Token: 0x060095A9 RID: 38313 RVA: 0x00360107 File Offset: 0x0035E307
		public Ratio(EconomyDetails.Resource input, EconomyDetails.Resource output, bool allow_negative_output)
		{
			this.input = input;
			this.output = output;
			this.allowNegativeOutput = allow_negative_output;
		}
	}

	// Token: 0x0200176E RID: 5998
	public class Scenario
	{
		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060095AA RID: 38314 RVA: 0x00360124 File Offset: 0x0035E324
		// (set) Token: 0x060095AB RID: 38315 RVA: 0x0036012C File Offset: 0x0035E32C
		public string name { get; private set; }

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060095AC RID: 38316 RVA: 0x00360135 File Offset: 0x0035E335
		// (set) Token: 0x060095AD RID: 38317 RVA: 0x0036013D File Offset: 0x0035E33D
		public float defaultCount { get; private set; }

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060095AE RID: 38318 RVA: 0x00360146 File Offset: 0x0035E346
		// (set) Token: 0x060095AF RID: 38319 RVA: 0x0036014E File Offset: 0x0035E34E
		public float timeInSeconds { get; set; }

		// Token: 0x060095B0 RID: 38320 RVA: 0x00360157 File Offset: 0x0035E357
		public Scenario(string name, float default_count, Func<EconomyDetails.Transformation, bool> filter)
		{
			this.name = name;
			this.defaultCount = default_count;
			this.filter = filter;
			this.timeInSeconds = 600f;
		}

		// Token: 0x060095B1 RID: 38321 RVA: 0x0036018A File Offset: 0x0035E38A
		public void AddEntry(EconomyDetails.Scenario.Entry entry)
		{
			this.entries.Add(entry);
		}

		// Token: 0x060095B2 RID: 38322 RVA: 0x00360198 File Offset: 0x0035E398
		public float GetCount(Tag tag)
		{
			foreach (EconomyDetails.Scenario.Entry entry in this.entries)
			{
				if (entry.tag == tag)
				{
					return entry.count;
				}
			}
			return this.defaultCount;
		}

		// Token: 0x060095B3 RID: 38323 RVA: 0x00360204 File Offset: 0x0035E404
		public bool IncludesTransformation(EconomyDetails.Transformation transformation)
		{
			if (this.filter != null && this.filter(transformation))
			{
				return true;
			}
			using (List<EconomyDetails.Scenario.Entry>.Enumerator enumerator = this.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.tag == transformation.tag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040072B1 RID: 29361
		private Func<EconomyDetails.Transformation, bool> filter;

		// Token: 0x040072B2 RID: 29362
		private List<EconomyDetails.Scenario.Entry> entries = new List<EconomyDetails.Scenario.Entry>();

		// Token: 0x02002588 RID: 9608
		public class Entry
		{
			// Token: 0x17000C23 RID: 3107
			// (get) Token: 0x0600BF3F RID: 48959 RVA: 0x003DB17B File Offset: 0x003D937B
			// (set) Token: 0x0600BF40 RID: 48960 RVA: 0x003DB183 File Offset: 0x003D9383
			public Tag tag { get; private set; }

			// Token: 0x17000C24 RID: 3108
			// (get) Token: 0x0600BF41 RID: 48961 RVA: 0x003DB18C File Offset: 0x003D938C
			// (set) Token: 0x0600BF42 RID: 48962 RVA: 0x003DB194 File Offset: 0x003D9394
			public float count { get; private set; }

			// Token: 0x0600BF43 RID: 48963 RVA: 0x003DB19D File Offset: 0x003D939D
			public Entry(Tag tag, float count)
			{
				this.tag = tag;
				this.count = count;
			}
		}
	}

	// Token: 0x0200176F RID: 5999
	public class Transformation
	{
		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x060095B4 RID: 38324 RVA: 0x00360280 File Offset: 0x0035E480
		// (set) Token: 0x060095B5 RID: 38325 RVA: 0x00360288 File Offset: 0x0035E488
		public Tag tag { get; private set; }

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x060095B6 RID: 38326 RVA: 0x00360291 File Offset: 0x0035E491
		// (set) Token: 0x060095B7 RID: 38327 RVA: 0x00360299 File Offset: 0x0035E499
		public EconomyDetails.Transformation.Type type { get; private set; }

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x060095B8 RID: 38328 RVA: 0x003602A2 File Offset: 0x0035E4A2
		// (set) Token: 0x060095B9 RID: 38329 RVA: 0x003602AA File Offset: 0x0035E4AA
		public float timeInSeconds { get; private set; }

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060095BA RID: 38330 RVA: 0x003602B3 File Offset: 0x0035E4B3
		// (set) Token: 0x060095BB RID: 38331 RVA: 0x003602BB File Offset: 0x0035E4BB
		public bool timeInvariant { get; private set; }

		// Token: 0x060095BC RID: 38332 RVA: 0x003602C4 File Offset: 0x0035E4C4
		public Transformation(Tag tag, EconomyDetails.Transformation.Type type, float time_in_seconds, bool timeInvariant = false)
		{
			this.tag = tag;
			this.type = type;
			this.timeInSeconds = time_in_seconds;
			this.timeInvariant = timeInvariant;
		}

		// Token: 0x060095BD RID: 38333 RVA: 0x003602F4 File Offset: 0x0035E4F4
		public void AddDelta(EconomyDetails.Transformation.Delta delta)
		{
			global::Debug.Assert(delta.resource != null);
			this.deltas.Add(delta);
		}

		// Token: 0x060095BE RID: 38334 RVA: 0x00360310 File Offset: 0x0035E510
		public EconomyDetails.Transformation.Delta GetDelta(EconomyDetails.Resource resource)
		{
			foreach (EconomyDetails.Transformation.Delta delta in this.deltas)
			{
				if (delta.resource == resource)
				{
					return delta;
				}
			}
			return null;
		}

		// Token: 0x040072B4 RID: 29364
		public List<EconomyDetails.Transformation.Delta> deltas = new List<EconomyDetails.Transformation.Delta>();

		// Token: 0x02002589 RID: 9609
		public class Delta
		{
			// Token: 0x17000C25 RID: 3109
			// (get) Token: 0x0600BF44 RID: 48964 RVA: 0x003DB1B3 File Offset: 0x003D93B3
			// (set) Token: 0x0600BF45 RID: 48965 RVA: 0x003DB1BB File Offset: 0x003D93BB
			public EconomyDetails.Resource resource { get; private set; }

			// Token: 0x17000C26 RID: 3110
			// (get) Token: 0x0600BF46 RID: 48966 RVA: 0x003DB1C4 File Offset: 0x003D93C4
			// (set) Token: 0x0600BF47 RID: 48967 RVA: 0x003DB1CC File Offset: 0x003D93CC
			public float amount { get; set; }

			// Token: 0x0600BF48 RID: 48968 RVA: 0x003DB1D5 File Offset: 0x003D93D5
			public Delta(EconomyDetails.Resource resource, float amount)
			{
				this.resource = resource;
				this.amount = amount;
			}
		}

		// Token: 0x0200258A RID: 9610
		public class Type
		{
			// Token: 0x17000C27 RID: 3111
			// (get) Token: 0x0600BF49 RID: 48969 RVA: 0x003DB1EB File Offset: 0x003D93EB
			// (set) Token: 0x0600BF4A RID: 48970 RVA: 0x003DB1F3 File Offset: 0x003D93F3
			public string id { get; private set; }

			// Token: 0x0600BF4B RID: 48971 RVA: 0x003DB1FC File Offset: 0x003D93FC
			public Type(string id)
			{
				this.id = id;
			}
		}
	}
}

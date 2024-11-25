using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000877 RID: 2167
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverter : StateMachineComponent<ElementConverter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003C83 RID: 15491 RVA: 0x0014F513 File Offset: 0x0014D713
	public void SetWorkSpeedMultiplier(float speed)
	{
		this.workSpeedMultiplier = speed;
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x0014F51C File Offset: 0x0014D71C
	public void SetConsumedElementActive(Tag elementId, bool active)
	{
		int i = 0;
		while (i < this.consumedElements.Length)
		{
			if (!(this.consumedElements[i].Tag != elementId))
			{
				this.consumedElements[i].IsActive = active;
				if (!this.ShowInUI)
				{
					break;
				}
				ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
				if (active)
				{
					base.smi.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, this.consumedElementStatusHandles);
					return;
				}
				base.smi.RemoveStatusItem<Tag>(consumedElement.Tag, this.consumedElementStatusHandles);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x0014F5B8 File Offset: 0x0014D7B8
	public void SetOutputElementActive(SimHashes element, bool active)
	{
		int i = 0;
		while (i < this.outputElements.Length)
		{
			if (this.outputElements[i].elementHash == element)
			{
				this.outputElements[i].IsActive = active;
				ElementConverter.OutputElement outputElement = this.outputElements[i];
				if (active)
				{
					base.smi.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, this.outputElementStatusHandles);
					return;
				}
				base.smi.RemoveStatusItem<SimHashes>(outputElement.elementHash, this.outputElementStatusHandles);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x0014F644 File Offset: 0x0014D844
	public void SetStorage(Storage storage)
	{
		this.storage = storage;
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06003C87 RID: 15495 RVA: 0x0014F64D File Offset: 0x0014D84D
	// (set) Token: 0x06003C88 RID: 15496 RVA: 0x0014F655 File Offset: 0x0014D855
	public float OutputMultiplier
	{
		get
		{
			return this.outputMultiplier;
		}
		set
		{
			this.outputMultiplier = value;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06003C89 RID: 15497 RVA: 0x0014F65E File Offset: 0x0014D85E
	public float AverageConvertRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.outputElements[0].accumulator);
		}
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x0014F680 File Offset: 0x0014D880
	public bool HasEnoughMass(Tag tag, bool includeInactive = false)
	{
		bool result = false;
		List<GameObject> items = this.storage.items;
		foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
		{
			if (!(tag != consumedElement.Tag) && (includeInactive || consumedElement.IsActive))
			{
				float num = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(tag))
					{
						num += gameObject.GetComponent<PrimaryElement>().Mass;
					}
				}
				result = (num >= consumedElement.MassConsumptionRate);
				break;
			}
		}
		return result;
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x0014F738 File Offset: 0x0014D938
	public bool HasEnoughMassToStartConverting(bool includeInactive = false)
	{
		float speedMultiplier = this.GetSpeedMultiplier();
		float num = 1f * speedMultiplier;
		bool flag = includeInactive || this.consumedElements.Length == 0;
		bool flag2 = true;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (includeInactive || consumedElement.IsActive)
			{
				float num2 = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag))
					{
						num2 += gameObject.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num2 < consumedElement.MassConsumptionRate * num)
				{
					flag2 = false;
					break;
				}
			}
		}
		return flag && flag2;
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x0014F820 File Offset: 0x0014DA20
	public bool CanConvertAtAll()
	{
		bool flag = this.consumedElements.Length == 0;
		bool flag2 = true;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (consumedElement.IsActive)
			{
				bool flag3 = false;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag) && gameObject.GetComponent<PrimaryElement>().Mass > 0f)
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					flag2 = false;
					break;
				}
			}
		}
		return flag && flag2;
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x0014F8DF File Offset: 0x0014DADF
	private float GetSpeedMultiplier()
	{
		return this.machinerySpeedAttribute.GetTotalValue() * this.workSpeedMultiplier;
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x0014F8F4 File Offset: 0x0014DAF4
	private void ConvertMass()
	{
		float speedMultiplier = this.GetSpeedMultiplier();
		float num = 1f * speedMultiplier;
		bool flag = this.consumedElements.Length == 0;
		float num2 = 1f;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (consumedElement.IsActive)
			{
				float num3 = consumedElement.MassConsumptionRate * num * num2;
				if (num3 <= 0f)
				{
					num2 = 0f;
					break;
				}
				float num4 = 0f;
				for (int j = 0; j < this.storage.items.Count; j++)
				{
					GameObject gameObject = this.storage.items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag))
					{
						PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
						float num5 = Mathf.Min(num3, component.Mass);
						num4 += num5 / num3;
					}
				}
				num2 = Mathf.Min(num2, num4);
			}
		}
		if (!flag || num2 <= 0f)
		{
			return;
		}
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		diseaseInfo.idx = byte.MaxValue;
		diseaseInfo.count = 0;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		for (int k = 0; k < this.consumedElements.Length; k++)
		{
			ElementConverter.ConsumedElement consumedElement2 = this.consumedElements[k];
			if (consumedElement2.IsActive)
			{
				float num9 = consumedElement2.MassConsumptionRate * num * num2;
				Game.Instance.accumulators.Accumulate(consumedElement2.Accumulator, num9);
				for (int l = 0; l < this.storage.items.Count; l++)
				{
					GameObject gameObject2 = this.storage.items[l];
					if (!(gameObject2 == null))
					{
						if (gameObject2.HasTag(consumedElement2.Tag))
						{
							PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
							component2.KeepZeroMassObject = true;
							float num10 = Mathf.Min(num9, component2.Mass);
							int num11 = (int)(num10 / component2.Mass * (float)component2.DiseaseCount);
							float num12 = num10 * component2.Element.specificHeatCapacity;
							num8 += num12;
							num7 += num12 * component2.Temperature;
							component2.Mass -= num10;
							component2.ModifyDiseaseCount(-num11, "ElementConverter.ConvertMass");
							num6 += num10;
							diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo.idx, diseaseInfo.count, component2.DiseaseIdx, num11);
							num9 -= num10;
							if (num9 <= 0f)
							{
								break;
							}
						}
						if (num9 <= 0f)
						{
							global::Debug.Assert(num9 <= 0f);
						}
					}
				}
			}
		}
		float num13 = (num8 > 0f) ? (num7 / num8) : 0f;
		if (this.onConvertMass != null && num6 > 0f)
		{
			this.onConvertMass(num6);
		}
		for (int m = 0; m < this.outputElements.Length; m++)
		{
			ElementConverter.OutputElement outputElement = this.outputElements[m];
			if (outputElement.IsActive)
			{
				SimUtil.DiseaseInfo diseaseInfo2 = diseaseInfo;
				if (this.totalDiseaseWeight <= 0f)
				{
					diseaseInfo2.idx = byte.MaxValue;
					diseaseInfo2.count = 0;
				}
				else
				{
					float num14 = outputElement.diseaseWeight / this.totalDiseaseWeight;
					diseaseInfo2.count = (int)((float)diseaseInfo2.count * num14);
				}
				if (outputElement.addedDiseaseIdx != 255)
				{
					diseaseInfo2 = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo2, new SimUtil.DiseaseInfo
					{
						idx = outputElement.addedDiseaseIdx,
						count = outputElement.addedDiseaseCount
					});
				}
				float num15 = outputElement.massGenerationRate * this.OutputMultiplier * num * num2;
				Game.Instance.accumulators.Accumulate(outputElement.accumulator, num15);
				float temperature;
				if (outputElement.useEntityTemperature || (num13 == 0f && outputElement.minOutputTemperature == 0f))
				{
					temperature = base.GetComponent<PrimaryElement>().Temperature;
				}
				else
				{
					temperature = Mathf.Max(outputElement.minOutputTemperature, num13);
				}
				Element element = ElementLoader.FindElementByHash(outputElement.elementHash);
				if (outputElement.storeOutput)
				{
					PrimaryElement primaryElement = this.storage.AddToPrimaryElement(outputElement.elementHash, num15, temperature);
					if (primaryElement == null)
					{
						if (element.IsGas)
						{
							this.storage.AddGasChunk(outputElement.elementHash, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, true);
						}
						else if (element.IsLiquid)
						{
							this.storage.AddLiquid(outputElement.elementHash, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, true);
						}
						else
						{
							GameObject go = element.substance.SpawnResource(base.transform.GetPosition(), num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, false, false);
							this.storage.Store(go, true, false, true, false);
						}
					}
					else
					{
						primaryElement.AddDisease(diseaseInfo2.idx, diseaseInfo2.count, "ElementConverter.ConvertMass");
					}
				}
				else
				{
					Vector3 vector = new Vector3(base.transform.GetPosition().x + outputElement.outputElementOffset.x, base.transform.GetPosition().y + outputElement.outputElementOffset.y, 0f);
					int num16 = Grid.PosToCell(vector);
					if (element.IsLiquid)
					{
						FallingWater.instance.AddParticle(num16, element.idx, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, false, false, false);
					}
					else if (element.IsSolid)
					{
						element.substance.SpawnResource(vector, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, false, false, false);
					}
					else
					{
						SimMessages.AddRemoveSubstance(num16, outputElement.elementHash, CellEventLogger.Instance.OxygenModifierSimUpdate, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, -1);
					}
				}
				if (outputElement.elementHash == SimHashes.Oxygen || outputElement.elementHash == SimHashes.ContaminatedOxygen)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num15, base.gameObject.GetProperName(), null);
				}
			}
		}
		this.storage.Trigger(-1697596308, base.gameObject);
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x0014FF60 File Offset: 0x0014E160
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.machinerySpeedAttribute = attributes.Add(Db.Get().Attributes.MachinerySpeed);
		if (ElementConverter.ElementConverterInput == null)
		{
			ElementConverter.ElementConverterInput = new StatusItem("ElementConverterInput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				ElementConverter.ConsumedElement consumedElement = (ElementConverter.ConsumedElement)data;
				str = str.Replace("{ElementTypes}", consumedElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedByTag(consumedElement.Tag, consumedElement.Rate, GameUtil.TimeSlice.PerSecond));
				return str;
			});
		}
		if (ElementConverter.ElementConverterOutput == null)
		{
			ElementConverter.ElementConverterOutput = new StatusItem("ElementConverterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				ElementConverter.OutputElement outputElement = (ElementConverter.OutputElement)data;
				str = str.Replace("{ElementTypes}", outputElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(outputElement.Rate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			});
		}
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x00150040 File Offset: 0x0014E240
	public void SetAllConsumedActive(bool active)
	{
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			this.consumedElements[i].IsActive = active;
		}
		base.smi.sm.canConvert.Set(active, base.smi, false);
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x00150090 File Offset: 0x0014E290
	public void SetConsumedActive(Tag id, bool active)
	{
		bool flag = this.consumedElements.Length == 0;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ref ElementConverter.ConsumedElement ptr = ref this.consumedElements[i];
			if (ptr.Tag == id)
			{
				ptr.IsActive = active;
				if (active)
				{
					flag = true;
					break;
				}
			}
			flag |= ptr.IsActive;
		}
		base.smi.sm.canConvert.Set(flag, base.smi, false);
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x0015010C File Offset: 0x0014E30C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			this.consumedElements[i].Accumulator = Game.Instance.accumulators.Add("ElementsConsumed", this);
		}
		this.totalDiseaseWeight = 0f;
		for (int j = 0; j < this.outputElements.Length; j++)
		{
			this.outputElements[j].accumulator = Game.Instance.accumulators.Add("OutputElements", this);
			this.totalDiseaseWeight += this.outputElements[j].diseaseWeight;
		}
		base.smi.StartSM();
	}

	// Token: 0x06003C93 RID: 15507 RVA: 0x001501C8 File Offset: 0x0014E3C8
	protected override void OnCleanUp()
	{
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			Game.Instance.accumulators.Remove(this.consumedElements[i].Accumulator);
		}
		for (int j = 0; j < this.outputElements.Length; j++)
		{
			Game.Instance.accumulators.Remove(this.outputElements[j].accumulator);
		}
		base.OnCleanUp();
	}

	// Token: 0x06003C94 RID: 15508 RVA: 0x00150244 File Offset: 0x0014E444
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (!this.showDescriptors)
		{
			return list;
		}
		if (this.consumedElements != null)
		{
			foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
			{
				if (consumedElement.IsActive)
				{
					Descriptor item = default(Descriptor);
					item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, consumedElement.Name, GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, consumedElement.Name, GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
					list.Add(item);
				}
			}
		}
		if (this.outputElements != null)
		{
			foreach (ElementConverter.OutputElement outputElement in this.outputElements)
			{
				if (outputElement.IsActive)
				{
					LocString loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_INPUTTEMP;
					LocString loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_INPUTTEMP;
					if (outputElement.useEntityTemperature)
					{
						loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP;
						loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP;
					}
					else if (outputElement.minOutputTemperature > 0f)
					{
						loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINTEMP;
						loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINTEMP;
					}
					Descriptor item2 = new Descriptor(string.Format(loc_string, outputElement.Name, GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(loc_string2, outputElement.Name, GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
					list.Add(item2);
				}
			}
		}
		return list;
	}

	// Token: 0x040024F5 RID: 9461
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040024F6 RID: 9462
	[MyCmpReq]
	private Storage storage;

	// Token: 0x040024F7 RID: 9463
	public Action<float> onConvertMass;

	// Token: 0x040024F8 RID: 9464
	private float totalDiseaseWeight = float.MaxValue;

	// Token: 0x040024F9 RID: 9465
	public Operational.State OperationalRequirement = Operational.State.Active;

	// Token: 0x040024FA RID: 9466
	private AttributeInstance machinerySpeedAttribute;

	// Token: 0x040024FB RID: 9467
	private float workSpeedMultiplier = 1f;

	// Token: 0x040024FC RID: 9468
	public bool showDescriptors = true;

	// Token: 0x040024FD RID: 9469
	private const float BASE_INTERVAL = 1f;

	// Token: 0x040024FE RID: 9470
	public ElementConverter.ConsumedElement[] consumedElements;

	// Token: 0x040024FF RID: 9471
	public ElementConverter.OutputElement[] outputElements;

	// Token: 0x04002500 RID: 9472
	public bool ShowInUI = true;

	// Token: 0x04002501 RID: 9473
	private float outputMultiplier = 1f;

	// Token: 0x04002502 RID: 9474
	private Dictionary<Tag, Guid> consumedElementStatusHandles = new Dictionary<Tag, Guid>();

	// Token: 0x04002503 RID: 9475
	private Dictionary<SimHashes, Guid> outputElementStatusHandles = new Dictionary<SimHashes, Guid>();

	// Token: 0x04002504 RID: 9476
	private static StatusItem ElementConverterInput;

	// Token: 0x04002505 RID: 9477
	private static StatusItem ElementConverterOutput;

	// Token: 0x0200177E RID: 6014
	[DebuggerDisplay("{tag} {massConsumptionRate}")]
	[Serializable]
	public struct ConsumedElement
	{
		// Token: 0x060095EE RID: 38382 RVA: 0x00360747 File Offset: 0x0035E947
		public ConsumedElement(Tag tag, float kgPerSecond, bool isActive = true)
		{
			this.Tag = tag;
			this.MassConsumptionRate = kgPerSecond;
			this.IsActive = isActive;
			this.Accumulator = HandleVector<int>.InvalidHandle;
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x060095EF RID: 38383 RVA: 0x00360769 File Offset: 0x0035E969
		public string Name
		{
			get
			{
				return this.Tag.ProperName();
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x060095F0 RID: 38384 RVA: 0x00360776 File Offset: 0x0035E976
		public float Rate
		{
			get
			{
				return Game.Instance.accumulators.GetAverageRate(this.Accumulator);
			}
		}

		// Token: 0x040072E2 RID: 29410
		public Tag Tag;

		// Token: 0x040072E3 RID: 29411
		public float MassConsumptionRate;

		// Token: 0x040072E4 RID: 29412
		public bool IsActive;

		// Token: 0x040072E5 RID: 29413
		public HandleVector<int>.Handle Accumulator;
	}

	// Token: 0x0200177F RID: 6015
	[Serializable]
	public struct OutputElement
	{
		// Token: 0x060095F1 RID: 38385 RVA: 0x00360790 File Offset: 0x0035E990
		public OutputElement(float kgPerSecond, SimHashes element, float minOutputTemperature, bool useEntityTemperature = false, bool storeOutput = false, float outputElementOffsetx = 0f, float outputElementOffsety = 0.5f, float diseaseWeight = 1f, byte addedDiseaseIdx = 255, int addedDiseaseCount = 0, bool isActive = true)
		{
			this.elementHash = element;
			this.minOutputTemperature = minOutputTemperature;
			this.useEntityTemperature = useEntityTemperature;
			this.storeOutput = storeOutput;
			this.massGenerationRate = kgPerSecond;
			this.outputElementOffset = new Vector2(outputElementOffsetx, outputElementOffsety);
			this.accumulator = HandleVector<int>.InvalidHandle;
			this.diseaseWeight = diseaseWeight;
			this.addedDiseaseIdx = addedDiseaseIdx;
			this.addedDiseaseCount = addedDiseaseCount;
			this.IsActive = isActive;
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x060095F2 RID: 38386 RVA: 0x003607FC File Offset: 0x0035E9FC
		public string Name
		{
			get
			{
				return ElementLoader.FindElementByHash(this.elementHash).tag.ProperName();
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x060095F3 RID: 38387 RVA: 0x00360813 File Offset: 0x0035EA13
		public float Rate
		{
			get
			{
				return Game.Instance.accumulators.GetAverageRate(this.accumulator);
			}
		}

		// Token: 0x040072E6 RID: 29414
		public bool IsActive;

		// Token: 0x040072E7 RID: 29415
		public SimHashes elementHash;

		// Token: 0x040072E8 RID: 29416
		public float minOutputTemperature;

		// Token: 0x040072E9 RID: 29417
		public bool useEntityTemperature;

		// Token: 0x040072EA RID: 29418
		public float massGenerationRate;

		// Token: 0x040072EB RID: 29419
		public bool storeOutput;

		// Token: 0x040072EC RID: 29420
		public Vector2 outputElementOffset;

		// Token: 0x040072ED RID: 29421
		public HandleVector<int>.Handle accumulator;

		// Token: 0x040072EE RID: 29422
		public float diseaseWeight;

		// Token: 0x040072EF RID: 29423
		public byte addedDiseaseIdx;

		// Token: 0x040072F0 RID: 29424
		public int addedDiseaseCount;
	}

	// Token: 0x02001780 RID: 6016
	public class StatesInstance : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.GameInstance
	{
		// Token: 0x060095F4 RID: 38388 RVA: 0x0036082A File Offset: 0x0035EA2A
		public StatesInstance(ElementConverter smi) : base(smi)
		{
			this.selectable = base.GetComponent<KSelectable>();
		}

		// Token: 0x060095F5 RID: 38389 RVA: 0x00360840 File Offset: 0x0035EA40
		public void AddStatusItems()
		{
			if (!base.master.ShowInUI)
			{
				return;
			}
			foreach (ElementConverter.ConsumedElement consumedElement in base.master.consumedElements)
			{
				if (consumedElement.IsActive)
				{
					this.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, base.master.consumedElementStatusHandles);
				}
			}
			foreach (ElementConverter.OutputElement outputElement in base.master.outputElements)
			{
				if (outputElement.IsActive)
				{
					this.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, base.master.outputElementStatusHandles);
				}
			}
		}

		// Token: 0x060095F6 RID: 38390 RVA: 0x003608F0 File Offset: 0x0035EAF0
		public void RemoveStatusItems()
		{
			if (!base.master.ShowInUI)
			{
				return;
			}
			for (int i = 0; i < base.master.consumedElements.Length; i++)
			{
				ElementConverter.ConsumedElement consumedElement = base.master.consumedElements[i];
				this.RemoveStatusItem<Tag>(consumedElement.Tag, base.master.consumedElementStatusHandles);
			}
			for (int j = 0; j < base.master.outputElements.Length; j++)
			{
				ElementConverter.OutputElement outputElement = base.master.outputElements[j];
				this.RemoveStatusItem<SimHashes>(outputElement.elementHash, base.master.outputElementStatusHandles);
			}
			base.master.consumedElementStatusHandles.Clear();
			base.master.outputElementStatusHandles.Clear();
		}

		// Token: 0x060095F7 RID: 38391 RVA: 0x003609B0 File Offset: 0x0035EBB0
		public void AddStatusItem<ElementType, IDType>(ElementType element, IDType id, StatusItem status, Dictionary<IDType, Guid> collection)
		{
			Guid value = this.selectable.AddStatusItem(status, element);
			collection[id] = value;
		}

		// Token: 0x060095F8 RID: 38392 RVA: 0x003609DC File Offset: 0x0035EBDC
		public void RemoveStatusItem<IDType>(IDType id, Dictionary<IDType, Guid> collection)
		{
			Guid guid;
			if (!collection.TryGetValue(id, out guid))
			{
				return;
			}
			this.selectable.RemoveStatusItem(guid, false);
		}

		// Token: 0x060095F9 RID: 38393 RVA: 0x00360A04 File Offset: 0x0035EC04
		public void OnOperationalRequirementChanged(object data)
		{
			Operational operational = data as Operational;
			bool value = (operational == null) ? ((bool)data) : operational.IsActive;
			base.sm.canConvert.Set(value, this, false);
		}

		// Token: 0x040072F1 RID: 29425
		private KSelectable selectable;
	}

	// Token: 0x02001781 RID: 6017
	public class States : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter>
	{
		// Token: 0x060095FA RID: 38394 RVA: 0x00360A44 File Offset: 0x0035EC44
		private bool ValidateStateTransition(ElementConverter.StatesInstance smi, bool _)
		{
			bool flag = smi.GetCurrentState() == smi.sm.disabled;
			if (smi.master.operational == null)
			{
				return flag;
			}
			bool flag2 = smi.master.consumedElements.Length == 0;
			bool flag3 = this.canConvert.Get(smi);
			int num = 0;
			while (!flag2 && num < smi.master.consumedElements.Length)
			{
				flag2 = smi.master.consumedElements[num].IsActive;
				num++;
			}
			if (flag3 && !flag2)
			{
				this.canConvert.Set(false, smi, true);
				return false;
			}
			return smi.master.operational.MeetsRequirements(smi.master.OperationalRequirement) == flag;
		}

		// Token: 0x060095FB RID: 38395 RVA: 0x00360B00 File Offset: 0x0035ED00
		private void OnEnterRoot(ElementConverter.StatesInstance smi)
		{
			int eventForState = (int)Operational.GetEventForState(smi.master.OperationalRequirement);
			smi.Subscribe(eventForState, new Action<object>(smi.OnOperationalRequirementChanged));
		}

		// Token: 0x060095FC RID: 38396 RVA: 0x00360B34 File Offset: 0x0035ED34
		private void OnExitRoot(ElementConverter.StatesInstance smi)
		{
			int eventForState = (int)Operational.GetEventForState(smi.master.OperationalRequirement);
			smi.Unsubscribe(eventForState, new Action<object>(smi.OnOperationalRequirementChanged));
		}

		// Token: 0x060095FD RID: 38397 RVA: 0x00360B68 File Offset: 0x0035ED68
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.Enter(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnEnterRoot)).Exit(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnExitRoot));
			this.disabled.ParamTransition<bool>(this.canConvert, this.converting, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition));
			this.converting.Enter("AddStatusItems", delegate(ElementConverter.StatesInstance smi)
			{
				smi.AddStatusItems();
			}).Exit("RemoveStatusItems", delegate(ElementConverter.StatesInstance smi)
			{
				smi.RemoveStatusItems();
			}).ParamTransition<bool>(this.canConvert, this.disabled, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition)).Update("ConvertMass", delegate(ElementConverter.StatesInstance smi, float dt)
			{
				smi.master.ConvertMass();
			}, UpdateRate.SIM_1000ms, true);
		}

		// Token: 0x040072F2 RID: 29426
		public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State disabled;

		// Token: 0x040072F3 RID: 29427
		public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State converting;

		// Token: 0x040072F4 RID: 29428
		public StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.BoolParameter canConvert;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008D1 RID: 2257
public class Geyser : StateMachineComponent<Geyser.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x0600402A RID: 16426 RVA: 0x0016B356 File Offset: 0x00169556
	// (set) Token: 0x06004029 RID: 16425 RVA: 0x0016B34D File Offset: 0x0016954D
	public float timeShift { get; private set; }

	// Token: 0x0600402B RID: 16427 RVA: 0x0016B35E File Offset: 0x0016955E
	public float GetCurrentLifeTime()
	{
		return GameClock.Instance.GetTime() + this.timeShift;
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x0016B374 File Offset: 0x00169574
	public void AlterTime(float timeOffset)
	{
		this.timeShift = Mathf.Max(timeOffset, -GameClock.Instance.GetTime());
		float num = this.RemainingEruptTime();
		float num2 = this.RemainingNonEruptTime();
		float num3 = this.RemainingActiveTime();
		float num4 = this.RemainingDormantTime();
		this.configuration.GetYearLength();
		if (num2 == 0f)
		{
			if ((num4 == 0f && this.configuration.GetYearOnDuration() - num3 < this.configuration.GetOnDuration() - num) | (num3 == 0f && this.configuration.GetYearOffDuration() - num4 >= this.configuration.GetOnDuration() - num))
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			base.smi.GoTo(base.smi.sm.erupt);
			return;
		}
		else
		{
			bool flag = (num4 == 0f && this.configuration.GetYearOnDuration() - num3 < this.configuration.GetIterationLength() - num2) | (num3 == 0f && this.configuration.GetYearOffDuration() - num4 >= this.configuration.GetIterationLength() - num2);
			float num5 = this.RemainingEruptPreTime();
			if (flag && num5 <= 0f)
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			if (num5 <= 0f)
			{
				base.smi.GoTo(base.smi.sm.idle);
				return;
			}
			float num6 = this.PreDuration() - num5;
			if ((num3 == 0f) ? (this.configuration.GetYearOffDuration() - num4 > num6) : (num6 > this.configuration.GetYearOnDuration() - num3))
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			base.smi.GoTo(base.smi.sm.pre_erupt);
			return;
		}
	}

	// Token: 0x0600402D RID: 16429 RVA: 0x0016B570 File Offset: 0x00169770
	public void ShiftTimeTo(Geyser.TimeShiftStep step)
	{
		float num = this.RemainingEruptTime();
		float num2 = this.RemainingNonEruptTime();
		float num3 = this.RemainingActiveTime();
		float num4 = this.RemainingDormantTime();
		float yearLength = this.configuration.GetYearLength();
		switch (step)
		{
		case Geyser.TimeShiftStep.ActiveState:
		{
			float num5 = (num3 > 0f) ? (this.configuration.GetYearOnDuration() - num3) : (yearLength - num4);
			this.AlterTime(this.timeShift - num5);
			return;
		}
		case Geyser.TimeShiftStep.DormantState:
		{
			float num6 = (num3 > 0f) ? num3 : (-(this.configuration.GetYearOffDuration() - num4));
			this.AlterTime(this.timeShift + num6);
			return;
		}
		case Geyser.TimeShiftStep.NextIteration:
		{
			float num7 = (num > 0f) ? (num + this.configuration.GetOffDuration()) : num2;
			this.AlterTime(this.timeShift + num7);
			return;
		}
		case Geyser.TimeShiftStep.PreviousIteration:
		{
			float num8 = (num > 0f) ? (-(this.configuration.GetOnDuration() - num)) : (-(this.configuration.GetIterationLength() - num2));
			if (num > 0f && Mathf.Abs(num8) < this.configuration.GetOnDuration() * 0.05f)
			{
				num8 -= this.configuration.GetIterationLength();
			}
			this.AlterTime(this.timeShift + num8);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x0600402E RID: 16430 RVA: 0x0016B6A8 File Offset: 0x001698A8
	public void AddModification(Geyser.GeyserModification modification)
	{
		this.modifications.Add(modification);
		this.UpdateModifier();
	}

	// Token: 0x0600402F RID: 16431 RVA: 0x0016B6BC File Offset: 0x001698BC
	public void RemoveModification(Geyser.GeyserModification modification)
	{
		this.modifications.Remove(modification);
		this.UpdateModifier();
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x0016B6D4 File Offset: 0x001698D4
	private void UpdateModifier()
	{
		this.modifier.Clear();
		foreach (Geyser.GeyserModification modification in this.modifications)
		{
			this.modifier.AddValues(modification);
		}
		this.configuration.SetModifier(this.modifier);
		this.ApplyConfigurationEmissionValues(this.configuration);
		this.RefreshGeotunerFeedback();
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x0016B75C File Offset: 0x0016995C
	public void RefreshGeotunerFeedback()
	{
		this.RefreshGeotunerStatusItem();
		this.RefreshStudiedMeter();
	}

	// Token: 0x06004032 RID: 16434 RVA: 0x0016B76C File Offset: 0x0016996C
	private void RefreshGeotunerStatusItem()
	{
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (this.GetAmountOfGeotunersPointingThisGeyser() > 0)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
			return;
		}
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
	}

	// Token: 0x06004033 RID: 16435 RVA: 0x0016B7C4 File Offset: 0x001699C4
	private void RefreshStudiedMeter()
	{
		if (this.studyable.Studied)
		{
			bool flag = this.GetAmountOfGeotunersPointingThisGeyser() > 0;
			GeyserConfig.TrackerMeterAnimNames trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.tracker;
			if (flag)
			{
				trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker;
				int amountOfGeotunersAffectingThisGeyser = this.GetAmountOfGeotunersAffectingThisGeyser();
				if (amountOfGeotunersAffectingThisGeyser > 0)
				{
					trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_minor;
				}
				if (amountOfGeotunersAffectingThisGeyser >= 5)
				{
					trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_major;
				}
			}
			this.studyable.studiedIndicator.meterController.Play(trackerMeterAnimNames.ToString(), KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06004034 RID: 16436 RVA: 0x0016B830 File Offset: 0x00169A30
	public int GetAmountOfGeotunersPointingThisGeyser()
	{
		return Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this);
	}

	// Token: 0x06004035 RID: 16437 RVA: 0x0016B858 File Offset: 0x00169A58
	public int GetAmountOfGeotunersPointingOrWillPointAtThisGeyser()
	{
		return Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this || x.GetFutureGeyser() == this);
	}

	// Token: 0x06004036 RID: 16438 RVA: 0x0016B880 File Offset: 0x00169A80
	public int GetAmountOfGeotunersAffectingThisGeyser()
	{
		int num = 0;
		for (int i = 0; i < this.modifications.Count; i++)
		{
			if (this.modifications[i].originID.Contains("GeoTuner"))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06004037 RID: 16439 RVA: 0x0016B8C7 File Offset: 0x00169AC7
	private void OnGeotunerChanged(object o)
	{
		this.RefreshGeotunerFeedback();
	}

	// Token: 0x06004038 RID: 16440 RVA: 0x0016B8D0 File Offset: 0x00169AD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		if (this.configuration == null || this.configuration.typeId == HashedString.Invalid)
		{
			this.configuration = base.GetComponent<GeyserConfigurator>().MakeConfiguration();
		}
		else
		{
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			if (this.configuration.geyserType.geyserTemperature - component.Temperature != 0f)
			{
				SimTemperatureTransfer component2 = base.gameObject.GetComponent<SimTemperatureTransfer>();
				component2.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Combine(component2.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnSimRegistered));
			}
		}
		this.ApplyConfigurationEmissionValues(this.configuration);
		this.GenerateName();
		base.smi.StartSM();
		Workable component3 = base.GetComponent<Studyable>();
		if (component3 != null)
		{
			component3.alwaysShowProgressBar = true;
		}
		Components.Geysers.Add(base.gameObject.GetMyWorldId(), this);
		base.gameObject.Subscribe(1763323737, new Action<object>(this.OnGeotunerChanged));
		this.RefreshStudiedMeter();
		this.UpdateModifier();
	}

	// Token: 0x06004039 RID: 16441 RVA: 0x0016B9EC File Offset: 0x00169BEC
	private void GenerateName()
	{
		StringKey key = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + this.configuration.geyserType.id.ToUpper() + ".NAME");
		if (this.nameable.savedName == Strings.Get(key))
		{
			int cell = Grid.PosToCell(base.gameObject);
			Quadrant[] quadrantOfCell = base.gameObject.GetMyWorld().GetQuadrantOfCell(cell, 2);
			int num = (int)quadrantOfCell[0];
			string str = num.ToString();
			num = (int)quadrantOfCell[1];
			string text = str + num.ToString();
			string[] array = NAMEGEN.GEYSER_IDS.IDs.ToString().Split('\n', StringSplitOptions.None);
			string text2 = array[UnityEngine.Random.Range(0, array.Length)];
			string name = string.Concat(new string[]
			{
				UI.StripLinkFormatting(base.gameObject.GetProperName()),
				" ",
				text2,
				text,
				"‑",
				UnityEngine.Random.Range(0, 10).ToString()
			});
			this.nameable.SetName(name);
		}
	}

	// Token: 0x0600403A RID: 16442 RVA: 0x0016BB00 File Offset: 0x00169D00
	public void ApplyConfigurationEmissionValues(GeyserConfigurator.GeyserInstanceConfiguration config)
	{
		this.emitter.emitRange = 2;
		this.emitter.maxPressure = config.GetMaxPressure();
		this.emitter.outputElement = new ElementConverter.OutputElement(config.GetEmitRate(), config.GetElement(), config.GetTemperature(), false, false, (float)this.outputOffset.x, (float)this.outputOffset.y, 1f, config.GetDiseaseIdx(), Mathf.RoundToInt((float)config.GetDiseaseCount() * config.GetEmitRate()), true);
		if (this.emitter.IsSimActive)
		{
			this.emitter.SetSimActive(true);
		}
	}

	// Token: 0x0600403B RID: 16443 RVA: 0x0016BB9E File Offset: 0x00169D9E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.gameObject.Unsubscribe(1763323737, new Action<object>(this.OnGeotunerChanged));
		Components.Geysers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x0600403C RID: 16444 RVA: 0x0016BBD8 File Offset: 0x00169DD8
	private void OnSimRegistered(SimTemperatureTransfer stt)
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		if (this.configuration.geyserType.geyserTemperature - component.Temperature != 0f)
		{
			component.Temperature = this.configuration.geyserType.geyserTemperature;
		}
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnSimRegistered));
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x0016BC48 File Offset: 0x00169E48
	public float RemainingPhaseTimeFrom2(float onDuration, float offDuration, float time, Geyser.Phase expectedPhase)
	{
		float num = onDuration + offDuration;
		float num2 = time % num;
		float result;
		Geyser.Phase phase;
		if (num2 < onDuration)
		{
			result = Mathf.Max(onDuration - num2, 0f);
			phase = Geyser.Phase.On;
		}
		else
		{
			result = Mathf.Max(onDuration + offDuration - num2, 0f);
			phase = Geyser.Phase.Off;
		}
		if (expectedPhase != Geyser.Phase.Any && phase != expectedPhase)
		{
			return 0f;
		}
		return result;
	}

	// Token: 0x0600403E RID: 16446 RVA: 0x0016BC98 File Offset: 0x00169E98
	public float RemainingPhaseTimeFrom4(float onDuration, float pstDuration, float offDuration, float preDuration, float time, Geyser.Phase expectedPhase)
	{
		float num = onDuration + pstDuration + offDuration + preDuration;
		float num2 = time % num;
		float result;
		Geyser.Phase phase;
		if (num2 < onDuration)
		{
			result = onDuration - num2;
			phase = Geyser.Phase.On;
		}
		else if (num2 < onDuration + pstDuration)
		{
			result = onDuration + pstDuration - num2;
			phase = Geyser.Phase.Pst;
		}
		else if (num2 < onDuration + pstDuration + offDuration)
		{
			result = onDuration + pstDuration + offDuration - num2;
			phase = Geyser.Phase.Off;
		}
		else
		{
			result = onDuration + pstDuration + offDuration + preDuration - num2;
			phase = Geyser.Phase.Pre;
		}
		if (expectedPhase != Geyser.Phase.Any && phase != expectedPhase)
		{
			return 0f;
		}
		return result;
	}

	// Token: 0x0600403F RID: 16447 RVA: 0x0016BD01 File Offset: 0x00169F01
	private float IdleDuration()
	{
		return this.configuration.GetOffDuration() * 0.84999996f;
	}

	// Token: 0x06004040 RID: 16448 RVA: 0x0016BD14 File Offset: 0x00169F14
	private float PreDuration()
	{
		return this.configuration.GetOffDuration() * 0.1f;
	}

	// Token: 0x06004041 RID: 16449 RVA: 0x0016BD27 File Offset: 0x00169F27
	private float PostDuration()
	{
		return this.configuration.GetOffDuration() * 0.05f;
	}

	// Token: 0x06004042 RID: 16450 RVA: 0x0016BD3A File Offset: 0x00169F3A
	private float EruptDuration()
	{
		return this.configuration.GetOnDuration();
	}

	// Token: 0x06004043 RID: 16451 RVA: 0x0016BD47 File Offset: 0x00169F47
	public bool ShouldGoDormant()
	{
		return this.RemainingActiveTime() <= 0f;
	}

	// Token: 0x06004044 RID: 16452 RVA: 0x0016BD59 File Offset: 0x00169F59
	public float RemainingIdleTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x06004045 RID: 16453 RVA: 0x0016BD80 File Offset: 0x00169F80
	public float RemainingEruptPreTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Pre);
	}

	// Token: 0x06004046 RID: 16454 RVA: 0x0016BDA7 File Offset: 0x00169FA7
	public float RemainingEruptTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetOnDuration(), this.configuration.GetOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.On);
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x0016BDCC File Offset: 0x00169FCC
	public float RemainingEruptPostTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Pst);
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x0016BDF3 File Offset: 0x00169FF3
	public float RemainingNonEruptTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetOnDuration(), this.configuration.GetOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x0016BE18 File Offset: 0x0016A018
	public float RemainingDormantTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetYearOnDuration(), this.configuration.GetYearOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x0600404A RID: 16458 RVA: 0x0016BE3D File Offset: 0x0016A03D
	public float RemainingActiveTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetYearOnDuration(), this.configuration.GetYearOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.On);
	}

	// Token: 0x0600404B RID: 16459 RVA: 0x0016BE64 File Offset: 0x0016A064
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.configuration.GetElement()).tag.ProperName();
		List<GeoTuner.Instance> items = Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId());
		GeoTuner.Instance instance = items.Find((GeoTuner.Instance g) => g.GetAssignedGeyser() == this);
		int num = items.Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this);
		bool flag = num > 0;
		string text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION, ElementLoader.FindElementByHash(this.configuration.GetElement()).name, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		if (flag)
		{
			Func<float, float> func = delegate(float emissionPerCycleModifier)
			{
				float num8 = 600f / this.configuration.GetIterationLength();
				return emissionPerCycleModifier / num8 / this.configuration.GetOnDuration();
			};
			int amountOfGeotunersAffectingThisGeyser = this.GetAmountOfGeotunersAffectingThisGeyser();
			float num2 = (Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.temperatureModifier * this.configuration.geyserType.temperature) : instance.currentGeyserModification.temperatureModifier;
			float num3 = func((Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * this.configuration.scaledRate) : instance.currentGeyserModification.massPerCycleModifier);
			float num4 = (float)amountOfGeotunersAffectingThisGeyser * num2;
			float num5 = (float)amountOfGeotunersAffectingThisGeyser * num3;
			string arg2 = ((num4 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num4, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
			string arg3 = ((num5 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num5, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
			string str = ((num2 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
			string str2 = ((num3 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num3, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
			text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED, ElementLoader.FindElementByHash(this.configuration.GetElement()).name, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			text += "\n";
			text = text + "\n" + string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_COUNT, amountOfGeotunersAffectingThisGeyser.ToString(), num.ToString());
			text += "\n";
			text = text + "\n" + string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_TOTAL, arg3, arg2);
			for (int i = 0; i < amountOfGeotunersAffectingThisGeyser; i++)
			{
				string text2 = "\n    • " + UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE.ToString();
				text2 = text2 + str2 + " " + str;
				text += text2;
			}
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PRODUCTION, arg, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), text, Descriptor.DescriptorType.Effect, false));
		if (this.configuration.GetDiseaseIdx() != 255)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_DISEASE, GameUtil.GetFormattedDiseaseName(this.configuration.GetDiseaseIdx(), false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_DISEASE, GameUtil.GetFormattedDiseaseName(this.configuration.GetDiseaseIdx(), false)), Descriptor.DescriptorType.Effect, false));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PERIOD, GameUtil.GetFormattedTime(this.configuration.GetOnDuration(), "F0"), GameUtil.GetFormattedTime(this.configuration.GetIterationLength(), "F0")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PERIOD, GameUtil.GetFormattedTime(this.configuration.GetOnDuration(), "F0"), GameUtil.GetFormattedTime(this.configuration.GetIterationLength(), "F0")), Descriptor.DescriptorType.Effect, false));
		Studyable component = base.GetComponent<Studyable>();
		if (component && !component.Studied)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_UNSTUDIED, Array.Empty<object>()), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_UNSTUDIED, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED, Array.Empty<object>()), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
		}
		else
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_PERIOD, GameUtil.GetFormattedCycles(this.configuration.GetYearOnDuration(), "F1", false), GameUtil.GetFormattedCycles(this.configuration.GetYearLength(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_PERIOD, GameUtil.GetFormattedCycles(this.configuration.GetYearOnDuration(), "F1", false), GameUtil.GetFormattedCycles(this.configuration.GetYearLength(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			if (base.smi.IsInsideState(base.smi.sm.dormant))
			{
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_ACTIVE, GameUtil.GetFormattedCycles(this.RemainingDormantTime(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_ACTIVE, GameUtil.GetFormattedCycles(this.RemainingDormantTime(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_DORMANT, GameUtil.GetFormattedCycles(this.RemainingActiveTime(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_DORMANT, GameUtil.GetFormattedCycles(this.RemainingActiveTime(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			string text3 = UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT.Replace("{average}", GameUtil.GetFormattedMass(this.configuration.GetAverageEmission(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{element}", this.configuration.geyserType.element.CreateTag().ProperName());
			if (flag)
			{
				text3 += "\n";
				text3 = text3 + "\n" + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE;
				int amountOfGeotunersAffectingThisGeyser2 = this.GetAmountOfGeotunersAffectingThisGeyser();
				float num6 = (Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * 100f) : (instance.currentGeyserModification.massPerCycleModifier * 100f / this.configuration.scaledRate);
				float num7 = num6 * (float)amountOfGeotunersAffectingThisGeyser2;
				text3 = text3 + GameUtil.AddPositiveSign(num7.ToString("0.0"), num7 > 0f) + "%";
				for (int j = 0; j < amountOfGeotunersAffectingThisGeyser2; j++)
				{
					string text4 = "\n    • " + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW.ToString();
					text4 = text4 + GameUtil.AddPositiveSign(num6.ToString("0.0"), num6 > 0f) + "%";
					text3 += text4;
				}
			}
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT, GameUtil.GetFormattedMass(this.configuration.GetAverageEmission(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), text3, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x04002A66 RID: 10854
	public static Geyser.ModificationMethod massModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002A67 RID: 10855
	public static Geyser.ModificationMethod temperatureModificationMethod = Geyser.ModificationMethod.Values;

	// Token: 0x04002A68 RID: 10856
	public static Geyser.ModificationMethod IterationDurationModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002A69 RID: 10857
	public static Geyser.ModificationMethod IterationPercentageModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002A6A RID: 10858
	public static Geyser.ModificationMethod yearDurationModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002A6B RID: 10859
	public static Geyser.ModificationMethod yearPercentageModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002A6C RID: 10860
	public static Geyser.ModificationMethod maxPressureModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002A6D RID: 10861
	[MyCmpAdd]
	private ElementEmitter emitter;

	// Token: 0x04002A6E RID: 10862
	[MyCmpAdd]
	private UserNameable nameable;

	// Token: 0x04002A6F RID: 10863
	[MyCmpGet]
	private Studyable studyable;

	// Token: 0x04002A70 RID: 10864
	[Serialize]
	public GeyserConfigurator.GeyserInstanceConfiguration configuration;

	// Token: 0x04002A71 RID: 10865
	public Vector2I outputOffset;

	// Token: 0x04002A72 RID: 10866
	public List<Geyser.GeyserModification> modifications = new List<Geyser.GeyserModification>();

	// Token: 0x04002A73 RID: 10867
	private Geyser.GeyserModification modifier;

	// Token: 0x04002A75 RID: 10869
	private const float PRE_PCT = 0.1f;

	// Token: 0x04002A76 RID: 10870
	private const float POST_PCT = 0.05f;

	// Token: 0x02001808 RID: 6152
	public enum ModificationMethod
	{
		// Token: 0x040074BE RID: 29886
		Values,
		// Token: 0x040074BF RID: 29887
		Percentages
	}

	// Token: 0x02001809 RID: 6153
	public struct GeyserModification
	{
		// Token: 0x0600973F RID: 38719 RVA: 0x00364C8C File Offset: 0x00362E8C
		public void Clear()
		{
			this.massPerCycleModifier = 0f;
			this.temperatureModifier = 0f;
			this.iterationDurationModifier = 0f;
			this.iterationPercentageModifier = 0f;
			this.yearDurationModifier = 0f;
			this.yearPercentageModifier = 0f;
			this.maxPressureModifier = 0f;
			this.modifyElement = false;
			this.newElement = (SimHashes)0;
		}

		// Token: 0x06009740 RID: 38720 RVA: 0x00364CF4 File Offset: 0x00362EF4
		public void AddValues(Geyser.GeyserModification modification)
		{
			this.massPerCycleModifier += modification.massPerCycleModifier;
			this.temperatureModifier += modification.temperatureModifier;
			this.iterationDurationModifier += modification.iterationDurationModifier;
			this.iterationPercentageModifier += modification.iterationPercentageModifier;
			this.yearDurationModifier += modification.yearDurationModifier;
			this.yearPercentageModifier += modification.yearPercentageModifier;
			this.maxPressureModifier += modification.maxPressureModifier;
			this.modifyElement |= modification.modifyElement;
			this.newElement = ((modification.newElement == (SimHashes)0) ? this.newElement : modification.newElement);
		}

		// Token: 0x06009741 RID: 38721 RVA: 0x00364DB5 File Offset: 0x00362FB5
		public bool IsNewElementInUse()
		{
			return this.modifyElement && this.newElement > (SimHashes)0;
		}

		// Token: 0x040074C0 RID: 29888
		public string originID;

		// Token: 0x040074C1 RID: 29889
		public float massPerCycleModifier;

		// Token: 0x040074C2 RID: 29890
		public float temperatureModifier;

		// Token: 0x040074C3 RID: 29891
		public float iterationDurationModifier;

		// Token: 0x040074C4 RID: 29892
		public float iterationPercentageModifier;

		// Token: 0x040074C5 RID: 29893
		public float yearDurationModifier;

		// Token: 0x040074C6 RID: 29894
		public float yearPercentageModifier;

		// Token: 0x040074C7 RID: 29895
		public float maxPressureModifier;

		// Token: 0x040074C8 RID: 29896
		public bool modifyElement;

		// Token: 0x040074C9 RID: 29897
		public SimHashes newElement;
	}

	// Token: 0x0200180A RID: 6154
	public class StatesInstance : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.GameInstance
	{
		// Token: 0x06009742 RID: 38722 RVA: 0x00364DCA File Offset: 0x00362FCA
		public StatesInstance(Geyser smi) : base(smi)
		{
		}
	}

	// Token: 0x0200180B RID: 6155
	public class States : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser>
	{
		// Token: 0x06009743 RID: 38723 RVA: 0x00364DD4 File Offset: 0x00362FD4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.DefaultState(this.idle).Enter(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
			this.dormant.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutDormant, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingDormantTime(), this.pre_erupt);
			this.idle.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle, null).Enter(delegate(Geyser.StatesInstance smi)
			{
				if (smi.master.ShouldGoDormant())
				{
					smi.GoTo(this.dormant);
				}
			}).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingIdleTime(), this.pre_erupt);
			this.pre_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptPreTime(), this.erupt);
			this.erupt.TriggerOnEnter(GameHashes.GeyserEruption, (Geyser.StatesInstance smi) => true).TriggerOnExit(GameHashes.GeyserEruption, (Geyser.StatesInstance smi) => false).DefaultState(this.erupt.erupting).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptTime(), this.post_erupt).Enter(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(true);
			}).Exit(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
			this.erupt.erupting.EventTransition(GameHashes.EmitterBlocked, this.erupt.overpressure, (Geyser.StatesInstance smi) => smi.GetComponent<ElementEmitter>().isEmitterBlocked).PlayAnim("erupt", KAnim.PlayMode.Loop);
			this.erupt.overpressure.EventTransition(GameHashes.EmitterUnblocked, this.erupt.erupting, (Geyser.StatesInstance smi) => !smi.GetComponent<ElementEmitter>().isEmitterBlocked).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).PlayAnim("inactive", KAnim.PlayMode.Loop);
			this.post_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptPostTime(), this.idle);
		}

		// Token: 0x040074CA RID: 29898
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State dormant;

		// Token: 0x040074CB RID: 29899
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State idle;

		// Token: 0x040074CC RID: 29900
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State pre_erupt;

		// Token: 0x040074CD RID: 29901
		public Geyser.States.EruptState erupt;

		// Token: 0x040074CE RID: 29902
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State post_erupt;

		// Token: 0x0200259D RID: 9629
		public class EruptState : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State
		{
			// Token: 0x0400A780 RID: 42880
			public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State erupting;

			// Token: 0x0400A781 RID: 42881
			public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State overpressure;
		}
	}

	// Token: 0x0200180C RID: 6156
	public enum TimeShiftStep
	{
		// Token: 0x040074D0 RID: 29904
		ActiveState,
		// Token: 0x040074D1 RID: 29905
		DormantState,
		// Token: 0x040074D2 RID: 29906
		NextIteration,
		// Token: 0x040074D3 RID: 29907
		PreviousIteration
	}

	// Token: 0x0200180D RID: 6157
	public enum Phase
	{
		// Token: 0x040074D5 RID: 29909
		Pre,
		// Token: 0x040074D6 RID: 29910
		On,
		// Token: 0x040074D7 RID: 29911
		Pst,
		// Token: 0x040074D8 RID: 29912
		Off,
		// Token: 0x040074D9 RID: 29913
		Any
	}
}

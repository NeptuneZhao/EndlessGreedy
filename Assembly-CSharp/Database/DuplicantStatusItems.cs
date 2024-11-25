using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E5F RID: 3679
	public class DuplicantStatusItems : StatusItems
	{
		// Token: 0x06007477 RID: 29815 RVA: 0x002D2753 File Offset: 0x002D0953
		public DuplicantStatusItems(ResourceSet parent) : base("DuplicantStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x06007478 RID: 29816 RVA: 0x002D2768 File Offset: 0x002D0968
		private StatusItem CreateStatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 2)
		{
			return base.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, null));
		}

		// Token: 0x06007479 RID: 29817 RVA: 0x002D2790 File Offset: 0x002D0990
		private StatusItem CreateStatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 2)
		{
			return base.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null));
		}

		// Token: 0x0600747A RID: 29818 RVA: 0x002D27BC File Offset: 0x002D09BC
		private void CreateStatusItems()
		{
			Func<string, object, string> resolveStringCallback = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null && workable.GetComponent<KSelectable>() != null)
				{
					str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
				}
				return str;
			};
			Func<string, object, string> resolveStringCallback2 = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null)
				{
					str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
					ComplexFabricatorWorkable complexFabricatorWorkable = workable as ComplexFabricatorWorkable;
					if (complexFabricatorWorkable != null)
					{
						ComplexRecipe currentWorkingOrder = complexFabricatorWorkable.CurrentWorkingOrder;
						if (currentWorkingOrder != null)
						{
							str = str.Replace("{Item}", currentWorkingOrder.FirstResult.ProperName());
						}
					}
				}
				return str;
			};
			this.BedUnreachable = this.CreateStatusItem("BedUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.BedUnreachable.AddNotification(null, null, null);
			this.DailyRationLimitReached = this.CreateStatusItem("DailyRationLimitReached", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.DailyRationLimitReached.AddNotification(null, null, null);
			this.HoldingBreath = this.CreateStatusItem("HoldingBreath", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Hungry = this.CreateStatusItem("Hungry", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Slippering = this.CreateStatusItem("Slippering", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Unhappy = this.CreateStatusItem("Unhappy", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Unhappy.AddNotification(null, null, null);
			this.NervousBreakdown = this.CreateStatusItem("NervousBreakdown", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.NervousBreakdown.AddNotification(null, null, null);
			this.NoRationsAvailable = this.CreateStatusItem("NoRationsAvailable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.PendingPacification = this.CreateStatusItem("PendingPacification", "DUPLICANTS", "status_item_pending_pacification", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnassigned = this.CreateStatusItem("QuarantineAreaUnassigned", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnassigned.AddNotification(null, null, null);
			this.QuarantineAreaUnreachable = this.CreateStatusItem("QuarantineAreaUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnreachable.AddNotification(null, null, null);
			this.Quarantined = this.CreateStatusItem("Quarantined", "DUPLICANTS", "status_item_quarantined", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RationsUnreachable = this.CreateStatusItem("RationsUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.RationsUnreachable.AddNotification(null, null, null);
			this.RationsNotPermitted = this.CreateStatusItem("RationsNotPermitted", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.RationsNotPermitted.AddNotification(null, null, null);
			this.Rotten = this.CreateStatusItem("Rotten", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Starving = this.CreateStatusItem("Starving", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Starving.AddNotification(null, null, null);
			this.Suffocating = this.CreateStatusItem("Suffocating", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.Suffocating.AddNotification(null, null, null);
			this.Tired = this.CreateStatusItem("Tired", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Idle = this.CreateStatusItem("Idle", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.IdleInRockets = this.CreateStatusItem("IdleInRockets", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Pacified = this.CreateStatusItem("Pacified", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Dead = this.CreateStatusItem("Dead", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Dead.resolveStringCallback = delegate(string str, object data)
			{
				Death death = (Death)data;
				return str.Replace("{Death}", death.Name);
			};
			this.MoveToSuitNotRequired = this.CreateStatusItem("MoveToSuitNotRequired", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.DroppingUnusedInventory = this.CreateStatusItem("DroppingUnusedInventory", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.MovingToSafeArea = this.CreateStatusItem("MovingToSafeArea", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ToiletUnreachable = this.CreateStatusItem("ToiletUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.ToiletUnreachable.AddNotification(null, null, null);
			this.NoUsableToilets = this.CreateStatusItem("NoUsableToilets", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.NoUsableToilets.AddNotification(null, null, null);
			this.NoToilets = this.CreateStatusItem("NoToilets", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.NoToilets.AddNotification(null, null, null);
			this.BreathingO2 = this.CreateStatusItem("BreathingO2", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
			this.BreathingO2.resolveStringCallback = delegate(string str, object data)
			{
				OxygenBreather oxygenBreather = (OxygenBreather)data;
				float averageRate = Game.Instance.accumulators.GetAverageRate(oxygenBreather.O2Accumulator);
				return str.Replace("{ConsumptionRate}", GameUtil.GetFormattedMass(-averageRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.EmittingCO2 = this.CreateStatusItem("EmittingCO2", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
			this.EmittingCO2.resolveStringCallback = delegate(string str, object data)
			{
				OxygenBreather oxygenBreather = (OxygenBreather)data;
				return str.Replace("{EmittingRate}", GameUtil.GetFormattedMass(oxygenBreather.CO2EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.Vomiting = this.CreateStatusItem("Vomiting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Coughing = this.CreateStatusItem("Coughing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowOxygen = this.CreateStatusItem("LowOxygen", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowOxygen.AddNotification(null, null, null);
			this.RedAlert = this.CreateStatusItem("RedAlert", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Dreaming = this.CreateStatusItem("Dreaming", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Sleeping = this.CreateStatusItem("Sleeping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Sleeping.resolveTooltipCallback = delegate(string str, object data)
			{
				if (data is SleepChore.StatesInstance)
				{
					string stateChangeNoiseSource = ((SleepChore.StatesInstance)data).stateChangeNoiseSource;
					if (!string.IsNullOrEmpty(stateChangeNoiseSource))
					{
						string text = DUPLICANTS.STATUSITEMS.SLEEPING.TOOLTIP;
						text = text.Replace("{Disturber}", stateChangeNoiseSource);
						str += text;
					}
				}
				return str;
			};
			this.SleepingExhausted = this.CreateStatusItem("SleepingExhausted", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByNoise = this.CreateStatusItem("SleepingInterruptedByNoise", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByLight = this.CreateStatusItem("SleepingInterruptedByLight", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByFearOfDark = this.CreateStatusItem("SleepingInterruptedByFearOfDark", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByMovement = this.CreateStatusItem("SleepingInterruptedByMovement", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByCold = this.CreateStatusItem("SleepingInterruptedByCold", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Eating = this.CreateStatusItem("Eating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Eating.resolveStringCallback = resolveStringCallback;
			this.Digging = this.CreateStatusItem("Digging", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cleaning = this.CreateStatusItem("Cleaning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cleaning.resolveStringCallback = resolveStringCallback;
			this.PickingUp = this.CreateStatusItem("PickingUp", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PickingUp.resolveStringCallback = resolveStringCallback;
			this.Mopping = this.CreateStatusItem("Mopping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cooking = this.CreateStatusItem("Cooking", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cooking.resolveStringCallback = resolveStringCallback2;
			this.Mushing = this.CreateStatusItem("Mushing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Mushing.resolveStringCallback = resolveStringCallback2;
			this.Researching = this.CreateStatusItem("Researching", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Researching.resolveStringCallback = delegate(string str, object data)
			{
				TechInstance activeResearch = Research.Instance.GetActiveResearch();
				if (activeResearch != null)
				{
					return str.Replace("{Tech}", activeResearch.tech.Name);
				}
				return str;
			};
			this.ResearchingFromPOI = this.CreateStatusItem("ResearchingFromPOI", DUPLICANTS.STATUSITEMS.RESEARCHING_FROM_POI.NAME, DUPLICANTS.STATUSITEMS.RESEARCHING_FROM_POI.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.MissionControlling = this.CreateStatusItem("MissionControlling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Tinkering = this.CreateStatusItem("Tinkering", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Tinkering.resolveStringCallback = delegate(string str, object data)
			{
				Tinkerable tinkerable = (Tinkerable)data;
				if (tinkerable != null)
				{
					return string.Format(str, tinkerable.tinkerMaterialTag.ProperName());
				}
				return str;
			};
			this.Storing = this.CreateStatusItem("Storing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Storing.resolveStringCallback = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null && workable.worker as StandardWorker != null)
				{
					KSelectable component = workable.GetComponent<KSelectable>();
					if (component)
					{
						str = str.Replace("{Target}", component.GetName());
					}
					Pickupable pickupable = (workable.worker as StandardWorker).workCompleteData as Pickupable;
					if (workable.worker != null && pickupable)
					{
						KSelectable component2 = pickupable.GetComponent<KSelectable>();
						if (component2)
						{
							str = str.Replace("{Item}", component2.GetName());
						}
					}
				}
				return str;
			};
			this.Building = this.CreateStatusItem("Building", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Building.resolveStringCallback = resolveStringCallback;
			this.Equipping = this.CreateStatusItem("Equipping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Equipping.resolveStringCallback = resolveStringCallback;
			this.WarmingUp = this.CreateStatusItem("WarmingUp", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WarmingUp.resolveStringCallback = resolveStringCallback;
			this.GeneratingPower = this.CreateStatusItem("GeneratingPower", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GeneratingPower.resolveStringCallback = resolveStringCallback;
			this.Harvesting = this.CreateStatusItem("Harvesting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Ranching = this.CreateStatusItem("Ranching", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Harvesting.resolveStringCallback = resolveStringCallback;
			this.Uprooting = this.CreateStatusItem("Uprooting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Uprooting.resolveStringCallback = resolveStringCallback;
			this.Emptying = this.CreateStatusItem("Emptying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Emptying.resolveStringCallback = resolveStringCallback;
			this.Toggling = this.CreateStatusItem("Toggling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Toggling.resolveStringCallback = resolveStringCallback;
			this.Deconstructing = this.CreateStatusItem("Deconstructing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Deconstructing.resolveStringCallback = resolveStringCallback;
			this.Disinfecting = this.CreateStatusItem("Disinfecting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Disinfecting.resolveStringCallback = resolveStringCallback;
			this.Upgrading = this.CreateStatusItem("Upgrading", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Upgrading.resolveStringCallback = resolveStringCallback;
			this.Fabricating = this.CreateStatusItem("Fabricating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Fabricating.resolveStringCallback = resolveStringCallback2;
			this.Processing = this.CreateStatusItem("Processing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Processing.resolveStringCallback = resolveStringCallback2;
			this.Spicing = this.CreateStatusItem("Spicing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Clearing = this.CreateStatusItem("Clearing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Clearing.resolveStringCallback = resolveStringCallback;
			this.GeneratingPower = this.CreateStatusItem("GeneratingPower", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GeneratingPower.resolveStringCallback = resolveStringCallback;
			this.Cold = this.CreateStatusItem("Cold", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Cold.resolveTooltipCallback = delegate(string str, object data)
			{
				ExternalTemperatureMonitor.Instance smi = ((ColdImmunityMonitor.Instance)data).GetSMI<ExternalTemperatureMonitor.Instance>();
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("ColdAir").SelfModifiers[2].Value.ToString());
				float dtu_s = smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage * 1000f;
				str = str.Replace("{currentTransferWattage}", GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
				AttributeInstance attributeInstance = smi.attributes.Get("ThermalConductivityBarrier");
				string text = "<b>" + attributeInstance.GetFormattedValue() + "</b>";
				for (int num = 0; num != attributeInstance.Modifiers.Count; num++)
				{
					AttributeModifier attributeModifier = attributeInstance.Modifiers[num];
					text += "\n";
					text = string.Concat(new string[]
					{
						text,
						"    • ",
						attributeModifier.GetDescription(),
						" <b>",
						attributeModifier.GetFormattedString(),
						"</b>"
					});
				}
				str = str.Replace("{conductivityBarrier}", text);
				return str;
			};
			this.ExitingCold = this.CreateStatusItem("ExitingCold", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ExitingCold.resolveTooltipCallback = delegate(string str, object data)
			{
				ColdImmunityMonitor.Instance instance = (ColdImmunityMonitor.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedTime(instance.ColdCountdown, "F0"));
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("ColdAir").SelfModifiers[2].Value.ToString());
				return str;
			};
			this.Hot = this.CreateStatusItem("Hot", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Hot.resolveTooltipCallback = delegate(string str, object data)
			{
				ExternalTemperatureMonitor.Instance smi = ((HeatImmunityMonitor.Instance)data).GetSMI<ExternalTemperatureMonitor.Instance>();
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("WarmAir").SelfModifiers[2].Value.ToString());
				float dtu_s = smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage * 1000f;
				str = str.Replace("{currentTransferWattage}", GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
				AttributeInstance attributeInstance = smi.attributes.Get("ThermalConductivityBarrier");
				string text = "<b>" + attributeInstance.GetFormattedValue() + "</b>";
				for (int num = 0; num != attributeInstance.Modifiers.Count; num++)
				{
					AttributeModifier attributeModifier = attributeInstance.Modifiers[num];
					text += "\n";
					text = string.Concat(new string[]
					{
						text,
						"    • ",
						attributeModifier.GetDescription(),
						" <b>",
						attributeModifier.GetFormattedString(),
						"</b>"
					});
				}
				str = str.Replace("{conductivityBarrier}", text);
				return str;
			};
			this.ExitingHot = this.CreateStatusItem("ExitingHot", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ExitingHot.resolveTooltipCallback = delegate(string str, object data)
			{
				HeatImmunityMonitor.Instance instance = (HeatImmunityMonitor.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedTime(instance.HeatCountdown, "F0"));
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("WarmAir").SelfModifiers[2].Value.ToString());
				return str;
			};
			this.BodyRegulatingHeating = this.CreateStatusItem("BodyRegulatingHeating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BodyRegulatingHeating.resolveStringCallback = delegate(string str, object data)
			{
				WarmBlooded.StatesInstance statesInstance = (WarmBlooded.StatesInstance)data;
				return str.Replace("{TempDelta}", GameUtil.GetFormattedTemperature(statesInstance.TemperatureDelta, GameUtil.TimeSlice.PerSecond, GameUtil.TemperatureInterpretation.Relative, true, false));
			};
			this.BodyRegulatingCooling = this.CreateStatusItem("BodyRegulatingCooling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BodyRegulatingCooling.resolveStringCallback = this.BodyRegulatingHeating.resolveStringCallback;
			this.EntombedChore = this.CreateStatusItem("EntombedChore", "DUPLICANTS", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.EntombedChore.AddNotification(null, null, null);
			this.EarlyMorning = this.CreateStatusItem("EarlyMorning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.NightTime = this.CreateStatusItem("NightTime", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorDecor = this.CreateStatusItem("PoorDecor", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorQualityOfLife = this.CreateStatusItem("PoorQualityOfLife", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorFoodQuality = this.CreateStatusItem("PoorFoodQuality", DUPLICANTS.STATUSITEMS.POOR_FOOD_QUALITY.NAME, DUPLICANTS.STATUSITEMS.POOR_FOOD_QUALITY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.GoodFoodQuality = this.CreateStatusItem("GoodFoodQuality", DUPLICANTS.STATUSITEMS.GOOD_FOOD_QUALITY.NAME, DUPLICANTS.STATUSITEMS.GOOD_FOOD_QUALITY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.Arting = this.CreateStatusItem("Arting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Arting.resolveStringCallback = resolveStringCallback;
			this.SevereWounds = this.CreateStatusItem("SevereWounds", "DUPLICANTS", "status_item_broken", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.SevereWounds.AddNotification(null, null, null);
			this.BionicOfflineIncapacitated = this.CreateStatusItem("BionicOfflineIncapacitated", "DUPLICANTS", "status_electrobank", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.BionicOfflineIncapacitated.AddNotification(null, null, null);
			this.BionicWantsOilChange = this.CreateStatusItem("BionicWantsOilChange", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicWaitingForReboot = this.CreateStatusItem("BionicWaitingForReboot", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicBeingRebooted = this.CreateStatusItem("BionicBeingRebooted", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicRequiresSkillPerk = this.CreateStatusItem("BionicRequiresSkillPerk", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicRequiresSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				string id = (string)data;
				SkillPerk perk = Db.Get().SkillPerks.Get(id);
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(perk);
				List<string> list = new List<string>();
				foreach (Skill skill in skillsWithPerk)
				{
					if (!skill.deprecated)
					{
						list.Add(skill.Name);
					}
				}
				str = str.Replace("{Skills}", string.Join(", ", list.ToArray()));
				return str;
			};
			this.Incapacitated = this.CreateStatusItem("Incapacitated", "DUPLICANTS", "status_item_broken", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.Incapacitated.AddNotification(null, null, null);
			this.Incapacitated.resolveStringCallback = delegate(string str, object data)
			{
				IncapacitationMonitor.Instance instance = (IncapacitationMonitor.Instance)data;
				float bleedLifeTime = instance.GetBleedLifeTime(instance);
				str = str.Replace("{CauseOfIncapacitation}", instance.GetCauseOfIncapacitation().Name);
				return str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedTime(bleedLifeTime, "F0"));
			};
			this.Relocating = this.CreateStatusItem("Relocating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Relocating.resolveStringCallback = resolveStringCallback;
			this.Fighting = this.CreateStatusItem("Fighting", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Fighting.AddNotification(null, null, null);
			this.Fleeing = this.CreateStatusItem("Fleeing", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Fleeing.AddNotification(null, null, null);
			this.Stressed = this.CreateStatusItem("Stressed", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Stressed.AddNotification(null, null, null);
			this.LashingOut = this.CreateStatusItem("LashingOut", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.LashingOut.AddNotification(null, null, null);
			this.LowImmunity = this.CreateStatusItem("LowImmunity", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowImmunity.AddNotification(null, null, null);
			this.Studying = this.CreateStatusItem("Studying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.InstallingElectrobank = this.CreateStatusItem("InstallingElectrobank", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Socializing = this.CreateStatusItem("Socializing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.Mingling = this.CreateStatusItem("Mingling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.BionicExplorerBooster = this.CreateStatusItem("BionicExplorerBooster", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 2);
			this.BionicExplorerBooster.resolveStringCallback = delegate(string str, object data)
			{
				BionicUpgrade_ExplorerBoosterMonitor.Instance instance = (BionicUpgrade_ExplorerBoosterMonitor.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedPercent(instance.CurrentProgress * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.ContactWithGerms = this.CreateStatusItem("ContactWithGerms", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.Disease.ID, true, 2);
			this.ContactWithGerms.resolveStringCallback = delegate(string str, object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				string name = Db.Get().Sicknesses.Get(exposureStatusData.exposure_type.sickness_id).Name;
				str = str.Replace("{Sickness}", name);
				return str;
			};
			this.ContactWithGerms.statusItemClickCallback = delegate(object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				Vector3 lastExposurePosition = exposureStatusData.owner.GetLastExposurePosition(exposureStatusData.exposure_type.germ_id);
				CameraController.Instance.CameraGoTo(lastExposurePosition, 2f, true);
				if (OverlayScreen.Instance.mode == OverlayModes.None.ID)
				{
					OverlayScreen.Instance.ToggleOverlay(OverlayModes.Disease.ID, true);
				}
			};
			this.ExposedToGerms = this.CreateStatusItem("ExposedToGerms", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.Disease.ID, true, 2);
			this.ExposedToGerms.resolveStringCallback = delegate(string str, object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				string name = Db.Get().Sicknesses.Get(exposureStatusData.exposure_type.sickness_id).Name;
				AttributeInstance attributeInstance = Db.Get().Attributes.GermResistance.Lookup(exposureStatusData.owner.gameObject);
				string lastDiseaseSource = exposureStatusData.owner.GetLastDiseaseSource(exposureStatusData.exposure_type.germ_id);
				GermExposureMonitor.Instance smi = exposureStatusData.owner.GetSMI<GermExposureMonitor.Instance>();
				float num = (float)exposureStatusData.exposure_type.base_resistance + GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[0];
				float totalValue = attributeInstance.GetTotalValue();
				float resistanceToExposureType = smi.GetResistanceToExposureType(exposureStatusData.exposure_type, -1f);
				float contractionChance = GermExposureMonitor.GetContractionChance(resistanceToExposureType);
				float exposureTier = smi.GetExposureTier(exposureStatusData.exposure_type.germ_id);
				float num2 = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int)exposureTier - 1] - GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[0];
				str = str.Replace("{Severity}", DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.EXPOSURE_TIERS[(int)exposureTier - 1].ToString());
				str = str.Replace("{Sickness}", name);
				str = str.Replace("{Source}", lastDiseaseSource);
				str = str.Replace("{Base}", GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Dupe}", GameUtil.GetFormattedSimple(totalValue, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Total}", GameUtil.GetFormattedSimple(resistanceToExposureType, GameUtil.TimeSlice.None, null));
				str = str.Replace("{ExposureLevelBonus}", GameUtil.GetFormattedSimple(num2, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Chance}", GameUtil.GetFormattedPercent(contractionChance * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.ExposedToGerms.statusItemClickCallback = delegate(object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				Vector3 lastExposurePosition = exposureStatusData.owner.GetLastExposurePosition(exposureStatusData.exposure_type.germ_id);
				CameraController.Instance.CameraGoTo(lastExposurePosition, 2f, true);
				if (OverlayScreen.Instance.mode == OverlayModes.None.ID)
				{
					OverlayScreen.Instance.ToggleOverlay(OverlayModes.Disease.ID, true);
				}
			};
			this.LightWorkEfficiencyBonus = this.CreateStatusItem("LightWorkEfficiencyBonus", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.LightWorkEfficiencyBonus.resolveStringCallback = delegate(string str, object data)
			{
				string arg = string.Format(DUPLICANTS.STATUSITEMS.LIGHTWORKEFFICIENCYBONUS.NO_BUILDING_WORK_ATTRIBUTE, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS * 100f, GameUtil.TimeSlice.None), true));
				return string.Format(str, arg);
			};
			this.LaboratoryWorkEfficiencyBonus = this.CreateStatusItem("LaboratoryWorkEfficiencyBonus", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.LaboratoryWorkEfficiencyBonus.resolveStringCallback = delegate(string str, object data)
			{
				string arg = string.Format(DUPLICANTS.STATUSITEMS.LABORATORYWORKEFFICIENCYBONUS.NO_BUILDING_WORK_ATTRIBUTE, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None), true));
				return string.Format(str, arg);
			};
			this.BeingProductive = this.CreateStatusItem("BeingProductive", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BalloonArtistPlanning = this.CreateStatusItem("BalloonArtistPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BalloonArtistHandingOut = this.CreateStatusItem("BalloonArtistHandingOut", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Partying = this.CreateStatusItem("Partying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.DataRainerPlanning = this.CreateStatusItem("DataRainerPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.DataRainerRaining = this.CreateStatusItem("DataRainerRaining", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RoboDancerPlanning = this.CreateStatusItem("RoboDancerPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RoboDancerDancing = this.CreateStatusItem("RoboDancerDancing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GasLiquidIrritation = this.CreateStatusItem("GasLiquidIrritated", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.GasLiquidIrritation.resolveStringCallback = ((string str, object data) => ((GasLiquidExposureMonitor.Instance)data).IsMajorIrritation() ? DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.NAME_MAJOR : DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.NAME_MINOR);
			this.GasLiquidIrritation.resolveTooltipCallback = delegate(string str, object data)
			{
				GasLiquidExposureMonitor.Instance instance = (GasLiquidExposureMonitor.Instance)data;
				string text = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP;
				string text2 = "";
				Effect appliedEffect = instance.sm.GetAppliedEffect(instance);
				if (appliedEffect != null)
				{
					text2 = Effect.CreateTooltip(appliedEffect, false, "\n    • ", true);
				}
				string text3 = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_EXPOSED.Replace("{element}", instance.CurrentlyExposedToElement().name);
				float currentExposure = instance.sm.GetCurrentExposure(instance);
				if (currentExposure < 0f)
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_DECREASE);
				}
				else if (currentExposure > 0f)
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_INCREASE);
				}
				else
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_STAYS);
				}
				float seconds = (instance.exposure - instance.minorIrritationThreshold) / Math.Abs(instance.exposureRate);
				string text4 = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_EXPOSURE_LEVEL.Replace("{time}", GameUtil.GetFormattedTime(seconds, "F0"));
				return string.Concat(new string[]
				{
					text,
					"\n\n",
					text2,
					"\n\n",
					text3,
					"\n\n",
					text4
				});
			};
			this.ExpellingRads = this.CreateStatusItem("ExpellingRads", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.AnalyzingGenes = this.CreateStatusItem("AnalyzingGenes", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.AnalyzingArtifact = this.CreateStatusItem("AnalyzingArtifact", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.MegaBrainTank_Pajamas_Wearing = this.CreateStatusItem("MegaBrainTank_Pajamas_Wearing", DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.NAME, DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MegaBrainTank_Pajamas_Wearing.resolveTooltipCallback_shouldStillCallIfDataIsNull = true;
			this.MegaBrainTank_Pajamas_Wearing.resolveTooltipCallback = delegate(string str, object data)
			{
				string str2 = DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.TOOLTIP;
				Effect effect = Db.Get().effects.Get("SleepClinic");
				string str3;
				if (effect != null)
				{
					str3 = Effect.CreateTooltip(effect, false, "\n    • ", true);
				}
				else
				{
					str3 = "";
				}
				return str2 + "\n\n" + str3;
			};
			this.MegaBrainTank_Pajamas_Sleeping = this.CreateStatusItem("MegaBrainTank_Pajamas_Sleeping", DUPLICANTS.STATUSITEMS.DREAMING.NAME, DUPLICANTS.STATUSITEMS.DREAMING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MegaBrainTank_Pajamas_Sleeping.resolveTooltipCallback = delegate(string str, object data)
			{
				ClinicDreamable clinicDreamable = (ClinicDreamable)data;
				return str.Replace("{time}", GameUtil.GetFormattedTime(clinicDreamable.WorkTimeRemaining, "F0"));
			};
			this.FossilHunt_WorkerExcavating = this.CreateStatusItem("FossilHunt_WorkerExcavating", DUPLICANTS.STATUSITEMS.FOSSILHUNT.WORKEREXCAVATING.NAME, DUPLICANTS.STATUSITEMS.FOSSILHUNT.WORKEREXCAVATING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MorbRoverMakerWorkingOnRevealing = this.CreateStatusItem("MorbRoverMakerWorkingOnRevealing", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_REVEALING.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_REVEALING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MorbRoverMakerDoctorWorking = this.CreateStatusItem("MorbRoverMakerDoctorWorking", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_WORKING_BUILDING.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_WORKING_BUILDING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.ArmingTrap = this.CreateStatusItem("ArmingTrap", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WaxedForTransitTube = this.CreateStatusItem("WaxedForTransitTube", "DUPLICANTS", "action_speed_up", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WaxedForTransitTube.resolveTooltipCallback = delegate(string str, object data)
			{
				float percent = (float)data * 100f;
				return str.Replace("{0}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
			};
			this.JoyResponse_HasBalloon = this.CreateStatusItem("JoyResponse_HasBalloon", DUPLICANTS.MODIFIERS.HASBALLOON.NAME, DUPLICANTS.MODIFIERS.HASBALLOON.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.JoyResponse_HasBalloon.resolveTooltipCallback = delegate(string str, object data)
			{
				EquippableBalloon.StatesInstance statesInstance = (EquippableBalloon.StatesInstance)data;
				return str + "\n\n" + DUPLICANTS.MODIFIERS.TIME_REMAINING.Replace("{0}", GameUtil.GetFormattedCycles(statesInstance.transitionTime - GameClock.Instance.GetTime(), "F1", false));
			};
			this.JoyResponse_HeardJoySinger = this.CreateStatusItem("JoyResponse_HeardJoySinger", DUPLICANTS.MODIFIERS.HEARDJOYSINGER.NAME, DUPLICANTS.MODIFIERS.HEARDJOYSINGER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.JoyResponse_HeardJoySinger.resolveTooltipCallback = delegate(string str, object data)
			{
				InspirationEffectMonitor.Instance instance = (InspirationEffectMonitor.Instance)data;
				return str + "\n\n" + DUPLICANTS.MODIFIERS.TIME_REMAINING.Replace("{0}", GameUtil.GetFormattedCycles(instance.sm.inspirationTimeRemaining.Get(instance), "F1", false));
			};
			this.JoyResponse_StickerBombing = this.CreateStatusItem("JoyResponse_StickerBombing", DUPLICANTS.MODIFIERS.ISSTICKERBOMBING.NAME, DUPLICANTS.MODIFIERS.ISSTICKERBOMBING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.Meteorphile = this.CreateStatusItem("Meteorphile", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.EnteringDock = this.CreateStatusItem("EnteringDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.ENTERINGDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.ENTERINGDOCK.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.UnreachableDock = this.CreateStatusItem("UnreachableDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.UNREACHABLEDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.UNREACHABLEDOCK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 2);
			this.NoHomeDock = this.CreateStatusItem("UnreachableDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.NOHOMEDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.NOHOMEDOCK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerCapacitorStatus = this.CreateStatusItem("RemoteWorkerCapacitorStatus", DUPLICANTS.STATUSITEMS.REMOTEWORKER.POWERSTATUS.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.POWERSTATUS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerCapacitorStatus.resolveStringCallback = delegate(string str, object obj)
			{
				RemoteWorkerCapacitor remoteWorkerCapacitor = obj as RemoteWorkerCapacitor;
				float joules = 0f;
				float percent = 0f;
				if (remoteWorkerCapacitor != null)
				{
					joules = remoteWorkerCapacitor.Charge;
					percent = remoteWorkerCapacitor.ChargeRatio * 100f;
				}
				return str.Replace("{CHARGE}", GameUtil.GetFormattedJoules(joules, "F1", GameUtil.TimeSlice.None)).Replace("{RATIO}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
			};
			this.RemoteWorkerLowPower = this.CreateStatusItem("RemoteWorkerLowPower", DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWPOWER.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWPOWER.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOutOfPower = this.CreateStatusItem("RemoteWorkerOutOfPower", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFPOWER.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFPOWER.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerHighGunkLevel = this.CreateStatusItem("RemoteWorkerHighGunkLevel", DUPLICANTS.STATUSITEMS.REMOTEWORKER.HIGHGUNK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.HIGHGUNK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerFullGunkLevel = this.CreateStatusItem("RemoteWorkerFullGunkLevel", DUPLICANTS.STATUSITEMS.REMOTEWORKER.FULLGUNK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.FULLGUNK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerLowOil = this.CreateStatusItem("RemoteWorkerLowOil", DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWOIL.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWOIL.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOutOfOil = this.CreateStatusItem("RemoteWorkerOutOfOil", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFOIL.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFOIL.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerRecharging = this.CreateStatusItem("RemoteWorkerRecharging", DUPLICANTS.STATUSITEMS.REMOTEWORKER.RECHARGING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.RECHARGING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOiling = this.CreateStatusItem("RemoteWorkerOiling", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OILING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OILING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerDraining = this.CreateStatusItem("RemoteWorkerDraining", DUPLICANTS.STATUSITEMS.REMOTEWORKER.DRAINING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.DRAINING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.BionicCriticalBattery = this.CreateStatusItem("BionicCriticalBattery", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.BionicCriticalBattery.AddNotification(null, null, null);
		}

		// Token: 0x040052AD RID: 21165
		public StatusItem Idle;

		// Token: 0x040052AE RID: 21166
		public StatusItem IdleInRockets;

		// Token: 0x040052AF RID: 21167
		public StatusItem Pacified;

		// Token: 0x040052B0 RID: 21168
		public StatusItem PendingPacification;

		// Token: 0x040052B1 RID: 21169
		public StatusItem Dead;

		// Token: 0x040052B2 RID: 21170
		public StatusItem MoveToSuitNotRequired;

		// Token: 0x040052B3 RID: 21171
		public StatusItem DroppingUnusedInventory;

		// Token: 0x040052B4 RID: 21172
		public StatusItem MovingToSafeArea;

		// Token: 0x040052B5 RID: 21173
		public StatusItem BedUnreachable;

		// Token: 0x040052B6 RID: 21174
		public StatusItem Hungry;

		// Token: 0x040052B7 RID: 21175
		public StatusItem Starving;

		// Token: 0x040052B8 RID: 21176
		public StatusItem Rotten;

		// Token: 0x040052B9 RID: 21177
		public StatusItem Quarantined;

		// Token: 0x040052BA RID: 21178
		public StatusItem NoRationsAvailable;

		// Token: 0x040052BB RID: 21179
		public StatusItem RationsUnreachable;

		// Token: 0x040052BC RID: 21180
		public StatusItem RationsNotPermitted;

		// Token: 0x040052BD RID: 21181
		public StatusItem DailyRationLimitReached;

		// Token: 0x040052BE RID: 21182
		public StatusItem Scalding;

		// Token: 0x040052BF RID: 21183
		public StatusItem Hot;

		// Token: 0x040052C0 RID: 21184
		public StatusItem Cold;

		// Token: 0x040052C1 RID: 21185
		public StatusItem ExitingCold;

		// Token: 0x040052C2 RID: 21186
		public StatusItem ExitingHot;

		// Token: 0x040052C3 RID: 21187
		public StatusItem QuarantineAreaUnassigned;

		// Token: 0x040052C4 RID: 21188
		public StatusItem QuarantineAreaUnreachable;

		// Token: 0x040052C5 RID: 21189
		public StatusItem Tired;

		// Token: 0x040052C6 RID: 21190
		public StatusItem NervousBreakdown;

		// Token: 0x040052C7 RID: 21191
		public StatusItem Unhappy;

		// Token: 0x040052C8 RID: 21192
		public StatusItem Suffocating;

		// Token: 0x040052C9 RID: 21193
		public StatusItem HoldingBreath;

		// Token: 0x040052CA RID: 21194
		public StatusItem ToiletUnreachable;

		// Token: 0x040052CB RID: 21195
		public StatusItem NoUsableToilets;

		// Token: 0x040052CC RID: 21196
		public StatusItem NoToilets;

		// Token: 0x040052CD RID: 21197
		public StatusItem Vomiting;

		// Token: 0x040052CE RID: 21198
		public StatusItem Coughing;

		// Token: 0x040052CF RID: 21199
		public StatusItem Slippering;

		// Token: 0x040052D0 RID: 21200
		public StatusItem BreathingO2;

		// Token: 0x040052D1 RID: 21201
		public StatusItem EmittingCO2;

		// Token: 0x040052D2 RID: 21202
		public StatusItem LowOxygen;

		// Token: 0x040052D3 RID: 21203
		public StatusItem RedAlert;

		// Token: 0x040052D4 RID: 21204
		public StatusItem Digging;

		// Token: 0x040052D5 RID: 21205
		public StatusItem Eating;

		// Token: 0x040052D6 RID: 21206
		public StatusItem Dreaming;

		// Token: 0x040052D7 RID: 21207
		public StatusItem Sleeping;

		// Token: 0x040052D8 RID: 21208
		public StatusItem SleepingExhausted;

		// Token: 0x040052D9 RID: 21209
		public StatusItem SleepingInterruptedByLight;

		// Token: 0x040052DA RID: 21210
		public StatusItem SleepingInterruptedByNoise;

		// Token: 0x040052DB RID: 21211
		public StatusItem SleepingInterruptedByFearOfDark;

		// Token: 0x040052DC RID: 21212
		public StatusItem SleepingInterruptedByMovement;

		// Token: 0x040052DD RID: 21213
		public StatusItem SleepingInterruptedByCold;

		// Token: 0x040052DE RID: 21214
		public StatusItem SleepingPeacefully;

		// Token: 0x040052DF RID: 21215
		public StatusItem SleepingBadly;

		// Token: 0x040052E0 RID: 21216
		public StatusItem SleepingTerribly;

		// Token: 0x040052E1 RID: 21217
		public StatusItem Cleaning;

		// Token: 0x040052E2 RID: 21218
		public StatusItem PickingUp;

		// Token: 0x040052E3 RID: 21219
		public StatusItem Mopping;

		// Token: 0x040052E4 RID: 21220
		public StatusItem Cooking;

		// Token: 0x040052E5 RID: 21221
		public StatusItem Arting;

		// Token: 0x040052E6 RID: 21222
		public StatusItem Mushing;

		// Token: 0x040052E7 RID: 21223
		public StatusItem Researching;

		// Token: 0x040052E8 RID: 21224
		public StatusItem ResearchingFromPOI;

		// Token: 0x040052E9 RID: 21225
		public StatusItem MissionControlling;

		// Token: 0x040052EA RID: 21226
		public StatusItem Tinkering;

		// Token: 0x040052EB RID: 21227
		public StatusItem Storing;

		// Token: 0x040052EC RID: 21228
		public StatusItem Building;

		// Token: 0x040052ED RID: 21229
		public StatusItem Equipping;

		// Token: 0x040052EE RID: 21230
		public StatusItem WarmingUp;

		// Token: 0x040052EF RID: 21231
		public StatusItem GeneratingPower;

		// Token: 0x040052F0 RID: 21232
		public StatusItem Ranching;

		// Token: 0x040052F1 RID: 21233
		public StatusItem Harvesting;

		// Token: 0x040052F2 RID: 21234
		public StatusItem Uprooting;

		// Token: 0x040052F3 RID: 21235
		public StatusItem Emptying;

		// Token: 0x040052F4 RID: 21236
		public StatusItem Toggling;

		// Token: 0x040052F5 RID: 21237
		public StatusItem Deconstructing;

		// Token: 0x040052F6 RID: 21238
		public StatusItem Disinfecting;

		// Token: 0x040052F7 RID: 21239
		public StatusItem Relocating;

		// Token: 0x040052F8 RID: 21240
		public StatusItem Upgrading;

		// Token: 0x040052F9 RID: 21241
		public StatusItem Fabricating;

		// Token: 0x040052FA RID: 21242
		public StatusItem Processing;

		// Token: 0x040052FB RID: 21243
		public StatusItem Spicing;

		// Token: 0x040052FC RID: 21244
		public StatusItem Clearing;

		// Token: 0x040052FD RID: 21245
		public StatusItem BodyRegulatingHeating;

		// Token: 0x040052FE RID: 21246
		public StatusItem BodyRegulatingCooling;

		// Token: 0x040052FF RID: 21247
		public StatusItem EntombedChore;

		// Token: 0x04005300 RID: 21248
		public StatusItem EarlyMorning;

		// Token: 0x04005301 RID: 21249
		public StatusItem NightTime;

		// Token: 0x04005302 RID: 21250
		public StatusItem PoorDecor;

		// Token: 0x04005303 RID: 21251
		public StatusItem PoorQualityOfLife;

		// Token: 0x04005304 RID: 21252
		public StatusItem PoorFoodQuality;

		// Token: 0x04005305 RID: 21253
		public StatusItem GoodFoodQuality;

		// Token: 0x04005306 RID: 21254
		public StatusItem SevereWounds;

		// Token: 0x04005307 RID: 21255
		public StatusItem Incapacitated;

		// Token: 0x04005308 RID: 21256
		public StatusItem BionicOfflineIncapacitated;

		// Token: 0x04005309 RID: 21257
		public StatusItem BionicWaitingForReboot;

		// Token: 0x0400530A RID: 21258
		public StatusItem BionicBeingRebooted;

		// Token: 0x0400530B RID: 21259
		public StatusItem BionicRequiresSkillPerk;

		// Token: 0x0400530C RID: 21260
		public StatusItem BionicWantsOilChange;

		// Token: 0x0400530D RID: 21261
		public StatusItem InstallingElectrobank;

		// Token: 0x0400530E RID: 21262
		public StatusItem Fighting;

		// Token: 0x0400530F RID: 21263
		public StatusItem Fleeing;

		// Token: 0x04005310 RID: 21264
		public StatusItem Stressed;

		// Token: 0x04005311 RID: 21265
		public StatusItem LashingOut;

		// Token: 0x04005312 RID: 21266
		public StatusItem LowImmunity;

		// Token: 0x04005313 RID: 21267
		public StatusItem Studying;

		// Token: 0x04005314 RID: 21268
		public StatusItem Socializing;

		// Token: 0x04005315 RID: 21269
		public StatusItem Mingling;

		// Token: 0x04005316 RID: 21270
		public StatusItem ContactWithGerms;

		// Token: 0x04005317 RID: 21271
		public StatusItem ExposedToGerms;

		// Token: 0x04005318 RID: 21272
		public StatusItem LightWorkEfficiencyBonus;

		// Token: 0x04005319 RID: 21273
		public StatusItem LaboratoryWorkEfficiencyBonus;

		// Token: 0x0400531A RID: 21274
		public StatusItem BeingProductive;

		// Token: 0x0400531B RID: 21275
		public StatusItem BalloonArtistPlanning;

		// Token: 0x0400531C RID: 21276
		public StatusItem BalloonArtistHandingOut;

		// Token: 0x0400531D RID: 21277
		public StatusItem Partying;

		// Token: 0x0400531E RID: 21278
		public StatusItem GasLiquidIrritation;

		// Token: 0x0400531F RID: 21279
		public StatusItem ExpellingRads;

		// Token: 0x04005320 RID: 21280
		public StatusItem AnalyzingGenes;

		// Token: 0x04005321 RID: 21281
		public StatusItem AnalyzingArtifact;

		// Token: 0x04005322 RID: 21282
		public StatusItem MegaBrainTank_Pajamas_Wearing;

		// Token: 0x04005323 RID: 21283
		public StatusItem MegaBrainTank_Pajamas_Sleeping;

		// Token: 0x04005324 RID: 21284
		public StatusItem JoyResponse_HasBalloon;

		// Token: 0x04005325 RID: 21285
		public StatusItem JoyResponse_HeardJoySinger;

		// Token: 0x04005326 RID: 21286
		public StatusItem JoyResponse_StickerBombing;

		// Token: 0x04005327 RID: 21287
		public StatusItem Meteorphile;

		// Token: 0x04005328 RID: 21288
		public StatusItem FossilHunt_WorkerExcavating;

		// Token: 0x04005329 RID: 21289
		public StatusItem MorbRoverMakerDoctorWorking;

		// Token: 0x0400532A RID: 21290
		public StatusItem MorbRoverMakerWorkingOnRevealing;

		// Token: 0x0400532B RID: 21291
		public StatusItem ArmingTrap;

		// Token: 0x0400532C RID: 21292
		public StatusItem WaxedForTransitTube;

		// Token: 0x0400532D RID: 21293
		public StatusItem DataRainerPlanning;

		// Token: 0x0400532E RID: 21294
		public StatusItem DataRainerRaining;

		// Token: 0x0400532F RID: 21295
		public StatusItem RoboDancerPlanning;

		// Token: 0x04005330 RID: 21296
		public StatusItem RoboDancerDancing;

		// Token: 0x04005331 RID: 21297
		public StatusItem BionicExplorerBooster;

		// Token: 0x04005332 RID: 21298
		public StatusItem EnteringDock;

		// Token: 0x04005333 RID: 21299
		public StatusItem UnreachableDock;

		// Token: 0x04005334 RID: 21300
		public StatusItem NoHomeDock;

		// Token: 0x04005335 RID: 21301
		public StatusItem RemoteWorkerCapacitorStatus;

		// Token: 0x04005336 RID: 21302
		public StatusItem RemoteWorkerLowPower;

		// Token: 0x04005337 RID: 21303
		public StatusItem RemoteWorkerOutOfPower;

		// Token: 0x04005338 RID: 21304
		public StatusItem RemoteWorkerHighGunkLevel;

		// Token: 0x04005339 RID: 21305
		public StatusItem RemoteWorkerFullGunkLevel;

		// Token: 0x0400533A RID: 21306
		public StatusItem RemoteWorkerLowOil;

		// Token: 0x0400533B RID: 21307
		public StatusItem RemoteWorkerOutOfOil;

		// Token: 0x0400533C RID: 21308
		public StatusItem RemoteWorkerRecharging;

		// Token: 0x0400533D RID: 21309
		public StatusItem RemoteWorkerOiling;

		// Token: 0x0400533E RID: 21310
		public StatusItem RemoteWorkerDraining;

		// Token: 0x0400533F RID: 21311
		public StatusItem BionicCriticalBattery;

		// Token: 0x04005340 RID: 21312
		private const int NONE_OVERLAY = 0;
	}
}

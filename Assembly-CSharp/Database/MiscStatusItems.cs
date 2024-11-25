using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E67 RID: 3687
	public class MiscStatusItems : StatusItems
	{
		// Token: 0x06007498 RID: 29848 RVA: 0x002D67FF File Offset: 0x002D49FF
		public MiscStatusItems(ResourceSet parent) : base("MiscStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x06007499 RID: 29849 RVA: 0x002D6814 File Offset: 0x002D4A14
		private StatusItem CreateStatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022)
		{
			return base.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, null));
		}

		// Token: 0x0600749A RID: 29850 RVA: 0x002D683C File Offset: 0x002D4A3C
		private StatusItem CreateStatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022)
		{
			return base.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null));
		}

		// Token: 0x0600749B RID: 29851 RVA: 0x002D6868 File Offset: 0x002D4A68
		private void CreateStatusItems()
		{
			this.AttentionRequired = this.CreateStatusItem("AttentionRequired", "MISC", "status_item_doubleexclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Edible = this.CreateStatusItem("Edible", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Edible.resolveStringCallback = delegate(string str, object data)
			{
				Edible edible = (Edible)data;
				str = string.Format(str, GameUtil.GetFormattedCalories(edible.Calories, GameUtil.TimeSlice.None, true));
				return str;
			};
			this.PendingClear = this.CreateStatusItem("PendingClear", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingClearNoStorage = this.CreateStatusItem("PendingClearNoStorage", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MarkedForCompost = this.CreateStatusItem("MarkedForCompost", "MISC", "status_item_pending_compost", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MarkedForCompostInStorage = this.CreateStatusItem("MarkedForCompostInStorage", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MarkedForDisinfection = this.CreateStatusItem("MarkedForDisinfection", "MISC", "status_item_disinfect", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.Disease.ID, true, 129022);
			this.NoClearLocationsAvailable = this.CreateStatusItem("NoClearLocationsAvailable", "MISC", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.WaitingForDig = this.CreateStatusItem("WaitingForDig", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.WaitingForMop = this.CreateStatusItem("WaitingForMop", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.OreMass = this.CreateStatusItem("OreMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.OreMass.resolveStringCallback = delegate(string str, object data)
			{
				GameObject gameObject = (GameObject)data;
				str = str.Replace("{Mass}", GameUtil.GetFormattedMass(gameObject.GetComponent<PrimaryElement>().Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.OreTemp = this.CreateStatusItem("OreTemp", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.OreTemp.resolveStringCallback = delegate(string str, object data)
			{
				GameObject gameObject = (GameObject)data;
				str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(gameObject.GetComponent<PrimaryElement>().Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.ElementalState = this.CreateStatusItem("ElementalState", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalState.resolveStringCallback = delegate(string str, object data)
			{
				Element element = ((Func<Element>)data)();
				str = str.Replace("{State}", element.GetStateString());
				return str;
			};
			this.ElementalCategory = this.CreateStatusItem("ElementalCategory", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalCategory.resolveStringCallback = delegate(string str, object data)
			{
				Element element = ((Func<Element>)data)();
				str = str.Replace("{Category}", element.GetMaterialCategoryTag().ProperName());
				return str;
			};
			this.ElementalTemperature = this.CreateStatusItem("ElementalTemperature", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalTemperature.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(cellSelectionObject.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.ElementalMass = this.CreateStatusItem("ElementalMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalMass.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				str = str.Replace("{Mass}", GameUtil.GetFormattedMass(cellSelectionObject.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.ElementalDisease = this.CreateStatusItem("ElementalDisease", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalDisease.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount, false));
				return str;
			};
			this.ElementalDisease.resolveTooltipCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount, true));
				return str;
			};
			this.GrowingBranches = new StatusItem("GrowingBranches", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
			this.TreeFilterableTags = this.CreateStatusItem("TreeFilterableTags", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TreeFilterableTags.resolveStringCallback = delegate(string str, object data)
			{
				TreeFilterable treeFilterable = (TreeFilterable)data;
				str = str.Replace("{Tags}", treeFilterable.GetTagsAsStatus(6));
				return str;
			};
			this.SublimationEmitting = this.CreateStatusItem("SublimationEmitting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SublimationEmitting.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				if (cellSelectionObject.element.sublimateId == (SimHashes)0)
				{
					return str;
				}
				str = str.Replace("{Element}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(cellSelectionObject.FlowRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.SublimationEmitting.resolveTooltipCallback = this.SublimationEmitting.resolveStringCallback;
			this.SublimationBlocked = this.CreateStatusItem("SublimationBlocked", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SublimationBlocked.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				if (cellSelectionObject.element.sublimateId == (SimHashes)0)
				{
					return str;
				}
				str = str.Replace("{Element}", cellSelectionObject.element.name);
				str = str.Replace("{SubElement}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
				return str;
			};
			this.SublimationBlocked.resolveTooltipCallback = this.SublimationBlocked.resolveStringCallback;
			this.SublimationOverpressure = this.CreateStatusItem("SublimationOverpressure", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SublimationOverpressure.resolveTooltipCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				if (cellSelectionObject.element.sublimateId == (SimHashes)0)
				{
					return str;
				}
				str = str.Replace("{Element}", cellSelectionObject.element.name);
				str = str.Replace("{SubElement}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
				return str;
			};
			this.Space = this.CreateStatusItem("Space", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.BuriedItem = this.CreateStatusItem("BuriedItem", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutOverPressure = this.CreateStatusItem("SpoutOverPressure", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutOverPressure.resolveStringCallback = delegate(string str, object data)
			{
				Geyser.StatesInstance statesInstance = (Geyser.StatesInstance)data;
				Studyable component = statesInstance.GetComponent<Studyable>();
				if (statesInstance != null && component != null && component.Studied)
				{
					str = str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTOVERPRESSURE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime(), "F1", false)));
				}
				else
				{
					str = str.Replace("{StudiedDetails}", "");
				}
				return str;
			};
			this.SpoutEmitting = this.CreateStatusItem("SpoutEmitting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutEmitting.resolveStringCallback = delegate(string str, object data)
			{
				Geyser.StatesInstance statesInstance = (Geyser.StatesInstance)data;
				Studyable component = statesInstance.GetComponent<Studyable>();
				if (statesInstance != null && component != null && component.Studied)
				{
					str = str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTEMITTING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime(), "F1", false)));
				}
				else
				{
					str = str.Replace("{StudiedDetails}", "");
				}
				return str;
			};
			this.SpoutPressureBuilding = this.CreateStatusItem("SpoutPressureBuilding", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutPressureBuilding.resolveStringCallback = delegate(string str, object data)
			{
				Geyser.StatesInstance statesInstance = (Geyser.StatesInstance)data;
				Studyable component = statesInstance.GetComponent<Studyable>();
				if (statesInstance != null && component != null && component.Studied)
				{
					str = str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTPRESSUREBUILDING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime(), "F1", false)));
				}
				else
				{
					str = str.Replace("{StudiedDetails}", "");
				}
				return str;
			};
			this.SpoutIdle = this.CreateStatusItem("SpoutIdle", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutIdle.resolveStringCallback = delegate(string str, object data)
			{
				Geyser.StatesInstance statesInstance = (Geyser.StatesInstance)data;
				Studyable component = statesInstance.GetComponent<Studyable>();
				if (statesInstance != null && component != null && component.Studied)
				{
					str = str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTIDLE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime(), "F1", false)));
				}
				else
				{
					str = str.Replace("{StudiedDetails}", "");
				}
				return str;
			};
			this.SpoutDormant = this.CreateStatusItem("SpoutDormant", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpicedFood = this.CreateStatusItem("SpicedFood", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpicedFood.resolveTooltipCallback = delegate(string baseString, object data)
			{
				string text = baseString;
				string str = "\n    • ";
				foreach (SpiceInstance spiceInstance in ((List<SpiceInstance>)data))
				{
					string str2 = "STRINGS.ITEMS.SPICES.";
					Tag id = spiceInstance.Id;
					string text2 = str2 + id.Name.ToUpper() + ".NAME";
					StringEntry stringEntry;
					Strings.TryGet(text2, out stringEntry);
					string str3 = (stringEntry == null) ? ("MISSING " + text2) : stringEntry.String;
					text = text + str + str3;
					string linePrefix = "\n        • ";
					if (spiceInstance.StatBonus != null)
					{
						text += Effect.CreateTooltip(spiceInstance.StatBonus, false, linePrefix, false);
					}
				}
				return text;
			};
			this.RehydratedFood = this.CreateStatusItem("RehydratedFood", "MISC", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.OrderAttack = this.CreateStatusItem("OrderAttack", "MISC", "status_item_attack", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.OrderCapture = this.CreateStatusItem("OrderCapture", "MISC", "status_item_capture", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingHarvest = this.CreateStatusItem("PendingHarvest", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NotMarkedForHarvest = this.CreateStatusItem("NotMarkedForHarvest", "MISC", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NotMarkedForHarvest.conditionalOverlayCallback = ((HashedString viewMode, object o) => !(viewMode != OverlayModes.None.ID));
			this.PendingUproot = this.CreateStatusItem("PendingUproot", "MISC", "status_item_pending_uproot", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PickupableUnreachable = this.CreateStatusItem("PickupableUnreachable", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Prioritized = this.CreateStatusItem("Prioritized", "MISC", "status_item_prioritized", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Using = this.CreateStatusItem("Using", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Using.resolveStringCallback = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null)
				{
					KSelectable component = workable.GetComponent<KSelectable>();
					if (component != null)
					{
						str = str.Replace("{Target}", component.GetName());
					}
				}
				return str;
			};
			this.Operating = this.CreateStatusItem("Operating", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Cleaning = this.CreateStatusItem("Cleaning", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RegionIsBlocked = this.CreateStatusItem("RegionIsBlocked", "MISC", "status_item_solids_blocking", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingStudy = this.CreateStatusItem("AwaitingStudy", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Studied = this.CreateStatusItem("Studied", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.HighEnergyParticleCount = this.CreateStatusItem("HighEnergyParticleCount", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.HighEnergyParticleCount.resolveStringCallback = delegate(string str, object data)
			{
				GameObject gameObject = (GameObject)data;
				return GameUtil.GetFormattedHighEnergyParticles(gameObject.IsNullOrDestroyed() ? 0f : gameObject.GetComponent<HighEnergyParticle>().payload, GameUtil.TimeSlice.None, true);
			};
			this.Durability = this.CreateStatusItem("Durability", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Durability.resolveStringCallback = delegate(string str, object data)
			{
				Durability component = ((GameObject)data).GetComponent<Durability>();
				str = str.Replace("{durability}", GameUtil.GetFormattedPercent(component.GetDurability() * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.BionicExplorerBooster = this.CreateStatusItem("BionicExplorerBooster", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.BionicExplorerBooster.resolveStringCallback = delegate(string str, object data)
			{
				BionicUpgrade_ExplorerBooster.Instance instance = (BionicUpgrade_ExplorerBooster.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedPercent(instance.Progress * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.BionicExplorerBoosterReady = this.CreateStatusItem("BionicExplorerBoosterReady", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022);
			this.StoredItemDurability = this.CreateStatusItem("StoredItemDurability", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.StoredItemDurability.resolveStringCallback = delegate(string str, object data)
			{
				Durability component = ((GameObject)data).GetComponent<Durability>();
				float percent = (component != null) ? (component.GetDurability() * 100f) : 100f;
				str = str.Replace("{durability}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
				return str;
			};
			this.ClusterMeteorRemainingTravelTime = this.CreateStatusItem("ClusterMeteorRemainingTravelTime", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ClusterMeteorRemainingTravelTime.resolveStringCallback = delegate(string str, object data)
			{
				float seconds = ((ClusterMapMeteorShower.Instance)data).ArrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
				str = str.Replace("{time}", GameUtil.GetFormattedCycles(seconds, "F1", false));
				return str;
			};
			this.ArtifactEntombed = this.CreateStatusItem("ArtifactEntombed", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TearOpen = this.CreateStatusItem("TearOpen", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TearClosed = this.CreateStatusItem("TearClosed", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MarkedForMove = this.CreateStatusItem("MarkedForMove", "MISC", "status_item_manually_controlled", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MoveStorageUnreachable = this.CreateStatusItem("MoveStorageUnreachable", "MISC", "status_item_manually_controlled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
		}

		// Token: 0x040053C7 RID: 21447
		public StatusItem AttentionRequired;

		// Token: 0x040053C8 RID: 21448
		public StatusItem MarkedForDisinfection;

		// Token: 0x040053C9 RID: 21449
		public StatusItem MarkedForCompost;

		// Token: 0x040053CA RID: 21450
		public StatusItem MarkedForCompostInStorage;

		// Token: 0x040053CB RID: 21451
		public StatusItem PendingClear;

		// Token: 0x040053CC RID: 21452
		public StatusItem PendingClearNoStorage;

		// Token: 0x040053CD RID: 21453
		public StatusItem Edible;

		// Token: 0x040053CE RID: 21454
		public StatusItem WaitingForDig;

		// Token: 0x040053CF RID: 21455
		public StatusItem WaitingForMop;

		// Token: 0x040053D0 RID: 21456
		public StatusItem OreMass;

		// Token: 0x040053D1 RID: 21457
		public StatusItem OreTemp;

		// Token: 0x040053D2 RID: 21458
		public StatusItem ElementalCategory;

		// Token: 0x040053D3 RID: 21459
		public StatusItem ElementalState;

		// Token: 0x040053D4 RID: 21460
		public StatusItem ElementalTemperature;

		// Token: 0x040053D5 RID: 21461
		public StatusItem ElementalMass;

		// Token: 0x040053D6 RID: 21462
		public StatusItem ElementalDisease;

		// Token: 0x040053D7 RID: 21463
		public StatusItem TreeFilterableTags;

		// Token: 0x040053D8 RID: 21464
		public StatusItem SublimationOverpressure;

		// Token: 0x040053D9 RID: 21465
		public StatusItem SublimationEmitting;

		// Token: 0x040053DA RID: 21466
		public StatusItem SublimationBlocked;

		// Token: 0x040053DB RID: 21467
		public StatusItem BuriedItem;

		// Token: 0x040053DC RID: 21468
		public StatusItem SpoutOverPressure;

		// Token: 0x040053DD RID: 21469
		public StatusItem SpoutEmitting;

		// Token: 0x040053DE RID: 21470
		public StatusItem SpoutPressureBuilding;

		// Token: 0x040053DF RID: 21471
		public StatusItem SpoutIdle;

		// Token: 0x040053E0 RID: 21472
		public StatusItem SpoutDormant;

		// Token: 0x040053E1 RID: 21473
		public StatusItem SpicedFood;

		// Token: 0x040053E2 RID: 21474
		public StatusItem RehydratedFood;

		// Token: 0x040053E3 RID: 21475
		public StatusItem OrderAttack;

		// Token: 0x040053E4 RID: 21476
		public StatusItem OrderCapture;

		// Token: 0x040053E5 RID: 21477
		public StatusItem PendingHarvest;

		// Token: 0x040053E6 RID: 21478
		public StatusItem NotMarkedForHarvest;

		// Token: 0x040053E7 RID: 21479
		public StatusItem PendingUproot;

		// Token: 0x040053E8 RID: 21480
		public StatusItem PickupableUnreachable;

		// Token: 0x040053E9 RID: 21481
		public StatusItem Prioritized;

		// Token: 0x040053EA RID: 21482
		public StatusItem Using;

		// Token: 0x040053EB RID: 21483
		public StatusItem Operating;

		// Token: 0x040053EC RID: 21484
		public StatusItem Cleaning;

		// Token: 0x040053ED RID: 21485
		public StatusItem RegionIsBlocked;

		// Token: 0x040053EE RID: 21486
		public StatusItem NoClearLocationsAvailable;

		// Token: 0x040053EF RID: 21487
		public StatusItem AwaitingStudy;

		// Token: 0x040053F0 RID: 21488
		public StatusItem Studied;

		// Token: 0x040053F1 RID: 21489
		public StatusItem StudiedGeyserTimeRemaining;

		// Token: 0x040053F2 RID: 21490
		public StatusItem Space;

		// Token: 0x040053F3 RID: 21491
		public StatusItem HighEnergyParticleCount;

		// Token: 0x040053F4 RID: 21492
		public StatusItem Durability;

		// Token: 0x040053F5 RID: 21493
		public StatusItem StoredItemDurability;

		// Token: 0x040053F6 RID: 21494
		public StatusItem ArtifactEntombed;

		// Token: 0x040053F7 RID: 21495
		public StatusItem TearOpen;

		// Token: 0x040053F8 RID: 21496
		public StatusItem TearClosed;

		// Token: 0x040053F9 RID: 21497
		public StatusItem ClusterMeteorRemainingTravelTime;

		// Token: 0x040053FA RID: 21498
		public StatusItem MarkedForMove;

		// Token: 0x040053FB RID: 21499
		public StatusItem MoveStorageUnreachable;

		// Token: 0x040053FC RID: 21500
		public StatusItem GrowingBranches;

		// Token: 0x040053FD RID: 21501
		public StatusItem BionicExplorerBooster;

		// Token: 0x040053FE RID: 21502
		public StatusItem BionicExplorerBoosterReady;
	}
}

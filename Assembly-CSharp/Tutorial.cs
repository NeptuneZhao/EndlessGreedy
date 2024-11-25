using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
[AddComponentMenu("KMonoBehaviour/scripts/Tutorial")]
public class Tutorial : KMonoBehaviour, IRender1000ms
{
	// Token: 0x060024CB RID: 9419 RVA: 0x000CD3BC File Offset: 0x000CB5BC
	public static void ResetHiddenTutorialMessages()
	{
		if (Tutorial.Instance != null)
		{
			Tutorial.Instance.tutorialMessagesSeen.Clear();
		}
		foreach (object obj in Enum.GetValues(typeof(Tutorial.TutorialMessages)))
		{
			Tutorial.TutorialMessages key = (Tutorial.TutorialMessages)obj;
			KPlayerPrefs.SetInt("HideTutorial_" + key.ToString(), 0);
			if (Tutorial.Instance != null)
			{
				Tutorial.Instance.hiddenTutorialMessages[key] = false;
			}
		}
		KPlayerPrefs.SetInt("HideTutorial_CheckState", 0);
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x000CD47C File Offset: 0x000CB67C
	private void LoadHiddenTutorialMessages()
	{
		foreach (object obj in Enum.GetValues(typeof(Tutorial.TutorialMessages)))
		{
			Tutorial.TutorialMessages key = (Tutorial.TutorialMessages)obj;
			bool value = KPlayerPrefs.GetInt("HideTutorial_" + key.ToString(), 0) != 0;
			this.hiddenTutorialMessages[key] = value;
		}
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x000CD508 File Offset: 0x000CB708
	public void HideTutorialMessage(Tutorial.TutorialMessages message)
	{
		this.hiddenTutorialMessages[message] = true;
		KPlayerPrefs.SetInt("HideTutorial_" + message.ToString(), 1);
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x060024CE RID: 9422 RVA: 0x000CD534 File Offset: 0x000CB734
	// (set) Token: 0x060024CF RID: 9423 RVA: 0x000CD53B File Offset: 0x000CB73B
	public static Tutorial Instance { get; private set; }

	// Token: 0x060024D0 RID: 9424 RVA: 0x000CD543 File Offset: 0x000CB743
	public static void DestroyInstance()
	{
		Tutorial.Instance = null;
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000CD54C File Offset: 0x000CB74C
	private void UpdateNotifierPosition()
	{
		if (this.notifierPosition == Vector3.zero)
		{
			GameObject activeTelepad = GameUtil.GetActiveTelepad();
			if (activeTelepad != null)
			{
				this.notifierPosition = activeTelepad.transform.GetPosition();
			}
		}
		this.notifier.transform.SetPosition(this.notifierPosition);
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x000CD5A2 File Offset: 0x000CB7A2
	protected override void OnPrefabInit()
	{
		Tutorial.Instance = this;
		this.LoadHiddenTutorialMessages();
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x000CD5B0 File Offset: 0x000CB7B0
	protected override void OnSpawn()
	{
		if (this.tutorialMessagesRemaining.Count != 0)
		{
			this.tutorialMessagesRemaining.Clear();
			this.tutorialMessagesSeen.Add(0);
			this.tutorialMessagesSeen.Add(1);
			this.tutorialMessagesSeen.Add(15);
			this.tutorialMessagesSeen.Add(11);
			if (GameUtil.GetCurrentCycle() > 1)
			{
				this.tutorialMessagesSeen.Add(6);
			}
			if (GameUtil.GetCurrentCycle() > 10)
			{
				this.tutorialMessagesSeen.Add(5);
				this.tutorialMessagesSeen.Add(10);
				this.tutorialMessagesSeen.Add(4);
				this.tutorialMessagesSeen.Add(13);
				this.tutorialMessagesSeen.Add(18);
				this.tutorialMessagesSeen.Add(7);
				this.tutorialMessagesSeen.Add(2);
			}
			if (GameUtil.GetCurrentCycle() > 30)
			{
				this.tutorialMessagesSeen.Add(8);
				this.tutorialMessagesSeen.Add(14);
				this.tutorialMessagesSeen.Add(19);
			}
			if (GameUtil.GetCurrentCycle() > 100)
			{
				this.tutorialMessagesSeen.Add(9);
				this.tutorialMessagesSeen.Add(16);
				this.tutorialMessagesSeen.Add(17);
			}
			if (BuildingInventory.Instance.BuildingCount("SuitLocker") > 0)
			{
				this.tutorialMessagesSeen.Add(12);
			}
		}
		if (this.saved_TM_COUNT < 24)
		{
			global::Debug.Log("Upgraded tutorial messages");
			this.saved_TM_COUNT = 24;
		}
		List<Tutorial.Item> list = new List<Tutorial.Item>();
		List<Tutorial.Item> list2 = list;
		Tutorial.Item item = new Tutorial.Item();
		item.notification = new Notification(MISC.NOTIFICATIONS.NEEDTOILET.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.NEEDTOILET.TOOLTIP.text, null, true, 5f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Plumbing");
		}, null, null, true, false, false);
		item.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.ToiletExists);
		list2.Add(item);
		this.itemTree.Add(list);
		List<Tutorial.Item> list3 = new List<Tutorial.Item>();
		List<Tutorial.Item> list4 = list3;
		Tutorial.Item item2 = new Tutorial.Item();
		item2.notification = new Notification(MISC.NOTIFICATIONS.NEEDFOOD.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.NEEDFOOD.TOOLTIP.text, null, true, 20f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Food");
		}, null, null, true, false, false);
		item2.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.FoodSourceExistsOnStartingWorld);
		list4.Add(item2);
		List<Tutorial.Item> list5 = list3;
		Tutorial.Item item3 = new Tutorial.Item();
		item3.notification = new Notification(MISC.NOTIFICATIONS.THERMALCOMFORT.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.THERMALCOMFORT.TOOLTIP.text, null, true, 0f, null, null, null, true, false, false);
		list5.Add(item3);
		this.itemTree.Add(list3);
		List<Tutorial.Item> list6 = new List<Tutorial.Item>();
		List<Tutorial.Item> list7 = list6;
		Tutorial.Item item4 = new Tutorial.Item();
		item4.notification = new Notification(MISC.NOTIFICATIONS.HYGENE_NEEDED.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.HYGENE_NEEDED.TOOLTIP, null, true, 20f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Medicine");
		}, null, null, true, false, false);
		item4.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.HygeneExists);
		list7.Add(item4);
		this.itemTree.Add(list6);
		List<Tutorial.Item> list8 = this.warningItems;
		Tutorial.Item item5 = new Tutorial.Item();
		item5.notification = new Notification(MISC.NOTIFICATIONS.NO_OXYGEN_GENERATOR.NAME, NotificationType.Tutorial, (List<Notification> n, object d) => MISC.NOTIFICATIONS.NO_OXYGEN_GENERATOR.TOOLTIP, null, false, 0f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Oxygen");
		}, null, null, true, false, false);
		item5.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.OxygenGeneratorBuilt);
		item5.minTimeToNotify = 80f;
		item5.lastNotifyTime = 0f;
		list8.Add(item5);
		this.warningItems.Add(new Tutorial.Item
		{
			notification = new Notification(MISC.NOTIFICATIONS.INSUFFICIENTOXYGENLASTCYCLE.NAME, NotificationType.Tutorial, new Func<List<Notification>, object, string>(this.OnOxygenTooltip), null, false, 0f, delegate(object d)
			{
				this.ZoomToNextOxygenGenerator();
			}, null, null, true, false, false),
			hideCondition = new Tutorial.HideConditionDelegate(this.OxygenGeneratorNotBuilt),
			requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.SufficientOxygenLastCycleAndThisCycle),
			minTimeToNotify = 80f,
			lastNotifyTime = 0f
		});
		this.warningItems.Add(new Tutorial.Item
		{
			notification = new Notification(MISC.NOTIFICATIONS.UNREFRIGERATEDFOOD.NAME, NotificationType.Tutorial, new Func<List<Notification>, object, string>(this.UnrefrigeratedFoodTooltip), null, false, 0f, delegate(object d)
			{
				this.ZoomToNextUnrefrigeratedFood();
			}, null, null, true, false, false),
			requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.FoodIsRefrigerated),
			minTimeToNotify = 6f,
			lastNotifyTime = 0f
		});
		List<Tutorial.Item> list9 = this.warningItems;
		Tutorial.Item item6 = new Tutorial.Item();
		item6.notification = new Notification(MISC.NOTIFICATIONS.NO_MEDICAL_COTS.NAME, NotificationType.Bad, (List<Notification> n, object o) => MISC.NOTIFICATIONS.NO_MEDICAL_COTS.TOOLTIP, null, false, 0f, delegate(object d)
		{
			PlanScreen.Instance.OpenCategoryByName("Medicine");
		}, null, null, true, false, false);
		item6.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.CanTreatSickDuplicant);
		item6.minTimeToNotify = 10f;
		item6.lastNotifyTime = 0f;
		list9.Add(item6);
		List<Tutorial.Item> list10 = this.warningItems;
		Tutorial.Item item7 = new Tutorial.Item();
		item7.notification = new Notification(string.Format(UI.ENDOFDAYREPORT.TRAVELTIMEWARNING.WARNING_TITLE, Array.Empty<object>()), NotificationType.BadMinor, (List<Notification> n, object d) => string.Format(UI.ENDOFDAYREPORT.TRAVELTIMEWARNING.WARNING_MESSAGE, GameUtil.GetFormattedPercent(40f, GameUtil.TimeSlice.None)), null, true, 0f, delegate(object d)
		{
			ManagementMenu.Instance.OpenReports(GameClock.Instance.GetCycle());
		}, null, null, true, false, false);
		item7.requirementSatisfied = new Tutorial.RequirementSatisfiedDelegate(this.LongTravelTimes);
		item7.minTimeToNotify = 1f;
		item7.lastNotifyTime = 0f;
		list10.Add(item7);
		DiscoveredResources.Instance.OnDiscover += this.OnDiscover;
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x000CDBF5 File Offset: 0x000CBDF5
	protected override void OnCleanUp()
	{
		DiscoveredResources.Instance.OnDiscover -= this.OnDiscover;
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x000CDC10 File Offset: 0x000CBE10
	private void OnDiscover(Tag category_tag, Tag tag)
	{
		Element element = ElementLoader.FindElementByHash(SimHashes.UraniumOre);
		if (element != null && tag == element.tag)
		{
			this.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
		}
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x000CDC44 File Offset: 0x000CBE44
	public Message TutorialMessage(Tutorial.TutorialMessages tm, bool queueMessage = true)
	{
		bool flag = false;
		Message message = null;
		switch (tm)
		{
		case Tutorial.TutorialMessages.TM_Basics:
			if (DistributionPlatform.Initialized && KInputManager.currentControllerIsGamepad)
			{
				message = new TutorialMessage(Tutorial.TutorialMessages.TM_Basics, MISC.NOTIFICATIONS.BASICCONTROLS.NAME, MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODYALT, MISC.NOTIFICATIONS.BASICCONTROLS.TOOLTIP, null, null, null, "", null);
			}
			else
			{
				message = new TutorialMessage(Tutorial.TutorialMessages.TM_Basics, MISC.NOTIFICATIONS.BASICCONTROLS.NAME, MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODY, MISC.NOTIFICATIONS.BASICCONTROLS.TOOLTIP, null, null, null, "", null);
			}
			break;
		case Tutorial.TutorialMessages.TM_Welcome:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Welcome, MISC.NOTIFICATIONS.WELCOMEMESSAGE.NAME, MISC.NOTIFICATIONS.WELCOMEMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.WELCOMEMESSAGE.TOOLTIP, null, null, null, "", null);
			break;
		case Tutorial.TutorialMessages.TM_StressManagement:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_StressManagement, MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.NAME, MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.STRESSMANAGEMENTMESSAGE.TOOLTIP, null, null, null, "hud_stress", null);
			break;
		case Tutorial.TutorialMessages.TM_Scheduling:
			flag = true;
			break;
		case Tutorial.TutorialMessages.TM_Mopping:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Mopping, MISC.NOTIFICATIONS.MOPPINGMESSAGE.NAME, MISC.NOTIFICATIONS.MOPPINGMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.MOPPINGMESSAGE.TOOLTIP, null, null, null, "icon_action_mop", null);
			break;
		case Tutorial.TutorialMessages.TM_Locomotion:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.NAME, MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.TOOLTIP, "tutorials\\Locomotion", "Tute_Locomotion", VIDEOS.LOCOMOTION, "action_navigable_regions", null);
			break;
		case Tutorial.TutorialMessages.TM_Priorities:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Priorities, MISC.NOTIFICATIONS.PRIORITIESMESSAGE.NAME, MISC.NOTIFICATIONS.PRIORITIESMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.PRIORITIESMESSAGE.TOOLTIP, null, null, null, "icon_action_prioritize", null);
			break;
		case Tutorial.TutorialMessages.TM_FetchingWater:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.NAME, MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.FETCHINGWATERMESSAGE.TOOLTIP, null, null, null, "element_liquid", null);
			break;
		case Tutorial.TutorialMessages.TM_ThermalComfort:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, MISC.NOTIFICATIONS.THERMALCOMFORT.NAME, MISC.NOTIFICATIONS.THERMALCOMFORT.MESSAGEBODY, MISC.NOTIFICATIONS.THERMALCOMFORT.TOOLTIP, null, null, null, "temperature", null);
			break;
		case Tutorial.TutorialMessages.TM_OverheatingBuildings:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_OverheatingBuildings, MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.NAME, MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.MESSAGEBODY, MISC.NOTIFICATIONS.TUTORIAL_OVERHEATING.TOOLTIP, null, null, null, "temperature", null);
			break;
		case Tutorial.TutorialMessages.TM_LotsOfGerms:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, MISC.NOTIFICATIONS.LOTS_OF_GERMS.NAME, MISC.NOTIFICATIONS.LOTS_OF_GERMS.MESSAGEBODY, MISC.NOTIFICATIONS.LOTS_OF_GERMS.TOOLTIP, null, null, null, "overlay_disease", null);
			break;
		case Tutorial.TutorialMessages.TM_DiseaseCooking:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_DiseaseCooking, MISC.NOTIFICATIONS.DISEASE_COOKING.NAME, MISC.NOTIFICATIONS.DISEASE_COOKING.MESSAGEBODY, MISC.NOTIFICATIONS.DISEASE_COOKING.TOOLTIP, null, null, null, "icon_category_food", null);
			break;
		case Tutorial.TutorialMessages.TM_Suits:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Suits, MISC.NOTIFICATIONS.SUITS.NAME, MISC.NOTIFICATIONS.SUITS.MESSAGEBODY, MISC.NOTIFICATIONS.SUITS.TOOLTIP, null, null, null, "overlay_suit", null);
			break;
		case Tutorial.TutorialMessages.TM_Morale:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Morale, MISC.NOTIFICATIONS.MORALE.NAME, MISC.NOTIFICATIONS.MORALE.MESSAGEBODY, MISC.NOTIFICATIONS.MORALE.TOOLTIP, "tutorials\\Morale", "Tute_Morale", VIDEOS.MORALE, "icon_category_morale", null);
			break;
		case Tutorial.TutorialMessages.TM_Schedule:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, MISC.NOTIFICATIONS.SCHEDULEMESSAGE.NAME, MISC.NOTIFICATIONS.SCHEDULEMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.SCHEDULEMESSAGE.TOOLTIP, null, null, null, "OverviewUI_schedule2_icon", null);
			break;
		case Tutorial.TutorialMessages.TM_Digging:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Digging, MISC.NOTIFICATIONS.DIGGING.NAME, MISC.NOTIFICATIONS.DIGGING.MESSAGEBODY, MISC.NOTIFICATIONS.DIGGING.TOOLTIP, "tutorials\\Digging", "Tute_Digging", VIDEOS.DIGGING, "icon_action_dig", null);
			break;
		case Tutorial.TutorialMessages.TM_Power:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Power, MISC.NOTIFICATIONS.POWER.NAME, MISC.NOTIFICATIONS.POWER.MESSAGEBODY, MISC.NOTIFICATIONS.POWER.TOOLTIP, "tutorials\\Power", "Tute_Power", VIDEOS.POWER, "overlay_power", null);
			break;
		case Tutorial.TutorialMessages.TM_Insulation:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, MISC.NOTIFICATIONS.INSULATION.NAME, MISC.NOTIFICATIONS.INSULATION.MESSAGEBODY, MISC.NOTIFICATIONS.INSULATION.TOOLTIP, "tutorials\\Insulation", "Tute_Insulation", VIDEOS.INSULATION, "icon_thermal_conductivity", null);
			break;
		case Tutorial.TutorialMessages.TM_Plumbing:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, MISC.NOTIFICATIONS.PLUMBING.NAME, MISC.NOTIFICATIONS.PLUMBING.MESSAGEBODY, MISC.NOTIFICATIONS.PLUMBING.TOOLTIP, "tutorials\\Piping", "Tute_Plumbing", VIDEOS.PLUMBING, "icon_category_plumbing", null);
			break;
		case Tutorial.TutorialMessages.TM_Radiation:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, MISC.NOTIFICATIONS.RADIATION.NAME, MISC.NOTIFICATIONS.RADIATION.MESSAGEBODY, MISC.NOTIFICATIONS.RADIATION.TOOLTIP, null, null, null, "icon_category_radiation", DlcManager.AVAILABLE_EXPANSION1_ONLY);
			break;
		case Tutorial.TutorialMessages.TM_BionicBattery:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_BionicBattery, MISC.NOTIFICATIONS.BIONICBATTERY.NAME, MISC.NOTIFICATIONS.BIONICBATTERY.MESSAGEBODY, MISC.NOTIFICATIONS.BIONICBATTERY.TOOLTIP, null, null, null, "electrobank_large", DlcManager.DLC3);
			break;
		case Tutorial.TutorialMessages.TM_GunkedToilet:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_GunkedToilet, MISC.NOTIFICATIONS.GUNKEDTOILET.NAME, MISC.NOTIFICATIONS.GUNKEDTOILET.MESSAGEBODY, MISC.NOTIFICATIONS.GUNKEDTOILET.TOOLTIP, null, null, null, "icon_plunger", DlcManager.DLC3);
			break;
		case Tutorial.TutorialMessages.TM_SlipperySurface:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_SlipperySurface, MISC.NOTIFICATIONS.SLIPPERYSURFACE.NAME, MISC.NOTIFICATIONS.SLIPPERYSURFACE.MESSAGEBODY, MISC.NOTIFICATIONS.SLIPPERYSURFACE.TOOLTIP, null, null, null, "icon_action_mop", null);
			break;
		case Tutorial.TutorialMessages.TM_BionicOil:
			message = new TutorialMessage(Tutorial.TutorialMessages.TM_BionicOil, MISC.NOTIFICATIONS.BIONICOIL.NAME, MISC.NOTIFICATIONS.BIONICOIL.MESSAGEBODY, MISC.NOTIFICATIONS.BIONICOIL.TOOLTIP, null, null, null, "icon_oil", DlcManager.DLC3);
			break;
		}
		DebugUtil.AssertArgs(message != null || flag, new object[]
		{
			"No tutorial message:",
			tm
		});
		if (queueMessage)
		{
			DebugUtil.AssertArgs(!flag, new object[]
			{
				"Attempted to queue deprecated Tutorial Message",
				tm
			});
			if (this.tutorialMessagesSeen.Contains((int)tm))
			{
				return null;
			}
			if (this.hiddenTutorialMessages.ContainsKey(tm) && this.hiddenTutorialMessages[tm])
			{
				return null;
			}
			this.tutorialMessagesSeen.Add((int)tm);
			Messenger.Instance.QueueMessage(message);
		}
		return message;
	}

	// Token: 0x060024D7 RID: 9431 RVA: 0x000CE2A4 File Offset: 0x000CC4A4
	private string OnOxygenTooltip(List<Notification> notifications, object data)
	{
		ReportManager.ReportEntry entry = ReportManager.Instance.YesterdaysReport.GetEntry(ReportManager.ReportType.OxygenCreated);
		return MISC.NOTIFICATIONS.INSUFFICIENTOXYGENLASTCYCLE.TOOLTIP.Replace("{EmittingRate}", GameUtil.GetFormattedMass(entry.Positive, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{ConsumptionRate}", GameUtil.GetFormattedMass(Mathf.Abs(entry.Negative), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x000CE30C File Offset: 0x000CC50C
	private string UnrefrigeratedFoodTooltip(List<Notification> notifications, object data)
	{
		string text = MISC.NOTIFICATIONS.UNREFRIGERATEDFOOD.TOOLTIP;
		ListPool<Pickupable, Tutorial>.PooledList pooledList = ListPool<Pickupable, Tutorial>.Allocate();
		this.GetUnrefrigeratedFood(pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			text = text + "\n" + pooledList[i].GetProperName();
		}
		pooledList.Recycle();
		return text;
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000CE364 File Offset: 0x000CC564
	private string OnLowFoodTooltip(List<Notification> notifications, object data)
	{
		global::Debug.Assert(((WorldContainer)data).id == ClusterManager.Instance.activeWorldId);
		float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ((WorldContainer)data).worldInventory, true);
		float f = (float)Components.LiveMinionIdentities.GetWorldItems(((WorldContainer)data).id, false).Count * DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE;
		return string.Format(MISC.NOTIFICATIONS.FOODLOW.TOOLTIP, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories(Mathf.Abs(f), GameUtil.TimeSlice.None, true));
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000CE3F8 File Offset: 0x000CC5F8
	public void DebugNotification()
	{
		NotificationType type;
		string text;
		if (this.debugMessageCount % 3 == 0)
		{
			type = NotificationType.Tutorial;
			text = "Warning message e.g. \"not enough oxygen\" uses Warning Color";
		}
		else if (this.debugMessageCount % 3 == 1)
		{
			type = NotificationType.BadMinor;
			text = "Normal message e.g. Idle. Uses Normal Color BG";
		}
		else
		{
			type = NotificationType.Bad;
			text = "Urgent important message. Uses Bad Color BG";
		}
		string format = "{0} ({1})";
		object arg = text;
		int num = this.debugMessageCount;
		this.debugMessageCount = num + 1;
		Notification notification = new Notification(string.Format(format, arg, num.ToString()), type, (List<Notification> n, object d) => MISC.NOTIFICATIONS.NEEDTOILET.TOOLTIP.text, null, true, 0f, null, null, null, true, false, false);
		this.notifier.Add(notification, "");
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000CE4A4 File Offset: 0x000CC6A4
	public void DebugNotificationMessage()
	{
		string str = "This is a message notification. ";
		int num = this.debugMessageCount;
		this.debugMessageCount = num + 1;
		Message message = new GenericMessage(str + num.ToString(), MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.MESSAGEBODY, MISC.NOTIFICATIONS.LOCOMOTIONMESSAGE.TOOLTIP, null);
		Messenger.Instance.QueueMessage(message);
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x000CE4F8 File Offset: 0x000CC6F8
	public void Render1000ms(float dt)
	{
		if (App.isLoading)
		{
			return;
		}
		if (Components.LiveMinionIdentities.Count == 0)
		{
			return;
		}
		if (this.itemTree.Count > 0)
		{
			List<Tutorial.Item> list = this.itemTree[0];
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Tutorial.Item item = list[i];
				if (item != null)
				{
					if (item.requirementSatisfied == null || item.requirementSatisfied())
					{
						item.notification.Clear();
						list.RemoveAt(i);
					}
					else if (item.hideCondition != null && item.hideCondition())
					{
						item.notification.Clear();
						list.RemoveAt(i);
					}
					else
					{
						this.UpdateNotifierPosition();
						this.notifier.Add(item.notification, "");
					}
				}
			}
			if (list.Count == 0)
			{
				this.itemTree.RemoveAt(0);
			}
		}
		foreach (Tutorial.Item item2 in this.warningItems)
		{
			if (item2.requirementSatisfied())
			{
				item2.notification.Clear();
				item2.lastNotifyTime = Time.time;
			}
			else if (item2.hideCondition != null && item2.hideCondition())
			{
				item2.notification.Clear();
				item2.lastNotifyTime = Time.time;
			}
			else if (item2.lastNotifyTime == 0f || Time.time - item2.lastNotifyTime > item2.minTimeToNotify)
			{
				this.notifier.Add(item2.notification, "");
				item2.lastNotifyTime = Time.time;
			}
		}
		if (GameClock.Instance.GetCycle() > 0 && !this.tutorialMessagesSeen.Contains(6) && !this.queuedPrioritiesMessage)
		{
			this.queuedPrioritiesMessage = true;
			GameScheduler.Instance.Schedule("PrioritiesTutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Priorities, true);
			}, null, null);
		}
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x000CE724 File Offset: 0x000CC924
	private bool OxygenGeneratorBuilt()
	{
		return this.oxygenGenerators.Count > 0;
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x000CE734 File Offset: 0x000CC934
	private bool OxygenGeneratorNotBuilt()
	{
		return this.oxygenGenerators.Count == 0;
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x000CE744 File Offset: 0x000CC944
	private bool SufficientOxygenLastCycleAndThisCycle()
	{
		if (ReportManager.Instance.YesterdaysReport == null)
		{
			return true;
		}
		ReportManager.ReportEntry entry = ReportManager.Instance.YesterdaysReport.GetEntry(ReportManager.ReportType.OxygenCreated);
		return ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.OxygenCreated).Net > 0.0001f || entry.Net > 0.0001f || (GameClock.Instance.GetCycle() < 1 && !GameClock.Instance.IsNighttime());
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x000CE7B9 File Offset: 0x000CC9B9
	private bool FoodIsRefrigerated()
	{
		return this.GetUnrefrigeratedFood(null) <= 0;
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x000CE7C8 File Offset: 0x000CC9C8
	private int GetUnrefrigeratedFood(List<Pickupable> foods)
	{
		int num = 0;
		if (ClusterManager.Instance.activeWorld.worldInventory != null)
		{
			ICollection<Pickupable> pickupables = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(GameTags.Edible, false);
			if (pickupables == null)
			{
				return 0;
			}
			foreach (Pickupable pickupable in pickupables)
			{
				if (pickupable.storage != null && (pickupable.storage.GetComponent<RationBox>() != null || pickupable.storage.GetComponent<Refrigerator>() != null))
				{
					Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
					if (smi != null && Rottable.RefrigerationLevel(smi) == Rottable.RotRefrigerationLevel.Normal && Rottable.AtmosphereQuality(smi) != Rottable.RotAtmosphereQuality.Sterilizing && smi != null && smi.RotConstitutionPercentage < 0.8f)
					{
						num++;
						if (foods != null)
						{
							foods.Add(pickupable);
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x000CE8BC File Offset: 0x000CCABC
	private bool EnergySourceExists()
	{
		return Game.Instance.circuitManager.HasGenerators();
	}

	// Token: 0x060024E3 RID: 9443 RVA: 0x000CE8CD File Offset: 0x000CCACD
	private bool BedExists()
	{
		return Components.NormalBeds.GlobalCount > 0;
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x000CE8DC File Offset: 0x000CCADC
	private bool EnoughFood()
	{
		int count = Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false).Count;
		float num = WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.activeWorld.worldInventory, true);
		float num2 = (float)count * FOOD.FOOD_CALORIES_PER_CYCLE;
		return num / num2 >= 1f;
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x000CE934 File Offset: 0x000CCB34
	private bool CanTreatSickDuplicant()
	{
		bool flag = Components.Clinics.Count >= 1;
		bool flag2 = false;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			using (IEnumerator<SicknessInstance> enumerator = Components.LiveMinionIdentities[i].GetSicknesses().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Sickness.severity >= Sickness.Severity.Major)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag2)
			{
				break;
			}
		}
		return !flag2 || flag;
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x000CE9C8 File Offset: 0x000CCBC8
	private bool LongTravelTimes()
	{
		if (ReportManager.Instance.reports.Count < 3)
		{
			return true;
		}
		float num = 0f;
		float num2 = 0f;
		for (int i = ReportManager.Instance.reports.Count - 1; i >= ReportManager.Instance.reports.Count - 3; i--)
		{
			ReportManager.ReportEntry entry = ReportManager.Instance.reports[i].GetEntry(ReportManager.ReportType.TravelTime);
			num += entry.Net;
			num2 += 600f * (float)entry.contextEntries.Count;
		}
		return num / num2 <= 0.4f;
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x000CEA64 File Offset: 0x000CCC64
	private bool FoodSourceExistsOnStartingWorld()
	{
		using (List<ComplexFabricator>.Enumerator enumerator = Components.ComplexFabricators.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetType() == typeof(MicrobeMusher))
				{
					return true;
				}
			}
		}
		return Components.PlantablePlots.GetItems(ClusterManager.Instance.GetStartWorld().id).Count > 0;
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x000CEAF0 File Offset: 0x000CCCF0
	private bool HygeneExists()
	{
		return Components.HandSanitizers.Count > 0;
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x000CEAFF File Offset: 0x000CCCFF
	private bool ToiletExists()
	{
		return Components.Toilets.Count > 0;
	}

	// Token: 0x060024EA RID: 9450 RVA: 0x000CEB10 File Offset: 0x000CCD10
	private void ZoomToNextOxygenGenerator()
	{
		if (this.oxygenGenerators.Count == 0)
		{
			return;
		}
		this.focusedOxygenGenerator %= this.oxygenGenerators.Count;
		GameObject gameObject = this.oxygenGenerators[this.focusedOxygenGenerator];
		if (gameObject != null)
		{
			Vector3 position = gameObject.transform.GetPosition();
			CameraController.Instance.SetTargetPos(position, 8f, true);
		}
		else
		{
			DebugUtil.DevLogErrorFormat("ZoomToNextOxygenGenerator generator was null: {0}", new object[]
			{
				gameObject
			});
		}
		this.focusedOxygenGenerator++;
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x000CEBA0 File Offset: 0x000CCDA0
	private void ZoomToNextUnrefrigeratedFood()
	{
		ListPool<Pickupable, Tutorial>.PooledList pooledList = ListPool<Pickupable, Tutorial>.Allocate();
		int unrefrigeratedFood = this.GetUnrefrigeratedFood(pooledList);
		if (pooledList.Count == 0)
		{
			return;
		}
		this.focusedUnrefrigFood++;
		if (this.focusedUnrefrigFood >= unrefrigeratedFood)
		{
			this.focusedUnrefrigFood = 0;
		}
		Pickupable pickupable = pooledList[this.focusedUnrefrigFood];
		if (pickupable != null)
		{
			CameraController.Instance.SetTargetPos(pickupable.transform.GetPosition(), 8f, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x040014D9 RID: 5337
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040014DA RID: 5338
	[Serialize]
	private int saved_TM_COUNT = 24;

	// Token: 0x040014DB RID: 5339
	[Serialize]
	private List<int> tutorialMessagesSeen = new List<int>();

	// Token: 0x040014DC RID: 5340
	[Obsolete("Contains invalid data")]
	[Serialize]
	private SerializedList<Tutorial.TutorialMessages> tutorialMessagesRemaining = new SerializedList<Tutorial.TutorialMessages>();

	// Token: 0x040014DD RID: 5341
	private const string HIDDEN_TUTORIAL_PREF_KEY_PREFIX = "HideTutorial_";

	// Token: 0x040014DE RID: 5342
	public const string HIDDEN_TUTORIAL_PREF_BUTTON_KEY = "HideTutorial_CheckState";

	// Token: 0x040014DF RID: 5343
	private Dictionary<Tutorial.TutorialMessages, bool> hiddenTutorialMessages = new Dictionary<Tutorial.TutorialMessages, bool>();

	// Token: 0x040014E0 RID: 5344
	private int debugMessageCount;

	// Token: 0x040014E1 RID: 5345
	private bool queuedPrioritiesMessage;

	// Token: 0x040014E2 RID: 5346
	private const float LOW_RATION_AMOUNT = 1f;

	// Token: 0x040014E4 RID: 5348
	private List<List<Tutorial.Item>> itemTree = new List<List<Tutorial.Item>>();

	// Token: 0x040014E5 RID: 5349
	private List<Tutorial.Item> warningItems = new List<Tutorial.Item>();

	// Token: 0x040014E6 RID: 5350
	private Vector3 notifierPosition;

	// Token: 0x040014E7 RID: 5351
	public List<GameObject> oxygenGenerators = new List<GameObject>();

	// Token: 0x040014E8 RID: 5352
	private int focusedOxygenGenerator;

	// Token: 0x040014E9 RID: 5353
	private int focusedUnrefrigFood = -1;

	// Token: 0x020013D4 RID: 5076
	public enum TutorialMessages
	{
		// Token: 0x040067F7 RID: 26615
		TM_Basics,
		// Token: 0x040067F8 RID: 26616
		TM_Welcome,
		// Token: 0x040067F9 RID: 26617
		TM_StressManagement,
		// Token: 0x040067FA RID: 26618
		TM_Scheduling,
		// Token: 0x040067FB RID: 26619
		TM_Mopping,
		// Token: 0x040067FC RID: 26620
		TM_Locomotion,
		// Token: 0x040067FD RID: 26621
		TM_Priorities,
		// Token: 0x040067FE RID: 26622
		TM_FetchingWater,
		// Token: 0x040067FF RID: 26623
		TM_ThermalComfort,
		// Token: 0x04006800 RID: 26624
		TM_OverheatingBuildings,
		// Token: 0x04006801 RID: 26625
		TM_LotsOfGerms,
		// Token: 0x04006802 RID: 26626
		TM_DiseaseCooking,
		// Token: 0x04006803 RID: 26627
		TM_Suits,
		// Token: 0x04006804 RID: 26628
		TM_Morale,
		// Token: 0x04006805 RID: 26629
		TM_Schedule,
		// Token: 0x04006806 RID: 26630
		TM_Digging,
		// Token: 0x04006807 RID: 26631
		TM_Power,
		// Token: 0x04006808 RID: 26632
		TM_Insulation,
		// Token: 0x04006809 RID: 26633
		TM_Plumbing,
		// Token: 0x0400680A RID: 26634
		TM_Radiation,
		// Token: 0x0400680B RID: 26635
		TM_BionicBattery,
		// Token: 0x0400680C RID: 26636
		TM_GunkedToilet,
		// Token: 0x0400680D RID: 26637
		TM_SlipperySurface,
		// Token: 0x0400680E RID: 26638
		TM_BionicOil,
		// Token: 0x0400680F RID: 26639
		TM_COUNT
	}

	// Token: 0x020013D5 RID: 5077
	// (Invoke) Token: 0x06008878 RID: 34936
	private delegate bool HideConditionDelegate();

	// Token: 0x020013D6 RID: 5078
	// (Invoke) Token: 0x0600887C RID: 34940
	private delegate bool RequirementSatisfiedDelegate();

	// Token: 0x020013D7 RID: 5079
	private class Item
	{
		// Token: 0x04006810 RID: 26640
		public Notification notification;

		// Token: 0x04006811 RID: 26641
		public Tutorial.HideConditionDelegate hideCondition;

		// Token: 0x04006812 RID: 26642
		public Tutorial.RequirementSatisfiedDelegate requirementSatisfied;

		// Token: 0x04006813 RID: 26643
		public float minTimeToNotify;

		// Token: 0x04006814 RID: 26644
		public float lastNotifyTime;
	}
}

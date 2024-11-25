using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007C3 RID: 1987
[AddComponentMenu("KMonoBehaviour/scripts/ColonyAchievementTracker")]
public class ColonyAchievementTracker : KMonoBehaviour, ISaveLoadableDetails, IRenderEveryTick
{
	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x060036CF RID: 14031 RVA: 0x0012A2A5 File Offset: 0x001284A5
	// (set) Token: 0x060036D0 RID: 14032 RVA: 0x0012A2B2 File Offset: 0x001284B2
	public bool GeothermalFacilityDiscovered
	{
		get
		{
			return (this.geothermalProgress & 1) == 1;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 1;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -2;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x060036D1 RID: 14033 RVA: 0x0012A2E1 File Offset: 0x001284E1
	// (set) Token: 0x060036D2 RID: 14034 RVA: 0x0012A2EE File Offset: 0x001284EE
	public bool GeothermalControllerRepaired
	{
		get
		{
			return (this.geothermalProgress & 2) == 2;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 2;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -3;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x060036D3 RID: 14035 RVA: 0x0012A31D File Offset: 0x0012851D
	// (set) Token: 0x060036D4 RID: 14036 RVA: 0x0012A32A File Offset: 0x0012852A
	public bool GeothermalControllerHasVented
	{
		get
		{
			return (this.geothermalProgress & 4) == 4;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 4;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -5;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x060036D5 RID: 14037 RVA: 0x0012A359 File Offset: 0x00128559
	// (set) Token: 0x060036D6 RID: 14038 RVA: 0x0012A366 File Offset: 0x00128566
	public bool GeothermalClearedEntombedVent
	{
		get
		{
			return (this.geothermalProgress & 8) == 8;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 8;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -9;
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x060036D7 RID: 14039 RVA: 0x0012A395 File Offset: 0x00128595
	// (set) Token: 0x060036D8 RID: 14040 RVA: 0x0012A3A4 File Offset: 0x001285A4
	public bool GeothermalVictoryPopupDismissed
	{
		get
		{
			return (this.geothermalProgress & 16) == 16;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 16;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -17;
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x060036D9 RID: 14041 RVA: 0x0012A3D4 File Offset: 0x001285D4
	public List<string> achievementsToDisplay
	{
		get
		{
			return this.completedAchievementsToDisplay;
		}
	}

	// Token: 0x060036DA RID: 14042 RVA: 0x0012A3DC File Offset: 0x001285DC
	public void ClearDisplayAchievements()
	{
		this.achievementsToDisplay.Clear();
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x0012A3EC File Offset: 0x001285EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (!this.achievements.ContainsKey(colonyAchievement.Id))
			{
				ColonyAchievementStatus value = new ColonyAchievementStatus(colonyAchievement.Id);
				this.achievements.Add(colonyAchievement.Id, value);
			}
		}
		this.forceCheckAchievementHandle = Game.Instance.Subscribe(395452326, new Action<object>(this.CheckAchievements));
		base.Subscribe<ColonyAchievementTracker>(631075836, ColonyAchievementTracker.OnNewDayDelegate);
		this.UpgradeTamedCritterAchievements();
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x0012A4B0 File Offset: 0x001286B0
	private void UpgradeTamedCritterAchievements()
	{
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in Db.Get().ColonyAchievements.TameAllBasicCritters.requirementChecklist)
		{
			CritterTypesWithTraits critterTypesWithTraits = colonyAchievementRequirement as CritterTypesWithTraits;
			if (critterTypesWithTraits != null)
			{
				critterTypesWithTraits.UpdateSavedState();
			}
		}
		foreach (ColonyAchievementRequirement colonyAchievementRequirement2 in Db.Get().ColonyAchievements.TameAGassyMoo.requirementChecklist)
		{
			CritterTypesWithTraits critterTypesWithTraits2 = colonyAchievementRequirement2 as CritterTypesWithTraits;
			if (critterTypesWithTraits2 != null)
			{
				critterTypesWithTraits2.UpdateSavedState();
			}
		}
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x0012A570 File Offset: 0x00128770
	public void RenderEveryTick(float dt)
	{
		if (this.updatingAchievement >= this.achievements.Count)
		{
			this.updatingAchievement = 0;
		}
		KeyValuePair<string, ColonyAchievementStatus> keyValuePair = this.achievements.ElementAt(this.updatingAchievement);
		this.updatingAchievement++;
		if (!keyValuePair.Value.success && !keyValuePair.Value.failed)
		{
			keyValuePair.Value.UpdateAchievement();
			if (keyValuePair.Value.success && !keyValuePair.Value.failed)
			{
				ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
				this.completedAchievementsToDisplay.Add(keyValuePair.Key);
				this.TriggerNewAchievementCompleted(keyValuePair.Key, null);
				RetireColonyUtility.SaveColonySummaryData();
			}
		}
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x0012A630 File Offset: 0x00128830
	private void CheckAchievements(object data = null)
	{
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			if (!keyValuePair.Value.success && !keyValuePair.Value.failed)
			{
				keyValuePair.Value.UpdateAchievement();
				if (keyValuePair.Value.success && !keyValuePair.Value.failed)
				{
					ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
					this.completedAchievementsToDisplay.Add(keyValuePair.Key);
					this.TriggerNewAchievementCompleted(keyValuePair.Key, null);
				}
			}
		}
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x0012A6F8 File Offset: 0x001288F8
	private static void UnlockPlatformAchievement(string achievement_id)
	{
		if (DebugHandler.InstantBuildMode)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: instant build mode", new object[]
			{
				achievement_id
			});
			return;
		}
		if (SaveGame.Instance.sandboxEnabled)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: sandbox mode", new object[]
			{
				achievement_id
			});
			return;
		}
		if (Game.Instance.debugWasUsed)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: debug was used.", new object[]
			{
				achievement_id
			});
			return;
		}
		ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.Get(achievement_id);
		if (colonyAchievement != null && !string.IsNullOrEmpty(colonyAchievement.platformAchievementId))
		{
			if (SteamAchievementService.Instance)
			{
				SteamAchievementService.Instance.Unlock(colonyAchievement.platformAchievementId);
				return;
			}
			global::Debug.LogWarningFormat("Steam achievement [{0}] was achieved, but achievement service was null", new object[]
			{
				colonyAchievement.platformAchievementId
			});
		}
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x0012A7BA File Offset: 0x001289BA
	public void DebugTriggerAchievement(string id)
	{
		this.achievements[id].failed = false;
		this.achievements[id].success = true;
	}

	// Token: 0x060036E1 RID: 14049 RVA: 0x0012A7E0 File Offset: 0x001289E0
	private void BeginVictorySequence(string achievementID)
	{
		RootMenu.Instance.canTogglePauseScreen = false;
		CameraController.Instance.DisableUserCameraControl = true;
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
		this.ToggleVictoryUI(true);
		StoryMessageScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.StoryMessageScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<StoryMessageScreen>();
		component.restoreInterfaceOnClose = false;
		component.title = COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_HEADER;
		component.body = string.Format(COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_BODY, "<b>" + Db.Get().ColonyAchievements.Get(achievementID).Name + "</b>\n" + Db.Get().ColonyAchievements.Get(achievementID).description);
		component.Show(true);
		CameraController.Instance.SetWorldInteractive(false);
		component.OnClose = (System.Action)Delegate.Combine(component.OnClose, new System.Action(delegate()
		{
			SpeedControlScreen.Instance.SetSpeed(1);
			if (!SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			CameraController.Instance.SetWorldInteractive(true);
			Db.Get().ColonyAchievements.Get(achievementID).victorySequence(this);
		}));
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x0012A924 File Offset: 0x00128B24
	public bool IsAchievementUnlocked(ColonyAchievement achievement)
	{
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			if (keyValuePair.Key == achievement.Id)
			{
				if (keyValuePair.Value.success)
				{
					return true;
				}
				keyValuePair.Value.UpdateAchievement();
				return keyValuePair.Value.success;
			}
		}
		return false;
	}

	// Token: 0x060036E3 RID: 14051 RVA: 0x0012A9B4 File Offset: 0x00128BB4
	protected override void OnCleanUp()
	{
		this.victorySchedulerHandle.ClearScheduler();
		Game.Instance.Unsubscribe(this.forceCheckAchievementHandle);
		this.checkAchievementsHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x060036E4 RID: 14052 RVA: 0x0012A9E4 File Offset: 0x00128BE4
	private void TriggerNewAchievementCompleted(string achievement, GameObject cameraTarget = null)
	{
		this.unlockedAchievementMetric[ColonyAchievementTracker.UnlockedAchievementKey] = achievement;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedAchievementMetric, "TriggerNewAchievementCompleted");
		bool flag = false;
		if (Db.Get().ColonyAchievements.Get(achievement).isVictoryCondition)
		{
			flag = true;
			this.BeginVictorySequence(achievement);
		}
		if (!flag)
		{
			AchievementEarnedMessage message = new AchievementEarnedMessage();
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x0012AA50 File Offset: 0x00128C50
	private void ToggleVictoryUI(bool victoryUIActive)
	{
		List<KScreen> list = new List<KScreen>();
		list.Add(NotificationScreen.Instance);
		list.Add(OverlayMenu.Instance);
		if (PlanScreen.Instance != null)
		{
			list.Add(PlanScreen.Instance);
		}
		if (BuildMenu.Instance != null)
		{
			list.Add(BuildMenu.Instance);
		}
		list.Add(ManagementMenu.Instance);
		list.Add(ToolMenu.Instance);
		list.Add(ToolMenu.Instance.PriorityScreen);
		list.Add(ResourceCategoryScreen.Instance);
		list.Add(TopLeftControlScreen.Instance);
		list.Add(global::DateTime.Instance);
		list.Add(BuildWatermark.Instance);
		list.Add(HoverTextScreen.Instance);
		list.Add(DetailsScreen.Instance);
		list.Add(DebugPaintElementScreen.Instance);
		list.Add(DebugBaseTemplateButton.Instance);
		list.Add(StarmapScreen.Instance);
		foreach (KScreen kscreen in list)
		{
			if (kscreen != null)
			{
				kscreen.Show(!victoryUIActive);
			}
		}
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x0012AB80 File Offset: 0x00128D80
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.achievements.Count);
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			writer.WriteKleiString(keyValuePair.Key);
			keyValuePair.Value.Serialize(writer);
		}
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x0012ABF8 File Offset: 0x00128DF8
	public void Deserialize(IReader reader)
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 10))
		{
			return;
		}
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = reader.ReadKleiString();
			ColonyAchievementStatus value = ColonyAchievementStatus.Deserialize(reader, text);
			if (Db.Get().ColonyAchievements.Exists(text))
			{
				this.achievements.Add(text, value);
			}
		}
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x0012AC60 File Offset: 0x00128E60
	public void LogFetchChore(GameObject fetcher, ChoreType choreType)
	{
		if (choreType == Db.Get().ChoreTypes.StorageFetch || choreType == Db.Get().ChoreTypes.BuildFetch || choreType == Db.Get().ChoreTypes.RepairFetch || choreType == Db.Get().ChoreTypes.FoodFetch || choreType == Db.Get().ChoreTypes.Transport)
		{
			return;
		}
		Dictionary<int, int> dictionary = null;
		if (fetcher.GetComponent<SolidTransferArm>() != null)
		{
			dictionary = this.fetchAutomatedChoreDeliveries;
		}
		else if (fetcher.GetComponent<MinionIdentity>() != null)
		{
			dictionary = this.fetchDupeChoreDeliveries;
		}
		if (dictionary != null)
		{
			int cycle = GameClock.Instance.GetCycle();
			if (!dictionary.ContainsKey(cycle))
			{
				dictionary.Add(cycle, 0);
			}
			Dictionary<int, int> dictionary2 = dictionary;
			int key = cycle;
			int num = dictionary2[key];
			dictionary2[key] = num + 1;
		}
	}

	// Token: 0x060036E9 RID: 14057 RVA: 0x0012AD29 File Offset: 0x00128F29
	public void LogCritterTamed(Tag prefabId)
	{
		this.tamedCritterTypes.Add(prefabId);
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x0012AD38 File Offset: 0x00128F38
	public void LogSuitChore(ChoreDriver driver)
	{
		if (driver == null || driver.GetComponent<MinionIdentity>() == null)
		{
			return;
		}
		bool flag = false;
		foreach (AssignableSlotInstance assignableSlotInstance in driver.GetComponent<MinionIdentity>().GetEquipment().Slots)
		{
			Equippable equippable = ((EquipmentSlotInstance)assignableSlotInstance).assignable as Equippable;
			if (equippable && equippable.GetComponent<KPrefabID>().IsAnyPrefabID(ColonyAchievementTracker.SuitTags))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			int cycle = GameClock.Instance.GetCycle();
			int instanceID = driver.GetComponent<KPrefabID>().InstanceID;
			if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
			{
				this.dupesCompleteChoresInSuits.Add(cycle, new List<int>
				{
					instanceID
				});
				return;
			}
			if (!this.dupesCompleteChoresInSuits[cycle].Contains(instanceID))
			{
				this.dupesCompleteChoresInSuits[cycle].Add(instanceID);
			}
		}
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x0012AE40 File Offset: 0x00129040
	public void LogAnalyzedSeed(Tag seed)
	{
		this.analyzedSeeds.Add(seed);
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x0012AE50 File Offset: 0x00129050
	public void OnNewDay(object data)
	{
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			if (minionStorage.GetComponent<CommandModule>() != null)
			{
				List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
				if (storedMinionInfo.Count > 0)
				{
					int cycle = GameClock.Instance.GetCycle();
					if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
					{
						this.dupesCompleteChoresInSuits.Add(cycle, new List<int>());
					}
					for (int i = 0; i < storedMinionInfo.Count; i++)
					{
						KPrefabID kprefabID = storedMinionInfo[i].serializedMinion.Get();
						if (kprefabID != null)
						{
							this.dupesCompleteChoresInSuits[cycle].Add(kprefabID.InstanceID);
						}
					}
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			SurviveARocketWithMinimumMorale surviveARocketWithMinimumMorale = Db.Get().ColonyAchievements.SurviveInARocket.requirementChecklist[0] as SurviveARocketWithMinimumMorale;
			if (surviveARocketWithMinimumMorale != null)
			{
				float minimumMorale = surviveARocketWithMinimumMorale.minimumMorale;
				int numberOfCycles = surviveARocketWithMinimumMorale.numberOfCycles;
				foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
				{
					if (worldContainer.IsModuleInterior)
					{
						if (!this.cyclesRocketDupeMoraleAboveRequirement.ContainsKey(worldContainer.id))
						{
							this.cyclesRocketDupeMoraleAboveRequirement.Add(worldContainer.id, 0);
						}
						if (worldContainer.GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Grounded)
						{
							List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(worldContainer.id, false);
							bool flag = worldItems.Count > 0;
							foreach (MinionIdentity cmp in worldItems)
							{
								if (Db.Get().Attributes.QualityOfLife.Lookup(cmp).GetTotalValue() < minimumMorale)
								{
									flag = false;
									break;
								}
							}
							this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = (flag ? (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] + 1) : 0);
						}
						else if (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] < numberOfCycles)
						{
							this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = 0;
						}
					}
				}
			}
		}
	}

	// Token: 0x04002062 RID: 8290
	public Dictionary<string, ColonyAchievementStatus> achievements = new Dictionary<string, ColonyAchievementStatus>();

	// Token: 0x04002063 RID: 8291
	[Serialize]
	public Dictionary<int, int> fetchAutomatedChoreDeliveries = new Dictionary<int, int>();

	// Token: 0x04002064 RID: 8292
	[Serialize]
	public Dictionary<int, int> fetchDupeChoreDeliveries = new Dictionary<int, int>();

	// Token: 0x04002065 RID: 8293
	[Serialize]
	public Dictionary<int, List<int>> dupesCompleteChoresInSuits = new Dictionary<int, List<int>>();

	// Token: 0x04002066 RID: 8294
	[Serialize]
	public HashSet<Tag> tamedCritterTypes = new HashSet<Tag>();

	// Token: 0x04002067 RID: 8295
	[Serialize]
	public bool defrostedDuplicant;

	// Token: 0x04002068 RID: 8296
	[Serialize]
	public HashSet<Tag> analyzedSeeds = new HashSet<Tag>();

	// Token: 0x04002069 RID: 8297
	[Serialize]
	public float totalMaterialsHarvestFromPOI;

	// Token: 0x0400206A RID: 8298
	[Serialize]
	public float radBoltTravelDistance;

	// Token: 0x0400206B RID: 8299
	[Serialize]
	public bool harvestAHiveWithoutGettingStung;

	// Token: 0x0400206C RID: 8300
	[Serialize]
	public Dictionary<int, int> cyclesRocketDupeMoraleAboveRequirement = new Dictionary<int, int>();

	// Token: 0x0400206D RID: 8301
	[Serialize]
	private int geothermalProgress;

	// Token: 0x0400206E RID: 8302
	private const int GEO_DISCOVERED_BIT = 1;

	// Token: 0x0400206F RID: 8303
	private const int GEO_CONTROLLER_REPAIRED_BIT = 2;

	// Token: 0x04002070 RID: 8304
	private const int GEO_CONTROLLER_VENTED_BIT = 4;

	// Token: 0x04002071 RID: 8305
	private const int GEO_CLEARED_ENTOMBED_BIT = 8;

	// Token: 0x04002072 RID: 8306
	private const int GEO_VICTORY_ACK_BIT = 16;

	// Token: 0x04002073 RID: 8307
	private SchedulerHandle checkAchievementsHandle;

	// Token: 0x04002074 RID: 8308
	private int forceCheckAchievementHandle = -1;

	// Token: 0x04002075 RID: 8309
	[Serialize]
	private int updatingAchievement;

	// Token: 0x04002076 RID: 8310
	[Serialize]
	private List<string> completedAchievementsToDisplay = new List<string>();

	// Token: 0x04002077 RID: 8311
	private SchedulerHandle victorySchedulerHandle;

	// Token: 0x04002078 RID: 8312
	public static readonly string UnlockedAchievementKey = "UnlockedAchievement";

	// Token: 0x04002079 RID: 8313
	private Dictionary<string, object> unlockedAchievementMetric = new Dictionary<string, object>
	{
		{
			ColonyAchievementTracker.UnlockedAchievementKey,
			null
		}
	};

	// Token: 0x0400207A RID: 8314
	private static readonly Tag[] SuitTags = new Tag[]
	{
		GameTags.AtmoSuit,
		GameTags.JetSuit,
		GameTags.LeadSuit
	};

	// Token: 0x0400207B RID: 8315
	private static readonly EventSystem.IntraObjectHandler<ColonyAchievementTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<ColonyAchievementTracker>(delegate(ColonyAchievementTracker component, object data)
	{
		component.OnNewDay(data);
	});
}

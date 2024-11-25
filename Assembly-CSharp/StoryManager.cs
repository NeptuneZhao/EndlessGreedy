using System;
using System.Collections.Generic;
using Database;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

// Token: 0x02000B20 RID: 2848
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryManager : KMonoBehaviour
{
	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x060054BA RID: 21690 RVA: 0x001E480C File Offset: 0x001E2A0C
	// (set) Token: 0x060054BB RID: 21691 RVA: 0x001E4813 File Offset: 0x001E2A13
	public static StoryManager Instance { get; private set; }

	// Token: 0x060054BC RID: 21692 RVA: 0x001E481B File Offset: 0x001E2A1B
	public static IReadOnlyList<StoryManager.StoryTelemetry> GetTelemetry()
	{
		return StoryManager.storyTelemetry;
	}

	// Token: 0x060054BD RID: 21693 RVA: 0x001E4824 File Offset: 0x001E2A24
	protected override void OnPrefabInit()
	{
		StoryManager.Instance = this;
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x060054BE RID: 21694 RVA: 0x001E487C File Offset: 0x001E2A7C
	protected override void OnCleanUp()
	{
		GameClock.Instance.Unsubscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x060054BF RID: 21695 RVA: 0x001E48CC File Offset: 0x001E2ACC
	public void InitialSaveSetup()
	{
		this.highestStoryCoordinateWhenGenerated = Db.Get().Stories.GetHighestCoordinate();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			foreach (string storyTraitTemplate in worldContainer.StoryTraitIds)
			{
				Story storyFromStoryTrait = Db.Get().Stories.GetStoryFromStoryTrait(storyTraitTemplate);
				this.CreateStory(storyFromStoryTrait, worldContainer.id);
			}
		}
		this.LogInitialSaveSetup();
	}

	// Token: 0x060054C0 RID: 21696 RVA: 0x001E4990 File Offset: 0x001E2B90
	public StoryInstance CreateStory(string id, int worldId)
	{
		Story story = Db.Get().Stories.Get(id);
		return this.CreateStory(story, worldId);
	}

	// Token: 0x060054C1 RID: 21697 RVA: 0x001E49B8 File Offset: 0x001E2BB8
	public StoryInstance CreateStory(Story story, int worldId)
	{
		StoryInstance storyInstance = new StoryInstance(story, worldId);
		this._stories.Add(story.HashId, storyInstance);
		StoryManager.InitTelemetry(storyInstance);
		if (story.autoStart)
		{
			this.BeginStoryEvent(story);
		}
		return storyInstance;
	}

	// Token: 0x060054C2 RID: 21698 RVA: 0x001E49F5 File Offset: 0x001E2BF5
	public StoryInstance GetStoryInstance(Story story)
	{
		return this.GetStoryInstance(story.HashId);
	}

	// Token: 0x060054C3 RID: 21699 RVA: 0x001E4A04 File Offset: 0x001E2C04
	public StoryInstance GetStoryInstance(int hash)
	{
		StoryInstance result;
		this._stories.TryGetValue(hash, out result);
		return result;
	}

	// Token: 0x060054C4 RID: 21700 RVA: 0x001E4A21 File Offset: 0x001E2C21
	public Dictionary<int, StoryInstance> GetStoryInstances()
	{
		return this._stories;
	}

	// Token: 0x060054C5 RID: 21701 RVA: 0x001E4A29 File Offset: 0x001E2C29
	public int GetHighestCoordinate()
	{
		return this.highestStoryCoordinateWhenGenerated;
	}

	// Token: 0x060054C6 RID: 21702 RVA: 0x001E4A31 File Offset: 0x001E2C31
	private string GetCompleteUnlockId(string id)
	{
		return id + "_STORY_COMPLETE";
	}

	// Token: 0x060054C7 RID: 21703 RVA: 0x001E4A3E File Offset: 0x001E2C3E
	public void ForceCreateStory(Story story, int worldId)
	{
		if (this.GetStoryInstance(story.HashId) == null)
		{
			this.CreateStory(story, worldId);
		}
	}

	// Token: 0x060054C8 RID: 21704 RVA: 0x001E4A58 File Offset: 0x001E2C58
	public void DiscoverStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.DISCOVERED, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.DISCOVERED;
	}

	// Token: 0x060054C9 RID: 21705 RVA: 0x001E4A88 File Offset: 0x001E2C88
	public void BeginStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.IN_PROGRESS, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.IN_PROGRESS;
	}

	// Token: 0x060054CA RID: 21706 RVA: 0x001E4AB7 File Offset: 0x001E2CB7
	public void CompleteStoryEvent(Story story, MonoBehaviour keepsakeSpawnTarget, FocusTargetSequence.Data sequenceData)
	{
		if (this.GetStoryInstance(story.HashId) == null || this.CheckState(StoryInstance.State.COMPLETE, story))
		{
			return;
		}
		FocusTargetSequence.Start(keepsakeSpawnTarget, sequenceData);
	}

	// Token: 0x060054CB RID: 21707 RVA: 0x001E4ADC File Offset: 0x001E2CDC
	public void CompleteStoryEvent(Story story, Vector3 keepsakeSpawnPosition)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null)
		{
			return;
		}
		GameObject prefab = Assets.GetPrefab(storyInstance.GetStory().keepsakePrefabId);
		if (prefab != null)
		{
			keepsakeSpawnPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			GameObject gameObject = Util.KInstantiate(prefab, keepsakeSpawnPosition);
			gameObject.SetActive(true);
			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
		}
		storyInstance.CurrentState = StoryInstance.State.COMPLETE;
		Game.Instance.unlocks.Unlock(this.GetCompleteUnlockId(story.Id), true);
	}

	// Token: 0x060054CC RID: 21708 RVA: 0x001E4B7C File Offset: 0x001E2D7C
	public bool CheckState(StoryInstance.State state, Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.CurrentState >= state;
	}

	// Token: 0x060054CD RID: 21709 RVA: 0x001E4BA7 File Offset: 0x001E2DA7
	public bool IsStoryComplete(Story story)
	{
		return this.CheckState(StoryInstance.State.COMPLETE, story);
	}

	// Token: 0x060054CE RID: 21710 RVA: 0x001E4BB1 File Offset: 0x001E2DB1
	public bool IsStoryCompleteGlobal(Story story)
	{
		return Game.Instance.unlocks.IsUnlocked(this.GetCompleteUnlockId(story.Id));
	}

	// Token: 0x060054CF RID: 21711 RVA: 0x001E4BD0 File Offset: 0x001E2DD0
	public StoryInstance DisplayPopup(Story story, StoryManager.PopupInfo info, System.Action popupCB = null, Notification.ClickCallback notificationCB = null)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || storyInstance.HasDisplayedPopup(info.PopupType))
		{
			return null;
		}
		EventInfoData eventInfoData = EventInfoDataHelper.GenerateStoryTraitData(info.Title, info.Description, info.CloseButtonText, info.TextureName, info.PopupType, info.CloseButtonToolTip, info.Minions, popupCB);
		if (info.extraButtons != null && info.extraButtons.Length != 0)
		{
			foreach (StoryManager.ExtraButtonInfo extraButtonInfo in info.extraButtons)
			{
				eventInfoData.SimpleOption(extraButtonInfo.ButtonText, extraButtonInfo.OnButtonClick).tooltip = extraButtonInfo.ButtonToolTip;
			}
		}
		Notification notification = null;
		if (!info.DisplayImmediate)
		{
			notification = EventInfoScreen.CreateNotification(eventInfoData, notificationCB);
		}
		storyInstance.SetPopupData(info, eventInfoData, notification);
		return storyInstance;
	}

	// Token: 0x060054D0 RID: 21712 RVA: 0x001E4CA0 File Offset: 0x001E2EA0
	public bool HasDisplayedPopup(Story story, EventInfoDataHelper.PopupType type)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.HasDisplayedPopup(type);
	}

	// Token: 0x060054D1 RID: 21713 RVA: 0x001E4CC8 File Offset: 0x001E2EC8
	private void LogInitialSaveSetup()
	{
		int num = 0;
		StoryManager.StoryCreationTelemetry[] array = new StoryManager.StoryCreationTelemetry[CustomGameSettings.Instance.CurrentStoryLevelsBySetting.Count];
		foreach (KeyValuePair<string, string> keyValuePair in CustomGameSettings.Instance.CurrentStoryLevelsBySetting)
		{
			array[num] = new StoryManager.StoryCreationTelemetry
			{
				StoryId = keyValuePair.Key,
				Enabled = CustomGameSettings.Instance.IsStoryActive(keyValuePair.Key, keyValuePair.Value)
			};
			num++;
		}
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "StoryTraitsCreation", array);
	}

	// Token: 0x060054D2 RID: 21714 RVA: 0x001E4D6C File Offset: 0x001E2F6C
	private void OnNewDayStarted(object _)
	{
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "SavedHighestStoryCoordinate", this.highestStoryCoordinateWhenGenerated);
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "StoryTraits", StoryManager.storyTelemetry);
	}

	// Token: 0x060054D3 RID: 21715 RVA: 0x001E4D94 File Offset: 0x001E2F94
	private static void InitTelemetry(StoryInstance story)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(story.worldId);
		if (world == null)
		{
			return;
		}
		story.Telemetry.StoryId = story.storyId;
		story.Telemetry.WorldId = world.worldName;
		StoryManager.storyTelemetry.Add(story.Telemetry);
	}

	// Token: 0x060054D4 RID: 21716 RVA: 0x001E4DF0 File Offset: 0x001E2FF0
	private void OnGameLoaded(object _)
	{
		StoryManager.storyTelemetry.Clear();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair in this._stories)
		{
			StoryManager.InitTelemetry(keyValuePair.Value);
		}
		CustomGameSettings.Instance.DisableAllStories();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair2 in this._stories)
		{
			SettingConfig config;
			if (keyValuePair2.Value.Telemetry.Retrofitted < 0f && CustomGameSettings.Instance.StorySettings.TryGetValue(keyValuePair2.Value.storyId, out config))
			{
				CustomGameSettings.Instance.SetStorySetting(config, true);
			}
		}
	}

	// Token: 0x060054D5 RID: 21717 RVA: 0x001E4EDC File Offset: 0x001E30DC
	public static void DestroyInstance()
	{
		StoryManager.storyTelemetry.Clear();
		StoryManager.Instance = null;
	}

	// Token: 0x04003790 RID: 14224
	public const int BEFORE_STORIES = -2;

	// Token: 0x04003792 RID: 14226
	private static List<StoryManager.StoryTelemetry> storyTelemetry = new List<StoryManager.StoryTelemetry>();

	// Token: 0x04003793 RID: 14227
	[Serialize]
	private Dictionary<int, StoryInstance> _stories = new Dictionary<int, StoryInstance>();

	// Token: 0x04003794 RID: 14228
	[Serialize]
	private int highestStoryCoordinateWhenGenerated = -2;

	// Token: 0x04003795 RID: 14229
	private const string STORY_TRAIT_KEY = "StoryTraits";

	// Token: 0x04003796 RID: 14230
	private const string STORY_CREATION_KEY = "StoryTraitsCreation";

	// Token: 0x04003797 RID: 14231
	private const string STORY_COORDINATE_KEY = "SavedHighestStoryCoordinate";

	// Token: 0x02001B6C RID: 7020
	public struct ExtraButtonInfo
	{
		// Token: 0x04007FB0 RID: 32688
		public string ButtonText;

		// Token: 0x04007FB1 RID: 32689
		public string ButtonToolTip;

		// Token: 0x04007FB2 RID: 32690
		public System.Action OnButtonClick;
	}

	// Token: 0x02001B6D RID: 7021
	public struct PopupInfo
	{
		// Token: 0x04007FB3 RID: 32691
		public string Title;

		// Token: 0x04007FB4 RID: 32692
		public string Description;

		// Token: 0x04007FB5 RID: 32693
		public string CloseButtonText;

		// Token: 0x04007FB6 RID: 32694
		public string CloseButtonToolTip;

		// Token: 0x04007FB7 RID: 32695
		public StoryManager.ExtraButtonInfo[] extraButtons;

		// Token: 0x04007FB8 RID: 32696
		public string TextureName;

		// Token: 0x04007FB9 RID: 32697
		public GameObject[] Minions;

		// Token: 0x04007FBA RID: 32698
		public bool DisplayImmediate;

		// Token: 0x04007FBB RID: 32699
		public EventInfoDataHelper.PopupType PopupType;
	}

	// Token: 0x02001B6E RID: 7022
	[SerializationConfig(MemberSerialization.OptIn)]
	public class StoryTelemetry : ISaveLoadable
	{
		// Token: 0x0600A368 RID: 41832 RVA: 0x00389C40 File Offset: 0x00387E40
		public void LogStateChange(StoryInstance.State state, float time)
		{
			switch (state)
			{
			case StoryInstance.State.RETROFITTED:
				this.Retrofitted = ((this.Retrofitted >= 0f) ? this.Retrofitted : time);
				return;
			case StoryInstance.State.NOT_STARTED:
				break;
			case StoryInstance.State.DISCOVERED:
				this.Discovered = ((this.Discovered >= 0f) ? this.Discovered : time);
				return;
			case StoryInstance.State.IN_PROGRESS:
				this.InProgress = ((this.InProgress >= 0f) ? this.InProgress : time);
				return;
			case StoryInstance.State.COMPLETE:
				this.Completed = ((this.Completed >= 0f) ? this.Completed : time);
				break;
			default:
				return;
			}
		}

		// Token: 0x04007FBC RID: 32700
		public string StoryId;

		// Token: 0x04007FBD RID: 32701
		public string WorldId;

		// Token: 0x04007FBE RID: 32702
		[Serialize]
		public float Retrofitted = -1f;

		// Token: 0x04007FBF RID: 32703
		[Serialize]
		public float Discovered = -1f;

		// Token: 0x04007FC0 RID: 32704
		[Serialize]
		public float InProgress = -1f;

		// Token: 0x04007FC1 RID: 32705
		[Serialize]
		public float Completed = -1f;
	}

	// Token: 0x02001B6F RID: 7023
	public class StoryCreationTelemetry
	{
		// Token: 0x04007FC2 RID: 32706
		public string StoryId;

		// Token: 0x04007FC3 RID: 32707
		public bool Enabled;
	}
}

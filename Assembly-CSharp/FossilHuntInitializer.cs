using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class FossilHuntInitializer : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x00039C48 File Offset: 0x00037E48
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inactive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Inactive.ParamTransition<bool>(this.storyCompleted, this.Active.StoryComplete, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue).ParamTransition<bool>(this.wasStoryStarted, this.Active.inProgress, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue);
		this.Active.inProgress.ParamTransition<bool>(this.storyCompleted, this.Active.StoryComplete, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue).OnSignal(this.CompleteStory, this.Active.StoryComplete);
		this.Active.StoryComplete.Enter(new StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State.Callback(FossilHuntInitializer.CompleteStoryTrait));
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00039CFC File Offset: 0x00037EFC
	public static bool OnUI_Quest_ObjectiveRowClicked(string rowLinkID)
	{
		rowLinkID = rowLinkID.ToUpper();
		if (!rowLinkID.Contains("MOVECAMERATO"))
		{
			return true;
		}
		string b = rowLinkID.Replace("MOVECAMERATO", "");
		if (Components.MajorFossilDigSites.Count > 0 && CodexCache.FormatLinkID(Components.MajorFossilDigSites[0].gameObject.PrefabID().ToString()) == b)
		{
			GameUtil.FocusCamera(Components.MajorFossilDigSites[0].transform, true);
			return false;
		}
		foreach (object obj in Components.MinorFossilDigSites)
		{
			MinorFossilDigSite.Instance instance = (MinorFossilDigSite.Instance)obj;
			if (CodexCache.FormatLinkID(instance.PrefabID().ToString()) == b)
			{
				CameraController.Instance.CameraGoTo(instance.transform.GetPosition(), 2f, true);
				SelectTool.Instance.Select(instance.gameObject.GetComponent<KSelectable>(), false);
				return false;
			}
		}
		return false;
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x00039E2C File Offset: 0x0003802C
	public static void CompleteStoryTrait(FossilHuntInitializer.Instance smi)
	{
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
		if (storyInstance == null)
		{
			return;
		}
		smi.sm.storyCompleted.Set(true, smi, false);
		if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
		{
			return;
		}
		smi.CompleteEvent();
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00039E80 File Offset: 0x00038080
	public static string ResolveStrings_QuestObjectivesRowTooltips(string originalText, object obj)
	{
		return originalText + CODEX.STORY_TRAITS.FOSSILHUNT.QUEST.LINKED_TOOLTIP;
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00039E94 File Offset: 0x00038094
	public static string ResolveQuestTitle(string title, QuestInstance quest)
	{
		int discoveredDigsitesRequired = FossilDigSiteConfig.DiscoveredDigsitesRequired;
		string str = Mathf.RoundToInt(quest.CurrentProgress * (float)discoveredDigsitesRequired).ToString() + "/" + discoveredDigsitesRequired.ToString();
		return title + " - " + str;
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00039EDC File Offset: 0x000380DC
	public static ICheckboxListGroupControl.ListGroup[] GetFossilHuntQuestData()
	{
		QuestInstance quest = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		ICheckboxListGroupControl.CheckboxItem[] checkBoxData = quest.GetCheckBoxData(null);
		for (int i = 0; i < checkBoxData.Length; i++)
		{
			checkBoxData[i].overrideLinkActions = new Func<string, bool>(FossilHuntInitializer.OnUI_Quest_ObjectiveRowClicked);
			checkBoxData[i].resolveTooltipCallback = new Func<string, object, string>(FossilHuntInitializer.ResolveStrings_QuestObjectivesRowTooltips);
		}
		if (quest != null)
		{
			return new ICheckboxListGroupControl.ListGroup[]
			{
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.FossilHuntQuest.Title, checkBoxData, (string title) => FossilHuntInitializer.ResolveQuestTitle(title, quest), null)
			};
		}
		return new ICheckboxListGroupControl.ListGroup[0];
	}

	// Token: 0x04000670 RID: 1648
	private GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State Inactive;

	// Token: 0x04000671 RID: 1649
	private FossilHuntInitializer.ActiveState Active;

	// Token: 0x04000672 RID: 1650
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.BoolParameter storyCompleted;

	// Token: 0x04000673 RID: 1651
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.BoolParameter wasStoryStarted;

	// Token: 0x04000674 RID: 1652
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.Signal CompleteStory;

	// Token: 0x04000675 RID: 1653
	public const string LINK_OVERRIDE_PREFIX = "MOVECAMERATO";

	// Token: 0x020010DF RID: 4319
	public class Def : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>.TraitDef
	{
		// Token: 0x06007D56 RID: 32086 RVA: 0x00308030 File Offset: 0x00306230
		public override void Configure(GameObject prefab)
		{
			this.Story = Db.Get().Stories.FossilHunt;
			this.CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(1, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			this.InitalLoreId = "story_trait_fossilhunt_initial";
			this.EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.NAME,
				Description = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.BUTTON,
				TextureName = "fossildigdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			this.CompleteLoreId = "story_trait_fossilhunt_complete";
			this.EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.NAME,
				Description = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.BUTTON,
				TextureName = "fossildigmining_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}

		// Token: 0x04005E3D RID: 24125
		public const string LORE_UNLOCK_PREFIX = "story_trait_fossilhunt_";

		// Token: 0x04005E3E RID: 24126
		public bool IsMainDigsite;
	}

	// Token: 0x020010E0 RID: 4320
	public class ActiveState : GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State
	{
		// Token: 0x04005E3F RID: 24127
		public GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State inProgress;

		// Token: 0x04005E40 RID: 24128
		public GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State StoryComplete;
	}

	// Token: 0x020010E1 RID: 4321
	public new class Instance : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>.TraitInstance
	{
		// Token: 0x06007D59 RID: 32089 RVA: 0x00308157 File Offset: 0x00306357
		public Instance(StateMachineController master, FossilHuntInitializer.Def def) : base(master, def)
		{
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06007D5A RID: 32090 RVA: 0x00308161 File Offset: 0x00306361
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06007D5B RID: 32091 RVA: 0x0030816D File Offset: 0x0030636D
		public string Description
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION;
			}
		}

		// Token: 0x06007D5C RID: 32092 RVA: 0x0030817C File Offset: 0x0030637C
		public override void StartSM()
		{
			base.StartSM();
			base.gameObject.GetSMI<MajorFossilDigSite>();
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance == null)
			{
				return;
			}
			if (base.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
			{
				StoryInstance.State currentState = storyInstance.CurrentState;
				if (currentState != StoryInstance.State.IN_PROGRESS)
				{
					if (currentState == StoryInstance.State.COMPLETE)
					{
						this.GoTo(base.sm.Active.StoryComplete);
					}
				}
				else
				{
					base.sm.wasStoryStarted.Set(true, this, false);
				}
			}
			StoryInstance storyInstance2 = storyInstance;
			storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		}

		// Token: 0x06007D5D RID: 32093 RVA: 0x0030823C File Offset: 0x0030643C
		protected override void OnCleanUp()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x06007D5E RID: 32094 RVA: 0x00308293 File Offset: 0x00306493
		private void OnStoryStateChanged(StoryInstance.State state)
		{
			if (state == StoryInstance.State.IN_PROGRESS)
			{
				base.sm.wasStoryStarted.Set(true, this, false);
			}
		}

		// Token: 0x06007D5F RID: 32095 RVA: 0x003082B0 File Offset: 0x003064B0
		protected override void OnObjectSelect(object clicked)
		{
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
			{
				this.RevealMajorFossilDigSites();
				this.RevealMinorFossilDigSites();
			}
			if (!(bool)clicked)
			{
				return;
			}
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
			if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE && (storyInstance.PendingType != EventInfoDataHelper.PopupType.COMPLETE || base.def.IsMainDigsite))
			{
				base.OnNotificationClicked(null);
				return;
			}
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
			{
				base.DisplayPopup(base.def.EventIntroInfo);
			}
		}

		// Token: 0x06007D60 RID: 32096 RVA: 0x00308358 File Offset: 0x00306558
		public override void OnPopupClosed()
		{
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.COMPLETE))
			{
				base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}
			base.OnPopupClosed();
		}

		// Token: 0x06007D61 RID: 32097 RVA: 0x00308380 File Offset: 0x00306580
		protected override void OnBuildingActivated(object activated)
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance == null || base.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
			{
				return;
			}
			this.RevealMinorFossilDigSites();
			this.RevealMajorFossilDigSites();
			base.OnBuildingActivated(activated);
		}

		// Token: 0x06007D62 RID: 32098 RVA: 0x003083DF File Offset: 0x003065DF
		public void RevealMajorFossilDigSites()
		{
			this.RevealAll(8, new Tag[]
			{
				"FossilDig"
			});
		}

		// Token: 0x06007D63 RID: 32099 RVA: 0x00308400 File Offset: 0x00306600
		public void RevealMinorFossilDigSites()
		{
			this.RevealAll(3, new Tag[]
			{
				"FossilResin",
				"FossilIce",
				"FossilRock"
			});
		}

		// Token: 0x06007D64 RID: 32100 RVA: 0x00308450 File Offset: 0x00306650
		private void RevealAll(int radius, params Tag[] tags)
		{
			foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(false, tags))
			{
				int baseX;
				int baseY;
				Grid.CellToXY(spawnable.cell, out baseX, out baseY);
				GridVisibility.Reveal(baseX, baseY, radius, (float)radius);
			}
		}

		// Token: 0x06007D65 RID: 32101 RVA: 0x003084C0 File Offset: 0x003066C0
		public override void OnCompleteStorySequence()
		{
			if (base.def.IsMainDigsite)
			{
				base.OnCompleteStorySequence();
			}
		}

		// Token: 0x06007D66 RID: 32102 RVA: 0x003084D8 File Offset: 0x003066D8
		public void ShowLoreUnlockedPopup(int popupID)
		{
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.NAME).AddDefaultOK(false);
			bool flag = CodexCache.GetEntryForLock(FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(popupID)) != null;
			Option<string> option = FossilDigSiteConfig.GetBodyContentForFossil(popupID);
			if (flag && option.HasValue)
			{
				infoDialogScreen.AddPlainText(option.Value).AddOption(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITFOSSILHUNT"), false);
				return;
			}
			infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
		}

		// Token: 0x06007D67 RID: 32103 RVA: 0x0030855C File Offset: 0x0030675C
		public void ShowObjectiveCompletedNotification()
		{
			FossilHuntInitializer.Instance.<>c__DisplayClass16_0 CS$<>8__locals1 = new FossilHuntInitializer.Instance.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance == null)
			{
				return;
			}
			CS$<>8__locals1.objectivesCompleted = Mathf.RoundToInt(instance.CurrentProgress * (float)instance.CriteriaCount);
			if (CS$<>8__locals1.objectivesCompleted == 0)
			{
				this.ShowFirstFossilExcavatedNotification();
				return;
			}
			string unlockID = FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(CS$<>8__locals1.objectivesCompleted);
			Game.Instance.unlocks.Unlock(unlockID, false);
			CS$<>8__locals1.<ShowObjectiveCompletedNotification>g__ShowNotificationAndWaitForClick|1().Then(delegate
			{
				CS$<>8__locals1.<>4__this.ShowLoreUnlockedPopup(CS$<>8__locals1.objectivesCompleted);
			});
		}

		// Token: 0x06007D68 RID: 32104 RVA: 0x003085F1 File Offset: 0x003067F1
		public void ShowFirstFossilExcavatedNotification()
		{
			this.<ShowFirstFossilExcavatedNotification>g__ShowNotificationAndWaitForClick|17_1().Then(delegate
			{
				this.ShowQuestUnlockedPopup();
			});
		}

		// Token: 0x06007D69 RID: 32105 RVA: 0x0030860C File Offset: 0x0030680C
		public void ShowQuestUnlockedPopup()
		{
			LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.NAME).AddDefaultOK(false).AddPlainText(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.DESCRIPTION.text.Value).AddOption(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.CHECK_BUTTON, delegate(InfoDialogScreen dialog)
			{
				dialog.Deactivate();
				GameUtil.FocusCamera(base.transform, true);
			}, false);
		}

		// Token: 0x06007D6B RID: 32107 RVA: 0x00308674 File Offset: 0x00306874
		[CompilerGenerated]
		private Promise <ShowFirstFossilExcavatedNotification>g__ShowNotificationAndWaitForClick|17_1()
		{
			return new Promise(delegate(System.Action resolve)
			{
				Notification notification = new Notification(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.NAME, NotificationType.Event, (List<Notification> notifications, object obj) => CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.TOOLTIP, null, false, 0f, delegate(object obj)
				{
					resolve();
				}, null, null, true, true, false);
				base.gameObject.AddOrGet<Notifier>().Add(notification, "");
			});
		}
	}
}

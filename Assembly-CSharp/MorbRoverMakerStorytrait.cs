using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000303 RID: 771
public class MorbRoverMakerStorytrait : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>
{
	// Token: 0x06001038 RID: 4152 RVA: 0x0005BD42 File Offset: 0x00059F42
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x040009DF RID: 2527
	public StateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, StateMachineController, MorbRoverMakerStorytrait.Def>.BoolParameter HasAnyBioBotBeenReleased;

	// Token: 0x0200112F RID: 4399
	public class Def : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitDef
	{
		// Token: 0x06007EC7 RID: 32455 RVA: 0x0030B1F4 File Offset: 0x003093F4
		public override void Configure(GameObject prefab)
		{
			this.Story = Db.Get().Stories.MorbRoverMaker;
			this.CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(0, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			this.InitalLoreId = "story_trait_morbrover_initial";
			this.EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.BUTTON,
				TextureName = "biobotdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			this.EventMachineRevealedInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_CLOSE,
				extraButtons = new StoryManager.ExtraButtonInfo[]
				{
					new StoryManager.ExtraButtonInfo
					{
						ButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_READLORE,
						OnButtonClick = delegate()
						{
							System.Action normalPopupOpenCodexButtonPressed = this.NormalPopupOpenCodexButtonPressed;
							if (normalPopupOpenCodexButtonPressed != null)
							{
								normalPopupOpenCodexButtonPressed();
							}
							this.UnlockRevealEntries();
							string entryForLock = CodexCache.GetEntryForLock(this.MachineRevealedLoreId);
							if (entryForLock == null)
							{
								DebugUtil.DevLogError("Missing codex entry for lock: " + this.MachineRevealedLoreId);
								return;
							}
							ManagementMenu.Instance.OpenCodexToEntry(entryForLock, null);
						}
					}
				},
				TextureName = "BioBotCleanedUp_kanim",
				PopupType = EventInfoDataHelper.PopupType.NORMAL
			};
			this.CompleteLoreId = "story_trait_morbrover_complete";
			this.EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.BUTTON,
				TextureName = "BioBotComplete_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}

		// Token: 0x06007EC8 RID: 32456 RVA: 0x0030B3A2 File Offset: 0x003095A2
		public void UnlockRevealEntries()
		{
			Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId, true);
			Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId2, true);
		}

		// Token: 0x04005F76 RID: 24438
		public const string LORE_UNLOCK_PREFIX = "story_trait_morbrover_";

		// Token: 0x04005F77 RID: 24439
		public string MachineRevealedLoreId = "story_trait_morbrover_reveal";

		// Token: 0x04005F78 RID: 24440
		public string MachineRevealedLoreId2 = "story_trait_morbrover_reveal_lore";

		// Token: 0x04005F79 RID: 24441
		public string CompleteLoreId2 = "story_trait_morbrover_complete_lore";

		// Token: 0x04005F7A RID: 24442
		public string CompleteLoreId3 = "story_trait_morbrover_biobot";

		// Token: 0x04005F7B RID: 24443
		public System.Action NormalPopupOpenCodexButtonPressed;

		// Token: 0x04005F7C RID: 24444
		public StoryManager.PopupInfo EventMachineRevealedInfo;
	}

	// Token: 0x02001130 RID: 4400
	public new class Instance : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitInstance
	{
		// Token: 0x06007ECB RID: 32459 RVA: 0x0030B459 File Offset: 0x00309659
		public Instance(StateMachineController master, MorbRoverMakerStorytrait.Def def) : base(master, def)
		{
			def.NormalPopupOpenCodexButtonPressed = (System.Action)Delegate.Combine(def.NormalPopupOpenCodexButtonPressed, new System.Action(this.OnNormalPopupOpenCodexButtonPressed));
		}

		// Token: 0x06007ECC RID: 32460 RVA: 0x0030B488 File Offset: 0x00309688
		public override void StartSM()
		{
			base.StartSM();
			this.machine = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			this.storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MorbRoverMaker.HashId);
			if (this.storyInstance == null)
			{
				return;
			}
			if (this.machine != null)
			{
				MorbRoverMaker.Instance instance = this.machine;
				instance.OnUncovered = (System.Action)Delegate.Combine(instance.OnUncovered, new System.Action(this.OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = this.machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Combine(instance2.OnRoverSpawned, new Action<GameObject>(this.OnRoverSpawned));
				if (this.machine.HasBeenRevealed && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE && this.storyInstance.CurrentState != StoryInstance.State.IN_PROGRESS)
				{
					base.DisplayPopup(base.def.EventMachineRevealedInfo);
				}
				if (this.machine.HasBeenRevealed && base.sm.HasAnyBioBotBeenReleased.Get(this) && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE)
				{
					this.CompleteEvent();
				}
			}
		}

		// Token: 0x06007ECD RID: 32461 RVA: 0x0030B5A0 File Offset: 0x003097A0
		private void OnMachineUncovered()
		{
			if (this.storyInstance != null && !this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				base.DisplayPopup(base.def.EventMachineRevealedInfo);
			}
		}

		// Token: 0x06007ECE RID: 32462 RVA: 0x0030B5C9 File Offset: 0x003097C9
		protected override void ShowEventNormalUI()
		{
			base.ShowEventNormalUI();
			if (this.storyInstance != null && this.storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
			{
				EventInfoScreen.ShowPopup(this.storyInstance.EventInfo);
			}
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x0030B5F8 File Offset: 0x003097F8
		public override void OnPopupClosed()
		{
			base.OnPopupClosed();
			if (this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId2, true);
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId3, true);
				return;
			}
			if (this.storyInstance != null && this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
				base.def.UnlockRevealEntries();
				return;
			}
		}

		// Token: 0x06007ED0 RID: 32464 RVA: 0x0030B679 File Offset: 0x00309879
		private void OnNormalPopupOpenCodexButtonPressed()
		{
			base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
		}

		// Token: 0x06007ED1 RID: 32465 RVA: 0x0030B682 File Offset: 0x00309882
		private void OnRoverSpawned(GameObject rover)
		{
			base.smi.sm.HasAnyBioBotBeenReleased.Set(true, base.smi, false);
			if (!this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				this.CompleteEvent();
			}
		}

		// Token: 0x06007ED2 RID: 32466 RVA: 0x0030B6B8 File Offset: 0x003098B8
		protected override void OnCleanUp()
		{
			if (this.machine != null)
			{
				MorbRoverMaker.Instance instance = this.machine;
				instance.OnUncovered = (System.Action)Delegate.Remove(instance.OnUncovered, new System.Action(this.OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = this.machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Remove(instance2.OnRoverSpawned, new Action<GameObject>(this.OnRoverSpawned));
			}
			base.OnCleanUp();
		}

		// Token: 0x04005F7D RID: 24445
		private MorbRoverMaker.Instance machine;

		// Token: 0x04005F7E RID: 24446
		private StoryInstance storyInstance;
	}
}

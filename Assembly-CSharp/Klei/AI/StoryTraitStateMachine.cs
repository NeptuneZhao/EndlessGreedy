using System;
using Database;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F75 RID: 3957
	public abstract class StoryTraitStateMachine<TStateMachine, TInstance, TDef> : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef> where TStateMachine : StoryTraitStateMachine<TStateMachine, TInstance, TDef> where TInstance : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitInstance where TDef : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitDef
	{
		// Token: 0x02002362 RID: 9058
		public class TraitDef : StateMachine.BaseDef
		{
			// Token: 0x04009E8E RID: 40590
			public string InitalLoreId;

			// Token: 0x04009E8F RID: 40591
			public string CompleteLoreId;

			// Token: 0x04009E90 RID: 40592
			public Story Story;

			// Token: 0x04009E91 RID: 40593
			public StoryCompleteData CompletionData;

			// Token: 0x04009E92 RID: 40594
			public StoryManager.PopupInfo EventIntroInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};

			// Token: 0x04009E93 RID: 40595
			public StoryManager.PopupInfo EventCompleteInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};
		}

		// Token: 0x02002363 RID: 9059
		public class TraitInstance : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef>.GameInstance
		{
			// Token: 0x0600B68F RID: 46735 RVA: 0x003CC1E4 File Offset: 0x003CA3E4
			public TraitInstance(StateMachineController master) : base(master)
			{
				StoryManager.Instance.ForceCreateStory(base.def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600B690 RID: 46736 RVA: 0x003CC244 File Offset: 0x003CA444
			public TraitInstance(StateMachineController master, TDef def) : base(master, def)
			{
				StoryManager.Instance.ForceCreateStory(def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600B691 RID: 46737 RVA: 0x003CC2A0 File Offset: 0x003CA4A0
			public override void StartSM()
			{
				this.selectable = base.GetComponent<KSelectable>();
				this.notifier = base.gameObject.AddOrGet<Notifier>();
				base.StartSM();
				base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				if (this.buildingActivatedHandle == -1)
				{
					this.buildingActivatedHandle = base.master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				}
				this.TriggerStoryEvent(StoryInstance.State.DISCOVERED);
			}

			// Token: 0x0600B692 RID: 46738 RVA: 0x003CC31B File Offset: 0x003CA51B
			public override void StopSM(string reason)
			{
				base.StopSM(reason);
				base.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				base.Unsubscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				this.buildingActivatedHandle = -1;
			}

			// Token: 0x0600B693 RID: 46739 RVA: 0x003CC35C File Offset: 0x003CA55C
			public void TriggerStoryEvent(StoryInstance.State storyEvent)
			{
				switch (storyEvent)
				{
				case StoryInstance.State.RETROFITTED:
				case StoryInstance.State.NOT_STARTED:
					return;
				case StoryInstance.State.DISCOVERED:
					StoryManager.Instance.DiscoverStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.IN_PROGRESS:
					StoryManager.Instance.BeginStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.COMPLETE:
				{
					Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.KeepSakeSpawnOffset), Grid.SceneLayer.Ore);
					StoryManager.Instance.CompleteStoryEvent(base.def.Story, keepsakeSpawnPosition);
					return;
				}
				default:
					throw new NotImplementedException(storyEvent.ToString());
				}
			}

			// Token: 0x0600B694 RID: 46740 RVA: 0x003CC419 File Offset: 0x003CA619
			protected virtual void OnBuildingActivated(object activated)
			{
				if (!(bool)activated)
				{
					return;
				}
				this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}

			// Token: 0x0600B695 RID: 46741 RVA: 0x003CC42C File Offset: 0x003CA62C
			protected virtual void OnObjectSelect(object clicked)
			{
				if (!(bool)clicked)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE)
				{
					this.OnNotificationClicked(null);
					return;
				}
				if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
				{
					this.DisplayPopup(base.def.EventIntroInfo);
				}
			}

			// Token: 0x0600B696 RID: 46742 RVA: 0x003CC4AC File Offset: 0x003CA6AC
			public virtual void CompleteEvent()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
				{
					return;
				}
				this.DisplayPopup(base.def.EventCompleteInfo);
			}

			// Token: 0x0600B697 RID: 46743 RVA: 0x003CC4FC File Offset: 0x003CA6FC
			public virtual void OnCompleteStorySequence()
			{
				this.TriggerStoryEvent(StoryInstance.State.COMPLETE);
			}

			// Token: 0x0600B698 RID: 46744 RVA: 0x003CC508 File Offset: 0x003CA708
			protected void DisplayPopup(StoryManager.PopupInfo info)
			{
				if (info.PopupType == EventInfoDataHelper.PopupType.NONE)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.DisplayPopup(base.def.Story, info, new System.Action(this.OnPopupClosed), new Notification.ClickCallback(this.OnNotificationClicked));
				if (storyInstance != null && !info.DisplayImmediate)
				{
					this.selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
					this.notifier.Add(storyInstance.Notification, "");
				}
			}

			// Token: 0x0600B699 RID: 46745 RVA: 0x003CC59C File Offset: 0x003CA79C
			public void OnNotificationClicked(object data = null)
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				this.selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
				this.notifier.Remove(storyInstance.Notification);
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.COMPLETE)
				{
					this.ShowEventCompleteUI();
					return;
				}
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
				{
					this.ShowEventNormalUI();
					return;
				}
				this.ShowEventBeginUI();
			}

			// Token: 0x0600B69A RID: 46746 RVA: 0x003CC620 File Offset: 0x003CA820
			public virtual void OnPopupClosed()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
				{
					Game.Instance.unlocks.Unlock(base.def.CompleteLoreId, true);
					return;
				}
				Game.Instance.unlocks.Unlock(base.def.InitalLoreId, true);
			}

			// Token: 0x0600B69B RID: 46747 RVA: 0x003CC69B File Offset: 0x003CA89B
			protected virtual void ShowEventBeginUI()
			{
			}

			// Token: 0x0600B69C RID: 46748 RVA: 0x003CC69D File Offset: 0x003CA89D
			protected virtual void ShowEventNormalUI()
			{
			}

			// Token: 0x0600B69D RID: 46749 RVA: 0x003CC6A0 File Offset: 0x003CA8A0
			protected virtual void ShowEventCompleteUI()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
				StoryManager.Instance.CompleteStoryEvent(base.def.Story, base.master, new FocusTargetSequence.Data
				{
					WorldId = base.master.GetMyWorldId(),
					OrthographicSize = 6f,
					TargetSize = 6f,
					Target = target,
					PopupData = storyInstance.EventInfo,
					CompleteCB = new System.Action(this.OnCompleteStorySequence),
					CanCompleteCB = null
				});
			}

			// Token: 0x04009E94 RID: 40596
			protected int buildingActivatedHandle = -1;

			// Token: 0x04009E95 RID: 40597
			protected Notifier notifier;

			// Token: 0x04009E96 RID: 40598
			protected KSelectable selectable;
		}
	}
}

using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200057F RID: 1407
public class LonelyMinion : GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>
{
	// Token: 0x060020AD RID: 8365 RVA: 0x000B67B0 File Offset: 0x000B49B0
	private bool HahCheckedMail(LonelyMinion.Instance smi)
	{
		if (smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL)
		{
			if (this.Mail.Get(smi) == smi.gameObject)
			{
				this.Mail.Set(null, smi, true);
				smi.AnimController.Play(LonelyMinionConfig.CHECK_MAIL_FAILURE, KAnim.PlayMode.Once, 1f, 0f);
				return false;
			}
			this.CheckForMail(smi);
			return false;
		}
		else
		{
			if (smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_FAILURE || smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_DUPLICATE)
			{
				smi.Drop();
				return false;
			}
			return smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_FAILURE || smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_SUCCESS || smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_DUPLICATE;
		}
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x000B68A0 File Offset: 0x000B4AA0
	private void CheckForMail(LonelyMinion.Instance smi)
	{
		Tag prefabTag = this.Mail.Get(smi).GetComponent<KPrefabID>().PrefabTag;
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		global::Debug.Assert(instance != null);
		Quest.ItemData data2 = new Quest.ItemData
		{
			CriteriaId = LonelyMinionConfig.FoodCriteriaId,
			SatisfyingItem = prefabTag,
			QualifyingTag = GameTags.Edible,
			CurrentValue = (float)EdiblesManager.GetFoodInfo(prefabTag.Name).Quality
		};
		LonelyMinion.MailStates mailStates = smi.GetCurrentState() as LonelyMinion.MailStates;
		bool flag;
		bool flag2;
		instance.TrackProgress(data2, out flag, out flag2);
		StateMachine.BaseState baseState = mailStates.Success;
		string title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.TASTYFOOD.NAME;
		string tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.TASTYFOOD.TOOLTIP;
		if (flag2)
		{
			baseState = mailStates.Duplicate;
			title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.REPEATEDFOOD.NAME;
			tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.REPEATEDFOOD.TOOLTIP;
		}
		else if (!flag)
		{
			baseState = mailStates.Failure;
			title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.CRAPPYFOOD.NAME;
			tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.CRAPPYFOOD.TOOLTIP;
		}
		Pickupable component = this.Mail.Get(smi).GetComponent<Pickupable>();
		smi.Pickup(component, baseState != mailStates.Success);
		smi.ScheduleGoTo(0.016f, baseState);
		Notification notification = new Notification(title, NotificationType.Event, (List<Notification> notificationList, object data) => data as string, tooltip_data, false, 0f, null, null, smi.transform.parent, true, true, true);
		smi.transform.parent.gameObject.AddOrGet<Notifier>().Add(notification, "");
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x000B6A48 File Offset: 0x000B4C48
	private void EvaluateCurrentDecor(LonelyMinion.Instance smi, float dt)
	{
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
		if (smi.GetCurrentState() == this.Inactive || instance.IsComplete)
		{
			return;
		}
		float num = LonelyMinionHouse.CalculateAverageDecor(smi.def.DecorInspectionArea);
		bool flag = num >= 0f || (num > smi.StartingAverageDecor && 1f - num / smi.StartingAverageDecor > 0.01f);
		if (!instance.IsStarted && !flag)
		{
			return;
		}
		bool flag2;
		bool flag3;
		instance.TrackProgress(new Quest.ItemData
		{
			CriteriaId = LonelyMinionConfig.DecorCriteriaId,
			CurrentValue = num
		}, out flag2, out flag3);
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x000B6B00 File Offset: 0x000B4D00
	private void DelayIdle(LonelyMinion.Instance smi, float dt)
	{
		if (smi.AnimController.currentAnim != smi.AnimController.defaultAnim)
		{
			return;
		}
		if (smi.IdleDelayTimer > 0f)
		{
			smi.IdleDelayTimer -= dt;
		}
		if (smi.IdleDelayTimer <= 0f)
		{
			this.PlayIdle(smi, smi.ChooseIdle());
			smi.IdleDelayTimer = UnityEngine.Random.Range(20f, 40f);
		}
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x000B6B7C File Offset: 0x000B4D7C
	private void PlayIdle(LonelyMinion.Instance smi, HashedString idleAnim)
	{
		if (!idleAnim.IsValid)
		{
			return;
		}
		if (idleAnim == LonelyMinionConfig.CHECK_MAIL)
		{
			this.Mail.Set(smi.gameObject, smi, false);
			return;
		}
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		int num = instance.GetCurrentCount(LonelyMinionConfig.FoodCriteriaId) - 1;
		if (idleAnim == LonelyMinionConfig.FOOD_IDLE && num >= 0)
		{
			KBatchedAnimController component = Assets.GetPrefab(instance.GetSatisfyingItem(LonelyMinionConfig.FoodCriteriaId, UnityEngine.Random.Range(0, num))).GetComponent<KBatchedAnimController>();
			smi.PackageSnapPoint.SwapAnims(component.AnimFiles);
			smi.PackageSnapPoint.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}
		if (!(idleAnim == LonelyMinionConfig.FOOD_IDLE) && !(idleAnim == LonelyMinionConfig.DECOR_IDLE) && !(idleAnim == LonelyMinionConfig.POWER_IDLE))
		{
			LonelyMinionHouse.Instance smi2 = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			smi.AnimController.GetSynchronizer().Remove(smi2.AnimController);
			if (idleAnim == LonelyMinionConfig.BLINDS_IDLE_0)
			{
				smi2.BlindsController.Play(LonelyMinionConfig.BLINDS_IDLE_0, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
		smi.AnimController.Play(idleAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x000B6CD4 File Offset: 0x000B4ED4
	private void OnIdleAnimComplete(LonelyMinion.Instance smi)
	{
		if (smi.AnimController.currentAnim == smi.AnimController.defaultAnim)
		{
			return;
		}
		if (!(smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_IDLE) && !(smi.AnimController.currentAnim == LonelyMinionConfig.DECOR_IDLE) && !(smi.AnimController.currentAnim == LonelyMinionConfig.POWER_IDLE))
		{
			LonelyMinionHouse.Instance smi2 = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			smi.AnimController.GetSynchronizer().Add(smi2.AnimController);
			if (smi.AnimController.currentAnim == LonelyMinionConfig.BLINDS_IDLE_0)
			{
				smi2.BlindsController.Play(string.Format("{0}_{1}", "meter_blinds", 0), KAnim.PlayMode.Paused, 1f, 0f);
			}
		}
		smi.AnimController.Play(smi.AnimController.defaultAnim, smi.AnimController.initialMode, 1f, 0f);
		if (this.Active.Get(smi) && this.Mail.Get(smi) != null)
		{
			smi.ScheduleGoTo(0f, this.CheckMail);
		}
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x000B6E20 File Offset: 0x000B5020
	private void OnBecomeInactive(LonelyMinion.Instance smi)
	{
		smi.AnimController.GetSynchronizer().Clear();
		smi.AnimController.Play(smi.AnimController.initialAnim, smi.AnimController.initialMode, 1f, 0f);
	}

	// Token: 0x060020B4 RID: 8372 RVA: 0x000B6E70 File Offset: 0x000B5070
	private void OnBecomeActive(LonelyMinion.Instance smi)
	{
		LonelyMinionHouse.Instance smi2 = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
		if (smi2 == null)
		{
			return;
		}
		smi.AnimController.GetSynchronizer().Add(smi2.AnimController);
		if (smi.StartingAverageDecor == float.NegativeInfinity)
		{
			smi.StartingAverageDecor = LonelyMinionHouse.CalculateAverageDecor(smi.def.DecorInspectionArea);
		}
	}

	// Token: 0x060020B5 RID: 8373 RVA: 0x000B6ECC File Offset: 0x000B50CC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inactive;
		this.root.ParamTransition<bool>(this.Active, this.Inactive, (LonelyMinion.Instance smi, bool p) => !this.Active.Get(smi)).ParamTransition<bool>(this.Active, this.Idle, (LonelyMinion.Instance smi, bool p) => this.Active.Get(smi)).Update(new Action<LonelyMinion.Instance, float>(this.EvaluateCurrentDecor), UpdateRate.SIM_1000ms, false);
		this.Inactive.Enter(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(this.OnBecomeInactive)).Exit(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(this.OnBecomeActive));
		this.Idle.ParamTransition<GameObject>(this.Mail, this.CheckMail, (LonelyMinion.Instance smi, GameObject p) => smi.AnimController.currentAnim == smi.AnimController.defaultAnim && this.Mail.Get(smi) != null).Update(new Action<LonelyMinion.Instance, float>(this.DelayIdle), UpdateRate.SIM_EVERY_TICK, false).EventHandler(GameHashes.AnimQueueComplete, new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(this.OnIdleAnimComplete)).Exit(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(this.OnIdleAnimComplete));
		this.CheckMail.Enter(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(LonelyMinion.MailStates.OnEnter)).ParamTransition<GameObject>(this.Mail, this.Idle, (LonelyMinion.Instance smi, GameObject p) => this.Mail.Get(smi) == null).EventTransition(GameHashes.AnimQueueComplete, this.Idle, new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.Transition.ConditionCallback(this.HahCheckedMail)).Exit(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(LonelyMinion.MailStates.OnExit));
		this.CheckMail.Success.Enter(delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_SUCCESS);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_SUCCESS);
		});
		this.CheckMail.Failure.Enter(delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_FAILURE);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_FAILURE);
		});
		this.CheckMail.Duplicate.Enter(delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_DUPLICATE);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_DUPLICATE);
		});
	}

	// Token: 0x0400125A RID: 4698
	public StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.TargetParameter Mail;

	// Token: 0x0400125B RID: 4699
	public StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.BoolParameter Active;

	// Token: 0x0400125C RID: 4700
	public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Idle;

	// Token: 0x0400125D RID: 4701
	public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Inactive;

	// Token: 0x0400125E RID: 4702
	public LonelyMinion.MailStates CheckMail;

	// Token: 0x02001375 RID: 4981
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040066AA RID: 26282
		public Personality Personality;

		// Token: 0x040066AB RID: 26283
		public HashedString QuestOwnerId;

		// Token: 0x040066AC RID: 26284
		public Extents DecorInspectionArea;
	}

	// Token: 0x02001376 RID: 4982
	public new class Instance : GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.GameInstance
	{
		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06008734 RID: 34612 RVA: 0x0032AE73 File Offset: 0x00329073
		public KBatchedAnimController AnimController
		{
			get
			{
				return this.animControllers[0];
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06008735 RID: 34613 RVA: 0x0032AE7D File Offset: 0x0032907D
		public KBatchedAnimController PackageSnapPoint
		{
			get
			{
				return this.animControllers[1];
			}
		}

		// Token: 0x06008736 RID: 34614 RVA: 0x0032AE88 File Offset: 0x00329088
		public Instance(StateMachineController master, LonelyMinion.Def def) : base(master, def)
		{
			this.animControllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>(true);
			this.storage = base.GetComponent<Storage>();
			global::Debug.Assert(def.Personality != null);
			base.GetComponent<Accessorizer>().ApplyMinionPersonality(def.Personality);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
			storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		}

		// Token: 0x06008737 RID: 34615 RVA: 0x0032AF30 File Offset: 0x00329130
		public override void StartSM()
		{
			LonelyMinionHouse.Instance smi = base.smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			base.smi.AnimController.GetSynchronizer().Add(smi.AnimController);
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.ShowQuestCompleteNotification));
			base.smi.IdleDelayTimer = UnityEngine.Random.Range(20f, 40f);
			this.InitializeIdles();
			base.StartSM();
		}

		// Token: 0x06008738 RID: 34616 RVA: 0x0032AFD4 File Offset: 0x003291D4
		public override void StopSM(string reason)
		{
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.ShowQuestCompleteNotification));
			this.StoryCleanUp();
			base.StopSM(reason);
			this.ResetHandle.ClearScheduler();
			this.ResetHandle.FreeResources();
		}

		// Token: 0x06008739 RID: 34617 RVA: 0x0032B044 File Offset: 0x00329244
		public HashedString ChooseIdle()
		{
			if (this.availableIdles.Count > 1)
			{
				this.availableIdles.Shuffle<HashedString>();
			}
			return this.availableIdles[0];
		}

		// Token: 0x0600873A RID: 34618 RVA: 0x0032B06C File Offset: 0x0032926C
		public void Pickup(Pickupable pickupable, bool store)
		{
			base.sm.Mail.Set(null, this, true);
			pickupable.storage.GetComponent<SingleEntityReceptacle>().OrderRemoveOccupant();
			this.PackageSnapPoint.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
			if (store)
			{
				this.storage.Store(pickupable.gameObject, true, true, false, false);
				return;
			}
			UnityEngine.Object.Destroy(pickupable.gameObject);
		}

		// Token: 0x0600873B RID: 34619 RVA: 0x0032B0E4 File Offset: 0x003292E4
		public void Drop()
		{
			this.storage.DropAll(this.PackageSnapPoint.transform.position, false, false, default(Vector3), true, null);
		}

		// Token: 0x0600873C RID: 34620 RVA: 0x0032B119 File Offset: 0x00329319
		private void OnStoryStateChanged(StoryInstance.State state)
		{
			if (state != StoryInstance.State.COMPLETE)
			{
				return;
			}
			this.StoryCleanUp();
		}

		// Token: 0x0600873D RID: 34621 RVA: 0x0032B128 File Offset: 0x00329328
		private void StoryCleanUp()
		{
			this.AnimController.GetSynchronizer().Clear();
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
			storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		}

		// Token: 0x0600873E RID: 34622 RVA: 0x0032B184 File Offset: 0x00329384
		private void InitializeIdles()
		{
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			if (instance.IsStarted)
			{
				this.availableIdles.Add(LonelyMinionConfig.FOOD_IDLE);
				if (!instance.IsComplete)
				{
					this.availableIdles.Add(LonelyMinionConfig.CHECK_MAIL);
				}
			}
			instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			if (instance.IsStarted)
			{
				this.availableIdles.Add(LonelyMinionConfig.DECOR_IDLE);
			}
			instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			if (instance.IsStarted)
			{
				this.availableIdles.Add(LonelyMinionConfig.POWER_IDLE);
			}
			LonelyMinionHouse.Instance smi = base.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			LonelyMinionHouse lonelyMinionHouse = smi.GetStateMachine() as LonelyMinionHouse;
			float num = 3f * lonelyMinionHouse.QuestProgress.Get(smi);
			int num2 = Mathf.Approximately((float)Mathf.CeilToInt(num), num) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num);
			if (num2 == 0)
			{
				this.availableIdles.Add(LonelyMinionConfig.BLINDS_IDLE_0);
				return;
			}
			int num3 = 1;
			while (num3 <= num2 && num3 != 3)
			{
				this.availableIdles.Add(string.Format("{0}_{1}", "idle_blinds", num3));
				num3++;
			}
		}

		// Token: 0x0600873F RID: 34623 RVA: 0x0032B2F4 File Offset: 0x003294F4
		public void UnlockQuestIdle(QuestInstance quest, Quest.State prevState, float delta)
		{
			if (prevState == Quest.State.NotStarted && quest.IsStarted)
			{
				if (quest.Id == Db.Get().Quests.LonelyMinionFoodQuest.IdHash)
				{
					this.availableIdles.Add(LonelyMinionConfig.FOOD_IDLE);
				}
				else if (quest.Id == Db.Get().Quests.LonelyMinionDecorQuest.IdHash)
				{
					this.availableIdles.Add(LonelyMinionConfig.DECOR_IDLE);
				}
				else
				{
					this.availableIdles.Add(LonelyMinionConfig.POWER_IDLE);
				}
			}
			if (!quest.IsComplete)
			{
				return;
			}
			if (quest.Id == Db.Get().Quests.LonelyMinionFoodQuest.IdHash)
			{
				this.availableIdles.Remove(LonelyMinionConfig.CHECK_MAIL);
			}
			LonelyMinionHouse.Instance smi = base.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			LonelyMinionHouse lonelyMinionHouse = smi.GetStateMachine() as LonelyMinionHouse;
			float num = 3f * lonelyMinionHouse.QuestProgress.Get(smi);
			int num2 = Mathf.Approximately((float)Mathf.CeilToInt(num), num) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num);
			if (num2 > 0 && num2 < 3)
			{
				this.availableIdles.Add(string.Format("{0}_{1}", "idle_blinds", num2));
			}
			this.availableIdles.Remove(LonelyMinionConfig.BLINDS_IDLE_0);
		}

		// Token: 0x06008740 RID: 34624 RVA: 0x0032B44C File Offset: 0x0032964C
		public void ShowQuestCompleteNotification(QuestInstance quest, Quest.State prevState, float delta = 0f)
		{
			if (!quest.IsComplete)
			{
				return;
			}
			string text = string.Empty;
			if (quest.Id != Db.Get().Quests.LonelyMinionGreetingQuest.IdHash)
			{
				text = "story_trait_lonelyminion_" + quest.Name.ToLower();
				Game.Instance.unlocks.Unlock(text, false);
			}
			Notification notification = new Notification(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.NAME, NotificationType.Event, null, null, false, 0f, new Notification.ClickCallback(this.ShowQuestCompletePopup), new global::Tuple<string, string>(text, quest.CompletionText), null, true, true, true);
			base.transform.parent.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}

		// Token: 0x06008741 RID: 34625 RVA: 0x0032B508 File Offset: 0x00329708
		private void ShowQuestCompletePopup(object data)
		{
			global::Tuple<string, string> tuple = data as global::Tuple<string, string>;
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.NAME).AddPlainText(tuple.second).AddDefaultOK(false);
			if (!string.IsNullOrEmpty(tuple.first))
			{
				infoDialogScreen.AddOption(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByLockKeyID(tuple.first, true), false);
			}
		}

		// Token: 0x040066AD RID: 26285
		public SchedulerHandle ResetHandle;

		// Token: 0x040066AE RID: 26286
		public float StartingAverageDecor = float.NegativeInfinity;

		// Token: 0x040066AF RID: 26287
		public float IdleDelayTimer;

		// Token: 0x040066B0 RID: 26288
		private KBatchedAnimController[] animControllers;

		// Token: 0x040066B1 RID: 26289
		private Storage storage;

		// Token: 0x040066B2 RID: 26290
		private const int maxIdles = 8;

		// Token: 0x040066B3 RID: 26291
		private List<HashedString> availableIdles = new List<HashedString>(8);
	}

	// Token: 0x02001377 RID: 4983
	public class MailStates : GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State
	{
		// Token: 0x06008742 RID: 34626 RVA: 0x0032B570 File Offset: 0x00329770
		public static void OnEnter(LonelyMinion.Instance smi)
		{
			KBatchedAnimController component = smi.sm.Mail.Get(smi).GetComponent<KBatchedAnimController>();
			smi.PackageSnapPoint.gameObject.SetActive(component.gameObject != smi.gameObject);
			if (smi.PackageSnapPoint.gameObject.activeSelf)
			{
				smi.PackageSnapPoint.SwapAnims(component.AnimFiles);
			}
			smi.AnimController.Play(LonelyMinionConfig.CHECK_MAIL, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06008743 RID: 34627 RVA: 0x0032B5F3 File Offset: 0x003297F3
		public static void OnExit(LonelyMinion.Instance smi)
		{
			smi.ResetHandle = smi.ScheduleNextFrame(new Action<object>(LonelyMinion.MailStates.ResetState), smi);
		}

		// Token: 0x06008744 RID: 34628 RVA: 0x0032B610 File Offset: 0x00329810
		private static void ResetState(object data)
		{
			LonelyMinion.Instance instance = data as LonelyMinion.Instance;
			instance.AnimController.Play(instance.AnimController.initialAnim, instance.AnimController.initialMode, 1f, 0f);
			instance.Drop();
		}

		// Token: 0x06008745 RID: 34629 RVA: 0x0032B65A File Offset: 0x0032985A
		public static void PlayAnims(LonelyMinion.Instance smi, HashedString anim)
		{
			if (anim.IsValid)
			{
				smi.AnimController.Queue(anim, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			smi.GoTo(smi.sm.Idle);
		}

		// Token: 0x040066B4 RID: 26292
		public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Success;

		// Token: 0x040066B5 RID: 26293
		public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Failure;

		// Token: 0x040066B6 RID: 26294
		public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Duplicate;
	}
}

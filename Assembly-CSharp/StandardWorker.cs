using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
[AddComponentMenu("KMonoBehaviour/scripts/Worker")]
public class StandardWorker : WorkerBase
{
	// Token: 0x06002322 RID: 8994 RVA: 0x000C3FDF File Offset: 0x000C21DF
	public override WorkerBase.State GetState()
	{
		return this.state;
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000C3FE7 File Offset: 0x000C21E7
	public override WorkerBase.StartWorkInfo GetStartWorkInfo()
	{
		return this.startWorkInfo;
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x000C3FEF File Offset: 0x000C21EF
	public override Workable GetWorkable()
	{
		if (this.startWorkInfo != null)
		{
			return this.startWorkInfo.workable;
		}
		return null;
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x000C4006 File Offset: 0x000C2206
	public override KBatchedAnimController GetAnimController()
	{
		return base.GetComponent<KBatchedAnimController>();
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000C400E File Offset: 0x000C220E
	public override AttributeConverterInstance GetAttributeConverter(string id)
	{
		return base.GetComponent<AttributeConverters>().GetConverter(id);
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000C401C File Offset: 0x000C221C
	public override Guid OfferStatusItem(StatusItem item, object data = null)
	{
		return base.GetComponent<KSelectable>().AddStatusItem(item, data);
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000C402B File Offset: 0x000C222B
	public override void RevokeStatusItem(Guid id)
	{
		base.GetComponent<KSelectable>().RemoveStatusItem(id, false);
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000C403B File Offset: 0x000C223B
	public override void SetWorkCompleteData(object data)
	{
		this.workCompleteData = data;
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x000C4044 File Offset: 0x000C2244
	public override bool UsesMultiTool()
	{
		return this.usesMultiTool;
	}

	// Token: 0x0600232B RID: 9003 RVA: 0x000C404C File Offset: 0x000C224C
	public override bool IsFetchDrone()
	{
		return this.isFetchDrone;
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x000C4054 File Offset: 0x000C2254
	public override CellOffset[] GetFetchCellOffsets()
	{
		if (this.fetchOffsets.Length == 0)
		{
			return null;
		}
		return this.fetchOffsets;
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x000C4067 File Offset: 0x000C2267
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.state = WorkerBase.State.Idle;
		base.Subscribe<StandardWorker>(1485595942, StandardWorker.OnChoreInterruptDelegate);
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000C4087 File Offset: 0x000C2287
	private string GetWorkableDebugString()
	{
		if (this.GetWorkable() == null)
		{
			return "Null";
		}
		return this.GetWorkable().name;
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x000C40A8 File Offset: 0x000C22A8
	public void CompleteWork()
	{
		this.successFullyCompleted = false;
		this.state = WorkerBase.State.Idle;
		Workable workable = this.GetWorkable();
		if (workable != null)
		{
			if (workable.triggerWorkReactions && workable.GetWorkTime() > 30f)
			{
				string conversationTopic = workable.GetConversationTopic();
				if (!conversationTopic.IsNullOrWhiteSpace())
				{
					this.CreateCompletionReactable(conversationTopic);
				}
			}
			this.DetachAnimOverrides();
			workable.CompleteWork(this);
			if (workable.worker != null && !(workable is Constructable) && !(workable is Deconstructable) && !(workable is Repairable) && !(workable is Disinfectable))
			{
				BonusEvent.GameplayEventData gameplayEventData = new BonusEvent.GameplayEventData();
				gameplayEventData.workable = workable;
				gameplayEventData.worker = workable.worker;
				gameplayEventData.building = workable.GetComponent<BuildingComplete>();
				gameplayEventData.eventTrigger = GameHashes.UseBuilding;
				GameplayEventManager.Instance.Trigger(1175726587, gameplayEventData);
			}
		}
		this.InternalStopWork(workable, false);
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000C4184 File Offset: 0x000C2384
	protected virtual void TryPlayingIdle()
	{
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
			if (navTypeData.idleAnim.IsValid)
			{
				base.GetComponent<KAnimControllerBase>().Play(navTypeData.idleAnim, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x000C41E0 File Offset: 0x000C23E0
	public override WorkerBase.WorkResult Work(float dt)
	{
		if (this.state == WorkerBase.State.PendingCompletion)
		{
			bool flag = Time.time - this.workPendingCompletionTime > 10f;
			if (!base.GetComponent<KAnimControllerBase>().IsStopped() && !flag)
			{
				return WorkerBase.WorkResult.InProgress;
			}
			this.TryPlayingIdle();
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return WorkerBase.WorkResult.Success;
			}
			this.StopWork();
			return WorkerBase.WorkResult.Failed;
		}
		else
		{
			if (this.state != WorkerBase.State.Completing)
			{
				Workable workable = this.GetWorkable();
				if (workable != null)
				{
					if (this.facing)
					{
						if (workable.ShouldFaceTargetWhenWorking())
						{
							this.facing.Face(workable.GetFacingTarget());
						}
						else
						{
							Rotatable component = workable.GetComponent<Rotatable>();
							bool flag2 = component != null && component.GetOrientation() == Orientation.FlipH;
							Vector3 vector = this.facing.transform.GetPosition();
							vector += (flag2 ? Vector3.left : Vector3.right);
							this.facing.Face(vector);
						}
					}
					if (dt > 0f && Game.Instance.FastWorkersModeActive)
					{
						dt = Mathf.Min(workable.WorkTimeRemaining + 0.01f, 5f);
					}
					Klei.AI.Attribute workAttribute = workable.GetWorkAttribute();
					AttributeLevels component2 = base.GetComponent<AttributeLevels>();
					if (workAttribute != null && workAttribute.IsTrainable && component2 != null)
					{
						float attributeExperienceMultiplier = workable.GetAttributeExperienceMultiplier();
						component2.AddExperience(workAttribute.Id, dt, attributeExperienceMultiplier);
					}
					string skillExperienceSkillGroup = workable.GetSkillExperienceSkillGroup();
					if (this.experienceRecipient != null && skillExperienceSkillGroup != null)
					{
						float skillExperienceMultiplier = workable.GetSkillExperienceMultiplier();
						this.experienceRecipient.AddExperienceWithAptitude(skillExperienceSkillGroup, dt, skillExperienceMultiplier);
					}
					float efficiencyMultiplier = workable.GetEfficiencyMultiplier(this);
					float dt2 = dt * efficiencyMultiplier * 1f;
					if (workable.WorkTick(this, dt2) && this.state == WorkerBase.State.Working)
					{
						this.successFullyCompleted = true;
						this.StartPlayingPostAnim();
						workable.OnPendingCompleteWork(this);
					}
				}
				return WorkerBase.WorkResult.InProgress;
			}
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return WorkerBase.WorkResult.Success;
			}
			this.StopWork();
			return WorkerBase.WorkResult.Failed;
		}
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000C43C4 File Offset: 0x000C25C4
	private void StartPlayingPostAnim()
	{
		Workable workable = this.GetWorkable();
		if (workable != null && !workable.alwaysShowProgressBar)
		{
			workable.ShowProgressBar(false);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
		this.state = WorkerBase.State.PendingCompletion;
		this.workPendingCompletionTime = Time.time;
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		HashedString[] workPstAnims = workable.GetWorkPstAnims(this, this.successFullyCompleted);
		if (this.smi == null)
		{
			if (workPstAnims != null && workPstAnims.Length != 0)
			{
				if (workable != null && workable.synchronizeAnims)
				{
					KAnimControllerBase animController = workable.GetAnimController();
					if (animController != null)
					{
						animController.Play(workPstAnims, KAnim.PlayMode.Once);
					}
				}
				else
				{
					component.Play(workPstAnims, KAnim.PlayMode.Once);
				}
			}
			else
			{
				this.state = WorkerBase.State.Completing;
			}
		}
		base.Trigger(-1142962013, this);
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x000C4480 File Offset: 0x000C2680
	protected virtual void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		this.state = WorkerBase.State.Idle;
		base.gameObject.RemoveTag(GameTags.PerformingWorkRequest);
		base.GetComponent<KAnimControllerBase>().Offset -= this.workAnimOffset;
		this.workAnimOffset = Vector3.zero;
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
		this.DetachAnimOverrides();
		this.ClearPasserbyReactable();
		AnimEventHandler component = base.GetComponent<AnimEventHandler>();
		if (component)
		{
			component.ClearContext();
		}
		if (this.previousStatusItem.item != null)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, this.previousStatusItem.item, this.previousStatusItem.data);
		}
		if (target_workable != null)
		{
			target_workable.Unsubscribe(this.onWorkChoreDisabledHandle);
			target_workable.StopWork(this, is_aborted);
		}
		if (this.smi != null)
		{
			this.smi.StopSM("stopping work");
			this.smi = null;
		}
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
		this.startWorkInfo = null;
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x000C45A4 File Offset: 0x000C27A4
	private void OnChoreInterrupt(object data)
	{
		if (this.state == WorkerBase.State.Working)
		{
			this.successFullyCompleted = false;
			this.StartPlayingPostAnim();
		}
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000C45BC File Offset: 0x000C27BC
	private void OnWorkChoreDisabled(object data)
	{
		string text = data as string;
		ChoreConsumer component = base.GetComponent<ChoreConsumer>();
		if (component != null && component.choreDriver != null)
		{
			component.choreDriver.GetCurrentChore().Fail((text != null) ? text : "WorkChoreDisabled");
		}
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000C460C File Offset: 0x000C280C
	public override void StopWork()
	{
		Workable workable = this.GetWorkable();
		if (this.state == WorkerBase.State.PendingCompletion || this.state == WorkerBase.State.Completing)
		{
			this.state = WorkerBase.State.Idle;
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				base.Trigger(1705586602, this);
			}
			else
			{
				base.Trigger(-993481695, this);
				this.InternalStopWork(workable, true);
			}
		}
		else if (this.state == WorkerBase.State.Working)
		{
			if (workable != null && workable.synchronizeAnims)
			{
				KAnimControllerBase animController = workable.GetAnimController();
				if (animController != null)
				{
					HashedString[] workPstAnims = workable.GetWorkPstAnims(this, false);
					if (workPstAnims != null && workPstAnims.Length != 0)
					{
						animController.Play(workPstAnims, KAnim.PlayMode.Once);
						animController.SetPositionPercent(1f);
					}
				}
			}
			base.Trigger(-993481695, this);
			this.InternalStopWork(workable, true);
		}
		base.Trigger(2027193395, this);
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000C46D8 File Offset: 0x000C28D8
	public override void StartWork(WorkerBase.StartWorkInfo start_work_info)
	{
		this.startWorkInfo = start_work_info;
		Game.Instance.StartedWork();
		Workable workable = this.GetWorkable();
		if (this.state != WorkerBase.State.Idle)
		{
			string text = "";
			if (workable != null)
			{
				text = workable.name;
			}
			global::Debug.LogError(string.Concat(new string[]
			{
				base.name,
				".",
				text,
				".state should be idle but instead it's:",
				this.state.ToString()
			}));
		}
		string name = workable.GetType().Name;
		try
		{
			base.gameObject.AddTag(GameTags.PerformingWorkRequest);
			this.state = WorkerBase.State.Working;
			base.gameObject.Trigger(1568504979, this);
			if (workable != null)
			{
				this.animInfo = workable.GetAnim(this);
				if (this.animInfo.smi != null)
				{
					this.smi = this.animInfo.smi;
					this.smi.StartSM();
				}
				Vector3 position = base.transform.GetPosition();
				position.z = Grid.GetLayerZ(workable.workLayer);
				base.transform.SetPosition(position);
				KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
				if (this.animInfo.smi == null)
				{
					this.AttachOverrideAnims(component);
				}
				HashedString[] workAnims = workable.GetWorkAnims(this);
				KAnim.PlayMode workAnimPlayMode = workable.GetWorkAnimPlayMode();
				Vector3 workOffset = workable.GetWorkOffset();
				this.workAnimOffset = workOffset;
				component.Offset += workOffset;
				if (this.usesMultiTool && this.animInfo.smi == null && workAnims != null && workAnims.Length != 0 && this.experienceRecipient != null)
				{
					if (workable.synchronizeAnims)
					{
						KAnimControllerBase animController = workable.GetAnimController();
						if (animController != null)
						{
							this.kanimSynchronizer = animController.GetSynchronizer();
							if (this.kanimSynchronizer != null)
							{
								this.kanimSynchronizer.Add(component);
							}
						}
						animController.Play(workAnims, workAnimPlayMode);
					}
					else
					{
						component.Play(workAnims, workAnimPlayMode);
					}
				}
			}
			workable.StartWork(this);
			if (workable == null)
			{
				global::Debug.LogWarning("Stopped work as soon as I started. This is usually a sign that a chore is open when it shouldn't be or that it's preconditions are wrong.");
			}
			else
			{
				this.onWorkChoreDisabledHandle = workable.Subscribe(2108245096, new Action<object>(this.OnWorkChoreDisabled));
				if (workable.triggerWorkReactions && workable.WorkTimeRemaining > 10f)
				{
					this.CreatePasserbyReactable();
				}
				KSelectable component2 = base.GetComponent<KSelectable>();
				this.previousStatusItem = component2.GetStatusItem(Db.Get().StatusItemCategories.Main);
				component2.SetStatusItem(Db.Get().StatusItemCategories.Main, workable.GetWorkerStatusItem(), workable);
			}
		}
		catch (Exception ex)
		{
			string str = "Exception in: Worker.StartWork(" + name + ")";
			DebugUtil.LogErrorArgs(this, new object[]
			{
				str + "\n" + ex.ToString()
			});
			throw;
		}
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000C49C0 File Offset: 0x000C2BC0
	private void Update()
	{
		if (this.state == WorkerBase.State.Working)
		{
			this.ForceSyncAnims();
		}
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000C49D1 File Offset: 0x000C2BD1
	private void ForceSyncAnims()
	{
		if (Time.deltaTime > 0f && this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.SyncTime();
		}
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000C49F4 File Offset: 0x000C2BF4
	public override bool InstantlyFinish()
	{
		Workable workable = this.GetWorkable();
		return workable != null && workable.InstantlyFinish(this);
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000C4A1C File Offset: 0x000C2C1C
	private void AttachOverrideAnims(KAnimControllerBase worker_controller)
	{
		if (this.animInfo.overrideAnims != null && this.animInfo.overrideAnims.Length != 0)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				worker_controller.AddAnimOverrides(this.animInfo.overrideAnims[i], 0f);
			}
		}
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000C4A74 File Offset: 0x000C2C74
	private void DetachAnimOverrides()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.RemoveWithoutIdleAnim(component);
			this.kanimSynchronizer = null;
		}
		if (this.animInfo.overrideAnims != null)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				component.RemoveAnimOverrides(this.animInfo.overrideAnims[i]);
			}
			this.animInfo.overrideAnims = null;
		}
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000C4AE8 File Offset: 0x000C2CE8
	private void CreateCompletionReactable(string topic)
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		EmoteReactable emoteReactable = OneshotReactableLocator.CreateOneshotReactable(base.gameObject, 3f, "WorkCompleteAcknowledgement", Db.Get().ChoreTypes.Emote, 9, 5, 100f);
		Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
		emoteReactable.SetEmote(clapCheer);
		emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.GetReactionEffect), null).RegisterEmoteStepCallbacks("clapcheer_pst", null, delegate(GameObject r)
		{
			r.Trigger(937885943, topic);
		});
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(topic, "ui", true);
		if (uisprite != null)
		{
			Thought thought = new Thought("Completion_" + topic, null, uisprite.first, "mode_satisfaction", "conversation_short", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, "", true, 4f);
			emoteReactable.SetThought(thought);
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000C4C00 File Offset: 0x000C2E00
	private void CreatePasserbyReactable()
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		if (this.passerbyReactable == null)
		{
			EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 30f, 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier, float.PositiveInfinity, 0f);
			Emote thumbsUp = Db.Get().Emotes.Minion.ThumbsUp;
			emoteReactable.SetEmote(thumbsUp).SetThought(Db.Get().Thoughts.Encourage).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsFacingMe)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsntPartying));
			emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.GetReactionEffect), null);
			this.passerbyReactable = emoteReactable;
		}
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000C4D00 File Offset: 0x000C2F00
	private void GetReactionEffect(GameObject reactor)
	{
		Effects component = base.GetComponent<Effects>();
		if (component != null)
		{
			component.Add("WorkEncouraged", true);
		}
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x000C4D2A File Offset: 0x000C2F2A
	private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
	{
		return transition.end == NavType.Floor;
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000C4D38 File Offset: 0x000C2F38
	private bool ReactorIsFacingMe(GameObject reactor, Navigator.ActiveTransition transition)
	{
		Facing component = reactor.GetComponent<Facing>();
		return base.transform.GetPosition().x < reactor.transform.GetPosition().x == component.GetFacing();
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000C4D78 File Offset: 0x000C2F78
	private bool ReactorIsntPartying(GameObject reactor, Navigator.ActiveTransition transition)
	{
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		return component.choreDriver.HasChore() && component.choreDriver.GetCurrentChore().choreType != Db.Get().ChoreTypes.Party;
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000C4DBF File Offset: 0x000C2FBF
	private void ClearPasserbyReactable()
	{
		if (this.passerbyReactable != null)
		{
			this.passerbyReactable.Cleanup();
			this.passerbyReactable = null;
		}
	}

	// Token: 0x040013FA RID: 5114
	private WorkerBase.State state;

	// Token: 0x040013FB RID: 5115
	private WorkerBase.StartWorkInfo startWorkInfo;

	// Token: 0x040013FC RID: 5116
	private const float EARLIEST_REACT_TIME = 1f;

	// Token: 0x040013FD RID: 5117
	[MyCmpGet]
	private Facing facing;

	// Token: 0x040013FE RID: 5118
	[MyCmpGet]
	private IExperienceRecipient experienceRecipient;

	// Token: 0x040013FF RID: 5119
	private float workPendingCompletionTime;

	// Token: 0x04001400 RID: 5120
	private int onWorkChoreDisabledHandle;

	// Token: 0x04001401 RID: 5121
	public object workCompleteData;

	// Token: 0x04001402 RID: 5122
	private Workable.AnimInfo animInfo;

	// Token: 0x04001403 RID: 5123
	private KAnimSynchronizer kanimSynchronizer;

	// Token: 0x04001404 RID: 5124
	private StatusItemGroup.Entry previousStatusItem;

	// Token: 0x04001405 RID: 5125
	private StateMachine.Instance smi;

	// Token: 0x04001406 RID: 5126
	private bool successFullyCompleted;

	// Token: 0x04001407 RID: 5127
	private Vector3 workAnimOffset = Vector3.zero;

	// Token: 0x04001408 RID: 5128
	public bool usesMultiTool = true;

	// Token: 0x04001409 RID: 5129
	public bool isFetchDrone;

	// Token: 0x0400140A RID: 5130
	public CellOffset[] fetchOffsets = new CellOffset[0];

	// Token: 0x0400140B RID: 5131
	private static readonly EventSystem.IntraObjectHandler<StandardWorker> OnChoreInterruptDelegate = new EventSystem.IntraObjectHandler<StandardWorker>(delegate(StandardWorker component, object data)
	{
		component.OnChoreInterrupt(data);
	});

	// Token: 0x0400140C RID: 5132
	private Reactable passerbyReactable;
}

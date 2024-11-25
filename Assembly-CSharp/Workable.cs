using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005F7 RID: 1527
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Workable")]
public class Workable : KMonoBehaviour, ISaveLoadable, IApproachable
{
	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x0600252B RID: 9515 RVA: 0x000D04D1 File Offset: 0x000CE6D1
	// (set) Token: 0x0600252C RID: 9516 RVA: 0x000D04D9 File Offset: 0x000CE6D9
	public WorkerBase worker { get; protected set; }

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x0600252D RID: 9517 RVA: 0x000D04E2 File Offset: 0x000CE6E2
	// (set) Token: 0x0600252E RID: 9518 RVA: 0x000D04EA File Offset: 0x000CE6EA
	public float WorkTimeRemaining
	{
		get
		{
			return this.workTimeRemaining;
		}
		set
		{
			this.workTimeRemaining = value;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x0600252F RID: 9519 RVA: 0x000D04F3 File Offset: 0x000CE6F3
	// (set) Token: 0x06002530 RID: 9520 RVA: 0x000D04FB File Offset: 0x000CE6FB
	public bool preferUnreservedCell { get; set; }

	// Token: 0x06002531 RID: 9521 RVA: 0x000D0504 File Offset: 0x000CE704
	public virtual float GetWorkTime()
	{
		return this.workTime;
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x000D050C File Offset: 0x000CE70C
	public WorkerBase GetWorker()
	{
		return this.worker;
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x000D0514 File Offset: 0x000CE714
	public virtual float GetPercentComplete()
	{
		if (this.workTimeRemaining > this.workTime)
		{
			return -1f;
		}
		return 1f - this.workTimeRemaining / this.workTime;
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x000D053D File Offset: 0x000CE73D
	public void ConfigureMultitoolContext(HashedString context, Tag hitEffectTag)
	{
		this.multitoolContext = context;
		this.multitoolHitEffectTag = hitEffectTag;
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x000D0550 File Offset: 0x000CE750
	public virtual Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		if (this.overrideAnims != null && this.overrideAnims.Length != 0)
		{
			BuildingFacade buildingFacade = this.GetBuildingFacade();
			bool flag = false;
			if (buildingFacade != null && !buildingFacade.IsOriginal)
			{
				flag = buildingFacade.interactAnims.TryGetValue(base.name, out result.overrideAnims);
			}
			if (!flag)
			{
				result.overrideAnims = this.overrideAnims;
			}
		}
		if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
		{
			result.smi = new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
		}
		return result;
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x000D05F3 File Offset: 0x000CE7F3
	public virtual HashedString[] GetWorkAnims(WorkerBase worker)
	{
		return this.workAnims;
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x000D05FB File Offset: 0x000CE7FB
	public virtual KAnim.PlayMode GetWorkAnimPlayMode()
	{
		return this.workAnimPlayMode;
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000D0603 File Offset: 0x000CE803
	public virtual HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		if (successfully_completed)
		{
			return this.workingPstComplete;
		}
		return this.workingPstFailed;
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000D0615 File Offset: 0x000CE815
	public virtual Vector3 GetWorkOffset()
	{
		return Vector3.zero;
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000D061C File Offset: 0x000CE81C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().MiscStatusItems.Using;
		this.workingStatusItem = Db.Get().MiscStatusItems.Operating;
		this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.RequiresSkillPerk;
		this.workTime = this.GetWorkTime();
		this.workTimeRemaining = Mathf.Min(this.workTimeRemaining, this.workTime);
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x000D0694 File Offset: 0x000CE894
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			if (this.skillsUpdateHandle != -1)
			{
				Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			}
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		if (this.requireMinionToWork && this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		this.minionUpdateHandle = Game.Instance.Subscribe(586301400, new Action<object>(this.UpdateStatusItem));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		if (base.gameObject.HasTag(this.laboratoryEfficiencyBonusTagRequired))
		{
			this.useLaboratoryEfficiencyBonus = true;
			base.Subscribe<Workable>(144050788, Workable.OnUpdateRoomDelegate);
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		this.UpdateStatusItem(null);
	}

	// Token: 0x0600253C RID: 9532 RVA: 0x000D079C File Offset: 0x000CE99C
	private void RefreshRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x000D07E4 File Offset: 0x000CE9E4
	private void OnUpdateRoom(object data)
	{
		if (this.worker == null)
		{
			return;
		}
		Room room = (Room)data;
		if (room != null && room.roomType == Db.Get().RoomTypes.Laboratory)
		{
			this.currentlyInLaboratory = true;
			if (this.laboratoryEfficiencyBonusStatusItemHandle == Guid.Empty)
			{
				this.laboratoryEfficiencyBonusStatusItemHandle = this.worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LaboratoryWorkEfficiencyBonus, this);
				return;
			}
		}
		else
		{
			this.currentlyInLaboratory = false;
			if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
			{
				this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
				this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
			}
		}
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x000D0894 File Offset: 0x000CEA94
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		component.RemoveStatusItem(this.workStatusItemHandle, false);
		if (this.worker == null)
		{
			if (this.requireMinionToWork && Components.LiveMinionIdentities.GetWorldItems(this.GetMyWorldId(), false).Count == 0)
			{
				this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.WorkRequiresMinion, null);
				return;
			}
			if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
			{
				if (!MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId()))
				{
					StatusItem status_item = DlcManager.FeatureClusterSpaceEnabled() ? Db.Get().BuildingStatusItems.ClusterColonyLacksRequiredSkillPerk : Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk;
					this.workStatusItemHandle = component.AddStatusItem(status_item, this.requiredSkillPerk);
					return;
				}
				this.workStatusItemHandle = component.AddStatusItem(this.readyForSkillWorkStatusItem, this.requiredSkillPerk);
				return;
			}
		}
		else if (this.workingStatusItem != null)
		{
			this.workStatusItemHandle = component.AddStatusItem(this.workingStatusItem, this);
		}
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x000D09AC File Offset: 0x000CEBAC
	protected override void OnLoadLevel()
	{
		this.overrideAnims = null;
		base.OnLoadLevel();
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x000D09BB File Offset: 0x000CEBBB
	public virtual int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x000D09C4 File Offset: 0x000CEBC4
	public void StartWork(WorkerBase worker_to_start)
	{
		global::Debug.Assert(worker_to_start != null, "How did we get a null worker?");
		this.worker = worker_to_start;
		this.UpdateStatusItem(null);
		if (this.showProgressBar)
		{
			this.ShowProgressBar(true);
		}
		if (this.useLaboratoryEfficiencyBonus)
		{
			this.RefreshRoom();
		}
		this.OnStartWork(this.worker);
		if (this.worker != null)
		{
			string conversationTopic = this.GetConversationTopic();
			if (conversationTopic != null)
			{
				this.worker.Trigger(937885943, conversationTopic);
			}
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStarted);
		}
		this.numberOfUses++;
		if (this.worker != null)
		{
			if (base.gameObject.GetComponent<KSelectable>() != null && base.gameObject.GetComponent<KSelectable>().IsSelected && this.worker.gameObject.GetComponent<LoopingSounds>() != null)
			{
				this.worker.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
			else if (this.worker.gameObject.GetComponent<KSelectable>() != null && this.worker.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
			{
				base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
		}
		base.gameObject.Trigger(853695848, this);
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x000D0B30 File Offset: 0x000CED30
	public bool WorkTick(WorkerBase worker, float dt)
	{
		bool flag = false;
		if (dt > 0f)
		{
			this.workTimeRemaining -= dt;
			flag = this.OnWorkTick(worker, dt);
		}
		return flag || this.workTimeRemaining < 0f;
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x000D0B70 File Offset: 0x000CED70
	public virtual float GetEfficiencyMultiplier(WorkerBase worker)
	{
		float num = 1f;
		if (this.attributeConverter != null)
		{
			AttributeConverterInstance attributeConverterInstance = worker.GetAttributeConverter(this.attributeConverter.Id);
			if (attributeConverterInstance != null)
			{
				num += attributeConverterInstance.Evaluate();
			}
		}
		if (this.lightEfficiencyBonus)
		{
			int num2 = Grid.PosToCell(worker.gameObject);
			if (Grid.IsValidCell(num2))
			{
				if (Grid.LightIntensity[num2] > DUPLICANTSTATS.STANDARD.Light.NO_LIGHT)
				{
					this.currentlyLit = true;
					num += DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS;
					if (this.lightEfficiencyBonusStatusItemHandle == Guid.Empty)
					{
						this.lightEfficiencyBonusStatusItemHandle = worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus, this);
					}
				}
				else
				{
					this.currentlyLit = false;
					if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
					{
						worker.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
					}
				}
			}
		}
		if (this.useLaboratoryEfficiencyBonus && this.currentlyInLaboratory)
		{
			num += 0.1f;
		}
		return Mathf.Max(num, this.minimumAttributeMultiplier);
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000D0C79 File Offset: 0x000CEE79
	public virtual Klei.AI.Attribute GetWorkAttribute()
	{
		if (this.attributeConverter != null)
		{
			return this.attributeConverter.attribute;
		}
		return null;
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x000D0C90 File Offset: 0x000CEE90
	public virtual string GetConversationTopic()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (!component.HasTag(GameTags.NotConversationTopic))
		{
			return component.PrefabTag.Name;
		}
		return null;
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x000D0CBE File Offset: 0x000CEEBE
	public float GetAttributeExperienceMultiplier()
	{
		return this.attributeExperienceMultiplier;
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x000D0CC6 File Offset: 0x000CEEC6
	public string GetSkillExperienceSkillGroup()
	{
		return this.skillExperienceSkillGroup;
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x000D0CCE File Offset: 0x000CEECE
	public float GetSkillExperienceMultiplier()
	{
		return this.skillExperienceMultiplier;
	}

	// Token: 0x06002549 RID: 9545 RVA: 0x000D0CD6 File Offset: 0x000CEED6
	protected virtual bool OnWorkTick(WorkerBase worker, float dt)
	{
		return false;
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x000D0CDC File Offset: 0x000CEEDC
	public void StopWork(WorkerBase workerToStop, bool aborted)
	{
		if (this.worker == workerToStop && aborted)
		{
			this.OnAbortWork(workerToStop);
		}
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(workerToStop);
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStopped);
		}
		this.OnStopWork(workerToStop);
		if (this.resetProgressOnStop)
		{
			this.workTimeRemaining = this.GetWorkTime();
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			workerToStop.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
			this.lightEfficiencyBonusStatusItemHandle = Guid.Empty;
		}
		if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
			this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
		}
		if (base.gameObject.GetComponent<KSelectable>() != null && !base.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
		{
			base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		else if (workerToStop.gameObject.GetComponent<KSelectable>() != null && !workerToStop.gameObject.GetComponent<KSelectable>().IsSelected && workerToStop.gameObject.GetComponent<LoopingSounds>() != null)
		{
			workerToStop.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		this.worker = null;
		base.gameObject.Trigger(679550494, this);
		this.UpdateStatusItem(null);
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x000D0E68 File Offset: 0x000CF068
	public virtual StatusItem GetWorkerStatusItem()
	{
		return this.workerStatusItem;
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x000D0E70 File Offset: 0x000CF070
	public void SetWorkerStatusItem(StatusItem item)
	{
		this.workerStatusItem = item;
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x000D0E7C File Offset: 0x000CF07C
	public void CompleteWork(WorkerBase worker)
	{
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(worker);
		}
		this.OnCompleteWork(worker);
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkCompleted);
		}
		this.workTimeRemaining = this.GetWorkTime();
		this.ShowProgressBar(false);
		base.gameObject.Trigger(-2011693419, this);
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x000D0ED8 File Offset: 0x000CF0D8
	public void SetReportType(ReportManager.ReportType report_type)
	{
		this.reportType = report_type;
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x000D0EE1 File Offset: 0x000CF0E1
	public ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x000D0EE9 File Offset: 0x000CF0E9
	protected virtual void OnStartWork(WorkerBase worker)
	{
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x000D0EEB File Offset: 0x000CF0EB
	protected virtual void OnStopWork(WorkerBase worker)
	{
	}

	// Token: 0x06002552 RID: 9554 RVA: 0x000D0EED File Offset: 0x000CF0ED
	protected virtual void OnCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x06002553 RID: 9555 RVA: 0x000D0EEF File Offset: 0x000CF0EF
	protected virtual void OnAbortWork(WorkerBase worker)
	{
	}

	// Token: 0x06002554 RID: 9556 RVA: 0x000D0EF1 File Offset: 0x000CF0F1
	public virtual void OnPendingCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x000D0EF3 File Offset: 0x000CF0F3
	public void SetOffsets(CellOffset[] offsets)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new StandardOffsetTracker(offsets);
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x000D0F14 File Offset: 0x000CF114
	public void SetOffsetTable(CellOffset[][] offset_table)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new OffsetTableTracker(offset_table, this);
	}

	// Token: 0x06002557 RID: 9559 RVA: 0x000D0F36 File Offset: 0x000CF136
	public virtual CellOffset[] GetOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.GetOffsets(cell);
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x000D0F5D File Offset: 0x000CF15D
	public virtual bool ValidateOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.ValidateOffsets(cell);
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x000D0F84 File Offset: 0x000CF184
	public CellOffset[] GetOffsets()
	{
		return this.GetOffsets(Grid.PosToCell(this));
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x000D0F92 File Offset: 0x000CF192
	public void SetWorkTime(float work_time)
	{
		this.workTime = work_time;
		this.workTimeRemaining = work_time;
	}

	// Token: 0x0600255B RID: 9563 RVA: 0x000D0FA2 File Offset: 0x000CF1A2
	public bool ShouldFaceTargetWhenWorking()
	{
		return this.faceTargetWhenWorking;
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x000D0FAA File Offset: 0x000CF1AA
	public virtual Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x0600255D RID: 9565 RVA: 0x000D0FB8 File Offset: 0x000CF1B8
	public void ShowProgressBar(bool show)
	{
		if (show)
		{
			if (this.progressBar == null)
			{
				this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, new Func<float>(this.GetPercentComplete));
			}
			this.progressBar.SetVisibility(true);
			return;
		}
		if (this.progressBar != null)
		{
			this.progressBar.gameObject.DeleteObject();
			this.progressBar = null;
		}
	}

	// Token: 0x0600255E RID: 9566 RVA: 0x000D1028 File Offset: 0x000CF228
	protected override void OnCleanUp()
	{
		this.ShowProgressBar(false);
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		if (this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		base.OnCleanUp();
		this.OnWorkableEventCB = null;
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x000D1090 File Offset: 0x000CF290
	public virtual Vector3 GetTargetPoint()
	{
		Vector3 vector = base.transform.GetPosition();
		float y = vector.y + 0.65f;
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			vector = component.bounds.center;
		}
		vector.y = y;
		vector.z = 0f;
		return vector;
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x000D10EA File Offset: 0x000CF2EA
	public int GetNavigationCost(Navigator navigator, int cell)
	{
		return navigator.GetNavigationCost(cell, this.GetOffsets(cell));
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x000D10FA File Offset: 0x000CF2FA
	public int GetNavigationCost(Navigator navigator)
	{
		return this.GetNavigationCost(navigator, Grid.PosToCell(this));
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x000D1109 File Offset: 0x000CF309
	private void TransferDiseaseWithWorker(WorkerBase worker)
	{
		if (this == null || worker == null)
		{
			return;
		}
		Workable.TransferDiseaseWithWorker(base.gameObject, worker.gameObject);
	}

	// Token: 0x06002563 RID: 9571 RVA: 0x000D1130 File Offset: 0x000CF330
	public static void TransferDiseaseWithWorker(GameObject workable, GameObject worker)
	{
		if (workable == null || worker == null)
		{
			return;
		}
		PrimaryElement component = workable.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		PrimaryElement component2 = worker.GetComponent<PrimaryElement>();
		if (component2 == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component2.DiseaseIdx;
		invalid.count = (int)((float)component2.DiseaseCount * 0.33f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = component.DiseaseIdx;
		invalid2.count = (int)((float)component.DiseaseCount * 0.33f);
		component2.ModifyDiseaseCount(-invalid.count, "Workable.TransferDiseaseWithWorker");
		component.ModifyDiseaseCount(-invalid2.count, "Workable.TransferDiseaseWithWorker");
		if (invalid.count > 0)
		{
			component.AddDisease(invalid.idx, invalid.count, "Workable.TransferDiseaseWithWorker");
		}
		if (invalid2.count > 0)
		{
			component2.AddDisease(invalid2.idx, invalid2.count, "Workable.TransferDiseaseWithWorker");
		}
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x000D1228 File Offset: 0x000CF428
	public void SetShouldShowSkillPerkStatusItem(bool shouldItBeShown)
	{
		this.shouldShowSkillPerkStatusItem = shouldItBeShown;
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			this.skillsUpdateHandle = -1;
		}
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x000D129C File Offset: 0x000CF49C
	public virtual bool InstantlyFinish(WorkerBase worker)
	{
		float num = worker.GetWorkable().WorkTimeRemaining;
		if (!float.IsInfinity(num))
		{
			worker.Work(num);
			return true;
		}
		DebugUtil.DevAssert(false, this.ToString() + " was asked to instantly finish but it has infinite work time! Override InstantlyFinish in your workable!", null);
		return false;
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x000D12E0 File Offset: 0x000CF4E0
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.trackUses)
		{
			Descriptor item = new Descriptor(string.Format(BUILDING.DETAILS.USE_COUNT, this.numberOfUses), string.Format(BUILDING.DETAILS.USE_COUNT_TOOLTIP, this.numberOfUses), Descriptor.DescriptorType.Detail, false);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x000D1340 File Offset: 0x000CF540
	public virtual BuildingFacade GetBuildingFacade()
	{
		return base.GetComponent<BuildingFacade>();
	}

	// Token: 0x06002568 RID: 9576 RVA: 0x000D1348 File Offset: 0x000CF548
	public virtual KAnimControllerBase GetAnimController()
	{
		return base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x06002569 RID: 9577 RVA: 0x000D1350 File Offset: 0x000CF550
	[ContextMenu("Refresh Reachability")]
	public void RefreshReachability()
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.ForceRefresh();
		}
	}

	// Token: 0x0400150A RID: 5386
	public float workTime;

	// Token: 0x0400150B RID: 5387
	protected bool showProgressBar = true;

	// Token: 0x0400150C RID: 5388
	public bool alwaysShowProgressBar;

	// Token: 0x0400150D RID: 5389
	protected bool lightEfficiencyBonus = true;

	// Token: 0x0400150E RID: 5390
	protected Guid lightEfficiencyBonusStatusItemHandle;

	// Token: 0x0400150F RID: 5391
	public bool currentlyLit;

	// Token: 0x04001510 RID: 5392
	public Tag laboratoryEfficiencyBonusTagRequired = RoomConstraints.ConstraintTags.ScienceBuilding;

	// Token: 0x04001511 RID: 5393
	private bool useLaboratoryEfficiencyBonus;

	// Token: 0x04001512 RID: 5394
	protected Guid laboratoryEfficiencyBonusStatusItemHandle;

	// Token: 0x04001513 RID: 5395
	private bool currentlyInLaboratory;

	// Token: 0x04001514 RID: 5396
	protected StatusItem workerStatusItem;

	// Token: 0x04001515 RID: 5397
	protected StatusItem workingStatusItem;

	// Token: 0x04001516 RID: 5398
	protected Guid workStatusItemHandle;

	// Token: 0x04001517 RID: 5399
	protected OffsetTracker offsetTracker;

	// Token: 0x04001518 RID: 5400
	[SerializeField]
	protected string attributeConverterId;

	// Token: 0x04001519 RID: 5401
	protected AttributeConverter attributeConverter;

	// Token: 0x0400151A RID: 5402
	protected float minimumAttributeMultiplier = 0.5f;

	// Token: 0x0400151B RID: 5403
	public bool resetProgressOnStop;

	// Token: 0x0400151C RID: 5404
	protected bool shouldTransferDiseaseWithWorker = true;

	// Token: 0x0400151D RID: 5405
	[SerializeField]
	protected float attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;

	// Token: 0x0400151E RID: 5406
	[SerializeField]
	protected string skillExperienceSkillGroup;

	// Token: 0x0400151F RID: 5407
	[SerializeField]
	protected float skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;

	// Token: 0x04001520 RID: 5408
	public bool triggerWorkReactions = true;

	// Token: 0x04001521 RID: 5409
	public ReportManager.ReportType reportType = ReportManager.ReportType.WorkTime;

	// Token: 0x04001522 RID: 5410
	[SerializeField]
	[Tooltip("What layer does the dupe switch to when interacting with the building")]
	public Grid.SceneLayer workLayer = Grid.SceneLayer.Move;

	// Token: 0x04001523 RID: 5411
	[SerializeField]
	[Serialize]
	protected float workTimeRemaining = float.PositiveInfinity;

	// Token: 0x04001524 RID: 5412
	[SerializeField]
	public KAnimFile[] overrideAnims;

	// Token: 0x04001525 RID: 5413
	[SerializeField]
	protected HashedString multitoolContext;

	// Token: 0x04001526 RID: 5414
	[SerializeField]
	protected Tag multitoolHitEffectTag;

	// Token: 0x04001527 RID: 5415
	[SerializeField]
	[Tooltip("Whether to user the KAnimSynchronizer or not")]
	public bool synchronizeAnims = true;

	// Token: 0x04001528 RID: 5416
	[SerializeField]
	[Tooltip("Whether to display number of uses in the details panel")]
	public bool trackUses;

	// Token: 0x04001529 RID: 5417
	[Serialize]
	protected int numberOfUses;

	// Token: 0x0400152A RID: 5418
	public Action<Workable, Workable.WorkableEvent> OnWorkableEventCB;

	// Token: 0x0400152B RID: 5419
	protected int skillsUpdateHandle = -1;

	// Token: 0x0400152C RID: 5420
	private int minionUpdateHandle = -1;

	// Token: 0x0400152D RID: 5421
	public string requiredSkillPerk;

	// Token: 0x0400152E RID: 5422
	[SerializeField]
	protected bool shouldShowSkillPerkStatusItem = true;

	// Token: 0x0400152F RID: 5423
	[SerializeField]
	public bool requireMinionToWork;

	// Token: 0x04001530 RID: 5424
	protected StatusItem readyForSkillWorkStatusItem;

	// Token: 0x04001531 RID: 5425
	public HashedString[] workAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04001532 RID: 5426
	public HashedString[] workingPstComplete = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04001533 RID: 5427
	public HashedString[] workingPstFailed = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04001534 RID: 5428
	public KAnim.PlayMode workAnimPlayMode;

	// Token: 0x04001535 RID: 5429
	public bool faceTargetWhenWorking;

	// Token: 0x04001536 RID: 5430
	private static readonly EventSystem.IntraObjectHandler<Workable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Workable>(delegate(Workable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x04001537 RID: 5431
	protected ProgressBar progressBar;

	// Token: 0x020013E6 RID: 5094
	public enum WorkableEvent
	{
		// Token: 0x04006858 RID: 26712
		WorkStarted,
		// Token: 0x04006859 RID: 26713
		WorkCompleted,
		// Token: 0x0400685A RID: 26714
		WorkStopped
	}

	// Token: 0x020013E7 RID: 5095
	public struct AnimInfo
	{
		// Token: 0x0400685B RID: 26715
		public KAnimFile[] overrideAnims;

		// Token: 0x0400685C RID: 26716
		public StateMachine.Instance smi;
	}
}

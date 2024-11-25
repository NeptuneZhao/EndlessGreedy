using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A69 RID: 2665
[AddComponentMenu("KMonoBehaviour/Workable/RoleStation")]
public class RoleStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06004D53 RID: 19795 RVA: 0x001BB96C File Offset: 0x001B9B6C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = true;
		this.UpdateStatusItemDelegate = new Action<object>(this.UpdateSkillPointAvailableStatusItem);
	}

	// Token: 0x06004D54 RID: 19796 RVA: 0x001BB990 File Offset: 0x001B9B90
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RoleStations.Add(this);
		this.smi = new RoleStation.RoleStationSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(7.53f);
		this.resetProgressOnStop = true;
		this.subscriptions.Add(Game.Instance.Subscribe(-1523247426, this.UpdateStatusItemDelegate));
		this.subscriptions.Add(Game.Instance.Subscribe(1505456302, this.UpdateStatusItemDelegate));
		this.UpdateSkillPointAvailableStatusItem(null);
	}

	// Token: 0x06004D55 RID: 19797 RVA: 0x001BBA20 File Offset: 0x001B9C20
	protected override void OnStopWork(WorkerBase worker)
	{
		Telepad.StatesInstance statesInstance = this.GetSMI<Telepad.StatesInstance>();
		statesInstance.sm.idlePortal.Trigger(statesInstance);
	}

	// Token: 0x06004D56 RID: 19798 RVA: 0x001BBA48 File Offset: 0x001B9C48
	private void UpdateSkillPointAvailableStatusItem(object data = null)
	{
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				if (this.skillPointAvailableStatusItem == Guid.Empty)
				{
					this.skillPointAvailableStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, null);
				}
				return;
			}
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, false);
		this.skillPointAvailableStatusItem = Guid.Empty;
	}

	// Token: 0x06004D57 RID: 19799 RVA: 0x001BBB14 File Offset: 0x001B9D14
	private Chore CreateWorkChore()
	{
		return new WorkChore<RoleStation>(Db.Get().ChoreTypes.LearnSkill, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
	}

	// Token: 0x06004D58 RID: 19800 RVA: 0x001BBB55 File Offset: 0x001B9D55
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<MinionResume>().SkillLearned();
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x001BBB69 File Offset: 0x001B9D69
	private void OnSelectRolesClick()
	{
		DetailsScreen.Instance.Show(false);
		ManagementMenu.Instance.ToggleSkills();
	}

	// Token: 0x06004D5A RID: 19802 RVA: 0x001BBB80 File Offset: 0x001B9D80
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.subscriptions)
		{
			Game.Instance.Unsubscribe(id);
		}
		Components.RoleStations.Remove(this);
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x001BBBE8 File Offset: 0x001B9DE8
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		return base.GetDescriptors(go);
	}

	// Token: 0x04003362 RID: 13154
	private Chore chore;

	// Token: 0x04003363 RID: 13155
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04003364 RID: 13156
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x04003365 RID: 13157
	private RoleStation.RoleStationSM.Instance smi;

	// Token: 0x04003366 RID: 13158
	private Guid skillPointAvailableStatusItem;

	// Token: 0x04003367 RID: 13159
	private Action<object> UpdateStatusItemDelegate;

	// Token: 0x04003368 RID: 13160
	private List<int> subscriptions = new List<int>();

	// Token: 0x02001A7F RID: 6783
	public class RoleStationSM : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation>
	{
		// Token: 0x0600A020 RID: 40992 RVA: 0x0037E958 File Offset: 0x0037CB58
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RoleStation.RoleStationSM.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.operational.ToggleChore((RoleStation.RoleStationSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x04007CA4 RID: 31908
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State unoperational;

		// Token: 0x04007CA5 RID: 31909
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State operational;

		// Token: 0x020025FA RID: 9722
		public new class Instance : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.GameInstance
		{
			// Token: 0x0600C0E3 RID: 49379 RVA: 0x003DD2B3 File Offset: 0x003DB4B3
			public Instance(RoleStation master) : base(master)
			{
			}
		}
	}
}

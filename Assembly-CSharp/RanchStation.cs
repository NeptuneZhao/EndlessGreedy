using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class RanchStation : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>
{
	// Token: 0x0600322B RID: 12843 RVA: 0x00113C34 File Offset: 0x00111E34
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Operational;
		this.Unoperational.TagTransition(GameTags.Operational, this.Operational, false);
		this.Operational.TagTransition(GameTags.Operational, this.Unoperational, true).ToggleChore((RanchStation.Instance smi) => smi.CreateChore(), this.Unoperational, this.Unoperational).Update("FindRanachable", delegate(RanchStation.Instance smi, float dt)
		{
			smi.FindRanchable(null);
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x04001DB0 RID: 7600
	public StateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.BoolParameter RancherIsReady;

	// Token: 0x04001DB1 RID: 7601
	public GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State Unoperational;

	// Token: 0x04001DB2 RID: 7602
	public RanchStation.OperationalState Operational;

	// Token: 0x020015CE RID: 5582
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006DCF RID: 28111
		public Func<GameObject, RanchStation.Instance, bool> IsCritterEligibleToBeRanchedCb;

		// Token: 0x04006DD0 RID: 28112
		public Action<GameObject> OnRanchCompleteCb;

		// Token: 0x04006DD1 RID: 28113
		public Action<GameObject, float, Workable> OnRanchWorkTick;

		// Token: 0x04006DD2 RID: 28114
		public HashedString RanchedPreAnim = "idle_loop";

		// Token: 0x04006DD3 RID: 28115
		public HashedString RanchedLoopAnim = "idle_loop";

		// Token: 0x04006DD4 RID: 28116
		public HashedString RanchedPstAnim = "idle_loop";

		// Token: 0x04006DD5 RID: 28117
		public HashedString RanchedAbortAnim = "idle_loop";

		// Token: 0x04006DD6 RID: 28118
		public HashedString RancherInteractAnim = "anim_interacts_rancherstation_kanim";

		// Token: 0x04006DD7 RID: 28119
		public StatusItem RanchingStatusItem = Db.Get().DuplicantStatusItems.Ranching;

		// Token: 0x04006DD8 RID: 28120
		public StatusItem CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingRanched;

		// Token: 0x04006DD9 RID: 28121
		public float WorkTime = 12f;

		// Token: 0x04006DDA RID: 28122
		public Func<RanchStation.Instance, int> GetTargetRanchCell = (RanchStation.Instance smi) => Grid.PosToCell(smi);
	}

	// Token: 0x020015CF RID: 5583
	public class OperationalState : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State
	{
	}

	// Token: 0x020015D0 RID: 5584
	public new class Instance : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.GameInstance
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06008FDF RID: 36831 RVA: 0x00349629 File Offset: 0x00347829
		public RanchedStates.Instance ActiveRanchable
		{
			get
			{
				return this.activeRanchable;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06008FE0 RID: 36832 RVA: 0x00349631 File Offset: 0x00347831
		private bool isCritterAvailableForRanching
		{
			get
			{
				return this.targetRanchables.Count > 0;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06008FE1 RID: 36833 RVA: 0x00349641 File Offset: 0x00347841
		public bool IsCritterAvailableForRanching
		{
			get
			{
				this.ValidateTargetRanchables();
				return this.isCritterAvailableForRanching;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06008FE2 RID: 36834 RVA: 0x0034964F File Offset: 0x0034784F
		public bool HasRancher
		{
			get
			{
				return this.rancher != null;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06008FE3 RID: 36835 RVA: 0x0034965D File Offset: 0x0034785D
		public bool IsRancherReady
		{
			get
			{
				return base.sm.RancherIsReady.Get(this);
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06008FE4 RID: 36836 RVA: 0x00349670 File Offset: 0x00347870
		public Extents StationExtents
		{
			get
			{
				return this.station.GetExtents();
			}
		}

		// Token: 0x06008FE5 RID: 36837 RVA: 0x0034967D File Offset: 0x0034787D
		public int GetRanchNavTarget()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06008FE6 RID: 36838 RVA: 0x00349690 File Offset: 0x00347890
		public Instance(IStateMachineTarget master, RanchStation.Def def) : base(master, def)
		{
			base.gameObject.AddOrGet<RancherChore.RancherWorkable>();
			this.station = base.GetComponent<BuildingComplete>();
		}

		// Token: 0x06008FE7 RID: 36839 RVA: 0x003496C0 File Offset: 0x003478C0
		public Chore CreateChore()
		{
			RancherChore rancherChore = new RancherChore(base.GetComponent<KPrefabID>());
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter targetParameter = rancherChore.smi.sm.rancher;
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Parameter<GameObject>.Context context = targetParameter.GetContext(rancherChore.smi);
			context.onDirty = (Action<RancherChore.RancherChoreStates.Instance>)Delegate.Combine(context.onDirty, new Action<RancherChore.RancherChoreStates.Instance>(this.OnRancherChanged));
			this.rancher = targetParameter.Get<WorkerBase>(rancherChore.smi);
			return rancherChore;
		}

		// Token: 0x06008FE8 RID: 36840 RVA: 0x0034972A File Offset: 0x0034792A
		public int GetTargetRanchCell()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06008FE9 RID: 36841 RVA: 0x00349740 File Offset: 0x00347940
		public override void StartSM()
		{
			base.StartSM();
			base.Subscribe(144050788, new Action<object>(this.OnRoomUpdated));
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.GetTargetRanchCell());
			if (cavityForCell != null && cavityForCell.room != null)
			{
				this.OnRoomUpdated(cavityForCell.room);
			}
		}

		// Token: 0x06008FEA RID: 36842 RVA: 0x00349797 File Offset: 0x00347997
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			base.Unsubscribe(144050788, new Action<object>(this.OnRoomUpdated));
		}

		// Token: 0x06008FEB RID: 36843 RVA: 0x003497B7 File Offset: 0x003479B7
		private void OnRoomUpdated(object data)
		{
			if (data == null)
			{
				return;
			}
			this.ranch = (data as Room);
			if (this.ranch.roomType != Db.Get().RoomTypes.CreaturePen)
			{
				this.TriggerRanchStationNoLongerAvailable();
				this.ranch = null;
			}
		}

		// Token: 0x06008FEC RID: 36844 RVA: 0x003497F2 File Offset: 0x003479F2
		private void OnRancherChanged(RancherChore.RancherChoreStates.Instance choreInstance)
		{
			this.rancher = choreInstance.sm.rancher.Get<WorkerBase>(choreInstance);
			this.TriggerRanchStationNoLongerAvailable();
		}

		// Token: 0x06008FED RID: 36845 RVA: 0x00349811 File Offset: 0x00347A11
		public bool TryGetRanched(RanchedStates.Instance ranchable)
		{
			return this.activeRanchable == null || this.activeRanchable == ranchable;
		}

		// Token: 0x06008FEE RID: 36846 RVA: 0x00349826 File Offset: 0x00347A26
		public void MessageCreatureArrived(RanchedStates.Instance critter)
		{
			this.activeRanchable = critter;
			base.sm.RancherIsReady.Set(false, this, false);
			base.Trigger(-1357116271, null);
		}

		// Token: 0x06008FEF RID: 36847 RVA: 0x0034984F File Offset: 0x00347A4F
		public void MessageRancherReady()
		{
			base.sm.RancherIsReady.Set(true, base.smi, false);
			this.MessageRanchables(GameHashes.RancherReadyAtRanchStation);
		}

		// Token: 0x06008FF0 RID: 36848 RVA: 0x00349878 File Offset: 0x00347A78
		private bool CanRanchableBeRanchedAtRanchStation(RanchableMonitor.Instance ranchable)
		{
			bool flag = !ranchable.IsNullOrStopped();
			if (flag && ranchable.TargetRanchStation != null && ranchable.TargetRanchStation != this)
			{
				flag = (!ranchable.TargetRanchStation.IsRunning() || !ranchable.TargetRanchStation.HasRancher);
			}
			flag = (flag && base.def.IsCritterEligibleToBeRanchedCb(ranchable.gameObject, this));
			flag = (flag && ranchable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>());
			if (flag)
			{
				int cell = Grid.PosToCell(ranchable.transform.GetPosition());
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
				if (cavityForCell == null || this.ranch == null || cavityForCell != this.ranch.cavity)
				{
					flag = false;
				}
				else
				{
					int cell2 = this.GetRanchNavTarget();
					if (ranchable.HasTag(GameTags.Creatures.Flyer))
					{
						cell2 = Grid.CellAbove(cell2);
					}
					flag = (ranchable.NavComponent.GetNavigationCost(cell2) != -1);
				}
			}
			return flag;
		}

		// Token: 0x06008FF1 RID: 36849 RVA: 0x00349964 File Offset: 0x00347B64
		public void ValidateTargetRanchables()
		{
			if (!this.HasRancher)
			{
				return;
			}
			foreach (RanchableMonitor.Instance instance in this.targetRanchables.ToArray())
			{
				if (instance.States == null || !this.CanRanchableBeRanchedAtRanchStation(instance))
				{
					this.Abandon(instance);
				}
			}
		}

		// Token: 0x06008FF2 RID: 36850 RVA: 0x003499B0 File Offset: 0x00347BB0
		public void FindRanchable(object _ = null)
		{
			if (this.ranch == null)
			{
				return;
			}
			this.ValidateTargetRanchables();
			if (this.targetRanchables.Count == 2)
			{
				return;
			}
			List<KPrefabID> creatures = this.ranch.cavity.creatures;
			if (this.HasRancher && !this.isCritterAvailableForRanching && creatures.Count == 0)
			{
				this.TryNotifyEmptyRanch();
			}
			for (int i = 0; i < creatures.Count; i++)
			{
				KPrefabID kprefabID = creatures[i];
				if (!(kprefabID == null))
				{
					RanchableMonitor.Instance smi = kprefabID.GetSMI<RanchableMonitor.Instance>();
					if (!this.targetRanchables.Contains(smi) && this.CanRanchableBeRanchedAtRanchStation(smi) && smi != null)
					{
						smi.States.SetRanchStation(this);
						this.targetRanchables.Add(smi);
						return;
					}
				}
			}
		}

		// Token: 0x06008FF3 RID: 36851 RVA: 0x00349A66 File Offset: 0x00347C66
		public Option<CavityInfo> GetCavityInfo()
		{
			if (this.ranch.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return this.ranch.cavity;
		}

		// Token: 0x06008FF4 RID: 36852 RVA: 0x00349A90 File Offset: 0x00347C90
		public void RanchCreature()
		{
			if (this.activeRanchable.IsNullOrStopped())
			{
				return;
			}
			global::Debug.Assert(this.activeRanchable != null, "targetRanchable was null");
			global::Debug.Assert(this.activeRanchable.GetMaster() != null, "GetMaster was null");
			global::Debug.Assert(base.def != null, "def was null");
			global::Debug.Assert(base.def.OnRanchCompleteCb != null, "onRanchCompleteCb cb was null");
			base.def.OnRanchCompleteCb(this.activeRanchable.gameObject);
			this.targetRanchables.Remove(this.activeRanchable.Monitor);
			this.activeRanchable.Trigger(1827504087, null);
			this.activeRanchable = null;
			this.FindRanchable(null);
		}

		// Token: 0x06008FF5 RID: 36853 RVA: 0x00349B54 File Offset: 0x00347D54
		public void TriggerRanchStationNoLongerAvailable()
		{
			for (int i = this.targetRanchables.Count - 1; i >= 0; i--)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (instance.IsNullOrStopped() || instance.States.IsNullOrStopped())
				{
					this.targetRanchables.RemoveAt(i);
				}
				else
				{
					this.targetRanchables.Remove(instance);
					instance.Trigger(1689625967, null);
				}
			}
			global::Debug.Assert(this.targetRanchables.Count == 0, "targetRanchables is not empty");
			this.activeRanchable = null;
			base.sm.RancherIsReady.Set(false, this, false);
		}

		// Token: 0x06008FF6 RID: 36854 RVA: 0x00349BF8 File Offset: 0x00347DF8
		public void MessageRanchables(GameHashes hash)
		{
			for (int i = 0; i < this.targetRanchables.Count; i++)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (!instance.IsNullOrStopped())
				{
					Game.BrainScheduler.PrioritizeBrain(instance.GetComponent<CreatureBrain>());
					if (!instance.States.IsNullOrStopped())
					{
						instance.Trigger((int)hash, null);
					}
				}
			}
		}

		// Token: 0x06008FF7 RID: 36855 RVA: 0x00349C58 File Offset: 0x00347E58
		public void Abandon(RanchableMonitor.Instance critter)
		{
			if (critter == null)
			{
				global::Debug.LogWarning("Null critter trying to abandon ranch station");
				this.targetRanchables.Remove(critter);
				return;
			}
			critter.TargetRanchStation = null;
			if (this.targetRanchables.Remove(critter))
			{
				if (critter.States == null)
				{
					return;
				}
				bool flag = !this.isCritterAvailableForRanching;
				if (critter.States == this.activeRanchable)
				{
					flag = true;
					this.activeRanchable = null;
				}
				if (flag)
				{
					this.TryNotifyEmptyRanch();
				}
			}
		}

		// Token: 0x06008FF8 RID: 36856 RVA: 0x00349CC8 File Offset: 0x00347EC8
		private void TryNotifyEmptyRanch()
		{
			if (!this.HasRancher)
			{
				return;
			}
			this.rancher.Trigger(-364750427, null);
		}

		// Token: 0x06008FF9 RID: 36857 RVA: 0x00349CE4 File Offset: 0x00347EE4
		public bool IsCritterInQueue(RanchableMonitor.Instance critter)
		{
			return this.targetRanchables.Contains(critter);
		}

		// Token: 0x06008FFA RID: 36858 RVA: 0x00349CF2 File Offset: 0x00347EF2
		public List<RanchableMonitor.Instance> DEBUG_GetTargetRanchables()
		{
			return this.targetRanchables;
		}

		// Token: 0x04006DDB RID: 28123
		private const int QUEUE_SIZE = 2;

		// Token: 0x04006DDC RID: 28124
		private List<RanchableMonitor.Instance> targetRanchables = new List<RanchableMonitor.Instance>();

		// Token: 0x04006DDD RID: 28125
		private RanchedStates.Instance activeRanchable;

		// Token: 0x04006DDE RID: 28126
		private Room ranch;

		// Token: 0x04006DDF RID: 28127
		private WorkerBase rancher;

		// Token: 0x04006DE0 RID: 28128
		private BuildingComplete station;
	}
}

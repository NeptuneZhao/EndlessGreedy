using System;
using System.Collections.Generic;

// Token: 0x02000733 RID: 1843
public class MissionControlCluster : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>
{
	// Token: 0x060030EB RID: 12523 RVA: 0x0010E23C File Offset: 0x0010C43C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inoperational;
		this.Inoperational.EventTransition(GameHashes.OperationalChanged, this.Operational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Operational.EventTransition(GameHashes.OperationalChanged, this.Inoperational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational.WrongRoom, GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Not(new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.IsInLabRoom))).Enter(new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State.Callback(this.OnEnterOperational)).DefaultState(this.Operational.NoRockets).Update(delegate(MissionControlCluster.Instance smi, float dt)
		{
			smi.UpdateWorkableRocketsInRange(null);
		}, UpdateRate.SIM_1000ms, false);
		this.Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, this.Operational.NoRockets, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.IsInLabRoom));
		this.Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlClusterBoost, null).ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.HasRockets, (MissionControlCluster.Instance smi, bool inRange) => this.WorkableRocketsAreInRange.Get(smi));
		this.Operational.HasRockets.ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.NoRockets, (MissionControlCluster.Instance smi, bool inRange) => !this.WorkableRocketsAreInRange.Get(smi)).ToggleChore(new Func<MissionControlCluster.Instance, Chore>(this.CreateChore), this.Operational);
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x0010E3D8 File Offset: 0x0010C5D8
	private Chore CreateChore(MissionControlCluster.Instance smi)
	{
		MissionControlClusterWorkable component = smi.master.gameObject.GetComponent<MissionControlClusterWorkable>();
		Chore result = new WorkChore<MissionControlClusterWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		Clustercraft randomBoostableClustercraft = smi.GetRandomBoostableClustercraft();
		component.TargetClustercraft = randomBoostableClustercraft;
		return result;
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x0010E42A File Offset: 0x0010C62A
	private void OnEnterOperational(MissionControlCluster.Instance smi)
	{
		smi.UpdateWorkableRocketsInRange(null);
		if (this.WorkableRocketsAreInRange.Get(smi))
		{
			smi.GoTo(this.Operational.HasRockets);
			return;
		}
		smi.GoTo(this.Operational.NoRockets);
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x0010E464 File Offset: 0x0010C664
	private bool ValidateOperationalTransition(MissionControlCluster.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Operational);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x0010E4A1 File Offset: 0x0010C6A1
	private bool IsInLabRoom(MissionControlCluster.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x04001CBA RID: 7354
	public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State Inoperational;

	// Token: 0x04001CBB RID: 7355
	public MissionControlCluster.OperationalState Operational;

	// Token: 0x04001CBC RID: 7356
	public StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.BoolParameter WorkableRocketsAreInRange;

	// Token: 0x02001591 RID: 5521
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001592 RID: 5522
	public new class Instance : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.GameInstance
	{
		// Token: 0x06008F1F RID: 36639 RVA: 0x00346A8C File Offset: 0x00344C8C
		public Instance(IStateMachineTarget master, MissionControlCluster.Def def) : base(master, def)
		{
		}

		// Token: 0x06008F20 RID: 36640 RVA: 0x00346AA8 File Offset: 0x00344CA8
		public override void StartSM()
		{
			base.StartSM();
			this.clusterUpdatedHandle = Game.Instance.Subscribe(-1298331547, new Action<object>(this.UpdateWorkableRocketsInRange));
		}

		// Token: 0x06008F21 RID: 36641 RVA: 0x00346AD1 File Offset: 0x00344CD1
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Game.Instance.Unsubscribe(this.clusterUpdatedHandle);
		}

		// Token: 0x06008F22 RID: 36642 RVA: 0x00346AEC File Offset: 0x00344CEC
		public void UpdateWorkableRocketsInRange(object data)
		{
			this.boostableClustercraft.Clear();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				if (ClusterGrid.Instance.IsInRange(Components.Clustercrafts[i].Location, myWorldLocation, 2) && !this.IsOwnWorld(Components.Clustercrafts[i]) && this.CanBeBoosted(Components.Clustercrafts[i]))
				{
					bool flag = false;
					foreach (object obj in Components.MissionControlClusterWorkables)
					{
						MissionControlClusterWorkable missionControlClusterWorkable = (MissionControlClusterWorkable)obj;
						if (!(missionControlClusterWorkable.gameObject == base.gameObject) && missionControlClusterWorkable.TargetClustercraft == Components.Clustercrafts[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.boostableClustercraft.Add(Components.Clustercrafts[i]);
					}
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(this.boostableClustercraft.Count > 0, base.smi, false);
		}

		// Token: 0x06008F23 RID: 36643 RVA: 0x00346C34 File Offset: 0x00344E34
		public Clustercraft GetRandomBoostableClustercraft()
		{
			return this.boostableClustercraft.GetRandom<Clustercraft>();
		}

		// Token: 0x06008F24 RID: 36644 RVA: 0x00346C41 File Offset: 0x00344E41
		private bool CanBeBoosted(Clustercraft clustercraft)
		{
			return clustercraft.controlStationBuffTimeRemaining == 0f && clustercraft.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && clustercraft.IsFlightInProgress();
		}

		// Token: 0x06008F25 RID: 36645 RVA: 0x00346C68 File Offset: 0x00344E68
		private bool IsOwnWorld(Clustercraft candidateClustercraft)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			WorldContainer interiorWorld = candidateClustercraft.ModuleInterface.GetInteriorWorld();
			return !(interiorWorld == null) && myWorldId == interiorWorld.id;
		}

		// Token: 0x06008F26 RID: 36646 RVA: 0x00346CA1 File Offset: 0x00344EA1
		public void ApplyEffect(Clustercraft clustercraft)
		{
			clustercraft.controlStationBuffTimeRemaining = 600f;
		}

		// Token: 0x04006D4B RID: 27979
		private int clusterUpdatedHandle = -1;

		// Token: 0x04006D4C RID: 27980
		private List<Clustercraft> boostableClustercraft = new List<Clustercraft>();

		// Token: 0x04006D4D RID: 27981
		[MyCmpReq]
		public RoomTracker roomTracker;
	}

	// Token: 0x02001593 RID: 5523
	public class OperationalState : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State
	{
		// Token: 0x04006D4E RID: 27982
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State WrongRoom;

		// Token: 0x04006D4F RID: 27983
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State NoRockets;

		// Token: 0x04006D50 RID: 27984
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State HasRockets;
	}
}

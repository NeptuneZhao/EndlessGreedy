using System;
using System.Collections.Generic;

// Token: 0x02000732 RID: 1842
public class MissionControl : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>
{
	// Token: 0x060030E3 RID: 12515 RVA: 0x0010DFA0 File Offset: 0x0010C1A0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inoperational;
		this.Inoperational.EventTransition(GameHashes.OperationalChanged, this.Operational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Operational.EventTransition(GameHashes.OperationalChanged, this.Inoperational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational.WrongRoom, GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Not(new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.IsInLabRoom))).Enter(new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State.Callback(this.OnEnterOperational)).DefaultState(this.Operational.NoRockets).Update(delegate(MissionControl.Instance smi, float dt)
		{
			smi.UpdateWorkableRockets(null);
		}, UpdateRate.SIM_1000ms, false);
		this.Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, this.Operational.NoRockets, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.IsInLabRoom));
		this.Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlBoost, null).ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.HasRockets, (MissionControl.Instance smi, bool inRange) => this.WorkableRocketsAreInRange.Get(smi));
		this.Operational.HasRockets.ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.NoRockets, (MissionControl.Instance smi, bool inRange) => !this.WorkableRocketsAreInRange.Get(smi)).ToggleChore(new Func<MissionControl.Instance, Chore>(this.CreateChore), this.Operational);
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x0010E13C File Offset: 0x0010C33C
	private Chore CreateChore(MissionControl.Instance smi)
	{
		MissionControlWorkable component = smi.master.gameObject.GetComponent<MissionControlWorkable>();
		Chore result = new WorkChore<MissionControlWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		Spacecraft randomBoostableSpacecraft = smi.GetRandomBoostableSpacecraft();
		component.TargetSpacecraft = randomBoostableSpacecraft;
		return result;
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x0010E18E File Offset: 0x0010C38E
	private void OnEnterOperational(MissionControl.Instance smi)
	{
		smi.UpdateWorkableRockets(null);
		if (this.WorkableRocketsAreInRange.Get(smi))
		{
			smi.GoTo(this.Operational.HasRockets);
			return;
		}
		smi.GoTo(this.Operational.NoRockets);
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x0010E1C8 File Offset: 0x0010C3C8
	private bool ValidateOperationalTransition(MissionControl.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Operational);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x0010E205 File Offset: 0x0010C405
	private bool IsInLabRoom(MissionControl.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x04001CB7 RID: 7351
	public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State Inoperational;

	// Token: 0x04001CB8 RID: 7352
	public MissionControl.OperationalState Operational;

	// Token: 0x04001CB9 RID: 7353
	public StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.BoolParameter WorkableRocketsAreInRange;

	// Token: 0x0200158D RID: 5517
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200158E RID: 5518
	public new class Instance : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.GameInstance
	{
		// Token: 0x06008F15 RID: 36629 RVA: 0x00346902 File Offset: 0x00344B02
		public Instance(IStateMachineTarget master, MissionControl.Def def) : base(master, def)
		{
		}

		// Token: 0x06008F16 RID: 36630 RVA: 0x00346918 File Offset: 0x00344B18
		public void UpdateWorkableRockets(object data)
		{
			this.boostableSpacecraft.Clear();
			for (int i = 0; i < SpacecraftManager.instance.GetSpacecraft().Count; i++)
			{
				if (this.CanBeBoosted(SpacecraftManager.instance.GetSpacecraft()[i]))
				{
					bool flag = false;
					foreach (object obj in Components.MissionControlWorkables)
					{
						MissionControlWorkable missionControlWorkable = (MissionControlWorkable)obj;
						if (!(missionControlWorkable.gameObject == base.gameObject) && missionControlWorkable.TargetSpacecraft == SpacecraftManager.instance.GetSpacecraft()[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.boostableSpacecraft.Add(SpacecraftManager.instance.GetSpacecraft()[i]);
					}
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(this.boostableSpacecraft.Count > 0, base.smi, false);
		}

		// Token: 0x06008F17 RID: 36631 RVA: 0x00346A28 File Offset: 0x00344C28
		public Spacecraft GetRandomBoostableSpacecraft()
		{
			return this.boostableSpacecraft.GetRandom<Spacecraft>();
		}

		// Token: 0x06008F18 RID: 36632 RVA: 0x00346A35 File Offset: 0x00344C35
		private bool CanBeBoosted(Spacecraft spacecraft)
		{
			return spacecraft.controlStationBuffTimeRemaining == 0f && spacecraft.state == Spacecraft.MissionState.Underway;
		}

		// Token: 0x06008F19 RID: 36633 RVA: 0x00346A52 File Offset: 0x00344C52
		public void ApplyEffect(Spacecraft spacecraft)
		{
			spacecraft.controlStationBuffTimeRemaining = 600f;
		}

		// Token: 0x04006D44 RID: 27972
		private List<Spacecraft> boostableSpacecraft = new List<Spacecraft>();

		// Token: 0x04006D45 RID: 27973
		[MyCmpReq]
		public RoomTracker roomTracker;
	}

	// Token: 0x0200158F RID: 5519
	public class OperationalState : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State
	{
		// Token: 0x04006D46 RID: 27974
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State WrongRoom;

		// Token: 0x04006D47 RID: 27975
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State NoRockets;

		// Token: 0x04006D48 RID: 27976
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State HasRockets;
	}
}

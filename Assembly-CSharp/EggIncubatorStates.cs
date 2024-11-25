using System;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
public class EggIncubatorStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance>
{
	// Token: 0x06002BCC RID: 11212 RVA: 0x000F5FDC File Offset: 0x000F41DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.empty;
		this.empty.PlayAnim("off", KAnim.PlayMode.Loop).EventTransition(GameHashes.OccupantChanged, this.egg, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasEgg)).EventTransition(GameHashes.OccupantChanged, this.baby, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby));
		this.egg.DefaultState(this.egg.unpowered).EventTransition(GameHashes.OccupantChanged, this.empty, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasAny))).EventTransition(GameHashes.OccupantChanged, this.baby, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby)).ToggleStatusItem(Db.Get().BuildingStatusItems.IncubatorProgress, (EggIncubatorStates.Instance smi) => smi.master.GetComponent<EggIncubator>());
		this.egg.lose_power.PlayAnim("no_power_pre").EventTransition(GameHashes.OperationalChanged, this.egg.incubating, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational)).OnAnimQueueComplete(this.egg.unpowered);
		this.egg.unpowered.PlayAnim("no_power_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.egg.incubating, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational));
		this.egg.incubating.PlayAnim("no_power_pst").QueueAnim("working_loop", true, null).EventTransition(GameHashes.OperationalChanged, this.egg.lose_power, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational)));
		this.baby.DefaultState(this.baby.idle).EventTransition(GameHashes.OccupantChanged, this.empty, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby)));
		this.baby.idle.PlayAnim("no_power_pre").QueueAnim("no_power_loop", true, null);
	}

	// Token: 0x06002BCD RID: 11213 RVA: 0x000F61E3 File Offset: 0x000F43E3
	public static bool IsOperational(EggIncubatorStates.Instance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x06002BCE RID: 11214 RVA: 0x000F61F0 File Offset: 0x000F43F0
	public static bool HasEgg(EggIncubatorStates.Instance smi)
	{
		GameObject occupant = smi.GetComponent<EggIncubator>().Occupant;
		return occupant && occupant.HasTag(GameTags.Egg);
	}

	// Token: 0x06002BCF RID: 11215 RVA: 0x000F6220 File Offset: 0x000F4420
	public static bool HasBaby(EggIncubatorStates.Instance smi)
	{
		GameObject occupant = smi.GetComponent<EggIncubator>().Occupant;
		return occupant && occupant.HasTag(GameTags.Creature);
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x000F624E File Offset: 0x000F444E
	public static bool HasAny(EggIncubatorStates.Instance smi)
	{
		return smi.GetComponent<EggIncubator>().Occupant;
	}

	// Token: 0x0400192F RID: 6447
	public StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.BoolParameter readyToHatch;

	// Token: 0x04001930 RID: 6448
	public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State empty;

	// Token: 0x04001931 RID: 6449
	public EggIncubatorStates.EggStates egg;

	// Token: 0x04001932 RID: 6450
	public EggIncubatorStates.BabyStates baby;

	// Token: 0x020014C1 RID: 5313
	public class EggStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006ADF RID: 27359
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State incubating;

		// Token: 0x04006AE0 RID: 27360
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State lose_power;

		// Token: 0x04006AE1 RID: 27361
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State unpowered;
	}

	// Token: 0x020014C2 RID: 5314
	public class BabyStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006AE2 RID: 27362
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State idle;
	}

	// Token: 0x020014C3 RID: 5315
	public new class Instance : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C07 RID: 35847 RVA: 0x00338B30 File Offset: 0x00336D30
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}

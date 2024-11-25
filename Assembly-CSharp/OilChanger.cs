using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200073D RID: 1853
public class OilChanger : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>
{
	// Token: 0x0600314D RID: 12621 RVA: 0x0010FEC8 File Offset: 0x0010E0C8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.inoperational;
		this.inoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.operational.oilNeeded);
		this.operational.oilNeeded.ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, null).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.Transition.ConditionCallback(OilChanger.HasEnoughLubricant));
		this.operational.ready.ToggleChore(new Func<OilChanger.Instance, Chore>(OilChanger.CreateChore), this.operational.oilNeeded);
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x0010FF8E File Offset: 0x0010E18E
	public static bool HasEnoughLubricant(OilChanger.Instance smi)
	{
		return smi.OilAmount >= smi.def.MIN_LUBRICANT_MASS_TO_WORK;
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x0010FFA6 File Offset: 0x0010E1A6
	private static bool IsOperational(OilChanger.Instance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x0010FFB0 File Offset: 0x0010E1B0
	private static WorkChore<OilChangerWorkableUse> CreateChore(OilChanger.Instance smi)
	{
		return new WorkChore<OilChangerWorkableUse>(Db.Get().ChoreTypes.OilChange, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
	}

	// Token: 0x04001CFA RID: 7418
	public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State inoperational;

	// Token: 0x04001CFB RID: 7419
	public OilChanger.OperationalStates operational;

	// Token: 0x020015A1 RID: 5537
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006D77 RID: 28023
		public float MIN_LUBRICANT_MASS_TO_WORK = 200f;
	}

	// Token: 0x020015A2 RID: 5538
	public class OperationalStates : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State
	{
		// Token: 0x04006D78 RID: 28024
		public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State oilNeeded;

		// Token: 0x04006D79 RID: 28025
		public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State ready;
	}

	// Token: 0x020015A3 RID: 5539
	public new class Instance : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.GameInstance, IFetchList
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06008F52 RID: 36690 RVA: 0x0034716F File Offset: 0x0034536F
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06008F53 RID: 36691 RVA: 0x0034717C File Offset: 0x0034537C
		public float OilAmount
		{
			get
			{
				return this.storage.GetMassAvailable(GameTags.LubricatingOil);
			}
		}

		// Token: 0x06008F54 RID: 36692 RVA: 0x00347190 File Offset: 0x00345390
		public Instance(IStateMachineTarget master, OilChanger.Def def)
		{
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			Tag lubricatingOil = GameTags.LubricatingOil;
			dictionary[lubricatingOil] = 0f;
			this.remainingLubricationMass = dictionary;
			base..ctor(master, def);
			this.storage = base.GetComponent<Storage>();
			this.operational = base.GetComponent<Operational>();
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06008F55 RID: 36693 RVA: 0x003471DA File Offset: 0x003453DA
		public Storage Destination
		{
			get
			{
				return this.storage;
			}
		}

		// Token: 0x06008F56 RID: 36694 RVA: 0x003471E2 File Offset: 0x003453E2
		public float GetMinimumAmount(Tag tag)
		{
			return base.def.MIN_LUBRICANT_MASS_TO_WORK;
		}

		// Token: 0x06008F57 RID: 36695 RVA: 0x003471EF File Offset: 0x003453EF
		public Dictionary<Tag, float> GetRemaining()
		{
			this.remainingLubricationMass[GameTags.LubricatingOil] = Mathf.Clamp(base.def.MIN_LUBRICANT_MASS_TO_WORK - this.OilAmount, 0f, base.def.MIN_LUBRICANT_MASS_TO_WORK);
			return this.remainingLubricationMass;
		}

		// Token: 0x06008F58 RID: 36696 RVA: 0x0034722E File Offset: 0x0034542E
		public Dictionary<Tag, float> GetRemainingMinimum()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04006D7A RID: 28026
		private Storage storage;

		// Token: 0x04006D7B RID: 28027
		private Operational operational;

		// Token: 0x04006D7C RID: 28028
		private Dictionary<Tag, float> remainingLubricationMass;
	}
}

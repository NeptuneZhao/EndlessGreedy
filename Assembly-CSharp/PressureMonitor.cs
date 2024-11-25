using System;
using Klei.AI;

// Token: 0x02000993 RID: 2451
public class PressureMonitor : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>
{
	// Token: 0x06004783 RID: 18307 RVA: 0x00198FB4 File Offset: 0x001971B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.Transition(this.inPressure, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas), UpdateRate.SIM_200ms);
		this.inPressure.Transition(this.safe, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas)), UpdateRate.SIM_200ms).DefaultState(this.inPressure.idle);
		this.inPressure.idle.EventTransition(GameHashes.EffectImmunityAdded, this.inPressure.immune, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)).Update(new Action<PressureMonitor.Instance, float>(PressureMonitor.HighPressureUpdate), UpdateRate.SIM_200ms, false);
		this.inPressure.immune.EventTransition(GameHashes.EffectImmunityRemoved, this.inPressure.idle, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)));
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x00199095 File Offset: 0x00197295
	public static bool IsInPressureGas(PressureMonitor.Instance smi)
	{
		return smi.IsInHighPressure();
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x0019909D File Offset: 0x0019729D
	public static bool IsImmuneToPressure(PressureMonitor.Instance smi)
	{
		return smi.IsImmuneToHighPressure();
	}

	// Token: 0x06004786 RID: 18310 RVA: 0x001990A5 File Offset: 0x001972A5
	public static void RemoveOverpressureEffect(PressureMonitor.Instance smi)
	{
		smi.RemoveEffect();
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x001990AD File Offset: 0x001972AD
	public static void HighPressureUpdate(PressureMonitor.Instance smi, float dt)
	{
		if (smi.timeinstate > 3f)
		{
			smi.AddEffect();
		}
	}

	// Token: 0x04002EAA RID: 11946
	public const string OVER_PRESSURE_EFFECT_NAME = "PoppedEarDrums";

	// Token: 0x04002EAB RID: 11947
	public const float TIME_IN_PRESSURE_BEFORE_EAR_POPS = 3f;

	// Token: 0x04002EAC RID: 11948
	private static CellOffset[] PRESSURE_TEST_OFFSET = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1)
	};

	// Token: 0x04002EAD RID: 11949
	public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State safe;

	// Token: 0x04002EAE RID: 11950
	public PressureMonitor.PressureStates inPressure;

	// Token: 0x0200195F RID: 6495
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001960 RID: 6496
	public class PressureStates : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State
	{
		// Token: 0x0400793F RID: 31039
		public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State idle;

		// Token: 0x04007940 RID: 31040
		public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State immune;
	}

	// Token: 0x02001961 RID: 6497
	public new class Instance : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.GameInstance
	{
		// Token: 0x06009C50 RID: 40016 RVA: 0x0037206F File Offset: 0x0037026F
		public Instance(IStateMachineTarget master, PressureMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x06009C51 RID: 40017 RVA: 0x00372085 File Offset: 0x00370285
		public bool IsImmuneToHighPressure()
		{
			return this.effects.HasImmunityTo(Db.Get().effects.Get("PoppedEarDrums"));
		}

		// Token: 0x06009C52 RID: 40018 RVA: 0x003720A8 File Offset: 0x003702A8
		public bool IsInHighPressure()
		{
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < PressureMonitor.PRESSURE_TEST_OFFSET.Length; i++)
			{
				int num = Grid.OffsetCell(cell, PressureMonitor.PRESSURE_TEST_OFFSET[i]);
				if (Grid.IsValidCell(num) && Grid.Element[num].IsGas && Grid.Mass[num] > 4f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009C53 RID: 40019 RVA: 0x00372110 File Offset: 0x00370310
		public void RemoveEffect()
		{
			this.effects.Remove("PoppedEarDrums");
		}

		// Token: 0x06009C54 RID: 40020 RVA: 0x00372122 File Offset: 0x00370322
		public void AddEffect()
		{
			this.effects.Add("PoppedEarDrums", true);
		}

		// Token: 0x04007941 RID: 31041
		private Effects effects;
	}
}

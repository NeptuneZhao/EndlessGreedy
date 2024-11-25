using System;

// Token: 0x020009B9 RID: 2489
[SkipSaveFileSerialization]
public class Claustrophobic : StateMachineComponent<Claustrophobic.StatesInstance>
{
	// Token: 0x06004868 RID: 18536 RVA: 0x0019EE16 File Offset: 0x0019D016
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x0019EE24 File Offset: 0x0019D024
	protected bool IsUncomfortable()
	{
		int num = 4;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < num - 1; i++)
		{
			int num2 = Grid.OffsetCell(cell, 0, i);
			if (Grid.IsValidCell(num2) && Grid.Solid[num2])
			{
				return true;
			}
			if (Grid.IsValidCell(Grid.CellRight(cell)) && Grid.IsValidCell(Grid.CellLeft(cell)) && Grid.Solid[Grid.CellRight(cell)] && Grid.Solid[Grid.CellLeft(cell)])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x020019C1 RID: 6593
	public class StatesInstance : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.GameInstance
	{
		// Token: 0x06009DF9 RID: 40441 RVA: 0x0037683F File Offset: 0x00374A3F
		public StatesInstance(Claustrophobic master) : base(master)
		{
		}
	}

	// Token: 0x020019C2 RID: 6594
	public class States : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic>
	{
		// Token: 0x06009DFA RID: 40442 RVA: 0x00376848 File Offset: 0x00374A48
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("ClaustrophobicCheck", delegate(Claustrophobic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Claustrophobic").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04007A9F RID: 31391
		public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State satisfied;

		// Token: 0x04007AA0 RID: 31392
		public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State suffering;
	}
}

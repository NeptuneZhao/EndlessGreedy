using System;

// Token: 0x020009BE RID: 2494
[SkipSaveFileSerialization]
public class SensitiveFeet : StateMachineComponent<SensitiveFeet.StatesInstance>
{
	// Token: 0x06004876 RID: 18550 RVA: 0x0019F006 File Offset: 0x0019D206
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004877 RID: 18551 RVA: 0x0019F014 File Offset: 0x0019D214
	protected bool IsUncomfortable()
	{
		int num = Grid.CellBelow(Grid.PosToCell(base.gameObject));
		return Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Objects[num, 9] == null;
	}

	// Token: 0x020019CB RID: 6603
	public class StatesInstance : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.GameInstance
	{
		// Token: 0x06009E0E RID: 40462 RVA: 0x00376B48 File Offset: 0x00374D48
		public StatesInstance(SensitiveFeet master) : base(master)
		{
		}
	}

	// Token: 0x020019CC RID: 6604
	public class States : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet>
	{
		// Token: 0x06009E0F RID: 40463 RVA: 0x00376B54 File Offset: 0x00374D54
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("SensitiveFeetCheck", delegate(SensitiveFeet.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("UncomfortableFeet").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04007AA7 RID: 31399
		public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State satisfied;

		// Token: 0x04007AA8 RID: 31400
		public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State suffering;
	}
}

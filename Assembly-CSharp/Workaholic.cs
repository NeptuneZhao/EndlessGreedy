using System;

// Token: 0x020009C0 RID: 2496
[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance>
{
	// Token: 0x0600487C RID: 18556 RVA: 0x0019F178 File Offset: 0x0019D378
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600487D RID: 18557 RVA: 0x0019F185 File Offset: 0x0019D385
	protected bool IsUncomfortable()
	{
		return base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore;
	}

	// Token: 0x020019CF RID: 6607
	public class StatesInstance : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.GameInstance
	{
		// Token: 0x06009E17 RID: 40471 RVA: 0x00376CFF File Offset: 0x00374EFF
		public StatesInstance(Workaholic master) : base(master)
		{
		}
	}

	// Token: 0x020019D0 RID: 6608
	public class States : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic>
	{
		// Token: 0x06009E18 RID: 40472 RVA: 0x00376D08 File Offset: 0x00374F08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("WorkaholicCheck", delegate(Workaholic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04007AAB RID: 31403
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State satisfied;

		// Token: 0x04007AAC RID: 31404
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State suffering;
	}
}

using System;
using UnityEngine;

// Token: 0x020009BA RID: 2490
[SkipSaveFileSerialization]
public class Climacophobic : StateMachineComponent<Climacophobic.StatesInstance>
{
	// Token: 0x0600486B RID: 18539 RVA: 0x0019EEB6 File Offset: 0x0019D0B6
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600486C RID: 18540 RVA: 0x0019EEC4 File Offset: 0x0019D0C4
	protected bool IsUncomfortable()
	{
		int num = 5;
		int cell = Grid.PosToCell(base.gameObject);
		if (this.isCellLadder(cell))
		{
			int num2 = 1;
			bool flag = true;
			bool flag2 = true;
			for (int i = 1; i < num; i++)
			{
				int cell2 = Grid.OffsetCell(cell, 0, i);
				int cell3 = Grid.OffsetCell(cell, 0, -i);
				if (flag && this.isCellLadder(cell2))
				{
					num2++;
				}
				else
				{
					flag = false;
				}
				if (flag2 && this.isCellLadder(cell3))
				{
					num2++;
				}
				else
				{
					flag2 = false;
				}
			}
			return num2 >= num;
		}
		return false;
	}

	// Token: 0x0600486D RID: 18541 RVA: 0x0019EF4C File Offset: 0x0019D14C
	private bool isCellLadder(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		return !(gameObject == null) && !(gameObject.GetComponent<Ladder>() == null);
	}

	// Token: 0x020019C3 RID: 6595
	public class StatesInstance : GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.GameInstance
	{
		// Token: 0x06009DFD RID: 40445 RVA: 0x003768DE File Offset: 0x00374ADE
		public StatesInstance(Climacophobic master) : base(master)
		{
		}
	}

	// Token: 0x020019C4 RID: 6596
	public class States : GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic>
	{
		// Token: 0x06009DFE RID: 40446 RVA: 0x003768E8 File Offset: 0x00374AE8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("ClimacophobicCheck", delegate(Climacophobic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Vertigo").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04007AA1 RID: 31393
		public GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.State satisfied;

		// Token: 0x04007AA2 RID: 31394
		public GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.State suffering;
	}
}

using System;

// Token: 0x020009BF RID: 2495
[SkipSaveFileSerialization]
public class SolitarySleeper : StateMachineComponent<SolitarySleeper.StatesInstance>
{
	// Token: 0x06004879 RID: 18553 RVA: 0x0019F067 File Offset: 0x0019D267
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600487A RID: 18554 RVA: 0x0019F074 File Offset: 0x0019D274
	protected bool IsUncomfortable()
	{
		if (!base.gameObject.GetSMI<StaminaMonitor.Instance>().IsSleeping())
		{
			return false;
		}
		int num = 5;
		bool flag = true;
		bool flag2 = true;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 1; i < num; i++)
		{
			int num2 = Grid.OffsetCell(cell, i, 0);
			int num3 = Grid.OffsetCell(cell, -i, 0);
			if (Grid.Solid[num3])
			{
				flag = false;
			}
			if (Grid.Solid[num2])
			{
				flag2 = false;
			}
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				if (flag && Grid.PosToCell(minionIdentity.gameObject) == num3)
				{
					return true;
				}
				if (flag2 && Grid.PosToCell(minionIdentity.gameObject) == num2)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x020019CD RID: 6605
	public class StatesInstance : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.GameInstance
	{
		// Token: 0x06009E12 RID: 40466 RVA: 0x00376BEA File Offset: 0x00374DEA
		public StatesInstance(SolitarySleeper master) : base(master)
		{
		}
	}

	// Token: 0x020019CE RID: 6606
	public class States : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper>
	{
		// Token: 0x06009E13 RID: 40467 RVA: 0x00376BF4 File Offset: 0x00374DF4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.TagTransition(GameTags.Dead, null, false).EventTransition(GameHashes.NewDay, this.satisfied, null).Update("SolitarySleeperCheck", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					if (smi.GetCurrentState() != this.suffering)
					{
						smi.GoTo(this.suffering);
						return;
					}
				}
				else if (smi.GetCurrentState() != this.satisfied)
				{
					smi.GoTo(this.satisfied);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.suffering.AddEffect("PeopleTooCloseWhileSleeping").ToggleExpression(Db.Get().Expressions.Uncomfortable, null).Update("PeopleTooCloseSleepFail", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				smi.master.gameObject.Trigger(1338475637, this);
			}, UpdateRate.SIM_1000ms, false);
			this.satisfied.DoNothing();
		}

		// Token: 0x04007AA9 RID: 31401
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State satisfied;

		// Token: 0x04007AAA RID: 31402
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State suffering;
	}
}

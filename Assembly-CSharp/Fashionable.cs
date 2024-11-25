using System;

// Token: 0x020009BB RID: 2491
[SkipSaveFileSerialization]
public class Fashionable : StateMachineComponent<Fashionable.StatesInstance>
{
	// Token: 0x0600486F RID: 18543 RVA: 0x0019EF94 File Offset: 0x0019D194
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x0019EFA4 File Offset: 0x0019D1A4
	protected bool IsUncomfortable()
	{
		ClothingWearer component = base.GetComponent<ClothingWearer>();
		return component != null && component.currentClothing.decorMod <= 0;
	}

	// Token: 0x020019C5 RID: 6597
	public class StatesInstance : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.GameInstance
	{
		// Token: 0x06009E01 RID: 40449 RVA: 0x0037697E File Offset: 0x00374B7E
		public StatesInstance(Fashionable master) : base(master)
		{
		}
	}

	// Token: 0x020019C6 RID: 6598
	public class States : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable>
	{
		// Token: 0x06009E02 RID: 40450 RVA: 0x00376988 File Offset: 0x00374B88
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.EventHandler(GameHashes.EquippedItemEquipper, delegate(Fashionable.StatesInstance smi)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}).EventHandler(GameHashes.UnequippedItemEquipper, delegate(Fashionable.StatesInstance smi)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			});
			this.suffering.AddEffect("UnfashionableClothing").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04007AA3 RID: 31395
		public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State satisfied;

		// Token: 0x04007AA4 RID: 31396
		public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State suffering;
	}
}

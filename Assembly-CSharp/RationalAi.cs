using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
public class RationalAi : GameStateMachine<RationalAi, RationalAi.Instance>
{
	// Token: 0x060016AC RID: 5804 RVA: 0x00079844 File Offset: 0x00077A44
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RationalAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RationalAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.TagTransition(GameTags.Dead, this.dead, false).ToggleStateMachineList(new Func<RationalAi.Instance, Func<RationalAi.Instance, StateMachine.Instance>[]>(RationalAi.GetStateMachinesToRunWhenAlive));
		this.dead.ToggleStateMachine((RationalAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).ToggleBrain("dead").Enter("RefreshUserMenu", delegate(RationalAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RationalAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		});
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x00079945 File Offset: 0x00077B45
	public static Func<RationalAi.Instance, StateMachine.Instance>[] GetStateMachinesToRunWhenAlive(RationalAi.Instance smi)
	{
		return smi.stateMachinesToRunWhenAlive;
	}

	// Token: 0x04000CA8 RID: 3240
	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State alive;

	// Token: 0x04000CA9 RID: 3241
	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x0200118D RID: 4493
	public new class Instance : GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008022 RID: 32802 RVA: 0x0030EF68 File Offset: 0x0030D168
		public Instance(IStateMachineTarget master, Tag minionModel) : base(master)
		{
			this.MinionModel = minionModel;
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
			component.prioritizeBrainIfNoChore = true;
		}

		// Token: 0x06008023 RID: 32803 RVA: 0x0030EFB9 File Offset: 0x0030D1B9
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}

		// Token: 0x04006059 RID: 24665
		public Tag MinionModel;

		// Token: 0x0400605A RID: 24666
		public Func<RationalAi.Instance, StateMachine.Instance>[] stateMachinesToRunWhenAlive;
	}
}

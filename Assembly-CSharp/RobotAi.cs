using System;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class RobotAi : GameStateMachine<RobotAi, RobotAi.Instance>
{
	// Token: 0x060016B0 RID: 5808 RVA: 0x00079980 File Offset: 0x00077B80
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RobotAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RobotAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.DefaultState(this.alive.normal).TagTransition(GameTags.Dead, this.dead, false).Toggle("Toggle Component Registration", delegate(RobotAi.Instance smi)
		{
			RobotAi.ToggleRegistration(smi, true);
		}, delegate(RobotAi.Instance smi)
		{
			RobotAi.ToggleRegistration(smi, false);
		});
		this.alive.normal.TagTransition(GameTags.Stored, this.alive.stored, false).Enter(delegate(RobotAi.Instance smi)
		{
			if (!smi.HasTag(GameTags.Robots.Models.FetchDrone))
			{
				smi.fallMonitor = new FallMonitor.Instance(smi.master, false, null);
				smi.fallMonitor.StartSM();
			}
		}).Exit(delegate(RobotAi.Instance smi)
		{
			if (smi.fallMonitor != null)
			{
				smi.fallMonitor.StopSM("StoredRobotAI");
			}
		});
		this.alive.stored.PlayAnim("in_storage").TagTransition(GameTags.Stored, this.alive.normal, true).ToggleBrain("stored").Enter(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Pause("stored");
		}).Exit(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Unpause("unstored");
		});
		this.dead.ToggleBrain("dead").ToggleComponentIfFound<Deconstructable>(false).ToggleStateMachine((RobotAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).Enter("RefreshUserMenu", delegate(RobotAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		}).Enter("Delete", new StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback(RobotAi.DeleteOnDeath));
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x00079BCC File Offset: 0x00077DCC
	public static void DeleteOnDeath(RobotAi.Instance smi)
	{
		if (((RobotAi.Def)smi.def).DeleteOnDead)
		{
			smi.gameObject.DeleteObject();
		}
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x00079BEB File Offset: 0x00077DEB
	private static void ToggleRegistration(RobotAi.Instance smi, bool register)
	{
		if (register)
		{
			Components.LiveRobotsIdentities.Add(smi);
			return;
		}
		Components.LiveRobotsIdentities.Remove(smi);
	}

	// Token: 0x04000CAA RID: 3242
	public RobotAi.AliveStates alive;

	// Token: 0x04000CAB RID: 3243
	public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x0200118F RID: 4495
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006060 RID: 24672
		public bool DeleteOnDead;
	}

	// Token: 0x02001190 RID: 4496
	public class AliveStates : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006061 RID: 24673
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x04006062 RID: 24674
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State stored;
	}

	// Token: 0x02001191 RID: 4497
	public new class Instance : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600802C RID: 32812 RVA: 0x0030F048 File Offset: 0x0030D248
		public Instance(IStateMachineTarget master, RobotAi.Def def) : base(master, def)
		{
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
			base.Subscribe(-1988963660, new Action<object>(this.OnBeginChore));
		}

		// Token: 0x0600802D RID: 32813 RVA: 0x0030F0A4 File Offset: 0x0030D2A4
		private void OnBeginChore(object data)
		{
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				component.DropAll(false, false, default(Vector3), true, null);
			}
		}

		// Token: 0x0600802E RID: 32814 RVA: 0x0030F0D4 File Offset: 0x0030D2D4
		protected override void OnCleanUp()
		{
			base.Unsubscribe(-1988963660, new Action<object>(this.OnBeginChore));
			base.OnCleanUp();
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x0030F0F3 File Offset: 0x0030D2F3
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}

		// Token: 0x04006063 RID: 24675
		public FallMonitor.Instance fallMonitor;
	}
}

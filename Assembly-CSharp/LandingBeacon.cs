using System;

// Token: 0x020006F8 RID: 1784
public class LandingBeacon : GameStateMachine<LandingBeacon, LandingBeacon.Instance>
{
	// Token: 0x06002D93 RID: 11667 RVA: 0x000FFC2C File Offset: 0x000FDE2C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Update(new Action<LandingBeacon.Instance, float>(LandingBeacon.UpdateLineOfSight), UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.working, (LandingBeacon.Instance smi) => smi.operational.IsOperational);
		this.working.DefaultState(this.working.pre).EventTransition(GameHashes.OperationalChanged, this.off, (LandingBeacon.Instance smi) => !smi.operational.IsOperational);
		this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
		this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter("SetActive", delegate(LandingBeacon.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive", delegate(LandingBeacon.Instance smi)
		{
			smi.operational.SetActive(false, false);
		});
	}

	// Token: 0x06002D94 RID: 11668 RVA: 0x000FFD70 File Offset: 0x000FDF70
	public static void UpdateLineOfSight(LandingBeacon.Instance smi, float dt)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		bool flag = true;
		int num = Grid.PosToCell(smi);
		int num2 = (int)myWorld.maximumBounds.y;
		while (Grid.CellRow(num) <= num2)
		{
			if (!Grid.IsValidCell(num) || Grid.Solid[num])
			{
				flag = false;
				break;
			}
			num = Grid.CellAbove(num);
		}
		if (smi.skyLastVisible != flag)
		{
			smi.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, !flag, null);
			smi.operational.SetFlag(LandingBeacon.noSurfaceSight, flag);
			smi.skyLastVisible = flag;
		}
	}

	// Token: 0x04001A79 RID: 6777
	public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04001A7A RID: 6778
	public LandingBeacon.WorkingStates working;

	// Token: 0x04001A7B RID: 6779
	public static readonly Operational.Flag noSurfaceSight = new Operational.Flag("noSurfaceSight", Operational.Flag.Type.Requirement);

	// Token: 0x0200152E RID: 5422
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200152F RID: 5423
	public class WorkingStates : GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006C2A RID: 27690
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State pre;

		// Token: 0x04006C2B RID: 27691
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State loop;

		// Token: 0x04006C2C RID: 27692
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State pst;
	}

	// Token: 0x02001530 RID: 5424
	public new class Instance : GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008D7F RID: 36223 RVA: 0x0033FC4C File Offset: 0x0033DE4C
		public Instance(IStateMachineTarget master, LandingBeacon.Def def) : base(master, def)
		{
			Components.LandingBeacons.Add(this);
			this.operational = base.GetComponent<Operational>();
			this.selectable = base.GetComponent<KSelectable>();
		}

		// Token: 0x06008D80 RID: 36224 RVA: 0x0033FC80 File Offset: 0x0033DE80
		public override void StartSM()
		{
			base.StartSM();
			LandingBeacon.UpdateLineOfSight(this, 0f);
		}

		// Token: 0x06008D81 RID: 36225 RVA: 0x0033FC93 File Offset: 0x0033DE93
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.LandingBeacons.Remove(this);
		}

		// Token: 0x06008D82 RID: 36226 RVA: 0x0033FCA6 File Offset: 0x0033DEA6
		public bool CanBeTargeted()
		{
			return base.IsInsideState(base.sm.working);
		}

		// Token: 0x04006C2D RID: 27693
		public Operational operational;

		// Token: 0x04006C2E RID: 27694
		public KSelectable selectable;

		// Token: 0x04006C2F RID: 27695
		public bool skyLastVisible = true;
	}
}

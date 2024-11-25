using System;

// Token: 0x02000780 RID: 1920
public class TeleportalPad : StateMachineComponent<TeleportalPad.StatesInstance>
{
	// Token: 0x06003430 RID: 13360 RVA: 0x0011CFB8 File Offset: 0x0011B1B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001EDB RID: 7899
	[MyCmpReq]
	private Operational operational;

	// Token: 0x02001627 RID: 5671
	public class StatesInstance : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.GameInstance
	{
		// Token: 0x06009121 RID: 37153 RVA: 0x0034F49D File Offset: 0x0034D69D
		public StatesInstance(TeleportalPad master) : base(master)
		{
		}
	}

	// Token: 0x02001628 RID: 5672
	public class States : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad>
	{
		// Token: 0x06009122 RID: 37154 RVA: 0x0034F4A8 File Offset: 0x0034D6A8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inactive;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (TeleportalPad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.inactive.PlayAnim("idle").EventTransition(GameHashes.OperationalChanged, this.no_target, (TeleportalPad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.no_target.Enter(delegate(TeleportalPad.StatesInstance smi)
			{
				if (smi.master.GetComponent<Teleporter>().HasTeleporterTarget())
				{
					smi.GoTo(this.portal_on.turn_on);
				}
			}).PlayAnim("idle").EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_on, (TeleportalPad.StatesInstance smi) => smi.master.GetComponent<Teleporter>().HasTeleporterTarget());
			this.portal_on.EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_off, (TeleportalPad.StatesInstance smi) => !smi.master.GetComponent<Teleporter>().HasTeleporterTarget());
			this.portal_on.turn_on.PlayAnim("working_pre").OnAnimQueueComplete(this.portal_on.loop);
			this.portal_on.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(TeleportalPad.StatesInstance smi, float dt)
			{
				Teleporter component = smi.master.GetComponent<Teleporter>();
				Teleporter teleporter = component.FindTeleportTarget();
				component.SetTeleportTarget(teleporter);
				if (teleporter != null)
				{
					component.TeleportObjects();
				}
			}, UpdateRate.SIM_200ms, false);
			this.portal_on.turn_off.PlayAnim("working_pst").OnAnimQueueComplete(this.no_target);
		}

		// Token: 0x04006ECD RID: 28365
		public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal targetTeleporter;

		// Token: 0x04006ECE RID: 28366
		public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal doTeleport;

		// Token: 0x04006ECF RID: 28367
		public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State inactive;

		// Token: 0x04006ED0 RID: 28368
		public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State no_target;

		// Token: 0x04006ED1 RID: 28369
		public TeleportalPad.States.PortalOnStates portal_on;

		// Token: 0x02002547 RID: 9543
		public class PortalOnStates : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State
		{
			// Token: 0x0400A61B RID: 42523
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_on;

			// Token: 0x0400A61C RID: 42524
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State loop;

			// Token: 0x0400A61D RID: 42525
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_off;
		}
	}
}

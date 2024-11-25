using System;

// Token: 0x0200074D RID: 1869
[SkipSaveFileSerialization]
public class PlanterBox : StateMachineComponent<PlanterBox.SMInstance>
{
	// Token: 0x060031D3 RID: 12755 RVA: 0x0011232C File Offset: 0x0011052C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001D53 RID: 7507
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x020015BC RID: 5564
	public class SMInstance : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.GameInstance
	{
		// Token: 0x06008FA0 RID: 36768 RVA: 0x00348252 File Offset: 0x00346452
		public SMInstance(PlanterBox master) : base(master)
		{
		}
	}

	// Token: 0x020015BD RID: 5565
	public class States : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox>
	{
		// Token: 0x06008FA1 RID: 36769 RVA: 0x0034825C File Offset: 0x0034645C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x04006DA1 RID: 28065
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State empty;

		// Token: 0x04006DA2 RID: 28066
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State full;
	}
}

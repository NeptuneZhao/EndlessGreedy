using System;

// Token: 0x020006D2 RID: 1746
public class FlowerVase : StateMachineComponent<FlowerVase.SMInstance>
{
	// Token: 0x06002C3C RID: 11324 RVA: 0x000F8575 File Offset: 0x000F6775
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002C3D RID: 11325 RVA: 0x000F857D File Offset: 0x000F677D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0400197E RID: 6526
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x0400197F RID: 6527
	[MyCmpReq]
	private KBoxCollider2D boxCollider;

	// Token: 0x020014D9 RID: 5337
	public class SMInstance : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.GameInstance
	{
		// Token: 0x06008C43 RID: 35907 RVA: 0x00339AED File Offset: 0x00337CED
		public SMInstance(FlowerVase master) : base(master)
		{
		}
	}

	// Token: 0x020014DA RID: 5338
	public class States : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase>
	{
		// Token: 0x06008C44 RID: 35908 RVA: 0x00339AF8 File Offset: 0x00337CF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x04006B1B RID: 27419
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State empty;

		// Token: 0x04006B1C RID: 27420
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State full;
	}
}

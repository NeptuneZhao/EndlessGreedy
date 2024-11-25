using System;

// Token: 0x020006CD RID: 1741
public class FarmTile : StateMachineComponent<FarmTile.SMInstance>
{
	// Token: 0x06002C0A RID: 11274 RVA: 0x000F78F3 File Offset: 0x000F5AF3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001961 RID: 6497
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x04001962 RID: 6498
	[MyCmpReq]
	private Storage storage;

	// Token: 0x020014CF RID: 5327
	public class SMInstance : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.GameInstance
	{
		// Token: 0x06008C29 RID: 35881 RVA: 0x00339232 File Offset: 0x00337432
		public SMInstance(FarmTile master) : base(master)
		{
		}

		// Token: 0x06008C2A RID: 35882 RVA: 0x0033923C File Offset: 0x0033743C
		public bool HasWater()
		{
			PrimaryElement primaryElement = base.master.storage.FindPrimaryElement(SimHashes.Water);
			return primaryElement != null && primaryElement.Mass > 0f;
		}
	}

	// Token: 0x020014D0 RID: 5328
	public class States : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile>
	{
		// Token: 0x06008C2B RID: 35883 RVA: 0x00339278 File Offset: 0x00337478
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant != null);
			this.empty.wet.EventTransition(GameHashes.OnStorageChange, this.empty.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.empty.dry.EventTransition(GameHashes.OnStorageChange, this.empty.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant == null);
			this.full.wet.EventTransition(GameHashes.OnStorageChange, this.full.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.dry.EventTransition(GameHashes.OnStorageChange, this.full.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
		}

		// Token: 0x04006B00 RID: 27392
		public FarmTile.States.FarmStates empty;

		// Token: 0x04006B01 RID: 27393
		public FarmTile.States.FarmStates full;

		// Token: 0x020024D3 RID: 9427
		public class FarmStates : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State
		{
			// Token: 0x0400A386 RID: 41862
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State wet;

			// Token: 0x0400A387 RID: 41863
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State dry;
		}
	}
}

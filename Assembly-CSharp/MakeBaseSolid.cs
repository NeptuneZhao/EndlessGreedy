using System;

// Token: 0x02000722 RID: 1826
public class MakeBaseSolid : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>
{
	// Token: 0x0600304F RID: 12367 RVA: 0x0010AFA6 File Offset: 0x001091A6
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToSolid)).Exit(new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToVacuum));
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x0010AFDC File Offset: 0x001091DC
	private static void ConvertToSolid(MakeBaseSolid.Instance smi)
	{
		if (smi.buildingComplete == null)
		{
			return;
		}
		int cell = Grid.PosToCell(smi.gameObject);
		PrimaryElement component = smi.GetComponent<PrimaryElement>();
		Building component2 = smi.GetComponent<Building>();
		foreach (CellOffset offset in smi.def.solidOffsets)
		{
			CellOffset rotatedOffset = component2.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			if (smi.def.occupyFoundationLayer)
			{
				SimMessages.ReplaceAndDisplaceElement(num, component.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, component.Mass, component.Temperature, byte.MaxValue, 0, -1);
				Grid.Objects[num, 9] = smi.gameObject;
			}
			else
			{
				SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0f, 0f, byte.MaxValue, 0, -1);
			}
			Grid.Foundation[num] = true;
			Grid.SetSolid(num, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
			SimMessages.SetCellProperties(num, 103);
			Grid.RenderedByWorld[num] = false;
			World.Instance.OnSolidChanged(num);
			GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
	}

	// Token: 0x06003051 RID: 12369 RVA: 0x0010B128 File Offset: 0x00109328
	private static void ConvertToVacuum(MakeBaseSolid.Instance smi)
	{
		if (smi.buildingComplete == null)
		{
			return;
		}
		int cell = Grid.PosToCell(smi.gameObject);
		Building component = smi.GetComponent<Building>();
		foreach (CellOffset offset in smi.def.solidOffsets)
		{
			CellOffset rotatedOffset = component.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0f, -1f, byte.MaxValue, 0, -1);
			Grid.Objects[num, 9] = null;
			Grid.Foundation[num] = false;
			Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierDestroy);
			SimMessages.ClearCellProperties(num, 103);
			Grid.RenderedByWorld[num] = true;
			World.Instance.OnSolidChanged(num);
			GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
	}

	// Token: 0x04001C50 RID: 7248
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)103;

	// Token: 0x02001564 RID: 5476
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006CA5 RID: 27813
		public CellOffset[] solidOffsets;

		// Token: 0x04006CA6 RID: 27814
		public bool occupyFoundationLayer = true;
	}

	// Token: 0x02001565 RID: 5477
	public new class Instance : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.GameInstance
	{
		// Token: 0x06008E40 RID: 36416 RVA: 0x003427DD File Offset: 0x003409DD
		public Instance(IStateMachineTarget master, MakeBaseSolid.Def def) : base(master, def)
		{
		}

		// Token: 0x04006CA7 RID: 27815
		[MyCmpGet]
		public BuildingComplete buildingComplete;
	}
}

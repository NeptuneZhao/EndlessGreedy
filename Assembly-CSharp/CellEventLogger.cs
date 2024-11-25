using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x020008A0 RID: 2208
public class CellEventLogger : EventLogger<CellEventInstance, CellEvent>
{
	// Token: 0x06003DD1 RID: 15825 RVA: 0x00155621 File Offset: 0x00153821
	public static void DestroyInstance()
	{
		CellEventLogger.Instance = null;
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x00155629 File Offset: 0x00153829
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackSend(int cell, int callback_id)
	{
		if (callback_id != -1)
		{
			this.CallbackToCellMap[callback_id] = cell;
		}
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x0015563C File Offset: 0x0015383C
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackReceive(int callback_id)
	{
		int invalidCell = Grid.InvalidCell;
		this.CallbackToCellMap.TryGetValue(callback_id, out invalidCell);
	}

	// Token: 0x06003DD4 RID: 15828 RVA: 0x00155660 File Offset: 0x00153860
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CellEventLogger.Instance = this;
		this.SimMessagesSolid = (base.AddEvent(new CellSolidEvent("SimMessageSolid", "Sim Message", false, true)) as CellSolidEvent);
		this.SimCellOccupierDestroy = (base.AddEvent(new CellSolidEvent("SimCellOccupierClearSolid", "Sim Cell Occupier Destroy", false, true)) as CellSolidEvent);
		this.SimCellOccupierForceSolid = (base.AddEvent(new CellSolidEvent("SimCellOccupierForceSolid", "Sim Cell Occupier Force Solid", false, true)) as CellSolidEvent);
		this.SimCellOccupierSolidChanged = (base.AddEvent(new CellSolidEvent("SimCellOccupierSolidChanged", "Sim Cell Occupier Solid Changed", false, true)) as CellSolidEvent);
		this.DoorOpen = (base.AddEvent(new CellElementEvent("DoorOpen", "Door Open", true, true)) as CellElementEvent);
		this.DoorClose = (base.AddEvent(new CellElementEvent("DoorClose", "Door Close", true, true)) as CellElementEvent);
		this.Excavator = (base.AddEvent(new CellElementEvent("Excavator", "Excavator", true, true)) as CellElementEvent);
		this.DebugTool = (base.AddEvent(new CellElementEvent("DebugTool", "Debug Tool", true, true)) as CellElementEvent);
		this.SandBoxTool = (base.AddEvent(new CellElementEvent("SandBoxTool", "Sandbox Tool", true, true)) as CellElementEvent);
		this.TemplateLoader = (base.AddEvent(new CellElementEvent("TemplateLoader", "Template Loader", true, true)) as CellElementEvent);
		this.Scenario = (base.AddEvent(new CellElementEvent("Scenario", "Scenario", true, true)) as CellElementEvent);
		this.SimCellOccupierOnSpawn = (base.AddEvent(new CellElementEvent("SimCellOccupierOnSpawn", "Sim Cell Occupier OnSpawn", true, true)) as CellElementEvent);
		this.SimCellOccupierDestroySelf = (base.AddEvent(new CellElementEvent("SimCellOccupierDestroySelf", "Sim Cell Occupier Destroy Self", true, true)) as CellElementEvent);
		this.WorldGapManager = (base.AddEvent(new CellElementEvent("WorldGapManager", "World Gap Manager", true, true)) as CellElementEvent);
		this.ReceiveElementChanged = (base.AddEvent(new CellElementEvent("ReceiveElementChanged", "Sim Message", false, false)) as CellElementEvent);
		this.ObjectSetSimOnSpawn = (base.AddEvent(new CellElementEvent("ObjectSetSimOnSpawn", "Object set sim on spawn", true, true)) as CellElementEvent);
		this.DecompositionDirtyWater = (base.AddEvent(new CellElementEvent("DecompositionDirtyWater", "Decomposition dirty water", true, true)) as CellElementEvent);
		this.SendCallback = (base.AddEvent(new CellCallbackEvent("SendCallback", true, true)) as CellCallbackEvent);
		this.ReceiveCallback = (base.AddEvent(new CellCallbackEvent("ReceiveCallback", false, true)) as CellCallbackEvent);
		this.Dig = (base.AddEvent(new CellDigEvent(true)) as CellDigEvent);
		this.WorldDamageDelayedSpawnFX = (base.AddEvent(new CellAddRemoveSubstanceEvent("WorldDamageDelayedSpawnFX", "World Damage Delayed Spawn FX", false)) as CellAddRemoveSubstanceEvent);
		this.OxygenModifierSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxygenModifierSimUpdate", "Oxygen Modifier SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.LiquidChunkOnStore = (base.AddEvent(new CellAddRemoveSubstanceEvent("LiquidChunkOnStore", "Liquid Chunk On Store", false)) as CellAddRemoveSubstanceEvent);
		this.FallingWaterAddToSim = (base.AddEvent(new CellAddRemoveSubstanceEvent("FallingWaterAddToSim", "Falling Water Add To Sim", false)) as CellAddRemoveSubstanceEvent);
		this.ExploderOnSpawn = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExploderOnSpawn", "Exploder OnSpawn", false)) as CellAddRemoveSubstanceEvent);
		this.ExhaustSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExhaustSimUpdate", "Exhaust SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.ElementConsumerSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementConsumerSimUpdate", "Element Consumer SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.SublimatesEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("SublimatesEmit", "Sublimates Emit", false)) as CellAddRemoveSubstanceEvent);
		this.Mop = (base.AddEvent(new CellAddRemoveSubstanceEvent("Mop", "Mop", false)) as CellAddRemoveSubstanceEvent);
		this.OreMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("OreMelted", "Ore Melted", false)) as CellAddRemoveSubstanceEvent);
		this.ConstructTile = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConstructTile", "ConstructTile", false)) as CellAddRemoveSubstanceEvent);
		this.Dumpable = (base.AddEvent(new CellAddRemoveSubstanceEvent("Dympable", "Dumpable", false)) as CellAddRemoveSubstanceEvent);
		this.Cough = (base.AddEvent(new CellAddRemoveSubstanceEvent("Cough", "Cough", false)) as CellAddRemoveSubstanceEvent);
		this.Meteor = (base.AddEvent(new CellAddRemoveSubstanceEvent("Meteor", "Meteor", false)) as CellAddRemoveSubstanceEvent);
		this.ElementChunkTransition = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementChunkTransition", "Element Chunk Transition", false)) as CellAddRemoveSubstanceEvent);
		this.OxyrockEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxyrockEmit", "Oxyrock Emit", false)) as CellAddRemoveSubstanceEvent);
		this.BleachstoneEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("BleachstoneEmit", "Bleachstone Emit", false)) as CellAddRemoveSubstanceEvent);
		this.UnstableGround = (base.AddEvent(new CellAddRemoveSubstanceEvent("UnstableGround", "Unstable Ground", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitFlowEmptyConduit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitFlowEmptyConduit", "Conduit Flow Empty Conduit", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitConsumerWrongElement = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitConsumerWrongElement", "Conduit Consumer Wrong Element", false)) as CellAddRemoveSubstanceEvent);
		this.OverheatableMeltingDown = (base.AddEvent(new CellAddRemoveSubstanceEvent("OverheatableMeltingDown", "Overheatable MeltingDown", false)) as CellAddRemoveSubstanceEvent);
		this.FabricatorProduceMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("FabricatorProduceMelted", "Fabricator Produce Melted", false)) as CellAddRemoveSubstanceEvent);
		this.PumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("PumpSimUpdate", "Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.WallPumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("WallPumpSimUpdate", "Wall Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.Vomit = (base.AddEvent(new CellAddRemoveSubstanceEvent("Vomit", "Vomit", false)) as CellAddRemoveSubstanceEvent);
		this.Tears = (base.AddEvent(new CellAddRemoveSubstanceEvent("Tears", "Tears", false)) as CellAddRemoveSubstanceEvent);
		this.Pee = (base.AddEvent(new CellAddRemoveSubstanceEvent("Pee", "Pee", false)) as CellAddRemoveSubstanceEvent);
		this.AlgaeHabitat = (base.AddEvent(new CellAddRemoveSubstanceEvent("AlgaeHabitat", "AlgaeHabitat", false)) as CellAddRemoveSubstanceEvent);
		this.CO2FilterOxygen = (base.AddEvent(new CellAddRemoveSubstanceEvent("CO2FilterOxygen", "CO2FilterOxygen", false)) as CellAddRemoveSubstanceEvent);
		this.ToiletEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ToiletEmit", "ToiletEmit", false)) as CellAddRemoveSubstanceEvent);
		this.ElementEmitted = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementEmitted", "Element Emitted", false)) as CellAddRemoveSubstanceEvent);
		this.CO2ManagerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ManagerFixedUpdate", "CO2Manager FixedUpdate", false)) as CellModifyMassEvent);
		this.EnvironmentConsumerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("EnvironmentConsumerFixedUpdate", "EnvironmentConsumer FixedUpdate", false)) as CellModifyMassEvent);
		this.ExcavatorShockwave = (base.AddEvent(new CellModifyMassEvent("ExcavatorShockwave", "Excavator Shockwave", false)) as CellModifyMassEvent);
		this.OxygenBreatherSimUpdate = (base.AddEvent(new CellModifyMassEvent("OxygenBreatherSimUpdate", "Oxygen Breather SimUpdate", false)) as CellModifyMassEvent);
		this.CO2ScrubberSimUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ScrubberSimUpdate", "CO2Scrubber SimUpdate", false)) as CellModifyMassEvent);
		this.RiverSourceSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverSourceSimUpdate", "RiverSource SimUpdate", false)) as CellModifyMassEvent);
		this.RiverTerminusSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverTerminusSimUpdate", "RiverTerminus SimUpdate", false)) as CellModifyMassEvent);
		this.DebugToolModifyMass = (base.AddEvent(new CellModifyMassEvent("DebugToolModifyMass", "DebugTool ModifyMass", false)) as CellModifyMassEvent);
		this.EnergyGeneratorModifyMass = (base.AddEvent(new CellModifyMassEvent("EnergyGeneratorModifyMass", "EnergyGenerator ModifyMass", false)) as CellModifyMassEvent);
		this.SolidFilterEvent = (base.AddEvent(new CellSolidFilterEvent("SolidFilterEvent", true)) as CellSolidFilterEvent);
	}

	// Token: 0x040025B6 RID: 9654
	public static CellEventLogger Instance;

	// Token: 0x040025B7 RID: 9655
	public CellSolidEvent SimMessagesSolid;

	// Token: 0x040025B8 RID: 9656
	public CellSolidEvent SimCellOccupierDestroy;

	// Token: 0x040025B9 RID: 9657
	public CellSolidEvent SimCellOccupierForceSolid;

	// Token: 0x040025BA RID: 9658
	public CellSolidEvent SimCellOccupierSolidChanged;

	// Token: 0x040025BB RID: 9659
	public CellElementEvent DoorOpen;

	// Token: 0x040025BC RID: 9660
	public CellElementEvent DoorClose;

	// Token: 0x040025BD RID: 9661
	public CellElementEvent Excavator;

	// Token: 0x040025BE RID: 9662
	public CellElementEvent DebugTool;

	// Token: 0x040025BF RID: 9663
	public CellElementEvent SandBoxTool;

	// Token: 0x040025C0 RID: 9664
	public CellElementEvent TemplateLoader;

	// Token: 0x040025C1 RID: 9665
	public CellElementEvent Scenario;

	// Token: 0x040025C2 RID: 9666
	public CellElementEvent SimCellOccupierOnSpawn;

	// Token: 0x040025C3 RID: 9667
	public CellElementEvent SimCellOccupierDestroySelf;

	// Token: 0x040025C4 RID: 9668
	public CellElementEvent WorldGapManager;

	// Token: 0x040025C5 RID: 9669
	public CellElementEvent ReceiveElementChanged;

	// Token: 0x040025C6 RID: 9670
	public CellElementEvent ObjectSetSimOnSpawn;

	// Token: 0x040025C7 RID: 9671
	public CellElementEvent DecompositionDirtyWater;

	// Token: 0x040025C8 RID: 9672
	public CellElementEvent LaunchpadDesolidify;

	// Token: 0x040025C9 RID: 9673
	public CellCallbackEvent SendCallback;

	// Token: 0x040025CA RID: 9674
	public CellCallbackEvent ReceiveCallback;

	// Token: 0x040025CB RID: 9675
	public CellDigEvent Dig;

	// Token: 0x040025CC RID: 9676
	public CellAddRemoveSubstanceEvent WorldDamageDelayedSpawnFX;

	// Token: 0x040025CD RID: 9677
	public CellAddRemoveSubstanceEvent SublimatesEmit;

	// Token: 0x040025CE RID: 9678
	public CellAddRemoveSubstanceEvent OxygenModifierSimUpdate;

	// Token: 0x040025CF RID: 9679
	public CellAddRemoveSubstanceEvent LiquidChunkOnStore;

	// Token: 0x040025D0 RID: 9680
	public CellAddRemoveSubstanceEvent FallingWaterAddToSim;

	// Token: 0x040025D1 RID: 9681
	public CellAddRemoveSubstanceEvent ExploderOnSpawn;

	// Token: 0x040025D2 RID: 9682
	public CellAddRemoveSubstanceEvent ExhaustSimUpdate;

	// Token: 0x040025D3 RID: 9683
	public CellAddRemoveSubstanceEvent ElementConsumerSimUpdate;

	// Token: 0x040025D4 RID: 9684
	public CellAddRemoveSubstanceEvent ElementChunkTransition;

	// Token: 0x040025D5 RID: 9685
	public CellAddRemoveSubstanceEvent OxyrockEmit;

	// Token: 0x040025D6 RID: 9686
	public CellAddRemoveSubstanceEvent BleachstoneEmit;

	// Token: 0x040025D7 RID: 9687
	public CellAddRemoveSubstanceEvent UnstableGround;

	// Token: 0x040025D8 RID: 9688
	public CellAddRemoveSubstanceEvent ConduitFlowEmptyConduit;

	// Token: 0x040025D9 RID: 9689
	public CellAddRemoveSubstanceEvent ConduitConsumerWrongElement;

	// Token: 0x040025DA RID: 9690
	public CellAddRemoveSubstanceEvent OverheatableMeltingDown;

	// Token: 0x040025DB RID: 9691
	public CellAddRemoveSubstanceEvent FabricatorProduceMelted;

	// Token: 0x040025DC RID: 9692
	public CellAddRemoveSubstanceEvent PumpSimUpdate;

	// Token: 0x040025DD RID: 9693
	public CellAddRemoveSubstanceEvent WallPumpSimUpdate;

	// Token: 0x040025DE RID: 9694
	public CellAddRemoveSubstanceEvent Vomit;

	// Token: 0x040025DF RID: 9695
	public CellAddRemoveSubstanceEvent Tears;

	// Token: 0x040025E0 RID: 9696
	public CellAddRemoveSubstanceEvent Pee;

	// Token: 0x040025E1 RID: 9697
	public CellAddRemoveSubstanceEvent AlgaeHabitat;

	// Token: 0x040025E2 RID: 9698
	public CellAddRemoveSubstanceEvent CO2FilterOxygen;

	// Token: 0x040025E3 RID: 9699
	public CellAddRemoveSubstanceEvent ToiletEmit;

	// Token: 0x040025E4 RID: 9700
	public CellAddRemoveSubstanceEvent ElementEmitted;

	// Token: 0x040025E5 RID: 9701
	public CellAddRemoveSubstanceEvent Mop;

	// Token: 0x040025E6 RID: 9702
	public CellAddRemoveSubstanceEvent OreMelted;

	// Token: 0x040025E7 RID: 9703
	public CellAddRemoveSubstanceEvent ConstructTile;

	// Token: 0x040025E8 RID: 9704
	public CellAddRemoveSubstanceEvent Dumpable;

	// Token: 0x040025E9 RID: 9705
	public CellAddRemoveSubstanceEvent Cough;

	// Token: 0x040025EA RID: 9706
	public CellAddRemoveSubstanceEvent Meteor;

	// Token: 0x040025EB RID: 9707
	public CellModifyMassEvent CO2ManagerFixedUpdate;

	// Token: 0x040025EC RID: 9708
	public CellModifyMassEvent EnvironmentConsumerFixedUpdate;

	// Token: 0x040025ED RID: 9709
	public CellModifyMassEvent ExcavatorShockwave;

	// Token: 0x040025EE RID: 9710
	public CellModifyMassEvent OxygenBreatherSimUpdate;

	// Token: 0x040025EF RID: 9711
	public CellModifyMassEvent CO2ScrubberSimUpdate;

	// Token: 0x040025F0 RID: 9712
	public CellModifyMassEvent RiverSourceSimUpdate;

	// Token: 0x040025F1 RID: 9713
	public CellModifyMassEvent RiverTerminusSimUpdate;

	// Token: 0x040025F2 RID: 9714
	public CellModifyMassEvent DebugToolModifyMass;

	// Token: 0x040025F3 RID: 9715
	public CellModifyMassEvent EnergyGeneratorModifyMass;

	// Token: 0x040025F4 RID: 9716
	public CellSolidFilterEvent SolidFilterEvent;

	// Token: 0x040025F5 RID: 9717
	public Dictionary<int, int> CallbackToCellMap = new Dictionary<int, int>();
}

using System;
using STRINGS;

// Token: 0x0200025F RID: 607
public class LogicGateAndConfig : LogicGateBaseConfig
{
	// Token: 0x06000C7D RID: 3197 RVA: 0x000491D2 File Offset: 0x000473D2
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.And;
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000491D5 File Offset: 0x000473D5
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none,
				new CellOffset(0, 1)
			};
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000C7F RID: 3199 RVA: 0x000491F7 File Offset: 0x000473F7
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0004920D File Offset: 0x0004740D
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x00049210 File Offset: 0x00047410
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0004925D File Offset: 0x0004745D
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateAND", "logic_and_kanim", 2, 2);
	}

	// Token: 0x040007FF RID: 2047
	public const string ID = "LogicGateAND";
}

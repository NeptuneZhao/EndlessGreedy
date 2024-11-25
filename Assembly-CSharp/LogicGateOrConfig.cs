using System;
using STRINGS;

// Token: 0x02000260 RID: 608
public class LogicGateOrConfig : LogicGateBaseConfig
{
	// Token: 0x06000C84 RID: 3204 RVA: 0x00049279 File Offset: 0x00047479
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Or;
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0004927C File Offset: 0x0004747C
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

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0004929E File Offset: 0x0004749E
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

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000C87 RID: 3207 RVA: 0x000492B4 File Offset: 0x000474B4
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x000492B8 File Offset: 0x000474B8
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00049305 File Offset: 0x00047505
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateOR", "logic_or_kanim", 2, 2);
	}

	// Token: 0x04000800 RID: 2048
	public const string ID = "LogicGateOR";
}

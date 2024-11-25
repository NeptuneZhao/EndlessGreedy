using System;
using STRINGS;

// Token: 0x02000262 RID: 610
public class LogicGateNotConfig : LogicGateBaseConfig
{
	// Token: 0x06000C92 RID: 3218 RVA: 0x000493C9 File Offset: 0x000475C9
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Not;
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000C93 RID: 3219 RVA: 0x000493CC File Offset: 0x000475CC
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none
			};
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000C94 RID: 3220 RVA: 0x000493E0 File Offset: 0x000475E0
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

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000C95 RID: 3221 RVA: 0x000493F6 File Offset: 0x000475F6
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x000493FC File Offset: 0x000475FC
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x00049449 File Offset: 0x00047649
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateNOT", "logic_not_kanim", 2, 1);
	}

	// Token: 0x04000802 RID: 2050
	public const string ID = "LogicGateNOT";
}

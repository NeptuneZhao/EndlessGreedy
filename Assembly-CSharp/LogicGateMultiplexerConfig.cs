using System;
using STRINGS;

// Token: 0x02000265 RID: 613
public class LogicGateMultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x06000CAB RID: 3243 RVA: 0x0004969D File Offset: 0x0004789D
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000CAC RID: 3244 RVA: 0x000496A0 File Offset: 0x000478A0
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3),
				new CellOffset(-1, 2),
				new CellOffset(-1, 1),
				new CellOffset(-1, 0)
			};
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000CAD RID: 3245 RVA: 0x000496E0 File Offset: 0x000478E0
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3)
			};
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000CAE RID: 3246 RVA: 0x000496F6 File Offset: 0x000478F6
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(0, 0),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0004971C File Offset: 0x0004791C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x00049769 File Offset: 0x00047969
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateMultiplexer", "logic_multiplexer_kanim", 3, 4);
	}

	// Token: 0x04000805 RID: 2053
	public const string ID = "LogicGateMultiplexer";
}

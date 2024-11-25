using System;
using STRINGS;

// Token: 0x02000266 RID: 614
public class LogicGateDemultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x06000CB2 RID: 3250 RVA: 0x00049785 File Offset: 0x00047985
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00049788 File Offset: 0x00047988
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3)
			};
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0004979E File Offset: 0x0004799E
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3),
				new CellOffset(1, 2),
				new CellOffset(1, 1),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x000497DE File Offset: 0x000479DE
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(0, 0)
			};
		}
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x00049804 File Offset: 0x00047A04
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x00049851 File Offset: 0x00047A51
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateDemultiplexer", "logic_demultiplexer_kanim", 3, 4);
	}

	// Token: 0x04000806 RID: 2054
	public const string ID = "LogicGateDemultiplexer";
}

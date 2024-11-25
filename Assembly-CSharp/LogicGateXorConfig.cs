using System;
using STRINGS;

// Token: 0x02000261 RID: 609
public class LogicGateXorConfig : LogicGateBaseConfig
{
	// Token: 0x06000C8B RID: 3211 RVA: 0x00049321 File Offset: 0x00047521
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Xor;
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00049324 File Offset: 0x00047524
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

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000C8D RID: 3213 RVA: 0x00049346 File Offset: 0x00047546
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

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0004935C File Offset: 0x0004755C
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x00049360 File Offset: 0x00047560
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

	// Token: 0x06000C90 RID: 3216 RVA: 0x000493AD File Offset: 0x000475AD
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateXOR", "logic_xor_kanim", 2, 2);
	}

	// Token: 0x04000801 RID: 2049
	public const string ID = "LogicGateXOR";
}

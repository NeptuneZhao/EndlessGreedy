using System;
using UnityEngine;

// Token: 0x02000708 RID: 1800
[AddComponentMenu("KMonoBehaviour/scripts/LogicGateBase")]
public class LogicGateBase : KMonoBehaviour
{
	// Token: 0x06002E7D RID: 11901 RVA: 0x0010405C File Offset: 0x0010225C
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06002E7E RID: 11902 RVA: 0x00104098 File Offset: 0x00102298
	public int InputCellOne
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[0]);
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06002E7F RID: 11903 RVA: 0x001040AC File Offset: 0x001022AC
	public int InputCellTwo
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[1]);
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06002E80 RID: 11904 RVA: 0x001040C0 File Offset: 0x001022C0
	public int InputCellThree
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[2]);
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06002E81 RID: 11905 RVA: 0x001040D4 File Offset: 0x001022D4
	public int InputCellFour
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[3]);
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06002E82 RID: 11906 RVA: 0x001040E8 File Offset: 0x001022E8
	public int OutputCellOne
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[0]);
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06002E83 RID: 11907 RVA: 0x001040FC File Offset: 0x001022FC
	public int OutputCellTwo
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[1]);
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06002E84 RID: 11908 RVA: 0x00104110 File Offset: 0x00102310
	public int OutputCellThree
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[2]);
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06002E85 RID: 11909 RVA: 0x00104124 File Offset: 0x00102324
	public int OutputCellFour
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[3]);
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06002E86 RID: 11910 RVA: 0x00104138 File Offset: 0x00102338
	public int ControlCellOne
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[0]);
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06002E87 RID: 11911 RVA: 0x0010414C File Offset: 0x0010234C
	public int ControlCellTwo
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[1]);
		}
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x00104160 File Offset: 0x00102360
	public int PortCell(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.InputCellOne;
		case LogicGateBase.PortId.InputTwo:
			return this.InputCellTwo;
		case LogicGateBase.PortId.InputThree:
			return this.InputCellThree;
		case LogicGateBase.PortId.InputFour:
			return this.InputCellFour;
		case LogicGateBase.PortId.OutputOne:
			return this.OutputCellOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.OutputCellTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.OutputCellThree;
		case LogicGateBase.PortId.OutputFour:
			return this.OutputCellFour;
		case LogicGateBase.PortId.ControlOne:
			return this.ControlCellOne;
		case LogicGateBase.PortId.ControlTwo:
			return this.ControlCellTwo;
		default:
			return this.OutputCellOne;
		}
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x001041EC File Offset: 0x001023EC
	public bool TryGetPortAtCell(int cell, out LogicGateBase.PortId port)
	{
		if (cell == this.InputCellOne)
		{
			port = LogicGateBase.PortId.InputOne;
			return true;
		}
		if ((this.RequiresTwoInputs || this.RequiresFourInputs) && cell == this.InputCellTwo)
		{
			port = LogicGateBase.PortId.InputTwo;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellThree)
		{
			port = LogicGateBase.PortId.InputThree;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellFour)
		{
			port = LogicGateBase.PortId.InputFour;
			return true;
		}
		if (cell == this.OutputCellOne)
		{
			port = LogicGateBase.PortId.OutputOne;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellTwo)
		{
			port = LogicGateBase.PortId.OutputTwo;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellThree)
		{
			port = LogicGateBase.PortId.OutputThree;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellFour)
		{
			port = LogicGateBase.PortId.OutputFour;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellOne)
		{
			port = LogicGateBase.PortId.ControlOne;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellTwo)
		{
			port = LogicGateBase.PortId.ControlTwo;
			return true;
		}
		port = LogicGateBase.PortId.InputOne;
		return false;
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06002E8A RID: 11914 RVA: 0x001042D2 File Offset: 0x001024D2
	public bool RequiresTwoInputs
	{
		get
		{
			return LogicGateBase.OpRequiresTwoInputs(this.op);
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06002E8B RID: 11915 RVA: 0x001042DF File Offset: 0x001024DF
	public bool RequiresFourInputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourInputs(this.op);
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06002E8C RID: 11916 RVA: 0x001042EC File Offset: 0x001024EC
	public bool RequiresFourOutputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourOutputs(this.op);
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06002E8D RID: 11917 RVA: 0x001042F9 File Offset: 0x001024F9
	public bool RequiresControlInputs
	{
		get
		{
			return LogicGateBase.OpRequiresControlInputs(this.op);
		}
	}

	// Token: 0x06002E8E RID: 11918 RVA: 0x00104306 File Offset: 0x00102506
	public static bool OpRequiresTwoInputs(LogicGateBase.Op op)
	{
		return op != LogicGateBase.Op.Not && op - LogicGateBase.Op.CustomSingle > 2;
	}

	// Token: 0x06002E8F RID: 11919 RVA: 0x00104315 File Offset: 0x00102515
	public static bool OpRequiresFourInputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x0010431E File Offset: 0x0010251E
	public static bool OpRequiresFourOutputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x06002E91 RID: 11921 RVA: 0x00104327 File Offset: 0x00102527
	public static bool OpRequiresControlInputs(LogicGateBase.Op op)
	{
		return op - LogicGateBase.Op.Multiplexer <= 1;
	}

	// Token: 0x04001B1D RID: 6941
	public static LogicModeUI uiSrcData;

	// Token: 0x04001B1E RID: 6942
	public static readonly HashedString OUTPUT_TWO_PORT_ID = new HashedString("LogicGateOutputTwo");

	// Token: 0x04001B1F RID: 6943
	public static readonly HashedString OUTPUT_THREE_PORT_ID = new HashedString("LogicGateOutputThree");

	// Token: 0x04001B20 RID: 6944
	public static readonly HashedString OUTPUT_FOUR_PORT_ID = new HashedString("LogicGateOutputFour");

	// Token: 0x04001B21 RID: 6945
	[SerializeField]
	public LogicGateBase.Op op;

	// Token: 0x04001B22 RID: 6946
	public static CellOffset[] portOffsets = new CellOffset[]
	{
		CellOffset.none,
		new CellOffset(0, 1),
		new CellOffset(1, 0)
	};

	// Token: 0x04001B23 RID: 6947
	public CellOffset[] inputPortOffsets;

	// Token: 0x04001B24 RID: 6948
	public CellOffset[] outputPortOffsets;

	// Token: 0x04001B25 RID: 6949
	public CellOffset[] controlPortOffsets;

	// Token: 0x02001544 RID: 5444
	public enum PortId
	{
		// Token: 0x04006C59 RID: 27737
		InputOne,
		// Token: 0x04006C5A RID: 27738
		InputTwo,
		// Token: 0x04006C5B RID: 27739
		InputThree,
		// Token: 0x04006C5C RID: 27740
		InputFour,
		// Token: 0x04006C5D RID: 27741
		OutputOne,
		// Token: 0x04006C5E RID: 27742
		OutputTwo,
		// Token: 0x04006C5F RID: 27743
		OutputThree,
		// Token: 0x04006C60 RID: 27744
		OutputFour,
		// Token: 0x04006C61 RID: 27745
		ControlOne,
		// Token: 0x04006C62 RID: 27746
		ControlTwo
	}

	// Token: 0x02001545 RID: 5445
	public enum Op
	{
		// Token: 0x04006C64 RID: 27748
		And,
		// Token: 0x04006C65 RID: 27749
		Or,
		// Token: 0x04006C66 RID: 27750
		Not,
		// Token: 0x04006C67 RID: 27751
		Xor,
		// Token: 0x04006C68 RID: 27752
		CustomSingle,
		// Token: 0x04006C69 RID: 27753
		Multiplexer,
		// Token: 0x04006C6A RID: 27754
		Demultiplexer
	}
}

using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000264 RID: 612
public class LogicGateFilterConfig : LogicGateBaseConfig
{
	// Token: 0x06000CA2 RID: 3234 RVA: 0x00049581 File Offset: 0x00047781
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x00049584 File Offset: 0x00047784
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

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00049598 File Offset: 0x00047798
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

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000495AE File Offset: 0x000477AE
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x000495B4 File Offset: 0x000477B4
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x00049601 File Offset: 0x00047801
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateFILTER", "logic_filter_kanim", 2, 1);
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x00049618 File Offset: 0x00047818
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateFilter logicGateFilter = go.AddComponent<LogicGateFilter>();
		logicGateFilter.op = this.GetLogicOp();
		logicGateFilter.inputPortOffsets = this.InputPortOffsets;
		logicGateFilter.outputPortOffsets = this.OutputPortOffsets;
		logicGateFilter.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateFilter>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000804 RID: 2052
	public const string ID = "LogicGateFILTER";
}

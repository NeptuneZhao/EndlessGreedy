using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000263 RID: 611
public class LogicGateBufferConfig : LogicGateBaseConfig
{
	// Token: 0x06000C99 RID: 3225 RVA: 0x00049465 File Offset: 0x00047665
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00049468 File Offset: 0x00047668
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

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0004947C File Offset: 0x0004767C
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

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00049492 File Offset: 0x00047692
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00049498 File Offset: 0x00047698
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x000494E5 File Offset: 0x000476E5
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateBUFFER", "logic_buffer_kanim", 2, 1);
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x000494FC File Offset: 0x000476FC
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateBuffer logicGateBuffer = go.AddComponent<LogicGateBuffer>();
		logicGateBuffer.op = this.GetLogicOp();
		logicGateBuffer.inputPortOffsets = this.InputPortOffsets;
		logicGateBuffer.outputPortOffsets = this.OutputPortOffsets;
		logicGateBuffer.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateBuffer>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000803 RID: 2051
	public const string ID = "LogicGateBUFFER";
}

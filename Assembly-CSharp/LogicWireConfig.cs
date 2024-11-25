using System;
using TUNING;
using UnityEngine;

// Token: 0x0200027C RID: 636
public class LogicWireConfig : BaseLogicWireConfig
{
	// Token: 0x06000D26 RID: 3366 RVA: 0x0004B5D0 File Offset: 0x000497D0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicWire";
		string anim = "logic_wires_kanim";
		float construction_time = 3f;
		float[] tier_TINY = BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier_TINY, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x0004B603 File Offset: 0x00049803
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.OneBit, go);
	}

	// Token: 0x04000821 RID: 2081
	public const string ID = "LogicWire";
}

using System;
using TUNING;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class LogicRibbonConfig : BaseLogicWireConfig
{
	// Token: 0x06000CFE RID: 3326 RVA: 0x0004AA34 File Offset: 0x00048C34
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicRibbon";
		string anim = "logic_ribbon_kanim";
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x0004AA67 File Offset: 0x00048C67
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.FourBit, go);
	}

	// Token: 0x04000816 RID: 2070
	public const string ID = "LogicRibbon";
}

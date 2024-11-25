using System;
using TUNING;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class WireConfig : BaseWireConfig
{
	// Token: 0x06001591 RID: 5521 RVA: 0x000765DC File Offset: 0x000747DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Wire";
		string anim = "utilities_electric_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x00076614 File Offset: 0x00074814
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
	}

	// Token: 0x04000C33 RID: 3123
	public const string ID = "Wire";
}

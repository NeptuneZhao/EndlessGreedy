using System;
using TUNING;
using UnityEngine;

// Token: 0x020003FF RID: 1023
public class WireHighWattageConfig : BaseWireConfig
{
	// Token: 0x06001594 RID: 5524 RVA: 0x00076628 File Offset: 0x00074828
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighWattageWire";
		string anim = "utilities_electric_insulated_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER5, none);
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		return buildingDef;
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x00076667 File Offset: 0x00074867
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max20000, go);
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x00076671 File Offset: 0x00074871
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
	}

	// Token: 0x04000C34 RID: 3124
	public const string ID = "HighWattageWire";
}

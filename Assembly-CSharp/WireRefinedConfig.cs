using System;
using TUNING;
using UnityEngine;

// Token: 0x02000402 RID: 1026
public class WireRefinedConfig : BaseWireConfig
{
	// Token: 0x060015A1 RID: 5537 RVA: 0x000767AC File Offset: 0x000749AC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefined";
		string anim = "utilities_electric_conduct_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.NONE, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		return buildingDef;
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x000767EF File Offset: 0x000749EF
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max2000, go);
	}

	// Token: 0x04000C37 RID: 3127
	public const string ID = "WireRefined";
}

using System;
using TUNING;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class WireRefinedHighWattageConfig : BaseWireConfig
{
	// Token: 0x060015A4 RID: 5540 RVA: 0x00076804 File Offset: 0x00074A04
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefinedHighWattage";
		string anim = "utilities_electric_conduct_hiwatt_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER3, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		return buildingDef;
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x0007684E File Offset: 0x00074A4E
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max50000, go);
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x00076858 File Offset: 0x00074A58
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x04000C38 RID: 3128
	public const string ID = "WireRefinedHighWattage";
}

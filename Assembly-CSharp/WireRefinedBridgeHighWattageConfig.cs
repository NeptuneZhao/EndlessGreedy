using System;
using TUNING;
using UnityEngine;

// Token: 0x02000401 RID: 1025
public class WireRefinedBridgeHighWattageConfig : WireBridgeHighWattageConfig
{
	// Token: 0x0600159C RID: 5532 RVA: 0x000766FC File Offset: 0x000748FC
	protected override string GetID()
	{
		return "WireRefinedBridgeHighWattage";
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x00076704 File Offset: 0x00074904
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("heavywatttile_conductive_kanim")
		};
		buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridgeHighWattage");
		return buildingDef;
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x0007676C File Offset: 0x0007496C
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max50000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x0007677C File Offset: 0x0007497C
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x04000C36 RID: 3126
	public new const string ID = "WireRefinedBridgeHighWattage";
}

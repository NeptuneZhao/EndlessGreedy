using System;
using TUNING;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public class WireRefinedBridgeConfig : WireBridgeConfig
{
	// Token: 0x06001598 RID: 5528 RVA: 0x00076682 File Offset: 0x00074882
	protected override string GetID()
	{
		return "WireRefinedBridge";
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x0007668C File Offset: 0x0007488C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("utilityelectricbridgeconductive_kanim")
		};
		buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridge");
		return buildingDef;
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x000766E4 File Offset: 0x000748E4
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max2000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x04000C35 RID: 3125
	public new const string ID = "WireRefinedBridge";
}

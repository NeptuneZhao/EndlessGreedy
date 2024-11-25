using System;
using TUNING;
using UnityEngine;

// Token: 0x02000238 RID: 568
public class LandingBeaconConfig : IBuildingConfig
{
	// Token: 0x06000BBD RID: 3005 RVA: 0x00044FB8 File Offset: 0x000431B8
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x00044FC0 File Offset: 0x000431C0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LandingBeacon";
		int width = 1;
		int height = 3;
		string anim = "landing_beacon_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 398.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		return buildingDef;
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0004505B File Offset: 0x0004325B
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGetDef<LandingBeacon.Def>();
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0004507C File Offset: 0x0004327C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00045084 File Offset: 0x00043284
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0004508C File Offset: 0x0004328C
	public override void DoPostConfigureComplete(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x00045094 File Offset: 0x00043294
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(LandingBeaconConfig.BeaconSkyVisibility);
		};
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x000450D4 File Offset: 0x000432D4
	private static bool BeaconSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "beacon assumes DLC", null);
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != 255)
		{
			int num = (int)ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]).maximumBounds.y;
			int num2 = cell;
			while (Grid.CellRow(num2) <= num)
			{
				if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
				{
					return false;
				}
				num2 = Grid.CellAbove(num2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x040007B6 RID: 1974
	public const string ID = "LandingBeacon";

	// Token: 0x040007B7 RID: 1975
	public const int LANDING_ACCURACY = 3;
}

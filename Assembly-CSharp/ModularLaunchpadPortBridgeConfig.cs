using System;
using TUNING;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class ModularLaunchpadPortBridgeConfig : IBuildingConfig
{
	// Token: 0x06000FE1 RID: 4065 RVA: 0x0005A920 File Offset: 0x00058B20
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x0005A928 File Offset: 0x00058B28
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ModularLaunchpadPortBridge";
		int width = 1;
		int height = 2;
		string anim = "rocket_loader_extension_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "medium";
		return buildingDef;
	}

	// Token: 0x06000FE3 RID: 4067 RVA: 0x0005A9B4 File Offset: 0x00058BB4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.ModularConduitPort, false);
		component.AddTag(GameTags.NotRocketInteriorBuilding, false);
		component.AddTag(BaseModularLaunchpadPortConfig.LinkTag, false);
		ChainedBuilding.Def def = go.AddOrGetDef<ChainedBuilding.Def>();
		def.headBuildingTag = "LaunchPad".ToTag();
		def.linkBuildingTag = BaseModularLaunchpadPortConfig.LinkTag;
		def.objectLayer = ObjectLayer.Building;
		go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
	}

	// Token: 0x06000FE4 RID: 4068 RVA: 0x0005AA30 File Offset: 0x00058C30
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040009AA RID: 2474
	public const string ID = "ModularLaunchpadPortBridge";
}

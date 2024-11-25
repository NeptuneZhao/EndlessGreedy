using System;
using TUNING;
using UnityEngine;

// Token: 0x02000242 RID: 578
public class LiquidCargoBaySmallConfig : IBuildingConfig
{
	// Token: 0x06000BF9 RID: 3065 RVA: 0x000465B4 File Offset: 0x000447B4
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x000465BC File Offset: 0x000447BC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidCargoBaySmall";
		int width = 3;
		int height = 3;
		string anim = "rocket_storage_liquid_small_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		return buildingDef;
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0004665C File Offset: 0x0004485C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
		};
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x000466C0 File Offset: 0x000448C0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.LIQUIDS, CargoBay.CargoType.Liquids);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR_PLUS, 0f, 0f);
	}

	// Token: 0x040007CB RID: 1995
	public const string ID = "LiquidCargoBaySmall";

	// Token: 0x040007CC RID: 1996
	public float CAPACITY = 900f * ROCKETRY.CARGO_CAPACITY_SCALE;
}

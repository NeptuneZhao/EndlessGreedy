using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000320 RID: 800
public class OxidizerTankLiquidConfig : IBuildingConfig
{
	// Token: 0x060010BF RID: 4287 RVA: 0x0005E59E File Offset: 0x0005C79E
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x0005E5A8 File Offset: 0x0005C7A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxidizerTankLiquid";
		int width = 5;
		int height = 5;
		string anim = "rocket_oxidizer_tank_liquid_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] fuel_TANK_DRY_MASS = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_DRY_MASS;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, fuel_TANK_DRY_MASS, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "grounded";
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.UtilityInputOffset = new CellOffset(2, 3);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x0005E678 File Offset: 0x0005C878
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x0005E6DC File Offset: 0x0005C8DC
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 2700f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		OxidizerTank oxidizerTank = go.AddOrGet<OxidizerTank>();
		oxidizerTank.consumeOnLand = !DlcManager.FeatureClusterSpaceEnabled();
		oxidizerTank.storage = storage;
		oxidizerTank.maxFillMass = 2700f;
		oxidizerTank.supportsMultipleOxidizers = false;
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<DropToUserCapacity>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.LiquidOxygen).tag;
		conduitConsumer.capacityKG = storage.capacityKg;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_oxidizer_tank_liquid_bg_kanim", false);
	}

	// Token: 0x04000A3E RID: 2622
	public const string ID = "OxidizerTankLiquid";

	// Token: 0x04000A3F RID: 2623
	public const float FuelCapacity = 2700f;
}

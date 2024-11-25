using System;
using TUNING;
using UnityEngine;

// Token: 0x020006B3 RID: 1715
public class DesalinatorConfig : IBuildingConfig
{
	// Token: 0x06002B34 RID: 11060 RVA: 0x000F29C4 File Offset: 0x000F0BC4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Desalinator";
		int width = 4;
		int height = 3;
		string anim = "desalinator_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 8f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06002B35 RID: 11061 RVA: 0x000F2A80 File Offset: 0x000F0C80
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		go.AddOrGet<Desalinator>().maxSalt = 945f;
		ElementConverter elementConverter = go.AddComponent<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("SaltWater"), 5f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(4.65f, SimHashes.Water, 0f, false, true, 0f, 0.5f, 0.75f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.35f, SimHashes.Salt, 0f, false, true, 0f, 0.5f, 0.25f, byte.MaxValue, 0, true)
		};
		ElementConverter elementConverter2 = go.AddComponent<ElementConverter>();
		elementConverter2.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Brine"), 5f, true)
		};
		elementConverter2.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(3.5f, SimHashes.Water, 0f, false, true, 0f, 0.5f, 0.75f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(1.5f, SimHashes.Salt, 0f, false, true, 0f, 0.5f, 0.25f, byte.MaxValue, 0, true)
		};
		DesalinatorWorkableEmpty desalinatorWorkableEmpty = go.AddOrGet<DesalinatorWorkableEmpty>();
		desalinatorWorkableEmpty.workTime = 90f;
		desalinatorWorkableEmpty.workLayer = Grid.SceneLayer.BuildingFront;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 20f;
		conduitConsumer.capacityTag = GameTags.AnyWater;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.SaltWater,
			SimHashes.Brine
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x000F2C8F File Offset: 0x000F0E8F
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x000F2C99 File Offset: 0x000F0E99
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x000F2CA2 File Offset: 0x000F0EA2
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
	}

	// Token: 0x040018CC RID: 6348
	public const string ID = "Desalinator";

	// Token: 0x040018CD RID: 6349
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x040018CE RID: 6350
	private const float INPUT_RATE = 5f;

	// Token: 0x040018CF RID: 6351
	private const float SALT_WATER_TO_SALT_OUTPUT_RATE = 0.35f;

	// Token: 0x040018D0 RID: 6352
	private const float SALT_WATER_TO_CLEAN_WATER_OUTPUT_RATE = 4.65f;

	// Token: 0x040018D1 RID: 6353
	private const float BRINE_TO_SALT_OUTPUT_RATE = 1.5f;

	// Token: 0x040018D2 RID: 6354
	private const float BRINE_TO_CLEAN_WATER_OUTPUT_RATE = 3.5f;
}

using System;
using TUNING;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class DevLifeSupportConfig : IBuildingConfig
{
	// Token: 0x06000200 RID: 512 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevLifeSupport";
		int width = 1;
		int height = 1;
		string anim = "dev_life_support_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000E24C File Offset: 0x0000C44C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		CellOffset cellOffset = new CellOffset(0, 1);
		ElementEmitter elementEmitter = go.AddOrGet<ElementEmitter>();
		elementEmitter.outputElement = new ElementConverter.OutputElement(50.000004f, SimHashes.Oxygen, 303.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true);
		elementEmitter.emissionFrequency = 1f;
		elementEmitter.maxPressure = 1.5f;
		PassiveElementConsumer passiveElementConsumer = go.AddOrGet<PassiveElementConsumer>();
		passiveElementConsumer.elementToConsume = SimHashes.CarbonDioxide;
		passiveElementConsumer.consumptionRate = 50.000004f;
		passiveElementConsumer.capacityKG = 50.000004f;
		passiveElementConsumer.consumptionRadius = 10;
		passiveElementConsumer.showInStatusPanel = true;
		passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
		passiveElementConsumer.isRequired = false;
		passiveElementConsumer.storeOnConsume = false;
		passiveElementConsumer.showDescriptor = false;
		passiveElementConsumer.ignoreActiveChanged = true;
		go.AddOrGet<DevLifeSupport>();
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000E353 File Offset: 0x0000C553
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000144 RID: 324
	public const string ID = "DevLifeSupport";

	// Token: 0x04000145 RID: 325
	private const float OXYGEN_GENERATION_RATE = 50.000004f;

	// Token: 0x04000146 RID: 326
	private const float OXYGEN_TEMPERATURE = 303.15f;

	// Token: 0x04000147 RID: 327
	private const float OXYGEN_MAX_PRESSURE = 1.5f;

	// Token: 0x04000148 RID: 328
	private const float CO2_CONSUMPTION_RATE = 50.000004f;
}

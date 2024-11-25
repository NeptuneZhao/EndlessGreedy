using System;
using TUNING;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class CompostConfig : IBuildingConfig
{
	// Token: 0x06000158 RID: 344 RVA: 0x000099E4 File Offset: 0x00007BE4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Compost";
		int width = 2;
		int height = 2;
		string anim = "compost_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, none, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		SoundEventVolumeCache.instance.AddVolume("anim_interacts_compost_kanim", "Compost_shovel_in", NOISE_POLLUTION.NOISY.TIER2);
		SoundEventVolumeCache.instance.AddVolume("anim_interacts_compost_kanim", "Compost_shovel_out", NOISE_POLLUTION.NOISY.TIER2);
		return buildingDef;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00009AA0 File Offset: 0x00007CA0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 2000f;
		go.AddOrGet<Compost>().simulatedInternalTemperature = 348.15f;
		CompostWorkable compostWorkable = go.AddOrGet<CompostWorkable>();
		compostWorkable.workTime = 20f;
		compostWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_compost_kanim")
		};
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(CompostConfig.COMPOST_TAG, 0.1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.1f, SimHashes.Dirt, 348.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		ElementDropper elementDropper = go.AddComponent<ElementDropper>();
		elementDropper.emitMass = 10f;
		elementDropper.emitTag = SimHashes.Dirt.CreateTag();
		elementDropper.emitOffset = new Vector3(0.5f, 1f, 0f);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = CompostConfig.COMPOST_TAG;
		manualDeliveryKG.capacity = 300f;
		manualDeliveryKG.refillMass = 60f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00009C04 File Offset: 0x00007E04
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040000D3 RID: 211
	public const string ID = "Compost";

	// Token: 0x040000D4 RID: 212
	public static readonly Tag COMPOST_TAG = GameTags.Compostable;

	// Token: 0x040000D5 RID: 213
	public const float SAND_INPUT_PER_SECOND = 0.1f;

	// Token: 0x040000D6 RID: 214
	public const float FERTILIZER_OUTPUT_PER_SECOND = 0.1f;

	// Token: 0x040000D7 RID: 215
	public const float FERTILIZER_OUTPUT_TEMP = 348.15f;

	// Token: 0x040000D8 RID: 216
	public const float INPUT_CAPACITY = 300f;

	// Token: 0x040000D9 RID: 217
	private const SimHashes OUTPUT_ELEMENT = SimHashes.Dirt;
}

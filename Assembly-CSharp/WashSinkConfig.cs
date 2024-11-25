using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
public class WashSinkConfig : IBuildingConfig
{
	// Token: 0x0600156E RID: 5486 RVA: 0x00075A6C File Offset: 0x00073C6C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WashSink";
		int width = 2;
		int height = 3;
		string anim = "wash_sink_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
		return buildingDef;
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x00075AF0 File Offset: 0x00073CF0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.AdvancedWashStation, false);
		HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
		handSanitizer.massConsumedPerUse = 5f;
		handSanitizer.consumedElement = SimHashes.Water;
		handSanitizer.outputElement = SimHashes.DirtyWater;
		handSanitizer.diseaseRemovalCount = WashSinkConfig.DISEASE_REMOVAL_COUNT;
		handSanitizer.maxUses = 2;
		handSanitizer.dirtyMeterOffset = Meter.Offset.Behind;
		go.AddOrGet<DirectionControl>();
		HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_washbasin_kanim")
		};
		work.overrideAnims = overrideAnims;
		work.workTime = 5f;
		work.trackUses = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.doDiseaseTransfer = false;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		go.GetComponent<KPrefabID>().prefabInitFn += this.OnInit;
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x00075C40 File Offset: 0x00073E40
	private void OnInit(GameObject go)
	{
		HandSanitizer.Work component = go.GetComponent<HandSanitizer.Work>();
		KAnimFile[] value = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_washbasin_kanim")
		};
		component.workerTypeOverrideAnims.Add(MinionConfig.ID, value);
		component.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_interacts_wash_sink_kanim")
		});
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x00075CB0 File Offset: 0x00073EB0
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000C22 RID: 3106
	public const string ID = "WashSink";

	// Token: 0x04000C23 RID: 3107
	public static readonly int DISEASE_REMOVAL_COUNT = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE + 20000;

	// Token: 0x04000C24 RID: 3108
	public const float WATER_PER_USE = 5f;

	// Token: 0x04000C25 RID: 3109
	public const int USES_PER_FLUSH = 2;

	// Token: 0x04000C26 RID: 3110
	public const float WORK_TIME = 5f;
}

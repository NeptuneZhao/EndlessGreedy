using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F3 RID: 1011
public class WallToiletConfig : IBuildingConfig
{
	// Token: 0x06001548 RID: 5448 RVA: 0x00074D64 File Offset: 0x00072F64
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x00074D6C File Offset: 0x00072F6C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WallToilet";
		int width = 1;
		int height = 3;
		string anim = "toilet_wall_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.WallFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
		buildingDef.UtilityOutputOffset = new CellOffset(-2, 0);
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x00074E24 File Offset: 0x00073024
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ToiletType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.FlushToiletType, false);
		FlushToilet flushToilet = go.AddOrGet<FlushToilet>();
		flushToilet.massConsumedPerUse = 2.5f;
		flushToilet.massEmittedPerUse = 2.5f + DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE;
		flushToilet.newPeeTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
		flushToilet.diseaseId = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
		flushToilet.diseasePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE;
		flushToilet.diseaseOnDupePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE / 5;
		flushToilet.requireOutput = false;
		flushToilet.meterOffset = Meter.Offset.Infront;
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_toilet_wall_kanim")
		};
		ToiletWorkableUse toiletWorkableUse = go.AddOrGet<ToiletWorkableUse>();
		toiletWorkableUse.overrideAnims = overrideAnims;
		toiletWorkableUse.workLayer = Grid.SceneLayer.BuildingUse;
		toiletWorkableUse.resetProgressOnStop = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 2.5f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		AutoStorageDropper.Def def = go.AddOrGetDef<AutoStorageDropper.Def>();
		def.dropOffset = new CellOffset(-2, 0);
		def.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		def.invertElementFilterInitialValue = true;
		def.blockedBySubstantialLiquid = true;
		def.fxOffset = new Vector3(0.5f, 0f, 0f);
		def.leftFx = new AutoStorageDropper.DropperFxConfig
		{
			animFile = "liquidleak_kanim",
			animName = "side",
			flipX = true,
			layer = Grid.SceneLayer.BuildingBack
		};
		def.rightFx = new AutoStorageDropper.DropperFxConfig
		{
			animFile = "liquidleak_kanim",
			animName = "side",
			flipX = false,
			layer = Grid.SceneLayer.BuildingBack
		};
		def.delay = 0f;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 12.5f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
		ownable.canBePublic = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00075059 File Offset: 0x00073259
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000C11 RID: 3089
	private const float WATER_USAGE = 2.5f;

	// Token: 0x04000C12 RID: 3090
	public const string ID = "WallToilet";
}

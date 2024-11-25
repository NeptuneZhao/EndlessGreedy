using System;
using TUNING;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class OuthouseConfig : IBuildingConfig
{
	// Token: 0x060010AC RID: 4268 RVA: 0x0005DC64 File Offset: 0x0005BE64
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Outhouse";
		int width = 2;
		int height = 3;
		string anim = "outhouse_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS_OR_WOOD = MATERIALS.RAW_MINERALS_OR_WOOD;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS_OR_WOOD, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER4, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
		buildingDef.AudioCategory = "Metal";
		SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_open", NOISE_POLLUTION.NOISY.TIER1);
		SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_close", NOISE_POLLUTION.NOISY.TIER1);
		return buildingDef;
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x0005DD10 File Offset: 0x0005BF10
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ToiletType, false);
		Toilet toilet = go.AddOrGet<Toilet>();
		toilet.maxFlushes = 15;
		toilet.dirtUsedPerFlush = 13f;
		toilet.solidWastePerUse = new Toilet.SpawnInfo(SimHashes.ToxicSand, DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE, 0f);
		toilet.solidWasteTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
		toilet.diseaseId = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
		toilet.diseasePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE;
		toilet.diseaseOnDupePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE;
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_outhouse_kanim")
		};
		ToiletWorkableUse toiletWorkableUse = go.AddOrGet<ToiletWorkableUse>();
		toiletWorkableUse.overrideAnims = overrideAnims;
		toiletWorkableUse.workLayer = Grid.SceneLayer.BuildingFront;
		ToiletWorkableClean toiletWorkableClean = go.AddOrGet<ToiletWorkableClean>();
		toiletWorkableClean.workTime = 90f;
		toiletWorkableClean.overrideAnims = overrideAnims;
		toiletWorkableClean.workLayer = Grid.SceneLayer.BuildingFront;
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("Dirt");
		manualDeliveryKG.capacity = 200f;
		manualDeliveryKG.refillMass = 0.01f;
		manualDeliveryKG.MinimumMass = 200f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		manualDeliveryKG.FillToCapacity = true;
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
		ownable.canBePublic = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0005DEC7 File Offset: 0x0005C0C7
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A34 RID: 2612
	public const string ID = "Outhouse";

	// Token: 0x04000A35 RID: 2613
	private const int USES_PER_REFILL = 15;

	// Token: 0x04000A36 RID: 2614
	private const float DIRT_PER_REFILL = 200f;

	// Token: 0x04000A37 RID: 2615
	private const float DIRT_PER_USE = 13f;
}

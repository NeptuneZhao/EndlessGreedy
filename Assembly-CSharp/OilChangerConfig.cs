using System;
using TUNING;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class OilChangerConfig : IBuildingConfig
{
	// Token: 0x06001066 RID: 4198 RVA: 0x0005CC25 File Offset: 0x0005AE25
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x0005CC2C File Offset: 0x0005AE2C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OilChanger";
		int width = 3;
		int height = 3;
		string anim = "oilchange_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		return buildingDef;
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x0005CCC0 File Offset: 0x0005AEC0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BionicUpkeepType, false);
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = this.OIL_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		OilChangerWorkableUse oilChangerWorkableUse = go.AddOrGet<OilChangerWorkableUse>();
		oilChangerWorkableUse.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_oilchange_kanim")
		};
		oilChangerWorkableUse.resetProgressOnStop = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = GameTags.LubricatingOil;
		conduitConsumer.capacityKG = this.OIL_CAPACITY;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		go.AddOrGetDef<OilChanger.Def>();
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x0005CD5E File Offset: 0x0005AF5E
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A16 RID: 2582
	public const string ID = "OilChanger";

	// Token: 0x04000A17 RID: 2583
	public float OIL_CAPACITY = 400f;
}

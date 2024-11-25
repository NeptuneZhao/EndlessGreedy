using System;
using TUNING;
using UnityEngine;

// Token: 0x02000253 RID: 595
public class LiquidPumpingStationConfig : IBuildingConfig
{
	// Token: 0x06000C49 RID: 3145 RVA: 0x00048038 File Offset: 0x00046238
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidPumpingStation";
		int width = 2;
		int height = 4;
		string anim = "waterpump_kanim";
		int hitpoints = 100;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.DefaultAnimState = "on";
		buildingDef.ShowInBuildMenu = true;
		return buildingDef;
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x000480D0 File Offset: 0x000462D0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<LiquidPumpingStation>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_waterpump_kanim")
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.allowItemRemoval = true;
		storage.showDescriptor = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		go.AddTag(GameTags.CorrosionProof);
		go.AddTag(GameTags.LiquidSource);
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00048150 File Offset: 0x00046350
	private static void AddGuide(GameObject go, bool occupy_tiles)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = go.transform;
		gameObject.transform.SetLocalPosition(Vector3.zero);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.Offset = go.GetComponent<Building>().Def.GetVisualizerOffset();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(new HashedString("waterpump_kanim"))
		};
		kbatchedAnimController.initialAnim = "place_guide";
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		kbatchedAnimController.isMovable = true;
		PumpingStationGuide pumpingStationGuide = gameObject.AddComponent<PumpingStationGuide>();
		pumpingStationGuide.parent = go;
		pumpingStationGuide.occupyTiles = occupy_tiles;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x000481EC File Offset: 0x000463EC
	public override void DoPostConfigureComplete(GameObject go)
	{
		LiquidPumpingStationConfig.AddGuide(go.GetComponent<Building>().Def.BuildingPreview, false);
		LiquidPumpingStationConfig.AddGuide(go.GetComponent<Building>().Def.BuildingUnderConstruction, false);
		go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(1, 0)
		};
	}

	// Token: 0x040007EB RID: 2027
	public const string ID = "LiquidPumpingStation";

	// Token: 0x040007EC RID: 2028
	public const int TAIL_LENGTH = 4;
}

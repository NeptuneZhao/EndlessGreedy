using System;
using TUNING;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class GunkEmptierConfig : IBuildingConfig
{
	// Token: 0x06000AB4 RID: 2740 RVA: 0x0003FFF3 File Offset: 0x0003E1F3
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x0003FFFC File Offset: 0x0003E1FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GunkEmptier";
		int width = 3;
		int height = 3;
		string anim = "gunkdump_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(-1, 0);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		return buildingDef;
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00040090 File Offset: 0x0003E290
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BionicUpkeepType, false);
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = GunkEmptierConfig.STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<GunkEmptierWorkable>();
		go.AddOrGetDef<GunkEmptier.Def>();
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.LiquidGunk
		};
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x000400F7 File Offset: 0x0003E2F7
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000711 RID: 1809
	public const string ID = "GunkEmptier";

	// Token: 0x04000712 RID: 1810
	private static float STORAGE_CAPACITY = GunkMonitor.GUNK_CAPACITY * 1.5f;
}

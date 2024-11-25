using System;
using TUNING;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class ClusterTelescopeEnclosedConfig : IBuildingConfig
{
	// Token: 0x0600013D RID: 317 RVA: 0x000091F7 File Offset: 0x000073F7
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00009200 File Offset: 0x00007400
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClusterTelescopeEnclosed";
		int width = 4;
		int height = 6;
		string anim = "telescope_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x0600013F RID: 319 RVA: 0x000092A4 File Offset: 0x000074A4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		go.AddOrGetDef<PoweredController.Def>();
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.analyzeClusterRadius = 4;
		def.workableOverrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_kanim")
		};
		def.skyVisibilityInfo = ClusterTelescopeEnclosedConfig.SKY_VISIBILITY_INFO;
		def.providesOxygen = true;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000937C File Offset: 0x0000757C
	public override void DoPostConfigureComplete(GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00009384 File Offset: 0x00007584
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000938C File Offset: 0x0000758C
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00009394 File Offset: 0x00007594
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 3;
		skyVisibilityVisualizer.TwoWideOrgin = true;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 5;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040000C4 RID: 196
	public const string ID = "ClusterTelescopeEnclosed";

	// Token: 0x040000C5 RID: 197
	public const int SCAN_RADIUS = 4;

	// Token: 0x040000C6 RID: 198
	public const int VERTICAL_SCAN_OFFSET = 3;

	// Token: 0x040000C7 RID: 199
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);
}

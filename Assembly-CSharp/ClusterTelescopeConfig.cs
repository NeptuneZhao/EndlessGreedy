using System;
using TUNING;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class ClusterTelescopeConfig : IBuildingConfig
{
	// Token: 0x06000134 RID: 308 RVA: 0x00009073 File Offset: 0x00007273
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000907C File Offset: 0x0000727C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClusterTelescope";
		int width = 3;
		int height = 3;
		string anim = "telescope_low_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000910C File Offset: 0x0000730C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.workableOverrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_low_kanim")
		};
		def.skyVisibilityInfo = ClusterTelescopeConfig.SKY_VISIBILITY_INFO;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00009191 File Offset: 0x00007391
	public override void DoPostConfigureComplete(GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00009199 File Offset: 0x00007399
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x000091A1 File Offset: 0x000073A1
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x0600013A RID: 314 RVA: 0x000091A9 File Offset: 0x000073A9
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 1;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 4;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040000C0 RID: 192
	public const string ID = "ClusterTelescope";

	// Token: 0x040000C1 RID: 193
	public const int SCAN_RADIUS = 4;

	// Token: 0x040000C2 RID: 194
	public const int VERTICAL_SCAN_OFFSET = 1;

	// Token: 0x040000C3 RID: 195
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 1), 4, new CellOffset(0, 1), 4, 0);
}

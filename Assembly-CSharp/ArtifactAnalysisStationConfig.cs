using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class ArtifactAnalysisStationConfig : IBuildingConfig
{
	// Token: 0x06000079 RID: 121 RVA: 0x00005118 File Offset: 0x00003318
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00005120 File Offset: 0x00003320
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ArtifactAnalysisStation";
		int width = 4;
		int height = 4;
		string anim = "artifact_analysis_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x000051A4 File Offset: 0x000033A4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<ArtifactAnalysisStation.Def>();
		go.AddOrGet<ArtifactAnalysisStationWorkable>();
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = GameTags.CharmedArtifact;
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.capacity = 1f;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00005231 File Offset: 0x00003431
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000064 RID: 100
	public const string ID = "ArtifactAnalysisStation";

	// Token: 0x04000065 RID: 101
	public const float WORK_TIME = 150f;
}

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class GeneticAnalysisStationConfig : IBuildingConfig
{
	// Token: 0x06000A58 RID: 2648 RVA: 0x0003CE39 File Offset: 0x0003B039
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0003CE40 File Offset: 0x0003B040
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GeneticAnalysisStation";
		int width = 7;
		int height = 2;
		string anim = "genetic_analysisstation_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.Deprecated = !DlcManager.FeaturePlantMutationsEnabled();
		return buildingDef;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0003CED4 File Offset: 0x0003B0D4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<GeneticAnalysisStation.Def>();
		go.AddOrGet<GeneticAnalysisStationWorkable>().finishedSeedDropOffset = new Vector3(-3f, 1.5f, 0f);
		Prioritizable.AddRef(go);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGetDef<PoweredActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = GameTags.UnidentifiedSeed;
		manualDeliveryKG.refillMass = 1.1f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.capacity = 5f;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0003CF91 File Offset: 0x0003B191
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0003CF94 File Offset: 0x0003B194
	public override void ConfigurePost(BuildingDef def)
	{
		List<Tag> list = new List<Tag>();
		foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.CropSeed))
		{
			if (gameObject.GetComponent<MutantPlant>() != null)
			{
				list.Add(gameObject.PrefabID());
			}
		}
		def.BuildingComplete.GetComponent<Storage>().storageFilters = list;
	}

	// Token: 0x040006B1 RID: 1713
	public const string ID = "GeneticAnalysisStation";
}

using System;
using TUNING;
using UnityEngine;

// Token: 0x020003BD RID: 957
public class SolidConduitOutboxConfig : IBuildingConfig
{
	// Token: 0x060013E4 RID: 5092 RVA: 0x0006D9E4 File Offset: 0x0006BBE4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidConduitOutbox";
		int width = 1;
		int height = 2;
		string anim = "conveyorout_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitOutbox");
		return buildingDef;
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x0006DA78 File Offset: 0x0006BC78
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		go.AddOrGet<SolidConduitOutbox>();
		go.AddOrGet<SolidConduitConsumer>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.capacityKg = 100f;
		storage.showInUI = true;
		storage.allowItemRemoval = true;
		go.AddOrGet<SimpleVent>();
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x0006DAB4 File Offset: 0x0006BCB4
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x0006DADC File Offset: 0x0006BCDC
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<Automatable>();
	}

	// Token: 0x04000B5E RID: 2910
	public const string ID = "SolidConduitOutbox";
}

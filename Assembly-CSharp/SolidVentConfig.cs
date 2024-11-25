using System;
using TUNING;
using UnityEngine;

// Token: 0x020003C5 RID: 965
public class SolidVentConfig : IBuildingConfig
{
	// Token: 0x0600140E RID: 5134 RVA: 0x0006E3E8 File Offset: 0x0006C5E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidVent";
		int width = 1;
		int height = 1;
		string anim = "conveyer_dropper_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidVent");
		return buildingDef;
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x0006E494 File Offset: 0x0006C694
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x0006E49D File Offset: 0x0006C69D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x0006E4C5 File Offset: 0x0006C6C5
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<SimpleVent>();
		go.AddOrGet<SolidConduitConsumer>();
		go.AddOrGet<SolidConduitDropper>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.capacityKg = 100f;
		storage.showInUI = true;
	}

	// Token: 0x04000B6B RID: 2923
	public const string ID = "SolidVent";

	// Token: 0x04000B6C RID: 2924
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
}

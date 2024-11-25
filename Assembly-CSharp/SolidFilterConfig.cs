using System;
using TUNING;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class SolidFilterConfig : IBuildingConfig
{
	// Token: 0x060013F8 RID: 5112 RVA: 0x0006DD88 File Offset: 0x0006BF88
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidFilter";
		int width = 3;
		int height = 1;
		string anim = "filter_material_conveyor_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidFilter");
		return buildingDef;
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x0006DE51 File Offset: 0x0006C051
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x0006DE64 File Offset: 0x0006C064
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x0006DE75 File Offset: 0x0006C075
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x0006DE85 File Offset: 0x0006C085
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<ElementFilter>().portInfo = this.secondaryPort;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Solid;
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x0006DEB5 File Offset: 0x0006C0B5
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
	}

	// Token: 0x04000B62 RID: 2914
	public const string ID = "SolidFilter";

	// Token: 0x04000B63 RID: 2915
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

	// Token: 0x04000B64 RID: 2916
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(0, 0));
}

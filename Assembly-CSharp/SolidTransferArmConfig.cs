using System;
using TUNING;
using UnityEngine;

// Token: 0x020003C4 RID: 964
public class SolidTransferArmConfig : IBuildingConfig
{
	// Token: 0x06001407 RID: 5127 RVA: 0x0006E284 File Offset: 0x0006C484
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SolidTransferArm", 3, 1, "conveyor_transferarm_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidTransferArm");
		return buildingDef;
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x0006E32A File Offset: 0x0006C52A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Operational>();
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x0006E33A File Offset: 0x0006C53A
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		SolidTransferArmConfig.AddVisualizer(go, true);
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x0006E343 File Offset: 0x0006C543
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		SolidTransferArmConfig.AddVisualizer(go, false);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x0006E36B File Offset: 0x0006C56B
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<SolidTransferArm>().pickupRange = 4;
		SolidTransferArmConfig.AddVisualizer(go, false);
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x0006E388 File Offset: 0x0006C588
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = new Vector2I(0, 0);
		rangeVisualizer.RangeMin.x = -4;
		rangeVisualizer.RangeMin.y = -4;
		rangeVisualizer.RangeMax.x = 4;
		rangeVisualizer.RangeMax.y = 4;
		rangeVisualizer.BlockingTileVisible = true;
	}

	// Token: 0x04000B69 RID: 2921
	public const string ID = "SolidTransferArm";

	// Token: 0x04000B6A RID: 2922
	private const int RANGE = 4;
}

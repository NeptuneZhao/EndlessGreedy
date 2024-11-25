using System;
using TUNING;
using UnityEngine;

// Token: 0x020003D0 RID: 976
public class StaterpillarLiquidConnectorConfig : IBuildingConfig
{
	// Token: 0x06001465 RID: 5221 RVA: 0x0006FDB1 File Offset: 0x0006DFB1
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x0006FDB8 File Offset: 0x0006DFB8
	public override BuildingDef CreateBuildingDef()
	{
		string id = StaterpillarLiquidConnectorConfig.ID;
		int width = 1;
		int height = 2;
		string anim = "egg_caterpillar_kanim";
		int hitpoints = 1000;
		float construction_time = 10f;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] construction_materials = all_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFoundationRotatable;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.OverheatTemperature = 423.15f;
		buildingDef.PermittedRotations = PermittedRotations.FlipV;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.PlayConstructionSounds = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x0006FE5C File Offset: 0x0006E05C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x0006FE74 File Offset: 0x0006E074
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Storage>();
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		conduitDispenser.isOn = false;
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<KSelectable>().IsSelectable = false;
	}

	// Token: 0x04000BAA RID: 2986
	public static readonly string ID = "StaterpillarLiquidConnector";

	// Token: 0x04000BAB RID: 2987
	private const int WIDTH = 1;

	// Token: 0x04000BAC RID: 2988
	private const int HEIGHT = 2;
}

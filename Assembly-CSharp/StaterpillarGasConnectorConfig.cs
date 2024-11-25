using System;
using TUNING;
using UnityEngine;

// Token: 0x020003CF RID: 975
public class StaterpillarGasConnectorConfig : IBuildingConfig
{
	// Token: 0x0600145F RID: 5215 RVA: 0x0006FC8D File Offset: 0x0006DE8D
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x0006FC94 File Offset: 0x0006DE94
	public override BuildingDef CreateBuildingDef()
	{
		string id = StaterpillarGasConnectorConfig.ID;
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
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.PlayConstructionSounds = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x0006FD38 File Offset: 0x0006DF38
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x0006FD50 File Offset: 0x0006DF50
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Storage>();
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		conduitDispenser.isOn = false;
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<KSelectable>().IsSelectable = false;
	}

	// Token: 0x04000BA7 RID: 2983
	public static readonly string ID = "StaterpillarGasConnector";

	// Token: 0x04000BA8 RID: 2984
	private const int WIDTH = 1;

	// Token: 0x04000BA9 RID: 2985
	private const int HEIGHT = 2;
}

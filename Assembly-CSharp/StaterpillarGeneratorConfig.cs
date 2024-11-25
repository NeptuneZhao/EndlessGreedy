using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020003CE RID: 974
public class StaterpillarGeneratorConfig : IBuildingConfig
{
	// Token: 0x06001457 RID: 5207 RVA: 0x0006FB50 File Offset: 0x0006DD50
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x0006FB58 File Offset: 0x0006DD58
	public override BuildingDef CreateBuildingDef()
	{
		string id = StaterpillarGeneratorConfig.ID;
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
		buildingDef.GeneratorWattageRating = 1600f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 2f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.OverheatTemperature = 423.15f;
		buildingDef.PermittedRotations = PermittedRotations.FlipV;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 1);
		buildingDef.PlayConstructionSounds = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x0006FC29 File Offset: 0x0006DE29
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x0006FC40 File Offset: 0x0006DE40
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x0006FC42 File Offset: 0x0006DE42
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x0006FC44 File Offset: 0x0006DE44
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<StaterpillarGenerator>().powerDistributionOrder = 9;
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.AddOrGet<Modifiers>();
		go.AddOrGet<Effects>();
		go.GetComponent<KSelectable>().IsSelectable = false;
	}

	// Token: 0x04000BA4 RID: 2980
	public static readonly string ID = "StaterpillarGenerator";

	// Token: 0x04000BA5 RID: 2981
	private const int WIDTH = 1;

	// Token: 0x04000BA6 RID: 2982
	private const int HEIGHT = 2;
}

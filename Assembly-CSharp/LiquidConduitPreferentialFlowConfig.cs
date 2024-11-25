using System;
using TUNING;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class LiquidConduitPreferentialFlowConfig : IBuildingConfig
{
	// Token: 0x06000C14 RID: 3092 RVA: 0x00046CF0 File Offset: 0x00044EF0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidConduitPreferentialFlow";
		int width = 2;
		int height = 2;
		string anim = "valveliquid_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, buildingDef.PrefabID);
		return buildingDef;
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00046DB8 File Offset: 0x00044FB8
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00046DC9 File Offset: 0x00044FC9
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00046DD9 File Offset: 0x00044FD9
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00046DEC File Offset: 0x00044FEC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitPreferentialFlow>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00046E1A File Offset: 0x0004501A
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040007D5 RID: 2005
	public const string ID = "LiquidConduitPreferentialFlow";

	// Token: 0x040007D6 RID: 2006
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x040007D7 RID: 2007
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));
}

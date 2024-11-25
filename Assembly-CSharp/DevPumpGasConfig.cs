using System;
using TUNING;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class DevPumpGasConfig : IBuildingConfig
{
	// Token: 0x06000209 RID: 521 RVA: 0x0000E4D4 File Offset: 0x0000C6D4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevPumpGas";
		int width = 2;
		int height = 2;
		string anim = "dev_pump_gas_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityOutputOffset = this.primaryPort.offset;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "DevPumpGas");
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000E581 File Offset: 0x0000C781
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000E594 File Offset: 0x0000C794
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddTag(GameTags.DevBuilding);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<DevPump>().elementState = Filterable.ElementState.Gas;
		go.AddOrGet<Storage>().capacityKg = 20f;
		go.AddTag(GameTags.CorrosionProof);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		go.AddOrGetDef<OperationalController.Def>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x0400014A RID: 330
	public const string ID = "DevPumpGas";

	// Token: 0x0400014B RID: 331
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x0400014C RID: 332
	private ConduitPortInfo primaryPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 1));
}

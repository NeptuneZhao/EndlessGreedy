using System;
using TUNING;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class SpaceHeaterConfig : IBuildingConfig
{
	// Token: 0x06001413 RID: 5139 RVA: 0x0006E4FC File Offset: 0x0006C6FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpaceHeater";
		int width = 2;
		int height = 2;
		string anim = "spaceheater_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.OverheatTemperature = 398.15f;
		return buildingDef;
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x0006E5A0 File Offset: 0x0006C7A0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WarmingStation, false);
		go.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
		SpaceHeater spaceHeater = go.AddOrGet<SpaceHeater>();
		spaceHeater.targetTemperature = 343.15f;
		spaceHeater.produceHeat = true;
		WarmthProvider.Def def = go.AddOrGetDef<WarmthProvider.Def>();
		def.RangeMax = SpaceHeaterConfig.MAX_RANGE;
		def.RangeMin = SpaceHeaterConfig.MIN_RANGE;
		go.AddOrGetDef<ColdImmunityProvider.Def>().range = new CellOffset[][]
		{
			new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(2, 0)
			},
			new CellOffset[]
			{
				new CellOffset(0, 0),
				new CellOffset(1, 0)
			}
		};
		this.AddVisualizer(go);
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x0006E660 File Offset: 0x0006C860
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x0006E669 File Offset: 0x0006C869
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x0006E672 File Offset: 0x0006C872
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x0006E684 File Offset: 0x0006C884
	private void AddVisualizer(GameObject go)
	{
		RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMax = SpaceHeaterConfig.MAX_RANGE;
		rangeVisualizer.RangeMin = SpaceHeaterConfig.MIN_RANGE;
		rangeVisualizer.BlockingTileVisible = false;
		go.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
	}

	// Token: 0x04000B6D RID: 2925
	public const string ID = "SpaceHeater";

	// Token: 0x04000B6E RID: 2926
	public const float MAX_SELF_HEAT = 32f;

	// Token: 0x04000B6F RID: 2927
	public const float MAX_EXHAUST_HEAT = 4f;

	// Token: 0x04000B70 RID: 2928
	public const float MIN_POWER_USAGE = 120f;

	// Token: 0x04000B71 RID: 2929
	public const float MAX_POWER_USAGE = 240f;

	// Token: 0x04000B72 RID: 2930
	public static Vector2I MAX_RANGE = new Vector2I(5, 5);

	// Token: 0x04000B73 RID: 2931
	public static Vector2I MIN_RANGE = new Vector2I(-4, -4);
}

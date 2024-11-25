using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class EggIncubatorConfig : IBuildingConfig
{
	// Token: 0x06000234 RID: 564 RVA: 0x0000F48C File Offset: 0x0000D68C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "EggIncubator";
		int width = 2;
		int height = 3;
		string anim = "incubator_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.OverheatTemperature = 363.15f;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		return buildingDef;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000F520 File Offset: 0x0000D720
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		BuildingTemplates.CreateDefaultStorage(go, false).SetDefaultStoredItemModifiers(EggIncubatorConfig.IncubatorStorage);
		EggIncubator eggIncubator = go.AddOrGet<EggIncubator>();
		eggIncubator.AddDepositTag(GameTags.Egg);
		eggIncubator.SetWorkTime(5f);
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000F554 File Offset: 0x0000D754
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000164 RID: 356
	public const string ID = "EggIncubator";

	// Token: 0x04000165 RID: 357
	public static readonly List<Storage.StoredItemModifier> IncubatorStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Preserve
	};
}

using System;
using TUNING;
using UnityEngine;

// Token: 0x02000021 RID: 33
public abstract class BaseBatteryConfig : IBuildingConfig
{
	// Token: 0x06000092 RID: 146 RVA: 0x000059DC File Offset: 0x00003BDC
	public BuildingDef CreateBuildingDef(string id, int width, int height, int hitpoints, string anim, float construction_time, float[] construction_mass, string[] construction_materials, float melting_point, float exhaust_temperature_active, float self_heat_kilowatts_active, EffectorValues decor, EffectorValues noise)
	{
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, decor, tier, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = exhaust_temperature_active;
		buildingDef.SelfHeatKilowattsWhenActive = self_heat_kilowatts_active;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.UseWhitePowerOutputConnectorColour = true;
		return buildingDef;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00005A49 File Offset: 0x00003C49
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00005A52 File Offset: 0x00003C52
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Battery>().powerSortOrder = 1000;
		go.AddOrGetDef<PoweredActiveController.Def>();
	}
}

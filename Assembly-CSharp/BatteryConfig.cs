using System;
using TUNING;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class BatteryConfig : BaseBatteryConfig
{
	// Token: 0x060000A7 RID: 167 RVA: 0x000060B0 File Offset: 0x000042B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Battery";
		int width = 1;
		int height = 2;
		int hitpoints = 30;
		string anim = "batterysm_kanim";
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0.25f;
		float self_heat_kilowatts_active = 1f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, all_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, BUILDINGS.DECOR.PENALTY.TIER1, none);
		buildingDef.Breakable = true;
		SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_rattle", NOISE_POLLUTION.NOISY.TIER1);
		return buildingDef;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000611B File Offset: 0x0000431B
	public override void DoPostConfigureComplete(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 10000f;
		battery.joulesLostPerSecond = 1.6666666f;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x0000613F File Offset: 0x0000433F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x04000078 RID: 120
	public const string ID = "Battery";
}

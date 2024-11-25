using System;
using TUNING;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class BatteryMediumConfig : BaseBatteryConfig
{
	// Token: 0x060000AB RID: 171 RVA: 0x00006164 File Offset: 0x00004364
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BatteryMedium";
		int width = 2;
		int height = 2;
		int hitpoints = 30;
		string anim = "batterymed_kanim";
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0.25f;
		float self_heat_kilowatts_active = 1f;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef result = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, all_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, BUILDINGS.DECOR.PENALTY.TIER2, tier2);
		SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
		return result;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x000061C8 File Offset: 0x000043C8
	public override void DoPostConfigureComplete(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 40000f;
		battery.joulesLostPerSecond = 3.3333333f;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000061EC File Offset: 0x000043EC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x04000079 RID: 121
	public const string ID = "BatteryMedium";
}

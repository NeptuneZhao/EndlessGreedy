using System;
using TUNING;
using UnityEngine;

// Token: 0x02000398 RID: 920
public class RoleStationConfig : IBuildingConfig
{
	// Token: 0x06001329 RID: 4905 RVA: 0x0006A314 File Offset: 0x00068514
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RoleStation";
		int width = 2;
		int height = 2;
		string anim = "job_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x0006A37E File Offset: 0x0006857E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x0006A392 File Offset: 0x00068592
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000B21 RID: 2849
	public const string ID = "RoleStation";
}

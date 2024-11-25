using System;
using TUNING;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class GravitasLabLightConfig : IBuildingConfig
{
	// Token: 0x06000AAB RID: 2731 RVA: 0x0003FE54 File Offset: 0x0003E054
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasLabLight";
		int width = 1;
		int height = 1;
		string anim = "gravitas_lab_light_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0003FEC1 File Offset: 0x0003E0C1
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0003FECE File Offset: 0x0003E0CE
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400070F RID: 1807
	public const string ID = "GravitasLabLight";
}

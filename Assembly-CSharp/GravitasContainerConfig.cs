using System;
using TUNING;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class GravitasContainerConfig : IBuildingConfig
{
	// Token: 0x06000A88 RID: 2696 RVA: 0x0003F29C File Offset: 0x0003D49C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasContainer";
		int width = 2;
		int height = 2;
		string anim = "gravitas_container_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0003F309 File Offset: 0x0003D509
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		go.AddOrGet<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.Building;
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0003F32C File Offset: 0x0003D52C
	public override void DoPostConfigureComplete(GameObject go)
	{
		PajamaDispenser pajamaDispenser = go.AddComponent<PajamaDispenser>();
		pajamaDispenser.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_container_kanim")
		};
		pajamaDispenser.SetWorkTime(30f);
		go.AddOrGet<Demolishable>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	// Token: 0x04000701 RID: 1793
	public const string ID = "GravitasContainer";

	// Token: 0x04000702 RID: 1794
	private const float WORK_TIME = 1.5f;
}

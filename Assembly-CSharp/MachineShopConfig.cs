using System;
using TUNING;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class MachineShopConfig : IBuildingConfig
{
	// Token: 0x06000D38 RID: 3384 RVA: 0x0004BD80 File Offset: 0x00049F80
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MachineShop";
		int width = 4;
		int height = 2;
		string anim = "machineshop_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x0004BDF5 File Offset: 0x00049FF5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.MachineShopType, false);
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0004BE0F File Offset: 0x0004A00F
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000838 RID: 2104
	public const string ID = "MachineShop";

	// Token: 0x04000839 RID: 2105
	public static readonly Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;

	// Token: 0x0400083A RID: 2106
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x0400083B RID: 2107
	public static readonly string ROLE_PERK = "IncreaseMachinery";
}

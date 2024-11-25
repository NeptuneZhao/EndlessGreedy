using System;
using TUNING;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class CornerMouldingConfig : IBuildingConfig
{
	// Token: 0x060001A1 RID: 417 RVA: 0x0000BA78 File Offset: 0x00009C78
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CornerMoulding";
		int width = 1;
		int height = 1;
		string anim = "corner_tile_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.InCorner;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 3
		}, none, 0.2f);
		buildingDef.DefaultAnimState = "corner";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000BB13 File Offset: 0x00009D13
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000BB26 File Offset: 0x00009D26
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400010D RID: 269
	public const string ID = "CornerMoulding";
}

using System;
using TUNING;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class FlyingCreatureBaitConfig : IBuildingConfig
{
	// Token: 0x06000839 RID: 2105 RVA: 0x000366F4 File Offset: 0x000348F4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FlyingCreatureBait", 1, 2, "airborne_critter_bait_kanim", 10, 10f, new float[]
		{
			50f,
			10f
		}, new string[]
		{
			"Metal",
			"FlyingCritterEdible"
		}, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Deprecated = true;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00036773 File Offset: 0x00034973
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<CreatureBait>();
		go.AddTag(GameTags.OneTimeUseLure);
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00036787 File Offset: 0x00034987
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00036789 File Offset: 0x00034989
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0003678C File Offset: 0x0003498C
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<SymbolOverrideController>().applySymbolOverridesEveryFrame = true;
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		Prioritizable.AddRef(go);
	}

	// Token: 0x040005C6 RID: 1478
	public const string ID = "FlyingCreatureBait";
}

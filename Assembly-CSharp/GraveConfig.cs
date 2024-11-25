using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class GraveConfig : IBuildingConfig
{
	// Token: 0x06000A83 RID: 2691 RVA: 0x0003F150 File Offset: 0x0003D350
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Grave";
		int width = 1;
		int height = 2;
		string anim = "gravestone_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		return buildingDef;
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0003F1BC File Offset: 0x0003D3BC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GraveConfig.STORAGE_OVERRIDE_ANIM_FILES = new KAnimFile[]
		{
			Assets.GetAnim("anim_bury_dupe_kanim")
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(GraveConfig.StorageModifiers);
		storage.overrideAnims = GraveConfig.STORAGE_OVERRIDE_ANIM_FILES;
		storage.workAnims = GraveConfig.STORAGE_WORK_ANIMS;
		storage.workingPstComplete = new HashedString[]
		{
			GraveConfig.STORAGE_PST_ANIM
		};
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		go.AddOrGet<Grave>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0003F24F File Offset: 0x0003D44F
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040006FB RID: 1787
	public const string ID = "Grave";

	// Token: 0x040006FC RID: 1788
	public const string AnimFile = "gravestone_kanim";

	// Token: 0x040006FD RID: 1789
	private static KAnimFile[] STORAGE_OVERRIDE_ANIM_FILES;

	// Token: 0x040006FE RID: 1790
	private static readonly HashedString[] STORAGE_WORK_ANIMS = new HashedString[]
	{
		"working_pre"
	};

	// Token: 0x040006FF RID: 1791
	private static readonly HashedString STORAGE_PST_ANIM = HashedString.Invalid;

	// Token: 0x04000700 RID: 1792
	private static readonly List<Storage.StoredItemModifier> StorageModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};
}

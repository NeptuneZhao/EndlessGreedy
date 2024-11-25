using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020003C8 RID: 968
public class SpecialCargoBayClusterConfig : IBuildingConfig
{
	// Token: 0x0600141F RID: 5151 RVA: 0x0006E8B6 File Offset: 0x0006CAB6
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x0006E8C0 File Offset: 0x0006CAC0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpecialCargoBayCluster";
		int width = 3;
		int height = 1;
		string anim = "rocket_storage_live_small_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x0006E968 File Offset: 0x0006CB68
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 1), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x0006E9CC File Offset: 0x0006CBCC
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(SpecialCargoBayClusterConfig.StoredCrittersModifiers);
		storage.showCapacityStatusItem = false;
		storage.showInUI = false;
		storage.storageFilters = new List<Tag>
		{
			GameTags.BagableCreature
		};
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.SetDefaultStoredItemModifiers(SpecialCargoBayClusterConfig.StoredLootModifiers);
		storage2.showCapacityStatusItem = false;
		storage2.showInUI = false;
		storage2.allowSettingOnlyFetchMarkedItems = false;
		storage2.allowItemRemoval = false;
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGetDef<SpecialCargoBayCluster.Def>();
		SpecialCargoBayClusterReceptacle specialCargoBayClusterReceptacle = go.AddOrGet<SpecialCargoBayClusterReceptacle>();
		specialCargoBayClusterReceptacle.AddDepositTag(GameTags.BagableCreature);
		specialCargoBayClusterReceptacle.AddAdditionalCriteria((GameObject obj) => obj.HasTag(GameTags.Creatures.Deliverable));
		specialCargoBayClusterReceptacle.sideProductStorage = storage2;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.INSIGNIFICANT, 0f, 0f);
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x04000B80 RID: 2944
	public const string ID = "SpecialCargoBayCluster";

	// Token: 0x04000B81 RID: 2945
	private static readonly List<Storage.StoredItemModifier> StoredCrittersModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Insulate
	};

	// Token: 0x04000B82 RID: 2946
	private static readonly List<Storage.StoredItemModifier> StoredLootModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};
}

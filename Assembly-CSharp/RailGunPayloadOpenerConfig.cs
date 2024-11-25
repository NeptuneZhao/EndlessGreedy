using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

// Token: 0x02000377 RID: 887
public class RailGunPayloadOpenerConfig : IBuildingConfig
{
	// Token: 0x06001262 RID: 4706 RVA: 0x00064C34 File Offset: 0x00062E34
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x00064C3C File Offset: 0x00062E3C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RailGunPayloadOpener";
		int width = 3;
		int height = 3;
		string anim = "railgun_emptier_kanim";
		int hitpoints = 250;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.DefaultAnimState = "on";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		return buildingDef;
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x00064CD3 File Offset: 0x00062ED3
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.liquidOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.gasOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.solidOutputPort;
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x00064D08 File Offset: 0x00062F08
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		RailGunPayloadOpener railGunPayloadOpener = go.AddOrGet<RailGunPayloadOpener>();
		railGunPayloadOpener.liquidPortInfo = this.liquidOutputPort;
		railGunPayloadOpener.gasPortInfo = this.gasOutputPort;
		railGunPayloadOpener.solidPortInfo = this.solidOutputPort;
		railGunPayloadOpener.payloadStorage = go.AddComponent<Storage>();
		railGunPayloadOpener.payloadStorage.showInUI = true;
		railGunPayloadOpener.payloadStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		railGunPayloadOpener.payloadStorage.storageFilters = new List<Tag>
		{
			GameTags.RailGunPayloadEmptyable
		};
		railGunPayloadOpener.payloadStorage.capacityKg = 10f;
		railGunPayloadOpener.resourceStorage = go.AddComponent<Storage>();
		railGunPayloadOpener.resourceStorage.showInUI = true;
		railGunPayloadOpener.resourceStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		List<Tag> list = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD.Concat(STORAGEFILTERS.GASES).ToList<Tag>();
		list = list.Concat(STORAGEFILTERS.LIQUIDS).ToList<Tag>();
		railGunPayloadOpener.resourceStorage.storageFilters = list;
		railGunPayloadOpener.resourceStorage.capacityKg = 20000f;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(railGunPayloadOpener.payloadStorage);
		manualDeliveryKG.RequestedItemTag = GameTags.RailGunPayloadEmptyable;
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.None;
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x00064E50 File Offset: 0x00063050
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingCellVisualizer>();
		DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 90f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		RequireInputs component = go.GetComponent<RequireInputs>();
		component.SetRequirements(true, false);
		component.requireConduitHasMass = false;
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x00064EBC File Offset: 0x000630BC
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPorts(go);
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x00064ED4 File Offset: 0x000630D4
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPorts(go);
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x04000AAB RID: 2731
	public const string ID = "RailGunPayloadOpener";

	// Token: 0x04000AAC RID: 2732
	private ConduitPortInfo liquidOutputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));

	// Token: 0x04000AAD RID: 2733
	private ConduitPortInfo gasOutputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 0));

	// Token: 0x04000AAE RID: 2734
	private ConduitPortInfo solidOutputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(-1, 0));
}

using System;
using TUNING;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class JetSuitLockerConfig : IBuildingConfig
{
	// Token: 0x06000B86 RID: 2950 RVA: 0x00043B14 File Offset: 0x00041D14
	public override BuildingDef CreateBuildingDef()
	{
		string id = "JetSuitLocker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_jetsuit_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitLocker");
		return buildingDef;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00043BA3 File Offset: 0x00041DA3
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.secondaryInputPort;
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x00043BB8 File Offset: 0x00041DB8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<SuitLocker>().OutfitTags = new Tag[]
		{
			GameTags.JetSuit
		};
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.capacityKG = 200f;
		go.AddComponent<JetSuitLocker>().portInfo = this.secondaryInputPort;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("JetSuitLocker"),
			new Tag("JetSuitMarker")
		};
		go.AddOrGet<Storage>().capacityKg = 500f;
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x00043C81 File Offset: 0x00041E81
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x00043C92 File Offset: 0x00041E92
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x00043CA2 File Offset: 0x00041EA2
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x0400079B RID: 1947
	public const string ID = "JetSuitLocker";

	// Token: 0x0400079C RID: 1948
	public const float O2_CAPACITY = 200f;

	// Token: 0x0400079D RID: 1949
	public const float SUIT_CAPACITY = 200f;

	// Token: 0x0400079E RID: 1950
	private ConduitPortInfo secondaryInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));
}

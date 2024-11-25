using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
public class WarpConduitSenderConfig : IBuildingConfig
{
	// Token: 0x06001555 RID: 5461 RVA: 0x000752E0 File Offset: 0x000734E0
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x000752E8 File Offset: 0x000734E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WarpConduitSender";
		int width = 4;
		int height = 3;
		string anim = "warp_conduit_sender_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.CanMove = true;
		buildingDef.Invincible = true;
		return buildingDef;
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x0007535F File Offset: 0x0007355F
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.liquidInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.gasInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.solidInputPort;
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x00075394 File Offset: 0x00073594
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		WarpConduitSender warpConduitSender = go.AddOrGet<WarpConduitSender>();
		warpConduitSender.liquidPortInfo = this.liquidInputPort;
		warpConduitSender.gasPortInfo = this.gasInputPort;
		warpConduitSender.solidPortInfo = this.solidInputPort;
		warpConduitSender.gasStorage = go.AddComponent<Storage>();
		warpConduitSender.gasStorage.showInUI = false;
		warpConduitSender.gasStorage.capacityKg = 1f;
		warpConduitSender.liquidStorage = go.AddComponent<Storage>();
		warpConduitSender.liquidStorage.showInUI = false;
		warpConduitSender.liquidStorage.capacityKg = 10f;
		warpConduitSender.solidStorage = go.AddComponent<Storage>();
		warpConduitSender.solidStorage.showInUI = false;
		warpConduitSender.solidStorage.capacityKg = 100f;
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.synchronizeAnims = true;
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_warp_conduit_sender_kanim")
		};
		activatable.workAnims = new HashedString[]
		{
			"sending_pre",
			"sending_loop"
		};
		activatable.workingPstComplete = new HashedString[]
		{
			"sending_pst"
		};
		activatable.workingPstFailed = new HashedString[]
		{
			"sending_pre"
		};
		activatable.SetWorkTime(30f);
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x00075514 File Offset: 0x00073714
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingCellVisualizer>();
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<Activatable>().requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x00075548 File Offset: 0x00073748
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x00075560 File Offset: 0x00073760
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x04000C17 RID: 3095
	public const string ID = "WarpConduitSender";

	// Token: 0x04000C18 RID: 3096
	private ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 1));

	// Token: 0x04000C19 RID: 3097
	private ConduitPortInfo liquidInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));

	// Token: 0x04000C1A RID: 3098
	private ConduitPortInfo solidInputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(2, 1));
}

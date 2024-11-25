using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public class WarpConduitReceiverConfig : IBuildingConfig
{
	// Token: 0x0600154D RID: 5453 RVA: 0x00075063 File Offset: 0x00073263
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x0007506C File Offset: 0x0007326C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WarpConduitReceiver";
		int width = 4;
		int height = 3;
		string anim = "warp_conduit_receiver_kanim";
		int hitpoints = 250;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.DefaultAnimState = "off";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Disinfectable = false;
		buildingDef.Invincible = true;
		buildingDef.Repairable = false;
		return buildingDef;
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x000750EA File Offset: 0x000732EA
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.liquidOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.gasOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.solidOutputPort;
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x00075120 File Offset: 0x00073320
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		WarpConduitReceiver warpConduitReceiver = go.AddOrGet<WarpConduitReceiver>();
		warpConduitReceiver.liquidPortInfo = this.liquidOutputPort;
		warpConduitReceiver.gasPortInfo = this.gasOutputPort;
		warpConduitReceiver.solidPortInfo = this.solidOutputPort;
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.synchronizeAnims = true;
		activatable.workAnims = new HashedString[]
		{
			"touchpanel_interact_pre",
			"touchpanel_interact_loop"
		};
		activatable.workingPstComplete = new HashedString[]
		{
			"touchpanel_interact_pst"
		};
		activatable.workingPstFailed = new HashedString[]
		{
			"touchpanel_interact_pst"
		};
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_warp_conduit_receiver_kanim")
		};
		activatable.SetWorkTime(30f);
		go.AddComponent<ConduitSecondaryOutput>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x0007522F File Offset: 0x0007342F
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingCellVisualizer>();
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<Activatable>().requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x00075263 File Offset: 0x00073463
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x0007527B File Offset: 0x0007347B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x04000C13 RID: 3091
	public const string ID = "WarpConduitReceiver";

	// Token: 0x04000C14 RID: 3092
	private ConduitPortInfo liquidOutputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));

	// Token: 0x04000C15 RID: 3093
	private ConduitPortInfo gasOutputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(-1, 1));

	// Token: 0x04000C16 RID: 3094
	private ConduitPortInfo solidOutputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(1, 1));
}

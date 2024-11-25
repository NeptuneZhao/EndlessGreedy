using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class DoorConfig : IBuildingConfig
{
	// Token: 0x06000226 RID: 550 RVA: 0x0000EF6C File Offset: 0x0000D16C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Door";
		int width = 1;
		int height = 2;
		string anim = "door_internal_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 1f);
		buildingDef.Entombable = true;
		buildingDef.Floodable = false;
		buildingDef.IsFoundation = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
		return buildingDef;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000F028 File Offset: 0x0000D228
	public static List<LogicPorts.Port> CreateSingleInputPortList(CellOffset offset)
	{
		return new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(Door.OPEN_CLOSE_PORT_ID, offset, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_ACTIVE, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_INACTIVE, false, false)
		};
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000F06C File Offset: 0x0000D26C
	public override void DoPostConfigureComplete(GameObject go)
	{
		Door door = go.AddOrGet<Door>();
		door.unpoweredAnimSpeed = 1f;
		door.doorType = Door.DoorType.Internal;
		door.doorOpeningSoundEventName = "Open_DoorInternal";
		door.doorClosingSoundEventName = "Close_DoorInternal";
		go.AddOrGet<AccessControl>().controlEnabled = true;
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
		go.AddOrGet<Workable>().workTime = 3f;
		go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
		go.AddOrGet<ZoneTile>();
		go.AddOrGet<KBoxCollider2D>();
		Prioritizable.AddRef(go);
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000F101 File Offset: 0x0000D301
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		go.AddTag(GameTags.NoCreatureIdling);
	}

	// Token: 0x0400015B RID: 347
	public const string ID = "Door";
}

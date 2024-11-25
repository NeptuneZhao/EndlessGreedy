using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020008E4 RID: 2276
public class GroundTrapConfig : IBuildingConfig
{
	// Token: 0x0600413A RID: 16698 RVA: 0x00172B7C File Offset: 0x00170D7C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureGroundTrap", 2, 2, "critter_trap_ground_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("TRAP_HAS_PREY_STATUS_PORT", new CellOffset(1, 0), STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		return buildingDef;
	}

	// Token: 0x0600413B RID: 16699 RVA: 0x00172C5C File Offset: 0x00170E5C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(go);
		go.AddOrGet<ArmTrapWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_critter_trap_ground_kanim")
		};
		go.AddOrGet<Operational>();
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(GroundTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Walker,
			GameTags.Creatures.Hoverer,
			GameTags.Creatures.Swimmer
		};
		trapTrigger.trappedOffset = new Vector2(0.5f, 0f);
		go.AddOrGetDef<ReusableTrap.Def>().OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
		go.AddOrGet<LogicPorts>();
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x0600413C RID: 16700 RVA: 0x00172D28 File Offset: 0x00170F28
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04002B43 RID: 11075
	public const string ID = "CreatureGroundTrap";

	// Token: 0x04002B44 RID: 11076
	public const string OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";

	// Token: 0x04002B45 RID: 11077
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}

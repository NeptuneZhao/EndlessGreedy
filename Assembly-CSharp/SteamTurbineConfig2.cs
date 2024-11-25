﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class SteamTurbineConfig2 : IBuildingConfig
{
	// Token: 0x0600147E RID: 5246 RVA: 0x00070658 File Offset: 0x0006E858
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SteamTurbine2";
		int width = 5;
		int height = 3;
		string anim = "steamturbine2_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		string[] array = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = array;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 1f);
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
		buildingDef.GeneratorWattageRating = SteamTurbineConfig2.MAX_WATTAGE;
		buildingDef.GeneratorBaseCapacity = SteamTurbineConfig2.MAX_WATTAGE;
		buildingDef.Entombable = true;
		buildingDef.IsFoundation = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(1, 0);
		buildingDef.OverheatTemperature = 1273.15f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x00070757 File Offset: 0x0006E957
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
		SteamTurbineConfig2.AddVisualizer(go);
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x0007077E File Offset: 0x0006E97E
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		SteamTurbineConfig2.AddVisualizer(go);
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x00070788 File Offset: 0x0006E988
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType, false);
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x000707DC File Offset: 0x0006E9DC
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.showDescriptor = false;
		storage.showInUI = false;
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.SetDefaultStoredItemModifiers(SteamTurbineConfig2.StoredItemModifiers);
		storage.capacityKg = 10f;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showDescriptor = false;
		storage2.showInUI = false;
		storage2.storageFilters = STORAGEFILTERS.GASES;
		storage2.SetDefaultStoredItemModifiers(SteamTurbineConfig2.StoredItemModifiers);
		SteamTurbine steamTurbine = go.AddOrGet<SteamTurbine>();
		steamTurbine.srcElem = SimHashes.Steam;
		steamTurbine.destElem = SimHashes.Water;
		steamTurbine.pumpKGRate = 2f;
		steamTurbine.maxSelfHeat = 64f;
		steamTurbine.wasteHeatToTurbinePercent = 0.1f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.storage = storage;
		conduitDispenser.alwaysDispense = true;
		go.AddOrGet<LogicOperationalController>();
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
			StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
			Extents extents = game_object.GetComponent<Building>().GetExtents();
			Extents newExtents = new Extents(extents.x, extents.y - 1, extents.width, extents.height + 1);
			payload.OverrideExtents(newExtents);
			GameComps.StructureTemperatures.SetPayload(handle, ref payload);
			Storage[] components = game_object.GetComponents<Storage>();
			game_object.GetComponent<SteamTurbine>().SetStorage(components[1], components[0]);
		};
		Tinkerable.MakePowerTinkerable(go);
		SteamTurbineConfig2.AddVisualizer(go);
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x000708F8 File Offset: 0x0006EAF8
	private static void AddVisualizer(GameObject go)
	{
		RangeVisualizer rangeVisualizer = go2.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -2;
		rangeVisualizer.RangeMin.y = -2;
		rangeVisualizer.RangeMax.x = 2;
		rangeVisualizer.RangeMax.y = -2;
		rangeVisualizer.TestLineOfSight = false;
		go2.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(SteamTurbineConfig2.SteamTurbineBlockingCB);
		};
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x00070970 File Offset: 0x0006EB70
	public static bool SteamTurbineBlockingCB(int cell)
	{
		Element element = ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		return element.IsLiquid || element.IsSolid;
	}

	// Token: 0x04000BB3 RID: 2995
	public const string ID = "SteamTurbine2";

	// Token: 0x04000BB4 RID: 2996
	public static float MAX_WATTAGE = 850f;

	// Token: 0x04000BB5 RID: 2997
	private const int HEIGHT = 3;

	// Token: 0x04000BB6 RID: 2998
	private const int WIDTH = 5;

	// Token: 0x04000BB7 RID: 2999
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};
}

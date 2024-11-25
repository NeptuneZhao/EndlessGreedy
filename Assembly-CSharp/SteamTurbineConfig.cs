﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020003D3 RID: 979
public class SteamTurbineConfig : IBuildingConfig
{
	// Token: 0x06001479 RID: 5241 RVA: 0x0007040C File Offset: 0x0006E60C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SteamTurbine";
		int width = 5;
		int height = 4;
		string anim = "steamturbine_kanim";
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
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 1f);
		buildingDef.GeneratorWattageRating = 2000f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.Entombable = true;
		buildingDef.IsFoundation = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(1, 0);
		buildingDef.OverheatTemperature = 1273.15f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x000704F4 File Offset: 0x0006E6F4
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x0007051C File Offset: 0x0006E71C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(SteamTurbineConfig.StoredItemModifiers);
		Turbine turbine = go.AddOrGet<Turbine>();
		turbine.srcElem = SimHashes.Steam;
		MakeBaseSolid.Def def = go.AddOrGetDef<MakeBaseSolid.Def>();
		def.solidOffsets = new CellOffset[5];
		for (int i = 0; i < 5; i++)
		{
			def.solidOffsets[i] = new CellOffset(i - 2, 0);
		}
		turbine.pumpKGRate = 10f;
		turbine.requiredMassFlowDifferential = 3f;
		turbine.minEmitMass = 10f;
		turbine.maxRPM = 4000f;
		turbine.rpmAcceleration = turbine.maxRPM / 30f;
		turbine.rpmDeceleration = turbine.maxRPM / 20f;
		turbine.minGenerationRPM = 3000f;
		turbine.minActiveTemperature = 500f;
		turbine.emitTemperature = 425f;
		go.AddOrGet<Generator>();
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
		};
	}

	// Token: 0x04000BAF RID: 2991
	public const string ID = "SteamTurbine";

	// Token: 0x04000BB0 RID: 2992
	private const int HEIGHT = 4;

	// Token: 0x04000BB1 RID: 2993
	private const int WIDTH = 5;

	// Token: 0x04000BB2 RID: 2994
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};
}

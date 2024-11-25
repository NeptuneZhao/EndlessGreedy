﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class CritterTrapPlantConfig : IEntityConfig
{
	// Token: 0x060006ED RID: 1773 RVA: 0x0002E29D File Offset: 0x0002C49D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0002E2A4 File Offset: 0x0002C4A4
	public GameObject CreatePrefab()
	{
		string id = "CritterTrapPlant";
		string name = STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.DESC;
		float mass = 4f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("venus_critter_trap_kanim");
		string initialAnim = "idle_open";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingBack;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		float freezing_ = TUNING.CREATURES.TEMPERATURE.FREEZING_3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, null, freezing_);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING, TUNING.CREATURES.TEMPERATURE.COOL, null, false, 0f, 0.15f, "PlantMeat", true, true, true, false, 2400f, 0f, 2200f, "CritterTrapPlantOriginal", STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.NAME);
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<MutantPlant>());
		TrapTrigger trapTrigger = gameObject.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Walker,
			GameTags.Creatures.Hoverer
		};
		trapTrigger.trappedOffset = new Vector2(0.5f, 0f);
		trapTrigger.enabled = false;
		CritterTrapPlant critterTrapPlant = gameObject.AddOrGet<CritterTrapPlant>();
		critterTrapPlant.gasOutputRate = 0.041666668f;
		critterTrapPlant.outputElement = SimHashes.Hydrogen;
		critterTrapPlant.gasVentThreshold = 33.25f;
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Storage>();
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.016666668f
			}
		});
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "CritterTrapPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.CRITTERTRAPPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.CRITTERTRAPPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_critter_trap_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 21, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, dlcIds), "CritterTrapPlant_preview", Assets.GetAnim("venus_critter_trap_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x0002E4C1 File Offset: 0x0002C6C1
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0002E4C3 File Offset: 0x0002C6C3
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004E7 RID: 1255
	public const string ID = "CritterTrapPlant";

	// Token: 0x040004E8 RID: 1256
	public const float WATER_RATE = 0.016666668f;

	// Token: 0x040004E9 RID: 1257
	public const float GAS_RATE = 0.041666668f;

	// Token: 0x040004EA RID: 1258
	public const float GAS_VENT_THRESHOLD = 33.25f;
}

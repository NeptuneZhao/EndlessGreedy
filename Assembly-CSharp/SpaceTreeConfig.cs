using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class SpaceTreeConfig : IEntityConfig
{
	// Token: 0x060007BC RID: 1980 RVA: 0x00033B7F File Offset: 0x00031D7F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00033B88 File Offset: 0x00031D88
	public GameObject CreatePrefab()
	{
		string id = "SpaceTree";
		string name = STRINGS.CREATURES.SPECIES.SPACETREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SPACETREE.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("syrup_tree_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 255f);
		string text = "SpaceTreeOriginal";
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 173.15f, 198.15f, 258.15f, 293.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.Snow,
			SimHashes.Vacuum
		}, false, 0f, 0.15f, null, true, false, true, false, 2400f, 0f, 12200f, text, STRINGS.CREATURES.SPECIES.SPACETREE.NAME);
		WiltCondition component = gameObject.GetComponent<WiltCondition>();
		component.WiltDelay = 0f;
		component.RecoveryDelay = 0f;
		Modifiers component2 = gameObject.GetComponent<Modifiers>();
		if (gameObject.GetComponent<Traits>() == null)
		{
			gameObject.AddOrGet<Traits>();
			component2.initialTraits.Add(text);
		}
		Crop.CropVal cropval = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == SimHashes.SugarWater.CreateTag());
		Klei.AI.Modifier modifier = Db.Get().traits.Get(component2.initialTraits[0]);
		component2.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
		modifier.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, 4.5f, STRINGS.CREATURES.SPECIES.SPACETREE.NAME, false, false, true));
		gameObject.AddOrGet<Crop>().Configure(cropval);
		KPrefabID component3 = gameObject.GetComponent<KPrefabID>();
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component3.PrefabID().ToString());
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			gameObject.AddOrGet<MutantPlant>().SpeciesID = component3.PrefabTag;
			SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		}
		Growing growing = gameObject.AddOrGet<Growing>();
		growing.shouldGrowOld = false;
		growing.maxAge = 2400f;
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SpaceTreeSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SPACETREE.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SPACETREE.DESC;
		KAnimFile anim = Assets.GetAnim("seed_syrup_tree_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.SPACETREE.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, dlcIds);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Snow.CreateTag(),
				massConsumptionRate = 0.16666667f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "SpaceTree_preview", Assets.GetAnim("syrup_tree_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		DirectlyEdiblePlant_StorageElement directlyEdiblePlant_StorageElement = gameObject.AddOrGet<DirectlyEdiblePlant_StorageElement>();
		directlyEdiblePlant_StorageElement.tagToConsume = SimHashes.SugarWater.CreateTag();
		directlyEdiblePlant_StorageElement.rateProducedPerCycle = 4f;
		directlyEdiblePlant_StorageElement.storageCapacity = 20f;
		directlyEdiblePlant_StorageElement.edibleCellOffsets = new CellOffset[]
		{
			new CellOffset(-1, 0),
			new CellOffset(1, 0),
			new CellOffset(-1, 1),
			new CellOffset(1, 1)
		};
		DirectlyEdiblePlant_TreeBranches directlyEdiblePlant_TreeBranches = gameObject.AddOrGet<DirectlyEdiblePlant_TreeBranches>();
		directlyEdiblePlant_TreeBranches.overrideCropID = "SpaceTreeBranch";
		directlyEdiblePlant_TreeBranches.MinimumEdibleMaturity = 1f;
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.showInUI = true;
		storage.capacityKg = 20f;
		storage.SetDefaultStoredItemModifiers(SpaceTreeConfig.storedItemModifiers);
		ConduitDispenser conduitDispenser = gameObject.AddOrGet<ConduitDispenser>();
		conduitDispenser.noBuildingOutputCellOffset = SpaceTreeConfig.OUTPUT_CONDUIT_CELL_OFFSET;
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.SetOnState(false);
		gameObject.AddOrGet<SpaceTreeSyrupHarvestWorkable>();
		UnstableEntombDefense.Def def = gameObject.AddOrGetDef<UnstableEntombDefense.Def>();
		def.defaultAnimName = "shake_trunk";
		def.Cooldown = 5f;
		PlantBranchGrower.Def def2 = gameObject.AddOrGetDef<PlantBranchGrower.Def>();
		def2.BRANCH_OFFSETS = new CellOffset[]
		{
			new CellOffset(-1, 1),
			new CellOffset(-1, 2),
			new CellOffset(0, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		};
		def2.BRANCH_PREFAB_NAME = "SpaceTreeBranch";
		def2.harvestOnDrown = true;
		def2.propagateHarvestDesignation = false;
		def2.MAX_BRANCH_COUNT = 5;
		SpaceTreePlant.Def def3 = gameObject.AddOrGetDef<SpaceTreePlant.Def>();
		def3.OptimalProductionDuration = 150f;
		def3.OptimalAmountOfBranches = 5;
		return gameObject;
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0003403E File Offset: 0x0003223E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00034040 File Offset: 0x00032240
	public void OnSpawn(GameObject inst)
	{
		EntityCellVisualizer entityCellVisualizer = inst.AddOrGet<EntityCellVisualizer>();
		entityCellVisualizer.AddPort(EntityCellVisualizer.Ports.LiquidOut, SpaceTreeConfig.OUTPUT_CONDUIT_CELL_OFFSET, entityCellVisualizer.Resources.liquidIOColours.output.connected);
	}

	// Token: 0x0400056C RID: 1388
	public const string ID = "SpaceTree";

	// Token: 0x0400056D RID: 1389
	public const string SEED_ID = "SpaceTreeSeed";

	// Token: 0x0400056E RID: 1390
	public const float Temperature_lethal_low = 173.15f;

	// Token: 0x0400056F RID: 1391
	public const float Temperature_warning_low = 198.15f;

	// Token: 0x04000570 RID: 1392
	public const float Temperature_warning_high = 258.15f;

	// Token: 0x04000571 RID: 1393
	public const float Temperature_lethal_high = 293.15f;

	// Token: 0x04000572 RID: 1394
	public const float SNOW_RATE = 0.16666667f;

	// Token: 0x04000573 RID: 1395
	public const float ENTOMB_DEFENSE_COOLDOWN = 5f;

	// Token: 0x04000574 RID: 1396
	public static CellOffset OUTPUT_CONDUIT_CELL_OFFSET = new CellOffset(0, 1);

	// Token: 0x04000575 RID: 1397
	public const float TRUNK_GROWTH_DURATION = 2700f;

	// Token: 0x04000576 RID: 1398
	public const int MAX_BRANCH_NUMBER = 5;

	// Token: 0x04000577 RID: 1399
	public const int OPTIMAL_LUX = 10000;

	// Token: 0x04000578 RID: 1400
	public const float MIN_REQUIRED_LIGHT_TO_GROW_BRANCHES = 300f;

	// Token: 0x04000579 RID: 1401
	public const float SUGAR_WATER_PRODUCTION_DURATION = 150f;

	// Token: 0x0400057A RID: 1402
	public const float SUGAR_WATER_CAPACITY = 20f;

	// Token: 0x0400057B RID: 1403
	public const string MANUAL_HARVEST_PRE_ANIM_NAME = "syrup_harvest_trunk_pre";

	// Token: 0x0400057C RID: 1404
	public const string MANUAL_HARVEST_LOOP_ANIM_NAME = "syrup_harvest_trunk_loop";

	// Token: 0x0400057D RID: 1405
	public const string MANUAL_HARVEST_PST_ANIM_NAME = "syrup_harvest_trunk_pst";

	// Token: 0x0400057E RID: 1406
	public const string MANUAL_HARVEST_INTERRUPT_ANIM_NAME = "syrup_harvest_trunk_loop";

	// Token: 0x0400057F RID: 1407
	private static readonly List<Storage.StoredItemModifier> storedItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};
}

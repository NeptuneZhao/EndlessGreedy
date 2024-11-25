using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class SaltPlantConfig : IEntityConfig
{
	// Token: 0x06000777 RID: 1911 RVA: 0x000319FA File Offset: 0x0002FBFA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00031A04 File Offset: 0x0002FC04
	public GameObject CreatePrefab()
	{
		string id = "SaltPlant";
		string name = STRINGS.CREATURES.SPECIES.SALTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SALTPLANT.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		KAnimFile anim = Assets.GetAnim("saltplant_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 258.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		GameObject template = gameObject;
		float temperature_lethal_low = 198.15f;
		float temperature_warning_low = 248.15f;
		float temperature_warning_high = 323.15f;
		float temperature_lethal_high = 393.15f;
		string crop_id = SimHashes.Salt.ToString();
		string text = STRINGS.CREATURES.SPECIES.SALTPLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.ChlorineGas
		}, true, 0f, 0.025f, crop_id, true, true, true, true, 2400f, 0f, 7400f, "SaltPlantOriginal", text);
		gameObject.AddOrGet<SaltPlant>();
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sand.CreateTag(),
				massConsumptionRate = 0.011666667f
			}
		});
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.capacityKg = 1f;
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.showDescriptor = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.ChlorineGas;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 4;
		elementConsumer.sampleCellOffset = new Vector3(0f, -1f);
		elementConsumer.consumptionRate = 0.006f;
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SaltPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_saltplant_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Bottom;
		text = STRINGS.CREATURES.SPECIES.SALTPLANT.DOMESTICATEDDESC;
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 5, text, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, null, "", false, null), "SaltPlant_preview", Assets.GetAnim("saltplant_kanim"), "place", 1, 2), 1, 2);
		return gameObject;
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00031C61 File Offset: 0x0002FE61
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00031C63 File Offset: 0x0002FE63
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x0400054D RID: 1357
	public const string ID = "SaltPlant";

	// Token: 0x0400054E RID: 1358
	public const string SEED_ID = "SaltPlantSeed";

	// Token: 0x0400054F RID: 1359
	public const float FERTILIZATION_RATE = 0.011666667f;

	// Token: 0x04000550 RID: 1360
	public const float CHLORINE_CONSUMPTION_RATE = 0.006f;
}

using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class SpiceVineConfig : IEntityConfig
{
	// Token: 0x060007E7 RID: 2023 RVA: 0x00034C8E File Offset: 0x00032E8E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00034C98 File Offset: 0x00032E98
	public GameObject CreatePrefab()
	{
		string id = "SpiceVine";
		string name = STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SPICE_VINE.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("vinespicenut_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 3;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 320f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 3);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 308.15f, 358.15f, 448.15f, null, true, 0f, 0.15f, SpiceNutConfig.ID, true, true, true, true, 2400f, 0f, 9800f, "SpiceVineOriginal", STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME);
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.058333334f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Phosphorite,
				massConsumptionRate = 0.0016666667f
			}
		});
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SpiceVineSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_spicenut_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Bottom;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.SPICE_VINE.DOMESTICATEDDESC;
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 4, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, null), "SpiceVine_preview", Assets.GetAnim("vinespicenut_kanim"), "place", 1, 3), 1, 3);
		return gameObject;
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00034EA5 File Offset: 0x000330A5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00034EA7 File Offset: 0x000330A7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000596 RID: 1430
	public const string ID = "SpiceVine";

	// Token: 0x04000597 RID: 1431
	public const string SEED_ID = "SpiceVineSeed";

	// Token: 0x04000598 RID: 1432
	public const float FERTILIZATION_RATE = 0.0016666667f;

	// Token: 0x04000599 RID: 1433
	public const float WATER_RATE = 0.058333334f;
}

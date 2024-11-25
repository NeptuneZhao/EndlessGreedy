using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class BulbPlantConfig : IEntityConfig
{
	// Token: 0x060006CE RID: 1742 RVA: 0x0002D6A3 File Offset: 0x0002B8A3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0002D6AC File Offset: 0x0002B8AC
	public GameObject CreatePrefab()
	{
		string id = "BulbPlant";
		string name = STRINGS.CREATURES.SPECIES.BULBPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BULBPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_bulb_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 288f, 293.15f, 313.15f, 333.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "BulbPlantOriginal", STRINGS.CREATURES.SPECIES.BULBPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "BulbPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_bulb_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.BULBPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, null, "", false, null), "BulbPlant_preview", Assets.GetAnim("potted_bulb_kanim"), "place", 1, 1);
		DiseaseDropper.Def def = gameObject.AddOrGetDef<DiseaseDropper.Def>();
		def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
		def.singleEmitQuantity = 0;
		def.averageEmitPerSecond = 5000;
		def.emitFrequency = 5f;
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "PollenGerms";
		return gameObject;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0002D876 File Offset: 0x0002BA76
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x0002D878 File Offset: 0x0002BA78
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004CA RID: 1226
	public const string ID = "BulbPlant";

	// Token: 0x040004CB RID: 1227
	public const string SEED_ID = "BulbPlantSeed";

	// Token: 0x040004CC RID: 1228
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER1;

	// Token: 0x040004CD RID: 1229
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}

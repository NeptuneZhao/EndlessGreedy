using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class IceFlowerConfig : IEntityConfig
{
	// Token: 0x06000749 RID: 1865 RVA: 0x00030A75 File Offset: 0x0002EC75
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00030A7C File Offset: 0x0002EC7C
	public GameObject CreatePrefab()
	{
		string id = "IceFlower";
		string name = STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.ICEFLOWER.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_ice_flower_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 243.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 173.15f, 203.15f, 278.15f, 318.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.ChlorineGas,
			SimHashes.Hydrogen
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "IceFlowerOriginal", STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "IceFlowerSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.DESC;
		KAnimFile anim = Assets.GetAnim("seed_ice_flower_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.ICEFLOWER.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, null, "", false, dlcIds), "IceFlower_preview", Assets.GetAnim("potted_ice_flower_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00030BF4 File Offset: 0x0002EDF4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x00030BF6 File Offset: 0x0002EDF6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000531 RID: 1329
	public const string ID = "IceFlower";

	// Token: 0x04000532 RID: 1330
	public const string SEED_ID = "IceFlowerSeed";

	// Token: 0x04000533 RID: 1331
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x04000534 RID: 1332
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}

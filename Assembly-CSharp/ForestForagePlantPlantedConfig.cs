using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class ForestForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x06000707 RID: 1799 RVA: 0x0002EB34 File Offset: 0x0002CD34
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0002EB3C File Offset: 0x0002CD3C
	public GameObject CreatePrefab()
	{
		string id = "ForestForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("podmelon_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<DrowningMonitor>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>();
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("ForestForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0002EC13 File Offset: 0x0002CE13
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0002EC15 File Offset: 0x0002CE15
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004FA RID: 1274
	public const string ID = "ForestForagePlantPlanted";
}

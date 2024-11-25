using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class BasicForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x060006BA RID: 1722 RVA: 0x0002CFC4 File Offset: 0x0002B1C4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0002CFCC File Offset: 0x0002B1CC
	public GameObject CreatePrefab()
	{
		string id = "BasicForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.BASICFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BASICFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("muckroot_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
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
		gameObject.AddOrGet<SeedProducer>().Configure("BasicForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0002D0A3 File Offset: 0x0002B2A3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0002D0A5 File Offset: 0x0002B2A5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004BE RID: 1214
	public const string ID = "BasicForagePlantPlanted";
}

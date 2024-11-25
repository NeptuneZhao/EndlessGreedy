using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class SwampForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x060007F7 RID: 2039 RVA: 0x0003502C File Offset: 0x0003322C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00035034 File Offset: 0x00033234
	public GameObject CreatePrefab()
	{
		string id = "SwampForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.SWAMPFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SWAMPFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swamptuber_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>();
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("SwampForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00035104 File Offset: 0x00033304
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00035106 File Offset: 0x00033306
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005A0 RID: 1440
	public const string ID = "SwampForagePlantPlanted";
}

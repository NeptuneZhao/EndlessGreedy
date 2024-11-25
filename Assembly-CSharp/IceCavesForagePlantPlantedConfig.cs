using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class IceCavesForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x06000744 RID: 1860 RVA: 0x00030958 File Offset: 0x0002EB58
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00030960 File Offset: 0x0002EB60
	public GameObject CreatePrefab()
	{
		string id = "IceCavesForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.ICECAVESFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.ICECAVESFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("frozenberries_kanim");
		string initialAnim = "idle";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingBack;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 253.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<DrowningMonitor>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("IceCavesForagePlant", SeedProducer.ProductionType.DigOnly, 2);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00030A69 File Offset: 0x0002EC69
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x00030A6B File Offset: 0x0002EC6B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000530 RID: 1328
	public const string ID = "IceCavesForagePlantPlanted";
}

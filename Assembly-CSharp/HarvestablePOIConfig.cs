using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D2 RID: 722
public class HarvestablePOIConfig : IMultiEntityConfig
{
	// Token: 0x06000F14 RID: 3860 RVA: 0x000571B4 File Offset: 0x000553B4
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (HarvestablePOIConfig.HarvestablePOIParams harvestablePOIParams in this.GenerateConfigs())
		{
			list.Add(HarvestablePOIConfig.CreateHarvestablePOI(harvestablePOIParams.id, harvestablePOIParams.anim, Strings.Get(harvestablePOIParams.nameStringKey), harvestablePOIParams.descStringKey, harvestablePOIParams.poiType.idHash, harvestablePOIParams.poiType.canProvideArtifacts));
		}
		return list;
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0005724C File Offset: 0x0005544C
	public static GameObject CreateHarvestablePOI(string id, string anim, string name, StringKey descStringKey, HashedString poiType, bool canProvideArtifacts = false)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, id, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<HarvestablePOIConfigurator>().presetType = poiType;
		HarvestablePOIClusterGridEntity harvestablePOIClusterGridEntity = gameObject.AddOrGet<HarvestablePOIClusterGridEntity>();
		harvestablePOIClusterGridEntity.m_name = name;
		harvestablePOIClusterGridEntity.m_Anim = anim;
		gameObject.AddOrGetDef<HarvestablePOIStates.Def>();
		if (canProvideArtifacts)
		{
			gameObject.AddOrGetDef<ArtifactPOIStates.Def>();
			gameObject.AddOrGet<ArtifactPOIConfigurator>().presetType = ArtifactPOIConfigurator.defaultArtifactPoiType.idHash;
		}
		gameObject.AddOrGet<InfoDescription>().description = Strings.Get(descStringKey);
		return gameObject;
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x000572C7 File Offset: 0x000554C7
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x000572C9 File Offset: 0x000554C9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x000572CC File Offset: 0x000554CC
	private List<HarvestablePOIConfig.HarvestablePOIParams> GenerateConfigs()
	{
		List<HarvestablePOIConfig.HarvestablePOIParams> list = new List<HarvestablePOIConfig.HarvestablePOIParams>();
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("cloud", new HarvestablePOIConfigurator.HarvestablePOIType("CarbonAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.RefinedCarbon,
				1.5f
			},
			{
				SimHashes.Carbon,
				5.5f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("metallic_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("MetallicAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.MoltenIron,
				1.25f
			},
			{
				SimHashes.Cuprite,
				1.75f
			},
			{
				SimHashes.Obsidian,
				7f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("satellite_field", new HarvestablePOIConfigurator.HarvestablePOIType("SatelliteField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Sand,
				3f
			},
			{
				SimHashes.IronOre,
				3f
			},
			{
				SimHashes.MoltenCopper,
				2.67f
			},
			{
				SimHashes.Glass,
				1.33f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("rocky_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("RockyAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Cuprite,
				2f
			},
			{
				SimHashes.SedimentaryRock,
				4f
			},
			{
				SimHashes.IgneousRock,
				4f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("interstellar_ice_field", new HarvestablePOIConfigurator.HarvestablePOIType("InterstellarIceField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Ice,
				2.5f
			},
			{
				SimHashes.SolidCarbonDioxide,
				7f
			},
			{
				SimHashes.SolidOxygen,
				0.5f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, new List<string>
		{
			Db.Get().OrbitalTypeCategories.iceCloud.Id,
			Db.Get().OrbitalTypeCategories.iceRock.Id
		}, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("organic_mass_field", new HarvestablePOIConfigurator.HarvestablePOIType("OrganicMassField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SlimeMold,
				3f
			},
			{
				SimHashes.Algae,
				3f
			},
			{
				SimHashes.ContaminatedOxygen,
				1f
			},
			{
				SimHashes.Dirt,
				3f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("ice_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("IceAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Ice,
				6f
			},
			{
				SimHashes.SolidCarbonDioxide,
				2f
			},
			{
				SimHashes.Oxygen,
				1.5f
			},
			{
				SimHashes.SolidMethane,
				0.5f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, new List<string>
		{
			Db.Get().OrbitalTypeCategories.iceCloud.Id,
			Db.Get().OrbitalTypeCategories.iceRock.Id
		}, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("gas_giant_cloud", new HarvestablePOIConfigurator.HarvestablePOIType("GasGiantCloud", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Methane,
				1f
			},
			{
				SimHashes.LiquidMethane,
				1f
			},
			{
				SimHashes.SolidMethane,
				1f
			},
			{
				SimHashes.Hydrogen,
				7f
			}
		}, 15000f, 20000f, 30000f, 60000f, true, HarvestablePOIConfig.GasFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("chlorine_cloud", new HarvestablePOIConfigurator.HarvestablePOIType("ChlorineCloud", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Chlorine,
				2.5f
			},
			{
				SimHashes.BleachStone,
				7.5f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.GasFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("gilded_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("GildedAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Gold,
				2.5f
			},
			{
				SimHashes.Fullerene,
				1f
			},
			{
				SimHashes.RefinedCarbon,
				1f
			},
			{
				SimHashes.SedimentaryRock,
				4.5f
			},
			{
				SimHashes.Regolith,
				1f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("glimmering_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("GlimmeringAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.MoltenTungsten,
				2f
			},
			{
				SimHashes.Wolframite,
				6f
			},
			{
				SimHashes.Carbon,
				1f
			},
			{
				SimHashes.CarbonDioxide,
				1f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("helium_cloud", new HarvestablePOIConfigurator.HarvestablePOIType("HeliumCloud", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Hydrogen,
				2f
			},
			{
				SimHashes.Water,
				8f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.GasFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("oily_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("OilyAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SolidCarbonDioxide,
				7.75f
			},
			{
				SimHashes.SolidMethane,
				1.125f
			},
			{
				SimHashes.CrudeOil,
				1.125f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("oxidized_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("OxidizedAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Rust,
				8f
			},
			{
				SimHashes.SolidCarbonDioxide,
				2f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("salty_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("SaltyAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SaltWater,
				5f
			},
			{
				SimHashes.Brine,
				4f
			},
			{
				SimHashes.SolidCarbonDioxide,
				1f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("frozen_ore_field", new HarvestablePOIConfigurator.HarvestablePOIType("FrozenOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Ice,
				2.33f
			},
			{
				SimHashes.DirtyIce,
				2.33f
			},
			{
				SimHashes.Snow,
				1.83f
			},
			{
				SimHashes.AluminumOre,
				2f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("foresty_ore_field", new HarvestablePOIConfigurator.HarvestablePOIType("ForestyOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.IgneousRock,
				7f
			},
			{
				SimHashes.AluminumOre,
				1f
			},
			{
				SimHashes.CarbonDioxide,
				2f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("swampy_ore_field", new HarvestablePOIConfigurator.HarvestablePOIType("SwampyOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Mud,
				2f
			},
			{
				SimHashes.ToxicSand,
				7f
			},
			{
				SimHashes.Cobaltite,
				1f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("sandy_ore_field", new HarvestablePOIConfigurator.HarvestablePOIType("SandyOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SandStone,
				4f
			},
			{
				SimHashes.Algae,
				2f
			},
			{
				SimHashes.Cuprite,
				1f
			},
			{
				SimHashes.Sand,
				3f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("radioactive_gas_cloud", new HarvestablePOIConfigurator.HarvestablePOIType("RadioactiveGasCloud", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.UraniumOre,
				2f
			},
			{
				SimHashes.Chlorine,
				2f
			},
			{
				SimHashes.CarbonDioxide,
				7f
			}
		}, 5000f, 10000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("radioactive_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("RadioactiveAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.UraniumOre,
				2f
			},
			{
				SimHashes.Sulfur,
				3f
			},
			{
				SimHashes.BleachStone,
				2f
			},
			{
				SimHashes.Rust,
				4f
			}
		}, 5000f, 10000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("oxygen_rich_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("OxygenRichAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Water,
				4f
			},
			{
				SimHashes.ContaminatedOxygen,
				2f
			},
			{
				SimHashes.Ice,
				4f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("interstellar_ocean", new HarvestablePOIConfigurator.HarvestablePOIType("InterstellarOcean", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SaltWater,
				2.5f
			},
			{
				SimHashes.Brine,
				2.5f
			},
			{
				SimHashes.Salt,
				2.5f
			},
			{
				SimHashes.Ice,
				2.5f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "EXPANSION1_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("ceres_debris_field", new HarvestablePOIConfigurator.HarvestablePOIType("DLC2CeresField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Cinnabar,
				4.5f
			},
			{
				SimHashes.Mercury,
				2.5f
			},
			{
				SimHashes.Ice,
				2.5f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "DLC2_ID")));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("ceres_starting_field", new HarvestablePOIConfigurator.HarvestablePOIType("DLC2CeresOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Cinnabar,
				2.5f
			},
			{
				SimHashes.Mercury,
				2.5f
			},
			{
				SimHashes.Ice,
				3.5f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, "DLC2_ID")));
		list.RemoveAll((HarvestablePOIConfig.HarvestablePOIParams poi) => !poi.poiType.dlcID.IsNullOrWhiteSpace() && !DlcManager.IsContentSubscribed(poi.poiType.dlcID));
		return list;
	}

	// Token: 0x04000938 RID: 2360
	public const string CarbonAsteroidField = "CarbonAsteroidField";

	// Token: 0x04000939 RID: 2361
	public const string MetallicAsteroidField = "MetallicAsteroidField";

	// Token: 0x0400093A RID: 2362
	public const string SatelliteField = "SatelliteField";

	// Token: 0x0400093B RID: 2363
	public const string RockyAsteroidField = "RockyAsteroidField";

	// Token: 0x0400093C RID: 2364
	public const string InterstellarIceField = "InterstellarIceField";

	// Token: 0x0400093D RID: 2365
	public const string OrganicMassField = "OrganicMassField";

	// Token: 0x0400093E RID: 2366
	public const string IceAsteroidField = "IceAsteroidField";

	// Token: 0x0400093F RID: 2367
	public const string GasGiantCloud = "GasGiantCloud";

	// Token: 0x04000940 RID: 2368
	public const string ChlorineCloud = "ChlorineCloud";

	// Token: 0x04000941 RID: 2369
	public const string GildedAsteroidField = "GildedAsteroidField";

	// Token: 0x04000942 RID: 2370
	public const string GlimmeringAsteroidField = "GlimmeringAsteroidField";

	// Token: 0x04000943 RID: 2371
	public const string HeliumCloud = "HeliumCloud";

	// Token: 0x04000944 RID: 2372
	public const string OilyAsteroidField = "OilyAsteroidField";

	// Token: 0x04000945 RID: 2373
	public const string OxidizedAsteroidField = "OxidizedAsteroidField";

	// Token: 0x04000946 RID: 2374
	public const string SaltyAsteroidField = "SaltyAsteroidField";

	// Token: 0x04000947 RID: 2375
	public const string FrozenOreField = "FrozenOreField";

	// Token: 0x04000948 RID: 2376
	public const string ForestyOreField = "ForestyOreField";

	// Token: 0x04000949 RID: 2377
	public const string SwampyOreField = "SwampyOreField";

	// Token: 0x0400094A RID: 2378
	public const string SandyOreField = "SandyOreField";

	// Token: 0x0400094B RID: 2379
	public const string RadioactiveGasCloud = "RadioactiveGasCloud";

	// Token: 0x0400094C RID: 2380
	public const string RadioactiveAsteroidField = "RadioactiveAsteroidField";

	// Token: 0x0400094D RID: 2381
	public const string OxygenRichAsteroidField = "OxygenRichAsteroidField";

	// Token: 0x0400094E RID: 2382
	public const string InterstellarOcean = "InterstellarOcean";

	// Token: 0x0400094F RID: 2383
	public const string DLC2CeresField = "DLC2CeresField";

	// Token: 0x04000950 RID: 2384
	public const string DLC2CeresOreField = "DLC2CeresOreField";

	// Token: 0x04000951 RID: 2385
	private static readonly List<string> GasFieldOrbit = new List<string>
	{
		Db.Get().OrbitalTypeCategories.iceCloud.Id,
		Db.Get().OrbitalTypeCategories.heliumCloud.Id,
		Db.Get().OrbitalTypeCategories.purpleGas.Id,
		Db.Get().OrbitalTypeCategories.radioactiveGas.Id
	};

	// Token: 0x04000952 RID: 2386
	private static readonly List<string> AsteroidFieldOrbit = new List<string>
	{
		Db.Get().OrbitalTypeCategories.iceRock.Id,
		Db.Get().OrbitalTypeCategories.frozenOre.Id,
		Db.Get().OrbitalTypeCategories.rocky.Id
	};

	// Token: 0x02001113 RID: 4371
	public struct HarvestablePOIParams
	{
		// Token: 0x06007E5D RID: 32349 RVA: 0x0030A3C4 File Offset: 0x003085C4
		public HarvestablePOIParams(string anim, HarvestablePOIConfigurator.HarvestablePOIType poiType)
		{
			this.id = "HarvestableSpacePOI_" + poiType.id;
			this.anim = anim;
			this.nameStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI." + poiType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI." + poiType.id.ToUpper() + ".DESC");
			this.poiType = poiType;
		}

		// Token: 0x04005F07 RID: 24327
		public string id;

		// Token: 0x04005F08 RID: 24328
		public string anim;

		// Token: 0x04005F09 RID: 24329
		public StringKey nameStringKey;

		// Token: 0x04005F0A RID: 24330
		public StringKey descStringKey;

		// Token: 0x04005F0B RID: 24331
		public HarvestablePOIConfigurator.HarvestablePOIType poiType;
	}
}

using System;
using System.Collections.Generic;
using Klei;
using TUNING;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class GeyserGenericConfig : IMultiEntityConfig
{
	// Token: 0x06000729 RID: 1833 RVA: 0x0002F89C File Offset: 0x0002DA9C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		List<GeyserGenericConfig.GeyserPrefabParams> configs = this.GenerateConfigs();
		foreach (GeyserGenericConfig.GeyserPrefabParams geyserPrefabParams in configs)
		{
			list.Add(GeyserGenericConfig.CreateGeyser(geyserPrefabParams.id, geyserPrefabParams.anim, geyserPrefabParams.width, geyserPrefabParams.height, Strings.Get(geyserPrefabParams.nameStringKey), Strings.Get(geyserPrefabParams.descStringKey), geyserPrefabParams.geyserType.idHash, geyserPrefabParams.geyserType.geyserTemperature));
		}
		configs.RemoveAll((GeyserGenericConfig.GeyserPrefabParams x) => !x.isGenericGeyser);
		GameObject gameObject = EntityTemplates.CreateEntity("GeyserGeneric", "Random Geyser Spawner", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			int num = 0;
			if (SaveLoader.Instance.clusterDetailSave != null)
			{
				num = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
			}
			else
			{
				global::Debug.LogWarning("Could not load global world seed for geysers");
			}
			num = num + (int)inst.transform.GetPosition().x + (int)inst.transform.GetPosition().y;
			int index = new KRandom(num).Next(0, configs.Count);
			GameUtil.KInstantiate(Assets.GetPrefab(configs[index].id), inst.transform.GetPosition(), Grid.SceneLayer.BuildingBack, null, 0).SetActive(true);
			inst.DeleteObject();
		};
		list.Add(gameObject);
		return list;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0002F9C4 File Offset: 0x0002DBC4
	public static GameObject CreateGeyser(string id, string anim, int width, int height, string name, string desc, HashedString presetType, float geyserTemperature)
	{
		float mass = 2000f;
		EffectorValues tier = BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim), "inactive", Grid.SceneLayer.BuildingBack, width, height, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.GeyserFeature
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Katairite, true);
		component.Temperature = geyserTemperature;
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uncoverable>();
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<GeyserConfigurator>().presetType = presetType;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0002FAD3 File Offset: 0x0002DCD3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0002FAD5 File Offset: 0x0002DCD5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0002FAD8 File Offset: 0x0002DCD8
	private List<GeyserGenericConfig.GeyserPrefabParams> GenerateConfigs()
	{
		List<GeyserGenericConfig.GeyserPrefabParams> list = new List<GeyserGenericConfig.GeyserPrefabParams>();
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_kanim", 2, 4, new GeyserConfigurator.GeyserType("steam", SimHashes.Steam, GeyserConfigurator.GeyserShape.Gas, 383.15f, 1000f, 2000f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_steam", SimHashes.Steam, GeyserConfigurator.GeyserShape.Gas, 773.15f, 500f, 1000f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_hot_kanim", 4, 2, new GeyserConfigurator.GeyserType("hot_water", SimHashes.Water, GeyserConfigurator.GeyserShape.Liquid, 368.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_water", SimHashes.DirtyWater, GeyserConfigurator.GeyserShape.Liquid, 263.15f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 263f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_filthy_kanim", 4, 2, new GeyserConfigurator.GeyserType("filthy_water", SimHashes.DirtyWater, GeyserConfigurator.GeyserShape.Liquid, 303.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "").AddDisease(new SimUtil.DiseaseInfo
		{
			idx = Db.Get().Diseases.GetIndex("FoodPoisoning"),
			count = 20000
		}), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_cool_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_salt_water", SimHashes.Brine, GeyserConfigurator.GeyserShape.Liquid, 263.15f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 263f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_kanim", 4, 2, new GeyserConfigurator.GeyserType("salt_water", SimHashes.SaltWater, GeyserConfigurator.GeyserShape.Liquid, 368.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_small_kanim", 3, 3, new GeyserConfigurator.GeyserType("small_volcano", SimHashes.Magma, GeyserConfigurator.GeyserShape.Molten, 2000f, 400f, 800f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_big_kanim", 3, 3, new GeyserConfigurator.GeyserType("big_volcano", SimHashes.Magma, GeyserConfigurator.GeyserShape.Molten, 2000f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_co2_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_co2", SimHashes.LiquidCarbonDioxide, GeyserConfigurator.GeyserShape.Liquid, 218f, 100f, 200f, 50f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 218f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_co2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_co2", SimHashes.CarbonDioxide, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_hydrogen_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_hydrogen", SimHashes.Hydrogen, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_po2", SimHashes.ContaminatedOxygen, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_slimy_kanim", 2, 4, new GeyserConfigurator.GeyserType("slimy_po2", SimHashes.ContaminatedOxygen, GeyserConfigurator.GeyserShape.Gas, 333.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "").AddDisease(new SimUtil.DiseaseInfo
		{
			idx = Db.Get().Diseases.GetIndex("SlimeLung"),
			count = 5000
		}), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_chlorine_kanim", 2, 4, new GeyserConfigurator.GeyserType("chlorine_gas", SimHashes.ChlorineGas, GeyserConfigurator.GeyserShape.Gas, 333.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_methane_kanim", 2, 4, new GeyserConfigurator.GeyserType("methane", SimHashes.Methane, GeyserConfigurator.GeyserShape.Gas, 423.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_copper_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_copper", SimHashes.MoltenCopper, GeyserConfigurator.GeyserShape.Molten, 2500f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_iron", SimHashes.MoltenIron, GeyserConfigurator.GeyserShape.Molten, 2800f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_gold_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_gold", SimHashes.MoltenGold, GeyserConfigurator.GeyserShape.Molten, 2900f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_aluminum_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_aluminum", SimHashes.MoltenAluminum, GeyserConfigurator.GeyserShape.Molten, 2000f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_tungsten_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_tungsten", SimHashes.MoltenTungsten, GeyserConfigurator.GeyserShape.Molten, 4000f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_niobium_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_cobalt_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_cobalt", SimHashes.MoltenCobalt, GeyserConfigurator.GeyserShape.Molten, 2500f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_oil_kanim", 4, 2, new GeyserConfigurator.GeyserType("oil_drip", SimHashes.CrudeOil, GeyserConfigurator.GeyserShape.Liquid, 600f, 1f, 250f, 50f, 600f, 600f, 1f, 1f, 100f, 500f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_sulfur_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_sulfur", SimHashes.LiquidSulfur, GeyserConfigurator.GeyserShape.Liquid, 438.34998f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.RemoveAll((GeyserGenericConfig.GeyserPrefabParams geyser) => !geyser.geyserType.DlcID.IsNullOrWhiteSpace() && !DlcManager.IsContentSubscribed(geyser.geyserType.DlcID));
		return list;
	}

	// Token: 0x0400050B RID: 1291
	public const string ID = "GeyserGeneric";

	// Token: 0x0400050C RID: 1292
	public const string Steam = "steam";

	// Token: 0x0400050D RID: 1293
	public const string HotSteam = "hot_steam";

	// Token: 0x0400050E RID: 1294
	public const string HotWater = "hot_water";

	// Token: 0x0400050F RID: 1295
	public const string SlushWater = "slush_water";

	// Token: 0x04000510 RID: 1296
	public const string FilthyWater = "filthy_water";

	// Token: 0x04000511 RID: 1297
	public const string SlushSaltWater = "slush_salt_water";

	// Token: 0x04000512 RID: 1298
	public const string SaltWater = "salt_water";

	// Token: 0x04000513 RID: 1299
	public const string SmallVolcano = "small_volcano";

	// Token: 0x04000514 RID: 1300
	public const string BigVolcano = "big_volcano";

	// Token: 0x04000515 RID: 1301
	public const string LiquidCO2 = "liquid_co2";

	// Token: 0x04000516 RID: 1302
	public const string HotCO2 = "hot_co2";

	// Token: 0x04000517 RID: 1303
	public const string HotHydrogen = "hot_hydrogen";

	// Token: 0x04000518 RID: 1304
	public const string HotPO2 = "hot_po2";

	// Token: 0x04000519 RID: 1305
	public const string SlimyPO2 = "slimy_po2";

	// Token: 0x0400051A RID: 1306
	public const string ChlorineGas = "chlorine_gas";

	// Token: 0x0400051B RID: 1307
	public const string Methane = "methane";

	// Token: 0x0400051C RID: 1308
	public const string MoltenCopper = "molten_copper";

	// Token: 0x0400051D RID: 1309
	public const string MoltenIron = "molten_iron";

	// Token: 0x0400051E RID: 1310
	public const string MoltenGold = "molten_gold";

	// Token: 0x0400051F RID: 1311
	public const string MoltenAluminum = "molten_aluminum";

	// Token: 0x04000520 RID: 1312
	public const string MoltenTungsten = "molten_tungsten";

	// Token: 0x04000521 RID: 1313
	public const string MoltenNiobium = "molten_niobium";

	// Token: 0x04000522 RID: 1314
	public const string MoltenCobalt = "molten_cobalt";

	// Token: 0x04000523 RID: 1315
	public const string OilDrip = "oil_drip";

	// Token: 0x04000524 RID: 1316
	public const string LiquidSulfur = "liquid_sulfur";

	// Token: 0x020010B7 RID: 4279
	public struct GeyserPrefabParams
	{
		// Token: 0x06007CB8 RID: 31928 RVA: 0x00306684 File Offset: 0x00304884
		public GeyserPrefabParams(string anim, int width, int height, GeyserConfigurator.GeyserType geyserType, bool isGenericGeyser)
		{
			this.id = "GeyserGeneric_" + geyserType.id;
			this.anim = anim;
			this.width = width;
			this.height = height;
			this.nameStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".DESC");
			this.geyserType = geyserType;
			this.isGenericGeyser = isGenericGeyser;
		}

		// Token: 0x04005D8B RID: 23947
		public string id;

		// Token: 0x04005D8C RID: 23948
		public string anim;

		// Token: 0x04005D8D RID: 23949
		public int width;

		// Token: 0x04005D8E RID: 23950
		public int height;

		// Token: 0x04005D8F RID: 23951
		public StringKey nameStringKey;

		// Token: 0x04005D90 RID: 23952
		public StringKey descStringKey;

		// Token: 0x04005D91 RID: 23953
		public GeyserConfigurator.GeyserType geyserType;

		// Token: 0x04005D92 RID: 23954
		public bool isGenericGeyser;
	}

	// Token: 0x020010B8 RID: 4280
	private static class TEMPERATURES
	{
		// Token: 0x04005D93 RID: 23955
		public const float BELOW_FREEZING = 263.15f;

		// Token: 0x04005D94 RID: 23956
		public const float DUPE_NORMAL = 303.15f;

		// Token: 0x04005D95 RID: 23957
		public const float DUPE_HOT = 333.15f;

		// Token: 0x04005D96 RID: 23958
		public const float BELOW_BOILING = 368.15f;

		// Token: 0x04005D97 RID: 23959
		public const float ABOVE_BOILING = 383.15f;

		// Token: 0x04005D98 RID: 23960
		public const float HOT1 = 423.15f;

		// Token: 0x04005D99 RID: 23961
		public const float HOT2 = 773.15f;

		// Token: 0x04005D9A RID: 23962
		public const float MOLTEN_MAGMA = 2000f;
	}

	// Token: 0x020010B9 RID: 4281
	public static class RATES
	{
		// Token: 0x04005D9B RID: 23963
		public const float GAS_SMALL_MIN = 40f;

		// Token: 0x04005D9C RID: 23964
		public const float GAS_SMALL_MAX = 80f;

		// Token: 0x04005D9D RID: 23965
		public const float GAS_NORMAL_MIN = 70f;

		// Token: 0x04005D9E RID: 23966
		public const float GAS_NORMAL_MAX = 140f;

		// Token: 0x04005D9F RID: 23967
		public const float GAS_BIG_MIN = 100f;

		// Token: 0x04005DA0 RID: 23968
		public const float GAS_BIG_MAX = 200f;

		// Token: 0x04005DA1 RID: 23969
		public const float LIQUID_SMALL_MIN = 500f;

		// Token: 0x04005DA2 RID: 23970
		public const float LIQUID_SMALL_MAX = 1000f;

		// Token: 0x04005DA3 RID: 23971
		public const float LIQUID_NORMAL_MIN = 1000f;

		// Token: 0x04005DA4 RID: 23972
		public const float LIQUID_NORMAL_MAX = 2000f;

		// Token: 0x04005DA5 RID: 23973
		public const float LIQUID_BIG_MIN = 2000f;

		// Token: 0x04005DA6 RID: 23974
		public const float LIQUID_BIG_MAX = 4000f;

		// Token: 0x04005DA7 RID: 23975
		public const float MOLTEN_NORMAL_MIN = 200f;

		// Token: 0x04005DA8 RID: 23976
		public const float MOLTEN_NORMAL_MAX = 400f;

		// Token: 0x04005DA9 RID: 23977
		public const float MOLTEN_BIG_MIN = 400f;

		// Token: 0x04005DAA RID: 23978
		public const float MOLTEN_BIG_MAX = 800f;

		// Token: 0x04005DAB RID: 23979
		public const float MOLTEN_HUGE_MIN = 800f;

		// Token: 0x04005DAC RID: 23980
		public const float MOLTEN_HUGE_MAX = 1600f;
	}

	// Token: 0x020010BA RID: 4282
	public static class MAX_PRESSURES
	{
		// Token: 0x04005DAD RID: 23981
		public const float GAS = 5f;

		// Token: 0x04005DAE RID: 23982
		public const float GAS_HIGH = 15f;

		// Token: 0x04005DAF RID: 23983
		public const float MOLTEN = 150f;

		// Token: 0x04005DB0 RID: 23984
		public const float LIQUID_SMALL = 50f;

		// Token: 0x04005DB1 RID: 23985
		public const float LIQUID = 500f;
	}

	// Token: 0x020010BB RID: 4283
	public static class ITERATIONS
	{
		// Token: 0x0200238B RID: 9099
		public static class INFREQUENT_MOLTEN
		{
			// Token: 0x04009EEA RID: 40682
			public const float PCT_MIN = 0.005f;

			// Token: 0x04009EEB RID: 40683
			public const float PCT_MAX = 0.01f;

			// Token: 0x04009EEC RID: 40684
			public const float LEN_MIN = 6000f;

			// Token: 0x04009EED RID: 40685
			public const float LEN_MAX = 12000f;
		}

		// Token: 0x0200238C RID: 9100
		public static class FREQUENT_MOLTEN
		{
			// Token: 0x04009EEE RID: 40686
			public const float PCT_MIN = 0.016666668f;

			// Token: 0x04009EEF RID: 40687
			public const float PCT_MAX = 0.1f;

			// Token: 0x04009EF0 RID: 40688
			public const float LEN_MIN = 480f;

			// Token: 0x04009EF1 RID: 40689
			public const float LEN_MAX = 1080f;
		}
	}
}

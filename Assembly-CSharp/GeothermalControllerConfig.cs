using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class GeothermalControllerConfig : IEntityConfig
{
	// Token: 0x06000A63 RID: 2659 RVA: 0x0003D940 File Offset: 0x0003BB40
	public static List<GeothermalVent.ElementInfo> GetClearingEntombedVentReward()
	{
		return new List<GeothermalVent.ElementInfo>
		{
			new GeothermalVent.ElementInfo
			{
				isSolid = false,
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Steam).idx,
				mass = 100f,
				temperature = 1102f,
				diseaseIdx = byte.MaxValue,
				diseaseCount = 0
			},
			new GeothermalVent.ElementInfo
			{
				isSolid = true,
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Lead).idx,
				mass = 144f,
				temperature = 502f,
				diseaseIdx = byte.MaxValue,
				diseaseCount = 0
			}
		};
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0003DA04 File Offset: 0x0003BC04
	public static List<GeothermalControllerConfig.Impurity> GetImpurities()
	{
		return new List<GeothermalControllerConfig.Impurity>
		{
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.IgneousRock).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Granite).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Obsidian).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SaltWater).idx,
				mass_kg = 320f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.DirtyWater).idx,
				mass_kg = 400f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Rust).idx,
				mass_kg = 125f,
				required_temp_range = new MathUtil.MinMax(330f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenLead).idx,
				mass_kg = 65f,
				required_temp_range = new MathUtil.MinMax(540f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SulfurGas).idx,
				mass_kg = 30f,
				required_temp_range = new MathUtil.MinMax(700f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SourGas).idx,
				mass_kg = 200f,
				required_temp_range = new MathUtil.MinMax(800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.IronOre).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(850f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenAluminum).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1200f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenCopper).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1300f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenGold).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1400f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Magma).idx,
				mass_kg = 75f,
				required_temp_range = new MathUtil.MinMax(1800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Hydrogen).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(1800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenIron).idx,
				mass_kg = 250f,
				required_temp_range = new MathUtil.MinMax(1900f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Wolframite).idx,
				mass_kg = 275f,
				required_temp_range = new MathUtil.MinMax(2000f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Fullerene).idx,
				mass_kg = 3f,
				required_temp_range = new MathUtil.MinMax(2500f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Niobium).idx,
				mass_kg = 5f,
				required_temp_range = new MathUtil.MinMax(2500f, float.MaxValue)
			}
		};
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x0003DF5B File Offset: 0x0003C15B
	public static float CalculateOutputTemperature(float inputTemperature)
	{
		if (inputTemperature < 1650f)
		{
			return Math.Min(1650f, inputTemperature + 150f);
		}
		return Math.Max(1650f, inputTemperature - 150f);
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0003DF88 File Offset: 0x0003C188
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0003DF90 File Offset: 0x0003C190
	GameObject IEntityConfig.CreatePrefab()
	{
		string id = "GeothermalControllerEntity";
		string name = STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.EFFECT + "\n\n" + STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geoplant_kanim"), "off", Grid.SceneLayer.BuildingBack, 7, 8, tier, tier2, SimHashes.Unobtanium, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddComponent<EntityCellVisualizer>();
		gameObject.AddComponent<GeothermalController>();
		gameObject.AddComponent<GeothermalPlantComponent>();
		gameObject.AddComponent<Operational>();
		gameObject.AddComponent<GeothermalController.ReconnectPipes>();
		gameObject.AddComponent<Notifier>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showDescriptor = false;
		storage.showInUI = false;
		storage.capacityKg = 12000f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Seal
		});
		return gameObject;
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0003E093 File Offset: 0x0003C293
	void IEntityConfig.OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0003E095 File Offset: 0x0003C295
	void IEntityConfig.OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006B9 RID: 1721
	public const string ID = "GeothermalControllerEntity";

	// Token: 0x040006BA RID: 1722
	public const string KEEPSAKE_ID = "keepsake_geothermalplant";

	// Token: 0x040006BB RID: 1723
	public const string COMPLETED_LORE_ENTRY_UNLOCK_ID = "notes_earthquake";

	// Token: 0x040006BC RID: 1724
	private const string ANIM_FILE = "gravitas_geoplant_kanim";

	// Token: 0x040006BD RID: 1725
	public const string OFFLINE_ANIM = "off";

	// Token: 0x040006BE RID: 1726
	public const string ONLINE_ANIM = "on";

	// Token: 0x040006BF RID: 1727
	public const string OBSTRUCTED_ANIM = "on";

	// Token: 0x040006C0 RID: 1728
	public const float WORKING_LOOP_DURATION_SECONDS = 16f;

	// Token: 0x040006C1 RID: 1729
	public const float HEATPUMP_CAPACITY_KG = 12000f;

	// Token: 0x040006C2 RID: 1730
	public const float OUTPUT_TARGET_TEMPERATURE = 1650f;

	// Token: 0x040006C3 RID: 1731
	public const float OUTPUT_DELTA_TEMPERATURE = 150f;

	// Token: 0x040006C4 RID: 1732
	public const float OUTPUT_PASSTHROUGH_RATIO = 0.92f;

	// Token: 0x040006C5 RID: 1733
	public static MathUtil.MinMax OUTPUT_VENT_WEIGHT_RANGE = new MathUtil.MinMax(43f, 57f);

	// Token: 0x040006C6 RID: 1734
	public static HashSet<Tag> STEEL_FETCH_TAGS = new HashSet<Tag>
	{
		GameTags.Steel
	};

	// Token: 0x040006C7 RID: 1735
	public const float STEEL_FETCH_QUANTITY_KG = 1200f;

	// Token: 0x040006C8 RID: 1736
	public const float RECONNECT_PUMP_CHORE_DURATION_SECONDS = 5f;

	// Token: 0x040006C9 RID: 1737
	public static HashedString RECONNECT_PUMP_ANIM_OVERRIDE = "anim_use_remote_kanim";

	// Token: 0x040006CA RID: 1738
	public const string BAROMETER_ANIM = "meter";

	// Token: 0x040006CB RID: 1739
	public const string BAROMETER_TARGET = "meter_target";

	// Token: 0x040006CC RID: 1740
	public static string[] BAROMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x040006CD RID: 1741
	public const string THERMOMETER_ANIM = "meter_temp";

	// Token: 0x040006CE RID: 1742
	public const string THERMOMETER_TARGET = "meter_target";

	// Token: 0x040006CF RID: 1743
	public static string[] THERMOMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x040006D0 RID: 1744
	public const float THERMOMETER_MIN_TEMP = 50f;

	// Token: 0x040006D1 RID: 1745
	public const float THERMOMETER_RANGE = 2450f;

	// Token: 0x040006D2 RID: 1746
	public static HashedString[] PRESSURE_ANIM_LOOPS = new HashedString[]
	{
		"pressure_loop",
		"high_pressure_loop",
		"high_pressure_loop2"
	};

	// Token: 0x040006D3 RID: 1747
	public static float[] PRESSURE_ANIM_THRESHOLDS = new float[]
	{
		0f,
		0.35f,
		0.85f
	};

	// Token: 0x040006D4 RID: 1748
	public const float CLEAR_ENTOMBED_VENT_THRESHOLD_TEMPERATURE = 602f;

	// Token: 0x020010E7 RID: 4327
	public struct Impurity
	{
		// Token: 0x04005E50 RID: 24144
		public ushort elementIdx;

		// Token: 0x04005E51 RID: 24145
		public float mass_kg;

		// Token: 0x04005E52 RID: 24146
		public MathUtil.MinMax required_temp_range;
	}
}

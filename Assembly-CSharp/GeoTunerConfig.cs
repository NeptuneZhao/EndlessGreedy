﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class GeoTunerConfig : IBuildingConfig
{
	// Token: 0x06000A5E RID: 2654 RVA: 0x0003D020 File Offset: 0x0003B220
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GeoTuner";
		int width = 4;
		int height = 3;
		string anim = "geoTuner_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.Entombable = true;
		buildingDef.Overheatable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "medium";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.UseStructureTemperature = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("GEYSER_ERUPTION_STATUS_PORT", new CellOffset(-1, 1), STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.RequiresPowerInput = true;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		return buildingDef;
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x0003D128 File Offset: 0x0003B328
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 0f;
		List<Storage.StoredItemModifier> defaultStoredItemModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Preserve
		};
		storage.SetDefaultStoredItemModifiers(defaultStoredItemModifiers);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
		manualDeliveryKG.capacity = 0f;
		manualDeliveryKG.refillMass = 0f;
		manualDeliveryKG.SetStorage(storage);
		go.AddOrGet<GeoTunerWorkable>();
		go.AddOrGet<GeoTunerSwitchGeyserWorkable>();
		go.AddOrGet<CopyBuildingSettings>();
		GeoTuner.Def def = go.AddOrGetDef<GeoTuner.Def>();
		def.OUTPUT_LOGIC_PORT_ID = "GEYSER_ERUPTION_STATUS_PORT";
		def.geotunedGeyserSettings = GeoTunerConfig.geotunerGeyserSettings;
		def.defaultSetting = GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.DEFAULT_CATEGORY];
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x0003D224 File Offset: 0x0003B424
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0003D230 File Offset: 0x0003B430
	// Note: this type is marked as 'beforefieldinit'.
	static GeoTunerConfig()
	{
		Dictionary<GeoTunerConfig.Category, GeoTunerConfig.GeotunedGeyserSettings> dictionary = new Dictionary<GeoTunerConfig.Category, GeoTunerConfig.GeotunedGeyserSettings>();
		dictionary[GeoTunerConfig.Category.DEFAULT_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Dirt.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.1f,
				temperatureModifier = 10f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.WATER_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.BleachStone.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 20f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.ORGANIC_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Salt.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 15f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.HYDROCARBON_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Katairite.CreateTag(),
			quantity = 100f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 15f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.VOLCANO_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Katairite.CreateTag(),
			quantity = 100f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 150f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.METALS_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Phosphorus.CreateTag(),
			quantity = 80f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 50f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.CO2_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.ToxicSand.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 5f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		GeoTunerConfig.CategorySettings = dictionary;
		GeoTunerConfig.geotunerGeyserSettings = new Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings>
		{
			{
				"steam",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"hot_steam",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"slimy_po2",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
			},
			{
				"hot_po2",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
			},
			{
				"methane",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
			},
			{
				"chlorine_gas",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
			},
			{
				"hot_co2",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.CO2_CATEGORY]
			},
			{
				"hot_hydrogen",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
			},
			{
				"hot_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"salt_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"slush_salt_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"filthy_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"slush_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"liquid_sulfur",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
			},
			{
				"liquid_co2",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.CO2_CATEGORY]
			},
			{
				"oil_drip",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
			},
			{
				"small_volcano",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.VOLCANO_CATEGORY]
			},
			{
				"big_volcano",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.VOLCANO_CATEGORY]
			},
			{
				"molten_copper",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_gold",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_iron",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_aluminum",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_cobalt",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_niobium",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_tungsten",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			}
		};
	}

	// Token: 0x040006B2 RID: 1714
	public const int MAX_GEOTUNED = 5;

	// Token: 0x040006B3 RID: 1715
	public static Dictionary<GeoTunerConfig.Category, GeoTunerConfig.GeotunedGeyserSettings> CategorySettings;

	// Token: 0x040006B4 RID: 1716
	public static Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings> geotunerGeyserSettings;

	// Token: 0x040006B5 RID: 1717
	public const string ID = "GeoTuner";

	// Token: 0x040006B6 RID: 1718
	public const string OUTPUT_LOGIC_PORT_ID = "GEYSER_ERUPTION_STATUS_PORT";

	// Token: 0x040006B7 RID: 1719
	public const string GeyserAnimationModelTarget = "geyser_target";

	// Token: 0x040006B8 RID: 1720
	public const string GeyserAnimation_GeyserSymbols_LogicLightSymbol = "light_bloom";

	// Token: 0x020010E5 RID: 4325
	public struct GeotunedGeyserSettings
	{
		// Token: 0x06007D74 RID: 32116 RVA: 0x00308762 File Offset: 0x00306962
		public GeotunedGeyserSettings(Tag material, float quantity, float duration, Geyser.GeyserModification template)
		{
			this.quantity = quantity;
			this.material = material;
			this.template = template;
			this.duration = duration;
		}

		// Token: 0x04005E44 RID: 24132
		public Tag material;

		// Token: 0x04005E45 RID: 24133
		public float quantity;

		// Token: 0x04005E46 RID: 24134
		public Geyser.GeyserModification template;

		// Token: 0x04005E47 RID: 24135
		public float duration;
	}

	// Token: 0x020010E6 RID: 4326
	public enum Category
	{
		// Token: 0x04005E49 RID: 24137
		DEFAULT_CATEGORY,
		// Token: 0x04005E4A RID: 24138
		WATER_CATEGORY,
		// Token: 0x04005E4B RID: 24139
		ORGANIC_CATEGORY,
		// Token: 0x04005E4C RID: 24140
		HYDROCARBON_CATEGORY,
		// Token: 0x04005E4D RID: 24141
		VOLCANO_CATEGORY,
		// Token: 0x04005E4E RID: 24142
		METALS_CATEGORY,
		// Token: 0x04005E4F RID: 24143
		CO2_CATEGORY
	}
}

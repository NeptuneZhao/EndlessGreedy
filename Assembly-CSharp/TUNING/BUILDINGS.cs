using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02000EEB RID: 3819
	public class BUILDINGS
	{
		// Token: 0x040056EA RID: 22250
		public const float DEFAULT_STORAGE_CAPACITY = 2000f;

		// Token: 0x040056EB RID: 22251
		public const float STANDARD_MANUAL_REFILL_LEVEL = 0.2f;

		// Token: 0x040056EC RID: 22252
		public const float MASS_TEMPERATURE_SCALE = 0.2f;

		// Token: 0x040056ED RID: 22253
		public const float AIRCONDITIONER_TEMPDELTA = -14f;

		// Token: 0x040056EE RID: 22254
		public const float MAX_ENVIRONMENT_DELTA = -50f;

		// Token: 0x040056EF RID: 22255
		public const float COMPOST_FLIP_TIME = 20f;

		// Token: 0x040056F0 RID: 22256
		public const int TUBE_LAUNCHER_MAX_CHARGES = 3;

		// Token: 0x040056F1 RID: 22257
		public const float TUBE_LAUNCHER_RECHARGE_TIME = 10f;

		// Token: 0x040056F2 RID: 22258
		public const float TUBE_LAUNCHER_WORK_TIME = 1f;

		// Token: 0x040056F3 RID: 22259
		public const float SMELTER_INGOT_INPUTKG = 500f;

		// Token: 0x040056F4 RID: 22260
		public const float SMELTER_INGOT_OUTPUTKG = 100f;

		// Token: 0x040056F5 RID: 22261
		public const float SMELTER_FABRICATIONTIME = 120f;

		// Token: 0x040056F6 RID: 22262
		public const float GEOREFINERY_SLAB_INPUTKG = 1000f;

		// Token: 0x040056F7 RID: 22263
		public const float GEOREFINERY_SLAB_OUTPUTKG = 200f;

		// Token: 0x040056F8 RID: 22264
		public const float GEOREFINERY_FABRICATIONTIME = 120f;

		// Token: 0x040056F9 RID: 22265
		public const float MASS_BURN_RATE_HYDROGENGENERATOR = 0.1f;

		// Token: 0x040056FA RID: 22266
		public const float COOKER_FOOD_TEMPERATURE = 368.15f;

		// Token: 0x040056FB RID: 22267
		public const float OVERHEAT_DAMAGE_INTERVAL = 7.5f;

		// Token: 0x040056FC RID: 22268
		public const float MIN_BUILD_TEMPERATURE = 0f;

		// Token: 0x040056FD RID: 22269
		public const float MAX_BUILD_TEMPERATURE = 318.15f;

		// Token: 0x040056FE RID: 22270
		public const float MELTDOWN_TEMPERATURE = 533.15f;

		// Token: 0x040056FF RID: 22271
		public const float REPAIR_FORCE_TEMPERATURE = 293.15f;

		// Token: 0x04005700 RID: 22272
		public const int REPAIR_EFFECTIVENESS_BASE = 10;

		// Token: 0x04005701 RID: 22273
		public static Dictionary<string, string> PLANSUBCATEGORYSORTING = new Dictionary<string, string>
		{
			{
				"Ladder",
				"ladders"
			},
			{
				"FirePole",
				"ladders"
			},
			{
				"LadderFast",
				"ladders"
			},
			{
				"Tile",
				"tiles"
			},
			{
				"SnowTile",
				"tiles"
			},
			{
				"WoodTile",
				"tiles"
			},
			{
				"GasPermeableMembrane",
				"tiles"
			},
			{
				"MeshTile",
				"tiles"
			},
			{
				"InsulationTile",
				"tiles"
			},
			{
				"PlasticTile",
				"tiles"
			},
			{
				"MetalTile",
				"tiles"
			},
			{
				"GlassTile",
				"tiles"
			},
			{
				"StorageTile",
				"tiles"
			},
			{
				"BunkerTile",
				"tiles"
			},
			{
				"ExteriorWall",
				"tiles"
			},
			{
				"CarpetTile",
				"tiles"
			},
			{
				"ExobaseHeadquarters",
				"printingpods"
			},
			{
				"Door",
				"doors"
			},
			{
				"ManualPressureDoor",
				"doors"
			},
			{
				"PressureDoor",
				"doors"
			},
			{
				"BunkerDoor",
				"doors"
			},
			{
				"StorageLocker",
				"storage"
			},
			{
				"StorageLockerSmart",
				"storage"
			},
			{
				"LiquidReservoir",
				"storage"
			},
			{
				"GasReservoir",
				"storage"
			},
			{
				"ObjectDispenser",
				"storage"
			},
			{
				"TravelTube",
				"transport"
			},
			{
				"TravelTubeEntrance",
				"transport"
			},
			{
				"TravelTubeWallBridge",
				"transport"
			},
			{
				RemoteWorkerDockConfig.ID,
				"operations"
			},
			{
				RemoteWorkTerminalConfig.ID,
				"operations"
			},
			{
				"MineralDeoxidizer",
				"producers"
			},
			{
				"SublimationStation",
				"producers"
			},
			{
				"Oxysconce",
				"producers"
			},
			{
				"Electrolyzer",
				"producers"
			},
			{
				"RustDeoxidizer",
				"producers"
			},
			{
				"AirFilter",
				"scrubbers"
			},
			{
				"CO2Scrubber",
				"scrubbers"
			},
			{
				"AlgaeHabitat",
				"scrubbers"
			},
			{
				"DevGenerator",
				"generators"
			},
			{
				"ManualGenerator",
				"generators"
			},
			{
				"Generator",
				"generators"
			},
			{
				"WoodGasGenerator",
				"generators"
			},
			{
				"HydrogenGenerator",
				"generators"
			},
			{
				"MethaneGenerator",
				"generators"
			},
			{
				"PetroleumGenerator",
				"generators"
			},
			{
				"SteamTurbine",
				"generators"
			},
			{
				"SteamTurbine2",
				"generators"
			},
			{
				"SolarPanel",
				"generators"
			},
			{
				"Wire",
				"wires"
			},
			{
				"WireBridge",
				"wires"
			},
			{
				"HighWattageWire",
				"wires"
			},
			{
				"WireBridgeHighWattage",
				"wires"
			},
			{
				"WireRefined",
				"wires"
			},
			{
				"WireRefinedBridge",
				"wires"
			},
			{
				"WireRefinedHighWattage",
				"wires"
			},
			{
				"WireRefinedBridgeHighWattage",
				"wires"
			},
			{
				"Battery",
				"batteries"
			},
			{
				"BatteryMedium",
				"batteries"
			},
			{
				"BatterySmart",
				"batteries"
			},
			{
				"ElectrobankCharger",
				"electrobankbuildings"
			},
			{
				"SmallElectrobankDischarger",
				"electrobankbuildings"
			},
			{
				"LargeElectrobankDischarger",
				"electrobankbuildings"
			},
			{
				"PowerTransformerSmall",
				"powercontrol"
			},
			{
				"PowerTransformer",
				"powercontrol"
			},
			{
				SwitchConfig.ID,
				"switches"
			},
			{
				LogicPowerRelayConfig.ID,
				"switches"
			},
			{
				TemperatureControlledSwitchConfig.ID,
				"switches"
			},
			{
				PressureSwitchLiquidConfig.ID,
				"switches"
			},
			{
				PressureSwitchGasConfig.ID,
				"switches"
			},
			{
				"MicrobeMusher",
				"cooking"
			},
			{
				"CookingStation",
				"cooking"
			},
			{
				"Deepfryer",
				"cooking"
			},
			{
				"GourmetCookingStation",
				"cooking"
			},
			{
				"SpiceGrinder",
				"cooking"
			},
			{
				"FoodDehydrator",
				"cooking"
			},
			{
				"FoodRehydrator",
				"cooking"
			},
			{
				"PlanterBox",
				"farming"
			},
			{
				"FarmTile",
				"farming"
			},
			{
				"HydroponicFarm",
				"farming"
			},
			{
				"RationBox",
				"storage"
			},
			{
				"Refrigerator",
				"storage"
			},
			{
				"CreatureDeliveryPoint",
				"ranching"
			},
			{
				"CritterDropOff",
				"ranching"
			},
			{
				"CritterPickUp",
				"ranching"
			},
			{
				"FishDeliveryPoint",
				"ranching"
			},
			{
				"CreatureFeeder",
				"ranching"
			},
			{
				"FishFeeder",
				"ranching"
			},
			{
				"MilkFeeder",
				"ranching"
			},
			{
				"EggIncubator",
				"ranching"
			},
			{
				"EggCracker",
				"ranching"
			},
			{
				"CreatureGroundTrap",
				"ranching"
			},
			{
				"CreatureAirTrap",
				"ranching"
			},
			{
				"WaterTrap",
				"ranching"
			},
			{
				"CritterCondo",
				"ranching"
			},
			{
				"UnderwaterCritterCondo",
				"ranching"
			},
			{
				"AirBorneCritterCondo",
				"ranching"
			},
			{
				"Outhouse",
				"washroom"
			},
			{
				"FlushToilet",
				"washroom"
			},
			{
				"WallToilet",
				"washroom"
			},
			{
				ShowerConfig.ID,
				"washroom"
			},
			{
				"GunkEmptier",
				"washroom"
			},
			{
				"LiquidConduit",
				"pipes"
			},
			{
				"InsulatedLiquidConduit",
				"pipes"
			},
			{
				"LiquidConduitRadiant",
				"pipes"
			},
			{
				"LiquidConduitBridge",
				"pipes"
			},
			{
				"ContactConductivePipeBridge",
				"pipes"
			},
			{
				"LiquidVent",
				"pipes"
			},
			{
				"LiquidPump",
				"pumps"
			},
			{
				"LiquidMiniPump",
				"pumps"
			},
			{
				"LiquidPumpingStation",
				"pumps"
			},
			{
				"DevPumpLiquid",
				"pumps"
			},
			{
				"BottleEmptier",
				"valves"
			},
			{
				"LiquidFilter",
				"valves"
			},
			{
				"LiquidConduitPreferentialFlow",
				"valves"
			},
			{
				"LiquidConduitOverflow",
				"valves"
			},
			{
				"LiquidValve",
				"valves"
			},
			{
				"LiquidLogicValve",
				"valves"
			},
			{
				"LiquidLimitValve",
				"valves"
			},
			{
				"LiquidBottler",
				"valves"
			},
			{
				"BottleEmptierConduitLiquid",
				"valves"
			},
			{
				LiquidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortLiquid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortLiquidUnloader",
				"buildmenuports"
			},
			{
				"GasConduit",
				"pipes"
			},
			{
				"InsulatedGasConduit",
				"pipes"
			},
			{
				"GasConduitRadiant",
				"pipes"
			},
			{
				"GasConduitBridge",
				"pipes"
			},
			{
				"GasVent",
				"pipes"
			},
			{
				"GasVentHighPressure",
				"pipes"
			},
			{
				"GasPump",
				"pumps"
			},
			{
				"GasMiniPump",
				"pumps"
			},
			{
				"DevPumpGas",
				"pumps"
			},
			{
				"GasBottler",
				"valves"
			},
			{
				"BottleEmptierGas",
				"valves"
			},
			{
				"BottleEmptierConduitGas",
				"valves"
			},
			{
				"GasFilter",
				"valves"
			},
			{
				"GasConduitPreferentialFlow",
				"valves"
			},
			{
				"GasConduitOverflow",
				"valves"
			},
			{
				"GasValve",
				"valves"
			},
			{
				"GasLogicValve",
				"valves"
			},
			{
				"GasLimitValve",
				"valves"
			},
			{
				GasConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortGas",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortGasUnloader",
				"buildmenuports"
			},
			{
				"Compost",
				"organic"
			},
			{
				"FertilizerMaker",
				"organic"
			},
			{
				"AlgaeDistillery",
				"organic"
			},
			{
				"EthanolDistillery",
				"organic"
			},
			{
				"SludgePress",
				"organic"
			},
			{
				"MilkFatSeparator",
				"organic"
			},
			{
				"MilkPress",
				"organic"
			},
			{
				"IceKettle",
				"materials"
			},
			{
				"WaterPurifier",
				"materials"
			},
			{
				"Desalinator",
				"materials"
			},
			{
				"RockCrusher",
				"materials"
			},
			{
				"Kiln",
				"materials"
			},
			{
				"MetalRefinery",
				"materials"
			},
			{
				"GlassForge",
				"materials"
			},
			{
				"OilRefinery",
				"oil"
			},
			{
				"Polymerizer",
				"oil"
			},
			{
				"OxyliteRefinery",
				"advanced"
			},
			{
				"SupermaterialRefinery",
				"advanced"
			},
			{
				"DiamondPress",
				"advanced"
			},
			{
				"Chlorinator",
				"advanced"
			},
			{
				"WashBasin",
				"hygiene"
			},
			{
				"WashSink",
				"hygiene"
			},
			{
				"HandSanitizer",
				"hygiene"
			},
			{
				"DecontaminationShower",
				"hygiene"
			},
			{
				"Apothecary",
				"medical"
			},
			{
				"DoctorStation",
				"medical"
			},
			{
				"AdvancedDoctorStation",
				"medical"
			},
			{
				"MedicalCot",
				"medical"
			},
			{
				"DevLifeSupport",
				"medical"
			},
			{
				"MassageTable",
				"wellness"
			},
			{
				"Grave",
				"wellness"
			},
			{
				"OilChanger",
				"wellness"
			},
			{
				"Bed",
				"beds"
			},
			{
				"LuxuryBed",
				"beds"
			},
			{
				LadderBedConfig.ID,
				"beds"
			},
			{
				"FloorLamp",
				"lights"
			},
			{
				"CeilingLight",
				"lights"
			},
			{
				"SunLamp",
				"lights"
			},
			{
				"DevLightGenerator",
				"lights"
			},
			{
				"MercuryCeilingLight",
				"lights"
			},
			{
				"DiningTable",
				"dining"
			},
			{
				"WaterCooler",
				"recreation"
			},
			{
				"Phonobox",
				"recreation"
			},
			{
				"ArcadeMachine",
				"recreation"
			},
			{
				"EspressoMachine",
				"recreation"
			},
			{
				"HotTub",
				"recreation"
			},
			{
				"MechanicalSurfboard",
				"recreation"
			},
			{
				"Sauna",
				"recreation"
			},
			{
				"Juicer",
				"recreation"
			},
			{
				"SodaFountain",
				"recreation"
			},
			{
				"BeachChair",
				"recreation"
			},
			{
				"VerticalWindTunnel",
				"recreation"
			},
			{
				"Telephone",
				"recreation"
			},
			{
				"FlowerVase",
				"decor"
			},
			{
				"FlowerVaseWall",
				"decor"
			},
			{
				"FlowerVaseHanging",
				"decor"
			},
			{
				"FlowerVaseHangingFancy",
				"decor"
			},
			{
				PixelPackConfig.ID,
				"decor"
			},
			{
				"SmallSculpture",
				"decor"
			},
			{
				"Sculpture",
				"decor"
			},
			{
				"IceSculpture",
				"decor"
			},
			{
				"MarbleSculpture",
				"decor"
			},
			{
				"MetalSculpture",
				"decor"
			},
			{
				"WoodSculpture",
				"decor"
			},
			{
				"CrownMoulding",
				"decor"
			},
			{
				"CornerMoulding",
				"decor"
			},
			{
				"Canvas",
				"decor"
			},
			{
				"CanvasWide",
				"decor"
			},
			{
				"CanvasTall",
				"decor"
			},
			{
				"ItemPedestal",
				"decor"
			},
			{
				"ParkSign",
				"decor"
			},
			{
				"MonumentBottom",
				"decor"
			},
			{
				"MonumentMiddle",
				"decor"
			},
			{
				"MonumentTop",
				"decor"
			},
			{
				"ResearchCenter",
				"research"
			},
			{
				"AdvancedResearchCenter",
				"research"
			},
			{
				"GeoTuner",
				"research"
			},
			{
				"NuclearResearchCenter",
				"research"
			},
			{
				"OrbitalResearchCenter",
				"research"
			},
			{
				"CosmicResearchCenter",
				"research"
			},
			{
				"DLC1CosmicResearchCenter",
				"research"
			},
			{
				"DataMiner",
				"research"
			},
			{
				"ArtifactAnalysisStation",
				"archaeology"
			},
			{
				"MissileFabricator",
				"meteordefense"
			},
			{
				"AstronautTrainingCenter",
				"exploration"
			},
			{
				"PowerControlStation",
				"industrialstation"
			},
			{
				"ResetSkillsStation",
				"industrialstation"
			},
			{
				"RoleStation",
				"workstations"
			},
			{
				"RanchStation",
				"ranching"
			},
			{
				"ShearingStation",
				"ranching"
			},
			{
				"MilkingStation",
				"ranching"
			},
			{
				"FarmStation",
				"farming"
			},
			{
				"GeneticAnalysisStation",
				"farming"
			},
			{
				"CraftingTable",
				"manufacturing"
			},
			{
				"AdvancedCraftingTable",
				"manufacturing"
			},
			{
				"ClothingFabricator",
				"manufacturing"
			},
			{
				"ClothingAlterationStation",
				"manufacturing"
			},
			{
				"SuitFabricator",
				"manufacturing"
			},
			{
				"OxygenMaskMarker",
				"equipment"
			},
			{
				"OxygenMaskLocker",
				"equipment"
			},
			{
				"SuitMarker",
				"equipment"
			},
			{
				"SuitLocker",
				"equipment"
			},
			{
				"JetSuitMarker",
				"equipment"
			},
			{
				"JetSuitLocker",
				"equipment"
			},
			{
				"MissileLauncher",
				"missiles"
			},
			{
				"LeadSuitMarker",
				"equipment"
			},
			{
				"LeadSuitLocker",
				"equipment"
			},
			{
				"Campfire",
				"temperature"
			},
			{
				"DevHeater",
				"temperature"
			},
			{
				"SpaceHeater",
				"temperature"
			},
			{
				"LiquidHeater",
				"temperature"
			},
			{
				"LiquidConditioner",
				"temperature"
			},
			{
				"LiquidCooledFan",
				"temperature"
			},
			{
				"IceCooledFan",
				"temperature"
			},
			{
				"IceMachine",
				"temperature"
			},
			{
				"AirConditioner",
				"temperature"
			},
			{
				"ThermalBlock",
				"temperature"
			},
			{
				"OreScrubber",
				"sanitation"
			},
			{
				"OilWellCap",
				"oil"
			},
			{
				"SweepBotStation",
				"sanitation"
			},
			{
				"LogicWire",
				"wires"
			},
			{
				"LogicWireBridge",
				"wires"
			},
			{
				"LogicRibbon",
				"wires"
			},
			{
				"LogicRibbonBridge",
				"wires"
			},
			{
				LogicRibbonReaderConfig.ID,
				"wires"
			},
			{
				LogicRibbonWriterConfig.ID,
				"wires"
			},
			{
				"LogicDuplicantSensor",
				"sensors"
			},
			{
				LogicPressureSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicPressureSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				LogicLightSensorConfig.ID,
				"sensors"
			},
			{
				LogicWattageSensorConfig.ID,
				"sensors"
			},
			{
				LogicTimeOfDaySensorConfig.ID,
				"sensors"
			},
			{
				LogicTimerSensorConfig.ID,
				"sensors"
			},
			{
				LogicDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicCritterCountSensorConfig.ID,
				"sensors"
			},
			{
				LogicRadiationSensorConfig.ID,
				"sensors"
			},
			{
				LogicHEPSensorConfig.ID,
				"sensors"
			},
			{
				CometDetectorConfig.ID,
				"sensors"
			},
			{
				LogicCounterConfig.ID,
				"logicmanager"
			},
			{
				"Checkpoint",
				"logicmanager"
			},
			{
				LogicAlarmConfig.ID,
				"logicmanager"
			},
			{
				LogicHammerConfig.ID,
				"logicaudio"
			},
			{
				LogicSwitchConfig.ID,
				"switches"
			},
			{
				"FloorSwitch",
				"switches"
			},
			{
				"LogicGateNOT",
				"logicgates"
			},
			{
				"LogicGateAND",
				"logicgates"
			},
			{
				"LogicGateOR",
				"logicgates"
			},
			{
				"LogicGateBUFFER",
				"logicgates"
			},
			{
				"LogicGateFILTER",
				"logicgates"
			},
			{
				"LogicGateXOR",
				"logicgates"
			},
			{
				LogicMemoryConfig.ID,
				"logicgates"
			},
			{
				"LogicGateMultiplexer",
				"logicgates"
			},
			{
				"LogicGateDemultiplexer",
				"logicgates"
			},
			{
				"LogicInterasteroidSender",
				"transmissions"
			},
			{
				"LogicInterasteroidReceiver",
				"transmissions"
			},
			{
				"SolidConduit",
				"conveyancestructures"
			},
			{
				"SolidConduitBridge",
				"conveyancestructures"
			},
			{
				"SolidConduitInbox",
				"conveyancestructures"
			},
			{
				"SolidConduitOutbox",
				"conveyancestructures"
			},
			{
				"SolidFilter",
				"conveyancestructures"
			},
			{
				"SolidVent",
				"conveyancestructures"
			},
			{
				"DevPumpSolid",
				"pumps"
			},
			{
				"SolidLogicValve",
				"valves"
			},
			{
				"SolidLimitValve",
				"valves"
			},
			{
				SolidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"AutoMiner",
				"automated"
			},
			{
				"SolidTransferArm",
				"automated"
			},
			{
				"ModularLaunchpadPortSolid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortSolidUnloader",
				"buildmenuports"
			},
			{
				"Telescope",
				"telescopes"
			},
			{
				"ClusterTelescope",
				"telescopes"
			},
			{
				"ClusterTelescopeEnclosed",
				"telescopes"
			},
			{
				"LaunchPad",
				"rocketstructures"
			},
			{
				"Gantry",
				"rocketstructures"
			},
			{
				"ModularLaunchpadPortBridge",
				"rocketstructures"
			},
			{
				"RailGun",
				"fittings"
			},
			{
				"RailGunPayloadOpener",
				"fittings"
			},
			{
				"LandingBeacon",
				"rocketnav"
			},
			{
				"SteamEngine",
				"engines"
			},
			{
				"KeroseneEngine",
				"engines"
			},
			{
				"HydrogenEngine",
				"engines"
			},
			{
				"SolidBooster",
				"engines"
			},
			{
				"LiquidFuelTank",
				"tanks"
			},
			{
				"OxidizerTank",
				"tanks"
			},
			{
				"OxidizerTankLiquid",
				"tanks"
			},
			{
				"CargoBay",
				"cargo"
			},
			{
				"GasCargoBay",
				"cargo"
			},
			{
				"LiquidCargoBay",
				"cargo"
			},
			{
				"SpecialCargoBay",
				"cargo"
			},
			{
				"CommandModule",
				"rocketnav"
			},
			{
				RocketControlStationConfig.ID,
				"rocketnav"
			},
			{
				LogicClusterLocationSensorConfig.ID,
				"rocketnav"
			},
			{
				"MissionControl",
				"rocketnav"
			},
			{
				"MissionControlCluster",
				"rocketnav"
			},
			{
				"RoboPilotCommandModule",
				"rocketnav"
			},
			{
				"TouristModule",
				"module"
			},
			{
				"ResearchModule",
				"module"
			},
			{
				"RocketInteriorPowerPlug",
				"fittings"
			},
			{
				"RocketInteriorLiquidInput",
				"fittings"
			},
			{
				"RocketInteriorLiquidOutput",
				"fittings"
			},
			{
				"RocketInteriorGasInput",
				"fittings"
			},
			{
				"RocketInteriorGasOutput",
				"fittings"
			},
			{
				"RocketInteriorSolidInput",
				"fittings"
			},
			{
				"RocketInteriorSolidOutput",
				"fittings"
			},
			{
				"ManualHighEnergyParticleSpawner",
				"producers"
			},
			{
				"HighEnergyParticleSpawner",
				"producers"
			},
			{
				"DevHEPSpawner",
				"producers"
			},
			{
				"HighEnergyParticleRedirector",
				"transmissions"
			},
			{
				"HEPBattery",
				"batteries"
			},
			{
				"HEPBridgeTile",
				"transmissions"
			},
			{
				"NuclearReactor",
				"producers"
			},
			{
				"UraniumCentrifuge",
				"producers"
			},
			{
				"RadiationLight",
				"producers"
			},
			{
				"DevRadiationGenerator",
				"producers"
			}
		};

		// Token: 0x04005702 RID: 22274
		public static List<PlanScreen.PlanInfo> PLANORDER = new List<PlanScreen.PlanInfo>
		{
			new PlanScreen.PlanInfo(new HashedString("Base"), false, new List<string>
			{
				"Ladder",
				"FirePole",
				"LadderFast",
				"Tile",
				"SnowTile",
				"WoodTile",
				"GasPermeableMembrane",
				"MeshTile",
				"InsulationTile",
				"PlasticTile",
				"MetalTile",
				"GlassTile",
				"StorageTile",
				"BunkerTile",
				"CarpetTile",
				"ExteriorWall",
				"ExobaseHeadquarters",
				"Door",
				"ManualPressureDoor",
				"PressureDoor",
				"BunkerDoor",
				"StorageLocker",
				"StorageLockerSmart",
				"LiquidReservoir",
				"GasReservoir",
				"ObjectDispenser",
				"TravelTube",
				"TravelTubeEntrance",
				"TravelTubeWallBridge"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Oxygen"), false, new List<string>
			{
				"MineralDeoxidizer",
				"SublimationStation",
				"Oxysconce",
				"AlgaeHabitat",
				"AirFilter",
				"CO2Scrubber",
				"Electrolyzer",
				"RustDeoxidizer"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Power"), false, new List<string>
			{
				"DevGenerator",
				"ManualGenerator",
				"Generator",
				"WoodGasGenerator",
				"HydrogenGenerator",
				"MethaneGenerator",
				"PetroleumGenerator",
				"SteamTurbine",
				"SteamTurbine2",
				"SolarPanel",
				"Wire",
				"WireBridge",
				"HighWattageWire",
				"WireBridgeHighWattage",
				"WireRefined",
				"WireRefinedBridge",
				"WireRefinedHighWattage",
				"WireRefinedBridgeHighWattage",
				"Battery",
				"BatteryMedium",
				"BatterySmart",
				"ElectrobankCharger",
				"SmallElectrobankDischarger",
				"LargeElectrobankDischarger",
				"PowerTransformerSmall",
				"PowerTransformer",
				SwitchConfig.ID,
				LogicPowerRelayConfig.ID,
				TemperatureControlledSwitchConfig.ID,
				PressureSwitchLiquidConfig.ID,
				PressureSwitchGasConfig.ID
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Food"), false, new List<string>
			{
				"MicrobeMusher",
				"CookingStation",
				"Deepfryer",
				"GourmetCookingStation",
				"SpiceGrinder",
				"FoodDehydrator",
				"FoodRehydrator",
				"PlanterBox",
				"FarmTile",
				"HydroponicFarm",
				"RationBox",
				"Refrigerator",
				"CreatureDeliveryPoint",
				"CritterPickUp",
				"CritterDropOff",
				"FishDeliveryPoint",
				"CreatureFeeder",
				"FishFeeder",
				"MilkFeeder",
				"EggIncubator",
				"EggCracker",
				"CreatureGroundTrap",
				"WaterTrap",
				"CreatureAirTrap",
				"CritterCondo",
				"UnderwaterCritterCondo",
				"AirBorneCritterCondo"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Plumbing"), false, new List<string>
			{
				"DevPumpLiquid",
				"Outhouse",
				"FlushToilet",
				"WallToilet",
				ShowerConfig.ID,
				"GunkEmptier",
				"LiquidPumpingStation",
				"BottleEmptier",
				"BottleEmptierConduitLiquid",
				"LiquidBottler",
				"LiquidConduit",
				"InsulatedLiquidConduit",
				"LiquidConduitRadiant",
				"LiquidConduitBridge",
				"LiquidConduitPreferentialFlow",
				"LiquidConduitOverflow",
				"LiquidPump",
				"LiquidMiniPump",
				"LiquidVent",
				"LiquidFilter",
				"LiquidValve",
				"LiquidLogicValve",
				"LiquidLimitValve",
				LiquidConduitElementSensorConfig.ID,
				LiquidConduitDiseaseSensorConfig.ID,
				LiquidConduitTemperatureSensorConfig.ID,
				"ModularLaunchpadPortLiquid",
				"ModularLaunchpadPortLiquidUnloader",
				"ContactConductivePipeBridge"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("HVAC"), false, new List<string>
			{
				"DevPumpGas",
				"GasConduit",
				"InsulatedGasConduit",
				"GasConduitRadiant",
				"GasConduitBridge",
				"GasConduitPreferentialFlow",
				"GasConduitOverflow",
				"GasPump",
				"GasMiniPump",
				"GasVent",
				"GasVentHighPressure",
				"GasFilter",
				"GasValve",
				"GasLogicValve",
				"GasLimitValve",
				"GasBottler",
				"BottleEmptierGas",
				"BottleEmptierConduitGas",
				"ModularLaunchpadPortGas",
				"ModularLaunchpadPortGasUnloader",
				GasConduitElementSensorConfig.ID,
				GasConduitDiseaseSensorConfig.ID,
				GasConduitTemperatureSensorConfig.ID
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Refining"), false, new List<string>
			{
				"Compost",
				"WaterPurifier",
				"Desalinator",
				"FertilizerMaker",
				"AlgaeDistillery",
				"EthanolDistillery",
				"RockCrusher",
				"Kiln",
				"SludgePress",
				"MetalRefinery",
				"GlassForge",
				"OilRefinery",
				"Polymerizer",
				"OxyliteRefinery",
				"Chlorinator",
				"SupermaterialRefinery",
				"DiamondPress",
				"MilkFatSeparator",
				"MilkPress"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Medical"), false, new List<string>
			{
				"DevLifeSupport",
				"WashBasin",
				"WashSink",
				"HandSanitizer",
				"DecontaminationShower",
				"OilChanger",
				"Apothecary",
				"DoctorStation",
				"AdvancedDoctorStation",
				"MedicalCot",
				"MassageTable",
				"Grave"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Furniture"), false, new List<string>
			{
				"Bed",
				"LuxuryBed",
				LadderBedConfig.ID,
				"FloorLamp",
				"CeilingLight",
				"SunLamp",
				"DevLightGenerator",
				"MercuryCeilingLight",
				"DiningTable",
				"WaterCooler",
				"Phonobox",
				"ArcadeMachine",
				"EspressoMachine",
				"HotTub",
				"MechanicalSurfboard",
				"Sauna",
				"Juicer",
				"SodaFountain",
				"BeachChair",
				"VerticalWindTunnel",
				PixelPackConfig.ID,
				"Telephone",
				"FlowerVase",
				"FlowerVaseWall",
				"FlowerVaseHanging",
				"FlowerVaseHangingFancy",
				"SmallSculpture",
				"Sculpture",
				"IceSculpture",
				"WoodSculpture",
				"MarbleSculpture",
				"MetalSculpture",
				"CrownMoulding",
				"CornerMoulding",
				"Canvas",
				"CanvasWide",
				"CanvasTall",
				"ItemPedestal",
				"MonumentBottom",
				"MonumentMiddle",
				"MonumentTop",
				"ParkSign"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Equipment"), false, new List<string>
			{
				"ResearchCenter",
				"AdvancedResearchCenter",
				"NuclearResearchCenter",
				"OrbitalResearchCenter",
				"CosmicResearchCenter",
				"DLC1CosmicResearchCenter",
				"Telescope",
				"GeoTuner",
				"DataMiner",
				"PowerControlStation",
				"FarmStation",
				"GeneticAnalysisStation",
				"RanchStation",
				"ShearingStation",
				"MilkingStation",
				"RoleStation",
				"ResetSkillsStation",
				"ArtifactAnalysisStation",
				RemoteWorkerDockConfig.ID,
				RemoteWorkTerminalConfig.ID,
				"MissileFabricator",
				"CraftingTable",
				"AdvancedCraftingTable",
				"ClothingFabricator",
				"ClothingAlterationStation",
				"SuitFabricator",
				"OxygenMaskMarker",
				"OxygenMaskLocker",
				"SuitMarker",
				"SuitLocker",
				"JetSuitMarker",
				"JetSuitLocker",
				"LeadSuitMarker",
				"LeadSuitLocker",
				"AstronautTrainingCenter"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Utilities"), true, new List<string>
			{
				"Campfire",
				"DevHeater",
				"IceKettle",
				"SpaceHeater",
				"LiquidHeater",
				"LiquidCooledFan",
				"IceCooledFan",
				"IceMachine",
				"AirConditioner",
				"LiquidConditioner",
				"OreScrubber",
				"OilWellCap",
				"ThermalBlock",
				"SweepBotStation"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Automation"), true, new List<string>
			{
				"LogicWire",
				"LogicWireBridge",
				"LogicRibbon",
				"LogicRibbonBridge",
				LogicSwitchConfig.ID,
				"LogicDuplicantSensor",
				LogicPressureSensorGasConfig.ID,
				LogicPressureSensorLiquidConfig.ID,
				LogicTemperatureSensorConfig.ID,
				LogicLightSensorConfig.ID,
				LogicWattageSensorConfig.ID,
				LogicTimeOfDaySensorConfig.ID,
				LogicTimerSensorConfig.ID,
				LogicDiseaseSensorConfig.ID,
				LogicElementSensorGasConfig.ID,
				LogicElementSensorLiquidConfig.ID,
				LogicCritterCountSensorConfig.ID,
				LogicRadiationSensorConfig.ID,
				LogicHEPSensorConfig.ID,
				LogicCounterConfig.ID,
				LogicAlarmConfig.ID,
				LogicHammerConfig.ID,
				"LogicInterasteroidSender",
				"LogicInterasteroidReceiver",
				LogicRibbonReaderConfig.ID,
				LogicRibbonWriterConfig.ID,
				"FloorSwitch",
				"Checkpoint",
				CometDetectorConfig.ID,
				"LogicGateNOT",
				"LogicGateAND",
				"LogicGateOR",
				"LogicGateBUFFER",
				"LogicGateFILTER",
				"LogicGateXOR",
				LogicMemoryConfig.ID,
				"LogicGateMultiplexer",
				"LogicGateDemultiplexer"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Conveyance"), true, new List<string>
			{
				"DevPumpSolid",
				"SolidTransferArm",
				"SolidConduit",
				"SolidConduitBridge",
				"SolidConduitInbox",
				"SolidConduitOutbox",
				"SolidFilter",
				"SolidVent",
				"SolidLogicValve",
				"SolidLimitValve",
				SolidConduitDiseaseSensorConfig.ID,
				SolidConduitElementSensorConfig.ID,
				SolidConduitTemperatureSensorConfig.ID,
				"AutoMiner",
				"ModularLaunchpadPortSolid",
				"ModularLaunchpadPortSolidUnloader"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Rocketry"), true, new List<string>
			{
				"ClusterTelescope",
				"ClusterTelescopeEnclosed",
				"MissionControl",
				"MissionControlCluster",
				"LaunchPad",
				"Gantry",
				"SteamEngine",
				"KeroseneEngine",
				"SolidBooster",
				"LiquidFuelTank",
				"OxidizerTank",
				"OxidizerTankLiquid",
				"CargoBay",
				"GasCargoBay",
				"LiquidCargoBay",
				"CommandModule",
				"RoboPilotCommandModule",
				"TouristModule",
				"ResearchModule",
				"SpecialCargoBay",
				"HydrogenEngine",
				RocketControlStationConfig.ID,
				"RocketInteriorPowerPlug",
				"RocketInteriorLiquidInput",
				"RocketInteriorLiquidOutput",
				"RocketInteriorGasInput",
				"RocketInteriorGasOutput",
				"RocketInteriorSolidInput",
				"RocketInteriorSolidOutput",
				LogicClusterLocationSensorConfig.ID,
				"RailGun",
				"RailGunPayloadOpener",
				"LandingBeacon",
				"MissileLauncher",
				"ModularLaunchpadPortBridge"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("HEP"), true, new List<string>
			{
				"RadiationLight",
				"ManualHighEnergyParticleSpawner",
				"NuclearReactor",
				"UraniumCentrifuge",
				"HighEnergyParticleSpawner",
				"DevHEPSpawner",
				"HighEnergyParticleRedirector",
				"HEPBattery",
				"HEPBridgeTile",
				"DevRadiationGenerator"
			}, "EXPANSION1_ID")
		};

		// Token: 0x04005703 RID: 22275
		public static List<Type> COMPONENT_DESCRIPTION_ORDER = new List<Type>
		{
			typeof(BottleEmptier),
			typeof(CookingStation),
			typeof(GourmetCookingStation),
			typeof(RoleStation),
			typeof(ResearchCenter),
			typeof(NuclearResearchCenter),
			typeof(LiquidCooledFan),
			typeof(HandSanitizer),
			typeof(HandSanitizer.Work),
			typeof(PlantAirConditioner),
			typeof(Clinic),
			typeof(BuildingElementEmitter),
			typeof(ElementConverter),
			typeof(ElementConsumer),
			typeof(PassiveElementConsumer),
			typeof(TinkerStation),
			typeof(EnergyConsumer),
			typeof(AirConditioner),
			typeof(Storage),
			typeof(Battery),
			typeof(AirFilter),
			typeof(FlushToilet),
			typeof(Toilet),
			typeof(EnergyGenerator),
			typeof(MassageTable),
			typeof(Shower),
			typeof(Ownable),
			typeof(PlantablePlot),
			typeof(RelaxationPoint),
			typeof(BuildingComplete),
			typeof(Building),
			typeof(BuildingPreview),
			typeof(BuildingUnderConstruction),
			typeof(Crop),
			typeof(Growing),
			typeof(Equippable),
			typeof(ColdBreather),
			typeof(ResearchPointObject),
			typeof(SuitTank),
			typeof(IlluminationVulnerable),
			typeof(TemperatureVulnerable),
			typeof(ExternalTemperatureMonitor),
			typeof(CritterTemperatureMonitor),
			typeof(PressureVulnerable),
			typeof(SubmersionMonitor),
			typeof(BatterySmart),
			typeof(Compost),
			typeof(Refrigerator),
			typeof(Bed),
			typeof(OreScrubber),
			typeof(OreScrubber.Work),
			typeof(MinimumOperatingTemperature),
			typeof(RoomTracker),
			typeof(EnergyConsumerSelfSustaining),
			typeof(ArcadeMachine),
			typeof(Telescope),
			typeof(EspressoMachine),
			typeof(JetSuitTank),
			typeof(Phonobox),
			typeof(ArcadeMachine),
			typeof(BeachChair),
			typeof(Sauna),
			typeof(VerticalWindTunnel),
			typeof(HotTub),
			typeof(Juicer),
			typeof(SodaFountain),
			typeof(MechanicalSurfboard),
			typeof(BottleEmptier),
			typeof(AccessControl),
			typeof(GammaRayOven),
			typeof(Reactor),
			typeof(HighEnergyParticlePort),
			typeof(LeadSuitTank),
			typeof(ActiveParticleConsumer.Def),
			typeof(WaterCooler),
			typeof(Edible),
			typeof(PlantableSeed),
			typeof(SicknessTrigger),
			typeof(MedicinalPill),
			typeof(SeedProducer),
			typeof(Geyser),
			typeof(SpaceHeater),
			typeof(Overheatable),
			typeof(CreatureCalorieMonitor.Def),
			typeof(LureableMonitor.Def),
			typeof(CropSleepingMonitor.Def),
			typeof(FertilizationMonitor.Def),
			typeof(IrrigationMonitor.Def),
			typeof(ScaleGrowthMonitor.Def),
			typeof(TravelTubeEntrance.Work),
			typeof(ToiletWorkableUse),
			typeof(ReceptacleMonitor),
			typeof(Light2D),
			typeof(Ladder),
			typeof(SimCellOccupier),
			typeof(Vent),
			typeof(LogicPorts),
			typeof(Capturable),
			typeof(Trappable),
			typeof(SpaceArtifact),
			typeof(MessStation),
			typeof(PlantElementEmitter),
			typeof(Radiator),
			typeof(DecorProvider)
		};

		// Token: 0x02001F9C RID: 8092
		public class PHARMACY
		{
			// Token: 0x02002667 RID: 9831
			public class FABRICATIONTIME
			{
				// Token: 0x0400AA89 RID: 43657
				public const float TIER0 = 50f;

				// Token: 0x0400AA8A RID: 43658
				public const float TIER1 = 100f;

				// Token: 0x0400AA8B RID: 43659
				public const float TIER2 = 200f;
			}
		}

		// Token: 0x02001F9D RID: 8093
		public class NUCLEAR_REACTOR
		{
			// Token: 0x02002668 RID: 9832
			public class REACTOR_MASSES
			{
				// Token: 0x0400AA8C RID: 43660
				public const float MIN = 1f;

				// Token: 0x0400AA8D RID: 43661
				public const float MAX = 10f;
			}
		}

		// Token: 0x02001F9E RID: 8094
		public class OVERPRESSURE
		{
			// Token: 0x04008F1B RID: 36635
			public const float TIER0 = 1.8f;
		}

		// Token: 0x02001F9F RID: 8095
		public class OVERHEAT_TEMPERATURES
		{
			// Token: 0x04008F1C RID: 36636
			public const float LOW_3 = 10f;

			// Token: 0x04008F1D RID: 36637
			public const float LOW_2 = 328.15f;

			// Token: 0x04008F1E RID: 36638
			public const float LOW_1 = 338.15f;

			// Token: 0x04008F1F RID: 36639
			public const float NORMAL = 348.15f;

			// Token: 0x04008F20 RID: 36640
			public const float HIGH_1 = 363.15f;

			// Token: 0x04008F21 RID: 36641
			public const float HIGH_2 = 398.15f;

			// Token: 0x04008F22 RID: 36642
			public const float HIGH_3 = 1273.15f;

			// Token: 0x04008F23 RID: 36643
			public const float HIGH_4 = 2273.15f;
		}

		// Token: 0x02001FA0 RID: 8096
		public class OVERHEAT_MATERIAL_MOD
		{
			// Token: 0x04008F24 RID: 36644
			public const float LOW_3 = -200f;

			// Token: 0x04008F25 RID: 36645
			public const float LOW_2 = -20f;

			// Token: 0x04008F26 RID: 36646
			public const float LOW_1 = -10f;

			// Token: 0x04008F27 RID: 36647
			public const float NORMAL = 0f;

			// Token: 0x04008F28 RID: 36648
			public const float HIGH_1 = 15f;

			// Token: 0x04008F29 RID: 36649
			public const float HIGH_2 = 50f;

			// Token: 0x04008F2A RID: 36650
			public const float HIGH_3 = 200f;

			// Token: 0x04008F2B RID: 36651
			public const float HIGH_4 = 500f;

			// Token: 0x04008F2C RID: 36652
			public const float HIGH_5 = 900f;
		}

		// Token: 0x02001FA1 RID: 8097
		public class DECOR_MATERIAL_MOD
		{
			// Token: 0x04008F2D RID: 36653
			public const float NORMAL = 0f;

			// Token: 0x04008F2E RID: 36654
			public const float HIGH_1 = 0.1f;

			// Token: 0x04008F2F RID: 36655
			public const float HIGH_2 = 0.2f;

			// Token: 0x04008F30 RID: 36656
			public const float HIGH_3 = 0.5f;

			// Token: 0x04008F31 RID: 36657
			public const float HIGH_4 = 1f;
		}

		// Token: 0x02001FA2 RID: 8098
		public class CONSTRUCTION_MASS_KG
		{
			// Token: 0x04008F32 RID: 36658
			public static readonly float[] TIER_TINY = new float[]
			{
				5f
			};

			// Token: 0x04008F33 RID: 36659
			public static readonly float[] TIER0 = new float[]
			{
				25f
			};

			// Token: 0x04008F34 RID: 36660
			public static readonly float[] TIER1 = new float[]
			{
				50f
			};

			// Token: 0x04008F35 RID: 36661
			public static readonly float[] TIER2 = new float[]
			{
				100f
			};

			// Token: 0x04008F36 RID: 36662
			public static readonly float[] TIER3 = new float[]
			{
				200f
			};

			// Token: 0x04008F37 RID: 36663
			public static readonly float[] TIER4 = new float[]
			{
				400f
			};

			// Token: 0x04008F38 RID: 36664
			public static readonly float[] TIER5 = new float[]
			{
				800f
			};

			// Token: 0x04008F39 RID: 36665
			public static readonly float[] TIER6 = new float[]
			{
				1200f
			};

			// Token: 0x04008F3A RID: 36666
			public static readonly float[] TIER7 = new float[]
			{
				2000f
			};
		}

		// Token: 0x02001FA3 RID: 8099
		public class ROCKETRY_MASS_KG
		{
			// Token: 0x04008F3B RID: 36667
			public static float[] COMMAND_MODULE_MASS = new float[]
			{
				200f
			};

			// Token: 0x04008F3C RID: 36668
			public static float[] CARGO_MASS = new float[]
			{
				1000f
			};

			// Token: 0x04008F3D RID: 36669
			public static float[] CARGO_MASS_SMALL = new float[]
			{
				400f
			};

			// Token: 0x04008F3E RID: 36670
			public static float[] FUEL_TANK_DRY_MASS = new float[]
			{
				100f
			};

			// Token: 0x04008F3F RID: 36671
			public static float[] FUEL_TANK_WET_MASS = new float[]
			{
				900f
			};

			// Token: 0x04008F40 RID: 36672
			public static float[] FUEL_TANK_WET_MASS_SMALL = new float[]
			{
				300f
			};

			// Token: 0x04008F41 RID: 36673
			public static float[] FUEL_TANK_WET_MASS_GAS = new float[]
			{
				100f
			};

			// Token: 0x04008F42 RID: 36674
			public static float[] FUEL_TANK_WET_MASS_GAS_LARGE = new float[]
			{
				150f
			};

			// Token: 0x04008F43 RID: 36675
			public static float[] OXIDIZER_TANK_OXIDIZER_MASS = new float[]
			{
				900f
			};

			// Token: 0x04008F44 RID: 36676
			public static float[] ENGINE_MASS_SMALL = new float[]
			{
				200f
			};

			// Token: 0x04008F45 RID: 36677
			public static float[] ENGINE_MASS_LARGE = new float[]
			{
				500f
			};

			// Token: 0x04008F46 RID: 36678
			public static float[] NOSE_CONE_TIER1 = new float[]
			{
				200f,
				100f
			};

			// Token: 0x04008F47 RID: 36679
			public static float[] NOSE_CONE_TIER2 = new float[]
			{
				400f,
				200f
			};

			// Token: 0x04008F48 RID: 36680
			public static float[] HOLLOW_TIER1 = new float[]
			{
				200f
			};

			// Token: 0x04008F49 RID: 36681
			public static float[] HOLLOW_TIER2 = new float[]
			{
				400f
			};

			// Token: 0x04008F4A RID: 36682
			public static float[] HOLLOW_TIER3 = new float[]
			{
				800f
			};

			// Token: 0x04008F4B RID: 36683
			public static float[] DENSE_TIER0 = new float[]
			{
				200f
			};

			// Token: 0x04008F4C RID: 36684
			public static float[] DENSE_TIER1 = new float[]
			{
				500f
			};

			// Token: 0x04008F4D RID: 36685
			public static float[] DENSE_TIER2 = new float[]
			{
				1000f
			};

			// Token: 0x04008F4E RID: 36686
			public static float[] DENSE_TIER3 = new float[]
			{
				2000f
			};
		}

		// Token: 0x02001FA4 RID: 8100
		public class ENERGY_CONSUMPTION_WHEN_ACTIVE
		{
			// Token: 0x04008F4F RID: 36687
			public const float TIER0 = 0f;

			// Token: 0x04008F50 RID: 36688
			public const float TIER1 = 5f;

			// Token: 0x04008F51 RID: 36689
			public const float TIER2 = 60f;

			// Token: 0x04008F52 RID: 36690
			public const float TIER3 = 120f;

			// Token: 0x04008F53 RID: 36691
			public const float TIER4 = 240f;

			// Token: 0x04008F54 RID: 36692
			public const float TIER5 = 480f;

			// Token: 0x04008F55 RID: 36693
			public const float TIER6 = 960f;

			// Token: 0x04008F56 RID: 36694
			public const float TIER7 = 1200f;

			// Token: 0x04008F57 RID: 36695
			public const float TIER8 = 1600f;
		}

		// Token: 0x02001FA5 RID: 8101
		public class EXHAUST_ENERGY_ACTIVE
		{
			// Token: 0x04008F58 RID: 36696
			public const float TIER0 = 0f;

			// Token: 0x04008F59 RID: 36697
			public const float TIER1 = 0.125f;

			// Token: 0x04008F5A RID: 36698
			public const float TIER2 = 0.25f;

			// Token: 0x04008F5B RID: 36699
			public const float TIER3 = 0.5f;

			// Token: 0x04008F5C RID: 36700
			public const float TIER4 = 1f;

			// Token: 0x04008F5D RID: 36701
			public const float TIER5 = 2f;

			// Token: 0x04008F5E RID: 36702
			public const float TIER6 = 4f;

			// Token: 0x04008F5F RID: 36703
			public const float TIER7 = 8f;

			// Token: 0x04008F60 RID: 36704
			public const float TIER8 = 16f;
		}

		// Token: 0x02001FA6 RID: 8102
		public class JOULES_LEAK_PER_CYCLE
		{
			// Token: 0x04008F61 RID: 36705
			public const float TIER0 = 400f;

			// Token: 0x04008F62 RID: 36706
			public const float TIER1 = 1000f;

			// Token: 0x04008F63 RID: 36707
			public const float TIER2 = 2000f;
		}

		// Token: 0x02001FA7 RID: 8103
		public class SELF_HEAT_KILOWATTS
		{
			// Token: 0x04008F64 RID: 36708
			public const float TIER0 = 0f;

			// Token: 0x04008F65 RID: 36709
			public const float TIER1 = 0.5f;

			// Token: 0x04008F66 RID: 36710
			public const float TIER2 = 1f;

			// Token: 0x04008F67 RID: 36711
			public const float TIER3 = 2f;

			// Token: 0x04008F68 RID: 36712
			public const float TIER4 = 4f;

			// Token: 0x04008F69 RID: 36713
			public const float TIER5 = 8f;

			// Token: 0x04008F6A RID: 36714
			public const float TIER6 = 16f;

			// Token: 0x04008F6B RID: 36715
			public const float TIER7 = 32f;

			// Token: 0x04008F6C RID: 36716
			public const float TIER8 = 64f;

			// Token: 0x04008F6D RID: 36717
			public const float TIER_NUCLEAR = 16384f;
		}

		// Token: 0x02001FA8 RID: 8104
		public class MELTING_POINT_KELVIN
		{
			// Token: 0x04008F6E RID: 36718
			public const float TIER0 = 800f;

			// Token: 0x04008F6F RID: 36719
			public const float TIER1 = 1600f;

			// Token: 0x04008F70 RID: 36720
			public const float TIER2 = 2400f;

			// Token: 0x04008F71 RID: 36721
			public const float TIER3 = 3200f;

			// Token: 0x04008F72 RID: 36722
			public const float TIER4 = 9999f;
		}

		// Token: 0x02001FA9 RID: 8105
		public class CONSTRUCTION_TIME_SECONDS
		{
			// Token: 0x04008F73 RID: 36723
			public const float TIER0 = 3f;

			// Token: 0x04008F74 RID: 36724
			public const float TIER1 = 10f;

			// Token: 0x04008F75 RID: 36725
			public const float TIER2 = 30f;

			// Token: 0x04008F76 RID: 36726
			public const float TIER3 = 60f;

			// Token: 0x04008F77 RID: 36727
			public const float TIER4 = 120f;

			// Token: 0x04008F78 RID: 36728
			public const float TIER5 = 240f;

			// Token: 0x04008F79 RID: 36729
			public const float TIER6 = 480f;
		}

		// Token: 0x02001FAA RID: 8106
		public class HITPOINTS
		{
			// Token: 0x04008F7A RID: 36730
			public const int TIER0 = 10;

			// Token: 0x04008F7B RID: 36731
			public const int TIER1 = 30;

			// Token: 0x04008F7C RID: 36732
			public const int TIER2 = 100;

			// Token: 0x04008F7D RID: 36733
			public const int TIER3 = 250;

			// Token: 0x04008F7E RID: 36734
			public const int TIER4 = 1000;
		}

		// Token: 0x02001FAB RID: 8107
		public class DAMAGE_SOURCES
		{
			// Token: 0x04008F7F RID: 36735
			public const int CONDUIT_CONTENTS_BOILED = 1;

			// Token: 0x04008F80 RID: 36736
			public const int CONDUIT_CONTENTS_FROZE = 1;

			// Token: 0x04008F81 RID: 36737
			public const int BAD_INPUT_ELEMENT = 1;

			// Token: 0x04008F82 RID: 36738
			public const int BUILDING_OVERHEATED = 1;

			// Token: 0x04008F83 RID: 36739
			public const int HIGH_LIQUID_PRESSURE = 10;

			// Token: 0x04008F84 RID: 36740
			public const int MICROMETEORITE = 1;

			// Token: 0x04008F85 RID: 36741
			public const int CORROSIVE_ELEMENT = 1;
		}

		// Token: 0x02001FAC RID: 8108
		public class RELOCATION_TIME_SECONDS
		{
			// Token: 0x04008F86 RID: 36742
			public const float DECONSTRUCT = 4f;

			// Token: 0x04008F87 RID: 36743
			public const float CONSTRUCT = 4f;
		}

		// Token: 0x02001FAD RID: 8109
		public class WORK_TIME_SECONDS
		{
			// Token: 0x04008F88 RID: 36744
			public const float VERYSHORT_WORK_TIME = 5f;

			// Token: 0x04008F89 RID: 36745
			public const float SHORT_WORK_TIME = 15f;

			// Token: 0x04008F8A RID: 36746
			public const float MEDIUM_WORK_TIME = 30f;

			// Token: 0x04008F8B RID: 36747
			public const float LONG_WORK_TIME = 90f;

			// Token: 0x04008F8C RID: 36748
			public const float VERY_LONG_WORK_TIME = 150f;

			// Token: 0x04008F8D RID: 36749
			public const float EXTENSIVE_WORK_TIME = 180f;
		}

		// Token: 0x02001FAE RID: 8110
		public class FABRICATION_TIME_SECONDS
		{
			// Token: 0x04008F8E RID: 36750
			public const float VERY_SHORT = 20f;

			// Token: 0x04008F8F RID: 36751
			public const float SHORT = 40f;

			// Token: 0x04008F90 RID: 36752
			public const float MODERATE = 80f;

			// Token: 0x04008F91 RID: 36753
			public const float LONG = 250f;
		}

		// Token: 0x02001FAF RID: 8111
		public class DECOR
		{
			// Token: 0x04008F92 RID: 36754
			public static readonly EffectorValues NONE = new EffectorValues
			{
				amount = 0,
				radius = 1
			};

			// Token: 0x02002669 RID: 9833
			public class BONUS
			{
				// Token: 0x0400AA8E RID: 43662
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = 5,
					radius = 1
				};

				// Token: 0x0400AA8F RID: 43663
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = 10,
					radius = 2
				};

				// Token: 0x0400AA90 RID: 43664
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = 15,
					radius = 3
				};

				// Token: 0x0400AA91 RID: 43665
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = 20,
					radius = 4
				};

				// Token: 0x0400AA92 RID: 43666
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = 25,
					radius = 5
				};

				// Token: 0x0400AA93 RID: 43667
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = 30,
					radius = 6
				};

				// Token: 0x02003546 RID: 13638
				public class MONUMENT
				{
					// Token: 0x0400D7D8 RID: 55256
					public static readonly EffectorValues COMPLETE = new EffectorValues
					{
						amount = 40,
						radius = 10
					};

					// Token: 0x0400D7D9 RID: 55257
					public static readonly EffectorValues INCOMPLETE = new EffectorValues
					{
						amount = 10,
						radius = 5
					};
				}
			}

			// Token: 0x0200266A RID: 9834
			public class PENALTY
			{
				// Token: 0x0400AA94 RID: 43668
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = -5,
					radius = 1
				};

				// Token: 0x0400AA95 RID: 43669
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = -10,
					radius = 2
				};

				// Token: 0x0400AA96 RID: 43670
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = -15,
					radius = 3
				};

				// Token: 0x0400AA97 RID: 43671
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = -20,
					radius = 4
				};

				// Token: 0x0400AA98 RID: 43672
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = -20,
					radius = 5
				};

				// Token: 0x0400AA99 RID: 43673
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = -25,
					radius = 6
				};
			}
		}

		// Token: 0x02001FB0 RID: 8112
		public class MASS_KG
		{
			// Token: 0x04008F93 RID: 36755
			public const float TIER0 = 25f;

			// Token: 0x04008F94 RID: 36756
			public const float TIER1 = 50f;

			// Token: 0x04008F95 RID: 36757
			public const float TIER2 = 100f;

			// Token: 0x04008F96 RID: 36758
			public const float TIER3 = 200f;

			// Token: 0x04008F97 RID: 36759
			public const float TIER4 = 400f;

			// Token: 0x04008F98 RID: 36760
			public const float TIER5 = 800f;

			// Token: 0x04008F99 RID: 36761
			public const float TIER6 = 1200f;

			// Token: 0x04008F9A RID: 36762
			public const float TIER7 = 2000f;
		}

		// Token: 0x02001FB1 RID: 8113
		public class UPGRADES
		{
			// Token: 0x04008F9B RID: 36763
			public const float BUILDTIME_TIER0 = 120f;

			// Token: 0x0200266B RID: 9835
			public class MATERIALTAGS
			{
				// Token: 0x0400AA9A RID: 43674
				public const string METAL = "Metal";

				// Token: 0x0400AA9B RID: 43675
				public const string REFINEDMETAL = "RefinedMetal";

				// Token: 0x0400AA9C RID: 43676
				public const string CARBON = "Carbon";
			}

			// Token: 0x0200266C RID: 9836
			public class MATERIALMASS
			{
				// Token: 0x0400AA9D RID: 43677
				public const int TIER0 = 100;

				// Token: 0x0400AA9E RID: 43678
				public const int TIER1 = 200;

				// Token: 0x0400AA9F RID: 43679
				public const int TIER2 = 400;

				// Token: 0x0400AAA0 RID: 43680
				public const int TIER3 = 500;
			}

			// Token: 0x0200266D RID: 9837
			public class MODIFIERAMOUNTS
			{
				// Token: 0x0400AAA1 RID: 43681
				public const float MANUALGENERATOR_ENERGYGENERATION = 1.2f;

				// Token: 0x0400AAA2 RID: 43682
				public const float MANUALGENERATOR_CAPACITY = 2f;

				// Token: 0x0400AAA3 RID: 43683
				public const float PROPANEGENERATOR_ENERGYGENERATION = 1.6f;

				// Token: 0x0400AAA4 RID: 43684
				public const float PROPANEGENERATOR_HEATGENERATION = 1.6f;

				// Token: 0x0400AAA5 RID: 43685
				public const float GENERATOR_HEATGENERATION = 0.8f;

				// Token: 0x0400AAA6 RID: 43686
				public const float GENERATOR_ENERGYGENERATION = 1.3f;

				// Token: 0x0400AAA7 RID: 43687
				public const float TURBINE_ENERGYGENERATION = 1.2f;

				// Token: 0x0400AAA8 RID: 43688
				public const float TURBINE_CAPACITY = 1.2f;

				// Token: 0x0400AAA9 RID: 43689
				public const float SUITRECHARGER_EXECUTIONTIME = 1.2f;

				// Token: 0x0400AAAA RID: 43690
				public const float SUITRECHARGER_HEATGENERATION = 1.2f;

				// Token: 0x0400AAAB RID: 43691
				public const float STORAGELOCKER_CAPACITY = 2f;

				// Token: 0x0400AAAC RID: 43692
				public const float SOLARPANEL_ENERGYGENERATION = 1.2f;

				// Token: 0x0400AAAD RID: 43693
				public const float SMELTER_HEATGENERATION = 0.7f;
			}
		}
	}
}

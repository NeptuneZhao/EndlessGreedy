using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using FMOD.Studio;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using ProcGenGame;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Token: 0x020008BD RID: 2237
[AddComponentMenu("KMonoBehaviour/scripts/Game")]
public class Game : KMonoBehaviour
{
	// Token: 0x06003EAC RID: 16044 RVA: 0x0015B0A4 File Offset: 0x001592A4
	public static bool IsOnMainThread()
	{
		return Game.MainThread == Thread.CurrentThread;
	}

	// Token: 0x06003EAD RID: 16045 RVA: 0x0015B0B2 File Offset: 0x001592B2
	public static bool IsQuitting()
	{
		return Game.quitting;
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06003EAE RID: 16046 RVA: 0x0015B0B9 File Offset: 0x001592B9
	// (set) Token: 0x06003EAF RID: 16047 RVA: 0x0015B0C1 File Offset: 0x001592C1
	public KInputHandler inputHandler { get; set; }

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06003EB0 RID: 16048 RVA: 0x0015B0CA File Offset: 0x001592CA
	// (set) Token: 0x06003EB1 RID: 16049 RVA: 0x0015B0D1 File Offset: 0x001592D1
	public static Game Instance { get; private set; }

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x0015B0D9 File Offset: 0x001592D9
	public static Camera MainCamera
	{
		get
		{
			if (Game.m_CachedCamera == null)
			{
				Game.m_CachedCamera = Camera.main;
			}
			return Game.m_CachedCamera;
		}
	}

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06003EB3 RID: 16051 RVA: 0x0015B0F7 File Offset: 0x001592F7
	// (set) Token: 0x06003EB4 RID: 16052 RVA: 0x0015B118 File Offset: 0x00159318
	public bool SaveToCloudActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, value2);
		}
	}

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06003EB5 RID: 16053 RVA: 0x0015B145 File Offset: 0x00159345
	// (set) Token: 0x06003EB6 RID: 16054 RVA: 0x0015B168 File Offset: 0x00159368
	public bool FastWorkersModeActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.FastWorkersMode).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.FastWorkersMode, value2);
		}
	}

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06003EB7 RID: 16055 RVA: 0x0015B195 File Offset: 0x00159395
	// (set) Token: 0x06003EB8 RID: 16056 RVA: 0x0015B1A0 File Offset: 0x001593A0
	public bool SandboxModeActive
	{
		get
		{
			return this.sandboxModeActive;
		}
		set
		{
			this.sandboxModeActive = value;
			base.Trigger(-1948169901, null);
			if (PlanScreen.Instance != null)
			{
				PlanScreen.Instance.Refresh();
			}
			if (BuildMenu.Instance != null)
			{
				BuildMenu.Instance.Refresh();
			}
			if (OverlayMenu.Instance != null)
			{
				OverlayMenu.Instance.Refresh();
			}
			if (ManagementMenu.Instance != null)
			{
				ManagementMenu.Instance.Refresh();
			}
		}
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x0015B21C File Offset: 0x0015941C
	public bool DebugOnlyBuildingsAllowed
	{
		get
		{
			return DebugHandler.enabled && (this.SandboxModeActive || DebugHandler.InstantBuildMode);
		}
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06003EBA RID: 16058 RVA: 0x0015B236 File Offset: 0x00159436
	// (set) Token: 0x06003EBB RID: 16059 RVA: 0x0015B23E File Offset: 0x0015943E
	public StatusItemRenderer statusItemRenderer { get; private set; }

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06003EBC RID: 16060 RVA: 0x0015B247 File Offset: 0x00159447
	// (set) Token: 0x06003EBD RID: 16061 RVA: 0x0015B24F File Offset: 0x0015944F
	public PrioritizableRenderer prioritizableRenderer { get; private set; }

	// Token: 0x06003EBE RID: 16062 RVA: 0x0015B258 File Offset: 0x00159458
	protected override void OnPrefabInit()
	{
		DebugUtil.LogArgs(new object[]
		{
			Time.realtimeSinceStartup,
			"Level Loaded....",
			SceneManager.GetActiveScene().name
		});
		Components.EntityCellVisualizers.OnAdd += this.OnAddBuildingCellVisualizer;
		Components.EntityCellVisualizers.OnRemove += this.OnRemoveBuildingCellVisualizer;
		Singleton<KBatchedAnimUpdater>.CreateInstance();
		Singleton<CellChangeMonitor>.CreateInstance();
		this.userMenu = new UserMenu();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.StopBE));
		Game.Instance = this;
		this.statusItemRenderer = new StatusItemRenderer();
		this.prioritizableRenderer = new PrioritizableRenderer();
		this.LoadEventHashes();
		this.savedInfo.InitializeEmptyVariables();
		this.gasFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.GasConduits) - 0.4f);
		this.liquidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.LiquidConduits) - 0.4f);
		this.solidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.SolidConduitContents) - 0.4f);
		Shader.WarmupAllShaders();
		Db.Get();
		Game.quitting = false;
		Game.PickupableLayer = LayerMask.NameToLayer("Pickupable");
		Game.BlockSelectionLayerMask = LayerMask.GetMask(new string[]
		{
			"BlockSelection"
		});
		this.world = World.Instance;
		KPrefabID.NextUniqueID = KPlayerPrefs.GetInt(Game.NextUniqueIDKey, 0);
		this.circuitManager = new CircuitManager();
		this.energySim = new EnergySim();
		this.gasConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 13);
		this.liquidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 17);
		this.electricalConduitSystem = new UtilityNetworkManager<ElectricalUtilityNetwork, Wire>(Grid.WidthInCells, Grid.HeightInCells, 27);
		this.logicCircuitSystem = new UtilityNetworkManager<LogicCircuitNetwork, LogicWire>(Grid.WidthInCells, Grid.HeightInCells, 32);
		this.logicCircuitManager = new LogicCircuitManager(this.logicCircuitSystem);
		this.travelTubeSystem = new UtilityNetworkTubesManager(Grid.WidthInCells, Grid.HeightInCells, 35);
		this.solidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, SolidConduit>(Grid.WidthInCells, Grid.HeightInCells, 21);
		this.conduitTemperatureManager = new ConduitTemperatureManager();
		this.conduitDiseaseManager = new ConduitDiseaseManager(this.conduitTemperatureManager);
		this.gasConduitFlow = new ConduitFlow(ConduitType.Gas, Grid.CellCount, this.gasConduitSystem, 1f, 0.25f);
		this.liquidConduitFlow = new ConduitFlow(ConduitType.Liquid, Grid.CellCount, this.liquidConduitSystem, 10f, 0.75f);
		this.solidConduitFlow = new SolidConduitFlow(Grid.CellCount, this.solidConduitSystem, 0.75f);
		this.gasFlowVisualizer = new ConduitFlowVisualizer(this.gasConduitFlow, this.gasConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundGas, Lighting.Instance.Settings.GasConduit);
		this.liquidFlowVisualizer = new ConduitFlowVisualizer(this.liquidConduitFlow, this.liquidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundLiquid, Lighting.Instance.Settings.LiquidConduit);
		this.solidFlowVisualizer = new SolidConduitFlowVisualizer(this.solidConduitFlow, this.solidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundSolid, Lighting.Instance.Settings.SolidConduit);
		this.accumulators = new Accumulators();
		this.plantElementAbsorbers = new PlantElementAbsorbers();
		this.activeFX = new ushort[Grid.CellCount];
		this.UnsafePrefabInit();
		Shader.SetGlobalVector("_MetalParameters", new Vector4(0f, 0f, 0f, 0f));
		Shader.SetGlobalVector("_WaterParameters", new Vector4(0f, 0f, 0f, 0f));
		this.InitializeFXSpawners();
		PathFinder.Initialize();
		new GameNavGrids(Pathfinding.Instance);
		this.screenMgr = global::Util.KInstantiate(this.screenManagerPrefab, null, null).GetComponent<GameScreenManager>();
		this.roomProber = new RoomProber();
		this.spaceScannerNetworkManager = new SpaceScannerNetworkManager();
		this.fetchManager = base.gameObject.AddComponent<FetchManager>();
		this.ediblesManager = base.gameObject.AddComponent<EdiblesManager>();
		Singleton<CellChangeMonitor>.Instance.SetGridSize(Grid.WidthInCells, Grid.HeightInCells);
		this.unlocks = base.GetComponent<Unlocks>();
		this.changelistsPlayedOn = new List<uint>();
		this.changelistsPlayedOn.Add(642695U);
		this.dateGenerated = System.DateTime.UtcNow.ToString("U", CultureInfo.InvariantCulture);
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x0015B6D2 File Offset: 0x001598D2
	public void SetGameStarted()
	{
		this.gameStarted = true;
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x0015B6DB File Offset: 0x001598DB
	public bool GameStarted()
	{
		return this.gameStarted;
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x0015B6E3 File Offset: 0x001598E3
	private void UnsafePrefabInit()
	{
		this.StepTheSim(0f);
	}

	// Token: 0x06003EC2 RID: 16066 RVA: 0x0015B6F1 File Offset: 0x001598F1
	protected override void OnLoadLevel()
	{
		base.Unsubscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate, false);
		base.Unsubscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate, false);
		base.OnLoadLevel();
	}

	// Token: 0x06003EC3 RID: 16067 RVA: 0x0015B71B File Offset: 0x0015991B
	private void MarkStatusItemRendererDirty(object data)
	{
		this.statusItemRenderer.MarkAllDirty();
	}

	// Token: 0x06003EC4 RID: 16068 RVA: 0x0015B728 File Offset: 0x00159928
	protected override void OnForcedCleanUp()
	{
		if (this.prioritizableRenderer != null)
		{
			this.prioritizableRenderer.Cleanup();
			this.prioritizableRenderer = null;
		}
		if (this.statusItemRenderer != null)
		{
			this.statusItemRenderer.Destroy();
			this.statusItemRenderer = null;
		}
		if (this.conduitTemperatureManager != null)
		{
			this.conduitTemperatureManager.Shutdown();
		}
		this.gasFlowVisualizer.FreeResources();
		this.liquidFlowVisualizer.FreeResources();
		this.solidFlowVisualizer.FreeResources();
		LightGridManager.Shutdown();
		RadiationGridManager.Shutdown();
		App.OnPreLoadScene = (System.Action)Delegate.Remove(App.OnPreLoadScene, new System.Action(this.StopBE));
		base.OnForcedCleanUp();
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x0015B7D0 File Offset: 0x001599D0
	protected override void OnSpawn()
	{
		global::Debug.Log("-- GAME --");
		Game.BrainScheduler = base.GetComponent<BrainScheduler>();
		PropertyTextures.FogOfWarScale = 0f;
		if (CameraController.Instance != null)
		{
			CameraController.Instance.EnableFreeCamera(false);
		}
		this.LocalPlayer = this.SpawnPlayer();
		WaterCubes.Instance.Init();
		SpeedControlScreen.Instance.Pause(false, false);
		LightGridManager.Initialise();
		RadiationGridManager.Initialise();
		this.RefreshRadiationLoop();
		this.UnsafeOnSpawn();
		Time.timeScale = 0f;
		if (this.tempIntroScreenPrefab != null)
		{
			global::Util.KInstantiate(this.tempIntroScreenPrefab, null, null);
		}
		if (SaveLoader.Instance.Cluster != null)
		{
			foreach (WorldGen worldGen in SaveLoader.Instance.Cluster.worlds)
			{
				this.Reset(worldGen.data.gameSpawnData, worldGen.WorldOffset);
			}
			NewBaseScreen.SetInitialCamera();
		}
		TagManager.FillMissingProperNames();
		CameraController.Instance.OrthographicSize = 20f;
		if (SaveLoader.Instance.loadedFromSave)
		{
			this.baseAlreadyCreated = true;
			base.Trigger(-1992507039, null);
			base.Trigger(-838649377, null);
		}
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
		for (int i = 0; i < array.Length; i++)
		{
			((MeshRenderer)array[i]).reflectionProbeUsage = ReflectionProbeUsage.Off;
		}
		base.Subscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate);
		base.Subscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate);
		this.solidConduitFlow.Initialize();
		SimAndRenderScheduler.instance.Add(this.roomProber, false);
		SimAndRenderScheduler.instance.Add(this.spaceScannerNetworkManager, false);
		SimAndRenderScheduler.instance.Add(KComponentSpawn.instance, false);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(new UpdateBucketWithUpdater<ISim200ms>.BatchUpdateDelegate(AmountInstance.BatchUpdate));
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(new UpdateBucketWithUpdater<ISim1000ms>.BatchUpdateDelegate(SolidTransferArm.BatchUpdate));
		if (!SaveLoader.Instance.loadedFromSave)
		{
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.SandboxMode.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SandboxMode);
			SaveGame.Instance.sandboxEnabled = !settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}
		this.mingleCellTracker = base.gameObject.AddComponent<MingleCellTracker>();
		if (Global.Instance != null)
		{
			Global.Instance.GetComponent<PerformanceMonitor>().Reset();
			Global.Instance.modManager.NotifyDialog(UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.TITLE, UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.MESSAGE, Global.Instance.globalCanvas);
		}
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x0015BA88 File Offset: 0x00159C88
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SimAndRenderScheduler.instance.Remove(KComponentSpawn.instance);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(null);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(null);
		this.DestroyInstances();
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x0015BABB File Offset: 0x00159CBB
	private new void OnDestroy()
	{
		base.OnDestroy();
		this.DestroyInstances();
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x0015BAC9 File Offset: 0x00159CC9
	private void UnsafeOnSpawn()
	{
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, 0, null, 0, null);
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0015BAE8 File Offset: 0x00159CE8
	private void RefreshRadiationLoop()
	{
		GameScheduler.Instance.Schedule("UpdateRadiation", 1f, delegate(object obj)
		{
			RadiationGridManager.Refresh();
			this.RefreshRadiationLoop();
		}, null, null);
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0015BB0D File Offset: 0x00159D0D
	public void SetMusicEnabled(bool enabled)
	{
		if (enabled)
		{
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			return;
		}
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x0015BB34 File Offset: 0x00159D34
	private Player SpawnPlayer()
	{
		Player component = global::Util.KInstantiate(this.playerPrefab, base.gameObject, null).GetComponent<Player>();
		component.ScreenManager = this.screenMgr;
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HudScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HoverTextScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.ToolTipScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		this.cameraController = global::Util.KInstantiate(this.cameraControllerPrefab, null, null).GetComponent<CameraController>();
		component.CameraController = this.cameraController;
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.cameraController, 1);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.cameraController, 1);
		}
		Global.GetInputManager().usedMenus.Add(this.cameraController);
		this.playerController = component.GetComponent<PlayerController>();
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.playerController, 20);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.playerController, 20);
		}
		Global.GetInputManager().usedMenus.Add(this.playerController);
		return component;
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x0015BC79 File Offset: 0x00159E79
	public void SetDupePassableSolid(int cell, bool passable, bool solid)
	{
		Grid.DupePassable[cell] = passable;
		this.gameSolidInfo.Add(new SolidInfo(cell, solid));
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x0015BC9C File Offset: 0x00159E9C
	private unsafe Sim.GameDataUpdate* StepTheSim(float dt)
	{
		Sim.GameDataUpdate* result;
		using (new KProfiler.Region("StepTheSim", null))
		{
			IntPtr intPtr = IntPtr.Zero;
			using (new KProfiler.Region("WaitingForSim", null))
			{
				if (Grid.Visible == null || Grid.Visible.Length == 0)
				{
					global::Debug.LogError("Invalid Grid.Visible, what have you done?!");
					return null;
				}
				intPtr = Sim.HandleMessage(SimMessageHashes.PrepareGameData, Grid.Visible.Length, Grid.Visible);
			}
			if (intPtr == IntPtr.Zero)
			{
				result = null;
			}
			else
			{
				Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)intPtr);
				Grid.elementIdx = ptr->elementIdx;
				Grid.temperature = ptr->temperature;
				Grid.mass = ptr->mass;
				Grid.radiation = ptr->radiation;
				Grid.properties = ptr->properties;
				Grid.strengthInfo = ptr->strengthInfo;
				Grid.insulation = ptr->insulation;
				Grid.diseaseIdx = ptr->diseaseIdx;
				Grid.diseaseCount = ptr->diseaseCount;
				Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
				Grid.exposedToSunlight = (byte*)((void*)ptr->propertyTextureExposedToSunlight);
				PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
				PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
				PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
				List<Element> elements = ElementLoader.elements;
				this.simData.emittedMassEntries = ptr->emittedMassEntries;
				this.simData.elementChunks = ptr->elementChunkInfos;
				this.simData.buildingTemperatures = ptr->buildingTemperatures;
				this.simData.diseaseEmittedInfos = ptr->diseaseEmittedInfos;
				this.simData.diseaseConsumedInfos = ptr->diseaseConsumedInfos;
				for (int i = 0; i < ptr->numSubstanceChangeInfo; i++)
				{
					Sim.SubstanceChangeInfo substanceChangeInfo = ptr->substanceChangeInfo[i];
					Element element = elements[(int)substanceChangeInfo.newElemIdx];
					Grid.Element[substanceChangeInfo.cellIdx] = element;
				}
				for (int j = 0; j < ptr->numSolidInfo; j++)
				{
					Sim.SolidInfo solidInfo = ptr->solidInfo[j];
					if (!this.solidChangedFilter.Contains(solidInfo.cellIdx))
					{
						this.solidInfo.Add(new SolidInfo(solidInfo.cellIdx, solidInfo.isSolid != 0));
						bool solid = solidInfo.isSolid != 0;
						Grid.SetSolid(solidInfo.cellIdx, solid, CellEventLogger.Instance.SimMessagesSolid);
					}
				}
				for (int k = 0; k < ptr->numCallbackInfo; k++)
				{
					Sim.CallbackInfo callbackInfo = ptr->callbackInfo[k];
					HandleVector<Game.CallbackInfo>.Handle handle = new HandleVector<Game.CallbackInfo>.Handle
					{
						index = callbackInfo.callbackIdx
					};
					if (!this.IsManuallyReleasedHandle(handle))
					{
						this.callbackInfo.Add(new Klei.CallbackInfo(handle));
					}
				}
				int numSpawnFallingLiquidInfo = ptr->numSpawnFallingLiquidInfo;
				for (int l = 0; l < numSpawnFallingLiquidInfo; l++)
				{
					Sim.SpawnFallingLiquidInfo spawnFallingLiquidInfo = ptr->spawnFallingLiquidInfo[l];
					FallingWater.instance.AddParticle(spawnFallingLiquidInfo.cellIdx, spawnFallingLiquidInfo.elemIdx, spawnFallingLiquidInfo.mass, spawnFallingLiquidInfo.temperature, spawnFallingLiquidInfo.diseaseIdx, spawnFallingLiquidInfo.diseaseCount, false, false, false, false);
				}
				int numDigInfo = ptr->numDigInfo;
				WorldDamage component = this.world.GetComponent<WorldDamage>();
				for (int m = 0; m < numDigInfo; m++)
				{
					Sim.SpawnOreInfo spawnOreInfo = ptr->digInfo[m];
					if (spawnOreInfo.temperature <= 0f && spawnOreInfo.mass > 0f)
					{
						global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
					}
					component.OnDigComplete(spawnOreInfo.cellIdx, spawnOreInfo.mass, spawnOreInfo.temperature, spawnOreInfo.elemIdx, spawnOreInfo.diseaseIdx, spawnOreInfo.diseaseCount);
				}
				int numSpawnOreInfo = ptr->numSpawnOreInfo;
				for (int n = 0; n < numSpawnOreInfo; n++)
				{
					Sim.SpawnOreInfo spawnOreInfo2 = ptr->spawnOreInfo[n];
					Vector3 position = Grid.CellToPosCCC(spawnOreInfo2.cellIdx, Grid.SceneLayer.Ore);
					Element element2 = ElementLoader.elements[(int)spawnOreInfo2.elemIdx];
					if (spawnOreInfo2.temperature <= 0f && spawnOreInfo2.mass > 0f)
					{
						global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
					}
					element2.substance.SpawnResource(position, spawnOreInfo2.mass, spawnOreInfo2.temperature, spawnOreInfo2.diseaseIdx, spawnOreInfo2.diseaseCount, false, false, false);
				}
				int numSpawnFXInfo = ptr->numSpawnFXInfo;
				for (int num = 0; num < numSpawnFXInfo; num++)
				{
					Sim.SpawnFXInfo spawnFXInfo = ptr->spawnFXInfo[num];
					this.SpawnFX((SpawnFXHashes)spawnFXInfo.fxHash, spawnFXInfo.cellIdx, spawnFXInfo.rotation);
				}
				UnstableGroundManager component2 = this.world.GetComponent<UnstableGroundManager>();
				int numUnstableCellInfo = ptr->numUnstableCellInfo;
				for (int num2 = 0; num2 < numUnstableCellInfo; num2++)
				{
					Sim.UnstableCellInfo unstableCellInfo = ptr->unstableCellInfo[num2];
					if (unstableCellInfo.fallingInfo == 0)
					{
						component2.Spawn(unstableCellInfo.cellIdx, ElementLoader.elements[(int)unstableCellInfo.elemIdx], unstableCellInfo.mass, unstableCellInfo.temperature, unstableCellInfo.diseaseIdx, unstableCellInfo.diseaseCount);
					}
				}
				int numWorldDamageInfo = ptr->numWorldDamageInfo;
				for (int num3 = 0; num3 < numWorldDamageInfo; num3++)
				{
					Sim.WorldDamageInfo damage_info = ptr->worldDamageInfo[num3];
					WorldDamage.Instance.ApplyDamage(damage_info);
				}
				for (int num4 = 0; num4 < ptr->numRemovedMassEntries; num4++)
				{
					ElementConsumer.AddMass(ptr->removedMassEntries[num4]);
				}
				int numMassConsumedCallbacks = ptr->numMassConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle2 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle);
				for (int num5 = 0; num5 < numMassConsumedCallbacks; num5++)
				{
					Sim.MassConsumedCallback massConsumedCallback = ptr->massConsumedCallbacks[num5];
					handle2.index = massConsumedCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.MassConsumedCallback> complexCallbackInfo = this.massConsumedCallbackManager.Release(handle2, "massConsumedCB");
					if (complexCallbackInfo.cb != null)
					{
						complexCallbackInfo.cb(massConsumedCallback, complexCallbackInfo.callbackData);
					}
				}
				int numMassEmittedCallbacks = ptr->numMassEmittedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle handle3 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle);
				for (int num6 = 0; num6 < numMassEmittedCallbacks; num6++)
				{
					Sim.MassEmittedCallback massEmittedCallback = ptr->massEmittedCallbacks[num6];
					handle3.index = massEmittedCallback.callbackIdx;
					if (this.massEmitCallbackManager.IsVersionValid(handle3))
					{
						Game.ComplexCallbackInfo<Sim.MassEmittedCallback> item = this.massEmitCallbackManager.GetItem(handle3);
						if (item.cb != null)
						{
							item.cb(massEmittedCallback, item.callbackData);
						}
					}
				}
				int numDiseaseConsumptionCallbacks = ptr->numDiseaseConsumptionCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle handle4 = default(HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle);
				for (int num7 = 0; num7 < numDiseaseConsumptionCallbacks; num7++)
				{
					Sim.DiseaseConsumptionCallback diseaseConsumptionCallback = ptr->diseaseConsumptionCallbacks[num7];
					handle4.index = diseaseConsumptionCallback.callbackIdx;
					if (this.diseaseConsumptionCallbackManager.IsVersionValid(handle4))
					{
						Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback> item2 = this.diseaseConsumptionCallbackManager.GetItem(handle4);
						if (item2.cb != null)
						{
							item2.cb(diseaseConsumptionCallback, item2.callbackData);
						}
					}
				}
				int numComponentStateChangedMessages = ptr->numComponentStateChangedMessages;
				HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle5 = default(HandleVector<Game.ComplexCallbackInfo<int>>.Handle);
				for (int num8 = 0; num8 < numComponentStateChangedMessages; num8++)
				{
					Sim.ComponentStateChangedMessage componentStateChangedMessage = ptr->componentStateChangedMessages[num8];
					handle5.index = componentStateChangedMessage.callbackIdx;
					if (this.simComponentCallbackManager.IsVersionValid(handle5))
					{
						Game.ComplexCallbackInfo<int> complexCallbackInfo2 = this.simComponentCallbackManager.Release(handle5, "component state changed cb");
						if (complexCallbackInfo2.cb != null)
						{
							complexCallbackInfo2.cb(componentStateChangedMessage.simHandle, complexCallbackInfo2.callbackData);
						}
					}
				}
				int numRadiationConsumedCallbacks = ptr->numRadiationConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle handle6 = default(HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle);
				for (int num9 = 0; num9 < numRadiationConsumedCallbacks; num9++)
				{
					Sim.ConsumedRadiationCallback consumedRadiationCallback = ptr->radiationConsumedCallbacks[num9];
					handle6.index = consumedRadiationCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback> complexCallbackInfo3 = this.radiationConsumedCallbackManager.Release(handle6, "radiationConsumedCB");
					if (complexCallbackInfo3.cb != null)
					{
						complexCallbackInfo3.cb(consumedRadiationCallback, complexCallbackInfo3.callbackData);
					}
				}
				int numElementChunkMeltedInfos = ptr->numElementChunkMeltedInfos;
				for (int num10 = 0; num10 < numElementChunkMeltedInfos; num10++)
				{
					SimTemperatureTransfer.DoOreMeltTransition(ptr->elementChunkMeltedInfos[num10].handle);
				}
				int numBuildingOverheatInfos = ptr->numBuildingOverheatInfos;
				for (int num11 = 0; num11 < numBuildingOverheatInfos; num11++)
				{
					StructureTemperatureComponents.DoOverheat(ptr->buildingOverheatInfos[num11].handle);
				}
				int numBuildingNoLongerOverheatedInfos = ptr->numBuildingNoLongerOverheatedInfos;
				for (int num12 = 0; num12 < numBuildingNoLongerOverheatedInfos; num12++)
				{
					StructureTemperatureComponents.DoNoLongerOverheated(ptr->buildingNoLongerOverheatedInfos[num12].handle);
				}
				int numBuildingMeltedInfos = ptr->numBuildingMeltedInfos;
				for (int num13 = 0; num13 < numBuildingMeltedInfos; num13++)
				{
					StructureTemperatureComponents.DoStateTransition(ptr->buildingMeltedInfos[num13].handle);
				}
				int numCellMeltedInfos = ptr->numCellMeltedInfos;
				for (int num14 = 0; num14 < numCellMeltedInfos; num14++)
				{
					int gameCell = ptr->cellMeltedInfos[num14].gameCell;
					GameObject gameObject = Grid.Objects[gameCell, 9];
					if (gameObject != null)
					{
						gameObject.Trigger(675471409, null);
						global::Util.KDestroyGameObject(gameObject);
					}
				}
				if (dt > 0f)
				{
					this.conduitTemperatureManager.Sim200ms(0.2f);
					this.conduitDiseaseManager.Sim200ms(0.2f);
					this.gasConduitFlow.Sim200ms(0.2f);
					this.liquidConduitFlow.Sim200ms(0.2f);
					this.solidConduitFlow.Sim200ms(0.2f);
					this.accumulators.Sim200ms(0.2f);
					this.plantElementAbsorbers.Sim200ms(0.2f);
				}
				Sim.DebugProperties debugProperties;
				debugProperties.buildingTemperatureScale = 100f;
				debugProperties.buildingToBuildingTemperatureScale = 0.001f;
				debugProperties.biomeTemperatureLerpRate = 0.001f;
				debugProperties.isDebugEditing = ((DebugPaintElementScreen.Instance != null && DebugPaintElementScreen.Instance.gameObject.activeSelf) ? 1 : 0);
				debugProperties.pad0 = (debugProperties.pad1 = (debugProperties.pad2 = 0));
				SimMessages.SetDebugProperties(debugProperties);
				if (dt > 0f)
				{
					if (this.circuitManager != null)
					{
						this.circuitManager.Sim200msFirst(dt);
					}
					if (this.energySim != null)
					{
						this.energySim.EnergySim200ms(dt);
					}
					if (this.circuitManager != null)
					{
						this.circuitManager.Sim200msLast(dt);
					}
				}
				result = ptr;
			}
		}
		return result;
	}

	// Token: 0x06003ECE RID: 16078 RVA: 0x0015C78C File Offset: 0x0015A98C
	public void AddSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Add(cell);
	}

	// Token: 0x06003ECF RID: 16079 RVA: 0x0015C79B File Offset: 0x0015A99B
	public void RemoveSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Remove(cell);
	}

	// Token: 0x06003ED0 RID: 16080 RVA: 0x0015C7AA File Offset: 0x0015A9AA
	public void SetIsLoading()
	{
		this.isLoading = true;
	}

	// Token: 0x06003ED1 RID: 16081 RVA: 0x0015C7B3 File Offset: 0x0015A9B3
	public bool IsLoading()
	{
		return this.isLoading;
	}

	// Token: 0x06003ED2 RID: 16082 RVA: 0x0015C7BC File Offset: 0x0015A9BC
	private void ShowDebugCellInfo()
	{
		int mouseCell = DebugHandler.GetMouseCell();
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(mouseCell, out num, out num2);
		string text = string.Concat(new string[]
		{
			mouseCell.ToString(),
			" (",
			num.ToString(),
			", ",
			num2.ToString(),
			")"
		});
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x06003ED3 RID: 16083 RVA: 0x0015C837 File Offset: 0x0015AA37
	public void ForceSimStep()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Force-stepping the sim"
		});
		this.simDt = 0.2f;
	}

	// Token: 0x06003ED4 RID: 16084 RVA: 0x0015C858 File Offset: 0x0015AA58
	private void Update()
	{
		if (this.isLoading)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (global::Debug.developerConsoleVisible)
		{
			global::Debug.developerConsoleVisible = false;
		}
		if (DebugHandler.DebugCellInfo)
		{
			this.ShowDebugCellInfo();
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		this.circuitManager.RenderEveryTick(deltaTime);
		this.logicCircuitManager.RenderEveryTick(deltaTime);
		this.solidConduitFlow.RenderEveryTick(deltaTime);
		Pathfinding.Instance.RenderEveryTick();
		Singleton<CellChangeMonitor>.Instance.RenderEveryTick();
		this.SimEveryTick(deltaTime);
	}

	// Token: 0x06003ED5 RID: 16085 RVA: 0x0015C8F0 File Offset: 0x0015AAF0
	private void SimEveryTick(float dt)
	{
		dt = Mathf.Min(dt, 0.2f);
		this.simDt += dt;
		if (this.simDt >= 0.016666668f)
		{
			do
			{
				this.simSubTick++;
				this.simSubTick %= 12;
				if (this.simSubTick == 0)
				{
					this.hasFirstSimTickRun = true;
					this.UnsafeSim200ms(0.2f);
				}
				if (this.hasFirstSimTickRun)
				{
					Singleton<StateMachineUpdater>.Instance.AdvanceOneSimSubTick();
				}
				this.simDt -= 0.016666668f;
			}
			while (this.simDt >= 0.016666668f);
			return;
		}
		this.UnsafeSim200ms(0f);
	}

	// Token: 0x06003ED6 RID: 16086 RVA: 0x0015C99C File Offset: 0x0015AB9C
	private unsafe void UnsafeSim200ms(float dt)
	{
		this.simActiveRegions.Clear();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.IsDiscovered)
			{
				Game.SimActiveRegion simActiveRegion = new Game.SimActiveRegion();
				simActiveRegion.region = new Pair<Vector2I, Vector2I>(worldContainer.WorldOffset, worldContainer.WorldOffset + worldContainer.WorldSize);
				simActiveRegion.currentSunlightIntensity = worldContainer.currentSunlightIntensity;
				simActiveRegion.currentCosmicRadiationIntensity = worldContainer.currentCosmicIntensity;
				this.simActiveRegions.Add(simActiveRegion);
			}
		}
		global::Debug.Assert(this.simActiveRegions.Count > 0, "Cannot send a frame to the sim with zero active regions");
		SimMessages.NewGameFrame(dt, this.simActiveRegions);
		Sim.GameDataUpdate* ptr = this.StepTheSim(dt);
		if (ptr == null)
		{
			global::Debug.LogError("UNEXPECTED!");
			return;
		}
		if (ptr->numFramesProcessed <= 0)
		{
			return;
		}
		this.gameSolidInfo.AddRange(this.solidInfo);
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, ptr->numSolidSubstanceChangeInfo, ptr->solidSubstanceChangeInfo, ptr->numLiquidChangeInfo, ptr->liquidChangeInfo);
		this.gameSolidInfo.Clear();
		this.solidInfo.Clear();
		this.callbackInfo.Clear();
		this.callbackManagerManuallyReleasedHandles.Clear();
		Pathfinding.Instance.UpdateNavGrids(false);
	}

	// Token: 0x06003ED7 RID: 16087 RVA: 0x0015CB04 File Offset: 0x0015AD04
	private void LateUpdateComponents()
	{
		this.UpdateOverlayScreen();
	}

	// Token: 0x06003ED8 RID: 16088 RVA: 0x0015CB0C File Offset: 0x0015AD0C
	private void OnAddBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		this.lastDrawnOverlayMode = default(HashedString);
		if (PlayerController.Instance != null)
		{
			BuildTool buildTool = PlayerController.Instance.ActiveTool as BuildTool;
			if (buildTool != null && buildTool.visualizer == entity_cell_visualizer.gameObject)
			{
				this.previewVisualizer = entity_cell_visualizer;
			}
		}
	}

	// Token: 0x06003ED9 RID: 16089 RVA: 0x0015CB65 File Offset: 0x0015AD65
	private void OnRemoveBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		if (this.previewVisualizer == entity_cell_visualizer)
		{
			this.previewVisualizer = null;
		}
	}

	// Token: 0x06003EDA RID: 16090 RVA: 0x0015CB7C File Offset: 0x0015AD7C
	private void UpdateOverlayScreen()
	{
		if (OverlayScreen.Instance == null)
		{
			return;
		}
		HashedString mode = OverlayScreen.Instance.GetMode();
		if (this.previewVisualizer != null)
		{
			this.previewVisualizer.DrawIcons(mode);
		}
		if (mode == this.lastDrawnOverlayMode)
		{
			return;
		}
		foreach (EntityCellVisualizer entityCellVisualizer in Components.EntityCellVisualizers.Items)
		{
			entityCellVisualizer.DrawIcons(mode);
		}
		this.lastDrawnOverlayMode = mode;
	}

	// Token: 0x06003EDB RID: 16091 RVA: 0x0015CC1C File Offset: 0x0015AE1C
	public void ForceOverlayUpdate(bool clearLastMode = false)
	{
		this.previousOverlayMode = OverlayModes.None.ID;
		if (clearLastMode)
		{
			this.lastDrawnOverlayMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x06003EDC RID: 16092 RVA: 0x0015CC38 File Offset: 0x0015AE38
	private void LateUpdate()
	{
		if (this.OnSpawnComplete != null)
		{
			this.OnSpawnComplete();
			this.OnSpawnComplete = null;
		}
		if (Time.timeScale == 0f && !this.IsPaused)
		{
			this.IsPaused = true;
			base.Trigger(-1788536802, this.IsPaused);
		}
		else if (Time.timeScale != 0f && this.IsPaused)
		{
			this.IsPaused = false;
			base.Trigger(-1788536802, this.IsPaused);
		}
		if (Input.GetMouseButton(0))
		{
			this.VisualTunerElement = null;
			int mouseCell = DebugHandler.GetMouseCell();
			if (Grid.IsValidCell(mouseCell))
			{
				Element visualTunerElement = Grid.Element[mouseCell];
				this.VisualTunerElement = visualTunerElement;
			}
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		HashedString mode = SimDebugView.Instance.GetMode();
		if (mode != this.previousOverlayMode)
		{
			this.previousOverlayMode = mode;
			if (mode == OverlayModes.LiquidConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(true, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.GasConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(true, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.SolidConveyor.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(true, true);
			}
			else
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, false);
				this.gasFlowVisualizer.ColourizePipeContents(false, false);
				this.solidFlowVisualizer.ColourizePipeContents(false, false);
			}
		}
		this.gasFlowVisualizer.Render(this.gasFlowPos.z, 0, this.gasConduitFlow.ContinuousLerpPercent, mode == OverlayModes.GasConduits.ID && this.gasConduitFlow.DiscreteLerpPercent != this.previousGasConduitFlowDiscreteLerpPercent);
		this.liquidFlowVisualizer.Render(this.liquidFlowPos.z, 0, this.liquidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.LiquidConduits.ID && this.liquidConduitFlow.DiscreteLerpPercent != this.previousLiquidConduitFlowDiscreteLerpPercent);
		this.solidFlowVisualizer.Render(this.solidFlowPos.z, 0, this.solidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.SolidConveyor.ID && this.solidConduitFlow.DiscreteLerpPercent != this.previousSolidConduitFlowDiscreteLerpPercent);
		this.previousGasConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.GasConduits.ID) ? this.gasConduitFlow.DiscreteLerpPercent : -1f);
		this.previousLiquidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.LiquidConduits.ID) ? this.liquidConduitFlow.DiscreteLerpPercent : -1f);
		this.previousSolidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.SolidConveyor.ID) ? this.solidConduitFlow.DiscreteLerpPercent : -1f);
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Shader.SetGlobalVector("_WsToCs", new Vector4(vector.x / (float)Grid.WidthInCells, vector.y / (float)Grid.HeightInCells, (vector2.x - vector.x) / (float)Grid.WidthInCells, (vector2.y - vector.y) / (float)Grid.HeightInCells));
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		Vector2I worldOffset = activeWorld.WorldOffset;
		Vector2I worldSize = activeWorld.WorldSize;
		Vector4 value = new Vector4((vector.x - (float)worldOffset.x) / (float)worldSize.x, (vector.y - (float)worldOffset.y) / (float)worldSize.y, (vector2.x - vector.x) / (float)worldSize.x, (vector2.y - vector.y) / (float)worldSize.y);
		Shader.SetGlobalVector("_WsToCcs", value);
		if (this.drawStatusItems)
		{
			this.statusItemRenderer.RenderEveryTick();
			this.prioritizableRenderer.RenderEveryTick();
		}
		this.LateUpdateComponents();
		Singleton<StateMachineUpdater>.Instance.Render(Time.unscaledDeltaTime);
		Singleton<StateMachineUpdater>.Instance.RenderEveryTick(Time.unscaledDeltaTime);
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
			if (component != null)
			{
				component.DrawPath();
			}
		}
		KFMOD.RenderEveryTick(Time.deltaTime);
		if (GenericGameSettings.instance.performanceCapture.waitTime != 0f)
		{
			this.UpdatePerformanceCapture();
		}
	}

	// Token: 0x06003EDD RID: 16093 RVA: 0x0015D128 File Offset: 0x0015B328
	private void UpdatePerformanceCapture()
	{
		if (this.IsPaused && SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (Time.timeSinceLevelLoad < GenericGameSettings.instance.performanceCapture.waitTime)
		{
			return;
		}
		uint num = 642695U;
		string text = System.DateTime.Now.ToShortDateString();
		string text2 = System.DateTime.Now.ToShortTimeString();
		string fileName = Path.GetFileName(GenericGameSettings.instance.performanceCapture.saveGame);
		string text3 = "Version,Date,Time,SaveGame";
		string text4 = string.Format("{0},{1},{2},{3}", new object[]
		{
			num,
			text,
			text2,
			fileName
		});
		float num2 = 0.1f;
		if (GenericGameSettings.instance.performanceCapture.gcStats)
		{
			global::Debug.Log("Begin GC profiling...");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			GC.Collect();
			num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
			global::Debug.Log("\tGC.Collect() took " + num2.ToString() + " seconds");
			MemorySnapshot memorySnapshot = new MemorySnapshot();
			string format = "{0},{1},{2},{3}";
			string path = "./memory/GCTypeMetrics.csv";
			if (!File.Exists(path))
			{
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					streamWriter.WriteLine(string.Format(format, new object[]
					{
						text3,
						"Type",
						"Instances",
						"References"
					}));
				}
			}
			using (StreamWriter streamWriter2 = new StreamWriter(path, true))
			{
				foreach (MemorySnapshot.TypeData typeData in memorySnapshot.types.Values)
				{
					streamWriter2.WriteLine(string.Format(format, new object[]
					{
						text4,
						"\"" + typeData.type.ToString() + "\"",
						typeData.instanceCount,
						typeData.refCount
					}));
				}
			}
			global::Debug.Log("...end GC profiling");
		}
		float fps = Global.Instance.GetComponent<PerformanceMonitor>().FPS;
		Directory.CreateDirectory("./memory");
		string format2 = "{0},{1},{2}";
		string path2 = "./memory/GeneralMetrics.csv";
		if (!File.Exists(path2))
		{
			using (StreamWriter streamWriter3 = new StreamWriter(path2))
			{
				streamWriter3.WriteLine(string.Format(format2, text3, "GCDuration", "FPS"));
			}
		}
		using (StreamWriter streamWriter4 = new StreamWriter(path2, true))
		{
			streamWriter4.WriteLine(string.Format(format2, text4, num2, fps));
		}
		GenericGameSettings.instance.performanceCapture.waitTime = 0f;
		App.Quit();
	}

	// Token: 0x06003EDE RID: 16094 RVA: 0x0015D430 File Offset: 0x0015B630
	public void Reset(GameSpawnData gsd, Vector2I world_offset)
	{
		using (new KProfiler.Region("World.Reset", null))
		{
			if (gsd != null)
			{
				foreach (KeyValuePair<Vector2I, bool> keyValuePair in gsd.preventFoWReveal)
				{
					if (keyValuePair.Value)
					{
						Vector2I v = new Vector2I(keyValuePair.Key.X + world_offset.X, keyValuePair.Key.Y + world_offset.Y);
						Grid.PreventFogOfWarReveal[Grid.PosToCell(v)] = keyValuePair.Value;
					}
				}
			}
		}
	}

	// Token: 0x06003EDF RID: 16095 RVA: 0x0015D508 File Offset: 0x0015B708
	private void OnApplicationQuit()
	{
		Game.quitting = true;
		Sim.Shutdown();
		AudioMixer.Destroy();
		if (this.screenMgr != null && this.screenMgr.gameObject != null)
		{
			UnityEngine.Object.Destroy(this.screenMgr.gameObject);
		}
		Console.WriteLine("Game.OnApplicationQuit()");
	}

	// Token: 0x06003EE0 RID: 16096 RVA: 0x0015D560 File Offset: 0x0015B760
	private void InitializeFXSpawners()
	{
		for (int i = 0; i < this.fxSpawnData.Length; i++)
		{
			int fx_idx = i;
			this.fxSpawnData[fx_idx].fxPrefab.SetActive(false);
			ushort fx_mask = (ushort)(1 << fx_idx);
			Action<SpawnFXHashes, GameObject> destroyer = delegate(SpawnFXHashes fxid, GameObject go)
			{
				if (!Game.IsQuitting())
				{
					int num = Grid.PosToCell(go);
					ushort[] array = this.activeFX;
					int num2 = num;
					array[num2] &= ~fx_mask;
					go.GetComponent<KAnimControllerBase>().enabled = false;
					this.fxPools[(int)fxid].ReleaseInstance(go);
				}
			};
			Func<GameObject> instantiator = delegate()
			{
				GameObject gameObject = GameUtil.KInstantiate(this.fxSpawnData[fx_idx].fxPrefab, Grid.SceneLayer.Front, null, 0);
				KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				gameObject.SetActive(true);
				component.onDestroySelf = delegate(GameObject go)
				{
					destroyer(this.fxSpawnData[fx_idx].id, go);
				};
				return gameObject;
			};
			GameObjectPool pool = new GameObjectPool(instantiator, this.fxSpawnData[fx_idx].initialCount);
			this.fxPools[(int)this.fxSpawnData[fx_idx].id] = pool;
			this.fxSpawner[(int)this.fxSpawnData[fx_idx].id] = delegate(Vector3 pos, float rotation)
			{
				Action<object> action = delegate(object obj)
				{
					int num = Grid.PosToCell(pos);
					if ((this.activeFX[num] & fx_mask) == 0)
					{
						ushort[] array = this.activeFX;
						int num2 = num;
						array[num2] |= fx_mask;
						GameObject instance = pool.GetInstance();
						Game.SpawnPoolData spawnPoolData = this.fxSpawnData[fx_idx];
						Quaternion rotation = Quaternion.identity;
						bool flipX = false;
						string s = spawnPoolData.initialAnim;
						Game.SpawnRotationConfig rotationConfig = spawnPoolData.rotationConfig;
						if (rotationConfig != Game.SpawnRotationConfig.Normal)
						{
							if (rotationConfig == Game.SpawnRotationConfig.StringName)
							{
								int num3 = (int)(rotation / 90f);
								if (num3 < 0)
								{
									num3 += spawnPoolData.rotationData.Length;
								}
								s = spawnPoolData.rotationData[num3].animName;
								flipX = spawnPoolData.rotationData[num3].flip;
							}
						}
						else
						{
							rotation = Quaternion.Euler(0f, 0f, rotation);
						}
						pos += spawnPoolData.spawnOffset;
						Vector2 vector = UnityEngine.Random.insideUnitCircle;
						vector.x *= spawnPoolData.spawnRandomOffset.x;
						vector.y *= spawnPoolData.spawnRandomOffset.y;
						vector = rotation * vector;
						pos.x += vector.x;
						pos.y += vector.y;
						instance.transform.SetPosition(pos);
						instance.transform.rotation = rotation;
						KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
						component.FlipX = flipX;
						component.TintColour = spawnPoolData.colour;
						component.Play(s, KAnim.PlayMode.Once, 1f, 0f);
						component.enabled = true;
					}
				};
				if (Game.Instance.IsPaused)
				{
					action(null);
					return;
				}
				GameScheduler.Instance.Schedule("SpawnFX", 0f, action, null, null);
			};
		}
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x0015D660 File Offset: 0x0015B860
	public void SpawnFX(SpawnFXHashes fx_id, int cell, float rotation)
	{
		Vector3 vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.Front);
		if (CameraController.Instance.IsVisiblePos(vector))
		{
			this.fxSpawner[(int)fx_id](vector, rotation);
		}
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x0015D696 File Offset: 0x0015B896
	public void SpawnFX(SpawnFXHashes fx_id, Vector3 pos, float rotation)
	{
		this.fxSpawner[(int)fx_id](pos, rotation);
	}

	// Token: 0x06003EE3 RID: 16099 RVA: 0x0015D6AB File Offset: 0x0015B8AB
	public static void SaveSettings(BinaryWriter writer)
	{
		Serializer.Serialize(new Game.Settings(Game.Instance), writer);
	}

	// Token: 0x06003EE4 RID: 16100 RVA: 0x0015D6C0 File Offset: 0x0015B8C0
	public static void LoadSettings(Deserializer deserializer)
	{
		Game.Settings settings = new Game.Settings();
		deserializer.Deserialize(settings);
		KPlayerPrefs.SetInt(Game.NextUniqueIDKey, settings.nextUniqueID);
		KleiMetrics.SetGameID(settings.gameID);
	}

	// Token: 0x06003EE5 RID: 16101 RVA: 0x0015D6F8 File Offset: 0x0015B8F8
	public void Save(BinaryWriter writer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = SaveLoader.Instance.clusterDetailSave;
		gameSaveData.debugWasUsed = this.debugWasUsed;
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		gameSaveData.autoPrioritizeRoles = this.autoPrioritizeRoles;
		gameSaveData.advancedPersonalPriorities = this.advancedPersonalPriorities;
		gameSaveData.savedInfo = this.savedInfo;
		global::Debug.Assert(gameSaveData.worldDetail != null, "World detail null");
		gameSaveData.dateGenerated = this.dateGenerated;
		if (!this.changelistsPlayedOn.Contains(642695U))
		{
			this.changelistsPlayedOn.Add(642695U);
		}
		gameSaveData.changelistsPlayedOn = this.changelistsPlayedOn;
		if (this.OnSave != null)
		{
			this.OnSave(gameSaveData);
		}
		Serializer.Serialize(gameSaveData, writer);
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x0015D814 File Offset: 0x0015BA14
	public void Load(Deserializer deserializer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = new WorldDetailSave();
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		deserializer.Deserialize(gameSaveData);
		this.gasConduitFlow = gameSaveData.gasConduitFlow;
		this.liquidConduitFlow = gameSaveData.liquidConduitFlow;
		this.debugWasUsed = gameSaveData.debugWasUsed;
		this.autoPrioritizeRoles = gameSaveData.autoPrioritizeRoles;
		this.advancedPersonalPriorities = gameSaveData.advancedPersonalPriorities;
		this.dateGenerated = gameSaveData.dateGenerated;
		this.changelistsPlayedOn = (gameSaveData.changelistsPlayedOn ?? new List<uint>());
		if (gameSaveData.dateGenerated.IsNullOrWhiteSpace())
		{
			this.dateGenerated = "Before U41 (Feb 2022)";
		}
		DebugUtil.LogArgs(new object[]
		{
			"SAVEINFO"
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Generated: " + this.dateGenerated
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Played on: " + string.Join<uint>(", ", this.changelistsPlayedOn)
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Debug was used: " + Game.Instance.debugWasUsed.ToString()
		});
		this.savedInfo = gameSaveData.savedInfo;
		this.savedInfo.InitializeEmptyVariables();
		CustomGameSettings.Instance.Print();
		KCrashReporter.debugWasUsed = this.debugWasUsed;
		SaveLoader.Instance.SetWorldDetail(gameSaveData.worldDetail);
		if (this.OnLoad != null)
		{
			this.OnLoad(gameSaveData);
		}
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x0015D9DF File Offset: 0x0015BBDF
	public void SetAutoSaveCallbacks(Game.SavingPreCB activatePreCB, Game.SavingActiveCB activateActiveCB, Game.SavingPostCB activatePostCB)
	{
		this.activatePreCB = activatePreCB;
		this.activateActiveCB = activateActiveCB;
		this.activatePostCB = activatePostCB;
	}

	// Token: 0x06003EE8 RID: 16104 RVA: 0x0015D9F6 File Offset: 0x0015BBF6
	public void StartDelayedInitialSave()
	{
		base.StartCoroutine(this.DelayedInitialSave());
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x0015DA05 File Offset: 0x0015BC05
	private IEnumerator DelayedInitialSave()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (GenericGameSettings.instance.devAutoWorldGenActive)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.SetDiscovered(true);
			}
			SaveGame.Instance.worldGenSpawner.SpawnEverything();
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().DEBUG_REVEAL_ENTIRE_MAP();
			if (CameraController.Instance != null)
			{
				CameraController.Instance.EnableFreeCamera(true);
			}
			for (int num2 = 0; num2 != Grid.WidthInCells * Grid.HeightInCells; num2++)
			{
				Grid.Reveal(num2, byte.MaxValue, false);
			}
			GenericGameSettings.instance.devAutoWorldGenActive = false;
		}
		SaveLoader.Instance.InitialSave();
		yield break;
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x0015DA10 File Offset: 0x0015BC10
	public void StartDelayedSave(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		if (this.activatePreCB != null)
		{
			this.activatePreCB(delegate
			{
				this.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
			});
			return;
		}
		base.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x0015DA7E File Offset: 0x0015BC7E
	private IEnumerator DelayedSave(string filename, bool isAutoSave, bool updateSavePointer)
	{
		while (PlayerController.Instance.IsDragging())
		{
			yield return null;
		}
		PlayerController.Instance.CancelDragging();
		PlayerController.Instance.AllowDragging(false);
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (this.activateActiveCB != null)
		{
			this.activateActiveCB();
			for (int i = 0; i < 1; i = num)
			{
				yield return null;
				num = i + 1;
			}
		}
		SaveLoader.Instance.Save(filename, isAutoSave, updateSavePointer);
		if (this.activatePostCB != null)
		{
			this.activatePostCB();
		}
		for (int i = 0; i < 5; i = num)
		{
			yield return null;
			num = i + 1;
		}
		PlayerController.Instance.AllowDragging(true);
		yield break;
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x0015DAA2 File Offset: 0x0015BCA2
	public void StartDelayed(int tick_delay, System.Action action)
	{
		base.StartCoroutine(this.DelayedExecutor(tick_delay, action));
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0015DAB3 File Offset: 0x0015BCB3
	private IEnumerator DelayedExecutor(int tick_delay, System.Action action)
	{
		int num;
		for (int i = 0; i < tick_delay; i = num)
		{
			yield return null;
			num = i + 1;
		}
		action();
		yield break;
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x0015DACC File Offset: 0x0015BCCC
	private void LoadEventHashes()
	{
		foreach (object obj in Enum.GetValues(typeof(GameHashes)))
		{
			GameHashes hash = (GameHashes)obj;
			HashCache.Get().Add((int)hash, hash.ToString());
		}
		foreach (object obj2 in Enum.GetValues(typeof(UtilHashes)))
		{
			UtilHashes hash2 = (UtilHashes)obj2;
			HashCache.Get().Add((int)hash2, hash2.ToString());
		}
		foreach (object obj3 in Enum.GetValues(typeof(UIHashes)))
		{
			UIHashes hash3 = (UIHashes)obj3;
			HashCache.Get().Add((int)hash3, hash3.ToString());
		}
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x0015DC08 File Offset: 0x0015BE08
	public void StopFE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = false;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		}
		MainMenu.Instance.StopMainMenuMusic();
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x0015DC6E File Offset: 0x0015BE6E
	public void StartBE()
	{
		Resources.UnloadUnusedAssets();
		AudioMixer.instance.Reset();
		AudioMixer.instance.StartPersistentSnapshots();
		MusicManager.instance.ConfigureSongs();
		if (MusicManager.instance.ShouldPlayDynamicMusicLoadedGame())
		{
			MusicManager.instance.PlayDynamicMusic();
		}
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x0015DCAC File Offset: 0x0015BEAC
	public void StopBE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = true;
		}
		LoopingSoundManager loopingSoundManager = LoopingSoundManager.Get();
		if (loopingSoundManager != null)
		{
			loopingSoundManager.StopAllSounds();
		}
		MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.StopPersistentSnapshots();
		foreach (List<SaveLoadRoot> list in SaveLoader.Instance.saveManager.GetLists().Values)
		{
			foreach (SaveLoadRoot saveLoadRoot in list)
			{
				if (saveLoadRoot.gameObject != null)
				{
					global::Util.KDestroyGameObject(saveLoadRoot.gameObject);
				}
			}
		}
		base.GetComponent<EntombedItemVisualizer>().Clear();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		KComponentSpawn.instance.comps.Clear();
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.cameraController);
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.playerController);
		Sim.Shutdown();
		SimAndRenderScheduler.instance.Reset();
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x0015DDFC File Offset: 0x0015BFFC
	public void SetStatusItemOffset(Transform transform, Vector3 offset)
	{
		this.statusItemRenderer.SetOffset(transform, offset);
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x0015DE0B File Offset: 0x0015C00B
	public void AddStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Add(transform, status_item);
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x0015DE1A File Offset: 0x0015C01A
	public void RemoveStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Remove(transform, status_item);
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06003EF5 RID: 16117 RVA: 0x0015DE29 File Offset: 0x0015C029
	public float LastTimeWorkStarted
	{
		get
		{
			return this.lastTimeWorkStarted;
		}
	}

	// Token: 0x06003EF6 RID: 16118 RVA: 0x0015DE31 File Offset: 0x0015C031
	public void StartedWork()
	{
		this.lastTimeWorkStarted = Time.time;
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x0015DE3E File Offset: 0x0015C03E
	private void SpawnOxygenBubbles(Vector3 position, float angle)
	{
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x0015DE40 File Offset: 0x0015C040
	public void ManualReleaseHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.callbackManagerManuallyReleasedHandles.Add(handle.index);
		this.callbackManager.Release(handle);
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x0015DE6B File Offset: 0x0015C06B
	private bool IsManuallyReleasedHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		return !this.callbackManager.IsVersionValid(handle) && this.callbackManagerManuallyReleasedHandles.Contains(handle.index);
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x0015DE92 File Offset: 0x0015C092
	[ContextMenu("Print")]
	private void Print()
	{
		Console.WriteLine("This is a console writeline test");
		global::Debug.Log("This is a debug log test");
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x0015DEA8 File Offset: 0x0015C0A8
	private void DestroyInstances()
	{
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		Db.Get().ResetProblematicDbs();
		GridSettings.ClearGrid();
		StateMachineManager.ResetParameters();
		ChoreTable.Instance.ResetParameters();
		BubbleManager.DestroyInstance();
		AmbientSoundManager.Destroy();
		AutoDisinfectableManager.DestroyInstance();
		BuildMenu.DestroyInstance();
		CancelTool.DestroyInstance();
		ClearTool.DestroyInstance();
		ChoreGroupManager.DestroyInstance();
		CO2Manager.DestroyInstance();
		ConsumerManager.DestroyInstance();
		CopySettingsTool.DestroyInstance();
		global::DateTime.DestroyInstance();
		DebugBaseTemplateButton.DestroyInstance();
		DebugPaintElementScreen.DestroyInstance();
		DetailsScreen.DestroyInstance();
		DietManager.DestroyInstance();
		DebugText.DestroyInstance();
		FactionManager.DestroyInstance();
		EmptyPipeTool.DestroyInstance();
		FetchListStatusItemUpdater.DestroyInstance();
		FishOvercrowingManager.DestroyInstance();
		FallingWater.DestroyInstance();
		GridCompositor.DestroyInstance();
		Infrared.DestroyInstance();
		KPrefabIDTracker.DestroyInstance();
		ManagementMenu.DestroyInstance();
		ClusterMapScreen.DestroyInstance();
		Messenger.DestroyInstance();
		LoopingSoundManager.DestroyInstance();
		MeterScreen.DestroyInstance();
		MinionGroupProber.DestroyInstance();
		NavPathDrawer.DestroyInstance();
		MinionIdentity.DestroyStatics();
		PathFinder.DestroyStatics();
		Pathfinding.DestroyInstance();
		PrebuildTool.DestroyInstance();
		PrioritizeTool.DestroyInstance();
		SelectTool.DestroyInstance();
		PopFXManager.DestroyInstance();
		ProgressBarsConfig.DestroyInstance();
		PropertyTextures.DestroyInstance();
		WorldResourceAmountTracker<RationTracker>.DestroyInstance();
		WorldResourceAmountTracker<ElectrobankTracker>.DestroyInstance();
		ReportManager.DestroyInstance();
		Research.DestroyInstance();
		RootMenu.DestroyInstance();
		SaveLoader.DestroyInstance();
		Scenario.DestroyInstance();
		SimDebugView.DestroyInstance();
		SpriteSheetAnimManager.DestroyInstance();
		ScheduleManager.DestroyInstance();
		Sounds.DestroyInstance();
		ToolMenu.DestroyInstance();
		WorldDamage.DestroyInstance();
		WaterCubes.DestroyInstance();
		WireBuildTool.DestroyInstance();
		VisibilityTester.DestroyInstance();
		Traces.DestroyInstance();
		TopLeftControlScreen.DestroyInstance();
		UtilityBuildTool.DestroyInstance();
		ReportScreen.DestroyInstance();
		ChorePreconditions.DestroyInstance();
		SandboxBrushTool.DestroyInstance();
		SandboxHeatTool.DestroyInstance();
		SandboxStressTool.DestroyInstance();
		SandboxCritterTool.DestroyInstance();
		SandboxClearFloorTool.DestroyInstance();
		GameScreenManager.DestroyInstance();
		GameScheduler.DestroyInstance();
		NavigationReservations.DestroyInstance();
		Tutorial.DestroyInstance();
		CameraController.DestroyInstance();
		CellEventLogger.DestroyInstance();
		GameFlowManager.DestroyInstance();
		Immigration.DestroyInstance();
		BuildTool.DestroyInstance();
		DebugTool.DestroyInstance();
		DeconstructTool.DestroyInstance();
		DisconnectTool.DestroyInstance();
		DigTool.DestroyInstance();
		DisinfectTool.DestroyInstance();
		HarvestTool.DestroyInstance();
		MopTool.DestroyInstance();
		MoveToLocationTool.DestroyInstance();
		PlaceTool.DestroyInstance();
		SpacecraftManager.DestroyInstance();
		GameplayEventManager.DestroyInstance();
		BuildingInventory.DestroyInstance();
		PlantSubSpeciesCatalog.DestroyInstance();
		SandboxDestroyerTool.DestroyInstance();
		SandboxFOWTool.DestroyInstance();
		SandboxFloodTool.DestroyInstance();
		SandboxSprinkleTool.DestroyInstance();
		StampTool.DestroyInstance();
		OnDemandUpdater.DestroyInstance();
		HoverTextScreen.DestroyInstance();
		ImmigrantScreen.DestroyInstance();
		OverlayMenu.DestroyInstance();
		NameDisplayScreen.DestroyInstance();
		PlanScreen.DestroyInstance();
		ResourceCategoryScreen.DestroyInstance();
		ResourceRemainingDisplayScreen.DestroyInstance();
		SandboxToolParameterMenu.DestroyInstance();
		SpeedControlScreen.DestroyInstance();
		Vignette.DestroyInstance();
		PlayerController.DestroyInstance();
		NotificationScreen.DestroyInstance();
		BuildingCellVisualizerResources.DestroyInstance();
		PauseScreen.DestroyInstance();
		SaveLoadRoot.DestroyStatics();
		KTime.DestroyInstance();
		DemoTimer.DestroyInstance();
		UIScheduler.DestroyInstance();
		SaveGame.DestroyInstance();
		GameClock.DestroyInstance();
		TimeOfDay.DestroyInstance();
		DeserializeWarnings.DestroyInstance();
		UISounds.DestroyInstance();
		RenderTextureDestroyer.DestroyInstance();
		HoverTextHelper.DestroyStatics();
		LoadScreen.DestroyInstance();
		LoadingOverlay.DestroyInstance();
		SimAndRenderScheduler.DestroyInstance();
		Singleton<CellChangeMonitor>.DestroyInstance();
		Singleton<StateMachineManager>.Instance.Clear();
		Singleton<StateMachineUpdater>.Instance.Clear();
		UpdateObjectCountParameter.Clear();
		MaterialSelectionPanel.ClearStatics();
		StarmapScreen.DestroyInstance();
		ClusterNameDisplayScreen.DestroyInstance();
		ClusterManager.DestroyInstance();
		ClusterGrid.DestroyInstance();
		PathFinderQueries.Reset();
		KBatchedAnimUpdater instance = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance != null)
		{
			instance.InitializeGrid();
		}
		GlobalChoreProvider.DestroyInstance();
		WorldSelector.DestroyInstance();
		ColonyDiagnosticUtility.DestroyInstance();
		DiscoveredResources.DestroyInstance();
		ClusterMapSelectTool.DestroyInstance();
		StoryManager.DestroyInstance();
		AnimEventHandlerManager.DestroyInstance();
		Game.Instance = null;
		Game.BrainScheduler = null;
		Grid.OnReveal = null;
		this.VisualTunerElement = null;
		Assets.ClearOnAddPrefab();
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		(KComponentSpawn.instance.comps as GameComps).Clear();
	}

	// Token: 0x04002692 RID: 9874
	private static readonly Thread MainThread = Thread.CurrentThread;

	// Token: 0x04002693 RID: 9875
	private static readonly string NextUniqueIDKey = "NextUniqueID";

	// Token: 0x04002694 RID: 9876
	public static string clusterId = null;

	// Token: 0x04002695 RID: 9877
	private PlayerController playerController;

	// Token: 0x04002696 RID: 9878
	private CameraController cameraController;

	// Token: 0x04002697 RID: 9879
	public Action<Game.GameSaveData> OnSave;

	// Token: 0x04002698 RID: 9880
	public Action<Game.GameSaveData> OnLoad;

	// Token: 0x04002699 RID: 9881
	public System.Action OnSpawnComplete;

	// Token: 0x0400269A RID: 9882
	[NonSerialized]
	public bool baseAlreadyCreated;

	// Token: 0x0400269B RID: 9883
	[NonSerialized]
	public bool autoPrioritizeRoles;

	// Token: 0x0400269C RID: 9884
	[NonSerialized]
	public bool advancedPersonalPriorities;

	// Token: 0x0400269D RID: 9885
	public Game.SavedInfo savedInfo;

	// Token: 0x0400269E RID: 9886
	public static bool quitting = false;

	// Token: 0x040026A0 RID: 9888
	public AssignmentManager assignmentManager;

	// Token: 0x040026A1 RID: 9889
	public GameObject playerPrefab;

	// Token: 0x040026A2 RID: 9890
	public GameObject screenManagerPrefab;

	// Token: 0x040026A3 RID: 9891
	public GameObject cameraControllerPrefab;

	// Token: 0x040026A5 RID: 9893
	private static Camera m_CachedCamera = null;

	// Token: 0x040026A6 RID: 9894
	public GameObject tempIntroScreenPrefab;

	// Token: 0x040026A7 RID: 9895
	public static int BlockSelectionLayerMask;

	// Token: 0x040026A8 RID: 9896
	public static int PickupableLayer;

	// Token: 0x040026A9 RID: 9897
	public static BrainScheduler BrainScheduler;

	// Token: 0x040026AA RID: 9898
	public Element VisualTunerElement;

	// Token: 0x040026AB RID: 9899
	public float currentFallbackSunlightIntensity;

	// Token: 0x040026AC RID: 9900
	public RoomProber roomProber;

	// Token: 0x040026AD RID: 9901
	public SpaceScannerNetworkManager spaceScannerNetworkManager;

	// Token: 0x040026AE RID: 9902
	public FetchManager fetchManager;

	// Token: 0x040026AF RID: 9903
	public EdiblesManager ediblesManager;

	// Token: 0x040026B0 RID: 9904
	public SpacecraftManager spacecraftManager;

	// Token: 0x040026B1 RID: 9905
	public UserMenu userMenu;

	// Token: 0x040026B2 RID: 9906
	public Unlocks unlocks;

	// Token: 0x040026B3 RID: 9907
	public Timelapser timelapser;

	// Token: 0x040026B4 RID: 9908
	private bool sandboxModeActive;

	// Token: 0x040026B5 RID: 9909
	public HandleVector<Game.CallbackInfo> callbackManager = new HandleVector<Game.CallbackInfo>(256);

	// Token: 0x040026B6 RID: 9910
	public List<int> callbackManagerManuallyReleasedHandles = new List<int>();

	// Token: 0x040026B7 RID: 9911
	public Game.ComplexCallbackHandleVector<int> simComponentCallbackManager = new Game.ComplexCallbackHandleVector<int>(256);

	// Token: 0x040026B8 RID: 9912
	public Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> massConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback>(64);

	// Token: 0x040026B9 RID: 9913
	public Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback> massEmitCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback>(64);

	// Token: 0x040026BA RID: 9914
	public Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback> diseaseConsumptionCallbackManager = new Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback>(64);

	// Token: 0x040026BB RID: 9915
	public Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback> radiationConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback>(256);

	// Token: 0x040026BC RID: 9916
	[NonSerialized]
	public Player LocalPlayer;

	// Token: 0x040026BD RID: 9917
	[SerializeField]
	public TextAsset maleNamesFile;

	// Token: 0x040026BE RID: 9918
	[SerializeField]
	public TextAsset femaleNamesFile;

	// Token: 0x040026BF RID: 9919
	[NonSerialized]
	public World world;

	// Token: 0x040026C0 RID: 9920
	[NonSerialized]
	public CircuitManager circuitManager;

	// Token: 0x040026C1 RID: 9921
	[NonSerialized]
	public EnergySim energySim;

	// Token: 0x040026C2 RID: 9922
	[NonSerialized]
	public LogicCircuitManager logicCircuitManager;

	// Token: 0x040026C3 RID: 9923
	private GameScreenManager screenMgr;

	// Token: 0x040026C4 RID: 9924
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> gasConduitSystem;

	// Token: 0x040026C5 RID: 9925
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> liquidConduitSystem;

	// Token: 0x040026C6 RID: 9926
	public UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem;

	// Token: 0x040026C7 RID: 9927
	public UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem;

	// Token: 0x040026C8 RID: 9928
	public UtilityNetworkTubesManager travelTubeSystem;

	// Token: 0x040026C9 RID: 9929
	public UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem;

	// Token: 0x040026CA RID: 9930
	public ConduitFlow gasConduitFlow;

	// Token: 0x040026CB RID: 9931
	public ConduitFlow liquidConduitFlow;

	// Token: 0x040026CC RID: 9932
	public SolidConduitFlow solidConduitFlow;

	// Token: 0x040026CD RID: 9933
	public Accumulators accumulators;

	// Token: 0x040026CE RID: 9934
	public PlantElementAbsorbers plantElementAbsorbers;

	// Token: 0x040026CF RID: 9935
	public Game.TemperatureOverlayModes temperatureOverlayMode;

	// Token: 0x040026D0 RID: 9936
	public bool showExpandedTemperatures;

	// Token: 0x040026D1 RID: 9937
	public List<Tag> tileOverlayFilters = new List<Tag>();

	// Token: 0x040026D2 RID: 9938
	public bool showGasConduitDisease;

	// Token: 0x040026D3 RID: 9939
	public bool showLiquidConduitDisease;

	// Token: 0x040026D4 RID: 9940
	public ConduitFlowVisualizer gasFlowVisualizer;

	// Token: 0x040026D5 RID: 9941
	public ConduitFlowVisualizer liquidFlowVisualizer;

	// Token: 0x040026D6 RID: 9942
	public SolidConduitFlowVisualizer solidFlowVisualizer;

	// Token: 0x040026D7 RID: 9943
	public ConduitTemperatureManager conduitTemperatureManager;

	// Token: 0x040026D8 RID: 9944
	public ConduitDiseaseManager conduitDiseaseManager;

	// Token: 0x040026D9 RID: 9945
	public MingleCellTracker mingleCellTracker;

	// Token: 0x040026DA RID: 9946
	private int simSubTick;

	// Token: 0x040026DB RID: 9947
	private bool hasFirstSimTickRun;

	// Token: 0x040026DC RID: 9948
	private float simDt;

	// Token: 0x040026DD RID: 9949
	public string dateGenerated;

	// Token: 0x040026DE RID: 9950
	public List<uint> changelistsPlayedOn;

	// Token: 0x040026DF RID: 9951
	[SerializeField]
	public Game.ConduitVisInfo liquidConduitVisInfo;

	// Token: 0x040026E0 RID: 9952
	[SerializeField]
	public Game.ConduitVisInfo gasConduitVisInfo;

	// Token: 0x040026E1 RID: 9953
	[SerializeField]
	public Game.ConduitVisInfo solidConduitVisInfo;

	// Token: 0x040026E2 RID: 9954
	[SerializeField]
	private Material liquidFlowMaterial;

	// Token: 0x040026E3 RID: 9955
	[SerializeField]
	private Material gasFlowMaterial;

	// Token: 0x040026E4 RID: 9956
	[SerializeField]
	private Color flowColour;

	// Token: 0x040026E5 RID: 9957
	private Vector3 gasFlowPos;

	// Token: 0x040026E6 RID: 9958
	private Vector3 liquidFlowPos;

	// Token: 0x040026E7 RID: 9959
	private Vector3 solidFlowPos;

	// Token: 0x040026E8 RID: 9960
	public bool drawStatusItems = true;

	// Token: 0x040026E9 RID: 9961
	private List<SolidInfo> solidInfo = new List<SolidInfo>();

	// Token: 0x040026EA RID: 9962
	private List<Klei.CallbackInfo> callbackInfo = new List<Klei.CallbackInfo>();

	// Token: 0x040026EB RID: 9963
	private List<SolidInfo> gameSolidInfo = new List<SolidInfo>();

	// Token: 0x040026EC RID: 9964
	private bool IsPaused;

	// Token: 0x040026ED RID: 9965
	private HashSet<int> solidChangedFilter = new HashSet<int>();

	// Token: 0x040026EE RID: 9966
	private HashedString lastDrawnOverlayMode;

	// Token: 0x040026EF RID: 9967
	private EntityCellVisualizer previewVisualizer;

	// Token: 0x040026F2 RID: 9970
	public SafetyConditions safetyConditions = new SafetyConditions();

	// Token: 0x040026F3 RID: 9971
	public SimData simData = new SimData();

	// Token: 0x040026F4 RID: 9972
	[MyCmpGet]
	private GameScenePartitioner gameScenePartitioner;

	// Token: 0x040026F5 RID: 9973
	private bool gameStarted;

	// Token: 0x040026F6 RID: 9974
	private static readonly EventSystem.IntraObjectHandler<Game> MarkStatusItemRendererDirtyDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.MarkStatusItemRendererDirty(data);
	});

	// Token: 0x040026F7 RID: 9975
	private static readonly EventSystem.IntraObjectHandler<Game> ActiveWorldChangedDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.ForceOverlayUpdate(true);
	});

	// Token: 0x040026F8 RID: 9976
	private ushort[] activeFX;

	// Token: 0x040026F9 RID: 9977
	public bool debugWasUsed;

	// Token: 0x040026FA RID: 9978
	private bool isLoading;

	// Token: 0x040026FB RID: 9979
	private List<Game.SimActiveRegion> simActiveRegions = new List<Game.SimActiveRegion>();

	// Token: 0x040026FC RID: 9980
	private HashedString previousOverlayMode = OverlayModes.None.ID;

	// Token: 0x040026FD RID: 9981
	private float previousGasConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x040026FE RID: 9982
	private float previousLiquidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x040026FF RID: 9983
	private float previousSolidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x04002700 RID: 9984
	[SerializeField]
	private Game.SpawnPoolData[] fxSpawnData;

	// Token: 0x04002701 RID: 9985
	private Dictionary<int, Action<Vector3, float>> fxSpawner = new Dictionary<int, Action<Vector3, float>>();

	// Token: 0x04002702 RID: 9986
	private Dictionary<int, GameObjectPool> fxPools = new Dictionary<int, GameObjectPool>();

	// Token: 0x04002703 RID: 9987
	private Game.SavingPreCB activatePreCB;

	// Token: 0x04002704 RID: 9988
	private Game.SavingActiveCB activateActiveCB;

	// Token: 0x04002705 RID: 9989
	private Game.SavingPostCB activatePostCB;

	// Token: 0x04002706 RID: 9990
	[SerializeField]
	public Game.UIColours uiColours = new Game.UIColours();

	// Token: 0x04002707 RID: 9991
	private float lastTimeWorkStarted = float.NegativeInfinity;

	// Token: 0x020017B4 RID: 6068
	[Serializable]
	public struct SavedInfo
	{
		// Token: 0x06009667 RID: 38503 RVA: 0x00361EF5 File Offset: 0x003600F5
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeEmptyVariables();
		}

		// Token: 0x06009668 RID: 38504 RVA: 0x00361EFD File Offset: 0x003600FD
		public void InitializeEmptyVariables()
		{
			if (this.creaturePoopAmount == null)
			{
				this.creaturePoopAmount = new Dictionary<Tag, float>();
			}
			if (this.powerCreatedbyGeneratorType == null)
			{
				this.powerCreatedbyGeneratorType = new Dictionary<Tag, float>();
			}
		}

		// Token: 0x0400736C RID: 29548
		public bool discoveredSurface;

		// Token: 0x0400736D RID: 29549
		public bool discoveredOilField;

		// Token: 0x0400736E RID: 29550
		public bool curedDisease;

		// Token: 0x0400736F RID: 29551
		public bool blockedCometWithBunkerDoor;

		// Token: 0x04007370 RID: 29552
		public Dictionary<Tag, float> creaturePoopAmount;

		// Token: 0x04007371 RID: 29553
		public Dictionary<Tag, float> powerCreatedbyGeneratorType;
	}

	// Token: 0x020017B5 RID: 6069
	public struct CallbackInfo
	{
		// Token: 0x06009669 RID: 38505 RVA: 0x00361F25 File Offset: 0x00360125
		public CallbackInfo(System.Action cb, bool manually_release = false)
		{
			this.cb = cb;
			this.manuallyRelease = manually_release;
		}

		// Token: 0x04007372 RID: 29554
		public System.Action cb;

		// Token: 0x04007373 RID: 29555
		public bool manuallyRelease;
	}

	// Token: 0x020017B6 RID: 6070
	public struct ComplexCallbackInfo<DataType>
	{
		// Token: 0x0600966A RID: 38506 RVA: 0x00361F35 File Offset: 0x00360135
		public ComplexCallbackInfo(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			this.cb = cb;
			this.debugInfo = debug_info;
			this.callbackData = callback_data;
		}

		// Token: 0x04007374 RID: 29556
		public Action<DataType, object> cb;

		// Token: 0x04007375 RID: 29557
		public object callbackData;

		// Token: 0x04007376 RID: 29558
		public string debugInfo;
	}

	// Token: 0x020017B7 RID: 6071
	public class ComplexCallbackHandleVector<DataType>
	{
		// Token: 0x0600966B RID: 38507 RVA: 0x00361F4C File Offset: 0x0036014C
		public ComplexCallbackHandleVector(int initial_size)
		{
			this.baseMgr = new HandleVector<Game.ComplexCallbackInfo<DataType>>(initial_size);
		}

		// Token: 0x0600966C RID: 38508 RVA: 0x00361F6B File Offset: 0x0036016B
		public HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle Add(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			return this.baseMgr.Add(new Game.ComplexCallbackInfo<DataType>(cb, callback_data, debug_info));
		}

		// Token: 0x0600966D RID: 38509 RVA: 0x00361F80 File Offset: 0x00360180
		public Game.ComplexCallbackInfo<DataType> GetItem(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			Game.ComplexCallbackInfo<DataType> item;
			try
			{
				item = this.baseMgr.GetItem(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was already released by " + str, null);
				}
				else
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was released ...... magically", null);
				}
				throw ex;
			}
			return item;
		}

		// Token: 0x0600966E RID: 38510 RVA: 0x00361FF0 File Offset: 0x003601F0
		public Game.ComplexCallbackInfo<DataType> Release(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle, string release_info)
		{
			Game.ComplexCallbackInfo<DataType> result;
			try
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandle(handle, out b, out key);
				this.releaseInfo[key] = release_info;
				result = this.baseMgr.Release(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, release_info + "is trying to release handle but it was already released by " + str, null);
				}
				else
				{
					KCrashReporter.Assert(false, release_info + "is trying to release a handle that was already released by some unknown thing", null);
				}
				throw ex;
			}
			return result;
		}

		// Token: 0x0600966F RID: 38511 RVA: 0x00362084 File Offset: 0x00360284
		public void Clear()
		{
			this.baseMgr.Clear();
		}

		// Token: 0x06009670 RID: 38512 RVA: 0x00362091 File Offset: 0x00360291
		public bool IsVersionValid(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			return this.baseMgr.IsVersionValid(handle);
		}

		// Token: 0x04007377 RID: 29559
		private HandleVector<Game.ComplexCallbackInfo<DataType>> baseMgr;

		// Token: 0x04007378 RID: 29560
		private Dictionary<int, string> releaseInfo = new Dictionary<int, string>();
	}

	// Token: 0x020017B8 RID: 6072
	public enum TemperatureOverlayModes
	{
		// Token: 0x0400737A RID: 29562
		AbsoluteTemperature,
		// Token: 0x0400737B RID: 29563
		AdaptiveTemperature,
		// Token: 0x0400737C RID: 29564
		HeatFlow,
		// Token: 0x0400737D RID: 29565
		StateChange,
		// Token: 0x0400737E RID: 29566
		RelativeTemperature
	}

	// Token: 0x020017B9 RID: 6073
	[Serializable]
	public class ConduitVisInfo
	{
		// Token: 0x0400737F RID: 29567
		public GameObject prefab;

		// Token: 0x04007380 RID: 29568
		[Header("Main View")]
		public Color32 tint;

		// Token: 0x04007381 RID: 29569
		public Color32 insulatedTint;

		// Token: 0x04007382 RID: 29570
		public Color32 radiantTint;

		// Token: 0x04007383 RID: 29571
		[Header("Overlay")]
		public string overlayTintName;

		// Token: 0x04007384 RID: 29572
		public string overlayInsulatedTintName;

		// Token: 0x04007385 RID: 29573
		public string overlayRadiantTintName;

		// Token: 0x04007386 RID: 29574
		public Vector2 overlayMassScaleRange = new Vector2f(1f, 1000f);

		// Token: 0x04007387 RID: 29575
		public Vector2 overlayMassScaleValues = new Vector2f(0.1f, 1f);
	}

	// Token: 0x020017BA RID: 6074
	private class WorldRegion
	{
		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06009672 RID: 38514 RVA: 0x003620DB File Offset: 0x003602DB
		public Vector2I regionMin
		{
			get
			{
				return this.min;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06009673 RID: 38515 RVA: 0x003620E3 File Offset: 0x003602E3
		public Vector2I regionMax
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x06009674 RID: 38516 RVA: 0x003620EC File Offset: 0x003602EC
		public void UpdateGameActiveRegion(int x0, int y0, int x1, int y1)
		{
			this.min.x = Mathf.Max(0, x0);
			this.min.y = Mathf.Max(0, y0);
			this.max.x = Mathf.Max(x1, this.regionMax.x);
			this.max.y = Mathf.Max(y1, this.regionMax.y);
		}

		// Token: 0x06009675 RID: 38517 RVA: 0x00362156 File Offset: 0x00360356
		public void UpdateGameActiveRegion(Vector2I simActiveRegionMin, Vector2I simActiveRegionMax)
		{
			this.min = simActiveRegionMin;
			this.max = simActiveRegionMax;
		}

		// Token: 0x04007388 RID: 29576
		private Vector2I min;

		// Token: 0x04007389 RID: 29577
		private Vector2I max;

		// Token: 0x0400738A RID: 29578
		public bool isActive;
	}

	// Token: 0x020017BB RID: 6075
	public class SimActiveRegion
	{
		// Token: 0x06009677 RID: 38519 RVA: 0x0036216E File Offset: 0x0036036E
		public SimActiveRegion()
		{
			this.region = default(Pair<Vector2I, Vector2I>);
			this.currentSunlightIntensity = (float)FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
			this.currentCosmicRadiationIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}

		// Token: 0x0400738B RID: 29579
		public Pair<Vector2I, Vector2I> region;

		// Token: 0x0400738C RID: 29580
		public float currentSunlightIntensity;

		// Token: 0x0400738D RID: 29581
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x020017BC RID: 6076
	private enum SpawnRotationConfig
	{
		// Token: 0x0400738F RID: 29583
		Normal,
		// Token: 0x04007390 RID: 29584
		StringName
	}

	// Token: 0x020017BD RID: 6077
	[Serializable]
	private struct SpawnRotationData
	{
		// Token: 0x04007391 RID: 29585
		public string animName;

		// Token: 0x04007392 RID: 29586
		public bool flip;
	}

	// Token: 0x020017BE RID: 6078
	[Serializable]
	private struct SpawnPoolData
	{
		// Token: 0x04007393 RID: 29587
		[HashedEnum]
		public SpawnFXHashes id;

		// Token: 0x04007394 RID: 29588
		public int initialCount;

		// Token: 0x04007395 RID: 29589
		public Color32 colour;

		// Token: 0x04007396 RID: 29590
		public GameObject fxPrefab;

		// Token: 0x04007397 RID: 29591
		public string initialAnim;

		// Token: 0x04007398 RID: 29592
		public Vector3 spawnOffset;

		// Token: 0x04007399 RID: 29593
		public Vector2 spawnRandomOffset;

		// Token: 0x0400739A RID: 29594
		public Game.SpawnRotationConfig rotationConfig;

		// Token: 0x0400739B RID: 29595
		public Game.SpawnRotationData[] rotationData;
	}

	// Token: 0x020017BF RID: 6079
	[Serializable]
	private class Settings
	{
		// Token: 0x06009678 RID: 38520 RVA: 0x0036219A File Offset: 0x0036039A
		public Settings(Game game)
		{
			this.nextUniqueID = KPrefabID.NextUniqueID;
			this.gameID = KleiMetrics.GameID();
		}

		// Token: 0x06009679 RID: 38521 RVA: 0x003621B8 File Offset: 0x003603B8
		public Settings()
		{
		}

		// Token: 0x0400739C RID: 29596
		public int nextUniqueID;

		// Token: 0x0400739D RID: 29597
		public int gameID;
	}

	// Token: 0x020017C0 RID: 6080
	public class GameSaveData
	{
		// Token: 0x0400739E RID: 29598
		public ConduitFlow gasConduitFlow;

		// Token: 0x0400739F RID: 29599
		public ConduitFlow liquidConduitFlow;

		// Token: 0x040073A0 RID: 29600
		public FallingWater fallingWater;

		// Token: 0x040073A1 RID: 29601
		public UnstableGroundManager unstableGround;

		// Token: 0x040073A2 RID: 29602
		public WorldDetailSave worldDetail;

		// Token: 0x040073A3 RID: 29603
		public CustomGameSettings customGameSettings;

		// Token: 0x040073A4 RID: 29604
		public StoryManager storySetings;

		// Token: 0x040073A5 RID: 29605
		public SpaceScannerNetworkManager spaceScannerNetworkManager;

		// Token: 0x040073A6 RID: 29606
		public bool debugWasUsed;

		// Token: 0x040073A7 RID: 29607
		public bool autoPrioritizeRoles;

		// Token: 0x040073A8 RID: 29608
		public bool advancedPersonalPriorities;

		// Token: 0x040073A9 RID: 29609
		public Game.SavedInfo savedInfo;

		// Token: 0x040073AA RID: 29610
		public string dateGenerated;

		// Token: 0x040073AB RID: 29611
		public List<uint> changelistsPlayedOn;
	}

	// Token: 0x020017C1 RID: 6081
	// (Invoke) Token: 0x0600967C RID: 38524
	public delegate void CansaveCB();

	// Token: 0x020017C2 RID: 6082
	// (Invoke) Token: 0x06009680 RID: 38528
	public delegate void SavingPreCB(Game.CansaveCB cb);

	// Token: 0x020017C3 RID: 6083
	// (Invoke) Token: 0x06009684 RID: 38532
	public delegate void SavingActiveCB();

	// Token: 0x020017C4 RID: 6084
	// (Invoke) Token: 0x06009688 RID: 38536
	public delegate void SavingPostCB();

	// Token: 0x020017C5 RID: 6085
	[Serializable]
	public struct LocationColours
	{
		// Token: 0x040073AC RID: 29612
		public Color unreachable;

		// Token: 0x040073AD RID: 29613
		public Color invalidLocation;

		// Token: 0x040073AE RID: 29614
		public Color validLocation;

		// Token: 0x040073AF RID: 29615
		public Color requiresRole;

		// Token: 0x040073B0 RID: 29616
		public Color unreachable_requiresRole;
	}

	// Token: 0x020017C6 RID: 6086
	[Serializable]
	public class UIColours
	{
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x0600968B RID: 38539 RVA: 0x003621C8 File Offset: 0x003603C8
		public Game.LocationColours Dig
		{
			get
			{
				return this.digColours;
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600968C RID: 38540 RVA: 0x003621D0 File Offset: 0x003603D0
		public Game.LocationColours Build
		{
			get
			{
				return this.buildColours;
			}
		}

		// Token: 0x040073B1 RID: 29617
		[SerializeField]
		private Game.LocationColours digColours;

		// Token: 0x040073B2 RID: 29618
		[SerializeField]
		private Game.LocationColours buildColours;
	}
}

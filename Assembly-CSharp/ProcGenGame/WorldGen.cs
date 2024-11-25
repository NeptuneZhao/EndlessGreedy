﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Delaunay.Geo;
using Klei;
using Klei.CustomSettings;
using KSerialization;
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;
using ProcGen;
using ProcGen.Map;
using ProcGen.Noise;
using STRINGS;
using UnityEngine;
using VoronoiTree;

namespace ProcGenGame
{
	// Token: 0x02000E10 RID: 3600
	[Serializable]
	public class WorldGen
	{
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06007260 RID: 29280 RVA: 0x002BA051 File Offset: 0x002B8251
		public static string WORLDGEN_SAVE_FILENAME
		{
			get
			{
				return System.IO.Path.Combine(global::Util.RootFolder(), "WorldGenDataSave.worldgen");
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06007261 RID: 29281 RVA: 0x002BA062 File Offset: 0x002B8262
		public static Diseases diseaseStats
		{
			get
			{
				if (WorldGen.m_diseasesDb == null)
				{
					WorldGen.m_diseasesDb = new Diseases(null, true);
				}
				return WorldGen.m_diseasesDb;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06007262 RID: 29282 RVA: 0x002BA07C File Offset: 0x002B827C
		public int BaseLeft
		{
			get
			{
				return this.Settings.GetBaseLocation().left;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06007263 RID: 29283 RVA: 0x002BA08E File Offset: 0x002B828E
		public int BaseRight
		{
			get
			{
				return this.Settings.GetBaseLocation().right;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06007264 RID: 29284 RVA: 0x002BA0A0 File Offset: 0x002B82A0
		public int BaseTop
		{
			get
			{
				return this.Settings.GetBaseLocation().top;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06007265 RID: 29285 RVA: 0x002BA0B2 File Offset: 0x002B82B2
		public int BaseBot
		{
			get
			{
				return this.Settings.GetBaseLocation().bottom;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06007266 RID: 29286 RVA: 0x002BA0C4 File Offset: 0x002B82C4
		// (set) Token: 0x06007267 RID: 29287 RVA: 0x002BA0CC File Offset: 0x002B82CC
		public Data data { get; private set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06007268 RID: 29288 RVA: 0x002BA0D5 File Offset: 0x002B82D5
		public bool HasData
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06007269 RID: 29289 RVA: 0x002BA0E0 File Offset: 0x002B82E0
		public bool HasNoiseData
		{
			get
			{
				return this.HasData && this.data.world != null;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x0600726A RID: 29290 RVA: 0x002BA0FA File Offset: 0x002B82FA
		public float[] DensityMap
		{
			get
			{
				return this.data.world.density;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600726B RID: 29291 RVA: 0x002BA10C File Offset: 0x002B830C
		public float[] HeatMap
		{
			get
			{
				return this.data.world.heatOffset;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600726C RID: 29292 RVA: 0x002BA11E File Offset: 0x002B831E
		public float[] OverrideMap
		{
			get
			{
				return this.data.world.overrides;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600726D RID: 29293 RVA: 0x002BA130 File Offset: 0x002B8330
		public float[] BaseNoiseMap
		{
			get
			{
				return this.data.world.data;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600726E RID: 29294 RVA: 0x002BA142 File Offset: 0x002B8342
		public float[] DefaultTendMap
		{
			get
			{
				return this.data.world.defaultTemp;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x0600726F RID: 29295 RVA: 0x002BA154 File Offset: 0x002B8354
		public Chunk World
		{
			get
			{
				return this.data.world;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06007270 RID: 29296 RVA: 0x002BA161 File Offset: 0x002B8361
		public Vector2I WorldSize
		{
			get
			{
				return this.data.world.size;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06007271 RID: 29297 RVA: 0x002BA173 File Offset: 0x002B8373
		public Vector2I WorldOffset
		{
			get
			{
				return this.data.world.offset;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06007272 RID: 29298 RVA: 0x002BA185 File Offset: 0x002B8385
		public WorldLayout WorldLayout
		{
			get
			{
				return this.data.worldLayout;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06007273 RID: 29299 RVA: 0x002BA192 File Offset: 0x002B8392
		public List<TerrainCell> OverworldCells
		{
			get
			{
				return this.data.overworldCells;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06007274 RID: 29300 RVA: 0x002BA19F File Offset: 0x002B839F
		public List<TerrainCell> TerrainCells
		{
			get
			{
				return this.data.terrainCells;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06007275 RID: 29301 RVA: 0x002BA1AC File Offset: 0x002B83AC
		public List<River> Rivers
		{
			get
			{
				return this.data.rivers;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06007276 RID: 29302 RVA: 0x002BA1B9 File Offset: 0x002B83B9
		public GameSpawnData SpawnData
		{
			get
			{
				return this.data.gameSpawnData;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06007277 RID: 29303 RVA: 0x002BA1C6 File Offset: 0x002B83C6
		public int ChunkEdgeSize
		{
			get
			{
				return this.data.chunkEdgeSize;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06007278 RID: 29304 RVA: 0x002BA1D3 File Offset: 0x002B83D3
		public HashSet<int> ClaimedCells
		{
			get
			{
				return this.claimedCells;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06007279 RID: 29305 RVA: 0x002BA1DB File Offset: 0x002B83DB
		public HashSet<int> HighPriorityClaimedCells
		{
			get
			{
				return this.highPriorityClaims;
			}
		}

		// Token: 0x0600727A RID: 29306 RVA: 0x002BA1E3 File Offset: 0x002B83E3
		public void ClearClaimedCells()
		{
			this.claimedCells.Clear();
			this.highPriorityClaims.Clear();
		}

		// Token: 0x0600727B RID: 29307 RVA: 0x002BA1FB File Offset: 0x002B83FB
		public void AddHighPriorityCells(HashSet<int> cells)
		{
			this.highPriorityClaims.Union(cells);
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x0600727C RID: 29308 RVA: 0x002BA20A File Offset: 0x002B840A
		// (set) Token: 0x0600727D RID: 29309 RVA: 0x002BA212 File Offset: 0x002B8412
		public WorldGenSettings Settings { get; private set; }

		// Token: 0x0600727E RID: 29310 RVA: 0x002BA21C File Offset: 0x002B841C
		public WorldGen(string worldName, List<string> chosenWorldTraits, List<string> chosenStoryTraits, bool assertMissingTraits)
		{
			WorldGen.LoadSettings(false);
			this.Settings = new WorldGenSettings(worldName, chosenWorldTraits, chosenStoryTraits, assertMissingTraits);
			this.data = new Data();
			this.data.chunkEdgeSize = this.Settings.GetIntSetting("ChunkEdgeSize");
		}

		// Token: 0x0600727F RID: 29311 RVA: 0x002BA2A8 File Offset: 0x002B84A8
		public WorldGen(string worldName, Data data, List<string> chosenTraits, List<string> chosenStoryTraits, bool assertMissingTraits)
		{
			WorldGen.LoadSettings(false);
			this.Settings = new WorldGenSettings(worldName, chosenTraits, chosenStoryTraits, assertMissingTraits);
			this.data = data;
		}

		// Token: 0x06007280 RID: 29312 RVA: 0x002BA314 File Offset: 0x002B8514
		public WorldGen(WorldPlacement world, int seed, List<string> chosenWorldTraits, List<string> chosenStoryTraits, bool assertMissingTraits)
		{
			WorldGen.LoadSettings(false);
			this.Settings = new WorldGenSettings(world, seed, chosenWorldTraits, chosenStoryTraits, assertMissingTraits);
			this.data = new Data();
			this.data.chunkEdgeSize = this.Settings.GetIntSetting("ChunkEdgeSize");
		}

		// Token: 0x06007281 RID: 29313 RVA: 0x002BA39F File Offset: 0x002B859F
		public static void SetupDefaultElements()
		{
			WorldGen.voidElement = ElementLoader.FindElementByHash(SimHashes.Void);
			WorldGen.vacuumElement = ElementLoader.FindElementByHash(SimHashes.Vacuum);
			WorldGen.katairiteElement = ElementLoader.FindElementByHash(SimHashes.Katairite);
			WorldGen.unobtaniumElement = ElementLoader.FindElementByHash(SimHashes.Unobtanium);
		}

		// Token: 0x06007282 RID: 29314 RVA: 0x002BA3DD File Offset: 0x002B85DD
		public void Reset()
		{
			this.wasLoaded = false;
		}

		// Token: 0x06007283 RID: 29315 RVA: 0x002BA3E8 File Offset: 0x002B85E8
		public static void LoadSettings(bool in_async_thread = false)
		{
			bool is_playing = Application.isPlaying;
			if (in_async_thread)
			{
				WorldGen.loadSettingsTask = Task.Run(delegate()
				{
					WorldGen.LoadSettings_Internal(is_playing, true);
				});
				return;
			}
			if (WorldGen.loadSettingsTask != null)
			{
				WorldGen.loadSettingsTask.Wait();
				WorldGen.loadSettingsTask = null;
			}
			WorldGen.LoadSettings_Internal(is_playing, false);
		}

		// Token: 0x06007284 RID: 29316 RVA: 0x002BA443 File Offset: 0x002B8643
		public static void WaitForPendingLoadSettings()
		{
			if (WorldGen.loadSettingsTask != null)
			{
				WorldGen.loadSettingsTask.Wait();
				WorldGen.loadSettingsTask = null;
			}
		}

		// Token: 0x06007285 RID: 29317 RVA: 0x002BA45C File Offset: 0x002B865C
		public static IEnumerator ListenForLoadSettingsErrorRoutine()
		{
			while (WorldGen.loadSettingsTask != null)
			{
				if (WorldGen.loadSettingsTask.Exception != null)
				{
					throw WorldGen.loadSettingsTask.Exception;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06007286 RID: 29318 RVA: 0x002BA464 File Offset: 0x002B8664
		private static void LoadSettings_Internal(bool is_playing, bool preloadTemplates = false)
		{
			ListPool<YamlIO.Error, WorldGen>.PooledList pooledList = ListPool<YamlIO.Error, WorldGen>.Allocate();
			if (SettingsCache.LoadFiles(pooledList))
			{
				TemplateCache.Init();
				if (preloadTemplates)
				{
					foreach (ProcGen.World world in SettingsCache.worlds.worldCache.Values)
					{
						if (world.worldTemplateRules != null)
						{
							foreach (ProcGen.World.TemplateSpawnRules templateSpawnRules in world.worldTemplateRules)
							{
								foreach (string templatePath in templateSpawnRules.names)
								{
									TemplateCache.GetTemplate(templatePath);
								}
							}
						}
					}
					foreach (SubWorld subWorld in SettingsCache.subworlds.Values)
					{
						if (subWorld.subworldTemplateRules != null)
						{
							foreach (ProcGen.World.TemplateSpawnRules templateSpawnRules2 in subWorld.subworldTemplateRules)
							{
								foreach (string templatePath2 in templateSpawnRules2.names)
								{
									TemplateCache.GetTemplate(templatePath2);
								}
							}
						}
					}
				}
				if (CustomGameSettings.Instance != null)
				{
					foreach (KeyValuePair<string, WorldMixingSettings> keyValuePair in SettingsCache.worldMixingSettings)
					{
						string key = keyValuePair.Key;
						if (keyValuePair.Value.isModded && CustomGameSettings.Instance.GetWorldMixingSettingForWorldgenFile(key) == null)
						{
							WorldMixingSettingConfig config = new WorldMixingSettingConfig(key, key, null, null, true, -1L);
							CustomGameSettings.Instance.AddMixingSettingsConfig(config);
						}
					}
					foreach (KeyValuePair<string, SubworldMixingSettings> keyValuePair2 in SettingsCache.subworldMixingSettings)
					{
						string key2 = keyValuePair2.Key;
						if (keyValuePair2.Value.isModded && CustomGameSettings.Instance.GetSubworldMixingSettingForWorldgenFile(key2) == null)
						{
							SubworldMixingSettingConfig config2 = new SubworldMixingSettingConfig(key2, key2, null, null, true, -1L);
							CustomGameSettings.Instance.AddMixingSettingsConfig(config2);
						}
					}
				}
			}
			CustomGameSettings.Instance != null;
			if (is_playing)
			{
				Global.Instance.modManager.HandleErrors(pooledList);
			}
			else
			{
				foreach (YamlIO.Error error in pooledList)
				{
					YamlIO.LogError(error, false);
				}
			}
			pooledList.Recycle();
		}

		// Token: 0x06007287 RID: 29319 RVA: 0x002BA78C File Offset: 0x002B898C
		public void InitRandom(int worldSeed, int layoutSeed, int terrainSeed, int noiseSeed)
		{
			this.data.globalWorldSeed = worldSeed;
			this.data.globalWorldLayoutSeed = layoutSeed;
			this.data.globalTerrainSeed = terrainSeed;
			this.data.globalNoiseSeed = noiseSeed;
			this.myRandom = new SeededRandom(worldSeed);
		}

		// Token: 0x06007288 RID: 29320 RVA: 0x002BA7CC File Offset: 0x002B89CC
		public void Initialise(WorldGen.OfflineCallbackFunction callbackFn, Action<OfflineWorldGen.ErrorInfo> error_cb, int worldSeed = -1, int layoutSeed = -1, int terrainSeed = -1, int noiseSeed = -1, bool debug = false, bool skipPlacingTemplates = false)
		{
			if (this.wasLoaded)
			{
				global::Debug.LogError("Initialise called after load");
				return;
			}
			this.successCallbackFn = callbackFn;
			this.errorCallback = error_cb;
			global::Debug.Assert(this.successCallbackFn != null);
			this.isRunningDebugGen = debug;
			this.skipPlacingTemplates = skipPlacingTemplates;
			this.running = false;
			int num = UnityEngine.Random.Range(0, int.MaxValue);
			if (worldSeed == -1)
			{
				worldSeed = num;
			}
			if (layoutSeed == -1)
			{
				layoutSeed = num;
			}
			if (terrainSeed == -1)
			{
				terrainSeed = num;
			}
			if (noiseSeed == -1)
			{
				noiseSeed = num;
			}
			this.data.gameSpawnData = new GameSpawnData();
			this.InitRandom(worldSeed, layoutSeed, terrainSeed, noiseSeed);
			this.successCallbackFn(UI.WORLDGEN.COMPLETE.key, 0f, WorldGenProgressStages.Stages.Failure);
			WorldLayout.SetLayerGradient(SettingsCache.layers.LevelLayers);
		}

		// Token: 0x06007289 RID: 29321 RVA: 0x002BA892 File Offset: 0x002B8A92
		public bool GenerateOffline()
		{
			if (!this.GenerateWorldData())
			{
				this.successCallbackFn(UI.WORLDGEN.FAILED.key, 1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			return true;
		}

		// Token: 0x0600728A RID: 29322 RVA: 0x002BA8BB File Offset: 0x002B8ABB
		private void PlaceTemplateSpawners(Vector2I position, TemplateContainer template, ref Dictionary<int, int> claimedCells)
		{
			this.data.gameSpawnData.AddTemplate(template, position, ref claimedCells);
		}

		// Token: 0x0600728B RID: 29323 RVA: 0x002BA8D0 File Offset: 0x002B8AD0
		public bool RenderOffline(bool doSettle, BinaryWriter writer, ref Sim.Cell[] cells, ref Sim.DiseaseCell[] dc, int baseId, ref List<WorldTrait> placedStoryTraits, bool isStartingWorld = false)
		{
			float[] bgTemp = null;
			dc = null;
			HashSet<int> hashSet = new HashSet<int>();
			this.POIBounds = new List<RectInt>();
			this.WriteOverWorldNoise(this.successCallbackFn);
			if (!this.RenderToMap(this.successCallbackFn, ref cells, ref bgTemp, ref dc, ref hashSet, ref this.POIBounds))
			{
				this.successCallbackFn(UI.WORLDGEN.FAILED.key, -100f, WorldGenProgressStages.Stages.Failure);
				if (!this.isRunningDebugGen)
				{
					return false;
				}
			}
			foreach (int num in hashSet)
			{
				cells[num].SetValues(WorldGen.unobtaniumElement, ElementLoader.elements);
				this.claimedPOICells[num] = 1;
			}
			try
			{
				if (!this.skipPlacingTemplates)
				{
					this.POISpawners = TemplateSpawning.DetermineTemplatesForWorld(this.Settings, this.data.terrainCells, this.myRandom, ref this.POIBounds, this.isRunningDebugGen, ref placedStoryTraits, this.successCallbackFn);
				}
			}
			catch (WorldgenException ex)
			{
				if (!this.isRunningDebugGen)
				{
					this.ReportWorldGenError(ex, ex.userMessage);
					return false;
				}
			}
			catch (Exception e)
			{
				if (!this.isRunningDebugGen)
				{
					this.ReportWorldGenError(e, null);
					return false;
				}
			}
			if (isStartingWorld)
			{
				this.EnsureEnoughElementsInStartingBiome(cells);
			}
			List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.StartWorld);
			foreach (TerrainCell terrainCell in this.OverworldCells)
			{
				foreach (TerrainCell terrainCell2 in terrainCellsForTag)
				{
					if (terrainCell.poly.PointInPolygon(terrainCell2.poly.Centroid()))
					{
						terrainCell.node.tags.Add(WorldGenTags.StartWorld);
						break;
					}
				}
			}
			if (doSettle)
			{
				this.running = WorldGenSimUtil.DoSettleSim(this.Settings, writer, ref cells, ref bgTemp, ref dc, this.successCallbackFn, this.data, this.POISpawners, this.errorCallback, baseId);
			}
			if (!this.skipPlacingTemplates)
			{
				foreach (TemplateSpawning.TemplateSpawner templateSpawner in this.POISpawners)
				{
					this.PlaceTemplateSpawners(templateSpawner.position, templateSpawner.container, ref this.claimedPOICells);
				}
			}
			if (doSettle)
			{
				this.SpawnMobsAndTemplates(cells, bgTemp, dc, new HashSet<int>(this.claimedPOICells.Keys));
			}
			this.successCallbackFn(UI.WORLDGEN.COMPLETE.key, 1f, WorldGenProgressStages.Stages.Complete);
			this.running = false;
			return true;
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x002BABD4 File Offset: 0x002B8DD4
		private void SpawnMobsAndTemplates(Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dc, HashSet<int> claimedCells)
		{
			MobSpawning.DetectNaturalCavities(this.TerrainCells, this.successCallbackFn, cells);
			SeededRandom rnd = new SeededRandom(this.data.globalTerrainSeed);
			for (int i = 0; i < this.TerrainCells.Count; i++)
			{
				float completePercent = (float)i / (float)this.TerrainCells.Count;
				this.successCallbackFn(UI.WORLDGEN.PLACINGCREATURES.key, completePercent, WorldGenProgressStages.Stages.PlacingCreatures);
				TerrainCell tc = this.TerrainCells[i];
				Dictionary<int, string> dictionary = MobSpawning.PlaceFeatureAmbientMobs(this.Settings, tc, rnd, cells, bgTemp, dc, claimedCells, this.isRunningDebugGen);
				if (dictionary != null)
				{
					this.data.gameSpawnData.AddRange(dictionary);
				}
				dictionary = MobSpawning.PlaceBiomeAmbientMobs(this.Settings, tc, rnd, cells, bgTemp, dc, claimedCells, this.isRunningDebugGen);
				if (dictionary != null)
				{
					this.data.gameSpawnData.AddRange(dictionary);
				}
			}
			this.successCallbackFn(UI.WORLDGEN.PLACINGCREATURES.key, 1f, WorldGenProgressStages.Stages.PlacingCreatures);
		}

		// Token: 0x0600728D RID: 29325 RVA: 0x002BACD4 File Offset: 0x002B8ED4
		public void ReportWorldGenError(Exception e, string errorMessage = null)
		{
			if (errorMessage == null)
			{
				errorMessage = UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE;
			}
			bool flag = FileSystem.IsModdedFile(SettingsCache.RewriteWorldgenPathYaml(this.Settings.world.filePath));
			string text = (CustomGameSettings.Instance != null) ? CustomGameSettings.Instance.GetSettingsCoordinate() : this.data.globalWorldLayoutSeed.ToString();
			global::Debug.LogWarning(string.Format("Worldgen Failure on seed {0}, modded={1}", text, flag));
			if (this.errorCallback != null)
			{
				this.errorCallback(new OfflineWorldGen.ErrorInfo
				{
					errorDesc = string.Format(errorMessage, text),
					exception = e
				});
			}
			GenericGameSettings.instance.devAutoWorldGenActive = false;
			if (!flag)
			{
				KCrashReporter.ReportError("WorldgenFailure: ", e.StackTrace, null, null, text + " - " + e.Message, false, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.WORLDGENFAILURE
				}, null);
			}
		}

		// Token: 0x0600728E RID: 29326 RVA: 0x002BADBE File Offset: 0x002B8FBE
		public void SetWorldSize(int width, int height)
		{
			this.data.world = new Chunk(0, 0, width, height);
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x002BADD4 File Offset: 0x002B8FD4
		public Vector2I GetSize()
		{
			return this.data.world.size;
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x002BADE6 File Offset: 0x002B8FE6
		public void SetPosition(Vector2I position)
		{
			this.data.world.offset = position;
		}

		// Token: 0x06007291 RID: 29329 RVA: 0x002BADF9 File Offset: 0x002B8FF9
		public Vector2I GetPosition()
		{
			return this.data.world.offset;
		}

		// Token: 0x06007292 RID: 29330 RVA: 0x002BAE0B File Offset: 0x002B900B
		public void SetClusterLocation(AxialI location)
		{
			this.data.clusterLocation = location;
		}

		// Token: 0x06007293 RID: 29331 RVA: 0x002BAE19 File Offset: 0x002B9019
		public AxialI GetClusterLocation()
		{
			return this.data.clusterLocation;
		}

		// Token: 0x06007294 RID: 29332 RVA: 0x002BAE28 File Offset: 0x002B9028
		public bool GenerateNoiseData(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			try
			{
				this.running = updateProgressFn(UI.WORLDGEN.SETUPNOISE.key, 0f, WorldGenProgressStages.Stages.SetupNoise);
				if (!this.running)
				{
					return false;
				}
				this.SetupNoise(updateProgressFn);
				this.running = updateProgressFn(UI.WORLDGEN.SETUPNOISE.key, 1f, WorldGenProgressStages.Stages.SetupNoise);
				if (!this.running)
				{
					return false;
				}
				this.GenerateUnChunkedNoise(updateProgressFn);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				this.ReportWorldGenError(ex, null);
				WorldGenLogger.LogException(message, stackTrace);
				this.running = this.successCallbackFn(new StringKey("Exception in GenerateNoiseData"), -1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			return true;
		}

		// Token: 0x06007295 RID: 29333 RVA: 0x002BAEEC File Offset: 0x002B90EC
		public bool GenerateLayout(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			try
			{
				this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 0f, WorldGenProgressStages.Stages.WorldLayout);
				if (!this.running)
				{
					return false;
				}
				global::Debug.Assert(this.data.world.size.x != 0 && this.data.world.size.y != 0, "Map size has not been set");
				this.data.worldLayout = new WorldLayout(this, this.data.world.size.x, this.data.world.size.y, this.data.globalWorldLayoutSeed);
				this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 1f, WorldGenProgressStages.Stages.WorldLayout);
				this.data.voronoiTree = null;
				try
				{
					this.data.voronoiTree = this.WorldLayout.GenerateOverworld(this.Settings.world.layoutMethod == ProcGen.World.LayoutMethod.PowerTree, this.isRunningDebugGen);
					this.WorldLayout.PopulateSubworlds();
					this.CompleteLayout(updateProgressFn);
				}
				catch (Exception ex)
				{
					string message = ex.Message;
					string stackTrace = ex.StackTrace;
					WorldGenLogger.LogException(message, stackTrace);
					this.ReportWorldGenError(ex, null);
					this.running = updateProgressFn(new StringKey("Exception in InitVoronoiTree"), -1f, WorldGenProgressStages.Stages.Failure);
					return false;
				}
				this.data.overworldCells = new List<TerrainCell>(40);
				for (int i = 0; i < this.data.voronoiTree.ChildCount(); i++)
				{
					VoronoiTree.Tree tree = this.data.voronoiTree.GetChild(i) as VoronoiTree.Tree;
					Cell node = this.data.worldLayout.overworldGraph.FindNodeByID(tree.site.id);
					this.data.overworldCells.Add(new TerrainCellLogged(node, tree.site, tree.minDistanceToTag));
				}
				this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 1f, WorldGenProgressStages.Stages.WorldLayout);
			}
			catch (Exception ex2)
			{
				string message2 = ex2.Message;
				string stackTrace2 = ex2.StackTrace;
				WorldGenLogger.LogException(message2, stackTrace2);
				this.ReportWorldGenError(ex2, null);
				this.successCallbackFn(new StringKey("Exception in GenerateLayout"), -1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			return true;
		}

		// Token: 0x06007296 RID: 29334 RVA: 0x002BB178 File Offset: 0x002B9378
		public bool CompleteLayout(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			try
			{
				this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0f, WorldGenProgressStages.Stages.CompleteLayout);
				if (!this.running)
				{
					return false;
				}
				this.data.terrainCells = null;
				this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0.65f, WorldGenProgressStages.Stages.CompleteLayout);
				if (!this.running)
				{
					return false;
				}
				this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0.75f, WorldGenProgressStages.Stages.CompleteLayout);
				if (!this.running)
				{
					return false;
				}
				this.data.terrainCells = new List<TerrainCell>(4000);
				List<VoronoiTree.Node> list = new List<VoronoiTree.Node>();
				this.data.voronoiTree.ForceLowestToLeaf();
				this.ApplyStartNode();
				this.ApplySwapTags();
				this.data.voronoiTree.GetLeafNodes(list, null);
				WorldLayout.ResetMapGraphFromVoronoiTree(list, this.WorldLayout.localGraph, true);
				for (int i = 0; i < list.Count; i++)
				{
					VoronoiTree.Node node = list[i];
					Cell tn = this.data.worldLayout.localGraph.FindNodeByID(node.site.id);
					if (tn != null)
					{
						TerrainCell terrainCell = this.data.terrainCells.Find((TerrainCell c) => c.node == tn);
						if (terrainCell == null)
						{
							TerrainCell item = new TerrainCellLogged(tn, node.site, node.parent.minDistanceToTag);
							this.data.terrainCells.Add(item);
						}
						else
						{
							global::Debug.LogWarning("Duplicate cell found" + terrainCell.node.NodeId.ToString());
						}
					}
				}
				for (int j = 0; j < this.data.terrainCells.Count; j++)
				{
					TerrainCell terrainCell2 = this.data.terrainCells[j];
					for (int k = j + 1; k < this.data.terrainCells.Count; k++)
					{
						int num = 0;
						TerrainCell terrainCell3 = this.data.terrainCells[k];
						LineSegment lineSegment;
						if (terrainCell3.poly.SharesEdge(terrainCell2.poly, ref num, out lineSegment) == Polygon.Commonality.Edge)
						{
							terrainCell2.neighbourTerrainCells.Add(k);
							terrainCell3.neighbourTerrainCells.Add(j);
						}
					}
				}
				this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 1f, WorldGenProgressStages.Stages.CompleteLayout);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				WorldGenLogger.LogException(message, stackTrace);
				this.successCallbackFn(new StringKey("Exception in CompleteLayout"), -1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			return true;
		}

		// Token: 0x06007297 RID: 29335 RVA: 0x002BB45C File Offset: 0x002B965C
		public void UpdateVoronoiNodeTags(VoronoiTree.Node node)
		{
			ProcGen.Node node2;
			if (node.tags.Contains(WorldGenTags.Overworld))
			{
				node2 = this.WorldLayout.overworldGraph.FindNodeByID(node.site.id);
			}
			else
			{
				node2 = this.WorldLayout.localGraph.FindNodeByID(node.site.id);
			}
			if (node2 != null)
			{
				node2.tags.Union(node.tags);
			}
		}

		// Token: 0x06007298 RID: 29336 RVA: 0x002BB4CB File Offset: 0x002B96CB
		public bool GenerateWorldData()
		{
			return this.GenerateNoiseData(this.successCallbackFn) && this.GenerateLayout(this.successCallbackFn);
		}

		// Token: 0x06007299 RID: 29337 RVA: 0x002BB4EC File Offset: 0x002B96EC
		public void EnsureEnoughElementsInStartingBiome(Sim.Cell[] cells)
		{
			List<StartingWorldElementSetting> defaultStartingElements = this.Settings.GetDefaultStartingElements();
			List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.StartWorld);
			foreach (StartingWorldElementSetting startingWorldElementSetting in defaultStartingElements)
			{
				float amount = startingWorldElementSetting.amount;
				Element element = ElementLoader.GetElement(new Tag(((SimHashes)Enum.Parse(typeof(SimHashes), startingWorldElementSetting.element, true)).ToString()));
				float num = 0f;
				int num2 = 0;
				foreach (TerrainCell terrainCell in terrainCellsForTag)
				{
					foreach (int num3 in terrainCell.GetAllCells())
					{
						if (element.idx == cells[num3].elementIdx)
						{
							num2++;
							num += cells[num3].mass;
						}
					}
				}
				DebugUtil.DevAssert(num2 > 0, string.Format("No {0} found in starting biome and trying to ensure at least {1}. Skipping.", element.id, amount), null);
				if (num < amount && num2 > 0)
				{
					float num4 = num / (float)num2;
					float num5 = (amount - num) / (float)num2;
					DebugUtil.DevAssert(num4 + num5 <= 2f * element.maxMass, string.Format("Number of cells ({0}) of {1} in the starting biome is insufficient, this will result in extremely dense cells. {2} but expecting less than {3}", new object[]
					{
						num2,
						element.id,
						num4 + num5,
						2f * element.maxMass
					}), null);
					foreach (TerrainCell terrainCell2 in terrainCellsForTag)
					{
						foreach (int num6 in terrainCell2.GetAllCells())
						{
							if (element.idx == cells[num6].elementIdx)
							{
								int num7 = num6;
								cells[num7].mass = cells[num7].mass + num5;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600729A RID: 29338 RVA: 0x002BB7C0 File Offset: 0x002B99C0
		public bool RenderToMap(WorldGen.OfflineCallbackFunction updateProgressFn, ref Sim.Cell[] cells, ref float[] bgTemp, ref Sim.DiseaseCell[] dcs, ref HashSet<int> borderCells, ref List<RectInt> poiBounds)
		{
			global::Debug.Assert(Grid.WidthInCells == this.Settings.world.worldsize.x);
			global::Debug.Assert(Grid.HeightInCells == this.Settings.world.worldsize.y);
			global::Debug.Assert(Grid.CellCount == Grid.WidthInCells * Grid.HeightInCells);
			global::Debug.Assert(Grid.CellSizeInMeters != 0f);
			borderCells = new HashSet<int>();
			cells = new Sim.Cell[Grid.CellCount];
			bgTemp = new float[Grid.CellCount];
			dcs = new Sim.DiseaseCell[Grid.CellCount];
			this.running = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, 0f, WorldGenProgressStages.Stages.ClearingLevel);
			if (!this.running)
			{
				return false;
			}
			for (int i = 0; i < cells.Length; i++)
			{
				cells[i].SetValues(WorldGen.katairiteElement, ElementLoader.elements);
				bgTemp[i] = -1f;
				dcs[i] = default(Sim.DiseaseCell);
				dcs[i].diseaseIdx = byte.MaxValue;
				this.running = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, (float)i / (float)Grid.CellCount, WorldGenProgressStages.Stages.ClearingLevel);
				if (!this.running)
				{
					return false;
				}
			}
			updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, 1f, WorldGenProgressStages.Stages.ClearingLevel);
			try
			{
				this.ProcessByTerrainCell(cells, bgTemp, dcs, updateProgressFn, this.highPriorityClaims);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				WorldGenLogger.LogException(message, stackTrace);
				this.running = updateProgressFn(new StringKey("Exception in ProcessByTerrainCell"), -1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			if (this.Settings.GetBoolSetting("DrawWorldBorder"))
			{
				SeededRandom rnd = new SeededRandom(0);
				this.DrawWorldBorder(cells, this.data.world, rnd, ref borderCells, ref poiBounds, updateProgressFn);
				updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, 1f, WorldGenProgressStages.Stages.DrawWorldBorder);
			}
			this.data.gameSpawnData.baseStartPos = this.data.worldLayout.GetStartLocation();
			return true;
		}

		// Token: 0x0600729B RID: 29339 RVA: 0x002BB9E8 File Offset: 0x002B9BE8
		public SubWorld GetSubWorldForNode(VoronoiTree.Tree node)
		{
			ProcGen.Node node2 = this.WorldLayout.overworldGraph.FindNodeByID(node.site.id);
			if (node2 == null)
			{
				return null;
			}
			if (!this.Settings.HasSubworld(node2.type))
			{
				return null;
			}
			return this.Settings.GetSubWorld(node2.type);
		}

		// Token: 0x0600729C RID: 29340 RVA: 0x002BBA3C File Offset: 0x002B9C3C
		public VoronoiTree.Tree GetOverworldForNode(Leaf leaf)
		{
			if (leaf == null)
			{
				return null;
			}
			return this.data.worldLayout.GetVoronoiTree().GetChildContainingLeaf(leaf);
		}

		// Token: 0x0600729D RID: 29341 RVA: 0x002BBA59 File Offset: 0x002B9C59
		public Leaf GetLeafForTerrainCell(TerrainCell cell)
		{
			if (cell == null)
			{
				return null;
			}
			return this.data.worldLayout.GetVoronoiTree().GetNodeForSite(cell.site) as Leaf;
		}

		// Token: 0x0600729E RID: 29342 RVA: 0x002BBA80 File Offset: 0x002B9C80
		public List<TerrainCell> GetTerrainCellsForTag(Tag tag)
		{
			List<TerrainCell> list = new List<TerrainCell>();
			List<VoronoiTree.Node> leafNodesWithTag = this.WorldLayout.GetLeafNodesWithTag(tag);
			for (int i = 0; i < leafNodesWithTag.Count; i++)
			{
				VoronoiTree.Node node = leafNodesWithTag[i];
				TerrainCell terrainCell = this.data.terrainCells.Find((TerrainCell cell) => cell.site.id == node.site.id);
				if (terrainCell != null)
				{
					list.Add(terrainCell);
				}
			}
			return list;
		}

		// Token: 0x0600729F RID: 29343 RVA: 0x002BBAF0 File Offset: 0x002B9CF0
		private void GetStartCells(out int baseX, out int baseY)
		{
			Vector2I startLocation = new Vector2I(this.data.world.size.x / 2, (int)((float)this.data.world.size.y * 0.7f));
			if (this.data.worldLayout != null)
			{
				startLocation = this.data.worldLayout.GetStartLocation();
			}
			baseX = startLocation.x;
			baseY = startLocation.y;
		}

		// Token: 0x060072A0 RID: 29344 RVA: 0x002BBB68 File Offset: 0x002B9D68
		public void FinalizeStartLocation()
		{
			if (string.IsNullOrEmpty(this.Settings.world.startSubworldName))
			{
				return;
			}
			List<VoronoiTree.Node> startNodes = this.WorldLayout.GetStartNodes();
			global::Debug.Assert(startNodes.Count > 0, "Couldn't find a start node on a world that expects it!!");
			TagSet other = new TagSet
			{
				WorldGenTags.StartLocation
			};
			for (int i = 1; i < startNodes.Count; i++)
			{
				startNodes[i].tags.Remove(other);
			}
		}

		// Token: 0x060072A1 RID: 29345 RVA: 0x002BBBE0 File Offset: 0x002B9DE0
		private void SwitchNodes(VoronoiTree.Node n1, VoronoiTree.Node n2)
		{
			if (n1 is VoronoiTree.Tree || n2 is VoronoiTree.Tree)
			{
				global::Debug.Log("WorldGen::SwitchNodes() Skipping tree node");
				return;
			}
			Diagram.Site site = n1.site;
			n1.site = n2.site;
			n2.site = site;
			Cell cell = this.data.worldLayout.localGraph.FindNodeByID(n1.site.id);
			ProcGen.Node node = this.data.worldLayout.localGraph.FindNodeByID(n2.site.id);
			string type = cell.type;
			cell.SetType(node.type);
			node.SetType(type);
		}

		// Token: 0x060072A2 RID: 29346 RVA: 0x002BBC7C File Offset: 0x002B9E7C
		private void ApplyStartNode()
		{
			List<VoronoiTree.Node> leafNodesWithTag = this.data.worldLayout.GetLeafNodesWithTag(WorldGenTags.StartLocation);
			if (leafNodesWithTag.Count == 0)
			{
				return;
			}
			VoronoiTree.Node node = leafNodesWithTag[0];
			VoronoiTree.Tree parent = node.parent;
			node.parent.AddTagToChildren(WorldGenTags.IgnoreCaveOverride);
			node.parent.tags.Remove(WorldGenTags.StartLocation);
		}

		// Token: 0x060072A3 RID: 29347 RVA: 0x002BBCDC File Offset: 0x002B9EDC
		private void ApplySwapTags()
		{
			List<VoronoiTree.Node> list = new List<VoronoiTree.Node>();
			for (int i = 0; i < this.data.voronoiTree.ChildCount(); i++)
			{
				if (this.data.voronoiTree.GetChild(i).tags.Contains(WorldGenTags.SwapLakesToBelow))
				{
					list.Add(this.data.voronoiTree.GetChild(i));
				}
			}
			foreach (VoronoiTree.Node node in list)
			{
				if (!node.tags.Contains(WorldGenTags.CenteralFeature))
				{
					List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
					((VoronoiTree.Tree)node).GetNodesWithoutTag(WorldGenTags.CenteralFeature, nodes);
					this.SwapNodesAround(WorldGenTags.Wet, true, nodes, node.site.poly.Centroid());
				}
			}
		}

		// Token: 0x060072A4 RID: 29348 RVA: 0x002BBDC8 File Offset: 0x002B9FC8
		private void SwapNodesAround(Tag swapTag, bool sendTagToBottom, List<VoronoiTree.Node> nodes, Vector2 pivot)
		{
			nodes.ShuffleSeeded(this.myRandom.RandomSource());
			List<VoronoiTree.Node> list = new List<VoronoiTree.Node>();
			List<VoronoiTree.Node> list2 = new List<VoronoiTree.Node>();
			foreach (VoronoiTree.Node node in nodes)
			{
				bool flag = node.tags.Contains(swapTag);
				bool flag2 = node.site.poly.Centroid().y > pivot.y;
				bool flag3 = (flag2 && sendTagToBottom) || (!flag2 && !sendTagToBottom);
				if (flag && flag3)
				{
					if (list2.Count > 0)
					{
						this.SwitchNodes(node, list2[0]);
						list2.RemoveAt(0);
					}
					else
					{
						list.Add(node);
					}
				}
				else if (!flag && !flag3)
				{
					if (list.Count > 0)
					{
						this.SwitchNodes(node, list[0]);
						list.RemoveAt(0);
					}
					else
					{
						list2.Add(node);
					}
				}
			}
			if (list2.Count > 0)
			{
				int num = 0;
				while (num < list.Count && list2.Count > 0)
				{
					this.SwitchNodes(list[num], list2[0]);
					list2.RemoveAt(0);
					num++;
				}
			}
		}

		// Token: 0x060072A5 RID: 29349 RVA: 0x002BBF18 File Offset: 0x002BA118
		public void GetElementForBiomePoint(Chunk chunk, ElementBandConfiguration elementBands, Vector2I pos, out Element element, out Sim.PhysicsData pd, out Sim.DiseaseCell dc, float erode)
		{
			TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), null);
			elementOverride = this.GetElementFromBiomeElementTable(chunk, pos, elementBands, erode);
			element = elementOverride.element;
			pd = elementOverride.pdelement;
			dc = elementOverride.dc;
		}

		// Token: 0x060072A6 RID: 29350 RVA: 0x002BBF70 File Offset: 0x002BA170
		public void ConvertIntersectingCellsToType(MathUtil.Pair<Vector2, Vector2> segment, string type)
		{
			List<Vector2I> line = ProcGen.Util.GetLine(segment.First, segment.Second);
			for (int i = 0; i < this.data.terrainCells.Count; i++)
			{
				if (this.data.terrainCells[i].node.type != type)
				{
					for (int j = 0; j < line.Count; j++)
					{
						if (this.data.terrainCells[i].poly.Contains(line[j]))
						{
							this.data.terrainCells[i].node.SetType(type);
						}
					}
				}
			}
		}

		// Token: 0x060072A7 RID: 29351 RVA: 0x002BC028 File Offset: 0x002BA228
		public string GetSubWorldType(Vector2I pos)
		{
			for (int i = 0; i < this.data.overworldCells.Count; i++)
			{
				if (this.data.overworldCells[i].poly.Contains(pos))
				{
					return this.data.overworldCells[i].node.type;
				}
			}
			return null;
		}

		// Token: 0x060072A8 RID: 29352 RVA: 0x002BC090 File Offset: 0x002BA290
		private void ProcessByTerrainCell(Sim.Cell[] map_cells, float[] bgTemp, Sim.DiseaseCell[] dcs, WorldGen.OfflineCallbackFunction updateProgressFn, HashSet<int> hightPriorityCells)
		{
			updateProgressFn(UI.WORLDGEN.PROCESSING.key, 0f, WorldGenProgressStages.Stages.Processing);
			SeededRandom seededRandom = new SeededRandom(this.data.globalTerrainSeed);
			try
			{
				for (int i = 0; i < this.data.terrainCells.Count; i++)
				{
					updateProgressFn(UI.WORLDGEN.PROCESSING.key, (float)i / (float)this.data.terrainCells.Count, WorldGenProgressStages.Stages.Processing);
					this.data.terrainCells[i].Process(this, map_cells, bgTemp, dcs, this.data.world, seededRandom);
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				updateProgressFn(new StringKey("Exception in TerrainCell.Process"), -1f, WorldGenProgressStages.Stages.Failure);
				global::Debug.LogError("Error:" + message + "\n" + stackTrace);
			}
			List<Border> list = new List<Border>();
			updateProgressFn(UI.WORLDGEN.BORDERS.key, 0f, WorldGenProgressStages.Stages.Borders);
			try
			{
				List<Edge> edgesWithTag = this.data.worldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeUnpassable);
				for (int j = 0; j < edgesWithTag.Count; j++)
				{
					Edge edge = edgesWithTag[j];
					List<Cell> cells = this.data.worldLayout.overworldGraph.GetNodes(edge);
					global::Debug.Assert(cells[0] != cells[1], "Both nodes on an arc were the same. Allegedly this means it was a world border but I don't think we do that anymore.");
					TerrainCell terrainCell = this.data.overworldCells.Find((TerrainCell c) => c.node == cells[0]);
					TerrainCell terrainCell2 = this.data.overworldCells.Find((TerrainCell c) => c.node == cells[1]);
					global::Debug.Assert(terrainCell != null && terrainCell2 != null, "NULL Terrainell nodes with EdgeUnpassable");
					terrainCell.LogInfo("BORDER WITH " + terrainCell2.site.id.ToString(), "UNPASSABLE", 0f);
					terrainCell2.LogInfo("BORDER WITH " + terrainCell.site.id.ToString(), "UNPASSABLE", 0f);
					list.Add(new Border(new Neighbors(terrainCell, terrainCell2), edge.corner0.position, edge.corner1.position)
					{
						element = SettingsCache.borders["impenetrable"],
						width = (float)seededRandom.RandomRange(2, 3)
					});
				}
				List<Edge> edgesWithTag2 = this.data.worldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeClosed);
				for (int k = 0; k < edgesWithTag2.Count; k++)
				{
					Edge edge2 = edgesWithTag2[k];
					if (!edgesWithTag.Contains(edge2))
					{
						List<Cell> cells = this.data.worldLayout.overworldGraph.GetNodes(edge2);
						global::Debug.Assert(cells[0] != cells[1], "Both nodes on an arc were the same. Allegedly this means it was a world border but I don't think we do that anymore.");
						TerrainCell terrainCell3 = this.data.overworldCells.Find((TerrainCell c) => c.node == cells[0]);
						TerrainCell terrainCell4 = this.data.overworldCells.Find((TerrainCell c) => c.node == cells[1]);
						global::Debug.Assert(terrainCell3 != null && terrainCell4 != null, "NULL Terraincell nodes with EdgeClosed");
						string borderOverride = this.Settings.GetSubWorld(terrainCell3.node.type).borderOverride;
						string borderOverride2 = this.Settings.GetSubWorld(terrainCell4.node.type).borderOverride;
						string text;
						if (!string.IsNullOrEmpty(borderOverride2) && !string.IsNullOrEmpty(borderOverride))
						{
							int borderOverridePriority = this.Settings.GetSubWorld(terrainCell3.node.type).borderOverridePriority;
							int borderOverridePriority2 = this.Settings.GetSubWorld(terrainCell4.node.type).borderOverridePriority;
							if (borderOverridePriority == borderOverridePriority2)
							{
								text = ((seededRandom.RandomValue() > 0.5f) ? borderOverride2 : borderOverride);
								terrainCell3.LogInfo("BORDER WITH " + terrainCell4.site.id.ToString(), "Picked Random:" + text, 0f);
								terrainCell4.LogInfo("BORDER WITH " + terrainCell3.site.id.ToString(), "Picked Random:" + text, 0f);
							}
							else
							{
								text = ((borderOverridePriority > borderOverridePriority2) ? borderOverride : borderOverride2);
								terrainCell3.LogInfo("BORDER WITH " + terrainCell4.site.id.ToString(), "Picked priority:" + text, 0f);
								terrainCell4.LogInfo("BORDER WITH " + terrainCell3.site.id.ToString(), "Picked priority:" + text, 0f);
							}
						}
						else if (string.IsNullOrEmpty(borderOverride2) && string.IsNullOrEmpty(borderOverride))
						{
							text = "hardToDig";
							terrainCell3.LogInfo("BORDER WITH " + terrainCell4.site.id.ToString(), "Both null", 0f);
							terrainCell4.LogInfo("BORDER WITH " + terrainCell3.site.id.ToString(), "Both null", 0f);
						}
						else
						{
							text = ((!string.IsNullOrEmpty(borderOverride2)) ? borderOverride2 : borderOverride);
							terrainCell3.LogInfo("BORDER WITH " + terrainCell4.site.id.ToString(), "Picked specific " + text, 0f);
							terrainCell4.LogInfo("BORDER WITH " + terrainCell3.site.id.ToString(), "Picked specific " + text, 0f);
						}
						if (!(text == "NONE"))
						{
							Border border = new Border(new Neighbors(terrainCell3, terrainCell4), edge2.corner0.position, edge2.corner1.position);
							border.element = SettingsCache.borders[text];
							MinMax minMax = new MinMax(1.5f, 2f);
							MinMax borderSizeOverride = this.Settings.GetSubWorld(terrainCell3.node.type).borderSizeOverride;
							MinMax borderSizeOverride2 = this.Settings.GetSubWorld(terrainCell4.node.type).borderSizeOverride;
							bool flag = borderSizeOverride.min != 0f || borderSizeOverride.max != 0f;
							bool flag2 = borderSizeOverride2.min != 0f || borderSizeOverride2.max != 0f;
							if (flag && flag2)
							{
								minMax = ((borderSizeOverride.max > borderSizeOverride2.max) ? borderSizeOverride : borderSizeOverride2);
							}
							else if (flag)
							{
								minMax = borderSizeOverride;
							}
							else if (flag2)
							{
								minMax = borderSizeOverride2;
							}
							border.width = seededRandom.RandomRange(minMax.min, minMax.max);
							list.Add(border);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				string message2 = ex2.Message;
				string stackTrace2 = ex2.StackTrace;
				updateProgressFn(new StringKey("Exception in Border creation"), -1f, WorldGenProgressStages.Stages.Failure);
				global::Debug.LogError("Error:" + message2 + " " + stackTrace2);
			}
			try
			{
				if (this.data.world.defaultTemp == null)
				{
					this.data.world.defaultTemp = new float[this.data.world.density.Length];
				}
				for (int l = 0; l < this.data.world.defaultTemp.Length; l++)
				{
					this.data.world.defaultTemp[l] = bgTemp[l];
				}
			}
			catch (Exception ex3)
			{
				string message3 = ex3.Message;
				string stackTrace3 = ex3.StackTrace;
				updateProgressFn(new StringKey("Exception in border.defaultTemp"), -1f, WorldGenProgressStages.Stages.Failure);
				global::Debug.LogError("Error:" + message3 + " " + stackTrace3);
			}
			try
			{
				TerrainCell.SetValuesFunction setValues = delegate(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
				{
					if (!Grid.IsValidCell(index))
					{
						global::Debug.LogError(string.Concat(new string[]
						{
							"Process::SetValuesFunction Index [",
							index.ToString(),
							"] is not valid. cells.Length [",
							map_cells.Length.ToString(),
							"]"
						}));
						return;
					}
					if (this.highPriorityClaims.Contains(index))
					{
						return;
					}
					if ((elem as Element).HasTag(GameTags.Special))
					{
						pd = (elem as Element).defaultValues;
					}
					map_cells[index].SetValues(elem as Element, pd, ElementLoader.elements);
					dcs[index] = dc;
				};
				for (int m = 0; m < list.Count; m++)
				{
					Border border2 = list[m];
					SubWorld subWorld = this.Settings.GetSubWorld(border2.neighbors.n0.node.type);
					SubWorld subWorld2 = this.Settings.GetSubWorld(border2.neighbors.n1.node.type);
					float num = (SettingsCache.temperatures[subWorld.temperatureRange].min + SettingsCache.temperatures[subWorld.temperatureRange].max) / 2f;
					float num2 = (SettingsCache.temperatures[subWorld2.temperatureRange].min + SettingsCache.temperatures[subWorld2.temperatureRange].max) / 2f;
					float num3 = Mathf.Min(SettingsCache.temperatures[subWorld.temperatureRange].min, SettingsCache.temperatures[subWorld2.temperatureRange].min);
					float num4 = Mathf.Max(SettingsCache.temperatures[subWorld.temperatureRange].max, SettingsCache.temperatures[subWorld2.temperatureRange].max);
					float midTemp = (num + num2) / 2f;
					float num5 = num4 - num3;
					float rangeLow = 2f;
					float rangeHigh = 5f;
					int snapLastCells = 1;
					if (num5 >= 150f)
					{
						rangeLow = 0f;
						rangeHigh = border2.width * 0.2f;
						snapLastCells = 2;
						border2.width = Mathf.Max(border2.width, 2f);
						float f = num - 273.15f;
						float f2 = num2 - 273.15f;
						if (Mathf.Abs(f) < Mathf.Abs(f2))
						{
							midTemp = num;
						}
						else
						{
							midTemp = num2;
						}
					}
					border2.Stagger(seededRandom, (float)seededRandom.RandomRange(8, 13), seededRandom.RandomRange(rangeLow, rangeHigh));
					border2.ConvertToMap(this.data.world, setValues, num, num2, midTemp, seededRandom, snapLastCells);
				}
			}
			catch (Exception ex4)
			{
				string message4 = ex4.Message;
				string stackTrace4 = ex4.StackTrace;
				updateProgressFn(new StringKey("Exception in border.ConvertToMap"), -1f, WorldGenProgressStages.Stages.Failure);
				global::Debug.LogError("Error:" + message4 + " " + stackTrace4);
			}
		}

		// Token: 0x060072A9 RID: 29353 RVA: 0x002BCB94 File Offset: 0x002BAD94
		private void DrawWorldBorder(Sim.Cell[] cells, Chunk world, SeededRandom rnd, ref HashSet<int> borderCells, ref List<RectInt> poiBounds, WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			WorldGen.<>c__DisplayClass136_0 CS$<>8__locals1 = new WorldGen.<>c__DisplayClass136_0();
			CS$<>8__locals1.world = world;
			bool boolSetting = this.Settings.GetBoolSetting("DrawWorldBorderForce");
			int intSetting = this.Settings.GetIntSetting("WorldBorderThickness");
			int intSetting2 = this.Settings.GetIntSetting("WorldBorderRange");
			ushort idx = WorldGen.vacuumElement.idx;
			ushort idx2 = WorldGen.voidElement.idx;
			ushort idx3 = WorldGen.unobtaniumElement.idx;
			float temperature = WorldGen.unobtaniumElement.defaultValues.temperature;
			float mass = WorldGen.unobtaniumElement.defaultValues.mass;
			int num = 0;
			int num2 = 0;
			updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, 0f, WorldGenProgressStages.Stages.DrawWorldBorder);
			int num3 = CS$<>8__locals1.world.size.y - 1;
			int num4 = 0;
			int num5 = CS$<>8__locals1.world.size.x - 1;
			List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.RemoveWorldBorderOverVacuum);
			int y;
			int num9;
			for (y = num3; y >= 0; y = num9 - 1)
			{
				updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, (float)y / (float)num3 * 0.33f, WorldGenProgressStages.Stages.DrawWorldBorder);
				num = Mathf.Max(-intSetting2, Mathf.Min(num + rnd.RandomRange(-2, 2), intSetting2));
				bool flag = terrainCellsForTag.Find((TerrainCell n) => n.poly.Contains(new Vector2(0f, (float)y))) != null;
				for (int i = 0; i < intSetting + num; i++)
				{
					int num6 = Grid.XYToCell(i, y);
					if (boolSetting || (cells[num6].elementIdx != idx && cells[num6].elementIdx != idx2 && flag) || !flag)
					{
						borderCells.Add(num6);
						cells[num6].SetValues(idx3, temperature, mass);
						num4 = Mathf.Max(num4, i);
					}
				}
				num2 = Mathf.Max(-intSetting2, Mathf.Min(num2 + rnd.RandomRange(-2, 2), intSetting2));
				bool flag2 = terrainCellsForTag.Find((TerrainCell n) => n.poly.Contains(new Vector2((float)(CS$<>8__locals1.world.size.x - 1), (float)y))) != null;
				for (int j = 0; j < intSetting + num2; j++)
				{
					int num7 = CS$<>8__locals1.world.size.x - 1 - j;
					int num8 = Grid.XYToCell(num7, y);
					if (boolSetting || (cells[num8].elementIdx != idx && cells[num8].elementIdx != idx2 && flag2) || !flag2)
					{
						borderCells.Add(num8);
						cells[num8].SetValues(idx3, temperature, mass);
						num5 = Mathf.Min(num5, num7);
					}
				}
				num9 = y;
			}
			this.POIBounds.Add(new RectInt(0, 0, num4 + 1, this.World.size.y));
			this.POIBounds.Add(new RectInt(num5, 0, CS$<>8__locals1.world.size.x - num5, this.World.size.y));
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = this.World.size.y - 1;
			int x;
			for (x = 0; x < CS$<>8__locals1.world.size.x; x = num9 + 1)
			{
				updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, (float)x / (float)CS$<>8__locals1.world.size.x * 0.66f + 0.33f, WorldGenProgressStages.Stages.DrawWorldBorder);
				num10 = Mathf.Max(-intSetting2, Mathf.Min(num10 + rnd.RandomRange(-2, 2), intSetting2));
				bool flag3 = terrainCellsForTag.Find((TerrainCell n) => n.poly.Contains(new Vector2((float)x, 0f))) != null;
				for (int k = 0; k < intSetting + num10; k++)
				{
					int num14 = Grid.XYToCell(x, k);
					if (boolSetting || (cells[num14].elementIdx != idx && cells[num14].elementIdx != idx2 && flag3) || !flag3)
					{
						borderCells.Add(num14);
						cells[num14].SetValues(idx3, temperature, mass);
						num12 = Mathf.Max(num12, k);
					}
				}
				num11 = Mathf.Max(-intSetting2, Mathf.Min(num11 + rnd.RandomRange(-2, 2), intSetting2));
				bool flag4 = terrainCellsForTag.Find((TerrainCell n) => n.poly.Contains(new Vector2((float)x, (float)(CS$<>8__locals1.world.size.y - 1)))) != null;
				for (int l = 0; l < intSetting + num11; l++)
				{
					int num15 = CS$<>8__locals1.world.size.y - 1 - l;
					int num16 = Grid.XYToCell(x, num15);
					if (boolSetting || (cells[num16].elementIdx != idx && cells[num16].elementIdx != idx2 && flag4) || !flag4)
					{
						borderCells.Add(num16);
						cells[num16].SetValues(idx3, temperature, mass);
						num13 = Mathf.Min(num13, num15);
					}
				}
				num9 = x;
			}
			this.POIBounds.Add(new RectInt(0, 0, this.World.size.x, num12 + 1));
			this.POIBounds.Add(new RectInt(0, num13, this.World.size.x, this.World.size.y - num13));
		}

		// Token: 0x060072AA RID: 29354 RVA: 0x002BD160 File Offset: 0x002BB360
		private void SetupNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			updateProgressFn(UI.WORLDGEN.BUILDNOISESOURCE.key, 0f, WorldGenProgressStages.Stages.SetupNoise);
			this.heatSource = this.BuildNoiseSource(this.data.world.size.x, this.data.world.size.y, "noise/Heat");
			updateProgressFn(UI.WORLDGEN.BUILDNOISESOURCE.key, 1f, WorldGenProgressStages.Stages.SetupNoise);
		}

		// Token: 0x060072AB RID: 29355 RVA: 0x002BD1D8 File Offset: 0x002BB3D8
		public NoiseMapBuilderPlane BuildNoiseSource(int width, int height, string name)
		{
			ProcGen.Noise.Tree tree = SettingsCache.noise.GetTree(name);
			global::Debug.Assert(tree != null, name);
			return this.BuildNoiseSource(width, height, tree);
		}

		// Token: 0x060072AC RID: 29356 RVA: 0x002BD204 File Offset: 0x002BB404
		public NoiseMapBuilderPlane BuildNoiseSource(int width, int height, ProcGen.Noise.Tree tree)
		{
			Vector2f lowerBound = tree.settings.lowerBound;
			Vector2f upperBound = tree.settings.upperBound;
			global::Debug.Assert(lowerBound.x < upperBound.x, string.Concat(new string[]
			{
				"BuildNoiseSource X range broken [l: ",
				lowerBound.x.ToString(),
				" h: ",
				upperBound.x.ToString(),
				"]"
			}));
			global::Debug.Assert(lowerBound.y < upperBound.y, string.Concat(new string[]
			{
				"BuildNoiseSource Y range broken [l: ",
				lowerBound.y.ToString(),
				" h: ",
				upperBound.y.ToString(),
				"]"
			}));
			global::Debug.Assert(width > 0, "BuildNoiseSource width <=0: [" + width.ToString() + "]");
			global::Debug.Assert(height > 0, "BuildNoiseSource height <=0: [" + height.ToString() + "]");
			NoiseMapBuilderPlane noiseMapBuilderPlane = new NoiseMapBuilderPlane(lowerBound.x, upperBound.x, lowerBound.y, upperBound.y, false);
			noiseMapBuilderPlane.SetSize(width, height);
			noiseMapBuilderPlane.SourceModule = tree.BuildFinalModule(this.data.globalNoiseSeed);
			return noiseMapBuilderPlane;
		}

		// Token: 0x060072AD RID: 29357 RVA: 0x002BD34C File Offset: 0x002BB54C
		private void GetMinMaxDataValues(float[] data, int width, int height)
		{
		}

		// Token: 0x060072AE RID: 29358 RVA: 0x002BD350 File Offset: 0x002BB550
		public static NoiseMap BuildNoiseMap(Vector2 offset, float zoom, NoiseMapBuilderPlane nmbp, int width, int height, NoiseMapBuilderCallback cb = null)
		{
			double num = (double)offset.x;
			double num2 = (double)offset.y;
			if (zoom == 0f)
			{
				zoom = 0.01f;
			}
			double num3 = num * (double)zoom;
			double num4 = (num + (double)width) * (double)zoom;
			double num5 = num2 * (double)zoom;
			double num6 = (num2 + (double)height) * (double)zoom;
			NoiseMap noiseMap = new NoiseMap(width, height);
			nmbp.NoiseMap = noiseMap;
			nmbp.SetBounds((float)num3, (float)num4, (float)num5, (float)num6);
			nmbp.CallBack = cb;
			nmbp.Build();
			return noiseMap;
		}

		// Token: 0x060072AF RID: 29359 RVA: 0x002BD3C8 File Offset: 0x002BB5C8
		public static float[] GenerateNoise(Vector2 offset, float zoom, NoiseMapBuilderPlane nmbp, int width, int height, NoiseMapBuilderCallback cb = null)
		{
			NoiseMap noiseMap = WorldGen.BuildNoiseMap(offset, zoom, nmbp, width, height, cb);
			float[] result = new float[noiseMap.Width * noiseMap.Height];
			noiseMap.CopyTo(ref result);
			return result;
		}

		// Token: 0x060072B0 RID: 29360 RVA: 0x002BD400 File Offset: 0x002BB600
		public static void Normalise(float[] data)
		{
			global::Debug.Assert(data != null && data.Length != 0, "MISSING DATA FOR NORMALIZE");
			float num = float.MaxValue;
			float num2 = float.MinValue;
			for (int i = 0; i < data.Length; i++)
			{
				num = Mathf.Min(data[i], num);
				num2 = Mathf.Max(data[i], num2);
			}
			float num3 = num2 - num;
			for (int j = 0; j < data.Length; j++)
			{
				data[j] = (data[j] - num) / num3;
			}
		}

		// Token: 0x060072B1 RID: 29361 RVA: 0x002BD474 File Offset: 0x002BB674
		private void GenerateUnChunkedNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			Vector2 offset = new Vector2(0f, 0f);
			updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, 0f, WorldGenProgressStages.Stages.GenerateNoise);
			NoiseMapBuilderCallback noiseMapBuilderCallback = delegate(int line)
			{
				updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float)((int)(0f + 0.25f * ((float)line / (float)this.data.world.size.y))), WorldGenProgressStages.Stages.GenerateNoise);
			};
			noiseMapBuilderCallback = delegate(int line)
			{
				updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float)((int)(0.25f + 0.25f * ((float)line / (float)this.data.world.size.y))), WorldGenProgressStages.Stages.GenerateNoise);
			};
			if (noiseMapBuilderCallback == null)
			{
				global::Debug.LogError("nupd is null");
			}
			this.data.world.heatOffset = WorldGen.GenerateNoise(offset, SettingsCache.noise.GetZoomForTree("noise/Heat"), this.heatSource, this.data.world.size.x, this.data.world.size.y, noiseMapBuilderCallback);
			this.data.world.data = new float[this.data.world.heatOffset.Length];
			this.data.world.density = new float[this.data.world.heatOffset.Length];
			this.data.world.overrides = new float[this.data.world.heatOffset.Length];
			updateProgressFn(UI.WORLDGEN.NORMALISENOISE.key, 0.5f, WorldGenProgressStages.Stages.GenerateNoise);
			if (SettingsCache.noise.ShouldNormaliseTree("noise/Heat"))
			{
				WorldGen.Normalise(this.data.world.heatOffset);
			}
			updateProgressFn(UI.WORLDGEN.NORMALISENOISE.key, 1f, WorldGenProgressStages.Stages.GenerateNoise);
		}

		// Token: 0x060072B2 RID: 29362 RVA: 0x002BD610 File Offset: 0x002BB810
		public void WriteOverWorldNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			Dictionary<HashedString, WorldGen.NoiseNormalizationStats> dictionary = new Dictionary<HashedString, WorldGen.NoiseNormalizationStats>();
			float num = (float)this.OverworldCells.Count;
			float perCell = 1f / num;
			float currentProgress = 0f;
			foreach (TerrainCell terrainCell in this.OverworldCells)
			{
				ProcGen.Noise.Tree tree = SettingsCache.noise.GetTree("noise/Default");
				ProcGen.Noise.Tree tree2 = SettingsCache.noise.GetTree("noise/DefaultCave");
				ProcGen.Noise.Tree tree3 = SettingsCache.noise.GetTree("noise/DefaultDensity");
				string s = "noise/Default";
				string s2 = "noise/DefaultCave";
				string s3 = "noise/DefaultDensity";
				SubWorld subWorld = this.Settings.GetSubWorld(terrainCell.node.type);
				if (subWorld == null)
				{
					global::Debug.Log("Couldnt find Subworld for overworld node [" + terrainCell.node.type + "] using defaults");
				}
				else
				{
					if (subWorld.biomeNoise != null)
					{
						ProcGen.Noise.Tree tree4 = SettingsCache.noise.GetTree(subWorld.biomeNoise);
						if (tree4 != null)
						{
							tree = tree4;
							s = subWorld.biomeNoise;
						}
					}
					if (subWorld.overrideNoise != null)
					{
						ProcGen.Noise.Tree tree5 = SettingsCache.noise.GetTree(subWorld.overrideNoise);
						if (tree5 != null)
						{
							tree2 = tree5;
							s2 = subWorld.overrideNoise;
						}
					}
					if (subWorld.densityNoise != null)
					{
						ProcGen.Noise.Tree tree6 = SettingsCache.noise.GetTree(subWorld.densityNoise);
						if (tree6 != null)
						{
							tree3 = tree6;
							s3 = subWorld.densityNoise;
						}
					}
				}
				WorldGen.NoiseNormalizationStats noiseNormalizationStats;
				if (!dictionary.TryGetValue(s, out noiseNormalizationStats))
				{
					noiseNormalizationStats = new WorldGen.NoiseNormalizationStats(this.BaseNoiseMap);
					dictionary.Add(s, noiseNormalizationStats);
				}
				WorldGen.NoiseNormalizationStats noiseNormalizationStats2;
				if (!dictionary.TryGetValue(s2, out noiseNormalizationStats2))
				{
					noiseNormalizationStats2 = new WorldGen.NoiseNormalizationStats(this.OverrideMap);
					dictionary.Add(s2, noiseNormalizationStats2);
				}
				WorldGen.NoiseNormalizationStats noiseNormalizationStats3;
				if (!dictionary.TryGetValue(s3, out noiseNormalizationStats3))
				{
					noiseNormalizationStats3 = new WorldGen.NoiseNormalizationStats(this.DensityMap);
					dictionary.Add(s3, noiseNormalizationStats3);
				}
				int width = (int)Mathf.Ceil(terrainCell.poly.bounds.width + 2f);
				int height = (int)Mathf.Ceil(terrainCell.poly.bounds.height + 2f);
				int num2 = (int)Mathf.Floor(terrainCell.poly.bounds.xMin - 1f);
				int num3 = (int)Mathf.Floor(terrainCell.poly.bounds.yMin - 1f);
				Vector2 vector;
				Vector2 offset = vector = new Vector2((float)num2, (float)num3);
				NoiseMapBuilderCallback cb = delegate(int line)
				{
					updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float)((int)(currentProgress + perCell * ((float)line / (float)height))), WorldGenProgressStages.Stages.NoiseMapBuilder);
				};
				NoiseMapBuilderPlane nmbp = this.BuildNoiseSource(width, height, tree);
				NoiseMap noiseMap = WorldGen.BuildNoiseMap(offset, tree.settings.zoom, nmbp, width, height, cb);
				NoiseMapBuilderPlane nmbp2 = this.BuildNoiseSource(width, height, tree2);
				NoiseMap noiseMap2 = WorldGen.BuildNoiseMap(offset, tree2.settings.zoom, nmbp2, width, height, cb);
				NoiseMapBuilderPlane nmbp3 = this.BuildNoiseSource(width, height, tree3);
				NoiseMap noiseMap3 = WorldGen.BuildNoiseMap(offset, tree3.settings.zoom, nmbp3, width, height, cb);
				vector.x = (float)((int)Mathf.Floor(terrainCell.poly.bounds.xMin));
				while (vector.x <= (float)((int)Mathf.Ceil(terrainCell.poly.bounds.xMax)))
				{
					vector.y = (float)((int)Mathf.Floor(terrainCell.poly.bounds.yMin));
					while (vector.y <= (float)((int)Mathf.Ceil(terrainCell.poly.bounds.yMax)))
					{
						if (terrainCell.poly.PointInPolygon(vector))
						{
							int num4 = Grid.XYToCell((int)vector.x, (int)vector.y);
							if (tree.settings.normalise)
							{
								noiseNormalizationStats.cells.Add(num4);
							}
							if (tree2.settings.normalise)
							{
								noiseNormalizationStats2.cells.Add(num4);
							}
							if (tree3.settings.normalise)
							{
								noiseNormalizationStats3.cells.Add(num4);
							}
							int x = (int)vector.x - num2;
							int y = (int)vector.y - num3;
							this.BaseNoiseMap[num4] = noiseMap.GetValue(x, y);
							this.OverrideMap[num4] = noiseMap2.GetValue(x, y);
							this.DensityMap[num4] = noiseMap3.GetValue(x, y);
							noiseNormalizationStats.min = Mathf.Min(this.BaseNoiseMap[num4], noiseNormalizationStats.min);
							noiseNormalizationStats.max = Mathf.Max(this.BaseNoiseMap[num4], noiseNormalizationStats.max);
							noiseNormalizationStats2.min = Mathf.Min(this.OverrideMap[num4], noiseNormalizationStats2.min);
							noiseNormalizationStats2.max = Mathf.Max(this.OverrideMap[num4], noiseNormalizationStats2.max);
							noiseNormalizationStats3.min = Mathf.Min(this.DensityMap[num4], noiseNormalizationStats3.min);
							noiseNormalizationStats3.max = Mathf.Max(this.DensityMap[num4], noiseNormalizationStats3.max);
						}
						vector.y += 1f;
					}
					vector.x += 1f;
				}
			}
			foreach (KeyValuePair<HashedString, WorldGen.NoiseNormalizationStats> keyValuePair in dictionary)
			{
				float num5 = keyValuePair.Value.max - keyValuePair.Value.min;
				foreach (int num6 in keyValuePair.Value.cells)
				{
					keyValuePair.Value.noise[num6] = (keyValuePair.Value.noise[num6] - keyValuePair.Value.min) / num5;
				}
			}
		}

		// Token: 0x060072B3 RID: 29363 RVA: 0x002BDCB8 File Offset: 0x002BBEB8
		private float GetValue(Chunk chunk, Vector2I pos)
		{
			int num = pos.x + this.data.world.size.x * pos.y;
			if (num < 0 || num >= chunk.data.Length)
			{
				throw new ArgumentOutOfRangeException("chunkDataIndex [" + num.ToString() + "]", "chunk data length [" + chunk.data.Length.ToString() + "]");
			}
			return chunk.data[num];
		}

		// Token: 0x060072B4 RID: 29364 RVA: 0x002BDD3C File Offset: 0x002BBF3C
		public bool InChunkRange(Chunk chunk, Vector2I pos)
		{
			int num = pos.x + this.data.world.size.x * pos.y;
			return num >= 0 && num < chunk.data.Length;
		}

		// Token: 0x060072B5 RID: 29365 RVA: 0x002BDD7F File Offset: 0x002BBF7F
		private TerrainCell.ElementOverride GetElementFromBiomeElementTable(Chunk chunk, Vector2I pos, List<ElementGradient> table, float erode)
		{
			return WorldGen.GetElementFromBiomeElementTable(this.GetValue(chunk, pos) * erode, table);
		}

		// Token: 0x060072B6 RID: 29366 RVA: 0x002BDD94 File Offset: 0x002BBF94
		public static TerrainCell.ElementOverride GetElementFromBiomeElementTable(float value, List<ElementGradient> table)
		{
			TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), null);
			if (table.Count == 0)
			{
				return elementOverride;
			}
			for (int i = 0; i < table.Count; i++)
			{
				global::Debug.Assert(table[i].content != null, i.ToString());
				if (value < table[i].maxValue)
				{
					return TerrainCell.GetElementOverride(table[i].content, table[i].overrides);
				}
			}
			return TerrainCell.GetElementOverride(table[table.Count - 1].content, table[table.Count - 1].overrides);
		}

		// Token: 0x060072B7 RID: 29367 RVA: 0x002BDE50 File Offset: 0x002BC050
		public static bool CanLoad(string fileName)
		{
			if (fileName == null || fileName == "")
			{
				return false;
			}
			bool result;
			try
			{
				if (File.Exists(fileName))
				{
					using (BinaryReader binaryReader = new BinaryReader(File.Open(fileName, FileMode.Open)))
					{
						return binaryReader.BaseStream.CanRead;
					}
				}
				result = false;
			}
			catch (FileNotFoundException)
			{
				result = false;
			}
			catch (Exception ex)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Failed to read " + fileName + "\n" + ex.ToString()
				});
				result = false;
			}
			return result;
		}

		// Token: 0x060072B8 RID: 29368 RVA: 0x002BDEF8 File Offset: 0x002BC0F8
		public static WorldGen Load(IReader reader, bool defaultDiscovered)
		{
			WorldGen result;
			try
			{
				WorldGenSave worldGenSave = new WorldGenSave();
				Deserializer.Deserialize(worldGenSave, reader);
				WorldGen worldGen = new WorldGen(worldGenSave.worldID, worldGenSave.data, worldGenSave.traitIDs, worldGenSave.storyTraitIDs, false);
				worldGen.isStartingWorld = true;
				if (worldGenSave.version.x != 1 || worldGenSave.version.y > 1)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						string.Concat(new string[]
						{
							"LoadWorldGenSim Error! Wrong save version Current: [",
							1.ToString(),
							".",
							1.ToString(),
							"] File: [",
							worldGenSave.version.x.ToString(),
							".",
							worldGenSave.version.y.ToString(),
							"]"
						})
					});
					worldGen.wasLoaded = false;
				}
				else
				{
					worldGen.wasLoaded = true;
				}
				result = worldGen;
			}
			catch (Exception ex)
			{
				DebugUtil.LogErrorArgs(new object[]
				{
					"WorldGen.Load Error!\n",
					ex.Message,
					ex.StackTrace
				});
				result = null;
			}
			return result;
		}

		// Token: 0x060072B9 RID: 29369 RVA: 0x002BE02C File Offset: 0x002BC22C
		public void DrawDebug()
		{
		}

		// Token: 0x04004ED4 RID: 20180
		private const string _WORLDGEN_SAVE_FILENAME = "WorldGenDataSave.worldgen";

		// Token: 0x04004ED5 RID: 20181
		private const int heatScale = 2;

		// Token: 0x04004ED6 RID: 20182
		private const int UNPASSABLE_EDGE_COUNT = 4;

		// Token: 0x04004ED7 RID: 20183
		private const string heat_noise_name = "noise/Heat";

		// Token: 0x04004ED8 RID: 20184
		private const string default_base_noise_name = "noise/Default";

		// Token: 0x04004ED9 RID: 20185
		private const string default_cave_noise_name = "noise/DefaultCave";

		// Token: 0x04004EDA RID: 20186
		private const string default_density_noise_name = "noise/DefaultDensity";

		// Token: 0x04004EDB RID: 20187
		public const int WORLDGEN_SAVE_MAJOR_VERSION = 1;

		// Token: 0x04004EDC RID: 20188
		public const int WORLDGEN_SAVE_MINOR_VERSION = 1;

		// Token: 0x04004EDD RID: 20189
		private const float EXTREME_TEMPERATURE_BORDER_RANGE = 150f;

		// Token: 0x04004EDE RID: 20190
		private const float EXTREME_TEMPERATURE_BORDER_MIN_WIDTH = 2f;

		// Token: 0x04004EDF RID: 20191
		public static Element voidElement;

		// Token: 0x04004EE0 RID: 20192
		public static Element vacuumElement;

		// Token: 0x04004EE1 RID: 20193
		public static Element katairiteElement;

		// Token: 0x04004EE2 RID: 20194
		public static Element unobtaniumElement;

		// Token: 0x04004EE3 RID: 20195
		private static Diseases m_diseasesDb;

		// Token: 0x04004EE4 RID: 20196
		public bool isRunningDebugGen;

		// Token: 0x04004EE5 RID: 20197
		public bool skipPlacingTemplates;

		// Token: 0x04004EE7 RID: 20199
		private HashSet<int> claimedCells = new HashSet<int>();

		// Token: 0x04004EE8 RID: 20200
		public Dictionary<int, int> claimedPOICells = new Dictionary<int, int>();

		// Token: 0x04004EE9 RID: 20201
		private HashSet<int> highPriorityClaims = new HashSet<int>();

		// Token: 0x04004EEA RID: 20202
		public List<RectInt> POIBounds = new List<RectInt>();

		// Token: 0x04004EEB RID: 20203
		public List<TemplateSpawning.TemplateSpawner> POISpawners;

		// Token: 0x04004EEC RID: 20204
		private WorldGen.OfflineCallbackFunction successCallbackFn;

		// Token: 0x04004EED RID: 20205
		private bool running = true;

		// Token: 0x04004EEE RID: 20206
		private Action<OfflineWorldGen.ErrorInfo> errorCallback;

		// Token: 0x04004EEF RID: 20207
		private SeededRandom myRandom;

		// Token: 0x04004EF0 RID: 20208
		private NoiseMapBuilderPlane heatSource;

		// Token: 0x04004EF2 RID: 20210
		private bool wasLoaded;

		// Token: 0x04004EF3 RID: 20211
		public int polyIndex = -1;

		// Token: 0x04004EF4 RID: 20212
		public bool isStartingWorld;

		// Token: 0x04004EF5 RID: 20213
		public bool isModuleInterior;

		// Token: 0x04004EF6 RID: 20214
		private static Task loadSettingsTask;

		// Token: 0x02001F2D RID: 7981
		// (Invoke) Token: 0x0600AD92 RID: 44434
		public delegate bool OfflineCallbackFunction(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage);

		// Token: 0x02001F2E RID: 7982
		public enum GenerateSection
		{
			// Token: 0x04008CD1 RID: 36049
			SolarSystem,
			// Token: 0x04008CD2 RID: 36050
			WorldNoise,
			// Token: 0x04008CD3 RID: 36051
			WorldLayout,
			// Token: 0x04008CD4 RID: 36052
			RenderToMap,
			// Token: 0x04008CD5 RID: 36053
			CollectSpawners
		}

		// Token: 0x02001F2F RID: 7983
		private class NoiseNormalizationStats
		{
			// Token: 0x0600AD95 RID: 44437 RVA: 0x003A9A52 File Offset: 0x003A7C52
			public NoiseNormalizationStats(float[] noise)
			{
				this.noise = noise;
			}

			// Token: 0x04008CD6 RID: 36054
			public float[] noise;

			// Token: 0x04008CD7 RID: 36055
			public float min = float.MaxValue;

			// Token: 0x04008CD8 RID: 36056
			public float max = float.MinValue;

			// Token: 0x04008CD9 RID: 36057
			public HashSet<int> cells = new HashSet<int>();
		}
	}
}

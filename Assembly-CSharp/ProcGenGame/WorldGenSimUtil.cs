﻿using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using KSerialization;
using ProcGen;
using STRINGS;
using TemplateClasses;

namespace ProcGenGame
{
	// Token: 0x02000E12 RID: 3602
	public static class WorldGenSimUtil
	{
		// Token: 0x060072D2 RID: 29394 RVA: 0x002BF9D8 File Offset: 0x002BDBD8
		public unsafe static bool DoSettleSim(WorldGenSettings settings, BinaryWriter writer, ref Sim.Cell[] cells, ref float[] bgTemp, ref Sim.DiseaseCell[] dcs, WorldGen.OfflineCallbackFunction updateProgressFn, Data data, List<TemplateSpawning.TemplateSpawner> templateSpawnTargets, Action<OfflineWorldGen.ErrorInfo> error_cb, int baseId)
		{
			Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
			SimMessages.CreateSimElementsTable(ElementLoader.elements);
			SimMessages.CreateDiseaseTable(WorldGen.diseaseStats);
			SimMessages.SimDataInitializeFromCells(Grid.WidthInCells, Grid.HeightInCells, cells, bgTemp, dcs, true);
			updateProgressFn(UI.WORLDGEN.SETTLESIM.key, 0f, WorldGenProgressStages.Stages.SettleSim);
			Sim.Start();
			byte[] array = new byte[Grid.CellCount];
			for (int i = 0; i < Grid.CellCount; i++)
			{
				array[i] = byte.MaxValue;
			}
			Vector2I a = new Vector2I(0, 0);
			Vector2I size = data.world.size;
			List<Game.SimActiveRegion> list = new List<Game.SimActiveRegion>();
			list.Add(new Game.SimActiveRegion
			{
				region = new Pair<Vector2I, Vector2I>(a, size)
			});
			for (int j = 0; j < 500; j++)
			{
				if (j == 498)
				{
					HashSet<int> hashSet = new HashSet<int>();
					if (templateSpawnTargets != null)
					{
						foreach (TemplateSpawning.TemplateSpawner templateSpawner in templateSpawnTargets)
						{
							if (templateSpawner.container.cells != null)
							{
								for (int k = 0; k < templateSpawner.container.cells.Count; k++)
								{
									Cell cell = templateSpawner.container.cells[k];
									int num = Grid.OffsetCell(Grid.XYToCell(templateSpawner.position.x, templateSpawner.position.y), cell.location_x, cell.location_y);
									if (Grid.IsValidCell(num) && !hashSet.Contains(num))
									{
										hashSet.Add(num);
										ushort elementIndex = ElementLoader.GetElementIndex(cell.element);
										float temperature = cell.temperature;
										float mass = cell.mass;
										byte index = WorldGen.diseaseStats.GetIndex(cell.diseaseName);
										int diseaseCount = cell.diseaseCount;
										SimMessages.ModifyCell(num, elementIndex, temperature, mass, index, diseaseCount, SimMessages.ReplaceType.Replace, false, -1);
									}
								}
							}
						}
					}
				}
				SimMessages.NewGameFrame(0.2f, list);
				IntPtr intPtr = Sim.HandleMessage(SimMessageHashes.PrepareGameData, array.Length, array);
				updateProgressFn(UI.WORLDGEN.SETTLESIM.key, (float)j / 500f, WorldGenProgressStages.Stages.SettleSim);
				if (intPtr == IntPtr.Zero)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Unexpected"
					});
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
					for (int l = 0; l < ptr->numSubstanceChangeInfo; l++)
					{
						Sim.SubstanceChangeInfo substanceChangeInfo = ptr->substanceChangeInfo[l];
						int cellIdx = substanceChangeInfo.cellIdx;
						cells[cellIdx].elementIdx = ptr->elementIdx[cellIdx];
						cells[cellIdx].insulation = ptr->insulation[cellIdx];
						cells[cellIdx].properties = ptr->properties[cellIdx];
						cells[cellIdx].temperature = ptr->temperature[cellIdx];
						cells[cellIdx].mass = ptr->mass[cellIdx];
						cells[cellIdx].strengthInfo = ptr->strengthInfo[cellIdx];
						dcs[cellIdx].diseaseIdx = ptr->diseaseIdx[cellIdx];
						dcs[cellIdx].elementCount = ptr->diseaseCount[cellIdx];
						Grid.Element[cellIdx] = ElementLoader.elements[(int)substanceChangeInfo.newElemIdx];
					}
					for (int m = 0; m < ptr->numSolidInfo; m++)
					{
						Sim.SolidInfo solidInfo = ptr->solidInfo[m];
						bool solid = solidInfo.isSolid != 0;
						Grid.SetSolid(solidInfo.cellIdx, solid, null);
					}
				}
			}
			bool result = WorldGenSimUtil.SaveSim(writer, data, baseId, error_cb);
			Sim.Shutdown();
			return result;
		}

		// Token: 0x060072D3 RID: 29395 RVA: 0x002BFE88 File Offset: 0x002BE088
		private static bool SaveSim(BinaryWriter writer, Data data, int baseId, Action<OfflineWorldGen.ErrorInfo> error_cb)
		{
			bool result;
			try
			{
				Manager.Clear();
				SimSaveFileStructure simSaveFileStructure = new SimSaveFileStructure();
				for (int i = 0; i < data.overworldCells.Count; i++)
				{
					simSaveFileStructure.worldDetail.overworldCells.Add(new WorldDetailSave.OverworldCell(SettingsCache.GetCachedSubWorld(data.overworldCells[i].node.type).zoneType, data.overworldCells[i]));
				}
				simSaveFileStructure.worldDetail.globalWorldSeed = data.globalWorldSeed;
				simSaveFileStructure.worldDetail.globalWorldLayoutSeed = data.globalWorldLayoutSeed;
				simSaveFileStructure.worldDetail.globalTerrainSeed = data.globalTerrainSeed;
				simSaveFileStructure.worldDetail.globalNoiseSeed = data.globalNoiseSeed;
				simSaveFileStructure.WidthInCells = Grid.WidthInCells;
				simSaveFileStructure.HeightInCells = Grid.HeightInCells;
				simSaveFileStructure.x = data.world.offset.x;
				simSaveFileStructure.y = data.world.offset.y;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						Sim.Save(binaryWriter, simSaveFileStructure.x, simSaveFileStructure.y);
					}
					simSaveFileStructure.Sim = memoryStream.ToArray();
				}
				try
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						using (BinaryWriter binaryWriter2 = new BinaryWriter(memoryStream2))
						{
							Serializer.Serialize(simSaveFileStructure, binaryWriter2);
						}
						Manager.SerializeDirectory(writer);
						writer.Write(memoryStream2.ToArray());
					}
				}
				catch (Exception ex)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						"Couldn't serialize",
						ex.Message,
						ex.StackTrace
					});
				}
				result = true;
			}
			catch (Exception ex2)
			{
				error_cb(new OfflineWorldGen.ErrorInfo
				{
					errorDesc = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, WorldGen.WORLDGEN_SAVE_FILENAME),
					exception = ex2
				});
				DebugUtil.LogErrorArgs(new object[]
				{
					"Couldn't write",
					ex2.Message,
					ex2.StackTrace
				});
				result = false;
			}
			return result;
		}

		// Token: 0x060072D4 RID: 29396 RVA: 0x002C0134 File Offset: 0x002BE334
		public static void LoadSim(IReader reader, int baseCount, List<SimSaveFileStructure> loadedWorlds)
		{
			try
			{
				for (int num = 0; num != baseCount; num++)
				{
					SimSaveFileStructure simSaveFileStructure = new SimSaveFileStructure();
					Manager.DeserializeDirectory(reader);
					Deserializer.Deserialize(simSaveFileStructure, reader);
					if (simSaveFileStructure.worldDetail == null)
					{
						Debug.LogError("Detail is null for world " + num.ToString());
					}
					else
					{
						loadedWorlds.Add(simSaveFileStructure);
					}
				}
			}
			catch (Exception ex)
			{
				DebugUtil.LogErrorArgs(new object[]
				{
					"LoadSim Error!\n",
					ex.Message,
					ex.StackTrace
				});
			}
		}

		// Token: 0x04004F0C RID: 20236
		private const int STEPS = 500;
	}
}

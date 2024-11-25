using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using KSerialization;
using ProcGen;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000AE8 RID: 2792
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SpacecraftManager")]
public class SpacecraftManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x0600530E RID: 21262 RVA: 0x001DC718 File Offset: 0x001DA918
	public static void DestroyInstance()
	{
		SpacecraftManager.instance = null;
	}

	// Token: 0x0600530F RID: 21263 RVA: 0x001DC720 File Offset: 0x001DA920
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SpacecraftManager.instance = this;
		if (this.savedSpacecraftDestinations == null)
		{
			this.savedSpacecraftDestinations = new Dictionary<int, int>();
		}
	}

	// Token: 0x06005310 RID: 21264 RVA: 0x001DC744 File Offset: 0x001DA944
	private void GenerateFixedDestinations()
	{
		SpaceDestinationTypes spaceDestinationTypes = Db.Get().SpaceDestinationTypes;
		if (this.destinations != null)
		{
			return;
		}
		this.destinations = new List<SpaceDestination>
		{
			new SpaceDestination(0, spaceDestinationTypes.CarbonaceousAsteroid.Id, 0),
			new SpaceDestination(1, spaceDestinationTypes.CarbonaceousAsteroid.Id, 0),
			new SpaceDestination(2, spaceDestinationTypes.MetallicAsteroid.Id, 1),
			new SpaceDestination(3, spaceDestinationTypes.RockyAsteroid.Id, 2),
			new SpaceDestination(4, spaceDestinationTypes.IcyDwarf.Id, 3),
			new SpaceDestination(5, spaceDestinationTypes.OrganicDwarf.Id, 4)
		};
	}

	// Token: 0x06005311 RID: 21265 RVA: 0x001DC800 File Offset: 0x001DAA00
	private void GenerateRandomDestinations()
	{
		KRandom krandom = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed);
		SpaceDestinationTypes spaceDestinationTypes = Db.Get().SpaceDestinationTypes;
		List<List<string>> list = new List<List<string>>
		{
			new List<string>(),
			new List<string>
			{
				spaceDestinationTypes.OilyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.Satellite.Id
			},
			new List<string>
			{
				spaceDestinationTypes.Satellite.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.ForestPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.SaltDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.OrganicDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.OrganicDwarf.Id,
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.ChlorinePlanet.Id,
				spaceDestinationTypes.RedDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.GasGiant.Id,
				spaceDestinationTypes.IceGiant.Id,
				spaceDestinationTypes.RustPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GasGiant.Id,
				spaceDestinationTypes.IceGiant.Id,
				spaceDestinationTypes.HydrogenGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.RustPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.MetallicAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.ShinyPlanet.Id,
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GoldAsteroid.Id,
				spaceDestinationTypes.OrganicDwarf.Id,
				spaceDestinationTypes.ForestPlanet.Id,
				spaceDestinationTypes.ChlorinePlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.VolcanoPlanet.Id,
				spaceDestinationTypes.IceGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.ShinyPlanet.Id,
				spaceDestinationTypes.RedDwarf.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.GasGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.HydrogenGiant.Id,
				spaceDestinationTypes.ForestPlanet.Id,
				spaceDestinationTypes.OilyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GoldAsteroid.Id,
				spaceDestinationTypes.SaltDwarf.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id
			}
		};
		List<int> list2 = new List<int>();
		int num = 3;
		int minValue = 15;
		int maxValue = 25;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Count != 0)
			{
				for (int j = 0; j < num; j++)
				{
					list2.Add(i);
				}
			}
		}
		SpacecraftManager.<>c__DisplayClass12_0 CS$<>8__locals1;
		CS$<>8__locals1.nextId = this.destinations.Count;
		int num2 = krandom.Next(minValue, maxValue);
		List<SpaceDestination> list3 = new List<SpaceDestination>();
		for (int k = 0; k < num2; k++)
		{
			int index = krandom.Next(0, list2.Count - 1);
			int num3 = list2[index];
			list2.RemoveAt(index);
			List<string> list4 = list[num3];
			string type = list4[krandom.Next(0, list4.Count)];
			SpaceDestination item = new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), type, num3);
			list3.Add(item);
		}
		list2.ShuffleSeeded(krandom);
		List<SpaceDestination> list5 = new List<SpaceDestination>();
		foreach (string name in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			DlcMixingSettings cachedDlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(name);
			if (cachedDlcMixingSettings != null)
			{
				foreach (DlcMixingSettings.SpaceDestinationMix spaceDestinationMix in cachedDlcMixingSettings.spaceDesinations)
				{
					bool flag = false;
					if (list2.Count > 0)
					{
						for (int l = 0; l < list2.Count; l++)
						{
							int num4 = list2[l];
							if (num4 >= spaceDestinationMix.minTier && num4 <= spaceDestinationMix.maxTier)
							{
								SpaceDestination item2 = new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), spaceDestinationMix.type, num4);
								list5.Add(item2);
								list2.RemoveAt(l);
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						for (int m = 0; m < list3.Count; m++)
						{
							SpaceDestination spaceDestination = list3[m];
							if (spaceDestination.distance >= spaceDestinationMix.minTier && spaceDestination.distance <= spaceDestinationMix.maxTier)
							{
								list3[m] = new SpaceDestination(spaceDestination.id, spaceDestinationMix.type, spaceDestination.distance);
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						KCrashReporter.ReportDevNotification("Base game failed to mix a space destination", Environment.StackTrace, "", false, null);
						UnityEngine.Debug.LogWarning("Mixing: Unable to place destination '" + spaceDestinationMix.type + "'");
					}
				}
			}
		}
		this.destinations.AddRange(list3);
		this.destinations.Add(new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), Db.Get().SpaceDestinationTypes.Earth.Id, 4));
		this.destinations.Add(new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), Db.Get().SpaceDestinationTypes.Wormhole.Id, list.Count));
		this.destinations.AddRange(list5);
	}

	// Token: 0x06005312 RID: 21266 RVA: 0x001DD000 File Offset: 0x001DB200
	private void RestoreDestinations()
	{
		if (this.destinationsGenerated)
		{
			return;
		}
		this.GenerateFixedDestinations();
		this.GenerateRandomDestinations();
		this.destinations.Sort((SpaceDestination a, SpaceDestination b) => a.distance.CompareTo(b.distance));
		List<float> list = new List<float>();
		for (int i = 0; i < 10; i++)
		{
			list.Add((float)i / 10f);
		}
		for (int j = 0; j < 20; j++)
		{
			list.Shuffle<float>();
			int num = 0;
			foreach (SpaceDestination spaceDestination in this.destinations)
			{
				if (spaceDestination.distance == j)
				{
					num++;
					spaceDestination.startingOrbitPercentage = list[num];
				}
			}
		}
		this.destinationsGenerated = true;
	}

	// Token: 0x06005313 RID: 21267 RVA: 0x001DD0E8 File Offset: 0x001DB2E8
	public SpaceDestination GetSpacecraftDestination(LaunchConditionManager lcm)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(lcm);
		return this.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id);
	}

	// Token: 0x06005314 RID: 21268 RVA: 0x001DD109 File Offset: 0x001DB309
	public SpaceDestination GetSpacecraftDestination(int spacecraftID)
	{
		this.CleanSavedSpacecraftDestinations();
		if (this.savedSpacecraftDestinations.ContainsKey(spacecraftID))
		{
			return this.GetDestination(this.savedSpacecraftDestinations[spacecraftID]);
		}
		return null;
	}

	// Token: 0x06005315 RID: 21269 RVA: 0x001DD134 File Offset: 0x001DB334
	public List<int> GetSpacecraftsForDestination(SpaceDestination destination)
	{
		this.CleanSavedSpacecraftDestinations();
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> keyValuePair in this.savedSpacecraftDestinations)
		{
			if (keyValuePair.Value == destination.id)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06005316 RID: 21270 RVA: 0x001DD1AC File Offset: 0x001DB3AC
	private void CleanSavedSpacecraftDestinations()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> keyValuePair in this.savedSpacecraftDestinations)
		{
			bool flag = false;
			using (List<Spacecraft>.Enumerator enumerator2 = this.spacecraft.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.id == keyValuePair.Key)
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			using (List<SpaceDestination>.Enumerator enumerator3 = this.destinations.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					if (enumerator3.Current.id == keyValuePair.Value)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag || !flag2)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (int key in list)
		{
			this.savedSpacecraftDestinations.Remove(key);
		}
	}

	// Token: 0x06005317 RID: 21271 RVA: 0x001DD300 File Offset: 0x001DB500
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.spacecraftManager = this;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			global::Debug.Assert(this.spacecraft == null || this.spacecraft.Count == 0);
			return;
		}
		this.RestoreDestinations();
	}

	// Token: 0x06005318 RID: 21272 RVA: 0x001DD340 File Offset: 0x001DB540
	public void SetSpacecraftDestination(LaunchConditionManager lcm, SpaceDestination destination)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(lcm);
		this.savedSpacecraftDestinations[spacecraftFromLaunchConditionManager.id] = destination.id;
	}

	// Token: 0x06005319 RID: 21273 RVA: 0x001DD36C File Offset: 0x001DB56C
	public int GetSpacecraftID(ILaunchableRocket rocket)
	{
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			if (spacecraft.launchConditions.gameObject == rocket.LaunchableGameObject)
			{
				return spacecraft.id;
			}
		}
		return -1;
	}

	// Token: 0x0600531A RID: 21274 RVA: 0x001DD3DC File Offset: 0x001DB5DC
	public SpaceDestination GetDestination(int destinationID)
	{
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			if (spaceDestination.id == destinationID)
			{
				return spaceDestination;
			}
		}
		global::Debug.LogErrorFormat("No space destination with ID {0}", new object[]
		{
			destinationID
		});
		return null;
	}

	// Token: 0x0600531B RID: 21275 RVA: 0x001DD454 File Offset: 0x001DB654
	public void RegisterSpacecraft(Spacecraft craft)
	{
		if (this.spacecraft.Contains(craft))
		{
			return;
		}
		if (craft.HasInvalidID())
		{
			craft.SetID(this.nextSpacecraftID);
			this.nextSpacecraftID++;
		}
		this.spacecraft.Add(craft);
	}

	// Token: 0x0600531C RID: 21276 RVA: 0x001DD494 File Offset: 0x001DB694
	public void UnregisterSpacecraft(LaunchConditionManager conditionManager)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(conditionManager);
		spacecraftFromLaunchConditionManager.SetState(Spacecraft.MissionState.Destroyed);
		this.spacecraft.Remove(spacecraftFromLaunchConditionManager);
	}

	// Token: 0x0600531D RID: 21277 RVA: 0x001DD4BD File Offset: 0x001DB6BD
	public List<Spacecraft> GetSpacecraft()
	{
		return this.spacecraft;
	}

	// Token: 0x0600531E RID: 21278 RVA: 0x001DD4C8 File Offset: 0x001DB6C8
	public Spacecraft GetSpacecraftFromLaunchConditionManager(LaunchConditionManager lcm)
	{
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			if (spacecraft.launchConditions == lcm)
			{
				return spacecraft;
			}
		}
		return null;
	}

	// Token: 0x0600531F RID: 21279 RVA: 0x001DD52C File Offset: 0x001DB72C
	public void Sim1000ms(float dt)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			spacecraft.ProgressMission(dt);
		}
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			spaceDestination.Replenish(dt);
		}
	}

	// Token: 0x06005320 RID: 21280 RVA: 0x001DD5C8 File Offset: 0x001DB7C8
	public void PushReadyToLandNotification(Spacecraft spacecraft)
	{
		Notification notification = new Notification(BUILDING.STATUSITEMS.SPACECRAFTREADYTOLAND.NOTIFICATION, NotificationType.Good, delegate(List<Notification> notificationList, object data)
		{
			string text = BUILDING.STATUSITEMS.SPACECRAFTREADYTOLAND.NOTIFICATION_TOOLTIP;
			foreach (Notification notification2 in notificationList)
			{
				text = text + "\n" + (string)notification2.tooltipData;
			}
			return text;
		}, "• " + spacecraft.rocketName, true, 0f, null, null, null, true, false, false);
		spacecraft.launchConditions.gameObject.AddOrGet<Notifier>().Add(notification, "");
	}

	// Token: 0x06005321 RID: 21281 RVA: 0x001DD63C File Offset: 0x001DB83C
	private void SpawnMissionResults(Dictionary<SimHashes, float> results)
	{
		foreach (KeyValuePair<SimHashes, float> keyValuePair in results)
		{
			ElementLoader.FindElementByHash(keyValuePair.Key).substance.SpawnResource(PlayerController.GetCursorPos(KInputManager.GetMousePos()), keyValuePair.Value, 300f, 0, 0, false, false, false);
		}
	}

	// Token: 0x06005322 RID: 21282 RVA: 0x001DD6B8 File Offset: 0x001DB8B8
	public float GetDestinationAnalysisScore(SpaceDestination destination)
	{
		return this.GetDestinationAnalysisScore(destination.id);
	}

	// Token: 0x06005323 RID: 21283 RVA: 0x001DD6C6 File Offset: 0x001DB8C6
	public float GetDestinationAnalysisScore(int destinationID)
	{
		if (this.destinationAnalysisScores.ContainsKey(destinationID))
		{
			return this.destinationAnalysisScores[destinationID];
		}
		return 0f;
	}

	// Token: 0x06005324 RID: 21284 RVA: 0x001DD6E8 File Offset: 0x001DB8E8
	public void EarnDestinationAnalysisPoints(int destinationID, float points)
	{
		if (!this.destinationAnalysisScores.ContainsKey(destinationID))
		{
			this.destinationAnalysisScores.Add(destinationID, 0f);
		}
		SpaceDestination destination = this.GetDestination(destinationID);
		SpacecraftManager.DestinationAnalysisState destinationAnalysisState = this.GetDestinationAnalysisState(destination);
		Dictionary<int, float> dictionary = this.destinationAnalysisScores;
		dictionary[destinationID] += points;
		SpacecraftManager.DestinationAnalysisState destinationAnalysisState2 = this.GetDestinationAnalysisState(destination);
		if (destinationAnalysisState != destinationAnalysisState2)
		{
			int starmapAnalysisDestinationID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
			if (starmapAnalysisDestinationID == destinationID)
			{
				if (destinationAnalysisState2 == SpacecraftManager.DestinationAnalysisState.Complete)
				{
					if (SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID).type == Db.Get().SpaceDestinationTypes.Earth.Id)
					{
						Game.Instance.unlocks.Unlock("earth", true);
					}
					if (SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID).type == Db.Get().SpaceDestinationTypes.Wormhole.Id)
					{
						Game.Instance.unlocks.Unlock("wormhole", true);
					}
					SpacecraftManager.instance.SetStarmapAnalysisDestinationID(-1);
				}
				base.Trigger(532901469, null);
			}
		}
	}

	// Token: 0x06005325 RID: 21285 RVA: 0x001DD800 File Offset: 0x001DBA00
	public SpacecraftManager.DestinationAnalysisState GetDestinationAnalysisState(SpaceDestination destination)
	{
		if (destination.startAnalyzed)
		{
			return SpacecraftManager.DestinationAnalysisState.Complete;
		}
		float destinationAnalysisScore = this.GetDestinationAnalysisScore(destination);
		if (destinationAnalysisScore >= (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE)
		{
			return SpacecraftManager.DestinationAnalysisState.Complete;
		}
		if (destinationAnalysisScore >= (float)ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED)
		{
			return SpacecraftManager.DestinationAnalysisState.Discovered;
		}
		return SpacecraftManager.DestinationAnalysisState.Hidden;
	}

	// Token: 0x06005326 RID: 21286 RVA: 0x001DD838 File Offset: 0x001DBA38
	public bool AreAllDestinationsAnalyzed()
	{
		foreach (SpaceDestination destination in this.destinations)
		{
			if (this.GetDestinationAnalysisState(destination) != SpacecraftManager.DestinationAnalysisState.Complete)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005327 RID: 21287 RVA: 0x001DD898 File Offset: 0x001DBA98
	public void DEBUG_RevealStarmap()
	{
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			this.EarnDestinationAnalysisPoints(spaceDestination.id, (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE);
		}
	}

	// Token: 0x06005328 RID: 21288 RVA: 0x001DD8F8 File Offset: 0x001DBAF8
	public void SetStarmapAnalysisDestinationID(int id)
	{
		this.analyzeDestinationID = id;
		base.Trigger(532901469, id);
	}

	// Token: 0x06005329 RID: 21289 RVA: 0x001DD912 File Offset: 0x001DBB12
	public int GetStarmapAnalysisDestinationID()
	{
		return this.analyzeDestinationID;
	}

	// Token: 0x0600532A RID: 21290 RVA: 0x001DD91A File Offset: 0x001DBB1A
	public bool HasAnalysisTarget()
	{
		return this.analyzeDestinationID != -1;
	}

	// Token: 0x0600532C RID: 21292 RVA: 0x001DD950 File Offset: 0x001DBB50
	[CompilerGenerated]
	internal static int <GenerateRandomDestinations>g__GetNextID|12_0(ref SpacecraftManager.<>c__DisplayClass12_0 A_0)
	{
		int nextId = A_0.nextId;
		A_0.nextId = nextId + 1;
		return nextId;
	}

	// Token: 0x040036D5 RID: 14037
	public static SpacecraftManager instance;

	// Token: 0x040036D6 RID: 14038
	[Serialize]
	private List<Spacecraft> spacecraft = new List<Spacecraft>();

	// Token: 0x040036D7 RID: 14039
	[Serialize]
	private int nextSpacecraftID;

	// Token: 0x040036D8 RID: 14040
	public const int INVALID_DESTINATION_ID = -1;

	// Token: 0x040036D9 RID: 14041
	[Serialize]
	private int analyzeDestinationID = -1;

	// Token: 0x040036DA RID: 14042
	[Serialize]
	public bool hasVisitedWormHole;

	// Token: 0x040036DB RID: 14043
	[Serialize]
	public List<SpaceDestination> destinations;

	// Token: 0x040036DC RID: 14044
	[Serialize]
	public Dictionary<int, int> savedSpacecraftDestinations;

	// Token: 0x040036DD RID: 14045
	[Serialize]
	public bool destinationsGenerated;

	// Token: 0x040036DE RID: 14046
	[Serialize]
	public Dictionary<int, float> destinationAnalysisScores = new Dictionary<int, float>();

	// Token: 0x02001B3B RID: 6971
	public enum DestinationAnalysisState
	{
		// Token: 0x04007F2B RID: 32555
		Hidden,
		// Token: 0x04007F2C RID: 32556
		Discovered,
		// Token: 0x04007F2D RID: 32557
		Complete
	}
}

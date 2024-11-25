using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200053C RID: 1340
public class ColonyDiagnosticUtility : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06001E97 RID: 7831 RVA: 0x000AA75C File Offset: 0x000A895C
	public static void DestroyInstance()
	{
		ColonyDiagnosticUtility.Instance = null;
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x000AA764 File Offset: 0x000A8964
	public ColonyDiagnostic.DiagnosticResult.Opinion GetWorldDiagnosticResult(int worldID)
	{
		ColonyDiagnostic.DiagnosticResult.Opinion opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Good;
		foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else
				{
					opinion = (ColonyDiagnostic.DiagnosticResult.Opinion)Math.Min((int)opinion, (int)colonyDiagnostic.LatestResult.opinion);
				}
			}
		}
		return opinion;
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x000AA820 File Offset: 0x000A8A20
	public string GetWorldDiagnosticResultStatus(int worldID)
	{
		ColonyDiagnostic colonyDiagnostic = null;
		foreach (ColonyDiagnostic colonyDiagnostic2 in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic2.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else if (colonyDiagnostic == null || colonyDiagnostic2.LatestResult.opinion < colonyDiagnostic.LatestResult.opinion)
				{
					colonyDiagnostic = colonyDiagnostic2;
				}
			}
		}
		if (colonyDiagnostic == null || colonyDiagnostic.LatestResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			return "";
		}
		return colonyDiagnostic.name;
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x000AA900 File Offset: 0x000A8B00
	public string GetWorldDiagnosticResultTooltip(int worldID)
	{
		string text = "";
		foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else if (colonyDiagnostic.LatestResult.opinion < ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
				{
					text = text + "\n" + colonyDiagnostic.LatestResult.GetFormattedMessage();
				}
			}
		}
		return text;
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x000AA9DC File Offset: 0x000A8BDC
	public bool IsDiagnosticTutorialDisabled(string id)
	{
		return ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id) && GameClock.Instance.GetTime() < ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id];
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x000AAA0F File Offset: 0x000A8C0F
	public void ClearDiagnosticTutorialSetting(string id)
	{
		if (ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id))
		{
			ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id] = -1f;
		}
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x000AAA38 File Offset: 0x000A8C38
	public bool IsCriteriaEnabled(int worldID, string diagnosticID, string criteriaID)
	{
		Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
		return dictionary.ContainsKey(diagnosticID) && !dictionary[diagnosticID].Contains(criteriaID);
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x000AAA70 File Offset: 0x000A8C70
	public void SetCriteriaEnabled(int worldID, string diagnosticID, string criteriaID, bool enabled)
	{
		Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
		global::Debug.Assert(dictionary.ContainsKey(diagnosticID), string.Format("Trying to set criteria on World {0} lacks diagnostic {1} that criteria {2} relates to", worldID, diagnosticID, criteriaID));
		List<string> list = dictionary[diagnosticID];
		if (enabled && list.Contains(criteriaID))
		{
			list.Remove(criteriaID);
		}
		if (!enabled && !list.Contains(criteriaID))
		{
			list.Add(criteriaID);
		}
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x000AAAD7 File Offset: 0x000A8CD7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ColonyDiagnosticUtility.Instance = this;
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x000AAAE8 File Offset: 0x000A8CE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 33))
		{
			string key = "IdleDiagnostic";
			foreach (int num in this.diagnosticDisplaySettings.Keys)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(num);
				if (this.diagnosticDisplaySettings[num].ContainsKey(key) && this.diagnosticDisplaySettings[num][key] != ColonyDiagnosticUtility.DisplaySetting.Always)
				{
					this.diagnosticDisplaySettings[num][key] = (world.IsModuleInterior ? ColonyDiagnosticUtility.DisplaySetting.Never : ColonyDiagnosticUtility.DisplaySetting.AlertOnly);
				}
			}
		}
		foreach (int worldID in ClusterManager.Instance.GetWorldIDsSorted())
		{
			this.AddWorld(worldID);
		}
		ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x000AAC34 File Offset: 0x000A8E34
	private void Refresh(object data)
	{
		int worldID = (int)data;
		this.AddWorld(worldID);
	}

	// Token: 0x06001EA2 RID: 7842 RVA: 0x000AAC50 File Offset: 0x000A8E50
	private void RemoveWorld(object data)
	{
		int key = (int)data;
		if (this.diagnosticDisplaySettings.Remove(key))
		{
			List<ColonyDiagnostic> list;
			if (this.worldDiagnostics.TryGetValue(key, out list))
			{
				foreach (ColonyDiagnostic colonyDiagnostic in list)
				{
					colonyDiagnostic.OnCleanUp();
				}
			}
			this.worldDiagnostics.Remove(key);
		}
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x000AACD0 File Offset: 0x000A8ED0
	public ColonyDiagnostic GetDiagnostic(string id, int worldID)
	{
		return this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match.id == id);
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x000AAD07 File Offset: 0x000A8F07
	public T GetDiagnostic<T>(int worldID) where T : ColonyDiagnostic
	{
		return (T)((object)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is T));
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x000AAD40 File Offset: 0x000A8F40
	public string GetDiagnosticName(string id)
	{
		foreach (KeyValuePair<int, List<ColonyDiagnostic>> keyValuePair in this.worldDiagnostics)
		{
			foreach (ColonyDiagnostic colonyDiagnostic in keyValuePair.Value)
			{
				if (colonyDiagnostic.id == id)
				{
					return colonyDiagnostic.name;
				}
			}
		}
		global::Debug.LogWarning("Cannot locate name of diagnostic " + id + " because no worlds have a diagnostic with that id ");
		return "";
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x000AAE00 File Offset: 0x000A9000
	public ChoreGroupDiagnostic GetChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup)
	{
		return (ChoreGroupDiagnostic)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is ChoreGroupDiagnostic && ((ChoreGroupDiagnostic)match).choreGroup == choreGroup);
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x000AAE3C File Offset: 0x000A903C
	public WorkTimeDiagnostic GetWorkTimeDiagnostic(int worldID, ChoreGroup choreGroup)
	{
		return (WorkTimeDiagnostic)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is WorkTimeDiagnostic && ((WorkTimeDiagnostic)match).choreGroup == choreGroup);
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x000AAE78 File Offset: 0x000A9078
	private void TryAddDiagnosticToWorldCollection(ref List<ColonyDiagnostic> newWorldDiagnostics, ColonyDiagnostic newDiagnostic)
	{
		if (!DlcManager.IsDlcListValidForCurrentContent(newDiagnostic.GetDlcIds()))
		{
			return;
		}
		newWorldDiagnostics.Add(newDiagnostic);
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x000AAE90 File Offset: 0x000A9090
	public void AddWorld(int worldID)
	{
		bool flag = false;
		if (!this.diagnosticDisplaySettings.ContainsKey(worldID))
		{
			this.diagnosticDisplaySettings.Add(worldID, new Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>());
			flag = true;
		}
		if (!this.diagnosticCriteriaDisabled.ContainsKey(worldID))
		{
			this.diagnosticCriteriaDisabled.Add(worldID, new Dictionary<string, List<string>>());
		}
		List<ColonyDiagnostic> list = new List<ColonyDiagnostic>();
		this.TryAddDiagnosticToWorldCollection(ref list, new BreathabilityDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new FoodDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new StressDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new RadiationDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new ReactorDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new IdleDiagnostic(worldID));
		if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
		{
			this.TryAddDiagnosticToWorldCollection(ref list, new FloatingRocketDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketFuelDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketOxidizerDiagnostic(worldID));
		}
		else
		{
			this.TryAddDiagnosticToWorldCollection(ref list, new BedDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new ToiletDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new PowerUseDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new BatteryDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new TrappedDuplicantDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new FarmDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new EntombedDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketsInOrbitDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new MeteorDiagnostic(worldID));
		}
		this.worldDiagnostics.Add(worldID, list);
		foreach (ColonyDiagnostic colonyDiagnostic in list)
		{
			if (!this.diagnosticDisplaySettings[worldID].ContainsKey(colonyDiagnostic.id))
			{
				this.diagnosticDisplaySettings[worldID].Add(colonyDiagnostic.id, ColonyDiagnosticUtility.DisplaySetting.AlertOnly);
			}
			if (!this.diagnosticCriteriaDisabled[worldID].ContainsKey(colonyDiagnostic.id))
			{
				this.diagnosticCriteriaDisabled[worldID].Add(colonyDiagnostic.id, new List<string>());
			}
		}
		if (flag)
		{
			this.diagnosticDisplaySettings[worldID]["BreathabilityDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID]["FoodDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID]["StressDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
			{
				this.diagnosticDisplaySettings[worldID]["FloatingRocketDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["RocketFuelDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["RocketOxidizerDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["IdleDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Never;
				return;
			}
			this.diagnosticDisplaySettings[worldID]["IdleDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.AlertOnly;
		}
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x000AB174 File Offset: 0x000A9374
	public void Sim1000ms(float dt)
	{
		if (ColonyDiagnosticUtility.IgnoreFirstUpdate)
		{
			ColonyDiagnosticUtility.IgnoreFirstUpdate = false;
		}
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x000AB184 File Offset: 0x000A9384
	public static bool PastNewBuildingGracePeriod(Transform building)
	{
		BuildingComplete component = building.GetComponent<BuildingComplete>();
		return !(component != null) || GameClock.Instance.GetTime() - component.creationTime >= 600f;
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x000AB1BC File Offset: 0x000A93BC
	public static bool IgnoreRocketsWithNoCrewRequested(int worldID, out ColonyDiagnostic.DiagnosticResult result)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
		string message = world.IsModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, message, null);
		if (world.IsModuleInterior)
		{
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				WorldContainer interiorWorld = Components.Clustercrafts[i].ModuleInterface.GetInteriorWorld();
				if (!(interiorWorld == null) && interiorWorld.id == worldID)
				{
					PassengerRocketModule passengerModule = Components.Clustercrafts[i].ModuleInterface.GetPassengerModule();
					if (passengerModule != null && !passengerModule.ShouldCrewGetIn())
					{
						result = default(ColonyDiagnostic.DiagnosticResult);
						result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
						result.Message = UI.COLONY_DIAGNOSTICS.NO_MINIONS_REQUESTED;
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0400113C RID: 4412
	public static ColonyDiagnosticUtility Instance;

	// Token: 0x0400113D RID: 4413
	private Dictionary<int, List<ColonyDiagnostic>> worldDiagnostics = new Dictionary<int, List<ColonyDiagnostic>>();

	// Token: 0x0400113E RID: 4414
	[Serialize]
	public Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>> diagnosticDisplaySettings = new Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>>();

	// Token: 0x0400113F RID: 4415
	[Serialize]
	public Dictionary<int, Dictionary<string, List<string>>> diagnosticCriteriaDisabled = new Dictionary<int, Dictionary<string, List<string>>>();

	// Token: 0x04001140 RID: 4416
	[Serialize]
	private Dictionary<string, float> diagnosticTutorialStatus = new Dictionary<string, float>
	{
		{
			"ToiletDiagnostic",
			450f
		},
		{
			"BedDiagnostic",
			900f
		},
		{
			"BreathabilityDiagnostic",
			1800f
		},
		{
			"FoodDiagnostic",
			3000f
		},
		{
			"FarmDiagnostic",
			6000f
		},
		{
			"StressDiagnostic",
			9000f
		},
		{
			"PowerUseDiagnostic",
			12000f
		},
		{
			"BatteryDiagnostic",
			12000f
		},
		{
			"IdleDiagnostic",
			600f
		}
	};

	// Token: 0x04001141 RID: 4417
	public static bool IgnoreFirstUpdate = true;

	// Token: 0x04001142 RID: 4418
	public static ColonyDiagnostic.DiagnosticResult NoDataResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x020012F7 RID: 4855
	public enum DisplaySetting
	{
		// Token: 0x0400652A RID: 25898
		Always,
		// Token: 0x0400652B RID: 25899
		AlertOnly,
		// Token: 0x0400652C RID: 25900
		Never,
		// Token: 0x0400652D RID: 25901
		LENGTH
	}
}

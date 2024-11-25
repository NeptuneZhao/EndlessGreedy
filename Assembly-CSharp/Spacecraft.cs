using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000AE7 RID: 2791
[SerializationConfig(MemberSerialization.OptIn)]
public class Spacecraft
{
	// Token: 0x060052FA RID: 21242 RVA: 0x001DC221 File Offset: 0x001DA421
	public Spacecraft(LaunchConditionManager launchConditions)
	{
		this.launchConditions = launchConditions;
	}

	// Token: 0x060052FB RID: 21243 RVA: 0x001DC252 File Offset: 0x001DA452
	public Spacecraft()
	{
	}

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x060052FC RID: 21244 RVA: 0x001DC27C File Offset: 0x001DA47C
	// (set) Token: 0x060052FD RID: 21245 RVA: 0x001DC289 File Offset: 0x001DA489
	public LaunchConditionManager launchConditions
	{
		get
		{
			return this.refLaunchConditions.Get();
		}
		set
		{
			this.refLaunchConditions.Set(value);
		}
	}

	// Token: 0x060052FE RID: 21246 RVA: 0x001DC297 File Offset: 0x001DA497
	public void SetRocketName(string newName)
	{
		this.rocketName = newName;
		this.UpdateNameOnRocketModules();
	}

	// Token: 0x060052FF RID: 21247 RVA: 0x001DC2A6 File Offset: 0x001DA4A6
	public string GetRocketName()
	{
		return this.rocketName;
	}

	// Token: 0x06005300 RID: 21248 RVA: 0x001DC2B0 File Offset: 0x001DA4B0
	public void UpdateNameOnRocketModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				component.SetParentRocketName(this.rocketName);
			}
		}
	}

	// Token: 0x06005301 RID: 21249 RVA: 0x001DC320 File Offset: 0x001DA520
	public bool HasInvalidID()
	{
		return this.id == -1;
	}

	// Token: 0x06005302 RID: 21250 RVA: 0x001DC32B File Offset: 0x001DA52B
	public void SetID(int id)
	{
		this.id = id;
	}

	// Token: 0x06005303 RID: 21251 RVA: 0x001DC334 File Offset: 0x001DA534
	public void SetState(Spacecraft.MissionState state)
	{
		this.state = state;
	}

	// Token: 0x06005304 RID: 21252 RVA: 0x001DC33D File Offset: 0x001DA53D
	public void BeginMission(SpaceDestination destination)
	{
		this.missionElapsed = 0f;
		this.missionDuration = (float)destination.OneBasedDistance * ROCKETRY.MISSION_DURATION_SCALE / this.GetPilotNavigationEfficiency();
		this.SetState(Spacecraft.MissionState.Launching);
	}

	// Token: 0x06005305 RID: 21253 RVA: 0x001DC36C File Offset: 0x001DA56C
	private float GetPilotNavigationEfficiency()
	{
		float num = 1f;
		if (!this.launchConditions.GetComponent<CommandModule>().robotPilotControlled)
		{
			List<MinionStorage.Info> storedMinionInfo = this.launchConditions.GetComponent<MinionStorage>().GetStoredMinionInfo();
			if (storedMinionInfo.Count < 1)
			{
				return 1f;
			}
			StoredMinionIdentity component = storedMinionInfo[0].serializedMinion.Get().GetComponent<StoredMinionIdentity>();
			string b = Db.Get().Attributes.SpaceNavigation.Id;
			using (Dictionary<string, bool>.Enumerator enumerator = component.MasteryBySkillID.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, bool> keyValuePair = enumerator.Current;
					foreach (SkillPerk skillPerk in Db.Get().Skills.Get(keyValuePair.Key).perks)
					{
						if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds))
						{
							SkillAttributePerk skillAttributePerk = skillPerk as SkillAttributePerk;
							if (skillAttributePerk != null && skillAttributePerk.modifier.AttributeId == b)
							{
								num += skillAttributePerk.modifier.Value;
							}
						}
					}
				}
				return num;
			}
		}
		RoboPilotModule component2 = this.launchConditions.GetComponent<RoboPilotModule>();
		if (component2 != null && component2.GetDataBanksStored() >= 1f)
		{
			num += component2.FlightEfficiencyModifier();
		}
		return num;
	}

	// Token: 0x06005306 RID: 21254 RVA: 0x001DC4EC File Offset: 0x001DA6EC
	public void ForceComplete()
	{
		this.missionElapsed = this.missionDuration;
	}

	// Token: 0x06005307 RID: 21255 RVA: 0x001DC4FC File Offset: 0x001DA6FC
	public void ProgressMission(float deltaTime)
	{
		if (this.state == Spacecraft.MissionState.Underway)
		{
			this.missionElapsed += deltaTime;
			if (this.controlStationBuffTimeRemaining > 0f)
			{
				this.missionElapsed += deltaTime * 0.20000005f;
				this.controlStationBuffTimeRemaining -= deltaTime;
			}
			else
			{
				this.controlStationBuffTimeRemaining = 0f;
			}
			if (this.missionElapsed > this.missionDuration)
			{
				this.CompleteMission();
			}
		}
	}

	// Token: 0x06005308 RID: 21256 RVA: 0x001DC570 File Offset: 0x001DA770
	public float GetTimeLeft()
	{
		return this.missionDuration - this.missionElapsed;
	}

	// Token: 0x06005309 RID: 21257 RVA: 0x001DC57F File Offset: 0x001DA77F
	public float GetDuration()
	{
		return this.missionDuration;
	}

	// Token: 0x0600530A RID: 21258 RVA: 0x001DC587 File Offset: 0x001DA787
	public void CompleteMission()
	{
		SpacecraftManager.instance.PushReadyToLandNotification(this);
		this.SetState(Spacecraft.MissionState.WaitingToLand);
		this.Land();
	}

	// Token: 0x0600530B RID: 21259 RVA: 0x001DC5A4 File Offset: 0x001DA7A4
	private void Land()
	{
		this.launchConditions.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			if (gameObject != this.launchConditions.gameObject)
			{
				gameObject.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
			}
		}
	}

	// Token: 0x0600530C RID: 21260 RVA: 0x001DC648 File Offset: 0x001DA848
	public void TemporallyTear()
	{
		SpacecraftManager.instance.hasVisitedWormHole = true;
		LaunchConditionManager launchConditions = this.launchConditions;
		for (int i = launchConditions.rocketModules.Count - 1; i >= 0; i--)
		{
			Storage component = launchConditions.rocketModules[i].GetComponent<Storage>();
			if (component != null)
			{
				component.ConsumeAllIgnoringDisease();
			}
			MinionStorage component2 = launchConditions.rocketModules[i].GetComponent<MinionStorage>();
			if (component2 != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
				for (int j = storedMinionInfo.Count - 1; j >= 0; j--)
				{
					component2.DeleteStoredMinion(storedMinionInfo[j].id);
				}
			}
			Util.KDestroyGameObject(launchConditions.rocketModules[i].gameObject);
		}
	}

	// Token: 0x0600530D RID: 21261 RVA: 0x001DC70B File Offset: 0x001DA90B
	public void GenerateName()
	{
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
	}

	// Token: 0x040036CE RID: 14030
	[Serialize]
	public int id = -1;

	// Token: 0x040036CF RID: 14031
	[Serialize]
	public string rocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x040036D0 RID: 14032
	[Serialize]
	public float controlStationBuffTimeRemaining;

	// Token: 0x040036D1 RID: 14033
	[Serialize]
	public Ref<LaunchConditionManager> refLaunchConditions = new Ref<LaunchConditionManager>();

	// Token: 0x040036D2 RID: 14034
	[Serialize]
	public Spacecraft.MissionState state;

	// Token: 0x040036D3 RID: 14035
	[Serialize]
	private float missionElapsed;

	// Token: 0x040036D4 RID: 14036
	[Serialize]
	private float missionDuration;

	// Token: 0x02001B3A RID: 6970
	public enum MissionState
	{
		// Token: 0x04007F24 RID: 32548
		Grounded,
		// Token: 0x04007F25 RID: 32549
		Launching,
		// Token: 0x04007F26 RID: 32550
		Underway,
		// Token: 0x04007F27 RID: 32551
		WaitingToLand,
		// Token: 0x04007F28 RID: 32552
		Landing,
		// Token: 0x04007F29 RID: 32553
		Destroyed
	}
}

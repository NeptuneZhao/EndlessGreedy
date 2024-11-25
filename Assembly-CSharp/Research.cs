using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A4D RID: 2637
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Research")]
public class Research : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06004C71 RID: 19569 RVA: 0x001B4D42 File Offset: 0x001B2F42
	public static void DestroyInstance()
	{
		Research.Instance = null;
	}

	// Token: 0x06004C72 RID: 19570 RVA: 0x001B4D4C File Offset: 0x001B2F4C
	public TechInstance GetTechInstance(string techID)
	{
		return this.techs.Find((TechInstance match) => match.tech.Id == techID);
	}

	// Token: 0x06004C73 RID: 19571 RVA: 0x001B4D7D File Offset: 0x001B2F7D
	public bool IsBeingResearched(Tech tech)
	{
		return this.activeResearch != null && tech != null && this.activeResearch.tech == tech;
	}

	// Token: 0x06004C74 RID: 19572 RVA: 0x001B4D9A File Offset: 0x001B2F9A
	protected override void OnPrefabInit()
	{
		Research.Instance = this;
		this.researchTypes = new ResearchTypes();
	}

	// Token: 0x06004C75 RID: 19573 RVA: 0x001B4DB0 File Offset: 0x001B2FB0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.globalPointInventory == null)
		{
			this.globalPointInventory = new ResearchPointInventory();
		}
		this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.OnRolesUpdated));
		this.OnRolesUpdated(null);
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearchBuildings);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearchBuildings);
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			IResearchCenter component = kprefabID.GetComponent<IResearchCenter>();
			if (component != null)
			{
				this.researchCenterPrefabs.Add(component);
			}
		}
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x001B4E7C File Offset: 0x001B307C
	public ResearchType GetResearchType(string id)
	{
		return this.researchTypes.GetResearchType(id);
	}

	// Token: 0x06004C77 RID: 19575 RVA: 0x001B4E8A File Offset: 0x001B308A
	public TechInstance GetActiveResearch()
	{
		return this.activeResearch;
	}

	// Token: 0x06004C78 RID: 19576 RVA: 0x001B4E92 File Offset: 0x001B3092
	public TechInstance GetTargetResearch()
	{
		if (this.queuedTech != null && this.queuedTech.Count > 0)
		{
			return this.queuedTech[this.queuedTech.Count - 1];
		}
		return null;
	}

	// Token: 0x06004C79 RID: 19577 RVA: 0x001B4EC4 File Offset: 0x001B30C4
	public TechInstance Get(Tech tech)
	{
		foreach (TechInstance techInstance in this.techs)
		{
			if (techInstance.tech == tech)
			{
				return techInstance;
			}
		}
		return null;
	}

	// Token: 0x06004C7A RID: 19578 RVA: 0x001B4F20 File Offset: 0x001B3120
	public TechInstance GetOrAdd(Tech tech)
	{
		TechInstance techInstance = this.techs.Find((TechInstance tc) => tc.tech == tech);
		if (techInstance != null)
		{
			return techInstance;
		}
		TechInstance techInstance2 = new TechInstance(tech);
		this.techs.Add(techInstance2);
		return techInstance2;
	}

	// Token: 0x06004C7B RID: 19579 RVA: 0x001B4F70 File Offset: 0x001B3170
	public void GetNextTech()
	{
		if (this.queuedTech.Count > 0)
		{
			this.queuedTech.RemoveAt(0);
		}
		if (this.queuedTech.Count > 0)
		{
			this.SetActiveResearch(this.queuedTech[this.queuedTech.Count - 1].tech, false);
			return;
		}
		this.SetActiveResearch(null, false);
	}

	// Token: 0x06004C7C RID: 19580 RVA: 0x001B4FD4 File Offset: 0x001B31D4
	private void AddTechToQueue(Tech tech)
	{
		TechInstance orAdd = this.GetOrAdd(tech);
		if (!orAdd.IsComplete() && !this.queuedTech.Contains(orAdd))
		{
			this.queuedTech.Add(orAdd);
		}
		orAdd.tech.requiredTech.ForEach(delegate(Tech _tech)
		{
			this.AddTechToQueue(_tech);
		});
	}

	// Token: 0x06004C7D RID: 19581 RVA: 0x001B5028 File Offset: 0x001B3228
	public void CancelResearch(Tech tech, bool clickedEntry = true)
	{
		Research.<>c__DisplayClass26_0 CS$<>8__locals1 = new Research.<>c__DisplayClass26_0();
		CS$<>8__locals1.tech = tech;
		CS$<>8__locals1.ti = this.queuedTech.Find((TechInstance qt) => qt.tech == CS$<>8__locals1.tech);
		if (CS$<>8__locals1.ti == null)
		{
			return;
		}
		this.SetActiveResearch(null, false);
		int i;
		int j;
		for (i = CS$<>8__locals1.ti.tech.unlockedTech.Count - 1; i >= 0; i = j - 1)
		{
			if (this.queuedTech.Find((TechInstance qt) => qt.tech == CS$<>8__locals1.ti.tech.unlockedTech[i]) != null)
			{
				this.CancelResearch(CS$<>8__locals1.ti.tech.unlockedTech[i], false);
			}
			j = i;
		}
		this.queuedTech.Remove(CS$<>8__locals1.ti);
		if (clickedEntry)
		{
			this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
		}
	}

	// Token: 0x06004C7E RID: 19582 RVA: 0x001B5120 File Offset: 0x001B3320
	private void NotifyResearchCenters(GameHashes hash, object data)
	{
		foreach (object obj in Components.ResearchCenters)
		{
			((KMonoBehaviour)obj).Trigger(-1914338957, data);
		}
		base.Trigger((int)hash, data);
	}

	// Token: 0x06004C7F RID: 19583 RVA: 0x001B5184 File Offset: 0x001B3384
	public void SetActiveResearch(Tech tech, bool clearQueue = false)
	{
		if (clearQueue)
		{
			this.queuedTech.Clear();
		}
		this.activeResearch = null;
		if (tech != null)
		{
			if (this.queuedTech.Count == 0)
			{
				this.AddTechToQueue(tech);
			}
			if (this.queuedTech.Count > 0)
			{
				this.queuedTech.Sort((TechInstance x, TechInstance y) => x.tech.tier.CompareTo(y.tech.tier));
				this.activeResearch = this.queuedTech[0];
			}
		}
		else
		{
			this.queuedTech.Clear();
		}
		this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
		this.CheckBuyResearch();
		this.CheckResearchBuildings(null);
		this.UpdateResearcherRoleNotification();
	}

	// Token: 0x06004C80 RID: 19584 RVA: 0x001B5238 File Offset: 0x001B3438
	private void UpdateResearcherRoleNotification()
	{
		if (this.NoResearcherRoleNotification != null)
		{
			this.notifier.Remove(this.NoResearcherRoleNotification);
			this.NoResearcherRoleNotification = null;
		}
		if (this.activeResearch != null)
		{
			Skill skill = null;
			if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("advanced") && this.activeResearch.tech.costsByResearchTypeID["advanced"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowAdvancedResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowAdvancedResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("space") && this.activeResearch.tech.costsByResearchTypeID["space"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowInterstellarResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowInterstellarResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("nuclear") && this.activeResearch.tech.costsByResearchTypeID["nuclear"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowNuclearResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowNuclearResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("orbital") && this.activeResearch.tech.costsByResearchTypeID["orbital"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowOrbitalResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowOrbitalResearch)[0];
			}
			if (skill != null)
			{
				this.NoResearcherRoleNotification = new Notification(RESEARCH.MESSAGING.NO_RESEARCHER_SKILL, NotificationType.Bad, new Func<List<Notification>, object, string>(this.NoResearcherRoleTooltip), skill, false, 12f, null, null, null, true, false, false);
				this.notifier.Add(this.NoResearcherRoleNotification, "");
			}
		}
	}

	// Token: 0x06004C81 RID: 19585 RVA: 0x001B54C0 File Offset: 0x001B36C0
	private string NoResearcherRoleTooltip(List<Notification> list, object data)
	{
		Skill skill = (Skill)data;
		return RESEARCH.MESSAGING.NO_RESEARCHER_SKILL_TOOLTIP.Replace("{ResearchType}", skill.Name);
	}

	// Token: 0x06004C82 RID: 19586 RVA: 0x001B54EC File Offset: 0x001B36EC
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.UseGlobalPointInventory && this.activeResearch == null)
		{
			global::Debug.LogWarning("No active research to add research points to. Global research inventory is disabled.");
			return;
		}
		(this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory).AddResearchPoints(researchTypeID, points);
		this.CheckBuyResearch();
		this.NotifyResearchCenters(GameHashes.ResearchPointsChanged, null);
	}

	// Token: 0x06004C83 RID: 19587 RVA: 0x001B5548 File Offset: 0x001B3748
	private void CheckBuyResearch()
	{
		if (this.activeResearch != null)
		{
			ResearchPointInventory researchPointInventory = this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory;
			if (this.activeResearch.tech.CanAfford(researchPointInventory))
			{
				foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
				{
					researchPointInventory.RemoveResearchPoints(keyValuePair.Key, keyValuePair.Value);
				}
				this.activeResearch.Purchased();
				Game.Instance.Trigger(-107300940, this.activeResearch.tech);
				this.GetNextTech();
			}
		}
	}

	// Token: 0x06004C84 RID: 19588 RVA: 0x001B5618 File Offset: 0x001B3818
	protected override void OnCleanUp()
	{
		if (Game.Instance != null && this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		Components.ResearchCenters.OnAdd -= new Action<IResearchCenter>(this.CheckResearchBuildings);
		Components.ResearchCenters.OnRemove -= new Action<IResearchCenter>(this.CheckResearchBuildings);
		base.OnCleanUp();
	}

	// Token: 0x06004C85 RID: 19589 RVA: 0x001B5680 File Offset: 0x001B3880
	public void CompleteQueue()
	{
		while (this.queuedTech.Count > 0)
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
			{
				this.AddResearchPoints(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06004C86 RID: 19590 RVA: 0x001B56FC File Offset: 0x001B38FC
	public List<TechInstance> GetResearchQueue()
	{
		return new List<TechInstance>(this.queuedTech);
	}

	// Token: 0x06004C87 RID: 19591 RVA: 0x001B570C File Offset: 0x001B390C
	[OnSerializing]
	internal void OnSerializing()
	{
		this.saveData = default(Research.SaveData);
		if (this.activeResearch != null)
		{
			this.saveData.activeResearchId = this.activeResearch.tech.Id;
		}
		else
		{
			this.saveData.activeResearchId = "";
		}
		if (this.queuedTech != null && this.queuedTech.Count > 0)
		{
			this.saveData.targetResearchId = this.queuedTech[this.queuedTech.Count - 1].tech.Id;
		}
		else
		{
			this.saveData.targetResearchId = "";
		}
		this.saveData.techs = new TechInstance.SaveData[this.techs.Count];
		for (int i = 0; i < this.techs.Count; i++)
		{
			this.saveData.techs[i] = this.techs[i].Save();
		}
	}

	// Token: 0x06004C88 RID: 19592 RVA: 0x001B5804 File Offset: 0x001B3A04
	[OnDeserialized]
	internal void OnDeserialized()
	{
		if (this.saveData.techs != null)
		{
			foreach (TechInstance.SaveData saveData in this.saveData.techs)
			{
				Tech tech = Db.Get().Techs.TryGet(saveData.techId);
				if (tech != null)
				{
					this.GetOrAdd(tech).Load(saveData);
				}
			}
		}
		foreach (TechInstance techInstance in this.techs)
		{
			if (this.saveData.targetResearchId == techInstance.tech.Id)
			{
				this.SetActiveResearch(techInstance.tech, false);
				break;
			}
		}
	}

	// Token: 0x06004C89 RID: 19593 RVA: 0x001B58D8 File Offset: 0x001B3AD8
	private void OnRolesUpdated(object data)
	{
		this.UpdateResearcherRoleNotification();
	}

	// Token: 0x06004C8A RID: 19594 RVA: 0x001B58E0 File Offset: 0x001B3AE0
	public string GetMissingResearchBuildingName()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
		{
			bool flag = true;
			if (keyValuePair.Value > 0f)
			{
				flag = false;
				using (List<IResearchCenter>.Enumerator enumerator2 = Components.ResearchCenters.Items.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetResearchType() == keyValuePair.Key)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				foreach (IResearchCenter researchCenter in this.researchCenterPrefabs)
				{
					if (researchCenter.GetResearchType() == keyValuePair.Key)
					{
						return ((KMonoBehaviour)researchCenter).GetProperName();
					}
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x06004C8B RID: 19595 RVA: 0x001B5A10 File Offset: 0x001B3C10
	private void CheckResearchBuildings(object data)
	{
		if (this.activeResearch == null)
		{
			this.notifier.Remove(this.MissingResearchStation);
			return;
		}
		if (string.IsNullOrEmpty(this.GetMissingResearchBuildingName()))
		{
			this.notifier.Remove(this.MissingResearchStation);
			return;
		}
		this.notifier.Add(this.MissingResearchStation, "");
	}

	// Token: 0x040032DE RID: 13022
	public static Research Instance;

	// Token: 0x040032DF RID: 13023
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040032E0 RID: 13024
	private List<TechInstance> techs = new List<TechInstance>();

	// Token: 0x040032E1 RID: 13025
	private List<TechInstance> queuedTech = new List<TechInstance>();

	// Token: 0x040032E2 RID: 13026
	private TechInstance activeResearch;

	// Token: 0x040032E3 RID: 13027
	private Notification NoResearcherRoleNotification;

	// Token: 0x040032E4 RID: 13028
	private Notification MissingResearchStation = new Notification(RESEARCH.MESSAGING.MISSING_RESEARCH_STATION, NotificationType.Bad, (List<Notification> list, object data) => RESEARCH.MESSAGING.MISSING_RESEARCH_STATION_TOOLTIP.ToString().Replace("{0}", Research.Instance.GetMissingResearchBuildingName()), null, false, 11f, null, null, null, true, false, false);

	// Token: 0x040032E5 RID: 13029
	private List<IResearchCenter> researchCenterPrefabs = new List<IResearchCenter>();

	// Token: 0x040032E6 RID: 13030
	protected int skillsUpdateHandle = -1;

	// Token: 0x040032E7 RID: 13031
	public ResearchTypes researchTypes;

	// Token: 0x040032E8 RID: 13032
	public bool UseGlobalPointInventory;

	// Token: 0x040032E9 RID: 13033
	[Serialize]
	public ResearchPointInventory globalPointInventory;

	// Token: 0x040032EA RID: 13034
	[Serialize]
	private Research.SaveData saveData;

	// Token: 0x02001A53 RID: 6739
	private struct SaveData
	{
		// Token: 0x04007C13 RID: 31763
		public string activeResearchId;

		// Token: 0x04007C14 RID: 31764
		public string targetResearchId;

		// Token: 0x04007C15 RID: 31765
		public TechInstance.SaveData[] techs;
	}
}

using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
public class GeothermalPlantComponent : KMonoBehaviour, ICheckboxListGroupControl, IRelatedEntities
{
	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06004058 RID: 16472 RVA: 0x0016C7D0 File Offset: 0x0016A9D0
	string ICheckboxListGroupControl.Title
	{
		get
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_TITLE;
		}
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06004059 RID: 16473 RVA: 0x0016C7DC File Offset: 0x0016A9DC
	string ICheckboxListGroupControl.Description
	{
		get
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_DESC;
		}
	}

	// Token: 0x0600405A RID: 16474 RVA: 0x0016C7E8 File Offset: 0x0016A9E8
	public ICheckboxListGroupControl.ListGroup[] GetData()
	{
		ColonyAchievement activateGeothermalPlant = Db.Get().ColonyAchievements.ActivateGeothermalPlant;
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[activateGeothermalPlant.requirementChecklist.Count];
		for (int i = 0; i < array.Length; i++)
		{
			ICheckboxListGroupControl.CheckboxItem checkboxItem = default(ICheckboxListGroupControl.CheckboxItem);
			bool flag = activateGeothermalPlant.requirementChecklist[i].Success();
			checkboxItem.isOn = flag;
			checkboxItem.text = (activateGeothermalPlant.requirementChecklist[i] as VictoryColonyAchievementRequirement).Name();
			checkboxItem.tooltip = activateGeothermalPlant.requirementChecklist[i].GetProgress(flag);
			array[i] = checkboxItem;
		}
		return new ICheckboxListGroupControl.ListGroup[]
		{
			new ICheckboxListGroupControl.ListGroup(activateGeothermalPlant.Name, array, null, null)
		};
	}

	// Token: 0x0600405B RID: 16475 RVA: 0x0016C8A2 File Offset: 0x0016AAA2
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x0600405C RID: 16476 RVA: 0x0016C8A5 File Offset: 0x0016AAA5
	public int CheckboxSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x0600405D RID: 16477 RVA: 0x0016C8A9 File Offset: 0x0016AAA9
	public static bool GeothermalControllerRepaired()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
	}

	// Token: 0x0600405E RID: 16478 RVA: 0x0016C8BA File Offset: 0x0016AABA
	public static bool GeothermalFacilityDiscovered()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered;
	}

	// Token: 0x0600405F RID: 16479 RVA: 0x0016C8CB File Offset: 0x0016AACB
	protected override void OnSpawn()
	{
		base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
	}

	// Token: 0x06004060 RID: 16480 RVA: 0x0016C8E5 File Offset: 0x0016AAE5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004061 RID: 16481 RVA: 0x0016C8F0 File Offset: 0x0016AAF0
	public static void DisplayPopup(string title, string desc, HashedString anim, System.Action onDismissCallback, Transform clickFocus = null)
	{
		EventInfoData eventInfoData = new EventInfoData(title, desc, anim);
		if (Components.LiveMinionIdentities.Count >= 2)
		{
			int num = UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count);
			int num2 = UnityEngine.Random.Range(1, Components.LiveMinionIdentities.Count);
			eventInfoData.minions = new GameObject[]
			{
				Components.LiveMinionIdentities[num].gameObject,
				Components.LiveMinionIdentities[(num + num2) % Components.LiveMinionIdentities.Count].gameObject
			};
		}
		else if (Components.LiveMinionIdentities.Count == 1)
		{
			eventInfoData.minions = new GameObject[]
			{
				Components.LiveMinionIdentities[0].gameObject
			};
		}
		eventInfoData.AddDefaultOption(onDismissCallback);
		eventInfoData.clickFocus = clickFocus;
		EventInfoScreen.ShowPopup(eventInfoData);
	}

	// Token: 0x06004062 RID: 16482 RVA: 0x0016C9BC File Offset: 0x0016ABBC
	protected void RevealAllVentsAndController()
	{
		foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, new Tag[]
		{
			"GeothermalVentEntity"
		}))
		{
			int baseX;
			int num;
			Grid.CellToXY(spawnable.cell, out baseX, out num);
			GridVisibility.Reveal(baseX, num + 2, 5, 5f);
		}
		foreach (WorldGenSpawner.Spawnable spawnable2 in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, new Tag[]
		{
			"GeothermalControllerEntity"
		}))
		{
			int baseX2;
			int num2;
			Grid.CellToXY(spawnable2.cell, out baseX2, out num2);
			GridVisibility.Reveal(baseX2, num2 + 3, 7, 7f);
		}
		SelectTool.Instance.Select(null, true);
	}

	// Token: 0x06004063 RID: 16483 RVA: 0x0016CACC File Offset: 0x0016ACCC
	protected void OnObjectSelect(object clicked)
	{
		base.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
		if (SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered)
		{
			return;
		}
		SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered = true;
		GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISCOVERED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISOCVERED_DESC, "geothermalplantintro_kanim", new System.Action(this.RevealAllVentsAndController), null);
	}

	// Token: 0x06004064 RID: 16484 RVA: 0x0016CB44 File Offset: 0x0016AD44
	public static void OnVentingHotMaterial(int worldid)
	{
		using (List<GeothermalVent>.Enumerator enumerator = Components.GeothermalVents.GetItems(worldid).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GeothermalVent vent = enumerator.Current;
				if (vent.IsQuestEntombed())
				{
					vent.SetQuestComplete();
					if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent)
					{
						GeothermalVictorySequence.VictoryVent = vent;
						GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_DESC, "geothermalplantachievement_kanim", delegate
						{
							SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent = true;
							if (!Db.Get().ColonyAchievements.ActivateGeothermalPlant.IsValidForSave())
							{
								GeothermalVictorySequence.Start(vent);
							}
						}, null);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06004065 RID: 16485 RVA: 0x0016CC0C File Offset: 0x0016AE0C
	public List<KSelectable> GetRelatedEntities()
	{
		List<KSelectable> list = new List<KSelectable>();
		int myWorldId = this.GetMyWorldId();
		foreach (GeothermalController geothermalController in Components.GeothermalControllers.GetItems(myWorldId))
		{
			list.Add(geothermalController.GetComponent<KSelectable>());
		}
		foreach (GeothermalVent geothermalVent in Components.GeothermalVents.GetItems(myWorldId))
		{
			list.Add(geothermalVent.GetComponent<KSelectable>());
		}
		return list;
	}

	// Token: 0x04002A7B RID: 10875
	public const string POPUP_DISCOVERED_KANIM = "geothermalplantintro_kanim";

	// Token: 0x04002A7C RID: 10876
	public const string POPUP_PROGRESS_KANIM = "geothermalplantonline_kanim";

	// Token: 0x04002A7D RID: 10877
	public const string POPUP_COMPLETE_KANIM = "geothermalplantachievement_kanim";
}

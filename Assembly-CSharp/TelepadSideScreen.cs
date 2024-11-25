using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB0 RID: 3504
public class TelepadSideScreen : SideScreenContent
{
	// Token: 0x06006EAC RID: 28332 RVA: 0x00299078 File Offset: 0x00297278
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.viewImmigrantsBtn.onClick += delegate()
		{
			ImmigrantScreen.InitializeImmigrantScreen(this.targetTelepad);
			Game.Instance.Trigger(288942073, null);
		};
		this.viewColonySummaryBtn.onClick += delegate()
		{
			this.newAchievementsEarned.gameObject.SetActive(false);
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
		};
		this.openRolesScreenButton.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleSkills();
		};
		this.BuildVictoryConditions();
	}

	// Token: 0x06006EAD RID: 28333 RVA: 0x002990E9 File Offset: 0x002972E9
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telepad>() != null;
	}

	// Token: 0x06006EAE RID: 28334 RVA: 0x002990F8 File Offset: 0x002972F8
	public override void SetTarget(GameObject target)
	{
		Telepad component = target.GetComponent<Telepad>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a telepad associated with it.");
			return;
		}
		this.targetTelepad = component;
		if (this.targetTelepad != null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06006EAF RID: 28335 RVA: 0x00299150 File Offset: 0x00297350
	private void Update()
	{
		if (this.targetTelepad != null)
		{
			if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver())
			{
				base.gameObject.SetActive(false);
				this.timeLabel.text = UI.UISIDESCREENS.TELEPADSIDESCREEN.GAMEOVER;
				this.SetContentState(true);
			}
			else
			{
				if (this.targetTelepad.GetComponent<Operational>().IsOperational)
				{
					this.timeLabel.text = string.Format(UI.UISIDESCREENS.TELEPADSIDESCREEN.NEXTPRODUCTION, GameUtil.GetFormattedCycles(this.targetTelepad.GetTimeRemaining(), "F1", false));
				}
				else
				{
					base.gameObject.SetActive(false);
				}
				this.SetContentState(!Immigration.Instance.ImmigrantsAvailable);
			}
			this.UpdateVictoryConditions();
			this.UpdateAchievementsUnlocked();
			this.UpdateSkills();
		}
	}

	// Token: 0x06006EB0 RID: 28336 RVA: 0x00299228 File Offset: 0x00297428
	private void SetContentState(bool isLabel)
	{
		if (this.timeLabel.gameObject.activeInHierarchy != isLabel)
		{
			this.timeLabel.gameObject.SetActive(isLabel);
		}
		if (this.viewImmigrantsBtn.gameObject.activeInHierarchy == isLabel)
		{
			this.viewImmigrantsBtn.gameObject.SetActive(!isLabel);
		}
	}

	// Token: 0x06006EB1 RID: 28337 RVA: 0x00299280 File Offset: 0x00297480
	private void BuildVictoryConditions()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (colonyAchievement.isVictoryCondition && !colonyAchievement.Disabled && colonyAchievement.IsValidForSave())
			{
				Dictionary<ColonyAchievementRequirement, GameObject> dictionary = new Dictionary<ColonyAchievementRequirement, GameObject>();
				this.victoryAchievementWidgets.Add(colonyAchievement, dictionary);
				GameObject gameObject = Util.KInstantiateUI(this.conditionContainerTemplate, this.victoryConditionsContainer, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(colonyAchievement.Name);
				foreach (ColonyAchievementRequirement colonyAchievementRequirement in colonyAchievement.requirementChecklist)
				{
					VictoryColonyAchievementRequirement victoryColonyAchievementRequirement = colonyAchievementRequirement as VictoryColonyAchievementRequirement;
					if (victoryColonyAchievementRequirement != null)
					{
						GameObject gameObject2 = Util.KInstantiateUI(this.checkboxLinePrefab, gameObject, true);
						gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(victoryColonyAchievementRequirement.Name());
						gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(victoryColonyAchievementRequirement.Description());
						dictionary.Add(colonyAchievementRequirement, gameObject2);
					}
					else
					{
						global::Debug.LogWarning(string.Format("Colony achievement {0} is not a victory requirement but it is attached to a victory achievement {1}.", colonyAchievementRequirement.GetType().ToString(), colonyAchievement.Name));
					}
				}
				this.entries.Add(colonyAchievement.Id, dictionary);
			}
		}
	}

	// Token: 0x06006EB2 RID: 28338 RVA: 0x00299424 File Offset: 0x00297624
	private void UpdateVictoryConditions()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (colonyAchievement.isVictoryCondition && !colonyAchievement.Disabled && colonyAchievement.IsValidForSave())
			{
				foreach (ColonyAchievementRequirement colonyAchievementRequirement in colonyAchievement.requirementChecklist)
				{
					this.entries[colonyAchievement.Id][colonyAchievementRequirement].GetComponent<HierarchyReferences>().GetReference<Image>("Check").enabled = colonyAchievementRequirement.Success();
				}
			}
		}
		foreach (KeyValuePair<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> keyValuePair in this.victoryAchievementWidgets)
		{
			foreach (KeyValuePair<ColonyAchievementRequirement, GameObject> keyValuePair2 in keyValuePair.Value)
			{
				keyValuePair2.Value.GetComponent<ToolTip>().SetSimpleTooltip(keyValuePair2.Key.GetProgress(keyValuePair2.Key.Success()));
			}
		}
	}

	// Token: 0x06006EB3 RID: 28339 RVA: 0x002995A8 File Offset: 0x002977A8
	private void UpdateAchievementsUnlocked()
	{
		if (SaveGame.Instance.ColonyAchievementTracker.achievementsToDisplay.Count > 0)
		{
			this.newAchievementsEarned.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006EB4 RID: 28340 RVA: 0x002995D4 File Offset: 0x002977D4
	private void UpdateSkills()
	{
		bool active = false;
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				active = true;
				break;
			}
		}
		this.skillPointsAvailable.gameObject.SetActive(active);
	}

	// Token: 0x04004B77 RID: 19319
	[SerializeField]
	private LocText timeLabel;

	// Token: 0x04004B78 RID: 19320
	[SerializeField]
	private KButton viewImmigrantsBtn;

	// Token: 0x04004B79 RID: 19321
	[SerializeField]
	private Telepad targetTelepad;

	// Token: 0x04004B7A RID: 19322
	[SerializeField]
	private KButton viewColonySummaryBtn;

	// Token: 0x04004B7B RID: 19323
	[SerializeField]
	private Image newAchievementsEarned;

	// Token: 0x04004B7C RID: 19324
	[SerializeField]
	private KButton openRolesScreenButton;

	// Token: 0x04004B7D RID: 19325
	[SerializeField]
	private Image skillPointsAvailable;

	// Token: 0x04004B7E RID: 19326
	[SerializeField]
	private GameObject victoryConditionsContainer;

	// Token: 0x04004B7F RID: 19327
	[SerializeField]
	private GameObject conditionContainerTemplate;

	// Token: 0x04004B80 RID: 19328
	[SerializeField]
	private GameObject checkboxLinePrefab;

	// Token: 0x04004B81 RID: 19329
	private Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>> entries = new Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>>();

	// Token: 0x04004B82 RID: 19330
	private Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> victoryAchievementWidgets = new Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>>();
}

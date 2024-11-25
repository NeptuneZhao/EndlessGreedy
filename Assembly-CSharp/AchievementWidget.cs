using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x02000BC1 RID: 3009
[AddComponentMenu("KMonoBehaviour/scripts/AchievementWidget")]
public class AchievementWidget : KMonoBehaviour
{
	// Token: 0x06005BA0 RID: 23456 RVA: 0x00216475 File Offset: 0x00214675
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle component = base.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.ExpandAchievement();
		}));
	}

	// Token: 0x06005BA1 RID: 23457 RVA: 0x002164A4 File Offset: 0x002146A4
	private void Update()
	{
	}

	// Token: 0x06005BA2 RID: 23458 RVA: 0x002164A6 File Offset: 0x002146A6
	private void ExpandAchievement()
	{
		if (SaveGame.Instance != null)
		{
			this.progressParent.gameObject.SetActive(!this.progressParent.gameObject.activeSelf);
		}
	}

	// Token: 0x06005BA3 RID: 23459 RVA: 0x002164D8 File Offset: 0x002146D8
	public void ActivateNewlyAchievedFlourish(float delay = 1f)
	{
		base.StartCoroutine(this.Flourish(delay));
	}

	// Token: 0x06005BA4 RID: 23460 RVA: 0x002164E8 File Offset: 0x002146E8
	private IEnumerator Flourish(float startDelay)
	{
		this.SetNeverAchieved();
		Canvas canvas = base.GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
		}
		yield return SequenceUtil.WaitForSecondsRealtime(startDelay);
		KScrollRect component = base.transform.parent.parent.GetComponent<KScrollRect>();
		float num = 1.1f;
		float smoothAutoScrollTarget = 1f + base.transform.localPosition.y * num / component.content.rect.height;
		component.SetSmoothAutoScrollTarget(smoothAutoScrollTarget);
		yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
		canvas.overrideSorting = true;
		canvas.sortingOrder = 30;
		GameObject icon = base.GetComponent<HierarchyReferences>().GetReference<Image>("icon").transform.parent.gameObject;
		foreach (KBatchedAnimController kbatchedAnimController in this.sparks)
		{
			if (kbatchedAnimController.transform.parent != icon.transform.parent)
			{
				kbatchedAnimController.GetComponent<KBatchedAnimController>().TintColour = new Color(1f, 0.86f, 0.56f, 1f);
				kbatchedAnimController.transform.SetParent(icon.transform.parent);
				kbatchedAnimController.transform.SetSiblingIndex(icon.transform.GetSiblingIndex());
				kbatchedAnimController.GetComponent<KBatchedAnimCanvasRenderer>().compare = CompareFunction.Always;
			}
		}
		HierarchyReferences component2 = base.GetComponent<HierarchyReferences>();
		component2.GetReference<Image>("iconBG").color = this.color_dark_red;
		component2.GetReference<Image>("iconBorder").color = this.color_gold;
		component2.GetReference<Image>("icon").color = this.color_gold;
		bool colorChanged = false;
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("AchievementUnlocked", false), Vector3.zero, 1f);
		int num2 = Mathf.RoundToInt(MathUtil.Clamp(1f, 7f, startDelay - startDelay % 1f / 1f)) - 1;
		instance.setParameterByName("num_achievements", (float)num2, false);
		KFMOD.EndOneShot(instance);
		for (float i = 0f; i < 1.2f; i += Time.unscaledDeltaTime)
		{
			icon.transform.localScale = Vector3.one * this.flourish_iconScaleCurve.Evaluate(i);
			this.sheenTransform.anchoredPosition = new Vector2(this.flourish_sheenPositionCurve.Evaluate(i), this.sheenTransform.anchoredPosition.y);
			if (i > 1f && !colorChanged)
			{
				colorChanged = true;
				KBatchedAnimController[] array = this.sparks;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].Play("spark", KAnim.PlayMode.Once, 1f, 0f);
				}
				this.SetAchievedNow();
			}
			yield return SequenceUtil.WaitForNextFrame;
		}
		icon.transform.localScale = Vector3.one;
		this.CompleteFlourish();
		for (float i = 0f; i < 0.6f; i += Time.unscaledDeltaTime)
		{
			yield return SequenceUtil.WaitForNextFrame;
		}
		base.transform.localScale = Vector3.one;
		yield break;
	}

	// Token: 0x06005BA5 RID: 23461 RVA: 0x00216500 File Offset: 0x00214700
	public void CompleteFlourish()
	{
		Canvas canvas = base.GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
		}
		canvas.overrideSorting = false;
	}

	// Token: 0x06005BA6 RID: 23462 RVA: 0x00216530 File Offset: 0x00214730
	public void SetAchievedNow()
	{
		base.GetComponent<MultiToggle>().ChangeState(1);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_red;
		component.GetReference<Image>("iconBorder").color = this.color_gold;
		component.GetReference<Image>("icon").color = this.color_gold;
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = Color.white;
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.ACHIEVED_THIS_COLONY_TOOLTIP);
	}

	// Token: 0x06005BA7 RID: 23463 RVA: 0x002165C8 File Offset: 0x002147C8
	public void SetAchievedBefore()
	{
		base.GetComponent<MultiToggle>().ChangeState(1);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_red;
		component.GetReference<Image>("iconBorder").color = this.color_gold;
		component.GetReference<Image>("icon").color = this.color_gold;
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = Color.white;
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.ACHIEVED_OTHER_COLONY_TOOLTIP);
	}

	// Token: 0x06005BA8 RID: 23464 RVA: 0x00216660 File Offset: 0x00214860
	public void SetNeverAchieved()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("icon").color = this.color_grey;
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.6f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.NOT_ACHIEVED_EVER);
	}

	// Token: 0x06005BA9 RID: 23465 RVA: 0x00216720 File Offset: 0x00214920
	public void SetNotAchieved()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("icon").color = this.color_grey;
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.6f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.NOT_ACHIEVED_THIS_COLONY);
	}

	// Token: 0x06005BAA RID: 23466 RVA: 0x002167E0 File Offset: 0x002149E0
	public void SetFailed()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBG").SetAlpha(0.5f);
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("iconBorder").SetAlpha(0.5f);
		component.GetReference<Image>("icon").color = this.color_grey;
		component.GetReference<Image>("icon").SetAlpha(0.5f);
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.25f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.FAILED_THIS_COLONY);
	}

	// Token: 0x06005BAB RID: 23467 RVA: 0x002168E0 File Offset: 0x00214AE0
	private void ConfigureToolTip(ToolTip tooltip, string status)
	{
		tooltip.ClearMultiStringTooltip();
		tooltip.AddMultiStringTooltip(status, null);
		if (SaveGame.Instance != null && !this.progressParent.gameObject.activeSelf)
		{
			tooltip.AddMultiStringTooltip(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXPAND_TOOLTIP, null);
		}
		if (DlcManager.IsDlcId(this.dlcIdFrom))
		{
			tooltip.AddMultiStringTooltip(string.Format(COLONY_ACHIEVEMENTS.DLC_ACHIEVEMENT, DlcManager.GetDlcTitle(this.dlcIdFrom)), null);
		}
	}

	// Token: 0x06005BAC RID: 23468 RVA: 0x0021695C File Offset: 0x00214B5C
	public void ShowProgress(ColonyAchievementStatus achievement)
	{
		if (this.progressParent == null)
		{
			return;
		}
		this.numRequirementsDisplayed = 0;
		for (int i = 0; i < achievement.Requirements.Count; i++)
		{
			ColonyAchievementRequirement colonyAchievementRequirement = achievement.Requirements[i];
			if (colonyAchievementRequirement is CritterTypesWithTraits)
			{
				this.ShowCritterChecklist(colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is DupesCompleteChoreInExoSuitForCycles)
			{
				this.ShowDupesInExoSuitsRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is DupesVsSolidTransferArmFetch)
			{
				this.ShowArmsOutPeformingDupesRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is ProduceXEngeryWithoutUsingYList)
			{
				this.ShowEngeryWithoutUsing(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is MinimumMorale)
			{
				this.ShowMinimumMoraleRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is SurviveARocketWithMinimumMorale)
			{
				this.ShowRocketMoraleRequirement(achievement.success, colonyAchievementRequirement);
			}
			else
			{
				this.ShowRequirement(achievement.success, colonyAchievementRequirement);
			}
		}
	}

	// Token: 0x06005BAD RID: 23469 RVA: 0x00216A3C File Offset: 0x00214C3C
	private HierarchyReferences GetNextRequirementWidget()
	{
		GameObject gameObject;
		if (this.progressParent.childCount <= this.numRequirementsDisplayed)
		{
			gameObject = global::Util.KInstantiateUI(this.requirementPrefab, this.progressParent.gameObject, true);
		}
		else
		{
			gameObject = this.progressParent.GetChild(this.numRequirementsDisplayed).gameObject;
			gameObject.SetActive(true);
		}
		this.numRequirementsDisplayed++;
		return gameObject.GetComponent<HierarchyReferences>();
	}

	// Token: 0x06005BAE RID: 23470 RVA: 0x00216AA8 File Offset: 0x00214CA8
	private void SetDescription(string str, HierarchyReferences refs)
	{
		refs.GetReference<LocText>("Desc").SetText(str);
	}

	// Token: 0x06005BAF RID: 23471 RVA: 0x00216ABB File Offset: 0x00214CBB
	private void SetIcon(Sprite sprite, Color color, HierarchyReferences refs)
	{
		Image reference = refs.GetReference<Image>("Icon");
		reference.sprite = sprite;
		reference.color = color;
		reference.gameObject.SetActive(true);
	}

	// Token: 0x06005BB0 RID: 23472 RVA: 0x00216AE1 File Offset: 0x00214CE1
	private void ShowIcon(bool show, HierarchyReferences refs)
	{
		refs.GetReference<Image>("Icon").gameObject.SetActive(show);
	}

	// Token: 0x06005BB1 RID: 23473 RVA: 0x00216AFC File Offset: 0x00214CFC
	private void ShowRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		bool flag = req.Success() || succeed;
		bool flag2 = req.Fail();
		if (flag && !flag2)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else if (flag2)
		{
			this.SetIcon(this.statusFailureIcon, Color.red, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		this.SetDescription(req.GetProgress(flag), nextRequirementWidget);
	}

	// Token: 0x06005BB2 RID: 23474 RVA: 0x00216B68 File Offset: 0x00214D68
	private void ShowCritterChecklist(ColonyAchievementRequirement req)
	{
		CritterTypesWithTraits critterTypesWithTraits = req as CritterTypesWithTraits;
		if (req == null)
		{
			return;
		}
		foreach (KeyValuePair<Tag, bool> keyValuePair in critterTypesWithTraits.critterTypesToCheck)
		{
			HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
			if (keyValuePair.Value)
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			else
			{
				this.ShowIcon(false, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TAME_A_CRITTER, keyValuePair.Key.Name.ProperName()), nextRequirementWidget);
		}
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x00216C1C File Offset: 0x00214E1C
	private void ShowArmsOutPeformingDupesRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		DupesVsSolidTransferArmFetch dupesVsSolidTransferArmFetch = req as DupesVsSolidTransferArmFetch;
		if (dupesVsSolidTransferArmFetch == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_PERFORMANCE, succeed ? dupesVsSolidTransferArmFetch.numCycles : dupesVsSolidTransferArmFetch.currentCycleCount, dupesVsSolidTransferArmFetch.numCycles), nextRequirementWidget);
		if (!succeed)
		{
			Dictionary<int, int> fetchDupeChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchDupeChoreDeliveries;
			Dictionary<int, int> fetchAutomatedChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchAutomatedChoreDeliveries;
			int num = 0;
			fetchDupeChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num);
			int num2 = 0;
			fetchAutomatedChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num2);
			nextRequirementWidget = this.GetNextRequirementWidget();
			if ((float)num < (float)num2 * dupesVsSolidTransferArmFetch.percentage)
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			else
			{
				this.ShowIcon(false, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_VS_DUPE_FETCHES, "SolidTransferArm", num2, num), nextRequirementWidget);
		}
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x00216D30 File Offset: 0x00214F30
	private void ShowDupesInExoSuitsRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		DupesCompleteChoreInExoSuitForCycles dupesCompleteChoreInExoSuitForCycles = req as DupesCompleteChoreInExoSuitForCycles;
		if (dupesCompleteChoreInExoSuitForCycles == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_CYCLES, succeed ? dupesCompleteChoreInExoSuitForCycles.numCycles : dupesCompleteChoreInExoSuitForCycles.currentCycleStreak, dupesCompleteChoreInExoSuitForCycles.numCycles), nextRequirementWidget);
		if (!succeed)
		{
			nextRequirementWidget = this.GetNextRequirementWidget();
			int num = dupesCompleteChoreInExoSuitForCycles.GetNumberOfDupesForCycle(GameClock.Instance.GetCycle());
			if (num >= Components.LiveMinionIdentities.Count)
			{
				num = Components.LiveMinionIdentities.Count;
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_THIS_CYCLE, num, Components.LiveMinionIdentities.Count), nextRequirementWidget);
		}
	}

	// Token: 0x06005BB5 RID: 23477 RVA: 0x00216E18 File Offset: 0x00215018
	private void ShowEngeryWithoutUsing(bool succeed, ColonyAchievementRequirement req)
	{
		ProduceXEngeryWithoutUsingYList produceXEngeryWithoutUsingYList = req as ProduceXEngeryWithoutUsingYList;
		if (req == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		float productionAmount = produceXEngeryWithoutUsingYList.GetProductionAmount(succeed);
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.GENERATE_POWER, GameUtil.GetFormattedRoundedJoules(productionAmount), GameUtil.GetFormattedRoundedJoules(produceXEngeryWithoutUsingYList.amountToProduce * 1000f)), nextRequirementWidget);
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		foreach (Tag key in produceXEngeryWithoutUsingYList.disallowedBuildings)
		{
			nextRequirementWidget = this.GetNextRequirementWidget();
			if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
			{
				this.SetIcon(this.statusFailureIcon, Color.red, nextRequirementWidget);
			}
			else
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			BuildingDef buildingDef = Assets.GetBuildingDef(key.Name);
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_BUILDING, buildingDef.Name), nextRequirementWidget);
		}
	}

	// Token: 0x06005BB6 RID: 23478 RVA: 0x00216F40 File Offset: 0x00215140
	private void ShowMinimumMoraleRequirement(bool success, ColonyAchievementRequirement req)
	{
		MinimumMorale minimumMorale = req as MinimumMorale;
		if (minimumMorale == null)
		{
			return;
		}
		if (success)
		{
			this.ShowRequirement(success, req);
			return;
		}
		foreach (object obj in Components.MinionAssignablesProxy)
		{
			GameObject targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
			if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead))
			{
				AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());
				if (attributeInstance != null)
				{
					HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
					if (attributeInstance.GetTotalValue() >= (float)minimumMorale.minimumMorale)
					{
						this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
					}
					else
					{
						this.ShowIcon(false, nextRequirementWidget);
					}
					this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MORALE, targetGameObject.GetProperName(), attributeInstance.GetTotalDisplayValue()), nextRequirementWidget);
				}
			}
		}
	}

	// Token: 0x06005BB7 RID: 23479 RVA: 0x0021704C File Offset: 0x0021524C
	private void ShowRocketMoraleRequirement(bool success, ColonyAchievementRequirement req)
	{
		SurviveARocketWithMinimumMorale surviveARocketWithMinimumMorale = req as SurviveARocketWithMinimumMorale;
		if (surviveARocketWithMinimumMorale == null)
		{
			return;
		}
		if (success)
		{
			this.ShowRequirement(success, req);
			return;
		}
		foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.ColonyAchievementTracker.cyclesRocketDupeMoraleAboveRequirement)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair.Key);
			if (world != null)
			{
				HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
				this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE, new object[]
				{
					surviveARocketWithMinimumMorale.minimumMorale,
					keyValuePair.Value,
					surviveARocketWithMinimumMorale.numberOfCycles,
					world.GetProperName()
				}), nextRequirementWidget);
			}
		}
	}

	// Token: 0x04003D5B RID: 15707
	private Color color_dark_red = new Color(0.28235295f, 0.16078432f, 0.14901961f);

	// Token: 0x04003D5C RID: 15708
	private Color color_gold = new Color(1f, 0.63529414f, 0.28627452f);

	// Token: 0x04003D5D RID: 15709
	private Color color_dark_grey = new Color(0.21568628f, 0.21568628f, 0.21568628f);

	// Token: 0x04003D5E RID: 15710
	private Color color_grey = new Color(0.6901961f, 0.6901961f, 0.6901961f);

	// Token: 0x04003D5F RID: 15711
	[SerializeField]
	private RectTransform sheenTransform;

	// Token: 0x04003D60 RID: 15712
	public AnimationCurve flourish_iconScaleCurve;

	// Token: 0x04003D61 RID: 15713
	public AnimationCurve flourish_sheenPositionCurve;

	// Token: 0x04003D62 RID: 15714
	public KBatchedAnimController[] sparks;

	// Token: 0x04003D63 RID: 15715
	[SerializeField]
	private RectTransform progressParent;

	// Token: 0x04003D64 RID: 15716
	[SerializeField]
	private GameObject requirementPrefab;

	// Token: 0x04003D65 RID: 15717
	[SerializeField]
	private Sprite statusSuccessIcon;

	// Token: 0x04003D66 RID: 15718
	[SerializeField]
	private Sprite statusFailureIcon;

	// Token: 0x04003D67 RID: 15719
	private int numRequirementsDisplayed;

	// Token: 0x04003D68 RID: 15720
	public string dlcIdFrom;
}

using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DBF RID: 3519
[AddComponentMenu("KMonoBehaviour/scripts/SkillMinionWidget")]
public class SkillMinionWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x06006F89 RID: 28553 RVA: 0x0029F629 File Offset: 0x0029D829
	// (set) Token: 0x06006F8A RID: 28554 RVA: 0x0029F631 File Offset: 0x0029D831
	public IAssignableIdentity assignableIdentity { get; private set; }

	// Token: 0x06006F8B RID: 28555 RVA: 0x0029F63C File Offset: 0x0029D83C
	public void SetMinon(IAssignableIdentity identity)
	{
		this.assignableIdentity = identity;
		this.portrait.SetIdentityObject(this.assignableIdentity, true);
		base.GetComponent<NotificationHighlightTarget>().targetKey = identity.GetSoleOwner().gameObject.GetInstanceID().ToString();
	}

	// Token: 0x06006F8C RID: 28556 RVA: 0x0029F685 File Offset: 0x0029D885
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleHover(true);
		this.soundPlayer.Play(1);
	}

	// Token: 0x06006F8D RID: 28557 RVA: 0x0029F69A File Offset: 0x0029D89A
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleHover(false);
	}

	// Token: 0x06006F8E RID: 28558 RVA: 0x0029F6A3 File Offset: 0x0029D8A3
	private void ToggleHover(bool on)
	{
		if (this.skillsScreen.CurrentlySelectedMinion != this.assignableIdentity)
		{
			this.SetColor(on ? this.hover_color : this.unselected_color);
		}
	}

	// Token: 0x06006F8F RID: 28559 RVA: 0x0029F6CF File Offset: 0x0029D8CF
	private void SetColor(Color color)
	{
		this.background.color = color;
		if (this.assignableIdentity != null && this.assignableIdentity as StoredMinionIdentity != null)
		{
			base.GetComponent<CanvasGroup>().alpha = 0.6f;
		}
	}

	// Token: 0x06006F90 RID: 28560 RVA: 0x0029F708 File Offset: 0x0029D908
	public void OnPointerClick(PointerEventData eventData)
	{
		this.skillsScreen.CurrentlySelectedMinion = this.assignableIdentity;
		base.GetComponent<NotificationHighlightTarget>().View();
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
	}

	// Token: 0x06006F91 RID: 28561 RVA: 0x0029F738 File Offset: 0x0029D938
	public void Refresh()
	{
		if (this.assignableIdentity.IsNullOrDestroyed())
		{
			return;
		}
		this.portrait.SetIdentityObject(this.assignableIdentity, true);
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
		this.hatDropDown.gameObject.SetActive(true);
		string hat;
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			int availableSkillpoints = component.AvailableSkillpoints;
			int totalSkillPointsGained = component.TotalSkillPointsGained;
			this.masteryPoints.text = ((availableSkillpoints > 0) ? GameUtil.ApplyBoldString(GameUtil.ColourizeString(new Color(0.5f, 1f, 0.5f, 1f), availableSkillpoints.ToString())) : "0");
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(component);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component);
			this.morale.text = string.Format("{0}/{1}", attributeInstance.GetTotalValue(), attributeInstance2.GetTotalValue());
			this.RefreshToolTip(component);
			List<IListableOption> list = new List<IListableOption>();
			foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
			{
				if (keyValuePair.Value)
				{
					list.Add(new SkillListable(keyValuePair.Key));
				}
			}
			this.hatDropDown.Initialize(list, new Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, minionIdentity);
			hat = (string.IsNullOrEmpty(component.TargetHat) ? component.CurrentHat : component.TargetHat);
		}
		else
		{
			ToolTip component2 = base.GetComponent<ToolTip>();
			component2.ClearMultiStringTooltip();
			component2.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), storedMinionIdentity.GetProperName()), null);
			hat = (string.IsNullOrEmpty(storedMinionIdentity.targetHat) ? storedMinionIdentity.currentHat : storedMinionIdentity.targetHat);
			this.masteryPoints.text = UI.TABLESCREENS.NA;
			this.morale.text = UI.TABLESCREENS.NA;
		}
		bool flag = this.skillsScreen.CurrentlySelectedMinion == this.assignableIdentity;
		if (this.skillsScreen.CurrentlySelectedMinion != null && this.assignableIdentity != null)
		{
			flag = (flag || this.skillsScreen.CurrentlySelectedMinion.GetSoleOwner() == this.assignableIdentity.GetSoleOwner());
		}
		this.SetColor(flag ? this.selected_color : this.unselected_color);
		HierarchyReferences component3 = base.GetComponent<HierarchyReferences>();
		this.RefreshHat(hat);
		component3.GetReference("openButton").gameObject.SetActive(minionIdentity != null);
	}

	// Token: 0x06006F92 RID: 28562 RVA: 0x0029FA1C File Offset: 0x0029DC1C
	private void RefreshToolTip(MinionResume resume)
	{
		if (resume != null)
		{
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(resume);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(resume);
			ToolTip component = base.GetComponent<ToolTip>();
			component.ClearMultiStringTooltip();
			component.AddMultiStringTooltip(this.assignableIdentity.GetProperName() + "\n\n", this.TooltipTextStyle_Header);
			component.AddMultiStringTooltip(string.Format(UI.SKILLS_SCREEN.CURRENT_MORALE, attributeInstance.GetTotalValue(), attributeInstance2.GetTotalValue()), null);
			component.AddMultiStringTooltip("\n" + UI.DETAILTABS.STATS.NAME + "\n\n", this.TooltipTextStyle_Header);
			foreach (AttributeInstance attributeInstance3 in resume.GetAttributes())
			{
				if (attributeInstance3.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill)
				{
					string text = UIConstants.ColorPrefixWhite;
					if (attributeInstance3.GetTotalValue() > 0f)
					{
						text = UIConstants.ColorPrefixGreen;
					}
					else if (attributeInstance3.GetTotalValue() < 0f)
					{
						text = UIConstants.ColorPrefixRed;
					}
					component.AddMultiStringTooltip(string.Concat(new string[]
					{
						"    • ",
						attributeInstance3.Name,
						": ",
						text,
						attributeInstance3.GetTotalValue().ToString(),
						UIConstants.ColorSuffix
					}), null);
				}
			}
		}
	}

	// Token: 0x06006F93 RID: 28563 RVA: 0x0029FBB0 File Offset: 0x0029DDB0
	public void RefreshHat(string hat)
	{
		base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite(string.IsNullOrEmpty(hat) ? "hat_role_none" : hat);
	}

	// Token: 0x06006F94 RID: 28564 RVA: 0x0029FBE8 File Offset: 0x0029DDE8
	private void OnHatDropEntryClick(IListableOption skill, object data)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity == null)
		{
			return;
		}
		MinionResume component = minionIdentity.GetComponent<MinionResume>();
		if (skill != null)
		{
			base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((skill as SkillListable).skillHat);
			if (component != null)
			{
				string skillHat = (skill as SkillListable).skillHat;
				component.SetHats(component.CurrentHat, skillHat);
				if (component.OwnsHat(skillHat))
				{
					new PutOnHatChore(component, Db.Get().ChoreTypes.SwitchHat);
				}
			}
		}
		else
		{
			base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite("hat_role_none");
			if (component != null)
			{
				component.SetHats(component.CurrentHat, null);
				component.ApplyTargetHat();
			}
		}
		this.skillsScreen.RefreshAll();
	}

	// Token: 0x06006F95 RID: 28565 RVA: 0x0029FCE0 File Offset: 0x0029DEE0
	private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			SkillListable skillListable = entry.entryData as SkillListable;
			entry.image.sprite = Assets.GetSprite(skillListable.skillHat);
		}
	}

	// Token: 0x06006F96 RID: 28566 RVA: 0x0029FD1C File Offset: 0x0029DF1C
	private int hatDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x04004C33 RID: 19507
	[SerializeField]
	private SkillsScreen skillsScreen;

	// Token: 0x04004C34 RID: 19508
	[SerializeField]
	private CrewPortrait portrait;

	// Token: 0x04004C35 RID: 19509
	[SerializeField]
	private LocText masteryPoints;

	// Token: 0x04004C36 RID: 19510
	[SerializeField]
	private LocText morale;

	// Token: 0x04004C37 RID: 19511
	[SerializeField]
	private Image background;

	// Token: 0x04004C38 RID: 19512
	[SerializeField]
	private Image hat_background;

	// Token: 0x04004C39 RID: 19513
	[SerializeField]
	private Color selected_color;

	// Token: 0x04004C3A RID: 19514
	[SerializeField]
	private Color unselected_color;

	// Token: 0x04004C3B RID: 19515
	[SerializeField]
	private Color hover_color;

	// Token: 0x04004C3C RID: 19516
	[SerializeField]
	private DropDown hatDropDown;

	// Token: 0x04004C3D RID: 19517
	[SerializeField]
	private TextStyleSetting TooltipTextStyle_Header;

	// Token: 0x04004C3E RID: 19518
	[SerializeField]
	private TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04004C3F RID: 19519
	public ButtonSoundPlayer soundPlayer;
}

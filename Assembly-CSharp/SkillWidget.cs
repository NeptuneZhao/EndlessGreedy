﻿using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000DC0 RID: 3520
[AddComponentMenu("KMonoBehaviour/scripts/SkillWidget")]
public class SkillWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x06006F98 RID: 28568 RVA: 0x0029FD27 File Offset: 0x0029DF27
	// (set) Token: 0x06006F99 RID: 28569 RVA: 0x0029FD2F File Offset: 0x0029DF2F
	public string skillID { get; private set; }

	// Token: 0x06006F9A RID: 28570 RVA: 0x0029FD38 File Offset: 0x0029DF38
	public void Refresh(string skillID)
	{
		Skill skill = Db.Get().Skills.Get(skillID);
		if (skill == null)
		{
			global::Debug.LogWarning("DbSkills is missing skillId " + skillID);
			return;
		}
		this.Name.text = skill.Name;
		LocText name = this.Name;
		name.text = name.text + "\n(" + Db.Get().SkillGroups.Get(skill.skillGroup).Name + ")";
		this.skillID = skillID;
		this.tooltip.SetSimpleTooltip(this.SkillTooltip(skill));
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		MinionResume minionResume = null;
		if (minionIdentity != null)
		{
			minionResume = minionIdentity.GetComponent<MinionResume>();
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = minionResume.GetSkillMasteryConditions(skillID);
			bool flag = minionResume.CanMasterSkill(skillMasteryConditions);
			if (!(minionResume == null) && (minionResume.HasMasteredSkill(skillID) || flag))
			{
				this.TitleBarBG.color = (minionResume.HasMasteredSkill(skillID) ? this.header_color_has_skill : this.header_color_can_assign);
				this.hatImage.material = this.defaultMaterial;
			}
			else
			{
				this.TitleBarBG.color = this.header_color_disabled;
				this.hatImage.material = this.desaturatedMaterial;
			}
		}
		else if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.HasMasteredSkill(skillID))
			{
				this.TitleBarBG.color = this.header_color_has_skill;
				this.hatImage.material = this.defaultMaterial;
			}
			else
			{
				this.TitleBarBG.color = this.header_color_disabled;
				this.hatImage.material = this.desaturatedMaterial;
			}
		}
		this.hatImage.sprite = Assets.GetSprite(skill.badge);
		bool active = false;
		bool flag2 = false;
		if (minionResume != null)
		{
			flag2 = minionResume.HasBeenGrantedSkill(skill);
			float num;
			minionResume.AptitudeBySkillGroup.TryGetValue(skill.skillGroup, out num);
			active = (num > 0f && !flag2);
		}
		this.aptitudeBox.SetActive(active);
		this.grantedBox.SetActive(flag2);
		if (flag2)
		{
			Sprite skillGrantSourceIcon = minionResume.GetSkillGrantSourceIcon(skill.Id);
			if (skillGrantSourceIcon != null)
			{
				this.grantedIcon.sprite = skillGrantSourceIcon;
			}
		}
		this.traitDisabledIcon.SetActive(minionResume != null && !minionResume.IsAbleToLearnSkill(skill.Id));
		string text = "";
		List<string> list = new List<string>();
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			MinionResume component = minionIdentity2.GetComponent<MinionResume>();
			if (component != null && component.HasMasteredSkill(skillID))
			{
				list.Add(component.GetProperName());
			}
		}
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					StoredMinionIdentity storedMinionIdentity2 = info.serializedMinion.Get<StoredMinionIdentity>();
					if (storedMinionIdentity2 != null && storedMinionIdentity2.HasMasteredSkill(skillID))
					{
						list.Add(storedMinionIdentity2.GetProperName());
					}
				}
			}
		}
		this.masteryCount.gameObject.SetActive(list.Count > 0);
		foreach (string str in list)
		{
			text = text + "\n    • " + str;
		}
		this.masteryCount.SetSimpleTooltip((list.Count > 0) ? string.Format(UI.ROLES_SCREEN.WIDGET.NUMBER_OF_MASTERS_TOOLTIP, text) : UI.ROLES_SCREEN.WIDGET.NO_MASTERS_TOOLTIP.text);
		this.masteryCount.GetComponentInChildren<LocText>().text = list.Count.ToString();
	}

	// Token: 0x06006F9B RID: 28571 RVA: 0x002A018C File Offset: 0x0029E38C
	public void RefreshLines()
	{
		this.prerequisiteSkillWidgets.Clear();
		List<Vector2> list = new List<Vector2>();
		foreach (string text in Db.Get().Skills.Get(this.skillID).priorSkills)
		{
			list.Add(this.skillsScreen.GetSkillWidgetLineTargetPosition(text));
			this.prerequisiteSkillWidgets.Add(this.skillsScreen.GetSkillWidget(text));
		}
		if (this.lines != null)
		{
			for (int i = this.lines.Length - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.lines[i].gameObject);
			}
		}
		this.linePoints.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			float num = this.lines_left.GetPosition().x - list[j].x - 12f;
			float y = 0f;
			this.linePoints.Add(new Vector2(0f, y));
			this.linePoints.Add(new Vector2(-num, y));
			this.linePoints.Add(new Vector2(-num, y));
			this.linePoints.Add(new Vector2(-num, -(this.lines_left.GetPosition().y - list[j].y)));
			this.linePoints.Add(new Vector2(-num, -(this.lines_left.GetPosition().y - list[j].y)));
			this.linePoints.Add(new Vector2(-(this.lines_left.GetPosition().x - list[j].x), -(this.lines_left.GetPosition().y - list[j].y)));
		}
		this.lines = new UILineRenderer[this.linePoints.Count / 2];
		int num2 = 0;
		for (int k = 0; k < this.linePoints.Count; k += 2)
		{
			GameObject gameObject = new GameObject("Line");
			gameObject.AddComponent<RectTransform>();
			gameObject.transform.SetParent(this.lines_left.transform);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			this.lines[num2] = gameObject.AddComponent<UILineRenderer>();
			this.lines[num2].color = new Color(0.6509804f, 0.6509804f, 0.6509804f, 1f);
			this.lines[num2].Points = new Vector2[]
			{
				this.linePoints[k],
				this.linePoints[k + 1]
			};
			num2++;
		}
	}

	// Token: 0x06006F9C RID: 28572 RVA: 0x002A04A0 File Offset: 0x0029E6A0
	public void ToggleBorderHighlight(bool on)
	{
		this.borderHighlight.SetActive(on);
		if (this.lines != null)
		{
			foreach (UILineRenderer uilineRenderer in this.lines)
			{
				uilineRenderer.color = (on ? this.line_color_active : this.line_color_default);
				uilineRenderer.LineThickness = (float)(on ? 4 : 2);
				uilineRenderer.SetAllDirty();
			}
		}
		for (int j = 0; j < this.prerequisiteSkillWidgets.Count; j++)
		{
			this.prerequisiteSkillWidgets[j].ToggleBorderHighlight(on);
		}
	}

	// Token: 0x06006F9D RID: 28573 RVA: 0x002A052B File Offset: 0x0029E72B
	public string SkillTooltip(Skill skill)
	{
		return "" + SkillWidget.SkillPerksString(skill) + "\n" + this.DuplicantSkillString(skill);
	}

	// Token: 0x06006F9E RID: 28574 RVA: 0x002A0550 File Offset: 0x0029E750
	public static string SkillPerksString(Skill skill)
	{
		string text = "";
		foreach (SkillPerk skillPerk in skill.perks)
		{
			if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text = text + "• " + skillPerk.Name;
			}
		}
		return text;
	}

	// Token: 0x06006F9F RID: 28575 RVA: 0x002A05DC File Offset: 0x0029E7DC
	public string CriteriaString(Skill skill)
	{
		bool flag = false;
		string text = "";
		text = text + "<b>" + UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.TITLE + "</b>\n";
		SkillGroup skillGroup = Db.Get().SkillGroups.Get(skill.skillGroup);
		if (skillGroup != null && skillGroup.relevantAttributes != null)
		{
			foreach (Klei.AI.Attribute attribute in skillGroup.relevantAttributes)
			{
				if (attribute != null)
				{
					text = text + "    • " + string.Format(UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.SKILLGROUP_ENABLED.DESCRIPTION, attribute.Name) + "\n";
					flag = true;
				}
			}
		}
		if (skill.priorSkills.Count > 0)
		{
			flag = true;
			for (int i = 0; i < skill.priorSkills.Count; i++)
			{
				text = text + "    • " + string.Format("{0}", Db.Get().Skills.Get(skill.priorSkills[i]).Name);
				text += "</color>";
				if (i != skill.priorSkills.Count - 1)
				{
					text += "\n";
				}
			}
		}
		if (!flag)
		{
			text = text + "    • " + string.Format(UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.NONE, skill.Name);
		}
		return text;
	}

	// Token: 0x06006FA0 RID: 28576 RVA: 0x002A074C File Offset: 0x0029E94C
	public string DuplicantSkillString(Skill skill)
	{
		string text = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			if (component == null)
			{
				return "";
			}
			LocString loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CAN_MASTER;
			if (component.HasMasteredSkill(skill.Id))
			{
				if (component.HasBeenGrantedSkill(skill))
				{
					text += "\n";
					loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_GRANTED;
					text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
				}
			}
			else
			{
				MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(skill.Id);
				if (!component.CanMasterSkill(skillMasteryConditions))
				{
					bool flag = false;
					text += "\n";
					loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CANNOT_MASTER;
					text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.UnableToLearn))
					{
						flag = true;
						string choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
						Trait trait;
						minionIdentity.GetComponent<Traits>().IsChoreGroupDisabled(choreGroupID, out trait);
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.PREVENTED_BY_TRAIT;
						text += string.Format(loc_string, trait.Name);
					}
					if (!flag)
					{
						if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.MissingPreviousSkill))
						{
							text += "\n";
							loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_PREVIOUS_SKILLS;
							text += string.Format(loc_string, Array.Empty<object>());
						}
					}
					if (!flag)
					{
						if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.NeedsSkillPoints))
						{
							text += "\n";
							loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_MORE_SKILL_POINTS;
							text += string.Format(loc_string, Array.Empty<object>());
						}
					}
				}
				else
				{
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.StressWarning))
					{
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.STRESS_WARNING_MESSAGE;
						text += string.Format(loc_string, skill.Name, minionIdentity.GetProperName());
					}
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.SkillAptitude))
					{
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_APTITUDE;
						text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06006FA1 RID: 28577 RVA: 0x002A0A3A File Offset: 0x0029EC3A
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleBorderHighlight(true);
		this.skillsScreen.HoverSkill(this.skillID);
		this.soundPlayer.Play(1);
	}

	// Token: 0x06006FA2 RID: 28578 RVA: 0x002A0A60 File Offset: 0x0029EC60
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleBorderHighlight(false);
		this.skillsScreen.HoverSkill(null);
	}

	// Token: 0x06006FA3 RID: 28579 RVA: 0x002A0A78 File Offset: 0x0029EC78
	public void OnPointerClick(PointerEventData eventData)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
			{
				component.ForceAddSkillPoint();
			}
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(this.skillID);
			bool flag = component.CanMasterSkill(skillMasteryConditions);
			if (component != null && !component.HasMasteredSkill(this.skillID) && flag)
			{
				component.MasterSkill(this.skillID);
				this.skillsScreen.RefreshAll();
			}
		}
	}

	// Token: 0x06006FA4 RID: 28580 RVA: 0x002A0B14 File Offset: 0x0029ED14
	public void OnPointerDown(PointerEventData eventData)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		MinionResume minionResume = null;
		bool flag = false;
		if (minionIdentity != null)
		{
			minionResume = minionIdentity.GetComponent<MinionResume>();
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = minionResume.GetSkillMasteryConditions(this.skillID);
			flag = minionResume.CanMasterSkill(skillMasteryConditions);
		}
		if (minionResume != null && !minionResume.HasMasteredSkill(this.skillID) && flag)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
			return;
		}
		KFMOD.PlayUISound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x04004C41 RID: 19521
	[SerializeField]
	private LocText Name;

	// Token: 0x04004C42 RID: 19522
	[SerializeField]
	private LocText Description;

	// Token: 0x04004C43 RID: 19523
	[SerializeField]
	private Image TitleBarBG;

	// Token: 0x04004C44 RID: 19524
	[SerializeField]
	private SkillsScreen skillsScreen;

	// Token: 0x04004C45 RID: 19525
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x04004C46 RID: 19526
	[SerializeField]
	private RectTransform lines_left;

	// Token: 0x04004C47 RID: 19527
	[SerializeField]
	public RectTransform lines_right;

	// Token: 0x04004C48 RID: 19528
	[SerializeField]
	private Color header_color_has_skill;

	// Token: 0x04004C49 RID: 19529
	[SerializeField]
	private Color header_color_can_assign;

	// Token: 0x04004C4A RID: 19530
	[SerializeField]
	private Color header_color_disabled;

	// Token: 0x04004C4B RID: 19531
	[SerializeField]
	private Color line_color_default;

	// Token: 0x04004C4C RID: 19532
	[SerializeField]
	private Color line_color_active;

	// Token: 0x04004C4D RID: 19533
	[SerializeField]
	private Image hatImage;

	// Token: 0x04004C4E RID: 19534
	[SerializeField]
	private GameObject borderHighlight;

	// Token: 0x04004C4F RID: 19535
	[SerializeField]
	private ToolTip masteryCount;

	// Token: 0x04004C50 RID: 19536
	[SerializeField]
	private GameObject aptitudeBox;

	// Token: 0x04004C51 RID: 19537
	[SerializeField]
	private GameObject grantedBox;

	// Token: 0x04004C52 RID: 19538
	[SerializeField]
	private Image grantedIcon;

	// Token: 0x04004C53 RID: 19539
	[SerializeField]
	private GameObject traitDisabledIcon;

	// Token: 0x04004C54 RID: 19540
	public TextStyleSetting TooltipTextStyle_Header;

	// Token: 0x04004C55 RID: 19541
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04004C56 RID: 19542
	private List<SkillWidget> prerequisiteSkillWidgets = new List<SkillWidget>();

	// Token: 0x04004C57 RID: 19543
	private UILineRenderer[] lines;

	// Token: 0x04004C58 RID: 19544
	private List<Vector2> linePoints = new List<Vector2>();

	// Token: 0x04004C59 RID: 19545
	public Material defaultMaterial;

	// Token: 0x04004C5A RID: 19546
	public Material desaturatedMaterial;

	// Token: 0x04004C5B RID: 19547
	public ButtonSoundPlayer soundPlayer;
}

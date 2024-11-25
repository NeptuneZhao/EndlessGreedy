using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B8A RID: 2954
public class CharacterContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x060058F6 RID: 22774 RVA: 0x00202232 File Offset: 0x00200432
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x060058F7 RID: 22775 RVA: 0x0020223A File Offset: 0x0020043A
	public MinionStartingStats Stats
	{
		get
		{
			return this.stats;
		}
	}

	// Token: 0x060058F8 RID: 22776 RVA: 0x00202244 File Offset: 0x00200444
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
		this.characterNameTitle.OnStartedEditing += this.OnStartedEditing;
		this.characterNameTitle.OnNameChanged += this.OnNameChanged;
		this.reshuffleButton.onClick += delegate()
		{
			this.Reshuffle(true);
		};
		List<IListableOption> list = new List<IListableOption>();
		foreach (SkillGroup item in new List<SkillGroup>(Db.Get().SkillGroups.resources))
		{
			list.Add(item);
		}
		this.archetypeDropDown.Initialize(list, new Action<IListableOption, object>(this.OnArchetypeEntryClick), new Func<IListableOption, IListableOption, object, int>(this.archetypeDropDownSort), new Action<DropDownEntry, object>(this.archetypeDropEntryRefreshAction), false, null);
		this.archetypeDropDown.CustomizeEmptyRow(Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NOARCHETYPESELECTED"), this.noArchetypeIcon);
		List<IListableOption> contentKeys = new List<IListableOption>
		{
			new CharacterContainer.MinionModelOption(DUPLICANTS.MODEL.STANDARD.NAME, new List<Tag>
			{
				GameTags.Minions.Models.Standard
			}, Assets.GetSprite("ui_duplicant_minion_selection")),
			new CharacterContainer.MinionModelOption(DUPLICANTS.MODEL.BIONIC.NAME, new List<Tag>
			{
				GameTags.Minions.Models.Bionic
			}, Assets.GetSprite("ui_duplicant_bionicminion_selection"))
		};
		this.modelDropDown.Initialize(contentKeys, new Action<IListableOption, object>(this.OnModelEntryClick), new Func<IListableOption, IListableOption, object, int>(this.modelDropDownSort), new Action<DropDownEntry, object>(this.modelDropEntryRefreshAction), true, null);
		this.modelDropDown.CustomizeEmptyRow(UI.CHARACTERCONTAINER_ALL_MODELS, Assets.GetSprite(this.allModelSprite));
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x060058F9 RID: 22777 RVA: 0x00202420 File Offset: 0x00200620
	public void ForceStopEditingTitle()
	{
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x060058FA RID: 22778 RVA: 0x0020242D File Offset: 0x0020062D
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x00202434 File Offset: 0x00200634
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.GenerateCharacter(this.controller.IsStarterMinion, null);
		yield break;
	}

	// Token: 0x060058FC RID: 22780 RVA: 0x00202443 File Offset: 0x00200643
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x060058FD RID: 22781 RVA: 0x00202470 File Offset: 0x00200670
	protected override void OnForcedCleanUp()
	{
		CharacterContainer.containers.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x060058FE RID: 22782 RVA: 0x00202484 File Offset: 0x00200684
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.controller != null)
		{
			CharacterSelectionController characterSelectionController = this.controller;
			characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Remove(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
			CharacterSelectionController characterSelectionController2 = this.controller;
			characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Remove(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
			CharacterSelectionController characterSelectionController3 = this.controller;
			characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Remove(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		}
	}

	// Token: 0x060058FF RID: 22783 RVA: 0x0020251C File Offset: 0x0020071C
	private void Initialize()
	{
		this.iconGroups = new List<GameObject>();
		this.traitEntries = new List<GameObject>();
		this.expectationLabels = new List<LocText>();
		this.aptitudeEntries = new List<GameObject>();
		if (CharacterContainer.containers == null)
		{
			CharacterContainer.containers = new List<CharacterContainer>();
		}
		CharacterContainer.containers.Add(this);
	}

	// Token: 0x06005900 RID: 22784 RVA: 0x00202571 File Offset: 0x00200771
	private void OnNameChanged(string newName)
	{
		this.stats.Name = newName;
		this.stats.personality.Name = newName;
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x06005901 RID: 22785 RVA: 0x002025AB File Offset: 0x002007AB
	private void OnStartedEditing()
	{
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06005902 RID: 22786 RVA: 0x002025B8 File Offset: 0x002007B8
	public void SetMinion(MinionStartingStats statsProposed)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		this.stats = statsProposed;
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x06005903 RID: 22787 RVA: 0x0020266C File Offset: 0x0020086C
	public void GenerateCharacter(bool is_starter, string guaranteedAptitudeID = null)
	{
		int num = 0;
		do
		{
			this.stats = new MinionStartingStats(this.permittedModels, is_starter, guaranteedAptitudeID, null, false);
			num++;
		}
		while (this.IsCharacterInvalid() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x06005904 RID: 22788 RVA: 0x0020271C File Offset: 0x0020091C
	private void SetAnimator()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(GameTags.MinionSelectPreview), this.contentBody.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = this.baseCharacterScale;
		}
		BaseMinionConfig.ConfigureSymbols(this.animController.gameObject, true);
		this.stats.ApplyTraits(this.animController.gameObject);
		this.stats.ApplyRace(this.animController.gameObject);
		this.stats.ApplyAccessories(this.animController.gameObject);
		this.stats.ApplyOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyJoyResponseOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyExperience(this.animController.gameObject);
		HashedString idleAnim = this.GetIdleAnim(this.stats);
		this.idle_anim = Assets.GetAnim(idleAnim);
		if (this.idle_anim != null)
		{
			this.animController.AddAnimOverrides(this.idle_anim, 0f);
		}
		KAnimFile anim = Assets.GetAnim(new HashedString("crewSelect_fx_kanim"));
		if (anim != null)
		{
			this.animController.AddAnimOverrides(anim, 0f);
		}
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005905 RID: 22789 RVA: 0x002028B4 File Offset: 0x00200AB4
	private HashedString GetIdleAnim(MinionStartingStats minionStartingStats)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, string[]> keyValuePair in CharacterContainer.traitIdleAnims)
		{
			foreach (Trait trait in minionStartingStats.Traits)
			{
				if (keyValuePair.Value.Contains(trait.Id))
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (keyValuePair.Value.Contains(minionStartingStats.joyTrait.Id) || keyValuePair.Value.Contains(minionStartingStats.stressTrait.Id))
			{
				list.Add(keyValuePair.Key);
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray()[UnityEngine.Random.Range(0, list.Count)];
		}
		return CharacterContainer.idleAnims[UnityEngine.Random.Range(0, CharacterContainer.idleAnims.Length)];
	}

	// Token: 0x06005906 RID: 22790 RVA: 0x002029E0 File Offset: 0x00200BE0
	private void SetInfoText()
	{
		this.traitEntries.ForEach(delegate(GameObject tl)
		{
			UnityEngine.Object.Destroy(tl.gameObject);
		});
		this.traitEntries.Clear();
		this.characterNameTitle.SetTitle(this.stats.Name);
		this.traitHeaderLabel.SetText((this.stats.personality.model == GameTags.Minions.Models.Bionic) ? UI.CHARACTERCONTAINER_TRAITS_TITLE_BIONIC : UI.CHARACTERCONTAINER_TRAITS_TITLE);
		for (int i = 1; i < this.stats.Traits.Count; i++)
		{
			Trait trait = this.stats.Traits[i];
			LocText locText = trait.PositiveTrait ? this.goodTrait : this.badTrait;
			LocText locText2 = Util.KInstantiateUI<LocText>(locText.gameObject, locText.transform.parent.gameObject, false);
			locText2.gameObject.SetActive(true);
			locText2.text = this.stats.Traits[i].Name;
			locText2.color = (trait.PositiveTrait ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR);
			locText2.GetComponent<ToolTip>().SetSimpleTooltip(trait.GetTooltip());
			for (int j = 0; j < trait.SelfModifiers.Count; j++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				string format = (trait.SelfModifiers[j].Value > 0f) ? UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED : UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED;
				componentInChildren.text = string.Format(format, Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"));
				trait.SelfModifiers[j].AttributeId == "GermResistance";
				Klei.AI.Attribute attribute = Db.Get().Attributes.Get(trait.SelfModifiers[j].AttributeId);
				string text = attribute.Description;
				text = string.Concat(new string[]
				{
					text,
					"\n\n",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"),
					": ",
					trait.SelfModifiers[j].GetFormattedString()
				});
				List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(attribute);
				for (int k = 0; k < convertersForAttribute.Count; k++)
				{
					string text2 = convertersForAttribute[k].DescriptionFromAttribute(convertersForAttribute[k].multiplier * trait.SelfModifiers[j].Value, null);
					if (text2 != "")
					{
						text = text + "\n    • " + text2;
					}
				}
				componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(text);
				this.traitEntries.Add(gameObject);
			}
			if (trait.disabledChoreGroups != null)
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject2.SetActive(true);
				LocText componentInChildren2 = gameObject2.GetComponentInChildren<LocText>();
				componentInChildren2.text = trait.GetDisabledChoresString(false);
				string text3 = "";
				string text4 = "";
				for (int l = 0; l < trait.disabledChoreGroups.Length; l++)
				{
					if (l > 0)
					{
						text3 += ", ";
						text4 += "\n";
					}
					text3 += trait.disabledChoreGroups[l].Name;
					text4 += trait.disabledChoreGroups[l].description;
				}
				componentInChildren2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(DUPLICANTS.TRAITS.CANNOT_DO_TASK_TOOLTIP, text3, text4));
				this.traitEntries.Add(gameObject2);
			}
			if (trait.ignoredEffects != null && trait.ignoredEffects.Length != 0)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject3.SetActive(true);
				LocText componentInChildren3 = gameObject3.GetComponentInChildren<LocText>();
				componentInChildren3.text = trait.GetIgnoredEffectsString(false);
				string text5 = "";
				for (int m = 0; m < trait.ignoredEffects.Length; m++)
				{
					if (m > 0)
					{
						text5 += "\n";
					}
					text5 += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS_TOOLTIP, Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".CAUSE"));
					if (m < trait.ignoredEffects.Length - 1)
					{
						text5 += ",";
					}
				}
				componentInChildren3.GetComponent<ToolTip>().SetSimpleTooltip(text5);
				this.traitEntries.Add(gameObject3);
			}
			StringEntry stringEntry;
			if (Strings.TryGet("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC", out stringEntry))
			{
				GameObject gameObject4 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject4.SetActive(true);
				LocText componentInChildren4 = gameObject4.GetComponentInChildren<LocText>();
				componentInChildren4.text = stringEntry.String;
				componentInChildren4.GetComponent<ToolTip>().SetSimpleTooltip(Strings.Get("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC_TOOLTIP"));
				this.traitEntries.Add(gameObject4);
			}
			this.traitEntries.Add(locText2.gameObject);
		}
		this.aptitudeEntries.ForEach(delegate(GameObject al)
		{
			UnityEngine.Object.Destroy(al.gameObject);
		});
		this.aptitudeEntries.Clear();
		this.expectationLabels.ForEach(delegate(LocText el)
		{
			UnityEngine.Object.Destroy(el.gameObject);
		});
		this.expectationLabels.Clear();
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.stats.skillAptitudes)
		{
			if (keyValuePair.Value != 0f)
			{
				SkillGroup skillGroup = Db.Get().SkillGroups.Get(keyValuePair.Key.IdHash);
				if (skillGroup == null)
				{
					global::Debug.LogWarningFormat("Role group not found for aptitude: {0}", new object[]
					{
						keyValuePair.Key
					});
				}
				else
				{
					GameObject gameObject5 = Util.KInstantiateUI(this.aptitudeEntry.gameObject, this.aptitudeEntry.transform.parent.gameObject, false);
					LocText locText3 = Util.KInstantiateUI<LocText>(this.aptitudeLabel.gameObject, gameObject5, false);
					locText3.gameObject.SetActive(true);
					locText3.text = skillGroup.Name;
					string simpleTooltip;
					if (skillGroup.choreGroupID != "")
					{
						ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(skillGroup.choreGroupID);
						simpleTooltip = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION_CHOREGROUP, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS, choreGroup.description);
					}
					else
					{
						simpleTooltip = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS);
					}
					locText3.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
					float num = (float)this.stats.StartingLevels[keyValuePair.Key.relevantAttributes[0].Id];
					LocText locText4 = Util.KInstantiateUI<LocText>(this.attributeLabelAptitude.gameObject, gameObject5, false);
					locText4.gameObject.SetActive(true);
					locText4.text = "+" + num.ToString() + " " + keyValuePair.Key.relevantAttributes[0].Name;
					string text6 = keyValuePair.Key.relevantAttributes[0].Description;
					text6 = string.Concat(new string[]
					{
						text6,
						"\n\n",
						keyValuePair.Key.relevantAttributes[0].Name,
						": +",
						num.ToString()
					});
					List<AttributeConverter> convertersForAttribute2 = Db.Get().AttributeConverters.GetConvertersForAttribute(keyValuePair.Key.relevantAttributes[0]);
					for (int n = 0; n < convertersForAttribute2.Count; n++)
					{
						text6 = text6 + "\n    • " + convertersForAttribute2[n].DescriptionFromAttribute(convertersForAttribute2[n].multiplier * num, null);
					}
					locText4.GetComponent<ToolTip>().SetSimpleTooltip(text6);
					gameObject5.gameObject.SetActive(true);
					this.aptitudeEntries.Add(gameObject5);
				}
			}
		}
		if (this.stats.stressTrait != null)
		{
			LocText locText5 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText5.gameObject.SetActive(true);
			locText5.text = string.Format(UI.CHARACTERCONTAINER_STRESSTRAIT, this.stats.stressTrait.Name);
			locText5.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.stressTrait.GetTooltip());
			this.expectationLabels.Add(locText5);
		}
		if (this.stats.joyTrait != null)
		{
			LocText locText6 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText6.gameObject.SetActive(true);
			locText6.text = string.Format(UI.CHARACTERCONTAINER_JOYTRAIT, this.stats.joyTrait.Name);
			locText6.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.joyTrait.GetTooltip());
			this.expectationLabels.Add(locText6);
		}
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x06005907 RID: 22791 RVA: 0x0020348C File Offset: 0x0020168C
	private IEnumerator SetAttributes()
	{
		yield return null;
		this.iconGroups.ForEach(delegate(GameObject icg)
		{
			UnityEngine.Object.Destroy(icg);
		});
		this.iconGroups.Clear();
		List<AttributeInstance> list = new List<AttributeInstance>(this.animController.gameObject.GetAttributes().AttributeTable);
		list.RemoveAll((AttributeInstance at) => at.Attribute.ShowInUI != Klei.AI.Attribute.Display.Skill);
		list = (from at in list
		orderby at.Name
		select at).ToList<AttributeInstance>();
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.iconGroup.gameObject, this.iconGroup.transform.parent.gameObject, false);
			LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
			gameObject.SetActive(true);
			float totalValue = list[i].GetTotalValue();
			if (totalValue > 0f)
			{
				componentInChildren.color = Constants.POSITIVE_COLOR;
			}
			else if (totalValue == 0f)
			{
				componentInChildren.color = Constants.NEUTRAL_COLOR;
			}
			else
			{
				componentInChildren.color = Constants.NEGATIVE_COLOR;
			}
			componentInChildren.text = string.Format(UI.CHARACTERCONTAINER_SKILL_VALUE, GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f), list[i].Name);
			AttributeInstance attributeInstance = list[i];
			string text = attributeInstance.Description;
			if (attributeInstance.Attribute.converters.Count > 0)
			{
				text += "\n";
				foreach (AttributeConverter attributeConverter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance converter = this.animController.gameObject.GetComponent<Klei.AI.AttributeConverters>().GetConverter(attributeConverter.Id);
					string text2 = converter.DescriptionFromAttribute(converter.Evaluate(), converter.gameObject);
					if (text2 != null)
					{
						text = text + "\n" + text2;
					}
				}
			}
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(text);
			this.iconGroups.Add(gameObject);
		}
		yield break;
	}

	// Token: 0x06005908 RID: 22792 RVA: 0x0020349C File Offset: 0x0020169C
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.stats);
		}
		if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
		{
			MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetActive();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.DeselectDeliverable();
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0f, true);
			}
		};
		this.selectedBorder.SetActive(true);
		this.titleBar.color = this.selectedTitleColor;
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Play("cheer_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005909 RID: 22793 RVA: 0x00203584 File Offset: 0x00201784
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.stats);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetInactive();
		this.selectButton.Deselect();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
		this.selectedBorder.SetActive(false);
		this.titleBar.color = this.deselectedTitleColor;
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600590A RID: 22794 RVA: 0x0020364A File Offset: 0x0020184A
	private void OnReplacedEvent(ITelepadDeliverable deliverable)
	{
		if (deliverable == this.stats)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x0600590B RID: 22795 RVA: 0x0020365C File Offset: 0x0020185C
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		if (this.controller.AllowsReplacing)
		{
			this.selectButton.onClick += this.ReplaceCharacterSelection;
			return;
		}
		this.selectButton.onClick += this.CantSelectCharacter;
	}

	// Token: 0x0600590C RID: 22796 RVA: 0x002036D2 File Offset: 0x002018D2
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x0600590D RID: 22797 RVA: 0x002036E4 File Offset: 0x002018E4
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x0600590E RID: 22798 RVA: 0x00203708 File Offset: 0x00201908
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x0600590F RID: 22799 RVA: 0x0020375C File Offset: 0x0020195C
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
		this.archetypeDropDown.gameObject.SetActive(enable);
		this.modelDropDown.transform.parent.gameObject.SetActive(enable && SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC3_ID"));
	}

	// Token: 0x06005910 RID: 22800 RVA: 0x002037BC File Offset: 0x002019BC
	public void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		if (this.fxAnim != null)
		{
			this.fxAnim.Play("loop", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.GenerateCharacter(is_starter, this.guaranteedAptitudeID);
	}

	// Token: 0x06005911 RID: 22801 RVA: 0x0020382C File Offset: 0x00201A2C
	public void SetController(CharacterSelectionController csc)
	{
		if (csc == this.controller)
		{
			return;
		}
		this.controller = csc;
		CharacterSelectionController characterSelectionController = this.controller;
		characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Combine(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
		CharacterSelectionController characterSelectionController2 = this.controller;
		characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Combine(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
		CharacterSelectionController characterSelectionController3 = this.controller;
		characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Combine(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		CharacterSelectionController characterSelectionController4 = this.controller;
		characterSelectionController4.OnReplacedEvent = (Action<ITelepadDeliverable>)Delegate.Combine(characterSelectionController4.OnReplacedEvent, new Action<ITelepadDeliverable>(this.OnReplacedEvent));
	}

	// Token: 0x06005912 RID: 22802 RVA: 0x002038EC File Offset: 0x00201AEC
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x06005913 RID: 22803 RVA: 0x0020394C File Offset: 0x00201B4C
	private bool IsCharacterInvalid()
	{
		return CharacterContainer.containers.Find((CharacterContainer container) => container != null && container.stats != null && container != this && container.stats.personality.Id == this.stats.personality.Id && container.stats.IsValid) != null || (SaveLoader.Instance != null && DlcManager.IsDlcId(this.stats.personality.requiredDlcId) && !SaveLoader.Instance.GameInfo.dlcIds.Contains(this.stats.personality.requiredDlcId)) || (this.stats.personality.model != GameTags.Minions.Models.Bionic && Components.LiveMinionIdentities.Items.Any((MinionIdentity id) => id.personalityResourceId == this.stats.personality.Id));
	}

	// Token: 0x06005914 RID: 22804 RVA: 0x00203A01 File Offset: 0x00201C01
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x06005915 RID: 22805 RVA: 0x00203A11 File Offset: 0x00201C11
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = true;
		base.OnPointerEnter(eventData);
	}

	// Token: 0x06005916 RID: 22806 RVA: 0x00203A26 File Offset: 0x00201C26
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = false;
		base.OnPointerExit(eventData);
	}

	// Token: 0x06005917 RID: 22807 RVA: 0x00203A3C File Offset: 0x00201C3C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape) || e.IsAction(global::Action.MouseRight))
		{
			this.characterNameTitle.ForceStopEditing();
			this.controller.OnPressBack();
			this.archetypeDropDown.scrollRect.gameObject.SetActive(false);
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyDown(e);
			return;
		}
		this.scroll_rect.OnKeyDown(e);
	}

	// Token: 0x06005918 RID: 22808 RVA: 0x00203B0C File Offset: 0x00201D0C
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyUp(e);
			return;
		}
		this.scroll_rect.OnKeyUp(e);
	}

	// Token: 0x06005919 RID: 22809 RVA: 0x00203B9B File Offset: 0x00201D9B
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.stats == null)
		{
			return;
		}
		this.SetAnimator();
	}

	// Token: 0x0600591A RID: 22810 RVA: 0x00203BB2 File Offset: 0x00201DB2
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x0600591B RID: 22811 RVA: 0x00203BC8 File Offset: 0x00201DC8
	private void OnArchetypeEntryClick(IListableOption skill, object data)
	{
		if (skill != null)
		{
			SkillGroup skillGroup = skill as SkillGroup;
			this.guaranteedAptitudeID = skillGroup.Id;
			this.selectedArchetypeIcon.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
			this.Reshuffle(true);
			return;
		}
		this.guaranteedAptitudeID = null;
		this.selectedArchetypeIcon.sprite = this.dropdownArrowIcon;
		this.Reshuffle(true);
	}

	// Token: 0x0600591C RID: 22812 RVA: 0x00203C2D File Offset: 0x00201E2D
	private int archetypeDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		if (b.Equals("Random"))
		{
			return -1;
		}
		return b.GetProperName().CompareTo(a.GetProperName());
	}

	// Token: 0x0600591D RID: 22813 RVA: 0x00203C50 File Offset: 0x00201E50
	private void archetypeDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			SkillGroup skillGroup = entry.entryData as SkillGroup;
			entry.image.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
		}
	}

	// Token: 0x0600591E RID: 22814 RVA: 0x00203C8C File Offset: 0x00201E8C
	private void OnModelEntryClick(IListableOption listItem, object data)
	{
		if (listItem == null)
		{
			this.permittedModels = this.allMinionModels;
			this.selectedModelIcon.sprite = Assets.GetSprite(this.allModelSprite);
			this.Reshuffle(true);
			return;
		}
		CharacterContainer.MinionModelOption minionModelOption = listItem as CharacterContainer.MinionModelOption;
		if (minionModelOption != null)
		{
			this.permittedModels = minionModelOption.permittedModels;
			this.selectedModelIcon.sprite = minionModelOption.sprite;
			this.Reshuffle(true);
		}
	}

	// Token: 0x0600591F RID: 22815 RVA: 0x00203CF9 File Offset: 0x00201EF9
	private int modelDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return a.GetProperName().CompareTo(b.GetProperName());
	}

	// Token: 0x06005920 RID: 22816 RVA: 0x00203D0C File Offset: 0x00201F0C
	private void modelDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			CharacterContainer.MinionModelOption minionModelOption = entry.entryData as CharacterContainer.MinionModelOption;
			entry.image.sprite = minionModelOption.sprite;
		}
	}

	// Token: 0x04003A63 RID: 14947
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x04003A64 RID: 14948
	[SerializeField]
	private LocText characterName;

	// Token: 0x04003A65 RID: 14949
	[SerializeField]
	private EditableTitleBar characterNameTitle;

	// Token: 0x04003A66 RID: 14950
	[SerializeField]
	private LocText characterJob;

	// Token: 0x04003A67 RID: 14951
	[SerializeField]
	private LocText traitHeaderLabel;

	// Token: 0x04003A68 RID: 14952
	public GameObject selectedBorder;

	// Token: 0x04003A69 RID: 14953
	[SerializeField]
	private Image titleBar;

	// Token: 0x04003A6A RID: 14954
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x04003A6B RID: 14955
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x04003A6C RID: 14956
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x04003A6D RID: 14957
	private KBatchedAnimController animController;

	// Token: 0x04003A6E RID: 14958
	[SerializeField]
	private GameObject iconGroup;

	// Token: 0x04003A6F RID: 14959
	private List<GameObject> iconGroups;

	// Token: 0x04003A70 RID: 14960
	[SerializeField]
	private LocText goodTrait;

	// Token: 0x04003A71 RID: 14961
	[SerializeField]
	private LocText badTrait;

	// Token: 0x04003A72 RID: 14962
	[SerializeField]
	private GameObject aptitudeEntry;

	// Token: 0x04003A73 RID: 14963
	[SerializeField]
	private Transform aptitudeLabel;

	// Token: 0x04003A74 RID: 14964
	[SerializeField]
	private Transform attributeLabelAptitude;

	// Token: 0x04003A75 RID: 14965
	[SerializeField]
	private Transform attributeLabelTrait;

	// Token: 0x04003A76 RID: 14966
	[SerializeField]
	private LocText expectationRight;

	// Token: 0x04003A77 RID: 14967
	private List<LocText> expectationLabels;

	// Token: 0x04003A78 RID: 14968
	[SerializeField]
	private DropDown archetypeDropDown;

	// Token: 0x04003A79 RID: 14969
	[SerializeField]
	private Image selectedArchetypeIcon;

	// Token: 0x04003A7A RID: 14970
	[SerializeField]
	private Sprite noArchetypeIcon;

	// Token: 0x04003A7B RID: 14971
	[SerializeField]
	private Sprite dropdownArrowIcon;

	// Token: 0x04003A7C RID: 14972
	private string guaranteedAptitudeID;

	// Token: 0x04003A7D RID: 14973
	private List<GameObject> aptitudeEntries;

	// Token: 0x04003A7E RID: 14974
	private List<GameObject> traitEntries;

	// Token: 0x04003A7F RID: 14975
	[SerializeField]
	private LocText description;

	// Token: 0x04003A80 RID: 14976
	[SerializeField]
	private Image selectedModelIcon;

	// Token: 0x04003A81 RID: 14977
	[SerializeField]
	private DropDown modelDropDown;

	// Token: 0x04003A82 RID: 14978
	private List<Tag> permittedModels = new List<Tag>
	{
		GameTags.Minions.Models.Standard,
		GameTags.Minions.Models.Bionic
	};

	// Token: 0x04003A83 RID: 14979
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04003A84 RID: 14980
	[SerializeField]
	private KBatchedAnimController fxAnim;

	// Token: 0x04003A85 RID: 14981
	private string allModelSprite = "ui_duplicant_any_selection";

	// Token: 0x04003A86 RID: 14982
	private MinionStartingStats stats;

	// Token: 0x04003A87 RID: 14983
	private CharacterSelectionController controller;

	// Token: 0x04003A88 RID: 14984
	private static List<CharacterContainer> containers;

	// Token: 0x04003A89 RID: 14985
	private KAnimFile idle_anim;

	// Token: 0x04003A8A RID: 14986
	[HideInInspector]
	public bool addMinionToIdentityList = true;

	// Token: 0x04003A8B RID: 14987
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x04003A8C RID: 14988
	[SerializeField]
	private KScrollRect scroll_rect;

	// Token: 0x04003A8D RID: 14989
	private static readonly Dictionary<HashedString, string[]> traitIdleAnims = new Dictionary<HashedString, string[]>
	{
		{
			"anim_idle_food_kanim",
			new string[]
			{
				"Foodie"
			}
		},
		{
			"anim_idle_animal_lover_kanim",
			new string[]
			{
				"RanchingUp"
			}
		},
		{
			"anim_idle_loner_kanim",
			new string[]
			{
				"Loner"
			}
		},
		{
			"anim_idle_mole_hands_kanim",
			new string[]
			{
				"MoleHands"
			}
		},
		{
			"anim_idle_buff_kanim",
			new string[]
			{
				"StrongArm"
			}
		},
		{
			"anim_idle_distracted_kanim",
			new string[]
			{
				"CantResearch",
				"CantBuild",
				"CantCook",
				"CantDig"
			}
		},
		{
			"anim_idle_coaster_kanim",
			new string[]
			{
				"HappySinger"
			}
		}
	};

	// Token: 0x04003A8E RID: 14990
	private List<Tag> allMinionModels = new List<Tag>
	{
		GameTags.Minions.Models.Standard,
		GameTags.Minions.Models.Bionic
	};

	// Token: 0x04003A8F RID: 14991
	private static readonly HashedString[] idleAnims = new HashedString[]
	{
		"anim_idle_healthy_kanim",
		"anim_idle_susceptible_kanim",
		"anim_idle_keener_kanim",
		"anim_idle_fastfeet_kanim",
		"anim_idle_breatherdeep_kanim",
		"anim_idle_breathershallow_kanim"
	};

	// Token: 0x04003A90 RID: 14992
	public float baseCharacterScale = 0.38f;

	// Token: 0x02001BEE RID: 7150
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x04008123 RID: 33059
		public string professionName;

		// Token: 0x04008124 RID: 33060
		public Sprite iconImg;
	}

	// Token: 0x02001BEF RID: 7151
	private class MinionModelOption : IListableOption
	{
		// Token: 0x0600A4DC RID: 42204 RVA: 0x0038DB29 File Offset: 0x0038BD29
		public MinionModelOption(string name, List<Tag> permittedModels, Sprite sprite)
		{
			this.properName = name;
			this.permittedModels = permittedModels;
			this.sprite = sprite;
		}

		// Token: 0x0600A4DD RID: 42205 RVA: 0x0038DB46 File Offset: 0x0038BD46
		public string GetProperName()
		{
			return this.properName;
		}

		// Token: 0x04008125 RID: 33061
		private string properName;

		// Token: 0x04008126 RID: 33062
		public List<Tag> permittedModels;

		// Token: 0x04008127 RID: 33063
		public Sprite sprite;
	}
}

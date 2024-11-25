using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B86 RID: 2950
public class BuildMenuCategoriesScreen : KIconToggleMenu
{
	// Token: 0x060058B7 RID: 22711 RVA: 0x00200157 File Offset: 0x001FE357
	public override float GetSortKey()
	{
		return 7f;
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x060058B8 RID: 22712 RVA: 0x0020015E File Offset: 0x001FE35E
	public HashedString Category
	{
		get
		{
			return this.category;
		}
	}

	// Token: 0x060058B9 RID: 22713 RVA: 0x00200166 File Offset: 0x001FE366
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.onSelect += this.OnClickCategory;
	}

	// Token: 0x060058BA RID: 22714 RVA: 0x00200180 File Offset: 0x001FE380
	public void Configure(HashedString category, int depth, object data, Dictionary<HashedString, List<BuildingDef>> categorized_building_map, Dictionary<HashedString, List<HashedString>> categorized_category_map, BuildMenuBuildingsScreen buildings_screen)
	{
		this.category = category;
		this.categorizedBuildingMap = categorized_building_map;
		this.categorizedCategoryMap = categorized_category_map;
		this.buildingsScreen = buildings_screen;
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (typeof(IList<BuildMenu.BuildingInfo>).IsAssignableFrom(data.GetType()))
		{
			this.buildingInfos = (IList<BuildMenu.BuildingInfo>)data;
		}
		else if (typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(data.GetType()))
		{
			this.subcategories = new List<HashedString>();
			foreach (BuildMenu.DisplayInfo displayInfo in ((IList<BuildMenu.DisplayInfo>)data))
			{
				string iconName = displayInfo.iconName;
				string text = HashCache.Get().Get(displayInfo.category).ToUpper();
				text = text.Replace(" ", "");
				KIconToggleMenu.ToggleInfo item = new KIconToggleMenu.ToggleInfo(Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".NAME"), iconName, new BuildMenuCategoriesScreen.UserData
				{
					category = displayInfo.category,
					depth = depth,
					requirementsState = PlanScreen.RequirementsState.Tech
				}, displayInfo.hotkey, Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".TOOLTIP"), "");
				list.Add(item);
				this.subcategories.Add(displayInfo.category);
			}
			base.Setup(list);
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
			});
		}
		this.UpdateBuildableStates(true);
	}

	// Token: 0x060058BB RID: 22715 RVA: 0x00200328 File Offset: 0x001FE528
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggle_info.userData;
		PlanScreen.RequirementsState requirementsState = userData.requirementsState;
		if (requirementsState - PlanScreen.RequirementsState.Materials <= 1)
		{
			if (this.selectedCategory != userData.category)
			{
				this.selectedCategory = userData.category;
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			}
			else
			{
				this.selectedCategory = HashedString.Invalid;
				this.ClearSelection();
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
			}
		}
		else
		{
			this.selectedCategory = HashedString.Invalid;
			this.ClearSelection();
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		toggle_info.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
		if (this.onCategoryClicked != null)
		{
			this.onCategoryClicked(this.selectedCategory, userData.depth);
		}
	}

	// Token: 0x060058BC RID: 22716 RVA: 0x002003F4 File Offset: 0x001FE5F4
	private void UpdateButtonStates()
	{
		if (this.toggleInfo != null && this.toggleInfo.Count > 0)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggleInfo.userData;
				HashedString x = userData.category;
				PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(x);
				bool flag = categoryRequirements == PlanScreen.RequirementsState.Tech;
				toggleInfo.toggle.gameObject.SetActive(!flag);
				if (categoryRequirements != PlanScreen.RequirementsState.Materials)
				{
					if (categoryRequirements == PlanScreen.RequirementsState.Complete)
					{
						ImageToggleState.State state = (!this.selectedCategory.IsValid || x != this.selectedCategory) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
						if (userData.currentToggleState == null || userData.currentToggleState.GetValueOrDefault() != state)
						{
							userData.currentToggleState = new ImageToggleState.State?(state);
							this.SetImageToggleState(toggleInfo.toggle.gameObject, state);
						}
					}
				}
				else
				{
					toggleInfo.toggle.fgImage.SetAlpha(flag ? 0.2509804f : 1f);
					ImageToggleState.State state2 = (this.selectedCategory.IsValid && x == this.selectedCategory) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					if (userData.currentToggleState == null || userData.currentToggleState.GetValueOrDefault() != state2)
					{
						userData.currentToggleState = new ImageToggleState.State?(state2);
						this.SetImageToggleState(toggleInfo.toggle.gameObject, state2);
					}
				}
				toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject.gameObject.SetActive(flag);
			}
		}
	}

	// Token: 0x060058BD RID: 22717 RVA: 0x002005B8 File Offset: 0x001FE7B8
	private void SetImageToggleState(GameObject target, ImageToggleState.State state)
	{
		ImageToggleState[] components = target.GetComponents<ImageToggleState>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].SetState(state);
		}
	}

	// Token: 0x060058BE RID: 22718 RVA: 0x002005E4 File Offset: 0x001FE7E4
	private PlanScreen.RequirementsState GetCategoryRequirements(HashedString category)
	{
		bool flag = true;
		bool flag2 = true;
		List<BuildingDef> list;
		if (this.categorizedBuildingMap.TryGetValue(category, out list))
		{
			if (list.Count <= 0)
			{
				goto IL_F3;
			}
			using (List<BuildingDef>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BuildingDef buildingDef = enumerator.Current;
					if (buildingDef.ShowInBuildMenu && buildingDef.IsAvailable())
					{
						PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(buildingDef);
						flag = (flag && requirementsState == PlanScreen.RequirementsState.Tech);
						flag2 = (flag2 && (requirementsState == PlanScreen.RequirementsState.Materials || requirementsState == PlanScreen.RequirementsState.Tech));
					}
				}
				goto IL_F3;
			}
		}
		List<HashedString> list2;
		if (this.categorizedCategoryMap.TryGetValue(category, out list2))
		{
			foreach (HashedString hashedString in list2)
			{
				PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(hashedString);
				flag = (flag && categoryRequirements == PlanScreen.RequirementsState.Tech);
				flag2 = (flag2 && (categoryRequirements == PlanScreen.RequirementsState.Materials || categoryRequirements == PlanScreen.RequirementsState.Tech));
			}
		}
		IL_F3:
		PlanScreen.RequirementsState result;
		if (flag)
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (flag2)
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		else
		{
			result = PlanScreen.RequirementsState.Complete;
		}
		if (DebugHandler.InstantBuildMode)
		{
			result = PlanScreen.RequirementsState.Complete;
		}
		return result;
	}

	// Token: 0x060058BF RID: 22719 RVA: 0x0020071C File Offset: 0x001FE91C
	public void UpdateNotifications(ICollection<HashedString> updated_categories)
	{
		if (this.toggleInfo == null)
		{
			return;
		}
		this.UpdateBuildableStates(false);
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
		{
			HashedString item = ((BuildMenuCategoriesScreen.UserData)toggleInfo.userData).category;
			if (updated_categories.Contains(item))
			{
				toggleInfo.toggle.gameObject.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
			}
		}
	}

	// Token: 0x060058C0 RID: 22720 RVA: 0x002007A4 File Offset: 0x001FE9A4
	public override void Close()
	{
		base.Close();
		this.selectedCategory = HashedString.Invalid;
		this.SetHasFocus(false);
		if (this.buildingInfos != null)
		{
			this.buildingsScreen.Close();
		}
	}

	// Token: 0x060058C1 RID: 22721 RVA: 0x002007D1 File Offset: 0x001FE9D1
	[ContextMenu("ForceUpdateBuildableStates")]
	private void ForceUpdateBuildableStates()
	{
		this.UpdateBuildableStates(true);
	}

	// Token: 0x060058C2 RID: 22722 RVA: 0x002007DC File Offset: 0x001FE9DC
	public void UpdateBuildableStates(bool skip_flourish)
	{
		if (this.subcategories != null && this.subcategories.Count > 0)
		{
			this.UpdateButtonStates();
			using (IEnumerator<KIconToggleMenu.ToggleInfo> enumerator = this.toggleInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KIconToggleMenu.ToggleInfo toggleInfo = enumerator.Current;
					BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggleInfo.userData;
					HashedString hashedString = userData.category;
					PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(hashedString);
					if (userData.requirementsState != categoryRequirements)
					{
						userData.requirementsState = categoryRequirements;
						toggleInfo.userData = userData;
						if (!skip_flourish)
						{
							toggleInfo.toggle.ActivateFlourish(false);
							string text = "NotificationPing";
							if (!toggleInfo.toggle.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag(text))
							{
								toggleInfo.toggle.gameObject.GetComponent<Animator>().Play(text);
								BuildMenu.Instance.PlayNewBuildingSounds();
							}
						}
					}
				}
				return;
			}
		}
		this.buildingsScreen.UpdateBuildableStates();
	}

	// Token: 0x060058C3 RID: 22723 RVA: 0x002008E0 File Offset: 0x001FEAE0
	protected override void OnShow(bool show)
	{
		if (this.buildingInfos != null)
		{
			if (show)
			{
				this.buildingsScreen.Configure(this.category, this.buildingInfos);
				this.buildingsScreen.Show(true);
			}
			else
			{
				this.buildingsScreen.Close();
			}
		}
		base.OnShow(show);
	}

	// Token: 0x060058C4 RID: 22724 RVA: 0x00200930 File Offset: 0x001FEB30
	public override void ClearSelection()
	{
		this.selectedCategory = HashedString.Invalid;
		base.ClearSelection();
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.isOn = false;
		}
	}

	// Token: 0x060058C5 RID: 22725 RVA: 0x00200994 File Offset: 0x001FEB94
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.modalKeyInputBehaviour)
		{
			if (this.HasFocus)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					Game.Instance.Trigger(288942073, null);
					return;
				}
				base.OnKeyDown(e);
				if (!e.Consumed)
				{
					global::Action action = e.GetAction();
					if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
					{
						e.TryConsume(action);
						return;
					}
				}
			}
		}
		else
		{
			base.OnKeyDown(e);
			if (e.Consumed)
			{
				this.UpdateButtonStates();
			}
		}
	}

	// Token: 0x060058C6 RID: 22726 RVA: 0x00200A04 File Offset: 0x001FEC04
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.modalKeyInputBehaviour)
		{
			if (this.HasFocus)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					Game.Instance.Trigger(288942073, null);
					return;
				}
				base.OnKeyUp(e);
				if (!e.Consumed)
				{
					global::Action action = e.GetAction();
					if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
					{
						e.TryConsume(action);
						return;
					}
				}
			}
		}
		else
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x060058C7 RID: 22727 RVA: 0x00200A66 File Offset: 0x001FEC66
	public override void SetHasFocus(bool has_focus)
	{
		base.SetHasFocus(has_focus);
		if (this.focusIndicator != null)
		{
			this.focusIndicator.color = (has_focus ? this.focusedColour : this.unfocusedColour);
		}
	}

	// Token: 0x04003A3F RID: 14911
	public Action<HashedString, int> onCategoryClicked;

	// Token: 0x04003A40 RID: 14912
	[SerializeField]
	public bool modalKeyInputBehaviour;

	// Token: 0x04003A41 RID: 14913
	[SerializeField]
	private Image focusIndicator;

	// Token: 0x04003A42 RID: 14914
	[SerializeField]
	private Color32 focusedColour;

	// Token: 0x04003A43 RID: 14915
	[SerializeField]
	private Color32 unfocusedColour;

	// Token: 0x04003A44 RID: 14916
	private IList<HashedString> subcategories;

	// Token: 0x04003A45 RID: 14917
	private Dictionary<HashedString, List<BuildingDef>> categorizedBuildingMap;

	// Token: 0x04003A46 RID: 14918
	private Dictionary<HashedString, List<HashedString>> categorizedCategoryMap;

	// Token: 0x04003A47 RID: 14919
	private BuildMenuBuildingsScreen buildingsScreen;

	// Token: 0x04003A48 RID: 14920
	private HashedString category;

	// Token: 0x04003A49 RID: 14921
	private IList<BuildMenu.BuildingInfo> buildingInfos;

	// Token: 0x04003A4A RID: 14922
	private HashedString selectedCategory = HashedString.Invalid;

	// Token: 0x02001BE8 RID: 7144
	private class UserData
	{
		// Token: 0x04008114 RID: 33044
		public HashedString category;

		// Token: 0x04008115 RID: 33045
		public int depth;

		// Token: 0x04008116 RID: 33046
		public PlanScreen.RequirementsState requirementsState;

		// Token: 0x04008117 RID: 33047
		public ImageToggleState.State? currentToggleState;
	}
}

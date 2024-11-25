using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6A RID: 3434
public class FilterSideScreen : SingleItemSelectionSideScreenBase
{
	// Token: 0x06006C16 RID: 27670 RVA: 0x0028A8E0 File Offset: 0x00288AE0
	public override bool IsValidForTarget(GameObject target)
	{
		bool flag;
		if (this.isLogicFilter)
		{
			flag = (target.GetComponent<ConduitElementSensor>() != null || target.GetComponent<LogicElementSensor>() != null);
		}
		else
		{
			flag = (target.GetComponent<ElementFilter>() != null || target.GetComponent<RocketConduitStorageAccess>() != null || target.GetComponent<DevPump>() != null);
		}
		return flag && target.GetComponent<Filterable>() != null;
	}

	// Token: 0x06006C17 RID: 27671 RVA: 0x0028A954 File Offset: 0x00288B54
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetFilterable = target.GetComponent<Filterable>();
		if (this.targetFilterable == null)
		{
			return;
		}
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.SOLID;
			goto IL_87;
		case Filterable.ElementState.Gas:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS;
			goto IL_87;
		}
		this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID;
		IL_87:
		this.Configure(this.targetFilterable);
		this.SetFilterTag(this.targetFilterable.SelectedTag);
	}

	// Token: 0x06006C18 RID: 27672 RVA: 0x0028AA05 File Offset: 0x00288C05
	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		this.SetFilterTag(rowClicked.tag);
		base.ItemRowClicked(rowClicked);
	}

	// Token: 0x06006C19 RID: 27673 RVA: 0x0028AA1C File Offset: 0x00288C1C
	private void Configure(Filterable filterable)
	{
		Dictionary<Tag, HashSet<Tag>> tagOptions = filterable.GetTagOptions();
		Tag tag = GameTags.Void;
		foreach (Tag tag2 in tagOptions.Keys)
		{
			using (HashSet<Tag>.Enumerator enumerator2 = tagOptions[tag2].GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == filterable.SelectedTag)
					{
						tag = tag2;
						break;
					}
				}
			}
		}
		this.SetData(tagOptions);
		SingleItemSelectionSideScreenBase.Category category = null;
		if (this.categories.TryGetValue(GameTags.Void, out category))
		{
			category.SetProihibedState(true);
		}
		if (tag != GameTags.Void)
		{
			this.categories[tag].SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
		}
		if (this.voidRow == null)
		{
			this.voidRow = this.GetOrCreateItemRow(GameTags.Void);
		}
		this.voidRow.transform.SetAsFirstSibling();
		if (filterable.SelectedTag != GameTags.Void)
		{
			this.SetSelectedItem(filterable.SelectedTag);
		}
		else
		{
			this.SetSelectedItem(this.voidRow);
		}
		this.RefreshUI();
	}

	// Token: 0x06006C1A RID: 27674 RVA: 0x0028AB6C File Offset: 0x00288D6C
	private void SetFilterTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		if (tag.IsValid)
		{
			this.targetFilterable.SelectedTag = tag;
		}
		this.RefreshUI();
	}

	// Token: 0x06006C1B RID: 27675 RVA: 0x0028AB98 File Offset: 0x00288D98
	private void RefreshUI()
	{
		LocString loc_string;
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.SOLID;
			goto IL_38;
		case Filterable.ElementState.Gas:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS;
			goto IL_38;
		}
		loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID;
		IL_38:
		this.currentSelectionLabel.text = string.Format(loc_string, UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
		if (base.CurrentSelectedItem == null || base.CurrentSelectedItem.tag != this.targetFilterable.SelectedTag)
		{
			this.SetSelectedItem(this.targetFilterable.SelectedTag);
		}
		if (this.targetFilterable.SelectedTag != GameTags.Void)
		{
			this.currentSelectionLabel.text = string.Format(loc_string, this.targetFilterable.SelectedTag.ProperName());
			return;
		}
		this.currentSelectionLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
	}

	// Token: 0x040049B0 RID: 18864
	public HierarchyReferences categoryFoldoutPrefab;

	// Token: 0x040049B1 RID: 18865
	public RectTransform elementEntryContainer;

	// Token: 0x040049B2 RID: 18866
	public Image outputIcon;

	// Token: 0x040049B3 RID: 18867
	public Image everythingElseIcon;

	// Token: 0x040049B4 RID: 18868
	public LocText outputElementHeaderLabel;

	// Token: 0x040049B5 RID: 18869
	public LocText everythingElseHeaderLabel;

	// Token: 0x040049B6 RID: 18870
	public LocText selectElementHeaderLabel;

	// Token: 0x040049B7 RID: 18871
	public LocText currentSelectionLabel;

	// Token: 0x040049B8 RID: 18872
	private SingleItemSelectionRow voidRow;

	// Token: 0x040049B9 RID: 18873
	public bool isLogicFilter;

	// Token: 0x040049BA RID: 18874
	private Filterable targetFilterable;
}

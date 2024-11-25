using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DA7 RID: 3495
public class SingleItemSelectionSideScreen : SingleItemSelectionSideScreenBase
{
	// Token: 0x06006E5E RID: 28254 RVA: 0x00297CF8 File Offset: 0x00295EF8
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<StorageTile.Instance>() != null && target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x06006E5F RID: 28255 RVA: 0x00297D10 File Offset: 0x00295F10
	private Tag GetTargetCurrentSelectedTag()
	{
		if (this.CurrentTarget != null)
		{
			return this.CurrentTarget.TargetTag;
		}
		return this.INVALID_OPTION_TAG;
	}

	// Token: 0x06006E60 RID: 28256 RVA: 0x00297D2C File Offset: 0x00295F2C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.CurrentTarget = target.GetSMI<StorageTile.Instance>();
		if (this.CurrentTarget != null)
		{
			Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
			foreach (Tag tag in new HashSet<Tag>(this.CurrentTarget.GetComponent<Storage>().storageFilters))
			{
				HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag);
				if (discoveredResourcesFromTag != null && discoveredResourcesFromTag.Count > 0)
				{
					dictionary.Add(tag, discoveredResourcesFromTag);
				}
			}
			this.SetData(dictionary);
			SingleItemSelectionSideScreenBase.Category category = null;
			if (!this.categories.TryGetValue(this.INVALID_OPTION_TAG, out category))
			{
				category = base.GetCategoryWithItem(this.INVALID_OPTION_TAG, false);
			}
			if (category != null)
			{
				category.SetProihibedState(true);
			}
			this.CreateNoneOption();
			Tag targetCurrentSelectedTag = this.GetTargetCurrentSelectedTag();
			if (targetCurrentSelectedTag != this.INVALID_OPTION_TAG)
			{
				this.SetSelectedItem(targetCurrentSelectedTag);
				base.GetCategoryWithItem(targetCurrentSelectedTag, false).SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
			}
			else
			{
				this.SetSelectedItem(this.noneOptionRow);
			}
			this.selectedItemLabel.SetItem(targetCurrentSelectedTag);
		}
	}

	// Token: 0x06006E61 RID: 28257 RVA: 0x00297E54 File Offset: 0x00296054
	private void CreateNoneOption()
	{
		if (this.noneOptionRow == null)
		{
			this.noneOptionRow = this.GetOrCreateItemRow(this.INVALID_OPTION_TAG);
		}
		this.noneOptionRow.transform.SetAsFirstSibling();
	}

	// Token: 0x06006E62 RID: 28258 RVA: 0x00297E88 File Offset: 0x00296088
	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		base.ItemRowClicked(rowClicked);
		this.selectedItemLabel.SetItem(rowClicked.tag);
		Tag targetCurrentSelectedTag = this.GetTargetCurrentSelectedTag();
		if (this.CurrentTarget != null && targetCurrentSelectedTag != rowClicked.tag)
		{
			this.CurrentTarget.SetTargetItem(rowClicked.tag);
		}
	}

	// Token: 0x04004B52 RID: 19282
	[SerializeField]
	private SingleItemSelectionSideScreen_SelectedItemSection selectedItemLabel;

	// Token: 0x04004B53 RID: 19283
	private StorageTile.Instance CurrentTarget;

	// Token: 0x04004B54 RID: 19284
	private SingleItemSelectionRow noneOptionRow;

	// Token: 0x04004B55 RID: 19285
	private Tag INVALID_OPTION_TAG = GameTags.Void;
}

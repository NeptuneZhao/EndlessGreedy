using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006CE RID: 1742
[AddComponentMenu("KMonoBehaviour/scripts/Filterable")]
public class Filterable : KMonoBehaviour
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06002C0C RID: 11276 RVA: 0x000F7910 File Offset: 0x000F5B10
	// (remove) Token: 0x06002C0D RID: 11277 RVA: 0x000F7948 File Offset: 0x000F5B48
	public event Action<Tag> onFilterChanged;

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06002C0E RID: 11278 RVA: 0x000F797D File Offset: 0x000F5B7D
	// (set) Token: 0x06002C0F RID: 11279 RVA: 0x000F7985 File Offset: 0x000F5B85
	public Tag SelectedTag
	{
		get
		{
			return this.selectedTag;
		}
		set
		{
			this.selectedTag = value;
			this.OnFilterChanged();
		}
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x000F7994 File Offset: 0x000F5B94
	public Dictionary<Tag, HashSet<Tag>> GetTagOptions()
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		if (this.filterElementState == Filterable.ElementState.Solid)
		{
			dictionary = DiscoveredResources.Instance.GetDiscoveredResourcesFromTagSet(Filterable.filterableCategories);
		}
		else
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (!element.disabled && ((element.IsGas && this.filterElementState == Filterable.ElementState.Gas) || (element.IsLiquid && this.filterElementState == Filterable.ElementState.Liquid)))
				{
					Tag materialCategoryTag = element.GetMaterialCategoryTag();
					if (!dictionary.ContainsKey(materialCategoryTag))
					{
						dictionary[materialCategoryTag] = new HashSet<Tag>();
					}
					Tag item = GameTagExtensions.Create(element.id);
					dictionary[materialCategoryTag].Add(item);
				}
			}
		}
		dictionary.Add(GameTags.Void, new HashSet<Tag>
		{
			GameTags.Void
		});
		return dictionary;
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x000F7A84 File Offset: 0x000F5C84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Filterable>(-905833192, Filterable.OnCopySettingsDelegate);
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x000F7AA0 File Offset: 0x000F5CA0
	private void OnCopySettings(object data)
	{
		Filterable component = ((GameObject)data).GetComponent<Filterable>();
		if (component != null)
		{
			this.SelectedTag = component.SelectedTag;
		}
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x000F7ACE File Offset: 0x000F5CCE
	protected override void OnSpawn()
	{
		this.OnFilterChanged();
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x000F7AD8 File Offset: 0x000F5CD8
	private void OnFilterChanged()
	{
		if (this.onFilterChanged != null)
		{
			this.onFilterChanged(this.selectedTag);
		}
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Filterable.filterSelected, this.selectedTag.IsValid);
		}
	}

	// Token: 0x04001963 RID: 6499
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001964 RID: 6500
	[Serialize]
	public Filterable.ElementState filterElementState;

	// Token: 0x04001965 RID: 6501
	[Serialize]
	private Tag selectedTag = GameTags.Void;

	// Token: 0x04001967 RID: 6503
	private static TagSet filterableCategories = new TagSet(new TagSet[]
	{
		GameTags.CalorieCategories,
		GameTags.UnitCategories,
		GameTags.MaterialCategories,
		GameTags.MaterialBuildingElements
	});

	// Token: 0x04001968 RID: 6504
	private static readonly Operational.Flag filterSelected = new Operational.Flag("filterSelected", Operational.Flag.Type.Requirement);

	// Token: 0x04001969 RID: 6505
	private static readonly EventSystem.IntraObjectHandler<Filterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Filterable>(delegate(Filterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020014D1 RID: 5329
	public enum ElementState
	{
		// Token: 0x04006B03 RID: 27395
		None,
		// Token: 0x04006B04 RID: 27396
		Solid,
		// Token: 0x04006B05 RID: 27397
		Liquid,
		// Token: 0x04006B06 RID: 27398
		Gas
	}
}

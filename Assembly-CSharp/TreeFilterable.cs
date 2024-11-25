using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200078F RID: 1935
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterable")]
public class TreeFilterable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x060034D4 RID: 13524 RVA: 0x0011FD49 File Offset: 0x0011DF49
	public HashSet<Tag> AcceptedTags
	{
		get
		{
			return this.acceptedTagSet;
		}
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x0011FD54 File Offset: 0x0011DF54
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			this.filterByStorageCategoriesOnSpawn = false;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.acceptedTagSet.UnionWith(this.acceptedTags);
			this.acceptedTags = null;
		}
	}

	// Token: 0x060034D6 RID: 13526 RVA: 0x0011FDB0 File Offset: 0x0011DFB0
	private void OnDiscover(Tag category_tag, Tag tag)
	{
		if (this.preventAutoAddOnDiscovery)
		{
			return;
		}
		if (this.storage.storageFilters.Contains(category_tag))
		{
			bool flag = false;
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag).Count <= 1)
			{
				foreach (Tag tag2 in this.storage.storageFilters)
				{
					if (!(tag2 == category_tag) && DiscoveredResources.Instance.IsDiscovered(tag2))
					{
						flag = true;
						foreach (Tag item in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2))
						{
							if (!this.acceptedTagSet.Contains(item))
							{
								return;
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
			}
			foreach (Tag tag3 in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag))
			{
				if (!(tag3 == tag) && !this.acceptedTagSet.Contains(tag3))
				{
					return;
				}
			}
			this.AddTagToFilter(tag);
		}
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x0011FF0C File Offset: 0x0011E10C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<TreeFilterable>(-905833192, TreeFilterable.OnCopySettingsDelegate);
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x0011FF28 File Offset: 0x0011E128
	protected override void OnSpawn()
	{
		DiscoveredResources.Instance.OnDiscover += this.OnDiscover;
		if (this.autoSelectStoredOnLoad && this.storage != null)
		{
			HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
			hashSet.UnionWith(this.storage.GetAllIDsInStorage());
			this.UpdateFilters(hashSet);
		}
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (this.filterByStorageCategoriesOnSpawn)
		{
			this.RemoveIncorrectAcceptedTags();
		}
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x0011FFB4 File Offset: 0x0011E1B4
	private void RemoveIncorrectAcceptedTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (Tag item in this.acceptedTagSet)
		{
			bool flag = false;
			foreach (Tag tag in this.storage.storageFilters)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(item);
			}
		}
		foreach (Tag t in list)
		{
			this.RemoveTagFromFilter(t);
		}
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x001200AC File Offset: 0x0011E2AC
	protected override void OnCleanUp()
	{
		DiscoveredResources.Instance.OnDiscover -= this.OnDiscover;
		base.OnCleanUp();
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x001200CC File Offset: 0x0011E2CC
	private void OnCopySettings(object data)
	{
		if (this.copySettingsEnabled)
		{
			TreeFilterable component = ((GameObject)data).GetComponent<TreeFilterable>();
			if (component != null)
			{
				this.UpdateFilters(component.GetTags());
			}
		}
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x00120102 File Offset: 0x0011E302
	public HashSet<Tag> GetTags()
	{
		return this.acceptedTagSet;
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x0012010A File Offset: 0x0011E30A
	public bool ContainsTag(Tag t)
	{
		return this.acceptedTagSet.Contains(t);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x00120118 File Offset: 0x0011E318
	public void AddTagToFilter(Tag t)
	{
		if (this.ContainsTag(t))
		{
			return;
		}
		this.UpdateFilters(new HashSet<Tag>(this.acceptedTagSet)
		{
			t
		});
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x0012014C File Offset: 0x0011E34C
	public void RemoveTagFromFilter(Tag t)
	{
		if (!this.ContainsTag(t))
		{
			return;
		}
		HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
		hashSet.Remove(t);
		this.UpdateFilters(hashSet);
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x00120180 File Offset: 0x0011E380
	public void UpdateFilters(HashSet<Tag> filters)
	{
		this.acceptedTagSet.Clear();
		this.acceptedTagSet.UnionWith(filters);
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (!this.dropIncorrectOnFilterChange || this.storage == null || this.storage.items == null)
		{
			return;
		}
		if (!this.filterAllStoragesOnBuilding)
		{
			this.DropFilteredItemsFromTargetStorage(this.storage);
			return;
		}
		foreach (Storage targetStorage in base.GetComponents<Storage>())
		{
			this.DropFilteredItemsFromTargetStorage(targetStorage);
		}
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x0012021C File Offset: 0x0011E41C
	private void DropFilteredItemsFromTargetStorage(Storage targetStorage)
	{
		for (int i = targetStorage.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = targetStorage.items[i];
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!this.acceptedTagSet.Contains(component.PrefabTag))
				{
					targetStorage.Drop(gameObject, true);
				}
			}
		}
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x0012027C File Offset: 0x0011E47C
	public string GetTagsAsStatus(int maxDisplays = 6)
	{
		string text = "Tags:\n";
		List<Tag> list = new List<Tag>(this.storage.storageFilters);
		list.Intersect(this.acceptedTagSet);
		for (int i = 0; i < Mathf.Min(list.Count, maxDisplays); i++)
		{
			text += list[i].ProperName();
			if (i < Mathf.Min(list.Count, maxDisplays) - 1)
			{
				text += "\n";
			}
			if (i == maxDisplays - 1 && list.Count > maxDisplays)
			{
				text += "\n...";
				break;
			}
		}
		if (base.tag.Length == 0)
		{
			text = "No tags selected";
		}
		return text;
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x00120328 File Offset: 0x0011E528
	private void RefreshTint()
	{
		bool flag = this.acceptedTagSet != null && this.acceptedTagSet.Count != 0;
		base.GetComponent<KBatchedAnimController>().TintColour = (flag ? this.filterTint : this.noFilterTint);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoStorageFilterSet, !flag, this);
	}

	// Token: 0x04001F3B RID: 7995
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001F3C RID: 7996
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F3D RID: 7997
	public static readonly Color32 FILTER_TINT = Color.white;

	// Token: 0x04001F3E RID: 7998
	public static readonly Color32 NO_FILTER_TINT = new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);

	// Token: 0x04001F3F RID: 7999
	public Color32 filterTint = TreeFilterable.FILTER_TINT;

	// Token: 0x04001F40 RID: 8000
	public Color32 noFilterTint = TreeFilterable.NO_FILTER_TINT;

	// Token: 0x04001F41 RID: 8001
	[SerializeField]
	public bool dropIncorrectOnFilterChange = true;

	// Token: 0x04001F42 RID: 8002
	[SerializeField]
	public bool autoSelectStoredOnLoad = true;

	// Token: 0x04001F43 RID: 8003
	public bool showUserMenu = true;

	// Token: 0x04001F44 RID: 8004
	public bool copySettingsEnabled = true;

	// Token: 0x04001F45 RID: 8005
	public bool preventAutoAddOnDiscovery;

	// Token: 0x04001F46 RID: 8006
	public string allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;

	// Token: 0x04001F47 RID: 8007
	public bool filterAllStoragesOnBuilding;

	// Token: 0x04001F48 RID: 8008
	public TreeFilterable.UISideScreenHeight uiHeight = TreeFilterable.UISideScreenHeight.Tall;

	// Token: 0x04001F49 RID: 8009
	public bool filterByStorageCategoriesOnSpawn = true;

	// Token: 0x04001F4A RID: 8010
	[SerializeField]
	[Serialize]
	[Obsolete("Deprecated, use acceptedTagSet")]
	private List<Tag> acceptedTags = new List<Tag>();

	// Token: 0x04001F4B RID: 8011
	[SerializeField]
	[Serialize]
	private HashSet<Tag> acceptedTagSet = new HashSet<Tag>();

	// Token: 0x04001F4C RID: 8012
	public Action<HashSet<Tag>> OnFilterChanged;

	// Token: 0x04001F4D RID: 8013
	private static readonly EventSystem.IntraObjectHandler<TreeFilterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<TreeFilterable>(delegate(TreeFilterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001643 RID: 5699
	public enum UISideScreenHeight
	{
		// Token: 0x04006F27 RID: 28455
		Short,
		// Token: 0x04006F28 RID: 28456
		Tall
	}
}

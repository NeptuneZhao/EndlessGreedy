using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006D1 RID: 1745
public class FlatTagFilterable : KMonoBehaviour
{
	// Token: 0x06002C36 RID: 11318 RVA: 0x000F83CF File Offset: 0x000F65CF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.filterByStorageCategoriesOnSpawn = false;
		component.UpdateFilters(new HashSet<Tag>(this.selectedTags));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x000F840C File Offset: 0x000F660C
	public void SelectTag(Tag tag, bool state)
	{
		global::Debug.Assert(this.tagOptions.Contains(tag), "The tag " + tag.Name + " is not valid for this filterable - it must be added to tagOptions");
		if (state)
		{
			if (!this.selectedTags.Contains(tag))
			{
				this.selectedTags.Add(tag);
			}
		}
		else if (this.selectedTags.Contains(tag))
		{
			this.selectedTags.Remove(tag);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x000F8490 File Offset: 0x000F6690
	public void ToggleTag(Tag tag)
	{
		this.SelectTag(tag, !this.selectedTags.Contains(tag));
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x000F84A8 File Offset: 0x000F66A8
	public string GetHeaderText()
	{
		return this.headerText;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x000F84B0 File Offset: 0x000F66B0
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (base.GetComponent<KPrefabID>().PrefabID() != gameObject.GetComponent<KPrefabID>().PrefabID())
		{
			return;
		}
		this.selectedTags.Clear();
		foreach (Tag tag in gameObject.GetComponent<FlatTagFilterable>().selectedTags)
		{
			this.SelectTag(tag, true);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x0400197A RID: 6522
	[Serialize]
	public List<Tag> selectedTags = new List<Tag>();

	// Token: 0x0400197B RID: 6523
	public List<Tag> tagOptions = new List<Tag>();

	// Token: 0x0400197C RID: 6524
	public string headerText;

	// Token: 0x0400197D RID: 6525
	public bool displayOnlyDiscoveredTags = true;
}

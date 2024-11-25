using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6C RID: 3436
public class FlatTagFilterSideScreen : SideScreenContent
{
	// Token: 0x06006C20 RID: 27680 RVA: 0x0028ACC5 File Offset: 0x00288EC5
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FlatTagFilterable>() != null;
	}

	// Token: 0x06006C21 RID: 27681 RVA: 0x0028ACD3 File Offset: 0x00288ED3
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.tagFilterable = target.GetComponent<FlatTagFilterable>();
		this.Build();
	}

	// Token: 0x06006C22 RID: 27682 RVA: 0x0028ACF0 File Offset: 0x00288EF0
	private void Build()
	{
		this.headerLabel.SetText(this.tagFilterable.GetHeaderText());
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.tagFilterable.tagOptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
			gameObject.gameObject.name = tag.ProperName();
			this.rows.Add(tag, gameObject);
		}
		this.Refresh();
	}

	// Token: 0x06006C23 RID: 27683 RVA: 0x0028ADE4 File Offset: 0x00288FE4
	private void Refresh()
	{
		using (Dictionary<Tag, GameObject>.Enumerator enumerator = this.rows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Tag, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.ProperNameStripLink());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.tagFilterable.ToggleTag(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.tagFilterable.selectedTags.Contains(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(!this.tagFilterable.displayOnlyDiscoveredTags || DiscoveredResources.Instance.IsDiscovered(kvp.Key));
			}
		}
	}

	// Token: 0x06006C24 RID: 27684 RVA: 0x0028AFA4 File Offset: 0x002891A4
	public override string GetTitle()
	{
		return this.tagFilterable.gameObject.GetProperName();
	}

	// Token: 0x040049BB RID: 18875
	private FlatTagFilterable tagFilterable;

	// Token: 0x040049BC RID: 18876
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x040049BD RID: 18877
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x040049BE RID: 18878
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x040049BF RID: 18879
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}

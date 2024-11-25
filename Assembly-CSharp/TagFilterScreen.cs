using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BB6 RID: 2998
public class TagFilterScreen : SideScreenContent
{
	// Token: 0x06005AF7 RID: 23287 RVA: 0x0021154B File Offset: 0x0020F74B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x06005AF8 RID: 23288 RVA: 0x0021155C File Offset: 0x0020F75C
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		if (!this.targetFilterable.showUserMenu)
		{
			return;
		}
		this.Filter(this.targetFilterable.AcceptedTags);
		base.Activate();
	}

	// Token: 0x06005AF9 RID: 23289 RVA: 0x002115C8 File Offset: 0x0020F7C8
	protected override void OnActivate()
	{
		this.rootItem = this.BuildDisplay(this.rootTag);
		this.treeControl.SetUserItemRoot(this.rootItem);
		this.treeControl.root.opened = true;
		this.Filter(this.treeControl.root, this.acceptedTags, false);
	}

	// Token: 0x06005AFA RID: 23290 RVA: 0x00211624 File Offset: 0x0020F824
	public static List<Tag> GetAllTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (TagFilterScreen.TagEntry tagEntry in TagFilterScreen.defaultRootTag.children)
		{
			if (tagEntry.tag.IsValid)
			{
				list.Add(tagEntry.tag);
			}
		}
		return list;
	}

	// Token: 0x06005AFB RID: 23291 RVA: 0x00211670 File Offset: 0x0020F870
	private KTreeControl.UserItem BuildDisplay(TagFilterScreen.TagEntry root)
	{
		KTreeControl.UserItem userItem = null;
		if (root.name != null && root.name != "")
		{
			userItem = new KTreeControl.UserItem
			{
				text = root.name,
				userData = root.tag
			};
			List<KTreeControl.UserItem> list = new List<KTreeControl.UserItem>();
			if (root.children != null)
			{
				foreach (TagFilterScreen.TagEntry root2 in root.children)
				{
					list.Add(this.BuildDisplay(root2));
				}
			}
			userItem.children = list;
		}
		return userItem;
	}

	// Token: 0x06005AFC RID: 23292 RVA: 0x002116FC File Offset: 0x0020F8FC
	private static KTreeControl.UserItem CreateTree(string tree_name, Tag tree_tag, IList<Element> items)
	{
		KTreeControl.UserItem userItem = new KTreeControl.UserItem
		{
			text = tree_name,
			userData = tree_tag,
			children = new List<KTreeControl.UserItem>()
		};
		foreach (Element element in items)
		{
			KTreeControl.UserItem item = new KTreeControl.UserItem
			{
				text = element.name,
				userData = GameTagExtensions.Create(element.id)
			};
			userItem.children.Add(item);
		}
		return userItem;
	}

	// Token: 0x06005AFD RID: 23293 RVA: 0x00211798 File Offset: 0x0020F998
	public void SetRootTag(TagFilterScreen.TagEntry root_tag)
	{
		this.rootTag = root_tag;
	}

	// Token: 0x06005AFE RID: 23294 RVA: 0x002117A1 File Offset: 0x0020F9A1
	public void Filter(HashSet<Tag> acceptedTags)
	{
		this.acceptedTags = acceptedTags;
	}

	// Token: 0x06005AFF RID: 23295 RVA: 0x002117AC File Offset: 0x0020F9AC
	private void Filter(KTreeItem root, HashSet<Tag> acceptedTags, bool parentEnabled)
	{
		root.checkboxChecked = (parentEnabled || (root.userData != null && acceptedTags.Contains((Tag)root.userData)));
		foreach (KTreeItem root2 in root.children)
		{
			this.Filter(root2, acceptedTags, root.checkboxChecked);
		}
		if (!root.checkboxChecked && root.children.Count > 0)
		{
			bool checkboxChecked = true;
			using (IEnumerator<KTreeItem> enumerator = root.children.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.checkboxChecked)
					{
						checkboxChecked = false;
						break;
					}
				}
			}
			root.checkboxChecked = checkboxChecked;
		}
	}

	// Token: 0x04003BE8 RID: 15336
	[SerializeField]
	private KTreeControl treeControl;

	// Token: 0x04003BE9 RID: 15337
	private KTreeControl.UserItem rootItem;

	// Token: 0x04003BEA RID: 15338
	private TagFilterScreen.TagEntry rootTag = TagFilterScreen.defaultRootTag;

	// Token: 0x04003BEB RID: 15339
	private HashSet<Tag> acceptedTags = new HashSet<Tag>();

	// Token: 0x04003BEC RID: 15340
	private TreeFilterable targetFilterable;

	// Token: 0x04003BED RID: 15341
	public static TagFilterScreen.TagEntry defaultRootTag = new TagFilterScreen.TagEntry
	{
		name = "All",
		tag = default(Tag),
		children = new TagFilterScreen.TagEntry[0]
	};

	// Token: 0x02001C46 RID: 7238
	public class TagEntry
	{
		// Token: 0x040082AE RID: 33454
		public string name;

		// Token: 0x040082AF RID: 33455
		public Tag tag;

		// Token: 0x040082B0 RID: 33456
		public TagFilterScreen.TagEntry[] children;
	}
}

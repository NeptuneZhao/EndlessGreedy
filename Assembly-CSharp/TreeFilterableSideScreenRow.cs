using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000DB9 RID: 3513
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenRow")]
public class TreeFilterableSideScreenRow : KMonoBehaviour
{
	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x06006F3A RID: 28474 RVA: 0x0029C185 File Offset: 0x0029A385
	// (set) Token: 0x06006F3B RID: 28475 RVA: 0x0029C18D File Offset: 0x0029A38D
	public bool ArrowExpanded { get; private set; }

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x06006F3C RID: 28476 RVA: 0x0029C196 File Offset: 0x0029A396
	// (set) Token: 0x06006F3D RID: 28477 RVA: 0x0029C19E File Offset: 0x0029A39E
	public TreeFilterableSideScreen Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	// Token: 0x06006F3E RID: 28478 RVA: 0x0029C1A8 File Offset: 0x0029A3A8
	public TreeFilterableSideScreenRow.State GetState()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			if (this.parent.GetElementTagAcceptedState(treeFilterableSideScreenElement.GetElementTag()))
			{
				flag = true;
			}
			else
			{
				flag2 = true;
			}
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (this.rowElements.Count <= 0)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		return TreeFilterableSideScreenRow.State.On;
	}

	// Token: 0x06006F3F RID: 28479 RVA: 0x0029C23C File Offset: 0x0029A43C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.checkBoxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (this.parent.CurrentSearchValue == "")
			{
				TreeFilterableSideScreenRow.State state = this.GetState();
				if (state > TreeFilterableSideScreenRow.State.Mixed)
				{
					if (state == TreeFilterableSideScreenRow.State.On)
					{
						this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
						return;
					}
				}
				else
				{
					this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
				}
			}
		}));
	}

	// Token: 0x06006F40 RID: 28480 RVA: 0x0029C26B File Offset: 0x0029A46B
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SetArrowToggleState(this.GetState() > TreeFilterableSideScreenRow.State.Off);
	}

	// Token: 0x06006F41 RID: 28481 RVA: 0x0029C282 File Offset: 0x0029A482
	protected override void OnCmpDisable()
	{
		this.SetArrowToggleState(false);
		base.OnCmpDisable();
	}

	// Token: 0x06006F42 RID: 28482 RVA: 0x0029C291 File Offset: 0x0029A491
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06006F43 RID: 28483 RVA: 0x0029C299 File Offset: 0x0029A499
	public void UpdateCheckBoxVisualState()
	{
		this.checkBoxToggle.ChangeState((int)this.GetState());
		this.visualDirty = false;
	}

	// Token: 0x06006F44 RID: 28484 RVA: 0x0029C2B4 File Offset: 0x0029A4B4
	public void ChangeCheckBoxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			for (int i = 0; i < this.rowElements.Count; i++)
			{
				this.rowElements[i].SetCheckBox(false);
			}
			break;
		case TreeFilterableSideScreenRow.State.On:
			for (int j = 0; j < this.rowElements.Count; j++)
			{
				this.rowElements[j].SetCheckBox(true);
			}
			break;
		}
		this.visualDirty = true;
	}

	// Token: 0x06006F45 RID: 28485 RVA: 0x0029C32E File Offset: 0x0029A52E
	private void ArrowToggleClicked()
	{
		this.SetArrowToggleState(!this.ArrowExpanded);
		this.RefreshArrowToggleState();
	}

	// Token: 0x06006F46 RID: 28486 RVA: 0x0029C345 File Offset: 0x0029A545
	public void SetArrowToggleState(bool state)
	{
		this.ArrowExpanded = state;
		this.RefreshArrowToggleState();
	}

	// Token: 0x06006F47 RID: 28487 RVA: 0x0029C354 File Offset: 0x0029A554
	private void RefreshArrowToggleState()
	{
		this.arrowToggle.ChangeState(this.ArrowExpanded ? 1 : 0);
		this.elementGroup.SetActive(this.ArrowExpanded);
		this.bgImg.enabled = this.ArrowExpanded;
	}

	// Token: 0x06006F48 RID: 28488 RVA: 0x0029C38F File Offset: 0x0029A58F
	private void ArrowToggleDisabledClick()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x06006F49 RID: 28489 RVA: 0x0029C3A1 File Offset: 0x0029A5A1
	public void ShowToggleBox(bool show)
	{
		this.checkBoxToggle.gameObject.SetActive(show);
	}

	// Token: 0x06006F4A RID: 28490 RVA: 0x0029C3B4 File Offset: 0x0029A5B4
	private void OnElementSelectionChanged(Tag t, bool state)
	{
		if (state)
		{
			this.parent.AddTag(t);
		}
		else
		{
			this.parent.RemoveTag(t);
		}
		this.visualDirty = true;
	}

	// Token: 0x06006F4B RID: 28491 RVA: 0x0029C3DC File Offset: 0x0029A5DC
	public void SetElement(Tag mainElementTag, bool state, Dictionary<Tag, bool> filterMap)
	{
		this.subTags.Clear();
		this.rowElements.Clear();
		this.elementName.text = mainElementTag.ProperName();
		this.bgImg.enabled = false;
		string simpleTooltip = string.Format(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.CATEGORYBUTTONTOOLTIP, mainElementTag.ProperName());
		this.checkBoxToggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		if (filterMap.Count == 0)
		{
			if (this.elementGroup.activeInHierarchy)
			{
				this.elementGroup.SetActive(false);
			}
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleDisabledClick);
			this.arrowToggle.ChangeState(0);
		}
		else
		{
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleClicked);
			this.arrowToggle.ChangeState(0);
			foreach (KeyValuePair<Tag, bool> keyValuePair in filterMap)
			{
				TreeFilterableSideScreenElement freeElement = this.parent.elementPool.GetFreeElement(this.elementGroup, true);
				freeElement.Parent = this.parent;
				freeElement.SetTag(keyValuePair.Key);
				freeElement.SetCheckBox(keyValuePair.Value);
				freeElement.OnSelectionChanged = new Action<Tag, bool>(this.OnElementSelectionChanged);
				freeElement.SetCheckBox(this.parent.IsTagAllowed(keyValuePair.Key));
				this.rowElements.Add(freeElement);
				this.subTags.Add(keyValuePair.Key);
			}
		}
		this.UpdateCheckBoxVisualState();
	}

	// Token: 0x06006F4C RID: 28492 RVA: 0x0029C57C File Offset: 0x0029A77C
	public void RefreshRowElements()
	{
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			treeFilterableSideScreenElement.SetCheckBox(this.parent.IsTagAllowed(treeFilterableSideScreenElement.GetElementTag()));
		}
	}

	// Token: 0x06006F4D RID: 28493 RVA: 0x0029C5E0 File Offset: 0x0029A7E0
	public void FilterAgainstSearch(Tag thisCategoryTag, string search)
	{
		bool flag = false;
		bool flag2 = thisCategoryTag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
		search = search.ToUpper();
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			bool flag3 = flag2 || treeFilterableSideScreenElement.GetElementTag().ProperNameStripLink().ToUpper().Contains(search.ToUpper());
			treeFilterableSideScreenElement.gameObject.SetActive(flag3);
			flag = (flag || flag3);
		}
		base.gameObject.SetActive(flag);
		if (search != "" && flag && this.arrowToggle.CurrentState == 0)
		{
			this.SetArrowToggleState(true);
		}
	}

	// Token: 0x04004BE1 RID: 19425
	public bool visualDirty;

	// Token: 0x04004BE2 RID: 19426
	public bool standardCommodity = true;

	// Token: 0x04004BE3 RID: 19427
	[SerializeField]
	private LocText elementName;

	// Token: 0x04004BE4 RID: 19428
	[SerializeField]
	private GameObject elementGroup;

	// Token: 0x04004BE5 RID: 19429
	[SerializeField]
	private MultiToggle checkBoxToggle;

	// Token: 0x04004BE6 RID: 19430
	[SerializeField]
	private MultiToggle arrowToggle;

	// Token: 0x04004BE7 RID: 19431
	[SerializeField]
	private KImage bgImg;

	// Token: 0x04004BE8 RID: 19432
	private List<Tag> subTags = new List<Tag>();

	// Token: 0x04004BE9 RID: 19433
	private List<TreeFilterableSideScreenElement> rowElements = new List<TreeFilterableSideScreenElement>();

	// Token: 0x04004BEA RID: 19434
	private TreeFilterableSideScreen parent;

	// Token: 0x02001EC9 RID: 7881
	public enum State
	{
		// Token: 0x04008B81 RID: 35713
		Off,
		// Token: 0x04008B82 RID: 35714
		Mixed,
		// Token: 0x04008B83 RID: 35715
		On
	}
}

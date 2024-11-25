using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF0 RID: 3568
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownList")]
public class BreakdownList : KMonoBehaviour
{
	// Token: 0x06007143 RID: 28995 RVA: 0x002AD894 File Offset: 0x002ABA94
	public BreakdownListRow AddRow()
	{
		BreakdownListRow breakdownListRow;
		if (this.unusedListRows.Count > 0)
		{
			breakdownListRow = this.unusedListRows[0];
			this.unusedListRows.RemoveAt(0);
		}
		else
		{
			breakdownListRow = UnityEngine.Object.Instantiate<BreakdownListRow>(this.listRowTemplate);
		}
		breakdownListRow.gameObject.transform.SetParent(base.transform);
		breakdownListRow.gameObject.transform.SetAsLastSibling();
		this.listRows.Add(breakdownListRow);
		breakdownListRow.gameObject.SetActive(true);
		return breakdownListRow;
	}

	// Token: 0x06007144 RID: 28996 RVA: 0x002AD915 File Offset: 0x002ABB15
	public GameObject AddCustomRow(GameObject newRow)
	{
		newRow.transform.SetParent(base.transform);
		newRow.gameObject.transform.SetAsLastSibling();
		this.customRows.Add(newRow);
		newRow.SetActive(true);
		return newRow;
	}

	// Token: 0x06007145 RID: 28997 RVA: 0x002AD94C File Offset: 0x002ABB4C
	public void ClearRows()
	{
		foreach (BreakdownListRow breakdownListRow in this.listRows)
		{
			this.unusedListRows.Add(breakdownListRow);
			breakdownListRow.gameObject.SetActive(false);
			breakdownListRow.ClearTooltip();
		}
		this.listRows.Clear();
		foreach (GameObject gameObject in this.customRows)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x06007146 RID: 28998 RVA: 0x002ADA04 File Offset: 0x002ABC04
	public void SetTitle(string title)
	{
		this.headerTitle.text = title;
	}

	// Token: 0x06007147 RID: 28999 RVA: 0x002ADA12 File Offset: 0x002ABC12
	public void SetDescription(string description)
	{
		if (description != null && description.Length >= 0)
		{
			this.infoTextLabel.gameObject.SetActive(true);
			this.infoTextLabel.text = description;
			return;
		}
		this.infoTextLabel.gameObject.SetActive(false);
	}

	// Token: 0x06007148 RID: 29000 RVA: 0x002ADA4F File Offset: 0x002ABC4F
	public void SetIcon(Sprite icon)
	{
		this.headerIcon.sprite = icon;
	}

	// Token: 0x04004DED RID: 19949
	public Image headerIcon;

	// Token: 0x04004DEE RID: 19950
	public Sprite headerIconSprite;

	// Token: 0x04004DEF RID: 19951
	public Image headerBar;

	// Token: 0x04004DF0 RID: 19952
	public LocText headerTitle;

	// Token: 0x04004DF1 RID: 19953
	public LocText headerValue;

	// Token: 0x04004DF2 RID: 19954
	public LocText infoTextLabel;

	// Token: 0x04004DF3 RID: 19955
	public BreakdownListRow listRowTemplate;

	// Token: 0x04004DF4 RID: 19956
	private List<BreakdownListRow> listRows = new List<BreakdownListRow>();

	// Token: 0x04004DF5 RID: 19957
	private List<BreakdownListRow> unusedListRows = new List<BreakdownListRow>();

	// Token: 0x04004DF6 RID: 19958
	private List<GameObject> customRows = new List<GameObject>();
}

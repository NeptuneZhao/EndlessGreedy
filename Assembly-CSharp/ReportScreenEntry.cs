using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D1D RID: 3357
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenEntry")]
public class ReportScreenEntry : KMonoBehaviour
{
	// Token: 0x060068E8 RID: 26856 RVA: 0x00274610 File Offset: 0x00272810
	public void SetMainEntry(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
	{
		if (this.mainRow == null)
		{
			this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, true).GetComponent<ReportScreenEntryRow>();
			MultiToggle toggle = this.mainRow.toggle;
			toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren = this.mainRow.name.GetComponentInChildren<MultiToggle>();
			componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren2 = this.mainRow.added.GetComponentInChildren<MultiToggle>();
			componentInChildren2.onClick = (System.Action)Delegate.Combine(componentInChildren2.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren3 = this.mainRow.removed.GetComponentInChildren<MultiToggle>();
			componentInChildren3.onClick = (System.Action)Delegate.Combine(componentInChildren3.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren4 = this.mainRow.net.GetComponentInChildren<MultiToggle>();
			componentInChildren4.onClick = (System.Action)Delegate.Combine(componentInChildren4.onClick, new System.Action(this.ToggleContext));
		}
		this.mainRow.SetLine(entry, reportGroup);
		this.currentContextCount = entry.contextEntries.Count;
		for (int i = 0; i < entry.contextEntries.Count; i++)
		{
			if (i >= this.contextRows.Count)
			{
				ReportScreenEntryRow component = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, false).GetComponent<ReportScreenEntryRow>();
				this.contextRows.Add(component);
			}
			this.contextRows[i].SetLine(entry.contextEntries[i], reportGroup);
		}
		this.UpdateVisibility();
	}

	// Token: 0x060068E9 RID: 26857 RVA: 0x002747CF File Offset: 0x002729CF
	private void ToggleContext()
	{
		this.mainRow.toggle.NextState();
		this.UpdateVisibility();
	}

	// Token: 0x060068EA RID: 26858 RVA: 0x002747E8 File Offset: 0x002729E8
	private void UpdateVisibility()
	{
		int i;
		for (i = 0; i < this.currentContextCount; i++)
		{
			this.contextRows[i].gameObject.SetActive(this.mainRow.toggle.CurrentState == 1);
		}
		while (i < this.contextRows.Count)
		{
			this.contextRows[i].gameObject.SetActive(false);
			i++;
		}
	}

	// Token: 0x04004706 RID: 18182
	[SerializeField]
	private ReportScreenEntryRow rowTemplate;

	// Token: 0x04004707 RID: 18183
	private ReportScreenEntryRow mainRow;

	// Token: 0x04004708 RID: 18184
	private List<ReportScreenEntryRow> contextRows = new List<ReportScreenEntryRow>();

	// Token: 0x04004709 RID: 18185
	private int currentContextCount;
}

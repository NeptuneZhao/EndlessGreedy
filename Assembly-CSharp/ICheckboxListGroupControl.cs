using System;

// Token: 0x02000D53 RID: 3411
public interface ICheckboxListGroupControl
{
	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x06006B5F RID: 27487
	string Title { get; }

	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x06006B60 RID: 27488
	string Description { get; }

	// Token: 0x06006B61 RID: 27489
	ICheckboxListGroupControl.ListGroup[] GetData();

	// Token: 0x06006B62 RID: 27490
	bool SidescreenEnabled();

	// Token: 0x06006B63 RID: 27491
	int CheckboxSideScreenSortOrder();

	// Token: 0x02001E83 RID: 7811
	public struct ListGroup
	{
		// Token: 0x0600ABBA RID: 43962 RVA: 0x003A5AC3 File Offset: 0x003A3CC3
		public ListGroup(string title, ICheckboxListGroupControl.CheckboxItem[] checkboxItems, Func<string, string> resolveTitleCallback = null, System.Action onItemClicked = null)
		{
			this.title = title;
			this.checkboxItems = checkboxItems;
			this.resolveTitleCallback = resolveTitleCallback;
			this.onItemClicked = onItemClicked;
		}

		// Token: 0x04008AC9 RID: 35529
		public Func<string, string> resolveTitleCallback;

		// Token: 0x04008ACA RID: 35530
		public System.Action onItemClicked;

		// Token: 0x04008ACB RID: 35531
		public string title;

		// Token: 0x04008ACC RID: 35532
		public ICheckboxListGroupControl.CheckboxItem[] checkboxItems;
	}

	// Token: 0x02001E84 RID: 7812
	public struct CheckboxItem
	{
		// Token: 0x04008ACD RID: 35533
		public string text;

		// Token: 0x04008ACE RID: 35534
		public string tooltip;

		// Token: 0x04008ACF RID: 35535
		public bool isOn;

		// Token: 0x04008AD0 RID: 35536
		public Func<string, bool> overrideLinkActions;

		// Token: 0x04008AD1 RID: 35537
		public Func<string, object, string> resolveTooltipCallback;
	}
}

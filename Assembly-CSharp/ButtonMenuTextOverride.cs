using System;

// Token: 0x02000D4F RID: 3407
[Serializable]
public struct ButtonMenuTextOverride
{
	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x06006B43 RID: 27459 RVA: 0x00285E4E File Offset: 0x0028404E
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.ToolTip);
		}
	}

	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x06006B44 RID: 27460 RVA: 0x00285E77 File Offset: 0x00284077
	public bool HasCancelText
	{
		get
		{
			return !string.IsNullOrEmpty(this.CancelText) && !string.IsNullOrEmpty(this.CancelToolTip);
		}
	}

	// Token: 0x04004921 RID: 18721
	public LocString Text;

	// Token: 0x04004922 RID: 18722
	public LocString CancelText;

	// Token: 0x04004923 RID: 18723
	public LocString ToolTip;

	// Token: 0x04004924 RID: 18724
	public LocString CancelToolTip;
}

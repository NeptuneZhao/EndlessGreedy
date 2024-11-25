using System;

// Token: 0x02000D4E RID: 3406
public interface ISidescreenButtonControl
{
	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x06006B3B RID: 27451
	string SidescreenButtonText { get; }

	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x06006B3C RID: 27452
	string SidescreenButtonTooltip { get; }

	// Token: 0x06006B3D RID: 27453
	void SetButtonTextOverride(ButtonMenuTextOverride textOverride);

	// Token: 0x06006B3E RID: 27454
	bool SidescreenEnabled();

	// Token: 0x06006B3F RID: 27455
	bool SidescreenButtonInteractable();

	// Token: 0x06006B40 RID: 27456
	void OnSidescreenButtonPressed();

	// Token: 0x06006B41 RID: 27457
	int HorizontalGroupID();

	// Token: 0x06006B42 RID: 27458
	int ButtonSideScreenSortOrder();
}

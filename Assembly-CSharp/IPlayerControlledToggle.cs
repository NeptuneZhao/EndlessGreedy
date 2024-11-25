using System;

// Token: 0x02000D8D RID: 3469
public interface IPlayerControlledToggle
{
	// Token: 0x06006D55 RID: 27989
	void ToggledByPlayer();

	// Token: 0x06006D56 RID: 27990
	bool ToggledOn();

	// Token: 0x06006D57 RID: 27991
	KSelectable GetSelectable();

	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x06006D58 RID: 27992
	string SideScreenTitleKey { get; }

	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x06006D59 RID: 27993
	// (set) Token: 0x06006D5A RID: 27994
	bool ToggleRequested { get; set; }
}

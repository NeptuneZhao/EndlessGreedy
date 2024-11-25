using System;

// Token: 0x02000DA4 RID: 3492
public interface ICheckboxControl
{
	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x06006E3F RID: 28223
	string CheckboxTitleKey { get; }

	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x06006E40 RID: 28224
	string CheckboxLabel { get; }

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06006E41 RID: 28225
	string CheckboxTooltip { get; }

	// Token: 0x06006E42 RID: 28226
	bool GetCheckboxValue();

	// Token: 0x06006E43 RID: 28227
	void SetCheckboxValue(bool value);
}

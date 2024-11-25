using System;

// Token: 0x02000D7A RID: 3450
public interface ILogicRibbonBitSelector
{
	// Token: 0x06006C8A RID: 27786
	void SetBitSelection(int bit);

	// Token: 0x06006C8B RID: 27787
	int GetBitSelection();

	// Token: 0x06006C8C RID: 27788
	int GetBitDepth();

	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x06006C8D RID: 27789
	string SideScreenTitle { get; }

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x06006C8E RID: 27790
	string SideScreenDescription { get; }

	// Token: 0x06006C8F RID: 27791
	bool SideScreenDisplayWriterDescription();

	// Token: 0x06006C90 RID: 27792
	bool SideScreenDisplayReaderDescription();

	// Token: 0x06006C91 RID: 27793
	bool IsBitActive(int bit);

	// Token: 0x06006C92 RID: 27794
	int GetOutputValue();

	// Token: 0x06006C93 RID: 27795
	int GetInputValue();

	// Token: 0x06006C94 RID: 27796
	void UpdateVisuals();
}

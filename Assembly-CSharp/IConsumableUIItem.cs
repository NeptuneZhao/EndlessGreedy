using System;

// Token: 0x02000CB1 RID: 3249
public interface IConsumableUIItem
{
	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x0600640A RID: 25610
	string ConsumableId { get; }

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x0600640B RID: 25611
	string ConsumableName { get; }

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x0600640C RID: 25612
	int MajorOrder { get; }

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x0600640D RID: 25613
	int MinorOrder { get; }

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x0600640E RID: 25614
	bool Display { get; }
}

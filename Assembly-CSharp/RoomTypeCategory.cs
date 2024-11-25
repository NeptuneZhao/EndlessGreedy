using System;

// Token: 0x02000600 RID: 1536
public class RoomTypeCategory : Resource
{
	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000D20C1 File Offset: 0x000D02C1
	// (set) Token: 0x060025B5 RID: 9653 RVA: 0x000D20C9 File Offset: 0x000D02C9
	public string colorName { get; private set; }

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000D20D2 File Offset: 0x000D02D2
	// (set) Token: 0x060025B7 RID: 9655 RVA: 0x000D20DA File Offset: 0x000D02DA
	public string icon { get; private set; }

	// Token: 0x060025B8 RID: 9656 RVA: 0x000D20E3 File Offset: 0x000D02E3
	public RoomTypeCategory(string id, string name, string colorName, string icon) : base(id, name)
	{
		this.colorName = colorName;
		this.icon = icon;
	}
}

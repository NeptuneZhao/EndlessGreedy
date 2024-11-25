using System;

// Token: 0x020005FB RID: 1531
public class Accessory : Resource
{
	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x0600258D RID: 9613 RVA: 0x000D1BA0 File Offset: 0x000CFDA0
	// (set) Token: 0x0600258E RID: 9614 RVA: 0x000D1BA8 File Offset: 0x000CFDA8
	public KAnim.Build.Symbol symbol { get; private set; }

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x0600258F RID: 9615 RVA: 0x000D1BB1 File Offset: 0x000CFDB1
	// (set) Token: 0x06002590 RID: 9616 RVA: 0x000D1BB9 File Offset: 0x000CFDB9
	public HashedString batchSource { get; private set; }

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06002591 RID: 9617 RVA: 0x000D1BC2 File Offset: 0x000CFDC2
	// (set) Token: 0x06002592 RID: 9618 RVA: 0x000D1BCA File Offset: 0x000CFDCA
	public AccessorySlot slot { get; private set; }

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06002593 RID: 9619 RVA: 0x000D1BD3 File Offset: 0x000CFDD3
	// (set) Token: 0x06002594 RID: 9620 RVA: 0x000D1BDB File Offset: 0x000CFDDB
	public KAnimFile animFile { get; private set; }

	// Token: 0x06002595 RID: 9621 RVA: 0x000D1BE4 File Offset: 0x000CFDE4
	public Accessory(string id, ResourceSet parent, AccessorySlot slot, HashedString batchSource, KAnim.Build.Symbol symbol, KAnimFile animFile = null, KAnimFile defaultAnimFile = null) : base(id, parent, null)
	{
		this.slot = slot;
		this.symbol = symbol;
		this.batchSource = batchSource;
		this.animFile = animFile;
	}

	// Token: 0x06002596 RID: 9622 RVA: 0x000D1C0E File Offset: 0x000CFE0E
	public bool IsDefault()
	{
		return this.animFile == this.slot.defaultAnimFile;
	}
}

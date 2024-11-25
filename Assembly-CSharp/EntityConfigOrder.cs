using System;

// Token: 0x02000883 RID: 2179
public class EntityConfigOrder : Attribute
{
	// Token: 0x06003D31 RID: 15665 RVA: 0x001529F6 File Offset: 0x00150BF6
	public EntityConfigOrder(int sort_order)
	{
		this.sortOrder = sort_order;
	}

	// Token: 0x0400255C RID: 9564
	public int sortOrder;
}

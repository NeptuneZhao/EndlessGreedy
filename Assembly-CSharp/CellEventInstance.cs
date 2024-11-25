using System;
using KSerialization;

// Token: 0x0200089F RID: 2207
[SerializationConfig(MemberSerialization.OptIn)]
public class CellEventInstance : EventInstanceBase, ISaveLoadable
{
	// Token: 0x06003DD0 RID: 15824 RVA: 0x00155602 File Offset: 0x00153802
	public CellEventInstance(int cell, int data, int data2, CellEvent ev) : base(ev)
	{
		this.cell = cell;
		this.data = data;
		this.data2 = data2;
	}

	// Token: 0x040025B3 RID: 9651
	[Serialize]
	public int cell;

	// Token: 0x040025B4 RID: 9652
	[Serialize]
	public int data;

	// Token: 0x040025B5 RID: 9653
	[Serialize]
	public int data2;
}

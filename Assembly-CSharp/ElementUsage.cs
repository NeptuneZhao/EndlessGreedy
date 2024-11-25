using System;

// Token: 0x02000C10 RID: 3088
public class ElementUsage
{
	// Token: 0x06005EA7 RID: 24231 RVA: 0x00232A70 File Offset: 0x00230C70
	public ElementUsage(Tag tag, float amount, bool continuous) : this(tag, amount, continuous, null)
	{
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x00232A7C File Offset: 0x00230C7C
	public ElementUsage(Tag tag, float amount, bool continuous, Func<Tag, float, bool, string> customFormating)
	{
		this.tag = tag;
		this.amount = amount;
		this.continuous = continuous;
		this.customFormating = customFormating;
	}

	// Token: 0x04003F42 RID: 16194
	public Tag tag;

	// Token: 0x04003F43 RID: 16195
	public float amount;

	// Token: 0x04003F44 RID: 16196
	public bool continuous;

	// Token: 0x04003F45 RID: 16197
	public Func<Tag, float, bool, string> customFormating;
}

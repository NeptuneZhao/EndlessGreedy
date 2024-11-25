using System;
using STRINGS;

// Token: 0x0200086C RID: 2156
internal struct EffectorEntry
{
	// Token: 0x06003C23 RID: 15395 RVA: 0x0014DE8B File Offset: 0x0014C08B
	public EffectorEntry(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x0014DEA4 File Offset: 0x0014C0A4
	public override string ToString()
	{
		string arg = "";
		if (this.count > 1)
		{
			arg = string.Format(UI.OVERLAYS.DECOR.COUNT, this.count);
		}
		return string.Format(UI.OVERLAYS.DECOR.ENTRY, GameUtil.GetFormattedDecor(this.value, false), this.name, arg);
	}

	// Token: 0x04002473 RID: 9331
	public string name;

	// Token: 0x04002474 RID: 9332
	public int count;

	// Token: 0x04002475 RID: 9333
	public float value;
}

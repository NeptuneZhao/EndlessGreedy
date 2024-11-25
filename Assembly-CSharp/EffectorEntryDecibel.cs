using System;

// Token: 0x0200086D RID: 2157
internal struct EffectorEntryDecibel
{
	// Token: 0x06003C25 RID: 15397 RVA: 0x0014DEFD File Offset: 0x0014C0FD
	public EffectorEntryDecibel(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x04002476 RID: 9334
	public string name;

	// Token: 0x04002477 RID: 9335
	public int count;

	// Token: 0x04002478 RID: 9336
	public float value;
}

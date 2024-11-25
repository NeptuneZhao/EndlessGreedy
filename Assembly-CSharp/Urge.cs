using System;

// Token: 0x0200046E RID: 1134
public class Urge : Resource
{
	// Token: 0x06001870 RID: 6256 RVA: 0x00082BE8 File Offset: 0x00080DE8
	public Urge(string id) : base(id, null, null)
	{
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x00082BF3 File Offset: 0x00080DF3
	public override string ToString()
	{
		return this.Id;
	}
}

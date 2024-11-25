using System;

// Token: 0x02000A95 RID: 2709
public class Shirt : Resource
{
	// Token: 0x06004F6C RID: 20332 RVA: 0x001C8B19 File Offset: 0x001C6D19
	public Shirt(string id) : base(id, null, null)
	{
		this.hash = new HashedString(id);
	}

	// Token: 0x040034CA RID: 13514
	public HashedString hash;
}

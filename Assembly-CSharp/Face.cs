using System;

// Token: 0x020008AD RID: 2221
public class Face : Resource
{
	// Token: 0x06003E06 RID: 15878 RVA: 0x00156999 File Offset: 0x00154B99
	public Face(string id, string headFXSymbol = null) : base(id, null, null)
	{
		this.hash = new HashedString(id);
		this.headFXHash = headFXSymbol;
	}

	// Token: 0x04002615 RID: 9749
	public HashedString hash;

	// Token: 0x04002616 RID: 9750
	public HashedString headFXHash;

	// Token: 0x04002617 RID: 9751
	private const string SYMBOL_PREFIX = "headfx_";
}

using System;
using System.Diagnostics;

// Token: 0x020008AB RID: 2219
[DebuggerDisplay("{face.hash} {priority}")]
public class Expression : Resource
{
	// Token: 0x06003E04 RID: 15876 RVA: 0x0015697F File Offset: 0x00154B7F
	public Expression(string id, ResourceSet parent, Face face) : base(id, parent, null)
	{
		this.face = face;
	}

	// Token: 0x04002613 RID: 9747
	public Face face;

	// Token: 0x04002614 RID: 9748
	public int priority;
}

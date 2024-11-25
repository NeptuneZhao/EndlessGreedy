using System;

// Token: 0x0200082F RID: 2095
public class Death : Resource
{
	// Token: 0x06003A42 RID: 14914 RVA: 0x0013E44D File Offset: 0x0013C64D
	public Death(string id, ResourceSet parent, string name, string description, string pre_anim, string loop_anim) : base(id, parent, name)
	{
		this.preAnim = pre_anim;
		this.loopAnim = loop_anim;
		this.description = description;
	}

	// Token: 0x04002301 RID: 8961
	public string preAnim;

	// Token: 0x04002302 RID: 8962
	public string loopAnim;

	// Token: 0x04002303 RID: 8963
	public string sound;

	// Token: 0x04002304 RID: 8964
	public string description;
}

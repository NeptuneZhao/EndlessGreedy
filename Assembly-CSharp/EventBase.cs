using System;

// Token: 0x020008A4 RID: 2212
public class EventBase : Resource
{
	// Token: 0x06003DDF RID: 15839 RVA: 0x0015602F File Offset: 0x0015422F
	public EventBase(string id) : base(id, id)
	{
		this.hash = Hash.SDBMLower(id);
	}

	// Token: 0x06003DE0 RID: 15840 RVA: 0x00156045 File Offset: 0x00154245
	public virtual string GetDescription(EventInstanceBase ev)
	{
		return "";
	}

	// Token: 0x040025F6 RID: 9718
	public int hash;
}

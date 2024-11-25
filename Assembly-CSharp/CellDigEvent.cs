using System;
using System.Diagnostics;

// Token: 0x0200089C RID: 2204
public class CellDigEvent : CellEvent
{
	// Token: 0x06003DC8 RID: 15816 RVA: 0x001554E3 File Offset: 0x001536E3
	public CellDigEvent(bool enable_logging = true) : base("Dig", "Dig", true, enable_logging)
	{
	}

	// Token: 0x06003DC9 RID: 15817 RVA: 0x001554F8 File Offset: 0x001536F8
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x00155524 File Offset: 0x00153724
	public override string GetDescription(EventInstanceBase ev)
	{
		return base.GetMessagePrefix() + "Dig=true";
	}
}

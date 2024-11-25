using System;
using System.Diagnostics;

// Token: 0x020008A2 RID: 2210
public class CellSolidEvent : CellEvent
{
	// Token: 0x06003DD9 RID: 15833 RVA: 0x00155F24 File Offset: 0x00154124
	public CellSolidEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x06003DDA RID: 15834 RVA: 0x00155F34 File Offset: 0x00154134
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, bool solid)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, solid ? 1 : 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003DDB RID: 15835 RVA: 0x00155F68 File Offset: 0x00154168
	public override string GetDescription(EventInstanceBase ev)
	{
		if ((ev as CellEventInstance).data == 1)
		{
			return base.GetMessagePrefix() + "Solid=true (" + this.reason + ")";
		}
		return base.GetMessagePrefix() + "Solid=false (" + this.reason + ")";
	}
}

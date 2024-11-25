using System;
using System.Diagnostics;

// Token: 0x0200089B RID: 2203
public class CellCallbackEvent : CellEvent
{
	// Token: 0x06003DC5 RID: 15813 RVA: 0x00155478 File Offset: 0x00153678
	public CellCallbackEvent(string id, bool is_send, bool enable_logging = true) : base(id, "Callback", is_send, enable_logging)
	{
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x00155488 File Offset: 0x00153688
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, callback_id, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003DC7 RID: 15815 RVA: 0x001554B4 File Offset: 0x001536B4
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Callback=" + cellEventInstance.data.ToString();
	}
}

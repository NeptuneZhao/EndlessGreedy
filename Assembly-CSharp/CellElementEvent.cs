using System;
using System.Diagnostics;

// Token: 0x0200089D RID: 2205
public class CellElementEvent : CellEvent
{
	// Token: 0x06003DCB RID: 15819 RVA: 0x00155536 File Offset: 0x00153736
	public CellElementEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x00155544 File Offset: 0x00153744
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x00155570 File Offset: 0x00153770
	public override string GetDescription(EventInstanceBase ev)
	{
		SimHashes data = (SimHashes)(ev as CellEventInstance).data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			" (",
			this.reason,
			")"
		});
	}
}

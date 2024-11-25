using System;

// Token: 0x0200089E RID: 2206
public class CellEvent : EventBase
{
	// Token: 0x06003DCE RID: 15822 RVA: 0x001555CE File Offset: 0x001537CE
	public CellEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id)
	{
		this.reason = reason;
		this.isSend = is_send;
		this.enableLogging = enable_logging;
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x001555ED File Offset: 0x001537ED
	public string GetMessagePrefix()
	{
		if (this.isSend)
		{
			return ">>>: ";
		}
		return "<<<: ";
	}

	// Token: 0x040025B0 RID: 9648
	public string reason;

	// Token: 0x040025B1 RID: 9649
	public bool isSend;

	// Token: 0x040025B2 RID: 9650
	public bool enableLogging;
}

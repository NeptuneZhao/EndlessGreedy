using System;
using System.Diagnostics;

// Token: 0x0200089A RID: 2202
public class CellAddRemoveSubstanceEvent : CellEvent
{
	// Token: 0x06003DC2 RID: 15810 RVA: 0x001553B5 File Offset: 0x001535B5
	public CellAddRemoveSubstanceEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x06003DC3 RID: 15811 RVA: 0x001553C4 File Offset: 0x001535C4
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x001553F8 File Offset: 0x001535F8
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		SimHashes data = (SimHashes)cellEventInstance.data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			", Mass=",
			((float)cellEventInstance.data2 / 1000f).ToString(),
			" (",
			this.reason,
			")"
		});
	}
}

using System;
using System.Diagnostics;

// Token: 0x020008A1 RID: 2209
public class CellModifyMassEvent : CellEvent
{
	// Token: 0x06003DD6 RID: 15830 RVA: 0x00155E63 File Offset: 0x00154063
	public CellModifyMassEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x00155E70 File Offset: 0x00154070
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x00155EA4 File Offset: 0x001540A4
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

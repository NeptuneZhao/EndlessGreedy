using System;
using System.Diagnostics;

// Token: 0x020008A3 RID: 2211
public class CellSolidFilterEvent : CellEvent
{
	// Token: 0x06003DDC RID: 15836 RVA: 0x00155FBA File Offset: 0x001541BA
	public CellSolidFilterEvent(string id, bool enable_logging = true) : base(id, "filtered", false, enable_logging)
	{
	}

	// Token: 0x06003DDD RID: 15837 RVA: 0x00155FCC File Offset: 0x001541CC
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

	// Token: 0x06003DDE RID: 15838 RVA: 0x00156000 File Offset: 0x00154200
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Filtered Solid Event solid=" + cellEventInstance.data.ToString();
	}
}

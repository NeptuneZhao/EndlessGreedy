using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000BD2 RID: 3026
public class DecorDisplayer : StandardAmountDisplayer
{
	// Token: 0x06005C41 RID: 23617 RVA: 0x0021C5CE File Offset: 0x0021A7CE
	public DecorDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new DecorDisplayer.DecorAttributeFormatter();
	}

	// Token: 0x06005C42 RID: 23618 RVA: 0x0021C5E8 File Offset: 0x0021A7E8
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(LocText.ParseText(master.description), this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		int cell = Grid.PosToCell(instance.gameObject);
		if (Grid.IsValidCell(cell))
		{
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, GameUtil.GetDecorAtCell(cell));
		}
		text += "\n";
		DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
		if (smi != null)
		{
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor(), GameUtil.TimeSlice.None));
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor(), GameUtil.TimeSlice.None));
		}
		return text;
	}

	// Token: 0x02001CC0 RID: 7360
	public class DecorAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600A6DB RID: 42715 RVA: 0x00398E90 File Offset: 0x00397090
		public DecorAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}

using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000BD3 RID: 3027
public class MaturityDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x06005C43 RID: 23619 RVA: 0x0021C6B9 File Offset: 0x0021A8B9
	public MaturityDisplayer() : base(GameUtil.TimeSlice.PerCycle)
	{
		this.formatter = new MaturityDisplayer.MaturityAttributeFormatter();
	}

	// Token: 0x06005C44 RID: 23620 RVA: 0x0021C6D0 File Offset: 0x0021A8D0
	public override string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		string text = base.GetTooltipDescription(master, instance);
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component.IsGrowing())
		{
			float seconds = (instance.GetMax() - instance.value) / instance.GetDelta();
			if (component != null && component.IsGrowing())
			{
				text += string.Format(CREATURES.STATS.MATURITY.TOOLTIP_GROWING_CROP, GameUtil.GetFormattedCycles(seconds, "F1", false), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
			}
			else
			{
				text += string.Format(CREATURES.STATS.MATURITY.TOOLTIP_GROWING, GameUtil.GetFormattedCycles(seconds, "F1", false));
			}
		}
		else if (component.ReachedNextHarvest())
		{
			text += CREATURES.STATS.MATURITY.TOOLTIP_GROWN;
		}
		else
		{
			text += CREATURES.STATS.MATURITY.TOOLTIP_STALLED;
		}
		return text;
	}

	// Token: 0x06005C45 RID: 23621 RVA: 0x0021C7A8 File Offset: 0x0021A9A8
	public override string GetDescription(Amount master, AmountInstance instance)
	{
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component != null && component.IsGrowing())
		{
			return string.Format(CREATURES.STATS.MATURITY.AMOUNT_DESC_FMT, master.Name, this.formatter.GetFormattedValue(base.ToPercent(instance.value, instance), GameUtil.TimeSlice.None), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
		}
		return base.GetDescription(master, instance);
	}

	// Token: 0x02001CC1 RID: 7361
	public class MaturityAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600A6DC RID: 42716 RVA: 0x00398E9A File Offset: 0x0039709A
		public MaturityAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
		{
		}

		// Token: 0x0600A6DD RID: 42717 RVA: 0x00398EA4 File Offset: 0x003970A4
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			float num = modifier.Value;
			GameUtil.TimeSlice timeSlice = base.DeltaTimeSlice;
			if (modifier.IsMultiplier)
			{
				num *= 100f;
				timeSlice = GameUtil.TimeSlice.None;
			}
			return this.GetFormattedValue(num, timeSlice);
		}
	}
}

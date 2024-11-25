using System;
using Klei.AI;

// Token: 0x02000BD8 RID: 3032
public class RadsPerCycleAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005C5A RID: 23642 RVA: 0x0021CEF0 File Offset: 0x0021B0F0
	public RadsPerCycleAttributeFormatter() : base(GameUtil.UnitClass.Radiation, GameUtil.TimeSlice.PerCycle)
	{
	}

	// Token: 0x06005C5B RID: 23643 RVA: 0x0021CEFA File Offset: 0x0021B0FA
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x06005C5C RID: 23644 RVA: 0x0021CF09 File Offset: 0x0021B109
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return base.GetFormattedValue(value / 600f, timeSlice);
	}
}

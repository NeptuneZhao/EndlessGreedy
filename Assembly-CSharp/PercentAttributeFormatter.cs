using System;
using Klei.AI;

// Token: 0x02000BDD RID: 3037
public class PercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005C6A RID: 23658 RVA: 0x0021D0BA File Offset: 0x0021B2BA
	public PercentAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06005C6B RID: 23659 RVA: 0x0021D0C4 File Offset: 0x0021B2C4
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x06005C6C RID: 23660 RVA: 0x0021D0D8 File Offset: 0x0021B2D8
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x06005C6D RID: 23661 RVA: 0x0021D0EC File Offset: 0x0021B2EC
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value * 100f, timeSlice);
	}
}

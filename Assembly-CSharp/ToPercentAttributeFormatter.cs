using System;
using Klei.AI;

// Token: 0x02000BDC RID: 3036
public class ToPercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005C66 RID: 23654 RVA: 0x0021D060 File Offset: 0x0021B260
	public ToPercentAttributeFormatter(float max, GameUtil.TimeSlice deltaTimeSlice = GameUtil.TimeSlice.None) : base(GameUtil.UnitClass.Percent, deltaTimeSlice)
	{
		this.max = max;
	}

	// Token: 0x06005C67 RID: 23655 RVA: 0x0021D07C File Offset: 0x0021B27C
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x06005C68 RID: 23656 RVA: 0x0021D090 File Offset: 0x0021B290
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x06005C69 RID: 23657 RVA: 0x0021D0A4 File Offset: 0x0021B2A4
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value / this.max * 100f, timeSlice);
	}

	// Token: 0x04003DA4 RID: 15780
	public float max = 1f;
}

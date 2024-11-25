using System;
using Klei.AI;

// Token: 0x02000BD9 RID: 3033
public class FoodQualityAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005C5D RID: 23645 RVA: 0x0021CF19 File Offset: 0x0021B119
	public FoodQualityAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06005C5E RID: 23646 RVA: 0x0021CF23 File Offset: 0x0021B123
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None);
	}

	// Token: 0x06005C5F RID: 23647 RVA: 0x0021CF32 File Offset: 0x0021B132
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetFormattedInt(modifier.Value, GameUtil.TimeSlice.None);
	}

	// Token: 0x06005C60 RID: 23648 RVA: 0x0021CF40 File Offset: 0x0021B140
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return Util.StripTextFormatting(GameUtil.GetFormattedFoodQuality((int)value));
	}
}

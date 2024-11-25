using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000BDA RID: 3034
public class QualityOfLifeAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005C61 RID: 23649 RVA: 0x0021CF4E File Offset: 0x0021B14E
	public QualityOfLifeAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06005C62 RID: 23650 RVA: 0x0021CF58 File Offset: 0x0021B158
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
		return string.Format(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.DESC_FORMAT, this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None), this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), GameUtil.TimeSlice.None));
	}

	// Token: 0x06005C63 RID: 23651 RVA: 0x0021CFAC File Offset: 0x0021B1AC
	public override string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
	{
		string text = base.GetTooltip(master, instance);
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
		text = text + "\n\n" + string.Format(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION, this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), GameUtil.TimeSlice.None));
		if (instance.GetTotalDisplayValue() - attributeInstance.GetTotalDisplayValue() >= 0f)
		{
			text = text + "\n\n" + DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_OVER;
		}
		else
		{
			text = text + "\n\n" + DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_UNDER;
		}
		return text;
	}
}

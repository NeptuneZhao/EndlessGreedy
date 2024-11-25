using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000BCC RID: 3020
public class AsPercentAmountDisplayer : IAmountDisplayer
{
	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06005C2A RID: 23594 RVA: 0x0021BCA7 File Offset: 0x00219EA7
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06005C2B RID: 23595 RVA: 0x0021BCAF File Offset: 0x00219EAF
	// (set) Token: 0x06005C2C RID: 23596 RVA: 0x0021BCBC File Offset: 0x00219EBC
	public GameUtil.TimeSlice DeltaTimeSlice
	{
		get
		{
			return this.formatter.DeltaTimeSlice;
		}
		set
		{
			this.formatter.DeltaTimeSlice = value;
		}
	}

	// Token: 0x06005C2D RID: 23597 RVA: 0x0021BCCA File Offset: 0x00219ECA
	public AsPercentAmountDisplayer(GameUtil.TimeSlice deltaTimeSlice)
	{
		this.formatter = new StandardAttributeFormatter(GameUtil.UnitClass.Percent, deltaTimeSlice);
	}

	// Token: 0x06005C2E RID: 23598 RVA: 0x0021BCDF File Offset: 0x00219EDF
	public string GetValueString(Amount master, AmountInstance instance)
	{
		return this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None);
	}

	// Token: 0x06005C2F RID: 23599 RVA: 0x0021BCFA File Offset: 0x00219EFA
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None));
	}

	// Token: 0x06005C30 RID: 23600 RVA: 0x0021BD25 File Offset: 0x00219F25
	public virtual string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		return string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
	}

	// Token: 0x06005C31 RID: 23601 RVA: 0x0021BD44 File Offset: 0x00219F44
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		text += "\n\n";
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			float modifierContribution = instance.deltaAttribute.GetModifierContribution(attributeModifier);
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice));
		}
		return text;
	}

	// Token: 0x06005C32 RID: 23602 RVA: 0x0021BE6D File Offset: 0x0021A06D
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x0021BE7B File Offset: 0x0021A07B
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		if (modifier.IsMultiplier)
		{
			return GameUtil.GetFormattedPercent(modifier.Value * 100f, GameUtil.TimeSlice.None);
		}
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x06005C34 RID: 23604 RVA: 0x0021BEA4 File Offset: 0x0021A0A4
	public string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return this.formatter.GetFormattedValue(value, timeSlice);
	}

	// Token: 0x06005C35 RID: 23605 RVA: 0x0021BEB3 File Offset: 0x0021A0B3
	protected float ToPercent(float value, AmountInstance instance)
	{
		return 100f * value / instance.GetMax();
	}

	// Token: 0x04003DA0 RID: 15776
	protected StandardAttributeFormatter formatter;
}

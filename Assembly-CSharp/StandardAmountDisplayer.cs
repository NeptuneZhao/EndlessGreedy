using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000BCB RID: 3019
public class StandardAmountDisplayer : IAmountDisplayer
{
	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06005C20 RID: 23584 RVA: 0x0021BA5F File Offset: 0x00219C5F
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06005C21 RID: 23585 RVA: 0x0021BA67 File Offset: 0x00219C67
	// (set) Token: 0x06005C22 RID: 23586 RVA: 0x0021BA74 File Offset: 0x00219C74
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

	// Token: 0x06005C23 RID: 23587 RVA: 0x0021BA82 File Offset: 0x00219C82
	public StandardAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice, StandardAttributeFormatter formatter = null, GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
	{
		this.tense = tense;
		if (formatter != null)
		{
			this.formatter = formatter;
			return;
		}
		this.formatter = new StandardAttributeFormatter(unitClass, deltaTimeSlice);
	}

	// Token: 0x06005C24 RID: 23588 RVA: 0x0021BAAC File Offset: 0x00219CAC
	public virtual string GetValueString(Amount master, AmountInstance instance)
	{
		if (!master.showMax)
		{
			return this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None);
		}
		return string.Format("{0} / {1}", this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), this.formatter.GetFormattedValue(instance.GetMax(), GameUtil.TimeSlice.None));
	}

	// Token: 0x06005C25 RID: 23589 RVA: 0x0021BB02 File Offset: 0x00219D02
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.GetValueString(master, instance));
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x0021BB1C File Offset: 0x00219D1C
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = "";
		if (master.description.IndexOf("{1}") > -1)
		{
			text += string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), GameUtil.GetIdentityDescriptor(instance.gameObject, this.tense));
		}
		else
		{
			text += string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		}
		text += "\n\n";
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerSecond)
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedModifier(attributeModifier));
		}
		return text;
	}

	// Token: 0x06005C27 RID: 23591 RVA: 0x0021BC7C File Offset: 0x00219E7C
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x06005C28 RID: 23592 RVA: 0x0021BC8A File Offset: 0x00219E8A
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x06005C29 RID: 23593 RVA: 0x0021BC98 File Offset: 0x00219E98
	public string GetFormattedValue(float value, GameUtil.TimeSlice time_slice)
	{
		return this.formatter.GetFormattedValue(value, time_slice);
	}

	// Token: 0x04003D9E RID: 15774
	protected StandardAttributeFormatter formatter;

	// Token: 0x04003D9F RID: 15775
	public GameUtil.IdentityDescriptorTense tense;
}

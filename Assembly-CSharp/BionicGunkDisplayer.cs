﻿using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000BCE RID: 3022
public class BionicGunkDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x06005C39 RID: 23609 RVA: 0x0021C186 File Offset: 0x0021A386
	public BionicGunkDisplayer(GameUtil.TimeSlice deltaTimeSlice) : base(deltaTimeSlice)
	{
	}

	// Token: 0x06005C3A RID: 23610 RVA: 0x0021C190 File Offset: 0x0021A390
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		BionicOilMonitor.Instance smi = instance.gameObject.GetSMI<BionicOilMonitor.Instance>();
		AmountInstance amountInstance = (smi == null) ? null : smi.oilAmount;
		string text = string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		text += "\n\n";
		float num = instance.deltaAttribute.GetTotalDisplayValue();
		if (smi != null)
		{
			float totalDisplayValue = amountInstance.deltaAttribute.GetTotalDisplayValue();
			if (totalDisplayValue < 0f)
			{
				num += Mathf.Abs(totalDisplayValue);
			}
		}
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(base.ToPercent(num, instance), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(base.ToPercent(num, instance), GameUtil.TimeSlice.PerSecond));
		}
		if (smi != null)
		{
			for (int num2 = 0; num2 != amountInstance.deltaAttribute.Modifiers.Count; num2++)
			{
				AttributeModifier attributeModifier = amountInstance.deltaAttribute.Modifiers[num2];
				float modifierContribution = amountInstance.deltaAttribute.GetModifierContribution(attributeModifier);
				if (modifierContribution < 0f)
				{
					float value = Mathf.Abs(modifierContribution);
					text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedValue(base.ToPercent(value, instance), this.formatter.DeltaTimeSlice));
				}
			}
		}
		for (int num3 = 0; num3 != instance.deltaAttribute.Modifiers.Count; num3++)
		{
			AttributeModifier attributeModifier2 = instance.deltaAttribute.Modifiers[num3];
			float modifierContribution2 = instance.deltaAttribute.GetModifierContribution(attributeModifier2);
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier2.GetDescription(), this.formatter.GetFormattedValue(base.ToPercent(modifierContribution2, instance), this.formatter.DeltaTimeSlice));
		}
		return text;
	}
}

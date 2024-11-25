using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000BCF RID: 3023
public class BionicOxygenTankDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x06005C3B RID: 23611 RVA: 0x0021C395 File Offset: 0x0021A595
	public BionicOxygenTankDisplayer(GameUtil.TimeSlice deltaTimeSlice) : base(deltaTimeSlice)
	{
	}

	// Token: 0x06005C3C RID: 23612 RVA: 0x0021C3A0 File Offset: 0x0021A5A0
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		BionicOxygenTankMonitor.Instance smi = instance.gameObject.GetSMI<BionicOxygenTankMonitor.Instance>();
		string text = string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		text += "\n\n";
		float num = instance.deltaAttribute.GetTotalDisplayValue();
		if (smi != null)
		{
			float totalValue = smi.airConsumptionRate.GetTotalValue();
			num += totalValue;
		}
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(base.ToPercent(num, instance), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(base.ToPercent(num, instance), GameUtil.TimeSlice.PerSecond));
		}
		Debug.Assert(instance.deltaAttribute.Modifiers.Count <= 0, "BionicOxygenTankDisplayer has found an invalid AttributeModifier. This particular Amount should not use AttributeModifiers, the rate of breathing is defined by  Db.Get().Attributes.AirConsumptionRate");
		return text;
	}
}

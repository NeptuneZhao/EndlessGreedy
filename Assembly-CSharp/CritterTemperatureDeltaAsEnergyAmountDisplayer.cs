using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000BD5 RID: 3029
public class CritterTemperatureDeltaAsEnergyAmountDisplayer : StandardAmountDisplayer
{
	// Token: 0x06005C48 RID: 23624 RVA: 0x0021C9A5 File Offset: 0x0021ABA5
	public CritterTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice timeSlice) : base(unitClass, timeSlice, null, GameUtil.IdentityDescriptorTense.Normal)
	{
	}

	// Token: 0x06005C49 RID: 23625 RVA: 0x0021C9B4 File Offset: 0x0021ABB4
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		CritterTemperatureMonitor.Def def = instance.gameObject.GetDef<CritterTemperatureMonitor.Def>();
		PrimaryElement component = instance.gameObject.GetComponent<PrimaryElement>();
		string text = string.Format(master.description, new object[]
		{
			this.formatter.GetFormattedValue(def.temperatureColdUncomfortable, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureHotUncomfortable, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureColdDeadly, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureHotDeadly, GameUtil.TimeSlice.None)
		});
		float num = ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * component.Mass * 1000f;
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += "\n\n";
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else if (instance.deltaAttribute.Modifiers.Count > 0)
		{
			text += "\n\n";
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
			text = text + "\n" + string.Format(UI.CHANGEPERSECOND, GameUtil.GetFormattedJoules(instance.deltaAttribute.GetTotalDisplayValue() * num, "F1", GameUtil.TimeSlice.None));
		}
		for (int num2 = 0; num2 != instance.deltaAttribute.Modifiers.Count; num2++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num2];
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), GameUtil.GetFormattedHeatEnergyRate(attributeModifier.Value * num * 1f, GameUtil.HeatEnergyFormatterUnit.Automatic));
		}
		return text;
	}
}

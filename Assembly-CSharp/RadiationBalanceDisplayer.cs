using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000BD1 RID: 3025
public class RadiationBalanceDisplayer : StandardAmountDisplayer
{
	// Token: 0x06005C3E RID: 23614 RVA: 0x0021C49E File Offset: 0x0021A69E
	public RadiationBalanceDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new RadiationBalanceDisplayer.RadiationAttributeFormatter();
	}

	// Token: 0x06005C3F RID: 23615 RVA: 0x0021C4B5 File Offset: 0x0021A6B5
	public override string GetValueString(Amount master, AmountInstance instance)
	{
		return base.GetValueString(master, instance) + UI.UNITSUFFIXES.RADIATION.RADS;
	}

	// Token: 0x06005C40 RID: 23616 RVA: 0x0021C4D0 File Offset: 0x0021A6D0
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = "";
		if (instance.gameObject.GetSMI<RadiationMonitor.Instance>() != null)
		{
			int num = Grid.PosToCell(instance.gameObject);
			if (Grid.IsValidCell(num))
			{
				text += DUPLICANTS.STATS.RADIATIONBALANCE.TOOLTIP_CURRENT_BALANCE;
			}
			text += "\n\n";
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(instance.gameObject).GetTotalValue());
			text += string.Format(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_EXPOSURE, Mathf.RoundToInt(Grid.Radiation[num] * num2));
			text += "\n";
			text += string.Format(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_REJUVENATION, Mathf.RoundToInt(Db.Get().Attributes.RadiationRecovery.Lookup(instance.gameObject).GetTotalValue() * 600f));
		}
		return text;
	}

	// Token: 0x02001CBF RID: 7359
	public class RadiationAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600A6DA RID: 42714 RVA: 0x00398E86 File Offset: 0x00397086
		public RadiationAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}

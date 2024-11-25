using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000CA6 RID: 3238
public class CrewRationsEntry : CrewListEntry
{
	// Token: 0x060063D9 RID: 25561 RVA: 0x00253BC6 File Offset: 0x00251DC6
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.rationMonitor = _identity.GetSMI<RationMonitor.Instance>();
		this.Refresh();
	}

	// Token: 0x060063DA RID: 25562 RVA: 0x00253BE4 File Offset: 0x00251DE4
	public override void Refresh()
	{
		base.Refresh();
		this.rationsEatenToday.text = GameUtil.GetFormattedCalories(this.rationMonitor.GetRationsAteToday(), GameUtil.TimeSlice.None, true);
		if (this.identity == null)
		{
			return;
		}
		foreach (AmountInstance amountInstance in this.identity.GetAmounts())
		{
			float min = amountInstance.GetMin();
			float max = amountInstance.GetMax();
			float num = max - min;
			string str = Mathf.RoundToInt((num - (max - amountInstance.value)) / num * 100f).ToString();
			if (amountInstance.amount == Db.Get().Amounts.Stress)
			{
				this.currentStressText.text = amountInstance.GetValueString();
				this.currentStressText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
				this.stressTrendImage.SetValue(amountInstance);
			}
			else if (amountInstance.amount == Db.Get().Amounts.Calories)
			{
				this.currentCaloriesText.text = str + "%";
				this.currentCaloriesText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
			else if (amountInstance.amount == Db.Get().Amounts.HitPoints)
			{
				this.currentHealthText.text = str + "%";
				this.currentHealthText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
		}
	}

	// Token: 0x040043CF RID: 17359
	public KButton incRationPerDayButton;

	// Token: 0x040043D0 RID: 17360
	public KButton decRationPerDayButton;

	// Token: 0x040043D1 RID: 17361
	public LocText rationPerDayText;

	// Token: 0x040043D2 RID: 17362
	public LocText rationsEatenToday;

	// Token: 0x040043D3 RID: 17363
	public LocText currentCaloriesText;

	// Token: 0x040043D4 RID: 17364
	public LocText currentStressText;

	// Token: 0x040043D5 RID: 17365
	public LocText currentHealthText;

	// Token: 0x040043D6 RID: 17366
	public ValueTrendImageToggle stressTrendImage;

	// Token: 0x040043D7 RID: 17367
	private RationMonitor.Instance rationMonitor;
}

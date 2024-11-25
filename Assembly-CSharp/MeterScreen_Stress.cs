using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000CDD RID: 3293
public class MeterScreen_Stress : MeterScreen_VTD_DuplicantIterator
{
	// Token: 0x060065CE RID: 26062 RVA: 0x0025F560 File Offset: 0x0025D760
	protected override string OnTooltip()
	{
		float maxStressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_AVGSTRESS, Mathf.Round(maxStressInActiveWorld).ToString() + "%"), this.ToolTipStyle_Header);
		Amount stress = Db.Get().Amounts.Stress;
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		List<MinionIdentity> stressedMinions = this.GetStressedMinions(worldMinionIdentities);
		bool flag = this.lastSelectedDuplicantIndex >= 0 && this.lastSelectedDuplicantIndex < worldMinionIdentities.Count;
		for (int i = 0; i < stressedMinions.Count; i++)
		{
			MinionIdentity minionIdentity = stressedMinions[i];
			AmountInstance amount = stress.Lookup(minionIdentity);
			base.AddToolTipAmountPercentLine(amount, minionIdentity, flag && worldMinionIdentities[this.lastSelectedDuplicantIndex] == minionIdentity);
		}
		return "";
	}

	// Token: 0x060065CF RID: 26063 RVA: 0x0025F644 File Offset: 0x0025D844
	protected override void InternalRefresh()
	{
		float maxStressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
		this.Label.text = Mathf.Round(maxStressInActiveWorld).ToString();
		WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<StressTracker>(ClusterManager.Instance.activeWorldId);
		this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor((worldTracker.GetCurrentValue() >= STRESS.ACTING_OUT_RESET) ? Constants.NEGATIVE_COLOR : Constants.NEUTRAL_COLOR);
		this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "stressData");
	}

	// Token: 0x060065D0 RID: 26064 RVA: 0x0025F6D0 File Offset: 0x0025D8D0
	private List<MinionIdentity> GetStressedMinions(List<MinionIdentity> minionIdentitiesList)
	{
		Amount stress_amount = Db.Get().Amounts.Stress;
		return (from x in new List<MinionIdentity>(this.GetWorldMinionIdentities())
		where !x.IsNullOrDestroyed()
		orderby stress_amount.Lookup(x).value descending
		select x).ToList<MinionIdentity>();
	}
}

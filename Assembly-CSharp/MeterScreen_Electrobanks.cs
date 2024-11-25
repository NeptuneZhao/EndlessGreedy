using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x02000CDA RID: 3290
public class MeterScreen_Electrobanks : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x060065C0 RID: 26048 RVA: 0x0025ED70 File Offset: 0x0025CF70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.LiveMinionIdentities.OnAdd += this.OnNewMinionAdded;
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		bool visibility;
		if (worldMinionIdentities != null)
		{
			visibility = (worldMinionIdentities.Find((MinionIdentity m) => m.model == BionicMinionConfig.MODEL) != null);
		}
		else
		{
			visibility = false;
		}
		this.SetVisibility(visibility);
	}

	// Token: 0x060065C1 RID: 26049 RVA: 0x0025EDD7 File Offset: 0x0025CFD7
	protected override void OnCleanUp()
	{
		Components.LiveMinionIdentities.OnAdd -= this.OnNewMinionAdded;
		base.OnCleanUp();
	}

	// Token: 0x060065C2 RID: 26050 RVA: 0x0025EDF5 File Offset: 0x0025CFF5
	private void OnNewMinionAdded(MinionIdentity id)
	{
		if (id.model == BionicMinionConfig.MODEL)
		{
			this.SetVisibility(true);
		}
	}

	// Token: 0x060065C3 RID: 26051 RVA: 0x0025EE10 File Offset: 0x0025D010
	public void SetVisibility(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x060065C4 RID: 26052 RVA: 0x0025EE20 File Offset: 0x0025D020
	protected override string OnTooltip()
	{
		this.joulesDictionary.Clear();
		string formattedJoules = GameUtil.GetFormattedJoules(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(this.joulesDictionary, ClusterManager.Instance.activeWorld.worldInventory, true), "F1", GameUtil.TimeSlice.None);
		this.Label.text = formattedJoules;
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_ELECTROBANK_JOULES, formattedJoules), this.ToolTipStyle_Header);
		this.Tooltip.AddMultiStringTooltip("", this.ToolTipStyle_Property);
		foreach (KeyValuePair<string, float> keyValuePair in (from x in this.joulesDictionary
		orderby x.Value descending
		select x).ToDictionary((KeyValuePair<string, float> t) => t.Key, (KeyValuePair<string, float> t) => t.Value))
		{
			GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
			this.Tooltip.AddMultiStringTooltip((prefab != null) ? string.Format("{0}: {1}", prefab.GetProperName(), GameUtil.GetFormattedJoules(keyValuePair.Value * 120000f, "F1", GameUtil.TimeSlice.None)) : string.Format(UI.TOOLTIPS.METERSCREEN_INVALID_ELECTROBANK_TYPE, keyValuePair.Key), this.ToolTipStyle_Property);
		}
		return "";
	}

	// Token: 0x060065C5 RID: 26053 RVA: 0x0025EFCC File Offset: 0x0025D1CC
	protected override void InternalRefresh()
	{
		if (!SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC3_ID"))
		{
			return;
		}
		if (this.Label != null && WorldResourceAmountTracker<ElectrobankTracker>.Get() != null)
		{
			long num = (long)WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(null, ClusterManager.Instance.activeWorld.worldInventory, true);
			if (this.cachedJoules != num)
			{
				this.Label.text = GameUtil.GetFormattedJoules((float)num, "F1", GameUtil.TimeSlice.None);
				this.cachedJoules = num;
			}
		}
		this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor(((float)this.cachedJoules > (float)this.GetWorldMinionIdentities().Count * 120000f) ? Constants.NEUTRAL_COLOR : Constants.NEGATIVE_COLOR);
		WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<ElectrobankJoulesTracker>(ClusterManager.Instance.activeWorldId);
		if (worldTracker != null)
		{
			this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "joules");
		}
	}

	// Token: 0x040044B4 RID: 17588
	private long cachedJoules = -1L;

	// Token: 0x040044B5 RID: 17589
	private Dictionary<string, float> joulesDictionary = new Dictionary<string, float>();
}

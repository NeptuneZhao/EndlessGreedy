using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200084E RID: 2126
public class RadiationDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B48 RID: 15176 RVA: 0x00145FA0 File Offset: 0x001441A0
	public RadiationDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<RadiationTracker>(worldID);
		this.trackerSampleCountSeconds = 150f;
		this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
		this.icon = "overlay_radiation";
		base.AddCriterion("CheckSick", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA.CHECKSICK, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckSick)));
		base.AddCriterion("CheckExposed", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA.CHECKEXPOSED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckExposure)));
	}

	// Token: 0x06003B49 RID: 15177 RVA: 0x00146038 File Offset: 0x00144238
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06003B4A RID: 15178 RVA: 0x0014603F File Offset: 0x0014423F
	public override string GetCurrentValueString()
	{
		return string.Format(UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.AVERAGE_RADS, GameUtil.GetFormattedRads(TrackerTool.Instance.GetWorldTracker<RadiationTracker>(base.worldID).GetCurrentValue(), GameUtil.TimeSlice.None));
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x0014606B File Offset: 0x0014426B
	public override string GetAverageValueString()
	{
		return string.Format(UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.AVERAGE_RADS, GameUtil.GetFormattedRads(TrackerTool.Instance.GetWorldTracker<RadiationTracker>(base.worldID).GetCurrentValue(), GameUtil.TimeSlice.None));
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x00146098 File Offset: 0x00144298
	private ColonyDiagnostic.DiagnosticResult CheckSick()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.NORMAL;
			foreach (MinionIdentity minionIdentity in worldItems)
			{
				RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
				if (smi != null && smi.sm.isSick.Get(smi))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
					result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_SICKNESS.FAIL;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(minionIdentity.gameObject.transform.position, minionIdentity.gameObject);
				}
			}
		}
		return result;
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x0014619C File Offset: 0x0014439C
	private ColonyDiagnostic.DiagnosticResult CheckExposure()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
			return result;
		}
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.PASS;
		foreach (MinionIdentity minionIdentity in worldItems)
		{
			RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
			if (smi != null)
			{
				RadiationMonitor sm = smi.sm;
				GameObject gameObject = minionIdentity.gameObject;
				Vector3 position = gameObject.transform.position;
				float p = sm.currentExposurePerCycle.Get(smi);
				float p2 = sm.radiationExposure.Get(smi);
				if (RadiationMonitor.COMPARE_LT_MINOR(smi, p) && RadiationMonitor.COMPARE_RECOVERY_IMMEDIATE(smi, p2))
				{
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(position, gameObject);
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
					result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.FAIL_CONCERN;
				}
				if (RadiationMonitor.COMPARE_GTE_DEADLY(smi, p))
				{
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(position, minionIdentity.gameObject);
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
					result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.FAIL_WARNING;
				}
			}
		}
		return result;
	}
}

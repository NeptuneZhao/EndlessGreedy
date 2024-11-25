using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x0200084B RID: 2123
public class IdleDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B3F RID: 15167 RVA: 0x00145A80 File Offset: 0x00143C80
	public IdleDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<IdleTracker>(worldID);
		this.icon = "icon_errand_operate";
		base.AddCriterion("CheckIdle", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.CRITERIA.CHECKIDLE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckIdle)));
	}

	// Token: 0x06003B40 RID: 15168 RVA: 0x00145AE0 File Offset: 0x00143CE0
	private ColonyDiagnostic.DiagnosticResult CheckIdle()
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
			result.Message = UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.NORMAL;
			if (this.tracker.GetMinValue(5f) > 0f && this.tracker.GetCurrentValue() > 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.IDLE;
				result.clickThroughObjects = this.tracker.objectsOfInterest;
			}
		}
		return result;
	}
}

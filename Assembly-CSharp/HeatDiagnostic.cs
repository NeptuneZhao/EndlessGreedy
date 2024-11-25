using System;
using STRINGS;

// Token: 0x0200084A RID: 2122
public class HeatDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B3D RID: 15165 RVA: 0x001459E4 File Offset: 0x00143BE4
	public HeatDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<BatteryTracker>(worldID);
		this.trackerSampleCountSeconds = 4f;
		base.AddCriterion("CheckHeat", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.CRITERIA.CHECKHEAT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHeat)));
	}

	// Token: 0x06003B3E RID: 15166 RVA: 0x00145A44 File Offset: 0x00143C44
	private ColonyDiagnostic.DiagnosticResult CheckHeat()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NORMAL
		};
	}
}

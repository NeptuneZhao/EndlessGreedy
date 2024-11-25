using System;
using STRINGS;

// Token: 0x0200083E RID: 2110
public class AllWorkTimeDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003AFA RID: 15098 RVA: 0x00143DB5 File Offset: 0x00141FB5
	public AllWorkTimeDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<AllWorkTimeTracker>(worldID);
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
	}

	// Token: 0x06003AFB RID: 15099 RVA: 0x00143DEC File Offset: 0x00141FEC
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}
}

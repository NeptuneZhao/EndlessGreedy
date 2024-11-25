using System;
using STRINGS;

// Token: 0x0200083D RID: 2109
public class AllChoresDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003AF8 RID: 15096 RVA: 0x00143D1F File Offset: 0x00141F1F
	public AllChoresDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<AllChoresCountTracker>(worldID);
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
		this.icon = "icon_errand_operate";
	}

	// Token: 0x06003AF9 RID: 15097 RVA: 0x00143D60 File Offset: 0x00141F60
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}
}

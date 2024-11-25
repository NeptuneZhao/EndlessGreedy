using System;
using STRINGS;

// Token: 0x02000856 RID: 2134
public class WorkTimeDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B68 RID: 15208 RVA: 0x001471AC File Offset: 0x001453AC
	public WorkTimeDiagnostic(int worldID, ChoreGroup choreGroup) : base(worldID, UI.COLONY_DIAGNOSTICS.WORKTIMEDIAGNOSTIC.ALL_NAME)
	{
		this.choreGroup = choreGroup;
		this.tracker = TrackerTool.Instance.GetWorkTimeTracker(worldID, choreGroup);
		this.trackerSampleCountSeconds = 100f;
		this.name = choreGroup.Name;
		this.id = "WorkTimeDiagnostic_" + choreGroup.Id;
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x00147224 File Offset: 0x00145424
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ((this.tracker.GetAverageValue(this.trackerSampleCountSeconds) > 0f) ? ColonyDiagnostic.DiagnosticResult.Opinion.Good : ColonyDiagnostic.DiagnosticResult.Opinion.Normal),
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)))
		};
	}

	// Token: 0x040023EE RID: 9198
	public ChoreGroup choreGroup;
}

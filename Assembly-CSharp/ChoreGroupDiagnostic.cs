using System;
using STRINGS;

// Token: 0x02000842 RID: 2114
public class ChoreGroupDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B0B RID: 15115 RVA: 0x00144918 File Offset: 0x00142B18
	public ChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup) : base(worldID, UI.COLONY_DIAGNOSTICS.CHOREGROUPDIAGNOSTIC.ALL_NAME)
	{
		this.choreGroup = choreGroup;
		this.tracker = TrackerTool.Instance.GetChoreGroupTracker(worldID, choreGroup);
		this.name = choreGroup.Name;
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
		this.id = "ChoreGroupDiagnostic_" + choreGroup.Id;
	}

	// Token: 0x06003B0C RID: 15116 RVA: 0x00144984 File Offset: 0x00142B84
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ((this.tracker.GetCurrentValue() > 0f) ? ColonyDiagnostic.DiagnosticResult.Opinion.Good : ColonyDiagnostic.DiagnosticResult.Opinion.Normal),
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}

	// Token: 0x040023D2 RID: 9170
	public ChoreGroup choreGroup;
}

using System;
using STRINGS;

// Token: 0x02000851 RID: 2129
public class RocketOxidizerDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B55 RID: 15189 RVA: 0x001465F9 File Offset: 0x001447F9
	public RocketOxidizerDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ROCKETOXIDIZERDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<RocketOxidizerTracker>(worldID);
		this.icon = "rocket_oxidizer";
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x00146628 File Offset: 0x00144828
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06003B57 RID: 15191 RVA: 0x00146630 File Offset: 0x00144830
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.worldID).gameObject.GetComponent<Clustercraft>();
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.ROCKETOXIDIZERDIAGNOSTIC.NORMAL;
		RocketEngineCluster engine = component.ModuleInterface.GetEngine();
		if (component.ModuleInterface.OxidizerPowerRemaining == 0f && engine != null && engine.requireOxidizer)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.ROCKETOXIDIZERDIAGNOSTIC.WARNING;
		}
		return result;
	}
}

using System;
using STRINGS;

// Token: 0x02000850 RID: 2128
public class RocketFuelDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B52 RID: 15186 RVA: 0x00146534 File Offset: 0x00144734
	public RocketFuelDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<RocketFuelTracker>(worldID);
		this.icon = "rocket_fuel";
	}

	// Token: 0x06003B53 RID: 15187 RVA: 0x00146563 File Offset: 0x00144763
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x0014656C File Offset: 0x0014476C
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.worldID).gameObject.GetComponent<Clustercraft>();
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.NORMAL;
		if (component.ModuleInterface.FuelRemaining == 0f)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.WARNING;
		}
		return result;
	}
}

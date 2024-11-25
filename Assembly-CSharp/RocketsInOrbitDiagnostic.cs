using System;
using STRINGS;

// Token: 0x02000852 RID: 2130
public class RocketsInOrbitDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B58 RID: 15192 RVA: 0x001466DC File Offset: 0x001448DC
	public RocketsInOrbitDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_errand_rocketry";
		base.AddCriterion("RocketsOrbiting", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.CRITERIA.CHECKORBIT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckOrbit)));
	}

	// Token: 0x06003B59 RID: 15193 RVA: 0x0014672B File Offset: 0x0014492B
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06003B5A RID: 15194 RVA: 0x00146734 File Offset: 0x00144934
	public ColonyDiagnostic.DiagnosticResult CheckOrbit()
	{
		AxialI myWorldLocation = ClusterManager.Instance.GetWorld(base.worldID).GetMyWorldLocation();
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		this.numRocketsInOrbit = 0;
		Clustercraft clustercraft = null;
		bool flag = false;
		foreach (Clustercraft clustercraft2 in Components.Clustercrafts.Items)
		{
			AxialI myWorldLocation2 = clustercraft2.GetMyWorldLocation();
			AxialI destination = clustercraft2.Destination;
			if (myWorldLocation2 != myWorldLocation && ClusterGrid.Instance.IsInRange(myWorldLocation2, myWorldLocation, 1) && ClusterGrid.Instance.IsInRange(myWorldLocation, destination, 1))
			{
				this.numRocketsInOrbit++;
				clustercraft = clustercraft2;
				flag = (flag || !clustercraft2.CanLandAtAsteroid(myWorldLocation, false));
			}
		}
		if (this.numRocketsInOrbit == 1 && clustercraft != null)
		{
			result.Message = string.Format(flag ? UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.WARNING_ONE_ROCKETS_STRANDED : UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.NORMAL_ONE_IN_ORBIT, clustercraft.Name);
		}
		else if (this.numRocketsInOrbit > 0)
		{
			result.Message = string.Format(flag ? UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.WARNING_ROCKETS_STRANDED : UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.NORMAL_IN_ORBIT, this.numRocketsInOrbit);
		}
		else
		{
			result.Message = UI.COLONY_DIAGNOSTICS.ROCKETINORBITDIAGNOSTIC.NORMAL_NO_ROCKETS;
		}
		if (flag)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
		}
		else if (this.numRocketsInOrbit > 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion;
		}
		return result;
	}

	// Token: 0x040023E9 RID: 9193
	private int numRocketsInOrbit;
}

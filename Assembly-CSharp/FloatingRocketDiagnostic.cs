using System;
using STRINGS;

// Token: 0x02000848 RID: 2120
public class FloatingRocketDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B34 RID: 15156 RVA: 0x00145492 File Offset: 0x00143692
	public FloatingRocketDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_errand_rocketry";
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x001454B0 File Offset: 0x001436B0
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x001454B8 File Offset: 0x001436B8
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(base.worldID);
		Clustercraft component = world.gameObject.GetComponent<Clustercraft>();
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		if (world.ParentWorldId == 255 || world.ParentWorldId == world.id)
		{
			result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_FLIGHT;
			if (component.Destination == component.Location)
			{
				bool flag = false;
				foreach (Ref<RocketModuleCluster> @ref in component.ModuleInterface.ClusterModules)
				{
					ResourceHarvestModule.StatesInstance smi = @ref.Get().GetSMI<ResourceHarvestModule.StatesInstance>();
					if (smi != null && smi.IsInsideState(smi.sm.not_grounded.harvesting))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
					result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_UTILITY;
				}
				else
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion;
					result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.WARNING_NO_DESTINATION;
				}
			}
			else if (component.Speed == 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.WARNING_NO_SPEED;
			}
		}
		else
		{
			result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_LANDED;
		}
		return result;
	}
}

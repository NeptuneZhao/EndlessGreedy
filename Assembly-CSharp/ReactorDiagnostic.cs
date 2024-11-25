using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200084F RID: 2127
public class ReactorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B4E RID: 15182 RVA: 0x00146318 File Offset: 0x00144518
	public ReactorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "overlay_radiation";
		base.AddCriterion("CheckTemperature", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA.CHECKTEMPERATURE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckTemperature)));
		base.AddCriterion("CheckCoolant", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA.CHECKCOOLANT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckCoolant)));
	}

	// Token: 0x06003B4F RID: 15183 RVA: 0x0014638D File Offset: 0x0014458D
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x00146394 File Offset: 0x00144594
	private ColonyDiagnostic.DiagnosticResult CheckTemperature()
	{
		List<Reactor> worldItems = Components.NuclearReactors.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.NORMAL;
		foreach (Reactor reactor in worldItems)
		{
			if (reactor.FuelTemperature > 1254.8625f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
				result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA_TEMPERATURE_WARNING;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(reactor.gameObject.transform.position, reactor.gameObject);
			}
		}
		return result;
	}

	// Token: 0x06003B51 RID: 15185 RVA: 0x00146460 File Offset: 0x00144660
	private ColonyDiagnostic.DiagnosticResult CheckCoolant()
	{
		List<Reactor> worldItems = Components.NuclearReactors.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.NORMAL;
		foreach (Reactor reactor in worldItems)
		{
			if (reactor.On && reactor.ReserveCoolantMass <= 45f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA_COOLANT_WARNING;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(reactor.gameObject.transform.position, reactor.gameObject);
			}
		}
		return result;
	}
}

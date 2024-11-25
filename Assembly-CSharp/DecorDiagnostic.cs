using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000844 RID: 2116
public class DecorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B1F RID: 15135 RVA: 0x00144DE4 File Offset: 0x00142FE4
	public DecorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_category_decor";
		base.AddCriterion("CheckDecor", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.CRITERIA.CHECKDECOR, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckDecor)));
	}

	// Token: 0x06003B20 RID: 15136 RVA: 0x00144E34 File Offset: 0x00143034
	private ColonyDiagnostic.DiagnosticResult CheckDecor()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		return result;
	}

	// Token: 0x06003B21 RID: 15137 RVA: 0x00144E84 File Offset: 0x00143084
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}
}

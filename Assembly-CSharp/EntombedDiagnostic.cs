using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000846 RID: 2118
public class EntombedDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B29 RID: 15145 RVA: 0x00144F08 File Offset: 0x00143108
	public EntombedDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_dig";
		base.AddCriterion("CheckEntombed", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.CRITERIA.CHECKENTOMBED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEntombed)));
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x00144F58 File Offset: 0x00143158
	private ColonyDiagnostic.DiagnosticResult CheckEntombed()
	{
		List<BuildingComplete> worldItems = Components.EntombedBuildings.GetWorldItems(base.worldID, false);
		this.m_entombedCount = 0;
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.NORMAL;
		foreach (BuildingComplete buildingComplete in worldItems)
		{
			if (!buildingComplete.IsNullOrDestroyed() && buildingComplete.prefabid.HasTag(GameTags.Entombed))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
				result.Message = UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.BUILDING_ENTOMBED;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(buildingComplete.gameObject.transform.position, buildingComplete.gameObject);
				this.m_entombedCount++;
			}
		}
		return result;
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x00145048 File Offset: 0x00143248
	public override string GetAverageValueString()
	{
		return this.m_entombedCount.ToString();
	}

	// Token: 0x040023E3 RID: 9187
	private int m_entombedCount;
}

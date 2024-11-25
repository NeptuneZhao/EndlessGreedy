using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x0200084C RID: 2124
public class MeteorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B41 RID: 15169 RVA: 0x00145BA0 File Offset: 0x00143DA0
	public MeteorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "meteors";
		base.AddCriterion("BombardmentUnderway", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.CRITERIA.CHECKUNDERWAY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckMeteorBombardment)));
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x00145BEF File Offset: 0x00143DEF
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06003B43 RID: 15171 RVA: 0x00145BF8 File Offset: 0x00143DF8
	public ColonyDiagnostic.DiagnosticResult CheckMeteorBombardment()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.NORMAL, null);
		List<GameplayEventInstance> list = new List<GameplayEventInstance>();
		GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(base.worldID, ref list);
		for (int i = 0; i < list.Count; i++)
		{
			MeteorShowerEvent.StatesInstance statesInstance = list[i].smi as MeteorShowerEvent.StatesInstance;
			if (statesInstance != null && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
				result.Message = string.Format(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.SHOWER_UNDERWAY, GameUtil.GetFormattedTime(statesInstance.BombardTimeRemaining(), "F0"));
			}
		}
		return result;
	}
}

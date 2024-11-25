using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000841 RID: 2113
public class BreathabilityDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B07 RID: 15111 RVA: 0x00144734 File Offset: 0x00142934
	public BreathabilityDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<BreathabilityTracker>(worldID);
		this.trackerSampleCountSeconds = 50f;
		this.icon = "overlay_oxygen";
		base.AddCriterion("CheckSuffocation", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKSUFFOCATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckSuffocation)));
		base.AddCriterion("CheckLowBreathability", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKLOWBREATHABILITY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLowBreathability)));
	}

	// Token: 0x06003B08 RID: 15112 RVA: 0x001447C8 File Offset: 0x001429C8
	private ColonyDiagnostic.DiagnosticResult CheckSuffocation()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		if (worldItems.Count != 0)
		{
			using (List<MinionIdentity>.Enumerator enumerator = worldItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity cmp = enumerator.Current;
					SuffocationMonitor.Instance smi = cmp.GetSMI<SuffocationMonitor.Instance>();
					if (smi != null && smi.IsInsideState(smi.sm.noOxygen.suffocating))
					{
						return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.SUFFOCATING, new global::Tuple<Vector3, GameObject>(smi.transform.position, smi.gameObject));
					}
				}
				goto IL_9B;
			}
			goto IL_8D;
			IL_9B:
			return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL, null);
		}
		IL_8D:
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
	}

	// Token: 0x06003B09 RID: 15113 RVA: 0x00144894 File Offset: 0x00142A94
	private ColonyDiagnostic.DiagnosticResult CheckLowBreathability()
	{
		if (Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count != 0 && this.tracker.GetAverageValue(this.trackerSampleCountSeconds) < 60f)
		{
			return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.POOR, null);
		}
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL, null);
	}

	// Token: 0x06003B0A RID: 15114 RVA: 0x001448F4 File Offset: 0x00142AF4
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}
}

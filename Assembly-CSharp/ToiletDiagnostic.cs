using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000854 RID: 2132
public class ToiletDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B5D RID: 15197 RVA: 0x00146A34 File Offset: 0x00144C34
	public ToiletDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_region_toilet";
		this.tracker = TrackerTool.Instance.GetWorldTracker<WorkingToiletTracker>(worldID);
		this.NO_MINIONS_WITH_BLADDER = (base.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_MINIONS_PLANETOID);
		base.AddCriterion("CheckHasAnyToilets", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKHASANYTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasAnyToilets)));
		base.AddCriterion("CheckEnoughToilets", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKENOUGHTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughToilets)));
		base.AddCriterion("CheckBladders", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKBLADDERS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckBladders)));
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x00146B00 File Offset: 0x00144D00
	private ColonyDiagnostic.DiagnosticResult CheckHasAnyToilets()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else if (this.toilets.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_TOILETS;
		}
		return result;
	}

	// Token: 0x06003B5F RID: 15199 RVA: 0x00146B6C File Offset: 0x00144D6C
	private ColonyDiagnostic.DiagnosticResult CheckEnoughToilets()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
			if (this.tracker.GetDataTimeLength() > 10f && this.tracker.GetAverageValue(this.trackerSampleCountSeconds) <= 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_WORKING_TOILETS;
			}
		}
		return result;
	}

	// Token: 0x06003B60 RID: 15200 RVA: 0x00146C10 File Offset: 0x00144E10
	private ColonyDiagnostic.DiagnosticResult CheckBladders()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
			WorldContainer world = ClusterManager.Instance.GetWorld(base.worldID);
			foreach (PeeChoreMonitor.Instance instance in Components.CriticalBladders.Items)
			{
				int myWorldId = instance.master.gameObject.GetMyWorldId();
				if (myWorldId == base.worldID || world.GetChildWorldIds().Contains(myWorldId))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
					result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.TOILET_URGENT;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003B61 RID: 15201 RVA: 0x00146D08 File Offset: 0x00144F08
	private bool MinionFilter(MinionIdentity minion)
	{
		return minion.modifiers.amounts.Has(Db.Get().Amounts.Bladder);
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x00146D2C File Offset: 0x00144F2C
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS_WITH_BLADDER, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		this.RefreshData();
		return base.Evaluate();
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x00146D65 File Offset: 0x00144F65
	private void RefreshData()
	{
		this.minionsWithBladders = Components.LiveMinionIdentities.GetWorldItems(base.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
		this.toilets = Components.Toilets.GetWorldItems(base.worldID, true);
	}

	// Token: 0x06003B64 RID: 15204 RVA: 0x00146DA4 File Offset: 0x00144FA4
	public override string GetAverageValueString()
	{
		if (this.minionsWithBladders == null || this.minionsWithBladders.Count == 0)
		{
			this.RefreshData();
		}
		int num = this.toilets.Count;
		for (int i = 0; i < this.toilets.Count; i++)
		{
			if (!this.toilets[i].IsNullOrDestroyed() && !this.toilets[i].IsUsable())
			{
				num--;
			}
		}
		return num.ToString() + ":" + this.minionsWithBladders.Count.ToString();
	}

	// Token: 0x040023EA RID: 9194
	private const bool INCLUDE_CHILD_WORLDS = true;

	// Token: 0x040023EB RID: 9195
	private List<MinionIdentity> minionsWithBladders;

	// Token: 0x040023EC RID: 9196
	private List<IUsable> toilets;

	// Token: 0x040023ED RID: 9197
	private readonly string NO_MINIONS_WITH_BLADDER;
}

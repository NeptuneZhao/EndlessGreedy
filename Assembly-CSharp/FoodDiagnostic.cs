using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000849 RID: 2121
public class FoodDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B37 RID: 15159 RVA: 0x00145634 File Offset: 0x00143834
	public FoodDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<KCalTracker>(worldID);
		this.icon = "icon_category_food";
		this.trackerSampleCountSeconds = 150f;
		this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
		base.AddCriterion("CheckEnoughFood", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKENOUGHFOOD, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughFood)));
		base.AddCriterion("CheckStarvation", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKSTARVATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckStarvation)));
		this.multiplier = MinionIdentity.GetCalorieBurnMultiplier();
		this.recommendedKCalPerDuplicant = 3000f * this.multiplier;
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x001456F4 File Offset: 0x001438F4
	private ColonyDiagnostic.DiagnosticResult CheckAnyFood()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.PASS, null);
		if (Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count != 0)
		{
			if (this.tracker.GetDataTimeLength() < 10f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
				result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
			}
			else if (this.tracker.GetAverageValue(this.trackerSampleCountSeconds) == 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
				result.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.FAIL;
			}
		}
		return result;
	}

	// Token: 0x06003B39 RID: 15161 RVA: 0x0014578C File Offset: 0x0014398C
	private ColonyDiagnostic.DiagnosticResult CheckEnoughFood()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		List<MinionIdentity> list = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).FindAll((MinionIdentity MID) => Db.Get().Amounts.Calories.Lookup(MID) != null);
		if (this.tracker.GetDataTimeLength() < 10f)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
		}
		else if ((float)list.Count * (1000f * this.recommendedKCalPerDuplicant) > this.tracker.GetAverageValue(this.trackerSampleCountSeconds))
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			float currentValue = this.tracker.GetCurrentValue();
			float f = (float)list.Count * DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE * this.multiplier;
			string formattedCalories = GameUtil.GetFormattedCalories(currentValue, GameUtil.TimeSlice.None, true);
			string formattedCalories2 = GameUtil.GetFormattedCalories(Mathf.Abs(f), GameUtil.TimeSlice.None, true);
			string text = MISC.NOTIFICATIONS.FOODLOW.TOOLTIP;
			text = text.Replace("{0}", formattedCalories);
			text = text.Replace("{1}", formattedCalories2);
			result.Message = text;
		}
		return result;
	}

	// Token: 0x06003B3A RID: 15162 RVA: 0x001458B8 File Offset: 0x00143AB8
	private ColonyDiagnostic.DiagnosticResult CheckStarvation()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.worldID, false))
		{
			if (!minionIdentity.IsNull())
			{
				CalorieMonitor.Instance smi = minionIdentity.GetSMI<CalorieMonitor.Instance>();
				if (!smi.IsNullOrStopped() && smi.IsInsideState(smi.sm.hungry.starving))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
					result.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.HUNGRY;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
				}
			}
		}
		return result;
	}

	// Token: 0x06003B3B RID: 15163 RVA: 0x00145990 File Offset: 0x00143B90
	public override string GetCurrentValueString()
	{
		return GameUtil.GetFormattedCalories(this.tracker.GetCurrentValue(), GameUtil.TimeSlice.None, true);
	}

	// Token: 0x06003B3C RID: 15164 RVA: 0x001459A4 File Offset: 0x00143BA4
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.NORMAL;
		}
		return diagnosticResult;
	}

	// Token: 0x040023E5 RID: 9189
	private const int CYCLES_OF_FOOD = 3;

	// Token: 0x040023E6 RID: 9190
	private const float BASE_KCAL_PER_CYCLE = 1000f;

	// Token: 0x040023E7 RID: 9191
	private float multiplier = 1f;

	// Token: 0x040023E8 RID: 9192
	private float recommendedKCalPerDuplicant;
}

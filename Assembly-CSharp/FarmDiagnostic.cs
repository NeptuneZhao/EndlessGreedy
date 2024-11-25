using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000847 RID: 2119
public class FarmDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003B2C RID: 15148 RVA: 0x00145058 File Offset: 0x00143258
	public FarmDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_errand_farm";
		base.AddCriterion("CheckHasFarms", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKHASFARMS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasFarms)));
		base.AddCriterion("CheckPlanted", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKPLANTED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckPlanted)));
		base.AddCriterion("CheckWilting", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKWILTING, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckWilting)));
		base.AddCriterion("CheckOperational", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKOPERATIONAL, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckOperational)));
	}

	// Token: 0x06003B2D RID: 15149 RVA: 0x00145119 File Offset: 0x00143319
	private void RefreshPlots()
	{
		this.plots = Components.PlantablePlots.GetItems(base.worldID).FindAll((PlantablePlot match) => match.HasDepositTag(GameTags.CropSeed));
	}

	// Token: 0x06003B2E RID: 15150 RVA: 0x00145158 File Offset: 0x00143358
	private ColonyDiagnostic.DiagnosticResult CheckHasFarms()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.plots.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE;
		}
		return result;
	}

	// Token: 0x06003B2F RID: 15151 RVA: 0x001451A0 File Offset: 0x001433A0
	private ColonyDiagnostic.DiagnosticResult CheckPlanted()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		bool flag = false;
		using (List<PlantablePlot>.Enumerator enumerator = this.plots.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.plant != null)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE_PLANTED;
		}
		return result;
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x00145230 File Offset: 0x00143430
	private ColonyDiagnostic.DiagnosticResult CheckWilting()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (PlantablePlot plantablePlot in this.plots)
		{
			if (plantablePlot.plant != null && plantablePlot.plant.HasTag(GameTags.Wilting))
			{
				StandardCropPlant component = plantablePlot.plant.GetComponent<StandardCropPlant>();
				if (component != null && component.smi.IsInsideState(component.smi.sm.alive.wilting) && component.smi.timeinstate > 15f)
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
					result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.WILTING;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(plantablePlot.transform.position, plantablePlot.gameObject);
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x00145340 File Offset: 0x00143540
	private ColonyDiagnostic.DiagnosticResult CheckOperational()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (PlantablePlot plantablePlot in this.plots)
		{
			if (plantablePlot.plant != null && !plantablePlot.HasTag(GameTags.Operational))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.INOPERATIONAL;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(plantablePlot.transform.position, plantablePlot.gameObject);
				break;
			}
		}
		return result;
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x001453F4 File Offset: 0x001435F4
	public override string GetAverageValueString()
	{
		if (this.plots == null)
		{
			this.RefreshPlots();
		}
		return TrackerTool.Instance.GetWorldTracker<CropTracker>(base.worldID).GetCurrentValue().ToString() + "/" + this.plots.Count.ToString();
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x0014544C File Offset: 0x0014364C
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		this.RefreshPlots();
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NORMAL;
		}
		return diagnosticResult;
	}

	// Token: 0x040023E4 RID: 9188
	private List<PlantablePlot> plots;
}

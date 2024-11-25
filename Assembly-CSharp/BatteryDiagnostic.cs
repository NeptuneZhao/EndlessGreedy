﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200083F RID: 2111
public class BatteryDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003AFC RID: 15100 RVA: 0x00143E44 File Offset: 0x00142044
	public BatteryDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<BatteryTracker>(worldID);
		this.trackerSampleCountSeconds = 4f;
		this.icon = "overlay_power";
		base.AddCriterion("CheckCapacity", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.CRITERIA.CHECKCAPACITY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckCapacity)));
		base.AddCriterion("CheckDead", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.CRITERIA.CHECKDEAD, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckDead)));
	}

	// Token: 0x06003AFD RID: 15101 RVA: 0x00143ED8 File Offset: 0x001420D8
	public ColonyDiagnostic.DiagnosticResult CheckCapacity()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		int num = 5;
		foreach (UtilityNetwork utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks())
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
			if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0)
			{
				float num2 = 0f;
				int num3 = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
				if ((int)Grid.WorldIdx[num3] == base.worldID)
				{
					ushort circuitID = Game.Instance.circuitManager.GetCircuitID(num3);
					List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID);
					if (batteriesOnCircuit != null && batteriesOnCircuit.Count != 0)
					{
						foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID))
						{
							result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
							result.Message = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NORMAL;
							num2 += battery.capacity;
						}
						if (num2 < Game.Instance.circuitManager.GetWattsUsedByCircuit(circuitID) * (float)num)
						{
							result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
							result.Message = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.LIMITED_CAPACITY;
							Battery battery2 = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID)[0];
							if (battery2 != null)
							{
								result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(battery2.transform.position, battery2.gameObject);
							}
						}
					}
					result.Message = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NONE;
				}
			}
		}
		return result;
	}

	// Token: 0x06003AFE RID: 15102 RVA: 0x001440D4 File Offset: 0x001422D4
	public ColonyDiagnostic.DiagnosticResult CheckDead()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (UtilityNetwork utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks())
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
			if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0)
			{
				int num = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
				if ((int)Grid.WorldIdx[num] == base.worldID)
				{
					ushort circuitID = Game.Instance.circuitManager.GetCircuitID(num);
					List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID);
					if (batteriesOnCircuit != null && batteriesOnCircuit.Count != 0)
					{
						foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID))
						{
							if (ColonyDiagnosticUtility.PastNewBuildingGracePeriod(battery.transform) && battery.CircuitID != 65535 && battery.JoulesAvailable == 0f)
							{
								result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
								result.Message = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.DEAD_BATTERY;
								result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(battery.transform.position, battery.gameObject);
								break;
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003AFF RID: 15103 RVA: 0x00144280 File Offset: 0x00142480
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

using System;
using UnityEngine;

// Token: 0x020005DA RID: 1498
public class PowerUseTracker : WorldTracker
{
	// Token: 0x06002472 RID: 9330 RVA: 0x000CB4C6 File Offset: 0x000C96C6
	public PowerUseTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x000CB4D0 File Offset: 0x000C96D0
	public override void UpdateData()
	{
		float num = 0f;
		foreach (UtilityNetwork utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks())
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
			if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0)
			{
				int num2 = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
				if ((int)Grid.WorldIdx[num2] == base.WorldID)
				{
					num += Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(num2));
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000CB590 File Offset: 0x000C9790
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Automatic, true);
	}
}

using System;
using UnityEngine;

// Token: 0x020005DB RID: 1499
public class BatteryTracker : WorldTracker
{
	// Token: 0x06002475 RID: 9333 RVA: 0x000CB59A File Offset: 0x000C979A
	public BatteryTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000CB5A4 File Offset: 0x000C97A4
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
					ushort circuitID = Game.Instance.circuitManager.GetCircuitID(num2);
					foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID))
					{
						num += battery.JoulesAvailable;
					}
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x000CB6B0 File Offset: 0x000C98B0
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}

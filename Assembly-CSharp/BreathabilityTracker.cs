using System;
using UnityEngine;

// Token: 0x020005D7 RID: 1495
public class BreathabilityTracker : WorldTracker
{
	// Token: 0x06002467 RID: 9319 RVA: 0x000CB33B File Offset: 0x000C953B
	public BreathabilityTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x000CB344 File Offset: 0x000C9544
	public override void UpdateData()
	{
		float num = 0f;
		if (Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false).Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		int num2 = 0;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false))
		{
			OxygenBreather component = minionIdentity.GetComponent<OxygenBreather>();
			if (!(component == null))
			{
				OxygenBreather.IGasProvider gasProvider = component.GetGasProvider();
				num2++;
				if (!component.IsSuffocating)
				{
					num += 100f;
					if (gasProvider.IsLowOxygen())
					{
						num -= 50f;
					}
				}
			}
		}
		num /= (float)num2;
		base.AddPoint((float)Mathf.RoundToInt(num));
	}

	// Token: 0x06002469 RID: 9321 RVA: 0x000CB414 File Offset: 0x000C9614
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}

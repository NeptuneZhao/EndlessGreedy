using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005DC RID: 1500
public class StressTracker : WorldTracker
{
	// Token: 0x06002478 RID: 9336 RVA: 0x000CB6BE File Offset: 0x000C98BE
	public StressTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x000CB6C8 File Offset: 0x000C98C8
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			if (Components.LiveMinionIdentities[i].GetMyWorldId() == base.WorldID)
			{
				num = Mathf.Max(num, Components.LiveMinionIdentities[i].gameObject.GetAmounts().GetValue(Db.Get().Amounts.Stress.Id));
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x000CB749 File Offset: 0x000C9949
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}

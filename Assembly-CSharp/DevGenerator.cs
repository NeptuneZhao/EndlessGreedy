using System;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
public class DevGenerator : Generator
{
	// Token: 0x06002B50 RID: 11088 RVA: 0x000F387C File Offset: 0x000F1A7C
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = this.wattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x040018D9 RID: 6361
	public float wattageRating = 100000f;
}

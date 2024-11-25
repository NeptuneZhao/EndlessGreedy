using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200055D RID: 1373
[AddComponentMenu("KMonoBehaviour/scripts/EffectArea")]
public class EffectArea : KMonoBehaviour
{
	// Token: 0x06001FCD RID: 8141 RVA: 0x000B2D79 File Offset: 0x000B0F79
	protected override void OnPrefabInit()
	{
		this.Effect = Db.Get().effects.Get(this.EffectName);
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000B2D98 File Offset: 0x000B0F98
	private void Update()
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			int num3 = 0;
			int num4 = 0;
			Grid.PosToXY(minionIdentity.transform.GetPosition(), out num3, out num4);
			if (Math.Abs(num3 - num) <= this.Area && Math.Abs(num4 - num2) <= this.Area)
			{
				minionIdentity.GetComponent<Effects>().Add(this.Effect, true);
			}
		}
	}

	// Token: 0x040011EC RID: 4588
	public string EffectName;

	// Token: 0x040011ED RID: 4589
	public int Area;

	// Token: 0x040011EE RID: 4590
	private Effect Effect;
}

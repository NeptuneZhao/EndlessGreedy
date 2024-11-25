using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
public class Attack
{
	// Token: 0x060036F9 RID: 14073 RVA: 0x0012B262 File Offset: 0x00129462
	public Attack(AttackProperties properties, GameObject[] targets)
	{
		this.properties = properties;
		this.targets = targets;
		this.RollHits();
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x0012B280 File Offset: 0x00129480
	private void RollHits()
	{
		int num = 0;
		while (num < this.targets.Length && num <= this.properties.maxHits - 1)
		{
			if (this.targets[num] != null)
			{
				new Hit(this.properties, this.targets[num]);
			}
			num++;
		}
	}

	// Token: 0x0400207E RID: 8318
	private AttackProperties properties;

	// Token: 0x0400207F RID: 8319
	private GameObject[] targets;

	// Token: 0x04002080 RID: 8320
	public List<Hit> Hits;
}

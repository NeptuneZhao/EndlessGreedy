using System;

// Token: 0x020007CB RID: 1995
[Serializable]
public class AttackEffect
{
	// Token: 0x060036FC RID: 14076 RVA: 0x0012B2E7 File Offset: 0x001294E7
	public AttackEffect(string ID, float probability)
	{
		this.effectID = ID;
		this.effectProbability = probability;
	}

	// Token: 0x04002089 RID: 8329
	public string effectID;

	// Token: 0x0400208A RID: 8330
	public float effectProbability;
}

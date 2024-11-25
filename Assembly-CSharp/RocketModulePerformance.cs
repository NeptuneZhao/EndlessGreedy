using System;

// Token: 0x02000AE3 RID: 2787
[Serializable]
public class RocketModulePerformance
{
	// Token: 0x060052DA RID: 21210 RVA: 0x001DB47B File Offset: 0x001D967B
	public RocketModulePerformance(float burden, float fuelKilogramPerDistance, float enginePower)
	{
		this.burden = burden;
		this.fuelKilogramPerDistance = fuelKilogramPerDistance;
		this.enginePower = enginePower;
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x060052DB RID: 21211 RVA: 0x001DB498 File Offset: 0x001D9698
	public float Burden
	{
		get
		{
			return this.burden;
		}
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x060052DC RID: 21212 RVA: 0x001DB4A0 File Offset: 0x001D96A0
	public float FuelKilogramPerDistance
	{
		get
		{
			return this.fuelKilogramPerDistance;
		}
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x060052DD RID: 21213 RVA: 0x001DB4A8 File Offset: 0x001D96A8
	public float EnginePower
	{
		get
		{
			return this.enginePower;
		}
	}

	// Token: 0x040036B7 RID: 14007
	public float burden;

	// Token: 0x040036B8 RID: 14008
	public float fuelKilogramPerDistance;

	// Token: 0x040036B9 RID: 14009
	public float enginePower;
}

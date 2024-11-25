using System;

// Token: 0x0200086B RID: 2155
[Serializable]
public struct EffectorValues
{
	// Token: 0x06003C1D RID: 15389 RVA: 0x0014DDBC File Offset: 0x0014BFBC
	public EffectorValues(int amt, int rad)
	{
		this.amount = amt;
		this.radius = rad;
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x0014DDCC File Offset: 0x0014BFCC
	public override bool Equals(object obj)
	{
		return obj is EffectorValues && this.Equals((EffectorValues)obj);
	}

	// Token: 0x06003C1F RID: 15391 RVA: 0x0014DDE4 File Offset: 0x0014BFE4
	public bool Equals(EffectorValues p)
	{
		return p != null && (this == p || (!(base.GetType() != p.GetType()) && this.amount == p.amount && this.radius == p.radius));
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x0014DE52 File Offset: 0x0014C052
	public override int GetHashCode()
	{
		return this.amount ^ this.radius;
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x0014DE61 File Offset: 0x0014C061
	public static bool operator ==(EffectorValues lhs, EffectorValues rhs)
	{
		if (lhs == null)
		{
			return rhs == null;
		}
		return lhs.Equals(rhs);
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x0014DE7F File Offset: 0x0014C07F
	public static bool operator !=(EffectorValues lhs, EffectorValues rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x04002471 RID: 9329
	public int amount;

	// Token: 0x04002472 RID: 9330
	public int radius;
}

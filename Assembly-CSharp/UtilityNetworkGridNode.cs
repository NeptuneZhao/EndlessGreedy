using System;

// Token: 0x02000B4E RID: 2894
public struct UtilityNetworkGridNode : IEquatable<UtilityNetworkGridNode>
{
	// Token: 0x0600565D RID: 22109 RVA: 0x001ED897 File Offset: 0x001EBA97
	public bool Equals(UtilityNetworkGridNode other)
	{
		return this.connections == other.connections && this.networkIdx == other.networkIdx;
	}

	// Token: 0x0600565E RID: 22110 RVA: 0x001ED8B8 File Offset: 0x001EBAB8
	public override bool Equals(object obj)
	{
		return ((UtilityNetworkGridNode)obj).Equals(this);
	}

	// Token: 0x0600565F RID: 22111 RVA: 0x001ED8D9 File Offset: 0x001EBAD9
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06005660 RID: 22112 RVA: 0x001ED8EB File Offset: 0x001EBAEB
	public static bool operator ==(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return x.Equals(y);
	}

	// Token: 0x06005661 RID: 22113 RVA: 0x001ED8F5 File Offset: 0x001EBAF5
	public static bool operator !=(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return !x.Equals(y);
	}

	// Token: 0x04003883 RID: 14467
	public UtilityConnections connections;

	// Token: 0x04003884 RID: 14468
	public int networkIdx;

	// Token: 0x04003885 RID: 14469
	public const int InvalidNetworkIdx = -1;
}

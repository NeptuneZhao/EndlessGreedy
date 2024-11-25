using System;

// Token: 0x02000B45 RID: 2885
public class UtilityNetwork
{
	// Token: 0x06005623 RID: 22051 RVA: 0x001ECE80 File Offset: 0x001EB080
	public virtual void AddItem(object item)
	{
	}

	// Token: 0x06005624 RID: 22052 RVA: 0x001ECE82 File Offset: 0x001EB082
	public virtual void RemoveItem(object item)
	{
	}

	// Token: 0x06005625 RID: 22053 RVA: 0x001ECE84 File Offset: 0x001EB084
	public virtual void ConnectItem(object item)
	{
	}

	// Token: 0x06005626 RID: 22054 RVA: 0x001ECE86 File Offset: 0x001EB086
	public virtual void DisconnectItem(object item)
	{
	}

	// Token: 0x06005627 RID: 22055 RVA: 0x001ECE88 File Offset: 0x001EB088
	public virtual void Reset(UtilityNetworkGridNode[] grid)
	{
	}

	// Token: 0x04003869 RID: 14441
	public int id;

	// Token: 0x0400386A RID: 14442
	public ConduitType conduitType;
}

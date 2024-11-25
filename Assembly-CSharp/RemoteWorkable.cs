using System;

// Token: 0x02000A1D RID: 2589
public abstract class RemoteWorkable : Workable, IRemoteDockWorkTarget
{
	// Token: 0x06004B1E RID: 19230 RVA: 0x001AD426 File Offset: 0x001AB626
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RemoteDockWorkTargets.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06004B1F RID: 19231 RVA: 0x001AD444 File Offset: 0x001AB644
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteDockWorkTargets.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06004B20 RID: 19232
	public abstract Chore RemoteDockChore { get; }

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06004B21 RID: 19233 RVA: 0x001AD462 File Offset: 0x001AB662
	public virtual IApproachable Approachable
	{
		get
		{
			return this;
		}
	}
}

using System;

// Token: 0x02000A1E RID: 2590
public abstract class RemoteDockWorkTargetComponent : KMonoBehaviour, IRemoteDockWorkTarget
{
	// Token: 0x06004B23 RID: 19235 RVA: 0x001AD46D File Offset: 0x001AB66D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RemoteDockWorkTargets.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06004B24 RID: 19236 RVA: 0x001AD48B File Offset: 0x001AB68B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteDockWorkTargets.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06004B25 RID: 19237
	public abstract Chore RemoteDockChore { get; }

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06004B26 RID: 19238 RVA: 0x001AD4A9 File Offset: 0x001AB6A9
	public virtual IApproachable Approachable
	{
		get
		{
			return base.gameObject.GetComponent<IApproachable>();
		}
	}
}

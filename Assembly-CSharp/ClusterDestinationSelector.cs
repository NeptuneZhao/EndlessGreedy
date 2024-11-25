using System;
using KSerialization;

// Token: 0x02000AB9 RID: 2745
public class ClusterDestinationSelector : KMonoBehaviour
{
	// Token: 0x060050EF RID: 20719 RVA: 0x001D11F5 File Offset: 0x001CF3F5
	protected override void OnPrefabInit()
	{
		base.Subscribe<ClusterDestinationSelector>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x060050F0 RID: 20720 RVA: 0x001D1209 File Offset: 0x001CF409
	protected virtual void OnClusterLocationChanged(object data)
	{
		if (((ClusterLocationChangedEvent)data).newLocation == this.m_destination)
		{
			base.Trigger(1796608350, data);
		}
	}

	// Token: 0x060050F1 RID: 20721 RVA: 0x001D122F File Offset: 0x001CF42F
	public int GetDestinationWorld()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination);
	}

	// Token: 0x060050F2 RID: 20722 RVA: 0x001D123C File Offset: 0x001CF43C
	public AxialI GetDestination()
	{
		return this.m_destination;
	}

	// Token: 0x060050F3 RID: 20723 RVA: 0x001D1244 File Offset: 0x001CF444
	public virtual void SetDestination(AxialI location)
	{
		if (this.requireAsteroidDestination)
		{
			Debug.Assert(ClusterUtil.GetAsteroidWorldIdAtLocation(location) != -1, string.Format("Cannot SetDestination to {0} as there is no world there", location));
		}
		this.m_destination = location;
		base.Trigger(543433792, location);
	}

	// Token: 0x060050F4 RID: 20724 RVA: 0x001D1292 File Offset: 0x001CF492
	public bool HasAsteroidDestination()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination) != -1;
	}

	// Token: 0x060050F5 RID: 20725 RVA: 0x001D12A5 File Offset: 0x001CF4A5
	public virtual bool IsAtDestination()
	{
		return this.GetMyWorldLocation() == this.m_destination;
	}

	// Token: 0x040035C8 RID: 13768
	[Serialize]
	protected AxialI m_destination;

	// Token: 0x040035C9 RID: 13769
	public bool assignable;

	// Token: 0x040035CA RID: 13770
	public bool requireAsteroidDestination;

	// Token: 0x040035CB RID: 13771
	[Serialize]
	public bool canNavigateFogOfWar;

	// Token: 0x040035CC RID: 13772
	public bool dodgesHiddenAsteroids;

	// Token: 0x040035CD RID: 13773
	public bool requireLaunchPadOnAsteroidDestination;

	// Token: 0x040035CE RID: 13774
	public bool shouldPointTowardsPath;

	// Token: 0x040035CF RID: 13775
	private EventSystem.IntraObjectHandler<ClusterDestinationSelector> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<ClusterDestinationSelector>(delegate(ClusterDestinationSelector cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}

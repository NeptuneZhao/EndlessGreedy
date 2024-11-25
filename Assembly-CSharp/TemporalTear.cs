using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000AEA RID: 2794
[SerializationConfig(MemberSerialization.OptIn)]
public class TemporalTear : ClusterGridEntity
{
	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06005338 RID: 21304 RVA: 0x001DDA14 File Offset: 0x001DBC14
	public override string Name
	{
		get
		{
			return Db.Get().SpaceDestinationTypes.Wormhole.typeName;
		}
	}

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06005339 RID: 21305 RVA: 0x001DDA2A File Offset: 0x001DBC2A
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x0600533A RID: 21306 RVA: 0x001DDA30 File Offset: 0x001DBC30
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("temporal_tear_kanim"),
					initialAnim = "closed_loop"
				}
			};
		}
	}

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x0600533B RID: 21307 RVA: 0x001DDA73 File Offset: 0x001DBC73
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x0600533C RID: 21308 RVA: 0x001DDA76 File Offset: 0x001DBC76
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x0600533D RID: 21309 RVA: 0x001DDA79 File Offset: 0x001DBC79
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ClusterManager.Instance.GetComponent<ClusterPOIManager>().RegisterTemporalTear(this);
		this.UpdateStatus();
	}

	// Token: 0x0600533E RID: 21310 RVA: 0x001DDA98 File Offset: 0x001DBC98
	public void UpdateStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		ClusterMapVisualizer clusterMapVisualizer = null;
		if (ClusterMapScreen.Instance != null)
		{
			clusterMapVisualizer = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		}
		if (this.IsOpen())
		{
			if (clusterMapVisualizer != null)
			{
				clusterMapVisualizer.PlayAnim("open_loop", KAnim.PlayMode.Loop);
			}
			component.RemoveStatusItem(Db.Get().MiscStatusItems.TearClosed, false);
			component.AddStatusItem(Db.Get().MiscStatusItems.TearOpen, null);
			return;
		}
		if (clusterMapVisualizer != null)
		{
			clusterMapVisualizer.PlayAnim("closed_loop", KAnim.PlayMode.Loop);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.TearOpen, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.TearClosed, null);
	}

	// Token: 0x0600533F RID: 21311 RVA: 0x001DDB5B File Offset: 0x001DBD5B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005340 RID: 21312 RVA: 0x001DDB64 File Offset: 0x001DBD64
	public void ConsumeCraft(Clustercraft craft)
	{
		if (this.m_open && craft.Location == base.Location && !craft.IsFlightInProgress())
		{
			for (int i = 0; i < Components.MinionIdentities.Count; i++)
			{
				MinionIdentity minionIdentity = Components.MinionIdentities[i];
				if (minionIdentity.GetMyWorldId() == craft.ModuleInterface.GetInteriorWorld().id)
				{
					Util.KDestroyGameObject(minionIdentity.gameObject);
				}
			}
			craft.DestroyCraftAndModules();
			this.m_hasConsumedCraft = true;
		}
	}

	// Token: 0x06005341 RID: 21313 RVA: 0x001DDBE5 File Offset: 0x001DBDE5
	public void Open()
	{
		this.m_open = true;
		this.UpdateStatus();
	}

	// Token: 0x06005342 RID: 21314 RVA: 0x001DDBF4 File Offset: 0x001DBDF4
	public bool IsOpen()
	{
		return this.m_open;
	}

	// Token: 0x06005343 RID: 21315 RVA: 0x001DDBFC File Offset: 0x001DBDFC
	public bool HasConsumedCraft()
	{
		return this.m_hasConsumedCraft;
	}

	// Token: 0x040036E0 RID: 14048
	[Serialize]
	private bool m_open;

	// Token: 0x040036E1 RID: 14049
	[Serialize]
	private bool m_hasConsumedCraft;
}

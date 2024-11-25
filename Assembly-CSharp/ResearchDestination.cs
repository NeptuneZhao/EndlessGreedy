using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x02000AD8 RID: 2776
[SerializationConfig(MemberSerialization.OptIn)]
public class ResearchDestination : ClusterGridEntity
{
	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06005285 RID: 21125 RVA: 0x001D95E5 File Offset: 0x001D77E5
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.RESEARCHDESTINATION.NAME;
		}
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06005286 RID: 21126 RVA: 0x001D95F1 File Offset: 0x001D77F1
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06005287 RID: 21127 RVA: 0x001D95F4 File Offset: 0x001D77F4
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>();
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06005288 RID: 21128 RVA: 0x001D95FB File Offset: 0x001D77FB
	public override bool IsVisible
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06005289 RID: 21129 RVA: 0x001D95FE File Offset: 0x001D77FE
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x0600528A RID: 21130 RVA: 0x001D9601 File Offset: 0x001D7801
	public void Init(AxialI location)
	{
		this.m_location = location;
	}
}

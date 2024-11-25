using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000AB1 RID: 2737
[SerializationConfig(MemberSerialization.OptIn)]
public class ArtifactPOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x060050AA RID: 20650 RVA: 0x001CFC81 File Offset: 0x001CDE81
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x060050AB RID: 20651 RVA: 0x001CFC89 File Offset: 0x001CDE89
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x060050AC RID: 20652 RVA: 0x001CFC8C File Offset: 0x001CDE8C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("gravitas_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "station_1" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x060050AD RID: 20653 RVA: 0x001CFCE4 File Offset: 0x001CDEE4
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x060050AE RID: 20654 RVA: 0x001CFCE7 File Offset: 0x001CDEE7
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060050AF RID: 20655 RVA: 0x001CFCEA File Offset: 0x001CDEEA
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x040035A4 RID: 13732
	public string m_name;

	// Token: 0x040035A5 RID: 13733
	public string m_Anim;
}

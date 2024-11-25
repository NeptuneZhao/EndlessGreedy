using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000AC6 RID: 2758
[SerializationConfig(MemberSerialization.OptIn)]
public class HarvestablePOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x060051F6 RID: 20982 RVA: 0x001D692F File Offset: 0x001D4B2F
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x060051F7 RID: 20983 RVA: 0x001D6937 File Offset: 0x001D4B37
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x060051F8 RID: 20984 RVA: 0x001D693C File Offset: 0x001D4B3C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("harvestable_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "cloud" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x060051F9 RID: 20985 RVA: 0x001D6994 File Offset: 0x001D4B94
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x060051FA RID: 20986 RVA: 0x001D6997 File Offset: 0x001D4B97
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060051FB RID: 20987 RVA: 0x001D699A File Offset: 0x001D4B9A
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x0400361D RID: 13853
	public string m_name;

	// Token: 0x0400361E RID: 13854
	public string m_Anim;
}

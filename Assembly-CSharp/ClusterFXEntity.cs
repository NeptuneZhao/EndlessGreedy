using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000ABA RID: 2746
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterFXEntity : ClusterGridEntity
{
	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x060050F7 RID: 20727 RVA: 0x001D12EA File Offset: 0x001CF4EA
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x060050F8 RID: 20728 RVA: 0x001D12F6 File Offset: 0x001CF4F6
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.FX;
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x060050F9 RID: 20729 RVA: 0x001D12FC File Offset: 0x001CF4FC
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.kAnimName),
					initialAnim = this.animName,
					playMode = this.animPlayMode,
					animOffset = this.animOffset
				}
			};
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x060050FA RID: 20730 RVA: 0x001D135B File Offset: 0x001CF55B
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x060050FB RID: 20731 RVA: 0x001D135E File Offset: 0x001CF55E
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x060050FC RID: 20732 RVA: 0x001D1361 File Offset: 0x001CF561
	public void Init(AxialI location, Vector3 animOffset)
	{
		base.Location = location;
		this.animOffset = animOffset;
	}

	// Token: 0x040035D0 RID: 13776
	[SerializeField]
	public string kAnimName;

	// Token: 0x040035D1 RID: 13777
	[SerializeField]
	public string animName;

	// Token: 0x040035D2 RID: 13778
	public KAnim.PlayMode animPlayMode = KAnim.PlayMode.Once;

	// Token: 0x040035D3 RID: 13779
	public Vector3 animOffset;
}

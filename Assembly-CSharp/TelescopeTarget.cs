using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x02000AE9 RID: 2793
[SerializationConfig(MemberSerialization.OptIn)]
public class TelescopeTarget : ClusterGridEntity
{
	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x0600532D RID: 21293 RVA: 0x001DD96E File Offset: 0x001DBB6E
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x0600532E RID: 21294 RVA: 0x001DD97A File Offset: 0x001DBB7A
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Telescope;
		}
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x0600532F RID: 21295 RVA: 0x001DD980 File Offset: 0x001DBB80
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("telescope_target_kanim"),
					initialAnim = "idle"
				}
			};
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06005330 RID: 21296 RVA: 0x001DD9C3 File Offset: 0x001DBBC3
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06005331 RID: 21297 RVA: 0x001DD9C6 File Offset: 0x001DBBC6
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06005332 RID: 21298 RVA: 0x001DD9C9 File Offset: 0x001DBBC9
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x06005333 RID: 21299 RVA: 0x001DD9D2 File Offset: 0x001DBBD2
	public void SetTargetMeteorShower(ClusterMapMeteorShower.Instance meteorShower)
	{
		this.targetMeteorShower = meteorShower;
	}

	// Token: 0x06005334 RID: 21300 RVA: 0x001DD9DB File Offset: 0x001DBBDB
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x06005335 RID: 21301 RVA: 0x001DD9DE File Offset: 0x001DBBDE
	public override bool ShowProgressBar()
	{
		return true;
	}

	// Token: 0x06005336 RID: 21302 RVA: 0x001DD9E1 File Offset: 0x001DBBE1
	public override float GetProgress()
	{
		if (this.targetMeteorShower != null)
		{
			return this.targetMeteorShower.IdentifyingProgress;
		}
		return SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().GetRevealCompleteFraction(base.Location);
	}

	// Token: 0x040036DF RID: 14047
	private ClusterMapMeteorShower.Instance targetMeteorShower;
}

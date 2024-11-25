using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A9 RID: 681
public class BallisticClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000E11 RID: 3601 RVA: 0x000515E7 File Offset: 0x0004F7E7
	public override string Name
	{
		get
		{
			return Strings.Get(this.nameKey);
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x000515F9 File Offset: 0x0004F7F9
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Payload;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000E13 RID: 3603 RVA: 0x000515FC File Offset: 0x0004F7FC
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = "idle_loop",
					symbolSwapTarget = this.clusterAnimSymbolSwapTarget,
					symbolSwapSymbol = this.clusterAnimSymbolSwapSymbol
				}
			};
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0005165A File Offset: 0x0004F85A
	public override bool IsVisible
	{
		get
		{
			return !base.gameObject.HasTag(GameTags.ClusterEntityGrounded);
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000E15 RID: 3605 RVA: 0x0005166F File Offset: 0x0004F86F
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x00051672 File Offset: 0x0004F872
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x00051678 File Offset: 0x0004F878
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
		this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
		this.m_clusterTraveler.onTravelCB = null;
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x000516C5 File Offset: 0x0004F8C5
	private float GetSpeed()
	{
		return 10f;
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x000516CC File Offset: 0x0004F8CC
	private bool CanTravel(bool tryingToLand)
	{
		return this.HasTag(GameTags.EntityInSpace);
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x000516D9 File Offset: 0x0004F8D9
	public void Configure(AxialI source, AxialI destination)
	{
		this.m_location = source;
		this.m_destionationSelector.SetDestination(destination);
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x000516EE File Offset: 0x0004F8EE
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x000516FB File Offset: 0x0004F8FB
	public override bool ShowProgressBar()
	{
		return this.m_selectable.IsSelected && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x00051717 File Offset: 0x0004F917
	public override float GetProgress()
	{
		return this.m_clusterTraveler.GetMoveProgress();
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x00051724 File Offset: 0x0004F924
	public void SwapSymbolFromSameAnim(string targetSymbolName, string swappedSymbolName)
	{
		this.clusterAnimSymbolSwapTarget = targetSymbolName;
		this.clusterAnimSymbolSwapSymbol = swappedSymbolName;
	}

	// Token: 0x040008D8 RID: 2264
	[MyCmpReq]
	private ClusterDestinationSelector m_destionationSelector;

	// Token: 0x040008D9 RID: 2265
	[MyCmpReq]
	private ClusterTraveler m_clusterTraveler;

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	public string clusterAnimName;

	// Token: 0x040008DB RID: 2267
	[SerializeField]
	public StringKey nameKey;

	// Token: 0x040008DC RID: 2268
	private string clusterAnimSymbolSwapTarget;

	// Token: 0x040008DD RID: 2269
	private string clusterAnimSymbolSwapSymbol;
}

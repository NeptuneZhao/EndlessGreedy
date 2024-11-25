using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000ABE RID: 2750
public class ClusterTraveler : KMonoBehaviour, ISim200ms
{
	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x0600511C RID: 20764 RVA: 0x001D1D50 File Offset: 0x001CFF50
	public List<AxialI> CurrentPath
	{
		get
		{
			if (this.m_cachedPath == null || this.m_destinationSelector.GetDestination() != this.m_cachedPathDestination)
			{
				this.m_cachedPathDestination = this.m_destinationSelector.GetDestination();
				this.m_cachedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector);
			}
			return this.m_cachedPath;
		}
	}

	// Token: 0x0600511D RID: 20765 RVA: 0x001D1DBB File Offset: 0x001CFFBB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterTravelers.Add(this);
	}

	// Token: 0x0600511E RID: 20766 RVA: 0x001D1DCE File Offset: 0x001CFFCE
	protected override void OnCleanUp()
	{
		Components.ClusterTravelers.Remove(this);
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		base.OnCleanUp();
	}

	// Token: 0x0600511F RID: 20767 RVA: 0x001D1DFC File Offset: 0x001CFFFC
	private void ForceRevealLocation(AxialI location)
	{
		if (!ClusterGrid.Instance.IsCellVisible(location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 0);
		}
	}

	// Token: 0x06005120 RID: 20768 RVA: 0x001D1E1C File Offset: 0x001D001C
	protected override void OnSpawn()
	{
		base.Subscribe<ClusterTraveler>(543433792, ClusterTraveler.ClusterDestinationChangedHandler);
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		this.UpdateAnimationTags();
		this.MarkPathDirty();
		this.RevalidatePath(false);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(this.m_clusterGridEntity.Location);
		}
	}

	// Token: 0x06005121 RID: 20769 RVA: 0x001D1E82 File Offset: 0x001D0082
	private void MarkPathDirty()
	{
		this.m_isPathDirty = true;
	}

	// Token: 0x06005122 RID: 20770 RVA: 0x001D1E8B File Offset: 0x001D008B
	private void OnClusterFogOfWarRevealed(object data)
	{
		this.MarkPathDirty();
	}

	// Token: 0x06005123 RID: 20771 RVA: 0x001D1E93 File Offset: 0x001D0093
	private void OnClusterDestinationChanged(object data)
	{
		if (this.m_destinationSelector.IsAtDestination())
		{
			this.m_movePotential = 0f;
			if (this.CurrentPath != null)
			{
				this.CurrentPath.Clear();
			}
		}
		this.MarkPathDirty();
	}

	// Token: 0x06005124 RID: 20772 RVA: 0x001D1EC6 File Offset: 0x001D00C6
	public int GetDestinationWorldID()
	{
		return this.m_destinationSelector.GetDestinationWorld();
	}

	// Token: 0x06005125 RID: 20773 RVA: 0x001D1ED3 File Offset: 0x001D00D3
	public float TravelETA()
	{
		if (!this.IsTraveling() || this.getSpeedCB == null)
		{
			return 0f;
		}
		return this.RemainingTravelDistance() / this.getSpeedCB();
	}

	// Token: 0x06005126 RID: 20774 RVA: 0x001D1F00 File Offset: 0x001D0100
	public float RemainingTravelDistance()
	{
		int num = this.RemainingTravelNodes();
		if (this.GetDestinationWorldID() >= 0)
		{
			num--;
			num = Mathf.Max(num, 0);
		}
		return (float)num * 600f - this.m_movePotential;
	}

	// Token: 0x06005127 RID: 20775 RVA: 0x001D1F38 File Offset: 0x001D0138
	public int RemainingTravelNodes()
	{
		if (this.CurrentPath == null)
		{
			return 0;
		}
		int count = this.CurrentPath.Count;
		return Mathf.Max(0, count);
	}

	// Token: 0x06005128 RID: 20776 RVA: 0x001D1F62 File Offset: 0x001D0162
	public float GetMoveProgress()
	{
		return this.m_movePotential / 600f;
	}

	// Token: 0x06005129 RID: 20777 RVA: 0x001D1F70 File Offset: 0x001D0170
	public bool IsTraveling()
	{
		return !this.m_destinationSelector.IsAtDestination();
	}

	// Token: 0x0600512A RID: 20778 RVA: 0x001D1F80 File Offset: 0x001D0180
	public void Sim200ms(float dt)
	{
		if (!this.IsTraveling())
		{
			return;
		}
		bool flag = this.CurrentPath != null && this.CurrentPath.Count > 0;
		bool flag2 = this.m_destinationSelector.HasAsteroidDestination();
		bool arg = flag2 && flag && this.CurrentPath.Count == 1;
		if (this.getCanTravelCB != null && !this.getCanTravelCB(arg))
		{
			return;
		}
		AxialI location = this.m_clusterGridEntity.Location;
		if (flag)
		{
			if (flag2)
			{
				bool requireLaunchPadOnAsteroidDestination = this.m_destinationSelector.requireLaunchPadOnAsteroidDestination;
			}
			if (!flag2 || this.CurrentPath.Count > 1 || !this.quickTravelToAsteroidIfInOrbit)
			{
				float num = dt * this.getSpeedCB();
				this.m_movePotential += num;
				if (this.m_movePotential >= 600f)
				{
					this.m_movePotential = 600f;
					if (this.AdvancePathOneStep())
					{
						global::Debug.Assert(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_clusterGridEntity.Location, EntityLayer.Asteroid) == null || (flag2 && this.CurrentPath.Count == 0), string.Format("Somehow this clustercraft pathed through an asteroid at {0}", this.m_clusterGridEntity.Location));
						this.m_movePotential -= 600f;
						if (this.onTravelCB != null)
						{
							this.onTravelCB();
						}
					}
				}
			}
			else
			{
				this.AdvancePathOneStep();
			}
		}
		this.RevalidatePath(true);
	}

	// Token: 0x0600512B RID: 20779 RVA: 0x001D20F8 File Offset: 0x001D02F8
	public bool AdvancePathOneStep()
	{
		if (this.validateTravelCB != null && !this.validateTravelCB(this.CurrentPath[0]))
		{
			return false;
		}
		AxialI location = this.CurrentPath[0];
		this.CurrentPath.RemoveAt(0);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(location);
		}
		this.m_clusterGridEntity.Location = location;
		this.UpdateAnimationTags();
		return true;
	}

	// Token: 0x0600512C RID: 20780 RVA: 0x001D2164 File Offset: 0x001D0364
	private void UpdateAnimationTags()
	{
		if (this.CurrentPath == null)
		{
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		if (!(ClusterGrid.Instance.GetAsteroidAtCell(this.m_clusterGridEntity.Location) != null))
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityMoving);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			return;
		}
		if (this.CurrentPath.Count == 0 || this.m_clusterGridEntity.Location == this.CurrentPath[this.CurrentPath.Count - 1])
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLaunching);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
	}

	// Token: 0x0600512D RID: 20781 RVA: 0x001D2294 File Offset: 0x001D0494
	public void RevalidatePath(bool react_to_change = true)
	{
		string reason;
		List<AxialI> cachedPath;
		if (this.HasCurrentPathChanged(out reason, out cachedPath))
		{
			if (this.stopAndNotifyWhenPathChanges && react_to_change)
			{
				this.m_destinationSelector.SetDestination(this.m_destinationSelector.GetMyWorldLocation());
				string message = MISC.NOTIFICATIONS.BADROCKETPATH.TOOLTIP;
				Notification notification = new Notification(MISC.NOTIFICATIONS.BADROCKETPATH.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => message + notificationList.ReduceMessages(false) + "\n\n" + reason, null, true, 0f, null, null, null, true, false, false);
				base.GetComponent<Notifier>().Add(notification, "");
				return;
			}
			this.m_cachedPath = cachedPath;
		}
	}

	// Token: 0x0600512E RID: 20782 RVA: 0x001D232C File Offset: 0x001D052C
	private bool HasCurrentPathChanged(out string reason, out List<AxialI> updatedPath)
	{
		if (!this.m_isPathDirty)
		{
			reason = null;
			updatedPath = null;
			return false;
		}
		this.m_isPathDirty = false;
		updatedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector, out reason, this.m_destinationSelector.dodgesHiddenAsteroids);
		if (updatedPath == null)
		{
			return true;
		}
		if (updatedPath.Count != this.m_cachedPath.Count)
		{
			return true;
		}
		for (int i = 0; i < this.m_cachedPath.Count; i++)
		{
			if (this.m_cachedPath[i] != updatedPath[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600512F RID: 20783 RVA: 0x001D23CF File Offset: 0x001D05CF
	[ContextMenu("Fill Move Potential")]
	public void FillMovePotential()
	{
		this.m_movePotential = 600f;
	}

	// Token: 0x040035DD RID: 13789
	[MyCmpReq]
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x040035DE RID: 13790
	[MyCmpReq]
	private ClusterGridEntity m_clusterGridEntity;

	// Token: 0x040035DF RID: 13791
	[Serialize]
	private float m_movePotential;

	// Token: 0x040035E0 RID: 13792
	public Func<float> getSpeedCB;

	// Token: 0x040035E1 RID: 13793
	public Func<bool, bool> getCanTravelCB;

	// Token: 0x040035E2 RID: 13794
	public Func<AxialI, bool> validateTravelCB;

	// Token: 0x040035E3 RID: 13795
	public System.Action onTravelCB;

	// Token: 0x040035E4 RID: 13796
	private AxialI m_cachedPathDestination;

	// Token: 0x040035E5 RID: 13797
	private List<AxialI> m_cachedPath;

	// Token: 0x040035E6 RID: 13798
	private bool m_isPathDirty;

	// Token: 0x040035E7 RID: 13799
	public bool revealsFogOfWarAsItTravels = true;

	// Token: 0x040035E8 RID: 13800
	public bool quickTravelToAsteroidIfInOrbit = true;

	// Token: 0x040035E9 RID: 13801
	public bool stopAndNotifyWhenPathChanges;

	// Token: 0x040035EA RID: 13802
	private static EventSystem.IntraObjectHandler<ClusterTraveler> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<ClusterTraveler>(delegate(ClusterTraveler cmp, object data)
	{
		cmp.OnClusterDestinationChanged(data);
	});
}

using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000AEF RID: 2799
public abstract class ClusterGridEntity : KMonoBehaviour
{
	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x06005386 RID: 21382
	public abstract string Name { get; }

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x06005387 RID: 21383
	public abstract EntityLayer Layer { get; }

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x06005388 RID: 21384
	public abstract List<ClusterGridEntity.AnimConfig> AnimConfigs { get; }

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06005389 RID: 21385
	public abstract bool IsVisible { get; }

	// Token: 0x0600538A RID: 21386 RVA: 0x001DF2E8 File Offset: 0x001DD4E8
	public virtual bool ShowName()
	{
		return false;
	}

	// Token: 0x0600538B RID: 21387 RVA: 0x001DF2EB File Offset: 0x001DD4EB
	public virtual bool ShowProgressBar()
	{
		return false;
	}

	// Token: 0x0600538C RID: 21388 RVA: 0x001DF2EE File Offset: 0x001DD4EE
	public virtual float GetProgress()
	{
		return 0f;
	}

	// Token: 0x0600538D RID: 21389 RVA: 0x001DF2F5 File Offset: 0x001DD4F5
	public virtual bool SpaceOutInSameHex()
	{
		return false;
	}

	// Token: 0x0600538E RID: 21390 RVA: 0x001DF2F8 File Offset: 0x001DD4F8
	public virtual bool KeepRotationWhenSpacingOutInHex()
	{
		return false;
	}

	// Token: 0x0600538F RID: 21391 RVA: 0x001DF2FB File Offset: 0x001DD4FB
	public virtual bool ShowPath()
	{
		return true;
	}

	// Token: 0x06005390 RID: 21392 RVA: 0x001DF2FE File Offset: 0x001DD4FE
	public virtual void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
	}

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06005391 RID: 21393
	public abstract ClusterRevealLevel IsVisibleInFOW { get; }

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06005392 RID: 21394 RVA: 0x001DF300 File Offset: 0x001DD500
	// (set) Token: 0x06005393 RID: 21395 RVA: 0x001DF308 File Offset: 0x001DD508
	public AxialI Location
	{
		get
		{
			return this.m_location;
		}
		set
		{
			if (value != this.m_location)
			{
				AxialI location = this.m_location;
				this.m_location = value;
				if (base.gameObject.GetSMI<StateMachine.Instance>() == null)
				{
					this.positionDirty = true;
				}
				this.SendClusterLocationChangedEvent(location, this.m_location);
			}
		}
	}

	// Token: 0x06005394 RID: 21396 RVA: 0x001DF354 File Offset: 0x001DD554
	protected override void OnSpawn()
	{
		ClusterGrid.Instance.RegisterEntity(this);
		if (this.m_selectable != null)
		{
			this.m_selectable.SetName(this.Name);
		}
		if (!this.isWorldEntity)
		{
			this.m_transform.SetLocalPosition(new Vector3(-1f, 0f, 0f));
		}
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.Trigger(1980521255, null);
		}
	}

	// Token: 0x06005395 RID: 21397 RVA: 0x001DF3D0 File Offset: 0x001DD5D0
	protected override void OnCleanUp()
	{
		ClusterGrid.Instance.UnregisterEntity(this);
	}

	// Token: 0x06005396 RID: 21398 RVA: 0x001DF3E0 File Offset: 0x001DD5E0
	public virtual Sprite GetUISprite()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			List<ClusterGridEntity.AnimConfig> animConfigs = this.AnimConfigs;
			if (animConfigs.Count > 0)
			{
				return Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile, "ui", false, "");
			}
		}
		else
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null)
			{
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(component.worldName);
				if (worldData == null)
				{
					return null;
				}
				return Assets.GetSprite(worldData.asteroidIcon);
			}
		}
		return null;
	}

	// Token: 0x06005397 RID: 21399 RVA: 0x001DF45C File Offset: 0x001DD65C
	public void SendClusterLocationChangedEvent(AxialI oldLocation, AxialI newLocation)
	{
		ClusterLocationChangedEvent data = new ClusterLocationChangedEvent
		{
			entity = this,
			oldLocation = oldLocation,
			newLocation = newLocation
		};
		base.Trigger(-1298331547, data);
		Game.Instance.Trigger(-1298331547, data);
		if (this.m_selectable != null && this.m_selectable.IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x040036FC RID: 14076
	[Serialize]
	protected AxialI m_location;

	// Token: 0x040036FD RID: 14077
	public bool positionDirty;

	// Token: 0x040036FE RID: 14078
	[MyCmpGet]
	protected KSelectable m_selectable;

	// Token: 0x040036FF RID: 14079
	[MyCmpReq]
	private Transform m_transform;

	// Token: 0x04003700 RID: 14080
	public bool isWorldEntity;

	// Token: 0x02001B48 RID: 6984
	public struct AnimConfig
	{
		// Token: 0x04007F49 RID: 32585
		public KAnimFile animFile;

		// Token: 0x04007F4A RID: 32586
		public string initialAnim;

		// Token: 0x04007F4B RID: 32587
		public KAnim.PlayMode playMode;

		// Token: 0x04007F4C RID: 32588
		public string symbolSwapTarget;

		// Token: 0x04007F4D RID: 32589
		public string symbolSwapSymbol;

		// Token: 0x04007F4E RID: 32590
		public Vector3 animOffset;

		// Token: 0x04007F4F RID: 32591
		public float animPlaySpeedModifier;
	}
}

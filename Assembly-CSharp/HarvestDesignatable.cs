using System;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008E7 RID: 2279
[AddComponentMenu("KMonoBehaviour/scripts/HarvestDesignatable")]
public class HarvestDesignatable : KMonoBehaviour
{
	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06004154 RID: 16724 RVA: 0x00173114 File Offset: 0x00171314
	public bool InPlanterBox
	{
		get
		{
			return this.isInPlanterBox;
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06004155 RID: 16725 RVA: 0x0017311C File Offset: 0x0017131C
	// (set) Token: 0x06004156 RID: 16726 RVA: 0x00173124 File Offset: 0x00171324
	public bool MarkedForHarvest
	{
		get
		{
			return this.isMarkedForHarvest;
		}
		set
		{
			this.isMarkedForHarvest = value;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06004157 RID: 16727 RVA: 0x0017312D File Offset: 0x0017132D
	public bool HarvestWhenReady
	{
		get
		{
			return this.harvestWhenReady;
		}
	}

	// Token: 0x06004158 RID: 16728 RVA: 0x00173135 File Offset: 0x00171335
	protected HarvestDesignatable()
	{
		this.onEnableOverlayDelegate = new Action<object>(this.OnEnableOverlay);
	}

	// Token: 0x06004159 RID: 16729 RVA: 0x00173168 File Offset: 0x00171368
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HarvestDesignatable>(1309017699, HarvestDesignatable.SetInPlanterBoxTrueDelegate);
	}

	// Token: 0x0600415A RID: 16730 RVA: 0x00173184 File Offset: 0x00171384
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		Components.HarvestDesignatables.Add(this);
		base.Subscribe<HarvestDesignatable>(493375141, HarvestDesignatable.OnRefreshUserMenuDelegate);
		base.Subscribe<HarvestDesignatable>(2127324410, HarvestDesignatable.OnCancelDelegate);
		Game.Instance.Subscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
		this.area = base.GetComponent<OccupyArea>();
	}

	// Token: 0x0600415B RID: 16731 RVA: 0x00173244 File Offset: 0x00171444
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HarvestDesignatables.Remove(this);
		this.DestroyOverlayIcon();
		Game.Instance.Unsubscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Unsubscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
	}

	// Token: 0x0600415C RID: 16732 RVA: 0x001732C8 File Offset: 0x001714C8
	private void DestroyOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			UnityEngine.Object.Destroy(this.HarvestWhenReadyOverlayIcon.gameObject);
			this.HarvestWhenReadyOverlayIcon = null;
		}
	}

	// Token: 0x0600415D RID: 16733 RVA: 0x001732F0 File Offset: 0x001714F0
	private void CreateOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			return;
		}
		if (base.GetComponent<AttackableBase>() == null)
		{
			this.HarvestWhenReadyOverlayIcon = Util.KInstantiate(Assets.UIPrefabs.HarvestWhenReadyOverlayIcon, GameScreenManager.Instance.worldSpaceCanvas, null).GetComponent<RectTransform>();
			Extents extents = base.GetComponent<OccupyArea>().GetExtents();
			Vector3 position;
			if (base.GetComponent<KPrefabID>().HasTag(GameTags.Hanging))
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)(extents.y + extents.height)) + this.iconOffset;
			}
			else
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)extents.y) + this.iconOffset;
			}
			this.HarvestWhenReadyOverlayIcon.transform.SetPosition(position);
			this.RefreshOverlayIcon(null);
		}
	}

	// Token: 0x0600415E RID: 16734 RVA: 0x001733E8 File Offset: 0x001715E8
	private void OnDisableOverlay(object data)
	{
		this.DestroyOverlayIcon();
	}

	// Token: 0x0600415F RID: 16735 RVA: 0x001733F0 File Offset: 0x001715F0
	private void OnEnableOverlay(object data)
	{
		if ((HashedString)data == OverlayModes.Harvest.ID)
		{
			this.CreateOverlayIcon();
			return;
		}
		this.DestroyOverlayIcon();
	}

	// Token: 0x06004160 RID: 16736 RVA: 0x00173414 File Offset: 0x00171614
	private void RefreshOverlayIcon(object data = null)
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			if ((Grid.IsVisible(Grid.PosToCell(base.gameObject)) && base.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId) || (CameraController.Instance != null && CameraController.Instance.FreeCameraEnabled))
			{
				if (!this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
				{
					this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(true);
				}
			}
			else if (this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
			{
				this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(false);
			}
			HierarchyReferences component = this.HarvestWhenReadyOverlayIcon.GetComponent<HierarchyReferences>();
			if (this.harvestWhenReady)
			{
				Image image = (Image)component.GetReference("On");
				image.gameObject.SetActive(true);
				image.color = GlobalAssets.Instance.colorSet.harvestEnabled;
				component.GetReference("Off").gameObject.SetActive(false);
				return;
			}
			component.GetReference("On").gameObject.SetActive(false);
			Image image2 = (Image)component.GetReference("Off");
			image2.gameObject.SetActive(true);
			image2.color = GlobalAssets.Instance.colorSet.harvestDisabled;
		}
	}

	// Token: 0x06004161 RID: 16737 RVA: 0x00173568 File Offset: 0x00171768
	public bool CanBeHarvested()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		return !(component != null) || component.CanBeHarvested;
	}

	// Token: 0x06004162 RID: 16738 RVA: 0x0017358D File Offset: 0x0017178D
	public void SetInPlanterBox(bool state)
	{
		if (state)
		{
			if (!this.isInPlanterBox)
			{
				this.isInPlanterBox = true;
				this.SetHarvestWhenReady(this.defaultHarvestStateWhenPlanted);
				return;
			}
		}
		else
		{
			this.isInPlanterBox = false;
		}
	}

	// Token: 0x06004163 RID: 16739 RVA: 0x001735B8 File Offset: 0x001717B8
	public void SetHarvestWhenReady(bool state)
	{
		this.harvestWhenReady = state;
		if (this.harvestWhenReady && this.CanBeHarvested() && !this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		if (this.isMarkedForHarvest && !this.harvestWhenReady)
		{
			this.OnCancel(null);
			if (this.CanBeHarvested() && this.isInPlanterBox)
			{
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		base.Trigger(-266953818, null);
		this.RefreshOverlayIcon(null);
	}

	// Token: 0x06004164 RID: 16740 RVA: 0x00173640 File Offset: 0x00171840
	protected virtual void OnCancel(object data = null)
	{
	}

	// Token: 0x06004165 RID: 16741 RVA: 0x00173644 File Offset: 0x00171844
	public virtual void MarkForHarvest()
	{
		if (!this.CanBeHarvested())
		{
			return;
		}
		this.isMarkedForHarvest = true;
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.OnMarkedForHarvest();
		}
	}

	// Token: 0x06004166 RID: 16742 RVA: 0x00173677 File Offset: 0x00171877
	protected virtual void OnClickHarvestWhenReady()
	{
		this.SetHarvestWhenReady(true);
	}

	// Token: 0x06004167 RID: 16743 RVA: 0x00173680 File Offset: 0x00171880
	protected virtual void OnClickCancelHarvestWhenReady()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.Trigger(2127324410, null);
		}
		this.SetHarvestWhenReady(false);
	}

	// Token: 0x06004168 RID: 16744 RVA: 0x001736B0 File Offset: 0x001718B0
	public virtual void OnRefreshUserMenu(object data)
	{
		if (this.showUserMenuButtons)
		{
			KIconButtonMenu.ButtonInfo button = this.harvestWhenReady ? new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickCancelHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.GAMEOBJECTEFFECTS.PLANT_DO_NOT_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.PLANT_MARK_FOR_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.HARVEST_WHEN_READY.TOOLTIP, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x04002B50 RID: 11088
	public Vector2 iconOffset = Vector2.zero;

	// Token: 0x04002B51 RID: 11089
	public bool defaultHarvestStateWhenPlanted = true;

	// Token: 0x04002B52 RID: 11090
	public OccupyArea area;

	// Token: 0x04002B53 RID: 11091
	[Serialize]
	protected bool isMarkedForHarvest;

	// Token: 0x04002B54 RID: 11092
	[Serialize]
	private bool isInPlanterBox;

	// Token: 0x04002B55 RID: 11093
	public bool showUserMenuButtons = true;

	// Token: 0x04002B56 RID: 11094
	[Serialize]
	protected bool harvestWhenReady;

	// Token: 0x04002B57 RID: 11095
	public RectTransform HarvestWhenReadyOverlayIcon;

	// Token: 0x04002B58 RID: 11096
	private Action<object> onEnableOverlayDelegate;

	// Token: 0x04002B59 RID: 11097
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnCancelDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04002B5A RID: 11098
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002B5B RID: 11099
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> SetInPlanterBoxTrueDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.SetInPlanterBox(true);
	});
}

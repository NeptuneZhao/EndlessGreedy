using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006BE RID: 1726
[AddComponentMenu("KMonoBehaviour/scripts/DirectionControl")]
public class DirectionControl : KMonoBehaviour
{
	// Token: 0x06002B84 RID: 11140 RVA: 0x000F43B0 File Offset: 0x000F25B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.allowedDirection = WorkableReactable.AllowedDirection.Any;
		this.directionInfos = new DirectionControl.DirectionInfo[]
		{
			new DirectionControl.DirectionInfo
			{
				allowLeft = true,
				allowRight = true,
				iconName = "action_direction_both",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_BOTH.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_BOTH.TOOLTIP
			},
			new DirectionControl.DirectionInfo
			{
				allowLeft = true,
				allowRight = false,
				iconName = "action_direction_left",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_LEFT.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_LEFT.TOOLTIP
			},
			new DirectionControl.DirectionInfo
			{
				allowLeft = false,
				allowRight = true,
				iconName = "action_direction_right",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_RIGHT.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_RIGHT.TOOLTIP
			}
		};
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DirectionControl, this);
	}

	// Token: 0x06002B85 RID: 11141 RVA: 0x000F44DC File Offset: 0x000F26DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetAllowedDirection(this.allowedDirection);
		base.Subscribe<DirectionControl>(493375141, DirectionControl.OnRefreshUserMenuDelegate);
		base.Subscribe<DirectionControl>(-905833192, DirectionControl.OnCopySettingsDelegate);
	}

	// Token: 0x06002B86 RID: 11142 RVA: 0x000F4514 File Offset: 0x000F2714
	private void SetAllowedDirection(WorkableReactable.AllowedDirection new_direction)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		DirectionControl.DirectionInfo directionInfo = this.directionInfos[(int)new_direction];
		bool flag = directionInfo.allowLeft && directionInfo.allowRight;
		bool is_visible = !flag && directionInfo.allowLeft;
		bool is_visible2 = !flag && directionInfo.allowRight;
		component.SetSymbolVisiblity("arrow2", flag);
		component.SetSymbolVisiblity("arrow_left", is_visible);
		component.SetSymbolVisiblity("arrow_right", is_visible2);
		if (new_direction != this.allowedDirection)
		{
			this.allowedDirection = new_direction;
			if (this.onDirectionChanged != null)
			{
				this.onDirectionChanged(this.allowedDirection);
			}
		}
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x000F45BB File Offset: 0x000F27BB
	private void OnChangeWorkableDirection()
	{
		this.SetAllowedDirection((WorkableReactable.AllowedDirection.Left + (int)this.allowedDirection) % (WorkableReactable.AllowedDirection)this.directionInfos.Length);
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x000F45D4 File Offset: 0x000F27D4
	private void OnCopySettings(object data)
	{
		DirectionControl component = ((GameObject)data).GetComponent<DirectionControl>();
		this.SetAllowedDirection(component.allowedDirection);
	}

	// Token: 0x06002B89 RID: 11145 RVA: 0x000F45FC File Offset: 0x000F27FC
	private void OnRefreshUserMenu(object data)
	{
		int num = (int)((WorkableReactable.AllowedDirection.Left + (int)this.allowedDirection) % (WorkableReactable.AllowedDirection)this.directionInfos.Length);
		DirectionControl.DirectionInfo directionInfo = this.directionInfos[num];
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(directionInfo.iconName, directionInfo.name, new System.Action(this.OnChangeWorkableDirection), global::Action.NumActions, null, null, null, directionInfo.tooltip, true), 0.4f);
	}

	// Token: 0x040018F7 RID: 6391
	[Serialize]
	public WorkableReactable.AllowedDirection allowedDirection;

	// Token: 0x040018F8 RID: 6392
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040018F9 RID: 6393
	private DirectionControl.DirectionInfo[] directionInfos;

	// Token: 0x040018FA RID: 6394
	public Action<WorkableReactable.AllowedDirection> onDirectionChanged;

	// Token: 0x040018FB RID: 6395
	private static readonly EventSystem.IntraObjectHandler<DirectionControl> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DirectionControl>(delegate(DirectionControl component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040018FC RID: 6396
	private static readonly EventSystem.IntraObjectHandler<DirectionControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DirectionControl>(delegate(DirectionControl component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020014BA RID: 5306
	private struct DirectionInfo
	{
		// Token: 0x04006ABE RID: 27326
		public bool allowLeft;

		// Token: 0x04006ABF RID: 27327
		public bool allowRight;

		// Token: 0x04006AC0 RID: 27328
		public string iconName;

		// Token: 0x04006AC1 RID: 27329
		public string name;

		// Token: 0x04006AC2 RID: 27330
		public string tooltip;
	}
}

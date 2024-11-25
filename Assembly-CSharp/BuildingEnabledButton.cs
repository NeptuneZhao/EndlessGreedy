using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000673 RID: 1651
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/BuildingEnabledButton")]
public class BuildingEnabledButton : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060028D7 RID: 10455 RVA: 0x000E74DF File Offset: 0x000E56DF
	// (set) Token: 0x060028D8 RID: 10456 RVA: 0x000E7504 File Offset: 0x000E5704
	public bool IsEnabled
	{
		get
		{
			return this.Operational != null && this.Operational.GetFlag(BuildingEnabledButton.EnabledFlag);
		}
		set
		{
			this.Operational.SetFlag(BuildingEnabledButton.EnabledFlag, value);
			Game.Instance.userMenu.Refresh(base.gameObject);
			this.buildingEnabled = value;
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.BuildingDisabled, !this.buildingEnabled, null);
			base.Trigger(1088293757, this.buildingEnabled);
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060028D9 RID: 10457 RVA: 0x000E7579 File Offset: 0x000E5779
	public bool WaitingForDisable
	{
		get
		{
			return this.IsEnabled && this.Toggleable.IsToggleQueued(this.ToggleIdx);
		}
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x000E7596 File Offset: 0x000E5796
	protected override void OnPrefabInit()
	{
		this.ToggleIdx = this.Toggleable.SetTarget(this);
		base.Subscribe<BuildingEnabledButton>(493375141, BuildingEnabledButton.OnRefreshUserMenuDelegate);
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x000E75BB File Offset: 0x000E57BB
	protected override void OnSpawn()
	{
		this.IsEnabled = this.buildingEnabled;
		if (this.queuedToggle)
		{
			this.OnMenuToggle();
		}
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x000E75D7 File Offset: 0x000E57D7
	public void HandleToggle()
	{
		this.queuedToggle = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x000E75F1 File Offset: 0x000E57F1
	public bool IsHandlerOn()
	{
		return this.IsEnabled;
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x000E75F9 File Offset: 0x000E57F9
	private void OnToggle()
	{
		this.IsEnabled = !this.IsEnabled;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x000E7620 File Offset: 0x000E5820
	private void OnMenuToggle()
	{
		if (!this.Toggleable.IsToggleQueued(this.ToggleIdx))
		{
			if (this.IsEnabled)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.queuedToggle = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.queuedToggle = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.Toggleable.Toggle(this.ToggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x000E76A4 File Offset: 0x000E58A4
	private void OnRefreshUserMenu(object data)
	{
		bool isEnabled = this.IsEnabled;
		bool flag = this.Toggleable.IsToggleQueued(this.ToggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isEnabled && !flag) || (!isEnabled && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME_OFF, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04001773 RID: 6003
	[MyCmpAdd]
	private Toggleable Toggleable;

	// Token: 0x04001774 RID: 6004
	[MyCmpReq]
	private Operational Operational;

	// Token: 0x04001775 RID: 6005
	private int ToggleIdx;

	// Token: 0x04001776 RID: 6006
	[Serialize]
	private bool buildingEnabled = true;

	// Token: 0x04001777 RID: 6007
	[Serialize]
	private bool queuedToggle;

	// Token: 0x04001778 RID: 6008
	public static readonly Operational.Flag EnabledFlag = new Operational.Flag("building_enabled", Operational.Flag.Type.Functional);

	// Token: 0x04001779 RID: 6009
	private static readonly EventSystem.IntraObjectHandler<BuildingEnabledButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingEnabledButton>(delegate(BuildingEnabledButton component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}

using System;
using System.Collections;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D9A RID: 3482
public class RocketModuleSideScreen : SideScreenContent
{
	// Token: 0x06006DCD RID: 28109 RVA: 0x00294A08 File Offset: 0x00292C08
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		RocketModuleSideScreen.instance = this;
	}

	// Token: 0x06006DCE RID: 28110 RVA: 0x00294A16 File Offset: 0x00292C16
	protected override void OnForcedCleanUp()
	{
		RocketModuleSideScreen.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006DCF RID: 28111 RVA: 0x00294A24 File Offset: 0x00292C24
	public override int GetSideScreenSortOrder()
	{
		return 500;
	}

	// Token: 0x06006DD0 RID: 28112 RVA: 0x00294A2C File Offset: 0x00292C2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.addNewModuleButton.onClick += delegate()
		{
			Vector2 vector = Vector2.zero;
			if (SelectModuleSideScreen.Instance != null)
			{
				vector = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
			}
			this.ClickAddNew(vector.y, null);
		};
		this.removeModuleButton.onClick += this.ClickRemove;
		this.moveModuleUpButton.onClick += this.ClickSwapUp;
		this.moveModuleDownButton.onClick += this.ClickSwapDown;
		this.changeModuleButton.onClick += delegate()
		{
			Vector2 vector = Vector2.zero;
			if (SelectModuleSideScreen.Instance != null)
			{
				vector = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
			}
			this.ClickChangeModule(vector.y);
		};
		this.viewInteriorButton.onClick += this.ClickViewInterior;
		this.moduleNameLabel.textStyleSetting = this.nameSetting;
		this.moduleDescriptionLabel.textStyleSetting = this.descriptionSetting;
		this.moduleNameLabel.ApplySettings();
		this.moduleDescriptionLabel.ApplySettings();
	}

	// Token: 0x06006DD1 RID: 28113 RVA: 0x00294B01 File Offset: 0x00292D01
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x06006DD2 RID: 28114 RVA: 0x00294B13 File Offset: 0x00292D13
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06006DD3 RID: 28115 RVA: 0x00294B1B File Offset: 0x00292D1B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ReorderableBuilding>() != null;
	}

	// Token: 0x06006DD4 RID: 28116 RVA: 0x00294B2C File Offset: 0x00292D2C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.reorderable = new_target.GetComponent<ReorderableBuilding>();
		this.moduleIcon.sprite = Def.GetUISprite(this.reorderable.gameObject, "ui", false).first;
		this.moduleNameLabel.SetText(this.reorderable.GetProperName());
		this.moduleDescriptionLabel.SetText(this.reorderable.GetComponent<Building>().Desc);
		this.UpdateButtonStates();
	}

	// Token: 0x06006DD5 RID: 28117 RVA: 0x00294BB8 File Offset: 0x00292DB8
	public void UpdateButtonStates()
	{
		this.changeModuleButton.isInteractable = this.reorderable.CanChangeModule();
		this.changeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.changeModuleButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.INVALID.text);
		this.addNewModuleButton.isInteractable = true;
		this.addNewModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.ADDMODULE.DESC.text);
		this.removeModuleButton.isInteractable = this.reorderable.CanRemoveModule();
		this.removeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.removeModuleButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.INVALID.text);
		this.moveModuleDownButton.isInteractable = this.reorderable.CanSwapDown(true);
		this.moveModuleDownButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleDownButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.INVALID.text);
		this.moveModuleUpButton.isInteractable = this.reorderable.CanSwapUp(true);
		this.moveModuleUpButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleUpButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.INVALID.text);
		ClustercraftExteriorDoor component = this.reorderable.GetComponent<ClustercraftExteriorDoor>();
		if (!(component != null) || !component.HasTargetWorld())
		{
			this.viewInteriorButton.isInteractable = false;
			this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.LABEL);
			this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.INVALID.text);
			return;
		}
		if (ClusterManager.Instance.activeWorld == component.GetTargetWorld())
		{
			this.changeModuleButton.isInteractable = false;
			this.addNewModuleButton.isInteractable = false;
			this.removeModuleButton.isInteractable = false;
			this.moveModuleDownButton.isInteractable = false;
			this.moveModuleUpButton.isInteractable = false;
			this.viewInteriorButton.isInteractable = (component.GetMyWorldId() != 255);
			this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL);
			this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.INVALID.text);
			return;
		}
		this.viewInteriorButton.isInteractable = (this.reorderable.GetComponent<PassengerRocketModule>() != null);
		this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.LABEL);
		this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.INVALID.text);
	}

	// Token: 0x06006DD6 RID: 28118 RVA: 0x00294EB8 File Offset: 0x002930B8
	public void ClickAddNew(float scrollViewPosition, BuildingDef autoSelectDef = null)
	{
		SelectModuleSideScreen selectModuleSideScreen = (SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
		selectModuleSideScreen.addingNewModule = true;
		selectModuleSideScreen.SetTarget(this.reorderable.gameObject);
		if (autoSelectDef != null)
		{
			selectModuleSideScreen.SelectModule(autoSelectDef);
		}
		this.ScrollToTargetPoint(scrollViewPosition);
	}

	// Token: 0x06006DD7 RID: 28119 RVA: 0x00294F14 File Offset: 0x00293114
	private void ScrollToTargetPoint(float scrollViewPosition)
	{
		if (SelectModuleSideScreen.Instance != null)
		{
			SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0f, scrollViewPosition);
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.DelayedScrollToTargetPoint(scrollViewPosition));
			}
		}
	}

	// Token: 0x06006DD8 RID: 28120 RVA: 0x00294F6D File Offset: 0x0029316D
	private IEnumerator DelayedScrollToTargetPoint(float scrollViewPosition)
	{
		if (SelectModuleSideScreen.Instance != null)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
			SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0f, scrollViewPosition);
		}
		yield break;
	}

	// Token: 0x06006DD9 RID: 28121 RVA: 0x00294F7C File Offset: 0x0029317C
	private void ClickRemove()
	{
		this.reorderable.Trigger(-790448070, null);
		this.UpdateButtonStates();
	}

	// Token: 0x06006DDA RID: 28122 RVA: 0x00294F95 File Offset: 0x00293195
	private void ClickSwapUp()
	{
		this.reorderable.SwapWithAbove(true);
		this.UpdateButtonStates();
	}

	// Token: 0x06006DDB RID: 28123 RVA: 0x00294FA9 File Offset: 0x002931A9
	private void ClickSwapDown()
	{
		this.reorderable.SwapWithBelow(true);
		this.UpdateButtonStates();
	}

	// Token: 0x06006DDC RID: 28124 RVA: 0x00294FBD File Offset: 0x002931BD
	private void ClickChangeModule(float scrollViewPosition)
	{
		SelectModuleSideScreen selectModuleSideScreen = (SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
		selectModuleSideScreen.addingNewModule = false;
		selectModuleSideScreen.SetTarget(this.reorderable.gameObject);
		this.ScrollToTargetPoint(scrollViewPosition);
	}

	// Token: 0x06006DDD RID: 28125 RVA: 0x00294FFC File Offset: 0x002931FC
	private void ClickViewInterior()
	{
		ClustercraftExteriorDoor component = this.reorderable.GetComponent<ClustercraftExteriorDoor>();
		PassengerRocketModule component2 = this.reorderable.GetComponent<PassengerRocketModule>();
		WorldContainer targetWorld = component.GetTargetWorld();
		WorldContainer myWorld = component.GetMyWorld();
		if (ClusterManager.Instance.activeWorld == targetWorld)
		{
			if (myWorld.id != 255)
			{
				AudioMixer.instance.Stop(component2.interiorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
				AudioMixer.instance.PauseSpaceVisibleSnapshot(false);
				ClusterManager.Instance.SetActiveWorld(myWorld.id);
			}
		}
		else
		{
			AudioMixer.instance.Start(component2.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			ClusterManager.Instance.SetActiveWorld(targetWorld.id);
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
		this.UpdateButtonStates();
	}

	// Token: 0x04004AED RID: 19181
	public static RocketModuleSideScreen instance;

	// Token: 0x04004AEE RID: 19182
	private ReorderableBuilding reorderable;

	// Token: 0x04004AEF RID: 19183
	public KScreen changeModuleSideScreen;

	// Token: 0x04004AF0 RID: 19184
	public Image moduleIcon;

	// Token: 0x04004AF1 RID: 19185
	[Header("Buttons")]
	public KButton addNewModuleButton;

	// Token: 0x04004AF2 RID: 19186
	public KButton removeModuleButton;

	// Token: 0x04004AF3 RID: 19187
	public KButton changeModuleButton;

	// Token: 0x04004AF4 RID: 19188
	public KButton moveModuleUpButton;

	// Token: 0x04004AF5 RID: 19189
	public KButton moveModuleDownButton;

	// Token: 0x04004AF6 RID: 19190
	public KButton viewInteriorButton;

	// Token: 0x04004AF7 RID: 19191
	[Header("Labels")]
	public LocText moduleNameLabel;

	// Token: 0x04004AF8 RID: 19192
	public LocText moduleDescriptionLabel;

	// Token: 0x04004AF9 RID: 19193
	public TextStyleSetting nameSetting;

	// Token: 0x04004AFA RID: 19194
	public TextStyleSetting descriptionSetting;
}

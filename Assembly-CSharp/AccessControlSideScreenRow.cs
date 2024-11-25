using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D41 RID: 3393
public class AccessControlSideScreenRow : AccessControlSideScreenDoor
{
	// Token: 0x06006AAC RID: 27308 RVA: 0x00282D99 File Offset: 0x00280F99
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.defaultButton.onValueChanged += this.OnDefaultButtonChanged;
	}

	// Token: 0x06006AAD RID: 27309 RVA: 0x00282DB8 File Offset: 0x00280FB8
	private void OnDefaultButtonChanged(bool state)
	{
		this.UpdateButtonStates(!state);
		if (this.defaultClickedCallback != null)
		{
			this.defaultClickedCallback(this.targetIdentity, !state);
		}
	}

	// Token: 0x06006AAE RID: 27310 RVA: 0x00282DE4 File Offset: 0x00280FE4
	protected override void UpdateButtonStates(bool isDefault)
	{
		base.UpdateButtonStates(isDefault);
		this.defaultButton.GetComponent<ToolTip>().SetSimpleTooltip(isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_CUSTOM : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_DEFAULT);
		this.defaultControls.SetActive(isDefault);
		this.customControls.SetActive(!isDefault);
	}

	// Token: 0x06006AAF RID: 27311 RVA: 0x00282E38 File Offset: 0x00281038
	public void SetMinionContent(MinionAssignablesProxy identity, AccessControl.Permission permission, bool isDefault, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange, Action<MinionAssignablesProxy, bool> onDefaultClick)
	{
		base.SetContent(permission, onPermissionChange);
		if (identity == null)
		{
			global::Debug.LogError("Invalid data received.");
			return;
		}
		if (this.portraitInstance == null)
		{
			this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, this.defaultButton.gameObject, false);
			this.portraitInstance.SetAlpha(1f);
		}
		this.targetIdentity = identity;
		this.portraitInstance.SetIdentityObject(identity, false);
		this.portraitInstance.SetSubTitle(isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_DEFAULT : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_CUSTOM);
		this.defaultClickedCallback = null;
		this.defaultButton.isOn = !isDefault;
		this.defaultClickedCallback = onDefaultClick;
	}

	// Token: 0x040048BA RID: 18618
	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	// Token: 0x040048BB RID: 18619
	private CrewPortrait portraitInstance;

	// Token: 0x040048BC RID: 18620
	public KToggle defaultButton;

	// Token: 0x040048BD RID: 18621
	public GameObject defaultControls;

	// Token: 0x040048BE RID: 18622
	public GameObject customControls;

	// Token: 0x040048BF RID: 18623
	private Action<MinionAssignablesProxy, bool> defaultClickedCallback;
}

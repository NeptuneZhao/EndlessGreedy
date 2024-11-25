using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D40 RID: 3392
[AddComponentMenu("KMonoBehaviour/scripts/AccessControlSideScreenDoor")]
public class AccessControlSideScreenDoor : KMonoBehaviour
{
	// Token: 0x06006AA6 RID: 27302 RVA: 0x00282BFB File Offset: 0x00280DFB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.leftButton.onClick += this.OnPermissionButtonClicked;
		this.rightButton.onClick += this.OnPermissionButtonClicked;
	}

	// Token: 0x06006AA7 RID: 27303 RVA: 0x00282C34 File Offset: 0x00280E34
	private void OnPermissionButtonClicked()
	{
		AccessControl.Permission arg;
		if (this.leftButton.isOn)
		{
			if (this.rightButton.isOn)
			{
				arg = AccessControl.Permission.Both;
			}
			else
			{
				arg = AccessControl.Permission.GoLeft;
			}
		}
		else if (this.rightButton.isOn)
		{
			arg = AccessControl.Permission.GoRight;
		}
		else
		{
			arg = AccessControl.Permission.Neither;
		}
		this.UpdateButtonStates(false);
		this.permissionChangedCallback(this.targetIdentity, arg);
	}

	// Token: 0x06006AA8 RID: 27304 RVA: 0x00282C90 File Offset: 0x00280E90
	protected virtual void UpdateButtonStates(bool isDefault)
	{
		ToolTip component = this.leftButton.GetComponent<ToolTip>();
		ToolTip component2 = this.rightButton.GetComponent<ToolTip>();
		if (this.isUpDown)
		{
			component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_DISABLED);
			component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_DISABLED);
			return;
		}
		component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_DISABLED);
		component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_DISABLED);
	}

	// Token: 0x06006AA9 RID: 27305 RVA: 0x00282D4E File Offset: 0x00280F4E
	public void SetRotated(bool rotated)
	{
		this.isUpDown = rotated;
	}

	// Token: 0x06006AAA RID: 27306 RVA: 0x00282D57 File Offset: 0x00280F57
	public void SetContent(AccessControl.Permission permission, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange)
	{
		this.permissionChangedCallback = onPermissionChange;
		this.leftButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft);
		this.rightButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight);
		this.UpdateButtonStates(false);
	}

	// Token: 0x040048B5 RID: 18613
	public KToggle leftButton;

	// Token: 0x040048B6 RID: 18614
	public KToggle rightButton;

	// Token: 0x040048B7 RID: 18615
	private Action<MinionAssignablesProxy, AccessControl.Permission> permissionChangedCallback;

	// Token: 0x040048B8 RID: 18616
	private bool isUpDown;

	// Token: 0x040048B9 RID: 18617
	protected MinionAssignablesProxy targetIdentity;
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000B6A RID: 2922
public class InputModuleSwitch : MonoBehaviour
{
	// Token: 0x060057CA RID: 22474 RVA: 0x001F9C08 File Offset: 0x001F7E08
	private void Update()
	{
		if (this.lastMousePosition != Input.mousePosition && KInputManager.currentControllerIsGamepad)
		{
			KInputManager.currentControllerIsGamepad = false;
			KInputManager.InputChange.Invoke();
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			this.virtualInput.enabled = KInputManager.currentControllerIsGamepad;
			if (this.standaloneInput.enabled)
			{
				this.standaloneInput.enabled = false;
				this.ChangeInputHandler();
				return;
			}
		}
		else
		{
			this.lastMousePosition = Input.mousePosition;
			this.standaloneInput.enabled = true;
			if (this.virtualInput.enabled)
			{
				this.virtualInput.enabled = false;
				this.ChangeInputHandler();
			}
		}
	}

	// Token: 0x060057CB RID: 22475 RVA: 0x001F9CAC File Offset: 0x001F7EAC
	private void ChangeInputHandler()
	{
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.usedMenus.Count; i++)
		{
			if (inputManager.usedMenus[i].Equals(null))
			{
				inputManager.usedMenus.RemoveAt(i);
			}
		}
		if (inputManager.GetControllerCount() > 1)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				Cursor.visible = false;
				inputManager.GetController(1).inputHandler.TransferHandles(inputManager.GetController(0).inputHandler);
				return;
			}
			Cursor.visible = true;
			inputManager.GetController(0).inputHandler.TransferHandles(inputManager.GetController(1).inputHandler);
		}
	}

	// Token: 0x04003960 RID: 14688
	public VirtualInputModule virtualInput;

	// Token: 0x04003961 RID: 14689
	public StandaloneInputModule standaloneInput;

	// Token: 0x04003962 RID: 14690
	private Vector3 lastMousePosition;
}

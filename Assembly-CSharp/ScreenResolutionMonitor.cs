using System;
using UnityEngine;

// Token: 0x02000DFC RID: 3580
public class ScreenResolutionMonitor : MonoBehaviour
{
	// Token: 0x06007196 RID: 29078 RVA: 0x002AFBA4 File Offset: 0x002ADDA4
	private void Awake()
	{
		this.previousSize = new Vector2((float)Screen.width, (float)Screen.height);
	}

	// Token: 0x06007197 RID: 29079 RVA: 0x002AFBC0 File Offset: 0x002ADDC0
	private void Update()
	{
		if ((this.previousSize.x != (float)Screen.width || this.previousSize.y != (float)Screen.height) && Game.Instance != null)
		{
			Game.Instance.Trigger(445618876, null);
			this.previousSize.x = (float)Screen.width;
			this.previousSize.y = (float)Screen.height;
		}
		this.UpdateShouldUseGamepadUIMode();
	}

	// Token: 0x06007198 RID: 29080 RVA: 0x002AFC38 File Offset: 0x002ADE38
	public static bool UsingGamepadUIMode()
	{
		return ScreenResolutionMonitor.previousGamepadUIMode;
	}

	// Token: 0x06007199 RID: 29081 RVA: 0x002AFC40 File Offset: 0x002ADE40
	private void UpdateShouldUseGamepadUIMode()
	{
		bool flag = (Screen.dpi > 130f && Screen.height < 900) || KInputManager.currentControllerIsGamepad;
		if (flag != ScreenResolutionMonitor.previousGamepadUIMode)
		{
			ScreenResolutionMonitor.previousGamepadUIMode = flag;
			if (Game.Instance == null)
			{
				return;
			}
			Game.Instance.Trigger(-442024484, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(flag ? "ControllerType_ToggleOn" : "ControllerType_ToggleOff", false));
		}
	}

	// Token: 0x04004E66 RID: 20070
	[SerializeField]
	private Vector2 previousSize;

	// Token: 0x04004E67 RID: 20071
	private static bool previousGamepadUIMode;

	// Token: 0x04004E68 RID: 20072
	private const float HIGH_DPI = 130f;
}

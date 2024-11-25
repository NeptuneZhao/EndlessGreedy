using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DEF RID: 3567
public class VirtualCursorOverlayFix : MonoBehaviour
{
	// Token: 0x06007140 RID: 28992 RVA: 0x002AD800 File Offset: 0x002ABA00
	private void Awake()
	{
		int width = Screen.currentResolution.width;
		int height = Screen.currentResolution.height;
		this.cursorRendTex = new RenderTexture(width, height, 0);
		this.screenSpaceCamera.enabled = true;
		this.screenSpaceCamera.targetTexture = this.cursorRendTex;
		this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
		base.StartCoroutine(this.RenderVirtualCursor());
	}

	// Token: 0x06007141 RID: 28993 RVA: 0x002AD87C File Offset: 0x002ABA7C
	private IEnumerator RenderVirtualCursor()
	{
		bool ShowCursor = KInputManager.currentControllerIsGamepad;
		while (Application.isPlaying)
		{
			ShowCursor = KInputManager.currentControllerIsGamepad;
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.C))
			{
				ShowCursor = true;
			}
			this.screenSpaceCamera.enabled = true;
			if (!this.screenSpaceOverlayImage.enabled && ShowCursor)
			{
				yield return SequenceUtil.WaitForSecondsRealtime(0.1f);
			}
			this.actualCursor.enabled = ShowCursor;
			this.screenSpaceOverlayImage.enabled = ShowCursor;
			this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04004DE9 RID: 19945
	private RenderTexture cursorRendTex;

	// Token: 0x04004DEA RID: 19946
	public Camera screenSpaceCamera;

	// Token: 0x04004DEB RID: 19947
	public Image screenSpaceOverlayImage;

	// Token: 0x04004DEC RID: 19948
	public RawImage actualCursor;
}

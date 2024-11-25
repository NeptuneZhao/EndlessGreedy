using System;
using UnityEngine;

// Token: 0x02000A43 RID: 2627
public class ScreenResize : MonoBehaviour
{
	// Token: 0x06004C11 RID: 19473 RVA: 0x001B2284 File Offset: 0x001B0484
	private void Awake()
	{
		ScreenResize.Instance = this;
		this.isFullscreen = Screen.fullScreen;
		this.OnResize = (System.Action)Delegate.Combine(this.OnResize, new System.Action(this.SaveResolutionToPrefs));
	}

	// Token: 0x06004C12 RID: 19474 RVA: 0x001B22BC File Offset: 0x001B04BC
	private void LateUpdate()
	{
		if (Screen.width != this.Width || Screen.height != this.Height || this.isFullscreen != Screen.fullScreen)
		{
			this.Width = Screen.width;
			this.Height = Screen.height;
			this.isFullscreen = Screen.fullScreen;
			this.TriggerResize();
		}
	}

	// Token: 0x06004C13 RID: 19475 RVA: 0x001B2317 File Offset: 0x001B0517
	public void TriggerResize()
	{
		if (this.OnResize != null)
		{
			this.OnResize();
		}
	}

	// Token: 0x06004C14 RID: 19476 RVA: 0x001B232C File Offset: 0x001B052C
	private void SaveResolutionToPrefs()
	{
		GraphicsOptionsScreen.OnResize();
	}

	// Token: 0x04003289 RID: 12937
	public System.Action OnResize;

	// Token: 0x0400328A RID: 12938
	public static ScreenResize Instance;

	// Token: 0x0400328B RID: 12939
	private int Width;

	// Token: 0x0400328C RID: 12940
	private int Height;

	// Token: 0x0400328D RID: 12941
	private bool isFullscreen;
}

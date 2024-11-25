using System;
using UnityEngine;

// Token: 0x02000B77 RID: 2935
public class FillRenderTargetEffect : MonoBehaviour
{
	// Token: 0x06005829 RID: 22569 RVA: 0x001FDC59 File Offset: 0x001FBE59
	public void SetFillTexture(Texture tex)
	{
		this.fillTexture = tex;
	}

	// Token: 0x0600582A RID: 22570 RVA: 0x001FDC62 File Offset: 0x001FBE62
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(this.fillTexture, null);
	}

	// Token: 0x040039A5 RID: 14757
	private Texture fillTexture;
}

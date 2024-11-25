using System;
using UnityEngine;

// Token: 0x02000A44 RID: 2628
public class SimDebugViewCompositor : MonoBehaviour
{
	// Token: 0x06004C16 RID: 19478 RVA: 0x001B233B File Offset: 0x001B053B
	private void Awake()
	{
		SimDebugViewCompositor.Instance = this;
	}

	// Token: 0x06004C17 RID: 19479 RVA: 0x001B2343 File Offset: 0x001B0543
	private void OnDestroy()
	{
		SimDebugViewCompositor.Instance = null;
	}

	// Token: 0x06004C18 RID: 19480 RVA: 0x001B234B File Offset: 0x001B054B
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/SimDebugViewCompositor"));
		this.Toggle(false);
	}

	// Token: 0x06004C19 RID: 19481 RVA: 0x001B2369 File Offset: 0x001B0569
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen.Instance.RunPostProcessEffects(src, dest);
		}
	}

	// Token: 0x06004C1A RID: 19482 RVA: 0x001B2391 File Offset: 0x001B0591
	public void Toggle(bool is_on)
	{
		base.enabled = is_on;
	}

	// Token: 0x0400328E RID: 12942
	public Material material;

	// Token: 0x0400328F RID: 12943
	public static SimDebugViewCompositor Instance;
}

using System;
using UnityEngine;

// Token: 0x02000A3F RID: 2623
public class MultipleRenderTargetProxy : MonoBehaviour
{
	// Token: 0x06004BFA RID: 19450 RVA: 0x001B1C18 File Offset: 0x001AFE18
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.CreateRenderTarget();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x06004BFB RID: 19451 RVA: 0x001B1C6F File Offset: 0x001AFE6F
	public void ToggleColouredOverlayView(bool enabled)
	{
		this.colouredOverlayBufferEnabled = enabled;
		this.CreateRenderTarget();
	}

	// Token: 0x06004BFC RID: 19452 RVA: 0x001B1C80 File Offset: 0x001AFE80
	private void CreateRenderTarget()
	{
		RenderBuffer[] array = new RenderBuffer[this.colouredOverlayBufferEnabled ? 3 : 2];
		this.Textures[0] = this.RecreateRT(this.Textures[0], 24, RenderTextureFormat.ARGB32);
		this.Textures[0].filterMode = FilterMode.Point;
		this.Textures[0].name = "MRT0";
		this.Textures[1] = this.RecreateRT(this.Textures[1], 0, RenderTextureFormat.R8);
		this.Textures[1].filterMode = FilterMode.Point;
		this.Textures[1].name = "MRT1";
		array[0] = this.Textures[0].colorBuffer;
		array[1] = this.Textures[1].colorBuffer;
		if (this.colouredOverlayBufferEnabled)
		{
			this.Textures[2] = this.RecreateRT(this.Textures[2], 0, RenderTextureFormat.ARGB32);
			this.Textures[2].filterMode = FilterMode.Bilinear;
			this.Textures[2].name = "MRT2";
			array[2] = this.Textures[2].colorBuffer;
		}
		base.GetComponent<Camera>().SetTargetBuffers(array, this.Textures[0].depthBuffer);
		this.OnShadersReloaded();
	}

	// Token: 0x06004BFD RID: 19453 RVA: 0x001B1DAC File Offset: 0x001AFFAC
	private RenderTexture RecreateRT(RenderTexture rt, int depth, RenderTextureFormat format)
	{
		RenderTexture result = rt;
		if (rt == null || rt.width != Screen.width || rt.height != Screen.height || rt.format != format)
		{
			if (rt != null)
			{
				rt.DestroyRenderTexture();
			}
			result = new RenderTexture(Screen.width, Screen.height, depth, format);
		}
		return result;
	}

	// Token: 0x06004BFE RID: 19454 RVA: 0x001B1E09 File Offset: 0x001B0009
	private void OnResize()
	{
		this.CreateRenderTarget();
	}

	// Token: 0x06004BFF RID: 19455 RVA: 0x001B1E11 File Offset: 0x001B0011
	private void Update()
	{
		if (!this.Textures[0].IsCreated())
		{
			this.CreateRenderTarget();
		}
	}

	// Token: 0x06004C00 RID: 19456 RVA: 0x001B1E28 File Offset: 0x001B0028
	private void OnShadersReloaded()
	{
		Shader.SetGlobalTexture("_MRT0", this.Textures[0]);
		Shader.SetGlobalTexture("_MRT1", this.Textures[1]);
		if (this.colouredOverlayBufferEnabled)
		{
			Shader.SetGlobalTexture("_MRT2", this.Textures[2]);
		}
	}

	// Token: 0x04003275 RID: 12917
	public RenderTexture[] Textures = new RenderTexture[3];

	// Token: 0x04003276 RID: 12918
	private bool colouredOverlayBufferEnabled;
}

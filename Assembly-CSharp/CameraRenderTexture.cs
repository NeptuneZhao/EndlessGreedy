using System;
using UnityEngine;

// Token: 0x02000A2F RID: 2607
public class CameraRenderTexture : MonoBehaviour
{
	// Token: 0x06004B82 RID: 19330 RVA: 0x001AE3FC File Offset: 0x001AC5FC
	private void Awake()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/CameraRenderTexture"));
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x001AE413 File Offset: 0x001AC613
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.OnResize();
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x001AE450 File Offset: 0x001AC650
	private void OnResize()
	{
		if (this.resultTexture != null)
		{
			this.resultTexture.DestroyRenderTexture();
		}
		this.resultTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
		this.resultTexture.name = base.name;
		this.resultTexture.filterMode = FilterMode.Point;
		this.resultTexture.autoGenerateMips = false;
		if (this.TextureName != "")
		{
			Shader.SetGlobalTexture(this.TextureName, this.resultTexture);
		}
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x001AE4D9 File Offset: 0x001AC6D9
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.resultTexture, this.material);
	}

	// Token: 0x06004B86 RID: 19334 RVA: 0x001AE4ED File Offset: 0x001AC6ED
	public RenderTexture GetTexture()
	{
		return this.resultTexture;
	}

	// Token: 0x06004B87 RID: 19335 RVA: 0x001AE4F5 File Offset: 0x001AC6F5
	public bool ShouldFlip()
	{
		return false;
	}

	// Token: 0x04003179 RID: 12665
	public string TextureName;

	// Token: 0x0400317A RID: 12666
	private RenderTexture resultTexture;

	// Token: 0x0400317B RID: 12667
	private Material material;
}

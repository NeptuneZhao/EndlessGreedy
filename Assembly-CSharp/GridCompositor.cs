using System;
using UnityEngine;

// Token: 0x02000A34 RID: 2612
public class GridCompositor : MonoBehaviour
{
	// Token: 0x06004B9E RID: 19358 RVA: 0x001AF7E8 File Offset: 0x001AD9E8
	public static void DestroyInstance()
	{
		GridCompositor.Instance = null;
	}

	// Token: 0x06004B9F RID: 19359 RVA: 0x001AF7F0 File Offset: 0x001AD9F0
	private void Awake()
	{
		GridCompositor.Instance = this;
		base.enabled = false;
	}

	// Token: 0x06004BA0 RID: 19360 RVA: 0x001AF7FF File Offset: 0x001AD9FF
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/GridCompositor"));
	}

	// Token: 0x06004BA1 RID: 19361 RVA: 0x001AF816 File Offset: 0x001ADA16
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
	}

	// Token: 0x06004BA2 RID: 19362 RVA: 0x001AF825 File Offset: 0x001ADA25
	public void ToggleMajor(bool on)
	{
		this.onMajor = on;
		this.Refresh();
	}

	// Token: 0x06004BA3 RID: 19363 RVA: 0x001AF834 File Offset: 0x001ADA34
	public void ToggleMinor(bool on)
	{
		this.onMinor = on;
		this.Refresh();
	}

	// Token: 0x06004BA4 RID: 19364 RVA: 0x001AF843 File Offset: 0x001ADA43
	private void Refresh()
	{
		base.enabled = (this.onMinor || this.onMajor);
	}

	// Token: 0x04003184 RID: 12676
	public Material material;

	// Token: 0x04003185 RID: 12677
	public static GridCompositor Instance;

	// Token: 0x04003186 RID: 12678
	private bool onMajor;

	// Token: 0x04003187 RID: 12679
	private bool onMinor;
}

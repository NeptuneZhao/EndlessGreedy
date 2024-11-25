using System;
using UnityEngine;

// Token: 0x02000B76 RID: 2934
public class BloomEffect : MonoBehaviour
{
	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06005822 RID: 22562 RVA: 0x001FD9E3 File Offset: 0x001FBBE3
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.blurShader);
				this.m_Material.hideFlags = HideFlags.DontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x06005823 RID: 22563 RVA: 0x001FDA17 File Offset: 0x001FBC17
	protected void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x06005824 RID: 22564 RVA: 0x001FDA34 File Offset: 0x001FBC34
	protected void Start()
	{
		if (!this.blurShader || !this.material.shader.isSupported)
		{
			base.enabled = false;
			return;
		}
		this.BloomMaskMaterial = new Material(Shader.Find("Klei/PostFX/BloomMask"));
		this.BloomCompositeMaterial = new Material(Shader.Find("Klei/PostFX/BloomComposite"));
	}

	// Token: 0x06005825 RID: 22565 RVA: 0x001FDA94 File Offset: 0x001FBC94
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06005826 RID: 22566 RVA: 0x001FDB00 File Offset: 0x001FBD00
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06005827 RID: 22567 RVA: 0x001FDB64 File Offset: 0x001FBD64
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
		temporary.name = "bloom_source";
		Graphics.Blit(source, temporary, this.BloomMaskMaterial);
		int width = Math.Max(source.width / 4, 4);
		int height = Math.Max(source.height / 4, 4);
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
		renderTexture.name = "bloom_downsampled";
		this.DownSample4x(temporary, renderTexture);
		RenderTexture.ReleaseTemporary(temporary);
		for (int i = 0; i < this.iterations; i++)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0);
			temporary2.name = "bloom_blurred";
			this.FourTapCone(renderTexture, temporary2, i);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary2;
		}
		this.BloomCompositeMaterial.SetTexture("_BloomTex", renderTexture);
		Graphics.Blit(source, destination, this.BloomCompositeMaterial);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x0400399F RID: 14751
	private Material BloomMaskMaterial;

	// Token: 0x040039A0 RID: 14752
	private Material BloomCompositeMaterial;

	// Token: 0x040039A1 RID: 14753
	public int iterations = 3;

	// Token: 0x040039A2 RID: 14754
	public float blurSpread = 0.6f;

	// Token: 0x040039A3 RID: 14755
	public Shader blurShader;

	// Token: 0x040039A4 RID: 14756
	private Material m_Material;
}

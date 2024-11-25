using System;
using UnityEngine;

// Token: 0x02000A37 RID: 2615
public class LightBufferCompositor : MonoBehaviour
{
	// Token: 0x06004BD6 RID: 19414 RVA: 0x001B055C File Offset: 0x001AE75C
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/LightBufferCompositor"));
		this.material.SetTexture("_InvalidTex", Assets.instance.invalidAreaTex);
		this.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
		this.OnShadersReloaded();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x001B05C4 File Offset: 0x001AE7C4
	private void OnEnable()
	{
		this.OnShadersReloaded();
	}

	// Token: 0x06004BD8 RID: 19416 RVA: 0x001B05CC File Offset: 0x001AE7CC
	private void DownSample4x(Texture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.blurMaterial, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06004BD9 RID: 19417 RVA: 0x001B062E File Offset: 0x001AE82E
	[ContextMenu("ToggleParticles")]
	private void ToggleParticles()
	{
		this.particlesEnabled = !this.particlesEnabled;
		this.UpdateMaterialState();
	}

	// Token: 0x06004BDA RID: 19418 RVA: 0x001B0645 File Offset: 0x001AE845
	public void SetParticlesEnabled(bool enabled)
	{
		this.particlesEnabled = enabled;
		this.UpdateMaterialState();
	}

	// Token: 0x06004BDB RID: 19419 RVA: 0x001B0654 File Offset: 0x001AE854
	private void UpdateMaterialState()
	{
		if (this.particlesEnabled)
		{
			this.material.DisableKeyword("DISABLE_TEMPERATURE_PARTICLES");
			return;
		}
		this.material.EnableKeyword("DISABLE_TEMPERATURE_PARTICLES");
	}

	// Token: 0x06004BDC RID: 19420 RVA: 0x001B0680 File Offset: 0x001AE880
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		RenderTexture renderTexture = null;
		if (PropertyTextures.instance != null)
		{
			Texture texture = PropertyTextures.instance.GetTexture(PropertyTextures.Property.Temperature);
			texture.name = "temperature_tex";
			renderTexture = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(texture, renderTexture, this.blurMaterial);
			Shader.SetGlobalTexture("_BlurredTemperature", renderTexture);
		}
		this.material.SetTexture("_LightBufferTex", LightBuffer.Instance.Texture);
		Graphics.Blit(src, dest, this.material);
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}

	// Token: 0x06004BDD RID: 19421 RVA: 0x001B071C File Offset: 0x001AE91C
	private void OnShadersReloaded()
	{
		if (this.material != null && Lighting.Instance != null)
		{
			this.material.SetTexture("_EmberTex", Lighting.Instance.Settings.EmberTex);
			this.material.SetTexture("_FrostTex", Lighting.Instance.Settings.FrostTex);
			this.material.SetTexture("_Thermal1Tex", Lighting.Instance.Settings.Thermal1Tex);
			this.material.SetTexture("_Thermal2Tex", Lighting.Instance.Settings.Thermal2Tex);
			this.material.SetTexture("_RadHaze1Tex", Lighting.Instance.Settings.Radiation1Tex);
			this.material.SetTexture("_RadHaze2Tex", Lighting.Instance.Settings.Radiation2Tex);
			this.material.SetTexture("_RadHaze3Tex", Lighting.Instance.Settings.Radiation3Tex);
			this.material.SetTexture("_NoiseTex", Lighting.Instance.Settings.NoiseTex);
		}
	}

	// Token: 0x040031A7 RID: 12711
	[SerializeField]
	private Material material;

	// Token: 0x040031A8 RID: 12712
	[SerializeField]
	private Material blurMaterial;

	// Token: 0x040031A9 RID: 12713
	private bool particlesEnabled = true;
}

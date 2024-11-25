using System;
using UnityEngine;

// Token: 0x020008BB RID: 2235
[AddComponentMenu("KMonoBehaviour/scripts/FlowOffsetRenderer")]
public class FlowOffsetRenderer : KMonoBehaviour
{
	// Token: 0x06003EA1 RID: 16033 RVA: 0x0015AC54 File Offset: 0x00158E54
	protected override void OnSpawn()
	{
		this.FlowMaterial = new Material(Shader.Find("Klei/Flow"));
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		this.OnResize();
		this.DoUpdate(0.1f);
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x0015ACB0 File Offset: 0x00158EB0
	private void OnResize()
	{
		for (int i = 0; i < this.OffsetTextures.Length; i++)
		{
			if (this.OffsetTextures[i] != null)
			{
				this.OffsetTextures[i].DestroyRenderTexture();
			}
			this.OffsetTextures[i] = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
			this.OffsetTextures[i].filterMode = FilterMode.Bilinear;
			this.OffsetTextures[i].name = "FlowOffsetTexture";
		}
	}

	// Token: 0x06003EA3 RID: 16035 RVA: 0x0015AD2C File Offset: 0x00158F2C
	private void LateUpdate()
	{
		if ((Time.deltaTime > 0f && Time.timeScale > 0f) || this.forceUpdate)
		{
			float num = Time.deltaTime / Time.timeScale;
			this.DoUpdate(num * Time.timeScale / 4f + num * 0.5f);
		}
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x0015AD80 File Offset: 0x00158F80
	private void DoUpdate(float dt)
	{
		this.CurrentTime += dt;
		float num = this.CurrentTime * this.PhaseMultiplier;
		num -= (float)((int)num);
		float num2 = num - (float)((int)num);
		float y = 1f;
		if (num2 <= this.GasPhase0)
		{
			y = 0f;
		}
		this.GasPhase0 = num2;
		float z = 1f;
		float num3 = num + 0.5f - (float)((int)(num + 0.5f));
		if (num3 <= this.GasPhase1)
		{
			z = 0f;
		}
		this.GasPhase1 = num3;
		Shader.SetGlobalVector(this.ParametersName, new Vector4(this.GasPhase0, 0f, 0f, 0f));
		Shader.SetGlobalVector("_NoiseParameters", new Vector4(this.NoiseInfluence, this.NoiseScale, 0f, 0f));
		RenderTexture renderTexture = this.OffsetTextures[this.OffsetIdx];
		this.OffsetIdx = (this.OffsetIdx + 1) % 2;
		RenderTexture renderTexture2 = this.OffsetTextures[this.OffsetIdx];
		Material flowMaterial = this.FlowMaterial;
		flowMaterial.SetTexture("_PreviousOffsetTex", renderTexture);
		flowMaterial.SetVector("_FlowParameters", new Vector4(Time.deltaTime * this.OffsetSpeed, y, z, 0f));
		flowMaterial.SetVector("_MinFlow", new Vector4(this.MinFlow0.x, this.MinFlow0.y, this.MinFlow1.x, this.MinFlow1.y));
		flowMaterial.SetVector("_VisibleArea", new Vector4(0f, 0f, (float)Grid.WidthInCells, (float)Grid.HeightInCells));
		flowMaterial.SetVector("_LiquidGasMask", new Vector4(this.LiquidGasMask.x, this.LiquidGasMask.y, 0f, 0f));
		Graphics.Blit(renderTexture, renderTexture2, flowMaterial);
		Shader.SetGlobalTexture(this.OffsetTextureName, renderTexture2);
	}

	// Token: 0x04002681 RID: 9857
	private float GasPhase0;

	// Token: 0x04002682 RID: 9858
	private float GasPhase1;

	// Token: 0x04002683 RID: 9859
	public float PhaseMultiplier;

	// Token: 0x04002684 RID: 9860
	public float NoiseInfluence;

	// Token: 0x04002685 RID: 9861
	public float NoiseScale;

	// Token: 0x04002686 RID: 9862
	public float OffsetSpeed;

	// Token: 0x04002687 RID: 9863
	public string OffsetTextureName;

	// Token: 0x04002688 RID: 9864
	public string ParametersName;

	// Token: 0x04002689 RID: 9865
	public Vector2 MinFlow0;

	// Token: 0x0400268A RID: 9866
	public Vector2 MinFlow1;

	// Token: 0x0400268B RID: 9867
	public Vector2 LiquidGasMask;

	// Token: 0x0400268C RID: 9868
	[SerializeField]
	private Material FlowMaterial;

	// Token: 0x0400268D RID: 9869
	[SerializeField]
	private bool forceUpdate;

	// Token: 0x0400268E RID: 9870
	private TextureLerper FlowLerper;

	// Token: 0x0400268F RID: 9871
	public RenderTexture[] OffsetTextures = new RenderTexture[2];

	// Token: 0x04002690 RID: 9872
	private int OffsetIdx;

	// Token: 0x04002691 RID: 9873
	private float CurrentTime;
}

using System;
using UnityEngine;

// Token: 0x02000A3A RID: 2618
[ExecuteInEditMode]
public class Lighting : MonoBehaviour
{
	// Token: 0x06004BE7 RID: 19431 RVA: 0x001B0A4C File Offset: 0x001AEC4C
	private void Awake()
	{
		Lighting.Instance = this;
	}

	// Token: 0x06004BE8 RID: 19432 RVA: 0x001B0A54 File Offset: 0x001AEC54
	private void OnDestroy()
	{
		Lighting.Instance = null;
	}

	// Token: 0x06004BE9 RID: 19433 RVA: 0x001B0A5C File Offset: 0x001AEC5C
	private Color PremultiplyAlpha(Color c)
	{
		return c * c.a;
	}

	// Token: 0x06004BEA RID: 19434 RVA: 0x001B0A6A File Offset: 0x001AEC6A
	private void Start()
	{
		this.UpdateLighting();
	}

	// Token: 0x06004BEB RID: 19435 RVA: 0x001B0A72 File Offset: 0x001AEC72
	private void Update()
	{
		this.UpdateLighting();
	}

	// Token: 0x06004BEC RID: 19436 RVA: 0x001B0A7C File Offset: 0x001AEC7C
	private void UpdateLighting()
	{
		Shader.SetGlobalInt(Lighting._liquidZ, -28);
		Shader.SetGlobalVector(Lighting._DigMapMapParameters, new Vector4(this.Settings.DigMapColour.r, this.Settings.DigMapColour.g, this.Settings.DigMapColour.b, this.Settings.DigMapScale));
		Shader.SetGlobalTexture(Lighting._DigDamageMap, this.Settings.DigDamageMap);
		Shader.SetGlobalTexture(Lighting._StateTransitionMap, this.Settings.StateTransitionMap);
		Shader.SetGlobalColor(Lighting._StateTransitionColor, this.Settings.StateTransitionColor);
		Shader.SetGlobalVector(Lighting._StateTransitionParameters, new Vector4(1f / this.Settings.StateTransitionUVScale, this.Settings.StateTransitionUVOffsetRate.x, this.Settings.StateTransitionUVOffsetRate.y, 0f));
		Shader.SetGlobalTexture(Lighting._FallingSolidMap, this.Settings.FallingSolidMap);
		Shader.SetGlobalColor(Lighting._FallingSolidColor, this.Settings.FallingSolidColor);
		Shader.SetGlobalVector(Lighting._FallingSolidParameters, new Vector4(1f / this.Settings.FallingSolidUVScale, this.Settings.FallingSolidUVOffsetRate.x, this.Settings.FallingSolidUVOffsetRate.y, 0f));
		Shader.SetGlobalColor(Lighting._WaterTrimColor, this.Settings.WaterTrimColor);
		Shader.SetGlobalVector(Lighting._WaterParameters2, new Vector4(this.Settings.WaterTrimSize, this.Settings.WaterAlphaTrimSize, 0f, this.Settings.WaterAlphaThreshold));
		Shader.SetGlobalVector(Lighting._WaterWaveParameters, new Vector4(this.Settings.WaterWaveAmplitude, this.Settings.WaterWaveFrequency, this.Settings.WaterWaveSpeed, 0f));
		Shader.SetGlobalVector(Lighting._WaterWaveParameters2, new Vector4(this.Settings.WaterWaveAmplitude2, this.Settings.WaterWaveFrequency2, this.Settings.WaterWaveSpeed2, 0f));
		Shader.SetGlobalVector(Lighting._WaterDetailParameters, new Vector4(this.Settings.WaterCubeMapScale, this.Settings.WaterDetailTiling, this.Settings.WaterColorScale, this.Settings.WaterDetailTiling2));
		Shader.SetGlobalVector(Lighting._WaterDistortionParameters, new Vector4(this.Settings.WaterDistortionScaleStart, this.Settings.WaterDistortionScaleEnd, this.Settings.WaterDepthColorOpacityStart, this.Settings.WaterDepthColorOpacityEnd));
		Shader.SetGlobalVector(Lighting._BloomParameters, new Vector4(this.Settings.BloomScale, 0f, 0f, 0f));
		Shader.SetGlobalVector(Lighting._LiquidParameters2, new Vector4(this.Settings.LiquidMin, this.Settings.LiquidMax, this.Settings.LiquidCutoff, this.Settings.LiquidTransparency));
		Shader.SetGlobalVector(Lighting._GridParameters, new Vector4(this.Settings.GridLineWidth, this.Settings.GridSize, this.Settings.GridMinIntensity, this.Settings.GridMaxIntensity));
		Shader.SetGlobalColor(Lighting._GridColor, this.Settings.GridColor);
		Shader.SetGlobalVector(Lighting._EdgeGlowParameters, new Vector4(this.Settings.EdgeGlowCutoffStart, this.Settings.EdgeGlowCutoffEnd, this.Settings.EdgeGlowIntensity, 0f));
		if (this.disableLighting)
		{
			Shader.SetGlobalVector(Lighting._SubstanceParameters, Vector4.one);
			Shader.SetGlobalVector(Lighting._TileEdgeParameters, Vector4.one);
		}
		else
		{
			Shader.SetGlobalVector(Lighting._SubstanceParameters, new Vector4(this.Settings.substanceEdgeParameters.intensity, this.Settings.substanceEdgeParameters.edgeIntensity, this.Settings.substanceEdgeParameters.directSunlightScale, this.Settings.substanceEdgeParameters.power));
			Shader.SetGlobalVector(Lighting._TileEdgeParameters, new Vector4(this.Settings.tileEdgeParameters.intensity, this.Settings.tileEdgeParameters.edgeIntensity, this.Settings.tileEdgeParameters.directSunlightScale, this.Settings.tileEdgeParameters.power));
		}
		float w = (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() == OverlayModes.Disease.ID) ? 1f : 0f;
		if (this.disableLighting)
		{
			Shader.SetGlobalVector(Lighting._AnimParameters, new Vector4(1f, this.Settings.WorldZoneAnimBlend, 0f, w));
		}
		else
		{
			Shader.SetGlobalVector(Lighting._AnimParameters, new Vector4(this.Settings.AnimIntensity, this.Settings.WorldZoneAnimBlend, 0f, w));
		}
		Shader.SetGlobalVector(Lighting._GasOpacity, new Vector4(this.Settings.GasMinOpacity, this.Settings.GasMaxOpacity, 0f, 0f));
		Shader.SetGlobalColor(Lighting._DarkenTintBackground, this.Settings.DarkenTints[0]);
		Shader.SetGlobalColor(Lighting._DarkenTintMidground, this.Settings.DarkenTints[1]);
		Shader.SetGlobalColor(Lighting._DarkenTintForeground, this.Settings.DarkenTints[2]);
		Shader.SetGlobalColor(Lighting._BrightenOverlay, this.Settings.BrightenOverlayColour);
		Shader.SetGlobalColor(Lighting._ColdFG, this.PremultiplyAlpha(this.Settings.ColdColours[2]));
		Shader.SetGlobalColor(Lighting._ColdMG, this.PremultiplyAlpha(this.Settings.ColdColours[1]));
		Shader.SetGlobalColor(Lighting._ColdBG, this.PremultiplyAlpha(this.Settings.ColdColours[0]));
		Shader.SetGlobalColor(Lighting._HotFG, this.PremultiplyAlpha(this.Settings.HotColours[2]));
		Shader.SetGlobalColor(Lighting._HotMG, this.PremultiplyAlpha(this.Settings.HotColours[1]));
		Shader.SetGlobalColor(Lighting._HotBG, this.PremultiplyAlpha(this.Settings.HotColours[0]));
		Shader.SetGlobalVector(Lighting._TemperatureParallax, this.Settings.TemperatureParallax);
		Shader.SetGlobalVector(Lighting._ColdUVOffset1, new Vector4(this.Settings.ColdBGUVOffset.x, this.Settings.ColdBGUVOffset.y, this.Settings.ColdMGUVOffset.x, this.Settings.ColdMGUVOffset.y));
		Shader.SetGlobalVector(Lighting._ColdUVOffset2, new Vector4(this.Settings.ColdFGUVOffset.x, this.Settings.ColdFGUVOffset.y, 0f, 0f));
		Shader.SetGlobalVector(Lighting._HotUVOffset1, new Vector4(this.Settings.HotBGUVOffset.x, this.Settings.HotBGUVOffset.y, this.Settings.HotMGUVOffset.x, this.Settings.HotMGUVOffset.y));
		Shader.SetGlobalVector(Lighting._HotUVOffset2, new Vector4(this.Settings.HotFGUVOffset.x, this.Settings.HotFGUVOffset.y, 0f, 0f));
		Shader.SetGlobalColor(Lighting._DustColour, this.PremultiplyAlpha(this.Settings.DustColour));
		Shader.SetGlobalVector(Lighting._DustInfo, new Vector4(this.Settings.DustScale, this.Settings.DustMovement.x, this.Settings.DustMovement.y, this.Settings.DustMovement.z));
		Shader.SetGlobalTexture(Lighting._DustTex, this.Settings.DustTex);
		Shader.SetGlobalVector(Lighting._DebugShowInfo, new Vector4(this.Settings.ShowDust, this.Settings.ShowGas, this.Settings.ShowShadow, this.Settings.ShowTemperature));
		Shader.SetGlobalVector(Lighting._HeatHazeParameters, this.Settings.HeatHazeParameters);
		Shader.SetGlobalTexture(Lighting._HeatHazeTexture, this.Settings.HeatHazeTexture);
		Shader.SetGlobalVector(Lighting._ShineParams, new Vector4(this.Settings.ShineCenter.x, this.Settings.ShineCenter.y, this.Settings.ShineRange.x, this.Settings.ShineRange.y));
		Shader.SetGlobalVector(Lighting._ShineParams2, new Vector4(this.Settings.ShineZoomSpeed, 0f, 0f, 0f));
		Shader.SetGlobalFloat(Lighting._WorldZoneGasBlend, this.Settings.WorldZoneGasBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneLiquidBlend, this.Settings.WorldZoneLiquidBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneForegroundBlend, this.Settings.WorldZoneForegroundBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneSimpleAnimBlend, this.Settings.WorldZoneSimpleAnimBlend);
		Shader.SetGlobalColor(Lighting._CharacterLitColour, this.Settings.characterLighting.litColour);
		Shader.SetGlobalColor(Lighting._CharacterUnlitColour, this.Settings.characterLighting.unlitColour);
		Shader.SetGlobalTexture(Lighting._BuildingDamagedTex, this.Settings.BuildingDamagedTex);
		Shader.SetGlobalVector(Lighting._BuildingDamagedUVParameters, this.Settings.BuildingDamagedUVParameters);
		Shader.SetGlobalTexture(Lighting._DiseaseOverlayTex, this.Settings.DiseaseOverlayTex);
		Shader.SetGlobalVector(Lighting._DiseaseOverlayTexInfo, this.Settings.DiseaseOverlayTexInfo);
		if (this.Settings.ShowRadiation)
		{
			Shader.SetGlobalColor(Lighting._RadHazeColor, this.PremultiplyAlpha(this.Settings.RadColor));
		}
		else
		{
			Shader.SetGlobalColor(Lighting._RadHazeColor, Color.clear);
		}
		Shader.SetGlobalVector(Lighting._RadUVOffset1, new Vector4(this.Settings.Rad1UVOffset.x, this.Settings.Rad1UVOffset.y, this.Settings.Rad2UVOffset.x, this.Settings.Rad2UVOffset.y));
		Shader.SetGlobalVector(Lighting._RadUVOffset2, new Vector4(this.Settings.Rad3UVOffset.x, this.Settings.Rad3UVOffset.y, this.Settings.Rad4UVOffset.x, this.Settings.Rad4UVOffset.y));
		Shader.SetGlobalVector(Lighting._RadUVScales, new Vector4(1f / this.Settings.RadUVScales.x, 1f / this.Settings.RadUVScales.y, 1f / this.Settings.RadUVScales.z, 1f / this.Settings.RadUVScales.w));
		Shader.SetGlobalVector(Lighting._RadRange1, new Vector4(this.Settings.Rad1Range.x, this.Settings.Rad1Range.y, this.Settings.Rad2Range.x, this.Settings.Rad2Range.y));
		Shader.SetGlobalVector(Lighting._RadRange2, new Vector4(this.Settings.Rad3Range.x, this.Settings.Rad3Range.y, this.Settings.Rad4Range.x, this.Settings.Rad4Range.y));
		if (LightBuffer.Instance != null && LightBuffer.Instance.Texture != null)
		{
			Shader.SetGlobalTexture(Lighting._LightBufferTex, LightBuffer.Instance.Texture);
		}
	}

	// Token: 0x040031B1 RID: 12721
	public global::LightingSettings Settings;

	// Token: 0x040031B2 RID: 12722
	public static Lighting Instance;

	// Token: 0x040031B3 RID: 12723
	[NonSerialized]
	public bool disableLighting;

	// Token: 0x040031B4 RID: 12724
	private static int _liquidZ = Shader.PropertyToID("_LiquidZ");

	// Token: 0x040031B5 RID: 12725
	private static int _DigMapMapParameters = Shader.PropertyToID("_DigMapMapParameters");

	// Token: 0x040031B6 RID: 12726
	private static int _DigDamageMap = Shader.PropertyToID("_DigDamageMap");

	// Token: 0x040031B7 RID: 12727
	private static int _StateTransitionMap = Shader.PropertyToID("_StateTransitionMap");

	// Token: 0x040031B8 RID: 12728
	private static int _StateTransitionColor = Shader.PropertyToID("_StateTransitionColor");

	// Token: 0x040031B9 RID: 12729
	private static int _StateTransitionParameters = Shader.PropertyToID("_StateTransitionParameters");

	// Token: 0x040031BA RID: 12730
	private static int _FallingSolidMap = Shader.PropertyToID("_FallingSolidMap");

	// Token: 0x040031BB RID: 12731
	private static int _FallingSolidColor = Shader.PropertyToID("_FallingSolidColor");

	// Token: 0x040031BC RID: 12732
	private static int _FallingSolidParameters = Shader.PropertyToID("_FallingSolidParameters");

	// Token: 0x040031BD RID: 12733
	private static int _WaterTrimColor = Shader.PropertyToID("_WaterTrimColor");

	// Token: 0x040031BE RID: 12734
	private static int _WaterParameters2 = Shader.PropertyToID("_WaterParameters2");

	// Token: 0x040031BF RID: 12735
	private static int _WaterWaveParameters = Shader.PropertyToID("_WaterWaveParameters");

	// Token: 0x040031C0 RID: 12736
	private static int _WaterWaveParameters2 = Shader.PropertyToID("_WaterWaveParameters2");

	// Token: 0x040031C1 RID: 12737
	private static int _WaterDetailParameters = Shader.PropertyToID("_WaterDetailParameters");

	// Token: 0x040031C2 RID: 12738
	private static int _WaterDistortionParameters = Shader.PropertyToID("_WaterDistortionParameters");

	// Token: 0x040031C3 RID: 12739
	private static int _BloomParameters = Shader.PropertyToID("_BloomParameters");

	// Token: 0x040031C4 RID: 12740
	private static int _LiquidParameters2 = Shader.PropertyToID("_LiquidParameters2");

	// Token: 0x040031C5 RID: 12741
	private static int _GridParameters = Shader.PropertyToID("_GridParameters");

	// Token: 0x040031C6 RID: 12742
	private static int _GridColor = Shader.PropertyToID("_GridColor");

	// Token: 0x040031C7 RID: 12743
	private static int _EdgeGlowParameters = Shader.PropertyToID("_EdgeGlowParameters");

	// Token: 0x040031C8 RID: 12744
	private static int _SubstanceParameters = Shader.PropertyToID("_SubstanceParameters");

	// Token: 0x040031C9 RID: 12745
	private static int _TileEdgeParameters = Shader.PropertyToID("_TileEdgeParameters");

	// Token: 0x040031CA RID: 12746
	private static int _AnimParameters = Shader.PropertyToID("_AnimParameters");

	// Token: 0x040031CB RID: 12747
	private static int _GasOpacity = Shader.PropertyToID("_GasOpacity");

	// Token: 0x040031CC RID: 12748
	private static int _DarkenTintBackground = Shader.PropertyToID("_DarkenTintBackground");

	// Token: 0x040031CD RID: 12749
	private static int _DarkenTintMidground = Shader.PropertyToID("_DarkenTintMidground");

	// Token: 0x040031CE RID: 12750
	private static int _DarkenTintForeground = Shader.PropertyToID("_DarkenTintForeground");

	// Token: 0x040031CF RID: 12751
	private static int _BrightenOverlay = Shader.PropertyToID("_BrightenOverlay");

	// Token: 0x040031D0 RID: 12752
	private static int _ColdFG = Shader.PropertyToID("_ColdFG");

	// Token: 0x040031D1 RID: 12753
	private static int _ColdMG = Shader.PropertyToID("_ColdMG");

	// Token: 0x040031D2 RID: 12754
	private static int _ColdBG = Shader.PropertyToID("_ColdBG");

	// Token: 0x040031D3 RID: 12755
	private static int _HotFG = Shader.PropertyToID("_HotFG");

	// Token: 0x040031D4 RID: 12756
	private static int _HotMG = Shader.PropertyToID("_HotMG");

	// Token: 0x040031D5 RID: 12757
	private static int _HotBG = Shader.PropertyToID("_HotBG");

	// Token: 0x040031D6 RID: 12758
	private static int _TemperatureParallax = Shader.PropertyToID("_TemperatureParallax");

	// Token: 0x040031D7 RID: 12759
	private static int _ColdUVOffset1 = Shader.PropertyToID("_ColdUVOffset1");

	// Token: 0x040031D8 RID: 12760
	private static int _ColdUVOffset2 = Shader.PropertyToID("_ColdUVOffset2");

	// Token: 0x040031D9 RID: 12761
	private static int _HotUVOffset1 = Shader.PropertyToID("_HotUVOffset1");

	// Token: 0x040031DA RID: 12762
	private static int _HotUVOffset2 = Shader.PropertyToID("_HotUVOffset2");

	// Token: 0x040031DB RID: 12763
	private static int _DustColour = Shader.PropertyToID("_DustColour");

	// Token: 0x040031DC RID: 12764
	private static int _DustInfo = Shader.PropertyToID("_DustInfo");

	// Token: 0x040031DD RID: 12765
	private static int _DustTex = Shader.PropertyToID("_DustTex");

	// Token: 0x040031DE RID: 12766
	private static int _DebugShowInfo = Shader.PropertyToID("_DebugShowInfo");

	// Token: 0x040031DF RID: 12767
	private static int _HeatHazeParameters = Shader.PropertyToID("_HeatHazeParameters");

	// Token: 0x040031E0 RID: 12768
	private static int _HeatHazeTexture = Shader.PropertyToID("_HeatHazeTexture");

	// Token: 0x040031E1 RID: 12769
	private static int _ShineParams = Shader.PropertyToID("_ShineParams");

	// Token: 0x040031E2 RID: 12770
	private static int _ShineParams2 = Shader.PropertyToID("_ShineParams2");

	// Token: 0x040031E3 RID: 12771
	private static int _WorldZoneGasBlend = Shader.PropertyToID("_WorldZoneGasBlend");

	// Token: 0x040031E4 RID: 12772
	private static int _WorldZoneLiquidBlend = Shader.PropertyToID("_WorldZoneLiquidBlend");

	// Token: 0x040031E5 RID: 12773
	private static int _WorldZoneForegroundBlend = Shader.PropertyToID("_WorldZoneForegroundBlend");

	// Token: 0x040031E6 RID: 12774
	private static int _WorldZoneSimpleAnimBlend = Shader.PropertyToID("_WorldZoneSimpleAnimBlend");

	// Token: 0x040031E7 RID: 12775
	private static int _CharacterLitColour = Shader.PropertyToID("_CharacterLitColour");

	// Token: 0x040031E8 RID: 12776
	private static int _CharacterUnlitColour = Shader.PropertyToID("_CharacterUnlitColour");

	// Token: 0x040031E9 RID: 12777
	private static int _BuildingDamagedTex = Shader.PropertyToID("_BuildingDamagedTex");

	// Token: 0x040031EA RID: 12778
	private static int _BuildingDamagedUVParameters = Shader.PropertyToID("_BuildingDamagedUVParameters");

	// Token: 0x040031EB RID: 12779
	private static int _DiseaseOverlayTex = Shader.PropertyToID("_DiseaseOverlayTex");

	// Token: 0x040031EC RID: 12780
	private static int _DiseaseOverlayTexInfo = Shader.PropertyToID("_DiseaseOverlayTexInfo");

	// Token: 0x040031ED RID: 12781
	private static int _RadHazeColor = Shader.PropertyToID("_RadHazeColor");

	// Token: 0x040031EE RID: 12782
	private static int _RadUVOffset1 = Shader.PropertyToID("_RadUVOffset1");

	// Token: 0x040031EF RID: 12783
	private static int _RadUVOffset2 = Shader.PropertyToID("_RadUVOffset2");

	// Token: 0x040031F0 RID: 12784
	private static int _RadUVScales = Shader.PropertyToID("_RadUVScales");

	// Token: 0x040031F1 RID: 12785
	private static int _RadRange1 = Shader.PropertyToID("_RadRange1");

	// Token: 0x040031F2 RID: 12786
	private static int _RadRange2 = Shader.PropertyToID("_RadRange2");

	// Token: 0x040031F3 RID: 12787
	private static int _LightBufferTex = Shader.PropertyToID("_LightBufferTex");
}

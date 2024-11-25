using System;
using UnityEngine;

// Token: 0x02000A3C RID: 2620
public class LightingSettings : ScriptableObject
{
	// Token: 0x040031FB RID: 12795
	[Header("Global")]
	public bool UpdateLightSettings;

	// Token: 0x040031FC RID: 12796
	public float BloomScale;

	// Token: 0x040031FD RID: 12797
	public Color32 LightColour = Color.white;

	// Token: 0x040031FE RID: 12798
	[Header("Digging")]
	public float DigMapScale;

	// Token: 0x040031FF RID: 12799
	public Color DigMapColour;

	// Token: 0x04003200 RID: 12800
	public Texture2D DigDamageMap;

	// Token: 0x04003201 RID: 12801
	[Header("State Transition")]
	public Texture2D StateTransitionMap;

	// Token: 0x04003202 RID: 12802
	public Color StateTransitionColor;

	// Token: 0x04003203 RID: 12803
	public float StateTransitionUVScale;

	// Token: 0x04003204 RID: 12804
	public Vector2 StateTransitionUVOffsetRate;

	// Token: 0x04003205 RID: 12805
	[Header("Falling Solids")]
	public Texture2D FallingSolidMap;

	// Token: 0x04003206 RID: 12806
	public Color FallingSolidColor;

	// Token: 0x04003207 RID: 12807
	public float FallingSolidUVScale;

	// Token: 0x04003208 RID: 12808
	public Vector2 FallingSolidUVOffsetRate;

	// Token: 0x04003209 RID: 12809
	[Header("Metal Shine")]
	public Vector2 ShineCenter;

	// Token: 0x0400320A RID: 12810
	public Vector2 ShineRange;

	// Token: 0x0400320B RID: 12811
	public float ShineZoomSpeed;

	// Token: 0x0400320C RID: 12812
	[Header("Water")]
	public Color WaterTrimColor;

	// Token: 0x0400320D RID: 12813
	public float WaterTrimSize;

	// Token: 0x0400320E RID: 12814
	public float WaterAlphaTrimSize;

	// Token: 0x0400320F RID: 12815
	public float WaterAlphaThreshold;

	// Token: 0x04003210 RID: 12816
	public float WaterCubesAlphaThreshold;

	// Token: 0x04003211 RID: 12817
	public float WaterWaveAmplitude;

	// Token: 0x04003212 RID: 12818
	public float WaterWaveFrequency;

	// Token: 0x04003213 RID: 12819
	public float WaterWaveSpeed;

	// Token: 0x04003214 RID: 12820
	public float WaterDetailSpeed;

	// Token: 0x04003215 RID: 12821
	public float WaterDetailTiling;

	// Token: 0x04003216 RID: 12822
	public float WaterDetailTiling2;

	// Token: 0x04003217 RID: 12823
	public Vector2 WaterDetailDirection;

	// Token: 0x04003218 RID: 12824
	public float WaterWaveAmplitude2;

	// Token: 0x04003219 RID: 12825
	public float WaterWaveFrequency2;

	// Token: 0x0400321A RID: 12826
	public float WaterWaveSpeed2;

	// Token: 0x0400321B RID: 12827
	public float WaterCubeMapScale;

	// Token: 0x0400321C RID: 12828
	public float WaterColorScale;

	// Token: 0x0400321D RID: 12829
	public float WaterDistortionScaleStart;

	// Token: 0x0400321E RID: 12830
	public float WaterDistortionScaleEnd;

	// Token: 0x0400321F RID: 12831
	public float WaterDepthColorOpacityStart;

	// Token: 0x04003220 RID: 12832
	public float WaterDepthColorOpacityEnd;

	// Token: 0x04003221 RID: 12833
	[Header("Liquid")]
	public float LiquidMin;

	// Token: 0x04003222 RID: 12834
	public float LiquidMax;

	// Token: 0x04003223 RID: 12835
	public float LiquidCutoff;

	// Token: 0x04003224 RID: 12836
	public float LiquidTransparency;

	// Token: 0x04003225 RID: 12837
	public float LiquidAmountOffset;

	// Token: 0x04003226 RID: 12838
	public float LiquidMaxMass;

	// Token: 0x04003227 RID: 12839
	[Header("Grid")]
	public float GridLineWidth;

	// Token: 0x04003228 RID: 12840
	public float GridSize;

	// Token: 0x04003229 RID: 12841
	public float GridMaxIntensity;

	// Token: 0x0400322A RID: 12842
	public float GridMinIntensity;

	// Token: 0x0400322B RID: 12843
	public Color GridColor;

	// Token: 0x0400322C RID: 12844
	[Header("Terrain")]
	public float EdgeGlowCutoffStart;

	// Token: 0x0400322D RID: 12845
	public float EdgeGlowCutoffEnd;

	// Token: 0x0400322E RID: 12846
	public float EdgeGlowIntensity;

	// Token: 0x0400322F RID: 12847
	public int BackgroundLayers;

	// Token: 0x04003230 RID: 12848
	public float BackgroundBaseParallax;

	// Token: 0x04003231 RID: 12849
	public float BackgroundLayerParallax;

	// Token: 0x04003232 RID: 12850
	public float BackgroundDarkening;

	// Token: 0x04003233 RID: 12851
	public float BackgroundClip;

	// Token: 0x04003234 RID: 12852
	public float BackgroundUVScale;

	// Token: 0x04003235 RID: 12853
	public global::LightingSettings.EdgeLighting substanceEdgeParameters;

	// Token: 0x04003236 RID: 12854
	public global::LightingSettings.EdgeLighting tileEdgeParameters;

	// Token: 0x04003237 RID: 12855
	public float AnimIntensity;

	// Token: 0x04003238 RID: 12856
	public float GasMinOpacity;

	// Token: 0x04003239 RID: 12857
	public float GasMaxOpacity;

	// Token: 0x0400323A RID: 12858
	public Color[] DarkenTints;

	// Token: 0x0400323B RID: 12859
	public global::LightingSettings.LightingColours characterLighting;

	// Token: 0x0400323C RID: 12860
	public Color BrightenOverlayColour;

	// Token: 0x0400323D RID: 12861
	public Color[] ColdColours;

	// Token: 0x0400323E RID: 12862
	public Color[] HotColours;

	// Token: 0x0400323F RID: 12863
	[Header("Temperature Overlay Effects")]
	public Vector4 TemperatureParallax;

	// Token: 0x04003240 RID: 12864
	public Texture2D EmberTex;

	// Token: 0x04003241 RID: 12865
	public Texture2D FrostTex;

	// Token: 0x04003242 RID: 12866
	public Texture2D Thermal1Tex;

	// Token: 0x04003243 RID: 12867
	public Texture2D Thermal2Tex;

	// Token: 0x04003244 RID: 12868
	public Vector2 ColdFGUVOffset;

	// Token: 0x04003245 RID: 12869
	public Vector2 ColdMGUVOffset;

	// Token: 0x04003246 RID: 12870
	public Vector2 ColdBGUVOffset;

	// Token: 0x04003247 RID: 12871
	public Vector2 HotFGUVOffset;

	// Token: 0x04003248 RID: 12872
	public Vector2 HotMGUVOffset;

	// Token: 0x04003249 RID: 12873
	public Vector2 HotBGUVOffset;

	// Token: 0x0400324A RID: 12874
	public Texture2D DustTex;

	// Token: 0x0400324B RID: 12875
	public Color DustColour;

	// Token: 0x0400324C RID: 12876
	public float DustScale;

	// Token: 0x0400324D RID: 12877
	public Vector3 DustMovement;

	// Token: 0x0400324E RID: 12878
	public float ShowGas;

	// Token: 0x0400324F RID: 12879
	public float ShowTemperature;

	// Token: 0x04003250 RID: 12880
	public float ShowDust;

	// Token: 0x04003251 RID: 12881
	public float ShowShadow;

	// Token: 0x04003252 RID: 12882
	public Vector4 HeatHazeParameters;

	// Token: 0x04003253 RID: 12883
	public Texture2D HeatHazeTexture;

	// Token: 0x04003254 RID: 12884
	[Header("Biome")]
	public float WorldZoneGasBlend;

	// Token: 0x04003255 RID: 12885
	public float WorldZoneLiquidBlend;

	// Token: 0x04003256 RID: 12886
	public float WorldZoneForegroundBlend;

	// Token: 0x04003257 RID: 12887
	public float WorldZoneSimpleAnimBlend;

	// Token: 0x04003258 RID: 12888
	public float WorldZoneAnimBlend;

	// Token: 0x04003259 RID: 12889
	[Header("FX")]
	public Color32 SmokeDamageTint;

	// Token: 0x0400325A RID: 12890
	[Header("Building Damage")]
	public Texture2D BuildingDamagedTex;

	// Token: 0x0400325B RID: 12891
	public Vector4 BuildingDamagedUVParameters;

	// Token: 0x0400325C RID: 12892
	[Header("Disease")]
	public Texture2D DiseaseOverlayTex;

	// Token: 0x0400325D RID: 12893
	public Vector4 DiseaseOverlayTexInfo;

	// Token: 0x0400325E RID: 12894
	[Header("Conduits")]
	public ConduitFlowVisualizer.Tuning GasConduit;

	// Token: 0x0400325F RID: 12895
	public ConduitFlowVisualizer.Tuning LiquidConduit;

	// Token: 0x04003260 RID: 12896
	public SolidConduitFlowVisualizer.Tuning SolidConduit;

	// Token: 0x04003261 RID: 12897
	[Header("Radiation Overlay")]
	public bool ShowRadiation;

	// Token: 0x04003262 RID: 12898
	public Texture2D Radiation1Tex;

	// Token: 0x04003263 RID: 12899
	public Texture2D Radiation2Tex;

	// Token: 0x04003264 RID: 12900
	public Texture2D Radiation3Tex;

	// Token: 0x04003265 RID: 12901
	public Texture2D Radiation4Tex;

	// Token: 0x04003266 RID: 12902
	public Texture2D NoiseTex;

	// Token: 0x04003267 RID: 12903
	public Color RadColor;

	// Token: 0x04003268 RID: 12904
	public Vector2 Rad1UVOffset;

	// Token: 0x04003269 RID: 12905
	public Vector2 Rad2UVOffset;

	// Token: 0x0400326A RID: 12906
	public Vector2 Rad3UVOffset;

	// Token: 0x0400326B RID: 12907
	public Vector2 Rad4UVOffset;

	// Token: 0x0400326C RID: 12908
	public Vector4 RadUVScales;

	// Token: 0x0400326D RID: 12909
	public Vector2 Rad1Range;

	// Token: 0x0400326E RID: 12910
	public Vector2 Rad2Range;

	// Token: 0x0400326F RID: 12911
	public Vector2 Rad3Range;

	// Token: 0x04003270 RID: 12912
	public Vector2 Rad4Range;

	// Token: 0x02001A4A RID: 6730
	[Serializable]
	public struct EdgeLighting
	{
		// Token: 0x04007BEA RID: 31722
		public float intensity;

		// Token: 0x04007BEB RID: 31723
		public float edgeIntensity;

		// Token: 0x04007BEC RID: 31724
		public float directSunlightScale;

		// Token: 0x04007BED RID: 31725
		public float power;
	}

	// Token: 0x02001A4B RID: 6731
	public enum TintLayers
	{
		// Token: 0x04007BEF RID: 31727
		Background,
		// Token: 0x04007BF0 RID: 31728
		Midground,
		// Token: 0x04007BF1 RID: 31729
		Foreground,
		// Token: 0x04007BF2 RID: 31730
		NumLayers
	}

	// Token: 0x02001A4C RID: 6732
	[Serializable]
	public struct LightingColours
	{
		// Token: 0x04007BF3 RID: 31731
		public Color32 litColour;

		// Token: 0x04007BF4 RID: 31732
		public Color32 unlitColour;
	}
}

using System;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class KBatchedAnimHeatPostProcessingEffect : KMonoBehaviour
{
	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06001BDA RID: 7130 RVA: 0x00091FF4 File Offset: 0x000901F4
	public float HeatProduction
	{
		get
		{
			return this.heatProduction;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06001BDB RID: 7131 RVA: 0x00091FFC File Offset: 0x000901FC
	public bool IsHeatProductionEnoughToShowEffect
	{
		get
		{
			return this.HeatProduction >= 1f;
		}
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x0009200E File Offset: 0x0009020E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.animController.postProcessingEffectsAllowed |= KAnimConverter.PostProcessingEffects.TemperatureOverlay;
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x00092029 File Offset: 0x00090229
	public void SetHeatBeingProducedValue(float heat)
	{
		this.heatProduction = heat;
		this.RefreshEffectVisualState();
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x00092038 File Offset: 0x00090238
	public void RefreshEffectVisualState()
	{
		if (base.enabled && this.IsHeatProductionEnoughToShowEffect)
		{
			this.SetParameterValue(1f);
			return;
		}
		this.SetParameterValue(0f);
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x00092061 File Offset: 0x00090261
	private void SetParameterValue(float value)
	{
		if (this.animController != null)
		{
			this.animController.postProcessingParameters = value;
		}
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x0009207D File Offset: 0x0009027D
	protected override void OnCmpEnable()
	{
		this.RefreshEffectVisualState();
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x00092085 File Offset: 0x00090285
	protected override void OnCmpDisable()
	{
		this.RefreshEffectVisualState();
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x00092090 File Offset: 0x00090290
	private void Update()
	{
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad / 1f);
		if (num != this.loopsPlayed)
		{
			this.loopsPlayed = num;
			this.OnNewLoopReached();
		}
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x000920C4 File Offset: 0x000902C4
	private void OnNewLoopReached()
	{
		if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Temperature.ID && this.IsHeatProductionEnoughToShowEffect)
		{
			Vector3 position = base.transform.GetPosition();
			string sound = GlobalAssets.GetSound("Temperature_Heat_Emission", false);
			position.z = 0f;
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, position, 1f, false));
		}
	}

	// Token: 0x04000FAF RID: 4015
	public const float SHOW_EFFECT_HEAT_TRESHOLD = 1f;

	// Token: 0x04000FB0 RID: 4016
	private const float DISABLING_VALUE = 0f;

	// Token: 0x04000FB1 RID: 4017
	private const float ENABLING_VALUE = 1f;

	// Token: 0x04000FB2 RID: 4018
	private float heatProduction;

	// Token: 0x04000FB3 RID: 4019
	public const float ANIM_DURATION = 1f;

	// Token: 0x04000FB4 RID: 4020
	private int loopsPlayed;

	// Token: 0x04000FB5 RID: 4021
	[MyCmpGet]
	private KBatchedAnimController animController;
}

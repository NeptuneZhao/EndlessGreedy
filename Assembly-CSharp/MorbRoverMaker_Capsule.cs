using System;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class MorbRoverMaker_Capsule : KMonoBehaviour
{
	// Token: 0x06001041 RID: 4161 RVA: 0x0005BF2C File Offset: 0x0005A12C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.MorbDevelopment_Meter = new MeterController(this.buildingAnimCtr, "meter_morb_target", "meter_morb", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.GermMeter = new MeterController(this.buildingAnimCtr, "meter_germs_target", "meter_germs", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.MorbDevelopment_Capsule_Meter = new MeterController(this.buildingAnimCtr, "meter_capsule_target", "meter_capsule", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.MorbDevelopment_Capsule_Meter.meterController.onAnimComplete += this.OnGermAddedAnimationComplete;
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0005BFC4 File Offset: 0x0005A1C4
	private void OnGermAddedAnimationComplete(HashedString animName)
	{
		if (animName == MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			this.MorbDevelopment_Capsule_Meter.meterController.Play("meter_capsule", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x0005BFF8 File Offset: 0x0005A1F8
	public void PlayPumpGermsAnimation()
	{
		if (this.MorbDevelopment_Capsule_Meter.meterController.currentAnim != MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			this.MorbDevelopment_Capsule_Meter.meterController.Play(MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0005C038 File Offset: 0x0005A238
	public void SetMorbDevelopmentProgress(float morbDevelopmentProgress)
	{
		global::Debug.Assert(true, "MORB PHASES COUNT needs to be larger than 0");
		string s = "meter_morb_" + (1 + Mathf.FloorToInt(morbDevelopmentProgress * 4f)).ToString();
		if (this.MorbDevelopment_Meter.meterController.currentAnim != s)
		{
			this.MorbDevelopment_Meter.meterController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x0005C0AF File Offset: 0x0005A2AF
	public void SetGermMeterProgress(float progress)
	{
		this.GermMeter.SetPositionPercent(progress);
	}

	// Token: 0x040009E4 RID: 2532
	public const byte MORB_PHASES_COUNT = 5;

	// Token: 0x040009E5 RID: 2533
	public const byte MORB_FIRST_PHASE_INDEX = 1;

	// Token: 0x040009E6 RID: 2534
	private const string GERM_METER_TARGET_NAME = "meter_germs_target";

	// Token: 0x040009E7 RID: 2535
	private const string GERM_METER_ANIMATION_NAME = "meter_germs";

	// Token: 0x040009E8 RID: 2536
	private const string MORB_METER_TARGET_NAME = "meter_morb_target";

	// Token: 0x040009E9 RID: 2537
	private const string MORB_METER_ANIMATION_NAME = "meter_morb";

	// Token: 0x040009EA RID: 2538
	private const string MORB_CAPSULE_METER_TARGET_NAME = "meter_capsule_target";

	// Token: 0x040009EB RID: 2539
	private const string MORB_CAPSULE_METER_ANIMATION_NAME = "meter_capsule";

	// Token: 0x040009EC RID: 2540
	private static HashedString MORB_CAPSULE_METER_PUMP_ANIM_NAME = new HashedString("germ_pump");

	// Token: 0x040009ED RID: 2541
	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	// Token: 0x040009EE RID: 2542
	private MeterController MorbDevelopment_Meter;

	// Token: 0x040009EF RID: 2543
	private MeterController MorbDevelopment_Capsule_Meter;

	// Token: 0x040009F0 RID: 2544
	private MeterController GermMeter;
}

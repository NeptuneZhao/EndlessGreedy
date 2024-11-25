using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006AA RID: 1706
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitDiseaseSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x06002ADB RID: 10971 RVA: 0x000F1558 File Offset: 0x000EF758
	protected override void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int num;
				int num2;
				bool flag;
				this.GetContentsDisease(out num, out num2, out flag);
				Color32 c = Color.white;
				if (num != 255)
				{
					Disease disease = Db.Get().Diseases[num];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(ConduitDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x000F1618 File Offset: 0x000EF818
	private void GetContentsDisease(out int diseaseIdx, out int diseaseCount, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			diseaseIdx = (int)contents.diseaseIdx;
			diseaseCount = contents.diseaseCount;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			diseaseIdx = (int)pickupable.PrimaryElement.DiseaseIdx;
			diseaseCount = pickupable.PrimaryElement.DiseaseCount;
			hasMass = true;
			return;
		}
		diseaseIdx = 0;
		diseaseCount = 0;
		hasMass = false;
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06002ADD RID: 10973 RVA: 0x000F16CC File Offset: 0x000EF8CC
	public override float CurrentValue
	{
		get
		{
			int num;
			int num2;
			bool flag;
			this.GetContentsDisease(out num, out num2, out flag);
			if (flag)
			{
				this.lastValue = (float)num2;
			}
			return this.lastValue;
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06002ADE RID: 10974 RVA: 0x000F16F6 File Offset: 0x000EF8F6
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000F16FD File Offset: 0x000EF8FD
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x000F1704 File Offset: 0x000EF904
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x000F170B File Offset: 0x000EF90B
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x000F1712 File Offset: 0x000EF912
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x000F1719 File Offset: 0x000EF919
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_DISEASE;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x000F1720 File Offset: 0x000EF920
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x000F172C File Offset: 0x000EF92C
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x000F1738 File Offset: 0x000EF938
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x000F1743 File Offset: 0x000EF943
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x000F1746 File Offset: 0x000EF946
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000F1749 File Offset: 0x000EF949
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06002AEA RID: 10986 RVA: 0x000F1750 File Offset: 0x000EF950
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06002AEB RID: 10987 RVA: 0x000F1753 File Offset: 0x000EF953
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06002AEC RID: 10988 RVA: 0x000F1756 File Offset: 0x000EF956
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x040018AA RID: 6314
	private const float rangeMin = 0f;

	// Token: 0x040018AB RID: 6315
	private const float rangeMax = 100000f;

	// Token: 0x040018AC RID: 6316
	[Serialize]
	private float lastValue;

	// Token: 0x040018AD RID: 6317
	private static readonly HashedString TINT_SYMBOL = "germs";
}

using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D78 RID: 3448
public class LimitValveSideScreen : SideScreenContent
{
	// Token: 0x06006C72 RID: 27762 RVA: 0x0028CCF0 File Offset: 0x0028AEF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetButton.onClick += this.ResetCounter;
		this.limitSlider.onReleaseHandle += this.OnReleaseHandle;
		this.limitSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
		};
		this.limitSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
		};
		this.limitSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
			this.OnReleaseHandle();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 3;
	}

	// Token: 0x06006C73 RID: 27763 RVA: 0x0028CD99 File Offset: 0x0028AF99
	public void OnReleaseHandle()
	{
		this.targetLimitValve.Limit = this.targetLimit;
	}

	// Token: 0x06006C74 RID: 27764 RVA: 0x0028CDAC File Offset: 0x0028AFAC
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LimitValve>() != null;
	}

	// Token: 0x06006C75 RID: 27765 RVA: 0x0028CDBC File Offset: 0x0028AFBC
	public override void SetTarget(GameObject target)
	{
		this.targetLimitValve = target.GetComponent<LimitValve>();
		if (this.targetLimitValve == null)
		{
			global::Debug.LogError("The target object does not have a LimitValve component.");
			return;
		}
		if (this.targetLimitValveSubHandle != -1)
		{
			base.Unsubscribe(this.targetLimitValveSubHandle);
		}
		this.targetLimitValveSubHandle = this.targetLimitValve.Subscribe(-1722241721, new Action<object>(this.UpdateAmountLabel));
		this.limitSlider.minValue = 0f;
		this.limitSlider.maxValue = 100f;
		this.limitSlider.SetRanges(this.targetLimitValve.GetRanges());
		this.limitSlider.value = this.limitSlider.GetPercentageFromValue(this.targetLimitValve.Limit);
		this.numberInput.minValue = 0f;
		this.numberInput.maxValue = this.targetLimitValve.maxLimitKg;
		this.numberInput.Activate();
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			this.minLimitLabel.text = GameUtil.GetFormattedUnits(0f, GameUtil.TimeSlice.None, true, "");
			this.maxLimitLabel.text = GameUtil.GetFormattedUnits(this.targetLimitValve.maxLimitKg, GameUtil.TimeSlice.None, true, "");
			this.numberInput.SetDisplayValue(GameUtil.GetFormattedUnits(Mathf.Max(0f, this.targetLimitValve.Limit), GameUtil.TimeSlice.None, false, LimitValveSideScreen.FLOAT_FORMAT));
			this.unitsLabel.text = UI.UNITSUFFIXES.UNITS;
			this.toolTip.enabled = true;
			this.toolTip.SetSimpleTooltip(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.SLIDER_TOOLTIP_UNITS);
		}
		else
		{
			this.minLimitLabel.text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
			this.maxLimitLabel.text = GameUtil.GetFormattedMass(this.targetLimitValve.maxLimitKg, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
			this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, this.targetLimitValve.Limit), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, false, LimitValveSideScreen.FLOAT_FORMAT));
			this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
			this.toolTip.enabled = false;
		}
		this.UpdateAmountLabel(null);
	}

	// Token: 0x06006C76 RID: 27766 RVA: 0x0028CFF8 File Offset: 0x0028B1F8
	private void UpdateAmountLabel(object obj = null)
	{
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			string formattedUnits = GameUtil.GetFormattedUnits(this.targetLimitValve.Amount, GameUtil.TimeSlice.None, true, LimitValveSideScreen.FLOAT_FORMAT);
			this.amountLabel.text = string.Format(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.AMOUNT, formattedUnits);
			return;
		}
		string formattedMass = GameUtil.GetFormattedMass(this.targetLimitValve.Amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, LimitValveSideScreen.FLOAT_FORMAT);
		this.amountLabel.text = string.Format(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.AMOUNT, formattedMass);
	}

	// Token: 0x06006C77 RID: 27767 RVA: 0x0028D07A File Offset: 0x0028B27A
	private void ResetCounter()
	{
		this.targetLimitValve.ResetAmount();
	}

	// Token: 0x06006C78 RID: 27768 RVA: 0x0028D088 File Offset: 0x0028B288
	private void ReceiveValueFromSlider(float sliderPercentage)
	{
		float num = this.limitSlider.GetValueForPercentage(sliderPercentage);
		num = (float)Mathf.RoundToInt(num);
		this.UpdateLimitValue(num);
	}

	// Token: 0x06006C79 RID: 27769 RVA: 0x0028D0B1 File Offset: 0x0028B2B1
	private void ReceiveValueFromInput(float input)
	{
		this.UpdateLimitValue(input);
		this.targetLimitValve.Limit = this.targetLimit;
	}

	// Token: 0x06006C7A RID: 27770 RVA: 0x0028D0CC File Offset: 0x0028B2CC
	private void UpdateLimitValue(float newValue)
	{
		this.targetLimit = newValue;
		this.limitSlider.value = this.limitSlider.GetPercentageFromValue(newValue);
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			this.numberInput.SetDisplayValue(GameUtil.GetFormattedUnits(newValue, GameUtil.TimeSlice.None, false, LimitValveSideScreen.FLOAT_FORMAT));
			return;
		}
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, false, LimitValveSideScreen.FLOAT_FORMAT));
	}

	// Token: 0x040049F8 RID: 18936
	public static readonly string FLOAT_FORMAT = "{0:0.#####}";

	// Token: 0x040049F9 RID: 18937
	private LimitValve targetLimitValve;

	// Token: 0x040049FA RID: 18938
	[Header("State")]
	[SerializeField]
	private LocText amountLabel;

	// Token: 0x040049FB RID: 18939
	[SerializeField]
	private KButton resetButton;

	// Token: 0x040049FC RID: 18940
	[Header("Slider")]
	[SerializeField]
	private NonLinearSlider limitSlider;

	// Token: 0x040049FD RID: 18941
	[SerializeField]
	private LocText minLimitLabel;

	// Token: 0x040049FE RID: 18942
	[SerializeField]
	private LocText maxLimitLabel;

	// Token: 0x040049FF RID: 18943
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04004A00 RID: 18944
	[Header("Input Field")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04004A01 RID: 18945
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04004A02 RID: 18946
	private float targetLimit;

	// Token: 0x04004A03 RID: 18947
	private int targetLimitValveSubHandle = -1;
}

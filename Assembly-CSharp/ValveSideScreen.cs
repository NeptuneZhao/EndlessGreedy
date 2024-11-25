using System;
using System.Collections;
using STRINGS;
using UnityEngine;

// Token: 0x02000DBA RID: 3514
public class ValveSideScreen : SideScreenContent
{
	// Token: 0x06006F50 RID: 28496 RVA: 0x0029C720 File Offset: 0x0029A920
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.AddTimeSliceText(UI.UNITSUFFIXES.MASS.GRAM, GameUtil.TimeSlice.PerSecond);
		this.flowSlider.onReleaseHandle += this.OnReleaseHandle;
		this.flowSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
			this.OnReleaseHandle();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x06006F51 RID: 28497 RVA: 0x0029C7CD File Offset: 0x0029A9CD
	public void OnReleaseHandle()
	{
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x06006F52 RID: 28498 RVA: 0x0029C7E0 File Offset: 0x0029A9E0
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Valve>() != null;
	}

	// Token: 0x06006F53 RID: 28499 RVA: 0x0029C7F0 File Offset: 0x0029A9F0
	public override void SetTarget(GameObject target)
	{
		this.targetValve = target.GetComponent<Valve>();
		if (this.targetValve == null)
		{
			global::Debug.LogError("The target object does not have a Valve component.");
			return;
		}
		this.flowSlider.minValue = 0f;
		this.flowSlider.maxValue = this.targetValve.MaxFlow;
		this.flowSlider.value = this.targetValve.DesiredFlow;
		this.minFlowLabel.text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.maxFlowLabel.text = GameUtil.GetFormattedMass(this.targetValve.MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.numberInput.minValue = 0f;
		this.numberInput.maxValue = this.targetValve.MaxFlow * 1000f;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, this.targetValve.DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
		this.numberInput.Activate();
	}

	// Token: 0x06006F54 RID: 28500 RVA: 0x0029C902 File Offset: 0x0029AB02
	private void ReceiveValueFromSlider(float newValue)
	{
		newValue = Mathf.Round(newValue * 1000f) / 1000f;
		this.UpdateFlowValue(newValue);
	}

	// Token: 0x06006F55 RID: 28501 RVA: 0x0029C920 File Offset: 0x0029AB20
	private void ReceiveValueFromInput(float input)
	{
		float newValue = input / 1000f;
		this.UpdateFlowValue(newValue);
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x06006F56 RID: 28502 RVA: 0x0029C94D File Offset: 0x0029AB4D
	private void UpdateFlowValue(float newValue)
	{
		this.targetFlow = newValue;
		this.flowSlider.value = newValue;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
	}

	// Token: 0x06006F57 RID: 28503 RVA: 0x0029C97B File Offset: 0x0029AB7B
	private IEnumerator SettingDelay(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OnReleaseHandle();
		yield break;
	}

	// Token: 0x04004BEC RID: 19436
	private Valve targetValve;

	// Token: 0x04004BED RID: 19437
	[Header("Slider")]
	[SerializeField]
	private KSlider flowSlider;

	// Token: 0x04004BEE RID: 19438
	[SerializeField]
	private LocText minFlowLabel;

	// Token: 0x04004BEF RID: 19439
	[SerializeField]
	private LocText maxFlowLabel;

	// Token: 0x04004BF0 RID: 19440
	[Header("Input Field")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04004BF1 RID: 19441
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04004BF2 RID: 19442
	private float targetFlow;
}

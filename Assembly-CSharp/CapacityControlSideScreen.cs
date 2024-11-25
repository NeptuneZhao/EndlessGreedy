using System;
using UnityEngine;

// Token: 0x02000D52 RID: 3410
public class CapacityControlSideScreen : SideScreenContent
{
	// Token: 0x06006B53 RID: 27475 RVA: 0x00286240 File Offset: 0x00284440
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = this.target.CapacityUnits;
		this.slider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x06006B54 RID: 27476 RVA: 0x002862D6 File Offset: 0x002844D6
	public override bool IsValidForTarget(GameObject target)
	{
		return !target.GetComponent<IUserControlledCapacity>().IsNullOrDestroyed() || target.GetSMI<IUserControlledCapacity>() != null;
	}

	// Token: 0x06006B55 RID: 27477 RVA: 0x002862F0 File Offset: 0x002844F0
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IUserControlledCapacity>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IUserControlledCapacity>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IThresholdSwitch component");
			return;
		}
		this.slider.minValue = this.target.MinCapacity;
		this.slider.maxValue = this.target.MaxCapacity;
		this.slider.value = this.target.UserMaxCapacity;
		this.slider.GetComponentInChildren<ToolTip>();
		this.unitsLabel.text = this.target.CapacityUnits;
		this.numberInput.minValue = this.target.MinCapacity;
		this.numberInput.maxValue = this.target.MaxCapacity;
		this.numberInput.currentValue = Mathf.Max(this.target.MinCapacity, Mathf.Min(this.target.MaxCapacity, this.target.UserMaxCapacity));
		this.numberInput.Activate();
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x06006B56 RID: 27478 RVA: 0x00286420 File Offset: 0x00284620
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06006B57 RID: 27479 RVA: 0x00286429 File Offset: 0x00284629
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06006B58 RID: 27480 RVA: 0x00286432 File Offset: 0x00284632
	private void UpdateMaxCapacity(float newValue)
	{
		this.target.UserMaxCapacity = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x06006B59 RID: 27481 RVA: 0x00286454 File Offset: 0x00284654
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.target.UserMaxCapacity.ToString());
	}

	// Token: 0x0400492D RID: 18733
	private IUserControlledCapacity target;

	// Token: 0x0400492E RID: 18734
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x0400492F RID: 18735
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04004930 RID: 18736
	[SerializeField]
	private LocText unitsLabel;
}

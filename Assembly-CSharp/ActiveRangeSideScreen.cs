using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000D43 RID: 3395
public class ActiveRangeSideScreen : SideScreenContent
{
	// Token: 0x06006ABD RID: 27325 RVA: 0x00282EFB File Offset: 0x002810FB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006ABE RID: 27326 RVA: 0x00282F04 File Offset: 0x00281104
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateValueLabel.maxValue = this.target.MaxValue;
		this.activateValueLabel.minValue = this.target.MinValue;
		this.deactivateValueLabel.maxValue = this.target.MaxValue;
		this.deactivateValueLabel.minValue = this.target.MinValue;
		this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
	}

	// Token: 0x06006ABF RID: 27327 RVA: 0x00282FA8 File Offset: 0x002811A8
	private void OnActivateValueChanged(float new_value)
	{
		this.target.ActivateValue = new_value;
		if (this.target.ActivateValue < this.target.DeactivateValue)
		{
			this.target.ActivateValue = this.target.DeactivateValue;
			this.activateValueSlider.value = this.target.ActivateValue;
		}
		this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
		this.RefreshTooltips();
	}

	// Token: 0x06006AC0 RID: 27328 RVA: 0x0028302C File Offset: 0x0028122C
	private void OnDeactivateValueChanged(float new_value)
	{
		this.target.DeactivateValue = new_value;
		if (this.target.DeactivateValue > this.target.ActivateValue)
		{
			this.target.DeactivateValue = this.activateValueSlider.value;
			this.deactivateValueSlider.value = this.target.DeactivateValue;
		}
		this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
		this.RefreshTooltips();
	}

	// Token: 0x06006AC1 RID: 27329 RVA: 0x002830B0 File Offset: 0x002812B0
	private void RefreshTooltips()
	{
		this.activateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.ActivateTooltip, this.activateValueSlider.value, this.deactivateValueSlider.value));
		this.deactivateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.DeactivateTooltip, this.deactivateValueSlider.value, this.activateValueSlider.value));
	}

	// Token: 0x06006AC2 RID: 27330 RVA: 0x0028313D File Offset: 0x0028133D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IActivationRangeTarget>() != null;
	}

	// Token: 0x06006AC3 RID: 27331 RVA: 0x00283148 File Offset: 0x00281348
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IActivationRangeTarget>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IActivationRangeTarget component");
			return;
		}
		this.activateLabel.text = this.target.ActivateSliderLabelText;
		this.deactivateLabel.text = this.target.DeactivateSliderLabelText;
		this.activateValueLabel.Activate();
		this.deactivateValueLabel.Activate();
		this.activateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.activateValueSlider.minValue = this.target.MinValue;
		this.activateValueSlider.maxValue = this.target.MaxValue;
		this.activateValueSlider.value = this.target.ActivateValue;
		this.activateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
		this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
		this.activateValueLabel.onEndEdit += delegate()
		{
			float activateValue = this.target.ActivateValue;
			float.TryParse(this.activateValueLabel.field.text, out activateValue);
			this.OnActivateValueChanged(activateValue);
			this.activateValueSlider.value = activateValue;
		};
		this.deactivateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnDeactivateValueChanged));
		this.deactivateValueSlider.minValue = this.target.MinValue;
		this.deactivateValueSlider.maxValue = this.target.MaxValue;
		this.deactivateValueSlider.value = this.target.DeactivateValue;
		this.deactivateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
		this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
		this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
		this.deactivateValueLabel.onEndEdit += delegate()
		{
			float deactivateValue = this.target.DeactivateValue;
			float.TryParse(this.deactivateValueLabel.field.text, out deactivateValue);
			this.OnDeactivateValueChanged(deactivateValue);
			this.deactivateValueSlider.value = deactivateValue;
		};
		this.RefreshTooltips();
	}

	// Token: 0x06006AC4 RID: 27332 RVA: 0x0028335A File Offset: 0x0028155A
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return this.target.ActivationRangeTitleText;
		}
		return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
	}

	// Token: 0x040048C0 RID: 18624
	private IActivationRangeTarget target;

	// Token: 0x040048C1 RID: 18625
	[SerializeField]
	private KSlider activateValueSlider;

	// Token: 0x040048C2 RID: 18626
	[SerializeField]
	private KSlider deactivateValueSlider;

	// Token: 0x040048C3 RID: 18627
	[SerializeField]
	private LocText activateLabel;

	// Token: 0x040048C4 RID: 18628
	[SerializeField]
	private LocText deactivateLabel;

	// Token: 0x040048C5 RID: 18629
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField activateValueLabel;

	// Token: 0x040048C6 RID: 18630
	[SerializeField]
	private KNumberInputField deactivateValueLabel;
}

using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D91 RID: 3473
public class RailGunSideScreen : SideScreenContent
{
	// Token: 0x06006D72 RID: 28018 RVA: 0x00292758 File Offset: 0x00290958
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
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

	// Token: 0x06006D73 RID: 28019 RVA: 0x002927E9 File Offset: 0x002909E9
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x06006D74 RID: 28020 RVA: 0x00292805 File Offset: 0x00290A05
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x06006D75 RID: 28021 RVA: 0x00292821 File Offset: 0x00290A21
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RailGun>() != null;
	}

	// Token: 0x06006D76 RID: 28022 RVA: 0x00292830 File Offset: 0x00290A30
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.selectedGun = new_target.GetComponent<RailGun>();
		if (this.selectedGun == null)
		{
			global::Debug.LogError("The gameObject received does not contain a RailGun component");
			return;
		}
		this.targetRailgunHEPStorageSubHandle = this.selectedGun.Subscribe(-1837862626, new Action<object>(this.UpdateHEPLabels));
		this.slider.minValue = this.selectedGun.MinLaunchMass;
		this.slider.maxValue = this.selectedGun.MaxLaunchMass;
		this.slider.value = this.selectedGun.launchMass;
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
		this.numberInput.minValue = this.selectedGun.MinLaunchMass;
		this.numberInput.maxValue = this.selectedGun.MaxLaunchMass;
		this.numberInput.currentValue = Mathf.Max(this.selectedGun.MinLaunchMass, Mathf.Min(this.selectedGun.MaxLaunchMass, this.selectedGun.launchMass));
		this.UpdateMaxCapacityLabel();
		this.numberInput.Activate();
		this.UpdateHEPLabels(null);
	}

	// Token: 0x06006D77 RID: 28023 RVA: 0x0029296A File Offset: 0x00290B6A
	public override void ClearTarget()
	{
		if (this.targetRailgunHEPStorageSubHandle != -1 && this.selectedGun != null)
		{
			this.selectedGun.Unsubscribe(this.targetRailgunHEPStorageSubHandle);
			this.targetRailgunHEPStorageSubHandle = -1;
		}
		this.selectedGun = null;
	}

	// Token: 0x06006D78 RID: 28024 RVA: 0x002929A4 File Offset: 0x00290BA4
	public void UpdateHEPLabels(object data = null)
	{
		if (this.selectedGun == null)
		{
			return;
		}
		string text = BUILDINGS.PREFABS.RAILGUN.SIDESCREEN_HEP_REQUIRED;
		text = text.Replace("{current}", this.selectedGun.CurrentEnergy.ToString());
		text = text.Replace("{required}", this.selectedGun.EnergyCost.ToString());
		this.hepStorageInfo.text = text;
	}

	// Token: 0x06006D79 RID: 28025 RVA: 0x00292A15 File Offset: 0x00290C15
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06006D7A RID: 28026 RVA: 0x00292A1E File Offset: 0x00290C1E
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06006D7B RID: 28027 RVA: 0x00292A27 File Offset: 0x00290C27
	private void UpdateMaxCapacity(float newValue)
	{
		this.selectedGun.launchMass = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
		this.selectedGun.Trigger(161772031, null);
	}

	// Token: 0x06006D7C RID: 28028 RVA: 0x00292A58 File Offset: 0x00290C58
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.selectedGun.launchMass.ToString());
	}

	// Token: 0x04004AA4 RID: 19108
	public GameObject content;

	// Token: 0x04004AA5 RID: 19109
	private RailGun selectedGun;

	// Token: 0x04004AA6 RID: 19110
	public LocText DescriptionText;

	// Token: 0x04004AA7 RID: 19111
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x04004AA8 RID: 19112
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04004AA9 RID: 19113
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04004AAA RID: 19114
	[SerializeField]
	private LocText hepStorageInfo;

	// Token: 0x04004AAB RID: 19115
	private int targetRailgunHEPStorageSubHandle = -1;
}

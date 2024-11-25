using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D61 RID: 3425
public class CounterSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06006BDB RID: 27611 RVA: 0x002899C3 File Offset: 0x00287BC3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006BDC RID: 27612 RVA: 0x002899CC File Offset: 0x00287BCC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetButton.onClick += this.ResetCounter;
		this.incrementMaxButton.onClick += this.IncrementMaxCount;
		this.decrementMaxButton.onClick += this.DecrementMaxCount;
		this.incrementModeButton.onClick += this.ToggleMode;
		this.advancedModeToggle.onClick += this.ToggleAdvanced;
		this.maxCountInput.onEndEdit += delegate()
		{
			this.UpdateMaxCountFromTextInput(this.maxCountInput.currentValue);
		};
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x06006BDD RID: 27613 RVA: 0x00289A7A File Offset: 0x00287C7A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCounter>() != null;
	}

	// Token: 0x06006BDE RID: 27614 RVA: 0x00289A88 File Offset: 0x00287C88
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.maxCountInput.minValue = 1f;
		this.maxCountInput.maxValue = 10f;
		this.targetLogicCounter = target.GetComponent<LogicCounter>();
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateMaxCountLabel(this.targetLogicCounter.maxCount);
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
	}

	// Token: 0x06006BDF RID: 27615 RVA: 0x00289B00 File Offset: 0x00287D00
	public void Render200ms(float dt)
	{
		if (this.targetLogicCounter == null)
		{
			return;
		}
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x06006BE0 RID: 27616 RVA: 0x00289B24 File Offset: 0x00287D24
	private void UpdateCurrentCountLabel(int value)
	{
		string text = value.ToString();
		if (value == this.targetLogicCounter.maxCount)
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Active);
		}
		else
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Standby);
		}
		this.currentCount.text = (this.targetLogicCounter.advancedMode ? string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_ADVANCED, text) : string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_SIMPLE, text));
	}

	// Token: 0x06006BE1 RID: 27617 RVA: 0x00289B93 File Offset: 0x00287D93
	private void UpdateMaxCountLabel(int value)
	{
		this.maxCountInput.SetAmount((float)value);
	}

	// Token: 0x06006BE2 RID: 27618 RVA: 0x00289BA2 File Offset: 0x00287DA2
	private void UpdateMaxCountFromTextInput(float newValue)
	{
		this.SetMaxCount((int)newValue);
	}

	// Token: 0x06006BE3 RID: 27619 RVA: 0x00289BAC File Offset: 0x00287DAC
	private void IncrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount + 1);
	}

	// Token: 0x06006BE4 RID: 27620 RVA: 0x00289BC1 File Offset: 0x00287DC1
	private void DecrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount - 1);
	}

	// Token: 0x06006BE5 RID: 27621 RVA: 0x00289BD8 File Offset: 0x00287DD8
	private void SetMaxCount(int newValue)
	{
		if (newValue > 10)
		{
			newValue = 1;
		}
		if (newValue < 1)
		{
			newValue = 10;
		}
		if (newValue < this.targetLogicCounter.currentCount)
		{
			this.targetLogicCounter.currentCount = newValue;
		}
		this.targetLogicCounter.maxCount = newValue;
		this.UpdateCounterStates();
		this.UpdateMaxCountLabel(newValue);
	}

	// Token: 0x06006BE6 RID: 27622 RVA: 0x00289C28 File Offset: 0x00287E28
	private void ResetCounter()
	{
		this.targetLogicCounter.ResetCounter();
	}

	// Token: 0x06006BE7 RID: 27623 RVA: 0x00289C35 File Offset: 0x00287E35
	private void UpdateCounterStates()
	{
		this.targetLogicCounter.SetCounterState();
		this.targetLogicCounter.UpdateLogicCircuit();
		this.targetLogicCounter.UpdateVisualState(true);
		this.targetLogicCounter.UpdateMeter();
	}

	// Token: 0x06006BE8 RID: 27624 RVA: 0x00289C64 File Offset: 0x00287E64
	private void ToggleMode()
	{
	}

	// Token: 0x06006BE9 RID: 27625 RVA: 0x00289C68 File Offset: 0x00287E68
	private void ToggleAdvanced()
	{
		this.targetLogicCounter.advancedMode = !this.targetLogicCounter.advancedMode;
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateCounterStates();
	}

	// Token: 0x04004990 RID: 18832
	public LogicCounter targetLogicCounter;

	// Token: 0x04004991 RID: 18833
	public KButton resetButton;

	// Token: 0x04004992 RID: 18834
	public KButton incrementMaxButton;

	// Token: 0x04004993 RID: 18835
	public KButton decrementMaxButton;

	// Token: 0x04004994 RID: 18836
	public KButton incrementModeButton;

	// Token: 0x04004995 RID: 18837
	public KToggle advancedModeToggle;

	// Token: 0x04004996 RID: 18838
	public KImage advancedModeCheckmark;

	// Token: 0x04004997 RID: 18839
	public LocText currentCount;

	// Token: 0x04004998 RID: 18840
	[SerializeField]
	private KNumberInputField maxCountInput;
}

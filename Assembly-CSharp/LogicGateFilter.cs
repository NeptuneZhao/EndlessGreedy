using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200070D RID: 1805
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateFilter : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x00106C0D File Offset: 0x00104E0D
	// (set) Token: 0x06002ED6 RID: 11990 RVA: 0x00106C18 File Offset: 0x00104E18
	public float DelayAmount
	{
		get
		{
			return this.delayAmount;
		}
		set
		{
			this.delayAmount = value;
			int delayAmountTicks = this.DelayAmountTicks;
			if (this.delayTicksRemaining > delayAmountTicks)
			{
				this.delayTicksRemaining = delayAmountTicks;
			}
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06002ED7 RID: 11991 RVA: 0x00106C43 File Offset: 0x00104E43
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x00106C56 File Offset: 0x00104E56
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06002ED9 RID: 11993 RVA: 0x00106C5D File Offset: 0x00104E5D
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x00106C69 File Offset: 0x00104E69
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x06002EDB RID: 11995 RVA: 0x00106C6C File Offset: 0x00104E6C
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x06002EDC RID: 11996 RVA: 0x00106C73 File Offset: 0x00104E73
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x06002EDD RID: 11997 RVA: 0x00106C7A File Offset: 0x00104E7A
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x06002EDE RID: 11998 RVA: 0x00106C82 File Offset: 0x00104E82
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x06002EDF RID: 11999 RVA: 0x00106C8B File Offset: 0x00104E8B
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x00106C92 File Offset: 0x00104E92
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x00106CB3 File Offset: 0x00104EB3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateFilter>(-905833192, LogicGateFilter.OnCopySettingsDelegate);
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x00106CCC File Offset: 0x00104ECC
	private void OnCopySettings(object data)
	{
		LogicGateFilter component = ((GameObject)data).GetComponent<LogicGateFilter>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x00106CFC File Offset: 0x00104EFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(0f);
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x00106D48 File Offset: 0x00104F48
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_negative)
		{
			positionPercent = 0f;
		}
		else if (this.delayTicksRemaining > 0)
		{
			positionPercent = (float)(this.DelayAmountTicks - this.delayTicksRemaining) / (float)this.DelayAmountTicks;
		}
		else
		{
			positionPercent = 1f;
		}
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06002EE5 RID: 12005 RVA: 0x00106D9F File Offset: 0x00104F9F
	public override void LogicTick()
	{
		if (!this.input_was_previously_negative && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x00106DD0 File Offset: 0x00104FD0
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 == 0)
		{
			this.input_was_previously_negative = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(1f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_negative)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_negative = false;
		}
		if (val1 != 0 && this.delayTicksRemaining <= 0)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x00106E34 File Offset: 0x00105034
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(0f);
		if (this.outputValueOne == 1)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 1;
		base.RefreshAnimation();
	}

	// Token: 0x04001B9A RID: 7066
	[Serialize]
	private bool input_was_previously_negative;

	// Token: 0x04001B9B RID: 7067
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x04001B9C RID: 7068
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x04001B9D RID: 7069
	private MeterController meter;

	// Token: 0x04001B9E RID: 7070
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001B9F RID: 7071
	private static readonly EventSystem.IntraObjectHandler<LogicGateFilter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateFilter>(delegate(LogicGateFilter component, object data)
	{
		component.OnCopySettings(data);
	});
}

using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200070C RID: 1804
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateBuffer : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x00106957 File Offset: 0x00104B57
	// (set) Token: 0x06002EC1 RID: 11969 RVA: 0x00106960 File Offset: 0x00104B60
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

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x0010698B File Offset: 0x00104B8B
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x0010699E File Offset: 0x00104B9E
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x001069A5 File Offset: 0x00104BA5
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x001069B1 File Offset: 0x00104BB1
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x06002EC6 RID: 11974 RVA: 0x001069B4 File Offset: 0x00104BB4
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x06002EC7 RID: 11975 RVA: 0x001069BB File Offset: 0x00104BBB
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x001069C2 File Offset: 0x00104BC2
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x06002EC9 RID: 11977 RVA: 0x001069CA File Offset: 0x00104BCA
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x06002ECA RID: 11978 RVA: 0x001069D3 File Offset: 0x00104BD3
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x001069DA File Offset: 0x00104BDA
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x001069FB File Offset: 0x00104BFB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateBuffer>(-905833192, LogicGateBuffer.OnCopySettingsDelegate);
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x00106A14 File Offset: 0x00104C14
	private void OnCopySettings(object data)
	{
		LogicGateBuffer component = ((GameObject)data).GetComponent<LogicGateBuffer>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x06002ECE RID: 11982 RVA: 0x00106A44 File Offset: 0x00104C44
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(1f);
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x00106A90 File Offset: 0x00104C90
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_positive)
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

	// Token: 0x06002ED0 RID: 11984 RVA: 0x00106AE7 File Offset: 0x00104CE7
	public override void LogicTick()
	{
		if (!this.input_was_previously_positive && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x00106B18 File Offset: 0x00104D18
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 != 0)
		{
			this.input_was_previously_positive = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(0f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_positive)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_positive = false;
		}
		if (val1 == 0 && this.delayTicksRemaining <= 0)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x00106B7C File Offset: 0x00104D7C
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(1f);
		if (this.outputValueOne == 0)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 0;
		base.RefreshAnimation();
	}

	// Token: 0x04001B94 RID: 7060
	[Serialize]
	private bool input_was_previously_positive;

	// Token: 0x04001B95 RID: 7061
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x04001B96 RID: 7062
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x04001B97 RID: 7063
	private MeterController meter;

	// Token: 0x04001B98 RID: 7064
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001B99 RID: 7065
	private static readonly EventSystem.IntraObjectHandler<LogicGateBuffer> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateBuffer>(delegate(LogicGateBuffer component, object data)
	{
		component.OnCopySettings(data);
	});
}

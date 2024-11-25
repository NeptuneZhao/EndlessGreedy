using System;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000685 RID: 1669
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
public class BatterySmart : Battery, IActivationRangeTarget
{
	// Token: 0x06002979 RID: 10617 RVA: 0x000EA3DB File Offset: 0x000E85DB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<BatterySmart>(-905833192, BatterySmart.OnCopySettingsDelegate);
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x000EA3F4 File Offset: 0x000E85F4
	private void OnCopySettings(object data)
	{
		BatterySmart component = ((GameObject)data).GetComponent<BatterySmart>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x000EA42E File Offset: 0x000E862E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CreateLogicMeter();
		base.Subscribe<BatterySmart>(-801688580, BatterySmart.OnLogicValueChangedDelegate);
		base.Subscribe<BatterySmart>(-592767678, BatterySmart.UpdateLogicCircuitDelegate);
	}

	// Token: 0x0600297C RID: 10620 RVA: 0x000EA45E File Offset: 0x000E865E
	private void CreateLogicMeter()
	{
		this.logicMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x0600297D RID: 10621 RVA: 0x000EA483 File Offset: 0x000E8683
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x000EA494 File Offset: 0x000E8694
	private void UpdateLogicCircuit(object data)
	{
		float num = (float)Mathf.RoundToInt(base.PercentFull * 100f);
		if (this.activated)
		{
			if (num >= (float)this.deactivateValue)
			{
				this.activated = false;
			}
		}
		else if (num <= (float)this.activateValue)
		{
			this.activated = true;
		}
		bool isOperational = this.operational.IsOperational;
		bool flag = this.activated && isOperational;
		this.logicPorts.SendSignal(BatterySmart.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x0600297F RID: 10623 RVA: 0x000EA50C File Offset: 0x000E870C
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == BatterySmart.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x06002980 RID: 10624 RVA: 0x000EA544 File Offset: 0x000E8744
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06002981 RID: 10625 RVA: 0x000EA568 File Offset: 0x000E8768
	// (set) Token: 0x06002982 RID: 10626 RVA: 0x000EA571 File Offset: 0x000E8771
	public float ActivateValue
	{
		get
		{
			return (float)this.deactivateValue;
		}
		set
		{
			this.deactivateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06002983 RID: 10627 RVA: 0x000EA582 File Offset: 0x000E8782
	// (set) Token: 0x06002984 RID: 10628 RVA: 0x000EA58B File Offset: 0x000E878B
	public float DeactivateValue
	{
		get
		{
			return (float)this.activateValue;
		}
		set
		{
			this.activateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06002985 RID: 10629 RVA: 0x000EA59C File Offset: 0x000E879C
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06002986 RID: 10630 RVA: 0x000EA5A3 File Offset: 0x000E87A3
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06002987 RID: 10631 RVA: 0x000EA5AA File Offset: 0x000E87AA
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06002988 RID: 10632 RVA: 0x000EA5AD File Offset: 0x000E87AD
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06002989 RID: 10633 RVA: 0x000EA5B9 File Offset: 0x000E87B9
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x0600298A RID: 10634 RVA: 0x000EA5C5 File Offset: 0x000E87C5
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x0600298B RID: 10635 RVA: 0x000EA5D1 File Offset: 0x000E87D1
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x0600298C RID: 10636 RVA: 0x000EA5DD File Offset: 0x000E87DD
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x040017ED RID: 6125
	public static readonly HashedString PORT_ID = "BatterySmartLogicPort";

	// Token: 0x040017EE RID: 6126
	[Serialize]
	private int activateValue;

	// Token: 0x040017EF RID: 6127
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x040017F0 RID: 6128
	[Serialize]
	private bool activated;

	// Token: 0x040017F1 RID: 6129
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x040017F2 RID: 6130
	private MeterController logicMeter;

	// Token: 0x040017F3 RID: 6131
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040017F4 RID: 6132
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040017F5 RID: 6133
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040017F6 RID: 6134
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}

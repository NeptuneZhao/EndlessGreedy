using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AA3 RID: 2723
[AddComponentMenu("KMonoBehaviour/scripts/SmartReservoir")]
public class SmartReservoir : KMonoBehaviour, IActivationRangeTarget, ISim200ms
{
	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06005019 RID: 20505 RVA: 0x001CCDE2 File Offset: 0x001CAFE2
	public float PercentFull
	{
		get
		{
			return this.storage.MassStored() / this.storage.Capacity();
		}
	}

	// Token: 0x0600501A RID: 20506 RVA: 0x001CCDFB File Offset: 0x001CAFFB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SmartReservoir>(-801688580, SmartReservoir.OnLogicValueChangedDelegate);
		base.Subscribe<SmartReservoir>(-592767678, SmartReservoir.UpdateLogicCircuitDelegate);
	}

	// Token: 0x0600501B RID: 20507 RVA: 0x001CCE25 File Offset: 0x001CB025
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SmartReservoir>(-905833192, SmartReservoir.OnCopySettingsDelegate);
	}

	// Token: 0x0600501C RID: 20508 RVA: 0x001CCE3E File Offset: 0x001CB03E
	public void Sim200ms(float dt)
	{
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x0600501D RID: 20509 RVA: 0x001CCE48 File Offset: 0x001CB048
	private void UpdateLogicCircuit(object data)
	{
		float num = this.PercentFull * 100f;
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
		bool flag = this.activated;
		this.logicPorts.SendSignal(SmartReservoir.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x0600501E RID: 20510 RVA: 0x001CCEAC File Offset: 0x001CB0AC
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == SmartReservoir.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x0600501F RID: 20511 RVA: 0x001CCEE4 File Offset: 0x001CB0E4
	private void OnCopySettings(object data)
	{
		SmartReservoir component = ((GameObject)data).GetComponent<SmartReservoir>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06005020 RID: 20512 RVA: 0x001CCF1E File Offset: 0x001CB11E
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06005021 RID: 20513 RVA: 0x001CCF42 File Offset: 0x001CB142
	// (set) Token: 0x06005022 RID: 20514 RVA: 0x001CCF4B File Offset: 0x001CB14B
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

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06005023 RID: 20515 RVA: 0x001CCF5C File Offset: 0x001CB15C
	// (set) Token: 0x06005024 RID: 20516 RVA: 0x001CCF65 File Offset: 0x001CB165
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

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x06005025 RID: 20517 RVA: 0x001CCF76 File Offset: 0x001CB176
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06005026 RID: 20518 RVA: 0x001CCF7D File Offset: 0x001CB17D
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06005027 RID: 20519 RVA: 0x001CCF84 File Offset: 0x001CB184
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06005028 RID: 20520 RVA: 0x001CCF87 File Offset: 0x001CB187
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06005029 RID: 20521 RVA: 0x001CCF93 File Offset: 0x001CB193
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x0600502A RID: 20522 RVA: 0x001CCF9F File Offset: 0x001CB19F
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x0600502B RID: 20523 RVA: 0x001CCFAB File Offset: 0x001CB1AB
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x0600502C RID: 20524 RVA: 0x001CCFB7 File Offset: 0x001CB1B7
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x04003539 RID: 13625
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400353A RID: 13626
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400353B RID: 13627
	[Serialize]
	private int activateValue;

	// Token: 0x0400353C RID: 13628
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x0400353D RID: 13629
	[Serialize]
	private bool activated;

	// Token: 0x0400353E RID: 13630
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x0400353F RID: 13631
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003540 RID: 13632
	private MeterController logicMeter;

	// Token: 0x04003541 RID: 13633
	public static readonly HashedString PORT_ID = "SmartReservoirLogicPort";

	// Token: 0x04003542 RID: 13634
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003543 RID: 13635
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04003544 RID: 13636
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}

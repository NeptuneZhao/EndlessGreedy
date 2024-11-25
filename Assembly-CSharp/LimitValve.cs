using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006FA RID: 1786
[SerializationConfig(MemberSerialization.OptIn)]
public class LimitValve : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06002D9F RID: 11679 RVA: 0x0010000C File Offset: 0x000FE20C
	public float RemainingCapacity
	{
		get
		{
			return Mathf.Max(0f, this.m_limit - this.m_amount);
		}
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x00100025 File Offset: 0x000FE225
	public NonLinearSlider.Range[] GetRanges()
	{
		if (this.sliderRanges != null && this.sliderRanges.Length != 0)
		{
			return this.sliderRanges;
		}
		return NonLinearSlider.GetDefaultRange(this.maxLimitKg);
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06002DA1 RID: 11681 RVA: 0x0010004A File Offset: 0x000FE24A
	// (set) Token: 0x06002DA2 RID: 11682 RVA: 0x00100052 File Offset: 0x000FE252
	public float Limit
	{
		get
		{
			return this.m_limit;
		}
		set
		{
			this.m_limit = value;
			this.Refresh();
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06002DA3 RID: 11683 RVA: 0x00100061 File Offset: 0x000FE261
	// (set) Token: 0x06002DA4 RID: 11684 RVA: 0x00100069 File Offset: 0x000FE269
	public float Amount
	{
		get
		{
			return this.m_amount;
		}
		set
		{
			this.m_amount = value;
			base.Trigger(-1722241721, this.Amount);
			this.Refresh();
		}
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x0010008E File Offset: 0x000FE28E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LimitValve>(-905833192, LimitValve.OnCopySettingsDelegate);
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x001000A8 File Offset: 0x000FE2A8
	protected override void OnSpawn()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.Subscribe<LimitValve>(-801688580, LimitValve.OnLogicValueChangedDelegate);
		if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
		{
			ConduitBridge conduitBridge = this.conduitBridge;
			conduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(conduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			ConduitBridge conduitBridge2 = this.conduitBridge;
			conduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(conduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		else if (this.conduitType == ConduitType.Solid)
		{
			SolidConduitBridge solidConduitBridge = this.solidConduitBridge;
			solidConduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(solidConduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			SolidConduitBridge solidConduitBridge2 = this.solidConduitBridge;
			solidConduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(solidConduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		if (this.limitMeter == null)
		{
			this.limitMeter = new MeterController(this.controller, "meter_target_counter", "meter_counter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_counter"
			});
		}
		this.Refresh();
		base.OnSpawn();
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x001001EA File Offset: 0x000FE3EA
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x0010021D File Offset: 0x000FE41D
	private void LogicTick()
	{
		if (this.m_resetRequested)
		{
			this.ResetAmount();
		}
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x0010022D File Offset: 0x000FE42D
	public void ResetAmount()
	{
		this.m_resetRequested = false;
		this.Amount = 0f;
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x00100244 File Offset: 0x000FE444
	private float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!this.operational.IsOperational)
		{
			return 0f;
		}
		if (this.conduitType == ConduitType.Solid && pickupable != null && GameTags.DisplayAsUnits.Contains(pickupable.KPrefabID.PrefabID()))
		{
			float num = pickupable.PrimaryElement.Units;
			if (this.RemainingCapacity < num)
			{
				num = (float)Mathf.FloorToInt(this.RemainingCapacity);
			}
			return num * pickupable.PrimaryElement.MassPerUnit;
		}
		return Mathf.Min(mass, this.RemainingCapacity);
	}

	// Token: 0x06002DAB RID: 11691 RVA: 0x001002D0 File Offset: 0x000FE4D0
	private void OnMassTransfer(SimHashes element, float transferredMass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!LogicCircuitNetwork.IsBitActive(0, this.ports.GetInputValue(LimitValve.RESET_PORT_ID)))
		{
			if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
			{
				this.Amount += transferredMass;
			}
			else if (this.conduitType == ConduitType.Solid && pickupable != null)
			{
				this.Amount += transferredMass / pickupable.PrimaryElement.MassPerUnit;
			}
		}
		this.operational.SetActive(this.operational.IsOperational && transferredMass > 0f, false);
		this.Refresh();
	}

	// Token: 0x06002DAC RID: 11692 RVA: 0x00100370 File Offset: 0x000FE570
	private void Refresh()
	{
		if (this.operational == null)
		{
			return;
		}
		this.ports.SendSignal(LimitValve.OUTPUT_PORT_ID, (this.RemainingCapacity <= 0f) ? 1 : 0);
		this.operational.SetFlag(LimitValve.limitNotReached, this.RemainingCapacity > 0f);
		if (this.RemainingCapacity > 0f)
		{
			this.limitMeter.meterController.Play("meter_counter", KAnim.PlayMode.Paused, 1f, 0f);
			this.limitMeter.SetPositionPercent(this.Amount / this.Limit);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitNotReached, this);
			return;
		}
		this.limitMeter.meterController.Play("meter_on", KAnim.PlayMode.Paused, 1f, 0f);
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitReached, this);
	}

	// Token: 0x06002DAD RID: 11693 RVA: 0x00100490 File Offset: 0x000FE690
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LimitValve.RESET_PORT_ID && LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue))
		{
			this.ResetAmount();
		}
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x001004CC File Offset: 0x000FE6CC
	private void OnCopySettings(object data)
	{
		LimitValve component = ((GameObject)data).GetComponent<LimitValve>();
		if (component != null)
		{
			this.Limit = component.Limit;
		}
	}

	// Token: 0x04001A83 RID: 6787
	public static readonly HashedString RESET_PORT_ID = new HashedString("LimitValveReset");

	// Token: 0x04001A84 RID: 6788
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LimitValveOutput");

	// Token: 0x04001A85 RID: 6789
	public static readonly Operational.Flag limitNotReached = new Operational.Flag("limitNotReached", Operational.Flag.Type.Requirement);

	// Token: 0x04001A86 RID: 6790
	public ConduitType conduitType;

	// Token: 0x04001A87 RID: 6791
	public float maxLimitKg = 100f;

	// Token: 0x04001A88 RID: 6792
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A89 RID: 6793
	[MyCmpReq]
	private LogicPorts ports;

	// Token: 0x04001A8A RID: 6794
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04001A8B RID: 6795
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001A8C RID: 6796
	[MyCmpGet]
	private ConduitBridge conduitBridge;

	// Token: 0x04001A8D RID: 6797
	[MyCmpGet]
	private SolidConduitBridge solidConduitBridge;

	// Token: 0x04001A8E RID: 6798
	[Serialize]
	[SerializeField]
	private float m_limit;

	// Token: 0x04001A8F RID: 6799
	[Serialize]
	private float m_amount;

	// Token: 0x04001A90 RID: 6800
	[Serialize]
	private bool m_resetRequested;

	// Token: 0x04001A91 RID: 6801
	private MeterController limitMeter;

	// Token: 0x04001A92 RID: 6802
	public bool displayUnitsInsteadOfMass;

	// Token: 0x04001A93 RID: 6803
	public NonLinearSlider.Range[] sliderRanges;

	// Token: 0x04001A94 RID: 6804
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001A95 RID: 6805
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001A96 RID: 6806
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnCopySettings(data);
	});
}

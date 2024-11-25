using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000709 RID: 1801
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGate : LogicGateBase, ILogicEventSender, ILogicNetworkConnection
{
	// Token: 0x06002E94 RID: 11924 RVA: 0x001043AC File Offset: 0x001025AC
	protected override void OnSpawn()
	{
		this.inputOne = new LogicEventHandler(base.InputCellOne, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		if (base.RequiresTwoInputs)
		{
			this.inputTwo = new LogicEventHandler(base.InputCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		}
		else if (base.RequiresFourInputs)
		{
			this.inputTwo = new LogicEventHandler(base.InputCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
			this.inputThree = new LogicEventHandler(base.InputCellThree, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
			this.inputFour = new LogicEventHandler(base.InputCellFour, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		}
		if (base.RequiresControlInputs)
		{
			this.controlOne = new LogicEventHandler(base.ControlCellOne, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.ControlInput);
			this.controlTwo = new LogicEventHandler(base.ControlCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.ControlInput);
		}
		if (base.RequiresFourOutputs)
		{
			this.outputTwo = new LogicPortVisualizer(base.OutputCellTwo, LogicPortSpriteType.Output);
			this.outputThree = new LogicPortVisualizer(base.OutputCellThree, LogicPortSpriteType.Output);
			this.outputFour = new LogicPortVisualizer(base.OutputCellFour, LogicPortSpriteType.Output);
			this.outputTwoSender = new LogicEventSender(LogicGateBase.OUTPUT_TWO_PORT_ID, base.OutputCellTwo, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_TWO_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
			this.outputThreeSender = new LogicEventSender(LogicGateBase.OUTPUT_THREE_PORT_ID, base.OutputCellThree, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_THREE_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
			this.outputFourSender = new LogicEventSender(LogicGateBase.OUTPUT_FOUR_PORT_ID, base.OutputCellFour, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_FOUR_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
		}
		base.Subscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate);
		base.Subscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate);
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || !component.IsBroken)
		{
			this.Connect();
		}
	}

	// Token: 0x06002E95 RID: 11925 RVA: 0x00104599 File Offset: 0x00102799
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.Disconnect();
		base.Unsubscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x001045D0 File Offset: 0x001027D0
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06002E97 RID: 11927 RVA: 0x001045D8 File Offset: 0x001027D8
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06002E98 RID: 11928 RVA: 0x001045E0 File Offset: 0x001027E0
	private void Connect()
	{
		if (!this.connected)
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
			this.connected = true;
			int outputCellOne = base.OutputCellOne;
			logicCircuitSystem.AddToNetworks(outputCellOne, this, true);
			this.outputOne = new LogicPortVisualizer(outputCellOne, LogicPortSpriteType.Output);
			logicCircuitManager.AddVisElem(this.outputOne);
			if (base.RequiresFourOutputs)
			{
				this.outputTwo = new LogicPortVisualizer(base.OutputCellTwo, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellTwo, this.outputTwoSender, true);
				logicCircuitManager.AddVisElem(this.outputTwo);
				this.outputThree = new LogicPortVisualizer(base.OutputCellThree, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellThree, this.outputThreeSender, true);
				logicCircuitManager.AddVisElem(this.outputThree);
				this.outputFour = new LogicPortVisualizer(base.OutputCellFour, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellFour, this.outputFourSender, true);
				logicCircuitManager.AddVisElem(this.outputFour);
			}
			int inputCellOne = base.InputCellOne;
			logicCircuitSystem.AddToNetworks(inputCellOne, this.inputOne, true);
			logicCircuitManager.AddVisElem(this.inputOne);
			if (base.RequiresTwoInputs)
			{
				int inputCellTwo = base.InputCellTwo;
				logicCircuitSystem.AddToNetworks(inputCellTwo, this.inputTwo, true);
				logicCircuitManager.AddVisElem(this.inputTwo);
			}
			else if (base.RequiresFourInputs)
			{
				logicCircuitSystem.AddToNetworks(base.InputCellTwo, this.inputTwo, true);
				logicCircuitManager.AddVisElem(this.inputTwo);
				logicCircuitSystem.AddToNetworks(base.InputCellThree, this.inputThree, true);
				logicCircuitManager.AddVisElem(this.inputThree);
				logicCircuitSystem.AddToNetworks(base.InputCellFour, this.inputFour, true);
				logicCircuitManager.AddVisElem(this.inputFour);
			}
			if (base.RequiresControlInputs)
			{
				logicCircuitSystem.AddToNetworks(base.ControlCellOne, this.controlOne, true);
				logicCircuitManager.AddVisElem(this.controlOne);
				logicCircuitSystem.AddToNetworks(base.ControlCellTwo, this.controlTwo, true);
				logicCircuitManager.AddVisElem(this.controlTwo);
			}
			this.RefreshAnimation();
		}
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x001047DC File Offset: 0x001029DC
	private void Disconnect()
	{
		if (this.connected)
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
			this.connected = false;
			int outputCellOne = base.OutputCellOne;
			logicCircuitSystem.RemoveFromNetworks(outputCellOne, this, true);
			logicCircuitManager.RemoveVisElem(this.outputOne);
			this.outputOne = null;
			if (base.RequiresFourOutputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellTwo, this.outputTwoSender, true);
				logicCircuitManager.RemoveVisElem(this.outputTwo);
				this.outputTwo = null;
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellThree, this.outputThreeSender, true);
				logicCircuitManager.RemoveVisElem(this.outputThree);
				this.outputThree = null;
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellFour, this.outputFourSender, true);
				logicCircuitManager.RemoveVisElem(this.outputFour);
				this.outputFour = null;
			}
			int inputCellOne = base.InputCellOne;
			logicCircuitSystem.RemoveFromNetworks(inputCellOne, this.inputOne, true);
			logicCircuitManager.RemoveVisElem(this.inputOne);
			this.inputOne = null;
			if (base.RequiresTwoInputs)
			{
				int inputCellTwo = base.InputCellTwo;
				logicCircuitSystem.RemoveFromNetworks(inputCellTwo, this.inputTwo, true);
				logicCircuitManager.RemoveVisElem(this.inputTwo);
				this.inputTwo = null;
			}
			else if (base.RequiresFourInputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.InputCellTwo, this.inputTwo, true);
				logicCircuitManager.RemoveVisElem(this.inputTwo);
				this.inputTwo = null;
				logicCircuitSystem.RemoveFromNetworks(base.InputCellThree, this.inputThree, true);
				logicCircuitManager.RemoveVisElem(this.inputThree);
				this.inputThree = null;
				logicCircuitSystem.RemoveFromNetworks(base.InputCellFour, this.inputFour, true);
				logicCircuitManager.RemoveVisElem(this.inputFour);
				this.inputFour = null;
			}
			if (base.RequiresControlInputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.ControlCellOne, this.controlOne, true);
				logicCircuitManager.RemoveVisElem(this.controlOne);
				this.controlOne = null;
				logicCircuitSystem.RemoveFromNetworks(base.ControlCellTwo, this.controlTwo, true);
				logicCircuitManager.RemoveVisElem(this.controlTwo);
				this.controlTwo = null;
			}
			this.RefreshAnimation();
		}
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x001049E0 File Offset: 0x00102BE0
	private void UpdateState(int new_value, int prev_value)
	{
		if (this.cleaningUp)
		{
			return;
		}
		int value = this.inputOne.Value;
		int num = (this.inputTwo != null) ? this.inputTwo.Value : 0;
		int num2 = (this.inputThree != null) ? this.inputThree.Value : 0;
		int num3 = (this.inputFour != null) ? this.inputFour.Value : 0;
		int value2 = (this.controlOne != null) ? this.controlOne.Value : 0;
		int value3 = (this.controlTwo != null) ? this.controlTwo.Value : 0;
		if (base.RequiresFourInputs && base.RequiresControlInputs)
		{
			this.outputValueOne = 0;
			if (this.op == LogicGateBase.Op.Multiplexer)
			{
				if (!LogicCircuitNetwork.IsBitActive(0, value3))
				{
					if (!LogicCircuitNetwork.IsBitActive(0, value2))
					{
						this.outputValueOne = value;
					}
					else
					{
						this.outputValueOne = num;
					}
				}
				else if (!LogicCircuitNetwork.IsBitActive(0, value2))
				{
					this.outputValueOne = num2;
				}
				else
				{
					this.outputValueOne = num3;
				}
			}
		}
		if (base.RequiresFourOutputs && base.RequiresControlInputs)
		{
			this.outputValueOne = 0;
			this.outputValueTwo = 0;
			this.outputTwoSender.SetValue(0);
			this.outputValueThree = 0;
			this.outputThreeSender.SetValue(0);
			this.outputValueFour = 0;
			this.outputFourSender.SetValue(0);
			if (this.op == LogicGateBase.Op.Demultiplexer)
			{
				if (!LogicCircuitNetwork.IsBitActive(0, value2))
				{
					if (!LogicCircuitNetwork.IsBitActive(0, value3))
					{
						this.outputValueOne = value;
					}
					else
					{
						this.outputValueTwo = value;
						this.outputTwoSender.SetValue(value);
					}
				}
				else if (!LogicCircuitNetwork.IsBitActive(0, value3))
				{
					this.outputValueThree = value;
					this.outputThreeSender.SetValue(value);
				}
				else
				{
					this.outputValueFour = value;
					this.outputFourSender.SetValue(value);
				}
			}
		}
		switch (this.op)
		{
		case LogicGateBase.Op.And:
			this.outputValueOne = (value & num);
			break;
		case LogicGateBase.Op.Or:
			this.outputValueOne = (value | num);
			break;
		case LogicGateBase.Op.Not:
		{
			LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
			int inputCellOne = base.InputCellOne;
			GameObject gameObject = Grid.Objects[inputCellOne, 31];
			if (gameObject != null)
			{
				LogicWire component = gameObject.GetComponent<LogicWire>();
				if (component != null)
				{
					bitDepth = component.MaxBitDepth;
				}
			}
			if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
			{
				uint num4 = (uint)value;
				num4 = ~num4;
				num4 &= 15U;
				this.outputValueOne = (int)num4;
			}
			else
			{
				this.outputValueOne = ((value == 0) ? 1 : 0);
			}
			break;
		}
		case LogicGateBase.Op.Xor:
			this.outputValueOne = (value ^ num);
			break;
		case LogicGateBase.Op.CustomSingle:
			this.outputValueOne = this.GetCustomValue(value, num);
			break;
		}
		this.RefreshAnimation();
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x00104C74 File Offset: 0x00102E74
	private void OnAdditionalOutputsLogicValueChanged(HashedString port_id, int new_value, int prev_value)
	{
		if (base.gameObject != null)
		{
			base.gameObject.Trigger(-801688580, new LogicValueChanged
			{
				portID = port_id,
				newValue = new_value,
				prevValue = prev_value
			});
		}
	}

	// Token: 0x06002E9C RID: 11932 RVA: 0x00104CC5 File Offset: 0x00102EC5
	public virtual void LogicTick()
	{
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x00104CC7 File Offset: 0x00102EC7
	protected virtual int GetCustomValue(int val1, int val2)
	{
		return val1;
	}

	// Token: 0x06002E9E RID: 11934 RVA: 0x00104CCC File Offset: 0x00102ECC
	public int GetPortValue(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.inputOne.Value;
		case LogicGateBase.PortId.InputTwo:
			if (base.RequiresTwoInputs || base.RequiresFourInputs)
			{
				return this.inputTwo.Value;
			}
			return 0;
		case LogicGateBase.PortId.InputThree:
			if (!base.RequiresFourInputs)
			{
				return 0;
			}
			return this.inputThree.Value;
		case LogicGateBase.PortId.InputFour:
			if (!base.RequiresFourInputs)
			{
				return 0;
			}
			return this.inputFour.Value;
		case LogicGateBase.PortId.OutputOne:
			return this.outputValueOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.outputValueTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.outputValueThree;
		case LogicGateBase.PortId.OutputFour:
			return this.outputValueFour;
		case LogicGateBase.PortId.ControlOne:
			return this.controlOne.Value;
		case LogicGateBase.PortId.ControlTwo:
			return this.controlTwo.Value;
		default:
			return this.outputValueOne;
		}
	}

	// Token: 0x06002E9F RID: 11935 RVA: 0x00104D9C File Offset: 0x00102F9C
	public bool GetPortConnected(LogicGateBase.PortId port)
	{
		if ((port == LogicGateBase.PortId.InputTwo && !base.RequiresTwoInputs && !base.RequiresFourInputs) || (port == LogicGateBase.PortId.InputThree && !base.RequiresFourInputs) || (port == LogicGateBase.PortId.InputFour && !base.RequiresFourInputs))
		{
			return false;
		}
		int cell = base.PortCell(port);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(cell) != null;
	}

	// Token: 0x06002EA0 RID: 11936 RVA: 0x00104DF2 File Offset: 0x00102FF2
	public void SetPortDescriptions(LogicGate.LogicGateDescriptions descriptions)
	{
		this.descriptions = descriptions;
	}

	// Token: 0x06002EA1 RID: 11937 RVA: 0x00104DFC File Offset: 0x00102FFC
	public LogicGate.LogicGateDescriptions.Description GetPortDescription(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			if (this.descriptions.inputOne != null)
			{
				return this.descriptions.inputOne;
			}
			if (!base.RequiresTwoInputs && !base.RequiresFourInputs)
			{
				return LogicGate.INPUT_ONE_SINGLE_DESCRIPTION;
			}
			return LogicGate.INPUT_ONE_MULTI_DESCRIPTION;
		case LogicGateBase.PortId.InputTwo:
			if (this.descriptions.inputTwo == null)
			{
				return LogicGate.INPUT_TWO_DESCRIPTION;
			}
			return this.descriptions.inputTwo;
		case LogicGateBase.PortId.InputThree:
			if (this.descriptions.inputThree == null)
			{
				return LogicGate.INPUT_THREE_DESCRIPTION;
			}
			return this.descriptions.inputThree;
		case LogicGateBase.PortId.InputFour:
			if (this.descriptions.inputFour == null)
			{
				return LogicGate.INPUT_FOUR_DESCRIPTION;
			}
			return this.descriptions.inputFour;
		case LogicGateBase.PortId.OutputOne:
			if (this.descriptions.inputOne != null)
			{
				return this.descriptions.inputOne;
			}
			if (!base.RequiresFourOutputs)
			{
				return LogicGate.OUTPUT_ONE_SINGLE_DESCRIPTION;
			}
			return LogicGate.OUTPUT_ONE_MULTI_DESCRIPTION;
		case LogicGateBase.PortId.OutputTwo:
			if (this.descriptions.outputTwo == null)
			{
				return LogicGate.OUTPUT_TWO_DESCRIPTION;
			}
			return this.descriptions.outputTwo;
		case LogicGateBase.PortId.OutputThree:
			if (this.descriptions.outputThree == null)
			{
				return LogicGate.OUTPUT_THREE_DESCRIPTION;
			}
			return this.descriptions.outputThree;
		case LogicGateBase.PortId.OutputFour:
			if (this.descriptions.outputFour == null)
			{
				return LogicGate.OUTPUT_FOUR_DESCRIPTION;
			}
			return this.descriptions.outputFour;
		case LogicGateBase.PortId.ControlOne:
			if (this.descriptions.controlOne == null)
			{
				return LogicGate.CONTROL_ONE_DESCRIPTION;
			}
			return this.descriptions.controlOne;
		case LogicGateBase.PortId.ControlTwo:
			if (this.descriptions.controlTwo == null)
			{
				return LogicGate.CONTROL_TWO_DESCRIPTION;
			}
			return this.descriptions.controlTwo;
		default:
			return this.descriptions.outputOne;
		}
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x00104FA1 File Offset: 0x001031A1
	public int GetLogicValue()
	{
		return this.outputValueOne;
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x00104FA9 File Offset: 0x001031A9
	public int GetLogicCell()
	{
		return this.GetLogicUICell();
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x00104FB1 File Offset: 0x001031B1
	public int GetLogicUICell()
	{
		return base.OutputCellOne;
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x00104FB9 File Offset: 0x001031B9
	public bool IsLogicInput()
	{
		return false;
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x00104FBC File Offset: 0x001031BC
	private LogicEventHandler GetInputFromControlValue(int val)
	{
		switch (val)
		{
		case 1:
			return this.inputTwo;
		case 2:
			return this.inputThree;
		case 3:
			return this.inputFour;
		}
		return this.inputOne;
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x00104FF1 File Offset: 0x001031F1
	private void ShowSymbolConditionally(bool showAnything, bool active, KBatchedAnimController kbac, KAnimHashedString ifTrue, KAnimHashedString ifFalse)
	{
		if (!showAnything)
		{
			kbac.SetSymbolVisiblity(ifTrue, false);
			kbac.SetSymbolVisiblity(ifFalse, false);
			return;
		}
		kbac.SetSymbolVisiblity(ifTrue, active);
		kbac.SetSymbolVisiblity(ifFalse, !active);
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x0010501E File Offset: 0x0010321E
	private void TintSymbolConditionally(bool tintAnything, bool condition, KBatchedAnimController kbac, KAnimHashedString symbol, Color ifTrue, Color ifFalse)
	{
		if (tintAnything)
		{
			kbac.SetSymbolTint(symbol, condition ? ifTrue : ifFalse);
			return;
		}
		kbac.SetSymbolTint(symbol, Color.white);
	}

	// Token: 0x06002EA9 RID: 11945 RVA: 0x00105042 File Offset: 0x00103242
	private void SetBloomSymbolShowing(bool showing, KBatchedAnimController kbac, KAnimHashedString symbol, KAnimHashedString bloomSymbol)
	{
		kbac.SetSymbolVisiblity(bloomSymbol, showing);
		kbac.SetSymbolVisiblity(symbol, !showing);
	}

	// Token: 0x06002EAA RID: 11946 RVA: 0x00105058 File Offset: 0x00103258
	protected void RefreshAnimation()
	{
		if (this.cleaningUp)
		{
			return;
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.op == LogicGateBase.Op.Multiplexer)
		{
			int num = LogicCircuitNetwork.GetBitValue(0, this.controlOne.Value) + LogicCircuitNetwork.GetBitValue(0, this.controlTwo.Value) * 2;
			if (this.lastAnimState != num)
			{
				if (this.lastAnimState == -1)
				{
					component.Play(num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(this.lastAnimState.ToString() + "_" + num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			this.lastAnimState = num;
			LogicEventHandler inputFromControlValue = this.GetInputFromControlValue(num);
			KAnimHashedString[] array = LogicGate.multiplexerSymbolPaths[num];
			LogicCircuitNetwork logicCircuitNetwork = Game.Instance.logicCircuitSystem.GetNetworkForCell(inputFromControlValue.GetLogicCell()) as LogicCircuitNetwork;
			UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellOne);
			UtilityNetwork networkForCell2 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellTwo);
			UtilityNetwork networkForCell3 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellThree);
			UtilityNetwork networkForCell4 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellFour);
			this.ShowSymbolConditionally(networkForCell != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL_BLM_RED, LogicGate.INPUT1_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell2 != null, this.inputTwo.Value == 0, component, LogicGate.INPUT2_SYMBOL_BLM_RED, LogicGate.INPUT2_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell3 != null, this.inputThree.Value == 0, component, LogicGate.INPUT3_SYMBOL_BLM_RED, LogicGate.INPUT3_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell4 != null, this.inputFour.Value == 0, component, LogicGate.INPUT4_SYMBOL_BLM_RED, LogicGate.INPUT4_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(logicCircuitNetwork != null, inputFromControlValue.Value == 0, component, LogicGate.OUTPUT1_SYMBOL_BLM_RED, LogicGate.OUTPUT1_SYMBOL_BLM_GRN);
			this.TintSymbolConditionally(networkForCell != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell2 != null, this.inputTwo.Value == 0, component, LogicGate.INPUT2_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell3 != null, this.inputThree.Value == 0, component, LogicGate.INPUT3_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell4 != null, this.inputFour.Value == 0, component, LogicGate.INPUT4_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(Game.Instance.logicCircuitSystem.GetNetworkForCell(base.OutputCellOne) != null && logicCircuitNetwork != null, inputFromControlValue.Value == 0, component, LogicGate.OUTPUT1_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			for (int i = 0; i < LogicGate.multiplexerSymbols.Length; i++)
			{
				KAnimHashedString symbol = LogicGate.multiplexerSymbols[i];
				KAnimHashedString kanimHashedString = LogicGate.multiplexerBloomSymbols[i];
				bool flag = Array.IndexOf<KAnimHashedString>(array, kanimHashedString) != -1 && logicCircuitNetwork != null;
				this.SetBloomSymbolShowing(flag, component, symbol, kanimHashedString);
				if (flag)
				{
					component.SetSymbolTint(kanimHashedString, (inputFromControlValue.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
				}
			}
			return;
		}
		if (this.op == LogicGateBase.Op.Demultiplexer)
		{
			int num2 = LogicCircuitNetwork.GetBitValue(0, this.controlOne.Value) * 2 + LogicCircuitNetwork.GetBitValue(0, this.controlTwo.Value);
			if (this.lastAnimState != num2)
			{
				if (this.lastAnimState == -1)
				{
					component.Play(num2.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(this.lastAnimState.ToString() + "_" + num2.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			this.lastAnimState = num2;
			KAnimHashedString[] array2 = LogicGate.demultiplexerSymbolPaths[num2];
			LogicCircuitNetwork logicCircuitNetwork2 = Game.Instance.logicCircuitSystem.GetNetworkForCell(this.inputOne.GetLogicCell()) as LogicCircuitNetwork;
			for (int j = 0; j < LogicGate.demultiplexerSymbols.Length; j++)
			{
				KAnimHashedString symbol2 = LogicGate.demultiplexerSymbols[j];
				KAnimHashedString kanimHashedString2 = LogicGate.demultiplexerBloomSymbols[j];
				bool flag2 = Array.IndexOf<KAnimHashedString>(array2, kanimHashedString2) != -1 && logicCircuitNetwork2 != null;
				this.SetBloomSymbolShowing(flag2, component, symbol2, kanimHashedString2);
				if (flag2)
				{
					component.SetSymbolTint(kanimHashedString2, (this.inputOne.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
				}
			}
			this.ShowSymbolConditionally(logicCircuitNetwork2 != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL_BLM_RED, LogicGate.INPUT1_SYMBOL_BLM_GRN);
			if (logicCircuitNetwork2 != null)
			{
				component.SetSymbolTint(LogicGate.INPUT1_SYMBOL_BLOOM, (this.inputOne.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
			}
			int[] array3 = new int[]
			{
				base.OutputCellOne,
				base.OutputCellTwo,
				base.OutputCellThree,
				base.OutputCellFour
			};
			for (int k = 0; k < LogicGate.demultiplexerOutputSymbols.Length; k++)
			{
				KAnimHashedString kanimHashedString3 = LogicGate.demultiplexerOutputSymbols[k];
				bool flag3 = Array.IndexOf<KAnimHashedString>(array2, kanimHashedString3) == -1 || this.inputOne.Value == 0;
				UtilityNetwork networkForCell5 = Game.Instance.logicCircuitSystem.GetNetworkForCell(array3[k]);
				this.TintSymbolConditionally(logicCircuitNetwork2 != null && networkForCell5 != null, flag3, component, kanimHashedString3, this.inactiveTintColor, this.activeTintColor);
				this.ShowSymbolConditionally(logicCircuitNetwork2 != null && networkForCell5 != null, flag3, component, LogicGate.demultiplexerOutputRedSymbols[k], LogicGate.demultiplexerOutputGreenSymbols[k]);
			}
			return;
		}
		if (this.op == LogicGateBase.Op.And || this.op == LogicGateBase.Op.Xor || this.op == LogicGateBase.Op.Not || this.op == LogicGateBase.Op.Or)
		{
			int outputCellOne = base.OutputCellOne;
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
			{
				component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (base.RequiresTwoInputs)
			{
				int num3 = this.inputOne.Value * 2 + this.inputTwo.Value;
				if (this.lastAnimState != num3)
				{
					if (this.lastAnimState == -1)
					{
						component.Play(num3.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					else
					{
						component.Play(this.lastAnimState.ToString() + "_" + num3.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					this.lastAnimState = num3;
					return;
				}
			}
			else
			{
				int value = this.inputOne.Value;
				if (this.lastAnimState != value)
				{
					if (this.lastAnimState == -1)
					{
						component.Play(value.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					else
					{
						component.Play(this.lastAnimState.ToString() + "_" + value.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					this.lastAnimState = value;
					return;
				}
			}
		}
		else
		{
			int outputCellOne2 = base.OutputCellOne;
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne2) is LogicCircuitNetwork))
			{
				component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (base.RequiresTwoInputs)
			{
				component.Play("on_" + (this.inputOne.Value + this.inputTwo.Value * 2 + this.outputValueOne * 4).ToString(), KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play("on_" + (this.inputOne.Value + this.outputValueOne * 4).ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002EAB RID: 11947 RVA: 0x0010585D File Offset: 0x00103A5D
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
	}

	// Token: 0x04001B26 RID: 6950
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE
	};

	// Token: 0x04001B27 RID: 6951
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_MULTI_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_INACTIVE
	};

	// Token: 0x04001B28 RID: 6952
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_INACTIVE
	};

	// Token: 0x04001B29 RID: 6953
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_THREE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_INACTIVE
	};

	// Token: 0x04001B2A RID: 6954
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_FOUR_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_INACTIVE
	};

	// Token: 0x04001B2B RID: 6955
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE
	};

	// Token: 0x04001B2C RID: 6956
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_ONE_MULTI_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_INACTIVE
	};

	// Token: 0x04001B2D RID: 6957
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_INACTIVE
	};

	// Token: 0x04001B2E RID: 6958
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_THREE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_INACTIVE
	};

	// Token: 0x04001B2F RID: 6959
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_FOUR_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_INACTIVE
	};

	// Token: 0x04001B30 RID: 6960
	private static readonly LogicGate.LogicGateDescriptions.Description CONTROL_ONE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE
	};

	// Token: 0x04001B31 RID: 6961
	private static readonly LogicGate.LogicGateDescriptions.Description CONTROL_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE
	};

	// Token: 0x04001B32 RID: 6962
	private LogicGate.LogicGateDescriptions descriptions;

	// Token: 0x04001B33 RID: 6963
	private LogicEventSender[] additionalOutputs;

	// Token: 0x04001B34 RID: 6964
	private const bool IS_CIRCUIT_ENDPOINT = true;

	// Token: 0x04001B35 RID: 6965
	private bool connected;

	// Token: 0x04001B36 RID: 6966
	protected bool cleaningUp;

	// Token: 0x04001B37 RID: 6967
	private int lastAnimState = -1;

	// Token: 0x04001B38 RID: 6968
	[Serialize]
	protected int outputValueOne;

	// Token: 0x04001B39 RID: 6969
	[Serialize]
	protected int outputValueTwo;

	// Token: 0x04001B3A RID: 6970
	[Serialize]
	protected int outputValueThree;

	// Token: 0x04001B3B RID: 6971
	[Serialize]
	protected int outputValueFour;

	// Token: 0x04001B3C RID: 6972
	private LogicEventHandler inputOne;

	// Token: 0x04001B3D RID: 6973
	private LogicEventHandler inputTwo;

	// Token: 0x04001B3E RID: 6974
	private LogicEventHandler inputThree;

	// Token: 0x04001B3F RID: 6975
	private LogicEventHandler inputFour;

	// Token: 0x04001B40 RID: 6976
	private LogicPortVisualizer outputOne;

	// Token: 0x04001B41 RID: 6977
	private LogicPortVisualizer outputTwo;

	// Token: 0x04001B42 RID: 6978
	private LogicPortVisualizer outputThree;

	// Token: 0x04001B43 RID: 6979
	private LogicPortVisualizer outputFour;

	// Token: 0x04001B44 RID: 6980
	private LogicEventSender outputTwoSender;

	// Token: 0x04001B45 RID: 6981
	private LogicEventSender outputThreeSender;

	// Token: 0x04001B46 RID: 6982
	private LogicEventSender outputFourSender;

	// Token: 0x04001B47 RID: 6983
	private LogicEventHandler controlOne;

	// Token: 0x04001B48 RID: 6984
	private LogicEventHandler controlTwo;

	// Token: 0x04001B49 RID: 6985
	private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicGate>(delegate(LogicGate component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001B4A RID: 6986
	private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicGate>(delegate(LogicGate component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x04001B4B RID: 6987
	private static KAnimHashedString INPUT1_SYMBOL = "input1";

	// Token: 0x04001B4C RID: 6988
	private static KAnimHashedString INPUT2_SYMBOL = "input2";

	// Token: 0x04001B4D RID: 6989
	private static KAnimHashedString INPUT3_SYMBOL = "input3";

	// Token: 0x04001B4E RID: 6990
	private static KAnimHashedString INPUT4_SYMBOL = "input4";

	// Token: 0x04001B4F RID: 6991
	private static KAnimHashedString OUTPUT1_SYMBOL = "output1";

	// Token: 0x04001B50 RID: 6992
	private static KAnimHashedString OUTPUT2_SYMBOL = "output2";

	// Token: 0x04001B51 RID: 6993
	private static KAnimHashedString OUTPUT3_SYMBOL = "output3";

	// Token: 0x04001B52 RID: 6994
	private static KAnimHashedString OUTPUT4_SYMBOL = "output4";

	// Token: 0x04001B53 RID: 6995
	private static KAnimHashedString INPUT1_SYMBOL_BLM_RED = "input1_red_bloom";

	// Token: 0x04001B54 RID: 6996
	private static KAnimHashedString INPUT1_SYMBOL_BLM_GRN = "input1_green_bloom";

	// Token: 0x04001B55 RID: 6997
	private static KAnimHashedString INPUT2_SYMBOL_BLM_RED = "input2_red_bloom";

	// Token: 0x04001B56 RID: 6998
	private static KAnimHashedString INPUT2_SYMBOL_BLM_GRN = "input2_green_bloom";

	// Token: 0x04001B57 RID: 6999
	private static KAnimHashedString INPUT3_SYMBOL_BLM_RED = "input3_red_bloom";

	// Token: 0x04001B58 RID: 7000
	private static KAnimHashedString INPUT3_SYMBOL_BLM_GRN = "input3_green_bloom";

	// Token: 0x04001B59 RID: 7001
	private static KAnimHashedString INPUT4_SYMBOL_BLM_RED = "input4_red_bloom";

	// Token: 0x04001B5A RID: 7002
	private static KAnimHashedString INPUT4_SYMBOL_BLM_GRN = "input4_green_bloom";

	// Token: 0x04001B5B RID: 7003
	private static KAnimHashedString OUTPUT1_SYMBOL_BLM_RED = "output1_red_bloom";

	// Token: 0x04001B5C RID: 7004
	private static KAnimHashedString OUTPUT1_SYMBOL_BLM_GRN = "output1_green_bloom";

	// Token: 0x04001B5D RID: 7005
	private static KAnimHashedString OUTPUT2_SYMBOL_BLM_RED = "output2_red_bloom";

	// Token: 0x04001B5E RID: 7006
	private static KAnimHashedString OUTPUT2_SYMBOL_BLM_GRN = "output2_green_bloom";

	// Token: 0x04001B5F RID: 7007
	private static KAnimHashedString OUTPUT3_SYMBOL_BLM_RED = "output3_red_bloom";

	// Token: 0x04001B60 RID: 7008
	private static KAnimHashedString OUTPUT3_SYMBOL_BLM_GRN = "output3_green_bloom";

	// Token: 0x04001B61 RID: 7009
	private static KAnimHashedString OUTPUT4_SYMBOL_BLM_RED = "output4_red_bloom";

	// Token: 0x04001B62 RID: 7010
	private static KAnimHashedString OUTPUT4_SYMBOL_BLM_GRN = "output4_green_bloom";

	// Token: 0x04001B63 RID: 7011
	private static KAnimHashedString LINE_LEFT_1_SYMBOL = "line_left_1";

	// Token: 0x04001B64 RID: 7012
	private static KAnimHashedString LINE_LEFT_2_SYMBOL = "line_left_2";

	// Token: 0x04001B65 RID: 7013
	private static KAnimHashedString LINE_LEFT_3_SYMBOL = "line_left_3";

	// Token: 0x04001B66 RID: 7014
	private static KAnimHashedString LINE_LEFT_4_SYMBOL = "line_left_4";

	// Token: 0x04001B67 RID: 7015
	private static KAnimHashedString LINE_RIGHT_1_SYMBOL = "line_right_1";

	// Token: 0x04001B68 RID: 7016
	private static KAnimHashedString LINE_RIGHT_2_SYMBOL = "line_right_2";

	// Token: 0x04001B69 RID: 7017
	private static KAnimHashedString LINE_RIGHT_3_SYMBOL = "line_right_3";

	// Token: 0x04001B6A RID: 7018
	private static KAnimHashedString LINE_RIGHT_4_SYMBOL = "line_right_4";

	// Token: 0x04001B6B RID: 7019
	private static KAnimHashedString FLIPPER_1_SYMBOL = "flipper1";

	// Token: 0x04001B6C RID: 7020
	private static KAnimHashedString FLIPPER_2_SYMBOL = "flipper2";

	// Token: 0x04001B6D RID: 7021
	private static KAnimHashedString FLIPPER_3_SYMBOL = "flipper3";

	// Token: 0x04001B6E RID: 7022
	private static KAnimHashedString INPUT_SYMBOL = "input";

	// Token: 0x04001B6F RID: 7023
	private static KAnimHashedString OUTPUT_SYMBOL = "output";

	// Token: 0x04001B70 RID: 7024
	private static KAnimHashedString INPUT1_SYMBOL_BLOOM = "input1_bloom";

	// Token: 0x04001B71 RID: 7025
	private static KAnimHashedString INPUT2_SYMBOL_BLOOM = "input2_bloom";

	// Token: 0x04001B72 RID: 7026
	private static KAnimHashedString INPUT3_SYMBOL_BLOOM = "input3_bloom";

	// Token: 0x04001B73 RID: 7027
	private static KAnimHashedString INPUT4_SYMBOL_BLOOM = "input4_bloom";

	// Token: 0x04001B74 RID: 7028
	private static KAnimHashedString OUTPUT1_SYMBOL_BLOOM = "output1_bloom";

	// Token: 0x04001B75 RID: 7029
	private static KAnimHashedString OUTPUT2_SYMBOL_BLOOM = "output2_bloom";

	// Token: 0x04001B76 RID: 7030
	private static KAnimHashedString OUTPUT3_SYMBOL_BLOOM = "output3_bloom";

	// Token: 0x04001B77 RID: 7031
	private static KAnimHashedString OUTPUT4_SYMBOL_BLOOM = "output4_bloom";

	// Token: 0x04001B78 RID: 7032
	private static KAnimHashedString LINE_LEFT_1_SYMBOL_BLOOM = "line_left_1_bloom";

	// Token: 0x04001B79 RID: 7033
	private static KAnimHashedString LINE_LEFT_2_SYMBOL_BLOOM = "line_left_2_bloom";

	// Token: 0x04001B7A RID: 7034
	private static KAnimHashedString LINE_LEFT_3_SYMBOL_BLOOM = "line_left_3_bloom";

	// Token: 0x04001B7B RID: 7035
	private static KAnimHashedString LINE_LEFT_4_SYMBOL_BLOOM = "line_left_4_bloom";

	// Token: 0x04001B7C RID: 7036
	private static KAnimHashedString LINE_RIGHT_1_SYMBOL_BLOOM = "line_right_1_bloom";

	// Token: 0x04001B7D RID: 7037
	private static KAnimHashedString LINE_RIGHT_2_SYMBOL_BLOOM = "line_right_2_bloom";

	// Token: 0x04001B7E RID: 7038
	private static KAnimHashedString LINE_RIGHT_3_SYMBOL_BLOOM = "line_right_3_bloom";

	// Token: 0x04001B7F RID: 7039
	private static KAnimHashedString LINE_RIGHT_4_SYMBOL_BLOOM = "line_right_4_bloom";

	// Token: 0x04001B80 RID: 7040
	private static KAnimHashedString FLIPPER_1_SYMBOL_BLOOM = "flipper1_bloom";

	// Token: 0x04001B81 RID: 7041
	private static KAnimHashedString FLIPPER_2_SYMBOL_BLOOM = "flipper2_bloom";

	// Token: 0x04001B82 RID: 7042
	private static KAnimHashedString FLIPPER_3_SYMBOL_BLOOM = "flipper3_bloom";

	// Token: 0x04001B83 RID: 7043
	private static KAnimHashedString INPUT_SYMBOL_BLOOM = "input_bloom";

	// Token: 0x04001B84 RID: 7044
	private static KAnimHashedString OUTPUT_SYMBOL_BLOOM = "output_bloom";

	// Token: 0x04001B85 RID: 7045
	private static KAnimHashedString[][] multiplexerSymbolPaths = new KAnimHashedString[][]
	{
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_3_SYMBOL_BLOOM,
			LogicGate.FLIPPER_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_4_SYMBOL_BLOOM,
			LogicGate.FLIPPER_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		}
	};

	// Token: 0x04001B86 RID: 7046
	private static KAnimHashedString[] multiplexerSymbols = new KAnimHashedString[]
	{
		LogicGate.LINE_LEFT_1_SYMBOL,
		LogicGate.LINE_LEFT_2_SYMBOL,
		LogicGate.LINE_LEFT_3_SYMBOL,
		LogicGate.LINE_LEFT_4_SYMBOL,
		LogicGate.LINE_RIGHT_1_SYMBOL,
		LogicGate.LINE_RIGHT_2_SYMBOL,
		LogicGate.FLIPPER_1_SYMBOL,
		LogicGate.FLIPPER_2_SYMBOL,
		LogicGate.FLIPPER_3_SYMBOL,
		LogicGate.OUTPUT_SYMBOL
	};

	// Token: 0x04001B87 RID: 7047
	private static KAnimHashedString[] multiplexerBloomSymbols = new KAnimHashedString[]
	{
		LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_3_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_4_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
		LogicGate.FLIPPER_1_SYMBOL_BLOOM,
		LogicGate.FLIPPER_2_SYMBOL_BLOOM,
		LogicGate.FLIPPER_3_SYMBOL_BLOOM,
		LogicGate.OUTPUT_SYMBOL_BLOOM
	};

	// Token: 0x04001B88 RID: 7048
	private static KAnimHashedString[][] demultiplexerSymbolPaths = new KAnimHashedString[][]
	{
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.OUTPUT1_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.OUTPUT2_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT3_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_4_SYMBOL_BLOOM,
			LogicGate.OUTPUT4_SYMBOL
		}
	};

	// Token: 0x04001B89 RID: 7049
	private static KAnimHashedString[] demultiplexerSymbols = new KAnimHashedString[]
	{
		LogicGate.INPUT_SYMBOL,
		LogicGate.LINE_LEFT_1_SYMBOL,
		LogicGate.LINE_LEFT_2_SYMBOL,
		LogicGate.LINE_RIGHT_1_SYMBOL,
		LogicGate.LINE_RIGHT_2_SYMBOL,
		LogicGate.LINE_RIGHT_3_SYMBOL,
		LogicGate.LINE_RIGHT_4_SYMBOL
	};

	// Token: 0x04001B8A RID: 7050
	private static KAnimHashedString[] demultiplexerBloomSymbols = new KAnimHashedString[]
	{
		LogicGate.INPUT_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_3_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_4_SYMBOL_BLOOM
	};

	// Token: 0x04001B8B RID: 7051
	private static KAnimHashedString[] demultiplexerOutputSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL,
		LogicGate.OUTPUT2_SYMBOL,
		LogicGate.OUTPUT3_SYMBOL,
		LogicGate.OUTPUT4_SYMBOL
	};

	// Token: 0x04001B8C RID: 7052
	private static KAnimHashedString[] demultiplexerOutputRedSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL_BLM_RED,
		LogicGate.OUTPUT2_SYMBOL_BLM_RED,
		LogicGate.OUTPUT3_SYMBOL_BLM_RED,
		LogicGate.OUTPUT4_SYMBOL_BLM_RED
	};

	// Token: 0x04001B8D RID: 7053
	private static KAnimHashedString[] demultiplexerOutputGreenSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT2_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT3_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT4_SYMBOL_BLM_GRN
	};

	// Token: 0x04001B8E RID: 7054
	private Color activeTintColor = new Color(0.5411765f, 0.9882353f, 0.29803923f);

	// Token: 0x04001B8F RID: 7055
	private Color inactiveTintColor = Color.red;

	// Token: 0x02001546 RID: 5446
	public class LogicGateDescriptions
	{
		// Token: 0x04006C6B RID: 27755
		public LogicGate.LogicGateDescriptions.Description inputOne;

		// Token: 0x04006C6C RID: 27756
		public LogicGate.LogicGateDescriptions.Description inputTwo;

		// Token: 0x04006C6D RID: 27757
		public LogicGate.LogicGateDescriptions.Description inputThree;

		// Token: 0x04006C6E RID: 27758
		public LogicGate.LogicGateDescriptions.Description inputFour;

		// Token: 0x04006C6F RID: 27759
		public LogicGate.LogicGateDescriptions.Description outputOne;

		// Token: 0x04006C70 RID: 27760
		public LogicGate.LogicGateDescriptions.Description outputTwo;

		// Token: 0x04006C71 RID: 27761
		public LogicGate.LogicGateDescriptions.Description outputThree;

		// Token: 0x04006C72 RID: 27762
		public LogicGate.LogicGateDescriptions.Description outputFour;

		// Token: 0x04006C73 RID: 27763
		public LogicGate.LogicGateDescriptions.Description controlOne;

		// Token: 0x04006C74 RID: 27764
		public LogicGate.LogicGateDescriptions.Description controlTwo;

		// Token: 0x020024FD RID: 9469
		public class Description
		{
			// Token: 0x0400A497 RID: 42135
			public string name;

			// Token: 0x0400A498 RID: 42136
			public string active;

			// Token: 0x0400A499 RID: 42137
			public string inactive;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200094E RID: 2382
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicPorts")]
public class LogicPorts : KMonoBehaviour, IGameObjectEffectDescriptor, IRenderEveryTick
{
	// Token: 0x06004577 RID: 17783 RVA: 0x0018BC2B File Offset: 0x00189E2B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06004578 RID: 17784 RVA: 0x0018BC3C File Offset: 0x00189E3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.isPhysical = (component == null || component is BuildingComplete);
		if (!this.isPhysical && !(component is BuildingUnderConstruction))
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.OnOverlayChanged(OverlayScreen.Instance.mode);
			this.CreateVisualizers();
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		if (this.isPhysical)
		{
			this.UpdateMissingWireIcon();
			this.CreatePhysicalPorts(false);
			return;
		}
		this.CreateVisualizers();
	}

	// Token: 0x06004579 RID: 17785 RVA: 0x0018BCF0 File Offset: 0x00189EF0
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.DestroyVisualizers();
		if (this.isPhysical)
		{
			this.DestroyPhysicalPorts();
		}
		base.OnCleanUp();
	}

	// Token: 0x0600457A RID: 17786 RVA: 0x0018BD3D File Offset: 0x00189F3D
	public void RenderEveryTick(float dt)
	{
		this.CreateVisualizers();
	}

	// Token: 0x0600457B RID: 17787 RVA: 0x0018BD45 File Offset: 0x00189F45
	public void HackRefreshVisualizers()
	{
		this.CreateVisualizers();
	}

	// Token: 0x0600457C RID: 17788 RVA: 0x0018BD50 File Offset: 0x00189F50
	private void CreateVisualizers()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool flag = num != this.cell;
		this.cell = num;
		if (!flag)
		{
			Rotatable component = base.GetComponent<Rotatable>();
			if (component != null)
			{
				Orientation orientation = component.GetOrientation();
				flag = (orientation != this.orientation);
				this.orientation = orientation;
			}
		}
		if (!flag)
		{
			return;
		}
		this.DestroyVisualizers();
		if (this.outputPortInfo != null)
		{
			this.outputPorts = new List<ILogicUIElement>();
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port port = this.outputPortInfo[i];
				LogicPortVisualizer logicPortVisualizer = new LogicPortVisualizer(this.GetActualCell(port.cellOffset), port.spriteType);
				this.outputPorts.Add(logicPortVisualizer);
				Game.Instance.logicCircuitManager.AddVisElem(logicPortVisualizer);
			}
		}
		if (this.inputPortInfo != null)
		{
			this.inputPorts = new List<ILogicUIElement>();
			for (int j = 0; j < this.inputPortInfo.Length; j++)
			{
				LogicPorts.Port port2 = this.inputPortInfo[j];
				LogicPortVisualizer logicPortVisualizer2 = new LogicPortVisualizer(this.GetActualCell(port2.cellOffset), port2.spriteType);
				this.inputPorts.Add(logicPortVisualizer2);
				Game.Instance.logicCircuitManager.AddVisElem(logicPortVisualizer2);
			}
		}
	}

	// Token: 0x0600457D RID: 17789 RVA: 0x0018BEA0 File Offset: 0x0018A0A0
	private void DestroyVisualizers()
	{
		if (this.outputPorts != null)
		{
			foreach (ILogicUIElement elem in this.outputPorts)
			{
				Game.Instance.logicCircuitManager.RemoveVisElem(elem);
			}
		}
		if (this.inputPorts != null)
		{
			foreach (ILogicUIElement elem2 in this.inputPorts)
			{
				Game.Instance.logicCircuitManager.RemoveVisElem(elem2);
			}
		}
	}

	// Token: 0x0600457E RID: 17790 RVA: 0x0018BF58 File Offset: 0x0018A158
	private void CreatePhysicalPorts(bool forceCreate = false)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num == this.cell && !forceCreate)
		{
			return;
		}
		this.cell = num;
		this.DestroyVisualizers();
		if (this.outputPortInfo != null)
		{
			this.outputPorts = new List<ILogicUIElement>();
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port info = this.outputPortInfo[i];
				LogicEventSender logicEventSender = new LogicEventSender(info.id, this.GetActualCell(info.cellOffset), delegate(int new_value, int prev_value)
				{
					if (this != null)
					{
						this.OnLogicValueChanged(info.id, new_value, prev_value);
					}
				}, new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
				this.outputPorts.Add(logicEventSender);
				Game.Instance.logicCircuitManager.AddVisElem(logicEventSender);
				Game.Instance.logicCircuitSystem.AddToNetworks(logicEventSender.GetLogicUICell(), logicEventSender, true);
			}
			if (this.serializedOutputValues != null && this.serializedOutputValues.Length == this.outputPorts.Count)
			{
				for (int j = 0; j < this.outputPorts.Count; j++)
				{
					(this.outputPorts[j] as LogicEventSender).SetValue(this.serializedOutputValues[j]);
				}
			}
			else
			{
				for (int k = 0; k < this.outputPorts.Count; k++)
				{
					(this.outputPorts[k] as LogicEventSender).SetValue(0);
				}
			}
		}
		this.serializedOutputValues = null;
		if (this.inputPortInfo != null)
		{
			this.inputPorts = new List<ILogicUIElement>();
			for (int l = 0; l < this.inputPortInfo.Length; l++)
			{
				LogicPorts.Port info = this.inputPortInfo[l];
				LogicEventHandler logicEventHandler = new LogicEventHandler(this.GetActualCell(info.cellOffset), delegate(int new_value, int prev_value)
				{
					if (this != null)
					{
						this.OnLogicValueChanged(info.id, new_value, prev_value);
					}
				}, new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
				this.inputPorts.Add(logicEventHandler);
				Game.Instance.logicCircuitManager.AddVisElem(logicEventHandler);
				Game.Instance.logicCircuitSystem.AddToNetworks(logicEventHandler.GetLogicUICell(), logicEventHandler, true);
			}
		}
	}

	// Token: 0x0600457F RID: 17791 RVA: 0x0018C1B4 File Offset: 0x0018A3B4
	private bool ShowMissingWireIcon()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		if (this.outputPortInfo != null)
		{
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port port = this.outputPortInfo[i];
				if (port.requiresConnection)
				{
					int portCell = this.GetPortCell(port.id);
					if (logicCircuitManager.GetNetworkForCell(portCell) == null)
					{
						return true;
					}
				}
			}
		}
		if (this.inputPortInfo != null)
		{
			for (int j = 0; j < this.inputPortInfo.Length; j++)
			{
				LogicPorts.Port port2 = this.inputPortInfo[j];
				if (port2.requiresConnection)
				{
					int portCell2 = this.GetPortCell(port2.id);
					if (logicCircuitManager.GetNetworkForCell(portCell2) == null)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06004580 RID: 17792 RVA: 0x0018C267 File Offset: 0x0018A467
	public void OnMove()
	{
		this.DestroyPhysicalPorts();
		this.CreatePhysicalPorts(false);
	}

	// Token: 0x06004581 RID: 17793 RVA: 0x0018C276 File Offset: 0x0018A476
	private void OnLogicNetworkConnectionChanged(int cell, bool connected)
	{
		this.UpdateMissingWireIcon();
	}

	// Token: 0x06004582 RID: 17794 RVA: 0x0018C27E File Offset: 0x0018A47E
	private void UpdateMissingWireIcon()
	{
		LogicCircuitManager.ToggleNoWireConnected(this.ShowMissingWireIcon(), base.gameObject);
	}

	// Token: 0x06004583 RID: 17795 RVA: 0x0018C294 File Offset: 0x0018A494
	private void DestroyPhysicalPorts()
	{
		if (this.outputPorts != null)
		{
			foreach (ILogicUIElement logicUIElement in this.outputPorts)
			{
				ILogicEventSender logicEventSender = (ILogicEventSender)logicUIElement;
				Game.Instance.logicCircuitSystem.RemoveFromNetworks(logicEventSender.GetLogicCell(), logicEventSender, true);
			}
		}
		if (this.inputPorts != null)
		{
			for (int i = 0; i < this.inputPorts.Count; i++)
			{
				LogicEventHandler logicEventHandler = this.inputPorts[i] as LogicEventHandler;
				if (logicEventHandler != null)
				{
					Game.Instance.logicCircuitSystem.RemoveFromNetworks(logicEventHandler.GetLogicCell(), logicEventHandler, true);
				}
			}
		}
	}

	// Token: 0x06004584 RID: 17796 RVA: 0x0018C350 File Offset: 0x0018A550
	private void OnLogicValueChanged(HashedString port_id, int new_value, int prev_value)
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

	// Token: 0x06004585 RID: 17797 RVA: 0x0018C3A4 File Offset: 0x0018A5A4
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x06004586 RID: 17798 RVA: 0x0018C3E0 File Offset: 0x0018A5E0
	public bool TryGetPortAtCell(int cell, out LogicPorts.Port port, out bool isInput)
	{
		foreach (LogicPorts.Port port2 in this.inputPortInfo)
		{
			if (this.GetActualCell(port2.cellOffset) == cell)
			{
				port = port2;
				isInput = true;
				return true;
			}
		}
		foreach (LogicPorts.Port port3 in this.outputPortInfo)
		{
			if (this.GetActualCell(port3.cellOffset) == cell)
			{
				port = port3;
				isInput = false;
				return true;
			}
		}
		port = default(LogicPorts.Port);
		isInput = false;
		return false;
	}

	// Token: 0x06004587 RID: 17799 RVA: 0x0018C468 File Offset: 0x0018A668
	public void SendSignal(HashedString port_id, int new_value)
	{
		if (this.outputPortInfo != null && this.outputPorts == null)
		{
			this.CreatePhysicalPorts(true);
		}
		foreach (ILogicUIElement logicUIElement in this.outputPorts)
		{
			LogicEventSender logicEventSender = (LogicEventSender)logicUIElement;
			if (logicEventSender.ID == port_id)
			{
				logicEventSender.SetValue(new_value);
				break;
			}
		}
	}

	// Token: 0x06004588 RID: 17800 RVA: 0x0018C4E8 File Offset: 0x0018A6E8
	public int GetPortCell(HashedString port_id)
	{
		foreach (LogicPorts.Port port in this.inputPortInfo)
		{
			if (port.id == port_id)
			{
				return this.GetActualCell(port.cellOffset);
			}
		}
		foreach (LogicPorts.Port port2 in this.outputPortInfo)
		{
			if (port2.id == port_id)
			{
				return this.GetActualCell(port2.cellOffset);
			}
		}
		return -1;
	}

	// Token: 0x06004589 RID: 17801 RVA: 0x0018C568 File Offset: 0x0018A768
	public int GetInputValue(HashedString port_id)
	{
		int num = 0;
		while (num < this.inputPortInfo.Length && this.inputPorts != null)
		{
			if (this.inputPortInfo[num].id == port_id)
			{
				LogicEventHandler logicEventHandler = this.inputPorts[num] as LogicEventHandler;
				if (logicEventHandler == null)
				{
					return 0;
				}
				return logicEventHandler.Value;
			}
			else
			{
				num++;
			}
		}
		return 0;
	}

	// Token: 0x0600458A RID: 17802 RVA: 0x0018C5C8 File Offset: 0x0018A7C8
	public int GetOutputValue(HashedString port_id)
	{
		for (int i = 0; i < this.outputPorts.Count; i++)
		{
			LogicEventSender logicEventSender = this.outputPorts[i] as LogicEventSender;
			if (logicEventSender == null)
			{
				return 0;
			}
			if (logicEventSender.ID == port_id)
			{
				return logicEventSender.GetLogicValue();
			}
		}
		return 0;
	}

	// Token: 0x0600458B RID: 17803 RVA: 0x0018C618 File Offset: 0x0018A818
	public bool IsPortConnected(HashedString port_id)
	{
		int portCell = this.GetPortCell(port_id);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell) != null;
	}

	// Token: 0x0600458C RID: 17804 RVA: 0x0018C640 File Offset: 0x0018A840
	private void OnOverlayChanged(HashedString mode)
	{
		if (mode == OverlayModes.Logic.ID)
		{
			base.enabled = true;
			this.CreateVisualizers();
			return;
		}
		base.enabled = false;
		this.DestroyVisualizers();
	}

	// Token: 0x0600458D RID: 17805 RVA: 0x0018C66C File Offset: 0x0018A86C
	public LogicWire.BitDepth GetConnectedWireBitDepth(HashedString port_id)
	{
		LogicWire.BitDepth result = LogicWire.BitDepth.NumRatings;
		int portCell = this.GetPortCell(port_id);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component = gameObject.GetComponent<LogicWire>();
			if (component != null)
			{
				result = component.MaxBitDepth;
			}
		}
		return result;
	}

	// Token: 0x0600458E RID: 17806 RVA: 0x0018C6B4 File Offset: 0x0018A8B4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		LogicPorts component = go.GetComponent<LogicPorts>();
		if (component != null)
		{
			if (component.inputPortInfo != null && component.inputPortInfo.Length != 0)
			{
				Descriptor item = new Descriptor(UI.LOGIC_PORTS.INPUT_PORTS, UI.LOGIC_PORTS.INPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
				list.Add(item);
				foreach (LogicPorts.Port port in component.inputPortInfo)
				{
					string tooltip = string.Format(UI.LOGIC_PORTS.INPUT_PORT_TOOLTIP, port.activeDescription, port.inactiveDescription);
					item = new Descriptor(port.description, tooltip, Descriptor.DescriptorType.Effect, false);
					item.IncreaseIndent();
					list.Add(item);
				}
			}
			if (component.outputPortInfo != null && component.outputPortInfo.Length != 0)
			{
				Descriptor item2 = new Descriptor(UI.LOGIC_PORTS.OUTPUT_PORTS, UI.LOGIC_PORTS.OUTPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
				list.Add(item2);
				foreach (LogicPorts.Port port2 in component.outputPortInfo)
				{
					string tooltip2 = string.Format(UI.LOGIC_PORTS.OUTPUT_PORT_TOOLTIP, port2.activeDescription, port2.inactiveDescription);
					item2 = new Descriptor(port2.description, tooltip2, Descriptor.DescriptorType.Effect, false);
					item2.IncreaseIndent();
					list.Add(item2);
				}
			}
		}
		return list;
	}

	// Token: 0x0600458F RID: 17807 RVA: 0x0018C81C File Offset: 0x0018AA1C
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.isPhysical && this.outputPorts != null)
		{
			this.serializedOutputValues = new int[this.outputPorts.Count];
			for (int i = 0; i < this.outputPorts.Count; i++)
			{
				LogicEventSender logicEventSender = this.outputPorts[i] as LogicEventSender;
				this.serializedOutputValues[i] = logicEventSender.GetLogicValue();
			}
		}
	}

	// Token: 0x06004590 RID: 17808 RVA: 0x0018C885 File Offset: 0x0018AA85
	[OnSerialized]
	private void OnSerialized()
	{
		this.serializedOutputValues = null;
	}

	// Token: 0x04002D4C RID: 11596
	[SerializeField]
	public LogicPorts.Port[] outputPortInfo;

	// Token: 0x04002D4D RID: 11597
	[SerializeField]
	public LogicPorts.Port[] inputPortInfo;

	// Token: 0x04002D4E RID: 11598
	public List<ILogicUIElement> outputPorts;

	// Token: 0x04002D4F RID: 11599
	public List<ILogicUIElement> inputPorts;

	// Token: 0x04002D50 RID: 11600
	private int cell = -1;

	// Token: 0x04002D51 RID: 11601
	private Orientation orientation = Orientation.NumRotations;

	// Token: 0x04002D52 RID: 11602
	[Serialize]
	private int[] serializedOutputValues;

	// Token: 0x04002D53 RID: 11603
	private bool isPhysical;

	// Token: 0x020018B2 RID: 6322
	[Serializable]
	public struct Port
	{
		// Token: 0x0600999F RID: 39327 RVA: 0x0036AAA2 File Offset: 0x00368CA2
		public Port(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon, LogicPortSpriteType sprite_type, bool display_custom_name = false)
		{
			this.id = id;
			this.cellOffset = cell_offset;
			this.description = description;
			this.activeDescription = activeDescription;
			this.inactiveDescription = inactiveDescription;
			this.requiresConnection = show_wire_missing_icon;
			this.spriteType = sprite_type;
			this.displayCustomName = display_custom_name;
		}

		// Token: 0x060099A0 RID: 39328 RVA: 0x0036AAE1 File Offset: 0x00368CE1
		public static LogicPorts.Port InputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Input, display_custom_name);
		}

		// Token: 0x060099A1 RID: 39329 RVA: 0x0036AAF3 File Offset: 0x00368CF3
		public static LogicPorts.Port OutputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Output, display_custom_name);
		}

		// Token: 0x060099A2 RID: 39330 RVA: 0x0036AB05 File Offset: 0x00368D05
		public static LogicPorts.Port RibbonInputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonInput, display_custom_name);
		}

		// Token: 0x060099A3 RID: 39331 RVA: 0x0036AB17 File Offset: 0x00368D17
		public static LogicPorts.Port RibbonOutputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonOutput, display_custom_name);
		}

		// Token: 0x0400772D RID: 30509
		public HashedString id;

		// Token: 0x0400772E RID: 30510
		public CellOffset cellOffset;

		// Token: 0x0400772F RID: 30511
		public string description;

		// Token: 0x04007730 RID: 30512
		public string activeDescription;

		// Token: 0x04007731 RID: 30513
		public string inactiveDescription;

		// Token: 0x04007732 RID: 30514
		public bool requiresConnection;

		// Token: 0x04007733 RID: 30515
		public LogicPortSpriteType spriteType;

		// Token: 0x04007734 RID: 30516
		public bool displayCustomName;
	}
}

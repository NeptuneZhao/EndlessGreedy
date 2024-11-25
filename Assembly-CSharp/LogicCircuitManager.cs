using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x0200094A RID: 2378
public class LogicCircuitManager
{
	// Token: 0x0600453F RID: 17727 RVA: 0x0018AB60 File Offset: 0x00188D60
	public LogicCircuitManager(UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduit_system)
	{
		this.conduitSystem = conduit_system;
		this.timeSinceBridgeRefresh = 0f;
		this.elapsedTime = 0f;
		for (int i = 0; i < 2; i++)
		{
			this.bridgeGroups[i] = new List<LogicUtilityNetworkLink>();
		}
	}

	// Token: 0x06004540 RID: 17728 RVA: 0x0018ABC0 File Offset: 0x00188DC0
	public void RenderEveryTick(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x06004541 RID: 17729 RVA: 0x0018ABCC File Offset: 0x00188DCC
	private void Refresh(float dt)
	{
		if (this.conduitSystem.IsDirty)
		{
			this.conduitSystem.Update();
			LogicCircuitNetwork.logicSoundRegister.Clear();
			this.PropagateSignals(true);
			this.elapsedTime = 0f;
		}
		else if (SpeedControlScreen.Instance != null && !SpeedControlScreen.Instance.IsPaused)
		{
			this.elapsedTime += dt;
			this.timeSinceBridgeRefresh += dt;
			while (this.elapsedTime > LogicCircuitManager.ClockTickInterval)
			{
				this.elapsedTime -= LogicCircuitManager.ClockTickInterval;
				this.PropagateSignals(false);
				if (this.onLogicTick != null)
				{
					this.onLogicTick();
				}
			}
			if (this.timeSinceBridgeRefresh > LogicCircuitManager.BridgeRefreshInterval)
			{
				this.UpdateCircuitBridgeLists();
				this.timeSinceBridgeRefresh = 0f;
			}
		}
		foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
			this.CheckCircuitOverloaded(dt, logicCircuitNetwork.id, logicCircuitNetwork.GetBitsUsed());
		}
	}

	// Token: 0x06004542 RID: 17730 RVA: 0x0018AD00 File Offset: 0x00188F00
	private void PropagateSignals(bool force_send_events)
	{
		IList<UtilityNetwork> networks = Game.Instance.logicCircuitSystem.GetNetworks();
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			((LogicCircuitNetwork)utilityNetwork).UpdateLogicValue();
		}
		foreach (UtilityNetwork utilityNetwork2 in networks)
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork2;
			logicCircuitNetwork.SendLogicEvents(force_send_events, logicCircuitNetwork.id);
		}
	}

	// Token: 0x06004543 RID: 17731 RVA: 0x0018AD9C File Offset: 0x00188F9C
	public LogicCircuitNetwork GetNetworkForCell(int cell)
	{
		return this.conduitSystem.GetNetworkForCell(cell) as LogicCircuitNetwork;
	}

	// Token: 0x06004544 RID: 17732 RVA: 0x0018ADAF File Offset: 0x00188FAF
	public void AddVisElem(ILogicUIElement elem)
	{
		this.uiVisElements.Add(elem);
		if (this.onElemAdded != null)
		{
			this.onElemAdded(elem);
		}
	}

	// Token: 0x06004545 RID: 17733 RVA: 0x0018ADD1 File Offset: 0x00188FD1
	public void RemoveVisElem(ILogicUIElement elem)
	{
		if (this.onElemRemoved != null)
		{
			this.onElemRemoved(elem);
		}
		this.uiVisElements.Remove(elem);
	}

	// Token: 0x06004546 RID: 17734 RVA: 0x0018ADF4 File Offset: 0x00188FF4
	public ReadOnlyCollection<ILogicUIElement> GetVisElements()
	{
		return this.uiVisElements.AsReadOnly();
	}

	// Token: 0x06004547 RID: 17735 RVA: 0x0018AE01 File Offset: 0x00189001
	public static void ToggleNoWireConnected(bool show_missing_wire, GameObject go)
	{
		go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoLogicWireConnected, show_missing_wire, null);
	}

	// Token: 0x06004548 RID: 17736 RVA: 0x0018AE20 File Offset: 0x00189020
	private void CheckCircuitOverloaded(float dt, int id, int bits_used)
	{
		UtilityNetwork networkByID = Game.Instance.logicCircuitSystem.GetNetworkByID(id);
		if (networkByID != null)
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)networkByID;
			if (logicCircuitNetwork != null)
			{
				logicCircuitNetwork.UpdateOverloadTime(dt, bits_used);
			}
		}
	}

	// Token: 0x06004549 RID: 17737 RVA: 0x0018AE53 File Offset: 0x00189053
	public void Connect(LogicUtilityNetworkLink bridge)
	{
		this.bridgeGroups[(int)bridge.bitDepth].Add(bridge);
	}

	// Token: 0x0600454A RID: 17738 RVA: 0x0018AE68 File Offset: 0x00189068
	public void Disconnect(LogicUtilityNetworkLink bridge)
	{
		this.bridgeGroups[(int)bridge.bitDepth].Remove(bridge);
	}

	// Token: 0x0600454B RID: 17739 RVA: 0x0018AE80 File Offset: 0x00189080
	private void UpdateCircuitBridgeLists()
	{
		foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
			if (this.updateEvenBridgeGroups)
			{
				if (logicCircuitNetwork.id % 2 == 0)
				{
					logicCircuitNetwork.UpdateRelevantBridges(this.bridgeGroups);
				}
			}
			else if (logicCircuitNetwork.id % 2 == 1)
			{
				logicCircuitNetwork.UpdateRelevantBridges(this.bridgeGroups);
			}
		}
		this.updateEvenBridgeGroups = !this.updateEvenBridgeGroups;
	}

	// Token: 0x04002D25 RID: 11557
	public static float ClockTickInterval = 0.1f;

	// Token: 0x04002D26 RID: 11558
	private float elapsedTime;

	// Token: 0x04002D27 RID: 11559
	private UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduitSystem;

	// Token: 0x04002D28 RID: 11560
	private List<ILogicUIElement> uiVisElements = new List<ILogicUIElement>();

	// Token: 0x04002D29 RID: 11561
	public static float BridgeRefreshInterval = 1f;

	// Token: 0x04002D2A RID: 11562
	private List<LogicUtilityNetworkLink>[] bridgeGroups = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x04002D2B RID: 11563
	private bool updateEvenBridgeGroups;

	// Token: 0x04002D2C RID: 11564
	private float timeSinceBridgeRefresh;

	// Token: 0x04002D2D RID: 11565
	public System.Action onLogicTick;

	// Token: 0x04002D2E RID: 11566
	public Action<ILogicUIElement> onElemAdded;

	// Token: 0x04002D2F RID: 11567
	public Action<ILogicUIElement> onElemRemoved;

	// Token: 0x020018AF RID: 6319
	private struct Signal
	{
		// Token: 0x06009999 RID: 39321 RVA: 0x0036AA64 File Offset: 0x00368C64
		public Signal(int cell, int value)
		{
			this.cell = cell;
			this.value = value;
		}

		// Token: 0x04007726 RID: 30502
		public int cell;

		// Token: 0x04007727 RID: 30503
		public int value;
	}
}

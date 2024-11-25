using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000713 RID: 1811
[AddComponentMenu("KMonoBehaviour/scripts/LogicOperationalController")]
public class LogicOperationalController : KMonoBehaviour
{
	// Token: 0x06002F56 RID: 12118 RVA: 0x00108444 File Offset: 0x00106644
	public static List<LogicPorts.Port> CreateSingleInputPortList(CellOffset offset)
	{
		return new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, offset, UI.LOGIC_PORTS.CONTROL_OPERATIONAL, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
		};
	}

	// Token: 0x06002F57 RID: 12119 RVA: 0x00108488 File Offset: 0x00106688
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicOperationalController>(-801688580, LogicOperationalController.OnLogicValueChangedDelegate);
		if (LogicOperationalController.infoStatusItem == null)
		{
			LogicOperationalController.infoStatusItem = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			LogicOperationalController.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicOperationalController.ResolveInfoStatusItemString);
		}
		this.CheckWireState();
	}

	// Token: 0x06002F58 RID: 12120 RVA: 0x001084F8 File Offset: 0x001066F8
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(LogicOperationalController.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06002F59 RID: 12121 RVA: 0x00108528 File Offset: 0x00106728
	private LogicCircuitNetwork CheckWireState()
	{
		LogicCircuitNetwork network = this.GetNetwork();
		int value = (network != null) ? network.OutputValue : this.unNetworkedValue;
		this.operational.SetFlag(LogicOperationalController.LogicOperationalFlag, LogicCircuitNetwork.IsBitActive(0, value));
		return network;
	}

	// Token: 0x06002F5A RID: 12122 RVA: 0x00108566 File Offset: 0x00106766
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		return ((LogicOperationalController)data).operational.GetFlag(LogicOperationalController.LogicOperationalFlag) ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
	}

	// Token: 0x06002F5B RID: 12123 RVA: 0x00108590 File Offset: 0x00106790
	private void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == LogicOperationalController.PORT_ID)
		{
			LogicCircuitNetwork logicCircuitNetwork = this.CheckWireState();
			base.GetComponent<KSelectable>().ToggleStatusItem(LogicOperationalController.infoStatusItem, logicCircuitNetwork != null, this);
		}
	}

	// Token: 0x04001BDD RID: 7133
	public static readonly HashedString PORT_ID = "LogicOperational";

	// Token: 0x04001BDE RID: 7134
	public int unNetworkedValue = 1;

	// Token: 0x04001BDF RID: 7135
	public static readonly Operational.Flag LogicOperationalFlag = new Operational.Flag("LogicOperational", Operational.Flag.Type.Requirement);

	// Token: 0x04001BE0 RID: 7136
	private static StatusItem infoStatusItem;

	// Token: 0x04001BE1 RID: 7137
	[MyCmpGet]
	public Operational operational;

	// Token: 0x04001BE2 RID: 7138
	private static readonly EventSystem.IntraObjectHandler<LogicOperationalController> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicOperationalController>(delegate(LogicOperationalController component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B5F RID: 2911
public class WireUtilityNetworkLink : UtilityNetworkLink, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem, ICircuitConnected
{
	// Token: 0x060056FC RID: 22268 RVA: 0x001F15C8 File Offset: 0x001EF7C8
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x060056FD RID: 22269 RVA: 0x001F15D0 File Offset: 0x001EF7D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060056FE RID: 22270 RVA: 0x001F15D8 File Offset: 0x001EF7D8
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveLink(cell1, cell2);
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x060056FF RID: 22271 RVA: 0x001F15FB File Offset: 0x001EF7FB
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddLink(cell1, cell2);
		Game.Instance.circuitManager.Connect(this);
	}

	// Token: 0x06005700 RID: 22272 RVA: 0x001F161E File Offset: 0x001EF81E
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06005701 RID: 22273 RVA: 0x001F162A File Offset: 0x001EF82A
	// (set) Token: 0x06005702 RID: 22274 RVA: 0x001F1632 File Offset: 0x001EF832
	public bool IsVirtual { get; private set; }

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06005703 RID: 22275 RVA: 0x001F163B File Offset: 0x001EF83B
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06005704 RID: 22276 RVA: 0x001F1643 File Offset: 0x001EF843
	// (set) Token: 0x06005705 RID: 22277 RVA: 0x001F164B File Offset: 0x001EF84B
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06005706 RID: 22278 RVA: 0x001F1654 File Offset: 0x001EF854
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06005707 RID: 22279 RVA: 0x001F1680 File Offset: 0x001EF880
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x040038FC RID: 14588
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}

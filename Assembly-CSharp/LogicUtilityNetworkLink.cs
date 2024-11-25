using System;
using System.Collections.Generic;

// Token: 0x02000950 RID: 2384
public class LogicUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x06004597 RID: 17815 RVA: 0x0018C8EE File Offset: 0x0018AAEE
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06004598 RID: 17816 RVA: 0x0018C8F6 File Offset: 0x0018AAF6
	protected override void OnConnect(int cell1, int cell2)
	{
		this.cell_one = cell1;
		this.cell_two = cell2;
		Game.Instance.logicCircuitSystem.AddLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Connect(this);
	}

	// Token: 0x06004599 RID: 17817 RVA: 0x0018C927 File Offset: 0x0018AB27
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.logicCircuitSystem.RemoveLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Disconnect(this);
	}

	// Token: 0x0600459A RID: 17818 RVA: 0x0018C94A File Offset: 0x0018AB4A
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x0600459B RID: 17819 RVA: 0x0018C958 File Offset: 0x0018AB58
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x0600459C RID: 17820 RVA: 0x0018C984 File Offset: 0x0018AB84
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x04002D56 RID: 11606
	public LogicWire.BitDepth bitDepth;

	// Token: 0x04002D57 RID: 11607
	public int cell_one;

	// Token: 0x04002D58 RID: 11608
	public int cell_two;
}

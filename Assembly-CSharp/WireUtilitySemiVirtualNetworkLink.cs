using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B60 RID: 2912
public class WireUtilitySemiVirtualNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, ICircuitConnected
{
	// Token: 0x06005709 RID: 22281 RVA: 0x001F16B0 File Offset: 0x001EF8B0
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x0600570A RID: 22282 RVA: 0x001F16B8 File Offset: 0x001EF8B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600570B RID: 22283 RVA: 0x001F16C0 File Offset: 0x001EF8C0
	protected override void OnSpawn()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			this.VirtualCircuitKey = component.CraftInterface;
		}
		else
		{
			CraftModuleInterface component2 = this.GetMyWorld().GetComponent<CraftModuleInterface>();
			if (component2 != null)
			{
				this.VirtualCircuitKey = component2;
			}
		}
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(this.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x0600570C RID: 22284 RVA: 0x001F1724 File Offset: 0x001EF924
	public void SetLinkConnected(bool connect)
	{
		if (connect && this.visualizeOnly)
		{
			this.visualizeOnly = false;
			if (base.isSpawned)
			{
				base.Connect();
				return;
			}
		}
		else if (!connect && !this.visualizeOnly)
		{
			if (base.isSpawned)
			{
				base.Disconnect();
			}
			this.visualizeOnly = true;
		}
	}

	// Token: 0x0600570D RID: 22285 RVA: 0x001F1772 File Offset: 0x001EF972
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x0600570E RID: 22286 RVA: 0x001F178A File Offset: 0x001EF98A
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x0600570F RID: 22287 RVA: 0x001F17A2 File Offset: 0x001EF9A2
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06005710 RID: 22288 RVA: 0x001F17AE File Offset: 0x001EF9AE
	// (set) Token: 0x06005711 RID: 22289 RVA: 0x001F17B6 File Offset: 0x001EF9B6
	public bool IsVirtual { get; private set; }

	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06005712 RID: 22290 RVA: 0x001F17BF File Offset: 0x001EF9BF
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06005713 RID: 22291 RVA: 0x001F17C7 File Offset: 0x001EF9C7
	// (set) Token: 0x06005714 RID: 22292 RVA: 0x001F17CF File Offset: 0x001EF9CF
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06005715 RID: 22293 RVA: 0x001F17D8 File Offset: 0x001EF9D8
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06005716 RID: 22294 RVA: 0x001F1804 File Offset: 0x001EFA04
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x040038FF RID: 14591
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}

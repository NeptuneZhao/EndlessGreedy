using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B47 RID: 2887
public class FlowUtilityNetwork : UtilityNetwork
{
	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06005630 RID: 22064 RVA: 0x001ED32A File Offset: 0x001EB52A
	public bool HasSinks
	{
		get
		{
			return this.sinks.Count > 0;
		}
	}

	// Token: 0x06005631 RID: 22065 RVA: 0x001ED33A File Offset: 0x001EB53A
	public int GetActiveCount()
	{
		return this.sinks.Count;
	}

	// Token: 0x06005632 RID: 22066 RVA: 0x001ED348 File Offset: 0x001EB548
	public override void AddItem(object generic_item)
	{
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)generic_item;
		if (item != null)
		{
			switch (item.EndpointType)
			{
			case Endpoint.Source:
				if (this.sources.Contains(item))
				{
					return;
				}
				this.sources.Add(item);
				item.Network = this;
				return;
			case Endpoint.Sink:
				if (this.sinks.Contains(item))
				{
					return;
				}
				this.sinks.Add(item);
				item.Network = this;
				return;
			case Endpoint.Conduit:
				this.conduitCount++;
				return;
			default:
				item.Network = this;
				break;
			}
		}
	}

	// Token: 0x06005633 RID: 22067 RVA: 0x001ED3D8 File Offset: 0x001EB5D8
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		for (int i = 0; i < this.sinks.Count; i++)
		{
			FlowUtilityNetwork.IItem item = this.sinks[i];
			item.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode = grid[item.Cell];
			utilityNetworkGridNode.networkIdx = -1;
			grid[item.Cell] = utilityNetworkGridNode;
		}
		for (int j = 0; j < this.sources.Count; j++)
		{
			FlowUtilityNetwork.IItem item2 = this.sources[j];
			item2.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode2 = grid[item2.Cell];
			utilityNetworkGridNode2.networkIdx = -1;
			grid[item2.Cell] = utilityNetworkGridNode2;
		}
		this.conduitCount = 0;
		for (int k = 0; k < this.conduits.Count; k++)
		{
			FlowUtilityNetwork.IItem item3 = this.conduits[k];
			item3.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode3 = grid[item3.Cell];
			utilityNetworkGridNode3.networkIdx = -1;
			grid[item3.Cell] = utilityNetworkGridNode3;
		}
	}

	// Token: 0x04003873 RID: 14451
	public List<FlowUtilityNetwork.IItem> sources = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04003874 RID: 14452
	public List<FlowUtilityNetwork.IItem> sinks = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04003875 RID: 14453
	public List<FlowUtilityNetwork.IItem> conduits = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04003876 RID: 14454
	public int conduitCount;

	// Token: 0x02001B9E RID: 7070
	public interface IItem
	{
		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x0600A3E5 RID: 41957
		int Cell { get; }

		// Token: 0x17000B42 RID: 2882
		// (set) Token: 0x0600A3E6 RID: 41958
		FlowUtilityNetwork Network { set; }

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x0600A3E7 RID: 41959
		Endpoint EndpointType { get; }

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x0600A3E8 RID: 41960
		ConduitType ConduitType { get; }

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x0600A3E9 RID: 41961
		GameObject GameObject { get; }
	}

	// Token: 0x02001B9F RID: 7071
	public class NetworkItem : FlowUtilityNetwork.IItem
	{
		// Token: 0x0600A3EA RID: 41962 RVA: 0x0038AE35 File Offset: 0x00389035
		public NetworkItem(ConduitType conduit_type, Endpoint endpoint_type, int cell, GameObject parent)
		{
			this.conduitType = conduit_type;
			this.endpointType = endpoint_type;
			this.cell = cell;
			this.parent = parent;
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x0600A3EB RID: 41963 RVA: 0x0038AE5A File Offset: 0x0038905A
		public Endpoint EndpointType
		{
			get
			{
				return this.endpointType;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x0600A3EC RID: 41964 RVA: 0x0038AE62 File Offset: 0x00389062
		public ConduitType ConduitType
		{
			get
			{
				return this.conduitType;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x0600A3ED RID: 41965 RVA: 0x0038AE6A File Offset: 0x0038906A
		public int Cell
		{
			get
			{
				return this.cell;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x0600A3EE RID: 41966 RVA: 0x0038AE72 File Offset: 0x00389072
		// (set) Token: 0x0600A3EF RID: 41967 RVA: 0x0038AE7A File Offset: 0x0038907A
		public FlowUtilityNetwork Network
		{
			get
			{
				return this.network;
			}
			set
			{
				this.network = value;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x0600A3F0 RID: 41968 RVA: 0x0038AE83 File Offset: 0x00389083
		public GameObject GameObject
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x04008039 RID: 32825
		private int cell;

		// Token: 0x0400803A RID: 32826
		private FlowUtilityNetwork network;

		// Token: 0x0400803B RID: 32827
		private Endpoint endpointType;

		// Token: 0x0400803C RID: 32828
		private ConduitType conduitType;

		// Token: 0x0400803D RID: 32829
		private GameObject parent;
	}
}

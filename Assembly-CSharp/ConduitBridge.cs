using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006A4 RID: 1700
[AddComponentMenu("KMonoBehaviour/scripts/ConduitBridge")]
public class ConduitBridge : ConduitBridgeBase, IBridgedNetworkItem
{
	// Token: 0x06002AB2 RID: 10930 RVA: 0x000F0AE5 File Offset: 0x000EECE5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.accumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x000F0B08 File Offset: 0x000EED08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.type).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000F0B57 File Offset: 0x000EED57
	protected override void OnCleanUp()
	{
		Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x000F0B94 File Offset: 0x000EED94
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		float num = contents.mass;
		if (this.desiredMassTransfer != null)
		{
			num = this.desiredMassTransfer(dt, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, null);
		}
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			float num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			if (num2 <= 0f)
			{
				base.SendEmptyOnMassTransfer();
				return;
			}
			flowManager.RemoveElement(this.inputCell, num2);
			Game.Instance.accumulators.Accumulate(this.accumulator, contents.mass);
			if (this.OnMassTransfer != null)
			{
				this.OnMassTransfer(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, null);
				return;
			}
		}
		else
		{
			base.SendEmptyOnMassTransfer();
		}
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000F0CC8 File Offset: 0x000EEEC8
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		UtilityNetwork networkForCell = networkManager.GetNetworkForCell(this.inputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
		networkForCell = networkManager.GetNetworkForCell(this.outputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x000F0D10 File Offset: 0x000EEF10
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		bool flag = false;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		return flag || networks.Contains(networkManager.GetNetworkForCell(this.inputCell)) || networks.Contains(networkManager.GetNetworkForCell(this.outputCell));
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000F0D57 File Offset: 0x000EEF57
	public int GetNetworkCell()
	{
		return this.inputCell;
	}

	// Token: 0x04001892 RID: 6290
	[SerializeField]
	public ConduitType type;

	// Token: 0x04001893 RID: 6291
	private int inputCell;

	// Token: 0x04001894 RID: 6292
	private int outputCell;

	// Token: 0x04001895 RID: 6293
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
}

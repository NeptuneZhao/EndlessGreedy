using System;
using UnityEngine;

// Token: 0x020006A7 RID: 1703
[AddComponentMenu("KMonoBehaviour/scripts/ConduitPreferentialFlow")]
public class ConduitPreferentialFlow : KMonoBehaviour, ISecondaryInput
{
	// Token: 0x06002AC2 RID: 10946 RVA: 0x000F0FB4 File Offset: 0x000EF1B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = component.GetRotatedOffset(this.portInfo.offset);
		int cell2 = Grid.OffsetCell(cell, rotatedOffset);
		Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.secondaryInput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell2, base.gameObject);
		networkManager.AddToNetworks(this.secondaryInput.Cell, this.secondaryInput, true);
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x000F1078 File Offset: 0x000EF278
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInput.Cell, this.secondaryInput, true);
		Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x000F10D4 File Offset: 0x000EF2D4
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		if (!flowManager.HasConduit(this.outputCell))
		{
			return;
		}
		int cell = this.inputCell;
		ConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
		if (contents.mass <= 0f)
		{
			cell = this.secondaryInput.Cell;
			contents = flowManager.GetContents(cell);
		}
		if (contents.mass > 0f)
		{
			float num = flowManager.AddElement(this.outputCell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
			if (num > 0f)
			{
				flowManager.RemoveElement(cell, num);
			}
		}
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x000F117D File Offset: 0x000EF37D
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002AC6 RID: 10950 RVA: 0x000F118D File Offset: 0x000EF38D
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x0400189C RID: 6300
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x0400189D RID: 6301
	private int inputCell;

	// Token: 0x0400189E RID: 6302
	private int outputCell;

	// Token: 0x0400189F RID: 6303
	private FlowUtilityNetwork.NetworkItem secondaryInput;
}

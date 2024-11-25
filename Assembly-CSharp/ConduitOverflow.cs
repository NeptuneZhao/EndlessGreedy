using System;
using UnityEngine;

// Token: 0x020006A6 RID: 1702
[AddComponentMenu("KMonoBehaviour/scripts/ConduitOverflow")]
public class ConduitOverflow : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x06002ABC RID: 10940 RVA: 0x000F0DA4 File Offset: 0x000EEFA4
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
		this.secondaryOutput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell2, base.gameObject);
		networkManager.AddToNetworks(this.secondaryOutput.Cell, this.secondaryOutput, true);
	}

	// Token: 0x06002ABD RID: 10941 RVA: 0x000F0E68 File Offset: 0x000EF068
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryOutput.Cell, this.secondaryOutput, true);
		Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06002ABE RID: 10942 RVA: 0x000F0EC4 File Offset: 0x000EF0C4
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		if (!flowManager.HasConduit(this.inputCell))
		{
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		int cell = this.outputCell;
		ConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		if (contents2.mass > 0f)
		{
			cell = this.secondaryOutput.Cell;
			contents2 = flowManager.GetContents(cell);
		}
		if (contents2.mass <= 0f)
		{
			float num = flowManager.AddElement(cell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
			if (num > 0f)
			{
				flowManager.RemoveElement(this.inputCell, num);
			}
		}
	}

	// Token: 0x06002ABF RID: 10943 RVA: 0x000F0F8C File Offset: 0x000EF18C
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002AC0 RID: 10944 RVA: 0x000F0F9C File Offset: 0x000EF19C
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		return this.portInfo.offset;
	}

	// Token: 0x04001898 RID: 6296
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04001899 RID: 6297
	private int inputCell;

	// Token: 0x0400189A RID: 6298
	private int outputCell;

	// Token: 0x0400189B RID: 6299
	private FlowUtilityNetwork.NetworkItem secondaryOutput;
}

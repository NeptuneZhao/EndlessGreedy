using System;
using UnityEngine;

// Token: 0x02000670 RID: 1648
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConduitEndpoints")]
public class BuildingConduitEndpoints : KMonoBehaviour
{
	// Token: 0x060028C0 RID: 10432 RVA: 0x000E6EAB File Offset: 0x000E50AB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.AddEndpoint();
	}

	// Token: 0x060028C1 RID: 10433 RVA: 0x000E6EB9 File Offset: 0x000E50B9
	protected override void OnCleanUp()
	{
		this.RemoveEndPoint();
		base.OnCleanUp();
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x000E6EC8 File Offset: 0x000E50C8
	public void RemoveEndPoint()
	{
		if (this.itemInput != null)
		{
			if (this.itemInput.ConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemInput.Cell, this.itemInput, true);
			}
			else
			{
				Conduit.GetNetworkManager(this.itemInput.ConduitType).RemoveFromNetworks(this.itemInput.Cell, this.itemInput, true);
			}
			this.itemInput = null;
		}
		if (this.itemOutput != null)
		{
			if (this.itemOutput.ConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemOutput.Cell, this.itemOutput, true);
			}
			else
			{
				Conduit.GetNetworkManager(this.itemOutput.ConduitType).RemoveFromNetworks(this.itemOutput.Cell, this.itemOutput, true);
			}
			this.itemOutput = null;
		}
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x000E6FA4 File Offset: 0x000E51A4
	public void AddEndpoint()
	{
		Building component = base.GetComponent<Building>();
		BuildingDef def = component.Def;
		this.RemoveEndPoint();
		if (def.InputConduitType != ConduitType.None)
		{
			int utilityInputCell = component.GetUtilityInputCell();
			this.itemInput = new FlowUtilityNetwork.NetworkItem(def.InputConduitType, Endpoint.Sink, utilityInputCell, base.gameObject);
			if (def.InputConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.AddToNetworks(utilityInputCell, this.itemInput, true);
			}
			else
			{
				Conduit.GetNetworkManager(def.InputConduitType).AddToNetworks(utilityInputCell, this.itemInput, true);
			}
		}
		if (def.OutputConduitType != ConduitType.None)
		{
			int utilityOutputCell = component.GetUtilityOutputCell();
			this.itemOutput = new FlowUtilityNetwork.NetworkItem(def.OutputConduitType, Endpoint.Source, utilityOutputCell, base.gameObject);
			if (def.OutputConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.AddToNetworks(utilityOutputCell, this.itemOutput, true);
				return;
			}
			Conduit.GetNetworkManager(def.OutputConduitType).AddToNetworks(utilityOutputCell, this.itemOutput, true);
		}
	}

	// Token: 0x04001764 RID: 5988
	private FlowUtilityNetwork.NetworkItem itemInput;

	// Token: 0x04001765 RID: 5989
	private FlowUtilityNetwork.NetworkItem itemOutput;
}

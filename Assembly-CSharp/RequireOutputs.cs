using System;
using UnityEngine;

// Token: 0x02000A4B RID: 2635
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireOutputs")]
public class RequireOutputs : KMonoBehaviour
{
	// Token: 0x06004C5F RID: 19551 RVA: 0x001B4448 File Offset: 0x001B2648
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ScenePartitionerLayer scenePartitionerLayer = null;
		Building component = base.GetComponent<Building>();
		this.utilityCell = component.GetUtilityOutputCell();
		this.conduitType = component.Def.OutputConduitType;
		switch (component.Def.OutputConduitType)
		{
		case ConduitType.Gas:
			scenePartitionerLayer = GameScenePartitioner.Instance.gasConduitsLayer;
			break;
		case ConduitType.Liquid:
			scenePartitionerLayer = GameScenePartitioner.Instance.liquidConduitsLayer;
			break;
		case ConduitType.Solid:
			scenePartitionerLayer = GameScenePartitioner.Instance.solidConduitsLayer;
			break;
		}
		this.UpdateConnectionState(true);
		this.UpdatePipeRoomState(true);
		if (scenePartitionerLayer != null)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("RequireOutputs", base.gameObject, this.utilityCell, scenePartitionerLayer, delegate(object data)
			{
				this.UpdateConnectionState(false);
			});
		}
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.UpdatePipeState), ConduitFlowPriority.First);
	}

	// Token: 0x06004C60 RID: 19552 RVA: 0x001B4520 File Offset: 0x001B2720
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		IConduitFlow conduitFlow = this.GetConduitFlow();
		if (conduitFlow != null)
		{
			conduitFlow.RemoveConduitUpdater(new Action<float>(this.UpdatePipeState));
		}
		base.OnCleanUp();
	}

	// Token: 0x06004C61 RID: 19553 RVA: 0x001B4560 File Offset: 0x001B2760
	private void UpdateConnectionState(bool force_update = false)
	{
		this.connected = this.IsConnected(this.utilityCell);
		if (this.connected != this.previouslyConnected || force_update)
		{
			this.operational.SetFlag(RequireOutputs.outputConnectedFlag, this.connected);
			this.previouslyConnected = this.connected;
			StatusItem status_item = null;
			switch (this.conduitType)
			{
			case ConduitType.Gas:
				status_item = Db.Get().BuildingStatusItems.NeedGasOut;
				break;
			case ConduitType.Liquid:
				status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
				break;
			case ConduitType.Solid:
				status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
				break;
			}
			this.hasPipeGuid = this.selectable.ToggleStatusItem(status_item, this.hasPipeGuid, !this.connected, this);
		}
	}

	// Token: 0x06004C62 RID: 19554 RVA: 0x001B4630 File Offset: 0x001B2830
	private bool OutputPipeIsEmpty()
	{
		if (this.ignoreFullPipe)
		{
			return true;
		}
		bool result = true;
		if (this.connected)
		{
			result = this.GetConduitFlow().IsConduitEmpty(this.utilityCell);
		}
		return result;
	}

	// Token: 0x06004C63 RID: 19555 RVA: 0x001B4664 File Offset: 0x001B2864
	private void UpdatePipeState(float dt)
	{
		this.UpdatePipeRoomState(false);
	}

	// Token: 0x06004C64 RID: 19556 RVA: 0x001B4670 File Offset: 0x001B2870
	private void UpdatePipeRoomState(bool force_update = false)
	{
		bool flag = this.OutputPipeIsEmpty();
		if (flag != this.previouslyHadRoom || force_update)
		{
			this.operational.SetFlag(RequireOutputs.pipesHaveRoomFlag, flag);
			this.previouslyHadRoom = flag;
			StatusItem status_item = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
			if (this.conduitType == ConduitType.Solid)
			{
				status_item = Db.Get().BuildingStatusItems.SolidConduitBlockedMultiples;
			}
			this.pipeBlockedGuid = this.selectable.ToggleStatusItem(status_item, this.pipeBlockedGuid, !flag, null);
		}
	}

	// Token: 0x06004C65 RID: 19557 RVA: 0x001B46F4 File Offset: 0x001B28F4
	private IConduitFlow GetConduitFlow()
	{
		switch (this.conduitType)
		{
		case ConduitType.Gas:
			return Game.Instance.gasConduitFlow;
		case ConduitType.Liquid:
			return Game.Instance.liquidConduitFlow;
		case ConduitType.Solid:
			return Game.Instance.solidConduitFlow;
		default:
			global::Debug.LogWarning("GetConduitFlow() called with unexpected conduitType: " + this.conduitType.ToString());
			return null;
		}
	}

	// Token: 0x06004C66 RID: 19558 RVA: 0x001B4760 File Offset: 0x001B2960
	private bool IsConnected(int cell)
	{
		return RequireOutputs.IsConnected(cell, this.conduitType);
	}

	// Token: 0x06004C67 RID: 19559 RVA: 0x001B4770 File Offset: 0x001B2970
	public static bool IsConnected(int cell, ConduitType conduitType)
	{
		ObjectLayer layer = ObjectLayer.NumLayers;
		switch (conduitType)
		{
		case ConduitType.Gas:
			layer = ObjectLayer.GasConduit;
			break;
		case ConduitType.Liquid:
			layer = ObjectLayer.LiquidConduit;
			break;
		case ConduitType.Solid:
			layer = ObjectLayer.SolidConduit;
			break;
		}
		GameObject gameObject = Grid.Objects[cell, (int)layer];
		return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
	}

	// Token: 0x040032D0 RID: 13008
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040032D1 RID: 13009
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040032D2 RID: 13010
	public bool ignoreFullPipe;

	// Token: 0x040032D3 RID: 13011
	private int utilityCell;

	// Token: 0x040032D4 RID: 13012
	private ConduitType conduitType;

	// Token: 0x040032D5 RID: 13013
	private static readonly Operational.Flag outputConnectedFlag = new Operational.Flag("output_connected", Operational.Flag.Type.Requirement);

	// Token: 0x040032D6 RID: 13014
	private static readonly Operational.Flag pipesHaveRoomFlag = new Operational.Flag("pipesHaveRoom", Operational.Flag.Type.Requirement);

	// Token: 0x040032D7 RID: 13015
	private bool previouslyConnected = true;

	// Token: 0x040032D8 RID: 13016
	private bool previouslyHadRoom = true;

	// Token: 0x040032D9 RID: 13017
	private bool connected;

	// Token: 0x040032DA RID: 13018
	private Guid hasPipeGuid;

	// Token: 0x040032DB RID: 13019
	private Guid pipeBlockedGuid;

	// Token: 0x040032DC RID: 13020
	private HandleVector<int>.Handle partitionerEntry;
}

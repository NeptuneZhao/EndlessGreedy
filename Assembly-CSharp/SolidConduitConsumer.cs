using System;
using UnityEngine;

// Token: 0x02000AA9 RID: 2729
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitConsumer")]
public class SolidConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x0600504E RID: 20558 RVA: 0x001CD987 File Offset: 0x001CBB87
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x0600504F RID: 20559 RVA: 0x001CD98F File Offset: 0x001CBB8F
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06005050 RID: 20560 RVA: 0x001CD992 File Offset: 0x001CBB92
	public bool IsConsuming
	{
		get
		{
			return this.consuming;
		}
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x06005051 RID: 20561 RVA: 0x001CD99C File Offset: 0x001CBB9C
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x06005052 RID: 20562 RVA: 0x001CD9D3 File Offset: 0x001CBBD3
	private SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x06005053 RID: 20563 RVA: 0x001CD9E0 File Offset: 0x001CBBE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetInputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06005054 RID: 20564 RVA: 0x001CDA5A File Offset: 0x001CBC5A
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005055 RID: 20565 RVA: 0x001CDA89 File Offset: 0x001CBC89
	private void OnConduitConnectionChanged(object data)
	{
		this.consuming = (this.consuming && this.IsConnected);
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06005056 RID: 20566 RVA: 0x001CDAB8 File Offset: 0x001CBCB8
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		SolidConduitFlow conduitFlow = this.GetConduitFlow();
		if (this.IsConnected)
		{
			SolidConduitFlow.ConduitContents contents = conduitFlow.GetContents(this.utilityCell);
			if (contents.pickupableHandle.IsValid() && (this.alwaysConsume || this.operational.IsOperational))
			{
				float num = (this.capacityTag != GameTags.Any) ? this.storage.GetMassAvailable(this.capacityTag) : this.storage.MassStored();
				float num2 = Mathf.Min(this.storage.capacityKg, this.capacityKG);
				float num3 = Mathf.Max(0f, num2 - num);
				if (num3 > 0f)
				{
					Pickupable pickupable = conduitFlow.GetPickupable(contents.pickupableHandle);
					if (pickupable.PrimaryElement.Mass <= num3 || pickupable.PrimaryElement.Mass > num2)
					{
						Pickupable pickupable2 = conduitFlow.RemovePickupable(this.utilityCell);
						if (pickupable2)
						{
							this.storage.Store(pickupable2.gameObject, true, false, true, false);
							flag = true;
						}
					}
				}
			}
		}
		if (this.storage != null)
		{
			this.storage.storageNetworkID = this.GetConnectedNetworkID();
		}
		this.consuming = flag;
	}

	// Token: 0x06005057 RID: 20567 RVA: 0x001CDBF8 File Offset: 0x001CBDF8
	private int GetConnectedNetworkID()
	{
		GameObject gameObject = Grid.Objects[this.utilityCell, 20];
		SolidConduit solidConduit = (gameObject != null) ? gameObject.GetComponent<SolidConduit>() : null;
		UtilityNetwork utilityNetwork = (solidConduit != null) ? solidConduit.GetNetwork() : null;
		if (utilityNetwork == null)
		{
			return -1;
		}
		return utilityNetwork.id;
	}

	// Token: 0x06005058 RID: 20568 RVA: 0x001CDC4C File Offset: 0x001CBE4C
	private int GetInputCell()
	{
		if (this.useSecondaryInput)
		{
			foreach (ISecondaryInput secondaryInput in base.GetComponents<ISecondaryInput>())
			{
				if (secondaryInput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), CellOffset.none);
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x0400355C RID: 13660
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x0400355D RID: 13661
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x0400355E RID: 13662
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x0400355F RID: 13663
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x04003560 RID: 13664
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003561 RID: 13665
	[MyCmpReq]
	private Building building;

	// Token: 0x04003562 RID: 13666
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04003563 RID: 13667
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003564 RID: 13668
	private int utilityCell = -1;

	// Token: 0x04003565 RID: 13669
	private bool consuming;
}

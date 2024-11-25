using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AAA RID: 2730
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitDispenser")]
public class SolidConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x0600505A RID: 20570 RVA: 0x001CDCE1 File Offset: 0x001CBEE1
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x0600505B RID: 20571 RVA: 0x001CDCE9 File Offset: 0x001CBEE9
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x0600505C RID: 20572 RVA: 0x001CDCEC File Offset: 0x001CBEEC
	public SolidConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitFlow().GetContents(this.utilityCell);
		}
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x0600505D RID: 20573 RVA: 0x001CDCFF File Offset: 0x001CBEFF
	public bool IsDispensing
	{
		get
		{
			return this.dispensing;
		}
	}

	// Token: 0x0600505E RID: 20574 RVA: 0x001CDD07 File Offset: 0x001CBF07
	public SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x0600505F RID: 20575 RVA: 0x001CDD14 File Offset: 0x001CBF14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetOutputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06005060 RID: 20576 RVA: 0x001CDD8F File Offset: 0x001CBF8F
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005061 RID: 20577 RVA: 0x001CDDBE File Offset: 0x001CBFBE
	private void OnConduitConnectionChanged(object data)
	{
		this.dispensing = (this.dispensing && this.IsConnected);
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06005062 RID: 20578 RVA: 0x001CDDF0 File Offset: 0x001CBFF0
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		this.operational.SetFlag(SolidConduitDispenser.outputConduitFlag, this.IsConnected);
		if (this.operational.IsOperational || this.alwaysDispense)
		{
			SolidConduitFlow conduitFlow = this.GetConduitFlow();
			if (conduitFlow.HasConduit(this.utilityCell) && conduitFlow.IsConduitEmpty(this.utilityCell))
			{
				Pickupable pickupable = this.FindSuitableItem();
				if (pickupable)
				{
					if (pickupable.PrimaryElement.Mass > 20f)
					{
						pickupable = pickupable.Take(20f);
					}
					conduitFlow.AddPickupable(this.utilityCell, pickupable);
					flag = true;
				}
			}
		}
		this.storage.storageNetworkID = this.GetConnectedNetworkID();
		this.dispensing = flag;
	}

	// Token: 0x06005063 RID: 20579 RVA: 0x001CDEA4 File Offset: 0x001CC0A4
	private bool isSolid(GameObject o)
	{
		PrimaryElement component = o.GetComponent<PrimaryElement>();
		return component == null || component.Element.IsLiquid || component.Element.IsGas;
	}

	// Token: 0x06005064 RID: 20580 RVA: 0x001CDEDC File Offset: 0x001CC0DC
	private Pickupable FindSuitableItem()
	{
		List<GameObject> list = this.storage.items;
		if (this.solidOnly)
		{
			List<GameObject> list2 = new List<GameObject>(list);
			list2.RemoveAll(new Predicate<GameObject>(this.isSolid));
			list = list2;
		}
		if (list.Count < 1)
		{
			return null;
		}
		this.round_robin_index %= list.Count;
		GameObject gameObject = list[this.round_robin_index];
		this.round_robin_index++;
		if (!gameObject)
		{
			return null;
		}
		return gameObject.GetComponent<Pickupable>();
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06005065 RID: 20581 RVA: 0x001CDF60 File Offset: 0x001CC160
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x06005066 RID: 20582 RVA: 0x001CDF98 File Offset: 0x001CC198
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

	// Token: 0x06005067 RID: 20583 RVA: 0x001CDFEC File Offset: 0x001CC1EC
	private int GetOutputCell()
	{
		Building component = base.GetComponent<Building>();
		if (this.useSecondaryOutput)
		{
			foreach (ISecondaryOutput secondaryOutput in base.GetComponents<ISecondaryOutput>())
			{
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), CellOffset.none);
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x04003566 RID: 13670
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x04003567 RID: 13671
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x04003568 RID: 13672
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x04003569 RID: 13673
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x0400356A RID: 13674
	[SerializeField]
	public bool solidOnly;

	// Token: 0x0400356B RID: 13675
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x0400356C RID: 13676
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400356D RID: 13677
	[MyCmpReq]
	public Storage storage;

	// Token: 0x0400356E RID: 13678
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400356F RID: 13679
	private int utilityCell = -1;

	// Token: 0x04003570 RID: 13680
	private bool dispensing;

	// Token: 0x04003571 RID: 13681
	private int round_robin_index;
}

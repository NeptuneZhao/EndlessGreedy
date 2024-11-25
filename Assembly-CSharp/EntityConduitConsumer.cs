using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000882 RID: 2178
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SpawnableConduitConsumer")]
public class EntityConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x06003D1A RID: 15642 RVA: 0x001522A8 File Offset: 0x001504A8
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06003D1B RID: 15643 RVA: 0x001522B0 File Offset: 0x001504B0
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06003D1C RID: 15644 RVA: 0x001522B8 File Offset: 0x001504B8
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null;
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06003D1D RID: 15645 RVA: 0x001522E0 File Offset: 0x001504E0
	public bool CanConsume
	{
		get
		{
			bool result = false;
			if (this.IsConnected)
			{
				result = (this.GetConduitManager().GetContents(this.utilityCell).mass > 0f);
			}
			return result;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06003D1E RID: 15646 RVA: 0x0015231C File Offset: 0x0015051C
	public float stored_mass
	{
		get
		{
			if (this.storage == null)
			{
				return 0f;
			}
			if (!(this.capacityTag != GameTags.Any))
			{
				return this.storage.MassStored();
			}
			return this.storage.GetMassAvailable(this.capacityTag);
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06003D1F RID: 15647 RVA: 0x0015236C File Offset: 0x0015056C
	public float space_remaining_kg
	{
		get
		{
			float num = this.capacityKG - this.stored_mass;
			if (!(this.storage == null))
			{
				return Mathf.Min(this.storage.RemainingCapacity(), num);
			}
			return num;
		}
	}

	// Token: 0x06003D20 RID: 15648 RVA: 0x001523A8 File Offset: 0x001505A8
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06003D21 RID: 15649 RVA: 0x001523B1 File Offset: 0x001505B1
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x06003D22 RID: 15650 RVA: 0x001523B9 File Offset: 0x001505B9
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x06003D23 RID: 15651 RVA: 0x001523D9 File Offset: 0x001505D9
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06003D24 RID: 15652 RVA: 0x00152402 File Offset: 0x00150602
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x06003D25 RID: 15653 RVA: 0x0015240A File Offset: 0x0015060A
	// (set) Token: 0x06003D26 RID: 15654 RVA: 0x0015241F File Offset: 0x0015061F
	public bool IsSatisfied
	{
		get
		{
			return this.satisfied || !this.isConsuming;
		}
		set
		{
			this.satisfied = (value || this.forceAlwaysSatisfied);
		}
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x00152434 File Offset: 0x00150634
	private ConduitFlow GetConduitManager()
	{
		ConduitType conduitType = this.conduitType;
		if (conduitType == ConduitType.Gas)
		{
			return Game.Instance.gasConduitFlow;
		}
		if (conduitType != ConduitType.Liquid)
		{
			return null;
		}
		return Game.Instance.liquidConduitFlow;
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x06003D28 RID: 15656 RVA: 0x0015246C File Offset: 0x0015066C
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x06003D29 RID: 15657 RVA: 0x0015249C File Offset: 0x0015069C
	private int GetInputCell(ConduitType inputConduitType)
	{
		return this.occupyArea.GetOffsetCellWithRotation(this.offset);
	}

	// Token: 0x06003D2A RID: 15658 RVA: 0x001524B0 File Offset: 0x001506B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.endpoint = new FlowUtilityNetwork.NetworkItem(conduitManager.conduitType, Endpoint.Sink, this.utilityCell, base.gameObject);
		if (conduitManager.conduitType == ConduitType.Solid)
		{
			Game.Instance.solidConduitSystem.AddToNetworks(this.utilityCell, this.endpoint, true);
		}
		else
		{
			Conduit.GetNetworkManager(conduitManager.conduitType).AddToNetworks(this.utilityCell, this.endpoint, true);
		}
		EntityCellVisualizer.Ports type = EntityCellVisualizer.Ports.LiquidIn;
		if (conduitManager.conduitType == ConduitType.Solid)
		{
			type = EntityCellVisualizer.Ports.SolidIn;
		}
		else if (conduitManager.conduitType == ConduitType.Gas)
		{
			type = EntityCellVisualizer.Ports.GasIn;
		}
		this.cellVisualizer.AddPort(type, this.offset);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x001525D4 File Offset: 0x001507D4
	protected override void OnCleanUp()
	{
		if (this.endpoint.ConduitType == ConduitType.Solid)
		{
			Game.Instance.solidConduitSystem.RemoveFromNetworks(this.endpoint.Cell, this.endpoint, true);
		}
		else
		{
			Conduit.GetNetworkManager(this.endpoint.ConduitType).RemoveFromNetworks(this.endpoint.Cell, this.endpoint, true);
		}
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003D2C RID: 15660 RVA: 0x00152666 File Offset: 0x00150866
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06003D2D RID: 15661 RVA: 0x0015267E File Offset: 0x0015087E
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x06003D2E RID: 15662 RVA: 0x00152688 File Offset: 0x00150888
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x06003D2F RID: 15663 RVA: 0x001526B4 File Offset: 0x001508B4
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
		if (!this.IsConnected)
		{
			return;
		}
		ConduitFlow.ConduitContents contents = conduit_mgr.GetContents(this.utilityCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		this.IsSatisfied = true;
		if (!this.alwaysConsume && !this.operational.MeetsRequirements(this.OperatingRequirement))
		{
			return;
		}
		float num = this.ConsumptionRate * dt;
		num = Mathf.Min(num, this.space_remaining_kg);
		Element element = ElementLoader.FindElementByHash(contents.element);
		if (contents.element != this.lastConsumedElement)
		{
			DiscoveredResources.Instance.Discover(element.tag, element.materialCategory);
		}
		float num2 = 0f;
		if (num > 0f)
		{
			ConduitFlow.ConduitContents conduitContents = conduit_mgr.RemoveElement(this.utilityCell, num);
			num2 = conduitContents.mass;
			this.lastConsumedElement = conduitContents.element;
		}
		bool flag = element.HasTag(this.capacityTag);
		if (num2 > 0f && this.capacityTag != GameTags.Any && !flag)
		{
			base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
			});
		}
		if (flag || this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
		{
			if (num2 > 0f)
			{
				this.consumedLastTick = true;
				int disease_count = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				Element element2 = ElementLoader.FindElementByHash(contents.element);
				ConduitType conduitType = this.conduitType;
				if (conduitType != ConduitType.Gas)
				{
					if (conduitType == ConduitType.Liquid)
					{
						if (element2.IsLiquid)
						{
							this.storage.AddLiquid(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
							return;
						}
						global::Debug.LogWarning("Liquid conduit consumer consuming non liquid: " + element2.id.ToString());
						return;
					}
				}
				else
				{
					if (element2.IsGas)
					{
						this.storage.AddGasChunk(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
						return;
					}
					global::Debug.LogWarning("Gas conduit consumer consuming non gas: " + element2.id.ToString());
					return;
				}
			}
		}
		else if (num2 > 0f)
		{
			this.consumedLastTick = true;
			if (this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x04002545 RID: 9541
	private FlowUtilityNetwork.NetworkItem endpoint;

	// Token: 0x04002546 RID: 9542
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04002547 RID: 9543
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x04002548 RID: 9544
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x04002549 RID: 9545
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x0400254A RID: 9546
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x0400254B RID: 9547
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x0400254C RID: 9548
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x0400254D RID: 9549
	[SerializeField]
	public bool isOn = true;

	// Token: 0x0400254E RID: 9550
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x0400254F RID: 9551
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x04002550 RID: 9552
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04002551 RID: 9553
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x04002552 RID: 9554
	[MyCmpReq]
	private EntityCellVisualizer cellVisualizer;

	// Token: 0x04002553 RID: 9555
	public Operational.State OperatingRequirement;

	// Token: 0x04002554 RID: 9556
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04002555 RID: 9557
	public CellOffset offset;

	// Token: 0x04002556 RID: 9558
	private int utilityCell = -1;

	// Token: 0x04002557 RID: 9559
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x04002558 RID: 9560
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x04002559 RID: 9561
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400255A RID: 9562
	private bool satisfied;

	// Token: 0x0400255B RID: 9563
	public EntityConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x02001789 RID: 6025
	public enum WrongElementResult
	{
		// Token: 0x04007307 RID: 29447
		Destroy,
		// Token: 0x04007308 RID: 29448
		Dump,
		// Token: 0x04007309 RID: 29449
		Store
	}
}

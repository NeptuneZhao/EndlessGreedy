using System;
using STRINGS;
using UnityEngine;

// Token: 0x020007D8 RID: 2008
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitConsumer")]
public class ConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06003755 RID: 14165 RVA: 0x0012DC6E File Offset: 0x0012BE6E
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06003756 RID: 14166 RVA: 0x0012DC76 File Offset: 0x0012BE76
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06003757 RID: 14167 RVA: 0x0012DC7E File Offset: 0x0012BE7E
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null && this.m_buildingComplete != null;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06003758 RID: 14168 RVA: 0x0012DCB8 File Offset: 0x0012BEB8
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

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06003759 RID: 14169 RVA: 0x0012DCF4 File Offset: 0x0012BEF4
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

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x0600375A RID: 14170 RVA: 0x0012DD44 File Offset: 0x0012BF44
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

	// Token: 0x0600375B RID: 14171 RVA: 0x0012DD80 File Offset: 0x0012BF80
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x0600375C RID: 14172 RVA: 0x0012DD89 File Offset: 0x0012BF89
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x0600375D RID: 14173 RVA: 0x0012DD91 File Offset: 0x0012BF91
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x0600375E RID: 14174 RVA: 0x0012DDB1 File Offset: 0x0012BFB1
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x0600375F RID: 14175 RVA: 0x0012DDDA File Offset: 0x0012BFDA
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06003760 RID: 14176 RVA: 0x0012DDE2 File Offset: 0x0012BFE2
	// (set) Token: 0x06003761 RID: 14177 RVA: 0x0012DDF7 File Offset: 0x0012BFF7
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

	// Token: 0x06003762 RID: 14178 RVA: 0x0012DE0C File Offset: 0x0012C00C
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

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06003763 RID: 14179 RVA: 0x0012DE44 File Offset: 0x0012C044
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x06003764 RID: 14180 RVA: 0x0012DE74 File Offset: 0x0012C074
	protected virtual int GetInputCell(ConduitType inputConduitType)
	{
		if (this.useSecondaryInput)
		{
			ISecondaryInput[] components = base.GetComponents<ISecondaryInput>();
			foreach (ISecondaryInput secondaryInput in components)
			{
				if (secondaryInput.HasSecondaryConduitType(inputConduitType))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(inputConduitType));
				}
			}
			global::Debug.LogWarning("No secondaryInput of type was found");
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(inputConduitType));
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x06003765 RID: 14181 RVA: 0x0012DEF4 File Offset: 0x0012C0F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06003766 RID: 14182 RVA: 0x0012DFBE File Offset: 0x0012C1BE
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003767 RID: 14183 RVA: 0x0012DFED File Offset: 0x0012C1ED
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06003768 RID: 14184 RVA: 0x0012E005 File Offset: 0x0012C205
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x06003769 RID: 14185 RVA: 0x0012E010 File Offset: 0x0012C210
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x0600376A RID: 14186 RVA: 0x0012E03C File Offset: 0x0012C23C
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		if (this.building.Def.CanMove)
		{
			this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
		}
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
		if (flag || this.wrongElementResult == ConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
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
			if (this.wrongElementResult == ConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x04002150 RID: 8528
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04002151 RID: 8529
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x04002152 RID: 8530
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x04002153 RID: 8531
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x04002154 RID: 8532
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x04002155 RID: 8533
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x04002156 RID: 8534
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x04002157 RID: 8535
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x04002158 RID: 8536
	[SerializeField]
	public bool isOn = true;

	// Token: 0x04002159 RID: 8537
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x0400215A RID: 8538
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x0400215B RID: 8539
	[MyCmpReq]
	public Operational operational;

	// Token: 0x0400215C RID: 8540
	[MyCmpReq]
	protected Building building;

	// Token: 0x0400215D RID: 8541
	public Operational.State OperatingRequirement;

	// Token: 0x0400215E RID: 8542
	public ISecondaryInput targetSecondaryInput;

	// Token: 0x0400215F RID: 8543
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04002160 RID: 8544
	[MyCmpGet]
	private BuildingComplete m_buildingComplete;

	// Token: 0x04002161 RID: 8545
	private int utilityCell = -1;

	// Token: 0x04002162 RID: 8546
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x04002163 RID: 8547
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x04002164 RID: 8548
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002165 RID: 8549
	private bool satisfied;

	// Token: 0x04002166 RID: 8550
	public ConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x0200169C RID: 5788
	public enum WrongElementResult
	{
		// Token: 0x0400702C RID: 28716
		Destroy,
		// Token: 0x0400702D RID: 28717
		Dump,
		// Token: 0x0400702E RID: 28718
		Store
	}
}

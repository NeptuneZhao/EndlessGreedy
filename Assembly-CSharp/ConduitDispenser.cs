using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020007DB RID: 2011
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitDispenser")]
public class ConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x06003775 RID: 14197 RVA: 0x0012E65F File Offset: 0x0012C85F
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06003776 RID: 14198 RVA: 0x0012E667 File Offset: 0x0012C867
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06003777 RID: 14199 RVA: 0x0012E66F File Offset: 0x0012C86F
	public ConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitManager().GetContents(this.utilityCell);
		}
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x0012E682 File Offset: 0x0012C882
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x0012E68C File Offset: 0x0012C88C
	public ConduitFlow GetConduitManager()
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

	// Token: 0x0600377A RID: 14202 RVA: 0x0012E6C1 File Offset: 0x0012C8C1
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x0012E6DC File Offset: 0x0012C8DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetOutputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x0012E7A7 File Offset: 0x0012C9A7
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x0012E7D6 File Offset: 0x0012C9D6
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x0012E7DF File Offset: 0x0012C9DF
	private void ConduitUpdate(float dt)
	{
		if (this.operational != null)
		{
			this.operational.SetFlag(ConduitDispenser.outputConduitFlag, this.IsConnected);
		}
		this.blocked = false;
		if (this.isOn)
		{
			this.Dispense(dt);
		}
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x0012E81C File Offset: 0x0012CA1C
	private void Dispense(float dt)
	{
		if ((this.operational != null && this.operational.IsOperational) || this.alwaysDispense)
		{
			if (this.building != null && this.building.Def.CanMove)
			{
				this.utilityCell = this.GetOutputCell(this.GetConduitManager().conduitType);
			}
			PrimaryElement primaryElement = this.FindSuitableElement();
			if (primaryElement != null)
			{
				primaryElement.KeepZeroMassObject = true;
				this.empty = false;
				float num = this.GetConduitManager().AddElement(this.utilityCell, primaryElement.ElementID, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount);
				if (num > 0f)
				{
					int num2 = (int)(num / primaryElement.Mass * (float)primaryElement.DiseaseCount);
					primaryElement.ModifyDiseaseCount(-num2, "ConduitDispenser.ConduitUpdate");
					primaryElement.Mass -= num;
					this.storage.Trigger(-1697596308, primaryElement.gameObject);
					return;
				}
				this.blocked = true;
				return;
			}
			else
			{
				this.empty = true;
			}
		}
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x0012E934 File Offset: 0x0012CB34
	private PrimaryElement FindSuitableElement()
	{
		List<GameObject> items = this.storage.items;
		int count = items.Count;
		for (int i = 0; i < count; i++)
		{
			int index = (i + this.elementOutputOffset) % count;
			PrimaryElement component = items[index].GetComponent<PrimaryElement>();
			if (component != null && component.Mass > 0f && ((this.conduitType == ConduitType.Liquid) ? component.Element.IsLiquid : component.Element.IsGas) && (this.elementFilter == null || this.elementFilter.Length == 0 || (!this.invertElementFilter && this.IsFilteredElement(component.ElementID)) || (this.invertElementFilter && !this.IsFilteredElement(component.ElementID))))
			{
				this.elementOutputOffset = (this.elementOutputOffset + 1) % count;
				return component;
			}
		}
		return null;
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x0012EA14 File Offset: 0x0012CC14
	private bool IsFilteredElement(SimHashes element)
	{
		for (int num = 0; num != this.elementFilter.Length; num++)
		{
			if (this.elementFilter[num] == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06003782 RID: 14210 RVA: 0x0012EA44 File Offset: 0x0012CC44
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x06003783 RID: 14211 RVA: 0x0012EA88 File Offset: 0x0012CC88
	private int GetOutputCell(ConduitType outputConduitType)
	{
		Building component = base.GetComponent<Building>();
		if (!(component != null))
		{
			return Grid.OffsetCell(Grid.PosToCell(this), this.noBuildingOutputCellOffset);
		}
		if (this.useSecondaryOutput)
		{
			ISecondaryOutput[] components = base.GetComponents<ISecondaryOutput>();
			foreach (ISecondaryOutput secondaryOutput in components)
			{
				if (secondaryOutput.HasSecondaryConduitType(outputConduitType))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(outputConduitType));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(outputConduitType));
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x04002168 RID: 8552
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04002169 RID: 8553
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x0400216A RID: 8554
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x0400216B RID: 8555
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x0400216C RID: 8556
	[SerializeField]
	public bool isOn = true;

	// Token: 0x0400216D RID: 8557
	[SerializeField]
	public bool blocked;

	// Token: 0x0400216E RID: 8558
	[SerializeField]
	public bool empty = true;

	// Token: 0x0400216F RID: 8559
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x04002170 RID: 8560
	[SerializeField]
	public CellOffset noBuildingOutputCellOffset;

	// Token: 0x04002171 RID: 8561
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x04002172 RID: 8562
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002173 RID: 8563
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04002174 RID: 8564
	[MyCmpGet]
	private Building building;

	// Token: 0x04002175 RID: 8565
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002176 RID: 8566
	private int utilityCell = -1;

	// Token: 0x04002177 RID: 8567
	private int elementOutputOffset;
}

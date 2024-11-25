using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020008CF RID: 2255
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Generator")]
public class Generator : KMonoBehaviour, ISaveLoadable, IEnergyProducer, ICircuitConnected
{
	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06004006 RID: 16390 RVA: 0x0016AD9F File Offset: 0x00168F9F
	public int PowerDistributionOrder
	{
		get
		{
			return this.powerDistributionOrder;
		}
	}

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06004007 RID: 16391 RVA: 0x0016ADA7 File Offset: 0x00168FA7
	public virtual float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06004008 RID: 16392 RVA: 0x0016ADAF File Offset: 0x00168FAF
	public virtual bool IsEmpty
	{
		get
		{
			return this.joulesAvailable <= 0f;
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06004009 RID: 16393 RVA: 0x0016ADC1 File Offset: 0x00168FC1
	public virtual float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x0600400A RID: 16394 RVA: 0x0016ADC9 File Offset: 0x00168FC9
	public float WattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating * this.Efficiency;
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x0600400B RID: 16395 RVA: 0x0016ADE2 File Offset: 0x00168FE2
	public float BaseWattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating;
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x0600400C RID: 16396 RVA: 0x0016ADF4 File Offset: 0x00168FF4
	public float PercentFull
	{
		get
		{
			if (this.Capacity == 0f)
			{
				return 1f;
			}
			return this.joulesAvailable / this.Capacity;
		}
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x0600400D RID: 16397 RVA: 0x0016AE16 File Offset: 0x00169016
	// (set) Token: 0x0600400E RID: 16398 RVA: 0x0016AE1E File Offset: 0x0016901E
	public int PowerCell { get; private set; }

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x0600400F RID: 16399 RVA: 0x0016AE27 File Offset: 0x00169027
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06004010 RID: 16400 RVA: 0x0016AE39 File Offset: 0x00169039
	private float Efficiency
	{
		get
		{
			return Mathf.Max(1f + this.generatorOutputAttribute.GetTotalValue() / 100f, 0f);
		}
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06004011 RID: 16401 RVA: 0x0016AE5C File Offset: 0x0016905C
	// (set) Token: 0x06004012 RID: 16402 RVA: 0x0016AE64 File Offset: 0x00169064
	public bool IsVirtual { get; protected set; }

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06004013 RID: 16403 RVA: 0x0016AE6D File Offset: 0x0016906D
	// (set) Token: 0x06004014 RID: 16404 RVA: 0x0016AE75 File Offset: 0x00169075
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x06004015 RID: 16405 RVA: 0x0016AE80 File Offset: 0x00169080
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.generatorOutputAttribute = attributes.Add(Db.Get().Attributes.GeneratorOutput);
	}

	// Token: 0x06004016 RID: 16406 RVA: 0x0016AEBC File Offset: 0x001690BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Generators.Add(this);
		base.Subscribe<Generator>(-1582839653, Generator.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.capacity = Generator.CalculateCapacity(this.building.Def, null);
		this.PowerCell = this.building.GetPowerOutputCell();
		this.CheckConnectionStatus();
		Game.Instance.energySim.AddGenerator(this);
	}

	// Token: 0x06004017 RID: 16407 RVA: 0x0016AF30 File Offset: 0x00169130
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x06004018 RID: 16408 RVA: 0x0016AF61 File Offset: 0x00169161
	public virtual bool IsProducingPower()
	{
		return this.operational.IsActive;
	}

	// Token: 0x06004019 RID: 16409 RVA: 0x0016AF6E File Offset: 0x0016916E
	public virtual void EnergySim200ms(float dt)
	{
		this.CheckConnectionStatus();
	}

	// Token: 0x0600401A RID: 16410 RVA: 0x0016AF78 File Offset: 0x00169178
	private void SetStatusItem(StatusItem status_item)
	{
		if (status_item != this.currentStatusItem && this.currentStatusItem != null)
		{
			this.statusItemID = this.selectable.RemoveStatusItem(this.statusItemID, false);
		}
		if (status_item != null && this.statusItemID == Guid.Empty)
		{
			this.statusItemID = this.selectable.AddStatusItem(status_item, this);
		}
		this.currentStatusItem = status_item;
	}

	// Token: 0x0600401B RID: 16411 RVA: 0x0016AFE0 File Offset: 0x001691E0
	private void CheckConnectionStatus()
	{
		if (this.CircuitID == 65535)
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, false);
			return;
		}
		if (!Game.Instance.circuitManager.HasConsumers(this.CircuitID) && !Game.Instance.circuitManager.HasBatteries(this.CircuitID))
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoPowerConsumers);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, true);
			return;
		}
		this.SetStatusItem(null);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
	}

	// Token: 0x0600401C RID: 16412 RVA: 0x0016B09E File Offset: 0x0016929E
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveGenerator(this);
		Game.Instance.circuitManager.Disconnect(this);
		Components.Generators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600401D RID: 16413 RVA: 0x0016B0D1 File Offset: 0x001692D1
	public static float CalculateCapacity(BuildingDef def, Element element)
	{
		if (element == null)
		{
			return def.GeneratorBaseCapacity;
		}
		return def.GeneratorBaseCapacity * (1f + (element.HasTag(GameTags.RefinedMetal) ? 1f : 0f));
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x0016B103 File Offset: 0x00169303
	public void ResetJoules()
	{
		this.joulesAvailable = 0f;
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x0016B110 File Offset: 0x00169310
	public virtual void ApplyDeltaJoules(float joulesDelta, bool canOverPower = false)
	{
		this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesDelta, 0f, canOverPower ? float.MaxValue : this.Capacity);
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x0016B13C File Offset: 0x0016933C
	public void GenerateJoules(float joulesAvailable, bool canOverPower = false)
	{
		global::Debug.Assert(base.GetComponent<Battery>() == null);
		this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesAvailable, 0f, canOverPower ? float.MaxValue : this.Capacity);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, this.joulesAvailable, this.GetProperName(), null);
		if (!Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(this.PrefabID()))
		{
			Game.Instance.savedInfo.powerCreatedbyGeneratorType.Add(this.PrefabID(), 0f);
		}
		Dictionary<Tag, float> powerCreatedbyGeneratorType = Game.Instance.savedInfo.powerCreatedbyGeneratorType;
		Tag key = this.PrefabID();
		powerCreatedbyGeneratorType[key] += this.joulesAvailable;
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x0016B202 File Offset: 0x00169402
	public void AssignJoulesAvailable(float joulesAvailable)
	{
		global::Debug.Assert(base.GetComponent<PowerTransformer>() != null);
		this.joulesAvailable = joulesAvailable;
	}

	// Token: 0x06004022 RID: 16418 RVA: 0x0016B21C File Offset: 0x0016941C
	public virtual void ConsumeEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x04002A4D RID: 10829
	protected const int SimUpdateSortKey = 1001;

	// Token: 0x04002A4E RID: 10830
	[MyCmpReq]
	protected Building building;

	// Token: 0x04002A4F RID: 10831
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04002A50 RID: 10832
	[MyCmpReq]
	protected KSelectable selectable;

	// Token: 0x04002A51 RID: 10833
	[Serialize]
	private float joulesAvailable;

	// Token: 0x04002A52 RID: 10834
	[SerializeField]
	public int powerDistributionOrder;

	// Token: 0x04002A53 RID: 10835
	public static readonly Operational.Flag generatorConnectedFlag = new Operational.Flag("GeneratorConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04002A54 RID: 10836
	protected static readonly Operational.Flag wireConnectedFlag = new Operational.Flag("generatorWireConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04002A55 RID: 10837
	private float capacity;

	// Token: 0x04002A59 RID: 10841
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x04002A5A RID: 10842
	[SerializeField]
	public Tag[] connectedTags = Generator.DEFAULT_CONNECTED_TAGS;

	// Token: 0x04002A5B RID: 10843
	public bool showConnectedConsumerStatusItems = true;

	// Token: 0x04002A5C RID: 10844
	private StatusItem currentStatusItem;

	// Token: 0x04002A5D RID: 10845
	private Guid statusItemID;

	// Token: 0x04002A5E RID: 10846
	private AttributeInstance generatorOutputAttribute;

	// Token: 0x04002A5F RID: 10847
	private static readonly EventSystem.IntraObjectHandler<Generator> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Generator>(delegate(Generator component, object data)
	{
		component.OnTagsChanged(data);
	});
}

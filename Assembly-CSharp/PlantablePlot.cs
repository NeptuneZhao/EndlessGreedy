using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200074C RID: 1868
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantablePlot : SingleEntityReceptacle, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x1700032C RID: 812
	// (get) Token: 0x060031B7 RID: 12727 RVA: 0x00111985 File Offset: 0x0010FB85
	// (set) Token: 0x060031B8 RID: 12728 RVA: 0x00111992 File Offset: 0x0010FB92
	public KPrefabID plant
	{
		get
		{
			return this.plantRef.Get();
		}
		set
		{
			this.plantRef.Set(value);
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x060031B9 RID: 12729 RVA: 0x001119A0 File Offset: 0x0010FBA0
	public bool ValidPlant
	{
		get
		{
			return this.plantPreview == null || this.plantPreview.Valid;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x060031BA RID: 12730 RVA: 0x001119BD File Offset: 0x0010FBBD
	public bool AcceptsFertilizer
	{
		get
		{
			return this.accepts_fertilizer;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x060031BB RID: 12731 RVA: 0x001119C5 File Offset: 0x0010FBC5
	public bool AcceptsIrrigation
	{
		get
		{
			return this.accepts_irrigation;
		}
	}

	// Token: 0x060031BC RID: 12732 RVA: 0x001119D0 File Offset: 0x0010FBD0
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!DlcManager.FeaturePlantMutationsEnabled())
		{
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
			return;
		}
		if (this.requestedEntityTag.IsValid && this.requestedEntityAdditionalFilterTag.IsValid && !PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag))
		{
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		}
	}

	// Token: 0x060031BD RID: 12733 RVA: 0x00111A30 File Offset: 0x0010FC30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.FarmFetch;
		this.statusItemNeed = Db.Get().BuildingStatusItems.NeedSeed;
		this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableSeed;
		this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingSeedDelivery;
		this.plantRef = new Ref<KPrefabID>();
		base.Subscribe<PlantablePlot>(-905833192, PlantablePlot.OnCopySettingsDelegate);
		base.Subscribe<PlantablePlot>(144050788, PlantablePlot.OnUpdateRoomDelegate);
		if (this.HasTag(GameTags.FarmTiles))
		{
			this.storage.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			DropAllWorkable component = base.GetComponent<DropAllWorkable>();
			if (component != null)
			{
				component.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			}
			Toggleable component2 = base.GetComponent<Toggleable>();
			if (component2 != null)
			{
				component2.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			}
		}
	}

	// Token: 0x060031BE RID: 12734 RVA: 0x00111B18 File Offset: 0x0010FD18
	private void OnCopySettings(object data)
	{
		PlantablePlot component = ((GameObject)data).GetComponent<PlantablePlot>();
		if (component != null)
		{
			if (base.occupyingObject == null && (this.requestedEntityTag != component.requestedEntityTag || this.requestedEntityAdditionalFilterTag != component.requestedEntityAdditionalFilterTag || component.occupyingObject != null))
			{
				Tag entityTag = component.requestedEntityTag;
				Tag additionalFilterTag = component.requestedEntityAdditionalFilterTag;
				if (component.occupyingObject != null)
				{
					SeedProducer component2 = component.occupyingObject.GetComponent<SeedProducer>();
					if (component2 != null)
					{
						entityTag = TagManager.Create(component2.seedInfo.seedId);
						MutantPlant component3 = component.occupyingObject.GetComponent<MutantPlant>();
						additionalFilterTag = (component3 ? component3.SubSpeciesID : Tag.Invalid);
					}
				}
				base.CancelActiveRequest();
				this.CreateOrder(entityTag, additionalFilterTag);
			}
			if (base.occupyingObject != null)
			{
				Prioritizable component4 = base.GetComponent<Prioritizable>();
				if (component4 != null)
				{
					Prioritizable component5 = base.occupyingObject.GetComponent<Prioritizable>();
					if (component5 != null)
					{
						component5.SetMasterPriority(component4.GetMasterPriority());
					}
				}
			}
		}
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x00111C3C File Offset: 0x0010FE3C
	public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		this.SetPreview(entityTag, false);
		if (this.ValidPlant)
		{
			base.CreateOrder(entityTag, additionalFilterTag);
			return;
		}
		this.SetPreview(Tag.Invalid, false);
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x00111C64 File Offset: 0x0010FE64
	private void SyncPriority(PrioritySetting priority)
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (!object.Equals(component.GetMasterPriority(), priority))
		{
			component.SetMasterPriority(priority);
		}
		if (base.occupyingObject != null)
		{
			Prioritizable component2 = base.occupyingObject.GetComponent<Prioritizable>();
			if (component2 != null && !object.Equals(component2.GetMasterPriority(), priority))
			{
				component2.SetMasterPriority(component.GetMasterPriority());
			}
		}
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x00111CE0 File Offset: 0x0010FEE0
	protected override void OnSpawn()
	{
		if (this.plant != null)
		{
			this.RegisterWithPlant(this.plant.gameObject);
		}
		base.OnSpawn();
		this.autoReplaceEntity = false;
		Components.PlantablePlots.Add(base.gameObject.GetMyWorldId(), this);
		Prioritizable component = base.GetComponent<Prioritizable>();
		component.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(component.onPriorityChanged, new Action<PrioritySetting>(this.SyncPriority));
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x00111D56 File Offset: 0x0010FF56
	public void SetFertilizationFlags(bool fertilizer, bool liquid_piping)
	{
		this.accepts_fertilizer = fertilizer;
		this.has_liquid_pipe_input = liquid_piping;
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x00111D68 File Offset: 0x0010FF68
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.plantPreview != null)
		{
			Util.KDestroyGameObject(this.plantPreview.gameObject);
		}
		if (base.occupyingObject)
		{
			base.occupyingObject.Trigger(-216549700, null);
		}
		Components.PlantablePlots.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x00111DD0 File Offset: 0x0010FFD0
	protected override GameObject SpawnOccupyingObject(GameObject depositedEntity)
	{
		PlantableSeed component = depositedEntity.GetComponent<PlantableSeed>();
		if (component != null)
		{
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(this), this.plantLayer);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(component.PlantID), position, this.plantLayer, null, 0);
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component2 != null)
			{
				component.GetComponent<MutantPlant>().CopyMutationsTo(component2);
			}
			gameObject.SetActive(true);
			this.destroyEntityOnDeposit = true;
			return gameObject;
		}
		this.destroyEntityOnDeposit = false;
		return depositedEntity;
	}

	// Token: 0x060031C5 RID: 12741 RVA: 0x00111E4C File Offset: 0x0011004C
	protected override void ConfigureOccupyingObject(GameObject newPlant)
	{
		KPrefabID component = newPlant.GetComponent<KPrefabID>();
		this.plantRef.Set(component);
		this.RegisterWithPlant(newPlant);
		UprootedMonitor component2 = newPlant.GetComponent<UprootedMonitor>();
		if (component2)
		{
			component2.canBeUprooted = false;
		}
		this.autoReplaceEntity = false;
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		if (component3 != null)
		{
			Prioritizable component4 = newPlant.GetComponent<Prioritizable>();
			if (component4 != null)
			{
				component4.SetMasterPriority(component3.GetMasterPriority());
				Prioritizable prioritizable = component4;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.SyncPriority));
			}
		}
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x00111EDE File Offset: 0x001100DE
	public void ReplacePlant(GameObject plant, bool keepStorage)
	{
		if (keepStorage)
		{
			this.UnsubscribeFromOccupant();
			base.occupyingObject = null;
		}
		base.ForceDeposit(plant);
	}

	// Token: 0x060031C7 RID: 12743 RVA: 0x00111EF8 File Offset: 0x001100F8
	protected override void PositionOccupyingObject()
	{
		base.PositionOccupyingObject();
		KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
		component.SetSceneLayer(this.plantLayer);
		this.OffsetAnim(component, this.occupyingObjectVisualOffset);
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x00111F30 File Offset: 0x00110130
	private void RegisterWithPlant(GameObject plant)
	{
		base.occupyingObject = plant;
		ReceptacleMonitor component = plant.GetComponent<ReceptacleMonitor>();
		if (component)
		{
			if (this.tagOnPlanted != Tag.Invalid)
			{
				component.AddTag(this.tagOnPlanted);
			}
			component.SetReceptacle(this);
		}
		plant.Trigger(1309017699, this.storage);
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x00111F89 File Offset: 0x00110189
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		if (base.occupyingObject != null)
		{
			base.Subscribe(base.occupyingObject, -216549700, new Action<object>(this.OnOccupantUprooted));
		}
	}

	// Token: 0x060031CA RID: 12746 RVA: 0x00111FBD File Offset: 0x001101BD
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		if (base.occupyingObject != null)
		{
			base.Unsubscribe(base.occupyingObject, -216549700, new Action<object>(this.OnOccupantUprooted));
		}
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x00111FF0 File Offset: 0x001101F0
	private void OnOccupantUprooted(object data)
	{
		this.autoReplaceEntity = false;
		this.requestedEntityTag = Tag.Invalid;
		this.requestedEntityAdditionalFilterTag = Tag.Invalid;
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x00112010 File Offset: 0x00110210
	public override void OrderRemoveOccupant()
	{
		if (base.Occupant == null)
		{
			return;
		}
		Uprootable component = base.Occupant.GetComponent<Uprootable>();
		if (component == null)
		{
			return;
		}
		component.MarkForUproot(true);
	}

	// Token: 0x060031CD RID: 12749 RVA: 0x0011204C File Offset: 0x0011024C
	public override void SetPreview(Tag entityTag, bool solid = false)
	{
		PlantableSeed plantableSeed = null;
		if (entityTag.IsValid)
		{
			GameObject prefab = Assets.GetPrefab(entityTag);
			if (prefab == null)
			{
				DebugUtil.LogWarningArgs(base.gameObject, new object[]
				{
					"Planter tried previewing a tag with no asset! If this was the 'Empty' tag, ignore it, that will go away in new save games. Otherwise... Eh? Tag was: ",
					entityTag
				});
				return;
			}
			plantableSeed = prefab.GetComponent<PlantableSeed>();
		}
		if (this.plantPreview != null)
		{
			KPrefabID component = this.plantPreview.GetComponent<KPrefabID>();
			if (plantableSeed != null && component != null && component.PrefabTag == plantableSeed.PreviewID)
			{
				return;
			}
			this.plantPreview.gameObject.Unsubscribe(-1820564715, new Action<object>(this.OnValidChanged));
			Util.KDestroyGameObject(this.plantPreview.gameObject);
		}
		if (plantableSeed != null)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(plantableSeed.PreviewID), Grid.SceneLayer.Front, null, 0);
			this.plantPreview = gameObject.GetComponent<EntityPreview>();
			gameObject.transform.SetPosition(Vector3.zero);
			gameObject.transform.SetParent(base.gameObject.transform, false);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			if (this.rotatable != null)
			{
				if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
				{
					gameObject.transform.SetLocalPosition(this.occupyingObjectRelativePosition);
				}
				else if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
				{
					gameObject.transform.SetLocalPosition(Rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition, Orientation.R90));
				}
				else
				{
					gameObject.transform.SetLocalPosition(Rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition, Orientation.R180));
				}
			}
			else
			{
				gameObject.transform.SetLocalPosition(this.occupyingObjectRelativePosition);
			}
			KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
			this.OffsetAnim(component2, this.occupyingObjectVisualOffset);
			gameObject.SetActive(true);
			gameObject.Subscribe(-1820564715, new Action<object>(this.OnValidChanged));
			if (solid)
			{
				this.plantPreview.SetSolid();
			}
			this.plantPreview.UpdateValidity();
		}
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x0011223C File Offset: 0x0011043C
	private void OffsetAnim(KBatchedAnimController kanim, Vector3 offset)
	{
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedOffset(offset);
		}
		kanim.Offset = offset;
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x00112261 File Offset: 0x00110461
	private void OnValidChanged(object obj)
	{
		base.Trigger(-1820564715, obj);
		if (!this.plantPreview.Valid && base.GetActiveRequest != null)
		{
			base.CancelActiveRequest();
		}
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x0011228C File Offset: 0x0011048C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ENABLESDOMESTICGROWTH, UI.BUILDINGEFFECTS.TOOLTIPS.ENABLESDOMESTICGROWTH, Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04001D48 RID: 7496
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001D49 RID: 7497
	public Tag tagOnPlanted = Tag.Invalid;

	// Token: 0x04001D4A RID: 7498
	[Serialize]
	private Ref<KPrefabID> plantRef;

	// Token: 0x04001D4B RID: 7499
	public Vector3 occupyingObjectVisualOffset = Vector3.zero;

	// Token: 0x04001D4C RID: 7500
	public Grid.SceneLayer plantLayer = Grid.SceneLayer.BuildingBack;

	// Token: 0x04001D4D RID: 7501
	private EntityPreview plantPreview;

	// Token: 0x04001D4E RID: 7502
	[SerializeField]
	private bool accepts_fertilizer;

	// Token: 0x04001D4F RID: 7503
	[SerializeField]
	private bool accepts_irrigation = true;

	// Token: 0x04001D50 RID: 7504
	[SerializeField]
	public bool has_liquid_pipe_input;

	// Token: 0x04001D51 RID: 7505
	private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>(delegate(PlantablePlot component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001D52 RID: 7506
	private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>(delegate(PlantablePlot component, object data)
	{
		if (component.plantRef.Get() != null)
		{
			component.plantRef.Get().Trigger(144050788, data);
		}
	});
}

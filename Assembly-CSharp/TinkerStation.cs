using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B35 RID: 2869
[AddComponentMenu("KMonoBehaviour/Workable/TinkerStation")]
public class TinkerStation : Workable, IGameObjectEffectDescriptor, ISim1000ms
{
	// Token: 0x17000663 RID: 1635
	// (set) Token: 0x06005586 RID: 21894 RVA: 0x001E8E13 File Offset: 0x001E7013
	public AttributeConverter AttributeConverter
	{
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x17000664 RID: 1636
	// (set) Token: 0x06005587 RID: 21895 RVA: 0x001E8E1C File Offset: 0x001E701C
	public float AttributeExperienceMultiplier
	{
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x17000665 RID: 1637
	// (set) Token: 0x06005588 RID: 21896 RVA: 0x001E8E25 File Offset: 0x001E7025
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x17000666 RID: 1638
	// (set) Token: 0x06005589 RID: 21897 RVA: 0x001E8E2E File Offset: 0x001E702E
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x001E8E38 File Offset: 0x001E7038
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		if (this.useFilteredStorage)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreType);
			this.filteredStorage = new FilteredStorage(this, null, null, false, byHash);
		}
		base.Subscribe<TinkerStation>(-592767678, TinkerStation.OnOperationalChangedDelegate);
	}

	// Token: 0x0600558B RID: 21899 RVA: 0x001E8ECF File Offset: 0x001E70CF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.useFilteredStorage && this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x001E8EF2 File Offset: 0x001E70F2
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x0600558D RID: 21901 RVA: 0x001E8F10 File Offset: 0x001E7110
	private bool CorrectRolePrecondition(MinionIdentity worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		return component != null && component.HasPerk(this.requiredSkillPerk);
	}

	// Token: 0x0600558E RID: 21902 RVA: 0x001E8F40 File Offset: 0x001E7140
	private void OnOperationalChanged(object data)
	{
		RoomTracker component = base.GetComponent<RoomTracker>();
		if (component != null && component.room != null)
		{
			component.room.RetriggerBuildings();
		}
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x001E8F70 File Offset: 0x001E7170
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x001E8FB0 File Offset: 0x001E71B0
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.ShowProgressBar(false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x001E8FF0 File Offset: 0x001E71F0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(this.inputMaterial, this.massPerTinker);
		if (primaryElement != null)
		{
			SimHashes elementID = primaryElement.ElementID;
			this.storage.ConsumeIgnoringDisease(elementID.CreateTag(), this.massPerTinker);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.outputPrefab), base.transform.GetPosition() + Vector3.up, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.SetElement(elementID, true);
			component.Temperature = this.outputTemperature;
			gameObject.SetActive(true);
		}
		this.chore = null;
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x001E9092 File Offset: 0x001E7292
	public void Sim1000ms(float dt)
	{
		this.UpdateChore();
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x001E909C File Offset: 0x001E729C
	private void UpdateChore()
	{
		if (this.operational.IsOperational && (this.ToolsRequested() || this.alwaysTinker) && this.HasMaterial())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<TinkerStation>(Db.Get().ChoreTypes.GetByHash(this.choreType), this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
				base.SetWorkTime(this.workTime);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Can't tinker");
			this.chore = null;
		}
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x001E914F File Offset: 0x001E734F
	private bool HasMaterial()
	{
		return this.storage.MassStored() > 0f;
	}

	// Token: 0x06005595 RID: 21909 RVA: 0x001E9164 File Offset: 0x001E7364
	private bool ToolsRequested()
	{
		return MaterialNeeds.GetAmount(this.outputPrefab, base.gameObject.GetMyWorldId(), false) > 0f && this.GetMyWorld().worldInventory.GetAmount(this.outputPrefab, true) <= 0f;
	}

	// Token: 0x06005596 RID: 21910 RVA: 0x001E91B4 File Offset: 0x001E73B4
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		string arg = this.inputMaterial.ProperName();
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		descriptors.AddRange(GameUtil.GetAllDescriptors(Assets.GetPrefab(this.outputPrefab), false));
		List<Tinkerable> list = new List<Tinkerable>();
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<Tinkerable>())
		{
			Tinkerable component = gameObject.GetComponent<Tinkerable>();
			if (component.tinkerMaterialTag == this.outputPrefab)
			{
				list.Add(component);
			}
		}
		if (list.Count > 0)
		{
			Effect effect = Db.Get().effects.Get(list[0].addedEffect);
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ADDED_EFFECT, effect.Name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ADDED_EFFECT, effect.Name, Effect.CreateTooltip(effect, true, "\n    • ", true)), Descriptor.DescriptorType.Effect, false));
			descriptors.Add(new Descriptor(this.EffectTitle, this.EffectTooltip, Descriptor.DescriptorType.Effect, false));
			foreach (Tinkerable cmp in list)
			{
				Descriptor item = new Descriptor(string.Format(this.EffectItemString, cmp.GetProperName()), string.Format(this.EffectItemTooltip, cmp.GetProperName()), Descriptor.DescriptorType.Effect, false);
				item.IncreaseIndent();
				descriptors.Add(item);
			}
		}
		return descriptors;
	}

	// Token: 0x06005597 RID: 21911 RVA: 0x001E93A4 File Offset: 0x001E75A4
	public static TinkerStation AddTinkerStation(GameObject go, string required_room_type)
	{
		TinkerStation result = go.AddOrGet<TinkerStation>();
		go.AddOrGet<RoomTracker>().requiredRoomType = required_room_type;
		return result;
	}

	// Token: 0x04003805 RID: 14341
	public HashedString choreType;

	// Token: 0x04003806 RID: 14342
	public HashedString fetchChoreType;

	// Token: 0x04003807 RID: 14343
	private Chore chore;

	// Token: 0x04003808 RID: 14344
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x04003809 RID: 14345
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400380A RID: 14346
	public bool useFilteredStorage;

	// Token: 0x0400380B RID: 14347
	protected FilteredStorage filteredStorage;

	// Token: 0x0400380C RID: 14348
	public bool alwaysTinker;

	// Token: 0x0400380D RID: 14349
	public float massPerTinker;

	// Token: 0x0400380E RID: 14350
	public Tag inputMaterial;

	// Token: 0x0400380F RID: 14351
	public Tag outputPrefab;

	// Token: 0x04003810 RID: 14352
	public float outputTemperature;

	// Token: 0x04003811 RID: 14353
	public string EffectTitle = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS;

	// Token: 0x04003812 RID: 14354
	public string EffectTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS;

	// Token: 0x04003813 RID: 14355
	public string EffectItemString = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS_ITEM;

	// Token: 0x04003814 RID: 14356
	public string EffectItemTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS_ITEM;

	// Token: 0x04003815 RID: 14357
	private static readonly EventSystem.IntraObjectHandler<TinkerStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TinkerStation>(delegate(TinkerStation component, object data)
	{
		component.OnOperationalChanged(data);
	});
}

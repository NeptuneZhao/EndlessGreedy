using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000783 RID: 1923
[AddComponentMenu("KMonoBehaviour/Workable/Telescope")]
public class Telescope : Workable, OxygenBreather.IGasProvider, IGameObjectEffectDescriptor, ISim200ms, BuildingStatusItems.ISkyVisInfo
{
	// Token: 0x06003445 RID: 13381 RVA: 0x0011D47D File Offset: 0x0011B67D
	float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
	{
		return this.percentClear;
	}

	// Token: 0x06003446 RID: 13382 RVA: 0x0011D488 File Offset: 0x0011B688
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x0011D4E0 File Offset: 0x0011B6E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SpacecraftManager.instance.Subscribe(532901469, new Action<object>(this.UpdateWorkingState));
		Components.Telescopes.Add(this);
		this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		this.operational = base.GetComponent<Operational>();
		this.storage = base.GetComponent<Storage>();
		this.UpdateWorkingState(null);
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x0011D55B File Offset: 0x0011B75B
	protected override void OnCleanUp()
	{
		Components.Telescopes.Remove(this);
		SpacecraftManager.instance.Unsubscribe(532901469, new Action<object>(this.UpdateWorkingState));
		base.OnCleanUp();
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x0011D58C File Offset: 0x0011B78C
	public void Sim200ms(float dt)
	{
		base.GetComponent<Building>().GetExtents();
		ValueTuple<bool, float> visibilityOf = TelescopeConfig.SKY_VISIBILITY_INFO.GetVisibilityOf(base.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		this.percentClear = item2;
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, this);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, this);
		Operational component2 = base.GetComponent<Operational>();
		component2.SetFlag(Telescope.visibleSkyFlag, item);
		if (!component2.IsActive && component2.IsOperational && this.chore == null)
		{
			this.chore = this.CreateChore();
			base.SetWorkTime(float.PositiveInfinity);
		}
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x0011D650 File Offset: 0x0011B850
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
	{
		WorkerBase worker = base.worker;
		if (worker == null)
		{
			return;
		}
		OxygenBreather component = worker.GetComponent<OxygenBreather>();
		KPrefabID component2 = worker.GetComponent<KPrefabID>();
		KSelectable component3 = base.GetComponent<KSelectable>();
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			base.ShowProgressBar(true);
			this.progressBar.SetUpdateFunc(delegate
			{
				if (SpacecraftManager.instance.HasAnalysisTarget())
				{
					return SpacecraftManager.instance.GetDestinationAnalysisScore(SpacecraftManager.instance.GetStarmapAnalysisDestinationID()) / (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE;
				}
				return 0f;
			});
			if (component != null)
			{
				this.workerGasProvider = component.GetGasProvider();
				component.SetGasProvider(this);
			}
			worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
			component2.AddTag(GameTags.Shaded, false);
			component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
			return;
		}
		if (ev != Workable.WorkableEvent.WorkStopped)
		{
			return;
		}
		if (component != null)
		{
			component.SetGasProvider(this.workerGasProvider);
		}
		worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
		base.ShowProgressBar(false);
		component2.RemoveTag(GameTags.Shaded);
		component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
	}

	// Token: 0x0600344B RID: 13387 RVA: 0x0011D752 File Offset: 0x0011B952
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.percentClear);
	}

	// Token: 0x0600344C RID: 13388 RVA: 0x0011D768 File Offset: 0x0011B968
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (SpacecraftManager.instance.HasAnalysisTarget())
		{
			int starmapAnalysisDestinationID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
			SpaceDestination destination = SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID);
			float num = 1f / (float)destination.OneBasedDistance;
			float num2 = (float)ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED;
			float default_CYCLES_PER_DISCOVERY = ROCKETRY.DESTINATION_ANALYSIS.DEFAULT_CYCLES_PER_DISCOVERY;
			float num3 = num2 / default_CYCLES_PER_DISCOVERY / 600f;
			float points = dt * num * num3;
			SpacecraftManager.instance.EarnDestinationAnalysisPoints(starmapAnalysisDestinationID, points);
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x0600344D RID: 13389 RVA: 0x0011D7DC File Offset: 0x0011B9DC
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
		descriptors.Add(item);
		return descriptors;
	}

	// Token: 0x0600344E RID: 13390 RVA: 0x0011D838 File Offset: 0x0011BA38
	protected Chore CreateChore()
	{
		WorkChore<Telescope> workChore = new WorkChore<Telescope>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(Telescope.ContainsOxygen, null);
		return workChore;
	}

	// Token: 0x0600344F RID: 13391 RVA: 0x0011D878 File Offset: 0x0011BA78
	protected void UpdateWorkingState(object data)
	{
		bool flag = false;
		if (SpacecraftManager.instance.HasAnalysisTarget() && SpacecraftManager.instance.GetDestinationAnalysisState(SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.GetStarmapAnalysisDestinationID())) != SpacecraftManager.DestinationAnalysisState.Complete)
		{
			flag = true;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		bool on = !flag && !SpacecraftManager.instance.AreAllDestinationsAnalyzed();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.NoApplicableAnalysisSelected, on, null);
		this.operational.SetFlag(Telescope.flag, flag);
		if (!flag && base.worker)
		{
			base.StopWork(base.worker, true);
		}
	}

	// Token: 0x06003450 RID: 13392 RVA: 0x0011D915 File Offset: 0x0011BB15
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06003451 RID: 13393 RVA: 0x0011D917 File Offset: 0x0011BB17
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06003452 RID: 13394 RVA: 0x0011D919 File Offset: 0x0011BB19
	public bool ShouldEmitCO2()
	{
		return false;
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x0011D91C File Offset: 0x0011BB1C
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x0011D920 File Offset: 0x0011BB20
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		if (this.storage.items.Count <= 0)
		{
			return false;
		}
		GameObject gameObject = this.storage.items[0];
		if (gameObject == null)
		{
			return false;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		bool result = component.Mass >= amount;
		component.Mass = Mathf.Max(0f, component.Mass - amount);
		return result;
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x0011D98C File Offset: 0x0011BB8C
	public bool IsLowOxygen()
	{
		if (this.storage.items.Count <= 0)
		{
			return true;
		}
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
		return primaryElement == null || primaryElement.Mass == 0f;
	}

	// Token: 0x04001EE1 RID: 7905
	private OxygenBreather.IGasProvider workerGasProvider;

	// Token: 0x04001EE2 RID: 7906
	private Operational operational;

	// Token: 0x04001EE3 RID: 7907
	private float percentClear;

	// Token: 0x04001EE4 RID: 7908
	private static readonly Operational.Flag visibleSkyFlag = new Operational.Flag("VisibleSky", Operational.Flag.Type.Requirement);

	// Token: 0x04001EE5 RID: 7909
	private Storage storage;

	// Token: 0x04001EE6 RID: 7910
	public static readonly Chore.Precondition ContainsOxygen = new Chore.Precondition
	{
		id = "ContainsOxygen",
		sortOrder = 1,
		description = DUPLICANTS.CHORES.PRECONDITIONS.CONTAINS_OXYGEN,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.chore.target.GetComponent<Storage>().FindFirstWithMass(GameTags.Oxygen, 0f) != null;
		}
	};

	// Token: 0x04001EE7 RID: 7911
	private Chore chore;

	// Token: 0x04001EE8 RID: 7912
	private static readonly Operational.Flag flag = new Operational.Flag("ValidTarget", Operational.Flag.Type.Requirement);
}

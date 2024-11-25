using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020006E6 RID: 1766
public class GourmetCookingStation : ComplexFabricator, IGameObjectEffectDescriptor
{
	// Token: 0x06002CF3 RID: 11507 RVA: 0x000FC830 File Offset: 0x000FAA30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepAdditionalTag = this.fuelTag;
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x06002CF4 RID: 11508 RVA: 0x000FC880 File Offset: 0x000FAA80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanElectricGrill.Id;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_cookstation_gourtmet_kanim")
		};
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable workable = this.workable;
		workable.OnWorkTickActions = (Action<WorkerBase, float>)Delegate.Combine(workable.OnWorkTickActions, new Action<WorkerBase, float>(delegate(WorkerBase worker, float dt)
		{
			global::Debug.Assert(worker != null, "How did we get a null worker?");
			if (this.diseaseCountKillRate > 0)
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				int num = Math.Max(1, (int)((float)this.diseaseCountKillRate * dt));
				component.ModifyDiseaseCount(-num, "GourmetCookingStation");
			}
		}));
		this.smi = new GourmetCookingStation.StatesInstance(this);
		this.smi.StartSM();
		base.GetComponent<ComplexFabricator>().workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorCooking;
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x000FC9A0 File Offset: 0x000FABA0
	public float GetAvailableFuel()
	{
		return this.inStorage.GetAmountAvailable(this.fuelTag);
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x000FC9B4 File Offset: 0x000FABB4
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ModifyDiseaseCount(-component.DiseaseCount, "GourmetCookingStation.CompleteOrder");
		}
		base.GetComponent<Operational>().SetActive(false, false);
		return list;
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x000FCA28 File Offset: 0x000FAC28
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.REMOVES_DISEASE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE, Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x040019EE RID: 6638
	private static readonly Operational.Flag gourmetCookingStationFlag = new Operational.Flag("gourmet_cooking_station", Operational.Flag.Type.Requirement);

	// Token: 0x040019EF RID: 6639
	public float GAS_CONSUMPTION_RATE;

	// Token: 0x040019F0 RID: 6640
	public float GAS_CONVERSION_RATIO = 0.1f;

	// Token: 0x040019F1 RID: 6641
	public const float START_FUEL_MASS = 5f;

	// Token: 0x040019F2 RID: 6642
	public Tag fuelTag;

	// Token: 0x040019F3 RID: 6643
	[SerializeField]
	private int diseaseCountKillRate = 150;

	// Token: 0x040019F4 RID: 6644
	private GourmetCookingStation.StatesInstance smi;

	// Token: 0x02001504 RID: 5380
	public class StatesInstance : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.GameInstance
	{
		// Token: 0x06008CE7 RID: 36071 RVA: 0x0033D15B File Offset: 0x0033B35B
		public StatesInstance(GourmetCookingStation smi) : base(smi)
		{
		}
	}

	// Token: 0x02001505 RID: 5381
	public class States : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation>
	{
		// Token: 0x06008CE8 RID: 36072 RVA: 0x0033D164 File Offset: 0x0033B364
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			if (GourmetCookingStation.States.waitingForFuelStatus == null)
			{
				GourmetCookingStation.States.waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
				GourmetCookingStation.States.waitingForFuelStatus.resolveStringCallback = delegate(string str, object obj)
				{
					GourmetCookingStation gourmetCookingStation = (GourmetCookingStation)obj;
					return string.Format(str, gourmetCookingStation.fuelTag.ProperName(), GameUtil.GetFormattedMass(5f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				};
			}
			default_state = this.waitingForFuel;
			this.waitingForFuel.Enter(delegate(GourmetCookingStation.StatesInstance smi)
			{
				smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, false);
			}).ToggleStatusItem(GourmetCookingStation.States.waitingForFuelStatus, (GourmetCookingStation.StatesInstance smi) => smi.master).EventTransition(GameHashes.OnStorageChange, this.ready, (GourmetCookingStation.StatesInstance smi) => smi.master.GetAvailableFuel() >= 5f);
			this.ready.Enter(delegate(GourmetCookingStation.StatesInstance smi)
			{
				smi.master.SetQueueDirty();
				smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, true);
			}).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (GourmetCookingStation.StatesInstance smi) => smi.master.GetAvailableFuel() <= 0f);
		}

		// Token: 0x04006BB6 RID: 27574
		public static StatusItem waitingForFuelStatus;

		// Token: 0x04006BB7 RID: 27575
		public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State waitingForFuel;

		// Token: 0x04006BB8 RID: 27576
		public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State ready;
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005AA RID: 1450
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Repairable")]
public class Repairable : Workable
{
	// Token: 0x0600227B RID: 8827 RVA: 0x000BFE4C File Offset: 0x000BE04C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		base.Subscribe<Repairable>(493375141, Repairable.OnRefreshUserMenuDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x000BFEFC File Offset: 0x000BE0FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new Repairable.SMInstance(this);
		this.smi.StartSM();
		this.workTime = float.PositiveInfinity;
		this.workTimeRemaining = float.PositiveInfinity;
	}

	// Token: 0x0600227D RID: 8829 RVA: 0x000BFF31 File Offset: 0x000BE131
	private void OnProxyStorageChanged(object data)
	{
		base.Trigger(-1697596308, data);
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x000BFF3F File Offset: 0x000BE13F
	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x000BFF4E File Offset: 0x000BE14E
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("Destroy Repairable");
		}
		base.OnCleanUp();
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000BFF70 File Offset: 0x000BE170
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject != null && this.smi != null)
		{
			if (this.smi.GetCurrentState() == this.smi.sm.forbidden)
			{
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.NAME, new System.Action(this.AllowRepair), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.TOOLTIP, true), 0.5f);
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.NAME, new System.Action(this.CancelRepair), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.TOOLTIP, true), 0.5f);
		}
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x000C0054 File Offset: 0x000BE254
	private void AllowRepair()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.hp.Repair(this.hp.MaxHitPoints);
			this.OnCompleteWork(null);
		}
		this.smi.sm.allow.Trigger(this.smi);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000C00A7 File Offset: 0x000BE2A7
	public void CancelRepair()
	{
		if (this.smi != null)
		{
			this.smi.sm.forbid.Trigger(this.smi);
		}
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000C00D4 File Offset: 0x000BE2D4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, false);
		}
		this.smi.sm.worker.Set(worker, this.smi);
		this.timeSpentRepairing = 0f;
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x000C012C File Offset: 0x000BE32C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float num = Mathf.Sqrt(base.GetComponent<PrimaryElement>().Mass);
		float num2 = ((this.expectedRepairTime < 0f) ? num : this.expectedRepairTime) * 0.1f;
		if (this.timeSpentRepairing >= num2)
		{
			this.timeSpentRepairing -= num2;
			int num3 = 0;
			if (worker != null)
			{
				num3 = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
			}
			int repair_amount = Mathf.CeilToInt((float)(10 + Math.Max(0, num3 * 10)) * 0.1f);
			this.hp.Repair(repair_amount);
			if (this.hp.HitPoints >= this.hp.MaxHitPoints)
			{
				return true;
			}
		}
		this.timeSpentRepairing += dt;
		return false;
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000C01F4 File Offset: 0x000BE3F4
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, true);
		}
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x000C0224 File Offset: 0x000BE424
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, true);
		}
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x000C0250 File Offset: 0x000BE450
	public void CreateStorageProxy()
	{
		if (this.storageProxy == null)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(RepairableStorageProxy.ID), base.transform.gameObject, null);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			this.storageProxy = gameObject.GetComponent<Storage>();
			this.storageProxy.prioritizable = base.transform.GetComponent<Prioritizable>();
			this.storageProxy.prioritizable.AddRef();
			gameObject.GetComponent<KSelectable>().entityName = base.transform.gameObject.GetProperName();
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000C02F4 File Offset: 0x000BE4F4
	[OnSerializing]
	private void OnSerializing()
	{
		this.storedData = null;
		if (this.storageProxy != null && !this.storageProxy.IsEmpty())
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					this.storageProxy.Serialize(binaryWriter);
				}
				this.storedData = memoryStream.ToArray();
			}
		}
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x000C037C File Offset: 0x000BE57C
	[OnSerialized]
	private void OnSerialized()
	{
		this.storedData = null;
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x000C0388 File Offset: 0x000BE588
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.storedData != null)
		{
			FastReader reader = new FastReader(this.storedData);
			this.CreateStorageProxy();
			this.storageProxy.Deserialize(reader);
			this.storedData = null;
		}
	}

	// Token: 0x04001375 RID: 4981
	public float expectedRepairTime = -1f;

	// Token: 0x04001376 RID: 4982
	[MyCmpGet]
	private BuildingHP hp;

	// Token: 0x04001377 RID: 4983
	private Repairable.SMInstance smi;

	// Token: 0x04001378 RID: 4984
	private Storage storageProxy;

	// Token: 0x04001379 RID: 4985
	[Serialize]
	private byte[] storedData;

	// Token: 0x0400137A RID: 4986
	private float timeSpentRepairing;

	// Token: 0x0400137B RID: 4987
	private static readonly Operational.Flag repairedFlag = new Operational.Flag("repaired", Operational.Flag.Type.Functional);

	// Token: 0x0400137C RID: 4988
	private static readonly EventSystem.IntraObjectHandler<Repairable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Repairable>(delegate(Repairable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0200139A RID: 5018
	public class SMInstance : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.GameInstance
	{
		// Token: 0x060087A7 RID: 34727 RVA: 0x0032C163 File Offset: 0x0032A363
		public SMInstance(Repairable smi) : base(smi)
		{
		}

		// Token: 0x060087A8 RID: 34728 RVA: 0x0032C16C File Offset: 0x0032A36C
		public bool HasRequiredMass()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float num = component.Mass * 0.1f;
			PrimaryElement primaryElement = base.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			return primaryElement != null && primaryElement.Mass >= num;
		}

		// Token: 0x060087A9 RID: 34729 RVA: 0x0032C1C0 File Offset: 0x0032A3C0
		public KeyValuePair<Tag, float> GetRequiredMass()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float num = component.Mass * 0.1f;
			PrimaryElement primaryElement = base.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			float value = (primaryElement != null) ? Math.Max(0f, num - primaryElement.Mass) : num;
			return new KeyValuePair<Tag, float>(component.Element.tag, value);
		}

		// Token: 0x060087AA RID: 34730 RVA: 0x0032C22D File Offset: 0x0032A42D
		public void ConsumeRepairMaterials()
		{
			base.smi.master.storageProxy.ConsumeAllIgnoringDisease();
		}

		// Token: 0x060087AB RID: 34731 RVA: 0x0032C244 File Offset: 0x0032A444
		public void DestroyStorageProxy()
		{
			if (base.smi.master.storageProxy != null)
			{
				base.smi.master.transform.GetComponent<Prioritizable>().RemoveRef();
				List<GameObject> list = new List<GameObject>();
				Storage storageProxy = base.smi.master.storageProxy;
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list;
				storageProxy.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				GameObject gameObject = base.smi.sm.worker.Get(base.smi);
				if (gameObject != null)
				{
					foreach (GameObject go in list)
					{
						go.Trigger(580035959, gameObject.GetComponent<WorkerBase>());
					}
				}
				base.smi.sm.worker.Set(null, base.smi);
				Util.KDestroyGameObject(base.smi.master.storageProxy.gameObject);
			}
		}

		// Token: 0x060087AC RID: 34732 RVA: 0x0032C358 File Offset: 0x0032A558
		public bool NeedsRepairs()
		{
			return base.smi.master.GetComponent<BuildingHP>().NeedsRepairs;
		}

		// Token: 0x04006718 RID: 26392
		private const float REQUIRED_MASS_SCALE = 0.1f;
	}

	// Token: 0x0200139B RID: 5019
	public class States : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable>
	{
		// Token: 0x060087AD RID: 34733 RVA: 0x0032C370 File Offset: 0x0032A570
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.repaired;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.forbidden.OnSignal(this.allow, this.repaired);
			this.allowed.Enter(delegate(Repairable.SMInstance smi)
			{
				smi.master.CreateStorageProxy();
			}).DefaultState(this.allowed.needMass).EventHandler(GameHashes.BuildingFullyRepaired, delegate(Repairable.SMInstance smi)
			{
				smi.ConsumeRepairMaterials();
			}).EventTransition(GameHashes.BuildingFullyRepaired, this.repaired, null).OnSignal(this.forbid, this.forbidden).Exit(delegate(Repairable.SMInstance smi)
			{
				smi.DestroyStorageProxy();
			});
			this.allowed.needMass.Enter(delegate(Repairable.SMInstance smi)
			{
				Prioritizable.AddRef(smi.master.storageProxy.transform.parent.gameObject);
			}).Exit(delegate(Repairable.SMInstance smi)
			{
				if (!smi.isMasterNull && smi.master.storageProxy != null)
				{
					Prioritizable.RemoveRef(smi.master.storageProxy.transform.parent.gameObject);
				}
			}).EventTransition(GameHashes.OnStorageChange, this.allowed.repairable, (Repairable.SMInstance smi) => smi.HasRequiredMass()).ToggleChore(new Func<Repairable.SMInstance, Chore>(this.CreateFetchChore), this.allowed.repairable, this.allowed.needMass).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForRepairMaterials, (Repairable.SMInstance smi) => smi.GetRequiredMass());
			this.allowed.repairable.ToggleRecurringChore(new Func<Repairable.SMInstance, Chore>(this.CreateRepairChore), null).ToggleStatusItem(Db.Get().BuildingStatusItems.PendingRepair, null);
			this.repaired.EventTransition(GameHashes.BuildingReceivedDamage, this.allowed, (Repairable.SMInstance smi) => smi.NeedsRepairs()).OnSignal(this.allow, this.allowed).OnSignal(this.forbid, this.forbidden);
		}

		// Token: 0x060087AE RID: 34734 RVA: 0x0032C5BC File Offset: 0x0032A7BC
		private Chore CreateFetchChore(Repairable.SMInstance smi)
		{
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			PrimaryElement primaryElement = smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			float amount = component.Mass * 0.1f - ((primaryElement != null) ? primaryElement.Mass : 0f);
			HashSet<Tag> tags = new HashSet<Tag>
			{
				GameTagExtensions.Create(component.ElementID)
			};
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storageProxy, amount, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		}

		// Token: 0x060087AF RID: 34735 RVA: 0x0032C658 File Offset: 0x0032A858
		private Chore CreateRepairChore(Repairable.SMInstance smi)
		{
			WorkChore<Repairable> workChore = new WorkChore<Repairable>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			Deconstructable component = smi.master.GetComponent<Deconstructable>();
			if (component != null)
			{
				workChore.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
			}
			Breakable component2 = smi.master.GetComponent<Breakable>();
			if (component2 != null)
			{
				workChore.AddPrecondition(Repairable.States.IsNotBeingAttacked, component2);
			}
			workChore.AddPrecondition(Repairable.States.IsNotAngry, null);
			return workChore;
		}

		// Token: 0x04006719 RID: 26393
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal allow;

		// Token: 0x0400671A RID: 26394
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal forbid;

		// Token: 0x0400671B RID: 26395
		public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State forbidden;

		// Token: 0x0400671C RID: 26396
		public Repairable.States.AllowedState allowed;

		// Token: 0x0400671D RID: 26397
		public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repaired;

		// Token: 0x0400671E RID: 26398
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.TargetParameter worker;

		// Token: 0x0400671F RID: 26399
		public static readonly Chore.Precondition IsNotBeingAttacked = new Chore.Precondition
		{
			id = "IsNotBeingAttacked",
			description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				bool result = true;
				if (data != null)
				{
					result = (((Breakable)data).worker == null);
				}
				return result;
			}
		};

		// Token: 0x04006720 RID: 26400
		public static readonly Chore.Precondition IsNotAngry = new Chore.Precondition
		{
			id = "IsNotAngry",
			description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_ANGRY,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Traits traits = context.consumerState.traits;
				AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject);
				return !(traits != null) || amountInstance == null || amountInstance.value < STRESS.ACTING_OUT_RESET || !traits.HasTrait("Aggressive");
			}
		};

		// Token: 0x0200249E RID: 9374
		public class AllowedState : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State
		{
			// Token: 0x0400A25B RID: 41563
			public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State needMass;

			// Token: 0x0400A25C RID: 41564
			public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repairable;
		}
	}
}

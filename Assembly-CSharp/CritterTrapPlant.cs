using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009EF RID: 2543
public class CritterTrapPlant : StateMachineComponent<CritterTrapPlant.StatesInstance>
{
	// Token: 0x060049AF RID: 18863 RVA: 0x001A5C00 File Offset: 0x001A3E00
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.master.growing.enabled = false;
		base.Subscribe<CritterTrapPlant>(-216549700, CritterTrapPlant.OnUprootedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x060049B0 RID: 18864 RVA: 0x001A5C3A File Offset: 0x001A3E3A
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x060049B1 RID: 18865 RVA: 0x001A5C54 File Offset: 0x001A3E54
	private void OnUprooted(object data = null)
	{
		GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), base.gameObject.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
		base.gameObject.Trigger(1623392196, null);
		base.gameObject.GetComponent<KBatchedAnimController>().StopAndClear();
		UnityEngine.Object.Destroy(base.gameObject.GetComponent<KBatchedAnimController>());
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060049B2 RID: 18866 RVA: 0x001A5CCB File Offset: 0x001A3ECB
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x001A5CE4 File Offset: 0x001A3EE4
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x0400303F RID: 12351
	[MyCmpReq]
	private Crop crop;

	// Token: 0x04003040 RID: 12352
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04003041 RID: 12353
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04003042 RID: 12354
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04003043 RID: 12355
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04003044 RID: 12356
	[MyCmpReq]
	private Harvestable harvestable;

	// Token: 0x04003045 RID: 12357
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003046 RID: 12358
	public float gasOutputRate;

	// Token: 0x04003047 RID: 12359
	public float gasVentThreshold;

	// Token: 0x04003048 RID: 12360
	public SimHashes outputElement;

	// Token: 0x04003049 RID: 12361
	private float GAS_TEMPERATURE_DELTA = 10f;

	// Token: 0x0400304A RID: 12362
	private static readonly EventSystem.IntraObjectHandler<CritterTrapPlant> OnUprootedDelegate = new EventSystem.IntraObjectHandler<CritterTrapPlant>(delegate(CritterTrapPlant component, object data)
	{
		component.OnUprooted(data);
	});

	// Token: 0x020019F7 RID: 6647
	public class StatesInstance : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.GameInstance
	{
		// Token: 0x06009E8D RID: 40589 RVA: 0x00378846 File Offset: 0x00376A46
		public StatesInstance(CritterTrapPlant master) : base(master)
		{
		}

		// Token: 0x06009E8E RID: 40590 RVA: 0x0037884F File Offset: 0x00376A4F
		public void OnTrapTriggered(object data)
		{
			base.smi.sm.trapTriggered.Trigger(base.smi);
		}

		// Token: 0x06009E8F RID: 40591 RVA: 0x0037886C File Offset: 0x00376A6C
		public void AddGas(float dt)
		{
			float temperature = base.smi.GetComponent<PrimaryElement>().Temperature + base.smi.master.GAS_TEMPERATURE_DELTA;
			base.smi.master.storage.AddGasChunk(base.smi.master.outputElement, base.smi.master.gasOutputRate * dt, temperature, byte.MaxValue, 0, false, true);
			if (this.ShouldVentGas())
			{
				base.smi.sm.ventGas.Trigger(base.smi);
			}
		}

		// Token: 0x06009E90 RID: 40592 RVA: 0x00378900 File Offset: 0x00376B00
		public void VentGas()
		{
			PrimaryElement primaryElement = base.smi.master.storage.FindPrimaryElement(base.smi.master.outputElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.smi.transform.GetPosition()), primaryElement.ElementID, CellEventLogger.Instance.Dumpable, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				base.smi.master.storage.ConsumeIgnoringDisease(primaryElement.gameObject);
			}
		}

		// Token: 0x06009E91 RID: 40593 RVA: 0x0037899C File Offset: 0x00376B9C
		public bool ShouldVentGas()
		{
			PrimaryElement primaryElement = base.smi.master.storage.FindPrimaryElement(base.smi.master.outputElement);
			return !(primaryElement == null) && primaryElement.Mass >= base.smi.master.gasVentThreshold;
		}
	}

	// Token: 0x020019F8 RID: 6648
	public class States : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant>
	{
		// Token: 0x06009E92 RID: 40594 RVA: 0x003789F8 File Offset: 0x00376BF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.trap;
			this.trap.DefaultState(this.trap.open);
			this.trap.open.ToggleComponent<TrapTrigger>(false).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
				smi.master.storage.ConsumeAllIgnoringDisease();
			}).EventHandler(GameHashes.TrapTriggered, delegate(CritterTrapPlant.StatesInstance smi, object data)
			{
				smi.OnTrapTriggered(data);
			}).EventTransition(GameHashes.Wilt, this.trap.wilting, null).OnSignal(this.trapTriggered, this.trap.trigger).ParamTransition<bool>(this.hasEatenCreature, this.trap.digesting, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.IsTrue).PlayAnim("idle_open", KAnim.PlayMode.Loop);
			this.trap.trigger.PlayAnim("trap", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.master.storage.ConsumeAllIgnoringDisease();
				smi.sm.hasEatenCreature.Set(true, smi, false);
			}).OnAnimQueueComplete(this.trap.digesting);
			this.trap.digesting.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).ToggleComponent<Growing>(false).EventTransition(GameHashes.Grow, this.fruiting.enter, (CritterTrapPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.Wilt, this.trap.wilting, null).DefaultState(this.trap.digesting.idle);
			this.trap.digesting.idle.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).Update(delegate(CritterTrapPlant.StatesInstance smi, float dt)
			{
				smi.AddGas(dt);
			}, UpdateRate.SIM_4000ms, false).OnSignal(this.ventGas, this.trap.digesting.vent_pre);
			this.trap.digesting.vent_pre.PlayAnim("vent_pre").Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
			}).OnAnimQueueComplete(this.trap.digesting.vent);
			this.trap.digesting.vent.PlayAnim("vent_loop", KAnim.PlayMode.Once).QueueAnim("vent_pst", false, null).OnAnimQueueComplete(this.trap.digesting.idle);
			this.trap.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.trap, (CritterTrapPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.fruiting.EventTransition(GameHashes.Wilt, this.fruiting.wilting, null).EventTransition(GameHashes.Harvest, this.harvest, null).DefaultState(this.fruiting.idle);
			this.fruiting.enter.PlayAnim("open_harvest", KAnim.PlayMode.Once).Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
				smi.master.storage.ConsumeAllIgnoringDisease();
			}).OnAnimQueueComplete(this.fruiting.idle);
			this.fruiting.idle.PlayAnim("harvestable_loop", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.fruiting.old, new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld), UpdateRate.SIM_4000ms);
			this.fruiting.old.PlayAnim("wilt1", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.fruiting.idle, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Not(new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld)), UpdateRate.SIM_4000ms);
			this.fruiting.wilting.PlayAnim("wilt1", KAnim.PlayMode.Once).EventTransition(GameHashes.WiltRecover, this.fruiting, (CritterTrapPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.harvest.PlayAnim("harvest", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (GameScheduler.Instance != null && smi.master != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.harvestable.SetCanBeHarvested(false);
			}).Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.sm.hasEatenCreature.Set(false, smi, false);
			}).OnAnimQueueComplete(this.trap.open);
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable harvestable = smi.master.harvestable;
				if (harvestable != null && harvestable.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
		}

		// Token: 0x06009E93 RID: 40595 RVA: 0x00378F05 File Offset: 0x00377105
		public bool IsOld(CritterTrapPlant.StatesInstance smi)
		{
			return smi.master.growing.PercentOldAge() > 0.5f;
		}

		// Token: 0x04007AEF RID: 31471
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal trapTriggered;

		// Token: 0x04007AF0 RID: 31472
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal ventGas;

		// Token: 0x04007AF1 RID: 31473
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.BoolParameter hasEatenCreature;

		// Token: 0x04007AF2 RID: 31474
		public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State dead;

		// Token: 0x04007AF3 RID: 31475
		public CritterTrapPlant.States.FruitingStates fruiting;

		// Token: 0x04007AF4 RID: 31476
		public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State harvest;

		// Token: 0x04007AF5 RID: 31477
		public CritterTrapPlant.States.TrapStates trap;

		// Token: 0x020025D6 RID: 9686
		public class DigestingStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x0400A871 RID: 43121
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;

			// Token: 0x0400A872 RID: 43122
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent_pre;

			// Token: 0x0400A873 RID: 43123
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent;
		}

		// Token: 0x020025D7 RID: 9687
		public class TrapStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x0400A874 RID: 43124
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State open;

			// Token: 0x0400A875 RID: 43125
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State trigger;

			// Token: 0x0400A876 RID: 43126
			public CritterTrapPlant.States.DigestingStates digesting;

			// Token: 0x0400A877 RID: 43127
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
		}

		// Token: 0x020025D8 RID: 9688
		public class FruitingStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x0400A878 RID: 43128
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State enter;

			// Token: 0x0400A879 RID: 43129
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;

			// Token: 0x0400A87A RID: 43130
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State old;

			// Token: 0x0400A87B RID: 43131
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
		}
	}
}

using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009FB RID: 2555
public class StandardCropPlant : StateMachineComponent<StandardCropPlant.StatesInstance>
{
	// Token: 0x060049F3 RID: 18931 RVA: 0x001A6F66 File Offset: 0x001A5166
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060049F4 RID: 18932 RVA: 0x001A6F79 File Offset: 0x001A5179
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060049F5 RID: 18933 RVA: 0x001A6F94 File Offset: 0x001A5194
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x060049F6 RID: 18934 RVA: 0x001A6FF1 File Offset: 0x001A51F1
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x060049F7 RID: 18935 RVA: 0x001A700C File Offset: 0x001A520C
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x04003081 RID: 12417
	private const int WILT_LEVELS = 3;

	// Token: 0x04003082 RID: 12418
	[MyCmpReq]
	private Crop crop;

	// Token: 0x04003083 RID: 12419
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04003084 RID: 12420
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04003085 RID: 12421
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04003086 RID: 12422
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04003087 RID: 12423
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x04003088 RID: 12424
	public bool wiltsOnReadyToHarvest;

	// Token: 0x04003089 RID: 12425
	public static StandardCropPlant.AnimSet defaultAnimSet = new StandardCropPlant.AnimSet
	{
		grow = "grow",
		grow_pst = "grow_pst",
		idle_full = "idle_full",
		wilt_base = "wilt",
		harvest = "harvest",
		waning = "waning"
	};

	// Token: 0x0400308A RID: 12426
	public StandardCropPlant.AnimSet anims = StandardCropPlant.defaultAnimSet;

	// Token: 0x02001A1A RID: 6682
	public class AnimSet
	{
		// Token: 0x06009F24 RID: 40740 RVA: 0x0037B508 File Offset: 0x00379708
		public string GetWiltLevel(int level)
		{
			if (this.m_wilt == null)
			{
				this.m_wilt = new string[3];
				for (int i = 0; i < 3; i++)
				{
					this.m_wilt[i] = this.wilt_base + (i + 1).ToString();
				}
			}
			return this.m_wilt[level - 1];
		}

		// Token: 0x04007B58 RID: 31576
		public string grow;

		// Token: 0x04007B59 RID: 31577
		public string grow_pst;

		// Token: 0x04007B5A RID: 31578
		public string idle_full;

		// Token: 0x04007B5B RID: 31579
		public string wilt_base;

		// Token: 0x04007B5C RID: 31580
		public string harvest;

		// Token: 0x04007B5D RID: 31581
		public string waning;

		// Token: 0x04007B5E RID: 31582
		private string[] m_wilt;
	}

	// Token: 0x02001A1B RID: 6683
	public class States : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant>
	{
		// Token: 0x06009F26 RID: 40742 RVA: 0x0037B568 File Offset: 0x00379768
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component != null && component.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blighted.InitializeStates(this.masterTarget, this.dead).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.waning, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Crop_Blighted, null).TagTransition(GameTags.Blighted, this.alive, true);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle).ToggleComponent<Growing>(false).TagTransition(GameTags.Blighted, this.blighted, false);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.pre_fruiting, (StandardCropPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.CropSleep, this.alive.sleeping, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping)).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Paused).Enter(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent)).Update(new Action<StandardCropPlant.StatesInstance, float>(StandardCropPlant.States.RefreshPositionPercent), UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.ConsumePlant, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent));
			this.alive.pre_fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow_pst, KAnim.PlayMode.Once).TriggerOnEnter(GameHashes.BurstEmitDisease, null).EventTransition(GameHashes.AnimQueueComplete, this.alive.fruiting, null).EventTransition(GameHashes.Wilt, this.alive.wilting, null).ScheduleGoTo(2f, this.alive.fruiting);
			this.alive.fruiting_lost.Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).GoTo(this.alive.idle);
			this.alive.wilting.PlayAnim(new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim), KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, (StandardCropPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Harvest, this.alive.harvest, null);
			this.alive.sleeping.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Once).EventTransition(GameHashes.CropWakeUp, this.alive.idle, GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Not(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping))).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Wilt, this.alive.wilting, null);
			this.alive.fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.idle_full, KAnim.PlayMode.Loop).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).EventHandlerTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi, object obj) => smi.master.wiltsOnReadyToHarvest).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Grow, this.alive.fruiting_lost, (StandardCropPlant.StatesInstance smi) => !smi.master.growing.ReachedNextHarvest());
			this.alive.harvest.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.harvest, KAnim.PlayMode.Once).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master != null)
				{
					smi.master.crop.SpawnConfiguredFruit(null);
				}
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).Exit(delegate(StandardCropPlant.StatesInstance smi)
			{
				smi.Trigger(113170146, null);
			}).OnAnimQueueComplete(this.alive.idle);
		}

		// Token: 0x06009F27 RID: 40743 RVA: 0x0037BA60 File Offset: 0x00379C60
		private static string GetWiltAnim(StandardCropPlant.StatesInstance smi)
		{
			float num = smi.master.growing.PercentOfCurrentHarvest();
			int level;
			if (num < 0.75f)
			{
				level = 1;
			}
			else if (num < 1f)
			{
				level = 2;
			}
			else
			{
				level = 3;
			}
			return smi.master.anims.GetWiltLevel(level);
		}

		// Token: 0x06009F28 RID: 40744 RVA: 0x0037BAA9 File Offset: 0x00379CA9
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi, float dt)
		{
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x06009F29 RID: 40745 RVA: 0x0037BAB6 File Offset: 0x00379CB6
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi)
		{
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x06009F2A RID: 40746 RVA: 0x0037BAC4 File Offset: 0x00379CC4
		public bool IsSleeping(StandardCropPlant.StatesInstance smi)
		{
			CropSleepingMonitor.Instance smi2 = smi.master.GetSMI<CropSleepingMonitor.Instance>();
			return smi2 != null && smi2.IsSleeping();
		}

		// Token: 0x04007B5F RID: 31583
		public StandardCropPlant.States.AliveStates alive;

		// Token: 0x04007B60 RID: 31584
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State dead;

		// Token: 0x04007B61 RID: 31585
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState blighted;

		// Token: 0x020025E9 RID: 9705
		public class AliveStates : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState
		{
			// Token: 0x0400A8C4 RID: 43204
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State idle;

			// Token: 0x0400A8C5 RID: 43205
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_fruiting;

			// Token: 0x0400A8C6 RID: 43206
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_lost;

			// Token: 0x0400A8C7 RID: 43207
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State barren;

			// Token: 0x0400A8C8 RID: 43208
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting;

			// Token: 0x0400A8C9 RID: 43209
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wilting;

			// Token: 0x0400A8CA RID: 43210
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State destroy;

			// Token: 0x0400A8CB RID: 43211
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State harvest;

			// Token: 0x0400A8CC RID: 43212
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State sleeping;
		}
	}

	// Token: 0x02001A1C RID: 6684
	public class StatesInstance : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.GameInstance
	{
		// Token: 0x06009F2C RID: 40748 RVA: 0x0037BAF0 File Offset: 0x00379CF0
		public StatesInstance(StandardCropPlant master) : base(master)
		{
		}
	}
}

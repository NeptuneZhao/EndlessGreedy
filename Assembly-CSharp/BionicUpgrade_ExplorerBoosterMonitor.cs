using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000663 RID: 1635
public class BionicUpgrade_ExplorerBoosterMonitor : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>
{
	// Token: 0x06002852 RID: 10322 RVA: 0x000E4844 File Offset: 0x000E2A44
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.attachToBooster;
		this.attachToBooster.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.FindAndAttachToInstalledBooster)).GoTo(this.Inactive);
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.MinionMigration, (BionicUpgrade_ExplorerBoosterMonitor.Instance smi) => Game.Instance, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBatterySaveMode))).EventTransition(GameHashes.ScheduleChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBatterySaveMode))).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline))).EventTransition(GameHashes.MinionMigration, (BionicUpgrade_ExplorerBoosterMonitor.Instance smi) => Game.Instance, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive))).DefaultState(this.Active.gatheringData);
		this.Active.gatheringData.OnSignal(this.ReadyToDiscoverSignal, this.Active.discover, new Func<BionicUpgrade_ExplorerBoosterMonitor.Instance, bool>(BionicUpgrade_ExplorerBoosterMonitor.IsReadyToDiscoverAndThereIsSomethingToDiscover)).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicExplorerBooster, null).Update(new Action<BionicUpgrade_ExplorerBoosterMonitor.Instance, float>(BionicUpgrade_ExplorerBoosterMonitor.DataGatheringUpdate), UpdateRate.SIM_200ms, false);
		this.Active.discover.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.ConsumeAllData)).Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.RevealUndiscoveredGeyser)).EnterTransition(this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover))).GoTo(this.Active.gatheringData);
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x000E4A8E File Offset: 0x000E2C8E
	public static bool ShouldBeActive(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline(smi) && BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBatterySaveMode(smi) && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x000E4AA8 File Offset: 0x000E2CA8
	public static bool IsReadyToDiscoverAndThereIsSomethingToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		return smi.IsReadyToDiscover && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x000E4ABA File Offset: 0x000E2CBA
	public static void ConsumeAllData(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		smi.ConsumeAllData();
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x000E4AC2 File Offset: 0x000E2CC2
	public static void FindAndAttachToInstalledBooster(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		smi.Initialize();
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x000E4ACA File Offset: 0x000E2CCA
	public static void DataGatheringUpdate(BionicUpgrade_ExplorerBoosterMonitor.Instance smi, float dt)
	{
		smi.GatheringDataUpdate(dt);
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x000E4AD4 File Offset: 0x000E2CD4
	public static bool IsThereGeysersToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		if (myWorld.id != 255)
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			list.AddRange(SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("GeyserGeneric", myWorld.id, false));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("OilWell", myWorld.id, false));
			return list.Count > 0;
		}
		return false;
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x000E4B70 File Offset: 0x000E2D70
	public static void RevealUndiscoveredGeyser(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		if (myWorld.id != 255)
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			list.AddRange(SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("GeyserGeneric", myWorld.id, false));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("OilWell", myWorld.id, false));
			if (list.Count > 0)
			{
				WorldGenSpawner.Spawnable random = list.GetRandom<WorldGenSpawner.Spawnable>();
				int baseX;
				int baseY;
				Grid.CellToXY(random.cell, out baseX, out baseY);
				GridVisibility.Reveal(baseX, baseY, 4, 4f);
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification geyserDiscoveredNotification = smi.GetGeyserDiscoveredNotification();
				int cell = random.cell;
				geyserDiscoveredNotification.customClickCallback = delegate(object obj)
				{
					GameUtil.FocusCamera(cell);
				};
				notifier.Add(geyserDiscoveredNotification, "");
			}
		}
	}

	// Token: 0x04001739 RID: 5945
	public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State attachToBooster;

	// Token: 0x0400173A RID: 5946
	public new BionicUpgrade_ExplorerBoosterMonitor.ActiveStates Active;

	// Token: 0x0400173B RID: 5947
	public StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Signal ReadyToDiscoverSignal;

	// Token: 0x0200143F RID: 5183
	public new class Def : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def
	{
		// Token: 0x060089DE RID: 35294 RVA: 0x00331B43 File Offset: 0x0032FD43
		public Def(string upgradeID) : base(upgradeID)
		{
		}
	}

	// Token: 0x02001440 RID: 5184
	public class ActiveStates : GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State
	{
		// Token: 0x0400693D RID: 26941
		public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State gatheringData;

		// Token: 0x0400693E RID: 26942
		public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State discover;
	}

	// Token: 0x02001441 RID: 5185
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.BaseInstance
	{
		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060089E0 RID: 35296 RVA: 0x00331B54 File Offset: 0x0032FD54
		public bool IsReadyToDiscover
		{
			get
			{
				return this.explorerBooster != null && this.explorerBooster.IsReady;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060089E1 RID: 35297 RVA: 0x00331B6B File Offset: 0x0032FD6B
		public float CurrentProgress
		{
			get
			{
				if (this.explorerBooster != null)
				{
					return this.explorerBooster.Progress;
				}
				return 0f;
			}
		}

		// Token: 0x060089E2 RID: 35298 RVA: 0x00331B86 File Offset: 0x0032FD86
		public Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBoosterMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060089E3 RID: 35299 RVA: 0x00331B90 File Offset: 0x0032FD90
		public void Initialize()
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					BionicUpgrade_ExplorerBooster.Instance smi = upgradeComponentSlot.installedUpgradeComponent.GetSMI<BionicUpgrade_ExplorerBooster.Instance>();
					if (smi != null && !smi.IsBeingMonitored)
					{
						this.explorerBooster = smi;
						smi.SetMonitor(this);
						return;
					}
				}
			}
		}

		// Token: 0x060089E4 RID: 35300 RVA: 0x00331BEE File Offset: 0x0032FDEE
		protected override void OnCleanUp()
		{
			if (this.explorerBooster != null)
			{
				this.explorerBooster.SetMonitor(null);
			}
			base.OnCleanUp();
		}

		// Token: 0x060089E5 RID: 35301 RVA: 0x00331C0C File Offset: 0x0032FE0C
		public void GatheringDataUpdate(float dt)
		{
			bool isReadyToDiscover = this.IsReadyToDiscover;
			float dataProgressDelta = (dt == 0f) ? 0f : (dt / 600f);
			this.explorerBooster.AddData(dataProgressDelta);
			if (this.IsReadyToDiscover && !isReadyToDiscover)
			{
				base.sm.ReadyToDiscoverSignal.Trigger(this);
			}
		}

		// Token: 0x060089E6 RID: 35302 RVA: 0x00331C5F File Offset: 0x0032FE5F
		public void ConsumeAllData()
		{
			this.explorerBooster.SetDataProgress(0f);
		}

		// Token: 0x060089E7 RID: 35303 RVA: 0x00331C74 File Offset: 0x0032FE74
		public Notification GetGeyserDiscoveredNotification()
		{
			return new Notification(DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_NAME, NotificationType.MessageImportant, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		}

		// Token: 0x060089E8 RID: 35304 RVA: 0x00331CBE File Offset: 0x0032FEBE
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x060089E9 RID: 35305 RVA: 0x00331CE4 File Offset: 0x0032FEE4
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x0400693F RID: 26943
		private BionicUpgrade_ExplorerBooster.Instance explorerBooster;
	}
}

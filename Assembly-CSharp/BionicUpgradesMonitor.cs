using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200096B RID: 2411
public class BionicUpgradesMonitor : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>
{
	// Token: 0x060046A1 RID: 18081 RVA: 0x001940F0 File Offset: 0x001922F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.initialize;
		this.initialize.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.InitializeSlots)).EnterTransition(this.firstSpawn, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsFirstTimeSpawningThisBionic)).GoTo(this.inactive);
		this.firstSpawn.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.SpawnAndInstallInitialUpgrade)).GoTo(this.inactive);
		this.inactive.EventTransition(GameHashes.BionicOnline, this.active, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers));
		this.active.DefaultState(this.active.idle).EventTransition(GameHashes.BionicOffline, this.inactive, GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Not(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline))).OnSignal(this.UpgradeInstallationStarted, this.installing).OnSignal(this.UpgradeUninstallStarted, this.uninstalling).EventHandler(GameHashes.BionicUpgradeWattageChanged, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers)).ToggleStateMachineList(new Func<BionicUpgradesMonitor.Instance, Func<BionicUpgradesMonitor.Instance, StateMachine.Instance>[]>(BionicUpgradesMonitor.GetUpgradesSMIs));
		this.active.idle.OnSignal(this.UpgradeSlotAssignationChanged, this.active.seeking, new Func<BionicUpgradesMonitor.Instance, bool>(BionicUpgradesMonitor.WantsToInstallNewUpgrades));
		this.active.seeking.OnSignal(this.UpgradeSlotAssignationChanged, this.active.idle, new Func<BionicUpgradesMonitor.Instance, bool>(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).DefaultState(this.active.seeking.unreachable);
		this.active.seeking.unreachable.EventTransition(GameHashes.NavigationCellChanged, this.active.seeking.inProgress, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.CanReachUpgradeToInstall));
		this.active.seeking.inProgress.ToggleChore((BionicUpgradesMonitor.Instance smi) => new SeekAndInstallBionicUpgradeChore(smi.master), this.active.idle, this.active.seeking.failed);
		this.active.seeking.failed.EnterTransition(this.active.idle, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).GoTo(this.active.seeking.unreachable);
		this.installing.ScheduleActionNextFrame("Active Delay", delegate(BionicUpgradesMonitor.Instance smi)
		{
			smi.GoTo(this.active);
		});
		this.uninstalling.ScheduleActionNextFrame("Delayed Redirection", delegate(BionicUpgradesMonitor.Instance smi)
		{
			smi.GoTo(this.active);
		});
	}

	// Token: 0x060046A2 RID: 18082 RVA: 0x001943A3 File Offset: 0x001925A3
	public static void InitializeSlots(BionicUpgradesMonitor.Instance smi)
	{
		smi.InitializeSlots();
	}

	// Token: 0x060046A3 RID: 18083 RVA: 0x001943AB File Offset: 0x001925AB
	public static bool IsBionicOnline(BionicUpgradesMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x060046A4 RID: 18084 RVA: 0x001943B3 File Offset: 0x001925B3
	public static bool CanReachUpgradeToInstall(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeAssignedAndReachable;
	}

	// Token: 0x060046A5 RID: 18085 RVA: 0x001943BB File Offset: 0x001925BB
	public static bool CanNotReachUpgradeToInstall(BionicUpgradesMonitor.Instance smi)
	{
		return !BionicUpgradesMonitor.CanReachUpgradeToInstall(smi);
	}

	// Token: 0x060046A6 RID: 18086 RVA: 0x001943C6 File Offset: 0x001925C6
	public static bool WantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeAssigned;
	}

	// Token: 0x060046A7 RID: 18087 RVA: 0x001943CE File Offset: 0x001925CE
	public static bool DoesNotWantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi)
	{
		return !BionicUpgradesMonitor.WantsToInstallNewUpgrades(smi);
	}

	// Token: 0x060046A8 RID: 18088 RVA: 0x001943D9 File Offset: 0x001925D9
	public static bool HasUpgradesInstalled(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeInstalled;
	}

	// Token: 0x060046A9 RID: 18089 RVA: 0x001943E1 File Offset: 0x001925E1
	public static bool IsFirstTimeSpawningThisBionic(BionicUpgradesMonitor.Instance smi)
	{
		return !smi.sm.InitialUpgradeSpawned.Get(smi);
	}

	// Token: 0x060046AA RID: 18090 RVA: 0x001943F7 File Offset: 0x001925F7
	public static void UpdateBatteryMonitorWattageModifiers(BionicUpgradesMonitor.Instance smi)
	{
		smi.UpdateBatteryMonitorWattageModifiers();
	}

	// Token: 0x060046AB RID: 18091 RVA: 0x001943FF File Offset: 0x001925FF
	public static Func<StateMachine.Instance, StateMachine.Instance>[] GetUpgradesSMIs(BionicUpgradesMonitor.Instance smi)
	{
		return smi.GetUpgradesSMIs();
	}

	// Token: 0x060046AC RID: 18092 RVA: 0x00194408 File Offset: 0x00192608
	public static void SpawnAndInstallInitialUpgrade(BionicUpgradesMonitor.Instance smi)
	{
		string text = smi.GetComponent<Traits>().GetTraitIds().Find((string t) => DUPLICANTSTATS.BIONICUPGRADETRAITS.Find((DUPLICANTSTATS.TraitVal st) => st.id == t).id == t);
		if (text != null)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(BionicUpgradeComponentConfig.GetBionicUpgradePrefabIDWithTraitID(text)), smi.master.transform.position);
			gameObject.SetActive(true);
			IAssignableIdentity component = smi.GetComponent<IAssignableIdentity>();
			BionicUpgradeComponent component2 = gameObject.GetComponent<BionicUpgradeComponent>();
			component2.Assign(component);
			smi.InstallUpgrade(component2);
		}
		smi.sm.InitialUpgradeSpawned.Set(true, smi, false);
	}

	// Token: 0x04002E07 RID: 11783
	public const int MAX_SLOT_COUNT = 3;

	// Token: 0x04002E08 RID: 11784
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State initialize;

	// Token: 0x04002E09 RID: 11785
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State firstSpawn;

	// Token: 0x04002E0A RID: 11786
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State installing;

	// Token: 0x04002E0B RID: 11787
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State uninstalling;

	// Token: 0x04002E0C RID: 11788
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inactive;

	// Token: 0x04002E0D RID: 11789
	public BionicUpgradesMonitor.ActiveStates active;

	// Token: 0x04002E0E RID: 11790
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeSlotAssignationChanged;

	// Token: 0x04002E0F RID: 11791
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeUninstallStarted;

	// Token: 0x04002E10 RID: 11792
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeInstallationStarted;

	// Token: 0x04002E11 RID: 11793
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.BoolParameter InitialUpgradeSpawned;

	// Token: 0x020018EE RID: 6382
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040077E4 RID: 30692
		public int SlotCount = 3;
	}

	// Token: 0x020018EF RID: 6383
	public class SeekingStates : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
	{
		// Token: 0x040077E5 RID: 30693
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State unreachable;

		// Token: 0x040077E6 RID: 30694
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inProgress;

		// Token: 0x040077E7 RID: 30695
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State failed;
	}

	// Token: 0x020018F0 RID: 6384
	public class ActiveStates : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
	{
		// Token: 0x040077E8 RID: 30696
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State idle;

		// Token: 0x040077E9 RID: 30697
		public BionicUpgradesMonitor.SeekingStates seeking;
	}

	// Token: 0x020018F1 RID: 6385
	public new class Instance : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.GameInstance
	{
		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06009A81 RID: 39553 RVA: 0x0036D435 File Offset: 0x0036B635
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06009A82 RID: 39554 RVA: 0x0036D44C File Offset: 0x0036B64C
		public bool HasAnyUpgradeAssigned
		{
			get
			{
				return this.upgradeComponentSlots != null && this.GetAnyAssignedSlot() != null;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06009A83 RID: 39555 RVA: 0x0036D461 File Offset: 0x0036B661
		public bool HasAnyUpgradeAssignedAndReachable
		{
			get
			{
				return this.GetAnyReachableAssignedSlot() != null;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06009A84 RID: 39556 RVA: 0x0036D46C File Offset: 0x0036B66C
		public bool HasAnyUpgradeInstalled
		{
			get
			{
				return this.upgradeComponentSlots != null && this.GetAnyInstalledUpgradeSlot() != null;
			}
		}

		// Token: 0x06009A85 RID: 39557 RVA: 0x0036D484 File Offset: 0x0036B684
		public Instance(IStateMachineTarget master, BionicUpgradesMonitor.Def def) : base(master, def)
		{
			IAssignableIdentity component = base.GetComponent<IAssignableIdentity>();
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.minionOwnables = component.GetSoleOwner();
			this.upgradesStorage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicUpgradeStorage);
			this.CreateAssignableSlots();
			this.CreateUpgradeSlots();
		}

		// Token: 0x06009A86 RID: 39558 RVA: 0x0036D500 File Offset: 0x0036B700
		public void InstallUpgrade(BionicUpgradeComponent upgradeComponent)
		{
			BionicUpgradesMonitor.UpgradeComponentSlot slotForAssignedUpgrade = this.GetSlotForAssignedUpgrade(upgradeComponent);
			if (slotForAssignedUpgrade == null)
			{
				return;
			}
			base.sm.UpgradeInstallationStarted.Trigger(this);
			slotForAssignedUpgrade.InternalInstall();
		}

		// Token: 0x06009A87 RID: 39559 RVA: 0x0036D530 File Offset: 0x0036B730
		public void UninstallUpgrade(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			if (slot != null && slot.HasUpgradeInstalled)
			{
				base.sm.UpgradeUninstallStarted.Trigger(this);
				slot.InternalUninstall();
			}
		}

		// Token: 0x06009A88 RID: 39560 RVA: 0x0036D554 File Offset: 0x0036B754
		public void UpdateBatteryMonitorWattageModifiers()
		{
			bool flag = false;
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				string text = "UPGRADE_SLOT_" + i.ToString();
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (!upgradeComponentSlot.HasUpgradeInstalled)
				{
					flag |= this.batteryMonitor.RemoveModifier(text, false);
				}
				else
				{
					BionicBatteryMonitor.WattageModifier modifier = new BionicBatteryMonitor.WattageModifier
					{
						id = text,
						name = upgradeComponentSlot.installedUpgradeComponent.CurrentWattageName,
						value = upgradeComponentSlot.installedUpgradeComponent.CurrentWattage,
						potentialValue = upgradeComponentSlot.installedUpgradeComponent.PotentialWattage
					};
					flag |= this.batteryMonitor.AddOrUpdateModifier(modifier, false);
				}
			}
			if (flag)
			{
				this.batteryMonitor.Trigger(1361471071, null);
			}
		}

		// Token: 0x06009A89 RID: 39561 RVA: 0x0036D620 File Offset: 0x0036B820
		public Func<StateMachine.Instance, StateMachine.Instance>[] GetUpgradesSMIs()
		{
			List<Func<StateMachine.Instance, StateMachine.Instance>> list = new List<Func<StateMachine.Instance, StateMachine.Instance>>();
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot.installedUpgradeComponent != null && upgradeComponentSlot.StateMachine != null)
				{
					list.Add(upgradeComponentSlot.StateMachine);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06009A8A RID: 39562 RVA: 0x0036D678 File Offset: 0x0036B878
		private void CreateAssignableSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			Equipment component = this.minionOwnables.GetComponent<Equipment>();
			int num = Mathf.Max(0, 2);
			for (int i = 0; i < num; i++)
			{
				string str = (i + 2).ToString();
				if (bionicUpgrade is OwnableSlot)
				{
					OwnableSlotInstance ownableSlotInstance = new OwnableSlotInstance(this.minionOwnables, (OwnableSlot)bionicUpgrade);
					OwnableSlotInstance ownableSlotInstance2 = ownableSlotInstance;
					ownableSlotInstance2.ID += str;
					this.minionOwnables.Add(ownableSlotInstance);
				}
				else if (bionicUpgrade is EquipmentSlot)
				{
					EquipmentSlotInstance equipmentSlotInstance = new EquipmentSlotInstance(component, (EquipmentSlot)bionicUpgrade);
					EquipmentSlotInstance equipmentSlotInstance2 = equipmentSlotInstance;
					equipmentSlotInstance2.ID += str;
					component.Add(equipmentSlotInstance);
				}
			}
		}

		// Token: 0x06009A8B RID: 39563 RVA: 0x0036D73C File Offset: 0x0036B93C
		private void CreateUpgradeSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			AssignableSlotInstance[] slots = this.minionOwnables.GetSlots(bionicUpgrade);
			this.upgradeComponentSlots = new BionicUpgradesMonitor.UpgradeComponentSlot[slots.Length];
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = new BionicUpgradesMonitor.UpgradeComponentSlot();
				this.upgradeComponentSlots[i] = upgradeComponentSlot;
			}
		}

		// Token: 0x06009A8C RID: 39564 RVA: 0x0036D798 File Offset: 0x0036B998
		public void InitializeSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			AssignableSlotInstance[] slots = this.minionOwnables.GetSlots(bionicUpgrade);
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				AssignableSlotInstance assignableSlotInstance = slots[i];
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				upgradeComponentSlot.Initialize(assignableSlotInstance, this.upgradesStorage);
				upgradeComponentSlot.OnInstalledUpgradeReassigned = (Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>)Delegate.Combine(upgradeComponentSlot.OnInstalledUpgradeReassigned, new Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>(this.OnInstalledUpgradeComponentReassigned));
				upgradeComponentSlot.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Combine(upgradeComponentSlot.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnSlotAssignationChanged));
			}
			for (int j = 0; j < this.upgradeComponentSlots.Length; j++)
			{
				this.upgradeComponentSlots[j].OnSpawn(this);
			}
		}

		// Token: 0x06009A8D RID: 39565 RVA: 0x0036D857 File Offset: 0x0036BA57
		private void OnSlotAssignationChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			base.sm.UpgradeSlotAssignationChanged.Trigger(this);
		}

		// Token: 0x06009A8E RID: 39566 RVA: 0x0036D86A File Offset: 0x0036BA6A
		private void OnInstalledUpgradeComponentReassigned(BionicUpgradesMonitor.UpgradeComponentSlot slot, IAssignableIdentity new_assignee)
		{
			if (!slot.AssignedUpgradeMatchesInstalledUpgrade)
			{
				this.UninstallUpgrade(slot);
			}
		}

		// Token: 0x06009A8F RID: 39567 RVA: 0x0036D87C File Offset: 0x0036BA7C
		private BionicUpgradesMonitor.UpgradeComponentSlot GetSlotForAssignedUpgrade(BionicUpgradeComponent upgradeComponent)
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && upgradeComponentSlot.assignedUpgradeComponent == upgradeComponent)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x06009A90 RID: 39568 RVA: 0x0036D8CC File Offset: 0x0036BACC
		public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyAssignedSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x06009A91 RID: 39569 RVA: 0x0036D90C File Offset: 0x0036BB0C
		public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyReachableAssignedSlot()
		{
			Navigator component = base.GetComponent<Navigator>();
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && component.CanReach(upgradeComponentSlot.assignedUpgradeComponent.GetComponent<IApproachable>()))
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x06009A92 RID: 39570 RVA: 0x0036D968 File Offset: 0x0036BB68
		private BionicUpgradesMonitor.UpgradeComponentSlot GetAnyInstalledUpgradeSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && upgradeComponentSlot.HasUpgradeInstalled)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x06009A93 RID: 39571 RVA: 0x0036D9A0 File Offset: 0x0036BBA0
		public BionicUpgradesMonitor.UpgradeComponentSlot GetFirstEmptySlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (!upgradeComponentSlot.HasUpgradeInstalled && !upgradeComponentSlot.HasUpgradeComponentAssigned)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x040077EA RID: 30698
		[Serialize]
		public BionicUpgradesMonitor.UpgradeComponentSlot[] upgradeComponentSlots;

		// Token: 0x040077EB RID: 30699
		private BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x040077EC RID: 30700
		private Storage upgradesStorage;

		// Token: 0x040077ED RID: 30701
		private Ownables minionOwnables;
	}

	// Token: 0x020018F2 RID: 6386
	[SerializationConfig(MemberSerialization.OptIn)]
	public class UpgradeComponentSlot
	{
		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06009A94 RID: 39572 RVA: 0x0036D9DC File Offset: 0x0036BBDC
		public bool HasUpgradeInstalled
		{
			get
			{
				return this.installedUpgradePrefabID != Tag.Invalid;
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06009A95 RID: 39573 RVA: 0x0036D9EE File Offset: 0x0036BBEE
		public bool HasUpgradeComponentAssigned
		{
			get
			{
				return this.assignableSlotInstance.IsAssigned() && !this.assignableSlotInstance.IsUnassigning();
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06009A96 RID: 39574 RVA: 0x0036DA0D File Offset: 0x0036BC0D
		public bool AssignedUpgradeMatchesInstalledUpgrade
		{
			get
			{
				return this.assignedUpgradeComponent == this.installedUpgradeComponent;
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06009A98 RID: 39576 RVA: 0x0036DA29 File Offset: 0x0036BC29
		// (set) Token: 0x06009A97 RID: 39575 RVA: 0x0036DA20 File Offset: 0x0036BC20
		public bool HasSpawned { get; private set; }

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06009A99 RID: 39577 RVA: 0x0036DA31 File Offset: 0x0036BC31
		public float WattageCost
		{
			get
			{
				if (!this.HasUpgradeInstalled)
				{
					return 0f;
				}
				return this.installedUpgradeComponent.CurrentWattage;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06009A9A RID: 39578 RVA: 0x0036DA4C File Offset: 0x0036BC4C
		public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
		{
			get
			{
				if (!this.HasUpgradeInstalled)
				{
					return null;
				}
				return this.installedUpgradeComponent.StateMachine;
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06009A9B RID: 39579 RVA: 0x0036DA63 File Offset: 0x0036BC63
		public Tag InstalledUpgradeID
		{
			get
			{
				return this.installedUpgradePrefabID;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06009A9C RID: 39580 RVA: 0x0036DA6B File Offset: 0x0036BC6B
		public BionicUpgradeComponent assignedUpgradeComponent
		{
			get
			{
				if (!this.assignableSlotInstance.IsUnassigning())
				{
					return this.assignableSlotInstance.assignable as BionicUpgradeComponent;
				}
				return null;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06009A9D RID: 39581 RVA: 0x0036DA8C File Offset: 0x0036BC8C
		public BionicUpgradeComponent installedUpgradeComponent
		{
			get
			{
				if (this.HasUpgradeInstalled)
				{
					if (this._installedUpgradeComponent == null)
					{
						global::Debug.LogWarning("Error on BionicUpgradeMonitor. storage does not contains bionic upgrade with id " + this.InstalledUpgradeID.ToString() + " this could be due to loading an old save on a new version");
						this.installedUpgradePrefabID = Tag.Invalid;
					}
					return this._installedUpgradeComponent;
				}
				this._installedUpgradeComponent = null;
				return null;
			}
		}

		// Token: 0x06009A9F RID: 39583 RVA: 0x0036DB0C File Offset: 0x0036BD0C
		public void Initialize(AssignableSlotInstance assignableSlotInstance, Storage storage)
		{
			this.assignableSlotInstance = assignableSlotInstance;
			this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().Subscribe(-1585839766, new Action<object>(this.OnAssignablesChanged));
			this.storage = storage;
			this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
		}

		// Token: 0x06009AA0 RID: 39584 RVA: 0x0036DB5F File Offset: 0x0036BD5F
		public AssignableSlotInstance GetAssignableSlotInstance()
		{
			return this.assignableSlotInstance;
		}

		// Token: 0x06009AA1 RID: 39585 RVA: 0x0036DB68 File Offset: 0x0036BD68
		public void OnSpawn(BionicUpgradesMonitor.Instance smi)
		{
			if (this.HasUpgradeInstalled && this._installedUpgradeComponent == null)
			{
				GameObject gameObject = null;
				int num = 0;
				List<GameObject> list = new List<GameObject>();
				this.storage.Find(this.InstalledUpgradeID, list);
				while (num < list.Count && this._installedUpgradeComponent == null)
				{
					GameObject gameObject2 = list[num];
					bool flag = false;
					foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
					{
						if (upgradeComponentSlot != this && upgradeComponentSlot.HasSpawned && !(upgradeComponentSlot.InstalledUpgradeID != this.InstalledUpgradeID) && upgradeComponentSlot.installedUpgradeComponent.gameObject == gameObject2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						gameObject = gameObject2;
						break;
					}
					num++;
				}
				if (gameObject != null)
				{
					this._installedUpgradeComponent = gameObject.GetComponent<BionicUpgradeComponent>();
				}
			}
			if (this.HasUpgradeInstalled && this.installedUpgradeComponent != null)
			{
				this.SubscribeToInstallledUpgradeAssignable();
			}
			this.HasSpawned = true;
		}

		// Token: 0x06009AA2 RID: 39586 RVA: 0x0036DC76 File Offset: 0x0036BE76
		public void SubscribeToInstallledUpgradeAssignable()
		{
			this.UnsubscribeFromInstalledUpgradeAssignable();
			this.installedUpgradeSubscribeCallbackIDX = this.installedUpgradeComponent.Subscribe(684616645, new Action<object>(this.OnInstalledComponentReassigned));
		}

		// Token: 0x06009AA3 RID: 39587 RVA: 0x0036DCA0 File Offset: 0x0036BEA0
		public void UnsubscribeFromInstalledUpgradeAssignable()
		{
			if (this.installedUpgradeSubscribeCallbackIDX != -1)
			{
				this.installedUpgradeComponent.Unsubscribe(this.installedUpgradeSubscribeCallbackIDX);
				this.installedUpgradeSubscribeCallbackIDX = -1;
			}
		}

		// Token: 0x06009AA4 RID: 39588 RVA: 0x0036DCC4 File Offset: 0x0036BEC4
		private void OnInstalledComponentReassigned(object obj)
		{
			IAssignableIdentity arg = (obj == null) ? null : ((IAssignableIdentity)obj);
			Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> onInstalledUpgradeReassigned = this.OnInstalledUpgradeReassigned;
			if (onInstalledUpgradeReassigned == null)
			{
				return;
			}
			onInstalledUpgradeReassigned(this, arg);
		}

		// Token: 0x06009AA5 RID: 39589 RVA: 0x0036DCF0 File Offset: 0x0036BEF0
		private void OnAssignablesChanged(object o)
		{
			if (this._lastAssignedUpgradeComponent != this.assignedUpgradeComponent)
			{
				this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
				Action<BionicUpgradesMonitor.UpgradeComponentSlot> onAssignedUpgradeChanged = this.OnAssignedUpgradeChanged;
				if (onAssignedUpgradeChanged == null)
				{
					return;
				}
				onAssignedUpgradeChanged(this);
			}
		}

		// Token: 0x06009AA6 RID: 39590 RVA: 0x0036DD24 File Offset: 0x0036BF24
		public void InternalInstall()
		{
			if (!this.HasUpgradeInstalled && this.HasUpgradeComponentAssigned)
			{
				this.storage.Store(this.assignedUpgradeComponent.gameObject, true, false, true, false);
				this.installedUpgradePrefabID = this.assignedUpgradeComponent.PrefabID();
				this._installedUpgradeComponent = this.assignedUpgradeComponent;
				this.SubscribeToInstallledUpgradeAssignable();
				GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject != null)
				{
					targetGameObject.Trigger(2000325176, null);
				}
			}
		}

		// Token: 0x06009AA7 RID: 39591 RVA: 0x0036DDAC File Offset: 0x0036BFAC
		public void InternalUninstall()
		{
			if (this.HasUpgradeInstalled)
			{
				this.UnsubscribeFromInstalledUpgradeAssignable();
				GameObject gameObject = this.installedUpgradeComponent.gameObject;
				this.installedUpgradeComponent.Unassign();
				this.storage.Drop(gameObject, true);
				this.installedUpgradePrefabID = Tag.Invalid;
				this._installedUpgradeComponent = null;
				GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject != null)
				{
					targetGameObject.Trigger(2000325176, null);
				}
			}
		}

		// Token: 0x040077EF RID: 30703
		private BionicUpgradeComponent _installedUpgradeComponent;

		// Token: 0x040077F0 RID: 30704
		private BionicUpgradeComponent _lastAssignedUpgradeComponent;

		// Token: 0x040077F1 RID: 30705
		[Serialize]
		private Tag installedUpgradePrefabID = Tag.Invalid;

		// Token: 0x040077F2 RID: 30706
		public Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> OnInstalledUpgradeReassigned;

		// Token: 0x040077F3 RID: 30707
		public Action<BionicUpgradesMonitor.UpgradeComponentSlot> OnAssignedUpgradeChanged;

		// Token: 0x040077F4 RID: 30708
		private AssignableSlotInstance assignableSlotInstance;

		// Token: 0x040077F5 RID: 30709
		private Storage storage;

		// Token: 0x040077F6 RID: 30710
		private int installedUpgradeSubscribeCallbackIDX = -1;
	}
}

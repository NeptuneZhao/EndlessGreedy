using System;

// Token: 0x02000665 RID: 1637
public class BionicUpgrade_SM<SMType, StateMachineInstanceType> : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def> where SMType : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def> where StateMachineInstanceType : BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance
{
	// Token: 0x06002861 RID: 10337 RVA: 0x000E4DDB File Offset: 0x000E2FDB
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
	}

	// Token: 0x06002862 RID: 10338 RVA: 0x000E4DEC File Offset: 0x000E2FEC
	public static bool IsOnline(BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x000E4DF4 File Offset: 0x000E2FF4
	public static bool IsInBatterySaveMode(BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
	{
		return smi.IsInBatterySavingMode;
	}

	// Token: 0x0400173C RID: 5948
	public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Active;

	// Token: 0x0400173D RID: 5949
	public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Inactive;

	// Token: 0x02001446 RID: 5190
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060089F6 RID: 35318 RVA: 0x00331F19 File Offset: 0x00330119
		public Def(string upgradeID)
		{
			this.UpgradeID = upgradeID;
		}

		// Token: 0x04006947 RID: 26951
		public string UpgradeID;

		// Token: 0x04006948 RID: 26952
		public Func<StateMachine.Instance, StateMachine.Instance>[] StateMachinesWhenActive;
	}

	// Token: 0x02001447 RID: 5191
	public abstract class BaseInstance : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.GameInstance, BionicUpgradeComponent.IWattageController
	{
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060089F7 RID: 35319 RVA: 0x00331F28 File Offset: 0x00330128
		public bool IsInBatterySavingMode
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsBatterySaveModeActive;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x060089F8 RID: 35320 RVA: 0x00331F3F File Offset: 0x0033013F
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x060089F9 RID: 35321 RVA: 0x00331F56 File Offset: 0x00330156
		public BionicUpgradeComponentConfig.BionicUpgradeData Data
		{
			get
			{
				return BionicUpgradeComponentConfig.UpgradesData[base.def.UpgradeID];
			}
		}

		// Token: 0x060089FA RID: 35322 RVA: 0x00331F72 File Offset: 0x00330172
		public BaseInstance(IStateMachineTarget master, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def def) : base(master, def)
		{
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.RegisterMonitorToUpgradeComponent();
			base.Subscribe(-426516281, new Action<object>(this.OnBatterySavingModeChanged));
		}

		// Token: 0x060089FB RID: 35323 RVA: 0x00331FAC File Offset: 0x003301AC
		private void RegisterMonitorToUpgradeComponent()
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					BionicUpgradeComponent installedUpgradeComponent = upgradeComponentSlot.installedUpgradeComponent;
					if (installedUpgradeComponent != null && !installedUpgradeComponent.HasWattageController)
					{
						this.upgradeComponent = installedUpgradeComponent;
						installedUpgradeComponent.SetWattageController(this);
						return;
					}
				}
			}
		}

		// Token: 0x060089FC RID: 35324 RVA: 0x0033200B File Offset: 0x0033020B
		private void UnregisterMonitorToUpgradeComponent()
		{
			if (this.upgradeComponent != null)
			{
				this.upgradeComponent.SetWattageController(null);
			}
		}

		// Token: 0x060089FD RID: 35325
		public abstract float GetCurrentWattageCost();

		// Token: 0x060089FE RID: 35326
		public abstract string GetCurrentWattageCostName();

		// Token: 0x060089FF RID: 35327 RVA: 0x00332027 File Offset: 0x00330227
		protected virtual void OnEnteringBatterySavingMode()
		{
		}

		// Token: 0x06008A00 RID: 35328 RVA: 0x00332029 File Offset: 0x00330229
		protected virtual void OnExitingBatterySavingMode()
		{
		}

		// Token: 0x06008A01 RID: 35329 RVA: 0x0033202B File Offset: 0x0033022B
		private void OnBatterySavingModeChanged(object o)
		{
			if ((bool)o)
			{
				this.OnEnteringBatterySavingMode();
				return;
			}
			this.OnExitingBatterySavingMode();
		}

		// Token: 0x06008A02 RID: 35330 RVA: 0x00332042 File Offset: 0x00330242
		protected override void OnCleanUp()
		{
			this.UnregisterMonitorToUpgradeComponent();
			base.Unsubscribe(-426516281, new Action<object>(this.OnBatterySavingModeChanged));
			base.OnCleanUp();
		}

		// Token: 0x04006949 RID: 26953
		protected BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x0400694A RID: 26954
		protected BionicUpgradeComponent upgradeComponent;
	}
}

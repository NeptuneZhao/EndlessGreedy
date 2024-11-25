using System;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
public class ElectrobankCharger : GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>
{
	// Token: 0x06002BDD RID: 11229 RVA: 0x000F6584 File Offset: 0x000F4784
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noBattery;
		this.noBattery.PlayAnim("off").EventHandler(GameHashes.OnStorageChange, delegate(ElectrobankCharger.Instance smi, object data)
		{
			smi.QueueElectrobank(null);
		}).ParamTransition<bool>(this.hasElectrobank, this.charging, GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.IsTrue).Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.QueueElectrobank(null);
		});
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.charging, (ElectrobankCharger.Instance smi) => smi.master.GetComponent<Operational>().IsOperational);
		this.charging.QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.QueueElectrobank(null);
			smi.master.GetComponent<Operational>().SetActive(true, false);
		}).Exit(delegate(ElectrobankCharger.Instance smi)
		{
			smi.master.GetComponent<Operational>().SetActive(false, false);
		}).Update(delegate(ElectrobankCharger.Instance smi, float dt)
		{
			smi.ChargeInternal(smi, dt);
		}, UpdateRate.SIM_EVERY_TICK, false).EventTransition(GameHashes.OperationalChanged, this.inoperational, (ElectrobankCharger.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).ParamTransition<float>(this.internalChargeAmount, this.full, (ElectrobankCharger.Instance smi, float dt) => this.internalChargeAmount.Get(smi) >= 120000f);
		this.full.PlayAnim("working_pst").Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.TransferChargeToElectrobank();
		}).OnAnimQueueComplete(this.noBattery);
	}

	// Token: 0x04001937 RID: 6455
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State noBattery;

	// Token: 0x04001938 RID: 6456
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State inoperational;

	// Token: 0x04001939 RID: 6457
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State charging;

	// Token: 0x0400193A RID: 6458
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State full;

	// Token: 0x0400193B RID: 6459
	public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.FloatParameter internalChargeAmount;

	// Token: 0x0400193C RID: 6460
	public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.BoolParameter hasElectrobank;

	// Token: 0x020014C6 RID: 5318
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014C7 RID: 5319
	public new class Instance : GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.GameInstance
	{
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06008C0C RID: 35852 RVA: 0x00338B62 File Offset: 0x00336D62
		public Storage Storage
		{
			get
			{
				if (this.storage == null)
				{
					this.storage = base.GetComponent<Storage>();
				}
				return this.storage;
			}
		}

		// Token: 0x06008C0D RID: 35853 RVA: 0x00338B84 File Offset: 0x00336D84
		public Instance(IStateMachineTarget master, ElectrobankCharger.Def def) : base(master, def)
		{
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x06008C0E RID: 35854 RVA: 0x00338BB1 File Offset: 0x00336DB1
		public void ChargeInternal(ElectrobankCharger.Instance smi, float dt)
		{
			smi.sm.internalChargeAmount.Delta(dt * 480f, smi);
			this.UpdateMeter();
		}

		// Token: 0x06008C0F RID: 35855 RVA: 0x00338BD2 File Offset: 0x00336DD2
		public void UpdateMeter()
		{
			this.meterController.SetPositionPercent(base.sm.internalChargeAmount.Get(base.smi) / 120000f);
		}

		// Token: 0x06008C10 RID: 35856 RVA: 0x00338BFB File Offset: 0x00336DFB
		public void TransferChargeToElectrobank()
		{
			this.targetElectrobank = Electrobank.ReplaceEmptyWithCharged(this.targetElectrobank, true);
			this.DequeueElectrobank();
		}

		// Token: 0x06008C11 RID: 35857 RVA: 0x00338C18 File Offset: 0x00336E18
		public void DequeueElectrobank()
		{
			this.targetElectrobank = null;
			base.smi.sm.hasElectrobank.Set(false, base.smi, false);
			base.smi.sm.internalChargeAmount.Set(0f, base.smi, false);
			this.UpdateMeter();
		}

		// Token: 0x06008C12 RID: 35858 RVA: 0x00338C74 File Offset: 0x00336E74
		public void QueueElectrobank(object data = null)
		{
			if (this.targetElectrobank == null)
			{
				for (int i = 0; i < this.Storage.items.Count; i++)
				{
					GameObject gameObject = this.Storage.items[i];
					if (gameObject != null && gameObject.HasTag(GameTags.EmptyPortableBattery))
					{
						this.targetElectrobank = gameObject;
						base.smi.sm.internalChargeAmount.Set(0f, base.smi, false);
						base.smi.sm.hasElectrobank.Set(true, base.smi, false);
						break;
					}
				}
			}
			this.UpdateMeter();
		}

		// Token: 0x04006AE9 RID: 27369
		private Storage storage;

		// Token: 0x04006AEA RID: 27370
		public GameObject targetElectrobank;

		// Token: 0x04006AEB RID: 27371
		private MeterController meterController;
	}
}

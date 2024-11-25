using System;
using UnityEngine;

// Token: 0x02000A11 RID: 2577
public class RadiationLight : StateMachineComponent<RadiationLight.StatesInstance>
{
	// Token: 0x06004AC1 RID: 19137 RVA: 0x001AB680 File Offset: 0x001A9880
	public void UpdateMeter()
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x06004AC2 RID: 19138 RVA: 0x001AB6A9 File Offset: 0x001A98A9
	public bool HasEnoughFuel()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06004AC3 RID: 19139 RVA: 0x001AB6B7 File Offset: 0x001A98B7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.UpdateMeter();
	}

	// Token: 0x040030FD RID: 12541
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040030FE RID: 12542
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040030FF RID: 12543
	[MyCmpGet]
	private RadiationEmitter emitter;

	// Token: 0x04003100 RID: 12544
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04003101 RID: 12545
	private MeterController meter;

	// Token: 0x04003102 RID: 12546
	public Tag elementToConsume;

	// Token: 0x04003103 RID: 12547
	public float consumptionRate;

	// Token: 0x02001A2F RID: 6703
	public class StatesInstance : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.GameInstance
	{
		// Token: 0x06009F48 RID: 40776 RVA: 0x0037BD54 File Offset: 0x00379F54
		public StatesInstance(RadiationLight smi) : base(smi)
		{
			if (base.GetComponent<Rotatable>().IsRotated)
			{
				RadiationEmitter component = base.GetComponent<RadiationEmitter>();
				component.emitDirection = 180f;
				component.emissionOffset = Vector3.left;
			}
			this.ToggleEmitter(false);
			smi.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
		}

		// Token: 0x06009F49 RID: 40777 RVA: 0x0037BDD1 File Offset: 0x00379FD1
		public void ToggleEmitter(bool on)
		{
			base.smi.master.operational.SetActive(on, false);
			base.smi.master.emitter.SetEmitting(on);
		}
	}

	// Token: 0x02001A30 RID: 6704
	public class States : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight>
	{
		// Token: 0x06009F4A RID: 40778 RVA: 0x0037BE00 File Offset: 0x0037A000
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready.idle;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(RadiationLight.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.waiting.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.ready.idle, (RadiationLight.StatesInstance smi) => smi.master.operational.IsOperational);
			this.ready.EventTransition(GameHashes.OperationalChanged, this.waiting, (RadiationLight.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.ready.idle);
			this.ready.idle.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready.on, (RadiationLight.StatesInstance smi) => smi.master.HasEnoughFuel());
			this.ready.on.PlayAnim("on").Enter(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(true);
			}).EventTransition(GameHashes.OnStorageChange, this.ready.idle, (RadiationLight.StatesInstance smi) => !smi.master.HasEnoughFuel()).Exit(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(false);
			});
		}

		// Token: 0x04007BB0 RID: 31664
		public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State waiting;

		// Token: 0x04007BB1 RID: 31665
		public RadiationLight.States.ReadyStates ready;

		// Token: 0x020025ED RID: 9709
		public class ReadyStates : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State
		{
			// Token: 0x0400A8E1 RID: 43233
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State idle;

			// Token: 0x0400A8E2 RID: 43234
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State on;
		}
	}
}

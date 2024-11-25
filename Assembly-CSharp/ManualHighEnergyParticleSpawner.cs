using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000724 RID: 1828
[SerializationConfig(MemberSerialization.OptIn)]
public class ManualHighEnergyParticleSpawner : StateMachineComponent<ManualHighEnergyParticleSpawner.StatesInstance>, IHighEnergyParticleDirection
{
	// Token: 0x17000310 RID: 784
	// (get) Token: 0x0600306C RID: 12396 RVA: 0x0010B828 File Offset: 0x00109A28
	// (set) Token: 0x0600306D RID: 12397 RVA: 0x0010B830 File Offset: 0x00109A30
	public EightDirection Direction
	{
		get
		{
			return this._direction;
		}
		set
		{
			this._direction = value;
			if (this.directionController != null)
			{
				this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
				this.directionController.controller.enabled = false;
				this.directionController.controller.enabled = true;
			}
		}
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x0010B888 File Offset: 0x00109A88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ManualHighEnergyParticleSpawner>(-905833192, ManualHighEnergyParticleSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x0010B8A4 File Offset: 0x00109AA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.radiationEmitter.SetEmitting(false);
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x0010B904 File Offset: 0x00109B04
	private void OnCopySettings(object data)
	{
		ManualHighEnergyParticleSpawner component = ((GameObject)data).GetComponent<ManualHighEnergyParticleSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
		}
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x0010B934 File Offset: 0x00109B34
	public void LauncherUpdate()
	{
		if (this.particleStorage.Particles > 0f)
		{
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.particleStorage.ConsumeAndGet(this.particleStorage.Particles);
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x04001C5E RID: 7262
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04001C5F RID: 7263
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x04001C60 RID: 7264
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04001C61 RID: 7265
	private EightDirectionController directionController;

	// Token: 0x04001C62 RID: 7266
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C63 RID: 7267
	private static readonly EventSystem.IntraObjectHandler<ManualHighEnergyParticleSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ManualHighEnergyParticleSpawner>(delegate(ManualHighEnergyParticleSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001568 RID: 5480
	public class StatesInstance : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.GameInstance
	{
		// Token: 0x06008E49 RID: 36425 RVA: 0x0034298A File Offset: 0x00340B8A
		public StatesInstance(ManualHighEnergyParticleSpawner smi) : base(smi)
		{
		}

		// Token: 0x06008E4A RID: 36426 RVA: 0x00342993 File Offset: 0x00340B93
		public bool IsComplexFabricatorWorkable(object data)
		{
			return data as ComplexFabricatorWorkable != null;
		}
	}

	// Token: 0x02001569 RID: 5481
	public class States : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner>
	{
		// Token: 0x06008E4B RID: 36427 RVA: 0x003429A4 File Offset: 0x00340BA4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.Enter(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(false);
			}).TagTransition(GameTags.Operational, this.ready, false);
			this.ready.DefaultState(this.ready.idle).TagTransition(GameTags.Operational, this.inoperational, true).Update(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate();
			}, UpdateRate.SIM_200ms, false);
			this.ready.idle.EventHandlerTransition(GameHashes.WorkableStartWork, this.ready.working, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data));
			this.ready.working.Enter(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(true);
			}).EventHandlerTransition(GameHashes.WorkableCompleteWork, this.ready.idle, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data)).EventHandlerTransition(GameHashes.WorkableStopWork, this.ready.idle, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data)).Exit(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(false);
			});
		}

		// Token: 0x04006CAC RID: 27820
		public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State inoperational;

		// Token: 0x04006CAD RID: 27821
		public ManualHighEnergyParticleSpawner.States.ReadyStates ready;

		// Token: 0x02002502 RID: 9474
		public class ReadyStates : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State
		{
			// Token: 0x0400A4A6 RID: 42150
			public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State idle;

			// Token: 0x0400A4A7 RID: 42151
			public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State working;
		}
	}
}

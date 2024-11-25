using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006EE RID: 1774
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticleSpawner : StateMachineComponent<HighEnergyParticleSpawner.StatesInstance>, IHighEnergyParticleDirection, IProgressBarSideScreen, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06002D33 RID: 11571 RVA: 0x000FDEDC File Offset: 0x000FC0DC
	public float PredictedPerCycleConsumptionRate
	{
		get
		{
			return (float)Mathf.FloorToInt(this.recentPerSecondConsumptionRate * 0.1f * 600f);
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06002D34 RID: 11572 RVA: 0x000FDEF6 File Offset: 0x000FC0F6
	// (set) Token: 0x06002D35 RID: 11573 RVA: 0x000FDF00 File Offset: 0x000FC100
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

	// Token: 0x06002D36 RID: 11574 RVA: 0x000FDF58 File Offset: 0x000FC158
	private void OnCopySettings(object data)
	{
		HighEnergyParticleSpawner component = ((GameObject)data).GetComponent<HighEnergyParticleSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.particleThreshold = component.particleThreshold;
		}
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x000FDF92 File Offset: 0x000FC192
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HighEnergyParticleSpawner>(-905833192, HighEnergyParticleSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x000FDFAC File Offset: 0x000FC1AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x000FE057 File Offset: 0x000FC257
	public float GetProgressBarMaxValue()
	{
		return this.particleThreshold;
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x000FE05F File Offset: 0x000FC25F
	public float GetProgressBarFillPercentage()
	{
		return this.particleStorage.Particles / this.particleThreshold;
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x000FE073 File Offset: 0x000FC273
	public string GetProgressBarTitleLabel()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_LABEL;
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x000FE080 File Offset: 0x000FC280
	public string GetProgressBarLabel()
	{
		return Mathf.FloorToInt(this.particleStorage.Particles).ToString() + "/" + Mathf.FloorToInt(this.particleThreshold).ToString();
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x000FE0C2 File Offset: 0x000FC2C2
	public string GetProgressBarTooltip()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_TOOLTIP;
	}

	// Token: 0x06002D3E RID: 11582 RVA: 0x000FE0CE File Offset: 0x000FC2CE
	public void DoConsumeParticlesWhileDisabled(float dt)
	{
		this.particleStorage.ConsumeAndGet(dt * 1f);
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
	}

	// Token: 0x06002D3F RID: 11583 RVA: 0x000FE0F4 File Offset: 0x000FC2F4
	public void LauncherUpdate(float dt)
	{
		this.radiationSampleTimer += dt;
		if (this.radiationSampleTimer >= this.radiationSampleRate)
		{
			this.radiationSampleTimer -= this.radiationSampleRate;
			int i = Grid.PosToCell(this);
			float num = Grid.Radiation[i];
			if (num != 0f && this.particleStorage.RemainingCapacity() > 0f)
			{
				base.smi.sm.isAbsorbingRadiation.Set(true, base.smi, false);
				this.recentPerSecondConsumptionRate = num / 600f;
				this.particleStorage.Store(this.recentPerSecondConsumptionRate * this.radiationSampleRate * 0.1f);
			}
			else
			{
				this.recentPerSecondConsumptionRate = 0f;
				base.smi.sm.isAbsorbingRadiation.Set(false, base.smi, false);
			}
		}
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
		if (!this.particleVisualPlaying && this.particleStorage.Particles > this.particleThreshold / 2f)
		{
			this.particleController.meterController.Play("orb_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.particleController.meterController.Queue("orb_idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.particleVisualPlaying = true;
		}
		this.launcherTimer += dt;
		if (this.launcherTimer < this.minLaunchInterval)
		{
			return;
		}
		if (this.particleStorage.Particles >= this.particleThreshold)
		{
			this.launcherTimer = 0f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.particleStorage.ConsumeAndGet(this.particleThreshold);
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
				this.particleVisualPlaying = false;
			}
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06002D40 RID: 11584 RVA: 0x000FE385 File Offset: 0x000FC585
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06002D41 RID: 11585 RVA: 0x000FE38C File Offset: 0x000FC58C
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x000FE398 File Offset: 0x000FC598
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06002D43 RID: 11587 RVA: 0x000FE39B File Offset: 0x000FC59B
	public float GetSliderMin(int index)
	{
		return (float)this.minSlider;
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x000FE3A4 File Offset: 0x000FC5A4
	public float GetSliderMax(int index)
	{
		return (float)this.maxSlider;
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x000FE3AD File Offset: 0x000FC5AD
	public float GetSliderValue(int index)
	{
		return this.particleThreshold;
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x000FE3B5 File Offset: 0x000FC5B5
	public void SetSliderValue(float value, int index)
	{
		this.particleThreshold = value;
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x000FE3BE File Offset: 0x000FC5BE
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x000FE3C5 File Offset: 0x000FC5C5
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
	}

	// Token: 0x04001A2C RID: 6700
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04001A2D RID: 6701
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001A2E RID: 6702
	private float recentPerSecondConsumptionRate;

	// Token: 0x04001A2F RID: 6703
	public int minSlider;

	// Token: 0x04001A30 RID: 6704
	public int maxSlider;

	// Token: 0x04001A31 RID: 6705
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04001A32 RID: 6706
	public float minLaunchInterval;

	// Token: 0x04001A33 RID: 6707
	public float radiationSampleRate;

	// Token: 0x04001A34 RID: 6708
	[Serialize]
	public float particleThreshold = 50f;

	// Token: 0x04001A35 RID: 6709
	private EightDirectionController directionController;

	// Token: 0x04001A36 RID: 6710
	private float launcherTimer;

	// Token: 0x04001A37 RID: 6711
	private float radiationSampleTimer;

	// Token: 0x04001A38 RID: 6712
	private MeterController particleController;

	// Token: 0x04001A39 RID: 6713
	private bool particleVisualPlaying;

	// Token: 0x04001A3A RID: 6714
	private MeterController progressMeterController;

	// Token: 0x04001A3B RID: 6715
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x04001A3C RID: 6716
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001A3D RID: 6717
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleSpawner>(delegate(HighEnergyParticleSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200151C RID: 5404
	public class StatesInstance : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.GameInstance
	{
		// Token: 0x06008D45 RID: 36165 RVA: 0x0033EB5B File Offset: 0x0033CD5B
		public StatesInstance(HighEnergyParticleSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x0200151D RID: 5405
	public class States : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner>
	{
		// Token: 0x06008D46 RID: 36166 RVA: 0x0033EB64 File Offset: 0x0033CD64
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).DefaultState(this.inoperational.empty);
			this.inoperational.empty.EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.losing, (HighEnergyParticleSpawner.StatesInstance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.inoperational.losing.ToggleStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.DoConsumeParticlesWhileDisabled(dt);
			}, UpdateRate.SIM_1000ms, false).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.empty, (HighEnergyParticleSpawner.StatesInstance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
			this.ready.idle.ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.absorbing, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsTrue).PlayAnim("on");
			this.ready.absorbing.Enter("SetActive(true)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.idle, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.CollectingHEP, (HighEnergyParticleSpawner.StatesInstance smi) => smi.master).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x04006BF7 RID: 27639
		public StateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x04006BF8 RID: 27640
		public HighEnergyParticleSpawner.States.ReadyStates ready;

		// Token: 0x04006BF9 RID: 27641
		public HighEnergyParticleSpawner.States.InoperationalStates inoperational;

		// Token: 0x020024F1 RID: 9457
		public class InoperationalStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x0400A44B RID: 42059
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State empty;

			// Token: 0x0400A44C RID: 42060
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State losing;
		}

		// Token: 0x020024F2 RID: 9458
		public class ReadyStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x0400A44D RID: 42061
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State idle;

			// Token: 0x0400A44E RID: 42062
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State absorbing;
		}
	}
}

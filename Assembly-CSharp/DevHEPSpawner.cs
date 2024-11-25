using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006B8 RID: 1720
[SerializationConfig(MemberSerialization.OptIn)]
public class DevHEPSpawner : StateMachineComponent<DevHEPSpawner.StatesInstance>, IHighEnergyParticleDirection, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06002B52 RID: 11090 RVA: 0x000F38FC File Offset: 0x000F1AFC
	// (set) Token: 0x06002B53 RID: 11091 RVA: 0x000F3904 File Offset: 0x000F1B04
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

	// Token: 0x06002B54 RID: 11092 RVA: 0x000F395C File Offset: 0x000F1B5C
	private void OnCopySettings(object data)
	{
		DevHEPSpawner component = ((GameObject)data).GetComponent<DevHEPSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.boltAmount = component.boltAmount;
		}
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x000F3996 File Offset: 0x000F1B96
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DevHEPSpawner>(-905833192, DevHEPSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x000F39B0 File Offset: 0x000F1BB0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x000F3A50 File Offset: 0x000F1C50
	public void LauncherUpdate(float dt)
	{
		if (this.boltAmount <= 0f)
		{
			return;
		}
		this.launcherTimer += dt;
		this.progressMeterController.SetPositionPercent(this.launcherTimer / 5f);
		if (this.launcherTimer > 5f)
		{
			this.launcherTimer -= 5f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.boltAmount;
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06002B58 RID: 11096 RVA: 0x000F3B8F File Offset: 0x000F1D8F
	public string SliderTitleKey
	{
		get
		{
			return "";
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06002B59 RID: 11097 RVA: 0x000F3B96 File Offset: 0x000F1D96
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x000F3BA2 File Offset: 0x000F1DA2
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x000F3BA5 File Offset: 0x000F1DA5
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x000F3BAC File Offset: 0x000F1DAC
	public float GetSliderMax(int index)
	{
		return 500f;
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x000F3BB3 File Offset: 0x000F1DB3
	public float GetSliderValue(int index)
	{
		return this.boltAmount;
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x000F3BBB File Offset: 0x000F1DBB
	public void SetSliderValue(float value, int index)
	{
		this.boltAmount = value;
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x000F3BC4 File Offset: 0x000F1DC4
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x000F3BCB File Offset: 0x000F1DCB
	string ISliderControl.GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x040018DA RID: 6362
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040018DB RID: 6363
	[Serialize]
	private EightDirection _direction;

	// Token: 0x040018DC RID: 6364
	public float boltAmount;

	// Token: 0x040018DD RID: 6365
	private EightDirectionController directionController;

	// Token: 0x040018DE RID: 6366
	private float launcherTimer;

	// Token: 0x040018DF RID: 6367
	private MeterController particleController;

	// Token: 0x040018E0 RID: 6368
	private MeterController progressMeterController;

	// Token: 0x040018E1 RID: 6369
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x040018E2 RID: 6370
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040018E3 RID: 6371
	private static readonly EventSystem.IntraObjectHandler<DevHEPSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DevHEPSpawner>(delegate(DevHEPSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020014B3 RID: 5299
	public class StatesInstance : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.GameInstance
	{
		// Token: 0x06008BCD RID: 35789 RVA: 0x00338249 File Offset: 0x00336449
		public StatesInstance(DevHEPSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x020014B4 RID: 5300
	public class States : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner>
	{
		// Token: 0x06008BCE RID: 35790 RVA: 0x00338254 File Offset: 0x00336454
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).Update(delegate(DevHEPSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
		}

		// Token: 0x04006AB2 RID: 27314
		public StateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x04006AB3 RID: 27315
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State ready;

		// Token: 0x04006AB4 RID: 27316
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State inoperational;
	}
}

using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008E5 RID: 2277
public class HEPBattery : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>
{
	// Token: 0x0600413F RID: 16703 RVA: 0x00172D40 File Offset: 0x00170F40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).Update(delegate(HEPBattery.Instance smi, float dt)
		{
			smi.DoConsumeParticlesWhileDisabled(dt);
			smi.UpdateDecayStatusItem(false);
		}, UpdateRate.SIM_200ms, false);
		this.operational.Enter("SetActive(true)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("on", KAnim.PlayMode.Loop).TagTransition(GameTags.Operational, this.inoperational, true).Update(new Action<HEPBattery.Instance, float>(this.LauncherUpdate), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06004140 RID: 16704 RVA: 0x00172E28 File Offset: 0x00171028
	public void LauncherUpdate(HEPBattery.Instance smi, float dt)
	{
		smi.UpdateDecayStatusItem(true);
		smi.UpdateMeter(null);
		smi.operational.SetActive(smi.particleStorage.Particles > 0f, false);
		smi.launcherTimer += dt;
		if (smi.launcherTimer < smi.def.minLaunchInterval || !smi.AllowSpawnParticles)
		{
			return;
		}
		if (smi.particleStorage.Particles >= smi.particleThreshold)
		{
			smi.launcherTimer = 0f;
			this.Fire(smi);
		}
	}

	// Token: 0x06004141 RID: 16705 RVA: 0x00172EB0 File Offset: 0x001710B0
	public void Fire(HEPBattery.Instance smi)
	{
		int highEnergyParticleOutputCell = smi.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
		gameObject.SetActive(true);
		if (gameObject != null)
		{
			HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
			component.payload = smi.particleStorage.ConsumeAndGet(smi.particleThreshold);
			component.SetDirection(smi.def.direction);
		}
	}

	// Token: 0x04002B46 RID: 11078
	public static readonly HashedString FIRE_PORT_ID = "HEPBatteryFire";

	// Token: 0x04002B47 RID: 11079
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State inoperational;

	// Token: 0x04002B48 RID: 11080
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State operational;

	// Token: 0x0200184E RID: 6222
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400757A RID: 30074
		public float particleDecayRate;

		// Token: 0x0400757B RID: 30075
		public float minLaunchInterval;

		// Token: 0x0400757C RID: 30076
		public float minSlider;

		// Token: 0x0400757D RID: 30077
		public float maxSlider;

		// Token: 0x0400757E RID: 30078
		public EightDirection direction;
	}

	// Token: 0x0200184F RID: 6223
	public new class Instance : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.GameInstance, ISingleSliderControl, ISliderControl
	{
		// Token: 0x060097D2 RID: 38866 RVA: 0x00366240 File Offset: 0x00364440
		public Instance(IStateMachineTarget master, HEPBattery.Def def) : base(master, def)
		{
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			this.UpdateMeter(null);
		}

		// Token: 0x060097D3 RID: 38867 RVA: 0x003662CA File Offset: 0x003644CA
		public void DoConsumeParticlesWhileDisabled(float dt)
		{
			if (this.m_skipFirstUpdate)
			{
				this.m_skipFirstUpdate = false;
				return;
			}
			this.particleStorage.ConsumeAndGet(dt * base.def.particleDecayRate);
			this.UpdateMeter(null);
		}

		// Token: 0x060097D4 RID: 38868 RVA: 0x003662FC File Offset: 0x003644FC
		public void UpdateMeter(object data = null)
		{
			this.meterController.SetPositionPercent(this.particleStorage.Particles / this.particleStorage.Capacity());
		}

		// Token: 0x060097D5 RID: 38869 RVA: 0x00366320 File Offset: 0x00364520
		public void UpdateDecayStatusItem(bool hasPower)
		{
			if (!hasPower)
			{
				if (this.particleStorage.Particles > 0f)
				{
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null);
						return;
					}
				}
				else if (this.statusHandle != Guid.Empty)
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
					this.statusHandle = Guid.Empty;
					return;
				}
			}
			else if (this.statusHandle != Guid.Empty)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x060097D6 RID: 38870 RVA: 0x003663DA File Offset: 0x003645DA
		public bool AllowSpawnParticles
		{
			get
			{
				return this.hasLogicWire && this.isLogicActive;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x060097D7 RID: 38871 RVA: 0x003663EC File Offset: 0x003645EC
		public bool HasLogicWire
		{
			get
			{
				return this.hasLogicWire;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x060097D8 RID: 38872 RVA: 0x003663F4 File Offset: 0x003645F4
		public bool IsLogicActive
		{
			get
			{
				return this.isLogicActive;
			}
		}

		// Token: 0x060097D9 RID: 38873 RVA: 0x003663FC File Offset: 0x003645FC
		private LogicCircuitNetwork GetNetwork()
		{
			int portCell = base.GetComponent<LogicPorts>().GetPortCell(HEPBattery.FIRE_PORT_ID);
			return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}

		// Token: 0x060097DA RID: 38874 RVA: 0x0036642C File Offset: 0x0036462C
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == HEPBattery.FIRE_PORT_ID)
			{
				this.isLogicActive = (logicValueChanged.newValue > 0);
				this.hasLogicWire = (this.GetNetwork() != null);
			}
		}

		// Token: 0x060097DB RID: 38875 RVA: 0x00366470 File Offset: 0x00364670
		private void OnCopySettings(object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject != null)
			{
				HEPBattery.Instance smi = gameObject.GetSMI<HEPBattery.Instance>();
				if (smi != null)
				{
					this.particleThreshold = smi.particleThreshold;
				}
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x060097DC RID: 38876 RVA: 0x0036649D File Offset: 0x0036469D
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x060097DD RID: 38877 RVA: 0x003664A4 File Offset: 0x003646A4
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
			}
		}

		// Token: 0x060097DE RID: 38878 RVA: 0x003664B0 File Offset: 0x003646B0
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x060097DF RID: 38879 RVA: 0x003664B3 File Offset: 0x003646B3
		public float GetSliderMin(int index)
		{
			return base.def.minSlider;
		}

		// Token: 0x060097E0 RID: 38880 RVA: 0x003664C0 File Offset: 0x003646C0
		public float GetSliderMax(int index)
		{
			return base.def.maxSlider;
		}

		// Token: 0x060097E1 RID: 38881 RVA: 0x003664CD File Offset: 0x003646CD
		public float GetSliderValue(int index)
		{
			return this.particleThreshold;
		}

		// Token: 0x060097E2 RID: 38882 RVA: 0x003664D5 File Offset: 0x003646D5
		public void SetSliderValue(float value, int index)
		{
			this.particleThreshold = value;
		}

		// Token: 0x060097E3 RID: 38883 RVA: 0x003664DE File Offset: 0x003646DE
		public string GetSliderTooltipKey(int index)
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
		}

		// Token: 0x060097E4 RID: 38884 RVA: 0x003664E5 File Offset: 0x003646E5
		string ISliderControl.GetSliderTooltip(int index)
		{
			return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
		}

		// Token: 0x0400757F RID: 30079
		[MyCmpReq]
		public HighEnergyParticleStorage particleStorage;

		// Token: 0x04007580 RID: 30080
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04007581 RID: 30081
		[MyCmpAdd]
		public CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04007582 RID: 30082
		[Serialize]
		public float launcherTimer;

		// Token: 0x04007583 RID: 30083
		[Serialize]
		public float particleThreshold = 50f;

		// Token: 0x04007584 RID: 30084
		public bool ShowWorkingStatus;

		// Token: 0x04007585 RID: 30085
		private bool m_skipFirstUpdate = true;

		// Token: 0x04007586 RID: 30086
		private MeterController meterController;

		// Token: 0x04007587 RID: 30087
		private Guid statusHandle = Guid.Empty;

		// Token: 0x04007588 RID: 30088
		private bool hasLogicWire;

		// Token: 0x04007589 RID: 30089
		private bool isLogicActive;
	}
}

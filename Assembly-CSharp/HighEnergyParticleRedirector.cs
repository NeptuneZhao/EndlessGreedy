using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006ED RID: 1773
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticleRedirector : StateMachineComponent<HighEnergyParticleRedirector.StatesInstance>, IHighEnergyParticleDirection
{
	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06002D22 RID: 11554 RVA: 0x000FDA58 File Offset: 0x000FBC58
	// (set) Token: 0x06002D23 RID: 11555 RVA: 0x000FDA60 File Offset: 0x000FBC60
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

	// Token: 0x06002D24 RID: 11556 RVA: 0x000FDAB8 File Offset: 0x000FBCB8
	private void OnCopySettings(object data)
	{
		HighEnergyParticleRedirector component = ((GameObject)data).GetComponent<HighEnergyParticleRedirector>();
		if (component != null)
		{
			this.Direction = component.Direction;
		}
	}

	// Token: 0x06002D25 RID: 11557 RVA: 0x000FDAE6 File Offset: 0x000FBCE6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HighEnergyParticleRedirector>(-905833192, HighEnergyParticleRedirector.OnCopySettingsDelegate);
		base.Subscribe<HighEnergyParticleRedirector>(-801688580, HighEnergyParticleRedirector.OnLogicValueChangedDelegate);
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x000FDB10 File Offset: 0x000FBD10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		HighEnergyParticlePort component = base.GetComponent<HighEnergyParticlePort>();
		if (component)
		{
			HighEnergyParticlePort highEnergyParticlePort = component;
			highEnergyParticlePort.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Combine(highEnergyParticlePort.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
		if (HighEnergyParticleRedirector.infoStatusItem_Logic == null)
		{
			HighEnergyParticleRedirector.infoStatusItem_Logic = new StatusItem("HEPRedirectorLogic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			HighEnergyParticleRedirector.infoStatusItem_Logic.resolveStringCallback = new Func<string, object, string>(HighEnergyParticleRedirector.ResolveInfoStatusItem);
			HighEnergyParticleRedirector.infoStatusItem_Logic.resolveTooltipCallback = new Func<string, object, string>(HighEnergyParticleRedirector.ResolveInfoStatusItemTooltip);
		}
		this.selectable.AddStatusItem(HighEnergyParticleRedirector.infoStatusItem_Logic, this);
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirector", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x000FDC02 File Offset: 0x000FBE02
	private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
	{
		return this.AllowIncomingParticles;
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x000FDC0C File Offset: 0x000FBE0C
	private void LaunchParticle()
	{
		if (base.smi.master.storage.Particles < 0.1f)
		{
			base.smi.master.storage.ConsumeAll();
			return;
		}
		int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
		gameObject.SetActive(true);
		if (gameObject != null)
		{
			HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
			component.payload = base.smi.master.storage.ConsumeAll();
			component.payload -= 0.1f;
			component.capturedBy = this.port;
			component.SetDirection(this.Direction);
			this.directionController.PlayAnim("redirector_send", KAnim.PlayMode.Once);
			this.directionController.controller.Queue("redirector", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x000FDD0C File Offset: 0x000FBF0C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		HighEnergyParticlePort component = base.GetComponent<HighEnergyParticlePort>();
		if (component != null)
		{
			HighEnergyParticlePort highEnergyParticlePort = component;
			highEnergyParticlePort.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Remove(highEnergyParticlePort.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06002D2A RID: 11562 RVA: 0x000FDD51 File Offset: 0x000FBF51
	public bool AllowIncomingParticles
	{
		get
		{
			return !this.hasLogicWire || (this.hasLogicWire && this.isLogicActive);
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06002D2B RID: 11563 RVA: 0x000FDD6D File Offset: 0x000FBF6D
	public bool HasLogicWire
	{
		get
		{
			return this.hasLogicWire;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06002D2C RID: 11564 RVA: 0x000FDD75 File Offset: 0x000FBF75
	public bool IsLogicActive
	{
		get
		{
			return this.isLogicActive;
		}
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x000FDD80 File Offset: 0x000FBF80
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(HighEnergyParticleRedirector.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x000FDDB0 File Offset: 0x000FBFB0
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == HighEnergyParticleRedirector.PORT_ID)
		{
			this.isLogicActive = (logicValueChanged.newValue > 0);
			this.hasLogicWire = (this.GetNetwork() != null);
		}
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x000FDDF4 File Offset: 0x000FBFF4
	private static string ResolveInfoStatusItem(string format_str, object data)
	{
		HighEnergyParticleRedirector highEnergyParticleRedirector = (HighEnergyParticleRedirector)data;
		if (!highEnergyParticleRedirector.HasLogicWire)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.NORMAL;
		}
		if (highEnergyParticleRedirector.IsLogicActive)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_CONTROLLED_ACTIVE;
		}
		return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_CONTROLLED_STANDBY;
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x000FDE38 File Offset: 0x000FC038
	private static string ResolveInfoStatusItemTooltip(string format_str, object data)
	{
		HighEnergyParticleRedirector highEnergyParticleRedirector = (HighEnergyParticleRedirector)data;
		if (!highEnergyParticleRedirector.HasLogicWire)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.NORMAL;
		}
		if (highEnergyParticleRedirector.IsLogicActive)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.LOGIC_CONTROLLED_ACTIVE;
		}
		return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.LOGIC_CONTROLLED_STANDBY;
	}

	// Token: 0x04001A1E RID: 6686
	public static readonly HashedString PORT_ID = "HEPRedirector";

	// Token: 0x04001A1F RID: 6687
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001A20 RID: 6688
	[MyCmpReq]
	private HighEnergyParticleStorage storage;

	// Token: 0x04001A21 RID: 6689
	[MyCmpGet]
	private HighEnergyParticlePort port;

	// Token: 0x04001A22 RID: 6690
	public float directorDelay;

	// Token: 0x04001A23 RID: 6691
	public bool directionControllable = true;

	// Token: 0x04001A24 RID: 6692
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04001A25 RID: 6693
	private EightDirectionController directionController;

	// Token: 0x04001A26 RID: 6694
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001A27 RID: 6695
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleRedirector> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleRedirector>(delegate(HighEnergyParticleRedirector component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001A28 RID: 6696
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleRedirector> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleRedirector>(delegate(HighEnergyParticleRedirector component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001A29 RID: 6697
	private bool hasLogicWire;

	// Token: 0x04001A2A RID: 6698
	private bool isLogicActive;

	// Token: 0x04001A2B RID: 6699
	private static StatusItem infoStatusItem_Logic;

	// Token: 0x02001519 RID: 5401
	public class StatesInstance : GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.GameInstance
	{
		// Token: 0x06008D3E RID: 36158 RVA: 0x0033EA40 File Offset: 0x0033CC40
		public StatesInstance(HighEnergyParticleRedirector smi) : base(smi)
		{
		}
	}

	// Token: 0x0200151A RID: 5402
	public class States : GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector>
	{
		// Token: 0x06008D3F RID: 36159 RVA: 0x0033EA4C File Offset: 0x0033CC4C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).EventTransition(GameHashes.OnParticleStorageChanged, this.redirect, null);
			this.redirect.PlayAnim("working_pre").QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).ScheduleGoTo((HighEnergyParticleRedirector.StatesInstance smi) => smi.master.directorDelay, this.ready).Exit(delegate(HighEnergyParticleRedirector.StatesInstance smi)
			{
				smi.master.LaunchParticle();
			});
		}

		// Token: 0x04006BF2 RID: 27634
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State inoperational;

		// Token: 0x04006BF3 RID: 27635
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State ready;

		// Token: 0x04006BF4 RID: 27636
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State redirect;

		// Token: 0x04006BF5 RID: 27637
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State launch;
	}
}

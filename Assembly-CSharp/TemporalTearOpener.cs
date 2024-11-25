using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000785 RID: 1925
public class TemporalTearOpener : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>
{
	// Token: 0x06003471 RID: 13425 RVA: 0x0011DCB4 File Offset: 0x0011BEB4
	private static StatusItem CreateColoniesStatusItem()
	{
		StatusItem statusItem = new StatusItem("Temporal_Tear_Opener_Insufficient_Colonies", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
		statusItem.resolveStringCallback = delegate(string str, object data)
		{
			TemporalTearOpener.Instance instance = (TemporalTearOpener.Instance)data;
			str = str.Replace("{progress}", string.Format("({0}/{1})", instance.CountColonies(), EstablishColonies.BASE_COUNT));
			return str;
		};
		return statusItem;
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x0011DD0C File Offset: 0x0011BF0C
	private static StatusItem CreateProgressStatusItem()
	{
		StatusItem statusItem = new StatusItem("Temporal_Tear_Opener_Progress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
		statusItem.resolveStringCallback = delegate(string str, object data)
		{
			TemporalTearOpener.Instance instance = (TemporalTearOpener.Instance)data;
			str = str.Replace("{progress}", GameUtil.GetFormattedPercent(instance.GetPercentComplete(), GameUtil.TimeSlice.None));
			return str;
		};
		return statusItem;
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x0011DD64 File Offset: 0x0011BF64
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.UpdateMeter();
			if (ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearOpen())
			{
				smi.GoTo(this.opening_tear_finish);
				return;
			}
			smi.GoTo(this.check_requirements);
		}).PlayAnim("off");
		this.check_requirements.DefaultState(this.check_requirements.has_target).Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.GetComponent<HighEnergyParticleStorage>().receiverOpen = false;
			smi.GetComponent<KBatchedAnimController>().Play("port_close", KAnim.PlayMode.Once, 1f, 0f);
			smi.GetComponent<KBatchedAnimController>().Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		});
		this.check_requirements.has_target.ToggleStatusItem(TemporalTearOpener.s_noTargetStatus, null).UpdateTransition(this.check_requirements.has_los, (TemporalTearOpener.Instance smi, float dt) => ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearRevealed(), UpdateRate.SIM_200ms, false);
		this.check_requirements.has_los.ToggleStatusItem(TemporalTearOpener.s_noLosStatus, null).UpdateTransition(this.check_requirements.enough_colonies, (TemporalTearOpener.Instance smi, float dt) => smi.HasLineOfSight(), UpdateRate.SIM_200ms, false);
		this.check_requirements.enough_colonies.ToggleStatusItem(TemporalTearOpener.s_insufficient_colonies, null).UpdateTransition(this.charging, (TemporalTearOpener.Instance smi, float dt) => smi.HasSufficientColonies(), UpdateRate.SIM_200ms, false);
		this.charging.DefaultState(this.charging.idle).ToggleStatusItem(TemporalTearOpener.s_progressStatus, (TemporalTearOpener.Instance smi) => smi).UpdateTransition(this.check_requirements.has_los, (TemporalTearOpener.Instance smi, float dt) => !smi.HasLineOfSight(), UpdateRate.SIM_200ms, false).UpdateTransition(this.check_requirements.enough_colonies, (TemporalTearOpener.Instance smi, float dt) => !smi.HasSufficientColonies(), UpdateRate.SIM_200ms, false).Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.GetComponent<HighEnergyParticleStorage>().receiverOpen = true;
			smi.GetComponent<KBatchedAnimController>().Play("port_open", KAnim.PlayMode.Once, 1f, 0f);
			smi.GetComponent<KBatchedAnimController>().Queue("inert", KAnim.PlayMode.Loop, 1f, 0f);
		});
		this.charging.idle.EventTransition(GameHashes.OnParticleStorageChanged, this.charging.consuming, (TemporalTearOpener.Instance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
		this.charging.consuming.EventTransition(GameHashes.OnParticleStorageChanged, this.charging.idle, (TemporalTearOpener.Instance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty()).UpdateTransition(this.ready, (TemporalTearOpener.Instance smi, float dt) => smi.ConsumeParticlesAndCheckComplete(dt), UpdateRate.SIM_200ms, false);
		this.ready.ToggleNotification((TemporalTearOpener.Instance smi) => new Notification(BUILDING.STATUSITEMS.TEMPORAL_TEAR_OPENER_READY.NOTIFICATION, NotificationType.Good, (List<Notification> a, object b) => BUILDING.STATUSITEMS.TEMPORAL_TEAR_OPENER_READY.NOTIFICATION_TOOLTIP, null, false, 0f, null, null, null, true, false, false));
		this.opening_tear_beam_pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.opening_tear_beam);
		this.opening_tear_beam.Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.CreateBeamFX();
		}).PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.opening_tear_finish);
		this.opening_tear_finish.PlayAnim("working_pst").Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.OpenTemporalTear();
		});
	}

	// Token: 0x04001EF2 RID: 7922
	private const float MIN_SUNLIGHT_EXPOSURE = 15f;

	// Token: 0x04001EF3 RID: 7923
	private static StatusItem s_noLosStatus = new StatusItem("Temporal_Tear_Opener_No_Los", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04001EF4 RID: 7924
	private static StatusItem s_insufficient_colonies = TemporalTearOpener.CreateColoniesStatusItem();

	// Token: 0x04001EF5 RID: 7925
	private static StatusItem s_noTargetStatus = new StatusItem("Temporal_Tear_Opener_No_Target", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04001EF6 RID: 7926
	private static StatusItem s_progressStatus = TemporalTearOpener.CreateProgressStatusItem();

	// Token: 0x04001EF7 RID: 7927
	private TemporalTearOpener.CheckRequirementsState check_requirements;

	// Token: 0x04001EF8 RID: 7928
	private TemporalTearOpener.ChargingState charging;

	// Token: 0x04001EF9 RID: 7929
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_beam_pre;

	// Token: 0x04001EFA RID: 7930
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_beam;

	// Token: 0x04001EFB RID: 7931
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_finish;

	// Token: 0x04001EFC RID: 7932
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State ready;

	// Token: 0x0200162B RID: 5675
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006ED5 RID: 28373
		public float consumeRate;

		// Token: 0x04006ED6 RID: 28374
		public float numParticlesToOpen;
	}

	// Token: 0x0200162C RID: 5676
	private class ChargingState : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State
	{
		// Token: 0x04006ED7 RID: 28375
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State idle;

		// Token: 0x04006ED8 RID: 28376
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State consuming;
	}

	// Token: 0x0200162D RID: 5677
	private class CheckRequirementsState : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State
	{
		// Token: 0x04006ED9 RID: 28377
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State has_target;

		// Token: 0x04006EDA RID: 28378
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State has_los;

		// Token: 0x04006EDB RID: 28379
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State enough_colonies;
	}

	// Token: 0x0200162E RID: 5678
	public new class Instance : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x0600912F RID: 37167 RVA: 0x0034F71B File Offset: 0x0034D91B
		public Instance(IStateMachineTarget master, TemporalTearOpener.Def def) : base(master, def)
		{
			this.m_meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			EnterTemporalTearSequence.tearOpenerGameObject = base.gameObject;
		}

		// Token: 0x06009130 RID: 37168 RVA: 0x0034F758 File Offset: 0x0034D958
		protected override void OnCleanUp()
		{
			if (EnterTemporalTearSequence.tearOpenerGameObject == base.gameObject)
			{
				EnterTemporalTearSequence.tearOpenerGameObject = null;
			}
			base.OnCleanUp();
		}

		// Token: 0x06009131 RID: 37169 RVA: 0x0034F778 File Offset: 0x0034D978
		public bool HasLineOfSight()
		{
			Extents extents = base.GetComponent<Building>().GetExtents();
			int x = extents.x;
			int num = extents.x + extents.width - 1;
			for (int i = x; i <= num; i++)
			{
				int i2 = Grid.XYToCell(i, extents.y);
				if ((float)Grid.ExposedToSunlight[i2] < 15f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009132 RID: 37170 RVA: 0x0034F7D5 File Offset: 0x0034D9D5
		public bool HasSufficientColonies()
		{
			return this.CountColonies() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x06009133 RID: 37171 RVA: 0x0034F7E8 File Offset: 0x0034D9E8
		public int CountColonies()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06009134 RID: 37172 RVA: 0x0034F834 File Offset: 0x0034DA34
		public bool ConsumeParticlesAndCheckComplete(float dt)
		{
			float amount = Mathf.Min(dt * base.def.consumeRate, base.def.numParticlesToOpen - this.m_particlesConsumed);
			float num = base.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet(amount);
			this.m_particlesConsumed += num;
			this.UpdateMeter();
			return this.m_particlesConsumed >= base.def.numParticlesToOpen;
		}

		// Token: 0x06009135 RID: 37173 RVA: 0x0034F89D File Offset: 0x0034DA9D
		public void UpdateMeter()
		{
			this.m_meter.SetPositionPercent(this.GetAmountComplete());
		}

		// Token: 0x06009136 RID: 37174 RVA: 0x0034F8B0 File Offset: 0x0034DAB0
		private float GetAmountComplete()
		{
			return Mathf.Min(this.m_particlesConsumed / base.def.numParticlesToOpen, 1f);
		}

		// Token: 0x06009137 RID: 37175 RVA: 0x0034F8CE File Offset: 0x0034DACE
		public float GetPercentComplete()
		{
			return this.GetAmountComplete() * 100f;
		}

		// Token: 0x06009138 RID: 37176 RVA: 0x0034F8DC File Offset: 0x0034DADC
		public void CreateBeamFX()
		{
			Vector3 position = base.gameObject.transform.position;
			position.y += 3.25f;
			Quaternion rotation = Quaternion.Euler(-90f, 90f, 0f);
			Util.KInstantiate(EffectPrefabs.Instance.OpenTemporalTearBeam, position, rotation, base.gameObject, null, true, 0);
		}

		// Token: 0x06009139 RID: 37177 RVA: 0x0034F93A File Offset: 0x0034DB3A
		public void OpenTemporalTear()
		{
			ClusterManager.Instance.GetClusterPOIManager().RevealTemporalTear();
			ClusterManager.Instance.GetClusterPOIManager().OpenTemporalTear(this.GetMyWorldId());
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x0600913A RID: 37178 RVA: 0x0034F960 File Offset: 0x0034DB60
		public string SidescreenButtonText
		{
			get
			{
				return BUILDINGS.PREFABS.TEMPORALTEAROPENER.SIDESCREEN.TEXT;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x0600913B RID: 37179 RVA: 0x0034F96C File Offset: 0x0034DB6C
		public string SidescreenButtonTooltip
		{
			get
			{
				return BUILDINGS.PREFABS.TEMPORALTEAROPENER.SIDESCREEN.TOOLTIP;
			}
		}

		// Token: 0x0600913C RID: 37180 RVA: 0x0034F978 File Offset: 0x0034DB78
		public bool SidescreenEnabled()
		{
			return this.GetCurrentState() == base.sm.ready || DebugHandler.InstantBuildMode;
		}

		// Token: 0x0600913D RID: 37181 RVA: 0x0034F994 File Offset: 0x0034DB94
		public bool SidescreenButtonInteractable()
		{
			return this.GetCurrentState() == base.sm.ready || DebugHandler.InstantBuildMode;
		}

		// Token: 0x0600913E RID: 37182 RVA: 0x0034F9B0 File Offset: 0x0034DBB0
		public void OnSidescreenButtonPressed()
		{
			ConfirmDialogScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<ConfirmDialogScreen>();
			string text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_MESSAGE;
			System.Action on_confirm = delegate()
			{
				this.FireTemporalTearOpener(base.smi);
			};
			System.Action on_cancel = delegate()
			{
			};
			string configurable_text = null;
			System.Action on_configurable_clicked = null;
			string confirm_text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_CONFIRM;
			string cancel_text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_CANCEL;
			component.PopupConfirmDialog(text, on_confirm, on_cancel, configurable_text, on_configurable_clicked, UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_TITLE, confirm_text, cancel_text, null);
		}

		// Token: 0x0600913F RID: 37183 RVA: 0x0034FA3C File Offset: 0x0034DC3C
		private void FireTemporalTearOpener(TemporalTearOpener.Instance smi)
		{
			smi.GoTo(base.sm.opening_tear_beam_pre);
		}

		// Token: 0x06009140 RID: 37184 RVA: 0x0034FA4F File Offset: 0x0034DC4F
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06009141 RID: 37185 RVA: 0x0034FA53 File Offset: 0x0034DC53
		public void SetButtonTextOverride(ButtonMenuTextOverride text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009142 RID: 37186 RVA: 0x0034FA5A File Offset: 0x0034DC5A
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x04006EDC RID: 28380
		[Serialize]
		private float m_particlesConsumed;

		// Token: 0x04006EDD RID: 28381
		private MeterController m_meter;
	}
}

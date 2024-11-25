using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000691 RID: 1681
public class Checkpoint : StateMachineComponent<Checkpoint.SMInstance>
{
	// Token: 0x1700022F RID: 559
	// (get) Token: 0x060029F1 RID: 10737 RVA: 0x000EC736 File Offset: 0x000EA936
	private bool RedLightDesiredState
	{
		get
		{
			return this.hasLogicWire && !this.hasInputHigh && this.operational.IsOperational;
		}
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x000EC758 File Offset: 0x000EA958
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Checkpoint>(-801688580, Checkpoint.OnLogicValueChangedDelegate);
		base.Subscribe<Checkpoint>(-592767678, Checkpoint.OnOperationalChangedDelegate);
		base.smi.StartSM();
		if (Checkpoint.infoStatusItem_Logic == null)
		{
			Checkpoint.infoStatusItem_Logic = new StatusItem("CheckpointLogic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Checkpoint.infoStatusItem_Logic.resolveStringCallback = new Func<string, object, string>(Checkpoint.ResolveInfoStatusItem_Logic);
		}
		this.Refresh(this.redLight);
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x000EC7E9 File Offset: 0x000EA9E9
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearReactable();
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x000EC7F7 File Offset: 0x000EA9F7
	public void RefreshLight()
	{
		if (this.redLight != this.RedLightDesiredState)
		{
			this.Refresh(this.RedLightDesiredState);
			this.statusDirty = true;
		}
		if (this.statusDirty)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x000EC828 File Offset: 0x000EAA28
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(Checkpoint.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x000EC856 File Offset: 0x000EAA56
	private static string ResolveInfoStatusItem_Logic(string format_str, object data)
	{
		return ((Checkpoint)data).RedLight ? BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_CLOSED : BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_OPEN;
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x000EC876 File Offset: 0x000EAA76
	private void CreateNewReactable()
	{
		if (this.reactable == null)
		{
			this.reactable = new Checkpoint.CheckpointReactable(this);
		}
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x000EC88C File Offset: 0x000EAA8C
	private void OrphanReactable()
	{
		this.reactable = null;
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000EC895 File Offset: 0x000EAA95
	private void ClearReactable()
	{
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
			this.reactable = null;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x060029FA RID: 10746 RVA: 0x000EC8B1 File Offset: 0x000EAAB1
	public bool RedLight
	{
		get
		{
			return this.redLight;
		}
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000EC8BC File Offset: 0x000EAABC
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == Checkpoint.PORT_ID)
		{
			this.hasInputHigh = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.hasLogicWire = (this.GetNetwork() != null);
			this.statusDirty = true;
		}
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x000EC90A File Offset: 0x000EAB0A
	private void OnOperationalChanged(object data)
	{
		this.statusDirty = true;
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x000EC914 File Offset: 0x000EAB14
	private void RefreshStatusItem()
	{
		bool on = this.operational.IsOperational && this.hasLogicWire;
		this.selectable.ToggleStatusItem(Checkpoint.infoStatusItem_Logic, on, this);
		this.statusDirty = false;
	}

	// Token: 0x060029FE RID: 10750 RVA: 0x000EC954 File Offset: 0x000EAB54
	private void Refresh(bool redLightState)
	{
		this.redLight = redLightState;
		this.operational.SetActive(this.operational.IsOperational && this.redLight, false);
		base.smi.sm.redLight.Set(this.redLight, base.smi, false);
		if (this.redLight)
		{
			this.CreateNewReactable();
			return;
		}
		this.ClearReactable();
	}

	// Token: 0x04001827 RID: 6183
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04001828 RID: 6184
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001829 RID: 6185
	private static StatusItem infoStatusItem_Logic;

	// Token: 0x0400182A RID: 6186
	private Checkpoint.CheckpointReactable reactable;

	// Token: 0x0400182B RID: 6187
	public static readonly HashedString PORT_ID = "Checkpoint";

	// Token: 0x0400182C RID: 6188
	private bool hasLogicWire;

	// Token: 0x0400182D RID: 6189
	private bool hasInputHigh;

	// Token: 0x0400182E RID: 6190
	private bool redLight;

	// Token: 0x0400182F RID: 6191
	private bool statusDirty = true;

	// Token: 0x04001830 RID: 6192
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001831 RID: 6193
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0200147A RID: 5242
	private class CheckpointReactable : Reactable
	{
		// Token: 0x06008AC3 RID: 35523 RVA: 0x00334A80 File Offset: 0x00332C80
		public CheckpointReactable(Checkpoint checkpoint) : base(checkpoint.gameObject, "CheckpointReactable", Db.Get().ChoreTypes.Checkpoint, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.checkpoint = checkpoint;
			this.rotated = this.gameObject.GetComponent<Rotatable>().IsRotated;
			this.preventChoreInterruption = false;
		}

		// Token: 0x06008AC4 RID: 35524 RVA: 0x00334AF0 File Offset: 0x00332CF0
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.checkpoint == null)
			{
				base.Cleanup();
				return false;
			}
			if (!this.checkpoint.RedLight)
			{
				return false;
			}
			if (this.rotated)
			{
				return transition.x < 0;
			}
			return transition.x > 0;
		}

		// Token: 0x06008AC5 RID: 35525 RVA: 0x00334B50 File Offset: 0x00332D50
		protected override void InternalBegin()
		{
			this.reactor_navigator = this.reactor.GetComponent<Navigator>();
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.checkpoint.OrphanReactable();
			this.checkpoint.CreateNewReactable();
		}

		// Token: 0x06008AC6 RID: 35526 RVA: 0x00334BE0 File Offset: 0x00332DE0
		public override void Update(float dt)
		{
			if (this.checkpoint == null || !this.checkpoint.RedLight || this.reactor_navigator == null)
			{
				base.Cleanup();
				return;
			}
			this.reactor_navigator.AdvancePath(false);
			if (!this.reactor_navigator.path.IsValid())
			{
				base.Cleanup();
				return;
			}
			NavGrid.Transition nextTransition = this.reactor_navigator.GetNextTransition();
			if (!(this.rotated ? (nextTransition.x < 0) : (nextTransition.x > 0)))
			{
				base.Cleanup();
			}
		}

		// Token: 0x06008AC7 RID: 35527 RVA: 0x00334C72 File Offset: 0x00332E72
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x06008AC8 RID: 35528 RVA: 0x00334CA1 File Offset: 0x00332EA1
		protected override void InternalCleanup()
		{
		}

		// Token: 0x040069DE RID: 27102
		private Checkpoint checkpoint;

		// Token: 0x040069DF RID: 27103
		private Navigator reactor_navigator;

		// Token: 0x040069E0 RID: 27104
		private bool rotated;
	}

	// Token: 0x0200147B RID: 5243
	public class SMInstance : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.GameInstance
	{
		// Token: 0x06008AC9 RID: 35529 RVA: 0x00334CA3 File Offset: 0x00332EA3
		public SMInstance(Checkpoint master) : base(master)
		{
		}
	}

	// Token: 0x0200147C RID: 5244
	public class States : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint>
	{
		// Token: 0x06008ACA RID: 35530 RVA: 0x00334CAC File Offset: 0x00332EAC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.go;
			this.root.Update("RefreshLight", delegate(Checkpoint.SMInstance smi, float dt)
			{
				smi.master.RefreshLight();
			}, UpdateRate.SIM_200ms, false);
			this.stop.ParamTransition<bool>(this.redLight, this.go, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsFalse).PlayAnim("red_light");
			this.go.ParamTransition<bool>(this.redLight, this.stop, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsTrue).PlayAnim("green_light");
		}

		// Token: 0x040069E1 RID: 27105
		public StateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.BoolParameter redLight;

		// Token: 0x040069E2 RID: 27106
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State stop;

		// Token: 0x040069E3 RID: 27107
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State go;
	}
}

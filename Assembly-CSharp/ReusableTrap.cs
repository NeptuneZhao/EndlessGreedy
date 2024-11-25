using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A5E RID: 2654
public class ReusableTrap : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>
{
	// Token: 0x06004D0B RID: 19723 RVA: 0x001B96DC File Offset: 0x001B78DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.operational;
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).DefaultState(this.noOperational.idle);
		this.noOperational.idle.EnterTransition(this.noOperational.releasing, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ParamTransition<bool>(this.IsArmed, this.noOperational.disarming, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).PlayAnim("off");
		this.noOperational.releasing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.Release)).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.disarming.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).PlayAnim("abort_armed").OnAnimQueueComplete(this.noOperational.idle);
		this.operational.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.unarmed);
		this.operational.unarmed.ParamTransition<bool>(this.IsArmed, this.operational.armed, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapNeedsArming, null).PlayAnim("unarmed").Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.StartArmTrapWorkChore)).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.CancelArmTrapWorkChore)).WorkableCompleteTransition(new Func<ReusableTrap.Instance, Workable>(ReusableTrap.GetWorkable), this.operational.armed);
		this.operational.armed.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsArmed)).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).PlayAnim("armed", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapArmed, null).Toggle("Enable/Disable Trap Trigger", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.EnableTrapTrigger), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Toggle("Enable/Disable Lure", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.ActivateLure), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableLure)).EventHandlerTransition(GameHashes.TrapTriggered, this.operational.capture.capturing, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.HasCritter_OnTrapTriggered));
		this.operational.capture.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).ToggleTag(GameTags.Trapped).DefaultState(this.operational.capture.capturing).EventHandlerTransition(GameHashes.OnStorageChange, this.operational.capture.release, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.OnStorageEmptied));
		this.operational.capture.capturing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.SetupCapturingAnimations)).Update(new Action<ReusableTrap.Instance, float>(ReusableTrap.OptionalCapturingAnimationUpdate), UpdateRate.RENDER_EVERY_TICK, false).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetCaptureAnimationName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.capture.idle).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.UnsetCapturingAnimations));
		this.operational.capture.idle.TriggerOnEnter(GameHashes.TrapCaptureCompleted, null).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapHasCritter, (ReusableTrap.Instance smi) => smi.CapturedCritter).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetIdleAnimationName), KAnim.PlayMode.Once);
		this.operational.capture.release.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).QueueAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName), false, null).OnAnimQueueComplete(this.operational.unarmed);
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x001B9B53 File Offset: 0x001B7D53
	public static void RefreshLogicOutput(ReusableTrap.Instance smi)
	{
		smi.RefreshLogicOutput();
	}

	// Token: 0x06004D0D RID: 19725 RVA: 0x001B9B5B File Offset: 0x001B7D5B
	public static void Release(ReusableTrap.Instance smi)
	{
		smi.Release();
	}

	// Token: 0x06004D0E RID: 19726 RVA: 0x001B9B63 File Offset: 0x001B7D63
	public static void StartArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06004D0F RID: 19727 RVA: 0x001B9B6B File Offset: 0x001B7D6B
	public static void CancelArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06004D10 RID: 19728 RVA: 0x001B9B73 File Offset: 0x001B7D73
	public static string GetIdleAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture_idle";
		}
		return "capture_idle_large";
	}

	// Token: 0x06004D11 RID: 19729 RVA: 0x001B9B88 File Offset: 0x001B7D88
	public static string GetCaptureAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture";
		}
		return "capture_large";
	}

	// Token: 0x06004D12 RID: 19730 RVA: 0x001B9B9D File Offset: 0x001B7D9D
	public static string GetReleaseAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.WasLastCritterLarge)
		{
			return "release";
		}
		return "release_large";
	}

	// Token: 0x06004D13 RID: 19731 RVA: 0x001B9BB2 File Offset: 0x001B7DB2
	public static bool OnStorageEmptied(ReusableTrap.Instance smi, object obj)
	{
		return !smi.HasCritter;
	}

	// Token: 0x06004D14 RID: 19732 RVA: 0x001B9BBD File Offset: 0x001B7DBD
	public static bool HasCritter_OnTrapTriggered(ReusableTrap.Instance smi, object capturedItem)
	{
		return smi.HasCritter;
	}

	// Token: 0x06004D15 RID: 19733 RVA: 0x001B9BC5 File Offset: 0x001B7DC5
	public static bool StorageContainsCritter(ReusableTrap.Instance smi)
	{
		return smi.HasCritter;
	}

	// Token: 0x06004D16 RID: 19734 RVA: 0x001B9BCD File Offset: 0x001B7DCD
	public static bool StorageIsEmpty(ReusableTrap.Instance smi)
	{
		return !smi.HasCritter;
	}

	// Token: 0x06004D17 RID: 19735 RVA: 0x001B9BD8 File Offset: 0x001B7DD8
	public static void EnableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(true);
	}

	// Token: 0x06004D18 RID: 19736 RVA: 0x001B9BE1 File Offset: 0x001B7DE1
	public static void DisableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(false);
	}

	// Token: 0x06004D19 RID: 19737 RVA: 0x001B9BEA File Offset: 0x001B7DEA
	public static ArmTrapWorkable GetWorkable(ReusableTrap.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x06004D1A RID: 19738 RVA: 0x001B9BF2 File Offset: 0x001B7DF2
	public static void ActivateLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(true);
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x001B9BFB File Offset: 0x001B7DFB
	public static void DisableLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(false);
	}

	// Token: 0x06004D1C RID: 19740 RVA: 0x001B9C04 File Offset: 0x001B7E04
	public static void SetupCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.SetupCapturingAnimations();
	}

	// Token: 0x06004D1D RID: 19741 RVA: 0x001B9C0C File Offset: 0x001B7E0C
	public static void UnsetCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.UnsetCapturingAnimations();
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x001B9C14 File Offset: 0x001B7E14
	public static void OptionalCapturingAnimationUpdate(ReusableTrap.Instance smi, float dt)
	{
		if (smi.def.usingSymbolChaseCapturingAnimations && smi.lastCritterCapturedAnimController != null)
		{
			if (smi.lastCritterCapturedAnimController.currentAnim != smi.CAPTURING_CRITTER_ANIMATION_NAME)
			{
				smi.lastCritterCapturedAnimController.Play(smi.CAPTURING_CRITTER_ANIMATION_NAME, KAnim.PlayMode.Once, 1f, 0f);
			}
			bool flag;
			Vector3 position = smi.animController.GetSymbolTransform(smi.CAPTURING_SYMBOL_NAME, out flag).GetColumn(3);
			smi.lastCritterCapturedAnimController.transform.SetPosition(position);
		}
	}

	// Token: 0x06004D1F RID: 19743 RVA: 0x001B9CB6 File Offset: 0x001B7EB6
	public static void MarkAsArmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(true, smi, false);
		smi.gameObject.AddTag(GameTags.TrapArmed);
	}

	// Token: 0x06004D20 RID: 19744 RVA: 0x001B9CDC File Offset: 0x001B7EDC
	public static void MarkAsUnarmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(false, smi, false);
		smi.gameObject.RemoveTag(GameTags.TrapArmed);
	}

	// Token: 0x0400332C RID: 13100
	public const string CAPTURE_ANIMATION_NAME = "capture";

	// Token: 0x0400332D RID: 13101
	public const string CAPTURE_LARGE_ANIMATION_NAME = "capture_large";

	// Token: 0x0400332E RID: 13102
	public const string CAPTURE_IDLE_ANIMATION_NAME = "capture_idle";

	// Token: 0x0400332F RID: 13103
	public const string CAPTURE_IDLE_LARGE_ANIMATION_NAME = "capture_idle_large";

	// Token: 0x04003330 RID: 13104
	public const string CAPTURE_RELEASE_ANIMATION_NAME = "release";

	// Token: 0x04003331 RID: 13105
	public const string CAPTURE_RELEASE_LARGE_ANIMATION_NAME = "release_large";

	// Token: 0x04003332 RID: 13106
	public const string UNARMED_ANIMATION_NAME = "unarmed";

	// Token: 0x04003333 RID: 13107
	public const string ARMED_ANIMATION_NAME = "armed";

	// Token: 0x04003334 RID: 13108
	public const string ABORT_ARMED_ANIMATION = "abort_armed";

	// Token: 0x04003335 RID: 13109
	public StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.BoolParameter IsArmed;

	// Token: 0x04003336 RID: 13110
	public ReusableTrap.NonOperationalStates noOperational;

	// Token: 0x04003337 RID: 13111
	public ReusableTrap.OperationalStates operational;

	// Token: 0x02001A60 RID: 6752
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06009FC5 RID: 40901 RVA: 0x0037DD35 File Offset: 0x0037BF35
		public bool usingLure
		{
			get
			{
				return this.lures != null && this.lures.Length != 0;
			}
		}

		// Token: 0x04007C45 RID: 31813
		public string OUTPUT_LOGIC_PORT_ID;

		// Token: 0x04007C46 RID: 31814
		public Tag[] lures;

		// Token: 0x04007C47 RID: 31815
		public CellOffset releaseCellOffset = CellOffset.none;

		// Token: 0x04007C48 RID: 31816
		public bool usingSymbolChaseCapturingAnimations;

		// Token: 0x04007C49 RID: 31817
		public Func<string> getTrappedAnimationNameCallback;
	}

	// Token: 0x02001A61 RID: 6753
	public class CaptureStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04007C4A RID: 31818
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State capturing;

		// Token: 0x04007C4B RID: 31819
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x04007C4C RID: 31820
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State release;
	}

	// Token: 0x02001A62 RID: 6754
	public class OperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04007C4D RID: 31821
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State unarmed;

		// Token: 0x04007C4E RID: 31822
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State armed;

		// Token: 0x04007C4F RID: 31823
		public ReusableTrap.CaptureStates capture;
	}

	// Token: 0x02001A63 RID: 6755
	public class NonOperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04007C50 RID: 31824
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x04007C51 RID: 31825
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State releasing;

		// Token: 0x04007C52 RID: 31826
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State disarming;
	}

	// Token: 0x02001A64 RID: 6756
	public new class Instance : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.GameInstance, TrappedStates.ITrapStateAnimationInstructions
	{
		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06009FCA RID: 40906 RVA: 0x0037DD76 File Offset: 0x0037BF76
		public bool IsCapturingLargeCritter
		{
			get
			{
				return this.HasCritter && this.CapturedCritter.HasTag(GameTags.LargeCreature);
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06009FCB RID: 40907 RVA: 0x0037DD92 File Offset: 0x0037BF92
		public bool HasCritter
		{
			get
			{
				return !this.storage.IsEmpty();
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06009FCC RID: 40908 RVA: 0x0037DDA2 File Offset: 0x0037BFA2
		public GameObject CapturedCritter
		{
			get
			{
				if (!this.HasCritter)
				{
					return null;
				}
				return this.storage.items[0];
			}
		}

		// Token: 0x06009FCD RID: 40909 RVA: 0x0037DDBF File Offset: 0x0037BFBF
		public ArmTrapWorkable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06009FCE RID: 40910 RVA: 0x0037DDC8 File Offset: 0x0037BFC8
		public void RefreshLogicOutput()
		{
			bool flag = base.IsInsideState(base.sm.operational) && this.HasCritter;
			this.logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag ? 1 : 0);
		}

		// Token: 0x06009FCF RID: 40911 RVA: 0x0037DE14 File Offset: 0x0037C014
		public Instance(IStateMachineTarget master, ReusableTrap.Def def) : base(master, def)
		{
		}

		// Token: 0x06009FD0 RID: 40912 RVA: 0x0037DE34 File Offset: 0x0037C034
		public override void StartSM()
		{
			base.StartSM();
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
			}
			ArmTrapWorkable armTrapWorkable = this.workable;
			armTrapWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(armTrapWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		}

		// Token: 0x06009FD1 RID: 40913 RVA: 0x0037DE84 File Offset: 0x0037C084
		private void OnWorkEvent(Workable workable, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped && workable.GetPercentComplete() < 1f && workable.GetPercentComplete() != 0f && base.IsInsideState(base.sm.operational.unarmed))
			{
				this.animController.Play("unarmed", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06009FD2 RID: 40914 RVA: 0x0037DEE7 File Offset: 0x0037C0E7
		public void SetTrapTriggerActiveState(bool active)
		{
			this.trapTrigger.enabled = active;
		}

		// Token: 0x06009FD3 RID: 40915 RVA: 0x0037DEF8 File Offset: 0x0037C0F8
		public void SetLureActiveState(bool activate)
		{
			if (base.def.usingLure)
			{
				Lure.Instance smi = base.gameObject.GetSMI<Lure.Instance>();
				if (smi != null)
				{
					smi.SetActiveLures(activate ? base.def.lures : null);
				}
			}
		}

		// Token: 0x06009FD4 RID: 40916 RVA: 0x0037DF38 File Offset: 0x0037C138
		public void SetupCapturingAnimations()
		{
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
				this.lastCritterCapturedAnimController = this.CapturedCritter.GetComponent<KBatchedAnimController>();
			}
		}

		// Token: 0x06009FD5 RID: 40917 RVA: 0x0037DF60 File Offset: 0x0037C160
		public void UnsetCapturingAnimations()
		{
			this.trapTrigger.SetStoredPosition(this.CapturedCritter);
			if (base.def.usingSymbolChaseCapturingAnimations && this.lastCritterCapturedAnimController != null)
			{
				this.lastCritterCapturedAnimController.Play("trapped", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.lastCritterCapturedAnimController = null;
		}

		// Token: 0x06009FD6 RID: 40918 RVA: 0x0037DFC0 File Offset: 0x0037C1C0
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<ArmTrapWorkable>(Db.Get().ChoreTypes.ArmTrap, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06009FD7 RID: 40919 RVA: 0x0037E006 File Offset: 0x0037C206
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("GroundTrap.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x06009FD8 RID: 40920 RVA: 0x0037E028 File Offset: 0x0037C228
		public void Release()
		{
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
				Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.smi.transform.GetPosition()), base.def.releaseCellOffset), Grid.SceneLayer.Creatures);
				List<GameObject> list = new List<GameObject>();
				Storage storage = this.storage;
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list;
				storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				foreach (GameObject gameObject in list)
				{
					gameObject.transform.SetPosition(position);
					KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.SetSceneLayer(Grid.SceneLayer.Creatures);
					}
				}
			}
		}

		// Token: 0x06009FD9 RID: 40921 RVA: 0x0037E0FC File Offset: 0x0037C2FC
		public string GetTrappedAnimationName()
		{
			if (base.def.getTrappedAnimationNameCallback != null)
			{
				return base.def.getTrappedAnimationNameCallback();
			}
			return null;
		}

		// Token: 0x04007C53 RID: 31827
		public string CAPTURING_CRITTER_ANIMATION_NAME = "caught_loop";

		// Token: 0x04007C54 RID: 31828
		public string CAPTURING_SYMBOL_NAME = "creatureSymbol";

		// Token: 0x04007C55 RID: 31829
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04007C56 RID: 31830
		[MyCmpGet]
		private ArmTrapWorkable workable;

		// Token: 0x04007C57 RID: 31831
		[MyCmpGet]
		private TrapTrigger trapTrigger;

		// Token: 0x04007C58 RID: 31832
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04007C59 RID: 31833
		[MyCmpGet]
		public LogicPorts logicPorts;

		// Token: 0x04007C5A RID: 31834
		public bool WasLastCritterLarge;

		// Token: 0x04007C5B RID: 31835
		public KBatchedAnimController lastCritterCapturedAnimController;

		// Token: 0x04007C5C RID: 31836
		private Chore chore;
	}
}

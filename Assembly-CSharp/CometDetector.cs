using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000697 RID: 1687
public class CometDetector : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>
{
	// Token: 0x06002A21 RID: 10785 RVA: 0x000EDA54 File Offset: 0x000EBC54
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(CometDetector.Instance smi)
		{
			smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(CometDetector.Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
				return;
			}
			smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
		}, UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (CometDetector.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, null).Enter("ToggleActive", delegate(CometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		}).Exit("ToggleActive", delegate(CometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(false, false);
		});
		this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
		this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, this.on.working, false).Enter("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false);
		}).Update("Scan Sky", delegate(CometDetector.Instance smi, float dt)
		{
			smi.ScanSky(false);
		}, UpdateRate.SIM_200ms, false);
		this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
		this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, null).Enter("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.SetLogicSignal(true);
		}).Exit("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.SetLogicSignal(false);
		}).Update("Scan Sky", delegate(CometDetector.Instance smi, float dt)
		{
			smi.ScanSky(true);
		}, UpdateRate.SIM_200ms, false);
		this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
		this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Detecting, this.on.working.pst, true);
		this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
	}

	// Token: 0x04001843 RID: 6211
	public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State off;

	// Token: 0x04001844 RID: 6212
	public CometDetector.OnStates on;

	// Token: 0x04001845 RID: 6213
	public StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.BoolParameter lastIsTargetDetected;

	// Token: 0x0200148F RID: 5263
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001490 RID: 5264
	public class OnStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
	{
		// Token: 0x04006A52 RID: 27218
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;

		// Token: 0x04006A53 RID: 27219
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;

		// Token: 0x04006A54 RID: 27220
		public CometDetector.WorkingStates working;

		// Token: 0x04006A55 RID: 27221
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
	}

	// Token: 0x02001491 RID: 5265
	public class WorkingStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
	{
		// Token: 0x04006A56 RID: 27222
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;

		// Token: 0x04006A57 RID: 27223
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;

		// Token: 0x04006A58 RID: 27224
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
	}

	// Token: 0x02001492 RID: 5266
	public new class Instance : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.GameInstance
	{
		// Token: 0x06008B4A RID: 35658 RVA: 0x00336345 File Offset: 0x00334545
		public Instance(IStateMachineTarget master, CometDetector.Def def) : base(master, def)
		{
			this.detectorNetworkDef = new DetectorNetwork.Def();
			this.targetCraft = new Ref<LaunchConditionManager>();
		}

		// Token: 0x06008B4B RID: 35659 RVA: 0x00336370 File Offset: 0x00334570
		public override void StartSM()
		{
			if (this.detectorNetwork == null)
			{
				this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(base.master);
			}
			this.detectorNetwork.StartSM();
			base.StartSM();
		}

		// Token: 0x06008B4C RID: 35660 RVA: 0x003363A7 File Offset: 0x003345A7
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			this.detectorNetwork.StopSM(reason);
		}

		// Token: 0x06008B4D RID: 35661 RVA: 0x003363BC File Offset: 0x003345BC
		public void UpdateDetectionState(bool currentDetection, bool expectedDetectionForState)
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (currentDetection)
			{
				component.AddTag(GameTags.Detecting, false);
			}
			else
			{
				component.RemoveTag(GameTags.Detecting);
			}
			if (currentDetection == expectedDetectionForState)
			{
				this.SetLogicSignal(currentDetection);
			}
		}

		// Token: 0x06008B4E RID: 35662 RVA: 0x003363F8 File Offset: 0x003345F8
		public void ScanSky(bool expectedDetectionForState)
		{
			LaunchConditionManager launchConditionManager = this.targetCraft.Get();
			Option<SpaceScannerTarget> option;
			if (launchConditionManager == null)
			{
				option = SpaceScannerTarget.MeteorShower();
			}
			else if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.targetCraft.Get()).state == Spacecraft.MissionState.Destroyed)
			{
				option = Option.None;
			}
			else
			{
				option = SpaceScannerTarget.RocketBaseGame(launchConditionManager);
			}
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this, false);
			this.UpdateDetectionState(flag, expectedDetectionForState);
		}

		// Token: 0x06008B4F RID: 35663 RVA: 0x003364A8 File Offset: 0x003346A8
		public void SetLogicSignal(bool on)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		// Token: 0x06008B50 RID: 35664 RVA: 0x003364C1 File Offset: 0x003346C1
		public void SetTargetCraft(LaunchConditionManager target)
		{
			this.targetCraft.Set(target);
		}

		// Token: 0x06008B51 RID: 35665 RVA: 0x003364CF File Offset: 0x003346CF
		public LaunchConditionManager GetTargetCraft()
		{
			return this.targetCraft.Get();
		}

		// Token: 0x04006A59 RID: 27225
		public bool ShowWorkingStatus;

		// Token: 0x04006A5A RID: 27226
		[Serialize]
		private Ref<LaunchConditionManager> targetCraft;

		// Token: 0x04006A5B RID: 27227
		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		// Token: 0x04006A5C RID: 27228
		private DetectorNetwork.Def detectorNetworkDef;

		// Token: 0x04006A5D RID: 27229
		private DetectorNetwork.Instance detectorNetwork;

		// Token: 0x04006A5E RID: 27230
		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();
	}
}

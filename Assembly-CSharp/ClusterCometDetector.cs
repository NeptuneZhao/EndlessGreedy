using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000695 RID: 1685
public class ClusterCometDetector : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>
{
	// Token: 0x06002A1B RID: 10779 RVA: 0x000ED138 File Offset: 0x000EB338
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(ClusterCometDetector.Instance smi)
		{
			smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(ClusterCometDetector.Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
				return;
			}
			smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
		}, UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (ClusterCometDetector.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, null).Enter("ToggleActive", delegate(ClusterCometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		}).Exit("ToggleActive", delegate(ClusterCometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(false, false);
		});
		this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
		this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, this.on.working, false).Enter("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false);
		}).Update("Scan Sky", delegate(ClusterCometDetector.Instance smi, float dt)
		{
			smi.ScanSky(false);
		}, UpdateRate.SIM_200ms, false);
		this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
		this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, null).Enter("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.SetLogicSignal(true);
		}).Exit("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.SetLogicSignal(false);
		}).Update("Scan Sky", delegate(ClusterCometDetector.Instance smi, float dt)
		{
			smi.ScanSky(true);
		}, UpdateRate.SIM_200ms, false);
		this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
		this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Detecting, this.on.working.pst, true);
		this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
	}

	// Token: 0x0400183C RID: 6204
	public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State off;

	// Token: 0x0400183D RID: 6205
	public ClusterCometDetector.OnStates on;

	// Token: 0x0400183E RID: 6206
	public StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.BoolParameter lastIsTargetDetected;

	// Token: 0x02001483 RID: 5251
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001484 RID: 5252
	public class OnStates : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
	{
		// Token: 0x040069FC RID: 27132
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;

		// Token: 0x040069FD RID: 27133
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;

		// Token: 0x040069FE RID: 27134
		public ClusterCometDetector.WorkingStates working;

		// Token: 0x040069FF RID: 27135
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
	}

	// Token: 0x02001485 RID: 5253
	public class WorkingStates : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
	{
		// Token: 0x04006A00 RID: 27136
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;

		// Token: 0x04006A01 RID: 27137
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;

		// Token: 0x04006A02 RID: 27138
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
	}

	// Token: 0x02001486 RID: 5254
	public new class Instance : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.GameInstance
	{
		// Token: 0x06008AE2 RID: 35554 RVA: 0x00335106 File Offset: 0x00333306
		public Instance(IStateMachineTarget master, ClusterCometDetector.Def def) : base(master, def)
		{
			this.detectorNetworkDef = new DetectorNetwork.Def();
		}

		// Token: 0x06008AE3 RID: 35555 RVA: 0x00335126 File Offset: 0x00333326
		public override void StartSM()
		{
			if (this.detectorNetwork == null)
			{
				this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(base.master);
			}
			this.detectorNetwork.StartSM();
			base.StartSM();
		}

		// Token: 0x06008AE4 RID: 35556 RVA: 0x0033515D File Offset: 0x0033335D
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			this.detectorNetwork.StopSM(reason);
		}

		// Token: 0x06008AE5 RID: 35557 RVA: 0x00335174 File Offset: 0x00333374
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

		// Token: 0x06008AE6 RID: 35558 RVA: 0x003351B0 File Offset: 0x003333B0
		public void ScanSky(bool expectedDetectionForState)
		{
			Option<SpaceScannerTarget> option;
			switch (this.GetDetectorState())
			{
			case ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower:
				option = SpaceScannerTarget.MeteorShower();
				break;
			case ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject:
				option = SpaceScannerTarget.BallisticObject();
				break;
			case ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket:
				if (this.targetCraft != null && this.targetCraft.Get() != null)
				{
					option = SpaceScannerTarget.RocketDlc1(this.targetCraft.Get());
				}
				else
				{
					option = Option.None;
				}
				break;
			default:
				throw new NotImplementedException();
			}
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this, false);
			this.UpdateDetectionState(flag, expectedDetectionForState);
		}

		// Token: 0x06008AE7 RID: 35559 RVA: 0x00335283 File Offset: 0x00333483
		public void SetLogicSignal(bool on)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		// Token: 0x06008AE8 RID: 35560 RVA: 0x0033529C File Offset: 0x0033349C
		public void SetDetectorState(ClusterCometDetector.Instance.ClusterCometDetectorState newState)
		{
			this.detectorState = newState;
		}

		// Token: 0x06008AE9 RID: 35561 RVA: 0x003352A5 File Offset: 0x003334A5
		public ClusterCometDetector.Instance.ClusterCometDetectorState GetDetectorState()
		{
			return this.detectorState;
		}

		// Token: 0x06008AEA RID: 35562 RVA: 0x003352AD File Offset: 0x003334AD
		public void SetClustercraftTarget(Clustercraft target)
		{
			if (target)
			{
				this.targetCraft = new Ref<Clustercraft>(target);
				return;
			}
			this.targetCraft = null;
		}

		// Token: 0x06008AEB RID: 35563 RVA: 0x003352CB File Offset: 0x003334CB
		public Clustercraft GetClustercraftTarget()
		{
			if (this.targetCraft == null)
			{
				return null;
			}
			return this.targetCraft.Get();
		}

		// Token: 0x04006A03 RID: 27139
		public bool ShowWorkingStatus;

		// Token: 0x04006A04 RID: 27140
		[Serialize]
		private ClusterCometDetector.Instance.ClusterCometDetectorState detectorState;

		// Token: 0x04006A05 RID: 27141
		[Serialize]
		private Ref<Clustercraft> targetCraft;

		// Token: 0x04006A06 RID: 27142
		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		// Token: 0x04006A07 RID: 27143
		private DetectorNetwork.Def detectorNetworkDef;

		// Token: 0x04006A08 RID: 27144
		private DetectorNetwork.Instance detectorNetwork;

		// Token: 0x04006A09 RID: 27145
		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();

		// Token: 0x020024C3 RID: 9411
		public enum ClusterCometDetectorState
		{
			// Token: 0x0400A308 RID: 41736
			MeteorShower,
			// Token: 0x0400A309 RID: 41737
			BallisticObject,
			// Token: 0x0400A30A RID: 41738
			Rocket
		}
	}
}

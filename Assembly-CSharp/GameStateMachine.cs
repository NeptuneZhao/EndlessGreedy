using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public abstract class GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> where StateMachineInstanceType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameInstance where MasterType : IStateMachineTarget
{
	// Token: 0x06001A36 RID: 6710 RVA: 0x0008AF63 File Offset: 0x00089163
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x0008AF6C File Offset: 0x0008916C
	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback And(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback first_condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback second_condition)
	{
		return (StateMachineInstanceType smi) => first_condition(smi) && second_condition(smi);
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x0008AF8C File Offset: 0x0008918C
	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback Or(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback first_condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback second_condition)
	{
		return (StateMachineInstanceType smi) => first_condition(smi) || second_condition(smi);
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x0008AFAC File Offset: 0x000891AC
	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback Not(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback transition_cb)
	{
		return (StateMachineInstanceType smi) => !transition_cb(smi);
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x0008AFC5 File Offset: 0x000891C5
	public override void BindStates()
	{
		base.BindState(null, this.root, "root");
		base.BindStates(this.root, this);
	}

	// Token: 0x04000EE9 RID: 3817
	public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State root = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State();

	// Token: 0x04000EEA RID: 3818
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Callback IsFalse = (StateMachineInstanceType smi, bool p) => !p;

	// Token: 0x04000EEB RID: 3819
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Callback IsTrue = (StateMachineInstanceType smi, bool p) => p;

	// Token: 0x04000EEC RID: 3820
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsZero = (StateMachineInstanceType smi, float p) => p == 0f;

	// Token: 0x04000EED RID: 3821
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTZero = (StateMachineInstanceType smi, float p) => p < 0f;

	// Token: 0x04000EEE RID: 3822
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTEZero = (StateMachineInstanceType smi, float p) => p <= 0f;

	// Token: 0x04000EEF RID: 3823
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTZero = (StateMachineInstanceType smi, float p) => p > 0f;

	// Token: 0x04000EF0 RID: 3824
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTEZero = (StateMachineInstanceType smi, float p) => p >= 0f;

	// Token: 0x04000EF1 RID: 3825
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsOne = (StateMachineInstanceType smi, float p) => p == 1f;

	// Token: 0x04000EF2 RID: 3826
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTOne = (StateMachineInstanceType smi, float p) => p < 1f;

	// Token: 0x04000EF3 RID: 3827
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTEOne = (StateMachineInstanceType smi, float p) => p <= 1f;

	// Token: 0x04000EF4 RID: 3828
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTOne = (StateMachineInstanceType smi, float p) => p > 1f;

	// Token: 0x04000EF5 RID: 3829
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTEOne = (StateMachineInstanceType smi, float p) => p >= 1f;

	// Token: 0x04000EF6 RID: 3830
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Callback IsNotNull = (StateMachineInstanceType smi, GameObject p) => p != null;

	// Token: 0x04000EF7 RID: 3831
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Callback IsNull = (StateMachineInstanceType smi, GameObject p) => p == null;

	// Token: 0x04000EF8 RID: 3832
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsZero_Int = (StateMachineInstanceType smi, int p) => p == 0;

	// Token: 0x04000EF9 RID: 3833
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsLTEOne_Int = (StateMachineInstanceType smi, int p) => p <= 1;

	// Token: 0x04000EFA RID: 3834
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsGTOne_Int = (StateMachineInstanceType smi, int p) => p > 1;

	// Token: 0x04000EFB RID: 3835
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsGTZero_Int = (StateMachineInstanceType smi, int p) => p > 0;

	// Token: 0x02001278 RID: 4728
	public class PreLoopPostState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x04006389 RID: 25481
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pre;

		// Token: 0x0400638A RID: 25482
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State loop;

		// Token: 0x0400638B RID: 25483
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pst;
	}

	// Token: 0x02001279 RID: 4729
	public class WorkingState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x0400638C RID: 25484
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State waiting;

		// Token: 0x0400638D RID: 25485
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_pre;

		// Token: 0x0400638E RID: 25486
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_loop;

		// Token: 0x0400638F RID: 25487
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_pst;
	}

	// Token: 0x0200127A RID: 4730
	public class GameInstance : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance
	{
		// Token: 0x06008349 RID: 33609 RVA: 0x0031EAE1 File Offset: 0x0031CCE1
		public void Queue(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			base.smi.GetComponent<KBatchedAnimController>().Queue(anim, mode, 1f, 0f);
		}

		// Token: 0x0600834A RID: 33610 RVA: 0x0031EB09 File Offset: 0x0031CD09
		public void Play(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			base.smi.GetComponent<KBatchedAnimController>().Play(anim, mode, 1f, 0f);
		}

		// Token: 0x0600834B RID: 33611 RVA: 0x0031EB31 File Offset: 0x0031CD31
		public GameInstance(MasterType master, DefType def) : base(master)
		{
			base.def = def;
		}

		// Token: 0x0600834C RID: 33612 RVA: 0x0031EB41 File Offset: 0x0031CD41
		public GameInstance(MasterType master) : base(master)
		{
		}
	}

	// Token: 0x0200127B RID: 4731
	public class TagTransitionData : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition
	{
		// Token: 0x0600834D RID: 33613 RVA: 0x0031EB4A File Offset: 0x0031CD4A
		public TagTransitionData(string name, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, Tag[] tags, bool on_remove, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, Func<StateMachineInstanceType, Tag[]> tags_callback = null) : base(name, source_state, target_state, idx, null)
		{
			this.tags = tags;
			this.onRemove = on_remove;
			this.target = target;
			this.tags_callback = tags_callback;
		}

		// Token: 0x0600834E RID: 33614 RVA: 0x0031EB78 File Offset: 0x0031CD78
		public override void Evaluate(StateMachine.Instance smi)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			if (!this.onRemove)
			{
				if (!this.HasAllTags(stateMachineInstanceType))
				{
					return;
				}
			}
			else if (this.HasAnyTags(stateMachineInstanceType))
			{
				return;
			}
			this.ExecuteTransition(stateMachineInstanceType);
		}

		// Token: 0x0600834F RID: 33615 RVA: 0x0031EBC2 File Offset: 0x0031CDC2
		private bool HasAllTags(StateMachineInstanceType smi)
		{
			return this.target.Get(smi).GetComponent<KPrefabID>().HasAllTags((this.tags_callback != null) ? this.tags_callback(smi) : this.tags);
		}

		// Token: 0x06008350 RID: 33616 RVA: 0x0031EBF6 File Offset: 0x0031CDF6
		private bool HasAnyTags(StateMachineInstanceType smi)
		{
			return this.target.Get(smi).GetComponent<KPrefabID>().HasAnyTags((this.tags_callback != null) ? this.tags_callback(smi) : this.tags);
		}

		// Token: 0x06008351 RID: 33617 RVA: 0x0031EC2A File Offset: 0x0031CE2A
		private void ExecuteTransition(StateMachineInstanceType smi)
		{
			if (this.is_executing)
			{
				return;
			}
			this.is_executing = true;
			smi.GoTo(this.targetState);
			this.is_executing = false;
		}

		// Token: 0x06008352 RID: 33618 RVA: 0x0031EC54 File Offset: 0x0031CE54
		private void OnCallback(StateMachineInstanceType smi)
		{
			if (this.target.Get(smi) == null)
			{
				return;
			}
			if (!this.onRemove)
			{
				if (!this.HasAllTags(smi))
				{
					return;
				}
			}
			else if (this.HasAnyTags(smi))
			{
				return;
			}
			this.ExecuteTransition(smi);
		}

		// Token: 0x06008353 RID: 33619 RVA: 0x0031EC90 File Offset: 0x0031CE90
		public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			StateMachineInstanceType smi_internal = smi as StateMachineInstanceType;
			global::Debug.Assert(smi_internal != null);
			StateMachine.BaseTransition.Context result = base.Register(smi_internal);
			result.handlerId = this.target.Get(smi_internal).Subscribe(-1582839653, delegate(object data)
			{
				this.OnCallback(smi_internal);
			});
			return result;
		}

		// Token: 0x06008354 RID: 33620 RVA: 0x0031ED10 File Offset: 0x0031CF10
		public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			base.Unregister(stateMachineInstanceType, context);
			if (this.target.Get(stateMachineInstanceType) != null)
			{
				this.target.Get(stateMachineInstanceType).Unsubscribe(context.handlerId);
			}
		}

		// Token: 0x04006390 RID: 25488
		private Tag[] tags;

		// Token: 0x04006391 RID: 25489
		private bool onRemove;

		// Token: 0x04006392 RID: 25490
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		// Token: 0x04006393 RID: 25491
		private bool is_executing;

		// Token: 0x04006394 RID: 25492
		private Func<StateMachineInstanceType, Tag[]> tags_callback;
	}

	// Token: 0x0200127C RID: 4732
	public class EventTransitionData : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition
	{
		// Token: 0x06008355 RID: 33621 RVA: 0x0031ED6F File Offset: 0x0031CF6F
		public EventTransitionData(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target) : base(evt.ToString(), source_state, target_state, idx, condition)
		{
			this.evtId = evt;
			this.target = target;
			this.globalEventSystemCallback = global_event_system_callback;
		}

		// Token: 0x06008356 RID: 33622 RVA: 0x0031EDA4 File Offset: 0x0031CFA4
		public override void Evaluate(StateMachine.Instance smi)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			if (this.condition != null && this.condition(stateMachineInstanceType))
			{
				this.ExecuteTransition(stateMachineInstanceType);
			}
		}

		// Token: 0x06008357 RID: 33623 RVA: 0x0031EDE8 File Offset: 0x0031CFE8
		private void ExecuteTransition(StateMachineInstanceType smi)
		{
			smi.GoTo(this.targetState);
		}

		// Token: 0x06008358 RID: 33624 RVA: 0x0031EDFB File Offset: 0x0031CFFB
		private void OnCallback(StateMachineInstanceType smi)
		{
			if (this.condition == null || this.condition(smi))
			{
				this.ExecuteTransition(smi);
			}
		}

		// Token: 0x06008359 RID: 33625 RVA: 0x0031EE1C File Offset: 0x0031D01C
		public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			StateMachineInstanceType smi_internal = smi as StateMachineInstanceType;
			global::Debug.Assert(smi_internal != null);
			StateMachine.BaseTransition.Context result = base.Register(smi_internal);
			Action<object> handler = delegate(object d)
			{
				this.OnCallback(smi_internal);
			};
			GameObject gameObject;
			if (this.globalEventSystemCallback != null)
			{
				gameObject = this.globalEventSystemCallback(smi_internal).gameObject;
			}
			else
			{
				gameObject = this.target.Get(smi_internal);
				if (gameObject == null)
				{
					throw new InvalidOperationException("TargetParameter: " + this.target.name + " is null");
				}
			}
			result.handlerId = gameObject.Subscribe((int)this.evtId, handler);
			return result;
		}

		// Token: 0x0600835A RID: 33626 RVA: 0x0031EEEC File Offset: 0x0031D0EC
		public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			base.Unregister(stateMachineInstanceType, context);
			GameObject gameObject = null;
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(stateMachineInstanceType);
				if (kmonoBehaviour != null)
				{
					gameObject = kmonoBehaviour.gameObject;
				}
			}
			else
			{
				gameObject = this.target.Get(stateMachineInstanceType);
			}
			if (gameObject != null)
			{
				gameObject.Unsubscribe(context.handlerId);
			}
		}

		// Token: 0x04006395 RID: 25493
		private GameHashes evtId;

		// Token: 0x04006396 RID: 25494
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		// Token: 0x04006397 RID: 25495
		private Func<StateMachineInstanceType, KMonoBehaviour> globalEventSystemCallback;
	}

	// Token: 0x0200127D RID: 4733
	public new class State : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x0600835B RID: 33627 RVA: 0x0031EF6C File Offset: 0x0031D16C
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter GetStateTarget()
		{
			if (this.stateTarget != null)
			{
				return this.stateTarget;
			}
			if (this.parent != null)
			{
				return ((GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)this.parent).GetStateTarget();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter targetParameter = this.sm.stateTarget;
			if (targetParameter == null)
			{
				return this.sm.masterTarget;
			}
			return targetParameter;
		}

		// Token: 0x0600835C RID: 33628 RVA: 0x0031EFC8 File Offset: 0x0031D1C8
		public int CreateDataTableEntry()
		{
			StateMachineType stateMachineType = this.sm;
			int dataTableSize = stateMachineType.dataTableSize;
			stateMachineType.dataTableSize = dataTableSize + 1;
			return dataTableSize;
		}

		// Token: 0x0600835D RID: 33629 RVA: 0x0031EFF0 File Offset: 0x0031D1F0
		public int CreateUpdateTableEntry()
		{
			StateMachineType stateMachineType = this.sm;
			int updateTableSize = stateMachineType.updateTableSize;
			stateMachineType.updateTableSize = updateTableSize + 1;
			return updateTableSize;
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x0600835E RID: 33630 RVA: 0x0031F018 File Offset: 0x0031D218
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State root
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600835F RID: 33631 RVA: 0x0031F01B File Offset: 0x0031D21B
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoNothing()
		{
			return this;
		}

		// Token: 0x06008360 RID: 33632 RVA: 0x0031F020 File Offset: 0x0031D220
		private static List<StateMachine.Action> AddAction(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback, List<StateMachine.Action> actions, bool add_to_end)
		{
			if (actions == null)
			{
				actions = new List<StateMachine.Action>();
			}
			StateMachine.Action item = new StateMachine.Action(name, callback);
			if (add_to_end)
			{
				actions.Add(item);
			}
			else
			{
				actions.Insert(0, item);
			}
			return actions;
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06008361 RID: 33633 RVA: 0x0031F055 File Offset: 0x0031D255
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State master
		{
			get
			{
				this.stateTarget = this.sm.masterTarget;
				return this;
			}
		}

		// Token: 0x06008362 RID: 33634 RVA: 0x0031F06E File Offset: 0x0031D26E
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Target(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
		{
			this.stateTarget = target;
			return this;
		}

		// Token: 0x06008363 RID: 33635 RVA: 0x0031F078 File Offset: 0x0031D278
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Update(Action<StateMachineInstanceType, float> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.Update(this.sm.name + "." + this.name, callback, update_rate, load_balance);
		}

		// Token: 0x06008364 RID: 33636 RVA: 0x0031F0A3 File Offset: 0x0031D2A3
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BatchUpdate(UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			return this.BatchUpdate(this.sm.name + "." + this.name, batch_update, update_rate);
		}

		// Token: 0x06008365 RID: 33637 RVA: 0x0031F0CD File Offset: 0x0031D2CD
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Enter(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.Enter("Enter", callback);
		}

		// Token: 0x06008366 RID: 33638 RVA: 0x0031F0DB File Offset: 0x0031D2DB
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Exit(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.Exit("Exit", callback);
		}

		// Token: 0x06008367 RID: 33639 RVA: 0x0031F0EC File Offset: 0x0031D2EC
		private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InternalUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater bucket_updater, UpdateRate update_rate, bool load_balance, UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update = null)
		{
			int updateTableIdx = this.CreateUpdateTableEntry();
			if (this.updateActions == null)
			{
				this.updateActions = new List<StateMachine.UpdateAction>();
			}
			StateMachine.UpdateAction updateAction = default(StateMachine.UpdateAction);
			updateAction.updateTableIdx = updateTableIdx;
			updateAction.updateRate = update_rate;
			updateAction.updater = bucket_updater;
			int num = 1;
			if (load_balance)
			{
				num = Singleton<StateMachineUpdater>.Instance.GetFrameCount(update_rate);
			}
			updateAction.buckets = new StateMachineUpdater.BaseUpdateBucket[num];
			for (int i = 0; i < num; i++)
			{
				UpdateBucketWithUpdater<StateMachineInstanceType> updateBucketWithUpdater = new UpdateBucketWithUpdater<StateMachineInstanceType>(name);
				updateBucketWithUpdater.batch_update_delegate = batch_update;
				Singleton<StateMachineUpdater>.Instance.AddBucket(update_rate, updateBucketWithUpdater);
				updateAction.buckets[i] = updateBucketWithUpdater;
			}
			this.updateActions.Add(updateAction);
			return this;
		}

		// Token: 0x06008368 RID: 33640 RVA: 0x0031F194 File Offset: 0x0031D394
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State UpdateTransition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State destination_state, Func<StateMachineInstanceType, float, bool> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			Action<StateMachineInstanceType, float> checkCallback = delegate(StateMachineInstanceType smi, float dt)
			{
				if (callback(smi, dt))
				{
					smi.GoTo(destination_state);
				}
			};
			this.Enter(delegate(StateMachineInstanceType smi)
			{
				checkCallback(smi, 0f);
			});
			this.Update(checkCallback, update_rate, load_balance);
			return this;
		}

		// Token: 0x06008369 RID: 33641 RVA: 0x0031F1EB File Offset: 0x0031D3EB
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Update(string name, Action<StateMachineInstanceType, float> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.InternalUpdate(name, new BucketUpdater<StateMachineInstanceType>(callback), update_rate, load_balance, null);
		}

		// Token: 0x0600836A RID: 33642 RVA: 0x0031F1FE File Offset: 0x0031D3FE
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BatchUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			return this.InternalUpdate(name, null, update_rate, false, batch_update);
		}

		// Token: 0x0600836B RID: 33643 RVA: 0x0031F20B File Offset: 0x0031D40B
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State FastUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater updater, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.InternalUpdate(name, updater, update_rate, load_balance, null);
		}

		// Token: 0x0600836C RID: 33644 RVA: 0x0031F219 File Offset: 0x0031D419
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Enter(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			this.enterActions = GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.AddAction(name, callback, this.enterActions, true);
			return this;
		}

		// Token: 0x0600836D RID: 33645 RVA: 0x0031F230 File Offset: 0x0031D430
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Exit(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			this.exitActions = GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.AddAction(name, callback, this.exitActions, false);
			return this;
		}

		// Token: 0x0600836E RID: 33646 RVA: 0x0031F248 File Offset: 0x0031D448
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Toggle(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback enter_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback exit_callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleEnter(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.dataTable[data_idx] = GameStateMachineHelper.HasToggleEnteredFlag;
				enter_callback(smi);
			});
			this.Exit("ToggleExit(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					smi.dataTable[data_idx] = null;
					exit_callback(smi);
				}
			});
			return this;
		}

		// Token: 0x0600836F RID: 33647 RVA: 0x0031F2BC File Offset: 0x0031D4BC
		private void Break(StateMachineInstanceType smi)
		{
		}

		// Token: 0x06008370 RID: 33648 RVA: 0x0031F2BE File Offset: 0x0031D4BE
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BreakOnEnter()
		{
			return this.Enter(delegate(StateMachineInstanceType smi)
			{
				this.Break(smi);
			});
		}

		// Token: 0x06008371 RID: 33649 RVA: 0x0031F2D2 File Offset: 0x0031D4D2
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BreakOnExit()
		{
			return this.Exit(delegate(StateMachineInstanceType smi)
			{
				this.Break(smi);
			});
		}

		// Token: 0x06008372 RID: 33650 RVA: 0x0031F2E8 File Offset: 0x0031D4E8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State AddEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(effect_name, true);
			});
			return this;
		}

		// Token: 0x06008373 RID: 33651 RVA: 0x0031F338 File Offset: 0x0031D538
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(Func<StateMachineInstanceType, KAnimFile> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableAnims()", delegate(StateMachineInstanceType smi)
			{
				KAnimFile kanimFile = chooser_callback(smi);
				if (kanimFile == null)
				{
					return;
				}
				state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(kanimFile, 0f);
			});
			this.Exit("Disableanims()", delegate(StateMachineInstanceType smi)
			{
				KAnimFile kanimFile = chooser_callback(smi);
				if (kanimFile == null)
				{
					return;
				}
				state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(kanimFile);
			});
			return this;
		}

		// Token: 0x06008374 RID: 33652 RVA: 0x0031F390 File Offset: 0x0031D590
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(Func<StateMachineInstanceType, HashedString> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableAnims()", delegate(StateMachineInstanceType smi)
			{
				HashedString hashedString = chooser_callback(smi);
				if (hashedString == null)
				{
					return;
				}
				if (hashedString.IsValid)
				{
					KAnimFile anim = Assets.GetAnim(hashedString);
					if (anim == null)
					{
						string str = "Missing anims: ";
						HashedString hashedString2 = hashedString;
						global::Debug.LogWarning(str + hashedString2.ToString());
						return;
					}
					state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(anim, 0f);
				}
			});
			this.Exit("Disableanims()", delegate(StateMachineInstanceType smi)
			{
				HashedString hashedString = chooser_callback(smi);
				if (hashedString == null)
				{
					return;
				}
				if (hashedString.IsValid)
				{
					KAnimFile anim = Assets.GetAnim(hashedString);
					if (anim != null)
					{
						state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(anim);
					}
				}
			});
			return this;
		}

		// Token: 0x06008375 RID: 33653 RVA: 0x0031F3E8 File Offset: 0x0031D5E8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(string anim_file, float priority = 0f)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Toggle("ToggleAnims(" + anim_file + ")", delegate(StateMachineInstanceType smi)
			{
				KAnimFile anim = Assets.GetAnim(anim_file);
				if (anim == null)
				{
					global::Debug.LogError("Trying to add missing override anims:" + anim_file);
				}
				state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(anim, priority);
			}, delegate(StateMachineInstanceType smi)
			{
				KAnimFile anim = Assets.GetAnim(anim_file);
				state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(anim);
			});
			return this;
		}

		// Token: 0x06008376 RID: 33654 RVA: 0x0031F44C File Offset: 0x0031D64C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAttributeModifier(string modifier_name, Func<StateMachineInstanceType, AttributeModifier> callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddAttributeModifier( " + modifier_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					AttributeModifier attributeModifier = callback(smi);
					DebugUtil.Assert(smi.dataTable[data_idx] == null);
					smi.dataTable[data_idx] = attributeModifier;
					state_target.Get(smi).GetAttributes().Add(attributeModifier);
				}
			});
			this.Exit("RemoveAttributeModifier( " + modifier_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					AttributeModifier modifier = (AttributeModifier)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					GameObject gameObject = state_target.Get(smi);
					if (gameObject != null)
					{
						gameObject.GetAttributes().Remove(modifier);
					}
				}
			});
			return this;
		}

		// Token: 0x06008377 RID: 33655 RVA: 0x0031F4CC File Offset: 0x0031D6CC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleLoopingSound(string event_name, Func<StateMachineInstanceType, bool> condition = null, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StartLoopingSound( " + event_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					state_target.Get(smi).GetComponent<LoopingSounds>().StartSound(event_name, pause_on_game_pause, enable_culling, enable_camera_scaled_position);
				}
			});
			this.Exit("StopLoopingSound( " + event_name + " )", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetComponent<LoopingSounds>().StopSound(event_name);
			});
			return this;
		}

		// Token: 0x06008378 RID: 33656 RVA: 0x0031F564 File Offset: 0x0031D764
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleLoopingSound(string state_label, Func<StateMachineInstanceType, string> event_name_callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("StartLoopingSound( " + state_label + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					string text = event_name_callback(smi);
					smi.dataTable[data_idx] = text;
					state_target.Get(smi).GetComponent<LoopingSounds>().StartSound(text);
				}
			});
			this.Exit("StopLoopingSound( " + state_label + " )", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					state_target.Get(smi).GetComponent<LoopingSounds>().StopSound((string)smi.dataTable[data_idx]);
					smi.dataTable[data_idx] = null;
				}
			});
			return this;
		}

		// Token: 0x06008379 RID: 33657 RVA: 0x0031F5E4 File Offset: 0x0031D7E4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State RefreshUserMenuOnEnter()
		{
			this.Enter("RefreshUserMenuOnEnter()", delegate(StateMachineInstanceType smi)
			{
				UserMenu userMenu = Game.Instance.userMenu;
				MasterType master = smi.master;
				userMenu.Refresh(master.gameObject);
			});
			return this;
		}

		// Token: 0x0600837A RID: 33658 RVA: 0x0031F614 File Offset: 0x0031D814
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableStartTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableStartTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkStarted)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableStartTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		// Token: 0x0600837B RID: 33659 RVA: 0x0031F69C File Offset: 0x0031D89C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableStopTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableStopTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkStopped)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableStopTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		// Token: 0x0600837C RID: 33660 RVA: 0x0031F724 File Offset: 0x0031D924
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableCompleteTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableCompleteTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkCompleted)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableCompleteTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		// Token: 0x0600837D RID: 33661 RVA: 0x0031F7AC File Offset: 0x0031D9AC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleGravity()
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddComponent<Gravity>()", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				smi.dataTable[data_idx] = gameObject;
				GameComps.Gravities.Add(gameObject, Vector2.zero, null);
			});
			this.Exit("RemoveComponent<Gravity>()", delegate(StateMachineInstanceType smi)
			{
				GameObject go = (GameObject)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				GameComps.Gravities.Remove(go);
			});
			return this;
		}

		// Token: 0x0600837E RID: 33662 RVA: 0x0031F808 File Offset: 0x0031DA08
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleGravity(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State landed_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.EventTransition(GameHashes.Landed, landed_state, null);
			this.Toggle("GravityComponent", delegate(StateMachineInstanceType smi)
			{
				GameComps.Gravities.Add(state_target.Get(smi), Vector2.zero, null);
			}, delegate(StateMachineInstanceType smi)
			{
				GameComps.Gravities.Remove(state_target.Get(smi));
			});
			return this;
		}

		// Token: 0x0600837F RID: 33663 RVA: 0x0031F85C File Offset: 0x0031DA5C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleThought(Func<StateMachineInstanceType, Thought> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
			});
			this.Exit("DisableThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
			});
			return this;
		}

		// Token: 0x06008380 RID: 33664 RVA: 0x0031F8B4 File Offset: 0x0031DAB4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleThought(Thought thought, Func<StateMachineInstanceType, bool> condition_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition_callback == null || condition_callback(smi))
				{
					state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
				}
			});
			if (condition_callback != null)
			{
				this.Update("ValidateThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					if (condition_callback(smi))
					{
						state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
						return;
					}
					state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
			});
			return this;
		}

		// Token: 0x06008381 RID: 33665 RVA: 0x0031F974 File Offset: 0x0031DB74
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCreatureThought(Func<StateMachineInstanceType, Thought> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableCreatureThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
			});
			this.Exit("DisableCreatureThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				CreatureThoughtGraph.Instance smi2 = state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>();
				if (smi2 != null)
				{
					smi2.RemoveThought(thought);
				}
			});
			return this;
		}

		// Token: 0x06008382 RID: 33666 RVA: 0x0031F9CC File Offset: 0x0031DBCC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCreatureThought(Thought thought, Func<StateMachineInstanceType, bool> condition_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddCreatureThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition_callback == null || condition_callback(smi))
				{
					state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
				}
			});
			if (condition_callback != null)
			{
				this.Update("ValidateCreatureThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					if (condition_callback(smi))
					{
						state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
						return;
					}
					state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().RemoveThought(thought);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveCreatureThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				CreatureThoughtGraph.Instance smi2 = state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>();
				if (smi2 != null)
				{
					smi2.RemoveThought(thought);
				}
			});
			return this;
		}

		// Token: 0x06008383 RID: 33667 RVA: 0x0031FA8C File Offset: 0x0031DC8C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleExpression(Func<StateMachineInstanceType, Expression> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddExpression", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<FaceGraph>(smi).AddExpression(chooser_callback(smi));
			});
			this.Exit("RemoveExpression", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<FaceGraph>(smi).RemoveExpression(chooser_callback(smi));
			});
			return this;
		}

		// Token: 0x06008384 RID: 33668 RVA: 0x0031FAE4 File Offset: 0x0031DCE4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleExpression(Expression expression, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					state_target.Get<FaceGraph>(smi).AddExpression(expression);
				}
			});
			if (condition != null)
			{
				this.Update("ValidateExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					if (condition(smi))
					{
						state_target.Get<FaceGraph>(smi).AddExpression(expression);
						return;
					}
					state_target.Get<FaceGraph>(smi).RemoveExpression(expression);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi)
			{
				FaceGraph faceGraph = state_target.Get<FaceGraph>(smi);
				if (faceGraph != null)
				{
					faceGraph.RemoveExpression(expression);
				}
			});
			return this;
		}

		// Token: 0x06008385 RID: 33669 RVA: 0x0031FBA4 File Offset: 0x0031DDA4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleMainStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddMainStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				object data = (callback != null) ? callback(smi) : smi;
				state_target.Get<KSelectable>(smi).SetStatusItem(Db.Get().StatusItemCategories.Main, status_item, data);
			});
			this.Exit("RemoveMainStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
				}
			});
			return this;
		}

		// Token: 0x06008386 RID: 33670 RVA: 0x0031FC2C File Offset: 0x0031DE2C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleMainStatusItem(Func<StateMachineInstanceType, StatusItem> status_item_cb, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddMainStatusItem(DynamicGeneration)", delegate(StateMachineInstanceType smi)
			{
				object data = (callback != null) ? callback(smi) : smi;
				state_target.Get<KSelectable>(smi).SetStatusItem(Db.Get().StatusItemCategories.Main, status_item_cb(smi), data);
			});
			this.Exit("RemoveMainStatusItem(DynamicGeneration)", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
				}
			});
			return this;
		}

		// Token: 0x06008387 RID: 33671 RVA: 0x0031FC8C File Offset: 0x0031DE8C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCategoryStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"AddCategoryStatusItem(",
				category.Id,
				", ",
				status_item.Id,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KSelectable>(smi).SetStatusItem(category, status_item, (data != null) ? data : smi);
			});
			this.Exit(string.Concat(new string[]
			{
				"RemoveCategoryStatusItem(",
				category.Id,
				", ",
				status_item.Id,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(category, null, null);
				}
			});
			return this;
		}

		// Token: 0x06008388 RID: 33672 RVA: 0x0031FD68 File Offset: 0x0031DF68
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, object data = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				object obj = data;
				if (obj == null)
				{
					obj = smi;
				}
				Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(status_item, obj);
				smi.dataTable[data_idx] = guid;
			});
			this.Exit("RemoveStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					Guid guid = (Guid)smi.dataTable[data_idx];
					kselectable.RemoveStatusItem(guid, false);
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x06008389 RID: 33673 RVA: 0x0031FDFC File Offset: 0x0031DFFC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleSnapOn(string snap_on)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("SnapOn(" + snap_on + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<SnapOn>(smi).AttachSnapOnByName(snap_on);
			});
			this.Exit("SnapOff(" + snap_on + ")", delegate(StateMachineInstanceType smi)
			{
				SnapOn snapOn = state_target.Get<SnapOn>(smi);
				if (snapOn != null)
				{
					snapOn.DetachSnapOnByName(snap_on);
				}
			});
			return this;
		}

		// Token: 0x0600838A RID: 33674 RVA: 0x0031FE74 File Offset: 0x0031E074
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleTag(Tag tag)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddTag(" + tag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).AddTag(tag, false);
			});
			this.Exit("RemoveTag(" + tag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).RemoveTag(tag);
			});
			return this;
		}

		// Token: 0x0600838B RID: 33675 RVA: 0x0031FEF8 File Offset: 0x0031E0F8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleTag(Func<StateMachineInstanceType, Tag> behaviour_tag_cb)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddTag(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).AddTag(behaviour_tag_cb(smi), false);
			});
			this.Exit("RemoveTag(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).RemoveTag(behaviour_tag_cb(smi));
			});
			return this;
		}

		// Token: 0x0600838C RID: 33676 RVA: 0x0031FF4F File Offset: 0x0031E14F
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback)
		{
			return this.ToggleStatusItem(status_item, callback, null);
		}

		// Token: 0x0600838D RID: 33677 RVA: 0x0031FF5C File Offset: 0x0031E15C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback, StatusItemCategory category)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (category == null)
				{
					object data = (callback != null) ? callback(smi) : null;
					Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(status_item, data);
					smi.dataTable[data_idx] = guid;
					return;
				}
				object data2 = (callback != null) ? callback(smi) : null;
				Guid guid2 = state_target.Get<KSelectable>(smi).SetStatusItem(category, status_item, data2);
				smi.dataTable[data_idx] = guid2;
			});
			this.Exit("RemoveStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					if (category == null)
					{
						Guid guid = (Guid)smi.dataTable[data_idx];
						kselectable.RemoveStatusItem(guid, false);
					}
					else
					{
						kselectable.SetStatusItem(category, null, null);
					}
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x0600838E RID: 33678 RVA: 0x0031FFF8 File Offset: 0x0031E1F8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(Func<StateMachineInstanceType, StatusItem> status_item_cb, Func<StateMachineInstanceType, object> data_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				StatusItem statusItem = status_item_cb(smi);
				if (statusItem != null)
				{
					object data = (data_callback != null) ? data_callback(smi) : null;
					Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(statusItem, data);
					smi.dataTable[data_idx] = guid;
				}
			});
			this.Exit("RemoveStatusItem(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					Guid guid = (Guid)smi.dataTable[data_idx];
					kselectable.RemoveStatusItem(guid, false);
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x0600838F RID: 33679 RVA: 0x00320064 File Offset: 0x0031E264
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleFX(Func<StateMachineInstanceType, StateMachine.Instance> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableFX()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = callback(smi);
				if (instance != null)
				{
					instance.StartSM();
					smi.dataTable[data_idx] = instance;
				}
			});
			this.Exit("DisableFX()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = (StateMachine.Instance)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (instance != null)
				{
					instance.StopSM("ToggleFX.Exit");
				}
			});
			return this;
		}

		// Token: 0x06008390 RID: 33680 RVA: 0x003200BC File Offset: 0x0031E2BC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BehaviourComplete(Func<StateMachineInstanceType, Tag> tag_cb, bool on_exit = false)
		{
			if (on_exit)
			{
				this.Exit("BehaviourComplete()", delegate(StateMachineInstanceType smi)
				{
					smi.Trigger(-739654666, tag_cb(smi));
					smi.GoTo(null);
				});
			}
			else
			{
				this.Enter("BehaviourComplete()", delegate(StateMachineInstanceType smi)
				{
					smi.Trigger(-739654666, tag_cb(smi));
					smi.GoTo(null);
				});
			}
			return this;
		}

		// Token: 0x06008391 RID: 33681 RVA: 0x0032010C File Offset: 0x0031E30C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BehaviourComplete(Tag tag, bool on_exit = false)
		{
			if (on_exit)
			{
				this.Exit("BehaviourComplete(" + tag.ToString() + ")", delegate(StateMachineInstanceType smi)
				{
					smi.Trigger(-739654666, tag);
					smi.GoTo(null);
				});
			}
			else
			{
				this.Enter("BehaviourComplete(" + tag.ToString() + ")", delegate(StateMachineInstanceType smi)
				{
					smi.Trigger(-739654666, tag);
					smi.GoTo(null);
				});
			}
			return this;
		}

		// Token: 0x06008392 RID: 33682 RVA: 0x00320194 File Offset: 0x0031E394
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBehaviour(Tag behaviour_tag, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback precondition, Action<StateMachineInstanceType> on_complete = null)
		{
			Func<object, bool> precondition_cb = (object obj) => precondition(obj as StateMachineInstanceType);
			this.Enter("AddPrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().AddBehaviourPrecondition(behaviour_tag, precondition_cb, smi);
				}
			});
			this.Exit("RemovePrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().RemoveBehaviourPrecondition(behaviour_tag, precondition_cb, smi);
				}
			});
			this.ToggleTag(behaviour_tag);
			if (on_complete != null)
			{
				this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object data)
				{
					if ((Tag)data == behaviour_tag)
					{
						on_complete(smi);
					}
				});
			}
			return this;
		}

		// Token: 0x06008393 RID: 33683 RVA: 0x0032022C File Offset: 0x0031E42C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBehaviour(Func<StateMachineInstanceType, Tag> behaviour_tag_cb, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback precondition, Action<StateMachineInstanceType> on_complete = null)
		{
			Func<object, bool> precondition_cb = (object obj) => precondition(obj as StateMachineInstanceType);
			this.Enter("AddPrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().AddBehaviourPrecondition(behaviour_tag_cb(smi), precondition_cb, smi);
				}
			});
			this.Exit("RemovePrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().RemoveBehaviourPrecondition(behaviour_tag_cb(smi), precondition_cb, smi);
				}
			});
			this.ToggleTag(behaviour_tag_cb);
			if (on_complete != null)
			{
				this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object data)
				{
					if ((Tag)data == behaviour_tag_cb(smi))
					{
						on_complete(smi);
					}
				});
			}
			return this;
		}

		// Token: 0x06008394 RID: 33684 RVA: 0x003202C4 File Offset: 0x0031E4C4
		public void ClearFetch(StateMachineInstanceType smi, int fetch_data_idx, int callback_data_idx)
		{
			FetchList2 fetchList = (FetchList2)smi.dataTable[fetch_data_idx];
			if (fetchList != null)
			{
				smi.dataTable[fetch_data_idx] = null;
				smi.dataTable[callback_data_idx] = null;
				fetchList.Cancel("ClearFetchListFromSM");
			}
		}

		// Token: 0x06008395 RID: 33685 RVA: 0x00320310 File Offset: 0x0031E510
		public void SetupFetch(Func<StateMachineInstanceType, FetchList2> create_fetchlist_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, StateMachineInstanceType smi, int fetch_data_idx, int callback_data_idx)
		{
			FetchList2 fetchList = create_fetchlist_callback(smi);
			System.Action action = delegate()
			{
				this.ClearFetch(smi, fetch_data_idx, callback_data_idx);
				smi.GoTo(target_state);
			};
			fetchList.Submit(action, true);
			smi.dataTable[fetch_data_idx] = fetchList;
			smi.dataTable[callback_data_idx] = action;
		}

		// Token: 0x06008396 RID: 33686 RVA: 0x0032039C File Offset: 0x0031E59C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleFetch(Func<StateMachineInstanceType, FetchList2> create_fetchlist_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleFetchEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupFetch(create_fetchlist_callback, target_state, smi, data_idx, callback_data_idx);
			});
			this.Exit("ToggleFetchExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearFetch(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		// Token: 0x06008397 RID: 33687 RVA: 0x00320410 File Offset: 0x0031E610
		private void ClearChore(StateMachineInstanceType smi, int chore_data_idx, int callback_data_idx)
		{
			Chore chore = (Chore)smi.dataTable[chore_data_idx];
			if (chore != null)
			{
				Action<Chore> value = (Action<Chore>)smi.dataTable[callback_data_idx];
				smi.dataTable[chore_data_idx] = null;
				smi.dataTable[callback_data_idx] = null;
				Chore chore2 = chore;
				chore2.onExit = (Action<Chore>)Delegate.Remove(chore2.onExit, value);
				chore.Cancel("ClearGlobalChore");
			}
		}

		// Token: 0x06008398 RID: 33688 RVA: 0x00320484 File Offset: 0x0031E684
		private Chore SetupChore(Func<StateMachineInstanceType, Chore> create_chore_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state, StateMachineInstanceType smi, int chore_data_idx, int callback_data_idx, bool is_success_state_reentrant, bool is_failure_state_reentrant)
		{
			Chore chore = create_chore_callback(smi);
			DebugUtil.DevAssert(!chore.IsPreemptable, "ToggleChore can't be used with preemptable chores! :( (but it should...)", null);
			chore.runUntilComplete = false;
			Action<Chore> action = delegate(Chore chore_param)
			{
				bool isComplete = chore.isComplete;
				if ((isComplete & is_success_state_reentrant) || (is_failure_state_reentrant && !isComplete))
				{
					this.SetupChore(create_chore_callback, success_state, failure_state, smi, chore_data_idx, callback_data_idx, is_success_state_reentrant, is_failure_state_reentrant);
					return;
				}
				GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = success_state;
				if (!isComplete)
				{
					state = failure_state;
				}
				this.ClearChore(smi, chore_data_idx, callback_data_idx);
				smi.GoTo(state);
			};
			Chore chore2 = chore;
			chore2.onExit = (Action<Chore>)Delegate.Combine(chore2.onExit, action);
			smi.dataTable[chore_data_idx] = chore;
			smi.dataTable[callback_data_idx] = action;
			return chore;
		}

		// Token: 0x06008399 RID: 33689 RVA: 0x0032057C File Offset: 0x0031E77C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleRecurringChore(Func<StateMachineInstanceType, Chore> callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleRecurringChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					this.SetupChore(callback, this, this, smi, data_idx, callback_data_idx, true, true);
				}
			});
			this.Exit("ToggleRecurringChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		// Token: 0x0600839A RID: 33690 RVA: 0x003205F0 File Offset: 0x0031E7F0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupChore(callback, target_state, target_state, smi, data_idx, callback_data_idx, false, false);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		// Token: 0x0600839B RID: 33691 RVA: 0x00320664 File Offset: 0x0031E864
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			bool is_success_state_reentrant = success_state == this;
			bool is_failure_state_reentrant = failure_state == this;
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupChore(callback, success_state, failure_state, smi, data_idx, callback_data_idx, is_success_state_reentrant, is_failure_state_reentrant);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		// Token: 0x0600839C RID: 33692 RVA: 0x003206FC File Offset: 0x0031E8FC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleReactable(Func<StateMachineInstanceType, Reactable> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter(delegate(StateMachineInstanceType smi)
			{
				smi.dataTable[data_idx] = callback(smi);
			});
			this.Exit(delegate(StateMachineInstanceType smi)
			{
				Reactable reactable = (Reactable)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (reactable != null)
				{
					reactable.Cleanup();
				}
			});
			return this;
		}

		// Token: 0x0600839D RID: 33693 RVA: 0x0032074C File Offset: 0x0031E94C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State RemoveEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("RemoveEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(effect_name);
			});
			return this;
		}

		// Token: 0x0600839E RID: 33694 RVA: 0x0032079C File Offset: 0x0031E99C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(effect_name, false);
			});
			this.Exit("RemoveEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(effect_name);
			});
			return this;
		}

		// Token: 0x0600839F RID: 33695 RVA: 0x00320814 File Offset: 0x0031EA14
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(Func<StateMachineInstanceType, Effect> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(callback(smi), false);
			});
			this.Exit("RemoveEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(callback(smi));
			});
			return this;
		}

		// Token: 0x060083A0 RID: 33696 RVA: 0x0032086C File Offset: 0x0031EA6C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(Func<StateMachineInstanceType, string> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(callback(smi), false);
			});
			this.Exit("RemoveEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(callback(smi));
			});
			return this;
		}

		// Token: 0x060083A1 RID: 33697 RVA: 0x003208C3 File Offset: 0x0031EAC3
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State LogOnExit(Func<StateMachineInstanceType, string> callback)
		{
			this.Enter("Log()", delegate(StateMachineInstanceType smi)
			{
			});
			return this;
		}

		// Token: 0x060083A2 RID: 33698 RVA: 0x003208F1 File Offset: 0x0031EAF1
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State LogOnEnter(Func<StateMachineInstanceType, string> callback)
		{
			this.Exit("Log()", delegate(StateMachineInstanceType smi)
			{
			});
			return this;
		}

		// Token: 0x060083A3 RID: 33699 RVA: 0x00320920 File Offset: 0x0031EB20
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleUrge(Urge urge)
		{
			return this.ToggleUrge((StateMachineInstanceType smi) => urge);
		}

		// Token: 0x060083A4 RID: 33700 RVA: 0x0032094C File Offset: 0x0031EB4C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleUrge(Func<StateMachineInstanceType, Urge> urge_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddUrge()", delegate(StateMachineInstanceType smi)
			{
				Urge urge = urge_callback(smi);
				state_target.Get<ChoreConsumer>(smi).AddUrge(urge);
			});
			this.Exit("RemoveUrge()", delegate(StateMachineInstanceType smi)
			{
				Urge urge = urge_callback(smi);
				ChoreConsumer choreConsumer = state_target.Get<ChoreConsumer>(smi);
				if (choreConsumer != null)
				{
					choreConsumer.RemoveUrge(urge);
				}
			});
			return this;
		}

		// Token: 0x060083A5 RID: 33701 RVA: 0x003209A3 File Offset: 0x0031EBA3
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnTargetLost(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			this.ParamTransition<GameObject>(parameter, target_state, (StateMachineInstanceType smi, GameObject p) => p == null);
			return this;
		}

		// Token: 0x060083A6 RID: 33702 RVA: 0x003209D0 File Offset: 0x0031EBD0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBrain(string reason)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StopBrain(" + reason + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Brain>(smi).Stop(reason);
			});
			this.Exit("ResetBrain(" + reason + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Brain>(smi).Reset(reason);
			});
			return this;
		}

		// Token: 0x060083A7 RID: 33703 RVA: 0x00320A48 File Offset: 0x0031EC48
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PreBrainUpdate(Action<StateMachineInstanceType> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnablePreBrainUpdate", delegate(StateMachineInstanceType smi)
			{
				System.Action action = delegate()
				{
					callback(smi);
				};
				smi.dataTable[data_idx] = action;
				Brain brain = state_target.Get<Brain>(smi);
				DebugUtil.AssertArgs(brain != null, new object[]
				{
					"PreBrainUpdate cannot find a brain"
				});
				brain.onPreUpdate += action;
			});
			this.Exit("DisablePreBrainUpdate", delegate(StateMachineInstanceType smi)
			{
				System.Action value = (System.Action)smi.dataTable[data_idx];
				state_target.Get<Brain>(smi).onPreUpdate -= value;
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x060083A8 RID: 33704 RVA: 0x00320AAC File Offset: 0x0031ECAC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TriggerOnEnter(GameHashes evt, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("Trigger(" + evt.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject go = state_target.Get(smi);
				object data = (callback != null) ? callback(smi) : null;
				go.Trigger((int)evt, data);
			});
			return this;
		}

		// Token: 0x060083A9 RID: 33705 RVA: 0x00320B10 File Offset: 0x0031ED10
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TriggerOnExit(GameHashes evt, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Exit("Trigger(" + evt.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					object data = (callback != null) ? callback(smi) : null;
					gameObject.Trigger((int)evt, data);
				}
			});
			return this;
		}

		// Token: 0x060083AA RID: 33706 RVA: 0x00320B74 File Offset: 0x0031ED74
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStateMachineList(Func<StateMachineInstanceType, Func<StateMachineInstanceType, StateMachine.Instance>[]> getListCallback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableListOfStateMachines()", delegate(StateMachineInstanceType smi)
			{
				Func<StateMachineInstanceType, StateMachine.Instance>[] array = getListCallback(smi);
				StateMachine.Instance[] array2 = new StateMachine.Instance[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					StateMachine.Instance instance = array[i](smi);
					instance.StartSM();
					array2[i] = instance;
				}
				smi.dataTable[data_idx] = array2;
			});
			this.Exit("DisableListOfStateMachines()", delegate(StateMachineInstanceType smi)
			{
				Func<StateMachineInstanceType, StateMachine.Instance>[] array = getListCallback(smi);
				StateMachine.Instance[] array2 = (StateMachine.Instance[])smi.dataTable[data_idx];
				for (int i = array.Length - 1; i >= 0; i--)
				{
					StateMachine.Instance instance = array2[i];
					if (instance != null)
					{
						instance.StopSM("ToggleListOfStateMachines.Exit");
					}
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x060083AB RID: 33707 RVA: 0x00320BCC File Offset: 0x0031EDCC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStateMachine(Func<StateMachineInstanceType, StateMachine.Instance> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableStateMachine()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = callback(smi);
				smi.dataTable[data_idx] = instance;
				instance.StartSM();
			});
			this.Exit("DisableStateMachine()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = (StateMachine.Instance)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (instance != null)
				{
					instance.StopSM("ToggleStateMachine.Exit");
				}
			});
			return this;
		}

		// Token: 0x060083AC RID: 33708 RVA: 0x00320C24 File Offset: 0x0031EE24
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleComponentIfFound<ComponentType>(bool disable = false) where ComponentType : MonoBehaviour
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					ComponentType component = gameObject.GetComponent<ComponentType>();
					if (component != null)
					{
						component.enabled = !disable;
					}
				}
			});
			this.Exit("DisableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					ComponentType component = gameObject.GetComponent<ComponentType>();
					if (component != null)
					{
						component.enabled = disable;
					}
				}
			});
			return this;
		}

		// Token: 0x060083AD RID: 33709 RVA: 0x00320CB0 File Offset: 0x0031EEB0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleComponent<ComponentType>(bool disable = false) where ComponentType : MonoBehaviour
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<ComponentType>(smi).enabled = !disable;
			});
			this.Exit("DisableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<ComponentType>(smi).enabled = disable;
			});
			return this;
		}

		// Token: 0x060083AE RID: 33710 RVA: 0x00320D3C File Offset: 0x0031EF3C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeOperationalFlag(Operational.Flag flag, bool init_val = false)
		{
			this.Enter(string.Concat(new string[]
			{
				"InitOperationalFlag (",
				flag.Name,
				", ",
				init_val.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, init_val);
			});
			return this;
		}

		// Token: 0x060083AF RID: 33711 RVA: 0x00320DB0 File Offset: 0x0031EFB0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleOperationalFlag(Operational.Flag flag)
		{
			this.Enter("ToggleOperationalFlag True (" + flag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, true);
			});
			this.Exit("ToggleOperationalFlag False (" + flag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, false);
			});
			return this;
		}

		// Token: 0x060083B0 RID: 33712 RVA: 0x00320E28 File Offset: 0x0031F028
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleReserve(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter reserver, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter requested_amount, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter actual_amount)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter(string.Concat(new string[]
			{
				"Reserve(",
				pickup_target.name,
				", ",
				requested_amount.name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				Pickupable pickupable = pickup_target.Get<Pickupable>(smi);
				GameObject gameObject = reserver.Get(smi);
				float val = requested_amount.Get(smi);
				float val2 = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(gameObject).GetTotalValue());
				float num = Math.Min(val, val2);
				num = Math.Min(num, pickupable.UnreservedAmount);
				if (num <= 0f)
				{
					pickupable.PrintReservations();
					global::Debug.LogError(string.Concat(new string[]
					{
						val2.ToString(),
						", ",
						val.ToString(),
						", ",
						pickupable.UnreservedAmount.ToString(),
						", ",
						num.ToString()
					}));
				}
				actual_amount.Set(num, smi, false);
				int num2 = pickupable.Reserve("ToggleReserve", gameObject, num);
				smi.dataTable[data_idx] = num2;
			});
			this.Exit(string.Concat(new string[]
			{
				"Unreserve(",
				pickup_target.name,
				", ",
				requested_amount.name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				int ticket = (int)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				Pickupable pickupable = pickup_target.Get<Pickupable>(smi);
				if (pickupable != null)
				{
					pickupable.Unreserve("ToggleReserve", ticket);
				}
			});
			return this;
		}

		// Token: 0x060083B1 RID: 33713 RVA: 0x00320F0C File Offset: 0x0031F10C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleWork(string work_type, Action<StateMachineInstanceType> callback, Func<StateMachineInstanceType, bool> validate_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StartWork(" + work_type + ")", delegate(StateMachineInstanceType smi)
			{
				if (validate_callback(smi))
				{
					callback(smi);
					return;
				}
				smi.GoTo(failure_state);
			});
			this.Update("Work(" + work_type + ")", delegate(StateMachineInstanceType smi, float dt)
			{
				if (validate_callback(smi))
				{
					WorkerBase.WorkResult workResult = state_target.Get<WorkerBase>(smi).Work(dt);
					if (workResult == WorkerBase.WorkResult.Success)
					{
						smi.GoTo(success_state);
						return;
					}
					if (workResult == WorkerBase.WorkResult.Failed)
					{
						smi.GoTo(failure_state);
						return;
					}
				}
				else
				{
					smi.GoTo(failure_state);
				}
			}, UpdateRate.SIM_33ms, false);
			this.Exit("StopWork()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<WorkerBase>(smi).StopWork();
			});
			return this;
		}

		// Token: 0x060083B2 RID: 33714 RVA: 0x00320FAC File Offset: 0x0031F1AC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleWork<WorkableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state, Func<StateMachineInstanceType, bool> is_valid_cb) where WorkableType : Workable
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork(typeof(WorkableType).Name, delegate(StateMachineInstanceType smi)
			{
				Workable workable = source_target.Get<WorkableType>(smi);
				state_target.Get<WorkerBase>(smi).StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => source_target.Get<WorkableType>(smi) != null && (is_valid_cb == null || is_valid_cb(smi)), success_state, failure_state);
			return this;
		}

		// Token: 0x060083B3 RID: 33715 RVA: 0x0032100C File Offset: 0x0031F20C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoEat(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Eat", delegate(StateMachineInstanceType smi)
			{
				Edible workable = source_target.Get<Edible>(smi);
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				float amount2 = amount.Get(smi);
				workerBase.StartWork(new Edible.EdibleStartWorkInfo(workable, amount2));
			}, (StateMachineInstanceType smi) => source_target.Get<Edible>(smi) != null, success_state, failure_state);
			return this;
		}

		// Token: 0x060083B4 RID: 33716 RVA: 0x00321064 File Offset: 0x0031F264
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoSleep(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter sleeper, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter bed, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Sleep", delegate(StateMachineInstanceType smi)
			{
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				Sleepable workable = bed.Get<Sleepable>(smi);
				workerBase.StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => bed.Get<Sleepable>(smi) != null, success_state, failure_state);
			return this;
		}

		// Token: 0x060083B5 RID: 33717 RVA: 0x003210B4 File Offset: 0x0031F2B4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoDelivery(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter worker_param, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter storage_param, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			this.ToggleWork("Pickup", delegate(StateMachineInstanceType smi)
			{
				WorkerBase workerBase = worker_param.Get<WorkerBase>(smi);
				Storage workable = storage_param.Get<Storage>(smi);
				workerBase.StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => storage_param.Get<Storage>(smi) != null, success_state, failure_state);
			return this;
		}

		// Token: 0x060083B6 RID: 33718 RVA: 0x00321100 File Offset: 0x0031F300
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoPickup(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter result_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Pickup", delegate(StateMachineInstanceType smi)
			{
				Pickupable pickupable = source_target.Get<Pickupable>(smi);
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				float amount2 = amount.Get(smi);
				workerBase.StartWork(new Pickupable.PickupableStartWorkInfo(pickupable, amount2, delegate(GameObject result)
				{
					result_target.Set(result, smi, false);
				}));
			}, (StateMachineInstanceType smi) => source_target.Get<Pickupable>(smi) != null || result_target.Get<Pickupable>(smi) != null, success_state, failure_state);
			return this;
		}

		// Token: 0x060083B7 RID: 33719 RVA: 0x00321160 File Offset: 0x0031F360
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleNotification(Func<StateMachineInstanceType, Notification> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = callback(smi);
				smi.dataTable[data_idx] = notification;
				state_target.AddOrGet<Notifier>(smi).Add(notification, "");
			});
			this.Exit("DisableNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = (Notification)smi.dataTable[data_idx];
				if (notification != null)
				{
					if (state_target != null)
					{
						Notifier notifier = state_target.Get<Notifier>(smi);
						if (notifier != null)
						{
							notifier.Remove(notification);
						}
					}
					smi.dataTable[data_idx] = null;
				}
			});
			return this;
		}

		// Token: 0x060083B8 RID: 33720 RVA: 0x003211C4 File Offset: 0x0031F3C4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoReport(ReportManager.ReportType reportType, Func<StateMachineInstanceType, float> callback, Func<StateMachineInstanceType, string> context_callback = null)
		{
			this.Enter("DoReport()", delegate(StateMachineInstanceType smi)
			{
				float value = callback(smi);
				string note = (context_callback != null) ? context_callback(smi) : null;
				ReportManager.Instance.ReportValue(reportType, value, note, null);
			});
			return this;
		}

		// Token: 0x060083B9 RID: 33721 RVA: 0x00321208 File Offset: 0x0031F408
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoNotification(Func<StateMachineInstanceType, Notification> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("DoNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = callback(smi);
				state_target.AddOrGet<Notifier>(smi).Add(notification, "");
			});
			return this;
		}

		// Token: 0x060083BA RID: 33722 RVA: 0x00321248 File Offset: 0x0031F448
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoTutorial(Tutorial.TutorialMessages msg)
		{
			this.Enter("DoTutorial()", delegate(StateMachineInstanceType smi)
			{
				Tutorial.Instance.TutorialMessage(msg, true);
			});
			return this;
		}

		// Token: 0x060083BB RID: 33723 RVA: 0x0032127C File Offset: 0x0031F47C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleScheduleCallback(string name, Func<StateMachineInstanceType, float> time_cb, Action<StateMachineInstanceType> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			Action<object> <>9__2;
			this.Enter("AddScheduledCallback(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				GameScheduler instance = GameScheduler.Instance;
				string name2 = name;
				float time = time_cb(smi);
				Action<object> callback2;
				if ((callback2 = <>9__2) == null)
				{
					callback2 = (<>9__2 = delegate(object smi_data)
					{
						callback((StateMachineInstanceType)((object)smi_data));
					});
				}
				SchedulerHandle schedulerHandle = instance.Schedule(name2, time, callback2, smi, null);
				DebugUtil.Assert(smi.dataTable[data_idx] == null);
				smi.dataTable[data_idx] = schedulerHandle;
			});
			this.Exit("RemoveScheduledCallback(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					SchedulerHandle schedulerHandle = (SchedulerHandle)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					schedulerHandle.ClearScheduler();
				}
			});
			return this;
		}

		// Token: 0x060083BC RID: 33724 RVA: 0x00321304 File Offset: 0x0031F504
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleGoTo(Func<StateMachineInstanceType, float> time_cb, StateMachine.BaseState state)
		{
			this.Enter("ScheduleGoTo(" + state.name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleGoTo(time_cb(smi), state);
			});
			return this;
		}

		// Token: 0x060083BD RID: 33725 RVA: 0x00321354 File Offset: 0x0031F554
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleGoTo(float time, StateMachine.BaseState state)
		{
			string[] array = new string[5];
			array[0] = "ScheduleGoTo(";
			array[1] = time.ToString();
			array[2] = ", ";
			int num = 3;
			StateMachine.BaseState state2 = state;
			array[num] = ((state2 != null) ? state2.name : null);
			array[4] = ")";
			this.Enter(string.Concat(array), delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleGoTo(time, state);
			});
			return this;
		}

		// Token: 0x060083BE RID: 33726 RVA: 0x003213D0 File Offset: 0x0031F5D0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleAction(string name, Func<StateMachineInstanceType, float> time_cb, Action<StateMachineInstanceType> action)
		{
			this.Enter("ScheduleAction(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.Schedule(time_cb(smi), delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		// Token: 0x060083BF RID: 33727 RVA: 0x00321418 File Offset: 0x0031F618
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleAction(string name, float time, Action<StateMachineInstanceType> action)
		{
			this.Enter(string.Concat(new string[]
			{
				"ScheduleAction(",
				time.ToString(),
				", ",
				name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				smi.Schedule(time, delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		// Token: 0x060083C0 RID: 33728 RVA: 0x00321484 File Offset: 0x0031F684
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleActionNextFrame(string name, Action<StateMachineInstanceType> action)
		{
			this.Enter("ScheduleActionNextFrame(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleNextFrame(delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		// Token: 0x060083C1 RID: 33729 RVA: 0x003214C4 File Offset: 0x0031F6C4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.EventHandler(evt, global_event_system_callback, delegate(StateMachineInstanceType smi, object d)
			{
				callback(smi);
			});
		}

		// Token: 0x060083C2 RID: 33730 RVA: 0x003214F4 File Offset: 0x0031F6F4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback)
		{
			if (this.events == null)
			{
				this.events = new List<StateEvent>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent(evt, callback, target, global_event_system_callback);
			this.events.Add(item);
			return this;
		}

		// Token: 0x060083C3 RID: 33731 RVA: 0x00321534 File Offset: 0x0031F734
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.EventHandler(evt, delegate(StateMachineInstanceType smi, object d)
			{
				callback(smi);
			});
		}

		// Token: 0x060083C4 RID: 33732 RVA: 0x00321561 File Offset: 0x0031F761
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback)
		{
			this.EventHandler(evt, null, callback);
			return this;
		}

		// Token: 0x060083C5 RID: 33733 RVA: 0x00321570 File Offset: 0x0031F770
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandlerTransition(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, Func<StateMachineInstanceType, object, bool> callback)
		{
			return this.EventHandler(evt, delegate(StateMachineInstanceType smi, object d)
			{
				if (callback(smi, d))
				{
					smi.GoTo(state);
				}
			});
		}

		// Token: 0x060083C6 RID: 33734 RVA: 0x003215A4 File Offset: 0x0031F7A4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandlerTransition(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, Func<StateMachineInstanceType, object, bool> callback)
		{
			return this.EventHandler(evt, global_event_system_callback, delegate(StateMachineInstanceType smi, object d)
			{
				if (callback(smi, d))
				{
					smi.GoTo(state);
				}
			});
		}

		// Token: 0x060083C7 RID: 33735 RVA: 0x003215DC File Offset: 0x0031F7DC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ParamTransition<ParameterType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Transition item = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Transition(this.transitions.Count, parameter, state, callback);
			this.transitions.Add(item);
			return this;
		}

		// Token: 0x060083C8 RID: 33736 RVA: 0x00321630 File Offset: 0x0031F830
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnSignal(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal signal, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, Func<StateMachineInstanceType, bool> callback)
		{
			this.ParamTransition<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>(signal, state, (StateMachineInstanceType smi, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter p) => callback(smi));
			return this;
		}

		// Token: 0x060083C9 RID: 33737 RVA: 0x00321660 File Offset: 0x0031F860
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnSignal(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal signal, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			this.ParamTransition<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>(signal, state, null);
			return this;
		}

		// Token: 0x060083CA RID: 33738 RVA: 0x00321670 File Offset: 0x0031F870
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EnterTransition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition)
		{
			string str = "(Stop)";
			if (state != null)
			{
				str = state.name;
			}
			this.Enter("Transition(" + str + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition(smi))
				{
					smi.GoTo(state);
				}
			});
			return this;
		}

		// Token: 0x060083CB RID: 33739 RVA: 0x003216D0 File Offset: 0x0031F8D0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Transition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			string str = "(Stop)";
			if (state != null)
			{
				str = state.name;
			}
			this.Enter("Transition(" + str + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition(smi))
				{
					smi.GoTo(state);
				}
			});
			this.FastUpdate("Transition(" + str + ")", new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.TransitionUpdater(condition, state), update_rate, false);
			return this;
		}

		// Token: 0x060083CC RID: 33740 RVA: 0x00321759 File Offset: 0x0031F959
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DefaultState(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State default_state)
		{
			this.defaultState = default_state;
			return this;
		}

		// Token: 0x060083CD RID: 33741 RVA: 0x00321764 File Offset: 0x0031F964
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State GoTo(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self", null);
			string str = "(null)";
			if (state != null)
			{
				str = state.name;
			}
			this.Update("GoTo(" + str + ")", delegate(StateMachineInstanceType smi, float dt)
			{
				smi.GoTo(state);
			}, UpdateRate.SIM_200ms, false);
			return this;
		}

		// Token: 0x060083CE RID: 33742 RVA: 0x003217D8 File Offset: 0x0031F9D8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State StopMoving()
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			this.Enter("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		// Token: 0x060083CF RID: 33743 RVA: 0x00321810 File Offset: 0x0031FA10
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStationaryIdling()
		{
			this.GetStateTarget();
			this.ToggleTag(GameTags.StationaryIdling);
			return this;
		}

		// Token: 0x060083D0 RID: 33744 RVA: 0x00321828 File Offset: 0x0031FA28
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnBehaviourComplete(Tag behaviour, Action<StateMachineInstanceType> cb)
		{
			this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object d)
			{
				if ((Tag)d == behaviour)
				{
					cb(smi);
				}
			});
			return this;
		}

		// Token: 0x060083D1 RID: 33745 RVA: 0x00321862 File Offset: 0x0031FA62
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo(Func<StateMachineInstanceType, int> cell_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state = null, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, bool update_cell = false)
		{
			return this.MoveTo(cell_callback, null, success_state, fail_state, update_cell);
		}

		// Token: 0x060083D2 RID: 33746 RVA: 0x00321870 File Offset: 0x0031FA70
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo(Func<StateMachineInstanceType, int> cell_callback, Func<StateMachineInstanceType, CellOffset[]> cell_offsets_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state = null, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, bool update_cell = false)
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			CellOffset[] default_offset = new CellOffset[1];
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("MoveTo()", delegate(StateMachineInstanceType smi)
			{
				int cell = cell_callback(smi);
				Navigator navigator = state_target.Get<Navigator>(smi);
				CellOffset[] offsets = default_offset;
				if (cell_offsets_callback != null)
				{
					offsets = cell_offsets_callback(smi);
				}
				navigator.GoTo(cell, offsets);
			});
			if (update_cell)
			{
				this.Update("MoveTo()", delegate(StateMachineInstanceType smi, float dt)
				{
					int cell = cell_callback(smi);
					state_target.Get<Navigator>(smi).UpdateTarget(cell);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetComponent<Navigator>().Stop(false, true);
			});
			return this;
		}

		// Token: 0x060083D3 RID: 33747 RVA: 0x00321918 File Offset: 0x0031FB18
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo<ApproachableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, CellOffset[] override_offsets = null, NavTactic tactic = null) where ApproachableType : IApproachable
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			CellOffset[] offsets;
			this.Enter("MoveTo(" + move_parameter.name + ")", delegate(StateMachineInstanceType smi)
			{
				offsets = override_offsets;
				IApproachable approachable = move_parameter.Get<ApproachableType>(smi);
				KMonoBehaviour kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
				if (kmonoBehaviour == null)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator component = state_target.Get(smi).GetComponent<Navigator>();
				if (offsets == null)
				{
					offsets = approachable.GetOffsets();
				}
				component.GoTo(kmonoBehaviour, offsets, tactic);
			});
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		// Token: 0x060083D4 RID: 33748 RVA: 0x003219BC File Offset: 0x0031FBBC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo<ApproachableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, Func<StateMachineInstanceType, CellOffset[]> override_offsets, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, NavTactic tactic = null) where ApproachableType : IApproachable
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			CellOffset[] offsets;
			this.Enter("MoveTo(" + move_parameter.name + ")", delegate(StateMachineInstanceType smi)
			{
				offsets = override_offsets(smi);
				IApproachable approachable = move_parameter.Get<ApproachableType>(smi);
				KMonoBehaviour kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
				if (kmonoBehaviour == null)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator component = state_target.Get(smi).GetComponent<Navigator>();
				if (offsets == null)
				{
					offsets = approachable.GetOffsets();
				}
				component.GoTo(kmonoBehaviour, offsets, tactic);
			});
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		// Token: 0x060083D5 RID: 33749 RVA: 0x00321A60 File Offset: 0x0031FC60
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Face(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter face_target, float x_offset = 0f)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("Face", delegate(StateMachineInstanceType smi)
			{
				if (face_target != null)
				{
					IApproachable approachable = face_target.Get<IApproachable>(smi);
					if (approachable != null)
					{
						float target_x = approachable.transform.GetPosition().x + x_offset;
						state_target.Get<Facing>(smi).Face(target_x);
					}
				}
			});
			return this;
		}

		// Token: 0x060083D6 RID: 33750 RVA: 0x00321AA8 File Offset: 0x0031FCA8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Tag[] tags, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData(tags.ToString(), this, state, this.transitions.Count, tags, on_remove, this.GetStateTarget(), null);
			this.transitions.Add(item);
			return this;
		}

		// Token: 0x060083D7 RID: 33751 RVA: 0x00321B0C File Offset: 0x0031FD0C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Func<StateMachineInstanceType, Tag[]> tags_cb, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData("DynamicTransition", this, state, this.transitions.Count, null, on_remove, this.GetStateTarget(), tags_cb);
			this.transitions.Add(item);
			return this;
		}

		// Token: 0x060083D8 RID: 33752 RVA: 0x00321B6C File Offset: 0x0031FD6C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Tag tag, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			return this.TagTransition(new Tag[]
			{
				tag
			}, state, on_remove);
		}

		// Token: 0x060083D9 RID: 33753 RVA: 0x00321B84 File Offset: 0x0031FD84
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventTransition(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition = null)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EventTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EventTransitionData(this, state, this.transitions.Count, evt, global_event_system_callback, condition, target);
			this.transitions.Add(item);
			return this;
		}

		// Token: 0x060083DA RID: 33754 RVA: 0x00321BE2 File Offset: 0x0031FDE2
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventTransition(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition = null)
		{
			return this.EventTransition(evt, null, state, condition);
		}

		// Token: 0x060083DB RID: 33755 RVA: 0x00321BEE File Offset: 0x0031FDEE
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ReturnSuccess()
		{
			this.Enter("ReturnSuccess()", delegate(StateMachineInstanceType smi)
			{
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("GameStateMachine.ReturnSuccess()");
			});
			return this;
		}

		// Token: 0x060083DC RID: 33756 RVA: 0x00321C1C File Offset: 0x0031FE1C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ReturnFailure()
		{
			this.Enter("ReturnFailure()", delegate(StateMachineInstanceType smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("GameStateMachine.ReturnFailure()");
			});
			return this;
		}

		// Token: 0x060083DD RID: 33757 RVA: 0x00321C4C File Offset: 0x0031FE4C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(string name, string tooltip, string icon = "", StatusItem.IconType icon_type = StatusItem.IconType.Info, NotificationType notification_type = NotificationType.Neutral, bool allow_multiples = false, HashedString render_overlay = default(HashedString), int status_overlays = 129022, Func<string, StateMachineInstanceType, string> resolve_string_callback = null, Func<string, StateMachineInstanceType, string> resolve_tooltip_callback = null, StatusItemCategory category = null)
		{
			StatusItem statusItem = new StatusItem(this.longName, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null);
			if (resolve_string_callback != null)
			{
				statusItem.resolveStringCallback = ((string str, object obj) => resolve_string_callback(str, (StateMachineInstanceType)((object)obj)));
			}
			if (resolve_tooltip_callback != null)
			{
				statusItem.resolveTooltipCallback = ((string str, object obj) => resolve_tooltip_callback(str, (StateMachineInstanceType)((object)obj)));
			}
			this.ToggleStatusItem(statusItem, (StateMachineInstanceType smi) => smi, category);
			return this;
		}

		// Token: 0x060083DE RID: 33758 RVA: 0x00321CE8 File Offset: 0x0031FEE8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			this.Enter(string.Concat(new string[]
			{
				"PlayAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x060083DF RID: 33759 RVA: 0x00321D6C File Offset: 0x0031FF6C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(Func<StateMachineInstanceType, string> anim_cb, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnim(" + mode.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim_cb(smi), mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x060083E0 RID: 33760 RVA: 0x00321DD0 File Offset: 0x0031FFD0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim, KAnim.PlayMode mode)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"PlayAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x060083E1 RID: 33761 RVA: 0x00321E54 File Offset: 0x00320054
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim, KAnim.PlayMode mode, Func<StateMachineInstanceType, string> suffix_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"PlayAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x060083E2 RID: 33762 RVA: 0x00321EDC File Offset: 0x003200DC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State QueueAnim(Func<StateMachineInstanceType, string> anim_cb, bool loop = false, Func<StateMachineInstanceType, string> suffix_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			if (loop)
			{
				mode = KAnim.PlayMode.Loop;
			}
			this.Enter("QueueAnim(" + mode.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Queue(anim_cb(smi) + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x060083E3 RID: 33763 RVA: 0x00321F50 File Offset: 0x00320150
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State QueueAnim(string anim, bool loop = false, Func<StateMachineInstanceType, string> suffix_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			if (loop)
			{
				mode = KAnim.PlayMode.Loop;
			}
			this.Enter(string.Concat(new string[]
			{
				"QueueAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Queue(anim + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x060083E4 RID: 33764 RVA: 0x00321FE4 File Offset: 0x003201E4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnims(Func<StateMachineInstanceType, HashedString[]> anims_callback, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnims", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					HashedString[] anim_names = anims_callback(smi);
					kanimControllerBase.Play(anim_names, mode);
				}
			});
			return this;
		}

		// Token: 0x060083E5 RID: 33765 RVA: 0x0032202C File Offset: 0x0032022C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnims(Func<StateMachineInstanceType, HashedString[]> anims_callback, Func<StateMachineInstanceType, KAnim.PlayMode> mode_cb)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnims", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					HashedString[] anim_names = anims_callback(smi);
					KAnim.PlayMode mode = mode_cb(smi);
					kanimControllerBase.Play(anim_names, mode);
				}
			});
			return this;
		}

		// Token: 0x060083E6 RID: 33766 RVA: 0x00322074 File Offset: 0x00320274
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnAnimQueueComplete(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("CheckIfAnimQueueIsEmpty", delegate(StateMachineInstanceType smi)
			{
				if (state_target.Get<KBatchedAnimController>(smi).IsStopped())
				{
					smi.GoTo(state);
				}
			});
			return this.EventTransition(GameHashes.AnimQueueComplete, state, null);
		}

		// Token: 0x060083E7 RID: 33767 RVA: 0x003220C4 File Offset: 0x003202C4
		internal void EventHandler()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04006398 RID: 25496
		[StateMachine.DoNotAutoCreate]
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

		// Token: 0x02002408 RID: 9224
		private class TransitionUpdater : UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater
		{
			// Token: 0x0600B8B0 RID: 47280 RVA: 0x003CFA9A File Offset: 0x003CDC9A
			public TransitionUpdater(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
			{
				this.condition = condition;
				this.state = state;
			}

			// Token: 0x0600B8B1 RID: 47281 RVA: 0x003CFAB0 File Offset: 0x003CDCB0
			public void Update(StateMachineInstanceType smi, float dt)
			{
				if (this.condition(smi))
				{
					smi.GoTo(this.state);
				}
			}

			// Token: 0x0400A0D7 RID: 41175
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

			// Token: 0x0400A0D8 RID: 41176
			private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state;
		}
	}

	// Token: 0x0200127E RID: 4734
	public class GameEvent : StateEvent
	{
		// Token: 0x060083EB RID: 33771 RVA: 0x003220E5 File Offset: 0x003202E5
		public GameEvent(GameHashes id, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback) : base(id.ToString())
		{
			this.id = id;
			this.target = target;
			this.callback = callback;
			this.globalEventSystemCallback = global_event_system_callback;
		}

		// Token: 0x060083EC RID: 33772 RVA: 0x00322118 File Offset: 0x00320318
		public override StateEvent.Context Subscribe(StateMachine.Instance smi)
		{
			StateEvent.Context result = base.Subscribe(smi);
			StateMachineInstanceType cast_smi = (StateMachineInstanceType)((object)smi);
			Action<object> handler = delegate(object d)
			{
				if (StateMachine.Instance.error)
				{
					return;
				}
				this.callback(cast_smi, d);
			};
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(cast_smi);
				result.data = kmonoBehaviour.Subscribe((int)this.id, handler);
			}
			else
			{
				result.data = this.target.Get(cast_smi).Subscribe((int)this.id, handler);
			}
			return result;
		}

		// Token: 0x060083ED RID: 33773 RVA: 0x003221A8 File Offset: 0x003203A8
		public override void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = (StateMachineInstanceType)((object)smi);
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(stateMachineInstanceType);
				if (kmonoBehaviour != null)
				{
					kmonoBehaviour.Unsubscribe(context.data);
					return;
				}
			}
			else
			{
				GameObject gameObject = this.target.Get(stateMachineInstanceType);
				if (gameObject != null)
				{
					gameObject.Unsubscribe(context.data);
				}
			}
		}

		// Token: 0x04006399 RID: 25497
		private GameHashes id;

		// Token: 0x0400639A RID: 25498
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		// Token: 0x0400639B RID: 25499
		private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback;

		// Token: 0x0400639C RID: 25500
		private Func<StateMachineInstanceType, KMonoBehaviour> globalEventSystemCallback;

		// Token: 0x02002474 RID: 9332
		// (Invoke) Token: 0x0600B9D8 RID: 47576
		public delegate void Callback(StateMachineInstanceType smi, object callback_data);
	}

	// Token: 0x0200127F RID: 4735
	public class ApproachSubState<ApproachableType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State where ApproachableType : IApproachable
	{
		// Token: 0x060083EE RID: 33774 RVA: 0x00322209 File Offset: 0x00320409
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter mover, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null, CellOffset[] override_offsets = null, NavTactic tactic = null)
		{
			base.root.Target(mover).OnTargetLost(move_target, failure_state).MoveTo<ApproachableType>(move_target, success_state, failure_state, override_offsets, (tactic == null) ? NavigationTactics.ReduceTravelDistance : tactic);
			return this;
		}

		// Token: 0x060083EF RID: 33775 RVA: 0x00322239 File Offset: 0x00320439
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter mover, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_target, Func<StateMachineInstanceType, CellOffset[]> override_offsets, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null, NavTactic tactic = null)
		{
			base.root.Target(mover).OnTargetLost(move_target, failure_state).MoveTo<ApproachableType>(move_target, success_state, override_offsets, failure_state, (tactic == null) ? NavigationTactics.ReduceTravelDistance : tactic);
			return this;
		}
	}

	// Token: 0x02001280 RID: 4736
	public class DebugGoToSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x060083F1 RID: 33777 RVA: 0x00322274 File Offset: 0x00320474
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State exit_state)
		{
			base.root.Enter("GoToCursor", delegate(StateMachineInstanceType smi)
			{
				this.GoToCursor(smi);
			}).EventHandler(GameHashes.DebugGoTo, (StateMachineInstanceType smi) => Game.Instance, delegate(StateMachineInstanceType smi)
			{
				this.GoToCursor(smi);
			}).EventTransition(GameHashes.DestinationReached, exit_state, null).EventTransition(GameHashes.NavigationFailed, exit_state, null);
			return this;
		}

		// Token: 0x060083F2 RID: 33778 RVA: 0x003222EC File Offset: 0x003204EC
		public void GoToCursor(StateMachineInstanceType smi)
		{
			smi.GetComponent<Navigator>().GoTo(Grid.PosToCell(DebugHandler.GetMousePos()), new CellOffset[1]);
		}
	}

	// Token: 0x02001281 RID: 4737
	public class DropSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x060083F6 RID: 33782 RVA: 0x0032232C File Offset: 0x0032052C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter carrier, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter item, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter drop_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null)
		{
			base.root.Target(carrier).Enter("Drop", delegate(StateMachineInstanceType smi)
			{
				Storage storage = carrier.Get<Storage>(smi);
				GameObject gameObject = item.Get(smi);
				storage.Drop(gameObject, true);
				int cell = Grid.CellAbove(Grid.PosToCell(drop_target.Get<Transform>(smi).GetPosition()));
				gameObject.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
				smi.GoTo(success_state);
			});
			return this;
		}
	}

	// Token: 0x02001282 RID: 4738
	public class FetchSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x060083F8 RID: 33784 RVA: 0x00322390 File Offset: 0x00320590
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter fetcher, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_source, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_chunk, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter requested_amount, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter actual_amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null)
		{
			base.Target(fetcher);
			base.root.DefaultState(this.approach).ToggleReserve(fetcher, pickup_source, requested_amount, actual_amount);
			this.approach.InitializeStates(fetcher, pickup_source, this.pickup, null, null, NavigationTactics.ReduceTravelDistance).OnTargetLost(pickup_source, failure_state);
			this.pickup.DoPickup(pickup_source, pickup_chunk, actual_amount, success_state, failure_state).EventTransition(GameHashes.AbortWork, failure_state, null);
			return this;
		}

		// Token: 0x0400639D RID: 25501
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ApproachSubState<Pickupable> approach;

		// Token: 0x0400639E RID: 25502
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pickup;

		// Token: 0x0400639F RID: 25503
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success;
	}

	// Token: 0x02001283 RID: 4739
	public class HungrySubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x060083FA RID: 33786 RVA: 0x00322410 File Offset: 0x00320610
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, StatusItem status_item)
		{
			base.Target(target);
			base.root.DefaultState(this.satisfied);
			this.satisfied.EventTransition(GameHashes.AddUrge, this.hungry, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.HungrySubState.IsHungry(smi));
			this.hungry.EventTransition(GameHashes.RemoveUrge, this.satisfied, (StateMachineInstanceType smi) => !GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.HungrySubState.IsHungry(smi)).ToggleStatusItem(status_item, null);
			return this;
		}

		// Token: 0x060083FB RID: 33787 RVA: 0x003224AB File Offset: 0x003206AB
		private static bool IsHungry(StateMachineInstanceType smi)
		{
			return smi.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
		}

		// Token: 0x040063A0 RID: 25504
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State satisfied;

		// Token: 0x040063A1 RID: 25505
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State hungry;
	}

	// Token: 0x02001284 RID: 4740
	public class PlantAliveSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x060083FD RID: 33789 RVA: 0x003224D4 File Offset: 0x003206D4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter plant, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State death_state = null)
		{
			base.root.Target(plant).TagTransition(GameTags.Uprooted, death_state, false).EventTransition(GameHashes.TooColdFatal, death_state, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.PlantAliveSubState.isLethalTemperature(plant.Get(smi))).EventTransition(GameHashes.TooHotFatal, death_state, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.PlantAliveSubState.isLethalTemperature(plant.Get(smi))).EventTransition(GameHashes.Drowned, death_state, null);
			return this;
		}

		// Token: 0x060083FE RID: 33790 RVA: 0x00322548 File Offset: 0x00320748
		public bool ForceUpdateStatus(GameObject plant)
		{
			TemperatureVulnerable component = plant.GetComponent<TemperatureVulnerable>();
			EntombVulnerable component2 = plant.GetComponent<EntombVulnerable>();
			PressureVulnerable component3 = plant.GetComponent<PressureVulnerable>();
			return (component == null || !component.IsLethal) && (component2 == null || !component2.GetEntombed) && (component3 == null || !component3.IsLethal);
		}

		// Token: 0x060083FF RID: 33791 RVA: 0x003225A4 File Offset: 0x003207A4
		private static bool isLethalTemperature(GameObject plant)
		{
			TemperatureVulnerable component = plant.GetComponent<TemperatureVulnerable>();
			return !(component == null) && (component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold || component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalHot);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ImGuiNET;
using KSerialization;
using UnityEngine;

// Token: 0x020004CA RID: 1226
public class StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine where StateMachineInstanceType : StateMachine.Instance where MasterType : IStateMachineTarget
{
	// Token: 0x06001A5F RID: 6751 RVA: 0x0008B58C File Offset: 0x0008978C
	public override string[] GetStateNames()
	{
		List<string> list = new List<string>();
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			list.Add(state.name);
		}
		return list.ToArray();
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x0008B5F0 File Offset: 0x000897F0
	public void Target(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
	{
		this.stateTarget = target;
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x0008B5FC File Offset: 0x000897FC
	public void BindState(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, string state_name)
	{
		if (parent_state != null)
		{
			state_name = parent_state.name + "." + state_name;
		}
		state.name = state_name;
		state.longName = this.name + "." + state_name;
		state.debugPushName = "PuS: " + state.longName;
		state.debugPopName = "PoS: " + state.longName;
		state.debugExecuteName = "EA: " + state.longName;
		List<StateMachine.BaseState> list;
		if (parent_state != null)
		{
			list = new List<StateMachine.BaseState>(parent_state.branch);
		}
		else
		{
			list = new List<StateMachine.BaseState>();
		}
		list.Add(state);
		state.parent = parent_state;
		state.branch = list.ToArray();
		this.maxDepth = Math.Max(state.branch.Length, this.maxDepth);
		this.states.Add(state);
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x0008B6D8 File Offset: 0x000898D8
	public void BindStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)fieldInfo.GetValue(state_machine);
				if (state != parent_state)
				{
					string name = fieldInfo.Name;
					this.BindState(parent_state, state, name);
					this.BindStates(state, state);
				}
			}
		}
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x0008B747 File Offset: 0x00089947
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x0008B750 File Offset: 0x00089950
	public override void BindStates()
	{
		this.BindStates(null, this);
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x0008B75A File Offset: 0x0008995A
	public override Type GetStateMachineInstanceType()
	{
		return typeof(StateMachineInstanceType);
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x0008B768 File Offset: 0x00089968
	public override StateMachine.BaseState GetState(string state_name)
	{
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			if (state.name == state_name)
			{
				return state;
			}
		}
		return null;
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x0008B7CC File Offset: 0x000899CC
	public override void FreeResources()
	{
		for (int i = 0; i < this.states.Count; i++)
		{
			this.states[i].FreeResources();
		}
		this.states.Clear();
		base.FreeResources();
	}

	// Token: 0x04000F08 RID: 3848
	private List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State> states = new List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State>();

	// Token: 0x04000F09 RID: 3849
	public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter masterTarget;

	// Token: 0x04000F0A RID: 3850
	[StateMachine.DoNotAutoCreate]
	protected StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

	// Token: 0x02001297 RID: 4759
	public class GenericInstance : StateMachine.Instance
	{
		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06008455 RID: 33877 RVA: 0x00322C5B File Offset: 0x00320E5B
		// (set) Token: 0x06008456 RID: 33878 RVA: 0x00322C63 File Offset: 0x00320E63
		public StateMachineType sm { get; private set; }

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06008457 RID: 33879 RVA: 0x00322C6C File Offset: 0x00320E6C
		protected StateMachineInstanceType smi
		{
			get
			{
				return (StateMachineInstanceType)((object)this);
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06008458 RID: 33880 RVA: 0x00322C74 File Offset: 0x00320E74
		// (set) Token: 0x06008459 RID: 33881 RVA: 0x00322C7C File Offset: 0x00320E7C
		public MasterType master { get; private set; }

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x0600845A RID: 33882 RVA: 0x00322C85 File Offset: 0x00320E85
		// (set) Token: 0x0600845B RID: 33883 RVA: 0x00322C8D File Offset: 0x00320E8D
		public DefType def { get; set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x0600845C RID: 33884 RVA: 0x00322C96 File Offset: 0x00320E96
		public bool isMasterNull
		{
			get
			{
				return this.internalSm.masterTarget.IsNull((StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600845D RID: 33885 RVA: 0x00322CAE File Offset: 0x00320EAE
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> internalSm
		{
			get
			{
				return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>)((object)this.sm);
			}
		}

		// Token: 0x0600845E RID: 33886 RVA: 0x00322CC0 File Offset: 0x00320EC0
		protected virtual void OnCleanUp()
		{
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600845F RID: 33887 RVA: 0x00322CC2 File Offset: 0x00320EC2
		public override float timeinstate
		{
			get
			{
				return Time.time - this.stateEnterTime;
			}
		}

		// Token: 0x06008460 RID: 33888 RVA: 0x00322CD0 File Offset: 0x00320ED0
		public override void FreeResources()
		{
			this.updateHandle.FreeResources();
			this.updateHandle = default(SchedulerHandle);
			this.controller = null;
			if (this.gotoStack != null)
			{
				this.gotoStack.Clear();
			}
			this.gotoStack = null;
			if (this.transitionStack != null)
			{
				this.transitionStack.Clear();
			}
			this.transitionStack = null;
			if (this.currentSchedulerGroup != null)
			{
				this.currentSchedulerGroup.FreeResources();
			}
			this.currentSchedulerGroup = null;
			if (this.stateStack != null)
			{
				for (int i = 0; i < this.stateStack.Length; i++)
				{
					if (this.stateStack[i].schedulerGroup != null)
					{
						this.stateStack[i].schedulerGroup.FreeResources();
					}
				}
			}
			this.stateStack = null;
			base.FreeResources();
		}

		// Token: 0x06008461 RID: 33889 RVA: 0x00322D9C File Offset: 0x00320F9C
		public GenericInstance(MasterType master) : base((StateMachine)((object)Singleton<StateMachineManager>.Instance.CreateStateMachine<StateMachineType>()), master)
		{
			this.master = master;
			this.stateStack = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[this.stateMachine.GetMaxDepth()];
			for (int i = 0; i < this.stateStack.Length; i++)
			{
				this.stateStack[i].schedulerGroup = Singleton<StateMachineManager>.Instance.CreateSchedulerGroup();
			}
			this.sm = (StateMachineType)((object)this.stateMachine);
			this.dataTable = new object[base.GetStateMachine().dataTableSize];
			this.updateTable = new StateMachine.Instance.UpdateTableEntry[base.GetStateMachine().updateTableSize];
			this.controller = master.GetComponent<StateMachineController>();
			if (this.controller == null)
			{
				this.controller = master.gameObject.AddComponent<StateMachineController>();
			}
			this.internalSm.masterTarget.Set(master.gameObject, this.smi, false);
			this.controller.AddStateMachineInstance(this);
		}

		// Token: 0x06008462 RID: 33890 RVA: 0x00322ED8 File Offset: 0x003210D8
		public override IStateMachineTarget GetMaster()
		{
			return this.master;
		}

		// Token: 0x06008463 RID: 33891 RVA: 0x00322EE8 File Offset: 0x003210E8
		private void PushEvent(StateEvent evt)
		{
			StateEvent.Context item = evt.Subscribe(this);
			this.subscribedEvents.Push(item);
		}

		// Token: 0x06008464 RID: 33892 RVA: 0x00322F0C File Offset: 0x0032110C
		private void PopEvent()
		{
			StateEvent.Context context = this.subscribedEvents.Pop();
			context.stateEvent.Unsubscribe(this, context);
		}

		// Token: 0x06008465 RID: 33893 RVA: 0x00322F34 File Offset: 0x00321134
		private bool TryEvaluateTransitions(StateMachine.BaseState state, int goto_id)
		{
			if (state.transitions == null)
			{
				return true;
			}
			bool result = true;
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition baseTransition = state.transitions[i];
				if (goto_id != this.gotoId)
				{
					result = false;
					break;
				}
				baseTransition.Evaluate(this.smi);
			}
			return result;
		}

		// Token: 0x06008466 RID: 33894 RVA: 0x00322F90 File Offset: 0x00321190
		private void PushTransitions(StateMachine.BaseState state)
		{
			if (state.transitions == null)
			{
				return;
			}
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition transition = state.transitions[i];
				this.PushTransition(transition);
			}
		}

		// Token: 0x06008467 RID: 33895 RVA: 0x00322FD0 File Offset: 0x003211D0
		private void PushTransition(StateMachine.BaseTransition transition)
		{
			StateMachine.BaseTransition.Context item = transition.Register(this.smi);
			this.transitionStack.Push(item);
		}

		// Token: 0x06008468 RID: 33896 RVA: 0x00322FFC File Offset: 0x003211FC
		private void PopTransition(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			StateMachine.BaseTransition.Context context = this.transitionStack.Pop();
			state.transitions[context.idx].Unregister(this.smi, context);
		}

		// Token: 0x06008469 RID: 33897 RVA: 0x00323038 File Offset: 0x00321238
		private void PushState(StateMachine.BaseState state)
		{
			int num = this.gotoId;
			this.currentActionIdx = -1;
			if (state.events != null)
			{
				foreach (StateEvent evt in state.events)
				{
					this.PushEvent(evt);
				}
			}
			this.PushTransitions(state);
			if (state.updateActions != null)
			{
				for (int i = 0; i < state.updateActions.Count; i++)
				{
					StateMachine.UpdateAction updateAction = state.updateActions[i];
					int updateTableIdx = updateAction.updateTableIdx;
					int nextBucketIdx = updateAction.nextBucketIdx;
					updateAction.nextBucketIdx = (updateAction.nextBucketIdx + 1) % updateAction.buckets.Length;
					UpdateBucketWithUpdater<StateMachineInstanceType> updateBucketWithUpdater = (UpdateBucketWithUpdater<StateMachineInstanceType>)updateAction.buckets[nextBucketIdx];
					this.smi.updateTable[updateTableIdx].bucket = updateBucketWithUpdater;
					this.smi.updateTable[updateTableIdx].handle = updateBucketWithUpdater.Add(this.smi, Singleton<StateMachineUpdater>.Instance.GetFrameTime(updateAction.updateRate, updateBucketWithUpdater.frame), (UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater)updateAction.updater);
					state.updateActions[i] = updateAction;
				}
			}
			this.stateEnterTime = Time.time;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int stackSize = this.stackSize;
			this.stackSize = stackSize + 1;
			array[stackSize].state = state;
			this.currentSchedulerGroup = this.stateStack[this.stackSize - 1].schedulerGroup;
			if (!this.TryEvaluateTransitions(state, num))
			{
				return;
			}
			if (num != this.gotoId)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
			int num2 = this.gotoId;
		}

		// Token: 0x0600846A RID: 33898 RVA: 0x00323214 File Offset: 0x00321414
		private void ExecuteActions(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, List<StateMachine.Action> actions)
		{
			if (actions == null)
			{
				return;
			}
			int num = this.gotoId;
			this.currentActionIdx++;
			while (this.currentActionIdx < actions.Count && num == this.gotoId)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback)actions[this.currentActionIdx].callback;
				try
				{
					callback(this.smi);
				}
				catch (Exception e)
				{
					if (!StateMachine.Instance.error)
					{
						base.Error();
						string text = "(NULL).";
						IStateMachineTarget master = this.GetMaster();
						if (!master.isNull)
						{
							KPrefabID component = master.GetComponent<KPrefabID>();
							if (component != null)
							{
								text = "(" + component.PrefabTag.ToString() + ").";
							}
							else
							{
								text = "(" + base.gameObject.name + ").";
							}
						}
						string text2 = string.Concat(new string[]
						{
							"Exception in: ",
							text,
							this.stateMachine.ToString(),
							".",
							state.name,
							"."
						});
						if (this.currentActionIdx > 0 && this.currentActionIdx < actions.Count)
						{
							text2 += actions[this.currentActionIdx].name;
						}
						DebugUtil.LogException(this.controller, text2, e);
					}
				}
				this.currentActionIdx++;
			}
			this.currentActionIdx = 2147483646;
		}

		// Token: 0x0600846B RID: 33899 RVA: 0x003233A8 File Offset: 0x003215A8
		private void PopState()
		{
			this.currentActionIdx = -1;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int num = this.stackSize - 1;
			this.stackSize = num;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry stackEntry = array[num];
			StateMachine.BaseState state = stackEntry.state;
			int num2 = 0;
			while (state.transitions != null && num2 < state.transitions.Count)
			{
				this.PopTransition((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state);
				num2++;
			}
			if (state.events != null)
			{
				for (int i = 0; i < state.events.Count; i++)
				{
					this.PopEvent();
				}
			}
			if (state.updateActions != null)
			{
				foreach (StateMachine.UpdateAction updateAction in state.updateActions)
				{
					int updateTableIdx = updateAction.updateTableIdx;
					StateMachineUpdater.BaseUpdateBucket baseUpdateBucket = (UpdateBucketWithUpdater<StateMachineInstanceType>)this.smi.updateTable[updateTableIdx].bucket;
					this.smi.updateTable[updateTableIdx].bucket = null;
					baseUpdateBucket.Remove(this.smi.updateTable[updateTableIdx].handle);
				}
			}
			stackEntry.schedulerGroup.Reset();
			this.currentSchedulerGroup = stackEntry.schedulerGroup;
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.exitActions);
		}

		// Token: 0x0600846C RID: 33900 RVA: 0x0032350C File Offset: 0x0032170C
		public override SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.Schedule(name, time, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x0600846D RID: 33901 RVA: 0x00323530 File Offset: 0x00321730
		public override SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.ScheduleNextFrame(name, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x0600846E RID: 33902 RVA: 0x00323552 File Offset: 0x00321752
		public override void StartSM()
		{
			if (this.controller != null && !this.controller.HasStateMachineInstance(this))
			{
				this.controller.AddStateMachineInstance(this);
			}
			base.StartSM();
		}

		// Token: 0x0600846F RID: 33903 RVA: 0x00323584 File Offset: 0x00321784
		public override void StopSM(string reason)
		{
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (!base.IsRunning())
			{
				return;
			}
			this.gotoId++;
			while (this.stackSize > 0)
			{
				this.PopState();
			}
			if (this.master != null && this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (this.status == StateMachine.Status.Running)
			{
				base.SetStatus(StateMachine.Status.Failed);
			}
			if (this.OnStop != null)
			{
				this.OnStop(reason, this.status);
			}
			for (int i = 0; i < this.parameterContexts.Length; i++)
			{
				this.parameterContexts[i].Cleanup();
			}
			this.OnCleanUp();
		}

		// Token: 0x06008470 RID: 33904 RVA: 0x00323652 File Offset: 0x00321852
		private void FinishStateInProgress(StateMachine.BaseState state)
		{
			if (state.enterActions == null)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
		}

		// Token: 0x06008471 RID: 33905 RVA: 0x00323670 File Offset: 0x00321870
		public override void GoTo(StateMachine.BaseState base_state)
		{
			if (App.IsExiting)
			{
				return;
			}
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.isMasterNull)
			{
				return;
			}
			if (this.smi.IsNullOrDestroyed())
			{
				return;
			}
			try
			{
				if (base.IsBreakOnGoToEnabled())
				{
					Debugger.Break();
				}
				if (base_state != null)
				{
					while (base_state.defaultState != null)
					{
						base_state = base_state.defaultState;
					}
				}
				if (this.GetCurrentState() == null)
				{
					base.SetStatus(StateMachine.Status.Running);
				}
				if (this.gotoStack.Count > 100)
				{
					string text = "Potential infinite transition loop detected in state machine: " + this.ToString() + "\nGoto stack:\n";
					foreach (StateMachine.BaseState baseState in this.gotoStack)
					{
						text = text + "\n" + baseState.name;
					}
					global::Debug.LogError(text);
					base.Error();
				}
				else
				{
					this.gotoStack.Push(base_state);
					if (base_state == null)
					{
						this.StopSM("StateMachine.GoTo(null)");
						this.gotoStack.Pop();
					}
					else
					{
						int num = this.gotoId + 1;
						this.gotoId = num;
						int num2 = num;
						StateMachine.BaseState[] branch = (base_state as StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State).branch;
						int num3 = 0;
						while (num3 < this.stackSize && num3 < branch.Length && this.stateStack[num3].state == branch[num3])
						{
							num3++;
						}
						int num4 = this.stackSize - 1;
						if (num4 >= 0 && num4 == num3 - 1)
						{
							this.FinishStateInProgress(this.stateStack[num4].state);
						}
						while (this.stackSize > num3 && num2 == this.gotoId)
						{
							this.PopState();
						}
						int num5 = num3;
						while (num5 < branch.Length && num2 == this.gotoId)
						{
							this.PushState(branch[num5]);
							num5++;
						}
						this.gotoStack.Pop();
					}
				}
			}
			catch (Exception ex)
			{
				if (!StateMachine.Instance.error)
				{
					base.Error();
					string text2 = "(Stop)";
					if (base_state != null)
					{
						text2 = base_state.name;
					}
					string text3 = "(NULL).";
					if (!this.GetMaster().isNull)
					{
						text3 = "(" + base.gameObject.name + ").";
					}
					string str = string.Concat(new string[]
					{
						"Exception in: ",
						text3,
						this.stateMachine.ToString(),
						".GoTo(",
						text2,
						")"
					});
					DebugUtil.LogErrorArgs(this.controller, new object[]
					{
						str + "\n" + ex.ToString()
					});
				}
			}
		}

		// Token: 0x06008472 RID: 33906 RVA: 0x0032393C File Offset: 0x00321B3C
		public override StateMachine.BaseState GetCurrentState()
		{
			if (this.stackSize > 0)
			{
				return this.stateStack[this.stackSize - 1].state;
			}
			return null;
		}

		// Token: 0x040063E0 RID: 25568
		private float stateEnterTime;

		// Token: 0x040063E1 RID: 25569
		private int gotoId;

		// Token: 0x040063E2 RID: 25570
		private int currentActionIdx = -1;

		// Token: 0x040063E3 RID: 25571
		private SchedulerHandle updateHandle;

		// Token: 0x040063E4 RID: 25572
		private Stack<StateMachine.BaseState> gotoStack = new Stack<StateMachine.BaseState>();

		// Token: 0x040063E5 RID: 25573
		protected Stack<StateMachine.BaseTransition.Context> transitionStack = new Stack<StateMachine.BaseTransition.Context>();

		// Token: 0x040063E9 RID: 25577
		protected StateMachineController controller;

		// Token: 0x040063EA RID: 25578
		private SchedulerGroup currentSchedulerGroup;

		// Token: 0x040063EB RID: 25579
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] stateStack;

		// Token: 0x0200247D RID: 9341
		public struct StackEntry
		{
			// Token: 0x0400A21C RID: 41500
			public StateMachine.BaseState state;

			// Token: 0x0400A21D RID: 41501
			public SchedulerGroup schedulerGroup;
		}
	}

	// Token: 0x02001298 RID: 4760
	public class State : StateMachine.BaseState
	{
		// Token: 0x040063EC RID: 25580
		protected StateMachineType sm;

		// Token: 0x0200247E RID: 9342
		// (Invoke) Token: 0x0600B9F1 RID: 47601
		public delegate void Callback(StateMachineInstanceType smi);
	}

	// Token: 0x02001299 RID: 4761
	public new abstract class ParameterTransition : StateMachine.ParameterTransition
	{
		// Token: 0x06008474 RID: 33908 RVA: 0x00323969 File Offset: 0x00321B69
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x0200129A RID: 4762
	public class Transition : StateMachine.BaseTransition
	{
		// Token: 0x06008475 RID: 33909 RVA: 0x00323976 File Offset: 0x00321B76
		public Transition(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition) : base(idx, name, source_state, target_state)
		{
			this.condition = condition;
		}

		// Token: 0x06008476 RID: 33910 RVA: 0x0032398B File Offset: 0x00321B8B
		public override string ToString()
		{
			if (this.targetState != null)
			{
				return this.name + "->" + this.targetState.name;
			}
			return this.name + "->(Stop)";
		}

		// Token: 0x040063ED RID: 25581
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

		// Token: 0x0200247F RID: 9343
		// (Invoke) Token: 0x0600B9F5 RID: 47605
		public delegate bool ConditionCallback(StateMachineInstanceType smi);
	}

	// Token: 0x0200129B RID: 4763
	public abstract class Parameter<ParameterType> : StateMachine.Parameter
	{
		// Token: 0x06008477 RID: 33911 RVA: 0x003239C1 File Offset: 0x00321BC1
		public Parameter()
		{
		}

		// Token: 0x06008478 RID: 33912 RVA: 0x003239C9 File Offset: 0x00321BC9
		public Parameter(ParameterType default_value)
		{
			this.defaultValue = default_value;
		}

		// Token: 0x06008479 RID: 33913 RVA: 0x003239D8 File Offset: 0x00321BD8
		public ParameterType Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).Set(value, smi, silenceEvents);
			return value;
		}

		// Token: 0x0600847A RID: 33914 RVA: 0x003239F4 File Offset: 0x00321BF4
		public ParameterType Get(StateMachineInstanceType smi)
		{
			return ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).value;
		}

		// Token: 0x0600847B RID: 33915 RVA: 0x00323A0C File Offset: 0x00321C0C
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context GetContext(StateMachineInstanceType smi)
		{
			return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this);
		}

		// Token: 0x040063EE RID: 25582
		public ParameterType defaultValue;

		// Token: 0x040063EF RID: 25583
		public bool isSignal;

		// Token: 0x02002480 RID: 9344
		// (Invoke) Token: 0x0600B9F9 RID: 47609
		public delegate bool Callback(StateMachineInstanceType smi, ParameterType p);

		// Token: 0x02002481 RID: 9345
		public class Transition : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition
		{
			// Token: 0x0600B9FC RID: 47612 RVA: 0x003D2862 File Offset: 0x003D0A62
			public Transition(int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback) : base(idx, parameter.name, null, state)
			{
				this.parameter = parameter;
				this.callback = callback;
			}

			// Token: 0x0600B9FD RID: 47613 RVA: 0x003D2884 File Offset: 0x003D0A84
			public override void Evaluate(StateMachine.Instance smi)
			{
				StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
				global::Debug.Assert(stateMachineInstanceType != null);
				if (this.parameter.isSignal && this.callback == null)
				{
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)stateMachineInstanceType.GetParameterContext(this.parameter);
				if (this.callback(stateMachineInstanceType, context.value))
				{
					stateMachineInstanceType.GoTo(this.targetState);
				}
			}

			// Token: 0x0600B9FE RID: 47614 RVA: 0x003D28FD File Offset: 0x003D0AFD
			private void Trigger(StateMachineInstanceType smi)
			{
				smi.GoTo(this.targetState);
			}

			// Token: 0x0600B9FF RID: 47615 RVA: 0x003D2910 File Offset: 0x003D0B10
			public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context2.onDirty, new Action<StateMachineInstanceType>(this.Trigger));
				}
				else
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
					context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context3.onDirty, new Action<StateMachineInstanceType>(this.Evaluate));
				}
				return new StateMachine.BaseTransition.Context(this);
			}

			// Token: 0x0600BA00 RID: 47616 RVA: 0x003D2994 File Offset: 0x003D0B94
			public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context transitionContext)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context2.onDirty, new Action<StateMachineInstanceType>(this.Trigger));
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
				context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context3.onDirty, new Action<StateMachineInstanceType>(this.Evaluate));
			}

			// Token: 0x0600BA01 RID: 47617 RVA: 0x003D2A0E File Offset: 0x003D0C0E
			public override string ToString()
			{
				if (this.targetState != null)
				{
					return this.parameter.name + "->" + this.targetState.name;
				}
				return this.parameter.name + "->(Stop)";
			}

			// Token: 0x0400A21E RID: 41502
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter;

			// Token: 0x0400A21F RID: 41503
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback;
		}

		// Token: 0x02002482 RID: 9346
		public new abstract class Context : StateMachine.Parameter.Context
		{
			// Token: 0x0600BA02 RID: 47618 RVA: 0x003D2A4E File Offset: 0x003D0C4E
			public Context(StateMachine.Parameter parameter, ParameterType default_value) : base(parameter)
			{
				this.value = default_value;
			}

			// Token: 0x0600BA03 RID: 47619 RVA: 0x003D2A5E File Offset: 0x003D0C5E
			public virtual void Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!EqualityComparer<ParameterType>.Default.Equals(value, this.value))
				{
					this.value = value;
					if (!silenceEvents && this.onDirty != null)
					{
						this.onDirty(smi);
					}
				}
			}

			// Token: 0x0400A220 RID: 41504
			public ParameterType value;

			// Token: 0x0400A221 RID: 41505
			public Action<StateMachineInstanceType> onDirty;
		}
	}

	// Token: 0x0200129C RID: 4764
	public class BoolParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>
	{
		// Token: 0x0600847C RID: 33916 RVA: 0x00323A1F File Offset: 0x00321C1F
		public BoolParameter()
		{
		}

		// Token: 0x0600847D RID: 33917 RVA: 0x00323A27 File Offset: 0x00321C27
		public BoolParameter(bool default_value) : base(default_value)
		{
		}

		// Token: 0x0600847E RID: 33918 RVA: 0x00323A30 File Offset: 0x00321C30
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.BoolParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002483 RID: 9347
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Context
		{
			// Token: 0x0600BA04 RID: 47620 RVA: 0x003D2A91 File Offset: 0x003D0C91
			public Context(StateMachine.Parameter parameter, bool default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA05 RID: 47621 RVA: 0x003D2A9B File Offset: 0x003D0C9B
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value ? 1 : 0);
			}

			// Token: 0x0600BA06 RID: 47622 RVA: 0x003D2AB0 File Offset: 0x003D0CB0
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (reader.ReadByte() > 0);
			}

			// Token: 0x0600BA07 RID: 47623 RVA: 0x003D2AC1 File Offset: 0x003D0CC1
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA08 RID: 47624 RVA: 0x003D2AC4 File Offset: 0x003D0CC4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				bool value = this.value;
				if (ImGui.Checkbox(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x0200129D RID: 4765
	public class Vector3Parameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>
	{
		// Token: 0x0600847F RID: 33919 RVA: 0x00323A3E File Offset: 0x00321C3E
		public Vector3Parameter()
		{
		}

		// Token: 0x06008480 RID: 33920 RVA: 0x00323A46 File Offset: 0x00321C46
		public Vector3Parameter(Vector3 default_value) : base(default_value)
		{
		}

		// Token: 0x06008481 RID: 33921 RVA: 0x00323A4F File Offset: 0x00321C4F
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Vector3Parameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002484 RID: 9348
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>.Context
		{
			// Token: 0x0600BA09 RID: 47625 RVA: 0x003D2AFC File Offset: 0x003D0CFC
			public Context(StateMachine.Parameter parameter, Vector3 default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA0A RID: 47626 RVA: 0x003D2B06 File Offset: 0x003D0D06
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.x);
				writer.Write(this.value.y);
				writer.Write(this.value.z);
			}

			// Token: 0x0600BA0B RID: 47627 RVA: 0x003D2B3B File Offset: 0x003D0D3B
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value.x = reader.ReadSingle();
				this.value.y = reader.ReadSingle();
				this.value.z = reader.ReadSingle();
			}

			// Token: 0x0600BA0C RID: 47628 RVA: 0x003D2B70 File Offset: 0x003D0D70
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA0D RID: 47629 RVA: 0x003D2B74 File Offset: 0x003D0D74
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				Vector3 value = this.value;
				if (ImGui.InputFloat3(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x0200129E RID: 4766
	public class EnumParameter<EnumType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>
	{
		// Token: 0x06008482 RID: 33922 RVA: 0x00323A5D File Offset: 0x00321C5D
		public EnumParameter(EnumType default_value) : base(default_value)
		{
		}

		// Token: 0x06008483 RID: 33923 RVA: 0x00323A66 File Offset: 0x00321C66
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EnumParameter<EnumType>.Context(this, this.defaultValue);
		}

		// Token: 0x02002485 RID: 9349
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>.Context
		{
			// Token: 0x0600BA0E RID: 47630 RVA: 0x003D2BAC File Offset: 0x003D0DAC
			public Context(StateMachine.Parameter parameter, EnumType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA0F RID: 47631 RVA: 0x003D2BB6 File Offset: 0x003D0DB6
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write((int)((object)this.value));
			}

			// Token: 0x0600BA10 RID: 47632 RVA: 0x003D2BCE File Offset: 0x003D0DCE
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (EnumType)((object)reader.ReadInt32());
			}

			// Token: 0x0600BA11 RID: 47633 RVA: 0x003D2BE6 File Offset: 0x003D0DE6
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA12 RID: 47634 RVA: 0x003D2BE8 File Offset: 0x003D0DE8
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string[] names = Enum.GetNames(typeof(EnumType));
				Array values = Enum.GetValues(typeof(EnumType));
				int index = Array.IndexOf(values, this.value);
				if (ImGui.Combo(this.parameter.name, ref index, names, names.Length))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set((EnumType)((object)values.GetValue(index)), smi, false);
				}
			}
		}
	}

	// Token: 0x0200129F RID: 4767
	public class FloatParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>
	{
		// Token: 0x06008484 RID: 33924 RVA: 0x00323A74 File Offset: 0x00321C74
		public FloatParameter()
		{
		}

		// Token: 0x06008485 RID: 33925 RVA: 0x00323A7C File Offset: 0x00321C7C
		public FloatParameter(float default_value) : base(default_value)
		{
		}

		// Token: 0x06008486 RID: 33926 RVA: 0x00323A88 File Offset: 0x00321C88
		public float Delta(float delta_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06008487 RID: 33927 RVA: 0x00323AAC File Offset: 0x00321CAC
		public float DeltaClamp(float delta_value, float min_value, float max_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			num = Mathf.Clamp(num, min_value, max_value);
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06008488 RID: 33928 RVA: 0x00323ADB File Offset: 0x00321CDB
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002486 RID: 9350
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Context
		{
			// Token: 0x0600BA13 RID: 47635 RVA: 0x003D2C5A File Offset: 0x003D0E5A
			public Context(StateMachine.Parameter parameter, float default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA14 RID: 47636 RVA: 0x003D2C64 File Offset: 0x003D0E64
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600BA15 RID: 47637 RVA: 0x003D2C72 File Offset: 0x003D0E72
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadSingle();
			}

			// Token: 0x0600BA16 RID: 47638 RVA: 0x003D2C80 File Offset: 0x003D0E80
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA17 RID: 47639 RVA: 0x003D2C84 File Offset: 0x003D0E84
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				float value = this.value;
				if (ImGui.InputFloat(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020012A0 RID: 4768
	public class IntParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>
	{
		// Token: 0x06008489 RID: 33929 RVA: 0x00323AE9 File Offset: 0x00321CE9
		public IntParameter()
		{
		}

		// Token: 0x0600848A RID: 33930 RVA: 0x00323AF1 File Offset: 0x00321CF1
		public IntParameter(int default_value) : base(default_value)
		{
		}

		// Token: 0x0600848B RID: 33931 RVA: 0x00323AFC File Offset: 0x00321CFC
		public int Delta(int delta_value, StateMachineInstanceType smi)
		{
			int num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x0600848C RID: 33932 RVA: 0x00323B20 File Offset: 0x00321D20
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.IntParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002487 RID: 9351
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Context
		{
			// Token: 0x0600BA18 RID: 47640 RVA: 0x003D2CBC File Offset: 0x003D0EBC
			public Context(StateMachine.Parameter parameter, int default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA19 RID: 47641 RVA: 0x003D2CC6 File Offset: 0x003D0EC6
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600BA1A RID: 47642 RVA: 0x003D2CD4 File Offset: 0x003D0ED4
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt32();
			}

			// Token: 0x0600BA1B RID: 47643 RVA: 0x003D2CE2 File Offset: 0x003D0EE2
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA1C RID: 47644 RVA: 0x003D2CE4 File Offset: 0x003D0EE4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				int value = this.value;
				if (ImGui.InputInt(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020012A1 RID: 4769
	public class LongParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>
	{
		// Token: 0x0600848D RID: 33933 RVA: 0x00323B2E File Offset: 0x00321D2E
		public LongParameter()
		{
		}

		// Token: 0x0600848E RID: 33934 RVA: 0x00323B36 File Offset: 0x00321D36
		public LongParameter(long default_value) : base(default_value)
		{
		}

		// Token: 0x0600848F RID: 33935 RVA: 0x00323B40 File Offset: 0x00321D40
		public long Delta(long delta_value, StateMachineInstanceType smi)
		{
			long num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06008490 RID: 33936 RVA: 0x00323B64 File Offset: 0x00321D64
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.LongParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002488 RID: 9352
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>.Context
		{
			// Token: 0x0600BA1D RID: 47645 RVA: 0x003D2D1C File Offset: 0x003D0F1C
			public Context(StateMachine.Parameter parameter, long default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA1E RID: 47646 RVA: 0x003D2D26 File Offset: 0x003D0F26
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600BA1F RID: 47647 RVA: 0x003D2D34 File Offset: 0x003D0F34
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt64();
			}

			// Token: 0x0600BA20 RID: 47648 RVA: 0x003D2D42 File Offset: 0x003D0F42
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA21 RID: 47649 RVA: 0x003D2D44 File Offset: 0x003D0F44
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				long value = this.value;
			}
		}
	}

	// Token: 0x020012A2 RID: 4770
	public class ResourceParameter<ResourceType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType> where ResourceType : Resource
	{
		// Token: 0x06008491 RID: 33937 RVA: 0x00323B74 File Offset: 0x00321D74
		public ResourceParameter() : base(default(ResourceType))
		{
		}

		// Token: 0x06008492 RID: 33938 RVA: 0x00323B90 File Offset: 0x00321D90
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ResourceParameter<ResourceType>.Context(this, this.defaultValue);
		}

		// Token: 0x02002489 RID: 9353
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>.Context
		{
			// Token: 0x0600BA22 RID: 47650 RVA: 0x003D2D4D File Offset: 0x003D0F4D
			public Context(StateMachine.Parameter parameter, ResourceType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA23 RID: 47651 RVA: 0x003D2D58 File Offset: 0x003D0F58
			public override void Serialize(BinaryWriter writer)
			{
				string str = "";
				if (this.value != null)
				{
					if (this.value.Guid == null)
					{
						global::Debug.LogError("Cannot serialize resource with invalid guid: " + this.value.Id);
					}
					else
					{
						str = this.value.Guid.Guid;
					}
				}
				writer.WriteKleiString(str);
			}

			// Token: 0x0600BA24 RID: 47652 RVA: 0x003D2DD0 File Offset: 0x003D0FD0
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				string text = reader.ReadKleiString();
				if (text != "")
				{
					ResourceGuid guid = new ResourceGuid(text, null);
					this.value = Db.Get().GetResource<ResourceType>(guid);
				}
			}

			// Token: 0x0600BA25 RID: 47653 RVA: 0x003D2E0A File Offset: 0x003D100A
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA26 RID: 47654 RVA: 0x003D2E0C File Offset: 0x003D100C
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x020012A3 RID: 4771
	public class TagParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>
	{
		// Token: 0x06008493 RID: 33939 RVA: 0x00323B9E File Offset: 0x00321D9E
		public TagParameter()
		{
		}

		// Token: 0x06008494 RID: 33940 RVA: 0x00323BA6 File Offset: 0x00321DA6
		public TagParameter(Tag default_value) : base(default_value)
		{
		}

		// Token: 0x06008495 RID: 33941 RVA: 0x00323BAF File Offset: 0x00321DAF
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagParameter.Context(this, this.defaultValue);
		}

		// Token: 0x0200248A RID: 9354
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>.Context
		{
			// Token: 0x0600BA27 RID: 47655 RVA: 0x003D2E4E File Offset: 0x003D104E
			public Context(StateMachine.Parameter parameter, Tag default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA28 RID: 47656 RVA: 0x003D2E58 File Offset: 0x003D1058
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.GetHash());
			}

			// Token: 0x0600BA29 RID: 47657 RVA: 0x003D2E6B File Offset: 0x003D106B
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = new Tag(reader.ReadInt32());
			}

			// Token: 0x0600BA2A RID: 47658 RVA: 0x003D2E7E File Offset: 0x003D107E
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA2B RID: 47659 RVA: 0x003D2E80 File Offset: 0x003D1080
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				ImGui.LabelText(this.parameter.name, this.value.ToString());
			}
		}
	}

	// Token: 0x020012A4 RID: 4772
	public class ObjectParameter<ObjectType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType> where ObjectType : class
	{
		// Token: 0x06008496 RID: 33942 RVA: 0x00323BC0 File Offset: 0x00321DC0
		public ObjectParameter() : base(default(ObjectType))
		{
		}

		// Token: 0x06008497 RID: 33943 RVA: 0x00323BDC File Offset: 0x00321DDC
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ObjectParameter<ObjectType>.Context(this, this.defaultValue);
		}

		// Token: 0x0200248B RID: 9355
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>.Context
		{
			// Token: 0x0600BA2C RID: 47660 RVA: 0x003D2EA3 File Offset: 0x003D10A3
			public Context(StateMachine.Parameter parameter, ObjectType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA2D RID: 47661 RVA: 0x003D2EAD File Offset: 0x003D10AD
			public override void Serialize(BinaryWriter writer)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x0600BA2E RID: 47662 RVA: 0x003D2EB9 File Offset: 0x003D10B9
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x0600BA2F RID: 47663 RVA: 0x003D2EC5 File Offset: 0x003D10C5
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA30 RID: 47664 RVA: 0x003D2EC8 File Offset: 0x003D10C8
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x020012A5 RID: 4773
	public class TargetParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>
	{
		// Token: 0x06008498 RID: 33944 RVA: 0x00323BEA File Offset: 0x00321DEA
		public TargetParameter() : base(null)
		{
		}

		// Token: 0x06008499 RID: 33945 RVA: 0x00323BF4 File Offset: 0x00321DF4
		public SMT GetSMI<SMT>(StateMachineInstanceType smi) where SMT : StateMachine.Instance
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				SMT smi2 = gameObject.GetSMI<SMT>();
				if (smi2 != null)
				{
					return smi2;
				}
				global::Debug.LogError(gameObject.name + " does not have state machine " + typeof(StateMachineType).Name);
			}
			return default(SMT);
		}

		// Token: 0x0600849A RID: 33946 RVA: 0x00323C50 File Offset: 0x00321E50
		public bool IsNull(StateMachineInstanceType smi)
		{
			return base.Get(smi) == null;
		}

		// Token: 0x0600849B RID: 33947 RVA: 0x00323C60 File Offset: 0x00321E60
		public ComponentType Get<ComponentType>(StateMachineInstanceType smi)
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType component = gameObject.GetComponent<ComponentType>();
				if (component != null)
				{
					return component;
				}
				global::Debug.LogError(gameObject.name + " does not have component " + typeof(ComponentType).Name);
			}
			return default(ComponentType);
		}

		// Token: 0x0600849C RID: 33948 RVA: 0x00323CBC File Offset: 0x00321EBC
		public ComponentType AddOrGet<ComponentType>(StateMachineInstanceType smi) where ComponentType : Component
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType componentType = gameObject.GetComponent<ComponentType>();
				if (componentType == null)
				{
					componentType = gameObject.AddComponent<ComponentType>();
				}
				return componentType;
			}
			return default(ComponentType);
		}

		// Token: 0x0600849D RID: 33949 RVA: 0x00323D04 File Offset: 0x00321F04
		public void Set(KMonoBehaviour value, StateMachineInstanceType smi)
		{
			GameObject value2 = null;
			if (value != null)
			{
				value2 = value.gameObject;
			}
			base.Set(value2, smi, false);
		}

		// Token: 0x0600849E RID: 33950 RVA: 0x00323D2D File Offset: 0x00321F2D
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context(this, this.defaultValue);
		}

		// Token: 0x0200248C RID: 9356
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Context
		{
			// Token: 0x0600BA31 RID: 47665 RVA: 0x003D2F0A File Offset: 0x003D110A
			public Context(StateMachine.Parameter parameter, GameObject default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA32 RID: 47666 RVA: 0x003D2F14 File Offset: 0x003D1114
			public override void Serialize(BinaryWriter writer)
			{
				if (this.value != null)
				{
					int instanceID = this.value.GetComponent<KPrefabID>().InstanceID;
					writer.Write(instanceID);
					return;
				}
				writer.Write(0);
			}

			// Token: 0x0600BA33 RID: 47667 RVA: 0x003D2F50 File Offset: 0x003D1150
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				try
				{
					int num = reader.ReadInt32();
					if (num != 0)
					{
						KPrefabID instance = KPrefabIDTracker.Get().GetInstance(num);
						if (instance != null)
						{
							this.value = instance.gameObject;
							this.objectDestroyedHandler = instance.Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
						}
						this.m_smi = (StateMachineInstanceType)((object)smi);
					}
				}
				catch (Exception ex)
				{
					if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
					{
						global::Debug.LogWarning("Missing statemachine target params. " + ex.Message);
					}
				}
			}

			// Token: 0x0600BA34 RID: 47668 RVA: 0x003D2FF4 File Offset: 0x003D11F4
			public override void Cleanup()
			{
				base.Cleanup();
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
					this.objectDestroyedHandler = 0;
				}
			}

			// Token: 0x0600BA35 RID: 47669 RVA: 0x003D3028 File Offset: 0x003D1228
			public override void Set(GameObject value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				this.m_smi = smi;
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
					this.objectDestroyedHandler = 0;
				}
				if (value != null)
				{
					this.objectDestroyedHandler = value.GetComponent<KMonoBehaviour>().Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
				}
				base.Set(value, smi, silenceEvents);
			}

			// Token: 0x0600BA36 RID: 47670 RVA: 0x003D309B File Offset: 0x003D129B
			private void OnObjectDestroyed(object data)
			{
				this.Set(null, this.m_smi, false);
			}

			// Token: 0x0600BA37 RID: 47671 RVA: 0x003D30AB File Offset: 0x003D12AB
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA38 RID: 47672 RVA: 0x003D30B0 File Offset: 0x003D12B0
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (this.value != null)
				{
					ImGui.LabelText(this.parameter.name, this.value.name);
					return;
				}
				ImGui.LabelText(this.parameter.name, "null");
			}

			// Token: 0x0400A222 RID: 41506
			private StateMachineInstanceType m_smi;

			// Token: 0x0400A223 RID: 41507
			private int objectDestroyedHandler;
		}
	}

	// Token: 0x020012A6 RID: 4774
	public class SignalParameter
	{
	}

	// Token: 0x020012A7 RID: 4775
	public class Signal : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>
	{
		// Token: 0x060084A0 RID: 33952 RVA: 0x00323D43 File Offset: 0x00321F43
		public Signal() : base(null)
		{
			this.isSignal = true;
		}

		// Token: 0x060084A1 RID: 33953 RVA: 0x00323D53 File Offset: 0x00321F53
		public void Trigger(StateMachineInstanceType smi)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context)smi.GetParameterContext(this)).Set(null, smi, false);
		}

		// Token: 0x060084A2 RID: 33954 RVA: 0x00323D6E File Offset: 0x00321F6E
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context(this, this.defaultValue);
		}

		// Token: 0x0200248D RID: 9357
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context
		{
			// Token: 0x0600BA39 RID: 47673 RVA: 0x003D30FC File Offset: 0x003D12FC
			public Context(StateMachine.Parameter parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600BA3A RID: 47674 RVA: 0x003D3106 File Offset: 0x003D1306
			public override void Serialize(BinaryWriter writer)
			{
			}

			// Token: 0x0600BA3B RID: 47675 RVA: 0x003D3108 File Offset: 0x003D1308
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
			}

			// Token: 0x0600BA3C RID: 47676 RVA: 0x003D310A File Offset: 0x003D130A
			public override void Set(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!silenceEvents && this.onDirty != null)
				{
					this.onDirty(smi);
				}
			}

			// Token: 0x0600BA3D RID: 47677 RVA: 0x003D3123 File Offset: 0x003D1323
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600BA3E RID: 47678 RVA: 0x003D3128 File Offset: 0x003D1328
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (ImGui.Button(this.parameter.name))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(null, smi, false);
				}
			}
		}
	}
}

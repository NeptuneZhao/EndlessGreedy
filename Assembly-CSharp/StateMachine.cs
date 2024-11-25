using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KSerialization;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
public abstract class StateMachine
{
	// Token: 0x06001A4F RID: 6735 RVA: 0x0008B36F File Offset: 0x0008956F
	public StateMachine()
	{
		this.name = base.GetType().FullName;
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x0008B394 File Offset: 0x00089594
	public virtual void FreeResources()
	{
		this.name = null;
		if (this.defaultState != null)
		{
			this.defaultState.FreeResources();
		}
		this.defaultState = null;
		this.parameters = null;
	}

	// Token: 0x06001A51 RID: 6737
	public abstract string[] GetStateNames();

	// Token: 0x06001A52 RID: 6738
	public abstract StateMachine.BaseState GetState(string name);

	// Token: 0x06001A53 RID: 6739
	public abstract void BindStates();

	// Token: 0x06001A54 RID: 6740
	public abstract Type GetStateMachineInstanceType();

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06001A55 RID: 6741 RVA: 0x0008B3BE File Offset: 0x000895BE
	// (set) Token: 0x06001A56 RID: 6742 RVA: 0x0008B3C6 File Offset: 0x000895C6
	public int version { get; protected set; }

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06001A57 RID: 6743 RVA: 0x0008B3CF File Offset: 0x000895CF
	// (set) Token: 0x06001A58 RID: 6744 RVA: 0x0008B3D7 File Offset: 0x000895D7
	public StateMachine.SerializeType serializable { get; protected set; }

	// Token: 0x06001A59 RID: 6745 RVA: 0x0008B3E0 File Offset: 0x000895E0
	public virtual void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = null;
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x0008B3E8 File Offset: 0x000895E8
	public void InitializeStateMachine()
	{
		this.debugSettings = StateMachineDebuggerSettings.Get().CreateEntry(base.GetType());
		StateMachine.BaseState baseState = null;
		this.InitializeStates(out baseState);
		DebugUtil.Assert(baseState != null);
		this.defaultState = baseState;
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x0008B428 File Offset: 0x00089628
	public void CreateStates(object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			bool flag = false;
			object[] customAttributes = fieldInfo.GetCustomAttributes(false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				if (customAttributes[j].GetType() == typeof(StateMachine.DoNotAutoCreate))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
				{
					StateMachine.BaseState baseState = (StateMachine.BaseState)Activator.CreateInstance(fieldInfo.FieldType);
					this.CreateStates(baseState);
					fieldInfo.SetValue(state_machine, baseState);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.Parameter)))
				{
					StateMachine.Parameter parameter = (StateMachine.Parameter)fieldInfo.GetValue(state_machine);
					if (parameter == null)
					{
						parameter = (StateMachine.Parameter)Activator.CreateInstance(fieldInfo.FieldType);
						fieldInfo.SetValue(state_machine, parameter);
					}
					parameter.name = fieldInfo.Name;
					parameter.idx = this.parameters.Length;
					this.parameters = this.parameters.Append(parameter);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine)))
				{
					fieldInfo.SetValue(state_machine, this);
				}
			}
		}
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x0008B571 File Offset: 0x00089771
	public StateMachine.BaseState GetDefaultState()
	{
		return this.defaultState;
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x0008B579 File Offset: 0x00089779
	public int GetMaxDepth()
	{
		return this.maxDepth;
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x0008B581 File Offset: 0x00089781
	public override string ToString()
	{
		return this.name;
	}

	// Token: 0x04000EFE RID: 3838
	protected string name;

	// Token: 0x04000EFF RID: 3839
	protected int maxDepth;

	// Token: 0x04000F00 RID: 3840
	protected StateMachine.BaseState defaultState;

	// Token: 0x04000F01 RID: 3841
	protected StateMachine.Parameter[] parameters = new StateMachine.Parameter[0];

	// Token: 0x04000F02 RID: 3842
	public int dataTableSize;

	// Token: 0x04000F03 RID: 3843
	public int updateTableSize;

	// Token: 0x04000F06 RID: 3846
	public StateMachineDebuggerSettings.Entry debugSettings;

	// Token: 0x04000F07 RID: 3847
	public bool saveHistory;

	// Token: 0x0200128B RID: 4747
	public sealed class DoNotAutoCreate : Attribute
	{
	}

	// Token: 0x0200128C RID: 4748
	public enum Status
	{
		// Token: 0x040063AD RID: 25517
		Initialized,
		// Token: 0x040063AE RID: 25518
		Running,
		// Token: 0x040063AF RID: 25519
		Failed,
		// Token: 0x040063B0 RID: 25520
		Success
	}

	// Token: 0x0200128D RID: 4749
	public class BaseDef
	{
		// Token: 0x0600841F RID: 33823 RVA: 0x00322774 File Offset: 0x00320974
		public StateMachine.Instance CreateSMI(IStateMachineTarget master)
		{
			return Singleton<StateMachineManager>.Instance.CreateSMIFromDef(master, this);
		}

		// Token: 0x06008420 RID: 33824 RVA: 0x00322782 File Offset: 0x00320982
		public Type GetStateMachineType()
		{
			return base.GetType().DeclaringType;
		}

		// Token: 0x06008421 RID: 33825 RVA: 0x0032278F File Offset: 0x0032098F
		public virtual void Configure(GameObject prefab)
		{
		}

		// Token: 0x040063B1 RID: 25521
		public bool preventStartSMIOnSpawn;
	}

	// Token: 0x0200128E RID: 4750
	public class Category : Resource
	{
		// Token: 0x06008423 RID: 33827 RVA: 0x00322799 File Offset: 0x00320999
		public Category(string id) : base(id, null, null)
		{
		}
	}

	// Token: 0x0200128F RID: 4751
	[SerializationConfig(MemberSerialization.OptIn)]
	public abstract class Instance
	{
		// Token: 0x06008424 RID: 33828
		public abstract StateMachine.BaseState GetCurrentState();

		// Token: 0x06008425 RID: 33829
		public abstract void GoTo(StateMachine.BaseState state);

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06008426 RID: 33830
		public abstract float timeinstate { get; }

		// Token: 0x06008427 RID: 33831
		public abstract IStateMachineTarget GetMaster();

		// Token: 0x06008428 RID: 33832
		public abstract void StopSM(string reason);

		// Token: 0x06008429 RID: 33833
		public abstract SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null);

		// Token: 0x0600842A RID: 33834
		public abstract SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null);

		// Token: 0x0600842B RID: 33835 RVA: 0x003227A4 File Offset: 0x003209A4
		public virtual void FreeResources()
		{
			this.stateMachine = null;
			if (this.subscribedEvents != null)
			{
				this.subscribedEvents.Clear();
			}
			this.subscribedEvents = null;
			this.parameterContexts = null;
			this.dataTable = null;
			this.updateTable = null;
		}

		// Token: 0x0600842C RID: 33836 RVA: 0x003227DC File Offset: 0x003209DC
		public Instance(StateMachine state_machine, IStateMachineTarget master)
		{
			this.stateMachine = state_machine;
			this.CreateParameterContexts();
			this.log = new LoggerFSSSS(this.stateMachine.name, 35);
		}

		// Token: 0x0600842D RID: 33837 RVA: 0x00322814 File Offset: 0x00320A14
		public bool IsRunning()
		{
			return this.GetCurrentState() != null;
		}

		// Token: 0x0600842E RID: 33838 RVA: 0x00322820 File Offset: 0x00320A20
		public void GoTo(string state_name)
		{
			DebugUtil.DevAssert(!KMonoBehaviour.isLoadingScene, "Using Goto while scene was loaded", null);
			StateMachine.BaseState state = this.stateMachine.GetState(state_name);
			this.GoTo(state);
		}

		// Token: 0x0600842F RID: 33839 RVA: 0x00322854 File Offset: 0x00320A54
		public int GetStackSize()
		{
			return this.stackSize;
		}

		// Token: 0x06008430 RID: 33840 RVA: 0x0032285C File Offset: 0x00320A5C
		public StateMachine GetStateMachine()
		{
			return this.stateMachine;
		}

		// Token: 0x06008431 RID: 33841 RVA: 0x00322864 File Offset: 0x00320A64
		[Conditional("UNITY_EDITOR")]
		public void Log(string a, string b = "", string c = "", string d = "")
		{
		}

		// Token: 0x06008432 RID: 33842 RVA: 0x00322866 File Offset: 0x00320A66
		public bool IsConsoleLoggingEnabled()
		{
			return this.enableConsoleLogging || this.stateMachine.debugSettings.enableConsoleLogging;
		}

		// Token: 0x06008433 RID: 33843 RVA: 0x00322882 File Offset: 0x00320A82
		public bool IsBreakOnGoToEnabled()
		{
			return this.breakOnGoTo || this.stateMachine.debugSettings.breakOnGoTo;
		}

		// Token: 0x06008434 RID: 33844 RVA: 0x0032289E File Offset: 0x00320A9E
		public LoggerFSSSS GetLog()
		{
			return this.log;
		}

		// Token: 0x06008435 RID: 33845 RVA: 0x003228A6 File Offset: 0x00320AA6
		public StateMachine.Parameter.Context[] GetParameterContexts()
		{
			return this.parameterContexts;
		}

		// Token: 0x06008436 RID: 33846 RVA: 0x003228AE File Offset: 0x00320AAE
		public StateMachine.Parameter.Context GetParameterContext(StateMachine.Parameter parameter)
		{
			return this.parameterContexts[parameter.idx];
		}

		// Token: 0x06008437 RID: 33847 RVA: 0x003228BD File Offset: 0x00320ABD
		public StateMachine.Status GetStatus()
		{
			return this.status;
		}

		// Token: 0x06008438 RID: 33848 RVA: 0x003228C5 File Offset: 0x00320AC5
		public void SetStatus(StateMachine.Status status)
		{
			this.status = status;
		}

		// Token: 0x06008439 RID: 33849 RVA: 0x003228CE File Offset: 0x00320ACE
		public void Error()
		{
			if (!StateMachine.Instance.error)
			{
				this.isCrashed = true;
				StateMachine.Instance.error = true;
				RestartWarning.ShouldWarn = true;
			}
		}

		// Token: 0x0600843A RID: 33850 RVA: 0x003228EC File Offset: 0x00320AEC
		public override string ToString()
		{
			string str = "";
			if (this.GetCurrentState() != null)
			{
				str = this.GetCurrentState().name;
			}
			else if (this.GetStatus() != StateMachine.Status.Initialized)
			{
				str = this.GetStatus().ToString();
			}
			return this.stateMachine.ToString() + "(" + str + ")";
		}

		// Token: 0x0600843B RID: 33851 RVA: 0x00322950 File Offset: 0x00320B50
		public virtual void StartSM()
		{
			if (!this.IsRunning())
			{
				StateMachineController component = this.GetComponent<StateMachineController>();
				MyAttributes.OnStart(this, component);
				StateMachine.BaseState defaultState = this.stateMachine.GetDefaultState();
				DebugUtil.Assert(defaultState != null);
				if (!component.Restore(this))
				{
					this.GoTo(defaultState);
				}
			}
		}

		// Token: 0x0600843C RID: 33852 RVA: 0x00322998 File Offset: 0x00320B98
		public bool HasTag(Tag tag)
		{
			return this.GetComponent<KPrefabID>().HasTag(tag);
		}

		// Token: 0x0600843D RID: 33853 RVA: 0x003229A8 File Offset: 0x00320BA8
		public bool IsInsideState(StateMachine.BaseState state)
		{
			StateMachine.BaseState currentState = this.GetCurrentState();
			if (currentState == null)
			{
				return false;
			}
			bool flag = state == currentState;
			int num = 0;
			while (!flag && num < currentState.branch.Length && !(flag = (state == currentState.branch[num])))
			{
				num++;
			}
			return flag;
		}

		// Token: 0x0600843E RID: 33854 RVA: 0x003229EC File Offset: 0x00320BEC
		public void ScheduleGoTo(float time, StateMachine.BaseState state)
		{
			if (this.scheduleGoToCallback == null)
			{
				this.scheduleGoToCallback = delegate(object d)
				{
					this.GoTo((StateMachine.BaseState)d);
				};
			}
			this.Schedule(time, this.scheduleGoToCallback, state);
		}

		// Token: 0x0600843F RID: 33855 RVA: 0x00322A17 File Offset: 0x00320C17
		public void Subscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Subscribe(hash, handler);
		}

		// Token: 0x06008440 RID: 33856 RVA: 0x00322A27 File Offset: 0x00320C27
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Unsubscribe(hash, handler);
		}

		// Token: 0x06008441 RID: 33857 RVA: 0x00322A36 File Offset: 0x00320C36
		public void Trigger(int hash, object data = null)
		{
			this.GetMaster().GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x06008442 RID: 33858 RVA: 0x00322A4A File Offset: 0x00320C4A
		public ComponentType Get<ComponentType>()
		{
			return this.GetComponent<ComponentType>();
		}

		// Token: 0x06008443 RID: 33859 RVA: 0x00322A52 File Offset: 0x00320C52
		public ComponentType GetComponent<ComponentType>()
		{
			return this.GetMaster().GetComponent<ComponentType>();
		}

		// Token: 0x06008444 RID: 33860 RVA: 0x00322A60 File Offset: 0x00320C60
		private void CreateParameterContexts()
		{
			this.parameterContexts = new StateMachine.Parameter.Context[this.stateMachine.parameters.Length];
			for (int i = 0; i < this.stateMachine.parameters.Length; i++)
			{
				this.parameterContexts[i] = this.stateMachine.parameters[i].CreateContext();
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06008445 RID: 33861 RVA: 0x00322AB7 File Offset: 0x00320CB7
		public GameObject gameObject
		{
			get
			{
				return this.GetMaster().gameObject;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06008446 RID: 33862 RVA: 0x00322AC4 File Offset: 0x00320CC4
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x040063B2 RID: 25522
		public string serializationSuffix;

		// Token: 0x040063B3 RID: 25523
		protected LoggerFSSSS log;

		// Token: 0x040063B4 RID: 25524
		protected StateMachine.Status status;

		// Token: 0x040063B5 RID: 25525
		protected StateMachine stateMachine;

		// Token: 0x040063B6 RID: 25526
		protected Stack<StateEvent.Context> subscribedEvents = new Stack<StateEvent.Context>();

		// Token: 0x040063B7 RID: 25527
		protected int stackSize;

		// Token: 0x040063B8 RID: 25528
		protected StateMachine.Parameter.Context[] parameterContexts;

		// Token: 0x040063B9 RID: 25529
		public object[] dataTable;

		// Token: 0x040063BA RID: 25530
		public StateMachine.Instance.UpdateTableEntry[] updateTable;

		// Token: 0x040063BB RID: 25531
		private Action<object> scheduleGoToCallback;

		// Token: 0x040063BC RID: 25532
		public Action<string, StateMachine.Status> OnStop;

		// Token: 0x040063BD RID: 25533
		public bool breakOnGoTo;

		// Token: 0x040063BE RID: 25534
		public bool enableConsoleLogging;

		// Token: 0x040063BF RID: 25535
		public bool isCrashed;

		// Token: 0x040063C0 RID: 25536
		public static bool error;

		// Token: 0x0200247A RID: 9338
		public struct UpdateTableEntry
		{
			// Token: 0x0400A217 RID: 41495
			public HandleVector<int>.Handle handle;

			// Token: 0x0400A218 RID: 41496
			public StateMachineUpdater.BaseUpdateBucket bucket;
		}
	}

	// Token: 0x02001290 RID: 4752
	[DebuggerDisplay("{longName}")]
	public class BaseState
	{
		// Token: 0x06008448 RID: 33864 RVA: 0x00322ADF File Offset: 0x00320CDF
		public BaseState()
		{
			this.branch = new StateMachine.BaseState[1];
			this.branch[0] = this;
		}

		// Token: 0x06008449 RID: 33865 RVA: 0x00322AFC File Offset: 0x00320CFC
		public void FreeResources()
		{
			if (this.name == null)
			{
				return;
			}
			this.name = null;
			if (this.defaultState != null)
			{
				this.defaultState.FreeResources();
			}
			this.defaultState = null;
			this.events = null;
			int num = 0;
			while (this.transitions != null && num < this.transitions.Count)
			{
				this.transitions[num].Clear();
				num++;
			}
			this.transitions = null;
			this.enterActions = null;
			this.exitActions = null;
			if (this.branch != null)
			{
				for (int i = 0; i < this.branch.Length; i++)
				{
					this.branch[i].FreeResources();
				}
			}
			this.branch = null;
			this.parent = null;
		}

		// Token: 0x0600844A RID: 33866 RVA: 0x00322BB4 File Offset: 0x00320DB4
		public int GetStateCount()
		{
			return this.branch.Length;
		}

		// Token: 0x0600844B RID: 33867 RVA: 0x00322BBE File Offset: 0x00320DBE
		public StateMachine.BaseState GetState(int idx)
		{
			return this.branch[idx];
		}

		// Token: 0x040063C1 RID: 25537
		public string name;

		// Token: 0x040063C2 RID: 25538
		public string longName;

		// Token: 0x040063C3 RID: 25539
		public string debugPushName;

		// Token: 0x040063C4 RID: 25540
		public string debugPopName;

		// Token: 0x040063C5 RID: 25541
		public string debugExecuteName;

		// Token: 0x040063C6 RID: 25542
		public StateMachine.BaseState defaultState;

		// Token: 0x040063C7 RID: 25543
		public List<StateEvent> events;

		// Token: 0x040063C8 RID: 25544
		public List<StateMachine.BaseTransition> transitions;

		// Token: 0x040063C9 RID: 25545
		public List<StateMachine.UpdateAction> updateActions;

		// Token: 0x040063CA RID: 25546
		public List<StateMachine.Action> enterActions;

		// Token: 0x040063CB RID: 25547
		public List<StateMachine.Action> exitActions;

		// Token: 0x040063CC RID: 25548
		public StateMachine.BaseState[] branch;

		// Token: 0x040063CD RID: 25549
		public StateMachine.BaseState parent;
	}

	// Token: 0x02001291 RID: 4753
	public class BaseTransition
	{
		// Token: 0x0600844C RID: 33868 RVA: 0x00322BC8 File Offset: 0x00320DC8
		public BaseTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state)
		{
			this.idx = idx;
			this.name = name;
			this.sourceState = source_state;
			this.targetState = target_state;
		}

		// Token: 0x0600844D RID: 33869 RVA: 0x00322BED File Offset: 0x00320DED
		public virtual void Evaluate(StateMachine.Instance smi)
		{
		}

		// Token: 0x0600844E RID: 33870 RVA: 0x00322BEF File Offset: 0x00320DEF
		public virtual StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			return new StateMachine.BaseTransition.Context(this);
		}

		// Token: 0x0600844F RID: 33871 RVA: 0x00322BF7 File Offset: 0x00320DF7
		public virtual void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
		}

		// Token: 0x06008450 RID: 33872 RVA: 0x00322BF9 File Offset: 0x00320DF9
		public void Clear()
		{
			this.name = null;
			if (this.sourceState != null)
			{
				this.sourceState.FreeResources();
			}
			this.sourceState = null;
			if (this.targetState != null)
			{
				this.targetState.FreeResources();
			}
			this.targetState = null;
		}

		// Token: 0x040063CE RID: 25550
		public int idx;

		// Token: 0x040063CF RID: 25551
		public string name;

		// Token: 0x040063D0 RID: 25552
		public StateMachine.BaseState sourceState;

		// Token: 0x040063D1 RID: 25553
		public StateMachine.BaseState targetState;

		// Token: 0x0200247B RID: 9339
		public struct Context
		{
			// Token: 0x0600B9E9 RID: 47593 RVA: 0x003D283C File Offset: 0x003D0A3C
			public Context(StateMachine.BaseTransition transition)
			{
				this.idx = transition.idx;
				this.handlerId = 0;
			}

			// Token: 0x0400A219 RID: 41497
			public int idx;

			// Token: 0x0400A21A RID: 41498
			public int handlerId;
		}
	}

	// Token: 0x02001292 RID: 4754
	public struct UpdateAction
	{
		// Token: 0x040063D2 RID: 25554
		public int updateTableIdx;

		// Token: 0x040063D3 RID: 25555
		public UpdateRate updateRate;

		// Token: 0x040063D4 RID: 25556
		public int nextBucketIdx;

		// Token: 0x040063D5 RID: 25557
		public StateMachineUpdater.BaseUpdateBucket[] buckets;

		// Token: 0x040063D6 RID: 25558
		public object updater;
	}

	// Token: 0x02001293 RID: 4755
	public struct Action
	{
		// Token: 0x06008451 RID: 33873 RVA: 0x00322C36 File Offset: 0x00320E36
		public Action(string name, object callback)
		{
			this.name = name;
			this.callback = callback;
		}

		// Token: 0x040063D7 RID: 25559
		public string name;

		// Token: 0x040063D8 RID: 25560
		public object callback;
	}

	// Token: 0x02001294 RID: 4756
	public class ParameterTransition : StateMachine.BaseTransition
	{
		// Token: 0x06008452 RID: 33874 RVA: 0x00322C46 File Offset: 0x00320E46
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x02001295 RID: 4757
	public abstract class Parameter
	{
		// Token: 0x06008453 RID: 33875
		public abstract StateMachine.Parameter.Context CreateContext();

		// Token: 0x040063D9 RID: 25561
		public string name;

		// Token: 0x040063DA RID: 25562
		public int idx;

		// Token: 0x0200247C RID: 9340
		public abstract class Context
		{
			// Token: 0x0600B9EA RID: 47594 RVA: 0x003D2851 File Offset: 0x003D0A51
			public Context(StateMachine.Parameter parameter)
			{
				this.parameter = parameter;
			}

			// Token: 0x0600B9EB RID: 47595
			public abstract void Serialize(BinaryWriter writer);

			// Token: 0x0600B9EC RID: 47596
			public abstract void Deserialize(IReader reader, StateMachine.Instance smi);

			// Token: 0x0600B9ED RID: 47597 RVA: 0x003D2860 File Offset: 0x003D0A60
			public virtual void Cleanup()
			{
			}

			// Token: 0x0600B9EE RID: 47598
			public abstract void ShowEditor(StateMachine.Instance base_smi);

			// Token: 0x0600B9EF RID: 47599
			public abstract void ShowDevTool(StateMachine.Instance base_smi);

			// Token: 0x0400A21B RID: 41499
			public StateMachine.Parameter parameter;
		}
	}

	// Token: 0x02001296 RID: 4758
	public enum SerializeType
	{
		// Token: 0x040063DC RID: 25564
		Never,
		// Token: 0x040063DD RID: 25565
		ParamsOnly,
		// Token: 0x040063DE RID: 25566
		CurrentStateOnly_DEPRECATED,
		// Token: 0x040063DF RID: 25567
		Both_DEPRECATED
	}
}

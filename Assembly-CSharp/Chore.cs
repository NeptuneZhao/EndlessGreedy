using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public abstract class Chore
{
	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06001790 RID: 6032
	// (set) Token: 0x06001791 RID: 6033
	public abstract int id { get; protected set; }

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06001792 RID: 6034
	// (set) Token: 0x06001793 RID: 6035
	public abstract int priorityMod { get; protected set; }

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06001794 RID: 6036
	// (set) Token: 0x06001795 RID: 6037
	public abstract ChoreType choreType { get; protected set; }

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06001796 RID: 6038
	// (set) Token: 0x06001797 RID: 6039
	public abstract ChoreDriver driver { get; protected set; }

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06001798 RID: 6040
	// (set) Token: 0x06001799 RID: 6041
	public abstract ChoreDriver lastDriver { get; protected set; }

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x0600179A RID: 6042
	public abstract bool isNull { get; }

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600179B RID: 6043
	public abstract GameObject gameObject { get; }

	// Token: 0x0600179C RID: 6044
	public abstract bool SatisfiesUrge(Urge urge);

	// Token: 0x0600179D RID: 6045
	public abstract bool IsValid();

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600179E RID: 6046
	// (set) Token: 0x0600179F RID: 6047
	public abstract IStateMachineTarget target { get; protected set; }

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060017A0 RID: 6048
	// (set) Token: 0x060017A1 RID: 6049
	public abstract bool isComplete { get; protected set; }

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060017A2 RID: 6050
	// (set) Token: 0x060017A3 RID: 6051
	public abstract bool IsPreemptable { get; protected set; }

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060017A4 RID: 6052
	// (set) Token: 0x060017A5 RID: 6053
	public abstract ChoreConsumer overrideTarget { get; protected set; }

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060017A6 RID: 6054
	// (set) Token: 0x060017A7 RID: 6055
	public abstract Prioritizable prioritizable { get; protected set; }

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060017A8 RID: 6056
	// (set) Token: 0x060017A9 RID: 6057
	public abstract ChoreProvider provider { get; set; }

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060017AA RID: 6058
	// (set) Token: 0x060017AB RID: 6059
	public abstract bool runUntilComplete { get; set; }

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060017AC RID: 6060
	// (set) Token: 0x060017AD RID: 6061
	public abstract bool isExpanded { get; protected set; }

	// Token: 0x060017AE RID: 6062
	public abstract List<Chore.PreconditionInstance> GetPreconditions();

	// Token: 0x060017AF RID: 6063
	public abstract bool CanPreempt(Chore.Precondition.Context context);

	// Token: 0x060017B0 RID: 6064
	public abstract void PrepareChore(ref Chore.Precondition.Context context);

	// Token: 0x060017B1 RID: 6065
	public abstract void Cancel(string reason);

	// Token: 0x060017B2 RID: 6066
	public abstract ReportManager.ReportType GetReportType();

	// Token: 0x060017B3 RID: 6067
	public abstract string GetReportName(string context = null);

	// Token: 0x060017B4 RID: 6068
	public abstract void AddPrecondition(Chore.Precondition precondition, object data = null);

	// Token: 0x060017B5 RID: 6069
	public abstract void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override);

	// Token: 0x060017B6 RID: 6070 RVA: 0x0007FDC0 File Offset: 0x0007DFC0
	public void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		this.CollectChores(consumer_state, succeeded_contexts, null, failed_contexts, is_attempting_override);
	}

	// Token: 0x060017B7 RID: 6071
	public abstract void Cleanup();

	// Token: 0x060017B8 RID: 6072
	public abstract void Fail(string reason);

	// Token: 0x060017B9 RID: 6073
	public abstract void Begin(Chore.Precondition.Context context);

	// Token: 0x060017BA RID: 6074
	public abstract bool InProgress();

	// Token: 0x060017BB RID: 6075 RVA: 0x0007FDCE File Offset: 0x0007DFCE
	public virtual string ResolveString(string str)
	{
		return str;
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x0007FDD1 File Offset: 0x0007DFD1
	public static int GetNextChoreID()
	{
		return ++Chore.nextId;
	}

	// Token: 0x04000D25 RID: 3365
	public PrioritySetting masterPriority;

	// Token: 0x04000D26 RID: 3366
	public bool showAvailabilityInHoverText = true;

	// Token: 0x04000D27 RID: 3367
	public Action<Chore> onExit;

	// Token: 0x04000D28 RID: 3368
	public Action<Chore> onComplete;

	// Token: 0x04000D29 RID: 3369
	private static int nextId;

	// Token: 0x04000D2A RID: 3370
	public const int MAX_PLAYER_BASIC_PRIORITY = 9;

	// Token: 0x04000D2B RID: 3371
	public const int MIN_PLAYER_BASIC_PRIORITY = 1;

	// Token: 0x04000D2C RID: 3372
	public const int MAX_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x04000D2D RID: 3373
	public const int MIN_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x04000D2E RID: 3374
	public const int MAX_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x04000D2F RID: 3375
	public const int MIN_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x04000D30 RID: 3376
	public const int DEFAULT_BASIC_PRIORITY = 5;

	// Token: 0x04000D31 RID: 3377
	public const int MAX_BASIC_PRIORITY = 10;

	// Token: 0x04000D32 RID: 3378
	public const int MIN_BASIC_PRIORITY = 0;

	// Token: 0x04000D33 RID: 3379
	public static bool ENABLE_PERSONAL_PRIORITIES = true;

	// Token: 0x04000D34 RID: 3380
	public static PrioritySetting DefaultPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x02001217 RID: 4631
	// (Invoke) Token: 0x06008226 RID: 33318
	public delegate bool PreconditionFn(ref Chore.Precondition.Context context, object data);

	// Token: 0x02001218 RID: 4632
	public struct PreconditionInstance
	{
		// Token: 0x0400625C RID: 25180
		public Chore.Precondition condition;

		// Token: 0x0400625D RID: 25181
		public object data;
	}

	// Token: 0x02001219 RID: 4633
	public struct Precondition
	{
		// Token: 0x0400625E RID: 25182
		public string id;

		// Token: 0x0400625F RID: 25183
		public string description;

		// Token: 0x04006260 RID: 25184
		public int sortOrder;

		// Token: 0x04006261 RID: 25185
		public Chore.PreconditionFn fn;

		// Token: 0x04006262 RID: 25186
		public bool canExecuteOnAnyThread;

		// Token: 0x020023F7 RID: 9207
		[DebuggerDisplay("{chore.GetType()}, {chore.gameObject.name}")]
		public struct Context : IComparable<Chore.Precondition.Context>, IEquatable<Chore.Precondition.Context>
		{
			// Token: 0x0600B86E RID: 47214 RVA: 0x003CEC14 File Offset: 0x003CCE14
			public Context(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.personalPriority = consumer_state.consumer.GetPersonalPriority(chore.choreType);
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.skippedPreconditions = false;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.skipMoreSatisfyingEarlyPrecondition = (RootMenu.Instance != null && RootMenu.Instance.IsBuildingChorePanelActive());
				this.SetPriority(chore);
			}

			// Token: 0x0600B86F RID: 47215 RVA: 0x003CECCC File Offset: 0x003CCECC
			public void Set(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.skippedPreconditions = false;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.SetPriority(chore);
			}

			// Token: 0x0600B870 RID: 47216 RVA: 0x003CED4C File Offset: 0x003CCF4C
			public void SetPriority(Chore chore)
			{
				this.priority = (Game.Instance.advancedPersonalPriorities ? chore.choreType.explicitPriority : chore.choreType.priority);
				this.priorityMod = chore.priorityMod;
				this.interruptPriority = chore.choreType.interruptPriority;
			}

			// Token: 0x0600B871 RID: 47217 RVA: 0x003CEDA0 File Offset: 0x003CCFA0
			public bool IsSuccess()
			{
				return this.failedPreconditionId == -1 && !this.skippedPreconditions;
			}

			// Token: 0x0600B872 RID: 47218 RVA: 0x003CEDB6 File Offset: 0x003CCFB6
			public bool IsComplete()
			{
				return !this.skippedPreconditions;
			}

			// Token: 0x0600B873 RID: 47219 RVA: 0x003CEDC4 File Offset: 0x003CCFC4
			public bool IsPotentialSuccess()
			{
				if (this.IsSuccess())
				{
					return true;
				}
				if (this.chore.driver == this.consumerState.choreDriver)
				{
					return true;
				}
				if (this.failedPreconditionId != -1)
				{
					if (this.failedPreconditionId >= 0 && this.failedPreconditionId < this.chore.GetPreconditions().Count)
					{
						return this.chore.GetPreconditions()[this.failedPreconditionId].condition.id == ChorePreconditions.instance.IsMoreSatisfyingLate.id;
					}
					DebugUtil.DevLogErrorFormat("failedPreconditionId out of range {0}/{1}", new object[]
					{
						this.failedPreconditionId,
						this.chore.GetPreconditions().Count
					});
				}
				return false;
			}

			// Token: 0x0600B874 RID: 47220 RVA: 0x003CEE94 File Offset: 0x003CD094
			private void DoPreconditions(bool mainThreadOnly)
			{
				bool flag = Game.IsOnMainThread();
				List<Chore.PreconditionInstance> preconditions = this.chore.GetPreconditions();
				this.skippedPreconditions = false;
				int i = 0;
				while (i < preconditions.Count)
				{
					Chore.PreconditionInstance preconditionInstance = preconditions[i];
					if (preconditionInstance.condition.canExecuteOnAnyThread)
					{
						if (!mainThreadOnly)
						{
							goto IL_43;
						}
					}
					else
					{
						if (flag)
						{
							goto IL_43;
						}
						this.skippedPreconditions = true;
					}
					IL_6B:
					i++;
					continue;
					IL_43:
					if (!preconditionInstance.condition.fn(ref this, preconditionInstance.data))
					{
						this.failedPreconditionId = i;
						this.skippedPreconditions = false;
						return;
					}
					goto IL_6B;
				}
			}

			// Token: 0x0600B875 RID: 47221 RVA: 0x003CEF19 File Offset: 0x003CD119
			public void RunPreconditions()
			{
				this.DoPreconditions(false);
			}

			// Token: 0x0600B876 RID: 47222 RVA: 0x003CEF22 File Offset: 0x003CD122
			public void FinishPreconditions()
			{
				this.DoPreconditions(true);
			}

			// Token: 0x0600B877 RID: 47223 RVA: 0x003CEF2C File Offset: 0x003CD12C
			public int CompareTo(Chore.Precondition.Context obj)
			{
				bool flag = this.failedPreconditionId != -1;
				bool flag2 = obj.failedPreconditionId != -1;
				if (flag == flag2)
				{
					int num = this.masterPriority.priority_class - obj.masterPriority.priority_class;
					if (num != 0)
					{
						return num;
					}
					int num2 = this.personalPriority - obj.personalPriority;
					if (num2 != 0)
					{
						return num2;
					}
					int num3 = this.masterPriority.priority_value - obj.masterPriority.priority_value;
					if (num3 != 0)
					{
						return num3;
					}
					int num4 = this.priority - obj.priority;
					if (num4 != 0)
					{
						return num4;
					}
					int num5 = this.priorityMod - obj.priorityMod;
					if (num5 != 0)
					{
						return num5;
					}
					int num6 = this.consumerPriority - obj.consumerPriority;
					if (num6 != 0)
					{
						return num6;
					}
					int num7 = obj.cost - this.cost;
					if (num7 != 0)
					{
						return num7;
					}
					if (this.chore == null && obj.chore == null)
					{
						return 0;
					}
					if (this.chore == null)
					{
						return -1;
					}
					if (obj.chore == null)
					{
						return 1;
					}
					return this.chore.id - obj.chore.id;
				}
				else
				{
					if (!flag)
					{
						return 1;
					}
					return -1;
				}
			}

			// Token: 0x0600B878 RID: 47224 RVA: 0x003CF048 File Offset: 0x003CD248
			public override bool Equals(object obj)
			{
				Chore.Precondition.Context obj2 = (Chore.Precondition.Context)obj;
				return this.CompareTo(obj2) == 0;
			}

			// Token: 0x0600B879 RID: 47225 RVA: 0x003CF066 File Offset: 0x003CD266
			public bool Equals(Chore.Precondition.Context other)
			{
				return this.CompareTo(other) == 0;
			}

			// Token: 0x0600B87A RID: 47226 RVA: 0x003CF072 File Offset: 0x003CD272
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x0600B87B RID: 47227 RVA: 0x003CF084 File Offset: 0x003CD284
			public static bool operator ==(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) == 0;
			}

			// Token: 0x0600B87C RID: 47228 RVA: 0x003CF091 File Offset: 0x003CD291
			public static bool operator !=(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) != 0;
			}

			// Token: 0x0600B87D RID: 47229 RVA: 0x003CF09E File Offset: 0x003CD29E
			public static bool ShouldFilter(string filter, string text)
			{
				return !string.IsNullOrEmpty(filter) && (string.IsNullOrEmpty(text) || text.ToLower().IndexOf(filter) < 0);
			}

			// Token: 0x0400A090 RID: 41104
			public PrioritySetting masterPriority;

			// Token: 0x0400A091 RID: 41105
			public int personalPriority;

			// Token: 0x0400A092 RID: 41106
			public int priority;

			// Token: 0x0400A093 RID: 41107
			public int priorityMod;

			// Token: 0x0400A094 RID: 41108
			public int interruptPriority;

			// Token: 0x0400A095 RID: 41109
			public int cost;

			// Token: 0x0400A096 RID: 41110
			public int consumerPriority;

			// Token: 0x0400A097 RID: 41111
			public Chore chore;

			// Token: 0x0400A098 RID: 41112
			public ChoreConsumerState consumerState;

			// Token: 0x0400A099 RID: 41113
			public int failedPreconditionId;

			// Token: 0x0400A09A RID: 41114
			public bool skippedPreconditions;

			// Token: 0x0400A09B RID: 41115
			public object data;

			// Token: 0x0400A09C RID: 41116
			public bool isAttemptingOverride;

			// Token: 0x0400A09D RID: 41117
			public ChoreType choreTypeForPermission;

			// Token: 0x0400A09E RID: 41118
			public bool skipMoreSatisfyingEarlyPrecondition;
		}
	}
}

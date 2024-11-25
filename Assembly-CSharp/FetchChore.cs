using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class FetchChore : Chore<FetchChore.StatesInstance>
{
	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06001703 RID: 5891 RVA: 0x0007C465 File Offset: 0x0007A665
	public float originalAmount
	{
		get
		{
			return base.smi.sm.requestedamount.Get(base.smi);
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06001704 RID: 5892 RVA: 0x0007C482 File Offset: 0x0007A682
	// (set) Token: 0x06001705 RID: 5893 RVA: 0x0007C49F File Offset: 0x0007A69F
	public float amount
	{
		get
		{
			return base.smi.sm.actualamount.Get(base.smi);
		}
		set
		{
			base.smi.sm.actualamount.Set(value, base.smi, false);
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06001706 RID: 5894 RVA: 0x0007C4BF File Offset: 0x0007A6BF
	// (set) Token: 0x06001707 RID: 5895 RVA: 0x0007C4DC File Offset: 0x0007A6DC
	public Pickupable fetchTarget
	{
		get
		{
			return base.smi.sm.chunk.Get<Pickupable>(base.smi);
		}
		set
		{
			base.smi.sm.chunk.Set(value, base.smi);
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06001708 RID: 5896 RVA: 0x0007C4FA File Offset: 0x0007A6FA
	// (set) Token: 0x06001709 RID: 5897 RVA: 0x0007C517 File Offset: 0x0007A717
	public GameObject fetcher
	{
		get
		{
			return base.smi.sm.fetcher.Get(base.smi);
		}
		set
		{
			base.smi.sm.fetcher.Set(value, base.smi, false);
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600170A RID: 5898 RVA: 0x0007C537 File Offset: 0x0007A737
	// (set) Token: 0x0600170B RID: 5899 RVA: 0x0007C53F File Offset: 0x0007A73F
	public Storage destination { get; private set; }

	// Token: 0x0600170C RID: 5900 RVA: 0x0007C548 File Offset: 0x0007A748
	public void FetchAreaBegin(Chore.Precondition.Context context, float amount_to_be_fetched)
	{
		this.amount = amount_to_be_fetched;
		base.smi.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, context.chore.choreType.Name, GameUtil.GetChoreName(this, context.data));
		base.Begin(context);
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x0007C5B8 File Offset: 0x0007A7B8
	public void FetchAreaEnd(ChoreDriver driver, Pickupable pickupable, bool is_success)
	{
		if (is_success)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, pickupable));
			this.fetchTarget = pickupable;
			this.driver = driver;
			this.fetcher = driver.gameObject;
			base.Succeed("FetchAreaEnd");
			SaveGame.Instance.ColonyAchievementTracker.LogFetchChore(this.fetcher, this.choreType);
			return;
		}
		base.SetOverrideTarget(null);
		this.Fail("FetchAreaFail");
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x0007C640 File Offset: 0x0007A840
	public Pickupable FindFetchTarget(ChoreConsumerState consumer_state)
	{
		if (!(this.destination != null))
		{
			return null;
		}
		if (consumer_state.hasSolidTransferArm)
		{
			return consumer_state.solidTransferArm.FindFetchTarget(this.destination, this);
		}
		return Game.Instance.fetchManager.FindFetchTarget(this.destination, this);
	}

	// Token: 0x0600170F RID: 5903 RVA: 0x0007C690 File Offset: 0x0007A890
	public override void Begin(Chore.Precondition.Context context)
	{
		Pickupable pickupable = (Pickupable)context.data;
		if (pickupable == null)
		{
			pickupable = this.FindFetchTarget(context.consumerState);
		}
		base.smi.sm.source.Set(pickupable.gameObject, base.smi, false);
		pickupable.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		base.Begin(context);
	}

	// Token: 0x06001710 RID: 5904 RVA: 0x0007C704 File Offset: 0x0007A904
	protected override void End(string reason)
	{
		Pickupable pickupable = base.smi.sm.source.Get<Pickupable>(base.smi);
		if (pickupable != null)
		{
			pickupable.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
		base.End(reason);
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x0007C754 File Offset: 0x0007A954
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.chunk.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x0007C784 File Offset: 0x0007A984
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
		context.chore = new FetchAreaChore(context);
	}

	// Token: 0x06001713 RID: 5907 RVA: 0x0007C797 File Offset: 0x0007A997
	public float AmountWaitingToFetch()
	{
		if (this.fetcher == null)
		{
			return this.originalAmount;
		}
		return this.amount;
	}

	// Token: 0x06001714 RID: 5908 RVA: 0x0007C7B4 File Offset: 0x0007A9B4
	public FetchChore(ChoreType choreType, Storage destination, float amount, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags = null, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, Operational.State operational_requirement = Operational.State.Operational, int priority_mod = 0) : base(choreType, destination, chore_provider, run_until_complete, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.basic, 5, false, true, priority_mod, false, ReportManager.ReportType.WorkTime)
	{
		if (choreType == null)
		{
			global::Debug.LogError("You must specify a chore type for fetching!");
		}
		this.tagsFirst = ((tags.Count > 0) ? tags.First<Tag>() : Tag.Invalid);
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Chore {0} is requesting {1} {2} to {3}", new object[]
				{
					choreType.Id,
					this.tagsFirst,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		base.SetPrioritizable((destination.prioritizable != null) ? destination.prioritizable : destination.GetComponent<Prioritizable>());
		base.smi = new FetchChore.StatesInstance(this);
		base.smi.sm.requestedamount.Set(amount, base.smi, false);
		this.destination = destination;
		DebugUtil.DevAssert(criteria != FetchChore.MatchCriteria.MatchTags || tags.Count <= 1, "For performance reasons fetch chores are limited to one tag when matching tags!", null);
		this.tags = tags;
		this.criteria = criteria;
		this.tagsHash = FetchChore.ComputeHashCodeForTags(tags);
		this.requiredTag = required_tag;
		this.forbiddenTags = ((forbidden_tags != null) ? forbidden_tags : new Tag[0]);
		this.forbidHash = FetchChore.ComputeHashCodeForTags(this.forbiddenTags);
		DebugUtil.DevAssert(!tags.Contains(GameTags.Preserved), "Fetch chore fetching invalid tags.", null);
		if (destination.GetOnlyFetchMarkedItems())
		{
			DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
			this.requiredTag = GameTags.Garbage;
		}
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, destination);
		this.AddPrecondition(FetchChore.IsFetchTargetAvailable, null);
		Deconstructable component = this.target.GetComponent<Deconstructable>();
		if (component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
		}
		BuildingEnabledButton component2 = this.target.GetComponent<BuildingEnabledButton>();
		if (component2 != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component2);
		}
		if (operational_requirement != Operational.State.None)
		{
			Operational component3 = destination.GetComponent<Operational>();
			if (component3 != null)
			{
				Chore.Precondition precondition = ChorePreconditions.instance.IsOperational;
				if (operational_requirement == Operational.State.Functional)
				{
					precondition = ChorePreconditions.instance.IsFunctional;
				}
				this.AddPrecondition(precondition, component3);
			}
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add(destination.name, this, Grid.PosToCell(destination), GameScenePartitioner.Instance.fetchChoreLayer, null);
		destination.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.automatable = destination.GetComponent<Automatable>();
		if (this.automatable)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsAllowedByAutomation, this.automatable);
		}
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x0007CAA0 File Offset: 0x0007ACA0
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		if (this.destination != null)
		{
			if (this.destination.GetOnlyFetchMarkedItems())
			{
				DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
				this.requiredTag = GameTags.Garbage;
				return;
			}
			this.requiredTag = Tag.Invalid;
		}
	}

	// Token: 0x06001716 RID: 5910 RVA: 0x0007CAF8 File Offset: 0x0007ACF8
	private void OnMasterPriorityChanged(PriorityScreen.PriorityClass priorityClass, int priority_value)
	{
		this.masterPriority.priority_class = priorityClass;
		this.masterPriority.priority_value = priority_value;
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x0007CB12 File Offset: 0x0007AD12
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x0007CB14 File Offset: 0x0007AD14
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		this.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded_contexts, null, failed_contexts, is_attempting_override);
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x0007CB22 File Offset: 0x0007AD22
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		base.CollectChores(consumer_state, succeeded_contexts, incomplete_contexts, failed_contexts, is_attempting_override);
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x0007CB34 File Offset: 0x0007AD34
	public override void Cleanup()
	{
		base.Cleanup();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.destination != null)
		{
			this.destination.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x0007CB84 File Offset: 0x0007AD84
	public static int ComputeHashCodeForTags(IEnumerable<Tag> tags)
	{
		int num = 123137;
		foreach (Tag tag in new SortedSet<Tag>(tags))
		{
			num = ((num << 5) + num ^ tag.GetHash());
		}
		return num;
	}

	// Token: 0x04000CF8 RID: 3320
	public HashSet<Tag> tags;

	// Token: 0x04000CF9 RID: 3321
	public Tag tagsFirst;

	// Token: 0x04000CFA RID: 3322
	public FetchChore.MatchCriteria criteria;

	// Token: 0x04000CFB RID: 3323
	public int tagsHash;

	// Token: 0x04000CFC RID: 3324
	public bool validateRequiredTagOnTagChange;

	// Token: 0x04000CFD RID: 3325
	public Tag requiredTag;

	// Token: 0x04000CFE RID: 3326
	public Tag[] forbiddenTags;

	// Token: 0x04000CFF RID: 3327
	public int forbidHash;

	// Token: 0x04000D00 RID: 3328
	public Automatable automatable;

	// Token: 0x04000D01 RID: 3329
	public bool allowMultifetch = true;

	// Token: 0x04000D02 RID: 3330
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000D03 RID: 3331
	public static readonly Chore.Precondition IsFetchTargetAvailable = new Chore.Precondition
	{
		id = "IsFetchTargetAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			FetchChore fetchChore = (FetchChore)context.chore;
			Pickupable pickupable = (Pickupable)context.data;
			bool flag;
			if (pickupable == null)
			{
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
				flag = (pickupable != null);
			}
			else
			{
				flag = FetchManager.IsFetchablePickup(pickupable, fetchChore, context.consumerState.storage);
			}
			if (flag)
			{
				if (pickupable == null)
				{
					global::Debug.Log(string.Format("Failed to find fetch target for {0}", fetchChore.destination));
					return false;
				}
				context.data = pickupable;
				int num;
				if (context.consumerState.consumer.GetNavigationCost(pickupable, out num))
				{
					context.cost += num;
					return true;
				}
			}
			return false;
		}
	};

	// Token: 0x020011BB RID: 4539
	public enum MatchCriteria
	{
		// Token: 0x04006137 RID: 24887
		MatchID,
		// Token: 0x04006138 RID: 24888
		MatchTags
	}

	// Token: 0x020011BC RID: 4540
	public class StatesInstance : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.GameInstance
	{
		// Token: 0x060080F7 RID: 33015 RVA: 0x003149E4 File Offset: 0x00312BE4
		public StatesInstance(FetchChore master) : base(master)
		{
		}
	}

	// Token: 0x020011BD RID: 4541
	public class States : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore>
	{
		// Token: 0x060080F8 RID: 33016 RVA: 0x003149ED File Offset: 0x00312BED
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
		}

		// Token: 0x04006139 RID: 24889
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter fetcher;

		// Token: 0x0400613A RID: 24890
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter source;

		// Token: 0x0400613B RID: 24891
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter chunk;

		// Token: 0x0400613C RID: 24892
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter requestedamount;

		// Token: 0x0400613D RID: 24893
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter actualamount;
	}
}

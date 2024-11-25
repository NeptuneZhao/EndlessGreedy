using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000473 RID: 1139
[DebuggerDisplay("{base.Id}")]
public abstract class GameplayEvent : Resource, IComparable<GameplayEvent>
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06001886 RID: 6278 RVA: 0x000830A0 File Offset: 0x000812A0
	// (set) Token: 0x06001887 RID: 6279 RVA: 0x000830A8 File Offset: 0x000812A8
	public int importance { get; private set; }

	// Token: 0x06001888 RID: 6280 RVA: 0x000830B4 File Offset: 0x000812B4
	public virtual bool IsAllowed()
	{
		if (this.WillNeverRunAgain())
		{
			return false;
		}
		if (!this.allowMultipleEventInstances && GameplayEventManager.Instance.IsGameplayEventActive(this))
		{
			return false;
		}
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (gameplayEventPrecondition.required && !gameplayEventPrecondition.condition())
			{
				return false;
			}
		}
		float sleepTimer = GameplayEventManager.Instance.GetSleepTimer(this);
		return GameUtil.GetCurrentTimeInCycles() >= sleepTimer;
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x00083154 File Offset: 0x00081354
	public void SetSleepTimer(float timeToSleepUntil)
	{
		GameplayEventManager.Instance.SetSleepTimerForEvent(this, timeToSleepUntil);
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x00083162 File Offset: 0x00081362
	public virtual bool WillNeverRunAgain()
	{
		return this.numTimesAllowed != -1 && GameplayEventManager.Instance.NumberOfPastEvents(this.Id) >= this.numTimesAllowed;
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x0008318F File Offset: 0x0008138F
	public int GetCashedPriority()
	{
		return this.calculatedPriority;
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x00083197 File Offset: 0x00081397
	public virtual int CalculatePriority()
	{
		this.calculatedPriority = this.basePriority + this.CalculateBoost();
		return this.calculatedPriority;
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x000831B4 File Offset: 0x000813B4
	public int CalculateBoost()
	{
		int num = 0;
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (!gameplayEventPrecondition.required && gameplayEventPrecondition.condition())
			{
				num += gameplayEventPrecondition.priorityModifier;
			}
		}
		return num;
	}

	// Token: 0x0600188E RID: 6286 RVA: 0x00083224 File Offset: 0x00081424
	public GameplayEvent AddPrecondition(GameplayEventPrecondition precondition)
	{
		precondition.required = true;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x0008323A File Offset: 0x0008143A
	public GameplayEvent AddPriorityBoost(GameplayEventPrecondition precondition, int priorityBoost)
	{
		precondition.required = false;
		precondition.priorityModifier = priorityBoost;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x00083257 File Offset: 0x00081457
	public GameplayEvent AddMinionFilter(GameplayEventMinionFilter filter)
	{
		this.minionFilters.Add(filter);
		return this;
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x00083266 File Offset: 0x00081466
	public GameplayEvent TrySpawnEventOnSuccess(HashedString evt)
	{
		this.successEvents.Add(evt);
		return this;
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x00083275 File Offset: 0x00081475
	public GameplayEvent TrySpawnEventOnFailure(HashedString evt)
	{
		this.failureEvents.Add(evt);
		return this;
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x00083284 File Offset: 0x00081484
	public GameplayEvent SetVisuals(HashedString animFileName)
	{
		this.animFileName = animFileName;
		return this;
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x0008328E File Offset: 0x0008148E
	public virtual Sprite GetDisplaySprite()
	{
		return null;
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x00083291 File Offset: 0x00081491
	public virtual string GetDisplayString()
	{
		return null;
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x00083294 File Offset: 0x00081494
	public MinionIdentity GetRandomFilteredMinion()
	{
		List<MinionIdentity> list = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		using (List<GameplayEventMinionFilter>.Enumerator enumerator = this.minionFilters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameplayEventMinionFilter filter = enumerator.Current;
				list.RemoveAll((MinionIdentity x) => !filter.filter(x));
			}
		}
		if (list.Count != 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		return null;
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x0008332C File Offset: 0x0008152C
	public MinionIdentity GetRandomMinionPrioritizeFiltered()
	{
		MinionIdentity randomFilteredMinion = this.GetRandomFilteredMinion();
		if (!(randomFilteredMinion == null))
		{
			return randomFilteredMinion;
		}
		return Components.LiveMinionIdentities.Items[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Items.Count)];
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x00083370 File Offset: 0x00081570
	public int CompareTo(GameplayEvent other)
	{
		return -this.GetCashedPriority().CompareTo(other.GetCashedPriority());
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x00083394 File Offset: 0x00081594
	public GameplayEvent(string id, int priority, int importance) : base(id, null, null)
	{
		this.tags = new List<Tag>();
		this.basePriority = priority;
		this.preconditions = new List<GameplayEventPrecondition>();
		this.minionFilters = new List<GameplayEventMinionFilter>();
		this.successEvents = new List<HashedString>();
		this.failureEvents = new List<HashedString>();
		this.importance = importance;
		this.animFileName = id;
	}

	// Token: 0x0600189A RID: 6298
	public abstract StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance);

	// Token: 0x0600189B RID: 6299 RVA: 0x00083404 File Offset: 0x00081604
	public GameplayEventInstance CreateInstance(int worldId)
	{
		GameplayEventInstance gameplayEventInstance = new GameplayEventInstance(this, worldId);
		if (this.tags != null)
		{
			gameplayEventInstance.tags.AddRange(this.tags);
		}
		return gameplayEventInstance;
	}

	// Token: 0x04000D9E RID: 3486
	public const int INFINITE = -1;

	// Token: 0x04000D9F RID: 3487
	public int numTimesAllowed = -1;

	// Token: 0x04000DA0 RID: 3488
	public bool allowMultipleEventInstances;

	// Token: 0x04000DA1 RID: 3489
	protected int basePriority;

	// Token: 0x04000DA2 RID: 3490
	protected int calculatedPriority;

	// Token: 0x04000DA4 RID: 3492
	public List<GameplayEventPrecondition> preconditions;

	// Token: 0x04000DA5 RID: 3493
	public List<GameplayEventMinionFilter> minionFilters;

	// Token: 0x04000DA6 RID: 3494
	public List<HashedString> successEvents;

	// Token: 0x04000DA7 RID: 3495
	public List<HashedString> failureEvents;

	// Token: 0x04000DA8 RID: 3496
	public string title;

	// Token: 0x04000DA9 RID: 3497
	public string description;

	// Token: 0x04000DAA RID: 3498
	public HashedString animFileName;

	// Token: 0x04000DAB RID: 3499
	public List<Tag> tags;
}

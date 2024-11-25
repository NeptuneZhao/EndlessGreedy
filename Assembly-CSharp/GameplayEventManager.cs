using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x020008C8 RID: 2248
public class GameplayEventManager : KMonoBehaviour
{
	// Token: 0x06003FDD RID: 16349 RVA: 0x0016A165 File Offset: 0x00168365
	public static void DestroyInstance()
	{
		GameplayEventManager.Instance = null;
	}

	// Token: 0x06003FDE RID: 16350 RVA: 0x0016A16D File Offset: 0x0016836D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameplayEventManager.Instance = this;
		this.notifier = base.GetComponent<Notifier>();
	}

	// Token: 0x06003FDF RID: 16351 RVA: 0x0016A187 File Offset: 0x00168387
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreEvents();
	}

	// Token: 0x06003FE0 RID: 16352 RVA: 0x0016A195 File Offset: 0x00168395
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameplayEventManager.Instance = null;
	}

	// Token: 0x06003FE1 RID: 16353 RVA: 0x0016A1A4 File Offset: 0x001683A4
	private void RestoreEvents()
	{
		this.activeEvents.RemoveAll((GameplayEventInstance x) => Db.Get().GameplayEvents.TryGet(x.eventID) == null);
		for (int i = this.activeEvents.Count - 1; i >= 0; i--)
		{
			GameplayEventInstance gameplayEventInstance = this.activeEvents[i];
			if (gameplayEventInstance.smi == null)
			{
				this.StartEventInstance(gameplayEventInstance, null);
			}
		}
	}

	// Token: 0x06003FE2 RID: 16354 RVA: 0x0016A211 File Offset: 0x00168411
	public void SetSleepTimerForEvent(GameplayEvent eventType, float time)
	{
		this.sleepTimers[eventType.IdHash] = time;
	}

	// Token: 0x06003FE3 RID: 16355 RVA: 0x0016A228 File Offset: 0x00168428
	public float GetSleepTimer(GameplayEvent eventType)
	{
		float num = 0f;
		this.sleepTimers.TryGetValue(eventType.IdHash, out num);
		this.sleepTimers[eventType.IdHash] = num;
		return num;
	}

	// Token: 0x06003FE4 RID: 16356 RVA: 0x0016A264 File Offset: 0x00168464
	public bool IsGameplayEventActive(GameplayEvent eventType)
	{
		return this.activeEvents.Find((GameplayEventInstance e) => e.eventID == eventType.IdHash) != null;
	}

	// Token: 0x06003FE5 RID: 16357 RVA: 0x0016A298 File Offset: 0x00168498
	public bool IsGameplayEventRunningWithTag(Tag tag)
	{
		using (List<GameplayEventInstance>.Enumerator enumerator = this.activeEvents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.tags.Contains(tag))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003FE6 RID: 16358 RVA: 0x0016A2F8 File Offset: 0x001684F8
	public void GetActiveEventsOfType<T>(int worldID, ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance gameplayEventInstance in this.activeEvents)
		{
			if (gameplayEventInstance.worldId == worldID && gameplayEventInstance.gameplayEvent is T)
			{
				results.Add(gameplayEventInstance);
			}
		}
	}

	// Token: 0x06003FE7 RID: 16359 RVA: 0x0016A36C File Offset: 0x0016856C
	public void GetActiveEventsOfType<T>(ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance gameplayEventInstance in this.activeEvents)
		{
			if (gameplayEventInstance.gameplayEvent is T)
			{
				results.Add(gameplayEventInstance);
			}
		}
	}

	// Token: 0x06003FE8 RID: 16360 RVA: 0x0016A3D8 File Offset: 0x001685D8
	private GameplayEventInstance CreateGameplayEvent(GameplayEvent gameplayEvent, int worldId)
	{
		return gameplayEvent.CreateInstance(worldId);
	}

	// Token: 0x06003FE9 RID: 16361 RVA: 0x0016A3E4 File Offset: 0x001685E4
	public GameplayEventInstance GetGameplayEventInstance(HashedString eventID, int worldId = -1)
	{
		return this.activeEvents.Find((GameplayEventInstance e) => e.eventID == eventID && (worldId == -1 || e.worldId == worldId));
	}

	// Token: 0x06003FEA RID: 16362 RVA: 0x0016A41C File Offset: 0x0016861C
	public GameplayEventInstance CreateOrGetEventInstance(GameplayEvent eventType, int worldId = -1)
	{
		GameplayEventInstance gameplayEventInstance = this.GetGameplayEventInstance(eventType.Id, worldId);
		if (gameplayEventInstance == null)
		{
			gameplayEventInstance = this.StartNewEvent(eventType, worldId, null);
		}
		return gameplayEventInstance;
	}

	// Token: 0x06003FEB RID: 16363 RVA: 0x0016A44C File Offset: 0x0016864C
	public void RemoveActiveEvent(GameplayEventInstance eventInstance, string reason = "RemoveActiveEvent() called")
	{
		GameplayEventInstance gameplayEventInstance = this.activeEvents.Find((GameplayEventInstance x) => x == eventInstance);
		if (gameplayEventInstance != null)
		{
			if (gameplayEventInstance.smi != null)
			{
				gameplayEventInstance.smi.StopSM(reason);
				return;
			}
			this.activeEvents.Remove(gameplayEventInstance);
		}
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x0016A4A4 File Offset: 0x001686A4
	public GameplayEventInstance StartNewEvent(GameplayEvent eventType, int worldId = -1, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		GameplayEventInstance gameplayEventInstance = this.CreateGameplayEvent(eventType, worldId);
		this.StartEventInstance(gameplayEventInstance, setupActionsBeforeStart);
		this.activeEvents.Add(gameplayEventInstance);
		int num;
		this.pastEvents.TryGetValue(gameplayEventInstance.eventID, out num);
		this.pastEvents[gameplayEventInstance.eventID] = num + 1;
		return gameplayEventInstance;
	}

	// Token: 0x06003FED RID: 16365 RVA: 0x0016A4F8 File Offset: 0x001686F8
	private void StartEventInstance(GameplayEventInstance gameplayEventInstance, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		StateMachine.Instance instance = gameplayEventInstance.PrepareEvent(this);
		StateMachine.Instance instance2 = instance;
		instance2.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(instance2.OnStop, new Action<string, StateMachine.Status>(delegate(string reason, StateMachine.Status status)
		{
			this.activeEvents.Remove(gameplayEventInstance);
		}));
		if (setupActionsBeforeStart != null)
		{
			setupActionsBeforeStart(instance);
		}
		gameplayEventInstance.StartEvent();
	}

	// Token: 0x06003FEE RID: 16366 RVA: 0x0016A560 File Offset: 0x00168760
	public int NumberOfPastEvents(HashedString eventID)
	{
		int result;
		this.pastEvents.TryGetValue(eventID, out result);
		return result;
	}

	// Token: 0x06003FEF RID: 16367 RVA: 0x0016A580 File Offset: 0x00168780
	public static Notification CreateStandardCancelledNotification(EventInfoData eventInfoData)
	{
		if (eventInfoData == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"eventPopup is null in CreateStandardCancelledNotification"
			});
			return null;
		}
		eventInfoData.FinalizeText();
		return new Notification(string.Format(GAMEPLAY_EVENTS.CANCELED, eventInfoData.title), NotificationType.Event, (List<Notification> list, object data) => string.Format(GAMEPLAY_EVENTS.CANCELED_TOOLTIP, eventInfoData.title), null, true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x04002A3B RID: 10811
	public static GameplayEventManager Instance;

	// Token: 0x04002A3C RID: 10812
	public Notifier notifier;

	// Token: 0x04002A3D RID: 10813
	[Serialize]
	private List<GameplayEventInstance> activeEvents = new List<GameplayEventInstance>();

	// Token: 0x04002A3E RID: 10814
	[Serialize]
	private Dictionary<HashedString, int> pastEvents = new Dictionary<HashedString, int>();

	// Token: 0x04002A3F RID: 10815
	[Serialize]
	private Dictionary<HashedString, float> sleepTimers = new Dictionary<HashedString, float>();
}

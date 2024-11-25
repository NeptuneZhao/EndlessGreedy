using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000475 RID: 1141
[SerializationConfig(MemberSerialization.OptIn)]
public class GameplayEventInstance : ISaveLoadable
{
	// Token: 0x17000093 RID: 147
	// (get) Token: 0x0600189D RID: 6301 RVA: 0x0008343E File Offset: 0x0008163E
	// (set) Token: 0x0600189E RID: 6302 RVA: 0x00083446 File Offset: 0x00081646
	public StateMachine.Instance smi { get; private set; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x0600189F RID: 6303 RVA: 0x0008344F File Offset: 0x0008164F
	// (set) Token: 0x060018A0 RID: 6304 RVA: 0x00083457 File Offset: 0x00081657
	public bool seenNotification
	{
		get
		{
			return this._seenNotification;
		}
		set
		{
			this._seenNotification = value;
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(-1122598290, this);
			});
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060018A1 RID: 6305 RVA: 0x00083477 File Offset: 0x00081677
	public GameplayEvent gameplayEvent
	{
		get
		{
			if (this._gameplayEvent == null)
			{
				this._gameplayEvent = Db.Get().GameplayEvents.TryGet(this.eventID);
			}
			return this._gameplayEvent;
		}
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000834A2 File Offset: 0x000816A2
	public GameplayEventInstance(GameplayEvent gameplayEvent, int worldId)
	{
		this.eventID = gameplayEvent.Id;
		this.tags = new List<Tag>();
		this.eventStartTime = GameUtil.GetCurrentTimeInCycles();
		this.worldId = worldId;
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000834D8 File Offset: 0x000816D8
	public StateMachine.Instance PrepareEvent(GameplayEventManager manager)
	{
		this.smi = this.gameplayEvent.GetSMI(manager, this);
		return this.smi;
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x000834F4 File Offset: 0x000816F4
	public void StartEvent()
	{
		GameplayEventManager.Instance.Trigger(1491341646, this);
		StateMachine.Instance smi = this.smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStop));
		this.smi.StartSM();
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x00083543 File Offset: 0x00081743
	public void RegisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		if (!this.monitorCallbackObjects.Contains(go))
		{
			this.monitorCallbackObjects.Add(go);
		}
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x00083572 File Offset: 0x00081772
	public void UnregisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		this.monitorCallbackObjects.Remove(go);
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x00083594 File Offset: 0x00081794
	public void OnStop(string reason, StateMachine.Status status)
	{
		GameplayEventManager.Instance.Trigger(1287635015, this);
		if (this.monitorCallbackObjects != null)
		{
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(1287635015, this);
			});
		}
		if (status == StateMachine.Status.Success)
		{
			using (List<HashedString>.Enumerator enumerator = this.gameplayEvent.successEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HashedString hashedString = enumerator.Current;
					GameplayEvent gameplayEvent = Db.Get().GameplayEvents.TryGet(hashedString);
					DebugUtil.DevAssert(gameplayEvent != null, string.Format("GameplayEvent {0} is null", hashedString), null);
					if (gameplayEvent != null && gameplayEvent.IsAllowed())
					{
						GameplayEventManager.Instance.StartNewEvent(gameplayEvent, -1, null);
					}
				}
				return;
			}
		}
		if (status == StateMachine.Status.Failed)
		{
			foreach (HashedString hashedString2 in this.gameplayEvent.failureEvents)
			{
				GameplayEvent gameplayEvent2 = Db.Get().GameplayEvents.TryGet(hashedString2);
				DebugUtil.DevAssert(gameplayEvent2 != null, string.Format("GameplayEvent {0} is null", hashedString2), null);
				if (gameplayEvent2 != null && gameplayEvent2.IsAllowed())
				{
					GameplayEventManager.Instance.StartNewEvent(gameplayEvent2, -1, null);
				}
			}
		}
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000836EC File Offset: 0x000818EC
	public float AgeInCycles()
	{
		return GameUtil.GetCurrentTimeInCycles() - this.eventStartTime;
	}

	// Token: 0x04000DAD RID: 3501
	[Serialize]
	public readonly HashedString eventID;

	// Token: 0x04000DAE RID: 3502
	[Serialize]
	public List<Tag> tags;

	// Token: 0x04000DAF RID: 3503
	[Serialize]
	public float eventStartTime;

	// Token: 0x04000DB0 RID: 3504
	[Serialize]
	public readonly int worldId;

	// Token: 0x04000DB1 RID: 3505
	[Serialize]
	private bool _seenNotification;

	// Token: 0x04000DB2 RID: 3506
	public List<GameObject> monitorCallbackObjects;

	// Token: 0x04000DB3 RID: 3507
	public GameplayEventInstance.GameplayEventPopupDataCallback GetEventPopupData;

	// Token: 0x04000DB4 RID: 3508
	private GameplayEvent _gameplayEvent;

	// Token: 0x0200122E RID: 4654
	// (Invoke) Token: 0x06008264 RID: 33380
	public delegate EventInfoData GameplayEventPopupDataCallback();
}

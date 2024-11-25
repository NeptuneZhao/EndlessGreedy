using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000981 RID: 2433
public class GameplayEventMonitor : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>
{
	// Token: 0x06004714 RID: 18196 RVA: 0x00196974 File Offset: 0x00194B74
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
		default_state = this.idle;
		this.root.EventHandler(GameHashes.GameplayEventMonitorStart, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnMonitorStart(data);
		}).EventHandler(GameHashes.GameplayEventMonitorEnd, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnMonitorEnd(data);
		}).EventHandler(GameHashes.GameplayEventMonitorChanged, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			this.UpdateFX(smi);
		});
		this.idle.EventTransition(GameHashes.GameplayEventMonitorStart, this.activeState, new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.HasEvents)).Enter(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State.Callback(this.UpdateEventDisplay));
		this.activeState.DefaultState(this.activeState.unseenEvents);
		this.activeState.unseenEvents.ToggleFX(new Func<GameplayEventMonitor.Instance, StateMachine.Instance>(this.CreateFX)).EventHandler(GameHashes.SelectObject, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnSelect(data);
		}).EventTransition(GameHashes.GameplayEventMonitorChanged, this.activeState.seenAllEvents, new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.SeenAll));
		this.activeState.seenAllEvents.EventTransition(GameHashes.GameplayEventMonitorStart, this.activeState.unseenEvents, GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Not(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.SeenAll))).Enter(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State.Callback(this.UpdateEventDisplay));
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x00196AF4 File Offset: 0x00194CF4
	private bool HasEvents(GameplayEventMonitor.Instance smi)
	{
		return smi.events.Count > 0;
	}

	// Token: 0x06004716 RID: 18198 RVA: 0x00196B04 File Offset: 0x00194D04
	private bool SeenAll(GameplayEventMonitor.Instance smi)
	{
		return smi.UnseenCount() == 0;
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x00196B0F File Offset: 0x00194D0F
	private void UpdateFX(GameplayEventMonitor.Instance smi)
	{
		if (smi.fx != null)
		{
			smi.fx.sm.notificationCount.Set(smi.UnseenCount(), smi.fx, false);
		}
	}

	// Token: 0x06004718 RID: 18200 RVA: 0x00196B3C File Offset: 0x00194D3C
	private GameplayEventFX.Instance CreateFX(GameplayEventMonitor.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			smi.fx = new GameplayEventFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));
			return smi.fx;
		}
		return null;
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x00196B74 File Offset: 0x00194D74
	public void UpdateEventDisplay(GameplayEventMonitor.Instance smi)
	{
		if (smi.events.Count == 0 || smi.UnseenCount() > 0)
		{
			NameDisplayScreen.Instance.SetGameplayEventDisplay(smi.master.gameObject, false, null, null);
			return;
		}
		int num = -1;
		GameplayEvent gameplayEvent = null;
		foreach (GameplayEventInstance gameplayEventInstance in smi.events)
		{
			Sprite displaySprite = gameplayEventInstance.gameplayEvent.GetDisplaySprite();
			if (gameplayEventInstance.gameplayEvent.importance > num && displaySprite != null)
			{
				num = gameplayEventInstance.gameplayEvent.importance;
				gameplayEvent = gameplayEventInstance.gameplayEvent;
			}
		}
		if (gameplayEvent != null)
		{
			NameDisplayScreen.Instance.SetGameplayEventDisplay(smi.master.gameObject, true, gameplayEvent.GetDisplayString(), gameplayEvent.GetDisplaySprite());
		}
	}

	// Token: 0x04002E5A RID: 11866
	public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State idle;

	// Token: 0x04002E5B RID: 11867
	public GameplayEventMonitor.ActiveState activeState;

	// Token: 0x0200192F RID: 6447
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001930 RID: 6448
	public class ActiveState : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State
	{
		// Token: 0x040078A8 RID: 30888
		public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State unseenEvents;

		// Token: 0x040078A9 RID: 30889
		public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State seenAllEvents;
	}

	// Token: 0x02001931 RID: 6449
	public new class Instance : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.GameInstance
	{
		// Token: 0x06009B8C RID: 39820 RVA: 0x0036FD17 File Offset: 0x0036DF17
		public Instance(IStateMachineTarget master, GameplayEventMonitor.Def def) : base(master, def)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06009B8D RID: 39821 RVA: 0x0036FD40 File Offset: 0x0036DF40
		public void OnMonitorStart(object data)
		{
			GameplayEventInstance gameplayEventInstance = data as GameplayEventInstance;
			if (!this.events.Contains(gameplayEventInstance))
			{
				this.events.Add(gameplayEventInstance);
				gameplayEventInstance.RegisterMonitorCallback(base.gameObject);
			}
			base.smi.sm.UpdateFX(base.smi);
			base.smi.sm.UpdateEventDisplay(base.smi);
		}

		// Token: 0x06009B8E RID: 39822 RVA: 0x0036FDA8 File Offset: 0x0036DFA8
		public void OnMonitorEnd(object data)
		{
			GameplayEventInstance gameplayEventInstance = data as GameplayEventInstance;
			if (this.events.Contains(gameplayEventInstance))
			{
				this.events.Remove(gameplayEventInstance);
				gameplayEventInstance.UnregisterMonitorCallback(base.gameObject);
			}
			base.smi.sm.UpdateFX(base.smi);
			base.smi.sm.UpdateEventDisplay(base.smi);
			if (this.events.Count == 0)
			{
				base.smi.GoTo(base.sm.idle);
			}
		}

		// Token: 0x06009B8F RID: 39823 RVA: 0x0036FE34 File Offset: 0x0036E034
		public void OnSelect(object data)
		{
			if (!(bool)data)
			{
				return;
			}
			foreach (GameplayEventInstance gameplayEventInstance in this.events)
			{
				if (!gameplayEventInstance.seenNotification && gameplayEventInstance.GetEventPopupData != null)
				{
					gameplayEventInstance.seenNotification = true;
					EventInfoScreen.ShowPopup(gameplayEventInstance.GetEventPopupData());
					break;
				}
			}
			if (this.UnseenCount() == 0)
			{
				base.smi.GoTo(base.sm.activeState.seenAllEvents);
			}
		}

		// Token: 0x06009B90 RID: 39824 RVA: 0x0036FED8 File Offset: 0x0036E0D8
		public int UnseenCount()
		{
			return this.events.Count((GameplayEventInstance evt) => !evt.seenNotification);
		}

		// Token: 0x040078AA RID: 30890
		public List<GameplayEventInstance> events = new List<GameplayEventInstance>();

		// Token: 0x040078AB RID: 30891
		public GameplayEventFX.Instance fx;
	}
}

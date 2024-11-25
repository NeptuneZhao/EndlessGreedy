using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F61 RID: 3937
	public class FoodFightEvent : GameplayEvent<FoodFightEvent.StatesInstance>
	{
		// Token: 0x060078F9 RID: 30969 RVA: 0x002FE0E0 File Offset: 0x002FC2E0
		public FoodFightEvent() : base("FoodFight", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.DESCRIPTION;
		}

		// Token: 0x060078FA RID: 30970 RVA: 0x002FE10F File Offset: 0x002FC30F
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new FoodFightEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x04005A59 RID: 23129
		public const float FUTURE_TIME = 60f;

		// Token: 0x04005A5A RID: 23130
		public const float DURATION = 60f;

		// Token: 0x0200234F RID: 9039
		public class StatesInstance : GameplayEventStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, FoodFightEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B636 RID: 46646 RVA: 0x003C9FD2 File Offset: 0x003C81D2
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, FoodFightEvent foodEvent) : base(master, eventInstance, foodEvent)
			{
			}

			// Token: 0x0600B637 RID: 46647 RVA: 0x003C9FE0 File Offset: 0x003C81E0
			public void CreateChores(FoodFightEvent.StatesInstance smi)
			{
				this.chores = new List<FoodFightChore>();
				List<Room> list = Game.Instance.roomProber.rooms.FindAll((Room match) => match.roomType == Db.Get().RoomTypes.MessHall || match.roomType == Db.Get().RoomTypes.GreatHall);
				if (list == null || list.Count == 0)
				{
					return;
				}
				List<GameObject> buildingsOnFloor = list[UnityEngine.Random.Range(0, list.Count)].GetBuildingsOnFloor();
				for (int i = 0; i < Math.Min(Components.LiveMinionIdentities.Count, buildingsOnFloor.Count); i++)
				{
					IStateMachineTarget master = Components.LiveMinionIdentities[i];
					GameObject gameObject = buildingsOnFloor[UnityEngine.Random.Range(0, buildingsOnFloor.Count)];
					GameObject locator = ChoreHelpers.CreateLocator("FoodFightLocator", gameObject.transform.position);
					FoodFightChore foodFightChore = new FoodFightChore(master, locator);
					buildingsOnFloor.Remove(gameObject);
					FoodFightChore foodFightChore2 = foodFightChore;
					foodFightChore2.onExit = (Action<Chore>)Delegate.Combine(foodFightChore2.onExit, new Action<Chore>(delegate(Chore data)
					{
						Util.KDestroyGameObject(locator);
					}));
					this.chores.Add(foodFightChore);
				}
			}

			// Token: 0x0600B638 RID: 46648 RVA: 0x003CA100 File Offset: 0x003C8300
			public void ClearChores()
			{
				if (this.chores != null)
				{
					for (int i = this.chores.Count - 1; i >= 0; i--)
					{
						if (this.chores[i] != null)
						{
							this.chores[i].Cancel("end");
						}
					}
				}
				this.chores = null;
			}

			// Token: 0x04009E55 RID: 40533
			public List<FoodFightChore> chores;
		}

		// Token: 0x02002350 RID: 9040
		public class States : GameplayEventStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, FoodFightEvent>
		{
			// Token: 0x0600B639 RID: 46649 RVA: 0x003CA158 File Offset: 0x003C8358
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.root.Exit(delegate(FoodFightEvent.StatesInstance smi)
				{
					smi.ClearChores();
				});
				this.planning.ToggleNotification((FoodFightEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.ToggleNotification((FoodFightEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.wait.ScheduleGoTo(60f, this.warmup.start);
				this.warmup.start.Enter(delegate(FoodFightEvent.StatesInstance smi)
				{
					smi.CreateChores(smi);
				}).Update(delegate(FoodFightEvent.StatesInstance smi, float data)
				{
					int num = 0;
					foreach (FoodFightChore foodFightChore in smi.chores)
					{
						if (foodFightChore.smi.IsInsideState(foodFightChore.smi.sm.waitForParticipants))
						{
							num++;
						}
					}
					if (num >= smi.chores.Count || smi.timeinstate > 30f)
					{
						foreach (FoodFightChore foodFightChore2 in smi.chores)
						{
							foodFightChore2.gameObject.Trigger(-2043101269, null);
						}
						smi.GoTo(this.partying);
					}
				}, UpdateRate.RENDER_1000ms, false);
				this.partying.ToggleNotification((FoodFightEvent.StatesInstance smi) => new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.UNDERWAY, NotificationType.Good, (List<Notification> a, object b) => GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.UNDERWAY_TOOLTIP, null, true, 0f, null, null, null, true, false, false)).ScheduleGoTo(60f, this.ending);
				this.ending.ReturnSuccess();
				this.canceled.DoNotification((FoodFightEvent.StatesInstance smi) => GameplayEventManager.CreateStandardCancelledNotification(this.GenerateEventPopupData(smi))).Enter(delegate(FoodFightEvent.StatesInstance smi)
				{
					foreach (object obj in Components.LiveMinionIdentities)
					{
						((MinionIdentity)obj).GetComponent<Effects>().Add("NoFunAllowed", true);
					}
				}).ReturnFailure();
			}

			// Token: 0x0600B63A RID: 46650 RVA: 0x003CA2C4 File Offset: 0x003C84C4
			public override EventInfoData GenerateEventPopupData(FoodFightEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD;
				eventInfoData.whenDescription = string.Format(GAMEPLAY_EVENTS.TIMES.IN_CYCLES, 0.1f);
				eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.ACCEPT_OPTION_NAME, null).callback = delegate()
				{
					smi.GoTo(smi.sm.warmup.wait);
				};
				eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.REJECT_OPTION_NAME, null).callback = delegate()
				{
					smi.GoTo(smi.sm.canceled);
				};
				return eventInfoData;
			}

			// Token: 0x04009E56 RID: 40534
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x04009E57 RID: 40535
			public FoodFightEvent.States.WarmupStates warmup;

			// Token: 0x04009E58 RID: 40536
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State partying;

			// Token: 0x04009E59 RID: 40537
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x04009E5A RID: 40538
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State canceled;

			// Token: 0x0200351C RID: 13596
			public class WarmupStates : GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400D770 RID: 55152
				public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State wait;

				// Token: 0x0400D771 RID: 55153
				public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State start;
			}
		}
	}
}

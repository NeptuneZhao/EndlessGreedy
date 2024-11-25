using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F5E RID: 3934
	public class BonusEvent : GameplayEvent<BonusEvent.StatesInstance>
	{
		// Token: 0x060078EA RID: 30954 RVA: 0x002FDDA8 File Offset: 0x002FBFA8
		public BonusEvent(string id, string overrideEffect = null, int numTimesAllowed = 1, bool preSelectMinion = false, int priority = 0) : base(id, priority, 0)
		{
			this.title = Strings.Get("STRINGS.GAMEPLAY_EVENTS.BONUS." + id.ToUpper() + ".NAME");
			this.description = Strings.Get("STRINGS.GAMEPLAY_EVENTS.BONUS." + id.ToUpper() + ".DESCRIPTION");
			this.effect = ((overrideEffect != null) ? overrideEffect : id);
			this.numTimesAllowed = numTimesAllowed;
			this.preSelectMinion = preSelectMinion;
			this.animFileName = id.ToLower() + "_kanim";
			base.AddPrecondition(GameplayEventPreconditions.Instance.LiveMinions(1));
		}

		// Token: 0x060078EB RID: 30955 RVA: 0x002FDE52 File Offset: 0x002FC052
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new BonusEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x060078EC RID: 30956 RVA: 0x002FDE5C File Offset: 0x002FC05C
		public BonusEvent TriggerOnNewBuilding(int triggerCount, params string[] buildings)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.NewBuilding;
			this.buildingTrigger = new HashSet<Tag>(buildings.ToTagList());
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x060078ED RID: 30957 RVA: 0x002FDE92 File Offset: 0x002FC092
		public BonusEvent TriggerOnUseBuilding(int triggerCount, params string[] buildings)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.UseBuilding;
			this.buildingTrigger = new HashSet<Tag>(buildings.ToTagList());
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x060078EE RID: 30958 RVA: 0x002FDEC8 File Offset: 0x002FC0C8
		public BonusEvent TriggerOnWorkableComplete(int triggerCount, params Type[] types)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.WorkableComplete;
			this.workableType = new HashSet<Type>(types);
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x060078EF RID: 30959 RVA: 0x002FDEF9 File Offset: 0x002FC0F9
		public BonusEvent SetExtraCondition(BonusEvent.ConditionFn extraCondition)
		{
			this.extraCondition = extraCondition;
			return this;
		}

		// Token: 0x060078F0 RID: 30960 RVA: 0x002FDF03 File Offset: 0x002FC103
		public BonusEvent SetRoomConstraints(bool hasOwnableInRoom, params RoomType[] types)
		{
			this.roomHasOwnable = hasOwnableInRoom;
			this.roomRestrictions = ((types == null) ? null : new HashSet<RoomType>(types));
			return this;
		}

		// Token: 0x060078F1 RID: 30961 RVA: 0x002FDF1F File Offset: 0x002FC11F
		public string GetEffectTooltip(Effect effect)
		{
			return effect.Name + "\n\n" + Effect.CreateTooltip(effect, true, "\n    • ", true);
		}

		// Token: 0x060078F2 RID: 30962 RVA: 0x002FDF40 File Offset: 0x002FC140
		public override Sprite GetDisplaySprite()
		{
			Effect effect = Db.Get().effects.Get(this.effect);
			if (effect.SelfModifiers.Count > 0)
			{
				return Assets.GetSprite(Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).uiFullColourSprite);
			}
			return null;
		}

		// Token: 0x060078F3 RID: 30963 RVA: 0x002FDFA4 File Offset: 0x002FC1A4
		public override string GetDisplayString()
		{
			Effect effect = Db.Get().effects.Get(this.effect);
			if (effect.SelfModifiers.Count > 0)
			{
				return Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).Name;
			}
			return null;
		}

		// Token: 0x04005A48 RID: 23112
		public const int PRE_SELECT_MINION_TIMEOUT = 5;

		// Token: 0x04005A49 RID: 23113
		public string effect;

		// Token: 0x04005A4A RID: 23114
		public bool preSelectMinion;

		// Token: 0x04005A4B RID: 23115
		public int numTimesToTrigger;

		// Token: 0x04005A4C RID: 23116
		public BonusEvent.TriggerType triggerType;

		// Token: 0x04005A4D RID: 23117
		public HashSet<Tag> buildingTrigger;

		// Token: 0x04005A4E RID: 23118
		public HashSet<Type> workableType;

		// Token: 0x04005A4F RID: 23119
		public HashSet<RoomType> roomRestrictions;

		// Token: 0x04005A50 RID: 23120
		public BonusEvent.ConditionFn extraCondition;

		// Token: 0x04005A51 RID: 23121
		public bool roomHasOwnable;

		// Token: 0x02002346 RID: 9030
		public enum TriggerType
		{
			// Token: 0x04009E38 RID: 40504
			None,
			// Token: 0x04009E39 RID: 40505
			NewBuilding,
			// Token: 0x04009E3A RID: 40506
			UseBuilding,
			// Token: 0x04009E3B RID: 40507
			WorkableComplete,
			// Token: 0x04009E3C RID: 40508
			AchievementUnlocked
		}

		// Token: 0x02002347 RID: 9031
		// (Invoke) Token: 0x0600B612 RID: 46610
		public delegate bool ConditionFn(BonusEvent.GameplayEventData data);

		// Token: 0x02002348 RID: 9032
		public class GameplayEventData
		{
			// Token: 0x04009E3D RID: 40509
			public GameHashes eventTrigger;

			// Token: 0x04009E3E RID: 40510
			public BuildingComplete building;

			// Token: 0x04009E3F RID: 40511
			public Workable workable;

			// Token: 0x04009E40 RID: 40512
			public WorkerBase worker;
		}

		// Token: 0x02002349 RID: 9033
		public class States : GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>
		{
			// Token: 0x0600B616 RID: 46614 RVA: 0x003C9318 File Offset: 0x003C7518
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.load;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.load.Enter(new StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.AssignPreSelectedMinionIfNeeded)).Transition(this.waitNewBuilding, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.NewBuilding, UpdateRate.SIM_200ms).Transition(this.waitUseBuilding, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.UseBuilding, UpdateRate.SIM_200ms).Transition(this.waitforWorkables, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.WorkableComplete, UpdateRate.SIM_200ms).Transition(this.waitForAchievement, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.AchievementUnlocked, UpdateRate.SIM_200ms).Transition(this.immediate, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.None, UpdateRate.SIM_200ms);
				this.waitNewBuilding.EventHandlerTransition(GameHashes.NewBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
				this.waitUseBuilding.EventHandlerTransition(GameHashes.UseBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
				this.waitforWorkables.EventHandlerTransition(GameHashes.UseBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.WorkableEventTrigger));
				this.immediate.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					GameObject gameObject = smi.sm.chosen.Get(smi);
					if (gameObject == null)
					{
						gameObject = smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject;
						smi.sm.chosen.Set(gameObject, smi, false);
					}
				}).GoTo(this.active);
				this.active.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					smi.sm.chosen.Get(smi).GetComponent<Effects>().Add(smi.gameplayEvent.effect, true);
				}).Enter(delegate(BonusEvent.StatesInstance smi)
				{
					base.MonitorStart(this.chosen, smi);
				}).Exit(delegate(BonusEvent.StatesInstance smi)
				{
					base.MonitorStop(this.chosen, smi);
				}).ScheduleGoTo(delegate(BonusEvent.StatesInstance smi)
				{
					Effect effect = this.GetEffect(smi);
					if (effect != null)
					{
						return effect.duration;
					}
					return 0f;
				}, this.ending).DefaultState(this.active.notify).OnTargetLost(this.chosen, this.ending).Target(this.chosen).TagTransition(GameTags.Dead, this.ending, false);
				this.active.notify.ToggleNotification((BonusEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.active.seenNotification.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					smi.eventInstance.seenNotification = true;
				});
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600B617 RID: 46615 RVA: 0x003C95BC File Offset: 0x003C77BC
			public override EventInfoData GenerateEventPopupData(BonusEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				GameObject gameObject = smi.sm.chosen.Get(smi);
				if (gameObject == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						"Minion not set for " + smi.gameplayEvent.Id
					});
					return null;
				}
				Effect effect = this.GetEffect(smi);
				if (effect == null)
				{
					return null;
				}
				eventInfoData.clickFocus = gameObject.transform;
				eventInfoData.minions = new GameObject[]
				{
					gameObject
				};
				eventInfoData.SetTextParameter("dupe", gameObject.GetProperName());
				if (smi.building != null)
				{
					eventInfoData.SetTextParameter("building", UI.FormatAsLink(smi.building.GetProperName(), smi.building.GetProperName().ToUpper()));
				}
				EventInfoData.Option option = eventInfoData.AddDefaultOption(delegate
				{
					smi.GoTo(smi.sm.active.seenNotification);
				});
				GAMEPLAY_EVENTS.BONUS_EVENT_DESCRIPTION.Replace("{effects}", Effect.CreateTooltip(effect, false, " ", false)).Replace("{durration}", GameUtil.GetFormattedCycles(effect.duration, "F1", false));
				foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
				{
					Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
					string text = string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attribute.Name, attributeModifier.GetFormattedString());
					text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_TOTAL, GameUtil.GetFormattedCycles(effect.duration, "F1", false));
					Sprite sprite = Assets.GetSprite(attribute.uiFullColourSprite);
					option.AddPositiveIcon(sprite, text, 1.75f);
				}
				return eventInfoData;
			}

			// Token: 0x0600B618 RID: 46616 RVA: 0x003C9804 File Offset: 0x003C7A04
			private void AssignPreSelectedMinionIfNeeded(BonusEvent.StatesInstance smi)
			{
				if (smi.gameplayEvent.preSelectMinion && smi.sm.chosen.Get(smi) == null)
				{
					smi.sm.chosen.Set(smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject, smi, false);
					smi.timesTriggered = 0;
				}
			}

			// Token: 0x0600B619 RID: 46617 RVA: 0x003C9864 File Offset: 0x003C7A64
			private bool IsCorrectMinion(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				if (!smi.gameplayEvent.preSelectMinion || !(smi.sm.chosen.Get(smi) != gameplayEventData.worker.gameObject))
				{
					return true;
				}
				if (GameUtil.GetCurrentTimeInCycles() - smi.lastTriggered > 5f && smi.PercentageUntilTriggered() < 0.5f)
				{
					smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
					smi.timesTriggered = 0;
					return true;
				}
				return false;
			}

			// Token: 0x0600B61A RID: 46618 RVA: 0x003C98EC File Offset: 0x003C7AEC
			private bool OtherConditionsAreSatisfied(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				if (smi.gameplayEvent.roomRestrictions != null)
				{
					Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(gameplayEventData.worker.gameObject);
					if (roomOfGameObject == null)
					{
						return false;
					}
					if (!smi.gameplayEvent.roomRestrictions.Contains(roomOfGameObject.roomType))
					{
						return false;
					}
					if (smi.gameplayEvent.roomHasOwnable)
					{
						bool flag = false;
						using (List<Ownables>.Enumerator enumerator = roomOfGameObject.GetOwners().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.gameObject == gameplayEventData.worker.gameObject)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							return false;
						}
					}
				}
				return smi.gameplayEvent.extraCondition == null || smi.gameplayEvent.extraCondition(gameplayEventData);
			}

			// Token: 0x0600B61B RID: 46619 RVA: 0x003C99D4 File Offset: 0x003C7BD4
			private bool IncrementAndTrigger(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				smi.timesTriggered++;
				smi.lastTriggered = GameUtil.GetCurrentTimeInCycles();
				if (smi.timesTriggered < smi.gameplayEvent.numTimesToTrigger)
				{
					return false;
				}
				smi.building = gameplayEventData.building;
				smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
				return true;
			}

			// Token: 0x0600B61C RID: 46620 RVA: 0x003C9A3C File Offset: 0x003C7C3C
			private bool BuildingEventTrigger(BonusEvent.StatesInstance smi, object data)
			{
				BonusEvent.GameplayEventData gameplayEventData = data as BonusEvent.GameplayEventData;
				if (gameplayEventData == null)
				{
					return false;
				}
				this.AssignPreSelectedMinionIfNeeded(smi);
				return !(gameplayEventData.building == null) && (smi.gameplayEvent.buildingTrigger.Count <= 0 || smi.gameplayEvent.buildingTrigger.Contains(gameplayEventData.building.prefabid.PrefabID())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
			}

			// Token: 0x0600B61D RID: 46621 RVA: 0x003C9AC4 File Offset: 0x003C7CC4
			private bool WorkableEventTrigger(BonusEvent.StatesInstance smi, object data)
			{
				BonusEvent.GameplayEventData gameplayEventData = data as BonusEvent.GameplayEventData;
				if (gameplayEventData == null)
				{
					return false;
				}
				this.AssignPreSelectedMinionIfNeeded(smi);
				return (smi.gameplayEvent.workableType.Count <= 0 || smi.gameplayEvent.workableType.Contains(gameplayEventData.workable.GetType())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
			}

			// Token: 0x0600B61E RID: 46622 RVA: 0x003C9B36 File Offset: 0x003C7D36
			private bool ChosenMinionDied(BonusEvent.StatesInstance smi, object data)
			{
				return smi.sm.chosen.Get(smi) == data as GameObject;
			}

			// Token: 0x0600B61F RID: 46623 RVA: 0x003C9B54 File Offset: 0x003C7D54
			private Effect GetEffect(BonusEvent.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.chosen.Get(smi);
				if (gameObject == null)
				{
					return null;
				}
				EffectInstance effectInstance = gameObject.GetComponent<Effects>().Get(smi.gameplayEvent.effect);
				if (effectInstance == null)
				{
					global::Debug.LogWarning(string.Format("Effect {0} not found on {1} in BonusEvent", smi.gameplayEvent.effect, gameObject));
					return null;
				}
				return effectInstance.effect;
			}

			// Token: 0x04009E41 RID: 40513
			public StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.TargetParameter chosen;

			// Token: 0x04009E42 RID: 40514
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State load;

			// Token: 0x04009E43 RID: 40515
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitNewBuilding;

			// Token: 0x04009E44 RID: 40516
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitUseBuilding;

			// Token: 0x04009E45 RID: 40517
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitForAchievement;

			// Token: 0x04009E46 RID: 40518
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitforWorkables;

			// Token: 0x04009E47 RID: 40519
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State immediate;

			// Token: 0x04009E48 RID: 40520
			public BonusEvent.States.ActiveStates active;

			// Token: 0x04009E49 RID: 40521
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x02003515 RID: 13589
			public class ActiveStates : GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400D75B RID: 55131
				public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State notify;

				// Token: 0x0400D75C RID: 55132
				public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State seenNotification;
			}
		}

		// Token: 0x0200234A RID: 9034
		public class StatesInstance : GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600B625 RID: 46629 RVA: 0x003C9C17 File Offset: 0x003C7E17
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, BonusEvent bonusEvent) : base(master, eventInstance, bonusEvent)
			{
				this.lastTriggered = GameUtil.GetCurrentTimeInCycles();
			}

			// Token: 0x0600B626 RID: 46630 RVA: 0x003C9C2D File Offset: 0x003C7E2D
			public float PercentageUntilTriggered()
			{
				return (float)this.timesTriggered / (float)base.smi.gameplayEvent.numTimesToTrigger;
			}

			// Token: 0x04009E4A RID: 40522
			[Serialize]
			public int timesTriggered;

			// Token: 0x04009E4B RID: 40523
			[Serialize]
			public float lastTriggered;

			// Token: 0x04009E4C RID: 40524
			public BuildingComplete building;
		}
	}
}

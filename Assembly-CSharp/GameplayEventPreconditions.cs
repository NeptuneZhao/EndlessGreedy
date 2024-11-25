using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;

// Token: 0x02000479 RID: 1145
public class GameplayEventPreconditions
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060018B6 RID: 6326 RVA: 0x00083953 File Offset: 0x00081B53
	public static GameplayEventPreconditions Instance
	{
		get
		{
			if (GameplayEventPreconditions._instance == null)
			{
				GameplayEventPreconditions._instance = new GameplayEventPreconditions();
			}
			return GameplayEventPreconditions._instance;
		}
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x0008396C File Offset: 0x00081B6C
	public GameplayEventPrecondition LiveMinions(int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Components.LiveMinionIdentities.Count >= count),
			description = string.Format("At least {0} dupes alive", count)
		};
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000839B8 File Offset: 0x00081BB8
	public GameplayEventPrecondition BuildingExists(string buildingId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => BuildingInventory.Instance.BuildingCount(new Tag(buildingId)) >= count),
			description = string.Format("{0} {1} has been built", count, buildingId)
		};
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x00083A14 File Offset: 0x00081C14
	public GameplayEventPrecondition ResearchCompleted(string techName)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Research.Instance.Get(Db.Get().Techs.Get(techName)).IsComplete()),
			description = "Has researched " + techName + "."
		};
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x00083A60 File Offset: 0x00081C60
	public GameplayEventPrecondition AchievementUnlocked(ColonyAchievement achievement)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => SaveGame.Instance.ColonyAchievementTracker.IsAchievementUnlocked(achievement)),
			description = "Unlocked the " + achievement.Id + " achievement"
		};
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x00083AB4 File Offset: 0x00081CB4
	public GameplayEventPrecondition RoomBuilt(RoomType roomType)
	{
		Predicate<Room> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				List<Room> rooms = Game.Instance.roomProber.rooms;
				Predicate<Room> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((Room match) => match.roomType == roomType));
				}
				return rooms.Exists(match2);
			},
			description = "Built a " + roomType.Id + " room"
		};
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x00083B08 File Offset: 0x00081D08
	public GameplayEventPrecondition CycleRestriction(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameUtil.GetCurrentTimeInCycles() >= min && GameUtil.GetCurrentTimeInCycles() <= max),
			description = string.Format("After cycle {0} and before cycle {1}", min, max)
		};
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x00083B68 File Offset: 0x00081D68
	public GameplayEventPrecondition MinionsWithEffect(string effectId, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<Effects>().Get(effectId) != null));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} effect applied", count, effectId)
		};
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x00083BC4 File Offset: 0x00081DC4
	public GameplayEventPrecondition MinionsWithStatusItem(StatusItem statusItem, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<KSelectable>().HasStatusItem(statusItem)));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} status item", count, statusItem)
		};
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x00083C20 File Offset: 0x00081E20
	public GameplayEventPrecondition MinionsWithChoreGroupPriorityOrGreater(ChoreGroup choreGroup, int count, int priority)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(MinionIdentity minion)
					{
						ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
						return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
					});
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have their {1} set to {2} or higher.", count, choreGroup.Name, priority)
		};
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x00083C90 File Offset: 0x00081E90
	public GameplayEventPrecondition PastEventCount(string evtId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evtId) >= count),
			description = string.Format("The {0} event has triggered {1} times.", evtId, count)
		};
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x00083CEC File Offset: 0x00081EEC
	public GameplayEventPrecondition PastEventCountAndNotActive(GameplayEvent evt, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evt.IdHash) >= count && !GameplayEventManager.Instance.IsGameplayEventActive(evt)),
			description = string.Format("The {0} event has triggered {1} times and is not active.", evt.Id, count)
		};
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x00083D4C File Offset: 0x00081F4C
	public GameplayEventPrecondition Not(GameplayEventPrecondition precondition)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => !precondition.condition()),
			description = "Not[" + precondition.description + "]"
		};
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x00083DA0 File Offset: 0x00081FA0
	public GameplayEventPrecondition Or(GameplayEventPrecondition precondition1, GameplayEventPrecondition precondition2)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => precondition1.condition() || precondition2.condition()),
			description = string.Concat(new string[]
			{
				"[",
				precondition1.description,
				"]-OR-[",
				precondition2.description,
				"]"
			})
		};
	}

	// Token: 0x04000DBC RID: 3516
	private static GameplayEventPreconditions _instance;
}

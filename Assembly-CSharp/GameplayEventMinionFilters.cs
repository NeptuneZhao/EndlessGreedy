using System;
using Database;

// Token: 0x02000477 RID: 1143
public class GameplayEventMinionFilters
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060018AC RID: 6316 RVA: 0x0008371E File Offset: 0x0008191E
	public static GameplayEventMinionFilters Instance
	{
		get
		{
			if (GameplayEventMinionFilters._instance == null)
			{
				GameplayEventMinionFilters._instance = new GameplayEventMinionFilters();
			}
			return GameplayEventMinionFilters._instance;
		}
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x00083738 File Offset: 0x00081938
	public GameplayEventMinionFilter HasMasteredSkill(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasMasteredSkill(skill.Id)),
			id = "HasMasteredSkill"
		};
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x00083774 File Offset: 0x00081974
	public GameplayEventMinionFilter HasSkillAptitude(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasSkillAptitude(skill)),
			id = "HasSkillAptitude"
		};
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x000837B0 File Offset: 0x000819B0
	public GameplayEventMinionFilter HasChoreGroupPriorityOrHigher(ChoreGroup choreGroup, int priority)
	{
		return new GameplayEventMinionFilter
		{
			filter = delegate(MinionIdentity minion)
			{
				ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
				return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
			},
			id = "HasChoreGroupPriorityOrHigher"
		};
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x000837F4 File Offset: 0x000819F4
	public GameplayEventMinionFilter AgeRange(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.arrivalTime >= min && minion.arrivalTime <= max),
			id = "AgeRange"
		};
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x00083837 File Offset: 0x00081A37
	public GameplayEventMinionFilter PriorityIn()
	{
		GameplayEventMinionFilter gameplayEventMinionFilter = new GameplayEventMinionFilter();
		gameplayEventMinionFilter.filter = ((MinionIdentity minion) => true);
		gameplayEventMinionFilter.id = "PriorityIn";
		return gameplayEventMinionFilter;
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x00083870 File Offset: 0x00081A70
	public GameplayEventMinionFilter Not(GameplayEventMinionFilter filter)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => !filter.filter(minion)),
			id = "Not[" + filter.id + "]"
		};
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x000838C4 File Offset: 0x00081AC4
	public GameplayEventMinionFilter Or(GameplayEventMinionFilter precondition1, GameplayEventMinionFilter precondition2)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => precondition1.filter(minion) || precondition2.filter(minion)),
			id = string.Concat(new string[]
			{
				"[",
				precondition1.id,
				"]-OR-[",
				precondition2.id,
				"]"
			})
		};
	}

	// Token: 0x04000DB7 RID: 3511
	private static GameplayEventMinionFilters _instance;
}

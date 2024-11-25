using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000E76 RID: 3702
	public class Quests : ResourceSet<Quest>
	{
		// Token: 0x060074CC RID: 29900 RVA: 0x002D8FDC File Offset: 0x002D71DC
		public Quests(ResourceSet parent) : base("Quests", parent)
		{
			this.LonelyMinionGreetingQuest = base.Add(new Quest("KnockQuest", new QuestCriteria[]
			{
				new QuestCriteria("Neighbor", null, 1, null, QuestCriteria.BehaviorFlags.None)
			}));
			this.LonelyMinionFoodQuest = base.Add(new Quest("FoodQuest", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("FoodQuality", new float[]
				{
					4f
				}, 3, new HashSet<Tag>
				{
					GameTags.Edible
				}, QuestCriteria.BehaviorFlags.UniqueItems)
			}));
			this.LonelyMinionPowerQuest = base.Add(new Quest("PluggedIn", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("SuppliedPower", new float[]
				{
					3000f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
			this.LonelyMinionDecorQuest = base.Add(new Quest("HighDecor", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("Decor", new float[]
				{
					120f
				}, 1, null, (QuestCriteria.BehaviorFlags)6)
			}));
			this.FossilHuntQuest = base.Add(new Quest("FossilHuntQuest", new QuestCriteria[]
			{
				new QuestCriteria_Equals("LostSpecimen", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostIceFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostResinFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostRockFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
		}

		// Token: 0x0400545D RID: 21597
		public Quest LonelyMinionGreetingQuest;

		// Token: 0x0400545E RID: 21598
		public Quest LonelyMinionFoodQuest;

		// Token: 0x0400545F RID: 21599
		public Quest LonelyMinionPowerQuest;

		// Token: 0x04005460 RID: 21600
		public Quest LonelyMinionDecorQuest;

		// Token: 0x04005461 RID: 21601
		public Quest FossilHuntQuest;
	}
}

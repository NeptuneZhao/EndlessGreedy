using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EAE RID: 3758
	public class TravelXUsingTransitTubes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075BD RID: 30141 RVA: 0x002E13C6 File Offset: 0x002DF5C6
		public TravelXUsingTransitTubes(NavType navType, int distanceToTravel)
		{
			this.navType = navType;
			this.distanceToTravel = distanceToTravel;
		}

		// Token: 0x060075BE RID: 30142 RVA: 0x002E13DC File Offset: 0x002DF5DC
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				if (component != null && component.distanceTravelledByNavType.ContainsKey(this.navType))
				{
					num += component.distanceTravelledByNavType[this.navType];
				}
			}
			return num >= this.distanceToTravel;
		}

		// Token: 0x060075BF RID: 30143 RVA: 0x002E1470 File Offset: 0x002DF670
		public void Deserialize(IReader reader)
		{
			byte b = reader.ReadByte();
			this.navType = (NavType)b;
			this.distanceToTravel = reader.ReadInt32();
		}

		// Token: 0x060075C0 RID: 30144 RVA: 0x002E1498 File Offset: 0x002DF698
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				if (component != null && component.distanceTravelledByNavType.ContainsKey(this.navType))
				{
					num += component.distanceTravelledByNavType[this.navType];
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TRAVELED_IN_TUBES, complete ? this.distanceToTravel : num, this.distanceToTravel);
		}

		// Token: 0x0400556E RID: 21870
		private int distanceToTravel;

		// Token: 0x0400556F RID: 21871
		private NavType navType;
	}
}

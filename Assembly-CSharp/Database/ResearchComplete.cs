using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EA3 RID: 3747
	public class ResearchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600758F RID: 30095 RVA: 0x002E0534 File Offset: 0x002DE734
		public override bool Success()
		{
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007590 RID: 30096 RVA: 0x002E0598 File Offset: 0x002DE798
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007591 RID: 30097 RVA: 0x002E059C File Offset: 0x002DE79C
		public override string GetProgress(bool complete)
		{
			if (complete)
			{
				return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TECH_RESEARCHED, Db.Get().Techs.resources.Count, Db.Get().Techs.resources.Count);
			}
			int num = 0;
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsComplete())
					{
						num++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TECH_RESEARCHED, num, Db.Get().Techs.resources.Count);
		}
	}
}

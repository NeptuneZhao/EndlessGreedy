using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EB5 RID: 3765
	public class ActivateLorePOI : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075DA RID: 30170 RVA: 0x002E1CEA File Offset: 0x002DFEEA
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x060075DB RID: 30171 RVA: 0x002E1CEC File Offset: 0x002DFEEC
		public override bool Success()
		{
			foreach (BuildingComplete buildingComplete in Components.TemplateBuildings.Items)
			{
				if (!(buildingComplete == null))
				{
					Unsealable component = buildingComplete.GetComponent<Unsealable>();
					if (component != null && component.unsealed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060075DC RID: 30172 RVA: 0x002E1D64 File Offset: 0x002DFF64
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.INVESTIGATE_A_POI;
		}
	}
}

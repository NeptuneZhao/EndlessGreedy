using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E9B RID: 3739
	public class RepairGeothermalController : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007567 RID: 30055 RVA: 0x002E00B4 File Offset: 0x002DE2B4
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007568 RID: 30056 RVA: 0x002E00C2 File Offset: 0x002DE2C2
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_DESCRIPTION;
		}

		// Token: 0x06007569 RID: 30057 RVA: 0x002E00CE File Offset: 0x002DE2CE
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_TITLE;
		}

		// Token: 0x0600756A RID: 30058 RVA: 0x002E00DA File Offset: 0x002DE2DA
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
		}
	}
}

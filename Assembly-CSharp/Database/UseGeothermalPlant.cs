using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E9C RID: 3740
	public class UseGeothermalPlant : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600756C RID: 30060 RVA: 0x002E00F3 File Offset: 0x002DE2F3
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600756D RID: 30061 RVA: 0x002E0101 File Offset: 0x002DE301
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.ACTIVATE_PLANT_TITLE;
		}

		// Token: 0x0600756E RID: 30062 RVA: 0x002E010D File Offset: 0x002DE30D
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented;
		}

		// Token: 0x0600756F RID: 30063 RVA: 0x002E011E File Offset: 0x002DE31E
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.ACTIVATE_PLANT_DESCRIPTION;
		}
	}
}

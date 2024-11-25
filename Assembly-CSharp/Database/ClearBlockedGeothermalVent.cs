using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E9D RID: 3741
	public class ClearBlockedGeothermalVent : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007571 RID: 30065 RVA: 0x002E0132 File Offset: 0x002DE332
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007572 RID: 30066 RVA: 0x002E0140 File Offset: 0x002DE340
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.UNBLOCK_VENT_TITLE;
		}

		// Token: 0x06007573 RID: 30067 RVA: 0x002E014C File Offset: 0x002DE34C
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent;
		}

		// Token: 0x06007574 RID: 30068 RVA: 0x002E015D File Offset: 0x002DE35D
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.UNBLOCK_VENT_DESCRIPTION;
		}
	}
}

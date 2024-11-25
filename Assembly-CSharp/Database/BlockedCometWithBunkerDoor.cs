using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EBB RID: 3771
	public class BlockedCometWithBunkerDoor : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075F3 RID: 30195 RVA: 0x002E221F File Offset: 0x002E041F
		public override bool Success()
		{
			return Game.Instance.savedInfo.blockedCometWithBunkerDoor;
		}

		// Token: 0x060075F4 RID: 30196 RVA: 0x002E2230 File Offset: 0x002E0430
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x060075F5 RID: 30197 RVA: 0x002E2232 File Offset: 0x002E0432
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BLOCKED_A_COMET;
		}
	}
}

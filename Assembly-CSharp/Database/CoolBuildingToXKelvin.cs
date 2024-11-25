using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EA8 RID: 3752
	public class CoolBuildingToXKelvin : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075A4 RID: 30116 RVA: 0x002E0DD8 File Offset: 0x002DEFD8
		public CoolBuildingToXKelvin(int kelvinToCoolTo)
		{
			this.kelvinToCoolTo = kelvinToCoolTo;
		}

		// Token: 0x060075A5 RID: 30117 RVA: 0x002E0DE7 File Offset: 0x002DEFE7
		public override bool Success()
		{
			return BuildingComplete.MinKelvinSeen <= (float)this.kelvinToCoolTo;
		}

		// Token: 0x060075A6 RID: 30118 RVA: 0x002E0DFA File Offset: 0x002DEFFA
		public void Deserialize(IReader reader)
		{
			this.kelvinToCoolTo = reader.ReadInt32();
		}

		// Token: 0x060075A7 RID: 30119 RVA: 0x002E0E08 File Offset: 0x002DF008
		public override string GetProgress(bool complete)
		{
			float minKelvinSeen = BuildingComplete.MinKelvinSeen;
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.KELVIN_COOLING, minKelvinSeen);
		}

		// Token: 0x04005568 RID: 21864
		private int kelvinToCoolTo;
	}
}

using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E9A RID: 3738
	public class DiscoverGeothermalFacility : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007562 RID: 30050 RVA: 0x002E007F File Offset: 0x002DE27F
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007563 RID: 30051 RVA: 0x002E008D File Offset: 0x002DE28D
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.DISCOVER_GEOTHERMAL_FACILITY_DESCRIPTION;
		}

		// Token: 0x06007564 RID: 30052 RVA: 0x002E0099 File Offset: 0x002DE299
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.DISCOVER_GEOTHERMAL_FACILITY_TITLE;
		}

		// Token: 0x06007565 RID: 30053 RVA: 0x002E00A5 File Offset: 0x002DE2A5
		public override bool Success()
		{
			return GeothermalPlantComponent.GeothermalFacilityDiscovered();
		}
	}
}

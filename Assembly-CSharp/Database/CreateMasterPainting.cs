using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EB4 RID: 3764
	public class CreateMasterPainting : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075D6 RID: 30166 RVA: 0x002E1C48 File Offset: 0x002DFE48
		public override bool Success()
		{
			foreach (Painting painting in Components.Paintings.Items)
			{
				if (painting != null)
				{
					ArtableStage artableStage = Db.GetArtableStages().TryGet(painting.CurrentStage);
					if (artableStage != null && artableStage.statusItem == Db.Get().ArtableStatuses.LookingGreat)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060075D7 RID: 30167 RVA: 0x002E1CD4 File Offset: 0x002DFED4
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x060075D8 RID: 30168 RVA: 0x002E1CD6 File Offset: 0x002DFED6
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CREATE_A_PAINTING;
		}
	}
}

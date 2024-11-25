using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E99 RID: 3737
	public class CollectedSpaceArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600755C RID: 30044 RVA: 0x002DFFDC File Offset: 0x002DE1DC
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_SPACE_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedSpaceArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x0600755D RID: 30045 RVA: 0x002E001F File Offset: 0x002DE21F
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600755E RID: 30046 RVA: 0x002E002D File Offset: 0x002DE22D
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount >= 10;
		}

		// Token: 0x0600755F RID: 30047 RVA: 0x002E0040 File Offset: 0x002DE240
		private int GetStudiedSpaceArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount;
		}

		// Token: 0x06007560 RID: 30048 RVA: 0x002E004C File Offset: 0x002DE24C
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_SPACE_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x04005557 RID: 21847
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}

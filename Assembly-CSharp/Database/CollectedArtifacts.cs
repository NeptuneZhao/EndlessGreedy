using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E98 RID: 3736
	public class CollectedArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007556 RID: 30038 RVA: 0x002DFF38 File Offset: 0x002DE138
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x06007557 RID: 30039 RVA: 0x002DFF7B File Offset: 0x002DE17B
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007558 RID: 30040 RVA: 0x002DFF89 File Offset: 0x002DE189
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount >= 10;
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x002DFF9C File Offset: 0x002DE19C
		private int GetStudiedArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount;
		}

		// Token: 0x0600755A RID: 30042 RVA: 0x002DFFA8 File Offset: 0x002DE1A8
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x04005556 RID: 21846
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}

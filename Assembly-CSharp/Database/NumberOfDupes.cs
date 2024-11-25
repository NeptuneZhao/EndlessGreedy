using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E92 RID: 3730
	public class NumberOfDupes : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007535 RID: 30005 RVA: 0x002DFA02 File Offset: 0x002DDC02
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS, this.numDupes);
		}

		// Token: 0x06007536 RID: 30006 RVA: 0x002DFA1E File Offset: 0x002DDC1E
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS_DESCRIPTION, this.numDupes);
		}

		// Token: 0x06007537 RID: 30007 RVA: 0x002DFA3A File Offset: 0x002DDC3A
		public NumberOfDupes(int num)
		{
			this.numDupes = num;
		}

		// Token: 0x06007538 RID: 30008 RVA: 0x002DFA49 File Offset: 0x002DDC49
		public override bool Success()
		{
			return Components.LiveMinionIdentities.Items.Count >= this.numDupes;
		}

		// Token: 0x06007539 RID: 30009 RVA: 0x002DFA65 File Offset: 0x002DDC65
		public void Deserialize(IReader reader)
		{
			this.numDupes = reader.ReadInt32();
		}

		// Token: 0x0600753A RID: 30010 RVA: 0x002DFA73 File Offset: 0x002DDC73
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POPULATION, complete ? this.numDupes : Components.LiveMinionIdentities.Items.Count, this.numDupes);
		}

		// Token: 0x04005550 RID: 21840
		private int numDupes;
	}
}

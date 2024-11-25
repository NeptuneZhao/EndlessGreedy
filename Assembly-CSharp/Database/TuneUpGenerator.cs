using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EB8 RID: 3768
	public class TuneUpGenerator : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075E6 RID: 30182 RVA: 0x002E1E77 File Offset: 0x002E0077
		public TuneUpGenerator(float numChoreseToComplete)
		{
			this.numChoreseToComplete = numChoreseToComplete;
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x002E1E88 File Offset: 0x002E0088
		public override bool Success()
		{
			float num = 0f;
			ReportManager.ReportEntry entry = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.ChoreStatus);
			for (int i = 0; i < entry.contextEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = entry.contextEntries[i];
				if (reportEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
				{
					num += reportEntry.Negative;
				}
			}
			string name = Db.Get().ChoreTypes.PowerTinker.Name;
			int count = ReportManager.Instance.reports.Count;
			for (int j = 0; j < count; j++)
			{
				ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[j].GetEntry(ReportManager.ReportType.ChoreStatus);
				int count2 = entry2.contextEntries.Count;
				for (int k = 0; k < count2; k++)
				{
					ReportManager.ReportEntry reportEntry2 = entry2.contextEntries[k];
					if (reportEntry2.context == name)
					{
						num += reportEntry2.Negative;
					}
				}
			}
			this.choresCompleted = Math.Abs(num);
			return Math.Abs(num) >= this.numChoreseToComplete;
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x002E1FB4 File Offset: 0x002E01B4
		public void Deserialize(IReader reader)
		{
			this.numChoreseToComplete = reader.ReadSingle();
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x002E1FC4 File Offset: 0x002E01C4
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CHORES_OF_TYPE, complete ? this.numChoreseToComplete : this.choresCompleted, this.numChoreseToComplete, Db.Get().ChoreTypes.PowerTinker.Name);
		}

		// Token: 0x04005576 RID: 21878
		private float numChoreseToComplete;

		// Token: 0x04005577 RID: 21879
		private float choresCompleted;
	}
}

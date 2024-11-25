using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EA4 RID: 3748
	public class SkillBranchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007593 RID: 30099 RVA: 0x002E0678 File Offset: 0x002DE878
		public SkillBranchComplete(List<Skill> skillsToMaster)
		{
			this.skillsToMaster = skillsToMaster;
		}

		// Token: 0x06007594 RID: 30100 RVA: 0x002E0688 File Offset: 0x002DE888
		public override bool Success()
		{
			foreach (MinionResume minionResume in Components.MinionResumes.Items)
			{
				foreach (Skill skill in this.skillsToMaster)
				{
					if (minionResume.HasMasteredSkill(skill.Id))
					{
						if (!minionResume.HasBeenGrantedSkill(skill))
						{
							return true;
						}
						List<Skill> allPriorSkills = Db.Get().Skills.GetAllPriorSkills(skill);
						bool flag = true;
						foreach (Skill skill2 in allPriorSkills)
						{
							flag = (flag && minionResume.HasMasteredSkill(skill2.Id));
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007595 RID: 30101 RVA: 0x002E07A8 File Offset: 0x002DE9A8
		public void Deserialize(IReader reader)
		{
			this.skillsToMaster = new List<Skill>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string id = reader.ReadKleiString();
				this.skillsToMaster.Add(Db.Get().Skills.Get(id));
			}
		}

		// Token: 0x06007596 RID: 30102 RVA: 0x002E07F5 File Offset: 0x002DE9F5
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SKILL_BRANCH;
		}

		// Token: 0x0400555D RID: 21853
		private List<Skill> skillsToMaster;
	}
}

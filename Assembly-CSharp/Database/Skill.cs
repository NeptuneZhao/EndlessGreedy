using System;
using System.Collections.Generic;
using TUNING;

namespace Database
{
	// Token: 0x02000ED2 RID: 3794
	public class Skill : Resource
	{
		// Token: 0x0600763D RID: 30269 RVA: 0x002E5F20 File Offset: 0x002E4120
		public Skill(string id, string name, string description, string dlcId, int tier, string hat, string badge, string skillGroup, List<SkillPerk> perks = null, List<string> priorSkills = null) : base(id, name)
		{
			this.description = description;
			this.dlcId = dlcId;
			this.tier = tier;
			this.hat = hat;
			this.badge = badge;
			this.skillGroup = skillGroup;
			this.perks = perks;
			if (this.perks == null)
			{
				this.perks = new List<SkillPerk>();
			}
			this.priorSkills = priorSkills;
			if (this.priorSkills == null)
			{
				this.priorSkills = new List<string>();
			}
		}

		// Token: 0x0600763E RID: 30270 RVA: 0x002E5F9A File Offset: 0x002E419A
		public int GetMoraleExpectation()
		{
			return SKILLS.SKILL_TIER_MORALE_COST[this.tier];
		}

		// Token: 0x0600763F RID: 30271 RVA: 0x002E5FA8 File Offset: 0x002E41A8
		public bool GivesPerk(SkillPerk perk)
		{
			return this.perks.Contains(perk);
		}

		// Token: 0x06007640 RID: 30272 RVA: 0x002E5FB8 File Offset: 0x002E41B8
		public bool GivesPerk(HashedString perkId)
		{
			using (List<SkillPerk>.Enumerator enumerator = this.perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IdHash == perkId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0400562C RID: 22060
		public string description;

		// Token: 0x0400562D RID: 22061
		public string dlcId;

		// Token: 0x0400562E RID: 22062
		public string skillGroup;

		// Token: 0x0400562F RID: 22063
		public string hat;

		// Token: 0x04005630 RID: 22064
		public string badge;

		// Token: 0x04005631 RID: 22065
		public int tier;

		// Token: 0x04005632 RID: 22066
		public bool deprecated;

		// Token: 0x04005633 RID: 22067
		public List<SkillPerk> perks;

		// Token: 0x04005634 RID: 22068
		public List<string> priorSkills;
	}
}

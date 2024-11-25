using System;

namespace Database
{
	// Token: 0x02000ECE RID: 3790
	public class SimpleSkillPerk : SkillPerk
	{
		// Token: 0x06007637 RID: 30263 RVA: 0x002E49B3 File Offset: 0x002E2BB3
		public SimpleSkillPerk(string id, string description) : base(id, description, null, null, null, false)
		{
		}

		// Token: 0x06007638 RID: 30264 RVA: 0x002E49C1 File Offset: 0x002E2BC1
		public SimpleSkillPerk(string id, string description, string[] requiredDlcIds) : base(id, description, null, null, null, requiredDlcIds, false)
		{
		}
	}
}

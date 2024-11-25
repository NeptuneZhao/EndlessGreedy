using System;

namespace Database
{
	// Token: 0x02000E90 RID: 3728
	public abstract class ColonyAchievementRequirement
	{
		// Token: 0x0600752B RID: 29995
		public abstract bool Success();

		// Token: 0x0600752C RID: 29996 RVA: 0x002DF950 File Offset: 0x002DDB50
		public virtual bool Fail()
		{
			return false;
		}

		// Token: 0x0600752D RID: 29997 RVA: 0x002DF953 File Offset: 0x002DDB53
		public virtual string GetProgress(bool complete)
		{
			return "";
		}
	}
}

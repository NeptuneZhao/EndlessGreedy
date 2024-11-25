using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EB6 RID: 3766
	public class CritterTypeExists : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075DE RID: 30174 RVA: 0x002E1D78 File Offset: 0x002DFF78
		public CritterTypeExists(List<Tag> critterTypes)
		{
			this.critterTypes = critterTypes;
		}

		// Token: 0x060075DF RID: 30175 RVA: 0x002E1D94 File Offset: 0x002DFF94
		public override bool Success()
		{
			foreach (Capturable cmp in Components.Capturables.Items)
			{
				if (this.critterTypes.Contains(cmp.PrefabID()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x002E1E00 File Offset: 0x002E0000
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.critterTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.critterTypes.Add(new Tag(name));
			}
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x002E1E44 File Offset: 0x002E0044
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HATCH_A_MORPH;
		}

		// Token: 0x04005575 RID: 21877
		private List<Tag> critterTypes = new List<Tag>();
	}
}

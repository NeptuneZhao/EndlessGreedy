using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E9F RID: 3743
	public class EstablishColonies : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600757B RID: 30075 RVA: 0x002E01B0 File Offset: 0x002DE3B0
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ESTABLISH_COLONIES.Replace("{goalBaseCount}", EstablishColonies.BASE_COUNT.ToString()).Replace("{baseCount}", this.GetColonyCount().ToString()).Replace("{neededCount}", EstablishColonies.BASE_COUNT.ToString());
		}

		// Token: 0x0600757C RID: 30076 RVA: 0x002E0207 File Offset: 0x002DE407
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600757D RID: 30077 RVA: 0x002E0215 File Offset: 0x002DE415
		public override bool Success()
		{
			return this.GetColonyCount() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x0600757E RID: 30078 RVA: 0x002E0227 File Offset: 0x002DE427
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.SEVERAL_COLONIES;
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x002E0234 File Offset: 0x002DE434
		private int GetColonyCount()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04005558 RID: 21848
		public static int BASE_COUNT = 5;
	}
}

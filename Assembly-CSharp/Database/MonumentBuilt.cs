using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x02000E91 RID: 3729
	public class MonumentBuilt : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600752F RID: 29999 RVA: 0x002DF962 File Offset: 0x002DDB62
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT;
		}

		// Token: 0x06007530 RID: 30000 RVA: 0x002DF96E File Offset: 0x002DDB6E
		public override string Description()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT_DESCRIPTION;
		}

		// Token: 0x06007531 RID: 30001 RVA: 0x002DF97C File Offset: 0x002DDB7C
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.MonumentParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((MonumentPart)enumerator.Current).IsMonumentCompleted())
					{
						Game.Instance.unlocks.Unlock("thriving", true);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007532 RID: 30002 RVA: 0x002DF9F0 File Offset: 0x002DDBF0
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007533 RID: 30003 RVA: 0x002DF9F2 File Offset: 0x002DDBF2
		public override string GetProgress(bool complete)
		{
			return this.Name();
		}
	}
}

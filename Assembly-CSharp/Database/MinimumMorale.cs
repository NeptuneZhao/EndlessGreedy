using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E96 RID: 3734
	public class MinimumMorale : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600754A RID: 30026 RVA: 0x002DFC93 File Offset: 0x002DDE93
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE, this.minimumMorale);
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x002DFCAF File Offset: 0x002DDEAF
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE_DESCRIPTION, this.minimumMorale);
		}

		// Token: 0x0600754C RID: 30028 RVA: 0x002DFCCB File Offset: 0x002DDECB
		public MinimumMorale(int minimumMorale = 16)
		{
			this.minimumMorale = minimumMorale;
		}

		// Token: 0x0600754D RID: 30029 RVA: 0x002DFCDC File Offset: 0x002DDEDC
		public override bool Success()
		{
			bool flag = true;
			foreach (object obj in Components.MinionAssignablesProxy)
			{
				GameObject targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
				if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead))
				{
					AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());
					flag = (attributeInstance != null && attributeInstance.GetTotalValue() >= (float)this.minimumMorale && flag);
				}
			}
			return flag;
		}

		// Token: 0x0600754E RID: 30030 RVA: 0x002DFD84 File Offset: 0x002DDF84
		public void Deserialize(IReader reader)
		{
			this.minimumMorale = reader.ReadInt32();
		}

		// Token: 0x0600754F RID: 30031 RVA: 0x002DFD92 File Offset: 0x002DDF92
		public override string GetProgress(bool complete)
		{
			return this.Description();
		}

		// Token: 0x04005554 RID: 21844
		public int minimumMorale;
	}
}

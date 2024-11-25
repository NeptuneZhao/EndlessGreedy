using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000EA6 RID: 3750
	public class CritterTypesWithTraits : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600759B RID: 30107 RVA: 0x002E0970 File Offset: 0x002DEB70
		public CritterTypesWithTraits(List<Tag> critterTypes)
		{
			foreach (Tag key in critterTypes)
			{
				if (!this.critterTypesToCheck.ContainsKey(key))
				{
					this.critterTypesToCheck.Add(key, false);
				}
			}
			this.hasTrait = false;
			this.trait = GameTags.Creatures.Wild;
		}

		// Token: 0x0600759C RID: 30108 RVA: 0x002E0A00 File Offset: 0x002DEC00
		public override bool Success()
		{
			HashSet<Tag> tamedCritterTypes = SaveGame.Instance.ColonyAchievementTracker.tamedCritterTypes;
			bool flag = true;
			foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
			{
				flag = (flag && tamedCritterTypes.Contains(keyValuePair.Key));
			}
			this.UpdateSavedState();
			return flag;
		}

		// Token: 0x0600759D RID: 30109 RVA: 0x002E0A7C File Offset: 0x002DEC7C
		public void UpdateSavedState()
		{
			this.revisedCritterTypesToCheckState.Clear();
			HashSet<Tag> tamedCritterTypes = SaveGame.Instance.ColonyAchievementTracker.tamedCritterTypes;
			foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
			{
				this.revisedCritterTypesToCheckState.Add(keyValuePair.Key, tamedCritterTypes.Contains(keyValuePair.Key));
			}
			foreach (KeyValuePair<Tag, bool> keyValuePair2 in this.revisedCritterTypesToCheckState)
			{
				this.critterTypesToCheck[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}

		// Token: 0x0600759E RID: 30110 RVA: 0x002E0B58 File Offset: 0x002DED58
		public void Deserialize(IReader reader)
		{
			this.critterTypesToCheck = new Dictionary<Tag, bool>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				bool value = reader.ReadByte() > 0;
				this.critterTypesToCheck.Add(new Tag(name), value);
			}
			this.hasTrait = (reader.ReadByte() > 0);
			this.trait = GameTags.Creatures.Wild;
		}

		// Token: 0x04005560 RID: 21856
		public Dictionary<Tag, bool> critterTypesToCheck = new Dictionary<Tag, bool>();

		// Token: 0x04005561 RID: 21857
		private Tag trait;

		// Token: 0x04005562 RID: 21858
		private bool hasTrait;

		// Token: 0x04005563 RID: 21859
		private Dictionary<Tag, bool> revisedCritterTypesToCheckState = new Dictionary<Tag, bool>();
	}
}

using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000EA7 RID: 3751
	public class ProduceXEngeryWithoutUsingYList : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600759F RID: 30111 RVA: 0x002E0BC0 File Offset: 0x002DEDC0
		public ProduceXEngeryWithoutUsingYList(float amountToProduce, List<Tag> disallowedBuildings)
		{
			this.disallowedBuildings = disallowedBuildings;
			this.amountToProduce = amountToProduce;
			this.usedDisallowedBuilding = false;
		}

		// Token: 0x060075A0 RID: 30112 RVA: 0x002E0BE8 File Offset: 0x002DEDE8
		public override bool Success()
		{
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num / 1000f > this.amountToProduce;
		}

		// Token: 0x060075A1 RID: 30113 RVA: 0x002E0C70 File Offset: 0x002DEE70
		public override bool Fail()
		{
			foreach (Tag key in this.disallowedBuildings)
			{
				if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060075A2 RID: 30114 RVA: 0x002E0CDC File Offset: 0x002DEEDC
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.disallowedBuildings = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.disallowedBuildings.Add(new Tag(name));
			}
			this.amountProduced = (float)reader.ReadDouble();
			this.amountToProduce = (float)reader.ReadDouble();
			this.usedDisallowedBuilding = (reader.ReadByte() > 0);
		}

		// Token: 0x060075A3 RID: 30115 RVA: 0x002E0D4C File Offset: 0x002DEF4C
		public float GetProductionAmount(bool complete)
		{
			if (complete)
			{
				return this.amountToProduce * 1000f;
			}
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		// Token: 0x04005564 RID: 21860
		public List<Tag> disallowedBuildings = new List<Tag>();

		// Token: 0x04005565 RID: 21861
		public float amountToProduce;

		// Token: 0x04005566 RID: 21862
		private float amountProduced;

		// Token: 0x04005567 RID: 21863
		private bool usedDisallowedBuilding;
	}
}

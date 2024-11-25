using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EA5 RID: 3749
	public class EquipNDupes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007597 RID: 30103 RVA: 0x002E0801 File Offset: 0x002DEA01
		public EquipNDupes(AssignableSlot equipmentSlot, int numToEquip)
		{
			this.equipmentSlot = equipmentSlot;
			this.numToEquip = numToEquip;
		}

		// Token: 0x06007598 RID: 30104 RVA: 0x002E0818 File Offset: 0x002DEA18
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return num >= this.numToEquip;
		}

		// Token: 0x06007599 RID: 30105 RVA: 0x002E0898 File Offset: 0x002DEA98
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.equipmentSlot = Db.Get().AssignableSlots.Get(id);
			this.numToEquip = reader.ReadInt32();
		}

		// Token: 0x0600759A RID: 30106 RVA: 0x002E08D0 File Offset: 0x002DEAD0
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CLOTHE_DUPES, complete ? this.numToEquip : num, this.numToEquip);
		}

		// Token: 0x0400555E RID: 21854
		private AssignableSlot equipmentSlot;

		// Token: 0x0400555F RID: 21855
		private int numToEquip;
	}
}

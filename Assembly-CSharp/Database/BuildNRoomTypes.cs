using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EA1 RID: 3745
	public class BuildNRoomTypes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007586 RID: 30086 RVA: 0x002E034E File Offset: 0x002DE54E
		public BuildNRoomTypes(RoomType roomType, int numToCreate = 1)
		{
			this.roomType = roomType;
			this.numToCreate = numToCreate;
		}

		// Token: 0x06007587 RID: 30087 RVA: 0x002E0364 File Offset: 0x002DE564
		public override bool Success()
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return num >= this.numToCreate;
		}

		// Token: 0x06007588 RID: 30088 RVA: 0x002E03D8 File Offset: 0x002DE5D8
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
			this.numToCreate = reader.ReadInt32();
		}

		// Token: 0x06007589 RID: 30089 RVA: 0x002E0410 File Offset: 0x002DE610
		public override string GetProgress(bool complete)
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_N_ROOMS, this.roomType.Name, complete ? this.numToCreate : num, this.numToCreate);
		}

		// Token: 0x0400555A RID: 21850
		private RoomType roomType;

		// Token: 0x0400555B RID: 21851
		private int numToCreate;
	}
}

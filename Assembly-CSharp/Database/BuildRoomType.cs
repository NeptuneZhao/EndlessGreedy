using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000EA0 RID: 3744
	public class BuildRoomType : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007582 RID: 30082 RVA: 0x002E028F File Offset: 0x002DE48F
		public BuildRoomType(RoomType roomType)
		{
			this.roomType = roomType;
		}

		// Token: 0x06007583 RID: 30083 RVA: 0x002E02A0 File Offset: 0x002DE4A0
		public override bool Success()
		{
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007584 RID: 30084 RVA: 0x002E0308 File Offset: 0x002DE508
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
		}

		// Token: 0x06007585 RID: 30085 RVA: 0x002E0332 File Offset: 0x002DE532
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_A_ROOM, this.roomType.Name);
		}

		// Token: 0x04005559 RID: 21849
		private RoomType roomType;
	}
}

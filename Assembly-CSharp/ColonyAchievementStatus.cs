using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Database;

// Token: 0x020007C2 RID: 1986
public class ColonyAchievementStatus
{
	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x060036CA RID: 14026 RVA: 0x0012A0DE File Offset: 0x001282DE
	public List<ColonyAchievementRequirement> Requirements
	{
		get
		{
			return this.m_achievement.requirementChecklist;
		}
	}

	// Token: 0x060036CB RID: 14027 RVA: 0x0012A0EC File Offset: 0x001282EC
	public ColonyAchievementStatus(string achievementId)
	{
		this.m_achievement = Db.Get().ColonyAchievements.TryGet(achievementId);
		if (this.m_achievement == null)
		{
			this.m_achievement = new ColonyAchievement();
			return;
		}
		if (!this.m_achievement.IsValidForSave())
		{
			this.m_achievement.Disabled = true;
		}
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x0012A144 File Offset: 0x00128344
	public void UpdateAchievement()
	{
		if (this.Requirements.Count <= 0)
		{
			return;
		}
		if (this.m_achievement.Disabled)
		{
			return;
		}
		this.success = true;
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in this.Requirements)
		{
			this.success &= colonyAchievementRequirement.Success();
			this.failed |= colonyAchievementRequirement.Fail();
		}
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x0012A1DC File Offset: 0x001283DC
	public static ColonyAchievementStatus Deserialize(IReader reader, string achievementId)
	{
		bool flag = reader.ReadByte() > 0;
		bool flag2 = reader.ReadByte() > 0;
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 22))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Type type = Type.GetType(reader.ReadKleiString());
				if (type != null)
				{
					AchievementRequirementSerialization_Deprecated achievementRequirementSerialization_Deprecated = FormatterServices.GetUninitializedObject(type) as AchievementRequirementSerialization_Deprecated;
					Debug.Assert(achievementRequirementSerialization_Deprecated != null, string.Format("Cannot deserialize old data for type {0}", type));
					achievementRequirementSerialization_Deprecated.Deserialize(reader);
				}
			}
		}
		return new ColonyAchievementStatus(achievementId)
		{
			success = flag,
			failed = flag2
		};
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x0012A27D File Offset: 0x0012847D
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.success ? 1 : 0);
		writer.Write(this.failed ? 1 : 0);
	}

	// Token: 0x0400205F RID: 8287
	public bool success;

	// Token: 0x04002060 RID: 8288
	public bool failed;

	// Token: 0x04002061 RID: 8289
	private ColonyAchievement m_achievement;
}

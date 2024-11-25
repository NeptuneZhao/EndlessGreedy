using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000A83 RID: 2691
[Serializable]
public class ScheduleBlock
{
	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06004EEF RID: 20207 RVA: 0x001C6907 File Offset: 0x001C4B07
	public List<ScheduleBlockType> allowed_types
	{
		get
		{
			Debug.Assert(!string.IsNullOrEmpty(this._groupId));
			return Db.Get().ScheduleGroups.Get(this._groupId).allowedTypes;
		}
	}

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x001C693F File Offset: 0x001C4B3F
	// (set) Token: 0x06004EF0 RID: 20208 RVA: 0x001C6936 File Offset: 0x001C4B36
	public string GroupId
	{
		get
		{
			return this._groupId;
		}
		set
		{
			this._groupId = value;
		}
	}

	// Token: 0x06004EF2 RID: 20210 RVA: 0x001C6947 File Offset: 0x001C4B47
	public ScheduleBlock(string name, string groupId)
	{
		this.name = name;
		this._groupId = groupId;
	}

	// Token: 0x06004EF3 RID: 20211 RVA: 0x001C6960 File Offset: 0x001C4B60
	public bool IsAllowed(ScheduleBlockType type)
	{
		if (this.allowed_types != null)
		{
			foreach (ScheduleBlockType scheduleBlockType in this.allowed_types)
			{
				if (type.IdHash == scheduleBlockType.IdHash)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04003478 RID: 13432
	[Serialize]
	public string name;

	// Token: 0x04003479 RID: 13433
	[Serialize]
	private string _groupId;
}

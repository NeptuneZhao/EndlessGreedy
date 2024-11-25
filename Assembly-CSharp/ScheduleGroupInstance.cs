using System;
using KSerialization;

// Token: 0x02000604 RID: 1540
[SerializationConfig(MemberSerialization.OptIn)]
public class ScheduleGroupInstance
{
	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060025EC RID: 9708 RVA: 0x000D2587 File Offset: 0x000D0787
	// (set) Token: 0x060025ED RID: 9709 RVA: 0x000D259E File Offset: 0x000D079E
	public ScheduleGroup scheduleGroup
	{
		get
		{
			return Db.Get().ScheduleGroups.Get(this.scheduleGroupID);
		}
		set
		{
			this.scheduleGroupID = value.Id;
		}
	}

	// Token: 0x060025EE RID: 9710 RVA: 0x000D25AC File Offset: 0x000D07AC
	public ScheduleGroupInstance(ScheduleGroup scheduleGroup)
	{
		this.scheduleGroup = scheduleGroup;
		this.segments = scheduleGroup.defaultSegments;
	}

	// Token: 0x040015A4 RID: 5540
	[Serialize]
	private string scheduleGroupID;

	// Token: 0x040015A5 RID: 5541
	[Serialize]
	public int segments;
}

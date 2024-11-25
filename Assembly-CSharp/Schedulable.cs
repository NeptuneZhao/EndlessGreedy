using System;
using UnityEngine;

// Token: 0x02000A82 RID: 2690
[AddComponentMenu("KMonoBehaviour/scripts/Schedulable")]
public class Schedulable : KMonoBehaviour
{
	// Token: 0x06004EE9 RID: 20201 RVA: 0x001C6848 File Offset: 0x001C4A48
	public Schedule GetSchedule()
	{
		return ScheduleManager.Instance.GetSchedule(this);
	}

	// Token: 0x06004EEA RID: 20202 RVA: 0x001C6858 File Offset: 0x001C4A58
	public bool IsAllowed(ScheduleBlockType schedule_block_type)
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Trying to schedule {0} but {1} is not on a valid world. Grid cell: {2}", schedule_block_type.Id, base.gameObject.name, Grid.PosToCell(base.gameObject.GetComponent<KPrefabID>()))
			});
			return false;
		}
		return myWorld.AlertManager.IsRedAlert() || ScheduleManager.Instance.IsAllowed(this, schedule_block_type);
	}

	// Token: 0x06004EEB RID: 20203 RVA: 0x001C68D5 File Offset: 0x001C4AD5
	public void OnScheduleChanged(Schedule schedule)
	{
		base.Trigger(467134493, schedule);
	}

	// Token: 0x06004EEC RID: 20204 RVA: 0x001C68E3 File Offset: 0x001C4AE3
	public void OnScheduleBlocksTick(Schedule schedule)
	{
		base.Trigger(1714332666, schedule);
	}

	// Token: 0x06004EED RID: 20205 RVA: 0x001C68F1 File Offset: 0x001C4AF1
	public void OnScheduleBlocksChanged(Schedule schedule)
	{
		base.Trigger(-894023145, schedule);
	}
}

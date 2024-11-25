using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E7B RID: 3707
	public class ScheduleGroups : ResourceSet<ScheduleGroup>
	{
		// Token: 0x060074D8 RID: 29912 RVA: 0x002DA94C File Offset: 0x002D8B4C
		public ScheduleGroup Add(string id, int defaultSegments, string name, string description, Color uiColor, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false)
		{
			ScheduleGroup scheduleGroup = new ScheduleGroup(id, this, defaultSegments, name, description, uiColor, notificationTooltip, allowedTypes, alarm);
			this.allGroups.Add(scheduleGroup);
			return scheduleGroup;
		}

		// Token: 0x060074D9 RID: 29913 RVA: 0x002DA97C File Offset: 0x002D8B7C
		public ScheduleGroups(ResourceSet parent) : base("ScheduleGroups", parent)
		{
			this.allGroups = new List<ScheduleGroup>();
			this.Hygene = this.Add("Hygene", 1, UI.SCHEDULEGROUPS.HYGENE.NAME, UI.SCHEDULEGROUPS.HYGENE.DESCRIPTION, Util.ColorFromHex("5A8DAF"), UI.SCHEDULEGROUPS.HYGENE.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>
			{
				Db.Get().ScheduleBlockTypes.Hygiene,
				Db.Get().ScheduleBlockTypes.Work
			}, false);
			this.Worktime = this.Add("Worktime", 18, UI.SCHEDULEGROUPS.WORKTIME.NAME, UI.SCHEDULEGROUPS.WORKTIME.DESCRIPTION, Util.ColorFromHex("FFA649"), UI.SCHEDULEGROUPS.WORKTIME.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>
			{
				Db.Get().ScheduleBlockTypes.Work
			}, true);
			this.Recreation = this.Add("Recreation", 2, UI.SCHEDULEGROUPS.RECREATION.NAME, UI.SCHEDULEGROUPS.RECREATION.DESCRIPTION, Util.ColorFromHex("70DFAD"), UI.SCHEDULEGROUPS.RECREATION.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>
			{
				Db.Get().ScheduleBlockTypes.Hygiene,
				Db.Get().ScheduleBlockTypes.Eat,
				Db.Get().ScheduleBlockTypes.Recreation,
				Db.Get().ScheduleBlockTypes.Work
			}, false);
			this.Sleep = this.Add("Sleep", 3, UI.SCHEDULEGROUPS.SLEEP.NAME, UI.SCHEDULEGROUPS.SLEEP.DESCRIPTION, Util.ColorFromHex("273469"), UI.SCHEDULEGROUPS.SLEEP.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>
			{
				Db.Get().ScheduleBlockTypes.Sleep
			}, false);
			int num = 0;
			foreach (ScheduleGroup scheduleGroup in this.allGroups)
			{
				num += scheduleGroup.defaultSegments;
			}
			global::Debug.Assert(num == 24, "Default schedule groups must add up to exactly 1 cycle!");
		}

		// Token: 0x060074DA RID: 29914 RVA: 0x002DABA4 File Offset: 0x002D8DA4
		public ScheduleGroup FindGroupForScheduleTypes(List<ScheduleBlockType> types)
		{
			foreach (ScheduleGroup scheduleGroup in this.allGroups)
			{
				if (Schedule.AreScheduleTypesIdentical(scheduleGroup.allowedTypes, types))
				{
					return scheduleGroup;
				}
			}
			return null;
		}

		// Token: 0x04005491 RID: 21649
		public List<ScheduleGroup> allGroups;

		// Token: 0x04005492 RID: 21650
		public ScheduleGroup Hygene;

		// Token: 0x04005493 RID: 21651
		public ScheduleGroup Worktime;

		// Token: 0x04005494 RID: 21652
		public ScheduleGroup Recreation;

		// Token: 0x04005495 RID: 21653
		public ScheduleGroup Sleep;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A85 RID: 2693
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleManager")]
public class ScheduleManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06004F11 RID: 20241 RVA: 0x001C731C File Offset: 0x001C551C
	// (remove) Token: 0x06004F12 RID: 20242 RVA: 0x001C7354 File Offset: 0x001C5554
	public event Action<List<Schedule>> onSchedulesChanged;

	// Token: 0x06004F13 RID: 20243 RVA: 0x001C7389 File Offset: 0x001C5589
	public static void DestroyInstance()
	{
		ScheduleManager.Instance = null;
	}

	// Token: 0x06004F14 RID: 20244 RVA: 0x001C7391 File Offset: 0x001C5591
	public Schedule GetDefaultBionicSchedule()
	{
		return this.schedules.Find((Schedule match) => match.isDefaultForBionics);
	}

	// Token: 0x06004F15 RID: 20245 RVA: 0x001C73BD File Offset: 0x001C55BD
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true);
		}
	}

	// Token: 0x06004F16 RID: 20246 RVA: 0x001C73D3 File Offset: 0x001C55D3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.schedules = new List<Schedule>();
		ScheduleManager.Instance = this;
	}

	// Token: 0x06004F17 RID: 20247 RVA: 0x001C73EC File Offset: 0x001C55EC
	protected override void OnSpawn()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true);
		}
		foreach (Schedule schedule in this.schedules)
		{
			schedule.ClearNullReferences();
		}
		List<ScheduleBlock> scheduleBlocksFromGroupDefaults = Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups);
		foreach (Schedule schedule2 in this.schedules)
		{
			List<ScheduleBlock> blocks = schedule2.GetBlocks();
			for (int i = 0; i < blocks.Count; i++)
			{
				ScheduleBlock scheduleBlock = blocks[i];
				if (Db.Get().ScheduleGroups.FindGroupForScheduleTypes(scheduleBlock.allowed_types) == null)
				{
					ScheduleGroup group = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(scheduleBlocksFromGroupDefaults[i].allowed_types);
					schedule2.SetBlockGroup(i, group);
				}
			}
		}
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			Schedulable component = minionIdentity.GetComponent<Schedulable>();
			if (this.GetSchedule(component) == null)
			{
				this.schedules[0].Assign(component);
			}
		}
		Components.LiveMinionIdentities.OnAdd += this.OnAddDupe;
		Components.LiveMinionIdentities.OnRemove += this.OnRemoveDupe;
	}

	// Token: 0x06004F18 RID: 20248 RVA: 0x001C7598 File Offset: 0x001C5798
	private void OnAddDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		Schedule schedule = this.schedules[0];
		if (minion.model == GameTags.Minions.Models.Bionic)
		{
			if (this.GetDefaultBionicSchedule() == null)
			{
				if (!this.hasDeletedDefaultBionicSchedule)
				{
					Schedule schedule2 = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, "_Bionics Default", false);
					schedule2.AddTimetable(Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups));
					schedule2.AddTimetable(Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups));
					for (int i = 0; i < schedule2.GetBlocks().Count; i++)
					{
						schedule2.SetBlockGroup(i, Db.Get().ScheduleGroups.Worktime);
					}
					for (int j = 1; j <= 6; j++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - j, Db.Get().ScheduleGroups.Sleep);
					}
					for (int k = 7; k <= 10; k++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - k, Db.Get().ScheduleGroups.Recreation);
					}
					for (int l = 11; l <= 11; l++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - l, Db.Get().ScheduleGroups.Hygene);
					}
					schedule = schedule2;
					schedule2.isDefaultForBionics = true;
				}
			}
			else
			{
				schedule = this.GetDefaultBionicSchedule();
			}
		}
		else if (this.GetSchedule(component) != null)
		{
			schedule = this.GetSchedule(component);
		}
		schedule.Assign(component);
	}

	// Token: 0x06004F19 RID: 20249 RVA: 0x001C7728 File Offset: 0x001C5928
	private void OnRemoveDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		Schedule schedule = this.GetSchedule(component);
		if (schedule != null)
		{
			schedule.Unassign(component);
		}
	}

	// Token: 0x06004F1A RID: 20250 RVA: 0x001C7750 File Offset: 0x001C5950
	public void OnStoredDupeDestroyed(StoredMinionIdentity dupe)
	{
		foreach (Schedule schedule in this.schedules)
		{
			schedule.Unassign(dupe.gameObject.GetComponent<Schedulable>());
		}
	}

	// Token: 0x06004F1B RID: 20251 RVA: 0x001C77AC File Offset: 0x001C59AC
	public void AddDefaultSchedule(bool alarmOn)
	{
		Schedule schedule = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT, alarmOn);
		if (Game.Instance.FastWorkersModeActive)
		{
			for (int i = 0; i < 21; i++)
			{
				schedule.SetBlockGroup(i, Db.Get().ScheduleGroups.Worktime);
			}
			schedule.SetBlockGroup(21, Db.Get().ScheduleGroups.Recreation);
			schedule.SetBlockGroup(22, Db.Get().ScheduleGroups.Recreation);
			schedule.SetBlockGroup(23, Db.Get().ScheduleGroups.Sleep);
		}
	}

	// Token: 0x06004F1C RID: 20252 RVA: 0x001C7850 File Offset: 0x001C5A50
	public Schedule AddSchedule(List<ScheduleGroup> groups, string name = null, bool alarmOn = false)
	{
		if (name == null)
		{
			this.scheduleNameIncrementor++;
			name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule(name, groups, alarmOn);
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x06004F1D RID: 20253 RVA: 0x001C78BC File Offset: 0x001C5ABC
	public Schedule DuplicateSchedule(Schedule source)
	{
		if (base.name == null)
		{
			this.scheduleNameIncrementor++;
			base.name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule("copy of " + source.name, source.GetBlocks(), source.alarmActivated);
		schedule.ProgressTimetableIdx = source.ProgressTimetableIdx;
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x06004F1E RID: 20254 RVA: 0x001C7954 File Offset: 0x001C5B54
	public void DeleteSchedule(Schedule schedule)
	{
		if (this.schedules.Count == 1)
		{
			return;
		}
		List<Ref<Schedulable>> assigned = schedule.GetAssigned();
		if (schedule.isDefaultForBionics)
		{
			this.hasDeletedDefaultBionicSchedule = true;
		}
		this.schedules.Remove(schedule);
		foreach (Ref<Schedulable> @ref in assigned)
		{
			this.schedules[0].Assign(@ref.Get());
		}
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
	}

	// Token: 0x06004F1F RID: 20255 RVA: 0x001C79FC File Offset: 0x001C5BFC
	public Schedule GetSchedule(Schedulable schedulable)
	{
		foreach (Schedule schedule in this.schedules)
		{
			if (schedule.IsAssigned(schedulable))
			{
				return schedule;
			}
		}
		return null;
	}

	// Token: 0x06004F20 RID: 20256 RVA: 0x001C7A58 File Offset: 0x001C5C58
	public List<Schedule> GetSchedules()
	{
		return this.schedules;
	}

	// Token: 0x06004F21 RID: 20257 RVA: 0x001C7A60 File Offset: 0x001C5C60
	public bool IsAllowed(Schedulable schedulable, ScheduleBlockType schedule_block_type)
	{
		Schedule schedule = this.GetSchedule(schedulable);
		return schedule != null && schedule.GetCurrentScheduleBlock().IsAllowed(schedule_block_type);
	}

	// Token: 0x06004F22 RID: 20258 RVA: 0x001C7A86 File Offset: 0x001C5C86
	public static int GetCurrentHour()
	{
		return Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23);
	}

	// Token: 0x06004F23 RID: 20259 RVA: 0x001C7AA0 File Offset: 0x001C5CA0
	public void Sim33ms(float dt)
	{
		int currentHour = ScheduleManager.GetCurrentHour();
		if (ScheduleManager.GetCurrentHour() != this.lastHour)
		{
			foreach (Schedule schedule in this.schedules)
			{
				schedule.Tick();
			}
			this.lastHour = currentHour;
		}
	}

	// Token: 0x06004F24 RID: 20260 RVA: 0x001C7B0C File Offset: 0x001C5D0C
	public void PlayScheduleAlarm(Schedule schedule, ScheduleBlock block, bool forwards)
	{
		Notification notification = new Notification(string.Format(MISC.NOTIFICATIONS.SCHEDULE_CHANGED.NAME, schedule.name, block.name), NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SCHEDULE_CHANGED.TOOLTIP.Replace("{0}", schedule.name).Replace("{1}", block.name).Replace("{2}", Db.Get().ScheduleGroups.Get(block.GroupId).notificationTooltip), null, true, 0f, null, null, null, true, false, false);
		base.GetComponent<Notifier>().Add(notification, "");
		base.StartCoroutine(this.PlayScheduleTone(schedule, forwards));
	}

	// Token: 0x06004F25 RID: 20261 RVA: 0x001C7B97 File Offset: 0x001C5D97
	private IEnumerator PlayScheduleTone(Schedule schedule, bool forwards)
	{
		int[] tones = schedule.GetTones();
		int num2;
		for (int i = 0; i < tones.Length; i = num2 + 1)
		{
			int num = forwards ? i : (tones.Length - 1 - i);
			this.PlayTone(tones[num], forwards);
			yield return SequenceUtil.WaitForSeconds(TuningData<ScheduleManager.Tuning>.Get().toneSpacingSeconds);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x06004F26 RID: 20262 RVA: 0x001C7BB4 File Offset: 0x001C5DB4
	private void PlayTone(int pitch, bool forwards)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("WorkChime_tone", false), Vector3.zero, 1f);
		instance.setParameterByName("WorkChime_pitch", (float)pitch, false);
		instance.setParameterByName("WorkChime_start", (float)(forwards ? 1 : 0), false);
		KFMOD.EndOneShot(instance);
	}

	// Token: 0x04003482 RID: 13442
	[Serialize]
	private List<Schedule> schedules;

	// Token: 0x04003483 RID: 13443
	[Serialize]
	private int lastHour;

	// Token: 0x04003484 RID: 13444
	[Serialize]
	private int scheduleNameIncrementor;

	// Token: 0x04003486 RID: 13446
	public static ScheduleManager Instance;

	// Token: 0x04003487 RID: 13447
	[Serialize]
	private bool hasDeletedDefaultBionicSchedule;

	// Token: 0x02001AB8 RID: 6840
	public class Tuning : TuningData<ScheduleManager.Tuning>
	{
		// Token: 0x04007D74 RID: 32116
		public float toneSpacingSeconds;

		// Token: 0x04007D75 RID: 32117
		public int minToneIndex;

		// Token: 0x04007D76 RID: 32118
		public int maxToneIndex;

		// Token: 0x04007D77 RID: 32119
		public int firstLastToneSpacing;
	}
}

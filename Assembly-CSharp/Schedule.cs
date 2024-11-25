using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000A84 RID: 2692
[SerializationConfig(MemberSerialization.OptIn)]
public class Schedule : ISaveLoadable, IListableOption
{
	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x001C69D0 File Offset: 0x001C4BD0
	// (set) Token: 0x06004EF5 RID: 20213 RVA: 0x001C69D8 File Offset: 0x001C4BD8
	public int ProgressTimetableIdx
	{
		get
		{
			return this.progressTimetableIdx;
		}
		set
		{
			this.progressTimetableIdx = value;
		}
	}

	// Token: 0x06004EF6 RID: 20214 RVA: 0x001C69E1 File Offset: 0x001C4BE1
	public ScheduleBlock GetCurrentScheduleBlock()
	{
		return this.GetBlock(this.GetCurrentBlockIdx());
	}

	// Token: 0x06004EF7 RID: 20215 RVA: 0x001C69EF File Offset: 0x001C4BEF
	public int GetCurrentBlockIdx()
	{
		return Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23) + this.progressTimetableIdx * 24;
	}

	// Token: 0x06004EF8 RID: 20216 RVA: 0x001C6A13 File Offset: 0x001C4C13
	public ScheduleBlock GetPreviousScheduleBlock()
	{
		return this.GetBlock(this.GetPreviousBlockIdx());
	}

	// Token: 0x06004EF9 RID: 20217 RVA: 0x001C6A24 File Offset: 0x001C4C24
	public int GetPreviousBlockIdx()
	{
		int num = this.GetCurrentBlockIdx() - 1;
		if (num == -1)
		{
			num = this.blocks.Count - 1;
		}
		return num;
	}

	// Token: 0x06004EFA RID: 20218 RVA: 0x001C6A4D File Offset: 0x001C4C4D
	public void ClearNullReferences()
	{
		this.assigned.RemoveAll((Ref<Schedulable> x) => x.Get() == null);
	}

	// Token: 0x06004EFB RID: 20219 RVA: 0x001C6A7C File Offset: 0x001C4C7C
	public Schedule(string name, List<ScheduleGroup> defaultGroups, bool alarmActivated)
	{
		this.name = name;
		this.alarmActivated = alarmActivated;
		this.blocks = new List<ScheduleBlock>(defaultGroups.Count);
		this.assigned = new List<Ref<Schedulable>>();
		this.tones = this.GenerateTones();
		this.SetBlocksToGroupDefaults(defaultGroups);
	}

	// Token: 0x06004EFC RID: 20220 RVA: 0x001C6AD4 File Offset: 0x001C4CD4
	public Schedule(string name, List<ScheduleBlock> sourceBlocks, bool alarmActivated)
	{
		this.name = name;
		this.alarmActivated = alarmActivated;
		this.blocks = new List<ScheduleBlock>();
		for (int i = 0; i < sourceBlocks.Count; i++)
		{
			this.blocks.Add(new ScheduleBlock(sourceBlocks[i].name, sourceBlocks[i].GroupId));
		}
		this.assigned = new List<Ref<Schedulable>>();
		this.tones = this.GenerateTones();
		this.Changed();
	}

	// Token: 0x06004EFD RID: 20221 RVA: 0x001C6B5D File Offset: 0x001C4D5D
	public void SetBlocksToGroupDefaults(List<ScheduleGroup> defaultGroups)
	{
		this.blocks = Schedule.GetScheduleBlocksFromGroupDefaults(defaultGroups);
		global::Debug.Assert(this.blocks.Count == 24);
		this.Changed();
	}

	// Token: 0x06004EFE RID: 20222 RVA: 0x001C6B88 File Offset: 0x001C4D88
	public static List<ScheduleBlock> GetScheduleBlocksFromGroupDefaults(List<ScheduleGroup> defaultGroups)
	{
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		for (int i = 0; i < defaultGroups.Count; i++)
		{
			ScheduleGroup scheduleGroup = defaultGroups[i];
			for (int j = 0; j < scheduleGroup.defaultSegments; j++)
			{
				list.Add(new ScheduleBlock(scheduleGroup.Name, scheduleGroup.Id));
			}
		}
		return list;
	}

	// Token: 0x06004EFF RID: 20223 RVA: 0x001C6BE0 File Offset: 0x001C4DE0
	public void Tick()
	{
		ScheduleBlock currentScheduleBlock = this.GetCurrentScheduleBlock();
		ScheduleBlock block = this.GetBlock(this.GetPreviousBlockIdx());
		global::Debug.Assert(block != currentScheduleBlock);
		if (this.GetCurrentBlockIdx() % 24 == 0)
		{
			this.progressTimetableIdx++;
			if (this.progressTimetableIdx >= this.blocks.Count / 24)
			{
				this.progressTimetableIdx = 0;
			}
			if (ScheduleScreen.Instance != null)
			{
				ScheduleScreen.Instance.OnChangeCurrentTimetable();
			}
		}
		if (!Schedule.AreScheduleTypesIdentical(currentScheduleBlock.allowed_types, block.allowed_types))
		{
			ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(currentScheduleBlock.allowed_types);
			ScheduleGroup scheduleGroup2 = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(block.allowed_types);
			if (this.alarmActivated && scheduleGroup2.alarm != scheduleGroup.alarm)
			{
				ScheduleManager.Instance.PlayScheduleAlarm(this, currentScheduleBlock, scheduleGroup.alarm);
			}
			foreach (Ref<Schedulable> @ref in this.GetAssigned())
			{
				@ref.Get().OnScheduleBlocksChanged(this);
			}
		}
		foreach (Ref<Schedulable> ref2 in this.GetAssigned())
		{
			ref2.Get().OnScheduleBlocksTick(this);
		}
	}

	// Token: 0x06004F00 RID: 20224 RVA: 0x001C6D54 File Offset: 0x001C4F54
	string IListableOption.GetProperName()
	{
		return this.name;
	}

	// Token: 0x06004F01 RID: 20225 RVA: 0x001C6D5C File Offset: 0x001C4F5C
	public int[] GenerateTones()
	{
		int minToneIndex = TuningData<ScheduleManager.Tuning>.Get().minToneIndex;
		int maxToneIndex = TuningData<ScheduleManager.Tuning>.Get().maxToneIndex;
		int firstLastToneSpacing = TuningData<ScheduleManager.Tuning>.Get().firstLastToneSpacing;
		int[] array = new int[4];
		array[0] = UnityEngine.Random.Range(minToneIndex, maxToneIndex - firstLastToneSpacing + 1);
		array[1] = UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1);
		array[2] = UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1);
		array[3] = UnityEngine.Random.Range(array[0] + firstLastToneSpacing, maxToneIndex + 1);
		return array;
	}

	// Token: 0x06004F02 RID: 20226 RVA: 0x001C6DC8 File Offset: 0x001C4FC8
	public List<Ref<Schedulable>> GetAssigned()
	{
		if (this.assigned == null)
		{
			this.assigned = new List<Ref<Schedulable>>();
		}
		return this.assigned;
	}

	// Token: 0x06004F03 RID: 20227 RVA: 0x001C6DE3 File Offset: 0x001C4FE3
	public int[] GetTones()
	{
		if (this.tones == null)
		{
			this.tones = this.GenerateTones();
		}
		return this.tones;
	}

	// Token: 0x06004F04 RID: 20228 RVA: 0x001C6DFF File Offset: 0x001C4FFF
	public void SetBlockGroup(int idx, ScheduleGroup group)
	{
		if (0 <= idx && idx < this.blocks.Count)
		{
			this.blocks[idx] = new ScheduleBlock(group.Name, group.Id);
			this.Changed();
		}
	}

	// Token: 0x06004F05 RID: 20229 RVA: 0x001C6E38 File Offset: 0x001C5038
	private void Changed()
	{
		foreach (Ref<Schedulable> @ref in this.GetAssigned())
		{
			@ref.Get().OnScheduleChanged(this);
		}
		if (this.onChanged != null)
		{
			this.onChanged(this);
		}
	}

	// Token: 0x06004F06 RID: 20230 RVA: 0x001C6EA4 File Offset: 0x001C50A4
	public List<ScheduleBlock> GetBlocks()
	{
		return this.blocks;
	}

	// Token: 0x06004F07 RID: 20231 RVA: 0x001C6EAC File Offset: 0x001C50AC
	public ScheduleBlock GetBlock(int idx)
	{
		return this.blocks[idx];
	}

	// Token: 0x06004F08 RID: 20232 RVA: 0x001C6EBA File Offset: 0x001C50BA
	public void InsertTimetable(int timetableIdx, List<ScheduleBlock> newBlocks)
	{
		this.blocks.InsertRange(timetableIdx * 24, newBlocks);
		if (timetableIdx <= this.progressTimetableIdx)
		{
			this.progressTimetableIdx++;
		}
	}

	// Token: 0x06004F09 RID: 20233 RVA: 0x001C6EE3 File Offset: 0x001C50E3
	public void AddTimetable(List<ScheduleBlock> newBlocks)
	{
		this.blocks.AddRange(newBlocks);
	}

	// Token: 0x06004F0A RID: 20234 RVA: 0x001C6EF4 File Offset: 0x001C50F4
	public void RemoveTimetable(int TimetableToRemoveIdx)
	{
		int index = TimetableToRemoveIdx * 24;
		int num = this.blocks.Count / 24;
		this.blocks.RemoveRange(index, 24);
		bool flag = TimetableToRemoveIdx == this.progressTimetableIdx;
		bool flag2 = this.progressTimetableIdx == num - 1;
		if (TimetableToRemoveIdx < this.progressTimetableIdx || (flag && flag2))
		{
			this.progressTimetableIdx--;
		}
		ScheduleScreen.Instance.OnChangeCurrentTimetable();
	}

	// Token: 0x06004F0B RID: 20235 RVA: 0x001C6F5F File Offset: 0x001C515F
	public void Assign(Schedulable schedulable)
	{
		if (!this.IsAssigned(schedulable))
		{
			this.GetAssigned().Add(new Ref<Schedulable>(schedulable));
		}
		this.Changed();
	}

	// Token: 0x06004F0C RID: 20236 RVA: 0x001C6F84 File Offset: 0x001C5184
	public void Unassign(Schedulable schedulable)
	{
		for (int i = 0; i < this.GetAssigned().Count; i++)
		{
			if (this.GetAssigned()[i].Get() == schedulable)
			{
				this.GetAssigned().RemoveAt(i);
				break;
			}
		}
		this.Changed();
	}

	// Token: 0x06004F0D RID: 20237 RVA: 0x001C6FD4 File Offset: 0x001C51D4
	public bool IsAssigned(Schedulable schedulable)
	{
		using (List<Ref<Schedulable>>.Enumerator enumerator = this.GetAssigned().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get() == schedulable)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004F0E RID: 20238 RVA: 0x001C7034 File Offset: 0x001C5234
	public static bool AreScheduleTypesIdentical(List<ScheduleBlockType> a, List<ScheduleBlockType> b)
	{
		if (a.Count != b.Count)
		{
			return false;
		}
		foreach (ScheduleBlockType scheduleBlockType in a)
		{
			bool flag = false;
			foreach (ScheduleBlockType scheduleBlockType2 in b)
			{
				if (scheduleBlockType.IdHash == scheduleBlockType2.IdHash)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004F0F RID: 20239 RVA: 0x001C70E8 File Offset: 0x001C52E8
	public bool ShiftTimetable(bool up, int timetableToShiftIdx = 0)
	{
		if (timetableToShiftIdx == 0 && up)
		{
			return false;
		}
		if (timetableToShiftIdx == this.blocks.Count / 24 - 1 && !up)
		{
			return false;
		}
		int num = timetableToShiftIdx * 24;
		List<ScheduleBlock> collection = new List<ScheduleBlock>();
		List<ScheduleBlock> collection2 = new List<ScheduleBlock>();
		if (up)
		{
			collection = this.blocks.GetRange(num, 24);
			collection2 = this.blocks.GetRange(num - 24, 24);
			this.blocks.RemoveRange(num - 24, 48);
			this.blocks.InsertRange(num - 24, collection2);
			this.blocks.InsertRange(num - 24, collection);
		}
		else
		{
			collection = this.blocks.GetRange(num, 24);
			collection2 = this.blocks.GetRange(num + 24, 24);
			this.blocks.RemoveRange(num, 48);
			this.blocks.InsertRange(num, collection);
			this.blocks.InsertRange(num, collection2);
		}
		this.Changed();
		return true;
	}

	// Token: 0x06004F10 RID: 20240 RVA: 0x001C71D0 File Offset: 0x001C53D0
	public void RotateBlocks(bool directionLeft, int timetableToRotateIdx = 0)
	{
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		int index = timetableToRotateIdx * 24;
		list = this.blocks.GetRange(index, 24);
		if (!directionLeft)
		{
			ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.Get(list[list.Count - 1].GroupId);
			for (int i = list.Count - 1; i >= 1; i--)
			{
				ScheduleGroup scheduleGroup2 = Db.Get().ScheduleGroups.Get(list[i - 1].GroupId);
				list[i].GroupId = scheduleGroup2.Id;
			}
			list[0].GroupId = scheduleGroup.Id;
		}
		else
		{
			ScheduleGroup scheduleGroup3 = Db.Get().ScheduleGroups.Get(list[0].GroupId);
			for (int j = 0; j < list.Count - 1; j++)
			{
				ScheduleGroup scheduleGroup4 = Db.Get().ScheduleGroups.Get(list[j + 1].GroupId);
				list[j].GroupId = scheduleGroup4.Id;
			}
			list[list.Count - 1].GroupId = scheduleGroup3.Id;
		}
		this.blocks.RemoveRange(index, 24);
		this.blocks.InsertRange(index, list);
		this.Changed();
	}

	// Token: 0x0400347A RID: 13434
	[Serialize]
	private List<ScheduleBlock> blocks;

	// Token: 0x0400347B RID: 13435
	[Serialize]
	private List<Ref<Schedulable>> assigned;

	// Token: 0x0400347C RID: 13436
	[Serialize]
	public string name;

	// Token: 0x0400347D RID: 13437
	[Serialize]
	public bool alarmActivated = true;

	// Token: 0x0400347E RID: 13438
	[Serialize]
	private int[] tones;

	// Token: 0x0400347F RID: 13439
	[Serialize]
	public bool isDefaultForBionics;

	// Token: 0x04003480 RID: 13440
	[Serialize]
	private int progressTimetableIdx;

	// Token: 0x04003481 RID: 13441
	public Action<Schedule> onChanged;
}

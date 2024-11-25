using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000D33 RID: 3379
public class ScheduleScreen : KScreen
{
	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x06006A46 RID: 27206 RVA: 0x00280937 File Offset: 0x0027EB37
	// (set) Token: 0x06006A47 RID: 27207 RVA: 0x0028093F File Offset: 0x0027EB3F
	public string SelectedPaint { get; set; }

	// Token: 0x06006A48 RID: 27208 RVA: 0x00280948 File Offset: 0x0027EB48
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06006A49 RID: 27209 RVA: 0x0028094F File Offset: 0x0027EB4F
	protected override void OnPrefabInit()
	{
		base.ConsumeMouseScroll = true;
		this.scheduleEntries = new List<ScheduleScreenEntry>();
		ScheduleScreen.Instance = this;
	}

	// Token: 0x06006A4A RID: 27210 RVA: 0x0028096C File Offset: 0x0027EB6C
	protected override void OnSpawn()
	{
		foreach (Schedule schedule in ScheduleManager.Instance.GetSchedules())
		{
			this.AddScheduleEntry(schedule);
		}
		this.addScheduleButton.onClick += this.OnAddScheduleClick;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		ScheduleManager.Instance.onSchedulesChanged += this.OnSchedulesChanged;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshWidgetWorldData));
	}

	// Token: 0x06006A4B RID: 27211 RVA: 0x00280A38 File Offset: 0x0027EC38
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		ScheduleManager.Instance.onSchedulesChanged -= this.OnSchedulesChanged;
		ScheduleScreen.Instance = null;
	}

	// Token: 0x06006A4C RID: 27212 RVA: 0x00280A5C File Offset: 0x0027EC5C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			base.Activate();
		}
	}

	// Token: 0x06006A4D RID: 27213 RVA: 0x00280A70 File Offset: 0x0027EC70
	public void RefreshAllPaintButtons()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshPaintButtons();
		}
	}

	// Token: 0x06006A4E RID: 27214 RVA: 0x00280AC0 File Offset: 0x0027ECC0
	private void OnAddScheduleClick()
	{
		ScheduleManager.Instance.AddDefaultSchedule(false);
	}

	// Token: 0x06006A4F RID: 27215 RVA: 0x00280AD0 File Offset: 0x0027ECD0
	private void AddScheduleEntry(Schedule schedule)
	{
		ScheduleScreenEntry scheduleScreenEntry = Util.KInstantiateUI<ScheduleScreenEntry>(this.scheduleEntryPrefab.gameObject, this.scheduleEntryContainer, true);
		scheduleScreenEntry.Setup(schedule);
		this.scheduleEntries.Add(scheduleScreenEntry);
	}

	// Token: 0x06006A50 RID: 27216 RVA: 0x00280B08 File Offset: 0x0027ED08
	private void OnSchedulesChanged(List<Schedule> schedules)
	{
		foreach (ScheduleScreenEntry original in this.scheduleEntries)
		{
			Util.KDestroyGameObject(original);
		}
		this.scheduleEntries.Clear();
		foreach (Schedule schedule in schedules)
		{
			this.AddScheduleEntry(schedule);
		}
	}

	// Token: 0x06006A51 RID: 27217 RVA: 0x00280BA0 File Offset: 0x0027EDA0
	private void RefreshWidgetWorldData(object data = null)
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshWidgetWorldData();
		}
	}

	// Token: 0x06006A52 RID: 27218 RVA: 0x00280BF0 File Offset: 0x0027EDF0
	public void OnChangeCurrentTimetable()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshTimeOfDayPositioner();
		}
	}

	// Token: 0x06006A53 RID: 27219 RVA: 0x00280C40 File Offset: 0x0027EE40
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.CheckBlockedInput())
		{
			if (!e.Consumed)
			{
				e.Consumed = true;
				return;
			}
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06006A54 RID: 27220 RVA: 0x00280C64 File Offset: 0x0027EE64
	private bool CheckBlockedInput()
	{
		bool result = false;
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
				{
					if (currentSelectedGameObject == scheduleScreenEntry.GetNameInputField())
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04004868 RID: 18536
	public static ScheduleScreen Instance;

	// Token: 0x0400486A RID: 18538
	[SerializeField]
	private ScheduleScreenEntry scheduleEntryPrefab;

	// Token: 0x0400486B RID: 18539
	[SerializeField]
	private GameObject scheduleEntryContainer;

	// Token: 0x0400486C RID: 18540
	[SerializeField]
	private KButton addScheduleButton;

	// Token: 0x0400486D RID: 18541
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400486E RID: 18542
	private List<ScheduleScreenEntry> scheduleEntries;
}

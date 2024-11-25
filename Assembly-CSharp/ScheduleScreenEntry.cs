using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D36 RID: 3382
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleScreenEntry")]
public class ScheduleScreenEntry : KMonoBehaviour
{
	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x06006A5B RID: 27227 RVA: 0x00280D2D File Offset: 0x0027EF2D
	// (set) Token: 0x06006A5C RID: 27228 RVA: 0x00280D35 File Offset: 0x0027EF35
	public Schedule schedule { get; private set; }

	// Token: 0x06006A5D RID: 27229 RVA: 0x00280D40 File Offset: 0x0027EF40
	public void Setup(Schedule schedule)
	{
		this.schedule = schedule;
		base.gameObject.name = "Schedule_" + schedule.name;
		this.title.SetTitle(schedule.name);
		this.title.OnNameChanged += this.OnNameChanged;
		this.duplicateScheduleButton.onClick += this.DuplicateSchedule;
		this.deleteScheduleButton.onClick += this.DeleteSchedule;
		this.timetableRows = new List<GameObject>();
		this.blockButtonsByTimetableRow = new Dictionary<GameObject, List<ScheduleBlockButton>>();
		int num = Mathf.CeilToInt((float)(schedule.GetBlocks().Count / 24));
		for (int i = 0; i < num; i++)
		{
			this.AddTimetableRow(i * 24);
		}
		this.minionWidgets = new List<ScheduleMinionWidget>();
		this.blankMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, false);
		this.blankMinionWidget.SetupBlank(schedule);
		this.RebuildMinionWidgets();
		this.RefreshStatus();
		this.RefreshAlarmButton();
		MultiToggle multiToggle = this.alarmButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnAlarmClicked));
		schedule.onChanged = (Action<Schedule>)Delegate.Combine(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		this.ConfigPaintButton(this.PaintButtonBathtime, Db.Get().ScheduleGroups.Hygene, Def.GetUISprite(Assets.GetPrefab(ShowerConfig.ID), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonWorktime, Db.Get().ScheduleGroups.Worktime, Def.GetUISprite(Assets.GetPrefab("ManualGenerator"), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonRecreation, Db.Get().ScheduleGroups.Recreation, Def.GetUISprite(Assets.GetPrefab("WaterCooler"), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonSleep, Db.Get().ScheduleGroups.Sleep, Def.GetUISprite(Assets.GetPrefab("Bed"), "ui", false).first);
		this.RefreshPaintButtons();
		this.RefreshTimeOfDayPositioner();
	}

	// Token: 0x06006A5E RID: 27230 RVA: 0x00280F89 File Offset: 0x0027F189
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.schedule != null)
		{
			Schedule schedule = this.schedule;
			schedule.onChanged = (Action<Schedule>)Delegate.Remove(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		}
	}

	// Token: 0x06006A5F RID: 27231 RVA: 0x00280FC0 File Offset: 0x0027F1C0
	private void DuplicateSchedule()
	{
		ScheduleManager.Instance.DuplicateSchedule(this.schedule);
	}

	// Token: 0x06006A60 RID: 27232 RVA: 0x00280FD3 File Offset: 0x0027F1D3
	private void DeleteSchedule()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x06006A61 RID: 27233 RVA: 0x00280FE8 File Offset: 0x0027F1E8
	public void RefreshTimeOfDayPositioner()
	{
		GameObject targetTimetable = this.timetableRows[this.schedule.ProgressTimetableIdx];
		this.timeOfDayPositioner.SetTargetTimetable(targetTimetable);
	}

	// Token: 0x06006A62 RID: 27234 RVA: 0x00281018 File Offset: 0x0027F218
	private void DuplicateTimetableRow(int sourceTimetableIdx)
	{
		List<ScheduleBlock> range = this.schedule.GetBlocks().GetRange(sourceTimetableIdx * 24, 24);
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		for (int i = 0; i < range.Count; i++)
		{
			list.Add(new ScheduleBlock(range[i].name, range[i].GroupId));
		}
		int num = sourceTimetableIdx + 1;
		this.schedule.InsertTimetable(num, list);
		this.AddTimetableRow(num * 24);
	}

	// Token: 0x06006A63 RID: 27235 RVA: 0x00281094 File Offset: 0x0027F294
	private void AddTimetableRow(int startingBlockIdx)
	{
		GameObject row = Util.KInstantiateUI(this.timetableRowPrefab, this.timetableRowContainer, true);
		int num = startingBlockIdx / 24;
		this.timetableRows.Insert(num, row);
		row.transform.SetSiblingIndex(num);
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		List<ScheduleBlockButton> list = new List<ScheduleBlockButton>();
		for (int i = startingBlockIdx; i < startingBlockIdx + 24; i++)
		{
			GameObject gameObject = component.GetReference<RectTransform>("BlockContainer").gameObject;
			ScheduleBlockButton scheduleBlockButton = Util.KInstantiateUI<ScheduleBlockButton>(this.blockButtonPrefab.gameObject, gameObject, true);
			scheduleBlockButton.Setup(i - startingBlockIdx);
			scheduleBlockButton.SetBlockTypes(this.schedule.GetBlock(i).allowed_types);
			list.Add(scheduleBlockButton);
		}
		this.blockButtonsByTimetableRow.Add(row, list);
		component.GetReference<ScheduleBlockPainter>("BlockPainter").SetEntry(this);
		component.GetReference<KButton>("DuplicateButton").onClick += delegate()
		{
			this.DuplicateTimetableRow(this.timetableRows.IndexOf(row));
		};
		component.GetReference<KButton>("DeleteButton").onClick += delegate()
		{
			this.RemoveTimetableRow(row);
		};
		component.GetReference<KButton>("RotateLeftButton").onClick += delegate()
		{
			this.schedule.RotateBlocks(true, this.timetableRows.IndexOf(row));
		};
		component.GetReference<KButton>("RotateRightButton").onClick += delegate()
		{
			this.schedule.RotateBlocks(false, this.timetableRows.IndexOf(row));
		};
		KButton rotateUpButton = component.GetReference<KButton>("ShiftUpButton");
		rotateUpButton.onClick += delegate()
		{
			int timetableToShiftIdx = this.timetableRows.IndexOf(row);
			this.schedule.ShiftTimetable(true, timetableToShiftIdx);
			if (rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_up")
			{
				rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up_reset";
				return;
			}
			rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up";
		};
		KButton rotateDownButton = component.GetReference<KButton>("ShiftDownButton");
		rotateDownButton.onClick += delegate()
		{
			int timetableToShiftIdx = this.timetableRows.IndexOf(row);
			this.schedule.ShiftTimetable(false, timetableToShiftIdx);
			if (rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_down")
			{
				rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down_reset";
				return;
			}
			rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down";
		};
	}

	// Token: 0x06006A64 RID: 27236 RVA: 0x0028124C File Offset: 0x0027F44C
	private void RemoveTimetableRow(GameObject row)
	{
		if (this.timetableRows.Count == 1)
		{
			return;
		}
		this.timeOfDayPositioner.SetTargetTimetable(null);
		int timetableToRemoveIdx = this.timetableRows.IndexOf(row);
		this.timetableRows.Remove(row);
		this.blockButtonsByTimetableRow.Remove(row);
		UnityEngine.Object.Destroy(row);
		this.schedule.RemoveTimetable(timetableToRemoveIdx);
	}

	// Token: 0x06006A65 RID: 27237 RVA: 0x002812AD File Offset: 0x0027F4AD
	public GameObject GetNameInputField()
	{
		return this.title.inputField.gameObject;
	}

	// Token: 0x06006A66 RID: 27238 RVA: 0x002812C0 File Offset: 0x0027F4C0
	private void RebuildMinionWidgets()
	{
		if (!this.MinionWidgetsNeedRebuild())
		{
			return;
		}
		foreach (ScheduleMinionWidget original in this.minionWidgets)
		{
			Util.KDestroyGameObject(original);
		}
		this.minionWidgets.Clear();
		foreach (Ref<Schedulable> @ref in this.schedule.GetAssigned())
		{
			ScheduleMinionWidget scheduleMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, true);
			scheduleMinionWidget.Setup(@ref.Get());
			this.minionWidgets.Add(scheduleMinionWidget);
		}
		if (Components.LiveMinionIdentities.Count > this.schedule.GetAssigned().Count)
		{
			this.blankMinionWidget.transform.SetAsLastSibling();
			this.blankMinionWidget.gameObject.SetActive(true);
			return;
		}
		this.blankMinionWidget.gameObject.SetActive(false);
	}

	// Token: 0x06006A67 RID: 27239 RVA: 0x002813E4 File Offset: 0x0027F5E4
	private bool MinionWidgetsNeedRebuild()
	{
		List<Ref<Schedulable>> assigned = this.schedule.GetAssigned();
		if (assigned.Count != this.minionWidgets.Count)
		{
			return true;
		}
		if (assigned.Count != Components.LiveMinionIdentities.Count != this.blankMinionWidget.gameObject.activeSelf)
		{
			return true;
		}
		for (int i = 0; i < assigned.Count; i++)
		{
			if (assigned[i].Get() != this.minionWidgets[i].schedulable)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006A68 RID: 27240 RVA: 0x00281474 File Offset: 0x0027F674
	public void RefreshWidgetWorldData()
	{
		foreach (ScheduleMinionWidget scheduleMinionWidget in this.minionWidgets)
		{
			if (!scheduleMinionWidget.IsNullOrDestroyed())
			{
				scheduleMinionWidget.RefreshWidgetWorldData();
			}
		}
	}

	// Token: 0x06006A69 RID: 27241 RVA: 0x002814D0 File Offset: 0x0027F6D0
	private void OnNameChanged(string newName)
	{
		this.schedule.name = newName;
		base.gameObject.name = "Schedule_" + this.schedule.name;
	}

	// Token: 0x06006A6A RID: 27242 RVA: 0x002814FE File Offset: 0x0027F6FE
	private void OnAlarmClicked()
	{
		this.schedule.alarmActivated = !this.schedule.alarmActivated;
		this.RefreshAlarmButton();
	}

	// Token: 0x06006A6B RID: 27243 RVA: 0x00281520 File Offset: 0x0027F720
	private void RefreshAlarmButton()
	{
		this.alarmButton.ChangeState(this.schedule.alarmActivated ? 1 : 0);
		ToolTip component = this.alarmButton.GetComponent<ToolTip>();
		component.SetSimpleTooltip(this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_BUTTON_ON_TOOLTIP : UI.SCHEDULESCREEN.ALARM_BUTTON_OFF_TOOLTIP);
		ToolTipScreen.Instance.MarkTooltipDirty(component);
		this.alarmField.text = (this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_TITLE_ENABLED : UI.SCHEDULESCREEN.ALARM_TITLE_DISABLED);
	}

	// Token: 0x06006A6C RID: 27244 RVA: 0x002815AD File Offset: 0x0027F7AD
	private void OnResetClicked()
	{
		this.schedule.SetBlocksToGroupDefaults(Db.Get().ScheduleGroups.allGroups);
	}

	// Token: 0x06006A6D RID: 27245 RVA: 0x002815C9 File Offset: 0x0027F7C9
	private void OnDeleteClicked()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x06006A6E RID: 27246 RVA: 0x002815DC File Offset: 0x0027F7DC
	private void OnScheduleChanged(Schedule changedSchedule)
	{
		foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
		{
			GameObject key = keyValuePair.Key;
			int num = this.timetableRows.IndexOf(key);
			List<ScheduleBlockButton> value = keyValuePair.Value;
			for (int i = 0; i < value.Count; i++)
			{
				int idx = num * 24 + i;
				value[i].SetBlockTypes(changedSchedule.GetBlock(idx).allowed_types);
			}
		}
		this.RefreshStatus();
		this.RebuildMinionWidgets();
	}

	// Token: 0x06006A6F RID: 27247 RVA: 0x0028168C File Offset: 0x0027F88C
	private void RefreshStatus()
	{
		this.blockTypeCounts.Clear();
		foreach (ScheduleBlockType scheduleBlockType in Db.Get().ScheduleBlockTypes.resources)
		{
			this.blockTypeCounts[scheduleBlockType.Id] = 0;
		}
		foreach (ScheduleBlock scheduleBlock in this.schedule.GetBlocks())
		{
			foreach (ScheduleBlockType scheduleBlockType2 in scheduleBlock.allowed_types)
			{
				Dictionary<string, int> dictionary = this.blockTypeCounts;
				string id = scheduleBlockType2.Id;
				int num = dictionary[id];
				dictionary[id] = num + 1;
			}
		}
		if (this.noteEntryRight == null)
		{
			return;
		}
		int num2 = 0;
		ToolTip component = this.noteEntryRight.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		foreach (KeyValuePair<string, int> keyValuePair in this.blockTypeCounts)
		{
			if (keyValuePair.Value == 0)
			{
				num2++;
				component.AddMultiStringTooltip(string.Format(UI.SCHEDULEGROUPS.NOTIME, Db.Get().ScheduleBlockTypes.Get(keyValuePair.Key).Name), null);
			}
		}
		if (num2 > 0)
		{
			this.noteEntryRight.text = string.Format(UI.SCHEDULEGROUPS.MISSINGBLOCKS, num2);
			return;
		}
		this.noteEntryRight.text = "";
	}

	// Token: 0x06006A70 RID: 27248 RVA: 0x00281870 File Offset: 0x0027FA70
	private void ConfigPaintButton(GameObject button, ScheduleGroup group, Sprite iconSprite)
	{
		string groupID = group.Id;
		button.GetComponent<MultiToggle>().onClick = delegate()
		{
			ScheduleScreen.Instance.SelectedPaint = groupID;
			ScheduleScreen.Instance.RefreshAllPaintButtons();
		};
		this.paintButtons.Add(group.Id, button);
		HierarchyReferences component = button.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = iconSprite;
		component.GetReference<LocText>("Label").text = group.Name;
	}

	// Token: 0x06006A71 RID: 27249 RVA: 0x002818E4 File Offset: 0x0027FAE4
	public void RefreshPaintButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.paintButtons)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == ScheduleScreen.Instance.SelectedPaint) ? 1 : 0);
		}
	}

	// Token: 0x06006A72 RID: 27250 RVA: 0x00281960 File Offset: 0x0027FB60
	public bool PaintBlock(ScheduleBlockButton blockButton)
	{
		foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
		{
			GameObject key = keyValuePair.Key;
			int i = 0;
			while (i < keyValuePair.Value.Count)
			{
				if (keyValuePair.Value[i] == blockButton)
				{
					int idx = this.timetableRows.IndexOf(key) * 24 + i;
					ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.Get(ScheduleScreen.Instance.SelectedPaint);
					if (this.schedule.GetBlock(idx).GroupId != scheduleGroup.Id)
					{
						this.schedule.SetBlockGroup(idx, scheduleGroup);
						return true;
					}
					return false;
				}
				else
				{
					i++;
				}
			}
		}
		return false;
	}

	// Token: 0x04004872 RID: 18546
	[SerializeField]
	private ScheduleBlockButton blockButtonPrefab;

	// Token: 0x04004873 RID: 18547
	[SerializeField]
	private ScheduleMinionWidget minionWidgetPrefab;

	// Token: 0x04004874 RID: 18548
	[SerializeField]
	private GameObject minionWidgetContainer;

	// Token: 0x04004875 RID: 18549
	private ScheduleMinionWidget blankMinionWidget;

	// Token: 0x04004876 RID: 18550
	[SerializeField]
	private KButton duplicateScheduleButton;

	// Token: 0x04004877 RID: 18551
	[SerializeField]
	private KButton deleteScheduleButton;

	// Token: 0x04004878 RID: 18552
	[SerializeField]
	private EditableTitleBar title;

	// Token: 0x04004879 RID: 18553
	[SerializeField]
	private LocText alarmField;

	// Token: 0x0400487A RID: 18554
	[SerializeField]
	private KButton optionsButton;

	// Token: 0x0400487B RID: 18555
	[SerializeField]
	private LocText noteEntryLeft;

	// Token: 0x0400487C RID: 18556
	[SerializeField]
	private LocText noteEntryRight;

	// Token: 0x0400487D RID: 18557
	[SerializeField]
	private MultiToggle alarmButton;

	// Token: 0x0400487E RID: 18558
	private List<GameObject> timetableRows;

	// Token: 0x0400487F RID: 18559
	private Dictionary<GameObject, List<ScheduleBlockButton>> blockButtonsByTimetableRow;

	// Token: 0x04004880 RID: 18560
	private List<ScheduleMinionWidget> minionWidgets;

	// Token: 0x04004881 RID: 18561
	[SerializeField]
	private GameObject timetableRowPrefab;

	// Token: 0x04004882 RID: 18562
	[SerializeField]
	private GameObject timetableRowContainer;

	// Token: 0x04004883 RID: 18563
	private Dictionary<string, GameObject> paintButtons = new Dictionary<string, GameObject>();

	// Token: 0x04004884 RID: 18564
	[SerializeField]
	private GameObject PaintButtonBathtime;

	// Token: 0x04004885 RID: 18565
	[SerializeField]
	private GameObject PaintButtonWorktime;

	// Token: 0x04004886 RID: 18566
	[SerializeField]
	private GameObject PaintButtonRecreation;

	// Token: 0x04004887 RID: 18567
	[SerializeField]
	private GameObject PaintButtonSleep;

	// Token: 0x04004888 RID: 18568
	[SerializeField]
	private TimeOfDayPositioner timeOfDayPositioner;

	// Token: 0x0400488A RID: 18570
	private Dictionary<string, int> blockTypeCounts = new Dictionary<string, int>();
}

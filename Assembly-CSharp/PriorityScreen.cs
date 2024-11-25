using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000D15 RID: 3349
public class PriorityScreen : KScreen
{
	// Token: 0x0600687C RID: 26748 RVA: 0x00271D98 File Offset: 0x0026FF98
	public void InstantiateButtons(Action<PrioritySetting> on_click, bool playSelectionSound = true)
	{
		this.onClick = on_click;
		for (int i = 1; i <= 9; i++)
		{
			int num = i;
			PriorityButton priorityButton = global::Util.KInstantiateUI<PriorityButton>(this.buttonPrefab_basic.gameObject, this.buttonPrefab_basic.transform.parent.gameObject, false);
			this.buttons_basic.Add(priorityButton);
			priorityButton.playSelectionSound = playSelectionSound;
			priorityButton.onClick = this.onClick;
			priorityButton.text.text = num.ToString();
			priorityButton.priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, num);
			priorityButton.tooltip.SetSimpleTooltip(string.Format(UI.PRIORITYSCREEN.BASIC, num));
		}
		this.buttonPrefab_basic.gameObject.SetActive(false);
		this.button_emergency.playSelectionSound = playSelectionSound;
		this.button_emergency.onClick = this.onClick;
		this.button_emergency.priority = new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1);
		this.button_emergency.tooltip.SetSimpleTooltip(UI.PRIORITYSCREEN.TOP_PRIORITY);
		this.button_toggleHigh.gameObject.SetActive(false);
		this.PriorityMenuContainer.SetActive(true);
		this.button_priorityMenu.gameObject.SetActive(true);
		this.button_priorityMenu.onClick += this.PriorityButtonClicked;
		this.button_priorityMenu.GetComponent<ToolTip>().SetSimpleTooltip(UI.PRIORITYSCREEN.OPEN_JOBS_SCREEN);
		this.diagram.SetActive(false);
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x0600687D RID: 26749 RVA: 0x00271F19 File Offset: 0x00270119
	private void OnClick(PrioritySetting priority)
	{
		if (this.onClick != null)
		{
			this.onClick(priority);
		}
	}

	// Token: 0x0600687E RID: 26750 RVA: 0x00271F2F File Offset: 0x0027012F
	public void ShowDiagram(bool show)
	{
		this.diagram.SetActive(show);
	}

	// Token: 0x0600687F RID: 26751 RVA: 0x00271F3D File Offset: 0x0027013D
	public void ResetPriority()
	{
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x06006880 RID: 26752 RVA: 0x00271F4D File Offset: 0x0027014D
	public void PriorityButtonClicked()
	{
		ManagementMenu.Instance.TogglePriorities();
	}

	// Token: 0x06006881 RID: 26753 RVA: 0x00271F5C File Offset: 0x0027015C
	private void RefreshButton(PriorityButton b, PrioritySetting priority, bool play_sound)
	{
		if (b.priority == priority)
		{
			b.toggle.Select();
			b.toggle.isOn = true;
			if (play_sound)
			{
				b.toggle.soundPlayer.Play(0);
				return;
			}
		}
		else
		{
			b.toggle.isOn = false;
		}
	}

	// Token: 0x06006882 RID: 26754 RVA: 0x00271FB0 File Offset: 0x002701B0
	public void SetScreenPriority(PrioritySetting priority, bool play_sound = false)
	{
		if (this.lastSelectedPriority == priority)
		{
			return;
		}
		this.lastSelectedPriority = priority;
		if (priority.priority_class == PriorityScreen.PriorityClass.high)
		{
			this.button_toggleHigh.isOn = true;
		}
		else if (priority.priority_class == PriorityScreen.PriorityClass.basic)
		{
			this.button_toggleHigh.isOn = false;
		}
		for (int i = 0; i < this.buttons_basic.Count; i++)
		{
			this.buttons_basic[i].priority = new PrioritySetting(this.button_toggleHigh.isOn ? PriorityScreen.PriorityClass.high : PriorityScreen.PriorityClass.basic, i + 1);
			this.buttons_basic[i].tooltip.SetSimpleTooltip(string.Format(this.button_toggleHigh.isOn ? UI.PRIORITYSCREEN.HIGH : UI.PRIORITYSCREEN.BASIC, i + 1));
			this.RefreshButton(this.buttons_basic[i], this.lastSelectedPriority, play_sound);
		}
		this.RefreshButton(this.button_emergency, this.lastSelectedPriority, play_sound);
	}

	// Token: 0x06006883 RID: 26755 RVA: 0x002720B1 File Offset: 0x002702B1
	public PrioritySetting GetLastSelectedPriority()
	{
		return this.lastSelectedPriority;
	}

	// Token: 0x06006884 RID: 26756 RVA: 0x002720BC File Offset: 0x002702BC
	public static void PlayPriorityConfirmSound(PrioritySetting priority)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Priority_Tool_Confirm", false), Vector3.zero, 1f);
		if (instance.isValid())
		{
			float num = 0f;
			if (priority.priority_class >= PriorityScreen.PriorityClass.high)
			{
				num += 10f;
			}
			if (priority.priority_class >= PriorityScreen.PriorityClass.topPriority)
			{
				num += 0f;
			}
			num += (float)priority.priority_value;
			instance.setParameterByName("priority", num, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x040046AD RID: 18093
	[SerializeField]
	protected PriorityButton buttonPrefab_basic;

	// Token: 0x040046AE RID: 18094
	[SerializeField]
	protected GameObject EmergencyContainer;

	// Token: 0x040046AF RID: 18095
	[SerializeField]
	protected PriorityButton button_emergency;

	// Token: 0x040046B0 RID: 18096
	[SerializeField]
	protected GameObject PriorityMenuContainer;

	// Token: 0x040046B1 RID: 18097
	[SerializeField]
	protected KButton button_priorityMenu;

	// Token: 0x040046B2 RID: 18098
	[SerializeField]
	protected KToggle button_toggleHigh;

	// Token: 0x040046B3 RID: 18099
	[SerializeField]
	protected GameObject diagram;

	// Token: 0x040046B4 RID: 18100
	protected List<PriorityButton> buttons_basic = new List<PriorityButton>();

	// Token: 0x040046B5 RID: 18101
	protected List<PriorityButton> buttons_emergency = new List<PriorityButton>();

	// Token: 0x040046B6 RID: 18102
	private PrioritySetting priority;

	// Token: 0x040046B7 RID: 18103
	private PrioritySetting lastSelectedPriority = new PrioritySetting(PriorityScreen.PriorityClass.basic, -1);

	// Token: 0x040046B8 RID: 18104
	private Action<PrioritySetting> onClick;

	// Token: 0x02001E39 RID: 7737
	public enum PriorityClass
	{
		// Token: 0x040089D1 RID: 35281
		idle = -1,
		// Token: 0x040089D2 RID: 35282
		basic,
		// Token: 0x040089D3 RID: 35283
		high,
		// Token: 0x040089D4 RID: 35284
		personalNeeds,
		// Token: 0x040089D5 RID: 35285
		topPriority,
		// Token: 0x040089D6 RID: 35286
		compulsory
	}
}

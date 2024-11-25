using System;
using System.Collections.Generic;
using Database;
using KSerialization;

// Token: 0x02000B1F RID: 2847
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryInstance : ISaveLoadable
{
	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x060054AB RID: 21675 RVA: 0x001E463E File Offset: 0x001E283E
	// (set) Token: 0x060054AC RID: 21676 RVA: 0x001E4648 File Offset: 0x001E2848
	public StoryInstance.State CurrentState
	{
		get
		{
			return this.state;
		}
		set
		{
			if (this.state == value)
			{
				return;
			}
			this.state = value;
			this.Telemetry.LogStateChange(this.state, GameClock.Instance.GetTimeInCycles());
			Action<StoryInstance.State> storyStateChanged = this.StoryStateChanged;
			if (storyStateChanged == null)
			{
				return;
			}
			storyStateChanged(this.state);
		}
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x060054AD RID: 21677 RVA: 0x001E4697 File Offset: 0x001E2897
	public StoryManager.StoryTelemetry Telemetry
	{
		get
		{
			if (this.telemetry == null)
			{
				this.telemetry = new StoryManager.StoryTelemetry();
			}
			return this.telemetry;
		}
	}

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x060054AE RID: 21678 RVA: 0x001E46B2 File Offset: 0x001E28B2
	// (set) Token: 0x060054AF RID: 21679 RVA: 0x001E46BA File Offset: 0x001E28BA
	public EventInfoData EventInfo { get; private set; }

	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x060054B0 RID: 21680 RVA: 0x001E46C3 File Offset: 0x001E28C3
	// (set) Token: 0x060054B1 RID: 21681 RVA: 0x001E46CB File Offset: 0x001E28CB
	public Notification Notification { get; private set; }

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x060054B2 RID: 21682 RVA: 0x001E46D4 File Offset: 0x001E28D4
	// (set) Token: 0x060054B3 RID: 21683 RVA: 0x001E46DC File Offset: 0x001E28DC
	public EventInfoDataHelper.PopupType PendingType { get; private set; } = EventInfoDataHelper.PopupType.NONE;

	// Token: 0x060054B4 RID: 21684 RVA: 0x001E46E5 File Offset: 0x001E28E5
	public Story GetStory()
	{
		if (this._story == null)
		{
			this._story = Db.Get().Stories.Get(this.storyId);
		}
		return this._story;
	}

	// Token: 0x060054B5 RID: 21685 RVA: 0x001E4710 File Offset: 0x001E2910
	public StoryInstance()
	{
	}

	// Token: 0x060054B6 RID: 21686 RVA: 0x001E472A File Offset: 0x001E292A
	public StoryInstance(Story story, int worldId)
	{
		this._story = story;
		this.storyId = story.Id;
		this.worldId = worldId;
	}

	// Token: 0x060054B7 RID: 21687 RVA: 0x001E475E File Offset: 0x001E295E
	public bool HasDisplayedPopup(EventInfoDataHelper.PopupType type)
	{
		return this.popupDisplayedStates != null && this.popupDisplayedStates.Contains(type);
	}

	// Token: 0x060054B8 RID: 21688 RVA: 0x001E4778 File Offset: 0x001E2978
	public void SetPopupData(StoryManager.PopupInfo info, EventInfoData eventInfo, Notification notification = null)
	{
		this.EventInfo = eventInfo;
		this.Notification = notification;
		this.PendingType = info.PopupType;
		eventInfo.showCallback = (System.Action)Delegate.Combine(eventInfo.showCallback, new System.Action(this.OnPopupDisplayed));
		if (info.DisplayImmediate)
		{
			EventInfoScreen.ShowPopup(eventInfo);
		}
	}

	// Token: 0x060054B9 RID: 21689 RVA: 0x001E47D0 File Offset: 0x001E29D0
	private void OnPopupDisplayed()
	{
		if (this.popupDisplayedStates == null)
		{
			this.popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();
		}
		this.popupDisplayedStates.Add(this.PendingType);
		this.EventInfo = null;
		this.Notification = null;
		this.PendingType = EventInfoDataHelper.PopupType.NONE;
	}

	// Token: 0x04003786 RID: 14214
	public Action<StoryInstance.State> StoryStateChanged;

	// Token: 0x04003787 RID: 14215
	[Serialize]
	public readonly string storyId;

	// Token: 0x04003788 RID: 14216
	[Serialize]
	public int worldId;

	// Token: 0x04003789 RID: 14217
	[Serialize]
	private StoryInstance.State state;

	// Token: 0x0400378A RID: 14218
	[Serialize]
	private StoryManager.StoryTelemetry telemetry;

	// Token: 0x0400378B RID: 14219
	[Serialize]
	private HashSet<EventInfoDataHelper.PopupType> popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();

	// Token: 0x0400378F RID: 14223
	private Story _story;

	// Token: 0x02001B6B RID: 7019
	public enum State
	{
		// Token: 0x04007FAB RID: 32683
		RETROFITTED = -1,
		// Token: 0x04007FAC RID: 32684
		NOT_STARTED,
		// Token: 0x04007FAD RID: 32685
		DISCOVERED,
		// Token: 0x04007FAE RID: 32686
		IN_PROGRESS,
		// Token: 0x04007FAF RID: 32687
		COMPLETE
	}
}

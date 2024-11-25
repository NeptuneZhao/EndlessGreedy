using System;
using UnityEngine;

// Token: 0x02000C42 RID: 3138
public class EventInfoDataHelper
{
	// Token: 0x06006070 RID: 24688 RVA: 0x0023DCA8 File Offset: 0x0023BEA8
	public static EventInfoData GenerateStoryTraitData(string titleText, string descriptionText, string buttonText, string animFileName, EventInfoDataHelper.PopupType popupType, string buttonTooltip = null, GameObject[] minions = null, System.Action callback = null)
	{
		EventInfoData eventInfoData = new EventInfoData(titleText, descriptionText, animFileName);
		eventInfoData.minions = minions;
		if (popupType <= EventInfoDataHelper.PopupType.NORMAL || popupType != EventInfoDataHelper.PopupType.COMPLETE)
		{
			eventInfoData.showCallback = delegate()
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup", false));
			};
		}
		else
		{
			eventInfoData.showCallback = delegate()
			{
				MusicManager.instance.PlaySong("Stinger_StoryTraitUnlock", false);
			};
		}
		EventInfoData.Option option = eventInfoData.AddOption(buttonText, null);
		option.callback = callback;
		option.tooltip = buttonTooltip;
		return eventInfoData;
	}

	// Token: 0x02001D25 RID: 7461
	public enum PopupType
	{
		// Token: 0x04008632 RID: 34354
		NONE = -1,
		// Token: 0x04008633 RID: 34355
		BEGIN,
		// Token: 0x04008634 RID: 34356
		NORMAL,
		// Token: 0x04008635 RID: 34357
		COMPLETE
	}
}

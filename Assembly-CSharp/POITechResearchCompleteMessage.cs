using System;
using KSerialization;
using STRINGS;

// Token: 0x02000CCF RID: 3279
public class POITechResearchCompleteMessage : Message
{
	// Token: 0x0600656C RID: 25964 RVA: 0x0025E130 File Offset: 0x0025C330
	public POITechResearchCompleteMessage()
	{
	}

	// Token: 0x0600656D RID: 25965 RVA: 0x0025E138 File Offset: 0x0025C338
	public POITechResearchCompleteMessage(POITechItemUnlocks.Def unlocked_items)
	{
		this.unlockedItemsdef = unlocked_items;
		this.popupName = unlocked_items.PopUpName;
		this.animName = unlocked_items.animName;
	}

	// Token: 0x0600656E RID: 25966 RVA: 0x0025E164 File Offset: 0x0025C364
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600656F RID: 25967 RVA: 0x0025E16C File Offset: 0x0025C36C
	public override string GetMessageBody()
	{
		string text = "";
		for (int i = 0; i < this.unlockedItemsdef.POITechUnlockIDs.Count; i++)
		{
			TechItem techItem = Db.Get().TechItems.TryGet(this.unlockedItemsdef.POITechUnlockIDs[i]);
			if (techItem != null)
			{
				text = text + "\n    • " + techItem.Name;
			}
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, text);
	}

	// Token: 0x06006570 RID: 25968 RVA: 0x0025E1E0 File Offset: 0x0025C3E0
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME;
	}

	// Token: 0x06006571 RID: 25969 RVA: 0x0025E1EC File Offset: 0x0025C3EC
	public override string GetTooltip()
	{
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.TOOLTIP, this.popupName);
	}

	// Token: 0x06006572 RID: 25970 RVA: 0x0025E203 File Offset: 0x0025C403
	public override bool IsValid()
	{
		return this.unlockedItemsdef != null;
	}

	// Token: 0x06006573 RID: 25971 RVA: 0x0025E20E File Offset: 0x0025C40E
	public override bool ShowDialog()
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, this.GetMessageBody(), this.animName);
		eventInfoData.AddDefaultOption(null);
		EventInfoScreen.ShowPopup(eventInfoData);
		Messenger.Instance.RemoveMessage(this);
		return false;
	}

	// Token: 0x06006574 RID: 25972 RVA: 0x0025E24A File Offset: 0x0025C44A
	public override bool ShowDismissButton()
	{
		return false;
	}

	// Token: 0x06006575 RID: 25973 RVA: 0x0025E24D File Offset: 0x0025C44D
	public override NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}

	// Token: 0x04004491 RID: 17553
	[Serialize]
	public POITechItemUnlocks.Def unlockedItemsdef;

	// Token: 0x04004492 RID: 17554
	[Serialize]
	public string popupName;

	// Token: 0x04004493 RID: 17555
	[Serialize]
	public string animName;
}

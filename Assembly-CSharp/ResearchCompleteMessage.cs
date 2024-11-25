using System;
using KSerialization;
using STRINGS;

// Token: 0x02000CD0 RID: 3280
public class ResearchCompleteMessage : Message
{
	// Token: 0x06006576 RID: 25974 RVA: 0x0025E250 File Offset: 0x0025C450
	public ResearchCompleteMessage()
	{
	}

	// Token: 0x06006577 RID: 25975 RVA: 0x0025E263 File Offset: 0x0025C463
	public ResearchCompleteMessage(Tech tech)
	{
		this.tech.Set(tech);
	}

	// Token: 0x06006578 RID: 25976 RVA: 0x0025E282 File Offset: 0x0025C482
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06006579 RID: 25977 RVA: 0x0025E28C File Offset: 0x0025C48C
	public override string GetMessageBody()
	{
		Tech tech = this.tech.Get();
		string text = "";
		for (int i = 0; i < tech.unlockedItems.Count; i++)
		{
			if (i != 0)
			{
				text += ", ";
			}
			text += tech.unlockedItems[i].Name;
		}
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.MESSAGEBODY, tech.Name, text);
	}

	// Token: 0x0600657A RID: 25978 RVA: 0x0025E2FE File Offset: 0x0025C4FE
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.RESEARCHCOMPLETE.NAME;
	}

	// Token: 0x0600657B RID: 25979 RVA: 0x0025E30C File Offset: 0x0025C50C
	public override string GetTooltip()
	{
		Tech tech = this.tech.Get();
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.TOOLTIP, tech.Name);
	}

	// Token: 0x0600657C RID: 25980 RVA: 0x0025E33A File Offset: 0x0025C53A
	public override bool IsValid()
	{
		return this.tech.Get() != null;
	}

	// Token: 0x04004494 RID: 17556
	[Serialize]
	private ResourceRef<Tech> tech = new ResourceRef<Tech>();
}

using System;
using KSerialization;

// Token: 0x02000CD6 RID: 3286
public class TutorialMessage : GenericMessage
{
	// Token: 0x0600659C RID: 26012 RVA: 0x0025E796 File Offset: 0x0025C996
	public TutorialMessage()
	{
	}

	// Token: 0x0600659D RID: 26013 RVA: 0x0025E7AC File Offset: 0x0025C9AC
	public TutorialMessage(Tutorial.TutorialMessages messageId, string title, string body, string tooltip, string videoClipId = null, string videoOverlayName = null, string videoTitleText = null, string icon = "", string[] overrideDLCIDs = null) : base(title, body, tooltip, null)
	{
		this.messageId = messageId;
		this.videoClipId = videoClipId;
		this.videoOverlayName = videoOverlayName;
		this.videoTitleText = videoTitleText;
		this.icon = icon;
		if (overrideDLCIDs != null)
		{
			this.DLCIDs = overrideDLCIDs;
		}
	}

	// Token: 0x0400449D RID: 17565
	[Serialize]
	public Tutorial.TutorialMessages messageId;

	// Token: 0x0400449E RID: 17566
	public string videoClipId;

	// Token: 0x0400449F RID: 17567
	public string videoOverlayName;

	// Token: 0x040044A0 RID: 17568
	public string videoTitleText;

	// Token: 0x040044A1 RID: 17569
	public string icon;

	// Token: 0x040044A2 RID: 17570
	public string[] DLCIDs = DlcManager.AVAILABLE_ALL_VERSIONS;
}

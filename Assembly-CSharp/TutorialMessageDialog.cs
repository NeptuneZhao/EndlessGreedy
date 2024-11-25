using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02000CD7 RID: 3287
public class TutorialMessageDialog : MessageDialog
{
	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x0600659E RID: 26014 RVA: 0x0025E802 File Offset: 0x0025CA02
	public override bool CanDontShowAgain
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600659F RID: 26015 RVA: 0x0025E805 File Offset: 0x0025CA05
	public override bool CanDisplay(Message message)
	{
		return typeof(TutorialMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x060065A0 RID: 26016 RVA: 0x0025E81C File Offset: 0x0025CA1C
	public override void SetMessage(Message base_message)
	{
		this.message = (base_message as TutorialMessage);
		this.description.text = this.message.GetMessageBody();
		if (!string.IsNullOrEmpty(this.message.videoClipId))
		{
			VideoClip video = Assets.GetVideo(this.message.videoClipId);
			this.SetVideo(video, this.message.videoOverlayName, this.message.videoTitleText);
		}
	}

	// Token: 0x060065A1 RID: 26017 RVA: 0x0025E88C File Offset: 0x0025CA8C
	public void SetVideo(VideoClip clip, string overlayName, string titleText)
	{
		if (this.videoWidget == null)
		{
			this.videoWidget = Util.KInstantiateUI(this.videoWidgetPrefab, base.transform.gameObject, true).GetComponent<VideoWidget>();
			this.videoWidget.transform.SetAsFirstSibling();
		}
		this.videoWidget.SetClip(clip, overlayName, new List<string>
		{
			titleText,
			VIDEOS.TUTORIAL_HEADER
		});
	}

	// Token: 0x060065A2 RID: 26018 RVA: 0x0025E902 File Offset: 0x0025CB02
	public override void OnClickAction()
	{
	}

	// Token: 0x060065A3 RID: 26019 RVA: 0x0025E904 File Offset: 0x0025CB04
	public override void OnDontShowAgain()
	{
		Tutorial.Instance.HideTutorialMessage(this.message.messageId);
	}

	// Token: 0x040044A3 RID: 17571
	[SerializeField]
	private LocText description;

	// Token: 0x040044A4 RID: 17572
	private TutorialMessage message;

	// Token: 0x040044A5 RID: 17573
	[SerializeField]
	private GameObject videoWidgetPrefab;

	// Token: 0x040044A6 RID: 17574
	private VideoWidget videoWidget;
}

using System;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C46 RID: 3142
public class FeedbackScreen : KModalScreen
{
	// Token: 0x06006096 RID: 24726 RVA: 0x0023F1E0 File Offset: 0x0023D3E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.FEEDBACK_SCREEN.TITLE);
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.bugForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		};
		this.suggestionForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/133-oxygen-not-included-suggestions-and-feedback/");
		};
		this.logsDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(Util.LogsFolder());
		};
		this.saveFilesDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(SaveLoader.GetSavePrefix());
		};
		if (SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.logsDirectoryButton.GetComponentInParent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
			this.saveFilesDirectoryButton.gameObject.SetActive(false);
			this.logsDirectoryButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400414A RID: 16714
	public LocText title;

	// Token: 0x0400414B RID: 16715
	public KButton dismissButton;

	// Token: 0x0400414C RID: 16716
	public KButton closeButton;

	// Token: 0x0400414D RID: 16717
	public KButton bugForumsButton;

	// Token: 0x0400414E RID: 16718
	public KButton suggestionForumsButton;

	// Token: 0x0400414F RID: 16719
	public KButton logsDirectoryButton;

	// Token: 0x04004150 RID: 16720
	public KButton saveFilesDirectoryButton;
}

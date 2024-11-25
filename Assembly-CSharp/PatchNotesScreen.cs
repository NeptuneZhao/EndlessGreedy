using System;
using UnityEngine;

// Token: 0x02000BAA RID: 2986
public class PatchNotesScreen : KModalScreen
{
	// Token: 0x06005A76 RID: 23158 RVA: 0x0020D0D0 File Offset: 0x0020B2D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		this.closeButton.onClick += this.MarkAsReadAndClose;
		this.closeButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = "HUD_Click_Close";
		this.okButton.onClick += this.MarkAsReadAndClose;
		this.previousVersion.onClick += delegate()
		{
			App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
		};
		this.fullPatchNotes.onClick += this.OnPatchNotesClick;
		PatchNotesScreen.instance = this;
	}

	// Token: 0x06005A77 RID: 23159 RVA: 0x0020D188 File Offset: 0x0020B388
	protected override void OnCleanUp()
	{
		PatchNotesScreen.instance = null;
	}

	// Token: 0x06005A78 RID: 23160 RVA: 0x0020D190 File Offset: 0x0020B390
	public static bool ShouldShowScreen()
	{
		return false;
	}

	// Token: 0x06005A79 RID: 23161 RVA: 0x0020D193 File Offset: 0x0020B393
	private void MarkAsReadAndClose()
	{
		KPlayerPrefs.SetInt("PatchNotesVersion", PatchNotesScreen.PatchNotesVersion);
		this.Deactivate();
	}

	// Token: 0x06005A7A RID: 23162 RVA: 0x0020D1AA File Offset: 0x0020B3AA
	public static void UpdatePatchNotes(string patchNotesSummary, string url)
	{
		PatchNotesScreen.m_patchNotesUrl = url;
		PatchNotesScreen.m_patchNotesText = patchNotesSummary;
		if (PatchNotesScreen.instance != null)
		{
			PatchNotesScreen.instance.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		}
	}

	// Token: 0x06005A7B RID: 23163 RVA: 0x0020D1D9 File Offset: 0x0020B3D9
	private void OnPatchNotesClick()
	{
		App.OpenWebURL(PatchNotesScreen.m_patchNotesUrl);
	}

	// Token: 0x06005A7C RID: 23164 RVA: 0x0020D1E5 File Offset: 0x0020B3E5
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.MarkAsReadAndClose();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04003B95 RID: 15253
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003B96 RID: 15254
	[SerializeField]
	private KButton okButton;

	// Token: 0x04003B97 RID: 15255
	[SerializeField]
	private KButton fullPatchNotes;

	// Token: 0x04003B98 RID: 15256
	[SerializeField]
	private KButton previousVersion;

	// Token: 0x04003B99 RID: 15257
	[SerializeField]
	private LocText changesLabel;

	// Token: 0x04003B9A RID: 15258
	private static string m_patchNotesUrl;

	// Token: 0x04003B9B RID: 15259
	private static string m_patchNotesText;

	// Token: 0x04003B9C RID: 15260
	private static int PatchNotesVersion = 9;

	// Token: 0x04003B9D RID: 15261
	private static PatchNotesScreen instance;
}

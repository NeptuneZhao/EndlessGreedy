using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000CFC RID: 3324
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class OldVersionMessageScreen : KModalScreen
{
	// Token: 0x06006720 RID: 26400 RVA: 0x00267FA8 File Offset: 0x002661A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/topic/140474-previous-update-steam-branch-access/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.quitButton.onClick += delegate()
		{
			App.Quit();
		};
	}

	// Token: 0x06006721 RID: 26401 RVA: 0x00268028 File Offset: 0x00266228
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.messageContainer.sizeDelta = new Vector2(Mathf.Max(384f, (float)Screen.width * 0.25f), this.messageContainer.sizeDelta.y);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x04004591 RID: 17809
	public KButton forumButton;

	// Token: 0x04004592 RID: 17810
	public KButton confirmButton;

	// Token: 0x04004593 RID: 17811
	public KButton quitButton;

	// Token: 0x04004594 RID: 17812
	public LocText bodyText;

	// Token: 0x04004595 RID: 17813
	public bool previewInEditor;

	// Token: 0x04004596 RID: 17814
	public RectTransform messageContainer;
}

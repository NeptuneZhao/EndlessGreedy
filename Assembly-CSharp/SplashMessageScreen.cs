using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DCB RID: 3531
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class SplashMessageScreen : KMonoBehaviour
{
	// Token: 0x0600700F RID: 28687 RVA: 0x002A4298 File Offset: 0x002A2498
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/118-oxygen-not-included/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.bodyText.text = UI.DEVELOPMENTBUILDS.ALPHA.LOADING.BODY;
	}

	// Token: 0x06007010 RID: 28688 RVA: 0x002A4301 File Offset: 0x002A2501
	private void OnEnable()
	{
		this.confirmButton.GetComponent<LayoutElement>();
		this.confirmButton.GetComponentInChildren<LocText>();
	}

	// Token: 0x06007011 RID: 28689 RVA: 0x002A431B File Offset: 0x002A251B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!DlcManager.IsExpansion1Active())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x04004CCB RID: 19659
	public KButton forumButton;

	// Token: 0x04004CCC RID: 19660
	public KButton confirmButton;

	// Token: 0x04004CCD RID: 19661
	public LocText bodyText;

	// Token: 0x04004CCE RID: 19662
	public bool previewInEditor;
}
